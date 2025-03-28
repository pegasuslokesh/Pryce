using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web;
using DevExpress.XtraReports.UI;
using System.Configuration;
using PegasusDataAccess;
using ClosedXML.Excel;
using System.IO;

public partial class CRM_OpportunityDashboard : System.Web.UI.Page
{
    SystemParameter ObjSysParam = null;
    FollowUp followup = null;
    Ems_ContactMaster objContact = null;
    Inv_ProductMaster objProductM = null;
    Common cmn = null;
    Attendance objAttendance = null;
    Set_AddressChild ObjAddress = null;
    CurrencyMaster objCurrency = null;
    IT_ObjectEntry objObjectEntry = null;
    OpportunityDashboard objOppoDashboard = null;
    LocationMaster ObjLocationMaster = null;
    EmployeeMaster ObjEmployeeMaster = null;
    UserMaster objUser = null;
    DataAccessClass ObjDa = null;
    PageControlCommon objPageCmn = null;

    public static string location_id;
    static string locationCondition = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        if (ddlInqData.SelectedValue == "Header")
        {
            DivHeader.Visible = true;
            DivDetail.Visible = false;
        }
        else
        {
            DivHeader.Visible = false;
            DivDetail.Visible = true;
        }

        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        followup = new FollowUp(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objProductM = new Inv_ProductMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objOppoDashboard = new OpportunityDashboard(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjEmployeeMaster = new EmployeeMaster(Session["DBConnection"].ToString());
        objUser = new UserMaster(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "396", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            fillLocation();
            ddlLocation.SelectedValue = Session["LocId"].ToString();
            FillDepartment();
            fillUser();
            fillProductCategory();
            ddlInqData_SelectedIndexChanged(null, null);
        }
    }
    public void fillProductCategory()
    {
        try
        {
            DataTable DtAllProductCat = new DataTable();
            DtAllProductCat = objOppoDashboard.getAllProductcategory();
            ddlProductCat.DataSource = DtAllProductCat;
            ddlProductCat.DataTextField = "Category_Name";
            ddlProductCat.DataValueField = "CategoryId";
            ddlProductCat.DataBind();
            ddlProductCat.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch
        {
        }
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
    private void fillGrid(int currentPageIndex = 1)
    {
        GvListData.Visible = true;
        gvDetailData.Visible = false;

        string strSearchCondition = string.Empty;
        if (ddlOption.SelectedIndex != 0 && txtValue.Text != string.Empty)
        {
            if (ddlFieldName.SelectedItem.Value == "IDate" || ddlFieldName.SelectedItem.Value == "followup_date")
            {
                strSearchCondition = ddlFieldName.SelectedValue + "='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 1)
            {
                strSearchCondition = ddlFieldName.SelectedValue + "='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                strSearchCondition = ddlFieldName.SelectedValue + " like '%" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                strSearchCondition = ddlFieldName.SelectedValue + " Like '%" + txtValue.Text.Trim() + "%'";
            }
        }
        string strWhereClause = string.Empty;
        strWhereClause = "opportunity_name <> '' and isActive='true' and Opportunity_amount <> 0";
        if (strSearchCondition != string.Empty)
        {
            strWhereClause = strWhereClause + " and " + strSearchCondition;
        }
        if (ddlUser.SelectedItem.ToString() != "--Select User--" && ddlUser.SelectedItem.ToString() != "")
        {
            strWhereClause = strWhereClause + " and  User_Id='" + ddlUser.SelectedValue + "'";
        }
        if (ddlLocation.SelectedItem.Text != "All")
        {
            strWhereClause = strWhereClause + " and  Location_Id='" + ddlLocation.SelectedValue.ToString() + "'";
        }
        if (ddlFieldName.SelectedValue == "Field4")
        {
            strWhereClause = strWhereClause + " and Field4='" + ddlAccesstype.SelectedItem + "'";
        }
        int totalRows = 0;
        using (DataTable dt = objOppoDashboard.getHeaderData((currentPageIndex - 1).ToString(), Session["GridSize"].ToString(), GvListData.Attributes["CurrentSortField"], GvListData.Attributes["CurrentSortDirection"], strWhereClause))
        {
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)GvListData, dt, "", "");
                totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";
            }
            else
            {
                GvListData.DataSource = null;
                GvListData.DataBind();
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
            }
            PageControlCommon.PopulatePager(rptPager, totalRows, currentPageIndex);
        }
    }
    protected void Page_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        hdnGvOppoHeaderCurrentPageIndex.Value = pageIndex.ToString();
        if (ddlInqData.SelectedItem.ToString() == "Header")
        {
            fillGrid(pageIndex);
        }
        else
        {
            fillGridDetail(pageIndex);
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        ddlLocation.SelectedValue = Session["LocId"].ToString();
        FillDepartment();
        fillUser();
        ddlInqData.SelectedIndex = 0;
        ddlFieldName.SelectedIndex = 0;
        ddlFieldName.Visible = true;
        txtValue.Text = "";
        txtnumeric.Text = "";
        fillGrid();
        txtValue.Visible = true;
        txtnumeric.Visible = false;
        ddlAccesstype.Visible = false;
        ddlFieldName.Visible = true;
        ddlDetailFieldName.Visible = false;
        DivHeader.Visible = true;
        DivDetail.Visible = false;
        div_productCat.Attributes["style"] = "display:none";
        ddlDetailFieldName.SelectedIndex = 0;
        gvDetailData.Columns[7].Visible = true;
        gvDetailData.Columns[8].Visible = true;
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        gvDetailData.Columns[7].Visible = true;
        gvDetailData.Columns[8].Visible = true;
        if (ddlInqData.SelectedValue == "Detail")
        {
            fillGridDetail(1);
        }
        else
        {
            fillGrid(1);
        }
        if (txtValue.Text != "")
            txtValue.Focus();
    }
    public string getdate(string data)
    {
        string custID = data.Trim().Split('/')[0].ToString();
        string productId = data.Trim().Split('/')[1].ToString();
        int count = 0;
        if (custID.Trim() != "" && productId.Trim() != "")
        {
            count++;
        }
        return count.ToString();
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
        DataTable dtres = (DataTable)ViewState["MessageDt"];
        if (dtres.Rows.Count != 0)
        {
            ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        dtres = null;
        return ArebicMessage;
    }
    protected void GvListData_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortField = e.SortExpression;
        SortDirection sortDirection = e.SortDirection;
        if (GvListData.Attributes["CurrentSortField"] != null &&
            GvListData.Attributes["CurrentSortDirection"] != null)
        {
            if (sortField == GvListData.Attributes["CurrentSortField"])
            {
                if (GvListData.Attributes["CurrentSortDirection"] == "ASC")
                {
                    sortDirection = SortDirection.Descending;
                }
                else
                {
                    sortDirection = SortDirection.Ascending;
                }
            }
        }
        GvListData.Attributes["CurrentSortField"] = sortField;
        GvListData.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        fillGrid(Int32.Parse(hdnGvOppoHeaderCurrentPageIndex.Value));
    }
    protected void btnGenerateQuote_Command(object sender, CommandEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "RedirectQuotation('" + e.CommandName.ToString() + "');", true);
    }
    protected void btnGenerateFollowup_Command(object sender, CommandEventArgs e)
    {
        Session["Oppo_SInquiryID"] = "0";
        Session["Oppo_SInquiryID"] = e.CommandName.ToString();
        FollowupUC.setLocationId(e.CommandArgument.ToString());
        FollowupUC.newBtnCall();
        //FollowupUC.fillFollowupListSession(hdnFollowupTableName.Value);
        //FollowupUC.fillFollowupList(hdnFollowupTableName.Value);
        //FollowupUC.fillFollowupBinSession(hdnFollowupTableName.Value);
        //FollowupUC.fillFollowupBin(hdnFollowupTableName.Value);
        FollowupUC.GetFollowupDocumentNumber();
        FollowupUC.SetGeneratedByName();
        FollowupUC.ResetFollowupType();
        using (DataTable dt = followup.getOppoHeaderDtlByID(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandName.ToString()))
            FollowupUC.fillHeader(dt, hdnFollowupTableName.Value);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Address_Open();showNewTab();", true);
    }
    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I4.Attributes.Add("Class", "fa fa-minus");
        Div4.Attributes.Add("Class", "box box-primary");
        ddlFieldName.Visible = true;
        ddlDetailFieldName.Visible = false;
        visbleTxtBox(ddlFieldName.SelectedValue);
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        try
        {
            EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
            using (DataTable dtCon = ObjEmployeeMaster.GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), prefixText.ToString()))
            {
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
        }
        catch (Exception error)
        {
        }
        return null;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetContactListCustomer(string prefixText, int count, string contextKey)
    {
        try
        {
            Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
            string id = string.Empty;
            id = WebUserControl_Followup.getCustid();
            using (DataTable dt_Contact = ObjContactMaster.GetContactAsPerFilterText(prefixText, id))
            {
                string[] filterlist = new string[dt_Contact.Rows.Count];
                if (dt_Contact.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_Contact.Rows.Count; i++)
                    {
                        filterlist[i] = dt_Contact.Rows[i]["Filtertext"].ToString();
                    }
                    return filterlist;
                }
                else
                {
                    filterlist[0] = ObjContactMaster.GetContactNameByContactiD(id) + "/" + id;
                    return filterlist;
                }
            }
                
        }
        catch (Exception error)
        {
        }
        return null;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCallLogs(string prefixText, int count, string contextKey)
    {
        CallLogs call = new CallLogs(HttpContext.Current.Session["DBConnection"].ToString());
        string id = WebUserControl_Followup.getCustid();
        using (DataTable dt1 = call.GetCallLogsFor_CustomerID(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), id, prefixText))
        {
            string[] txt = new string[dt1.Rows.Count];
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    txt[i] = dt1.Rows[i]["filteredText"].ToString();
                }
            }
            return txt;
        }
            
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListVisitLogs(string prefixText, int count, string contextKey)
    {
        SM_WorkOrder visit = new SM_WorkOrder(HttpContext.Current.Session["DBConnection"].ToString());
        string id = WebUserControl_Followup.getCustid();
        using (DataTable dt1 = visit.GetWorkOrderNoPreText(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), id,prefixText))
        {
            string[] txt = new string[dt1.Rows.Count];
            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    txt[i] = dt1.Rows[i]["Work_Order_No"].ToString() + "/" + dt1.Rows[i]["Trans_Id"].ToString();
                }
            }
            return txt;
        }            
    }
    protected void lblSalesStage_Command(object sender, CommandEventArgs e)
    {
        resetFields();
        string trans_id = e.CommandArgument.ToString();
        using (DataTable oppodata = objOppoDashboard.getOppDataForHeaderByTransId(trans_id))
        {
            // filling header data
            OppoName.Text = oppodata.Rows[0]["Opportunity_name"].ToString();
            OppAmt.Text = SetDecimal(oppodata.Rows[0]["Opportunity_amount"].ToString(), oppodata.Rows[0]["field1"].ToString()) + "" + oppodata.Rows[0]["oppoCurrency_Code"].ToString();
            GeneratedBy.Text = oppodata.Rows[0]["emp_name"].ToString();
            GeneratedOn.Text = GetDate(oppodata.Rows[0]["CreatedDate"].ToString());
            lblCustomerName.Text = oppodata.Rows[0]["CustomerName"].ToString();
            tbFollowup.Text = oppodata.Rows[0]["TotalFollowup"].ToString();
            tbCall.Text = oppodata.Rows[0]["TotalCalls"].ToString();
            tbVisit.Text = oppodata.Rows[0]["TotalVisit"].ToString();
        }
        //filling product history
        using (DataTable oppodata_forBusiness = objOppoDashboard.getAllOpportunityDataByTransId_forBusiness(trans_id))
        {
            gvProduct.DataSource = oppodata_forBusiness;
            gvProduct.DataBind();
            string quote_ids = "", order_ids = "";
            //filling quotation data
            using (DataTable quoteDate = objOppoDashboard.getQuoteDateByOppoTransId(trans_id))
            {
                gvquoteData.DataSource = quoteDate;
                gvquoteData.DataBind();
                if (gvquoteData.Rows.Count > 0)
                {
                    div_quote.Attributes.Add("style", "display:block;");
                    object sumObject;
                    sumObject = quoteDate.Compute("Sum(amount)", "");
                    lblGeneratedQuote.Text = "Total Quotation :" + quoteDate.Rows.Count + "   (" + SetDecimal(sumObject.ToString(), quoteDate.Rows[0]["field1"].ToString()) + " " + quoteDate.Rows[0]["quoteCurrency_Code"].ToString() + ")";
                    int open = 0, close = 0, lost = 0;
                    foreach (DataRow dr in quoteDate.Rows)
                    {
                        quote_ids = quote_ids + dr["SQuotation_Id"].ToString() + ",";
                        if (dr["status"].ToString().Trim() == "Open")
                        {
                            open++;
                        }
                        else if (dr["status"].ToString().Trim() == "Close")
                        {
                            close++;
                        }
                        else if (dr["status"].ToString().Trim() == "Lost")
                        {
                            lost++;
                        }
                    }
                    lblOpenTotal.Text = "Open Quotes: " + open;
                    lblClosedTotal.Text = "Close Quotes: " + close;
                    lblLostTotal.Text = "Lost Quotes: " + lost;
                    int Last_pos = quote_ids.LastIndexOf(",");
                    quote_ids = quote_ids.Substring(0, Last_pos - 0);
                    //filling order data
                    using (DataTable OrderDate = objOppoDashboard.getOrderDataByQuoteTransId(quote_ids))
                    {
                        gvOrderDate.DataSource = OrderDate;
                        gvOrderDate.DataBind();
                        if (OrderDate.Rows.Count > 0)
                        {
                            div_order.Attributes.Add("style", "display:block;");
                            using (DataTable dtdistinctOrderCount = OrderDate.DefaultView.ToTable("order_trans_id", true, "order_trans_id"))
                            {
                                int countOrder = dtdistinctOrderCount.Rows.Count;
                                sumObject = OrderDate.Compute("Sum(NetAmount)", "");
                                lblGeneratedOrder.Text = "Total Sales Order :" + countOrder + "     (" + SetDecimal(sumObject.ToString(), OrderDate.Rows[0]["field1"].ToString()) + " " + OrderDate.Rows[0]["orderCurrency_Code"].ToString() + ")";
                            }
                            foreach (DataRow dr1 in OrderDate.Rows)
                            {
                                order_ids = order_ids + dr1["order_trans_id"].ToString() + ",";
                            }
                            Last_pos = 0;
                            Last_pos = order_ids.LastIndexOf(",");
                            order_ids = order_ids.Substring(0, Last_pos - 0);
                            //filling invoice data
                            using (DataTable InvoiceDate = objOppoDashboard.getInvoiceDataByOrderTransId(order_ids))
                            {
                                gvInvoiceDate.DataSource = InvoiceDate;
                                gvInvoiceDate.DataBind();
                                if (InvoiceDate.Rows.Count > 0)
                                {
                                    div_invoice.Attributes.Add("style", "display:block;");
                                    using (DataTable dtdistinctInvoiceCount = InvoiceDate.DefaultView.ToTable("invoice_no", true, "invoice_no"))
                                    {
                                        int count = dtdistinctInvoiceCount.Rows.Count;
                                        sumObject = InvoiceDate.Compute("Sum(GrandTotal)", "");
                                        lblGeneratedInvoice.Text = "Total Invoice :" + count + "    (" + SetDecimal(sumObject.ToString(), InvoiceDate.Rows[0]["field1"].ToString()) + " " + InvoiceDate.Rows[0]["invoiceCurrency_code"].ToString() + ")";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_DetailedDate_Open()", true);
    }
    public void resetFields()
    {
        gvProduct.DataSource = null;
        gvProduct.DataBind();
        gvInvoiceDate.DataSource = null;
        gvInvoiceDate.DataBind();
        gvOrderDate.DataSource = null;
        gvOrderDate.DataBind();
        gvquoteData.DataSource = null;
        gvquoteData.DataBind();
        lblGeneratedInvoice.Text = "";
        lblGeneratedOrder.Text = "";
        lblGeneratedQuote.Text = "";
        div_quote.Attributes.Add("style", "display:none;");
        div_order.Attributes.Add("style", "display:none;");
        div_invoice.Attributes.Add("style", "display:none;");
    }
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        I4.Attributes.Add("Class", "fa fa-minus");
        Div4.Attributes.Add("Class", "box box-primary");
        FillDepartment();
        fillUser();
        ddlInqData_SelectedIndexChanged(sender, e);
    }
    //public void fillLocation()
    //{
    //    location_id = "";
    //    locationCondition = "Location_Id=";
    //    ddlLocation.Items.Clear();
    //    DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
    //    try
    //    {
    //        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
    //    }
    //    catch
    //    {
    //    }
    //    if (!Common.GetStatus())
    //    {
    //        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L");
    //        if (LocIds != "")
    //        {
    //            dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
    //        }
    //    }
    //    if (dtLoc.Rows.Count > 0)
    //    {
    //        for (int i = 0; i < dtLoc.Rows.Count; i++)
    //        {
    //            ddlLocation.Items.Add(new ListItem(dtLoc.Rows[i]["Location_Name"].ToString(), dtLoc.Rows[i]["Location_Id"].ToString()));
    //            if (i == dtLoc.Rows.Count - 1)
    //            {
    //                location_id = location_id + dtLoc.Rows[i]["Location_Id"].ToString();
    //                locationCondition = locationCondition + dtLoc.Rows[i]["Location_Id"].ToString();
    //            }
    //            else
    //            {
    //                location_id = location_id + dtLoc.Rows[i]["Location_Id"].ToString() + ",";
    //                locationCondition = locationCondition + dtLoc.Rows[i]["Location_Id"].ToString() + " or Location_Id=";
    //            }
    //        }
    //        ddlLocation.Items.Insert(0, new ListItem("All", location_id));
    //    }
    //    else
    //    {
    //        ddlLocation.Items.Clear();
    //    }
    //    dtLoc = null;
    //}
    //added by divya parakh 3/7/2018
    public void fillLocation()
    {
        locationCondition = "";
        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());

        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        string LocIds = "";
        if (!Common.GetStatus(Session["EmpId"].ToString()))
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
            //for (int i = 0; i < dtLoc.Rows.Count; i++)
            //{
            //ddlLocation.Items.Add(new ListItem(dtLoc.Rows[i]["Location_Name"].ToString(), dtLoc.Rows[i]["Location_Id"].ToString()));
            //}
        }
        else
        {
            ddlLocation.Items.Clear();
        }
        //cmn.FillUser(Session["CompId"].ToString(), Session["UserId"].ToString(), ddlUser, objObjectEntry.GetModuleIdAndName("54").Rows[0]["Module_Id"].ToString(), "54", ddlLocation.SelectedValue, locationCondition);
        dtLoc = null;
    }
    public string SetDecimal(string amount, string strCurrencyId)
    {
        if (amount == "")
        {
            return "";
        }
        return SystemParameter.GetAmountWithDecimal(amount, strCurrencyId);
    }
    protected void ddlInqData_SelectedIndexChanged(object sender, EventArgs e)
    {
        I4.Attributes.Add("Class", "fa fa-minus");
        Div4.Attributes.Add("Class", "box box-primary");
        ddlUser_SelectedIndexChanged(sender, e);
        div_productCat.Attributes["style"] = "display:none";
        if (ddlInqData.SelectedValue == "Header")
        {
            ddlFieldName.Visible = true;
            ddlDetailFieldName.Visible = false;
            //exportEXCEL.Visible = true;
            //exportPDF.Visible = true;
        }
        else
        {
            ddlFieldName.Visible = false;
            ddlDetailFieldName.Visible = true;
            div_productCat.Attributes["style"] = "display:block";
            //exportEXCEL.Visible = false;
            //exportPDF.Visible = false;
        }
    }
    private void fillGridDetail(int currentPageIndex = 1)
    {
        GvListData.Visible = false;
        gvDetailData.Visible = true;
        gvDetailData.Columns[7].Visible = true;
        gvDetailData.Columns[8].Visible = true;
        string strSearchCondition = string.Empty;
        if (ddlOption.SelectedIndex != 0 && txtValue.Text != string.Empty)
        {
            if (ddlFieldName.SelectedItem.Value == "IDate" || ddlFieldName.SelectedItem.Value == "followup_date")
            {
                strSearchCondition = ddlFieldName.SelectedValue + "='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 1)
            {
                strSearchCondition = ddlFieldName.SelectedValue + "='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                strSearchCondition = ddlFieldName.SelectedValue + " like '%" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                strSearchCondition = ddlFieldName.SelectedValue + " Like '%" + txtValue.Text.Trim() + "%'";
            }
        }
        string strWhereClause = string.Empty;
        strWhereClause = "opportunity_name <> '' and CreatedByName <> '' and isActive='true' and Opportunity_amount <> 0 and daysfrequency <> 0 and Company_id='" + Session["CompId"].ToString() + "'";
        if (strSearchCondition != string.Empty)
        {
            strWhereClause = strWhereClause + " and " + strSearchCondition;
        }
        if (ddlUser.SelectedItem.ToString() != "--Select User--")
        {
            strWhereClause = strWhereClause + " and  User_Id='" + ddlUser.SelectedValue + "'";
        }
        if (ddlLocation.SelectedItem.Text != "All")
        {
            strWhereClause = strWhereClause + " and  Location_Id='" + ddlLocation.SelectedValue.ToString() + "'";
        }
        if (ddlProductCat.SelectedItem.Text != "Select")
        {
            strWhereClause = strWhereClause + " and  CategoryId='" + ddlProductCat.SelectedValue.ToString() + "'";
        }
        if (ddlDetailFieldName.SelectedItem.Value == "CreatedDate/1" || ddlDetailFieldName.SelectedItem.Value == "CreatedDate/2")
        {
            gvDetailData.Columns[7].Visible = false;
            gvDetailData.Columns[8].Visible = false;
            if (txtnumeric.Text.Trim() != "")
            {
                if (ddlDetailFieldName.SelectedItem.Value == "CreatedDate/2")
                {
                    strWhereClause = strWhereClause + " and UpComingOppo='yes'";
                }
                if (ddlDetailFieldName.SelectedItem.Value == "CreatedDate/1")
                {
                    strWhereClause = strWhereClause + " and GoneOppo='yes'";
                }
            }
        }
        int totalRows = 0;
        using (DataTable dt = objOppoDashboard.getDetailData((currentPageIndex - 1).ToString(), Session["GridSize"].ToString(), gvDetailData.Attributes["CurrentSortField"], gvDetailData.Attributes["CurrentSortDirection"], strWhereClause, txtnumeric.Text))
        {
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)gvDetailData, dt, "", "");
                totalRows = Int32.Parse(dt.Rows[0]["TotalCount"].ToString());
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows[0]["TotalCount"].ToString() + "";
            }
            else
            {
                gvDetailData.DataSource = null;
                gvDetailData.DataBind();
                lblTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
            }
            PageControlCommon.PopulatePager(rptPager, totalRows, currentPageIndex);
        }
    }
    protected void gvDetailData_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortField = e.SortExpression;
        SortDirection sortDirection = e.SortDirection;
        if (gvDetailData.Attributes["CurrentSortField"] != null &&
            gvDetailData.Attributes["CurrentSortDirection"] != null)
        {
            if (sortField == gvDetailData.Attributes["CurrentSortField"])
            {
                if (gvDetailData.Attributes["CurrentSortDirection"] == "ASC")
                {
                    sortDirection = SortDirection.Descending;
                }
                else
                {
                    sortDirection = SortDirection.Ascending;
                }
            }
        }
        gvDetailData.Attributes["CurrentSortField"] = sortField;
        gvDetailData.Attributes["CurrentSortDirection"] =
                (sortDirection == SortDirection.Ascending ? "ASC" : "DESC");
        fillGridDetail(Int32.Parse(hdnGvOppoHeaderCurrentPageIndex.Value));
    }
    //public void fillUser()
    //{
    //    if (ddlLocation.SelectedIndex == 0)
    //    {
    //        fillLocation();
    //        cmn.FillUser(Session["CompId"].ToString(), "SUPERADMIN", ddlUser, objObjectEntry.GetModuleIdAndName("54").Rows[0]["Module_Id"].ToString(), "54", "0", locationCondition);
    //    }
    //    else
    //    {
    //        locationCondition = "Location_Id=" + ddlLocation.SelectedValue;
    //        cmn.FillUser(Session["CompId"].ToString(), "SUPERADMIN", ddlUser, objObjectEntry.GetModuleIdAndName("54").Rows[0]["Module_Id"].ToString(), "54", ddlLocation.SelectedValue, locationCondition);
    //    }
    //}
    public void fillUser()
    {
        try
        {
            string strEmpId = string.Empty;
            string strLocationDept = string.Empty;
            string strLocId = Session["LocId"].ToString();

            strLocId = ddlLocation.SelectedValue;
            strLocationDept = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (strLocationDept == "")
            {
                strLocationDept = "0,";
            }
            strEmpId += getEmpList(Session["BrandId"].ToString(), strLocId, strLocationDept);


            DataTable dtEmp = new DataTable();

            string isSingle = objUser.CheckIsSingleUser(Session["UserId"].ToString(), Session["CompId"].ToString());
            bool IsSingleUser = false;
            try
            {
                IsSingleUser = Convert.ToBoolean(isSingle);
            }
            catch
            {
                IsSingleUser = false;
            }

            // can see multiple employee data
            if (IsSingleUser == false)
            {
                //for normal user
                if (Session["EmpId"].ToString() != "0")
                {
                    dtEmp = ObjEmployeeMaster.GetEmployeeMasterOnRole(Session["CompId"].ToString(), strEmpId);
                    //dtEmp = new DataView(dtEmp, "Emp_Id in (" + strEmpId.Substring(0, strEmpId.ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
                else
                {
                    //for super admin
                    if (ddlLocation.SelectedIndex > 0)
                    {
                        dtEmp = ObjEmployeeMaster.GetEmployeeMasterOnRole(Session["CompId"].ToString());
                        dtEmp = new DataView(dtEmp, "Location_Id=" + ddlLocation.SelectedValue.Trim() + "", "emp_name asc", DataViewRowState.CurrentRows).ToTable();
                    }
                }
            }
            else
            {
                dtEmp = ObjEmployeeMaster.GetEmployeeMasterOnRole(Session["CompId"].ToString(), Session["EmpId"].ToString());
                //dtEmp = new DataView(dtEmp, "Emp_Id='" + Session["EmpId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            ddlUser.DataSource = dtEmp;
            ddlUser.DataTextField = "Emp_name";
            ddlUser.DataValueField = "user_id";
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, new ListItem("--Select User--", "--Select User--"));
        }
        catch
        {

        }
    }
    public void FillEmployee(string strDeptId)
    {
        string strEmpId = string.Empty;
        string strLocationDept = string.Empty;
        string strLocId = ddlLocation.SelectedValue;
        if (ddlLocation.SelectedIndex > 0)
        {
            strLocationDept = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (strLocationDept == "")
            {
                strLocationDept = "0,";
            }
            strEmpId += getEmpList(Session["BrandId"].ToString(), strLocId, strLocationDept) + ",";
        }
        else
        {
            foreach (ListItem li in ddlLocation.Items)
            {
                if (li.Value.Length >= 3)
                {
                    continue;
                }
                strLocId = li.Value;
                strLocationDept = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
                if (strLocationDept == "")
                {
                    strLocationDept = "0,";
                }
                strEmpId += getEmpList(Session["BrandId"].ToString(), strLocId, strLocationDept) + ",";
            }
        }
        DataTable dtEmp = ObjEmployeeMaster.GetEmployeeMasterOnRole(Session["CompId"].ToString());
        // Nitin Jasin , Get According to UserId to Get Records for Single User 
        DataTable dt = objUser.GetUserMasterByUserId(Session["UserId"].ToString(), "");
        bool IsSingleUser = false;
        try
        {
            IsSingleUser = Convert.ToBoolean(dt.Rows[0]["Field6"].ToString());
        }
        catch
        {
            IsSingleUser = false;
        }
        if (IsSingleUser == false)
        {
            dtEmp = new DataView(dtEmp, "Emp_Id in (" + strEmpId.Substring(0, strEmpId.ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            if (strDeptId != "" && strDeptId != "0")
            {
                dtEmp = new DataView(dtEmp, "Department_Id in (" + strDeptId + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            ddlUser.DataSource = dtEmp;
            ddlUser.DataTextField = "Emp_Name";
            ddlUser.DataValueField = "Emp_Id";
            ddlUser.DataBind();
        }
        else
        {
            dt = new DataView(dtEmp, "Emp_Id='" + dt.Rows[0]["Emp_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            ddlUser.DataSource = dtEmp;
            ddlUser.DataTextField = "Emp_Name";
            ddlUser.DataValueField = "Emp_Id";
        }
        ddlUser.Items.Insert(0, new ListItem("--Select User--", "0"));
        dt = null;
        dtEmp = null;
    }
    public string getEmpList(string strBrandId, string strLocationId, string strdepartmentId)
    {
        string strEmpList = "0";
        using (DataTable dtEmp = ObjDa.return_DataTable(" SELECt STUFF((SELECT DISTINCT ',' + RTRIM(Set_EmployeeMaster.Emp_Id) FROM Set_EmployeeMaster where Brand_Id=" + strBrandId + " and  location_id =" + strLocationId + " and Department_Id in (" + strdepartmentId.Substring(0, strdepartmentId.Length - 1) + ") and IsActive='True' FOR xml PATH ('')), 1, 1, '')"))
        {
            if (dtEmp.Rows[0][0] != null)
            {
                strEmpList = dtEmp.Rows[0][0].ToString();
            }
        }
            
        if (strEmpList == "")
        {
            strEmpList = "0";
        }
        return strEmpList;
    }
    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlInqData.SelectedValue == "Header")
        {
            fillGrid();
        }
        if (ddlInqData.SelectedValue == "Detail")
        {
            fillGridDetail();
        }
    }
    public void visbleTxtBox(string name)
    {
        txtnumeric.Visible = false;
        txtValue.Visible = true;
        ddlAccesstype.Visible = false;
        if (name == "CreatedDate/1" || name == "CreatedDate/3" || name == "CreatedDate/2")
        {
            txtnumeric.Visible = true;
            txtValue.Visible = false;
            ddlAccesstype.Visible = false;
        }
        if (name == "Field4")
        {
            txtnumeric.Visible = false;
            txtValue.Visible = false;
            ddlAccesstype.Visible = true;
        }
    }
    protected void ddlDetailFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I4.Attributes.Add("Class", "fa fa-minus");
        Div4.Attributes.Add("Class", "box box-primary");
        ddlFieldName.Visible = false;
        ddlDetailFieldName.Visible = true;
        visbleTxtBox(ddlDetailFieldName.SelectedValue);
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContactNumber(string prefixText, int count, string contextKey)
    {
        ContactNoMaster objContactNumMaster = new ContactNoMaster(HttpContext.Current.Session["DBConnection"].ToString());
        using (DataTable dt = objContactNumMaster.getNumberList_PreText(prefixText))
        {
            string[] txt = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["Phone_no"].ToString();
            }
            return txt;
        }            
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmailMaster(string prefixText, int count, string contextKey)
    {
        ES_EmailMaster_Header Email = new ES_EmailMaster_Header(HttpContext.Current.Session["DBConnection"].ToString());
        using (DataTable dt = Email.GetEmailIdPreFixText(prefixText))
        {
            string[] str = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Email_Id"].ToString();
            }
            return str;
        }            
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDepartmentMaster(string prefixText, int count, string contextKey)
    {
        using (DataTable dt = new DepartmentMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetDepartmentListPreText(prefixText))
        {
            string[] str = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["Dep_Name"].ToString() + "/" + dt.Rows[i]["Dep_Id"].ToString();
            }
            return str;
        }            
    }
    protected void ddlProductCat_SelectedIndexChanged(object sender, EventArgs e)
    {
        I4.Attributes.Add("Class", "fa fa-minus");
        Div4.Attributes.Add("Class", "box box-primary");
        fillGridDetail();
    }
    protected void exportPDF_Click(object sender, EventArgs e)
    {
        ExportXtraReport(".pdf");
    }
    protected void exportEXCEL_Click(object sender, EventArgs e)
    {
        ExportXtraReport(".xls");
    }
    private void ExportXtraReport(string extension)
    {
        XtraReport RptShift = new XtraReport();
        RptShift.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "Oppo_summary.repx");
        AttendanceDataSet rptdata = new AttendanceDataSet();
        rptdata.EnforceConstraints = false;
        AttendanceDataSetTableAdapters.sp_OpportunitySummaryReportTableAdapter adp = new AttendanceDataSetTableAdapters.sp_OpportunitySummaryReportTableAdapter();
        adp.Fill(rptdata.sp_OpportunitySummaryReport, ddlLocation.SelectedValue.ToString(), 1);
        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        string BrandName = "";
        CompanyName = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "1");
        // Image Url
        Imageurl = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "2");
        // Get Brand Name
        BrandName = objAttendance.GetBrandName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["Lang"].ToString());
        using (DataTable DtAddress = ObjAddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString()))
        {
            if (DtAddress.Rows.Count > 0)
            {
                CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
            }
        }
            
        
        XRPictureBox xrPictureBox1 = (XRPictureBox)RptShift.FindControl("xrPictureBox1", true);
        try
        {
            xrPictureBox1.ImageUrl = Imageurl;
        }
        catch
        {
        }
        XRLabel xrTitle = (XRLabel)RptShift.FindControl("xrTitle", true);
        //xrTitle.Text = getReportName()+ " From Date : "+txtFromDate.Text  +" To Date : "+ txtToDate.Text ;
        xrTitle.Text = "Opportunity Summary Report";
        XRLabel xrCompName = (XRLabel)RptShift.FindControl("xrCompName", true);
        xrCompName.Text = CompanyName;
        XRLabel xrCompAddress = (XRLabel)RptShift.FindControl("xrCompAddress", true);
        xrCompAddress.Text = CompanyAddress;
        XRLabel rptBrand = (XRLabel)RptShift.FindControl("rptBrand", true);
        rptBrand.Text = BrandName;
        try
        {
            using (DataTable empDt = new EmployeeMaster(Session["DBConnection"].ToString()).GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString()))
            {
                XRLabel xrCreatedBy = (XRLabel)RptShift.FindControl("xrCreatedBy", true);
                xrCreatedBy.Text = empDt.Rows[0]["Emp_Name"].ToString();
            }                
        }
        catch
        {
        }
        RptShift.DataSource = rptdata.sp_OpportunitySummaryReport;
        RptShift.DataMember = "sp_OpportunitySummaryReport";
        RptShift.CreateDocument(true);
        string dateT = System.DateTime.Now.ToString("hhmmyyss");
        string rptname = "Opportunity Summary Report" + dateT;
        if (extension == ".pdf")
        {
            RptShift.ExportToPdf(@"" + ConfigurationManager.AppSettings["ReportPath"].ToString() + rptname + ".pdf");
            rptname = rptname + ".pdf";
            Response.ContentType = "application/pdf";
        }
        if (extension == ".xls")
        {
            RptShift.ExportToXls(@"" + ConfigurationManager.AppSettings["ReportPath"].ToString() + rptname + ".xls");
            rptname = rptname + ".xls";
            Response.ContentType = "application/vnd.ms-excel";
        }
        DisplayMessage("Exported Successfully at " + ConfigurationManager.AppSettings["ReportPath"].ToString() + rptname);
        //Response.Redirect(ConfigurationManager.AppSettings["ReportPath"].ToString() + rptname);
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + rptname);
        Response.TransmitFile(ConfigurationManager.AppSettings["ReportPath"].ToString() + rptname);
        Response.End();
    }
    protected void lblOpportunityName_Command(object sender, CommandEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "RedirectOpportunity('" + e.CommandArgument.ToString() + "');", true);
    }
    protected void gvQuotationNo_Command(object sender, CommandEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "quoteRedirect('" + e.CommandArgument.ToString() + "');", true);
    }
    protected void Btn_FillGrid_Click(object sender, EventArgs e)
    {
        if (ddlInqData.SelectedIndex == 0)
        {
            fillGrid();
        }
        else
        {
            fillGridDetail();
        }
    }
    public void FillDepartment()
    {
        ddlDepartment.Items.Clear();
        DataTable dt = ObjEmployeeMaster.GetEmployeeOrDepartment("0", "0", "0", "0", "0");
        if (ddlLocation.SelectedIndex != 0)
        {
            dt = new DataView(dt, "Location_Id = '" + ddlLocation.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            dt = dt.DefaultView.ToTable(false, "Dep_Id", "DeptName");
            DataView view = new DataView(dt);
            dt = view.ToTable(true, "Dep_Id", "DeptName");
        }
        string DepIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D",Session["LocId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        if (!Common.GetStatus(Session["EmpId"].ToString()))
        {
            if (DepIds != "")
            {
                dt = new DataView(dt, "Dep_Id in(" + DepIds.Substring(0, DepIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        if (dt.Rows.Count > 0)
        {
            ddlDepartment.Items.Add(new System.Web.UI.WebControls.ListItem("All", "0"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddlDepartment.Items.Add(new System.Web.UI.WebControls.ListItem(dt.Rows[i]["DeptName"].ToString(), dt.Rows[i]["Dep_Id"].ToString()));
            }
        }
        dt = null;
    }
    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        I4.Attributes.Add("Class", "fa fa-minus");
        Div4.Attributes.Add("Class", "box box-primary");
        if (ddlDepartment.SelectedIndex == 0)
        {
            fillUser();
        }
        else
        {
            FillEmployee(ddlDepartment.SelectedValue);
        }
    }

    
    protected void Exportdata()
    {
        DataTable DtAllData = new DataTable();
        DataTable dtAdd = new DataTable();

        try
        {


            if (ddlInqData.SelectedValue == "Header")
            {
                dtAdd = objOppoDashboard.GetAllGridData(Session["CompId"].ToString(), ddlLocation.SelectedValue.ToString());
            }
            else
            {
                dtAdd = objOppoDashboard.getAllGridDetailed(Session["CompId"].ToString(), ddlLocation.SelectedValue.ToString(), txtnumeric.Text, ddlProductCat.SelectedValue);
            }

            string condition = string.Empty;
            if (ddlFieldName.SelectedValue == "Field4")
            {
                condition = "Field4='" + ddlAccesstype.SelectedItem + "'";
                DataView view1 = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
                dtAdd = view1.ToTable();

                return;
            }


            if (ddlInqData.SelectedValue == "Detail")
            {
                if (ddlDetailFieldName.SelectedItem.Value == "CreatedDate/1" || ddlDetailFieldName.SelectedItem.Value == "CreatedDate/2")
                {
                    if (txtnumeric.Text.Trim() != "")
                    {
                        if (ddlDetailFieldName.SelectedItem.Value == "CreatedDate/1" || ddlDetailFieldName.SelectedItem.Value == "CreatedDate/2")
                        {
                            //gone events
                            int parsedValue;
                            if (int.TryParse(txtnumeric.Text, out parsedValue))
                            {
                                condition = "ExpectedBusiness<>0";
                            }
                        }
                    }
                }
                else
                {
                    if (txtValue.Text.Trim() != "")
                    {
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
                    }
                }

                DataView view1 = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
                dtAdd = view1.ToTable();
            }
            else
            {
                if (txtValue.Text.Trim() != "")
                {
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
                }
                DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
                dtAdd = view.ToTable();
            }

            if (ddlInqData.SelectedValue == "Detail")
            {
                dtAdd.Columns["Opportunity_name"].SetOrdinal(0);
                dtAdd.Columns["CustomerName"].SetOrdinal(1);
                dtAdd.Columns["SuggestedProductName"].SetOrdinal(2);
                dtAdd.Columns["EstimatedUnitPrice"].SetOrdinal(3);
                dtAdd.Columns["Quantity"].SetOrdinal(4);
                dtAdd.Columns["DaysFrequency"].SetOrdinal(5);
                dtAdd.Columns["UpComingOppoDays"].SetOrdinal(6);
                dtAdd.Columns["GoneOppoDays"].SetOrdinal(7);
                dtAdd.Columns["followup_date"].SetOrdinal(8);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                dtAdd.Columns.RemoveAt(9);
                ExportTableData(dtAdd, "OpportunityDashboardDetail");
            }
            else
            {
               
                dtAdd.Columns["Opportunity_name"].SetOrdinal(0);
                dtAdd.Columns["IDate"].SetOrdinal(1);
                dtAdd.Columns["CustomerName"].SetOrdinal(2);
                dtAdd.Columns["sales_stage"].SetOrdinal(3);
                dtAdd.Columns["Opportunity_amount"].SetOrdinal(4);
                dtAdd.Columns["HandledByName"].SetOrdinal(5);
                dtAdd.Columns["followup_date"].SetOrdinal(6);
                dtAdd.Columns.RemoveAt(7);
                dtAdd.Columns.RemoveAt(7);
                dtAdd.Columns.RemoveAt(7);
                ExportTableData(dtAdd, "OpportunityDashboardHeader");
            } 
        }
        catch
        {
        }
    }

    public void ExportTableData(DataTable dtdata,string Name)
    {
        string strFname = Name;
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dtdata, strFname);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + strFname + ".xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }


    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            Exportdata();
        }
         catch
        {

        }
    }
}