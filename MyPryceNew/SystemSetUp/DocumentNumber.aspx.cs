using PegasusDataAccess;
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

public partial class SystemSetUp_DocumentNumber : BasePage
{
    string StrBrandId = string.Empty;
    string StrLocId = string.Empty;
    string strCompId = string.Empty;
    Common cmn = null;
    Doc_ModuleMaster objModule = null;
    Doc_ObjectEntry objectEntry = null;
    SystemParameter objSys = null;
    Set_DocNumber objDocNo = null;
    UserMaster ObjUser = null;
    EmployeeMaster Objemployee = null;
    ES_MailSubject objMailSubject = null;
    PageControlCommon objPageCmn = null;
    DataAccessClass ObjDa = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objModule = new Doc_ModuleMaster(Session["DBConnection"].ToString());
        objectEntry = new Doc_ObjectEntry(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        Objemployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objMailSubject = new ES_MailSubject(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        StrBrandId = Session["BrandId"].ToString();
        StrLocId = Session["LocId"].ToString();
        strCompId = Session["CompId"].ToString();

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "90", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            FillModule();
            FillObject();
            ViewState["DtProduct"] = null;
            gvProductdocument.DataSource = null;
            gvProductdocument.DataBind();

            //txtFromDate.Text = DateTime.Now.ToString(HttpContext.Current.Session["DateFormat"].ToString());
            btnNew_Click(null, null);
        }
        AllPageCode();
    }
    #region DocumentNumber
    protected void Check_Clicked(Object sender, EventArgs e)
    {
        if (chkFinancialYear.Checked == true)
        {
            lblFromMonth.Visible = true;
            ddlFromMonth.Visible = true;
            ddlFromMonth.SelectedValue = "--Select--";
        }
        else if (chkFinancialYear.Checked == false)
        {
            lblFromMonth.Visible = false;
            ddlFromMonth.Visible = false;
            ddlFromMonth.SelectedValue = "--Select--";
        }
    }
    protected void gvDocMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDocMaster.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Doc_Number"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvDocMaster, dt, "", "");
        AllPageCode();
    }
    protected void gvDocMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Doc_Number"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Doc_Number"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvDocMaster, dt, "", "");
        AllPageCode();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = objDocNo.GetDocumentNumberAll(strCompId, Session["BrandId"].ToString(), Session["LocId"].ToString());
        dt = new DataView(dt, "Trans_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {
            editid.Value = e.CommandArgument.ToString();
            if (Convert.ToBoolean(dt.Rows[0]["IsUse"].ToString()))
            {
                pnlDocNew.Visible = false;
                DisplayMessage("Document No is in Use");
                return;
            }

            try
            {
                ddlModuleName.Enabled = false;
                ddlObjectName.Enabled = false;

                ddlModuleName.SelectedValue = dt.Rows[0]["Module_Id"].ToString();
                ddlModuleName_SelectedIndexChanged1(null, null);
                ddlObjectName.SelectedValue = dt.Rows[0]["Object_Id"].ToString();
                txtPrefixName.Text = dt.Rows[0]["Prefix"].ToString();
                txtSuffixName.Text = dt.Rows[0]["Suffix"].ToString();
                String CompanyId = dt.Rows[0]["CompId"].ToString();
                String BrandId = dt.Rows[0]["BrandId"].ToString();
                string LocationId = dt.Rows[0]["LocationId"].ToString();
                string DeptId = dt.Rows[0]["DeptId"].ToString();
                string EmpId = dt.Rows[0]["EmpId"].ToString();
                string Year = dt.Rows[0]["Year"].ToString();
                string Month = dt.Rows[0]["Month"].ToString();
                string Day = dt.Rows[0]["Day"].ToString();

                string strFinancialYearValue = dt.Rows[0]["FinancialYearValue"].ToString();
                string strAutoGenerateNumber = dt.Rows[0]["AutoGenerateNumber"].ToString();
                string strAutoGenerateMonth = dt.Rows[0]["AutoGenerateNumberMonth"].ToString();


                if (CompanyId.Trim() == "True")
                {
                    chkCompanyId.Checked = true;
                }
                else
                {
                    chkCompanyId.Checked = false;
                }
                if (BrandId.Trim() == "True")
                {
                    chkBrandId.Checked = true;
                }
                else
                {
                    chkBrandId.Checked = false;
                }
                if (LocationId.Trim() == "True")
                {
                    chkLocationId.Checked = true;
                }
                else
                {
                    chkLocationId.Checked = false;
                }
                if (DeptId.Trim() == "True")
                {
                    chkDepartmentId.Checked = true;

                }
                else
                {
                    chkDepartmentId.Checked = false;
                }
                if (EmpId.Trim() == "True")
                {
                    chkEmpId.Checked = true;

                }
                else
                {
                    chkEmpId.Checked = false;
                }
                if (Year.Trim() == "True")
                {
                    chkYear.Checked = true;
                }
                else
                {
                    chkYear.Checked = false;
                }
                if (Month.Trim() == "True")
                {
                    chkMonth.Checked = true;
                }
                else
                {
                    chkMonth.Checked = false;
                }
                if (Day.Trim() == "True")
                {
                    chkDay.Checked = true;

                }
                else
                {
                    chkDay.Checked = false;
                }

                //Newly Added On 07-04-2023
                if (ddlObjectName.SelectedItem.Text == "Sales Invoice")
                {
                    chkFinancialYear.Visible = true;
                    chkNumberAutoCalculate.Visible = true;
                    lblFromMonth.Visible = true;
                    ddlFromMonth.Visible = true;
                }
                if (strFinancialYearValue.Trim() == "True")
                {
                    chkFinancialYear.Checked = true;
                    if (chkFinancialYear.Checked == true)
                    {
                        chkFinancialYear.Visible = true;
                        chkNumberAutoCalculate.Visible = true;
                        lblFromMonth.Visible = true;
                        ddlFromMonth.Visible = true;
                        ddlFromMonth.SelectedValue = strAutoGenerateMonth;
                    }
                }
                else
                {
                    chkFinancialYear.Checked = false;
                }
                if (strAutoGenerateNumber.Trim() == "True")
                {
                    chkNumberAutoCalculate.Checked = true;
                }
                else
                {
                    chkNumberAutoCalculate.Checked = false;
                }
            }
            catch
            {

            }

            pnlDocNew.Visible = true;
            //gvDocMaster.Visible = false;

        }



    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        DataTable dt = objDocNo.GetDocumentNumberAll(strCompId, Session["BrandId"].ToString(), Session["LocId"].ToString());
        dt = new DataView(dt, "Trans_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count > 0)
        {

            if (Convert.ToBoolean(dt.Rows[0]["IsUse"].ToString()))
            {
                DisplayMessage("Document number in use,can not be delete");
                return;
            }
        }


        b = objDocNo.DeleteDocumentNumber(strCompId, Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), "0", "0");
        if (b != 0)
        {
            DisplayMessage("Record Deleted");


            FillGrid();
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {

        ddlModuleName.Focus();
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");


        PnlList.Visible = false;
        PnlNewEdit.Visible = true;
        FillGrid();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int b = 0;
        if (ddlModuleName.SelectedIndex == 0)
        {
            DisplayMessage("Select Module Name");
            ddlModuleName.Focus();
            return;
        }
        if (ddlObjectName.SelectedIndex == 0)
        {
            DisplayMessage("Select Object Name");
            ddlObjectName.Focus();
            return;
        }

        if (chkNumberAutoCalculate.Checked == true)
        {
            if (chkFinancialYear.Checked == false)
            {
                DisplayMessage("Select Financial Year");
                ddlObjectName.Focus();
                return;
            }
        }

        string strMonthValue = "0";
        if (chkFinancialYear.Checked == true)
        {
            if (ddlFromMonth.SelectedValue == "--Select--")
            {
                DisplayMessage("Please Select Month");
                ddlFromMonth.Focus();
                return;
            }
            else
            {
                strMonthValue = ddlFromMonth.SelectedValue;
            }
        }
        else
        {
            strMonthValue = "0";
        }

        if (ddlObjectName.SelectedItem.Text == "Product")
        {
            if (gvProductdocument.Rows.Count == 0)
            {
                DisplayMessage("Add at Least one document Number");
                return;
            }
        }

        if (editid.Value == "")
        {
            DataTable dt = objDocNo.GetDocumentNumberAll(strCompId, Session["BrandId"].ToString(), Session["LocId"].ToString());
            dt = new DataView(dt, "Module_Id=" + ddlModuleName.SelectedValue + " and Object_Id=" + ddlObjectName.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count == 0)
            {

                if (ddlObjectName.SelectedItem.Text == "Product")
                {

                    if (ViewState["DtProduct"] != null)
                    {
                        DataTable dtProduct = (DataTable)ViewState["DtProduct"];
                        foreach (DataRow dr in dtProduct.Rows)
                        {
                            b = objDocNo.InsertDocumentNumber(strCompId, Session["BrandId"].ToString(), Session["LocId"].ToString(), ddlModuleName.SelectedValue, ddlObjectName.SelectedValue, dr["Prefix"].ToString(), dr["Suffix"].ToString(), dr["CompId"].ToString(), dr["BrandId"].ToString(), dr["LocationId"].ToString(), false.ToString(), dr["EmpId"].ToString(), dr["Year"].ToString(), dr["Month"].ToString(), dr["Day"].ToString(), "False", dr["ModelId"].ToString(), dr["CategoryId"].ToString(), dr["ManufacturingBrandId"].ToString(), dr["SupplierId"].ToString(), "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), dr["FinancialYearValue"].ToString(), dr["AutoGenerateNumber"].ToString(), dr["AutoGenerateNumberMonth"].ToString(), dr["Colour"].ToString(), dr["Size"].ToString());
                        }
                    }
                }
                else
                {
                    b = objDocNo.InsertDocumentNumber(strCompId, Session["BrandId"].ToString(), Session["LocId"].ToString(), ddlModuleName.SelectedValue, ddlObjectName.SelectedValue, txtPrefixName.Text, txtSuffixName.Text, chkCompanyId.Checked.ToString(), chkBrandId.Checked.ToString(), chkLocationId.Checked.ToString(), chkDepartmentId.Checked.ToString(), chkEmpId.Checked.ToString(), chkYear.Checked.ToString(), chkMonth.Checked.ToString(), chkDay.Checked.ToString(), "False", "", "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), chkFinancialYear.Checked.ToString(), chkNumberAutoCalculate.Checked.ToString(), strMonthValue, "False", "False");
                }
            }
            else
            {

                if (ddlObjectName.SelectedItem.Text == "Product")
                {
                    try
                    {
                        objDocNo.DeleteDocumentNumberbyModuleIdandObjectId(strCompId, Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", ddlModuleName.SelectedValue, ddlObjectName.SelectedValue);
                    }
                    catch
                    {
                    }
                    if (ViewState["DtProduct"] != null)
                    {
                        DataTable dtProduct = (DataTable)ViewState["DtProduct"];
                        foreach (DataRow dr in dtProduct.Rows)
                        {
                            b = objDocNo.InsertDocumentNumber(strCompId, Session["BrandId"].ToString(), Session["LocId"].ToString(), ddlModuleName.SelectedValue, ddlObjectName.SelectedValue, dr["Prefix"].ToString(), dr["Suffix"].ToString(), dr["CompId"].ToString(), dr["BrandId"].ToString(), dr["LocationId"].ToString(), false.ToString(), dr["EmpId"].ToString(), dr["Year"].ToString(), dr["Month"].ToString(), dr["Day"].ToString(), "False", dr["ModelId"].ToString(), dr["CategoryId"].ToString(), dr["ManufacturingBrandId"].ToString(), dr["SupplierId"].ToString(), "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), chkFinancialYear.Checked.ToString(), chkNumberAutoCalculate.Checked.ToString(), strMonthValue, dr["Colour"].ToString(), dr["Size"].ToString());
                        }
                    }
                }


            }
            if (b != 0)
            {
                DisplayMessage("Record Saved", "green");
                ResetDocNo();
                FillGrid();
            }
        }
        else
        {
            b = objDocNo.UpdateDocumentNumber(strCompId, Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, ddlModuleName.SelectedValue, ddlObjectName.SelectedValue, txtPrefixName.Text, txtSuffixName.Text, chkCompanyId.Checked.ToString(), chkBrandId.Checked.ToString(), chkLocationId.Checked.ToString(), chkDepartmentId.Checked.ToString(), chkEmpId.Checked.ToString(), chkYear.Checked.ToString(), chkMonth.Checked.ToString(), chkDay.Checked.ToString(), "", "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), chkFinancialYear.Checked.ToString(), chkNumberAutoCalculate.Checked.ToString(), strMonthValue, "False", "False");

            if (b != 0)
            {
                DisplayMessage("Record Updated", "green");
                ResetDocNo();
                FillGrid();
            }
            else
            {
                DisplayMessage("Record Not Updated");
            }
        }

        gvDocMaster.Visible = true;
        pnlDocNew.Visible = false;
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ResetDocNo();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

        ResetDocNo();
        gvDocMaster.Visible = true;
        pnlDocNew.Visible = false;
        FillGrid();
    }
    public void ResetDocNo()
    {
        txtSuffixName.Text = "";
        chkCompanyId.Checked = false;
        chkBrandId.Checked = false;
        chkLocationId.Checked = false;
        chkEmpId.Checked = false;
        chkMonth.Checked = false;
        chkDay.Checked = false;
        chkYear.Checked = false;
        chkDepartmentId.Checked = false;
        txtPrefixName.Text = "";
        editid.Value = "";
        ddlModuleName.Enabled = true;
        ddlObjectName.Enabled = true;
        chkDepartmentId.Visible = true;
        ChkModelId.Visible = false;
        chkManufacturingbrandId.Visible = false;
        chkCategoryId.Visible = false;
        chkSupplierId.Visible = false;
        btnAdddocProduct.Visible = false;
        ChkModelId.Checked = false;
        chkCategoryId.Checked = false;
        chkManufacturingbrandId.Checked = false;
        chkSupplierId.Checked = false;
        gvProductdocument.DataSource = null;
        gvProductdocument.DataBind();
        ViewState["DtProduct"] = null;


    }
    #endregion

    #region MailSubject

    public void ResetMailSubject()
    {

        txtMailPrefix.Text = "";
        txtMailSuffix.Text = "";
        chkMailEmpId.Checked = false;
        chkMailMonth.Checked = false;
        chkMailDay.Checked = false;
        chkMailYear.Checked = false;
        chkMailContactId.Checked = false;
        chkMailEmpId.Checked = false;
        chkDocNo.Checked = false;
        EditMailSubjectId.Value = "";
        ddlModuleName.Enabled = true;
        ddlObjectName.Enabled = true;
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");


        PnlList.Visible = true;
        PnlNewEdit.Visible = false;
        FillGrid();
    }
    protected void gvMailSubject_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDocMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Doc_Number"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvMailSubject, dt, "", "");
        AllPageCode();
    }
    protected void gvMailSubject_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFEmailSort.Value = HDFEmailSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Doc_Number"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFEmailSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Doc_Number"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvMailSubject, dt, "", "");
        AllPageCode();
    }
    protected void btnMailEdit_Command(object sender, CommandEventArgs e)
    {

        DataTable dt = objMailSubject.GetEmailSubjectAll(strCompId);
        dt = new DataView(dt, "Trans_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();


        if (dt.Rows.Count > 0)
        {
            EditMailSubjectId.Value = e.CommandArgument.ToString();

            try
            {
                ddlModuleName.Enabled = false;
                ddlObjectName.Enabled = false;

                ddlModuleName.SelectedValue = dt.Rows[0]["Module_Id"].ToString();
                ddlModuleName_SelectedIndexChanged1(null, null);
                ddlObjectName.SelectedValue = dt.Rows[0]["Object_Id"].ToString();
                txtMailPrefix.Text = dt.Rows[0]["Prefix"].ToString();
                txtMailSuffix.Text = dt.Rows[0]["Suffix"].ToString();
                string DocNo = dt.Rows[0]["DocNo"].ToString();
                string ContactId = dt.Rows[0]["Contact_Id"].ToString();
                string EmpId = dt.Rows[0]["EmpId"].ToString();
                string Year = dt.Rows[0]["Year"].ToString();
                string Month = dt.Rows[0]["Month"].ToString();
                string Day = dt.Rows[0]["Day"].ToString();


                if (DocNo.Trim() == "True")
                {
                    chkDocNo.Checked = true;
                }
                else
                {
                    chkDocNo.Checked = false;
                }
                if (ContactId.Trim() == "True")
                {
                    chkMailContactId.Checked = true;

                }
                else
                {
                    chkMailContactId.Checked = false;
                }
                if (EmpId.Trim() == "True")
                {
                    chkMailEmpId.Checked = true;

                }
                else
                {
                    chkMailEmpId.Checked = false;
                }
                if (Year.Trim() == "True")
                {
                    chkMailYear.Checked = true;
                }
                else
                {
                    chkMailYear.Checked = false;
                }
                if (Month.Trim() == "True")
                {
                    chkMailMonth.Checked = true;
                }
                else
                {
                    chkMailMonth.Checked = false;
                }
                if (Day.Trim() == "True")
                {
                    chkMailDay.Checked = true;

                }
                else
                {
                    chkMailDay.Checked = false;
                }

            }
            catch
            {

            }

            pnlEmailNew.Visible = true;
            gvMailSubject.Visible = false;

        }



    }
    protected void IbtnMailDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        DataTable dt = objMailSubject.GetEmailSubjectAll(strCompId);
        dt = new DataView(dt, "Trans_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();


        b = objMailSubject.DeleteEmailSubject(strCompId, e.CommandArgument.ToString(), "0", "0");
        if (b != 0)
        {
            DisplayMessage("Record Deleted");


            FillGrid();
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
    }
    protected void btnMailSubjectSave_Click(object sender, EventArgs e)
    {
        int b = 0;
        if (ddlModuleName.SelectedIndex == 0)
        {
            DisplayMessage("Select Module Name");
            ddlModuleName.Focus();
            return;
        }
        if (ddlObjectName.SelectedIndex == 0)
        {
            DisplayMessage("Select Object Name");
            ddlObjectName.Focus();
            return;
        }
        if (EditMailSubjectId.Value == "")
        {
            DataTable dt = objMailSubject.GetEmailSubjectAll(strCompId);
            dt = new DataView(dt, "Module_Id=" + ddlModuleName.SelectedValue + " and Object_Id=" + ddlObjectName.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count == 0)
            {
                b = objMailSubject.InsertEmailSubject(strCompId, ddlModuleName.SelectedValue, ddlObjectName.SelectedValue, txtMailPrefix.Text, txtMailSuffix.Text, chkMailContactId.Checked.ToString(), chkMailEmpId.Checked.ToString(), chkDocNo.Checked.ToString(), chkMailYear.Checked.ToString(), chkMailMonth.Checked.ToString(), chkMailDay.Checked.ToString(), false.ToString(), "", "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            }

            if (b != 0)
            {
                DisplayMessage("Record Saved", "green");
                ResetMailSubject();
                FillGrid();

            }

        }
        else
        {
            b = objMailSubject.UpdateEmailSubject(strCompId, EditMailSubjectId.Value, ddlModuleName.SelectedValue, ddlObjectName.SelectedValue, txtMailPrefix.Text, txtMailSuffix.Text, chkMailContactId.Checked.ToString(), chkMailEmpId.Checked.ToString(), chkDocNo.Checked.ToString(), chkMailYear.Checked.ToString(), chkMailMonth.Checked.ToString(), chkMailDay.Checked.ToString(), "", "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

            if (b != 0)
            {
                DisplayMessage("Record Updated", "green");
                ResetMailSubject();
                FillGrid();
            }
            else
            {
                DisplayMessage("Record Not Updated");
            }
        }

        pnlEmailNew.Visible = false;
        gvMailSubject.Visible = true;
    }
    protected void btnMailSubjectReset_Click(object sender, EventArgs e)
    {
        ResetMailSubject();
    }
    protected void btnMailSubjectCancel_Click(object sender, EventArgs e)
    {
        FillGrid();

        ResetMailSubject();
        pnlEmailNew.Visible = false;
        gvMailSubject.Visible = true;
    }
    #endregion
    #region Comman
    public void AllPageCode()
    {

        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        Common ObjComman = new Common(Session["DBConnection"].ToString());
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("90", (DataTable)Session["ModuleName"]);
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
        Page.Title = objSys.GetSysTitle();
        StrBrandId = Session["BrandId"].ToString();
        StrLocId = Session["LocId"].ToString();
        strCompId = Session["CompId"].ToString();
        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
        if (Session["EmpId"].ToString() == "0")
        {

            btnSave.Visible = true;
            btnMailSubjectSave.Visible = true;
            foreach (GridViewRow Row in gvDocMaster.Rows)
            {
                ((ImageButton)Row.FindControl("btnEdit")).Visible = true;
                ((ImageButton)Row.FindControl("IbtnDelete")).Visible = true;
            }
            foreach (GridViewRow Row in gvMailSubject.Rows)
            {
                ((ImageButton)Row.FindControl("btnEdit")).Visible = true;
                ((ImageButton)Row.FindControl("IbtnDelete")).Visible = true;
            }


        }

        DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "90", Session["CompId"].ToString());

        foreach (DataRow DtRow in dtAllPageCode.Rows)
        {

            if (DtRow["Op_Id"].ToString() == "1")
            {
                btnSave.Visible = true;
                btnMailSubjectSave.Visible = true;
            }
            foreach (GridViewRow Row in gvDocMaster.Rows)
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
            foreach (GridViewRow Row in gvMailSubject.Rows)
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
        }
    }
    public void FillModule()
    {
        DataTable Dt = new DataTable();
        Dt = objModule.GetModuleName();
        if (Dt.Rows.Count > 0)
        {
            string Cod = ObjDa.get_SingleValue("Select product_code from Application_Lic_Main");
            if (Cod != "" && Cod != null && Cod != "@NOTFOUND@")
            {
                string ProductCode = Common.Decrypt(Cod);
                if (ProductCode == "TNA021" || ProductCode == "TNA025" || ProductCode == "TNA0220" || ProductCode == "TNA0250" || ProductCode == "TNA02100")
                {
                    Dt = new DataView(Dt, "Module_Id IN (108,8)", "", DataViewRowState.CurrentRows).ToTable();

                }
                if (ProductCode == "SFPRY1" || ProductCode == "SFPRY2" || ProductCode == "SFPRY3")
                {
                    Dt = new DataView(Dt, "Module_Id IN (11, 12, 13, 8)", "", DataViewRowState.CurrentRows).ToTable();

                }
                if (ProductCode == "SFPRY4" || ProductCode == "SFPRY5")
                {
                    Dt = new DataView(Dt, "Module_Id IN (11, 12, 13, 8)", "", DataViewRowState.CurrentRows).ToTable();

                }
                if (ProductCode == "SFPRY6" || ProductCode == "SFPRY7")
                {
                    Dt = new DataView(Dt, "Module_Id IN (11, 12, 13, 8,16,150)", "", DataViewRowState.CurrentRows).ToTable();

                }
                if (ProductCode == "SFPRY8")
                {
                    Dt = new DataView(Dt, "Module_Id IN (150,160,8)", "", DataViewRowState.CurrentRows).ToTable();

                }
                if (ProductCode == "SFPRY9")
                {
                    Dt = new DataView(Dt, "Module_Id IN (8)", "", DataViewRowState.CurrentRows).ToTable();
                }
            }

            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)ddlModuleName, Dt, "Module_Name", "Module_Id");
        }
        else
        {
            ddlModuleName.Items.Insert(0, "--Select--");
            ddlModuleName.SelectedIndex = 0;
        }
    }
    public void FillObject()
    {
        if (ddlModuleName.SelectedValue != "--Select--" && ddlModuleName.SelectedValue != "0")
        {
            DataTable DtObject = new DataTable();
            DtObject = objectEntry.GetObjectNameByModuleId(ddlModuleName.SelectedValue);
            if (DtObject.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 12-05-2015
                objPageCmn.FillData((object)ddlObjectName, DtObject, "Object_Name", "Object_Id");
            }
            else
            {
                ddlObjectName.Items.Clear();
                ddlObjectName.Items.Insert(0, "--Select--");
                ddlObjectName.SelectedIndex = 0;
            }
        }
        else
        {
            ddlObjectName.Items.Clear();
            ddlObjectName.Items.Insert(0, "--Select--");
            ddlObjectName.SelectedIndex = 0;
        }
    }

    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    public void Reset()
    {
        ddlModuleName.Focus();
        FillModule();
        FillObject();
        txtPrefixName.Text = "";
        txtSuffixName.Text = "";
        chkCompanyId.Checked = false;
        chkBrandId.Checked = false;
        chkLocationId.Checked = false;
        chkEmpId.Checked = false;
        chkMonth.Checked = false;
        chkDay.Checked = false;
        chkYear.Checked = false;
        chkDepartmentId.Checked = false;
        ChkModelId.Checked = false;
        chkCategoryId.Checked = false;
        chkManufacturingbrandId.Checked = false;
        chkSupplierId.Checked = false;

        //  lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        ddlModuleName.Enabled = true;
        ddlObjectName.Enabled = true;
        gvDocMaster.DataSource = null;
        gvDocMaster.DataBind();
        pnlDocNo.Visible = false;

        pnlEmailSubject.Visible = false;
        txtMailPrefix.Text = "";
        txtMailSuffix.Text = "";
        chkMailEmpId.Checked = false;
        chkMailMonth.Checked = false;
        chkMailDay.Checked = false;
        chkMailYear.Checked = false;
        chkMailContactId.Checked = false;
        chkMailEmpId.Checked = false;
        chkDocNo.Checked = false;
        ViewState["DtProduct"] = null;
        gvProductdocument.DataSource = null;
        gvProductdocument.DataBind();
        chkDepartmentId.Visible = true;
        ChkModelId.Visible = false;
        chkManufacturingbrandId.Visible = false;
        chkCategoryId.Visible = false;
        chkSupplierId.Visible = false;

    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        if (ddlModuleName.SelectedIndex == 0)
        {
            DisplayMessage("Select Module Name");
            ddlModuleName.Focus();
            return;
        }
        if (ddlObjectName.SelectedIndex == 0)
        {
            DisplayMessage("Select Object Name");
            ddlObjectName.Focus();
            return;
        }
        FillGrid();
    }
    protected void ddlModuleName_SelectedIndexChanged1(object sender, EventArgs e)
    {
        DataTable dtObject = new DataTable();
        if (ddlModuleName.SelectedIndex == 0)
        {
            DisplayMessage("Select Module Name");
            ddlModuleName.Focus();
            FillObject();
            return;
        }
        FillObject();
    }
    public String SetobjectName(string ObjectId)
    {
        string ObjectName = string.Empty;
        DataTable Dt = objectEntry.GetObjectNameByObjectId(ObjectId);
        ObjectName = Dt.Rows[0]["Object_Name"].ToString();
        return ObjectName;
    }
    public string setModuleName(string ModuleId)
    {
        string ModuleName = string.Empty;
        DataTable Dt = objModule.GetModuleNameByModuleId(ModuleId);
        ModuleName = Dt.Rows[0]["Module_Name"].ToString();
        return ModuleName;
    }
    public void FillGrid()
    {
        if (ddlModuleName.SelectedIndex != 0)
        {
            if (ddlObjectName.SelectedIndex != 0)
            {
                DataTable dt = new DataTable();
                if (PnlNewEdit.Visible == true)
                {
                    pnlDocNo.Visible = true;
                    dt = new DataView(objDocNo.GetDocumentNumberAll(strCompId, Session["BrandId"].ToString(), Session["LocId"].ToString()), "Module_Id='" + ddlModuleName.SelectedValue + "' and Object_Id='" + ddlObjectName.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count == 0)
                    {
                        pnlDocNew.Visible = true;
                        if (ddlObjectName.SelectedItem.Text == "Product")
                        {
                            pnlDocNew.Visible = true;
                            chkDepartmentId.Visible = false;
                            ChkModelId.Visible = true;
                            chkManufacturingbrandId.Visible = true;
                            chkCategoryId.Visible = true;
                            chkSupplierId.Visible = true;
                            btnAdddocProduct.Visible = true;
                            chkColour.Visible = true;
                            chkSize.Visible = true;
                        }
                        else if (ddlObjectName.SelectedItem.Text == "Sales Invoice")
                        {
                            chkFinancialYear.Visible = true;
                            chkNumberAutoCalculate.Visible = true;
                            lblFromMonth.Visible = true;
                            ddlFromMonth.Visible = true;
                        }
                        else
                        {
                            chkDepartmentId.Visible = true;
                            ChkModelId.Visible = false;
                            chkManufacturingbrandId.Visible = false;
                            chkCategoryId.Visible = false;
                            chkSupplierId.Visible = false;
                            chkColour.Visible = false;
                            chkSize.Visible = false;
                            btnAdddocProduct.Visible = false;
                            ViewState["DtProduct"] = null;
                            gvProductdocument.DataSource = null;
                            gvProductdocument.DataBind();
                        }
                    }
                    else
                    {
                        pnlDocNew.Visible = false;

                        if (ddlObjectName.SelectedItem.Text == "Product")
                        {
                            pnlDocNew.Visible = true;
                            chkDepartmentId.Visible = false;
                            ChkModelId.Visible = true;
                            chkManufacturingbrandId.Visible = true;
                            chkCategoryId.Visible = true;
                            chkSupplierId.Visible = true;
                            btnAdddocProduct.Visible = true;

                            chkColour.Visible = true;
                            chkSize.Visible = true;

                            DataTable dtProduct = new DataTable();
                            dtProduct.Columns.Add("Trans_Id", typeof(int));
                            dtProduct.Columns.Add("Prefix");
                            dtProduct.Columns.Add("Suffix");
                            dtProduct.Columns.Add("CompId", typeof(bool));
                            dtProduct.Columns.Add("BrandId", typeof(bool));
                            dtProduct.Columns.Add("LocationId", typeof(bool));
                            dtProduct.Columns.Add("EmpId", typeof(bool));
                            dtProduct.Columns.Add("ModelId", typeof(bool));
                            dtProduct.Columns.Add("ManufacturingBrandId", typeof(bool));
                            dtProduct.Columns.Add("CategoryId", typeof(bool));
                            dtProduct.Columns.Add("SupplierId", typeof(bool));
                            dtProduct.Columns.Add("Day", typeof(bool));
                            dtProduct.Columns.Add("Month", typeof(bool));
                            dtProduct.Columns.Add("Year", typeof(bool));
                            dtProduct.Columns.Add("FinancialYearValue", typeof(bool));
                            dtProduct.Columns.Add("AutoGenerateNumber", typeof(bool));
                            dtProduct.Columns.Add("AutoGenerateNumberMonth", typeof(int));
                            dtProduct.Columns.Add("Colour", typeof(bool));
                            dtProduct.Columns.Add("Size", typeof(bool));

                            foreach (DataRow dr in dt.Rows)
                            {
                                DataRow drNew = dtProduct.NewRow();

                                drNew["Trans_Id"] = dr["Trans_Id"].ToString();
                                drNew["Prefix"] = dr["Prefix"].ToString();
                                drNew["Suffix"] = dr["Suffix"].ToString();
                                drNew["CompId"] = dr["CompId"].ToString();
                                drNew["BrandId"] = dr["BrandId"].ToString();
                                drNew["LocationId"] = dr["LocationId"].ToString();
                                drNew["EmpId"] = dr["EmpId"].ToString();
                                drNew["ModelId"] = dr["Field1"].ToString();
                                drNew["CategoryId"] = dr["Field2"].ToString();
                                drNew["ManufacturingBrandId"] = dr["Field3"].ToString();
                                drNew["SupplierId"] = dr["Field4"].ToString();
                                drNew["Day"] = dr["Day"].ToString();
                                drNew["Month"] = dr["Month"].ToString();
                                drNew["Year"] = dr["Year"].ToString();
                                drNew["FinancialYearValue"] = dr["FinancialYearValue"].ToString();
                                drNew["AutoGenerateNumber"] = dr["AutoGenerateNumber"].ToString();
                                drNew["AutoGenerateNumberMonth"] = dr["AutoGenerateNumberMonth"].ToString();
                                drNew["Colour"] = dr["Colour"].ToString();
                                drNew["Size"] = dr["Size"].ToString();

                                dtProduct.Rows.Add(drNew);
                            }

                            ViewState["DtProduct"] = dtProduct;
                            //Common Function add By Lokesh on 12-05-2015
                            objPageCmn.FillData((object)gvProductdocument, dtProduct, "", "");
                        }
                    }
                }
                if (PnlList.Visible == true)
                {
                    pnlEmailSubject.Visible = true;

                    dt = new DataView(objMailSubject.GetEmailSubjectAll(strCompId), "Module_Id='" + ddlModuleName.SelectedValue + "' and Object_Id='" + ddlObjectName.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count == 0)
                    {
                        pnlEmailNew.Visible = true;
                    }
                    else
                    {
                        pnlEmailNew.Visible = false;
                    }
                }


                dt.Columns.Add("Module_Name");
                dt.Columns.Add("Object_Name");

                //dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataTable Dtmodule = objModule.GetModuleNameByModuleId(dt.Rows[i]["Module_Id"].ToString());
                    DataTable dtObject = objectEntry.GetObjectNameByObjectId(dt.Rows[i]["Object_Id"].ToString());

                    try
                    {
                        dt.Rows[i]["Module_Name"] = Dtmodule.Rows[0]["Module_Name"].ToString();
                        dt.Rows[i]["Object_Name"] = dtObject.Rows[0]["Object_Name"].ToString();

                    }
                    catch
                    {

                    }
                }
                if (PnlNewEdit.Visible == true)
                {
                    if (ddlObjectName.SelectedItem.Text == "Product")
                    {
                        gvDocMaster.DataSource = null;
                        gvDocMaster.DataBind();
                    }
                    else
                    {
                        //Common Function add By Lokesh on 12-05-2015
                        objPageCmn.FillData((object)gvDocMaster, dt, "", "");
                    }
                }
                if (PnlList.Visible == true)
                {
                    //Common Function add By Lokesh on 12-05-2015
                    objPageCmn.FillData((object)gvMailSubject, dt, "", "");
                }
                AllPageCode();
                Session["dtFilter_Doc_Number"] = dt;
                Session["Doc"] = dt;
            }
        }
        //lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

    }

    #endregion


    protected void btnAddNewRecord_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();

        if (ViewState["DtProduct"] == null)
        {
            dt.Columns.Add("Trans_Id", typeof(int));
            dt.Columns.Add("Prefix");
            dt.Columns.Add("Suffix");
            dt.Columns.Add("CompId", typeof(bool));
            dt.Columns.Add("BrandId", typeof(bool));
            dt.Columns.Add("LocationId", typeof(bool));
            dt.Columns.Add("EmpId", typeof(bool));
            dt.Columns.Add("ModelId", typeof(bool));
            dt.Columns.Add("ManufacturingBrandId", typeof(bool));
            dt.Columns.Add("CategoryId", typeof(bool));
            dt.Columns.Add("SupplierId", typeof(bool));
            dt.Columns.Add("Day", typeof(bool));
            dt.Columns.Add("Month", typeof(bool));
            dt.Columns.Add("Year", typeof(bool));
            dt.Columns.Add("FinancialYearValue", typeof(bool));
            dt.Columns.Add("AutoGenerateNumber", typeof(bool));
            dt.Columns.Add("AutoGenerateNumberMonth", typeof(int));
            dt.Columns.Add("Colour", typeof(bool));
            dt.Columns.Add("Size", typeof(bool));

            DataRow dr = dt.NewRow();



            dr["Trans_Id"] = "1";
            dr["Prefix"] = txtPrefixName.Text;
            dr["Suffix"] = txtSuffixName.Text;
            dr["CompId"] = chkCompanyId.Checked;
            dr["BrandId"] = chkBrandId.Checked;
            dr["LocationId"] = chkLocationId.Checked;
            dr["EmpId"] = chkEmpId.Checked;
            dr["ModelId"] = ChkModelId.Checked;
            dr["ManufacturingBrandId"] = chkManufacturingbrandId.Checked;
            dr["CategoryId"] = chkCategoryId.Checked;
            dr["Day"] = chkDay.Checked;
            dr["Month"] = chkMonth.Checked;
            dr["Year"] = chkYear.Checked;
            dr["SupplierId"] = chkSupplierId.Checked;
            dr["FinancialYearValue"] = chkFinancialYear.Checked;
            dr["AutoGenerateNumber"] = chkNumberAutoCalculate.Checked;
            if (ddlFromMonth.SelectedValue == "--Select--")
            {
                dr["AutoGenerateNumberMonth"] = "0";
            }
            else
            {
                dr["AutoGenerateNumberMonth"] = ddlFromMonth.SelectedValue;
            }
            dr["Colour"] = chkColour.Checked;
            dr["Size"] = chkSize.Checked;

            dt.Rows.Add(dr);
        }
        else
        {
            dt = (DataTable)ViewState["DtProduct"];
            DataRow dr = dt.NewRow();

            DataTable dtSerial = dt.Copy();

            dtSerial = new DataView(dtSerial, "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();

            if (dtSerial.Rows.Count > 0)
            {
                dr["Trans_Id"] = (float.Parse(dtSerial.Rows[0]["Trans_Id"].ToString()) + 1).ToString();
            }
            else
            {
                dr["Trans_Id"] = "1";
            }
            dr["Prefix"] = txtPrefixName.Text;
            dr["Suffix"] = txtSuffixName.Text;
            dr["CompId"] = chkCompanyId.Checked;
            dr["BrandId"] = chkBrandId.Checked;
            dr["LocationId"] = chkLocationId.Checked;
            dr["EmpId"] = chkEmpId.Checked;
            dr["ModelId"] = ChkModelId.Checked;
            dr["ManufacturingBrandId"] = chkManufacturingbrandId.Checked;
            dr["CategoryId"] = chkCategoryId.Checked;
            dr["Day"] = chkDay.Checked;
            dr["Month"] = chkMonth.Checked;
            dr["Year"] = chkYear.Checked;
            dr["SupplierId"] = chkSupplierId.Checked;
            dr["FinancialYearValue"] = chkFinancialYear.Checked;
            dr["AutoGenerateNumber"] = chkNumberAutoCalculate.Checked;

            if (ddlFromMonth.SelectedValue == "--Select--")
            {
                dr["AutoGenerateNumberMonth"] = "0";
            }
            else
            {
                dr["AutoGenerateNumberMonth"] = ddlFromMonth.SelectedValue;
            }
            dr["Colour"] = chkColour.Checked;
            dr["Size"] = chkSize.Checked;
            dt.Rows.Add(dr);
        }


        ViewState["DtProduct"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvProductdocument, dt, "", "");
    }
    protected void IbtnDeleteDocument_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataTable();
        if (ViewState["DtProduct"] != null)
        {
            dt = (DataTable)ViewState["DtProduct"];
            dt = new DataView(dt, "Trans_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }

        ViewState["DtProduct"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvProductdocument, dt, "", "");
    }
}


