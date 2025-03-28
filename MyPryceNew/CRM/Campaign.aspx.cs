using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using PegasusDataAccess;

public partial class CRM_Campaign : System.Web.UI.Page
{
    Common cmn = null;
    Campaign Camp = null;
    LocationMaster ObjLocationMaster = null;
    Inv_ProductMaster objProduct = null;
    CurrencyMaster objCurrency = null;
    SystemParameter objSys = null;
    Ems_ContactMaster ObjContactMaster = null;
    DataAccessClass objDa = null;
    HR_EmployeeDetail objempDetails = null;
    PageControlCommon objPageCmn = null;

    string condition = string.Empty;
    static string LocationCondition = "";
    public static string strCurrencyId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        Camp = new Campaign(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objProduct = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objCurrency = new CurrencyMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objempDetails = new HR_EmployeeDetail(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
          
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../CRM/Campaign.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);


            Gv_Product_Add.DataSource = null;
            Gv_Product_Add.DataBind();
            Gv_Contact_Add.DataSource = null;
            Gv_Contact_Add.DataBind();
            Txt_Start_Date.Attributes.Add("readonly", "readonly");
            Txt_End_Date.Attributes.Add("readonly", "readonly");
            txtbinValueDate.Attributes.Add("readonly", "readonly");

            imgBtnRestore.Visible = false;
            ImgbtnSelectAll.Visible = false;
            //CalendarExtender2.SelectedDate = DateTime.Now;
            fillLocation();
            ddlLocation.SelectedValue = Session["LocId"].ToString();

            //used when we click on reminder
            if (Request.QueryString["SearchField"] != null)
            {
                ddlFieldName.SelectedIndex = 0;
                ddlOption.SelectedIndex = 2;
                txtValue.Text = Request.QueryString["SearchField"].ToString();
                btnbind_Click(null, null);
            }
            else
            {
                fillgrid();
            }

            FillCurrency();

            try
            {
                ddlCurrency.SelectedValue = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), ddlLocation.SelectedValue).Rows[0]["Field1"].ToString();
                strCurrencyId = ddlCurrency.SelectedValue;
            }
            catch (Exception ex)
            {
                DisplayMessage(ex.Message.ToString());
            }
            txtValue.Focus();
        }


    }
    private void FillCurrency()
    {
        DataTable dsCurrency = null;
        dsCurrency = objCurrency.GetCurrencyMaster();
        if (dsCurrency.Rows.Count > 0)
        {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)ddlCurrency, dsCurrency, "Currency_Name", "Currency_ID");
        }
        else
        {
            ddlCurrency.Items.Add("--Select--");
            ddlCurrency.SelectedValue = "--Select--";
        }
    }

    public string GetDate(object obj)
    {
        DateTime Date = new DateTime();
        if (obj.ToString() != "")
        {
            Date = Convert.ToDateTime(obj.ToString());
            return Date.ToString(objSys.SetDateFormat());
        }
        return "";
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {

        Btn_Save.Visible = clsPagePermission.bAdd;

        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanAdd.Value = clsPagePermission.bAdd.ToString().ToLower();
        hdnCanUpload.Value = clsPagePermission.bUpload.ToString().ToLower();

        imgBtnRestore.Visible = clsPagePermission.bRestore;

        //IT_ObjectEntry objObjectEntry = new IT_ObjectEntry();


        ////New Code 
        //string strModuleId = string.Empty;
        //string strModuleName = string.Empty;

        //DataTable dtModule = objObjectEntry.GetModuleIdAndName("397");
        //if (dtModule.Rows.Count > 0)
        //{
        //    strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
        //    strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        //}
        //else
        //{
        //    Session.Abandon();
        //    Response.Redirect("~/ERPLogin.aspx");
        //}
        ////End Code

        //Page.Title = objSys.GetSysTitle();

        //Session["AccordianId"] = strModuleId;
        //Session["HeaderText"] = strModuleName;

        //if (Session["EmpId"].ToString() == "0")
        //{
        //    Btn_Save.Visible = true;

        //    try
        //    {
        //        GV_Campaign_List.Columns[0].Visible = true;

        //        GV_Campaign_List.Columns[1].Visible = true;

        //        GV_Campaign_List.Columns[2].Visible = true;

        //        GV_Campaign_List.Columns[3].Visible = true;
        //    }
        //    catch
        //    {

        //    }

        //    imgBtnRestore.Visible = true;
        //    //ImgbtnSelectAll.Visible = false;
        //}
        //DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "397");
        //if (dtAllPageCode.Rows.Count != 0)
        //{
        //    if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
        //    {

        //    }
        //    else
        //    {
        //        foreach (DataRow DtRow in dtAllPageCode.Rows)
        //        {
        //            if (DtRow["Op_Id"].ToString() == "1")
        //            {
        //                Btn_Save.Visible = true;
        //                try
        //                {
        //                    GV_Campaign_List.Columns[3].Visible = true;
        //                }
        //                catch
        //                {

        //                }
        //            }

        //            if (DtRow["Op_Id"].ToString() == "2")
        //            {
        //                try
        //                {
        //                    GV_Campaign_List.Columns[0].Visible = true;
        //                }
        //                catch
        //                {

        //                }
        //            }
        //            if (DtRow["Op_Id"].ToString() == "3")
        //            {
        //                try
        //                {
        //                    GV_Campaign_List.Columns[1].Visible = true;
        //                }
        //                catch
        //                {

        //                }
        //            }
        //            if (DtRow["Op_Id"].ToString() == "8")
        //            {
        //                try
        //                {
        //                    GV_Campaign_List.Columns[2].Visible = true;
        //                }
        //                catch
        //                {

        //                }
        //            }
        //            if (DtRow["Op_Id"].ToString() == "4")
        //            {
        //                imgBtnRestore.Visible = true;
        //                ImgbtnSelectAll.Visible = false;
        //            }
        //        }
        //    }
        //}
    }
    public void fillgridSession()
    {
        DataTable DT_Camp_List = new DataTable();
        DT_Camp_List = Camp.GetAllCampaignActiveData(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue);

        if (ddlLocation.SelectedItem.Text == "All")
        {
            DT_Camp_List = new DataView(DT_Camp_List, LocationCondition, "", DataViewRowState.CurrentRows).ToTable();
        }

        Session["Camp_List"] = DT_Camp_List;
    }
    public void fillgrid()
    {
        fillgridSession();
        DataTable DT_Camp_List = Session["Camp_List"] as DataTable;
        GV_Campaign_List.DataSource = DT_Camp_List;
        GV_Campaign_List.DataBind();
        lblTotalRecords.Text = "Total Records: " + DT_Camp_List.Rows.Count.ToString();
        ////AllPageCode();
    }

    public void fillgridbinSession()
    {
        DataTable Dt_Camp_Bin = new DataTable();
        Dt_Camp_Bin = Camp.GetAllCampaignInActiveData(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue);
        if (ddlLocation.SelectedItem.Text == "All")
        {
            Dt_Camp_Bin = new DataView(Dt_Camp_Bin, LocationCondition, "", DataViewRowState.CurrentRows).ToTable();
        }

        Session["dtBinFilter"] = Dt_Camp_Bin;
    }
    public void fillgridbin()
    {
        fillgridbinSession();
        DataTable Dt_Camp_Bin = Session["dtBinFilter"] as DataTable;
        Gv_Camapign_Bin.DataSource = Dt_Camp_Bin;
        Gv_Camapign_Bin.DataBind();
        //Session["dtBinFilter"] = Dt_Camp_Bin;
        lblbinTotalRecords.Text = "Total Records: " + Dt_Camp_Bin.Rows.Count.ToString();

        if (Gv_Camapign_Bin.Rows.Count != 0)
        {
            imgBtnRestore.Visible = true;
            ImgbtnSelectAll.Visible = false;
        }
        else
        {
            imgBtnRestore.Visible = false;
            ImgbtnSelectAll.Visible = false;
        }

        ////AllPageCode();
    }

    protected void Btn_Edit_Command(object sender, CommandEventArgs e)
    {
        txt_contactName.Text = "";
        txtProductName.Text = "";

        GridViewRow gvRow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        string Trans_ID = e.CommandArgument.ToString();
        Edit_ID.Value = e.CommandArgument.ToString();
        DataTable Dt_Camp_List = Session["Camp_List"] as DataTable;
        Dt_Camp_List = new DataView(Dt_Camp_List, "Trans_ID = " + Trans_ID, "", DataViewRowState.CurrentRows).ToTable();

        if (Dt_Camp_List.Rows.Count > 0)
        {
            Txt_Campaign_Name.Text = Dt_Camp_List.Rows[0]["Campaign_Name"].ToString();
            Txt_Start_Date.Text = GetDate(Dt_Camp_List.Rows[0]["Start_Date"].ToString());
            Txt_End_Date.Text = GetDate(Dt_Camp_List.Rows[0]["End_Date"].ToString());
            txt_Description.Text = Dt_Camp_List.Rows[0]["Description"].ToString();
            ddl_C_Type.Text = Dt_Camp_List.Rows[0]["Campaign_type"].ToString();
            Txt_Budget.Text = Dt_Camp_List.Rows[0]["Budget"].ToString();
            ddlCurrency.SelectedValue = Dt_Camp_List.Rows[0]["Currency_id"].ToString();

            try
            {
                HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
                if (Dt_Camp_List.Rows[0]["Assign_to"].ToString() == "")
                {
                    txtAssignTo.Text = "";
                    hdnAssignedToEmpId.Value = "";
                }
                else
                {
                    DataTable dt_assigntoDtls = HR_EmployeeDetail.GetEmpName_Code(Dt_Camp_List.Rows[0]["Assign_to"].ToString());
                    txtAssignTo.Text = dt_assigntoDtls.Rows[0]["Emp_Name"].ToString() + "/" + dt_assigntoDtls.Rows[0]["Emp_Code"].ToString();
                    hdnAssignedToEmpId.Value = Dt_Camp_List.Rows[0]["Assign_to"].ToString();
                }
            }
            catch
            {

            }
            DataTable Dt_Product = Camp.ProductDataByCampaignId(Trans_ID);
            DataTable Dt_Contact = Camp.ContactDataByCampaignId(Trans_ID);

            Gv_Product_Add.DataSource = Dt_Product;
            Gv_Product_Add.DataBind();
            Gv_Contact_Add.DataSource = Dt_Contact;
            Gv_Contact_Add.DataBind();
        }
        else
        {
            Gv_Product_Add.DataSource = null;
            Gv_Product_Add.DataBind();
            Gv_Contact_Add.DataSource = null;
            Gv_Contact_Add.DataBind();
        }
        Lbl_Tab_New.Text = Resources.Attendance.Edit;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_ContactAdd_Open()", true);
    }

    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        string Trans_ID = e.CommandArgument.ToString();
        int InActiveRef = 0;

        InActiveRef = Camp.InActivateCampaignRecordById(e.CommandArgument.ToString(), Session["UserId"].ToString());

        if (InActiveRef != 0)
        {
            DisplayMessage("Record Deleted");
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);

            GV_Campaign_List.DataSource = null;
            GV_Campaign_List.DataBind();
            fillgrid();
            fillgridbin();
            ////AllPageCode();
        }
    }

    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFieldName.SelectedValue.ToString() == "Start_Date" || ddlFieldName.SelectedValue.ToString() == "End_Date")
        {
            txtValue.Visible = false;
            TxtValueDate.Visible = true;
            txtValue.Text = "";
            TxtValueDate.Attributes.Add("readonly", "readonly");
        }
        else
        {
            TxtValueDate.Visible = false;
            txtValue.Visible = true;
            TxtValueDate.Text = "";
        }
    }

    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        fillgridSession();
        DataTable dtCust = (DataTable)Session["Camp_List"];

        if (ddlFieldName.SelectedValue.ToString() == "Start_Date" || ddlFieldName.SelectedValue.ToString() == "End_Date")
        {
            string condition = string.Empty;
            if (TxtValueDate.Text.Trim() == "")
            {
                DisplayMessage("Please Enter Date");
                TxtValueDate.Focus();
                return;
            }
            if (ddlOption.SelectedIndex == 1)
            {
                // condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + TxtValueDate.Text.Trim() + "'";
                condition = ddlFieldName.SelectedValue + "='" + TxtValueDate.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = ddlFieldName.SelectedValue + "='" + TxtValueDate.Text.Trim() + "'";
            }
            else
            {
                condition = ddlFieldName.SelectedValue + "='" + TxtValueDate.Text.Trim() + "'";
            }
        }
        else
        {
            string condition = string.Empty;
            if (txtValue.Text.Trim() == "")
            {
                DisplayMessage("Please Enter Credentials");
                txtValue.Focus();
                return;
            }
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
                if (ddlOption.SelectedIndex == 0)
                {
                    DisplayMessage("Select Options");
                    fillgrid();
                }
                else
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
                }
            }
        }

        if (dtCust != null)
        {
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            objPageCmn.FillData((object)GV_Campaign_List, view.ToTable(), "", "");
            Session["Camp_List"] = view.ToTable();
            ////AllPageCode();
        }
        else
        {
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ":0";
            objPageCmn.FillData((object)GV_Campaign_List, null, "", "");
        }
        ddlOption.Focus();
        ////AllPageCode();
    }



    protected void Refresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        txtValue.Text = "";
        TxtValueDate.Text = "";
        txtValue.Visible = true;
        TxtValueDate.Visible = false;
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        fillgrid();
        ////AllPageCode();
    }

    protected void GV_Campaign_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GV_Campaign_List.PageIndex = e.NewPageIndex;
        DataTable dt_list_grid = (DataTable)Session["Camp_List"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GV_Campaign_List, dt_list_grid, "", "");
        ////AllPageCode();
    }

    protected void GV_Campaign_List_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt_List_Sort = (DataTable)Session["Camp_List"];
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

        dt_List_Sort = (new DataView(dt_List_Sort, "", e.SortExpression + " " + ViewState["SortDir"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        objPageCmn.FillData((object)GV_Campaign_List, dt_List_Sort, "", "");
        Session["Camp_List"] = dt_List_Sort;
        ////AllPageCode();
    }

    public bool checkValidations()
    {
        if (Txt_Campaign_Name.Text == "")
        {
            DisplayMessage("Enter Campaign Name");
            Txt_Campaign_Name.Focus();
            return false;
        }
        else if (Txt_Start_Date.Text == "")
        {
            DisplayMessage("Enter Start Date");
            Txt_Start_Date.Focus();
            return false;
        }
        else if (Txt_End_Date.Text == "")
        {
            DisplayMessage("Enter End Date");
            Txt_End_Date.Focus();
            return false;
        }

        else if (txtAssignTo.Text == "")
        {
            DisplayMessage("Enter Assigned To Person Name");
            txtAssignTo.Focus();
            return false;
        }

        else if (objSys.getDateForInput(Txt_Start_Date.Text) > objSys.getDateForInput(Txt_End_Date.Text))
        {
            DisplayMessage("End Date must be greater than Start Date");
            Txt_End_Date.Focus();
            Txt_End_Date.Text = "";
            return false;
        }

        else if (ddl_C_Type.SelectedIndex <= 0)
        {
            DisplayMessage("select Campaign Type");
            ddl_C_Type.Focus();
            return false;
        }
        else if (Txt_Budget.Text == "")
        {
            DisplayMessage("Enter Budget");
            Txt_Budget.Focus();
            return false;
        }
        else
        {
            int parsedValue;
            decimal parseddecimal;
            if (!int.TryParse(Txt_Budget.Text, out parsedValue))
            {

                if (!decimal.TryParse(Txt_Budget.Text, out parseddecimal))
                {
                    DisplayMessage("Amount entered was not in correct format");
                    Txt_Budget.Text = "";
                    Txt_Budget.Focus();
                    return false;
                }
            }
        }

        if (txt_Description.Text == "")
        {
            DisplayMessage("Enter Description Of Campaign");
            txt_Description.Focus();
            return false;
        }
        //else if (Gv_Product_Add.Rows.Count == 0)
        //{
        //    DisplayMessage("Enter Product Detailes");
        //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_AddProduct_Open()", true);
        //    txtProductName.Focus();
        //    return false;
        //}
        //else if (Gv_Contact_Add.Rows.Count == 0)
        //{
        //    DisplayMessage("Enter Contact Detailes");
        //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_ContactAdd_Open()", true);
        //    txt_contactName.Focus();
        //    return false;
        //}

        return true;
    }


    protected void Btn_Save_Click(object sender, EventArgs e)
    {
        Btn_Save.Enabled = false;
        string targetSals = "", productId = "", contactId = "", empToVisit = "", visitdate = "", visitCloseDate = "";
        int CampaignId = 0;
        int productRef = 0;
        int contactRef = 0;


        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            if (!checkValidations())
            {
                Btn_Save.Enabled = true;
                return;
            }
            else if (Edit_ID.Value == "")
            {

                CampaignId = Camp.InsertCampaign(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Txt_Campaign_Name.Text, Convert.ToDateTime(Txt_Start_Date.Text).ToString(), Convert.ToDateTime(Txt_End_Date.Text).ToString(), Session["UserId"].ToString(), Session["UserId"].ToString(), Txt_Budget.Text, txt_Description.Text, ddl_C_Type.Text, ddlCurrency.SelectedValue, hdnAssignedToEmpId.Value, ref trns);

                if (CampaignId != 0)
                {
                    for (int i = 0; i < Gv_Product_Add.Rows.Count; i++)
                    {
                        productId = (Gv_Product_Add.Rows[i].FindControl("hdnProductID") as HiddenField).Value;
                        targetSals = (Gv_Product_Add.Rows[i].FindControl("Lbl_TargetQty") as Label).Text;
                        productRef = Camp.InsertProduct(CampaignId.ToString(), productId, Session["UserId"].ToString(), Session["UserId"].ToString(), targetSals, ref trns);
                    }


                    deleteReminder(CampaignId.ToString());

                    for (int i = 0; i < Gv_Contact_Add.Rows.Count; i++)
                    {
                        contactId = (Gv_Contact_Add.Rows[i].FindControl("Hdn_Trans_ID_Master") as HiddenField).Value;
                        empToVisit = (Gv_Contact_Add.Rows[i].FindControl("Lbl_EmpToVisit") as Label).Text;
                        visitdate = (Gv_Contact_Add.Rows[i].FindControl("Lbl_Visitdate") as Label).Text;
                        visitCloseDate = (Gv_Contact_Add.Rows[i].FindControl("Lbl_VisitClosedDate") as Label).Text;

                        if (empToVisit.Trim() != "")
                        {
                            contactRef = Camp.InsertContact(CampaignId.ToString(), contactId, objempDetails.GetEmployeeId(empToVisit.Split('/')[1].ToString()), visitdate, visitCloseDate, Session["UserId"].ToString(), Session["UserId"].ToString(), ref trns);
                        }
                        else
                        {
                            contactRef = Camp.InsertContact(CampaignId.ToString(), contactId, "", visitdate, visitCloseDate, Session["UserId"].ToString(), Session["UserId"].ToString(), ref trns);
                        }

                        if (visitdate.Trim() != "")
                        {
                            string customer = (Gv_Contact_Add.Rows[i].FindControl("Lbl_Contact_Master") as Label).Text;
                            string message = "Pending Visit for Today for " + customer + "";
                            setReminder(visitdate, Txt_Campaign_Name.Text, CampaignId.ToString(), message);
                        }

                    }

                    DisplayMessage("Record Saved", "green");
                    trns.Commit();

                    ddl_C_Type.SelectedIndex = 0;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);

                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    trns.Dispose();
                    con.Dispose();
                    fillgrid();
                    ////AllPageCode();

                }

            }
            else
            {
                // Edit case

                CampaignId = Camp.UpdateCampaign(Edit_ID.Value, Txt_Campaign_Name.Text, Convert.ToDateTime(Txt_Start_Date.Text).ToString(), Convert.ToDateTime(Txt_End_Date.Text).ToString(), Session["UserId"].ToString(), Session["UserId"].ToString(), Txt_Budget.Text, txt_Description.Text, ddl_C_Type.Text, ddlCurrency.SelectedValue, hdnAssignedToEmpId.Value, ref trns);
                if (CampaignId != 0)
                {

                    if (Gv_Product_Add.Rows.Count > 0)
                    {
                        //first delete all old product dtls records
                        productRef = Camp.DeleteProduct(Edit_ID.Value, ref trns);

                        //second insert 1 by 1 record from product grid
                        for (int i = 0; i < Gv_Product_Add.Rows.Count; i++)
                        {
                            productId = (Gv_Product_Add.Rows[i].FindControl("hdnProductID") as HiddenField).Value;
                            targetSals = (Gv_Product_Add.Rows[i].FindControl("Lbl_TargetQty") as Label).Text;
                            productRef = Camp.InsertProduct(Edit_ID.Value, productId, Session["UserId"].ToString(), Session["UserId"].ToString(), targetSals, ref trns);
                        }
                    }

                    if (Gv_Contact_Add.Rows.Count > 0)
                    {
                        // delete all old contact records
                        contactRef = Camp.DeleteContact(Edit_ID.Value, ref trns);

                        //insert 1 by 1 record from contact grid
                        for (int i = 0; i < Gv_Contact_Add.Rows.Count; i++)
                        {
                            contactId = (Gv_Contact_Add.Rows[i].FindControl("Hdn_Trans_ID_Master") as HiddenField).Value;
                            empToVisit = (Gv_Contact_Add.Rows[i].FindControl("Lbl_EmpToVisit") as Label).Text;
                            visitdate = (Gv_Contact_Add.Rows[i].FindControl("Lbl_Visitdate") as Label).Text;
                            visitCloseDate = (Gv_Contact_Add.Rows[i].FindControl("Lbl_VisitClosedDate") as Label).Text;

                            if (empToVisit.Trim() != "")
                            {
                                contactRef = Camp.InsertContact(Edit_ID.Value, contactId, objempDetails.GetEmployeeId(empToVisit.Split('/')[1].ToString()), visitdate, visitCloseDate, Session["UserId"].ToString(), Session["UserId"].ToString(), ref trns);

                            }
                            else
                            {
                                contactRef = Camp.InsertContact(Edit_ID.Value, contactId, "", visitdate, visitCloseDate, Session["UserId"].ToString(), Session["UserId"].ToString(), ref trns);
                            }

                        }

                    }

                    DisplayMessage("Record Updated", "green");
                    Lbl_Tab_New.Text = "New";
                    ddl_C_Type.SelectedIndex = 0;
                    trns.Commit();
                    fillgrid();
                    ////AllPageCode();
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    trns.Dispose();
                    con.Dispose();
                }
                else
                {
                    DisplayMessage("Record Not Update");
                    trns.Rollback();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    trns.Dispose();
                    con.Dispose();
                }

            }
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

        Btn_Save.Enabled = true;
    }


    protected void Btn_Reset_Click(object sender, EventArgs e)
    {
        Txt_Start_Date.Text = "";
        Txt_End_Date.Text = "";
        Txt_Campaign_Name.Text = "";
        Txt_Campaign_Name.Focus();
        Txt_Budget.Text = "";
        txt_Description.Text = "";
        if (strCurrencyId == "")
        {
            try
            {
                ddlCurrency.SelectedValue = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), ddlLocation.SelectedValue).Rows[0]["Field1"].ToString();
                strCurrencyId = ddlCurrency.SelectedValue;
                ddlCurrency.SelectedValue = strCurrencyId;
            }
            catch
            {

            }
        }
        else
        {
            ddlCurrency.SelectedValue = strCurrencyId;
        }
        txtProductName.Text = "";
        ddl_C_Type.SelectedIndex = 0;
        Txt_Target.Text = "";
        Gv_Product_Add.DataSource = null;
        Gv_Product_Add.DataBind();
        Gv_Contact_Add.DataSource = null;
        Gv_Contact_Add.DataBind();
        Lbl_Tab_New.Text = "New";
        Edit_ID.Value = "";
        txtAssignTo.Text = "";
        hdnAssignedToEmpId.Value = "";
    }

    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {
        Btn_Reset_Click(sender, e);
        Trans_ID.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        Gv_Product_Add.DataSource = null;
        Gv_Product_Add.DataBind();
        Gv_Contact_Add.DataSource = null;
        Gv_Contact_Add.DataBind();
    }

    protected void btnProductAdd_Click(object sender, EventArgs e)
    {
        string productId = "", ProductName = "", SalesQty = "";
        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_AddProduct_Open()", true);


        if (txtProductName.Text == "")
        {
            DisplayMessage("Enter Product");
            txtProductName.Focus();
            return;
        }
        if (Txt_Target.Text == "")
        {
            DisplayMessage("Enter Target of sale");
            Txt_Target.Focus();
            return;
        }
        else
        {
            if (Convert.ToInt32(Txt_Target.Text) == 0)
            {
                DisplayMessage("Target Sales Must Be Greter");
                Txt_Target.Text = "";
                Txt_Target.Focus();
                return;
            }
        }

        DataTable Product_Master = objProduct.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), txtProductName.Text, HttpContext.Current.Session["FinanceYearId"].ToString(),Session["LocId"].ToString());

        if (Product_Master.Rows.Count > 0)
        {
            string product = "";
            for (int i = 0; i < Gv_Product_Add.Rows.Count; i++)
            {
                product = (Gv_Product_Add.Rows[i].FindControl("hdnProductID") as HiddenField).Value;
                if (product == Product_Master.Rows[0]["ProductId"].ToString())
                {
                    DisplayMessage("Product Already Exists");
                    txtProductName.Text = "";
                    Txt_Target.Text = "";
                    txtProductName.Focus();
                    return;
                }
            }
            DataTable Dt_Product_Grid = new DataTable();
            Dt_Product_Grid.Columns.AddRange(new DataColumn[3] { new DataColumn("ProductId", typeof(int)), new DataColumn("EProductName"), new DataColumn("Target_sales_qty") });

            for (int i = 0; i < Gv_Product_Add.Rows.Count; i++)
            {
                productId = (Gv_Product_Add.Rows[i].FindControl("hdnProductID") as HiddenField).Value;
                ProductName = (Gv_Product_Add.Rows[i].FindControl("Lbl_ProductName") as Label).Text;
                SalesQty = (Gv_Product_Add.Rows[i].FindControl("Lbl_TargetQty") as Label).Text;
                Dt_Product_Grid.Rows.Add(productId, ProductName, SalesQty);
            }

            Dt_Product_Grid.Rows.Add(Product_Master.Rows[0]["ProductId"].ToString(), Product_Master.Rows[0]["EProductName"].ToString(), Txt_Target.Text);
            Gv_Product_Add.DataSource = Dt_Product_Grid;
            Gv_Product_Add.DataBind();
            txtProductName.Text = "";
            Txt_Target.Text = "";
            Txt_Target.Focus();
            txtProductName.Focus();


        }
    }

    protected void Txt_Campaign_Name_TextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt_Duplicacy = Camp.CheckDuplicacy(Session["CompId"].ToString().ToString(), Txt_Campaign_Name.Text.Trim(), "1");
            if (dt_Duplicacy.Rows.Count > 0)
            {
                Txt_Campaign_Name.Text = "";
                DisplayMessage("Campaign Name Already Exists");
                Txt_Campaign_Name.Focus();
                return;
            }

        }
    }

    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        string strProductId = string.Empty;
        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_AddProduct_Open()", true);
        Btn_Add_Div.Attributes.Add("Class", "fa fa-minus");
        Div_Box_Add.Attributes.Add("Class", "box box-primary");

        //Btn_Add_Div.Attributes.Add("Class", "fa fa-minus");
        //Btn_Add_Div.Attributes.Add("Class", "box box-primary");
        if (txtProductName.Text != "")
        {
            strProductId = GetProductDtl();
            if (strProductId != "" && strProductId != "0")
            {
                txtProductName.Text = strProductId;
                txtProductName.Focus();
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txtProductName.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
            }
        }
        else
        {
            txtProductName.Text = "";
            txtProductName.Focus();
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
        }
        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_AddProduct_Open()", true);
    }

    private string GetProductDtl()
    {
        string retval = "";
        try
        {
            if (txtProductName.Text != "")
            {
                DataTable dtProduct = objProduct.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), txtProductName.Text, HttpContext.Current.Session["FinanceYearId"].ToString(),Session["LocId"].ToString());

                if (dtProduct.Rows.Count > 0)
                {
                    retval = dtProduct.Rows[0]["EProductName"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
        }

        return retval;
    }

    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)Gv_Camapign_Bin.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < Gv_Camapign_Bin.Rows.Count; i++)
        {
            ((CheckBox)Gv_Camapign_Bin.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((HiddenField)(Gv_Camapign_Bin.Rows[i].FindControl("hdnTrans_Id"))).Value.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((HiddenField)(Gv_Camapign_Bin.Rows[i].FindControl("hdnTrans_Id"))).Value.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((HiddenField)(Gv_Camapign_Bin.Rows[i].FindControl("hdnTrans_Id"))).Value.Trim().ToString())
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

    protected void Gv_Camapign_Bin_Sorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt_Bin_Sorting = (DataTable)Session["dtBinFilter"];
        DataView dv = new DataView(dt_Bin_Sorting);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt_Bin_Sorting = dv.ToTable();
        Session["dtBinFilter"] = dt_Bin_Sorting;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)Gv_Camapign_Bin, dt_Bin_Sorting, "", "");
        lblSelectedRecord.Text = "";
        ////AllPageCode();
    }

    protected void Gv_Camapign_Bin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Gv_Camapign_Bin.PageIndex = e.NewPageIndex;
        DataTable dt_Bin_paging = (DataTable)Session["dtBinFilter"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)Gv_Camapign_Bin, dt_Bin_paging, "", "");

        for (int i = 0; i < Gv_Camapign_Bin.Rows.Count; i++)
        {
            HiddenField lblconid = (HiddenField)Gv_Camapign_Bin.Rows[i].FindControl("hdnTrans_Id");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)Gv_Camapign_Bin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }
        ////AllPageCode();
    }

    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        DataTable dtPbrand = (DataTable)Session["dtBinFilter"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPbrand.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Trans_ID"]))
                {
                    lblSelectedRecord.Text += dr["Trans_ID"] + ",";
                }
            }
            for (int i = 0; i < Gv_Camapign_Bin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                HiddenField lblconid = (HiddenField)Gv_Camapign_Bin.Rows[i].FindControl("hdnTrans_Id");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)Gv_Camapign_Bin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtBinFilter"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)Gv_Camapign_Bin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
        //AllPageCode();
    }

    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        DateTime date = Convert.ToDateTime(System.DateTime.Now.ToString());

        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {

                    Camp.ActivateCampaignRecordById(lblSelectedRecord.Text.Split(',')[j].Trim(), Session["UserId"].ToString());

                    fillgridbin();
                    fillgrid();
                    DisplayMessage("Record Activate");
                }
            }
        }


        int fleg = 0;
        foreach (GridViewRow Gvr in Gv_Camapign_Bin.Rows)
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
        //AllPageCode();
    }

    protected void btnAssignTask_Command(object sender, CommandEventArgs e)
    {

        Session["CampaignData"] = "";
        lblCampId.Text = e.CommandArgument.ToString();
        try
        {
            Session["CampaignData"] = e.CommandArgument.ToString();
            AddTaskUC.requestData();
            AddTaskUC.fillDate();
        }
        catch (Exception ex)
        {
        }
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Address_Open()", true);

    }
    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        txtbinValueDate.Text = "";
        fillgridbin();
    }

    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        fillgridbin();
        DataTable dtCust = (DataTable)Session["dtbinFilter"];
        if (ddlbinFieldName.SelectedValue.ToString() == "Start_Date" || ddlbinFieldName.SelectedValue.ToString() == "End_Date")
        {
            string condition = string.Empty;
            if (txtbinValueDate.Text.Trim() == "")
            {
                DisplayMessage("Please Enter Date");
                txtbinValueDate.Focus();
                return;
            }
            if (ddlbinOption.SelectedIndex == 1)
            {
                // condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + TxtValueDate.Text.Trim() + "'";
                condition = ddlbinFieldName.SelectedValue + "='" + txtbinValueDate.Text.Trim() + "'";
            }
            else if (ddlbinOption.SelectedIndex == 2)
            {
                condition = ddlbinFieldName.SelectedValue + "='" + txtbinValueDate.Text.Trim() + "'";
            }
            else
            {
                condition = ddlbinFieldName.SelectedValue + "='" + txtbinValueDate.Text.Trim() + "'";
            }
        }
        else
        {
            string condition = string.Empty;
            if (txtbinValue.Text.Trim() == "")
            {
                DisplayMessage("Please Enter Credentials");
                txtbinValue.Focus();
                return;
            }
            if (ddlbinOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + txtbinValue.Text.Trim() + "'";
            }
            else if (ddlbinOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + txtbinValue.Text.Trim() + "%'";
            }
            else
            {
                if (ddlbinOption.SelectedIndex == 0)
                {
                    DisplayMessage("Select Options");
                    fillgridbin();
                }
                else
                {
                    condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + txtbinValue.Text.Trim() + "%'";
                }
            }

            if (dtCust != null)
            {
                DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
                lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
                objPageCmn.FillData((object)Gv_Camapign_Bin, view.ToTable(), "", "");
                Session["dtBinFilter"] = view.ToTable();
                //AllPageCode();

            }
            else
            {

                lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ":0";
                objPageCmn.FillData((object)Gv_Camapign_Bin, null, "", "");
            }

            ddlbinOption.Focus();
            //AllPageCode();
        }
    }

    protected void ddlbinFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlbinFieldName.SelectedValue.ToString() == "Start_Date" || ddlbinFieldName.SelectedValue.ToString() == "End_Date")
        {
            txtbinValue.Visible = false;
            txtbinValueDate.Visible = true;
            txtbinValue.Text = "";
        }
        else
        {
            txtbinValueDate.Visible = false;
            txtbinValue.Visible = true;
            txtbinValue.Text = "";
        }
    }

    protected void Gv_Product_Add_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_AddProduct_Open()", true);
        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_ContactAdd_Open()", true);

        DataTable Dt_Product_Grid = new DataTable();
        Dt_Product_Grid.Columns.AddRange(new DataColumn[3] { new DataColumn("ProductId", typeof(int)), new DataColumn("EProductName"), new DataColumn("Target_sales_qty") });
        string productId = "", ProductName = "", SalesQty = "";
        for (int i = 0; i < Gv_Product_Add.Rows.Count; i++)
        {
            productId = (Gv_Product_Add.Rows[i].FindControl("hdnProductID") as HiddenField).Value;
            ProductName = (Gv_Product_Add.Rows[i].FindControl("Lbl_ProductName") as Label).Text;
            SalesQty = (Gv_Product_Add.Rows[i].FindControl("Lbl_TargetQty") as Label).Text;
            Dt_Product_Grid.Rows.Add(productId, ProductName, SalesQty);
        }

        Dt_Product_Grid.Rows.RemoveAt(e.RowIndex);
        Gv_Product_Add.DataSource = Dt_Product_Grid;
        Gv_Product_Add.DataBind();
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        try
        {
            Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = PM.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(),HttpContext.Current.Session["LocId"].ToString());
            string[] txt = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["EProductName"].ToString();
            }
            return txt;
        }
        catch (Exception error)
        {

        }
        return null;

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        try
        {
            EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dtCon = ObjEmployeeMaster.GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), prefixText.ToString());

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
        catch (Exception error)
        {

        }
        return null;

    }

    protected void Btn_Contactadd_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_ContactAdd_Open()", true);

        if (txt_contactName.Text.Trim() == "")
        {
            DisplayMessage("Enter Contact Name");
            txt_contactName.Focus();
            return;
        }
        else
        {
            int start_pos = txt_contactName.Text.LastIndexOf("/") + 1;
            int last_pos = txt_contactName.Text.Length;
            string id = txt_contactName.Text.Substring(start_pos, last_pos - start_pos);

            DataTable contactInfo = ObjContactMaster.GetContactTrueById(id);
            string contactID = "";
            for (int i = 0; i < Gv_Contact_Add.Rows.Count; i++)
            {
                contactID = (Gv_Contact_Add.Rows[i].FindControl("Hdn_Trans_ID_Master") as HiddenField).Value;
                if (contactID == id)
                {
                    DisplayMessage("Contact Already Exists");
                    txt_contactName.Text = "";
                    txt_contactName.Focus();
                    return;
                }

            }

            DataTable Dt_Contact_Grid = new DataTable();
            Dt_Contact_Grid.Columns.AddRange(new DataColumn[5] { new DataColumn("contact_id", typeof(int)), new DataColumn("Name"), new DataColumn("empToVisit"), new DataColumn("Field2"), new DataColumn("Field3") });

            string ContactId = "", ContactName = "", empToVisit = "", visitDate = "", VisitClosedDate = "";
            for (int i = 0; i < Gv_Contact_Add.Rows.Count; i++)
            {
                ContactId = (Gv_Contact_Add.Rows[i].FindControl("Hdn_Trans_ID_Master") as HiddenField).Value;
                ContactName = (Gv_Contact_Add.Rows[i].FindControl("Lbl_Contact_Master") as Label).Text;
                empToVisit = (Gv_Contact_Add.Rows[i].FindControl("Lbl_EmpToVisit") as Label).Text;
                visitDate = (Gv_Contact_Add.Rows[i].FindControl("Lbl_Visitdate") as Label).Text;
                VisitClosedDate = (Gv_Contact_Add.Rows[i].FindControl("Lbl_VisitClosedDate") as Label).Text;
                Dt_Contact_Grid.Rows.Add(ContactId, ContactName, empToVisit, visitDate, VisitClosedDate);
            }

            Dt_Contact_Grid.Rows.Add(id, txt_contactName.Text, txtEmployeeToVisit.Text, txtVisitDate.Text, txtVisitClosedDate.Text);
            Gv_Contact_Add.DataSource = Dt_Contact_Grid;
            Gv_Contact_Add.DataBind();
            txt_contactName.Text = "";
            txt_contactName.Focus();
            txtEmployeeToVisit.Text = "";
            txtVisitClosedDate.Text = "";
            txtVisitDate.Text = "";
        }
    }


    protected void txt_contactName_TextChanged(object sender, EventArgs e)
    {
        Btn_Add_Div1.Attributes.Add("Class", "fa fa-minus");
        Div_Box_Add1.Attributes.Add("Class", "box box-primary");

        string strCustomerId = string.Empty;
        if (txt_contactName.Text != "")
        {
            strCustomerId = GetContactId();
            if (strCustomerId != "" && strCustomerId != "0")
            {
                txt_contactName.Focus();
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                txt_contactName.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txt_contactName);
            }
        }
        else
        {
            txt_contactName.Text = "";
            txt_contactName.Focus();
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txt_contactName);
        }
    }

    private string GetContactId()
    {
        string retval = "";
        try
        {
            if (txt_contactName.Text != "")
            {
                int start_pos = txt_contactName.Text.LastIndexOf("/") + 1;
                int last_pos = txt_contactName.Text.Length;
                string id = txt_contactName.Text.Substring(start_pos, last_pos - start_pos);
                if (start_pos != 0)
                {
                    //DataTable dtSupp = ObjContactMaster.GetContactTrueAllData();
                    DataTable contactInfo = ObjContactMaster.GetContactTrueById(id);

                    int Last_pos_name = txt_contactName.Text.LastIndexOf("/");
                    string name = txt_contactName.Text.Substring(0, Last_pos_name - 0);
                    //string name = txt_contactName.Text.Trim().Split('/')[0].ToString().Trim();
                    contactInfo = new DataView(contactInfo, "Name='" + name + "' ", "", DataViewRowState.CurrentRows).ToTable();

                    if (contactInfo.Rows.Count > 0)
                    {
                        retval = id;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        return retval;
    }

    protected void Gv_Contact_Add_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        Btn_Add_Div1.Attributes.Add("Class", "fa fa-minus");
        Div_Box_Add1.Attributes.Add("Class", "box box-primary");
        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_ContactAdd_Open()", true);

        DataTable Dt_Contact_Grid = new DataTable();
        Dt_Contact_Grid.Columns.AddRange(new DataColumn[5] { new DataColumn("contact_id", typeof(int)), new DataColumn("Name"), new DataColumn("empToVisit"), new DataColumn("Field2"), new DataColumn("Field3") });
        string ContactId = "", ContactName = "", empToVisit = "", visitdate = "", visitCloseDate = "";
        for (int i = 0; i < Gv_Contact_Add.Rows.Count; i++)
        {
            ContactId = (Gv_Contact_Add.Rows[i].FindControl("Hdn_Trans_ID_Master") as HiddenField).Value;
            ContactName = (Gv_Contact_Add.Rows[i].FindControl("Lbl_Contact_Master") as Label).Text;
            empToVisit = (Gv_Contact_Add.Rows[i].FindControl("Lbl_EmpToVisit") as Label).Text;
            visitdate = (Gv_Contact_Add.Rows[i].FindControl("Lbl_Visitdate") as Label).Text;
            visitCloseDate = (Gv_Contact_Add.Rows[i].FindControl("Lbl_VisitClosedDate") as Label).Text;

            Dt_Contact_Grid.Rows.Add(ContactId, ContactName, empToVisit, visitdate, visitCloseDate);
        }

        Dt_Contact_Grid.Rows.RemoveAt(e.RowIndex);
        Gv_Contact_Add.DataSource = Dt_Contact_Grid;
        Gv_Contact_Add.DataBind();
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        try
        {
            Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dtCustomer = ObjContactMaster.GetContactTrueAllData();
            string filtertext = "Name like '%" + prefixText + "%' and Field6 = 'True'";
            DataTable dtCon = new DataView(dtCustomer, filtertext, "", DataViewRowState.CurrentRows).ToTable();

            string[] filterlist = new string[dtCon.Rows.Count];
            if (dtCon.Rows.Count > 0)
            {
                for (int i = 0; i < dtCon.Rows.Count; i++)
                {
                    filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Trans_Id"].ToString();
                    //filterlist[i] = dtCon.Rows[i]["Name"].ToString();
                }
            }
            return filterlist;
        }
        catch (Exception error)
        {
        }
        return null;
    }



    protected void Btn_Add_New_Contact_Click(object sender, EventArgs e)
    {
        string strCmd = string.Format("window.open('../EMS/ContactMaster.aspx?page=SO?','Window','width=1024 height=720 left=200 top=150');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }


    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        HiddenField lb = (HiddenField)Gv_Camapign_Bin.Rows[index].FindControl("hdnTrans_Id");
        if (((CheckBox)Gv_Camapign_Bin.Rows[index].FindControl("chkgvSelect")).Checked)
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

    protected void btnFileUpload_Command(object sender, CommandEventArgs e)
    {
        FUpload1.setID(e.CommandArgument.ToString(), Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + Session["LocId"].ToString() + "/Campaign", "CRM", "Campaign", "", e.CommandName.ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }

    public void fillLocation()
    {
        LocationCondition = "Location_Id=";
        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());

        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        if (!Common.GetStatus(Session["EmpId"].ToString()))
        {
            string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.Items.Add(new ListItem("All", "0"));
            for (int i = 0; i < dtLoc.Rows.Count; i++)
            {
                ddlLocation.Items.Add(new ListItem(dtLoc.Rows[i]["Location_Name"].ToString(), dtLoc.Rows[i]["Location_Id"].ToString()));

                if (i == dtLoc.Rows.Count - 1)
                {
                    LocationCondition = LocationCondition + dtLoc.Rows[i]["Location_Id"].ToString();
                }
                else
                {
                    LocationCondition = LocationCondition + dtLoc.Rows[i]["Location_Id"].ToString() + " or Location_Id=";
                }
            }
        }
        else
        {
            ddlLocation.Items.Clear();
        }

        //cmn.FillUser(Session["CompId"].ToString(), Session["UserId"].ToString(), ddlUser, objObjectEntry.GetModuleIdAndName("54").Rows[0]["Module_Id"].ToString(), "54", ddlLocation.SelectedValue);
    }

    public void fillDropdown(DropDownList ddl, DataTable dt, string DataTextField, string DataValueField)
    {

        if (dt.Rows.Count > 0)
        {
            ddl.DataSource = dt;
            ddl.DataTextField = DataTextField;
            ddl.DataValueField = DataValueField;
            ddl.DataBind();
        }
        else
        {
            ddl = null;
        }
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgridSession();
        fillgrid();
        fillgridbinSession();
        fillgridbin();
    }


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetContactList(string prefixText, int count, string contextKey)
    {
        EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCon = new DataTable();

        try
        {
            dtCon = ObjEmployeeMaster.GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "0", prefixText.ToString());
        }
        catch
        {
            dtCon = new DataTable();
        }

        string[] txt = new string[dtCon.Rows.Count];

        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                txt[i] += dtCon.Rows[i]["Emp_Name"].ToString() + "/" + dtCon.Rows[i]["Emp_Code"].ToString() + "/" + dtCon.Rows[i]["Emp_Id"].ToString();
            }

        }

        return txt;
    }

    protected void txtAssignTo_TextChanged(object sender, EventArgs e)
    {
        string strGeneratedById = string.Empty;

        TextBox tb = sender as TextBox;
        string name = tb.Text;
        if (tb.Text != "")
        {
            HR_EmployeeDetail HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
            string Emp_Code = tb.Text.Split('/')[1].ToString();
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(Emp_Code);
            string Emp_Name = tb.Text.Split('/')[0].ToString() + "/" + Emp_ID;
            strGeneratedById = Emp_ID;
            //getID(Emp_Name);

            if (strGeneratedById != "" && strGeneratedById != "0")
            {
                tb.Text = name;
                tb.Focus();
                hdnAssignedToEmpId.Value = strGeneratedById;
            }
            else
            {
                DisplayMessage("Select In Suggestions Only");
                tb.Text = "";
                tb.Focus();
                hdnAssignedToEmpId.Value = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(tb);
            }
        }
        else
        {
            tb.Text = "";
            tb.Focus();
            hdnAssignedToEmpId.Value = "";
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(tb);
        }
    }

    public string hasDateOrNot(string date)
    {
        if (date == "")
        {
            return "";
        }
        else
        {
            DateTime MyDateTime = DateTime.Parse(date);
            return MyDateTime.ToString("dd-MMM-yyyy");
        }
    }
    public void setReminder(string dueDate, string campaignName, string campaignId, string message)
    {

        int reminder_id = new Reminder(Session["DBConnection"].ToString()).insertData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "crm_campaign", campaignId, message, "", System.DateTime.Now.ToString(), "1", dueDate, "Once", Session["EmpId"].ToString(), "On", "false", "false", "true", Session["UserId"].ToString(), Session["UserId"].ToString());
        new ReminderLogs(Session["DBConnection"].ToString()).insertLogData(reminder_id.ToString(), dueDate, "", Session["UserId"].ToString(), Session["UserId"].ToString());
        new Reminder(Session["DBConnection"].ToString()).Set_Url(reminder_id.ToString(), "../CRM/Campaign.aspx?SearchField=" + campaignName + "");
    }

    public void deleteReminder(string campaignId)
    {
        new Reminder(Session["DBConnection"].ToString()).setIsActiveFalseByRef_table_name_n_pk("crm_campaign ", campaignId);
    }

    protected void Btn_Bin_Click(object sender, EventArgs e)
    {
        fillgridbinSession();
        fillgridbin();
    }
}