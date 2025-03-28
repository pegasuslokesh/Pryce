using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.Data.SqlClient;
using System.Threading;

public partial class ServiceManagement_JobCard : System.Web.UI.Page
{
    SM_Ticket_Master objTicketMaster = null;
    Set_CustomerMaster ObjCustomer = null;
    SystemParameter objSysParam = null;
    Set_DocNumber ObjDoc = null;
    Ems_ContactMaster objContact = null;
    EmployeeMaster objEmpMaster = null;
    Sm_TicketFeedback ObjTicketFeedback = null;
    Inv_ProductMaster ObjProductMaster = null;
    Inv_UnitMaster objUnitMaster = null;
    Prj_Project_Tools objProjecttools = null;
    Inv_SalesInvoiceHeader objSinvoiceHeader = null;
    Inv_SalesInvoiceDetail objSinvoiceDetail = null;
    DataAccessClass objDa = null;
    LocationMaster objLocation = null;
    SM_JobCard_Header objJobCardheader = null;
    SM_JobCard_ItemDetail objJobCardItemdetail = null;
    SM_JobCards_SpareLabourDetail objJobCardLabourdetail = null;
    Set_ApplicationParameter objAppParam = null;
    PurchaseInvoice ObjPurchaseInvoice = null;
    PurchaseInvoiceDetail ObjPurchaseInvoiceDetail = null;
    Common cmn = null;
    PageControlCommon objPageCmn = null;



    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objTicketMaster = new SM_Ticket_Master(Session["DBConnection"].ToString());
        ObjCustomer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjDoc = new Set_DocNumber(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objEmpMaster = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjTicketFeedback = new Sm_TicketFeedback(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objUnitMaster = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objProjecttools = new Prj_Project_Tools(Session["DBConnection"].ToString());
        objSinvoiceHeader = new Inv_SalesInvoiceHeader(Session["DBConnection"].ToString());
        objSinvoiceDetail = new Inv_SalesInvoiceDetail(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objJobCardheader = new SM_JobCard_Header(Session["DBConnection"].ToString());
        objJobCardItemdetail = new SM_JobCard_ItemDetail(Session["DBConnection"].ToString());
        objJobCardLabourdetail = new SM_JobCards_SpareLabourDetail(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        ObjPurchaseInvoice = new PurchaseInvoice(Session["DBConnection"].ToString());
        ObjPurchaseInvoiceDetail = new PurchaseInvoiceDetail(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        Page.Title = objSysParam.GetSysTitle();
        Calender.Format = Session["DateFormat"].ToString();
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../ServiceManagement/JobCard.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            objPageCmn.fillLocationWithAllOption(ddlLocation);
            objPageCmn.fillLocation(ddlLoc);
            Reset();
            CalendartxtValueDate.Format = Session["DateFormat"].ToString();
            CalendarExtender_txtExpectedEnddate.Format = Session["DateFormat"].ToString();
            btnRefreshReport_Click(null, null);
            ddlrefProductName.Items.Clear();
            ListItem Li = new ListItem();
            Li.Text = "--Select--";
            Li.Value = "0";

            ddlrefProductName.Items.Insert(0, Li);
            CalendarExtendertxtdelivertydate.Format = Session["DateFormat"].ToString();
            CalendarExtendertxtEnddate.Format = Session["DateFormat"].ToString();
            txtJobNo.Text = "";
            txtJobNo.Text = GetDocumentNumber();
            ViewState["DocNo"] = txtJobNo.Text;
            getTermsandCondition();


            //for get sales perso n name according login employee
            if (Session["EmpId"].ToString() != "0")
            {
                DataTable DtEmp = objEmpMaster.GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString());
                txtHandledEmp.Text = DtEmp.Rows[0]["Emp_Name"].ToString() + "/" + DtEmp.Rows[0]["Emp_Code"].ToString();
                txtAssignedTo.Text = txtHandledEmp.Text;
            }
            //code end
            Session["ContactID"] = null;

        }

        try
        {
            GvJobCard.DataSource = Session["dtCInquiry"] as DataTable;
            GvJobCard.DataBind();
        }
        catch
        {

        }

    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        BtnExportExcel.Visible = clsPagePermission.bDownload;
        BtnExportPDF.Visible = clsPagePermission.bDownload;
        btnInquirySave.Visible = clsPagePermission.bEdit;
        btnClose.Visible = clsPagePermission.bView;
        GvJobCard.Columns[0].Visible = clsPagePermission.bPrint;
        GvJobCard.Columns[1].Visible = clsPagePermission.bView;
        GvJobCard.Columns[2].Visible = clsPagePermission.bEdit;
        GvJobCard.Columns[3].Visible = clsPagePermission.bDelete;
    }

    public void getTermsandCondition()
    {
        try
        {
            txtTerms.Content = objAppParam.GetApplicationParameterByParamName("Job_Card_Terms", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()).Rows[0]["Description"].ToString();
        }
        catch
        {
            txtTerms.Content = null;
        }

    }

    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {

        FillGrid();

        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueDate.Text = "";
        txtValueDate.Visible = false;
        txtValue.Visible = true;
        txtValue.Focus();

    }
    protected void btnInquirySave_Click(object sender, EventArgs e)
    {


        string strVoucherid = string.Empty;
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

        if (txtjobdate.Text == "")
        {
            DisplayMessage("Enter Job date");
            txtjobdate.Focus();
            return;

        }


        string strCustomerId = string.Empty;
        string strContactId = string.Empty;
        string strHandlemEmployee = string.Empty;
        string strEnddate = string.Empty;
        string Status = string.Empty;


        if (ddlJobType.SelectedIndex == 0)
        {
            if (txtECustomer.Text == "")
            {
                DisplayMessage("Enter Customer Name");
                txtECustomer.Focus();
                return;
            }

            strCustomerId = txtECustomer.Text.Split('/')[3].ToString();
        }
        else
        {
            strCustomerId = "0";
        }


        string strContactUpdate = "";
        if (txtEContact.Text != "")
        {
            strContactId = txtEContact.Text.Split('/')[3].ToString();
            try
            {
                strContactUpdate = "update  Ems_ContactMaster Set ";
                if (txtContactNo.Text.Length > 0)
                {
                    strContactUpdate += " Field2 = '" + txtContactNo.Text + "' ";
                }
                if (txtEmailId.Text.Length > 0)
                {
                    if (strContactUpdate != "update  Ems_ContactMaster Set")
                    {
                        strContactUpdate += ",Field1 = '" + txtEmailId.Text + "' ";
                    }
                    else
                    {
                        strContactUpdate += " Field1 = '" + txtEmailId.Text + "' ";

                    }

                }
                if (strContactUpdate != "update  Ems_ContactMaster Set")
                {
                    strContactUpdate = strContactUpdate + " Where     Trans_Id = '" + strContactId + "'";
                }
                else
                {
                    strContactUpdate = "";
                }
            }
            catch
            {

            }
          
           

        }
        else
        {
            strContactId = "0";
        }


        if (txtHandledEmp.Text == "")
        {
            DisplayMessage("Enter Handled Employee Name");
            txtHandledEmp.Focus();
            return;
        }
        else
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = txtHandledEmp.Text.Split('/')[1].ToString();
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
            if (Emp_ID == "0" || Emp_ID == "")
            {
                DisplayMessage("Employee not exists");
                txtHandledEmp.Focus();
                txtHandledEmp.Text = "";
                return;
            }
            //strHandlemEmployee = txtHandledEmp.Text.Split('/')[1].ToString();
            strHandlemEmployee = Emp_ID;
        }


        if (txtExpectedEnddate.Text == "")
        {
            DisplayMessage("Enter Expected End date");
            txtExpectedEnddate.Focus();
            return;
        }

        if(txtContactNo.Text == "")
        {
            DisplayMessage("Please Update Contact No on Contact Master,Then Select Contact Again!");
            txtContactNo.Focus();
            return;
        }


        if (txtEnddate.Text == "")
        {
            strEnddate = "1900-01-01";
        }
        else
        {
            strEnddate = txtEnddate.Text;
        }


        if (((Button)sender).ID.Trim() == "btnInquirySave")
        {
            Status = ddlStatus.SelectedValue;
        }
        else if (((Button)sender).ID.Trim() == "btnClose")
        {
            Status = "Close";
            if (txtEnddate.Text == "" || txtEnddate.Text == "01-Jan-1900")
            {
                strEnddate = DateTime.Now.ToString();
            }

            string isInRMA = objDa.get_SingleValue("select COUNT(*) from SM_GetPass_Detail inner join SM_JobCards_ItemDetail on SM_JobCards_ItemDetail.Trans_Id = SM_GetPass_Detail.Job_No where SM_GetPass_Detail.Status='Send' and SM_JobCards_ItemDetail.Header_Id='" + hdnValue.Value + "'");
            isInRMA = isInRMA == "@NOTFOUND@" ? "0" : isInRMA;
            if (isInRMA != "0")
            {
                DisplayMessage("Cant close the job card because some of the product are send in RMA");
                return;
            }
        }
        string strTicketNo = string.Empty;


        if (txtticketno.Text.Trim() == "")
        {
            strTicketNo = "0";
        }
        else
        {
            strTicketNo = txtticketno.Text.Trim();
        }


        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        string strInvoicedate = string.Empty;


        try
        {
            string itemdata = "";

            if (hdnValue.Value == "")
            {
                if(strContactUpdate.Length> 0)
                {
                    objDa.execute_Command(strContactUpdate, ref trns);
                }

                int b = objJobCardheader.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, txtJobNo.Text, txtjobdate.Text, txtExpectedEnddate.Text, strEnddate, strCustomerId, strTicketNo, strHandlemEmployee, ddlInvoiceType.SelectedValue, strContactId, txtContactNo.Text, txtEmailId.Text, txtRemarks.Text, Status, txtTerms.Content, "", ddlJobType.SelectedValue.Trim(), ChkCall.Checked.ToString(), "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                if (b != 0)
                {

                   

                    strVoucherid = b.ToString();

                    string dtCount = objJobCardheader.getCount(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, ref trns);
                    objJobCardheader.Updatecode(b.ToString(), txtJobNo.Text + dtCount, ref trns);
                    txtJobNo.Text = txtJobNo.Text + dtCount;

                    //here we will insert record in item detail 


                    DataTable dtItemdetail = getItemDetailinDatatable();
                    //string itemdata = "";
                    if (dtItemdetail.Rows.Count > 0)
                    {
                        itemdata = "<h4 style='float:left;color:#064184; margin-left:10px; line-height:20px; font-weight:bold; font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:18px; letter-spacing:1px; margin-left:10px;'> Item Details : </h4>    </div> <table width='100%' cellpadding='2' cellspacing='3' border='0' style='font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:30px; border:solid 1px #333333;'><tr style='background:#064184; color:#FFFFFF; margin-left:10px; line-height:40px; font-weight:bold; font-size:16px; letter-spacing:1px;'><td style='padding-left:10px;' colspan='2'>Product id </td><td style='padding-left:10px;' colspan='2'>Product Name </td>  <td style='padding-left:10px;' colspan='2'> Quantity</td>  <td style='padding-left:10px;' colspan='2'>Status</td>  <td style='padding-left:10px;' colspan='2'> Responsible Person</td>  <td style='padding-left:10px;' colspan='2'>Problem</td>  </tr>";
                    }
                    else
                    {
                        itemdata = "</div>";
                    }

                    int counter = 0;

                    //dt.Columns.Add("DeliveryDate");
                    //dt.Columns.Add("Problem");


                    foreach (DataRow dr in dtItemdetail.Rows)
                    {
                        counter++;

                        if (dr["Invoice_Date"].ToString() == "")
                        {

                            strInvoicedate = "01/01/1900";
                        }
                        else
                        {
                            strInvoicedate = dr["Invoice_Date"].ToString();
                        }


                        objJobCardItemdetail.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, b.ToString(), counter.ToString(), dr["Invoice_Id"].ToString(), dr["Invoice_No"].ToString(), strInvoicedate, dr["ProductSerialNo"].ToString(), dr["ProductId"].ToString(), dr["Qty"].ToString(), dr["Status"].ToString(), dr["Responsible_Person"].ToString(), DateTime.Now.ToString(), DateTime.Now.ToString(), dr["DeliveryDate"].ToString(), dr["Problem"].ToString(), dr["Field3"].ToString(), "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                        itemdata += "<tr style='background:#eeeeee;color:#000;'><td style='padding-left:10px;' colspan='2'>" + dr["ProductId"].ToString() + "</td><td style='padding-left:10px;' colspan='2'>" + SuggestedProductName(dr["ProductId"].ToString()) + "</td><td style='padding-left:10px;' colspan='2'>" + dr["Qty"].ToString() + "</td><td style='padding-left:10px;' colspan='2'>" + dr["Status"].ToString() + "</td><td style='padding-left:10px;' colspan='2'>" + GetAssignedPersonName(dr["Responsible_Person"].ToString()) + "</td><td style='padding-left:10px;' colspan='2'>" + dr["Problem"].ToString() + "</td></tr>";
                    }

                    if (dtItemdetail.Rows.Count > 0)
                    {
                        itemdata += "</table>";
                    }
                    else
                    {
                        itemdata += "";
                    }


                    if (hdnEmpEmailId.Value.Trim() != "")
                    {
                        try
                        {

                            //new Thread(() =>
                            //{
                            //    Thread.CurrentThread.IsBackground = true;
                            //    /* run your code here */
                            //    sendMail(hdnEmpEmailId.Value, txtECustomer.Text.Split('/')[0].ToString(), txtExpectedEnddate.Text, itemdata);
                            //}).Start();


                            sendMail(hdnEmpEmailId.Value, txtECustomer.Text.Split('/')[0].ToString(), txtExpectedEnddate.Text, itemdata);
                        }
                        catch
                        {

                        }
                    }




                    //insert record in labour detail table


                    DataTable dtlabourDetail = getSpareDetailinDatatable();

                    counter = 0;
                    foreach (DataRow dr in dtlabourDetail.Rows)
                    {
                        counter++;
                        objJobCardLabourdetail.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, b.ToString(), counter.ToString(), dr["Ref_Product_Id"].ToString(), dr["ProductId"].ToString(), dr["Unit"].ToString(), dr["Quantity"].ToString(), dr["Unit_Price"].ToString(), (Convert.ToDecimal(dr["Quantity"].ToString()) * Convert.ToDecimal(dr["Unit_Price"].ToString())).ToString(), dr["Field1"].ToString(), "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                    }

                    DisplayMessage("Record Saved", "green");

                }


            }
            else
            {

                objJobCardheader.UpdateRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, hdnValue.Value, txtJobNo.Text, txtjobdate.Text, txtExpectedEnddate.Text, strEnddate, strCustomerId, strTicketNo, strHandlemEmployee, ddlInvoiceType.SelectedValue, strContactId, txtContactNo.Text, txtEmailId.Text, txtRemarks.Text, Status, txtTerms.Content, "", ddlJobType.SelectedValue.Trim(), ChkCall.Checked.ToString(), "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                strVoucherid = hdnValue.Value;

                //here we will delete and reinsert record in item detail 

                objJobCardItemdetail.DeleteRecord_By_HeaderId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, hdnValue.Value, ref trns);

                DataTable dtItemdetail = getItemDetailinDatatable();
                if (dtItemdetail.Rows.Count > 0)
                {
                    itemdata = "<h4 style='float:left;color:#064184; margin-left:10px; line-height:20px; font-weight:bold; font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:18px; letter-spacing:1px; margin-left:10px;'> Item Details : </h4>    </div> <table width='100%' cellpadding='2' cellspacing='3' border='0' style='font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:30px; border:solid 1px #333333;'><tr style='background:#064184; color:#FFFFFF; margin-left:10px; line-height:40px; font-weight:bold; font-size:16px; letter-spacing:1px;'><td style='padding-left:10px;' colspan='2'>Product id </td><td style='padding-left:10px;' colspan='2'>Product Name </td>  <td style='padding-left:10px;' colspan='2'> Quantity</td>  <td style='padding-left:10px;' colspan='2'>Status</td>  <td style='padding-left:10px;' colspan='2'> Responsible Person</td>  <td style='padding-left:10px;' colspan='2'>Problem</td> <td style='padding-left:10px;' colspan='2'>Action</td> </tr>";
                }
                else
                {
                    itemdata = "</div>";
                }

                int counter = 0;

                foreach (DataRow dr in dtItemdetail.Rows)
                {
                    counter++;

                    if (dr["Invoice_Date"].ToString() == "")
                    {

                        strInvoicedate = "01/01/1900";
                    }
                    else
                    {
                        strInvoicedate = dr["Invoice_Date"].ToString();
                    }
                    objJobCardItemdetail.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, hdnValue.Value, counter.ToString(), dr["Invoice_Id"].ToString(), dr["Invoice_No"].ToString(), strInvoicedate, dr["ProductSerialNo"].ToString(), dr["ProductId"].ToString(), dr["Qty"].ToString(), dr["Status"].ToString(), dr["Responsible_Person"].ToString(), DateTime.Now.ToString(), DateTime.Now.ToString(), dr["DeliveryDate"].ToString(), dr["Problem"].ToString(), dr["Field3"].ToString(), "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                    itemdata += "<tr style='background:#eeeeee;color:#000;'><td style='padding-left:10px;' colspan='2'>" + dr["ProductId"].ToString() + "</td><td style='padding-left:10px;' colspan='2'>" + SuggestedProductName(dr["ProductId"].ToString()) + "</td><td style='padding-left:10px;' colspan='2'>" + dr["Qty"].ToString() + "</td><td style='padding-left:10px;' colspan='2'>" + dr["Status"].ToString() + "</td><td style='padding-left:10px;' colspan='2'>" + GetAssignedPersonName(dr["Responsible_Person"].ToString()) + "</td><td style='padding-left:10px;' colspan='2'>" + dr["Problem"].ToString() + "</td><td style='padding-left:10px;' colspan='2'>" + dr["Field3"].ToString() + "</td></tr>";


                }

                if (dtItemdetail.Rows.Count > 0)
                {
                    itemdata += "</table>";
                }
                else
                {
                    itemdata += "";
                }


                //delete and reinsert record in labour detail table

                objJobCardLabourdetail.DeleteRecord_By_HeaderId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, hdnValue.Value, ref trns);

                DataTable dtlabourDetail = getSpareDetailinDatatable();

                counter = 0;

                foreach (DataRow dr in dtlabourDetail.Rows)
                {
                    //int.TryParse(dr["Quantity"].ToString(),out Quantity);
                    //int.TryParse(dr["Unit_Price"].ToString(),out Unit_Price);
                    counter++;
                    objJobCardLabourdetail.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, hdnValue.Value, counter.ToString(), dr["Ref_Product_Id"].ToString(), dr["ProductId"].ToString(), dr["Unit"].ToString(), dr["Quantity"].ToString(), dr["Unit_Price"].ToString(), (Convert.ToDecimal(dr["Quantity"].ToString()) * Convert.ToDecimal(dr["Unit_Price"].ToString())).ToString(), dr["Field1"].ToString(), "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }


                DisplayMessage("Record Updated", "green");
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }


            //on closing we are generating invoice internally

            if (ddlJobType.SelectedIndex == 0)
            {
                if (((Button)sender).ID.Trim() == "btnClose")
                {
                    if (hdnEmpEmailId.Value.Trim() != "")
                    {
                        try
                        {

                            //new Thread(() =>
                            //{
                            //    Thread.CurrentThread.IsBackground = true;
                            //    /* run your code here */
                            //    sendMail(hdnEmpEmailId.Value, txtECustomer.Text.Split('/')[0].ToString(), txtExpectedEnddate.Text, itemdata);
                            //}).Start();

                            sendMail(hdnEmpEmailId.Value, txtECustomer.Text.Split('/')[0].ToString(), txtExpectedEnddate.Text, itemdata);
                        }
                        catch
                        {

                        }
                    }

                    SaveInvoice(strVoucherid, strCustomerId, ref trns);
                }
            }



            //code end
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

        if (((Button)sender).ID.Trim() == "btnClose")
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../ServiceManagementReport/JobCardReport.aspx?Id=" + strVoucherid + "&&Customer_Id=" + strCustomerId + "','','height=650,width=950,scrollbars=Yes')", true);

        }

    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnInquiryCancel_Click(object sender, EventArgs e)
    {
        Reset();

        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {

        if (ddlFieldName.SelectedItem.Value == "Job_date")
        {
            if (txtValueDate.Text != "")
            {

                try
                {

                    txtValue.Text = objSysParam.getDateForInput(txtValueDate.Text).ToString();
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
            DataTable dtCustomInq = (DataTable)Session["dtCInquiry"];
            DataView view = new DataView(dtCustomInq, condition, "", DataViewRowState.CurrentRows);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            //cmn.FillData((object)GvCustomerInquiry, view.ToTable(), "", "");
            GvJobCard.DataSource = view.ToTable();
            GvJobCard.DataBind();

            Session["dtCInquiry"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";

        }
        //AllPageCode();
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }
    protected void GvCustomerInquiry_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtCInquiry"];
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
        Session["dtCInquiry"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvJobCard, dt, "", "");

        //AllPageCode();
    }
    protected void GvCustomerInquiry_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvJobCard.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtCInquiry"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvJobCard, dt, "", "");

        // AllPageCode();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        Reset();
        ResetDetailsection();
        ResetSpareDetailSection();
        ddlLoc.SelectedValue = e.CommandName.ToString();
        ddlLoc.Enabled = false;
        DataTable dt = objJobCardheader.GetAllRecord_By_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, e.CommandArgument.ToString());
        if (dt.Rows.Count != 0)
        {

            LinkButton b = (LinkButton)sender;
            string objSenderID = b.ID;

            if (objSenderID == "lnkViewDetail")
            {
                Lbl_Tab_New.Text = Resources.Attendance.View;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                btnInquirySave.Enabled = false;
            }
            else
            {
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                btnInquirySave.Enabled = true;
                div_call.Attributes["style"] = "display:block;";
                div_endDate.Attributes["style"] = "display:block;";
                div_br.Attributes["style"] = "display:block;";
            }


            if (Lbl_Tab_New.Text == "New")
            {
                btnClose.Visible = false;
            }
            else if (Lbl_Tab_New.Text == Resources.Attendance.New)
            {
                btnClose.Visible = false;
            }
            else if (Lbl_Tab_New.Text == "Edit")
            {
                btnClose.Visible = true;
            }
            else if (Lbl_Tab_New.Text == Resources.Attendance.Edit)
            {
                btnClose.Visible = true;
            }


            if (dt.Rows[0]["Field4"].ToString().Trim() == "" || dt.Rows[0]["Field4"].ToString().Trim() == "False")
            {
                ChkCall.Checked = false;
            }
            else
            {
                ChkCall.Checked = true;
            }


            ddlJobType.SelectedValue = dt.Rows[0]["Field3"].ToString().Trim();
            ddlJobType_OnSelectedIndexChanged(null, null);
            hdnValue.Value = e.CommandArgument.ToString();

            txtJobNo.Text = dt.Rows[0]["Job_No"].ToString();
            txtjobdate.Text = Convert.ToDateTime(dt.Rows[0]["Job_date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtExpectedEnddate.Text = Convert.ToDateTime(dt.Rows[0]["Expected_Job_End_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            //01-Jan-00 12:00:00 AM

            if (dt.Rows[0]["Job_End_Date"].ToString() != "01-Jan-00 12:00:00 AM")
            {
                txtEnddate.Text = Convert.ToDateTime(dt.Rows[0]["Job_End_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            }
            else
            {
                txtEnddate.Text = "";
            }


            txtECustomer.Text = dt.Rows[0]["CustomerName"].ToString() + "///" + dt.Rows[0]["Customer_Id"].ToString();

            Session["ContactID"] = dt.Rows[0]["Customer_Id"].ToString();
            if (dt.Rows[0]["Contact_Person"].ToString() != "0")
            {
                txtEContact.Text = dt.Rows[0]["ContactPersonName"].ToString() + "///" + dt.Rows[0]["Contact_Person"].ToString();

            }

            txtContactNo.Text = dt.Rows[0]["Contact_Person_Telephone"].ToString();
            txtEmailId.Text = dt.Rows[0]["Contact_Person_EmailId"].ToString();
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(dt.Rows[0]["Assign_to"].ToString());
            if (Emp_Code == "0" || Emp_Code == "")
            {
                DisplayMessage("Employee not exists");
                return;
            }
            txtHandledEmp.Text = dt.Rows[0]["SalesPersonName"].ToString() + "/" + Emp_Code;
            DataTable dtEmp = objEmpMaster.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), Emp_Code);
            hdnEmpEmailId.Value = dtEmp.Rows[0]["Email_Id"].ToString();
            ddlInvoiceType.SelectedValue = dt.Rows[0]["Invoice_Type"].ToString();
            txtTerms.Content = dt.Rows[0]["Field1"].ToString();

            if (dt.Rows[0]["Ticket_Id"].ToString().Trim() != "0")
            {
                txtticketno.Text = dt.Rows[0]["Ticket_Id"].ToString();
            }
            else
            {
                txtticketno.Text = "";

            }

            if (txtticketno.Text.Trim() != "")
            {
                txtticketno_OnTextChanged(null, null);
            }
            ddlStatus.SelectedValue = dt.Rows[0]["Status"].ToString();
            txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();

            //get item detail 


            DataTable dtDetail = objJobCardItemdetail.GetAllRecord_By_HeaderId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, e.CommandArgument.ToString());

            DataTable dtItemDetail = dtDetail.DefaultView.ToTable(true, "Serial_No", "Invoice_Id", "Invoice_No", "Invoice_Date", "ProductSerialNo", "ProductId", "Qty", "Status", "Responsible_Person", "DeliveryDate", "Problem", "Field3");

            objPageCmn.FillData((object)GvItemDetail, dtItemDetail, "", "");
            //get labour detail 


            DataTable DtLaboutDetail = objJobCardLabourdetail.GetAllRecord_By_HeaderId(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue, e.CommandArgument.ToString());


            DtLaboutDetail = DtLaboutDetail.DefaultView.ToTable(true, "Trans_Id", "Ref_Product_Id", "ProductId", "Unit", "Quantity", "Unit_Price", "Field1");

            objPageCmn.FillData((object)gvProductRequest, DtLaboutDetail, "", "");

            decimal total = 0;
            decimal labourCharges = 0;
            decimal otherCharges = 0;

            foreach (DataRow dr in DtLaboutDetail.Rows)
            {
                if (dr["Field1"].ToString() == "Labour")
                {
                    labourCharges += Convert.ToDecimal(dr["Quantity"].ToString()) * Convert.ToDecimal(dr["Unit_Price"].ToString());
                }
                if (dr["Field1"].ToString() == "Other")
                {
                    otherCharges += Convert.ToDecimal(dr["Quantity"].ToString()) * Convert.ToDecimal(dr["Unit_Price"].ToString());
                }
                total = total + Convert.ToDecimal(dr["Quantity"].ToString()) * Convert.ToDecimal(dr["Unit_Price"].ToString());
            }

            lblLabourCharges.Text = "Labour Charges: " + SetDecimal(labourCharges.ToString());
            lblOtherCharges.Text = "Other Charges: " + SetDecimal(otherCharges.ToString());
            lblTotal.Text = "Total Amt: " + SetDecimal(total.ToString());
            txtTotalCharges.Text = SetDecimal(total.ToString());
            BingRefProductList();

            TabContainer2.ActiveTabIndex = 0;
        }
    }
    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {

        btnEdit_Command(sender, e);

    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        DataTable dt = objJobCardheader.GetAllRecord_By_TransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandName.ToString(), e.CommandArgument.ToString());
        if (dt.Rows.Count != 0)
        {
            if (dt.Rows[0]["Status"].ToString() == "Close")
            {
                DisplayMessage("job card closed , you can not delete");
                return;
            }
        }

        int b = 0;
        hdnValue.Value = e.CommandArgument.ToString();
        b = objJobCardheader.DeleteRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandName.ToString(), hdnValue.Value);
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
        //AllPageCode();

    }


    protected void txtECustomer_TextChanged(object sender, EventArgs e)
    {
        Session["ContactID"] = null;
        string strCustomerId = string.Empty;
        if (txtECustomer.Text != "")
        {
            try
            {
                strCustomerId = txtECustomer.Text.Split('/')[3].ToString();
            }
            catch
            {
                strCustomerId = "0";

            }
            if (strCustomerId == "0")
            {
                DisplayMessage("Select In Suggestions Only");
                txtECustomer.Text = "";
                txtEContact.Text = "";
                txtContactNo.Text = "";
                txtEmailId.Text = "";
                txtECustomer.Focus();
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtECustomer);
                //txtContactNo.Text = "";
                //txtEContact.Text = "";
                //txtEmailId.Text = "";
                //Session["ContactID"] = "0";
            }
            else
            {
                txtContactNo.Text = txtECustomer.Text.Split('/')[1].ToString();
                txtEmailId.Text = txtECustomer.Text.Split('/')[2].ToString();
                DataTable dt = objContact.GetContactTrueAllData(strCustomerId, "Individual");
                txtEContact.Text = "";
                if (dt.Rows.Count > 0)
                {
                    txtEContact.Text = dt.Rows[0]["Name"].ToString() + "/" + dt.Rows[0]["Field2"].ToString() + "/" + dt.Rows[0]["Field1"].ToString() + "/" + dt.Rows[0]["Trans_Id"].ToString();
                }
                else
                {
                    txtEContact.Text = txtECustomer.Text;
                }
                txtEContact_TextChanged(null, null);
                txtEContact.Focus();
                Session["ContactID"] = strCustomerId;
            }
            txtEContact.Focus();

        }
        // AllPageCode();
    }



    protected void txtEContact_TextChanged(object sender, EventArgs e)
    {
        string strCustomerId = string.Empty;
        if (txtEContact.Text != "")
        {
            try
            {
                strCustomerId = txtEContact.Text.Split('/')[3].ToString();

            }
            catch
            {
                strCustomerId = "0";

            }
            if (strCustomerId == "0")
            {
                DisplayMessage("Select In Suggestions Only");
                txtEContact.Text = "";
                txtContactNo.Text = "";
                txtEmailId.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtEContact);
            }
            else
            {
                DataTable dtcust = objContact.GetContactTrueById(strCustomerId.ToString());

                if (dtcust.Rows.Count > 0)
                {
                    if (dtcust.Rows[0]["Field2"].ToString() != "")
                    {

                        txtContactNo.Text = dtcust.Rows[0]["Field2"].ToString();
                    }
                    if (dtcust.Rows[0]["Field1"].ToString() != "")
                    {
                        txtEmailId.Text = dtcust.Rows[0]["Field1"].ToString();
                    }
                }
            }

            txtContactNo.Focus();
        }
        // TabContainer2.ActiveTabIndex = 1;
        // AllPageCode();
    }




    protected void txtHandledEmp_TextChanged(object sender, EventArgs e)
    {


        string strEmpId = string.Empty;
        if (((TextBox)sender).Text != "")
        {
            try
            {
                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_Code = ((TextBox)sender).Text.Split('/')[1].ToString();
                string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                hdnEmpEmailId.Value = HR_EmployeeDetail.GetEmpEmailID(Emp_ID);

                if (Emp_ID == "0" || Emp_ID == "")
                {
                    DisplayMessage("Employee not exists");
                    ((TextBox)sender).Focus();
                    ((TextBox)sender).Text = "";
                    return;
                }

                // strEmpId = ((TextBox)sender).Text.Split('/')[1].ToString();
                strEmpId = Emp_ID;
                if (((TextBox)sender).ID.Trim() == "txtHandledEmp")
                {

                    txtAssignedTo.Text = ((TextBox)sender).Text;

                }


                ddlStatus.Focus();

            }
            catch
            {
                strEmpId = "0";

            }
            if (strEmpId == "0")
            {
                DisplayMessage("Select In Suggestions Only");
                ((TextBox)sender).Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(((TextBox)sender));
                return;
            }
        }
    }



    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContact(string prefixText, int count, string contextKey)
    {
        string id = string.Empty;
        DataTable dt = new DataTable();

        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        try
        {
            id = HttpContext.Current.Session["ContactID"].ToString();
        }
        catch
        {
            id = "0";
        }


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
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        try
        {
            Set_CustomerMaster objcustomer = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString());

            DataTable dtCustomer = objcustomer.GetCustomerAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);


            string filtertext = "Name like '%" + prefixText + "%'";
            DataTable dtCon = new DataView(dtCustomer, filtertext, "Name Asc", DataViewRowState.CurrentRows).ToTable();

            string[] filterlist = new string[dtCon.Rows.Count];
            if (dtCon.Rows.Count > 0)
            {
                for (int i = 0; i < dtCon.Rows.Count; i++)
                {
                    filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Field2"].ToString() + "/" + dtCon.Rows[i]["Field1"].ToString() + "/" + dtCon.Rows[i]["Customer_Id"].ToString();
                }
            }
            return filterlist;
        }
        catch (Exception err)
        {
            string[] filterlist = new string[1];
            return filterlist;
        }

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListRefTo(string prefixText, int count, string contextKey)
    {

        //EmployeeMaster ObjEmp = new EmployeeMaster();

        DataTable dtCon = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", prefixText);
        //ObjEmp.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());


        //        dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "' and LOcation_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();



        // DataTable dtMain = new DataTable();
        // dtMain = dt.Copy();


        //string filtertext = "(Emp_Name like '" + prefixText + "%' OR Convert(Emp_Code, System.String) like '" + prefixText + "%')";
        //DataTable dtCon = new DataView(dt, filtertext, "Emp_Name asc", DataViewRowState.CurrentRows).ToTable();

        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Emp_Name"].ToString() + "/" + dtCon.Rows[i]["Emp_Code"].ToString();
            }

        }
        return filterlist;

    }
    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "" && strDate != "01-Jan-00 12:00:00 AM")
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

        ddlJobType.SelectedIndex = 0;
        ddlJobType_OnSelectedIndexChanged(null, null);

        div_call.Attributes["style"] = "display:none;";
        div_endDate.Attributes["style"] = "display:none;";
        div_br.Attributes["style"] = "display:none;";
        txtJobNo.Text = "";
        txtJobNo.Text = GetDocumentNumber();
        ViewState["DocNo"] = txtJobNo.Text;
        txtjobdate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
        txtExpectedEnddate.Text = "";
        txtEnddate.Text = "";
        txtECustomer.Text = "";
        txtEContact.Text = "";
        txtContactNo.Text = "";
        txtEmailId.Text = "";
        txtHandledEmp.Text = "";
        txtticketno.Text = "";
        txtRemarks.Text = "";
        hdnEmpEmailId.Value = "";
        txtHandledEmp.Text = "";
        ddlInvoiceType.SelectedIndex = 0;
        ChkCall.Checked = false;
        ddlStatus.SelectedIndex = 0;
        txtAction.Text = "";
        hdnTicketid.Value = "0";
        txtticketno.Text = "";
        trTicketDetail.Visible = false;
        lnkticketdesc.Visible = false;
        //Editor1.Content = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        btnClose.Visible = false;
        hdnValue.Value = "";
        ViewState["Select"] = null;
        ResetDetailsection();
        ResetSpareDetailSection();
        TabContainer2.ActiveTabIndex = 0;
        objPageCmn.FillData((object)GvItemDetail, null, "", "");
        objPageCmn.FillData((object)gvProductRequest, null, "", "");
        getTermsandCondition();
        Session["ContactID"] = null;
        Session["ProductId"] = null;
        GvItemDetail.Enabled = true;
    }
    public void FillGrid()
    {
        DataTable dt = new DataTable();
        dt = objJobCardheader.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue);
        if (ddlPosted.SelectedIndex != 2)
        {
            dt = new DataView(dt, "Status='" + ddlPosted.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        //cmn.FillData((object)GvCustomerInquiry, dt, "", "");
        GvJobCard.DataSource = dt;
        GvJobCard.DataBind();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        Session["dtCInquiry"] = dt;
    }


    protected string GetDocumentNumber()
    {
        string s = ObjDoc.GetDocumentNo(true, Session["CompId"].ToString(), true, "158", "348", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        return s;
    }
    public string GetEmpName()
    {
        string EmpName = string.Empty;

        DataTable dtEmployee = objEmpMaster.GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString());
        try
        {
            EmpName = dtEmployee.Rows[0]["Emp_Name"].ToString() + "/" + dtEmployee.Rows[0]["Emp_Id"].ToString();
        }
        catch
        {
        }

        return EmpName;
    }
    #region Date Search
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldName.SelectedItem.Value == "Job_date")
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

    #endregion

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]

    public static string[] GetCompletionListTicketNo(string prefixText, int count, string contextKey)
    {
        SM_Ticket_Master objTickeMaster = new SM_Ticket_Master(HttpContext.Current.Session["DBConnection"].ToString());
        SM_TicketEmployee objTicketemployee = new SM_TicketEmployee(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dtTicket = objTickeMaster.GetAllRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        try
        {
            dtTicket = new DataView(dtTicket, "Status='Open' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }


        string[] filterlist = new string[dtTicket.Rows.Count];
        if (dtTicket.Rows.Count > 0)
        {
            for (int i = 0; i < dtTicket.Rows.Count; i++)
            {
                filterlist[i] = dtTicket.Rows[i]["Ticket_No"].ToString();
            }
        }

        return filterlist;
    }

    protected void txtticketno_OnTextChanged(object sendre, EventArgs e)
    {
        if (txtticketno.Text != "")
        {

            DataTable dtTicket = new DataTable();

            dtTicket = objTicketMaster.GetAllRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLoc.SelectedValue);

            try
            {
                dtTicket = new DataView(dtTicket, "Ticket_No='" + txtticketno.Text + "' and Status='Open' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();

            }
            catch
            {

            }
            if (dtTicket.Rows.Count > 0)
            {
                trTicketDetail.Visible = true;
                lnkticketdesc.Visible = true;
                hdnTicketid.Value = dtTicket.Rows[0]["Trans_Id"].ToString();
                lblTickeDate.Text = GetDate(dtTicket.Rows[0]["Ticket_Date"].ToString());
                lblStatus.Text = dtTicket.Rows[0]["Status"].ToString();
                lblTaskType.Text = dtTicket.Rows[0]["Task_Type"].ToString();
                lblCustomerNameValue.Text = dtTicket.Rows[0]["CustomerName"].ToString();

                lblScheduledate.Text = GetDate(dtTicket.Rows[0]["Schedule_Date"].ToString());
                lblDescriptionvalue.Text = dtTicket.Rows[0]["Description"].ToString();

            }
            else
            {
                DisplayMessage("Ticket Not Found");
                txtticketno.Text = "";
                txtticketno.Focus();
                lnkticketdesc.Visible = false;
                trTicketDetail.Visible = false;
                hdnTicketid.Value = "0";
                return;
            }
        }
        else
        {
            lnkticketdesc.Visible = false;
            hdnTicketid.Value = "0";
        }
    }



    #region ItemDetail



    protected void ddlProduct_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        itemHistory.Text = "";
        ddlWarranty.SelectedValue = GetWarrranty(ddlProduct.SelectedValue, Convert.ToDateTime(txtInvoicedate.Text));
        hdnItemId.Value = ddlProduct.SelectedValue;
        itemHistory.Text = ddlProduct.SelectedItem.ToString();
    }


    public string GetWarrranty(string strproductId, DateTime InvoiceDate)
    {
        string strStatus = "No";
        double WarrantyDay = 0;

        try
        {
            WarrantyDay = Convert.ToDouble(ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlProduct.SelectedValue, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["URL"].ToString());
        }
        catch
        {

        }

        double compareday = (DateTime.Now - InvoiceDate).TotalDays;

        if (compareday <= WarrantyDay)
        {
            strStatus = "Yes";

        }




        return strStatus;
    }

    public void ResetDetailsection()
    {
        txtAction.Text = "";
        txtsalesinvoice.Text = "";
        txtInvoicedate.Text = "";
        ddlProduct.Items.Clear();
        txtItemCode.Text = "";
        txtItemname.Text = "";
        hdnItemId.Value = "0";
        hdnItemEditId.Value = "0";
        txtQty.Text = "1";
        txtSerialNo.Enabled = true;
        txtsalesinvoice.Enabled = true;
        txtSerialNo.Text = "";
        txtSerialNo.Focus();
        ddlIemStatus.SelectedIndex = 0;
        BingRefProductList();
        txtdelivertydate.Text = "";
        txtItemProblem.Text = "";
        Session["ProductId"] = null;
    }



    protected void txtItemCode_TextChanged(object sender, EventArgs e)
    {

        if (ddlJobType.SelectedIndex == 0)
        {
            if (txtECustomer.Text == "")
            {
                DisplayMessage("Enter Customer Name");
                ((TextBox)sender).Text = "";
                txtECustomer.Focus();
                return;
            }
        }
        DataTable dt = new DataTable();
        dt = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ((TextBox)sender).Text.ToString());
        if (dt != null && dt.Rows.Count > 0)
        {

            txtItemname.Text = dt.Rows[0]["EProductName"].ToString();
            txtItemCode.Text = dt.Rows[0]["ProductCode"].ToString();
            hdnItemId.Value = dt.Rows[0]["ProductId"].ToString();


            Session["ProductId"] = hdnItemId.Value;
            ddlProduct.Items.Clear();

        }
        else
        {
            DisplayMessage("Product Not Found");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
            return;
        }

    }


    protected void txtsalesinvoice_OnTextChanged(object sender, EventArgs e)
    {
        DataTable dtInvoice = new DataTable();
        DataTable dtDetail = new DataTable();

        string strInvoiceNo = string.Empty;

        ddlProduct.Items.Clear();


        if (txtsalesinvoice.Text != "")
        {
            try
            {
                strInvoiceNo = txtsalesinvoice.Text.Split('/')[0].ToString();
            }
            catch
            {
                strInvoiceNo = "0";
            }



            if (ddlJobType.SelectedIndex == 0)
            {



                dtInvoice = objSinvoiceHeader.GetSInvHeaderAllByInvoiceNo(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), strInvoiceNo.Trim());

                if (dtInvoice.Rows.Count > 0)
                {

                    txtItemCode.Text = "";
                    txtItemname.Text = "";

                    hdnInvoiceId.Value = dtInvoice.Rows[0]["Trans_Id"].ToString();

                    Session["ProductId"] = null;

                    if (txtECustomer.Text == "")
                    {
                        txtECustomer.Text = dtInvoice.Rows[0]["CustomerName"].ToString() + "///" + dtInvoice.Rows[0]["Supplier_Id"].ToString();
                        Session["ContactID"] = dtInvoice.Rows[0]["Supplier_Id"].ToString();
                    }
                    else
                    {
                        if (dtInvoice.Rows[0]["Supplier_Id"].ToString() != Session["ContactID"].ToString())
                        {
                            DisplayMessage("Invoice should be allow for selected customer");
                            txtsalesinvoice.Text = "";
                            txtsalesinvoice.Focus();
                            return;
                        }

                    }



                    txtInvoicedate.Text = Convert.ToDateTime(dtInvoice.Rows[0]["Invoice_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                    hdnInvoiceId.Value = dtInvoice.Rows[0]["Trans_Id"].ToString();
                    dtDetail = objSinvoiceDetail.GetSInvDetailByInvoiceNo(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), dtInvoice.Rows[0]["Trans_Id"].ToString(), Session["FinanceYearId"].ToString());

                    hdnItemId.Value = dtDetail.Rows[0]["Product_Id"].ToString();

                    ddlProduct.DataSource = dtDetail;
                    ddlProduct.DataTextField = "ProductName";
                    ddlProduct.DataValueField = "Product_Id";
                    ddlProduct.DataBind();

                    ddlWarranty.SelectedValue = GetWarrranty(ddlProduct.SelectedValue, Convert.ToDateTime(txtInvoicedate.Text));


                }
                else
                {
                    ResetDetailsection();
                }
            }
            else
            {
                dtInvoice = ObjPurchaseInvoice.GetDataByInvoiceNo(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), strInvoiceNo.Trim());


                if (dtInvoice.Rows.Count > 0)
                {

                    txtItemCode.Text = "";
                    txtItemname.Text = "";

                    Session["ProductId"] = null;

                    txtInvoicedate.Text = Convert.ToDateTime(dtInvoice.Rows[0]["InvoiceDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

                    hdnInvoiceId.Value = dtInvoice.Rows[0]["TransID"].ToString();
                    dtDetail = ObjPurchaseInvoiceDetail.GetPurchaseInvoiceDetailByInvoiceNo(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), dtInvoice.Rows[0]["TransID"].ToString());

                    ddlProduct.DataSource = dtDetail;
                    ddlProduct.DataTextField = "ProductName";
                    ddlProduct.DataValueField = "ProductId";
                    ddlProduct.DataBind();
                }
                else
                {
                    ResetDetailsection();
                }

            }

        }
        else
        {
            ResetDetailsection();
        }
    }



    protected void txtSerialNo_OnTextChanged(object sender, EventArgs e)
    {
        string strsql = string.Empty;
        DataTable dtInvoice = new DataTable();
        DataTable dtDetail = new DataTable();
        ddlProduct.Items.Clear();


        if (ddlJobType.SelectedIndex == 0)
        {

            strsql = "select TransType,TransTypeId,ProductId from Inv_StockBatchMaster where Inv_StockBatchMaster.Company_Id=" + Session["CompId"].ToString() + " and Inv_StockBatchMaster.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_StockBatchMaster.Location_Id=" + Session["LocId"].ToString() + " and  Inv_StockBatchMaster.TransType in ('SI','DV') and Inv_StockBatchMaster.SerialNo='" + txtSerialNo.Text.Trim() + "' ";
        }
        else
        {
            strsql = "select TransType,TransTypeId,ProductId from Inv_StockBatchMaster where Inv_StockBatchMaster.Company_Id=" + Session["CompId"].ToString() + " and Inv_StockBatchMaster.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_StockBatchMaster.Location_Id=" + Session["LocId"].ToString() + " and  Inv_StockBatchMaster.TransType in ('PG') and Inv_StockBatchMaster.SerialNo='" + txtSerialNo.Text.Trim() + "' ";
        }
        DataTable dt = objDa.return_DataTable(strsql);

        if (dt.Rows.Count == 0)
        {
            DisplayMessage("Invoice Not Found");
            ResetDetailsection();
            return;
        }
        else
        {



            if (ddlJobType.SelectedIndex == 0)
            {

                if (dt.Rows[0]["TransType"].ToString() == "SI")
                {
                    dtInvoice = objSinvoiceHeader.GetSInvHeaderAllByTransId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["TransTypeId"].ToString());

                }
                else if (dt.Rows[0]["TransType"].ToString() == "DV")
                {
                    strsql = "select Inv_SalesInvoiceHeader.Trans_Id,Inv_SalesInvoiceHeader.Invoice_No,Inv_SalesInvoiceHeader.Invoice_Date,Inv_SalesInvoiceHeader.Supplier_Id,Ems_contactMaster.Name as CustomerName from Inv_SalesInvoiceHeader inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id =Inv_SalesInvoiceDetail.Invoice_No inner join ems_contactmaster on Inv_SalesInvoiceHeader.Supplier_Id= ems_contactmaster.Trans_Id where Inv_SalesInvoiceDetail.SIFromTransNo in (select Order_Id from Inv_SalesDeliveryVoucher_Detail where Voucher_No=" + dt.Rows[0]["TransTypeId"].ToString() + " and Product_Id=" + dt.Rows[0]["ProductId"].ToString() + ")";

                    dtInvoice = objDa.return_DataTable(strsql);
                }

                if (dtInvoice.Rows.Count > 0)
                {
                    txtsalesinvoice.Text = dtInvoice.Rows[0]["Invoice_No"].ToString();
                    txtInvoicedate.Text = Convert.ToDateTime(dtInvoice.Rows[0]["Invoice_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    hdnInvoiceId.Value = dtInvoice.Rows[0]["Trans_Id"].ToString();
                    dtDetail = objSinvoiceDetail.GetSInvDetailByInvoiceNo(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["Locid"].ToString(), dtInvoice.Rows[0]["Trans_Id"].ToString(), Session["FinanceYearId"].ToString());

                    if (txtECustomer.Text == "")
                    {
                        txtECustomer.Text = dtInvoice.Rows[0]["CustomerName"].ToString() + "///" + dtInvoice.Rows[0]["Supplier_Id"].ToString();
                        Session["ContactID"] = dtInvoice.Rows[0]["Supplier_Id"].ToString();
                    }

                    dtDetail = new DataView(dtDetail, "Product_Id=" + dt.Rows[0]["ProductId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

                    ddlProduct.DataSource = dtDetail;
                    ddlProduct.DataTextField = "ProductName";
                    ddlProduct.DataValueField = "Product_Id";
                    ddlProduct.DataBind();
                }
                else
                {
                    DisplayMessage("Invoice Not Found");
                    ResetDetailsection();
                    return;
                }
            }
            else
            {

                dtInvoice = ObjPurchaseInvoice.GetPurchaseInvoiceTrueAllByTransId(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["LocId"].ToString(), dt.Rows[0]["TransTypeId"].ToString());

                if (dtInvoice.Rows.Count > 0)
                {
                    txtsalesinvoice.Text = dtInvoice.Rows[0]["InvoiceNo"].ToString();
                    txtInvoicedate.Text = Convert.ToDateTime(dtInvoice.Rows[0]["InvoiceDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
                    hdnInvoiceId.Value = dtInvoice.Rows[0]["TransId"].ToString();
                    DataTable DtDetail = ObjPurchaseInvoiceDetail.GetPurchaseInvoiceDetailByInvoiceNo(Session["CompId"].ToString(), Session["Brandid"].ToString(), Session["LocId"].ToString(), hdnInvoiceId.Value);


                    dtDetail = new DataView(dtDetail, "ProductId=" + dt.Rows[0]["ProductId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

                    ddlProduct.DataSource = dtDetail;
                    ddlProduct.DataTextField = "ProductName";
                    ddlProduct.DataValueField = "ProductId";
                    ddlProduct.DataBind();

                }

            }
        }

        txtQty.Text = "1";
    }


    protected void btnItemSave_Click(object sender, EventArgs e)
    {

        if (txtsalesinvoice.Text.Trim() == "")
        {
            hdnInvoiceId.Value = "0";
        }
        if ((txtItemCode.Text == "" && txtItemname.Text == "") && ddlProduct == null)
        {
            DisplayMessage("Product Not Found");
            txtsalesinvoice.Focus();
            return;
        }
        if (txtAssignedTo.Text.Trim() == "")
        {
            DisplayMessage("Assigned Person Not Found");
            txtAssignedTo.Focus();
            return;
        }

        if (txtItemProblem.Text.Trim() == "")
        {
            DisplayMessage("Enter Problem");
            txtItemProblem.Focus();
            return;
        }

        if (txtQty.Text == "")
        {
            txtQty.Text = "1";
        }


        if (ddlProduct != null)
        {
            if (ddlProduct.SelectedIndex != -1)
            {
                hdnItemId.Value = ddlProduct.SelectedValue;
            }
        }

        if (hdnItemId.Value == "0" && hdnInvoiceId.Value == "0")
        {
            DisplayMessage("Please Enter Product Details Product Id, Product Name, Unit Name, Quantity and Unit Price");
            return;
        }

        DataTable dt = new DataTable();

        //foreach (DataRow dr in dtDetail.Rows)
        //{
        //    if (objDa.return_DataTable("select * from SM_GetPass_Detail where Job_No=" + dr["Trans_Id"].ToString() + "").Rows.Count > 0)
        //    {
        //        GvItemDetail.Enabled = false;
        //        break;
        //    }
        //}

        if (hdnItemEditId.Value.Trim() == "0")
        {


            dt.Columns.Add("Serial_No", typeof(float));
            dt.Columns.Add("Invoice_Id", typeof(float));
            dt.Columns.Add("Invoice_No");
            dt.Columns.Add("Invoice_Date");
            dt.Columns.Add("ProductSerialNo");
            dt.Columns.Add("ProductId");
            dt.Columns.Add("Qty");
            dt.Columns.Add("Status");
            dt.Columns.Add("Responsible_Person");
            dt.Columns.Add("DeliveryDate");
            dt.Columns.Add("Problem");
            dt.Columns.Add("Field3");
            if (GvItemDetail.Rows.Count == 0)
            {

                DataRow dr = dt.NewRow();

                dr["Serial_No"] = 1;
                dr["Invoice_Id"] = hdnInvoiceId.Value;
                dr["Invoice_No"] = txtsalesinvoice.Text.Split('/')[0].ToString();
                dr["Invoice_Date"] = txtInvoicedate.Text;
                dr["ProductSerialNo"] = txtSerialNo.Text;
                dr["ProductId"] = hdnItemId.Value;
                dr["Qty"] = txtQty.Text;
                dr["Status"] = ddlIemStatus.SelectedValue;

                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_Code = txtAssignedTo.Text.Split('/')[1].ToString();
                string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                if (Emp_ID == "0" || Emp_ID == "")
                {
                    DisplayMessage("Employee not exists");
                    txtAssignedTo.Focus();
                    txtAssignedTo.Text = "";
                    return;
                }
                //dr["Responsible_Person"] = txtAssignedTo.Text.Split('/')[1].ToString();
                dr["Responsible_Person"] = Emp_ID;
                dr["DeliveryDate"] = txtdelivertydate.Text;
                dr["Problem"] = txtItemProblem.Text;
                dr["Field3"] = txtAction.Text;
                dt.Rows.Add(dr);
            }
            else
            {
                dt = getItemDetailinDatatable();



                DataRow dr = dt.NewRow();

                dr["Serial_No"] = float.Parse(new DataView(dt, "", "Serial_No desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["Serial_No"].ToString()) + 1;

                dr["Invoice_Id"] = hdnInvoiceId.Value;
                dr["Invoice_No"] = txtsalesinvoice.Text.Split('/')[0].ToString();
                dr["Invoice_Date"] = txtInvoicedate.Text;
                dr["ProductSerialNo"] = txtSerialNo.Text;
                dr["ProductId"] = hdnItemId.Value;
                dr["Qty"] = txtQty.Text;
                dr["Status"] = ddlIemStatus.SelectedValue;
                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                string Emp_Code = txtAssignedTo.Text.Split('/')[1].ToString();
                string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                if (Emp_ID == "0" || Emp_ID == "")
                {
                    DisplayMessage("Employee not exists");
                    txtAssignedTo.Focus();
                    txtAssignedTo.Text = "";
                    return;
                }
                //dr["Responsible_Person"] = txtAssignedTo.Text.Split('/')[1].ToString();
                dr["Responsible_Person"] = Emp_ID;
                dr["DeliveryDate"] = txtdelivertydate.Text;
                dr["Problem"] = txtItemProblem.Text;
                dr["Field3"] = txtAction.Text;
                dt.Rows.Add(dr);
            }
        }
        else
        {
            dt = getItemDetailinDatatable();


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Serial_No"].ToString() == hdnItemEditId.Value.Trim())
                {
                    DataTable dtDetail = objJobCardItemdetail.GetAllRecord_By_HeaderId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnValue.Value);

                    foreach (DataRow dr in dtDetail.Rows)
                    {
                        if (objDa.return_DataTable("select * from SM_GetPass_Detail where Job_No=" + dr["Trans_Id"].ToString() + "").Rows.Count > 0)
                        {
                            if (ProductCode(dt.Rows[i]["ProductId"].ToString()) != txtItemCode.Text.Trim())
                            {
                                DisplayMessage("Cant Change The Product Name.");
                                return;
                            }
                        }
                    }

                    HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                    string Emp_Code = txtAssignedTo.Text.Split('/')[1].ToString();
                    string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
                    if (Emp_ID == "0" || Emp_ID == "")
                    {
                        DisplayMessage("Employee not exists");
                        txtAssignedTo.Focus();
                        txtAssignedTo.Text = "";
                        return;
                    }
                    dt.Rows[i]["Responsible_Person"] = Emp_ID;
                    //dt.Rows[i]["Responsible_Person"] = txtAssignedTo.Text.Split('/')[1].ToString();

                    dt.Rows[i]["Qty"] = txtQty.Text;

                    dt.Rows[i]["Status"] = ddlIemStatus.SelectedValue;

                    dt.Rows[i]["DeliveryDate"] = txtdelivertydate.Text;

                    dt.Rows[i]["Problem"] = txtItemProblem.Text;

                    dt.Rows[i]["Field3"] = txtAction.Text;

                }
            }
        }
        objPageCmn.FillData((object)GvItemDetail, dt, "", "");
        ResetDetailsection();
    }
    protected void btnItemCancel_Click(object sender, EventArgs e)
    {
        ResetDetailsection();

    }


    public DataTable getItemDetailinDatatable()
    {


        DataTable dt = new DataTable();


        dt.Columns.Add("Serial_No", typeof(float));
        dt.Columns.Add("Invoice_Id", typeof(float));
        dt.Columns.Add("Invoice_No");
        dt.Columns.Add("Invoice_Date");
        dt.Columns.Add("ProductSerialNo");
        dt.Columns.Add("ProductId");
        dt.Columns.Add("Qty");
        dt.Columns.Add("Status");
        dt.Columns.Add("Responsible_Person");
        dt.Columns.Add("DeliveryDate");
        dt.Columns.Add("Problem");
        dt.Columns.Add("Field3");

        foreach (GridViewRow gvrow in GvItemDetail.Rows)
        {


            DataRow dr = dt.NewRow();
            dr["Serial_No"] = ((Label)gvrow.FindControl("lblSNo")).Text;
            dr["Invoice_Id"] = ((Label)gvrow.FindControl("lblgvInvoiceId")).Text;
            dr["Invoice_No"] = ((Label)gvrow.FindControl("lblgvInvoiceNo")).Text;
            dr["Invoice_Date"] = ((Label)gvrow.FindControl("lblgvInvoiceDate")).Text;
            dr["ProductSerialNo"] = ((Label)gvrow.FindControl("lblSerialNo")).Text;
            dr["ProductId"] = ((Label)gvrow.FindControl("lblgvProductId")).Text;
            dr["Qty"] = ((Label)gvrow.FindControl("lblgvRequiredQty")).Text;
            dr["Status"] = ((Label)gvrow.FindControl("lblgvStatus")).Text;
            dr["Responsible_Person"] = ((Label)gvrow.FindControl("lblgvAssignedtoId")).Text;
            dr["DeliveryDate"] = ((Label)gvrow.FindControl("lblDeliverydate")).Text;
            dr["Problem"] = ((Label)gvrow.FindControl("lblgvProblem")).Text;
            dr["Field3"] = ((Label)gvrow.FindControl("lblgvAction")).Text;
            dt.Rows.Add(dr);
        }
        return dt;
    }

    public string SuggestedProductName(string ProductId)
    {
        string ProductName = string.Empty;
        DataTable dt = new DataTable();
        try
        {
            dt = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        }
        catch
        {

        }
        if (dt.Rows.Count != 0)
        {
            ProductName = dt.Rows[0]["EProductName"].ToString();
        }
        return ProductName;
    }
    public string ProductCode(string ProductId)
    {
        string ProductName = string.Empty;

        DataTable dt = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
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

    protected string GetUnitName(string ProductId)
    {
        string strUnitName = string.Empty;

        string strUnitId = string.Empty;


        DataTable dt = new DataTable();
        try
        {
            dt = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        }
        catch
        {

        }
        if (dt.Rows.Count != 0)
        {
            strUnitId = dt.Rows[0]["UnitId"].ToString();
        }

        if (strUnitId != "0" && strUnitId != "")
        {
            DataTable dtUName = objUnitMaster.GetUnitMasterById(Session["CompId"].ToString(), strUnitId);
            if (dtUName.Rows.Count > 0)
            {
                strUnitName = dtUName.Rows[0]["Unit_Name"].ToString();
            }
        }
        else
        {
            strUnitName = "";
        }
        return strUnitName;
    }

    public string UnitName(string UnitId)
    {
        string UnitName = string.Empty;
        DataTable dt = objUnitMaster.GetUnitMasterById(Session["CompId"].ToString(), UnitId.ToString());
        if (dt.Rows.Count != 0)
        {
            UnitName = dt.Rows[0]["Unit_Name"].ToString();
        }
        return UnitName;
    }
    protected string GetAssignedPersonName(string strEmpId)
    {
        string strEmpName = string.Empty;

        DataTable dtEmp = objEmpMaster.GetEmployeeMasterById(Session["CompId"].ToString(), strEmpId);


        if (dtEmp.Rows.Count > 0)
        {
            strEmpName = dtEmp.Rows[0]["Emp_Name"].ToString();
        }

        return strEmpName;
    }

    protected void imgBtnItemEdit_Command(object sender, CommandEventArgs e)
    {

        hdnItemEditId.Value = e.CommandArgument.ToString();
        DataTable dt = getItemDetailinDatatable();
        dt = new DataView(dt, "Serial_No=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {

            txtSerialNo.Text = dt.Rows[0]["ProductSerialNo"].ToString();
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = HR_EmployeeDetail.GetEmployeeCode(dt.Rows[0]["Responsible_Person"].ToString());
            if (Emp_Code == "0" || Emp_Code == "")
            {
                DisplayMessage("Employee not exists");
                return;
            }
            txtAssignedTo.Text = GetAssignedPersonName(dt.Rows[0]["Responsible_Person"].ToString()) + "/" + Emp_Code;
            txtsalesinvoice.Text = dt.Rows[0]["Invoice_No"].ToString();
            if (txtsalesinvoice.Text.Trim() != "")
            {
                txtsalesinvoice_OnTextChanged(null, null);
            }

            try
            {
                txtInvoicedate.Text = Convert.ToDateTime(dt.Rows[0]["Invoice_Date"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());

            }
            catch
            {
                txtInvoicedate.Text = "";

            }

            hdnItemId.Value = dt.Rows[0]["ProductId"].ToString();
            txtItemname.Text = SuggestedProductName(dt.Rows[0]["ProductId"].ToString());
            txtItemCode.Text = ProductCode(dt.Rows[0]["ProductId"].ToString());

            txtQty.Text = dt.Rows[0]["Qty"].ToString();
            ddlIemStatus.SelectedValue = dt.Rows[0]["Status"].ToString();

            txtItemProblem.Text = dt.Rows[0]["Problem"].ToString();
            txtdelivertydate.Text = dt.Rows[0]["DeliveryDate"].ToString();
            txtAction.Text = dt.Rows[0]["Field3"].ToString();

            txtSerialNo.Enabled = false;
            txtsalesinvoice.Enabled = false;

        }
    }

    #region Close
    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
        //AllPageCode();

    }
    #endregion
    protected void imgBtnItemDelete_Command(object sender, CommandEventArgs e)
    {
        hdnItemEditId.Value = e.CommandArgument.ToString();
        DataTable dt = getItemDetailinDatatable();
        dt = new DataView(dt, "Serial_No<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        objPageCmn.FillData((object)GvItemDetail, dt, "", "");
        ResetDetailsection();
    }

    public void BingRefProductList()
    {

        ddlrefProductName.Items.Clear();
        ListItem Li0 = new ListItem();
        Li0.Text = "--Select--";
        Li0.Value = "0";

        ddlrefProductName.Items.Insert(0, Li0);

        int counter = 0;
        foreach (GridViewRow gvrow in GvItemDetail.Rows)
        {
            counter++;
            ListItem Li = new ListItem();

            Li.Text = SuggestedProductName(((Label)gvrow.FindControl("lblgvProductId")).Text);
            Li.Value = ((Label)gvrow.FindControl("lblgvProductId")).Text;

            ddlrefProductName.Items.Insert(counter, Li);
        }
        ddlrefProductName.SelectedIndex = 0;
    }




    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListItemName(string prefixText, int count, string contextKey)
    {

        Set_ApplicationParameter objAppParam = new Set_ApplicationParameter(HttpContext.Current.Session["DBConnection"].ToString());
        DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());


        DataTable dt = objDa.return_DataTable("select distinct pm.ProductCode,pm.EProductName from Inv_ProductMaster as pm  LEFT JOIN Inv_Product_CompanyBrand ON Pm.ProductId = Inv_Product_CompanyBrand.ProductId      where pm.IsActive = 'True' AND pm.Field1 = ' ' AND Pm.Field3 = 'True'  and Inv_Product_CompanyBrand.Company_Id = " + HttpContext.Current.Session["CompId"].ToString() + " AND Inv_Product_CompanyBrand.BrandId = " + HttpContext.Current.Session["BrandId"].ToString() + " and pm.EProductName like '%" + prefixText + "%'");
        //if (dt.Rows.Count == 0)
        //{
        //    dt = PM.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());

        //}
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
    public static string[] GetCompletionListItemCode(string prefixText, int count, string contextKey)
    {
        Set_ApplicationParameter objAppParam = new Set_ApplicationParameter(HttpContext.Current.Session["DBConnection"].ToString());
        DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dtcategory = objAppParam.GetApplicationParameterByParamName("Job_Card_Parts_Tools_Category", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string strcategoryList = "0";

        if (dtcategory.Rows.Count > 0)
        {
            strcategoryList = dtcategory.Rows[0]["Param_Value"].ToString();
        }


        DataTable dt = objDa.return_DataTable("select distinct pm.ProductCode,pm.EProductName from Inv_ProductMaster as pm  LEFT JOIN Inv_Product_CompanyBrand ON Pm.ProductId = Inv_Product_CompanyBrand.ProductId      where pm.IsActive = 'True' AND pm.Field1 = ' ' AND Pm.Field3 = 'True'  and Inv_Product_CompanyBrand.Company_Id = " + HttpContext.Current.Session["CompId"].ToString() + " AND Inv_Product_CompanyBrand.BrandId = " + HttpContext.Current.Session["BrandId"].ToString() + " and pm.ProductCode like '%" + prefixText + "%'");


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
    public static string[] GetCompletionListSalesInvoiceNo(string prefixText, int count, string contextKey)
    {
        SystemParameter ObjSys = new SystemParameter(HttpContext.Current.Session["DBConnection"].ToString());

        DataAccessClass ObjDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_SalesInvoiceHeader objSinvoiceHeader = new Inv_SalesInvoiceHeader(HttpContext.Current.Session["DBConnection"].ToString());
        //Inv_SalesInvoiceDetail objInvDetail = new Inv_SalesInvoiceDetail();
        DataTable dt = new DataTable();
        string[] str = new string[0];
        if (HttpContext.Current.Session["ContactID"] != null)
        {

            dt = ObjDa.return_DataTable("select invoice_no,invoice_date from inv_salesinvoiceheader where  location_id=" + HttpContext.Current.Session["LocId"].ToString() + " and isactive='True' and supplier_id=" + HttpContext.Current.Session["ContactID"].ToString() + " and invoice_no like '%" + prefixText + "%'  order by inv_salesinvoiceheader.invoice_date");

            if (dt.Rows.Count == 0)
            {
                dt = ObjDa.return_DataTable("select invoice_no,invoice_date from inv_salesinvoiceheader where  location_id=" + HttpContext.Current.Session["LocId"].ToString() + " and isactive='True' and supplier_id=" + HttpContext.Current.Session["ContactID"].ToString() + "   order by inv_salesinvoiceheader.invoice_date");

            }


        }

        if (HttpContext.Current.Session["ContactID"] != null && HttpContext.Current.Session["ProductId"] != null)
        {
            dt = ObjDa.return_DataTable("select  distinct inv_salesinvoiceheader.invoice_no,inv_salesinvoiceheader.invoice_date from inv_salesinvoiceheader   inner join Inv_SalesInvoiceDetail on  inv_salesinvoiceheader.Trans_Id =Inv_SalesInvoiceDetail.Invoice_No    where  inv_salesinvoiceheader.location_id=" + HttpContext.Current.Session["LocId"].ToString() + " and inv_salesinvoiceheader.isactive='True' and inv_salesinvoiceheader.supplier_id=" + HttpContext.Current.Session["ContactID"].ToString() + " and Inv_SalesInvoiceDetail.Product_Id=" + HttpContext.Current.Session["ProductId"].ToString() + "  and  inv_salesinvoiceheader.invoice_no like '%" + prefixText + "%'  order by inv_salesinvoiceheader.invoice_date");


            if (dt.Rows.Count == 0)
            {
                dt = ObjDa.return_DataTable("select  distinct inv_salesinvoiceheader.invoice_no,inv_salesinvoiceheader.invoice_date from inv_salesinvoiceheader   inner join Inv_SalesInvoiceDetail on  inv_salesinvoiceheader.Trans_Id =Inv_SalesInvoiceDetail.Invoice_No    where  inv_salesinvoiceheader.location_id=" + HttpContext.Current.Session["LocId"].ToString() + " and inv_salesinvoiceheader.isactive='True' and inv_salesinvoiceheader.supplier_id=" + HttpContext.Current.Session["ContactID"].ToString() + " and Inv_SalesInvoiceDetail.Product_Id=" + HttpContext.Current.Session["ProductId"].ToString() + " order by inv_salesinvoiceheader.invoice_date");
            }


        }








        if (dt != null)
        {
            str = new string[dt.Rows.Count];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = dt.Rows[i]["Invoice_No"].ToString() + " / " + Convert.ToDateTime(dt.Rows[i]["Invoice_Date"].ToString()).ToString(ObjSys.SetDateFormat());
                }
            }
        }
        return str;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListPurchaseInvoiceNo(string prefixText, int count, string contextKey)
    {
        PurchaseInvoice objPInvoiceHeader = new PurchaseInvoice(HttpContext.Current.Session["DBConnection"].ToString());
        string[] str = new string[0];
        DataTable dtTemp = new DataTable();
        DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());


        DataTable dt = objPInvoiceHeader.GetPurchaseInvoiceTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        if (HttpContext.Current.Session["ProductId"] != null)
        {

            dt = objDa.return_DataTable("select  distinct Inv_PurchaseInvoiceHeader.InvoiceNo from Inv_PurchaseInvoiceHeader   inner join Inv_PurchaseInvoiceDetail on  Inv_PurchaseInvoiceHeader.TransID	 =Inv_PurchaseInvoiceDetail.InvoiceNo    where  Inv_PurchaseInvoiceHeader.Location_ID=" + HttpContext.Current.Session["LocId"].ToString() + " and Inv_PurchaseInvoiceHeader.isactive='True' and Inv_PurchaseInvoiceDetail.ProductId=" + HttpContext.Current.Session["ProductId"].ToString() + "    order by Inv_PurchaseInvoiceHeader.invoice_date");
        }

        if (dt != null)
        {

            dtTemp = dt.Copy();


            dt = new DataView(dt, "InvoiceNo like '%" + prefixText + "%'", "", DataViewRowState.CurrentRows).ToTable();

            if (dt.Rows.Count == 0)
            {
                dt = dtTemp.Copy();
            }

            str = new string[dt.Rows.Count];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    str[i] = dt.Rows[i]["InvoiceNo"].ToString();
                }
            }
        }

        return str;
    }

    #endregion

    #region SparePartsDetail

    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {



        if (((TextBox)sender).Text != "")
        {
            DataTable dt = new DataTable();
            dt = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ((TextBox)sender).Text.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                FillUnit(dt.Rows[0]["ProductId"].ToString());
                txtProductName.Text = dt.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dt.Rows[0]["ProductCode"].ToString();
                hdnProductId.Value = dt.Rows[0]["ProductId"].ToString();
            }
            else
            {
                DisplayMessage("Product Not Found");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
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

    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        if (txtProductName.Text != "")
        {
            DataTable dt = new DataTable();
            dt = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtProductName.Text.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                FillUnit(dt.Rows[0]["ProductId"].ToString());
                txtProductName.Text = dt.Rows[0]["EProductName"].ToString();
                txtProductcode.Text = dt.Rows[0]["ProductCode"].ToString();
                hdnProductId.Value = dt.Rows[0]["ProductId"].ToString();
            }
            else
            {
                DisplayMessage("Product Not Found");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
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

    public void FillUnit(string ProductId)
    {

        Inventory_Common_Page.FillUnitDropDown_ByProductId(ddlUnit, ProductId, Session["DBConnection"].ToString());

    }




    public DataTable getSpareDetailinDatatable()
    {


        DataTable dt = new DataTable();


        dt.Columns.Add("Trans_Id", typeof(float));
        dt.Columns.Add("Ref_Product_Id");
        dt.Columns.Add("ProductId");
        dt.Columns.Add("Unit");
        dt.Columns.Add("Quantity");
        dt.Columns.Add("Unit_Price");
        dt.Columns.Add("Field1");

        decimal totalAmt = 0;
        decimal labourCharges = 0;
        decimal otherCharges = 0;

        foreach (GridViewRow gvrow in gvProductRequest.Rows)
        {
            DataRow dr = dt.NewRow();
            dr["Trans_Id"] = ((Label)gvrow.FindControl("lblTransId")).Text;
            dr["Ref_Product_Id"] = ((Label)gvrow.FindControl("lblrefproductId")).Text;
            dr["ProductId"] = ((Label)gvrow.FindControl("lblPID")).Text;
            dr["Unit"] = ((Label)gvrow.FindControl("lblUID")).Text;
            dr["Quantity"] = ((Label)gvrow.FindControl("lblReqQty")).Text;
            dr["Unit_Price"] = ((Label)gvrow.FindControl("lblUnitPrice")).Text;
            dr["Field1"] = ((HiddenField)gvrow.FindControl("hdncharge")).Value;
            dt.Rows.Add(dr);
            //if(dr["ItemType"].ToString()=="S")
            if (dr["Field1"].ToString() == "Labour")
            {
                labourCharges += Convert.ToDecimal(dr["Quantity"].ToString()) * Convert.ToDecimal(dr["Unit_Price"].ToString());
            }
            //if (dr["ItemType"].ToString() == "NS")
            if (dr["Field1"].ToString() == "Other")
            {
                otherCharges += Convert.ToDecimal(dr["Quantity"].ToString()) * Convert.ToDecimal(dr["Unit_Price"].ToString());
            }

            totalAmt += Convert.ToDecimal(dr["Quantity"].ToString()) * Convert.ToDecimal(dr["Unit_Price"].ToString());

        }
        lblTotal.Text = "Total Amt: " + SetDecimal(totalAmt.ToString());
        txtTotalCharges.Text = SetDecimal(totalAmt.ToString());
        lblLabourCharges.Text = "Labour Charges: " + SetDecimal(labourCharges.ToString());
        lblOtherCharges.Text = "Other Charges: " + SetDecimal(otherCharges.ToString());
        return dt;
    }


    protected void btnProductSave_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();

        if (gvProductRequest.Rows.Count == 0)
        {
            dt.Columns.Add("Trans_Id", typeof(float));
            dt.Columns.Add("Ref_Product_Id");
            dt.Columns.Add("ProductId");
            dt.Columns.Add("Unit");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Unit_Price");
            dt.Columns.Add("Field1");

            DataRow dr = dt.NewRow();

            dr["Trans_Id"] = 1;
            dr["Ref_Product_Id"] = ddlrefProductName.SelectedValue;
            dr["ProductId"] = hdnProductId.Value;
            dr["Unit"] = ddlUnit.SelectedValue;
            dr["Quantity"] = txtRequestQty.Text;
            dr["Unit_Price"] = txtUnitPrice.Text;
            dr["Field1"] = ddlCharges.SelectedValue;

            dt.Rows.Add(dr);

            lblTotal.Text = "Total Amt: " + SetDecimal((Convert.ToDecimal(dr["Quantity"].ToString()) * Convert.ToDecimal(dr["Unit_Price"].ToString())).ToString());
            txtTotalCharges.Text = SetDecimal((Convert.ToDecimal(dr["Quantity"].ToString()) * Convert.ToDecimal(dr["Unit_Price"].ToString())).ToString());
            //if(hdnItemType.Value=="S")
            if (ddlCharges.SelectedValue == "Labour")
            {
                lblLabourCharges.Text = "Labour Charges: " + SetDecimal((Convert.ToDecimal(dr["Quantity"].ToString()) * Convert.ToDecimal(dr["Unit_Price"].ToString())).ToString());
            }
            //if (hdnItemType.Value == "NS")
            if (ddlCharges.SelectedValue == "Other")
            {
                lblOtherCharges.Text = "Other Charges: " + SetDecimal((Convert.ToDecimal(dr["Quantity"].ToString()) * Convert.ToDecimal(dr["Unit_Price"].ToString())).ToString());
            }
        }
        else
        {

            dt = getSpareDetailinDatatable();



            DataRow dr = dt.NewRow();

            dr["Trans_Id"] = float.Parse(new DataView(dt, "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable().Rows[0]["Trans_Id"].ToString()) + 1;

            dr["Ref_Product_Id"] = ddlrefProductName.SelectedValue;
            dr["ProductId"] = hdnProductId.Value;
            dr["Unit"] = ddlUnit.SelectedValue;
            dr["Quantity"] = txtRequestQty.Text;
            dr["Unit_Price"] = txtUnitPrice.Text;
            dr["Field1"] = ddlCharges.SelectedValue;
            dt.Rows.Add(dr);
        }

        objPageCmn.FillData((object)gvProductRequest, dt, "", "");
        getSpareDetailinDatatable();
        ResetSpareDetailSection();


    }


    public string getLineTotal(string quantity, string unitPrice)
    {
        string strLineTotal = string.Empty;

        if (quantity == "")
        {
            quantity = "1";
        }

        if (unitPrice == "")
        {
            unitPrice = "0";
        }


        strLineTotal = SetDecimal((Convert.ToDouble(quantity) * Convert.ToDouble(unitPrice)).ToString());

        return strLineTotal;
    }


    public string SetDecimal(string amount)
    {
        if (amount == "")
        {
            amount = "0";
        }
        //return objSysParam.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), amount);
        return amount;
    }

    protected void IbtnDelete_Command1(object sender, CommandEventArgs e)
    {
        DataTable dt = getSpareDetailinDatatable();
        dt = new DataView(dt, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        objPageCmn.FillData((object)gvProductRequest, dt, "", "");
        getSpareDetailinDatatable();
    }
    protected void btnProductCancel_Click(object sender, EventArgs e)
    {
        ResetSpareDetailSection();

    }

    public void ResetSpareDetailSection()
    {

        ddlrefProductName.SelectedIndex = 0;
        txtProductcode.Text = "";
        txtProductName.Text = "";
        txtRequestQty.Text = "1";
        txtUnitPrice.Text = "0";
        txtProductcode.Focus();
        hdnItemType.Value = "";
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {

        Set_ApplicationParameter objAppParam = new Set_ApplicationParameter(HttpContext.Current.Session["DBConnection"].ToString());
        DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dtcategory = objAppParam.GetApplicationParameterByParamName("Job_Card_Parts_Tools_Category", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string strcategoryList = "0";

        if (dtcategory.Rows.Count > 0)
        {
            strcategoryList = dtcategory.Rows[0]["Param_Value"].ToString();
        }

        DataTable dt = objDa.return_DataTable("select distinct pm.ProductCode,pm.EProductName from Inv_ProductMaster as pm  LEFT JOIN Inv_Product_CompanyBrand ON Pm.ProductId = Inv_Product_CompanyBrand.ProductId  left join Inv_Product_Category on pm.ProductId=Inv_Product_Category.ProductId    where pm.IsActive = 'True' AND pm.Field1 = ' ' AND Pm.Field3 = 'True' and Inv_Product_Category.CategoryId in (" + strcategoryList + ") and Inv_Product_CompanyBrand.Company_Id = " + HttpContext.Current.Session["CompId"].ToString() + " AND Inv_Product_CompanyBrand.BrandId = " + HttpContext.Current.Session["BrandId"].ToString() + " and pm.EProductName like '%" + prefixText + "%'");
        //if (dt.Rows.Count == 0)
        //{
        //    dt = PM.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());

        //}
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
        Set_ApplicationParameter objAppParam = new Set_ApplicationParameter(HttpContext.Current.Session["DBConnection"].ToString());
        DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dtcategory = objAppParam.GetApplicationParameterByParamName("Job_Card_Parts_Tools_Category", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        string strcategoryList = "0";

        if (dtcategory.Rows.Count > 0)
        {
            strcategoryList = dtcategory.Rows[0]["Param_Value"].ToString();
        }


        DataTable dt = objDa.return_DataTable("select distinct pm.ProductCode,pm.EProductName from Inv_ProductMaster as pm  LEFT JOIN Inv_Product_CompanyBrand ON Pm.ProductId = Inv_Product_CompanyBrand.ProductId  left join Inv_Product_Category on pm.ProductId=Inv_Product_Category.ProductId    where pm.IsActive = 'True' AND pm.Field1 = ' ' AND Pm.Field3 = 'True' and Inv_Product_Category.CategoryId in (" + strcategoryList + ") and Inv_Product_CompanyBrand.Company_Id = " + HttpContext.Current.Session["CompId"].ToString() + " AND Inv_Product_CompanyBrand.BrandId = " + HttpContext.Current.Session["BrandId"].ToString() + " and  pm.ProductCode like '%" + prefixText + "%'");


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
    #endregion


    #region Print
    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        //Code for Approval
        string strCmd = string.Format("window.open('../ServiceManagementReport/JobCardReport.aspx?Id=" + e.CommandArgument.ToString() + "&&Customer_Id=" + e.CommandName.ToString() + "','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }

    #endregion

    #region CustomerHistory

    protected void lnkcustomerHistory_OnClick(object sender, EventArgs e)
    {
        string CustomerId = string.Empty;


        if (txtECustomer.Text != "")
        {
            try
            {
                CustomerId = txtECustomer.Text.Split('/')[3].ToString();
            }
            catch
            {
                CustomerId = "0";
            }
        }
        else
        {
            CustomerId = "0";
        }

        string strCmd = string.Format("window.open('../Purchase/CustomerHistory.aspx?ContactId=" + CustomerId + "&&Page=SINV','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
    #endregion



    #region Invoicesaving

    public void SaveInvoice(string JobCardId, string strCustomerId, ref SqlTransaction trns)
    {
        using (DataTable _dt = objSinvoiceHeader.GetSInvHeaderAllByFromTransType(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "J", JobCardId))
        {
            if (_dt.Rows.Count > 0)
            {
                return;
            }
        }
        string strAddressId = string.Empty;
        //here we are getting shipping address and invoice address
        DataTable dtAddress = objContact.GetAddressByRefType_Id("Contact", strCustomerId, ref trns);
        if (dtAddress != null && dtAddress.Rows.Count > 0)
        {
            strAddressId = dtAddress.Rows[0]["Trans_id"].ToString();

        }

        Set_DocNumber objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());

        Inv_StockDetail objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());


        double TotalQty = 0;
        double TotalAmount = 0;


        foreach (GridViewRow gvrow in gvProductRequest.Rows)
        {

            TotalQty += Convert.ToDouble(((Label)gvrow.FindControl("lblReqQty")).Text);
            TotalAmount += Convert.ToDouble(((Label)gvrow.FindControl("lblLineTotal")).Text);

        }


        if (TotalAmount == 0)
        {
            return;
        }
        HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
        string Emp_Code = txtHandledEmp.Text.Split('/')[1].ToString();
        string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
        if (Emp_ID == "0" || Emp_ID == "")
        {
            DisplayMessage("Employee not exists");
            txtHandledEmp.Focus();
            txtHandledEmp.Text = "";
            return;
        }

        if (txtEnddate.Text.Trim() == "01-Jan-1900")
        {
            txtEnddate.Text = System.DateTime.Now.ToString("dd-MMM-yyyy");
        }

        int b = objSinvoiceHeader.InsertSInvHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "", objSysParam.getDateForInput(txtEnddate.Text).ToString(), "0", SystemParameter.GetLocationCurrencyId(Session["DBConnection"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "J", JobCardId, Emp_ID, "", "", "", "0", "", false.ToString(), "0", TotalAmount.ToString(), TotalQty.ToString(), TotalAmount.ToString(), "0", "0", TotalAmount.ToString(), "0", "0", TotalAmount.ToString(), strCustomerId, "", "0", txtJobNo.Text, "", TotalAmount.ToString(), "", "", "", strAddressId, "0", strAddressId, "Approved", "1", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "0", "0", ref trns);


        //update invoice id in job card detail table

        objDa.execute_Command("update SM_JobCards_Header set Field2='" + b.ToString() + "' where Trans_Id=" + JobCardId + "", ref trns);

        string strVoucherNo = objDocNo.GetDocumentNo(true, "0", false, "13", "92", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());


        DataTable Dttemp = new DataTable();
        DataTable dtCount = objSinvoiceHeader.GetSInvHeaderAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);

        string strInvoiceNo = string.Empty;

        if (dtCount.Rows.Count == 0)
        {
            objSinvoiceHeader.Updatecode(b.ToString(), strVoucherNo + "1", ref trns);
            strInvoiceNo = strVoucherNo + "1";
        }
        else
        {
            DataTable dtCount1 = new DataView(dtCount, "Invoice_No='" + strVoucherNo + dtCount.Rows.Count + "'", "", DataViewRowState.CurrentRows).ToTable();
            int NoRow = dtCount.Rows.Count;
            if (dtCount1.Rows.Count > 0)
            {
                bool bCodeFlag = true;
                while (bCodeFlag)
                {
                    NoRow += 1;
                    DataTable dtTemp = new DataView(dtCount, "Invoice_No='" + strVoucherNo + NoRow + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtTemp.Rows.Count == 0)
                    {
                        bCodeFlag = false;
                    }
                }
            }

            objSinvoiceHeader.Updatecode(b.ToString(), strVoucherNo + NoRow.ToString(), ref trns);
            strInvoiceNo = strVoucherNo + NoRow.ToString();

        }


        int counter = 0;
        string AvgCost = string.Empty;

        foreach (GridViewRow gvrow in gvProductRequest.Rows)
        {

            try
            {
                AvgCost = objStockDetail.GetStockDetail_By_CompanyId_BrandId_LocationId_and_ProductId_and_FinanceyearId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), ((Label)gvrow.FindControl("lblPID")).Text).Rows[0]["Field2"].ToString();
            }
            catch
            {
                AvgCost = "0";
            }

            objSinvoiceDetail.InsertSInvDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), b.ToString(), counter.ToString(), "0", "", "D", "0", ((Label)gvrow.FindControl("lblPID")).Text, "", ((Label)gvrow.FindControl("lblUID")).Text, ((Label)gvrow.FindControl("lblUnitPrice")).Text, "0", "0", ((Label)gvrow.FindControl("lblReqQty")).Text, "0", "0", "0", "0", "False", false.ToString(), AvgCost, "0", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

        }


        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Sales/SalesInvoicePrint.aspx?Id=" + b.ToString() + "','','height=650,width=950,scrollbars=Yes')", true);



    }

    #endregion

    //here we  set visitbility according the job type external and internal


    #region JobType
    protected void ddlJobType_OnSelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlJobType.SelectedValue.Trim() == "External")
        {
            trCustomer.Visible = true;
            trCustomerhistory.Visible = true;
            trContact.Visible = true;
            trContactNo.Visible = true;
            AutoCompleteExtender_PurchaseInvoice.Enabled = false;
            AutoCompleteExtende_SalesInvoice.Enabled = true;
            ddlProduct.AutoPostBack = true;
            ddlWarranty.SelectedIndex = 0;
        }
        else
        {
            trCustomer.Visible = false;
            trCustomerhistory.Visible = false;
            trContact.Visible = false;
            trContactNo.Visible = false;

            AutoCompleteExtender_PurchaseInvoice.Enabled = true;
            AutoCompleteExtende_SalesInvoice.Enabled = false;
            ddlProduct.AutoPostBack = false;
            ddlWarranty.SelectedIndex = 1;
        }

    }
    #endregion



    protected void btnRepairHistory_Click(object sender, EventArgs e)
    {
        if (hdnInvoiceId.Value == "")
        {
            hdnInvoiceId.Value = "0";
            itemHistory.Text = txtItemname.Text;
        }
        else
        {
            itemHistory.Text = ddlProduct.SelectedItem.Text;
            hdnItemId.Value = ddlProduct.SelectedValue;
        }

        if (hdnItemId.Value == "0")
        {
            DisplayMessage("Please Enter Product Details");
            return;
        }
        if (txtECustomer.Text.Split('/')[3].ToString() == "")
        {
            DisplayMessage("Please Enter Customer Details");
            return;
        }


        DataTable repairHistory_data = objJobCardheader.GetAllRepairHistory(Session["LocId"].ToString(), txtECustomer.Text.Split('/')[3].ToString(), hdnInvoiceId.Value, hdnItemId.Value);
        gvRepairHistory.DataSource = repairHistory_data;
        gvRepairHistory.DataBind();
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "Modal_repairHistory();", true);

    }

    public void sendMail(string EmailAdd, string customername, string expecteddate, string Itemdetails)
    {
        string Master_Email_SMTP = objAppParam.GetApplicationParameterValueByParamName("Support_Email_SMTP", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).ToString();
        string Master_Email_Port = objAppParam.GetApplicationParameterValueByParamName("Support_Email_Port", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).ToString();
        string Master_Email = objAppParam.GetApplicationParameterValueByParamName("Support_Email", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).ToString();
        string Master_Email_Password = Common.Decrypt(objAppParam.GetApplicationParameterValueByParamName("Support_Email_Password", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).ToString());
        string Email_Display_Name = objAppParam.GetApplicationParameterValueByParamName("Support_Display_Text", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).ToString();

        string MailBody_Employee = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title>New Job Card Registration</title></head><body><div style='background:#eee; height:80px; border-bottom:solid 1px #cccccc; margin-bottom:15px;'><div style='float:left;color:#064184; margin-left:10px; line-height:40px; font-weight:bold; font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:22px; letter-spacing:1px; margin-top:20px;'>Auto Reply - New Job Card Assignment</div>		</div>                <div>                	<p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; font-size:14px;'> New Job Card has been Assigned to you for : '" + customername + "' which is to be end by " + expecteddate + " </p> " + Itemdetails + " <br/> <p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:20px; font-size:14px;'> 	 <p style='color:#064184; margin-left:10px;font-family:Arial, Helvetica, sans-serif; font-size:14px; line-height:20px; font-size:14px;'> <br/> Thank you </p></body></html>";

        string strsubject = string.Empty;
        strsubject = "Job Card Detail";

        new SendMailSms(Session["DBConnection"].ToString()).SendMail_TicketInfo(EmailAdd, "", "", strsubject, MailBody_Employee, Session["CompID"].ToString(), "", Master_Email, Master_Email_Password, Email_Display_Name, Master_Email_SMTP, Master_Email_Port, "", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
    }

    protected void imgBtnItemhistory_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;



        if ((gvRow.FindControl("lblgvInvoiceId") as Label).Text == "")
        {
            hdnInvoiceId.Value = "0";
        }
        else
        {
            hdnInvoiceId.Value = (gvRow.FindControl("lblgvInvoiceId") as Label).Text;
        }

        hdnItemId.Value = e.CommandArgument.ToString();
        itemHistory.Text = e.CommandName.ToString();


        if (txtECustomer.Text.Split('/')[3].ToString() == "")
        {
            DisplayMessage("Please Enter Customer Details");
            return;
        }


        DataTable repairHistory_data = objJobCardheader.GetAllRepairHistory(ddlLoc.SelectedValue, txtECustomer.Text.Split('/')[3].ToString(), hdnInvoiceId.Value, hdnItemId.Value);
        gvRepairHistory.DataSource = repairHistory_data;
        gvRepairHistory.DataBind();
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "Modal_repairHistory();", true);
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
}