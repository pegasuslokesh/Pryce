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

public partial class Sales_CustomerInquiry : BasePage
{
    IT_ObjectEntry objObjectEntry = null;
    Set_CustomerMaster ObjCustomer = null;
    SystemParameter objSysParam = null;
    Inv_CustomerInquiry ObjCustInquiry = null;
    Set_DocNumber ObjDoc = null;
    Ems_ContactMaster objContact = null;
    EmployeeMaster objEmpMaster = null;
    PageControlCommon objPageCmn = null;

    Common cmn = null;
    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        ObjCustomer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjCustInquiry = new Inv_CustomerInquiry(Session["DBConnection"].ToString());
        ObjDoc = new Set_DocNumber(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objEmpMaster = new EmployeeMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        Page.Title = objSysParam.GetSysTitle();
        Calender.Format = Session["DateFormat"].ToString();
        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();

        if (!IsPostBack)
        {
          
            cmn = new Common(Session["DBConnection"].ToString());

            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "232", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            Reset();
            btnList_Click(null, null);
            ViewState["DocNo"] = GetDocumentNumber();
        }
        AllPageCode();
    }
    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        Common ObjComman = new Common(Session["DBConnection"].ToString());

        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("232", (DataTable)Session["ModuleName"]);
        if (dtModule.Rows.Count > 0)
        {
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
            strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        }
        else
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }
        //End Code

        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;

        if (Session["EmpId"].ToString() == "0")
        {
            btnInquirySave.Visible = true;

            foreach (GridViewRow Row in GvCustomerInquiry.Rows)
            {
                ((ImageButton)Row.FindControl("btnEdit")).Visible = true;
                ((ImageButton)Row.FindControl("IbtnDelete")).Visible = true;
            }

            imgBtnRestore.Visible = true;
            ImgbtnSelectAll.Visible = false;
        }

        DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "232",Session["CompId"].ToString());

        foreach (DataRow DtRow in dtAllPageCode.Rows)
        {
            if (DtRow["Op_Id"].ToString() == "1")
            {
                btnInquirySave.Visible = true;
            }

            foreach (GridViewRow Row in GvCustomerInquiry.Rows)
            {
                if (DtRow["Op_Id"].ToString() == "2")
                {
                    ((ImageButton)Row.FindControl("btnEdit")).Visible = true;
                }
                if (DtRow["Op_Id"].ToString() == "3")
                {
                    ((ImageButton)Row.FindControl("IbtnDelete")).Visible = true;
                }
            }
            if (DtRow["Op_Id"].ToString() == "4")
            {
                imgBtnRestore.Visible = true;
                ImgbtnSelectAll.Visible = false;
            }
        }
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlNewEdit.Visible = false;
        PnlBin.Visible = true;
        PnlList.Visible = false;
        FillBinGrid();
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = true;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
        FillGrid();

    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = false;
        PnlNewEdit.Visible = true;
        PnlBin.Visible = false;
        AllPageCode();
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {

        FillGrid();
        FillBinGrid();
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";

    }
    protected void btnInquirySave_Click(object sender, EventArgs e)
    {
        if (txtCINo.Text == "")
        {
            DisplayMessage("Enter Inquiry No");
            txtCINo.Focus();
            return;
        }
        if (txtCIDate.Text == "")
        {
            DisplayMessage("Enter Date");
            txtCIDate.Focus();
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtCIDate.Text.Trim());
            }
            catch
            {
                txtCIDate.Text = "";
                txtCIDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString()).ToString();
                txtCIDate.Focus();

                DisplayMessage("Enter Date");
                return;
            }
        }


        string StrCustomer = string.Empty;

        string StrContact = string.Empty;

        bool strrValues = false;
        if (RdoListCustomer.SelectedValue == "1")
        {
            if (txtECustomer.Text == "")
            {
                txtECustomer.Focus();
                DisplayMessage("Select Customer Name");
                return;
            }
            else
            {
                StrCustomer = txtECustomer.Text.Split('/')[1].ToString();
                if (txtEContact.Text == "")
                {
                    txtEContact.Focus();
                    DisplayMessage("Select Contact Name");
                    return;
                }
                else
                {
                    StrContact = txtEContact.Text.Split('/')[1].ToString();
                }

            }
            strrValues = true; // 1 for Existing Customer
        }
        else
        {
            if (txtNCustomer.Text == "")
            {
                txtNCustomer.Focus();
                DisplayMessage("Enter Customer Name");
                return;
            }
            else
            {
                StrCustomer = txtNCustomer.Text.Trim();
                if (txtContact.Text == "")
                {
                    txtContact.Focus();
                    DisplayMessage("Enter Contact Name");
                    return;
                }
                StrContact = txtContact.Text.Trim();
            }
            strrValues = false; //0 for New Customer
        }

        if (txtContactNo.Text == "")
        {

            DisplayMessage("Enter Contact No");
            txtContactNo.Focus();
            return;
        }
        if (ddlCallType.SelectedIndex == 0)
        {
            DisplayMessage("Select Call Type");
            ddlCallType.Focus();
            return;
        }

        string strRefTo = string.Empty;
        if (txtRefTo.Text != "")
        {
            strRefTo = txtRefTo.Text.Split('/')[1].ToString();

        }
        else
        {
            strRefTo = "0";
        }

        if (Editor1.Content == "")
        {
            Editor1.Focus();
            DisplayMessage("Enter Description ");
            return;
        }


        if (hdnValue.Value == "")
        {

            if (txtCINo.Text != "")
            {
                if ((new DataView(ObjCustInquiry.GetCustomerInquiryAll(StrCompId, StrBrandId, strLocationId), "Inquiry_No='" + txtCINo.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count != 0))
                {
                    DisplayMessage("Inquiry No Already Exist");
                    txtCINo.Text = "";
                    return;
                }
            }

            int b = 0;
            b = ObjCustInquiry.InsertCustomerInquiry(StrCompId, StrBrandId, strLocationId, txtCINo.Text.Trim(), txtCIDate.Text.Trim(), StrCustomer.Trim(), Editor1.Content.Trim(), StrContact, txtContactNo.Text.Trim(), txtEmailId.Text.Trim(), ddlCallType.SelectedValue.ToString(), ddlStatus.SelectedValue.ToString(), strrValues.ToString(), strRefTo.ToString(), true.ToString(), StrUserId.Trim(), DateTime.Now.ToString(), StrUserId.Trim(), DateTime.Now.ToString());

            string strMaxId = string.Empty;

            if (b != 0)
            {
                strMaxId = b.ToString();

                if (txtCINo.Text == ViewState["DocNo"].ToString())
                {

                    DataTable dtCount = ObjCustInquiry.GetCustomerInquiryAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                    if (dtCount.Rows.Count == 0)
                    {
                        ObjCustInquiry.Updatecode(b.ToString(), txtCINo.Text +"1");
                        txtCINo.Text = txtCINo.Text + "1";
                    }
                    else
                    {
                        ObjCustInquiry.Updatecode(b.ToString(), txtCINo.Text + dtCount.Rows.Count);
                        txtCINo.Text = txtCINo.Text + dtCount.Rows.Count;
                    }
                  

                }

                DisplayMessage("Record Saved","green");
            }
        }
        else
        {
            ObjCustInquiry.UpdateCustomerInquiry(hdnValue.Value, StrCompId, StrBrandId, strLocationId, txtCINo.Text.Trim(), txtCIDate.Text.Trim(), StrCustomer.Trim(), Editor1.Content.Trim(), StrContact, txtContactNo.Text.Trim(), txtEmailId.Text.Trim(), ddlCallType.SelectedValue.ToString(), ddlStatus.SelectedValue.ToString(), strrValues.ToString(), strRefTo.ToString(), true.ToString(), StrUserId.Trim(), DateTime.Now.ToString(), StrUserId.Trim(), DateTime.Now.ToString());
            DisplayMessage("Record Updated", "green");
        }
        Reset();
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnInquiryCancel_Click(object sender, EventArgs e)
    {
        Reset();
        btnList_Click(null, null);
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
    {
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
            objPageCmn.FillData((object)GvCustomerInquiry, view.ToTable(), "", "");
           
            Session["dtCInquiry"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";

        }
        AllPageCode();
        txtValue.Focus();
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
        objPageCmn.FillData((object)GvCustomerInquiry, dt, "", "");

        AllPageCode();
    }
    protected void GvCustomerInquiry_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvCustomerInquiry.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtCInquiry"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvCustomerInquiry, dt, "", "");
       
        AllPageCode();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = ObjCustInquiry.GetCustomerInquiryAllTrueByTransId(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());
        if (dt.Rows.Count != 0)
        {
            TrIn.Visible = true;
            txtCINo.ReadOnly = true;
            btnNew.Text = Resources.Attendance.Edit;
            btnNew_Click(null, null);
            hdnValue.Value = e.CommandArgument.ToString();
            txtCIDate.Text = Convert.ToDateTime(dt.Rows[0]["InquiryDate"].ToString()).ToString(HttpContext.Current.Session["DateFormat"].ToString());
            txtCINo.Text = dt.Rows[0]["Inquiry_No"].ToString();
            Editor1.Content = dt.Rows[0]["Description"].ToString();

            txtContactNo.Text = dt.Rows[0]["Field2"].ToString();
            txtEmailId.Text = dt.Rows[0]["Field3"].ToString();
            if (dt.Rows[0]["Field5"].ToString() != "")
            {
                ddlStatus.SelectedValue = dt.Rows[0]["Field5"].ToString();
            }
            else
            {
                ddlStatus.SelectedIndex = 1;
            }
            if (dt.Rows[0]["Field4"].ToString() != "")
            {
                ddlCallType.SelectedValue = dt.Rows[0]["Field4"].ToString();
            }
            else
            {
                ddlCallType.SelectedIndex = 1;
            }

            if (dt.Rows[0]["Field6"].ToString() == "True")
            {
                txtECustomer.Text = dt.Rows[0]["CustomerName"].ToString() + "/" + dt.Rows[0]["Custmer"].ToString();
                RdoListCustomer.SelectedValue = "1";
                txtEContact.Text = dt.Rows[0]["ContactName"].ToString() + "/" + dt.Rows[0]["Field1"].ToString();

            }
            else
            {
                txtNCustomer.Text = dt.Rows[0]["CustomerName"].ToString();
                txtContact.Text = dt.Rows[0]["Field1"].ToString();
                RdoListCustomer.SelectedValue = "0";
            }
            if (dt.Rows[0]["Field7"].ToString() != "0")
            {
                txtRefTo.Text = dt.Rows[0]["EmployeeName"].ToString() + "/" + dt.Rows[0]["Field7"].ToString();
            }
            RdoListCustomer_SelectedIndexChanged(null, null);
        }
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        hdnValue.Value = e.CommandArgument.ToString();
        b = ObjCustInquiry.DeleteCustomerInquiry(StrCompId, StrBrandId, strLocationId, hdnValue.Value, "false", StrUserId, DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Delete");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        FillBinGrid(); //Update grid view in bin tab
        FillGrid();
        Reset();

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
                DisplayMessage("Select In Suggestions Only");
                txtECustomer.Text = "";
                txtEContact.Text = "";
                txtContactNo.Text = "";
                txtEmailId.Text = "";
                txtECustomer.Focus();
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtECustomer);
            }
            else
            {
                txtEContact.Text = "";
                txtEContact.Focus();
                Session["ContactID"] = strCustomerId;
            }


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
                strCustomerId = txtEContact.Text.Split('/')[1].ToString();

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
                    txtContactNo.Text = dtcust.Rows[0]["Field2"].ToString();
                    txtEmailId.Text = dtcust.Rows[0]["Field1"].ToString();
                }
            }


        }
        // AllPageCode();
    }

    protected void txtRefTo_TextChanged(object sender, EventArgs e)
    {
        string strEmpId = string.Empty;
        if (txtRefTo.Text != "")
        {
            try
            {
                strEmpId = txtRefTo.Text.Split('/')[1].ToString();

            }
            catch
            {
                strEmpId = "0";

            }
            if (strEmpId == "0")
            {
                DisplayMessage("Select In Suggestions Only");
                txtRefTo.Text = "";
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRefTo);
                return;

            }


        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]

    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Set_CustomerMaster objcustomer = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dtCustomer = objcustomer.GetCustomerAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());


        string filtertext = "Filtertext like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtCustomer, filtertext, "", DataViewRowState.CurrentRows).ToTable();

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


    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContact(string prefixText, int count, string contextKey)
    {

        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string id = HttpContext.Current.Session["ContactID"].ToString();


        DataTable dt = ObjContactMaster.GetContactTrueAllData(id, "Individual");

        string[] filterlist = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                filterlist[i] = dt.Rows[i]["Name"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString();
            }
            return filterlist;
        }
        else
        {
            DataTable dtcon = ObjContactMaster.GetContactTrueById(id);
            string[] filterlistcon = new string[dtcon.Rows.Count];
            for (int i = 0; i < dtcon.Rows.Count; i++)
            {
                filterlistcon[i] = dtcon.Rows[i]["Name"].ToString() + "/" + dtcon.Rows[i]["Trans_Id"].ToString();
            }
            return filterlistcon;

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



   


    protected void RdoListCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RdoListCustomer.SelectedValue == "0")
        {
            txtECustomer.Visible = false;
            txtNCustomer.Visible = true;
            txtContact.Visible = true;
            txtEContact.Visible = false;
        }
        else
        {
            txtECustomer.Visible = true;
            txtNCustomer.Visible = false;
            txtContact.Visible = false;
            txtEContact.Visible = true;


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
        txtECustomer.Text = "";
        txtNCustomer.Text = "";

        txtEContact.Text = "";
        txtContact.Text = "";

        ddlCallType.SelectedIndex = 1;
        ddlStatus.SelectedIndex = 1;

        txtContactNo.Text = "";
        txtEmailId.Text = "";

        txtRefTo.Text = "";

        txtCIDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString()).ToString();
        Editor1.Content = "";
        RdoListCustomer.SelectedValue = "0";
        RdoListCustomer_SelectedIndexChanged(null, null);
        btnNew.Text = Resources.Attendance.New;
        txtCINo.Text = "";

        txtCINo.Text = GetDocumentNumber();
        ViewState["DocNo"] = txtCINo.Text;

        hdnValue.Value = "";
        ViewState["Select"] = null;
        TrIn.Visible = true;
        txtCINo.ReadOnly = false;
        objPageCmn.FillUser(StrCompId, StrUserId, ddlUser, objObjectEntry.GetModuleIdAndName("232", (DataTable)Session["ModuleName"]).Rows[0]["Module_Id"].ToString(), "232");

    }
    public void FillGrid()
    {
        DataTable dt = ObjCustInquiry.GetCustomerInquiryAllTrue(StrCompId, StrBrandId, strLocationId);
        if (ddlUser.SelectedValue.Trim() != "--Select User--")
        {
            dt = new DataView(dt, "CreatedBy='" + ddlUser.SelectedValue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvCustomerInquiry, dt, "", "");
      

        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
      
        Session["dtCInquiry"] = dt;
        AllPageCode();
    }
    public void FillBinGrid()
    {
        DataTable dt = ObjCustInquiry.GetCustomerInquiryAllFalse(StrCompId, StrBrandId, strLocationId);
        if (ddlUser.SelectedValue.Trim() != "--Select User--")
        {
            dt = new DataView(dt, "CreatedBy='" + ddlUser.SelectedValue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvCustomerInquiryBin, dt, "", "");
        

        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        Session["dtCInquiryBin"] = dt;
        AllPageCode();
    }

    protected void GvCustomerInquiryBin_Sorting(object sender, GridViewSortEventArgs e)
    {
        ViewState["Select"] = null;
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = ObjCustInquiry.GetCustomerInquiryAllFalse(StrCompId, StrBrandId, strLocationId);
        if (ddlUser.SelectedValue != "--Select User--")
        {
            dt = new DataView(dt, "CreatedBy='" + StrUserId.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtCInquiryBin"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvCustomerInquiryBin, dt, "", "");
       
        lblSelectedRecord.Text = "";
        AllPageCode();

    }
    protected void GvCustomerInquiryBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvCustomerInquiryBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtCInquiryBin"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvCustomerInquiryBin, dt, "", "");
       

        string temp = string.Empty;

        for (int i = 0; i < GvCustomerInquiryBin.Rows.Count; i++)
        {
            Label lblconid = (Label)GvCustomerInquiryBin.Rows[i].FindControl("lblgvInquiryId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)GvCustomerInquiryBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
        AllPageCode();
    }

    protected void btnbindBin_Click(object sender, EventArgs e)
    {
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


            DataTable dtCust = (DataTable)Session["dtCInquiryBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtCInquiryBin"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvCustomerInquiryBin, view.ToTable(), "", "");
      
            lblSelectedRecord.Text = "";
            if (view.ToTable().Rows.Count == 0)
            {
                FillBinGrid();
            }
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        }
        AllPageCode();
        txtValueBin.Focus();
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        FillGrid();
        FillBinGrid();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 0;
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);

    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        int b = 0;
        DataTable dt = ObjCustInquiry.GetCustomerInquiryAllFalse(StrCompId, StrBrandId, strLocationId);

        if (GvCustomerInquiryBin.Rows.Count != 0)
        {
            if (lblSelectedRecord.Text != "")
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        b = ObjCustInquiry.DeleteCustomerInquiry(StrCompId, StrBrandId, strLocationId, lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    }
                }
            }

            if (b != 0)
            {
                FillGrid();
                FillBinGrid();

                lblSelectedRecord.Text = "";
                DisplayMessage("Record Activate");
            }
            else
            {
                int fleg = 0;
                foreach (GridViewRow Gvr in GvCustomerInquiryBin.Rows)
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
        DataTable dtCustInq = (DataTable)Session["dtCInquiryBin"];

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
            for (int i = 0; i < GvCustomerInquiryBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)GvCustomerInquiryBin.Rows[i].FindControl("lblgvInquiryId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)GvCustomerInquiryBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtCustInqiury = (DataTable)Session["dtCInquiryBin"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvCustomerInquiryBin, dtCustInqiury, "", "");
           
            ViewState["Select"] = null;
        }
        AllPageCode();

    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvCustomerInquiryBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvCustomerInquiryBin.Rows.Count; i++)
        {
            ((CheckBox)GvCustomerInquiryBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvCustomerInquiryBin.Rows[i].FindControl("lblgvInquiryId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvCustomerInquiryBin.Rows[i].FindControl("lblgvInquiryId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvCustomerInquiryBin.Rows[i].FindControl("lblgvInquiryId"))).Text.Trim().ToString())
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
        Label lb = (Label)GvCustomerInquiryBin.Rows[index].FindControl("lblgvInquiryId");
        if (((CheckBox)GvCustomerInquiryBin.Rows[index].FindControl("chkSelect")).Checked)
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
        string s = ObjDoc.GetDocumentNo(true, StrCompId, true, "13", "232", "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
        return s;
    }


    protected void ddlUser_Click(object sender, EventArgs e)
    {
        
        FillGrid();
        FillBinGrid();
    }




}
