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
using System.Text;
using System.Configuration;
using PegasusDataAccess;

public partial class EMS_MailMarketing : System.Web.UI.Page
{
  
    SystemParameter objSys = null;
    Ems_ContactMaster ObjContactMaster = null;
    Ems_GroupMaster objGroup = null;
    Set_DocNumber ObjDocumentNo = null;
    Ems_Contact_Group objCG = null;
    Common cmn = null;
    Arc_Directory_Master objDir = null;
    Arc_FileTransaction ObjFile = null;
    UserPermission objUserPermission = null;
    Ems_TemplateMaster objTemplate = null;
    EMS_TemplateMaster_Reference objTemMasterRef = null;
    Set_Approval_Employee objEmpApproval = null;
    Ems_MailMarketing objMailMarket = null;
    Inv_ProductMaster ObjProductMaster = null;
    DataTable DtTemp = new DataTable();
    Inv_ModelMaster ObjInvModelMaster = null;
    Inv_ProductCategoryMaster ObjProductCateMaster = null;
    NotificationMaster Obj_Notifiacation = null;
    DepartmentMaster objdept = null;
    CountryMaster objCountry = null;
    UserDetail objUserDetail = null;
    ES_SendMailHeader Obj_SendMailHeader = null;
    SendMailSms ObjSendMailSms = null;
    UserMaster ObjUserMaster = null;
    ES_EmailMaster_Header objEmailHeader = null;
    DesignationMaster objdesg = null;
    Inv_ProductBrandMaster objProB = null;
    DataAccessClass objDa = null;
    EmployeeMaster objEmployee = null;

    string ContactId = string.Empty;
    string a = string.Empty;

    string StrCompId = string.Empty;
    string strBrandId = string.Empty;
    string StrUserId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
     


        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        StrCompId = Session["CompId"].ToString();
        strBrandId = Session["BrandId"].ToString();
        StrUserId = Session["UserId"].ToString();

        objSys = new SystemParameter(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objGroup = new Ems_GroupMaster(Session["DBConnection"].ToString());
        ObjDocumentNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objCG = new Ems_Contact_Group(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objUserPermission = new UserPermission(Session["DBConnection"].ToString());
        objTemplate = new Ems_TemplateMaster(Session["DBConnection"].ToString());
        objTemMasterRef = new EMS_TemplateMaster_Reference(Session["DBConnection"].ToString());
        objEmpApproval = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objMailMarket = new Ems_MailMarketing(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjInvModelMaster = new Inv_ModelMaster(Session["DBConnection"].ToString());
        ObjProductCateMaster = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());
        Obj_Notifiacation = new NotificationMaster(Session["DBConnection"].ToString());
        objdept = new DepartmentMaster(Session["DBConnection"].ToString());
        objCountry = new CountryMaster(Session["DBConnection"].ToString());
        objUserDetail = new UserDetail(Session["DBConnection"].ToString());
        Obj_SendMailHeader = new ES_SendMailHeader(Session["DBConnection"].ToString());
        ObjSendMailSms = new SendMailSms(Session["DBConnection"].ToString());
        ObjUserMaster = new UserMaster(Session["DBConnection"].ToString());
        objEmailHeader = new ES_EmailMaster_Header(Session["DBConnection"].ToString());
        objdesg = new DesignationMaster(Session["DBConnection"].ToString());
        objProB = new Inv_ProductBrandMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objDir = new Arc_Directory_Master(Session["DBConnection"].ToString());
        ObjFile = new Arc_FileTransaction(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
          
            Session["IsAppproval"] = true;
            Session["Is_View_all_user"] = false;
            Session["IsDownload"] = false;
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../EMS/MailMarketing.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            txtValue.Focus();
           
            FillGridBin();
            FillProductCategory();
            btnList_Click(null, null);
            //BindTree();
            Session["treeNode"] = null;
            Session["dtRadio"] = null;
            multiview.ActiveViewIndex = 0;
            FillUserEmailAccount();
            //FillMailList();
            Session["ChkMailList"] = null;
            Session["dtMailList"] = null;
            Session["hdnFldSelectedValues"] = null;
            FillCountryList();
            FilldesignationList();
            lblstep1selectedRecord.Text = "Total Record :  0";
            lblstep1selectedRecord.Enabled = false;
            FillProductBrand();
            txtEmployeeName.Visible = false;
            lblEmployee.Visible = false;
            if ((bool)Session["Is_View_all_user"] == false)
            {
                ddlFilter.Items.Remove(new ListItem("--Select--", "0"));
                ddlFilter.Items.Remove(new ListItem("All", "All"));

                ddlFilter.SelectedValue = "Personal";
                Chk_SelectAll_Contact.Checked = false;
                ddlFilter_SelectedIndexChanged(null, null);
            }
            else
            {
                Div_Filter.Visible = true;
            }


            Session["ContactFinal"] = null;
            FillGrid();
        }

        Session["NotShow"] = "True";
      


        navTree.Attributes.Add("onclick", "postBackByObject()");
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        btnSaveandsend.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCansave.Value = clsPagePermission.bAdd.ToString().ToLower();
        Session["Is_View_all_user"] = clsPagePermission.bViewAllUserRecord;
        Session["IsAppproval"] = clsPagePermission.bVerify == true ? false : true;
        Session["IsDownload"] = clsPagePermission.bDownload;
        hdnCanDownload.Value = clsPagePermission.bDownload.ToString().ToLower();
    }

    //protected override PageStatePersister PageStatePersister
    //{
    //    get
    //    {
    //        return new SessionPageStatePersister(Page);
    //    }
    //}
    public void FillProductBrand()
    {
        DataTable dt = objProB.GetProductBrandTrueAllData(StrCompId.ToString());
        try
        {
            ddlbrandsearch.DataSource = dt;
            ddlbrandsearch.DataTextField = "Brand_Name";
            ddlbrandsearch.DataValueField = "PBrandId";
            ddlbrandsearch.DataBind();
            ddlbrandsearch.Items.Insert(0, "--Select One--");
            ddlbrandsearch.SelectedIndex = 0;
        }
        catch
        {
            ddlbrandsearch.Items.Insert(0, "--Select One--");
            ddlbrandsearch.SelectedIndex = 0;
        }
    }
  
    public void FillCountryList()
    {
        DataTable dt = objCountry.GetCountryMaster();
        try
        {
            dt = new DataView(dt, "", "Country_Name", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }
        chkCountry.DataSource = dt;
        chkCountry.DataTextField = "Country_Name";
        chkCountry.DataValueField = "Country_Id";
        chkCountry.DataBind();
    }
    public void FilldesignationList()
    {
        DataTable dt = objdesg.GetDesignationMaster();
        try
        {
            dt = new DataView(dt, "Designation<>' '", "Designation asc", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        chkDesignation.DataSource = dt;
        chkDesignation.DataTextField = "Designation";
        chkDesignation.DataValueField = "Designation_Id";
        chkDesignation.DataBind();
    }
    public void FillGrid()
    {
        DataTable dt = objMailMarket.GetRecordHeader("0", "2");

        if (!((bool)Session["Is_View_all_user"]))
        {
            dt = new DataView(dt, "CreatedBy='" + Session["UserId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        if (ddlStatus.SelectedValue.Trim() != "0")
        {
            dt = new DataView(dt, "Field5='" + ddlStatus.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        gvTemplateMaster.DataSource = dt;
        gvTemplateMaster.DataBind();
        gvTemplateMaster.Dispose();
        Session["dtFilter_Mail_Marketing"] = dt;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
       
        dt.Dispose();
    }
    public void FillGridBin()
    {
        //string FileTypeId = "0";

        DataTable dt = new DataTable();
        dt = objMailMarket.GetRecordHeader("0", "3");

        gvTemplateMasterBin.DataSource = dt;
        gvTemplateMasterBin.DataBind();


        Session["dtbinFilter"] = dt;

        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
       

    }
    protected void gvTemplateMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTemplateMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Mail_Marketing"];
        gvTemplateMaster.DataSource = dt;
        gvTemplateMaster.DataBind();
     

    }
    protected void gvTemplateMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Mail_Marketing"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Mail_Marketing"] = dt;
        gvTemplateMaster.DataSource = dt;
        gvTemplateMaster.DataBind();
       
    }
    protected void gvTemplateMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        gvTemplateMasterBin.PageIndex = e.NewPageIndex;
        if (HDFSortbin.Value == "")
            FillGridBin();
        else
        {
            DataTable dt = (DataTable)Session["dtbinFilter"];
            gvTemplateMasterBin.DataSource = dt;
            gvTemplateMasterBin.DataBind();
           
        }
        string temp = string.Empty;
        bool isselcted;

        for (int i = 0; i < gvTemplateMasterBin.Rows.Count; i++)
        {
            Label lblconid = (Label)gvTemplateMasterBin.Rows[i].FindControl("lblFileId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvTemplateMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }

    }
    protected void gvTemplateMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        string FileTypeid = "0";
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objTemplate.GetRecord("0", "", "3");

        //dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        gvTemplateMasterBin.DataSource = dt;
        gvTemplateMasterBin.DataBind();
    

    }
    //protected void gvContact_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
    //    {
    //        CheckBox chkBxSelect = (CheckBox)e.Row.Cells[0].FindControl("chkBxSelect");
    //        CheckBox chkBxHeader = (CheckBox)this.gvContact.HeaderRow.FindControl("chkBxHeader");
    //        HiddenField hdnFldId = (HiddenField)e.Row.Cells[0].FindControl("hdnFldId");
    //        chkBxSelect.Attributes["onclick"] = string.Format("javascript:ChildClick(this,document.getElementById('{0}'),'{1}');", chkBxHeader.ClientID, hdnFldId.Value.Trim());
    //    }
    //}
    //protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    //{
    //    CheckBox chkSelAll = ((CheckBox)gvContact.HeaderRow.FindControl("chkgvSelectAll"));
    //    for (int i = 0; i < gvTemplateMasterBin.Rows.Count; i++)
    //    {
    //        ((CheckBox)gvContact.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
    //        if (chkSelAll.Checked)
    //        {
    //            if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvContact.Rows[i].FindControl("lblFileId"))).Text.Trim().ToString()))
    //            {
    //                lblSelectedRecord.Text += ((Label)(gvContact.Rows[i].FindControl("lblFileId"))).Text.Trim().ToString() + ",";
    //            }
    //        }
    //        else
    //        {
    //            string temp = string.Empty;
    //            string[] split = lblSelectedRecord.Text.Split(',');
    //            foreach (string item in split)
    //            {
    //                if (item != ((Label)(gvContact.Rows[i].FindControl("lblFileId"))).Text.Trim().ToString())
    //                {
    //                    if (item != "")
    //                    {
    //                        temp += item + ",";
    //                    }
    //                }
    //            }
    //            lblSelectedRecord.Text = temp;
    //        }
    //    }
    //}
    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvTemplateMasterBin.Rows[index].FindControl("lblFileId");
        if (((CheckBox)gvTemplateMasterBin.Rows[index].FindControl("chkgvSelect")).Checked)
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

    protected void btnResendSuccessMail_Command(object sender, CommandEventArgs e)
    {
        DataTable dtDetail = objMailMarket.GetRecordDetail(e.CommandArgument.ToString(), "4");
        dtDetail = new DataView(dtDetail, "Field1='True'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtDetail.Rows.Count > 0)
        {
            Session["Page&MailType&Id"] = "EMS" + "," + "Success" + "," + e.CommandArgument.ToString();
            string strCmd = string.Format("window.open('../EmailSystem/SendMail.aspx?Page=EMS','window','height=660,width=1100,scrollbars=Yes');");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);

        }
        else
        {
            DisplayMessage("No Success Mail Contacts Exists for Selected Record");
            return;
        }
    }
    protected void btnFailureMail_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        DataTable dt = objMailMarket.GetRecordDetail(editid.Value.ToString(), "4");
        dt = new DataView(dt, "Field1='False'", "", DataViewRowState.CurrentRows).ToTable(true, "Contact_Id", "Name", "EmailId", "MobileNo");
        //dt.Columns["Field11"].ColumnName = "EmailId";
        // dt.Columns["Field21"].ColumnName = "MobileNo";
        if (dt.Rows.Count > 0)
        {
            ExportTableData(dt, "FailureMailList");
        }
        else
        {
            DisplayMessage("No Failure Mail Contacts Exists for Selected Record");
            return;
        }
    }
    public void ExportTableData(DataTable dtdata, string Mail)
    {
        Response.ClearContent();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", Mail + ".xls"));
        Response.ContentType = "application/ms-excel";
        DataTable dt = dtdata.Copy();
        string str = string.Empty;
        foreach (DataColumn dtcol in dt.Columns)
        {
            Response.Write(str + dtcol.ColumnName);
            str = "\t";
        }
        Response.Write("\n");
        foreach (DataRow dr in dt.Rows)
        {
            str = "";
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                Response.Write(str + Convert.ToString(dr[j]));
                str = "\t";
            }
            Response.Write("\n");
        }
        Response.End();

    }
    public string GetDate(object obj)
    {
        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());
        return Date.ToString(objSys.SetDateFormat());
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        //btnNew_Click(null, null);
        Div_Filter.Visible = false;
        Div_Step.Visible = false;
        Lbl_Tab_New.Text = Resources.Attendance.View;

        try
        {
            editid.Value = e.CommandArgument.ToString();
        }
        catch
        {
            editid.Value = Session["EditId"].ToString();
        }

        DataTable dt = objMailMarket.GetRecordHeader(editid.Value.ToString(), "5");
        //dt = new DataView(dt, "Trans_id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {

            lblemailid.Text = objUserDetail.GetbyTransId(dt.Rows[0]["Field2"].ToString(), StrCompId).Rows[0]["Email"].ToString();
            ddlEmailUser.Visible = false;
            lblemailid.Visible = true;
            txtMailHeader.Text = dt.Rows[0]["MailHeader"].ToString();
            txtdisplayText.Text = dt.Rows[0]["Field3"].ToString();
            lblsentmailcount.Text = dt.Rows[0]["TotalMail"].ToString();
            if (dt.Rows[0]["Field4"].ToString() != "" && dt.Rows[0]["Field4"].ToString() != "0")
            {
                try
                {
                    ddlbrandsearch.SelectedValue = dt.Rows[0]["Field4"].ToString();
                }
                catch
                {
                    ddlbrandsearch.SelectedIndex = 0;
                }
            }
            else
            {
                ddlbrandsearch.SelectedIndex = 0;
            }
            // Session["TemplateId"] = dt.Rows[0]["Template_Id"].ToString();
            //string s =  Session["TemplateId"].ToString();
            Editor1.Content = dt.Rows[0]["Template_Content"].ToString();
            try
            {
                ddlEmailUser.SelectedValue = dt.Rows[0]["Template_Content"].ToString();
            }
            catch
            {
            }
            Editor1.Enabled = false;

            lblDate.Text = "Date : " + dt.Rows[0]["ModifiedDate"].ToString();
            lblDate.Visible = true;
            //DataTable dtCG =  objTemplate.GetRecord(s, "", "4");
            //dtlistTemplate.DataSource = dtCG;
            //dtlistTemplate.DataBind();

            //foreach (DataListItem item in dtlistTemplate.Items)
            //{
            //    if (((HiddenField)item.FindControl("hdnrbtnvalue")).Value == s)
            //    {
            //        ((RadioButton)item.FindControl("rbtnTemplate")).Checked = true;

            //    }

            //}


            //   Session["TemplateId"] = dt.Rows[0]["Trans_Id"].ToString();
            //  Session["TemplateId"] = dt.Rows[0]["Template_Id"].ToString();

        }

        DataTable dtDetail = objMailMarket.GetRecordDetail(editid.Value.ToString(), "4");
        if (dtDetail.Rows.Count > 0)
        {
            Session["dtEdit"] = dtDetail;
            gvEditContacts.DataSource = dtDetail;
            gvEditContacts.DataBind();
            gvEditContacts.Visible = true;

        }
        multiview.ActiveViewIndex = 5;
        //ltrstep.Visible = false;
        imgstep.Visible = false;
        btnSave.Visible = false;
        btnBack.Visible = false;
        btnReset.Visible = false;
        btnSaveandsend.Visible = false;



        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        GridViewRow Gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;

        if (((Label)Gvrow.FindControl("lblStatus")).Text.Trim() == "Pending")
        {
            DisplayMessage("Approval is Pending , you can not delete");
            return;
        }

        int b = 0;
        b = objMailMarket.CRUDHeaderRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), "0", "", "", "", "", "", "", "", "", false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "3");
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
            }
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }
    public void Reset()
    {
        chkShowAll.Checked = false;
        if ((bool)Session["Is_View_all_user"] == false)
        {
            ddlFilter.Items.Remove(new ListItem("--Select--", "0"));
            ddlFilter.Items.Remove(new ListItem("All", "All"));

            ddlFilter.SelectedValue = "Personal";
            Chk_SelectAll_Contact.Checked = false;
            ddlFilter_SelectedIndexChanged(null, null);
        }
        else
        {
            Div_Filter.Visible = true;
        }
        //DataTable dt = ObjUserMaster.GetUserMasterByUserId(Session["UserId"].ToString(), "");
        //if (dt.Rows.Count > 0)
        //{
        //    Session["Mail_Filter_Dt"] = dt;
        //    if (dt.Rows[0]["Field6"].ToString() == "True")
        //    {
        //        ddlFilter.Items.Remove(new ListItem("--Select--", "0"));
        //        ddlFilter.Items.Remove(new ListItem("All", "All"));
        //        ddlFilter.SelectedValue = "Personal";
        //        Chk_SelectAll_Contact.Checked = false;
        //        ddlFilter_SelectedIndexChanged(null, null);
        //    }

        //}
        chkSelectAllCountry.Checked = false;
        chkSelectAllDesignation.Checked = false;
        Chk_SelectAll_Contact.Checked = false;

        Div_Step.Visible = true;
        Session["selectedcountrylist"] = null;
        Session["ChkMailList"] = null;
        //chkDefaultMail.Visible = false;
        Editor1.Content = "";

        //rbn1.SelectedIndex = -1;
        lblSelectedRecord.Text = "";
        Session["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;

        Session["SendMailCount"] = null;
        Session["TemplateId"] = null;
        Session["dtEdit"] = null;
        Session["dtRadio"] = null;
        //BindTree();
        gvEditContacts.DataSource = null;
        gvEditContacts.DataBind();
        dvEditGrid.Visible = false;
        PnlDeptCountry.Visible = false;
        FilterBy.Visible = false;
        GvDepartment.DataSource = null;
        GvDepartment.DataBind();
        GVCountry.DataSource = null;
        GVCountry.DataBind();
        ddlFilterBy.SelectedValue = "--Select--";

        Session["hdnFldSelectedValues"] = "";
        Session["treeNode"] = "";

        Session["NewContact"] = null;
        lblgvEditContactsSelected.Text = "";
        lblSelectedRecord.Text = "";

        FillProductCategory();

        ChkProductChildCategory.Items.Clear();

        txtMailHeader.Text = "";

        Session["dtRadio"] = null;
        //rbn1.DataSource = null;
        Session["NotShow"] = "True";
        dtlistTemplate.DataSource = null;
        dtlistTemplate.DataBind();

        Lbl_Tab_New.Text = Resources.Attendance.New;
        Session["ProductCategory"] = null;
        Session["ContactFinal"] = null;
        gvContactFinal.DataSource = null;
        gvContactFinal.DataBind();
        multiview.ActiveViewIndex = 0;
        imgstep.ImageUrl = "../Images/Step1.png";
        lblstep1selectedRecord.Text = "Total Record :  0";
        lblstep1selectedRecord.Enabled = false;
        FillCountryList();
        FilldesignationList();
        Chk_SelectAll_Contact.Checked = false;
        Chk_SelectAll_Contact_CheckedChanged(null, null);
        //ltrstep.Text = "<span style='font-size: 12pt;font-weight: bold;'>Step 1</Span><span style='font-size: 12pt; color:#ffffff;'> >> Step 2 >> Step 3 >> Step 4 >> Step 5";
        //ltrstep.Visible = true;
        lblsentmailcount.Text = "0";
        imgstep.Visible = true;
        
        Session["Counter"] = null;
        Session["ChkMailList"] = null;

        btnBack.Visible = true;
        btnReset.Visible = true;
        lblDate.Visible = false;
        ddlEmailUser.Visible = true;
        lblemailid.Visible = false;

        txtdisplayText.Text = "";
        FillGrid();
        ddlbrandsearch.SelectedIndex = 0;
        chkInvoiceRecordOnly.Checked = false;
        Chk_SelectAll_Contact.Visible = true;
        Chk_SelectAll_Contact.Checked = false;
        chkInvoiceRecordOnly.Visible = true;
        ddlcategorysearch.SelectedIndex = 0;
        txtProductcode.Text = "";
        txtProductName.Text = "";
        Div_Filter.Visible = true;
        txtModelNo.Text = "";
        chkSelectAllcategory.Checked = false;
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        if (Lbl_Tab_New.Text == "New")
        {
            txtValue.Focus();
            pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

            pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
            pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

            PnlList.Visible = true;
            PnlNewEdit.Visible = false;
            PnlBin.Visible = false;
            multiview.ActiveViewIndex = 0;
            imgstep.ImageUrl = "../Images/Step1.png";
            //ltrstep.Text = "<span style='font-size: 12pt;font-weight: bold;'>Step 1</Span><span style='font-size: 12pt; color:#ffffff;'> >> Step 2 >> Step 3 >> Step 4 >> Step 5";
        }
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        if (Lbl_Tab_New.Text == "New")
        {

            if ((bool)Session["Is_View_all_user"] == false)
            {
                ddlFilter.Items.Remove(new ListItem("--Select--", "0"));
                ddlFilter.Items.Remove(new ListItem("All", "All"));

                ddlFilter.SelectedValue = "Personal";
                Chk_SelectAll_Contact.Checked = false;
                ddlFilter_SelectedIndexChanged(null, null);
            }
            else
            {
                Div_Filter.Visible = true;
            }


            //DataTable dt = ObjUserMaster.GetUserMasterByUserId(Session["UserId"].ToString(), "");
            //if (dt.Rows.Count > 0)
            //{
            //    Session["Mail_Filter_Dt"] = dt;
            //    if (dt.Rows[0]["Field6"].ToString() == "True")
            //    {
            //        ddlFilter.Items.Remove(new ListItem("--Select--", "0"));
            //        ddlFilter.Items.Remove(new ListItem("All", "All"));
            //        ddlFilter.SelectedValue = "Personal";
            //        Chk_SelectAll_Contact.Checked = false;
            //        ddlFilter_SelectedIndexChanged(null, null);
            //    }
            //    else
            //        Div_Filter.Visible = true;
            //}

            pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
            pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
            pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
            PnlList.Visible = false;
            PnlNewEdit.Visible = true;
            PnlBin.Visible = false;
            // ddlTemplateType.Focus();
        }
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        txtbinValue.Focus();
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlNewEdit.Visible = false;
        PnlBin.Visible = true;
        PnlList.Visible = false;
        FillGridBin();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        Session["CHECKED_ITEMS"] = null;
        FillGrid();
        FillGridBin();
        ddlOption.SelectedIndex = 2;
       
        txtValue.Text = "";
        txtValue.Focus();
     
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
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
            }
            DataTable dtCust = (DataTable)Session["dtFilter_Mail_Marketing"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            //Session["dtFilter_Mail_Marketing"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            gvTemplateMaster.DataSource = view.ToTable();
            gvTemplateMaster.DataBind();

          
            btnRefresh.Focus();

        }
        txtValue.Focus();
    }
    protected void btnbinbind_Click(object sender, ImageClickEventArgs e)
    {
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
            DataTable dtCust = (DataTable)Session["dtbinFilter"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            gvTemplateMasterBin.DataSource = view.ToTable();
            gvTemplateMasterBin.DataBind();


          
        }
        txtbinValue.Focus();
    }
    protected void btnbinRefresh_Click(object sender, ImageClickEventArgs e)
    {
        FillGrid();
        FillGridBin();

        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 1;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }
    protected void imgBtnRestore_Click(object sender, ImageClickEventArgs e)
    {
        int b = 0;
        ArrayList userdetail = new ArrayList();
        if (gvTemplateMasterBin.Rows.Count > 0)
        {
            SaveCheckedValuesemplog();
            if (Session["CHECKED_ITEMS"] != null)
            {
                userdetail = (ArrayList)Session["CHECKED_ITEMS"];
            }
        }
        if (userdetail.Count > 0)
        {
            for (int j = 0; j < userdetail.Count; j++)
            {
                if (userdetail[j].ToString() != "")
                {
                    // b = objVacancy.DeleteRecord(lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                    b = objMailMarket.CRUDHeaderRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), userdetail[j].ToString(), "0", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "4");
                }
            }
        }

        if (b != 0)
        {

            FillGrid();
            FillGridBin();
            lblSelectedRecord.Text = "";
            Session["Select"] = null;
            DisplayMessage("Record Activated");
            Session["CHECKED_ITEMS"] = null;
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in gvTemplateMasterBin.Rows)
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
    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvTemplateMasterBin.Rows)
        {
            index = (int)gvTemplateMasterBin.DataKeys[gvrow.RowIndex].Value;
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
            foreach (GridViewRow gvrow in gvTemplateMasterBin.Rows)
            {
                int index = (int)gvTemplateMasterBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {

        DataTable dtUnit = (DataTable)Session["dtbinFilter"];

        if (Session["Select"] == null)
        {
            Session["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Trans_Id"]))
                {
                    lblSelectedRecord.Text += dr["Vacancy_Id"] + ",";
                }
            }
            for (int i = 0; i < gvTemplateMasterBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)gvTemplateMasterBin.Rows[i].FindControl("lblFileId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvTemplateMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtBinFilter"];
            gvTemplateMasterBin.DataSource = dtUnit1;
            gvTemplateMasterBin.DataBind();
            Session["Select"] = null;
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ddlFilter.SelectedIndex = 0;
        lblEmployee.Visible = false;
        txtEmployeeName.Text = string.Empty;
        txtEmployeeName.Visible = false;
        Reset();
        Chk_SelectAll_Contact_CheckedChanged(null, null);
        
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        FillGrid();
        FillGridBin();

        Reset();
        btnList_Click(null, null);
        Div_Filter.Visible = true;

        ddlFilter.Visible = true;
        lblFilter.Visible = true;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
      
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListTemplateName(string prefixText, int count, string contextKey)
    {
        DataTable dt = new DataTable();
        string ddlTemplateType = HttpContext.Current.Session["ddlTemplateType"].ToString();
        Ems_TemplateMaster objTemplate = new Ems_TemplateMaster(HttpContext.Current.Session["DBConnection"].ToString());
        if (ddlTemplateType == "1")
        {
            dt = new DataView(objTemplate.GetRecord("0", "", "2"), "Template_Name like '%" + prefixText.ToString() + "%'", "Template_Name asc", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
        }
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Template_Name"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionProductName(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster ObjProductMaster = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt = ObjProductMaster.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());

        dt = new DataView(dt, "EProductName Like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["EProductName"].ToString();
        }
        return txt;
    }
    //protected void rbn1_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    DtTemp = (DataTable) Session["dtRadio"];
    //    if (DtTemp != null)
    //    {
    //        if (rbn1.SelectedValue != "")
    //        {
    //            DataTable dtTempNew = new DataView(DtTemp, "Trans_Id =" + rbn1.SelectedValue, "", DataViewRowState.CurrentRows).ToTable();

    //            Editor1.Content = dtTempNew.Rows[0]["Template_COntent"].ToString();

    //             Session["TemplateId"] = dtTempNew.Rows[0]["Trans_Id"].ToString();

    //        }
    //        else
    //        {
    //            Editor1.Content = "";
    //        }
    //    }
    //}

    public void CheckParentNodes(TreeNode startNode)
    {

        startNode.Checked = true;

        if (startNode.Parent != null)
        {
            CheckParentNodes(startNode.Parent);
        }
    }
    private void CheckTreeNodeRecursive(TreeNode parent, bool fCheck)
    {
        foreach (TreeNode child in parent.ChildNodes)
        {
            if (child.Checked != fCheck)
            {
                child.Checked = fCheck;
            }

            if (child.ChildNodes.Count > 0)
            {
                CheckTreeNodeRecursive(child, fCheck);
            }
        }
    }
    protected void navTree_SelectedNodeChanged1(object sender, EventArgs e)
    {
        try
        {
            if (navTree.SelectedNode.Checked == true)
            {
                UnSelectChild(navTree.SelectedNode);
            }
            else
            {
                SelectChild(navTree.SelectedNode);
            }
        }
        catch (Exception)
        {
        }
    }
    private void UnSelectChild(TreeNode treeNode)
    {


        DataTable dtGrid = new DataTable();
        int i = 0;
        ArrayList userdetails = new ArrayList();
        treeNode.Checked = false;
        bool b = false;


        string value = treeNode.Value + ",";
        string SelectedNode = Session["treeNode"].ToString();


        int index = SelectedNode.IndexOf(value);

        int count = value.Length;
        try
        {
            SelectedNode = SelectedNode.Remove(index, count);
        }
        catch
        {

        }

        Session["treeNode"] = SelectedNode;

        while (i < treeNode.ChildNodes.Count)
        {
            if (treeNode.ChildNodes[i].Checked == false)
            {
                treeNode.ChildNodes[i].Checked = false;
                UnSelectChild(treeNode.ChildNodes[i]);

            }
            i++;
        }
        int flag = 0;
        foreach (TreeNode n in navTree.Nodes)
        {
            if (n.Checked)
            {
                flag = 1;
            }

        }
        if (flag == 0)
        {
            Session["treeNode"] = "";

        }

        navTree.DataBind();
    }
    public void BindTree()
    {
        string RoleId = string.Empty;
        string moduleids = string.Empty;
        navTree.DataSource = null;
        navTree.DataBind();
        navTree.Nodes.Clear();

        DataTable dtContactGroup = objCG.GetContactGroupTrueAllData();

        DataTable DtGroupMainNode = new DataTable();

        DtGroupMainNode = new DataView(objGroup.GetGroupMasterOnlyMainNode(), "", "Group_Name Asc", DataViewRowState.CurrentRows).ToTable();

        foreach (DataRow datarow in DtGroupMainNode.Rows)
        {
            TreeNode tn = new TreeNode();

            tn = new TreeNode(datarow["Group_Name"].ToString(), datarow["Group_Id"].ToString());

            DataTable dtModuleSaved = new DataView(dtContactGroup, "Group_Id='" + datarow["Group_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            tn.SelectAction = TreeNodeSelectAction.Expand;
            FillChild(dtContactGroup, tn.Value, tn);


            navTree.Nodes.Add(tn);
        }

        navTree.DataBind();
        navTree.CollapseAll();
        return;
    }
    private void FillChild(DataTable dtContactGroup, string index, TreeNode tn)//fill up child nodes and respective child nodes of them 
    {
        DataTable dt = new DataTable();
        dt = new DataView(objGroup.GetGroupMasterByParentId(index), "", "Group_Name Asc", DataViewRowState.CurrentRows).ToTable();


        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn1 = new TreeNode();
            tn1.Text = dt.Rows[i]["Group_Name"].ToString();
            tn1.Value = dt.Rows[i]["Group_Id"].ToString();
            tn1.SelectAction = TreeNodeSelectAction.Expand;
            tn.ChildNodes.Add(tn1);
            foreach (DataRow Dr in dtContactGroup.Rows)
            {
                if (dt.Rows[i]["Group_Id"].ToString() == Dr["Group_Id"].ToString())
                {
                    // tn1.Checked = true;
                }
            }


            FillChild(dtContactGroup, (dt.Rows[i]["Group_Id"].ToString()), tn1);
            i++;
        }
        navTree.DataBind();
    }

    private void FillChild1(DataTable dtContactGroup, string index, TreeNode tn)//fill up child nodes and respective child nodes of them 
    {
        DataTable dt = new DataTable();
        dt = new DataView(dtContactGroup, "Parent_Id=" + index + "", "Group_Name Asc", DataViewRowState.CurrentRows).ToTable();


        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn1 = new TreeNode();
            tn1.Text = dt.Rows[i]["Group_Name"].ToString();
            tn1.Value = dt.Rows[i]["Group_Id"].ToString();
            tn1.SelectAction = TreeNodeSelectAction.Expand;
            tn.ChildNodes.Add(tn1);
            foreach (DataRow Dr in dtContactGroup.Rows)
            {
                if (dt.Rows[i]["Group_Id"].ToString() == Dr["Group_Id"].ToString())
                {
                    // tn1.Checked = true;
                }
            }


            FillChild1(dtContactGroup, (dt.Rows[i]["Group_Id"].ToString()), tn1);
            i++;
        }
        navTree.DataBind();
    }

    protected void chkgvEditContactSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvEditContacts.HeaderRow.FindControl("chkgvEditContactSelectAll"));
        for (int i = 0; i < gvEditContacts.Rows.Count; i++)
        {
            ((CheckBox)gvEditContacts.Rows[i].FindControl("chkgvEditSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblgvEditContactsSelected.Text.Split(',').Contains(((Label)(gvEditContacts.Rows[i].FindControl("lblgvEditContactId"))).Text.Trim().ToString()))
                {
                    lblgvEditContactsSelected.Text += ((Label)(gvEditContacts.Rows[i].FindControl("lblgvEditContactId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblgvEditContactsSelected.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvEditContacts.Rows[i].FindControl("lblgvEditContactId"))).Text.Trim().ToString())
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
                lblgvEditContactsSelected.Text = temp;
            }
        }
    }
    protected void ImgBtngvEditContDelete_Command(object sender, CommandEventArgs e)
    {

        for (int i = 0; i < gvEditContacts.Rows.Count; i++)
        {
            if (((CheckBox)gvEditContacts.Rows[i].FindControl("chkgvEditSelect")).Checked)
            {
                if (!lblgvEditContactsSelected.Text.Split(',').Contains(((Label)(gvEditContacts.Rows[i].FindControl("lblgvEditContactId"))).Text.Trim().ToString()))
                {
                    lblgvEditContactsSelected.Text += ((Label)(gvEditContacts.Rows[i].FindControl("lblgvEditContactId"))).Text.Trim().ToString() + ",";
                }
            }
        }

        if (lblgvEditContactsSelected.Text != "")
        {
            DataTable dtDetail = (DataTable)Session["dtEdit"];
            for (int j = 0; j < lblgvEditContactsSelected.Text.Split(',').Length; j++)
            {
                if (lblgvEditContactsSelected.Text.Split(',')[j] != "")
                {
                    dtDetail = new DataView(dtDetail, "Contact_Id <>'" + lblgvEditContactsSelected.Text.Split(',')[j].Trim().ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
            Session["dtEdit"] = dtDetail;
            gvEditContacts.DataSource = dtDetail;
            gvEditContacts.DataBind();

            if (Session["treeNode"] != null && Session["treeNode"] != "")
            {
                DataTable dtGrid = objMailMarket.GetEmsContactList(Session["treeNode"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString());
                Session["NewContact"] = dtGrid;
                foreach (DataRow row1 in dtGrid.Rows)
                {
                    foreach (DataRow row2 in dtDetail.Rows)
                    {
                        if (row1["Contact_Id"].ToString() == row2["Contact_Id"].ToString())
                        {
                            //dt3.ImportRow(row2);
                            Session["NewContact"] = new DataView((DataTable)(Session["NewContact"]), "Contact_Id<>'" + row1["Contact_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        }
                    }
                }
                DataTable dt = (DataTable)Session["NewContact"];

            }
            lblgvEditContactsSelected.Text = "";


        }
        else
        {
            DisplayMessage("First Select Contact");
            gvEditContacts.Focus();
            return;
        }
    }
    protected void ddlFilterBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlFilterBy.SelectedValue == "--Select--")
        {
            GvDepartment.DataSource = null;
            GvDepartment.DataBind();
            GVCountry.DataSource = null;
            GVCountry.DataBind();
        }
        else if (ddlFilterBy.SelectedValue == "Dept")
        {
            DataTable dtDept = new DataTable();
            dtDept = objdept.GetDepartmentMaster();
            dtDept = new DataView(dtDept, "", "Dep_Name Asc", DataViewRowState.CurrentRows).ToTable();
            if (dtDept.Rows.Count > 0)
            {
                GvDepartment.DataSource = dtDept;
                GvDepartment.DataBind();
                GVCountry.DataSource = null;
                GVCountry.DataBind();
            }

            else
            {
                GvDepartment.DataSource = null;
                GvDepartment.DataBind();
            }
        }
        else if (ddlFilterBy.SelectedValue == "Country")
        {
            DataTable dtCountry = new DataTable();
            dtCountry = objCountry.GetCountryMaster();
            if (dtCountry.Rows.Count > 0)
            {
                GVCountry.DataSource = dtCountry;
                GVCountry.DataBind();
                GvDepartment.DataSource = null;
                GvDepartment.DataBind();
            }
            else
            {
                GVCountry.DataSource = null;
                GVCountry.DataBind();
            }
        }

        DataTable dtOldData = (DataTable)Session["NewContact"];


        Session["hdnFldSelectedValues"] = "";

    }
    protected void chkDeptBxSelect_CheckedChanged(object sender, EventArgs e)
    {
        string strDepartmentId = string.Empty;
        foreach (GridViewRow gvr in GvDepartment.Rows)
        {
            CheckBox chkDept = (CheckBox)gvr.FindControl("chkDeptBxSelect");

            if (chkDept.Checked == true)
            {
                if (strDepartmentId == "")
                {
                    strDepartmentId = ((HiddenField)gvr.FindControl("hdnFDeptId")).Value.ToString();
                }
                else
                {
                    strDepartmentId = strDepartmentId.Trim() + "," + ((HiddenField)gvr.FindControl("hdnFDeptId")).Value.ToString();
                }
            }
        }

        DataTable dtDept = (DataTable)Session["NewContact"];
        if (strDepartmentId != "")
        {
            dtDept = new DataView(dtDept, "Dep_Id in (" + strDepartmentId + ")", "", DataViewRowState.CurrentRows).ToTable();
        }


        Session["hdnFldSelectedValues"] = "";

    }
    protected void chkDeptBxHeader_CheckedChanged(object sender, EventArgs e)
    {
        string strDepartmentId = string.Empty;
        CheckBox chkDHeader = ((CheckBox)GvDepartment.HeaderRow.FindControl("chkDeptBxHeader"));
        foreach (GridViewRow gvrD in GvDepartment.Rows)
        {
            if (chkDHeader.Checked == true)
            {
                ((CheckBox)gvrD.FindControl("chkDeptBxSelect")).Checked = true;
                if (strDepartmentId == "")
                {
                    strDepartmentId = ((HiddenField)gvrD.FindControl("hdnFDeptId")).Value.ToString();
                }
                else
                {
                    strDepartmentId = strDepartmentId.Trim() + "," + ((HiddenField)gvrD.FindControl("hdnFDeptId")).Value.ToString();
                }
            }
            else
            {
                ((CheckBox)gvrD.FindControl("chkDeptBxSelect")).Checked = false;
                strDepartmentId = "";
            }
        }

        DataTable dtDept = (DataTable)Session["NewContact"];
        if (strDepartmentId != "")
        {
            dtDept = new DataView(dtDept, "Dep_Id in (" + strDepartmentId + ")", "", DataViewRowState.CurrentRows).ToTable();
        }


        Session["hdnFldSelectedValues"] = "";

    }
    protected void chkBxCountrySelect_CheckedChanged(object sender, EventArgs e)
    {
        string strCountryId = string.Empty;
        foreach (GridViewRow gvrC in GVCountry.Rows)
        {
            CheckBox chkCountry = (CheckBox)gvrC.FindControl("chkBxCountrySelect");

            if (chkCountry.Checked == true)
            {
                if (strCountryId == "")
                {
                    strCountryId = ((HiddenField)gvrC.FindControl("hdnFCountryId")).Value.ToString();
                }
                else
                {
                    strCountryId = strCountryId.Trim() + "," + ((HiddenField)gvrC.FindControl("hdnFCountryId")).Value.ToString();
                }
            }
        }

        DataTable dtCountry = (DataTable)Session["NewContact"];

        if (strCountryId != "")
        {
            dtCountry = new DataView(dtCountry, "CountryId in (" + strCountryId + ")", "", DataViewRowState.CurrentRows).ToTable();
        }


        Session["hdnFldSelectedValues"] = "";

    }
    protected void chkCountryBxHeader_CheckedChanged(object sender, EventArgs e)
    {
        string strCountryId = string.Empty;
        CheckBox chkHeader = ((CheckBox)GVCountry.HeaderRow.FindControl("chkCountryBxHeader"));
        foreach (GridViewRow gvrC in GVCountry.Rows)
        {
            if (chkHeader.Checked == true)
            {
                ((CheckBox)gvrC.FindControl("chkBxCountrySelect")).Checked = true;
                if (strCountryId == "")
                {
                    strCountryId = ((HiddenField)gvrC.FindControl("hdnFCountryId")).Value.ToString();
                }
                else
                {
                    strCountryId = strCountryId.Trim() + "," + ((HiddenField)gvrC.FindControl("hdnFCountryId")).Value.ToString();
                }
            }
            else
            {
                ((CheckBox)gvrC.FindControl("chkBxCountrySelect")).Checked = false;
                strCountryId = "";
            }
        }

        DataTable dtCountry = (DataTable)Session["NewContact"];

        if (strCountryId != "")
        {
            dtCountry = new DataView(dtCountry, "CountryId in (" + strCountryId + ")", "", DataViewRowState.CurrentRows).ToTable();
        }


    }
    #region Contact


    #endregion
    #region Product

    #endregion
    public string getImageUrl(string ImageName)
    {
        string url = string.Empty;
        if (File.Exists(Server.MapPath("../CompanyResource/Template/" + ImageName)) == true)
        {
            url = "../CompanyResource/Template/" + ImageName;
        }
        else
        {
            url = "../Bootstrap_Files/dist/img/NoImage.jpg";
        }

        //string url = string.Empty;
        //if (ImageName != "")
        //{
        //    url = "~/CompanyResource/Template/" + ImageName;
        //}
        //else
        //{
        //    url = "~/CompanyResource/Template/NoImage.jpg";

        //}

        return url;
    }

    public string GetImage(object obj)
    {
        string url = string.Empty;
        if (File.Exists(Server.MapPath("../CompanyResource/Template/" + obj.ToString())) == true)
        {
            url = "../CompanyResource/Template/" + obj.ToString();
        }
        else
        {
            url = "../Bootstrap_Files/dist/img/NoImage.jpg";
        }
        return url;
    }

    protected void rbtnTemplate_OnCheckedChanged(object sender, EventArgs e)
    {

        DataListItem li = (DataListItem)((RadioButton)sender).Parent;

        string transid = ((HiddenField)li.FindControl("hdnrbtnvalue")).Value;


        DtTemp = (DataTable)Session["dtRadio"];
        if (DtTemp != null)
        {

            DataTable dtTempNew = new DataView(DtTemp, "Trans_Id =" + transid, "", DataViewRowState.CurrentRows).ToTable();

            Editor1.Content = dtTempNew.Rows[0]["Template_COntent"].ToString();

            if (dtTempNew.Rows[0]["Field3"].ToString().Trim() != "" && dtTempNew.Rows[0]["Field3"].ToString().Trim() != "0")
            {
                ddlbrandsearch.SelectedValue = dtTempNew.Rows[0]["Field3"].ToString();
            }
            else
            {
                ddlbrandsearch.SelectedIndex = 0;
            }

            Session["TemplateId"] = dtTempNew.Rows[0]["Trans_Id"].ToString();


        }

        foreach (DataListItem item in dtlistTemplate.Items)
        {
            if (((HiddenField)item.FindControl("hdnrbtnvalue")).Value != transid)
            {
                ((RadioButton)item.FindControl("rbtnTemplate")).Checked = false;

            }
            else
            {
                ((RadioButton)item.FindControl("rbtnTemplate")).Checked = true;
            }
        }
    }
    protected void imgurlTemplate_OnClick(object sender, EventArgs e)
    {

        DataListItem li = (DataListItem)((ImageButton)sender).Parent;

        string transid = ((HiddenField)li.FindControl("hdnrbtnvalue")).Value;


        DtTemp = (DataTable)Session["dtRadio"];
        if (DtTemp != null)
        {

            DataTable dtTempNew = new DataView(DtTemp, "Trans_Id =" + transid, "", DataViewRowState.CurrentRows).ToTable();

            Editor1.Content = dtTempNew.Rows[0]["Template_COntent"].ToString();

            Session["TemplateId"] = dtTempNew.Rows[0]["Trans_Id"].ToString();

        }

        foreach (DataListItem item in dtlistTemplate.Items)
        {
            if (((HiddenField)item.FindControl("hdnrbtnvalue")).Value != transid)
            {
                ((RadioButton)item.FindControl("rbtnTemplate")).Checked = false;

            }
            else
            {
                ((RadioButton)item.FindControl("rbtnTemplate")).Checked = true;
            }

        }
    }
    #region Multiview
    #region viewcontact
    protected void BtnNextContactList_Click(object sender, EventArgs e)
    {
        int contactCounter = 0;
        int counter = 0;
        if (ddlEmailUser != null)
        {
            if (ddlEmailUser.Items.Count == 0)
            {
                DisplayMessage("First Configure Your Mail account");
                return;
            }

        }
        else
        {
            DisplayMessage("First Configure Your Mail account");
            return;
        }



        DataTable dtContact = (DataTable)Session["NewContact"];
        Session["ContactFinal"] = (DataTable)Session["NewContact"];

        if (dtContact != null)
        {
            contactCounter = dtContact.Rows.Count;
        }


        txtEmployeeName.Visible = false;
        lblEmployee.Visible = false;
        Div_Filter.Visible = false;
        ddlFilter.Visible = true;
        lblFilter.Visible = true;
        chkInvoiceRecordOnly.Visible = false;
        lstEmailSelect.Items.Clear();
        lstEmail.Items.Clear();

        string countryidlist = string.Empty;
        string designationIDlist = string.Empty;
        foreach (ListItem li in chkCountry.Items)
        {
            if (li.Selected)
            {
                if (countryidlist == "")
                {

                    countryidlist = "'" + li.Value + "'";
                }
                else
                {

                    countryidlist = countryidlist + "," + "'" + li.Value + "'";

                }

            }
        }

        foreach (ListItem li in chkDesignation.Items)
        {
            if (li.Selected)
            {
                if (designationIDlist == "")
                {

                    designationIDlist = "'" + li.Value + "'";
                }
                else
                {

                    designationIDlist = designationIDlist + "," + "'" + li.Value + "'";
                }
            }
        }

        if (countryidlist != "")
        {


            try
            {
                dtContact = new DataView(dtContact, "Country_Id in (" + countryidlist.Trim() + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }

        }

        if (designationIDlist != "")
        {

            try
            {
                dtContact = new DataView(dtContact, "Designation_Id in (" + designationIDlist + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }

        }



        //if (chkInvoiceRecordOnly.Checked)
        //{

        //    if (ddlcategorysearch.SelectedIndex > 0)
        //    {
        //        try
        //        {
        //            dtContact = new DataView(dtContact, "CategoryId=" + ddlcategorysearch.SelectedValue.Trim() + "", "", DataViewRowState.CurrentRows).ToTable();
        //        }
        //        catch
        //        {
        //        }
        //    }

        //    if (txtProductcode.Text.Trim() != "")
        //    {
        //        try
        //        {
        //            dtContact = new DataView(dtContact, "Product_Id =" + hdnproductId.Value + "", "", DataViewRowState.CurrentRows).ToTable();
        //        }
        //        catch
        //        {
        //        }
        //    }
        //}


        if (dtContact == null)
        {
            counter = 1;
        }
        else
        {
            if (dtContact.Rows.Count == 0)
            {

                counter = 1;

            }
        }

        Session["hdnFldSelectedValues"] = "";
        Double Counter = 0;
        if (dtContact != null)
        {
            foreach (DataRow dr in dtContact.Rows)
            {


                if (!Session["hdnFldSelectedValues"].ToString().Split(',').Contains(dr["Contact_Id"].ToString()))
                {
                    Session["hdnFldSelectedValues"] += dr["Contact_Id"].ToString() + ",";
                }

                if (dr["Field1"].ToString().Trim() != "")
                {
                    if (lstEmailSelect.Items.FindByText(dr["Field1"].ToString().Trim()) == null)
                    {
                        lstEmailSelect.Items.Add(dr["Field1"].ToString().Trim());
                        Counter++;
                    }
                }

            }
        }

        Session["SendMailCount"] = Counter;

        multiview.ActiveViewIndex = 2;
        imgstep.ImageUrl = "../Images/Step3.png";
        lblFinalEmailListrecord.Text = "Total Email : " + Counter.ToString();
        lblFinalEmailListrecord_Contact.Text = "Total Contact : " + contactCounter.ToString();
    }
    protected void BtnResetContactList_Click(object sender, EventArgs e)
    {

        ddlFilter.SelectedIndex = 0;
        lblEmployee.Visible = false;
        txtEmployeeName.Text = string.Empty;
        txtEmployeeName.Visible = false;
        Reset();
        Chk_SelectAll_Contact_CheckedChanged(null, null);
        Div_Filter.Visible = true;

        ddlFilter.Visible = true;
        lblFilter.Visible = true;

        //navTree.DataSource = null;
        //navTree.DataBind();
        //navTree.Nodes.Clear();
        //Reset();
    }
    protected void BtnCancelContactList_Click(object sender, EventArgs e)
    {
        btnCancel_Click(null, null);
    }
    #endregion
    #region FinalContactList
    public void TotalSelectedRecord()
    {
        Double TotalRecord = 0;
        foreach (GridViewRow gvr in gvContactFinal.Rows)
        {
            if (((CheckBox)gvr.FindControl("chkBxSelect")).Checked)
            {
                TotalRecord++;
            }

        }
        lbltotalselectedRecord.Text = "Total Selected : " + TotalRecord.ToString();
    }
    protected void btnbindContactList_Click(object sender, ImageClickEventArgs e)
    {

        if (ddlOptionContactFinal.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOptionContactFinal.SelectedIndex == 1)
            {
                condition = ddlFieldNameContactFinal.SelectedValue + "='" + txtValueContactFinal.Text.Trim() + "'";
            }
            else if (ddlOptionContactFinal.SelectedIndex == 2)
            {
                condition = ddlFieldNameContactFinal.SelectedValue + " Like '%" + txtValueContactFinal.Text.Trim() + "%'";
            }
            else
            {
                condition = ddlFieldNameContactFinal.SelectedValue + " like '" + txtValueContactFinal.Text.Trim() + "%'";
            }
            DataTable dtContact = null;
            dtContact = (DataTable)Session["ContactFinal"];

            if (dtContact != null)
            {
                dtContact = new DataView(dtContact, condition, "", DataViewRowState.CurrentRows).ToTable();

                gvContactFinal.DataSource = dtContact;
                gvContactFinal.DataBind();

                lblTotalSelectedRecordContactFinal.Text = Resources.Attendance.Total_Records + " : " + dtContact.Rows.Count + "";
                Session["ContactFinal"] = dtContact;
                //view.ToTable();
            }
        }
        
    }
    protected void btnRefreshContactList_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtContact = (DataTable)Session["NewContact"];
        Session["ContactFinal"] = dtContact;
        gvContactFinal.DataSource = dtContact;
        gvContactFinal.DataBind();
        lblTotalSelectedRecordContactFinal.Text = Resources.Attendance.Total_Records + " : " + dtContact.Rows.Count + "";
        txtValueContactFinal.Text = "";
        ddlOptionContactFinal.SelectedIndex = 2;
        ddlFieldNameContactFinal.SelectedIndex = 0;

       
    }
    protected void chkBxHeader_OnCheckedChanged(object sender, EventArgs e)
    {
        
    }
    protected void chkBxSelect_OnCheckedChanged(object sender, EventArgs e)
    {
    }
    protected void BtnNextContactListFinal_Click(object sender, EventArgs e)
    {
        Double Counter = 0;
        Session["hdnFldSelectedValues"] = "";

        //if (Session["hdnFldSelectedValues"] == "" && Session["ChkMailList"] == null)
        //{
        //    DisplayMessage("Select at Least One Contact");
        //    return;
        //}
        //if (Session["hdnFldSelectedValues"] == "" && ((ArrayList)Session["ChkMailList"]).Count == 0)
        //{
        //    DisplayMessage("Select at Least One Contact");
        //    return;
        //}


        if (Session["Counter"] != null)
        {
            Counter = (Double)Session["Counter"];
        }

        lstEmail.Items.Clear();
        lstEmailSelect.Items.Clear();
        txtEmailIdSearch.Text = "";

        foreach (GridViewRow gvr in gvContactFinal.Rows)
        {
            if (((CheckBox)gvr.FindControl("chkBxSelect")).Checked)
            {
                Counter++;
                if (!Session["hdnFldSelectedValues"].ToString().Split(',').Contains(((Label)gvr.FindControl("hdnFldId")).Text.ToString()))
                {
                    Session["hdnFldSelectedValues"] += ((Label)gvr.FindControl("hdnFldId")).Text.ToString() + ",";
                }

                if (((Label)gvr.FindControl("lblTName")).Text.Trim() != "")
                {
                    if (lstEmailSelect.Items.FindByText(((Label)gvr.FindControl("lblTName")).Text.Trim()) == null)
                    {
                        lstEmailSelect.Items.Add(((Label)gvr.FindControl("lblTName")).Text.Trim());
                    }
                }
            }
        }


        if (Counter == 0)
        {
            DisplayMessage("Select at Least One Contact");
            return;
        }


        lblFinalEmailListrecord.Text = "Total Email : " + lstEmailSelect.Items.Count.ToString();

        Session["SendMailCount"] = Counter;
        multiview.ActiveViewIndex = 2;
        imgstep.ImageUrl = "../Images/Step3.png";

        // ltrstep.Text = "<span style='font-size: 12pt; color:#ffffff;'>Step 1 >> Step 2 >> </Span><span style='font-size: 12pt;font-weight: bold;'>Step 3</Span><span style='font-size: 12pt; color:#ffffff;'> >> Step 4 >> Step 5";

      
    }
    protected void BtnBackContactListFinal1_Click(object sender, EventArgs e)
    {
        ddlFilter.Visible = true;
        lblFilter.Visible = true;

        if (ddlFilter.SelectedValue == "Other")
        {
            txtEmployeeName.Visible = true;
            lblEmployee.Visible = true;
        }

        multiview.ActiveViewIndex = 0;
        imgstep.ImageUrl = "../Images/Step1.png";
        //ltrstep.Text = "<span style='font-size: 12pt;font-weight: bold;'>Step 1</Span><span style='font-size: 12pt; color:#ffffff;'> >> Step 2 >> Step 3 >> Step 4 >> Step 5";

    }
    protected void BtnResetContactListFinal_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void BtnCancelContactListFinal_Click(object sender, EventArgs e)
    {
        btnCancel_Click(null, null);
    }
    #endregion
    #region EmailListRegion
    public void FillMailList()
    {
        //DataTable Dtallmail = new DataTable();



        //    Dtallmail = objEmailHeader.Get_Emailall_Notexistincontact();


        ////if (chkIncludefilter.Checked)
        ////{
        //    try
        //    {
        //        if ( Session["selectedcountrylist"] != null)
        //        {

        //            Dtallmail = new DataView(Dtallmail, "Field1 in (" +  Session["selectedcountrylist"].ToString() + ")", "", DataViewRowState.CurrentRows).ToTable();
        //        }
        //    }
        //    catch
        //    {
        //    }
        ////}
        //chkDefaultMailList.DataSource = Dtallmail;
        //chkDefaultMailList.DataTextField = "Email_Id";
        //chkDefaultMailList.DataValueField = "Trans_Id";
        //chkDefaultMailList.DataBind();
        //Session["dtMailList"] = Dtallmail;
        //chkDefaultMailList.Dispose();
        //Dtallmail.Dispose();

    }


    #endregion


    #region FinalEmaillist



    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCountryName(string prefixText, int count, string contextKey)
    {
        CountryMaster objCountryMaster = new CountryMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objCountryMaster.GetCountryMaster(), "Country_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Country_Name"].ToString();
        }
        return txt;
    }


    protected void imgsearchEmailId_OnClick(object sender, EventArgs e)
    {
        DataTable Dtallmail = new DataTable();

        lstEmail.Items.Clear();
        if (txtEmailIdSearch.Text != "")
        {
            if (ddlFieldnameEmailFilter.SelectedIndex == 0)
            {
                Dtallmail = objEmailHeader.Get_EmailbyLikeStatement(txtEmailIdSearch.Text.Trim());
            }
            else
            {
                Dtallmail = objEmailHeader.Get_EmailbyCountryName(txtEmailIdSearch.Text.Trim());
            }

            lstEmail.DataSource = Dtallmail;
            lstEmail.DataTextField = "Trans_Id";
            lstEmail.DataTextField = "Email_Id";
            lstEmail.DataBind();
        }
    }


    protected void ImgRefreshEmailid_OnClick(object sender, EventArgs e)
    {
        txtEmailIdSearch.Text = "";
        lstEmail.Items.Clear();
        txtEmailIdSearch.Focus();
        ddlFieldnameEmailFilter.SelectedIndex = 0;
    }


    protected void btnPushDept_Click(object sender, EventArgs e)
    {
        int duplicateCounter = 0;

        if (lstEmail.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstEmail.Items)
            {
                if (li.Selected)
                {
                    if (lstEmailSelect.Items.FindByText(li.Text) == null)
                    {
                        lstEmailSelect.Items.Add(li);
                    }
                    else
                    {
                        duplicateCounter = 1;
                    }

                }
            }
            foreach (ListItem li in lstEmailSelect.Items)
            {
                lstEmail.Items.Remove(li);
            }
            lstEmailSelect.SelectedIndex = -1;
        }
        btnPushDept.Focus();

        lblFinalEmailListrecord.Text = "Total Email : " + lstEmailSelect.Items.Count.ToString();

        if (duplicateCounter == 1)
        {
            DisplayMessage("Duplicate Record Found");
        }
    }
    protected void btnPullDept_Click(object sender, EventArgs e)
    {
        if (lstEmailSelect.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstEmailSelect.Items)
            {
                if (li.Selected)
                {

                    lstEmail.Items.Add(li);
                }
            }
            foreach (ListItem li in lstEmail.Items)
            {
                if (li.Selected)
                {
                    lstEmailSelect.Items.Remove(li);
                }
            }
            lstEmail.SelectedIndex = -1;
        }


        lstEmail.Items.Clear();

        //btnPullDept.Focus();
        lblFinalEmailListrecord.Text = "Total Email : " + lstEmailSelect.Items.Count.ToString();
    }
    protected void btnPushAllDept_Click(object sender, EventArgs e)
    {
        int duplicateCounter = 0;

        foreach (ListItem li in lstEmail.Items)
        {
            if (lstEmailSelect.Items.FindByText(li.Text) == null)
            {
                lstEmailSelect.Items.Add(li);
            }
            else
            {
                duplicateCounter++;
            }
        }
        foreach (ListItem DeptItem in lstEmailSelect.Items)
        {
            lstEmail.Items.Remove(DeptItem);
        }
        btnPushAllDept.Focus();

        lblFinalEmailListrecord.Text = "Total Email : " + lstEmailSelect.Items.Count.ToString();


        if (duplicateCounter > 0)
        {
            DisplayMessage("Duplicate Record Found = " + duplicateCounter.ToString());
        }
    }
    protected void btnPullAllDept_Click(object sender, EventArgs e)
    {
        lstEmailSelect.Items.Clear();

        //btnPullAllDept.Focus();

        lblFinalEmailListrecord.Text = "Total Email : " + lstEmailSelect.Items.Count.ToString();
    }
    protected void btnNextFinalEmailList_Click(object sender, EventArgs e)
    {

        if (lstEmailSelect.Items.Count == 0)
        {
            DisplayMessage("Email list not found");
            return;
        }
        multiview.ActiveViewIndex = 4;
        imgstep.ImageUrl = "../Images/Step4.png";
        GetRadioList();
    }
    protected void btnBackFinalEmailList_Click(object sender, EventArgs e)
    {
        multiview.ActiveViewIndex = 0;
        imgstep.ImageUrl = "../Images/Step1.png";
        Div_Filter.Visible = true;


        chkInvoiceRecordOnly.Visible = true;
        ddlFilter.Visible = true;
        lblFilter.Visible = true;
        //multiview.ActiveViewIndex = 1;
        //imgstep.ImageUrl = "../Images/Step2.png";
    }
    #endregion
    #region Product category
    private void FillProductCategory()
    {
        ddlcategorysearch.Items.Clear();
        DataTable dsCategory = null;
        dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(StrCompId.ToString());

        if (dsCategory.Rows.Count > 0)
        {
            dsCategory = new DataView(dsCategory, "", "Category_Name Asc", DataViewRowState.CurrentRows).ToTable();

            ChkProductCategory.Items.Clear();
            ChkProductCategory.DataSource = dsCategory;
            ChkProductCategory.DataTextField = "Category_Name";
            ChkProductCategory.DataValueField = "Category_Id";
            ChkProductCategory.DataBind();

            ddlcategorysearch.DataSource = dsCategory;
            ddlcategorysearch.DataTextField = "Category_Name";
            ddlcategorysearch.DataValueField = "Category_Id";
            ddlcategorysearch.DataBind();
            ddlcategorysearch.Items.Insert(0, "--Select--");


            //lstFruits.DataSource = dsCategory;
            //lstFruits.DataTextField = "Category_Name";
            //lstFruits.DataValueField = "Category_Id";
            //lstFruits.DataBind();

        }


    }
    protected void ChkProductCategory_SelectedIndexChanged(object sender, EventArgs e)
    {

        ChkProductChildCategory.Items.Clear();
        DataTable dtProduct = new DataTable();
        DataTable dtCat = new DataTable();



        foreach (ListItem li in ChkProductCategory.Items)
        {
            if (li.Selected == true)
            {
                dtProduct = ObjInvModelMaster.GetProductNameByCategoryID(li.Value);

                dtCat.Merge(dtProduct);


            }


        }//end for


        if (dtCat.Rows.Count > 0)
        {
            dtCat = new DataView(dtCat, "", "EProductName Asc", DataViewRowState.CurrentRows).ToTable();


            ChkProductChildCategory.DataSource = dtCat;
            ChkProductChildCategory.DataTextField = "TemplateProductName";
            ChkProductChildCategory.DataValueField = "ProductID";
            ChkProductChildCategory.DataBind();
        }

        // GetRadioList();
        //DataTable dtProCat = new DataTable();
        //if (productCat != "")
        //{

        //    dtProCat = objTemMasterRef.GetTemplateMasterByMultipleRefIdNType("PC", productCat);
        //}
        //if(dtProCat.Rows.Count > 0)
        //{

        //   if ( Session["treeNode"] != null &&  Session["treeNode"] != "")
        //    {
        //        string Treenodeforsql =  Session["treeNode"].ToString();
        //        Treenodeforsql = Treenodeforsql.Remove(Treenodeforsql.Length - 1);
        //        DataTable dtRadio = objTemMasterRef.GetTemplateMasterByMultipleRefIdNType("CG", Treenodeforsql.ToString());
        //        if (dtRadio.Rows.Count > 0)
        //        {
        //            dtProCat.Merge(dtRadio);
        //        }


        //    }
        //}
        //else
        //{
        //    if (( Session["treeNode"] != "") && ( Session["treeNode"] != null))
        //    {
        //        string Treenodeforsql =  Session["treeNode"].ToString();
        //        Treenodeforsql = Treenodeforsql.Remove(Treenodeforsql.Length - 1);
        //        DataTable dtRadio = objTemMasterRef.GetTemplateMasterByMultipleRefIdNType("CG", Treenodeforsql.ToString());
        //        if (dtRadio.Rows.Count > 0)
        //        {
        //            dtProCat = dtRadio;
        //        }


        //    }
        //     else
        //     {
        //         dtProCat = null;
        //     }
        //}

        //if (dtProCat.Rows.Count > 0)
        //{

        //    // dtProCat = new DataView(dtProCat, "", "Trans_Id asc", DataViewRowState.CurrentRows).ToTable(true, "Trans_Id");

        //    string disTrans = string.Empty;
        //    for (int k = 0; k < dtProCat.Rows.Count; k++)
        //    {
        //        if (disTrans == "")
        //        {
        //            disTrans = dtProCat.Rows[k]["Trans_Id"].ToString();
        //        }
        //        else
        //        {
        //            disTrans = disTrans + "," + dtProCat.Rows[k]["Trans_Id"].ToString();
        //        }
        //    }

        //    dtProCat = objTemMasterRef.GetTemplateMasterDistinctByMultipleId(disTrans);



        //}

        //else
        //{
        //    if (( Session["treeNode"] == "") || ( Session["treeNode"] == null))
        //    {

        //    }
        //}

        //rbn1.DataSource = null;
        //rbn1.DataBind();
        //rbn1.SelectedIndex = -1;
        //rbn1.ClearSelection();
        //rbn1.Items.Clear();
        //if (dtProCat != null)
        //{
        //    rbn1.DataSource = dtProCat;
        //    //(DataTable) Session["dtRadio"];
        //    rbn1.DataValueField = "Trans_Id";
        //    rbn1.DataTextField = "Template_Name";
        //    rbn1.DataBind();
        //    rbn1.Visible = true;
        //}

    }
    protected void imgProductsearch_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtProduct = new DataTable();
        int counter = 0;
        if (txtProductSearch.Text != "")
        {


            try
            {
                dtProduct = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtProductSearch.Text.ToString());
            }
            catch
            {
            }
            if (dtProduct != null)
            {
                if (dtProduct.Rows.Count > 0)
                {
                    foreach (ListItem li in ChkProductChildCategory.Items)
                    {

                        if (dtProduct.Rows[0]["ProductId"].ToString() == li.Value)
                        {
                            counter = 1;
                            break;
                        }
                    }

                }
                else
                {
                    DisplayMessage("Product Not Found");
                    txtProductSearch.Focus();
                    return;
                }
            }
            else
            {
                DisplayMessage("Product Not Found");
                txtProductSearch.Focus();
                return;
            }

        }
        if (counter == 1)
        {
            ChkProductChildCategory.DataSource = dtProduct;
            ChkProductChildCategory.DataTextField = "TemplateProductName";
            ChkProductChildCategory.DataValueField = "ProductId";
            ChkProductChildCategory.DataBind();
        }
        else
        {
            DisplayMessage("Product Not Found");
            txtProductSearch.Focus();
            return;
        }
    }
    protected void imgProductRefresh_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtProduct = new DataTable();
        DataTable dtCat = new DataTable();
        foreach (ListItem li in ChkProductCategory.Items)
        {
            if (li.Selected == true)
            {
                dtProduct = ObjInvModelMaster.GetProductNameByCategoryID(li.Value);

                dtCat.Merge(dtProduct);

            }

        }

        if (dtCat.Rows.Count > 0)
        {
            dtCat = new DataView(dtCat, "", "EProductName Asc", DataViewRowState.CurrentRows).ToTable();


            ChkProductChildCategory.DataSource = dtCat;
            ChkProductChildCategory.DataTextField = "TemplateProductName";
            ChkProductChildCategory.DataValueField = "ProductID";
            ChkProductChildCategory.DataBind();
        }
        txtProductSearch.Text = "";

    }

    public void GetRadioList()
    {
        DataTable dtTemp = new DataTable();
        DataTable dtProCat = new DataTable();
        DataTable dt = new DataTable();




        if (chkShowAll.Checked)
        {

            dt = objTemplate.GetRecord("0", "0", "2");
        }
        else
        {
            if (!chkInvoiceRecordOnly.Checked)
            {
                if (Session["treeNode"] != null && Session["treeNode"] != "")
                {
                    string Treenodeforsql = Session["treeNode"].ToString();
                    Treenodeforsql = Treenodeforsql.Remove(Treenodeforsql.Length - 1);
                    DataTable dtRadio = objTemMasterRef.GetTemplateMasterByMultipleRefIdNType("CG", Treenodeforsql.ToString());
                    if (dtRadio.Rows.Count > 0)
                    {
                        dtProCat.Merge(dtRadio);
                    }
                    else
                    {
                        dtProCat = objTemplate.GetRecord("0", "0", "2");
                    }
                }
                else
                {
                    dtProCat = objTemplate.GetRecord("0", "0", "2");
                }

            }
            else
            {

                dtProCat = objTemplate.GetRecord("0", "0", "2");

            }





            string productCat = string.Empty;

            foreach (ListItem li in ChkProductCategory.Items)
            {
                if (li.Selected)
                {
                    if (productCat == "")
                    {
                        productCat = li.Value.ToString().Trim();
                    }
                    else
                    {
                        productCat = productCat.Trim() + "," + li.Value.ToString().Trim();
                    }
                }
            }


            //if (ddlcategorysearch.SelectedIndex > 0)
            //{
            //    productCat = ddlcategorysearch.SelectedValue.Trim();
            //}

            if (productCat.Trim() != "")
            {

                DataTable dtRadio = objTemMasterRef.GetTemplateMasterByMultipleRefIdNType("PC", productCat.Trim());
                if (dtRadio.Rows.Count > 0)
                {


                    DataTable dtCopy = dtProCat.Copy();
                    // dtProCat = null;
                    dtProCat = new DataTable();

                    for (int i = 0; i < dtRadio.Rows.Count; i++)
                    {
                        dt = new DataView(dtCopy, "Trans_id=" + dtRadio.Rows[i]["Trans_id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

                        if (dt.Rows.Count > 0)
                        {
                            dtProCat.Merge(dt);
                        }

                    }
                    //dtProCat.Merge(dtRadio);
                }
            }
            string ProductList = string.Empty;

            if (txtProductcode.Text.Trim() != "")
            {
                ProductList = hdnproductId.Value;
            }


            //foreach (ListItem li in ChkProductChildCategory.Items)
            //{
            //    if (li.Selected == true)
            //    {
            //        if (ProductList == "")
            //        {
            //            ProductList = li.Value;
            //        }
            //        else
            //        {
            //            ProductList = ProductList + "," + li.Value;

            //        }
            //    }
            //}
            if (ProductList != "")
            {
                DataTable dtRadio = objTemMasterRef.GetTemplateMasterByMultipleRefIdNType("IM", ProductList);
                if (dtRadio.Rows.Count > 0)
                {


                    DataTable dtCopy = dtProCat.Copy();
                    dtProCat = new DataTable();


                    for (int i = 0; i < dtRadio.Rows.Count; i++)
                    {
                        dt = new DataView(dtCopy, "Trans_id=" + dtRadio.Rows[i]["Trans_id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

                        if (dt.Rows.Count > 0)
                        {
                            dtProCat.Merge(dt);
                        }

                    }
                    //dtProCat.Merge(dtRadio);
                }
            }




            if (dtProCat.Rows.Count > 0)
            {

                // dtProCat = new DataView(dtProCat, "", "Trans_Id asc", DataViewRowState.CurrentRows).ToTable(true, "Trans_Id");

                string disTrans = string.Empty;
                for (int k = 0; k < dtProCat.Rows.Count; k++)
                {
                    if (disTrans == "")
                    {
                        disTrans = dtProCat.Rows[k]["Trans_Id"].ToString();
                    }
                    else
                    {
                        disTrans = disTrans + "," + dtProCat.Rows[k]["Trans_Id"].ToString();
                    }
                }

                dt = objTemMasterRef.GetTemplateMasterDistinctByMultipleId(disTrans);
            }

            if (txtModelNo.Text != "")
            {
                dt = new DataView(dt, "Field2='" + hdnModelId.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
            }


        }




        if (dt.Rows.Count > 0)
        {
            try
            {
                dt = new DataView(dt, "", "Trans_Id desc", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            //rbn1.DataSource = null;
            //rbn1.DataBind();
            //rbn1.SelectedIndex = -1;
            //rbn1.ClearSelection();
            //rbn1.Items.Clear();
            Editor1.Content = "";
            //rbn1.DataSource = dt;
            dtlistTemplate.DataSource = dt;
            dtlistTemplate.DataBind();

            //rbn1.DataValueField = "Trans_Id";
            //rbn1.DataTextField = "Template_Name";
            //rbn1.DataBind();
            Session["dtRadio"] = dt;
            lblTotalRecordTemplate.Text = "Total Records : " + dt.Rows.Count.ToString();
        }
        else
        {
            dtlistTemplate.DataSource = null;
            dtlistTemplate.DataBind();
            //rbn1.DataSource = null;
            //rbn1.DataBind();
            //rbn1.SelectedIndex = -1;
            //rbn1.ClearSelection();
            //rbn1.Items.Clear();
            Session["dtRadio"] = null;
            Editor1.Content = "";
            lblTotalRecordTemplate.Text = "Total Records : 0";
        }
    }
    protected void ChkProductChildCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dtProduct = new DataTable();
        DataTable dtCat = new DataTable();
        string productCat = string.Empty;


        foreach (ListItem li in ChkProductChildCategory.Items)
        {
            if (li.Selected == true)
            {
                if (productCat == "")
                {
                    productCat = li.Value.ToString();
                }
                else
                {
                    productCat = productCat + "," + li.Value.ToString();
                }

            }
        }

        //DataTable dtProCat = new DataTable();
        //if (productCat != "")
        //{
        //    dtProCat = objTemMasterRef.GetTemplateMasterByMultipleRefIdNType("IM", productCat);

        //    if ( Session["dtRadio"] != null)
        //    {
        //        dtProCat.Merge((DataTable)( Session["dtRadio"]));

        //        dtProCat = new DataView(dtProCat, "", "Trans_Id asc", DataViewRowState.CurrentRows).ToTable(true, "Trans_Id");

        //        string disTrans = string.Empty;
        //        for (int k = 0; k < dtProCat.Rows.Count; k++)
        //        {
        //            if (disTrans == "")
        //            {
        //                disTrans = dtProCat.Rows[k]["Trans_Id"].ToString();
        //            }
        //            else
        //            {
        //                disTrans = disTrans + "," + dtProCat.Rows[k]["Trans_Id"].ToString();
        //            }
        //        }
        //        dtProCat = objTemMasterRef.GetTemplateMasterDistinctByMultipleId(disTrans);

        //         Session["dtRadio"] = dtProCat;
        //    }
        //    else
        //    {
        //         Session["dtRadio"] = dtProCat;
        //    }

        //    rbn1.DataSource = null;
        //    rbn1.DataBind();
        //    rbn1.SelectedIndex = -1;
        //    rbn1.ClearSelection();
        //    rbn1.Items.Clear();
        //    rbn1.DataSource = (DataTable) Session["dtRadio"];
        //    rbn1.DataValueField = "Trans_Id";
        //    rbn1.DataTextField = "Template_Name";
        //    rbn1.DataBind();
        //    rbn1.Visible = true;
        //}



    }

    protected void BtnNextProductList_Click(object sender, EventArgs e)
    {







        GetRadioList();
        if (Session["dtRadio"] == null)
        {
            DisplayMessage("Template Not Found");
            return;
        }
        multiview.ActiveViewIndex = 4;
        imgstep.ImageUrl = "../Images/Step5.png";
        //ltrstep.Text = "<span style='font-size: 12pt; color:#ffffff;'>Step 1 >> Step 2 >> Step 3 >> </Span><span style='font-size: 12pt;font-weight: bold;'>Step 4</Span><span style='font-size: 12pt; color:#ffffff;'>  >> Step 5";

        DataTable dt = (DataTable)Session["dtRadio"];
        lblTotalRecordTemplate.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count + "";

    }
    protected void BtnBackProductList_Click(object sender, EventArgs e)
    {
        multiview.ActiveViewIndex = 2;
        imgstep.ImageUrl = "../Images/Step3.png";
    }
    protected void BtnResetProductList_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void BtnCancelProductList_Click(object sender, EventArgs e)
    {
        btnCancel_Click(null, null);
    }
    #endregion
    #region TemplateSelection
    protected void btnbindTemplate_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        string condition = string.Empty;
        if (ddlOptionTemplate.SelectedIndex == 1)
        {
            condition = ddlFieldNameTemplate.SelectedValue + "='" + txtValueTemplate.Text.Trim() + "'";
        }
        else if (ddlOptionContactFinal.SelectedIndex == 2)
        {
            condition = ddlFieldNameTemplate.SelectedValue + " Like '%" + txtValueTemplate.Text.Trim() + "%'";
        }
        else
        {
            condition = ddlFieldNameTemplate.SelectedValue + " like '" + txtValueTemplate.Text.Trim() + "%'";
        }
        DataTable dtTemplate = null;
        dtTemplate = (DataTable)Session["dtRadio"];

        if (dtTemplate != null)
        {
            dtTemplate = new DataView(dtTemplate, condition, "", DataViewRowState.CurrentRows).ToTable();

            dtlistTemplate.DataSource = dtTemplate;
            dtlistTemplate.DataBind();

            lblTotalRecordTemplate.Text = Resources.Attendance.Total_Records + " : " + dtTemplate.Rows.Count + "";
            Session["dtRadio"] = dtTemplate;
            //view.ToTable();
        }

      
        txtValueTemplate.Focus();
    }
    protected void btnRefreshTemplate_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        GetRadioList();
        DataTable dt = (DataTable)Session["dtRadio"];
        lblTotalRecordTemplate.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count + "";
        txtValueTemplate.Text = "";
    }
    protected void BtnNextTemplate_Click(object sender, EventArgs e)
    {
        bool IsSelect = false;
        foreach (DataListItem item in dtlistTemplate.Items)
        {
            if (((RadioButton)item.FindControl("rbtnTemplate")).Checked)
            {
                IsSelect = true;
                break;
            }
        }
        if (!IsSelect)
        {
            DisplayMessage("Select at Least One Template");
            return;
        }

        multiview.ActiveViewIndex = 5;
        imgstep.ImageUrl = "../Images/Step5.png";
        lblsentmailcount.Text = lstEmailSelect.Items.Count.ToString();
        //ltrstep.Text = "<span style='font-size: 12pt; color:#ffffff;'>Step 1 >> Step 2 >> Step 3 >> Step 4 >> </Span><span style='font-size: 12pt;font-weight: bold;'>Step 5</Span>";


    }
    protected void BtnBackTemplate_Click(object sender, EventArgs e)
    {
        multiview.ActiveViewIndex = 2;
        imgstep.ImageUrl = "../Images/Step3.png";
        //  ltrstep.Text = "<span style='font-size: 12pt; color:#ffffff;'>Step 1 >> Step 2 >>  </Span><span style='font-size: 12pt;font-weight: bold;'>Step 3</Span><span style='font-size: 12pt; color:#ffffff;'>  >> Step 4 >> Step 5";

    }
    protected void BtnResetTemplate_Click(object sender, EventArgs e)
    {
        Reset();

    }
    protected void BtnCancelTemplate_Click(object sender, EventArgs e)
    {
        btnCancel_Click(null, null);
    }
    #endregion
    #region ViewFinal
    protected void gvEditContacts_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEditContacts.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtEdit"];
        gvEditContacts.DataSource = dt;
        gvEditContacts.DataBind();


    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
        string btnid = btn.ID;
        btnSave_Click(sender, null);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool IsAppproval = (bool)Session["IsAppproval"];


        DataTable dtEmpApproval = new DataTable();
        string ApprovalEmpPermission = string.Empty;
        string StrStatus = "Approved";
        if (IsAppproval)
        {
            StrStatus = "Pending";
            ApprovalEmpPermission = objSys.Get_Approval_Parameter_By_Name("SalesOrder").Rows[0]["Approval_Level"].ToString();

            dtEmpApproval = objEmpApproval.getApprovalChainByObjectid(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(),Common.GetObjectIdbyPageURL("../EMS/MailMarketing.aspx", Session["DBConnection"].ToString()), Session["EmpId"].ToString());

            if (dtEmpApproval.Rows.Count == 0)
            {
                DisplayMessage("Approval setup issue , please contact to your admin");
                return;
            }

        }

        string ContactList = string.Empty;
        int flag = 0;
        string BrandId = string.Empty;
        if (Editor1.Content == "")
        {
            DisplayMessage("Select Template");
            return;
        }

        if (txtMailHeader.Text == "")
        {
            DisplayMessage("Fill Mail Header For Mail");
            txtMailHeader.Focus();
            return;
        }
        if (ddlbrandsearch.SelectedIndex == 0)
        {
            BrandId = "0";
        }
        else
        {
            BrandId = ddlbrandsearch.SelectedValue;
        }

        int b = 0;

        if (editid.Value == "")
        {
            try
            {
                b = objMailMarket.CRUDHeaderRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["TemplateId"].ToString(), Editor1.Content, txtMailHeader.Text, ddlEmailUser.SelectedValue, txtdisplayText.Text, BrandId, StrStatus, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "1");
            }
            catch
            {
                b = objMailMarket.CRUDHeaderRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["TemplateId"].ToString(), Editor1.Content, txtMailHeader.Text, ddlEmailUser.SelectedValue, txtdisplayText.Text, BrandId, StrStatus, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "1");
            }

            if (b != 0)
            {
                //inserting record in approval section if approval functionality enable for login user 
                if (IsAppproval)
                {
                    for (int j = 0; j < dtEmpApproval.Rows.Count; j++)
                    {
                        string PriorityEmpId = dtEmpApproval.Rows[j]["Emp_Id"].ToString();
                        string IsPriority = dtEmpApproval.Rows[j]["Priority"].ToString();
                        int cur_trans_id = 0;
                        if (ApprovalEmpPermission == "1")
                        {
                            cur_trans_id = objEmpApproval.InsertApprovalTransaciton(dtEmpApproval.Rows[j]["Approval_Id"].ToString(), Session["CompId"].ToString(), "0", "0", "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                        }
                        else if (ApprovalEmpPermission == "2")
                        {
                            cur_trans_id = objEmpApproval.InsertApprovalTransaciton(dtEmpApproval.Rows[j]["Approval_Id"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                        }
                        else if (ApprovalEmpPermission == "3")
                        {
                            cur_trans_id = objEmpApproval.InsertApprovalTransaciton(dtEmpApproval.Rows[j]["Approval_Id"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                        }
                        else
                        {
                            cur_trans_id = objEmpApproval.InsertApprovalTransaciton(dtEmpApproval.Rows[j]["Approval_Id"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", Session["EmpId"].ToString(), b.ToString(), PriorityEmpId, IsPriority, System.DateTime.Now.ToString(), "01/01/1900", "Pending", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString());
                        }
                        // Insert Notification For Leave by  ghanshyam suthar
                        Session["PriorityEmpId"] = PriorityEmpId;
                        Session["cur_trans_id"] = cur_trans_id;
                        Session["Ref_ID"] = b.ToString();
                        Set_Notification();

                    }
                }

                string EmailId = string.Empty;
                ContactList = Session["hdnFldSelectedValues"].ToString().Trim();
                foreach (GridViewRow gvrow in gvContactFinal.Rows)
                {
                    if (((CheckBox)gvrow.FindControl("chkBxSelect")).Checked)
                    {
                        if (((Label)gvrow.FindControl("lblTName")).Text.Trim() != "")
                        {
                            if (lstEmailSelect.Items.FindByText(((Label)gvrow.FindControl("lblTName")).Text.Trim()) != null)
                            {



                                if (!EmailId.Trim().Split(',').Contains(((Label)gvrow.FindControl("lblTName")).Text.Trim()))
                                {

                                    if (((Button)sender).ID == "btnSaveandsend")
                                    {
                                        objMailMarket.CRUDDetailRecord("0", b.ToString(), ((Label)gvrow.FindControl("hdnFldId")).Text, "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "1");
                                    }
                                    else
                                    {
                                        objMailMarket.CRUDDetailRecord("0", b.ToString(), ((Label)gvrow.FindControl("hdnFldId")).Text, "", "", "Send", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "1");

                                    }
                                }

                                EmailId += ((Label)gvrow.FindControl("lblTName")).Text.Trim() + ",";
                            }

                        }
                    }
                }


                foreach (ListItem li in lstEmailSelect.Items)
                {
                    if (!EmailId.Trim().Split(',').Contains(li.Text.Trim()))
                    {
                        if (((Button)sender).ID == "btnSaveandsend" || ((Button)sender).ID == "btnSave")
                        {
                            objMailMarket.CRUDDetailRecord("0", b.ToString(), "0", "", li.Text, "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "1");
                        }
                        else
                        {
                            objMailMarket.CRUDDetailRecord("0", b.ToString(), "0", "", li.Text, "Send", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "1");

                        }
                    }
                }

                //foreach (ListItem li in chkDefaultMailList.Items)
                //{
                //    if (li.Selected)
                //    {
                //        if (!EmailId.Split(',').Contains(li.Text))
                //        {
                //            if (((Button)sender).ID == "btnSaveandsend")
                //            {
                //                objMailMarket.CRUDDetailRecord("0", b.ToString(), "0", "", li.Text, "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "1");
                //            }
                //            else
                //            {
                //                objMailMarket.CRUDDetailRecord("0", b.ToString(), "0", "", li.Text,"Send", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "1");

                //            }
                //        }
                //    }
                //}




                FillGrid();

                if (IsAppproval)
                {
                    DisplayMessage("Record Saved and sent in approval section", "green");
                }
                else
                {
                    if (((Button)sender).ID == "btnSaveandsend")
                    {

                        DisplayMessage("Record saved and your mail send process is started", "green");

                    }
                    else
                    {
                        DisplayMessage("Record Saved", "green");
                    }
                }
                Reset();
            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }
        else
        {
            b = objMailMarket.CRUDHeaderRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, Session["TemplateId"].ToString(), "", txtMailHeader.Text, "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "2");

            if (b != 0)
            {
                ContactList = Session["hdnFldSelectedValues"].ToString().Trim();

                foreach (string str in ContactList.Split(','))
                {
                    if (str != "")
                    {
                        objMailMarket.CRUDDetailRecord("0", editid.Value.ToString(), str.ToString(), "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "1");
                    }
                }
                //if (chkPrevious.Checked == true)
                //{
                try
                {
                    DataTable dtEditGrid = (DataTable)Session["dtEdit"];
                    if (dtEditGrid.Rows.Count > 0)
                    {
                        for (int id = 0; id < dtEditGrid.Rows.Count; id++)
                        {
                            objMailMarket.CRUDDetailRecord("0", editid.Value.ToString(), dtEditGrid.Rows[id]["Contact_Id"].ToString(), "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "1");
                        }
                    }
                }
                catch
                {

                }
                //}

                DisplayMessage("Record Updated", "green");
                FillGrid();

                if (ddlEmailUser != null)
                {
                    if (Session["EmpId"].ToString() == "0")
                    {
                        Session["Page&MailType&Id"] = "EMS" + "," + "All" + "," + editid.Value;
                        //SendMail(editid.Value, txtMailHeader.Text, "ALL");
                    }
                    else
                    {
                        DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), Session["ModuleId"].ToString(), "252", HttpContext.Current.Session["CompId"].ToString());
                        if (dtAllPageCode.Rows.Count != 0)
                        {
                            if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
                            {
                            }
                            else
                            {
                                foreach (DataRow DtRow in dtAllPageCode.Rows)
                                {
                                    if (DtRow["Op_Id"].ToString() == "10")
                                    {
                                        Session["Page&MailType&Id"] = "EMS" + "," + "All" + "," + editid.Value;
                                        //SendMail(editid.Value, txtMailHeader.Text, "ALL");
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                btnList_Click(null, null);
                Reset();
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {
                DisplayMessage("Record Not Updated");
            }
        }
    }

    private void Set_Notification()
    {
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();
        string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        string URL = HttpContext.Current.Request.Url.AbsoluteUri.Substring(currentUrl.IndexOf("/ems"));

        int index = URL.LastIndexOf(".aspx");
        if (index > 0)
            URL = URL.Substring(0, index + 5);

        Dt_Request_Type = Obj_Notifiacation.Get_Request_Type(".." + URL, Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString());
        string Request_URL = "../MasterSetUp/EmployeeApproval.aspx?Request_ID=" + Dt_Request_Type.Rows[0]["Request_Emp_ID"].ToString() + "&Request_Type=" + Dt_Request_Type.Rows[0]["Approval_Id"].ToString() + "";
        string Message = string.Empty;
        Message = GetEmployeeCode(Session["UserId"].ToString()) + " request for email marketing on " + System.DateTime.Now.ToString();

        Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["EmpId"].ToString(), Session["EmpId"].ToString(), Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", Session["cur_trans_id"].ToString(), "False", ViewState["Emp_Img"].ToString(), "", "", "", "", Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["UserId"].ToString(), System.DateTime.Now.ToString(), Session["Ref_ID"].ToString(), "1");

    }


    protected string GetEmployeeCode(string strEmployeeId)
    {
        string strEmployeeName = string.Empty;
        if (strEmployeeId != "0" && strEmployeeId != "")
        {
            DataTable dtEName = objEmployee.GetEmployeeMasterByEmpCode(StrCompId, strEmployeeId);
            if (dtEName.Rows.Count > 0)
            {
                strEmployeeName = dtEName.Rows[0]["Emp_Name"].ToString();
                ViewState["Emp_Img"] = "../CompanyResource/2/" + dtEName.Rows[0]["Emp_Image"].ToString();
            }
            else
            {
                ViewState["Emp_Img"] = "";
            }
        }
        else
        {
            strEmployeeName = "";
            ViewState["Emp_Img"] = "";
        }
        return strEmployeeName;
    }


    protected void btnBack_Click(object sender, EventArgs e)
    {
        multiview.ActiveViewIndex = 4;
        imgstep.ImageUrl = "../Images/Step5.png";
        //ltrstep.Text = "<span style='font-size: 12pt; color:#ffffff;'>Step 1 >> Step 2 >> Step 3 >>  </Span><span style='font-size: 12pt;font-weight: bold;'>Step 4</Span><span style='font-size: 12pt; color:#ffffff;'> >> Step 5";


    }
    #endregion
    #region Email
    public void FillUserEmailAccount()
    {
        DataTable dt = objUserDetail.GetbyUserId(Session["UserId"].ToString(), StrCompId);
        ddlEmailUser.DataSource = dt;
        ddlEmailUser.DataTextField = "Email";
        ddlEmailUser.DataValueField = "Trans_Id";
        ddlEmailUser.DataBind();
        dt = new DataView(dt, "IsDefault='True'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count != 0)
        {
            ddlEmailUser.SelectedValue = dt.Rows[0]["Trans_Id"].ToString();
            Session["UserEmailSelect"] = dt.Rows[0]["Trans_Id"].ToString();
        }
    }
    public bool SendMail(string Id, string Subject, string sendTo, string Userid, string Mailid)
    {

        string displayText = string.Empty;
        bool issendmail = false;
        string StrFromId = string.Empty;
        string StrUserName = string.Empty;
        string StrPass = string.Empty;
        string ManufacturingBrandid = string.Empty;
        string EmpName = ObjUserMaster.GetUserMasterByUserId(Session["UserId"].ToString(), Session["CompId"].ToString()).Rows[0]["EmpName"].ToString();
        try
        {
            string[] strUPEId = GetUserNamePassEmpId(Userid, Mailid);
            StrFromId = strUPEId[0].ToString();
            StrUserName = strUPEId[1].ToString();
            StrPass = strUPEId[2].ToString();

        }
        catch
        {
            StrFromId = "0";
        }

        DataTable dt = objMailMarket.GetRecordHeader(Id, "5");
        //dt = new DataView(dt, "Trans_id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {

            ManufacturingBrandid = dt.Rows[0]["Field4"].ToString();
            if (dt.Rows[0]["Field3"].ToString() != "")
            {
                displayText = dt.Rows[0]["Field3"].ToString();
            }
            else
            {
                displayText = EmpName;
            }
        }

        int i = Obj_SendMailHeader.SendMail_Insert(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), StrFromId, "0", "0", Subject, Editor1.Content, "P", StrUserName.ToString(), Session["TotalLength"].ToString(), "", "", "", true.ToString(), "1/1/1800", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        DataTable dtContactMail = objMailMarket.GetRecordDetail(Id, "4");
        if (sendTo.Trim() == "FAILURE")
        {
            dtContactMail = new DataView(dtContactMail, "Field1='False'", "", DataViewRowState.CurrentRows).ToTable(true, "Id", "Contact_Id", "Name", "EmailId", "MobileNo");
        }
        if (sendTo.Trim() == "PENDING")
        {
            dtContactMail = new DataView(dtContactMail, "Field1=' '", "", DataViewRowState.CurrentRows).ToTable(true, "Id", "Contact_Id", "Name", "EmailId", "MobileNo");
        }

        if (dtContactMail.Rows.Count > 0)
        {
            string EmailId = string.Empty;
            bool b = false;
            for (int j = 0; j < dtContactMail.Rows.Count; j++)
            {
                if (dtContactMail.Rows[j]["EMailId"].ToString() != "")
                {
                    try
                    {
                        //Email,MailContent,ContactId, MancuBrandId(Field4)
                        Editor1.Content = UpdateMailContent(dtContactMail.Rows[j]["EMailId"].ToString(), Editor1.Content, dtContactMail.Rows[j]["Contact_Id"].ToString(), ManufacturingBrandid);

                        if (!EmailId.Split(',').Contains(dtContactMail.Rows[j]["EMailId"].ToString()))
                        {

                            b = ObjSendMailSms.SendMail(dtContactMail.Rows[j]["EMailId"].ToString() + ";", "", "", Subject, Editor1.Content, StrCompId, "", StrUserName, StrPass, displayText, HttpContext.Current.Session["UserId"].ToString(),"", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                        }
                        else
                        {
                            b = true;
                        }


                        if (b == true)
                        {

                            issendmail = true;
                            EmailId += dtContactMail.Rows[j]["EMailId"].ToString() + ",";
                            objMailMarket.CRUDDetailRecord(dtContactMail.Rows[j]["Id"].ToString(), Id, "0", "True", dtContactMail.Rows[j]["EMailId"].ToString(), "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "6");
                        }
                        else
                        {
                            objMailMarket.CRUDDetailRecord(dtContactMail.Rows[j]["Id"].ToString(), Id, "0", "False", dtContactMail.Rows[j]["EMailId"].ToString(), "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "6");

                        }
                    }
                    catch
                    {
                        objMailMarket.CRUDDetailRecord(dtContactMail.Rows[j]["Id"].ToString(), Id, "0", "False", dtContactMail.Rows[j]["EMailId"].ToString(), "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "6");

                    }
                }
            }
        }

        return issendmail;
        //string url = "../EmailSystem/SendMail.aspx?Page=EMS";
        //ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('" + url + "','','height=660,width=1100,scrollbars=Yes')", true);
    }

    private string UpdateMailContent(string EmailId, string Content, string ContactId, string ManucBrand)
    {


        //string TransId = objEmailHeader.Get_EmailMasterHeaderTrueByEmailId(EmailId).Rows[0]["Trans_id"].ToString();

        if (ManucBrand == "0" || ManucBrand == "")
        {

        }
        else
        {

            string AddUnregisterContent = objProB.GetProductBrandByPBrandId(Session["CompId"].ToString(), ManucBrand).Rows[0]["Field2"].ToString();
            if (AddUnregisterContent != "")
            {
                Content += "<br/>" + AddUnregisterContent;
                Content = Content.Replace("@E", EmailId);
                Content = Content.Replace("@C", ContactId);



            }

        }

        return Content;
    }
    public string[] GetUserNamePassEmpId(string UserId, string EmailId)
    {
        string[] s = new string[3];
        try
        {
            DataTable dtUM = new DataView(objUserDetail.GetbyUserId(UserId, Session["CompId"].ToString()), "Email='" + EmailId + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtUM.Rows.Count != 0)
            {
                s[0] = Session["EmpId"].ToString();
                s[1] = dtUM.Rows[0]["Email"].ToString();
                s[2] = dtUM.Rows[0]["Password"].ToString();
            }
        }
        catch
        {
            s[0] = "0";
            s[1] = null;
            s[2] = null;

        }
        return s;

    }
    protected void btnTotalMail_OnCommand(object sender, CommandEventArgs e)
    {

        DataTable dt = objMailMarket.GetRecordDetail(e.CommandArgument.ToString(), "4");
        //dt = new DataView(dt, "Field1='True'", "", DataViewRowState.CurrentRows).ToTable(true, "Contact_Id", "Name", "EmailId", "MobileNo");
        //dt.Columns["Field11"].ColumnName = "EmailId";
        // dt.Columns["Field21"].ColumnName = "MobileNo";
        if (dt.Rows.Count > 0)
        {
            ExportTableData(dt, "All Mail List");
        }
        else
        {
            DisplayMessage("No Contacts Exists for Selected Record");
            return;
        }
    }
    protected void btnsentMail_OnCommand(object sender, CommandEventArgs e)
    {

        DataTable dt = objMailMarket.GetRecordDetail(e.CommandArgument.ToString(), "4");
        dt = new DataView(dt, "Field1='True'", "", DataViewRowState.CurrentRows).ToTable(true, "Contact_Id", "Name", "EmailId", "MobileNo");
        //dt.Columns["Field11"].ColumnName = "EmailId";
        // dt.Columns["Field21"].ColumnName = "MobileNo";
        if (dt.Rows.Count > 0)
        {
            ExportTableData(dt, "Success Mail List");
        }
        else
        {
            DisplayMessage("No Success Mail Contacts Exists for Selected Record");
            return;
        }
    }
    protected void btnFailureMail_OnCommand(object sender, CommandEventArgs e)
    {

        DataTable dt = objMailMarket.GetRecordDetail(e.CommandArgument.ToString(), "4");
        dt = new DataView(dt, "Field1='False'", "", DataViewRowState.CurrentRows).ToTable(true, "Contact_Id", "Name", "EmailId", "MobileNo");
        //dt.Columns["Field11"].ColumnName = "EmailId";
        // dt.Columns["Field21"].ColumnName = "MobileNo";
        if (dt.Rows.Count > 0)
        {
            ExportTableData(dt, "Failure Mail List");
        }
        else
        {
            DisplayMessage("No Failure Mail Contacts Exists for Selected Record");
            return;
        }
    }
    protected void btnPendingMail_OnCommand(object sender, CommandEventArgs e)
    {

        DataTable dt = objMailMarket.GetRecordDetail(e.CommandArgument.ToString(), "4");
        dt = new DataView(dt, "Field1=' '", "", DataViewRowState.CurrentRows).ToTable(true, "Contact_Id", "Name", "EmailId", "MobileNo");
        //dt.Columns["Field11"].ColumnName = "EmailId";
        // dt.Columns["Field21"].ColumnName = "MobileNo";
        if (dt.Rows.Count > 0)
        {
            ExportTableData(dt, "Pending Mail List");
        }
        else
        {
            DisplayMessage("No Pending Mail Contacts Exists for Selected Record");
            return;
        }
    }
    protected void btnResendFailureMail_Command(object sender, CommandEventArgs e)
    {

        string Emailid = string.Empty;
        string UserId = string.Empty;
        GridViewRow gvrow = (GridViewRow)((ImageButton)sender).Parent.Parent;

        DataTable dtDetail = objMailMarket.GetRecordDetail(e.CommandArgument.ToString(), "4");
        dtDetail = new DataView(dtDetail, "Field1='False'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtDetail.Rows.Count > 0)
        {
            try
            {
                objMailMarket.CRUDDetailRecord("0", e.CommandArgument.ToString(), "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "9");
            }
            catch
            {

            }

            DisplayMessage("Failure mail send process is started");

        }
        else
        {
            DisplayMessage("No Failure Mail Contacts Exists for Selected Record");
            return;
        }
    }
    protected void btnResendAllMail_Command(object sender, CommandEventArgs e)
    {
        string Emailid = string.Empty;
        string UserId = string.Empty;
        GridViewRow gvrow = (GridViewRow)((ImageButton)sender).Parent.Parent;

        DataTable dtDetail = objMailMarket.GetRecordDetail(e.CommandArgument.ToString(), "4");
        // dtDetail = new DataView(dtDetail, "Field1='False'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtDetail.Rows.Count > 0)
        {
            try
            {
                objMailMarket.CRUDDetailRecord("0", e.CommandArgument.ToString(), "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "7");
            }
            catch
            {

            }

            DisplayMessage("Mail resend process is started");
        }
        else
        {
            DisplayMessage("No Mail Contacts Exists for Selected Record");
            return;
        }
    }
    protected void btnResendPendingMail_Command(object sender, CommandEventArgs e)
    {
        string Emailid = string.Empty;
        string UserId = string.Empty;
        GridViewRow gvrow = (GridViewRow)((ImageButton)sender).Parent.Parent;

        DataTable dtDetail = objMailMarket.GetRecordDetail(e.CommandArgument.ToString(), "4");
        dtDetail = new DataView(dtDetail, "Field1=' '", "", DataViewRowState.CurrentRows).ToTable();
        if (dtDetail.Rows.Count > 0)
        {
            try
            {
                objMailMarket.CRUDDetailRecord("0", e.CommandArgument.ToString(), "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "8");
            }
            catch
            {

            }

            DisplayMessage("Pending mail send process is started");
        }
        else
        {
            DisplayMessage("No Pending Mail Contacts Exists for Selected Record");
            return;
        }
    }
    #endregion
    #endregion
    public string GetDate(string Date)
    {
        string Newdate = string.Empty;

        Newdate = Convert.ToDateTime(Date).ToString(objSys.SetDateFormat());

        return Newdate;
    }
    public string GetEmployeeName(string UserId)
    {
        string EmpName = string.Empty;

        try
        {
            if (ObjUserMaster.GetUserMasterForUserName(UserId, Session["CompId"].ToString()).Rows[0]["Emp_Id"].ToString() == "0")
            {
                EmpName = UserId;
            }
            else
            {
                EmpName = ObjUserMaster.GetUserMasterForUserName(UserId, Session["CompId"].ToString()).Rows[0]["EmpName"].ToString();

            }
        }
        catch
        {
            EmpName = UserId;
        }


        return EmpName;
    }



    #region SelectAllCountry


    protected void chkSelectAllCountry_OnCheckedChanged(object sender, EventArgs e)
    {

        foreach (ListItem li in chkCountry.Items)
        {
            li.Selected = chkSelectAllCountry.Checked;
        }

    }

    #endregion


    #region SelectAllDesignation

    protected void chkSelectAllDesignation_OnCheckedChanged(object sender, EventArgs e)
    {
        foreach (ListItem li in chkDesignation.Items)
        {
            li.Selected = chkSelectAllDesignation.Checked;
        }

    }


    #endregion


    #region SelectAllprodctcategory

    protected void chkSelectAllcategory_OnCheckedChanged(object sender, EventArgs e)
    {
        txtProductcode.Text = "";
        txtProductName.Text = "";
        txtModelNo.Text = "";
        foreach (ListItem li in ChkProductCategory.Items)
        {
            li.Selected = chkSelectAllcategory.Checked;
        }

        if (chkInvoiceRecordOnly.Checked)
        {
            ddlFilter_SelectedIndexChanged(null, null);
        }

    }


    #endregion



    public void BindTreeByFilter(DataTable dt)
    {
        string RoleId = string.Empty;
        string moduleids = string.Empty;
        navTree.DataSource = null;
        navTree.DataBind();
        navTree.Nodes.Clear();

        DataTable dtContactGroup = dt;
        if (dtContactGroup.Rows.Count == 0)
        {
            return;
        }
        DataTable DtGroupMainNode = new DataTable();

        DtGroupMainNode = dt;

        DtGroupMainNode = new DataView(dtContactGroup, "Parent_Id='0'", "Group_Name Asc", DataViewRowState.CurrentRows).ToTable();

        foreach (DataRow datarow in DtGroupMainNode.Rows)
        {
            TreeNode tn = new TreeNode();

            tn = new TreeNode(datarow["Group_Name"].ToString(), datarow["Group_Id"].ToString());

            DataTable dtModuleSaved = new DataView(dtContactGroup, "Parent_Id='" + datarow["Group_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            tn.SelectAction = TreeNodeSelectAction.Expand;
            FillChild1(dtContactGroup, tn.Value, tn);


            navTree.Nodes.Add(tn);
        }

        navTree.DataBind();
        navTree.CollapseAll();
        return;
    }

    //protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (int.Parse(ddlEmployee.SelectedValue) > 0)
    //    {
    //        lblstep1selectedRecord.Text = "Total Record :  0";
    //        DataTable dt = new DataTable();
    //        string EmpId = ddlEmployee.SelectedValue;
    //        string sqlQuery = "SELECT * from Ems_GroupMaster where Parent_Id=0 and IsActive='True' and Group_Id in ( Select Group_Id from Ems_Contact_Group where Contact_Id = "+ EmpId +" ) Order by Group_Name Asc";
    //        dt = objDa.return_DataTable(sqlQuery);
    //        BindTreeByFilter(dt);
    //    }
    //}

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        DataTable dt = new DataTable();
        DataAccessClass objDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        string sqlQuery = @"Select distinct SUMR.User_Id, SEM.Emp_Name from Set_EmployeeMaster SEM inner join Set_UserMaster SUMR on SEM.Emp_Id = SUMR.Emp_Id
                          LEFT JOIN Ems_FavoriteContact EFC ON EFC.User_Id = SUMR.User_Id
                          LEFT JOIN Ems_ContactMaster ECM ON ECM.Trans_Id = EFC.Contact_Id
                          where SEM.IsActive = 'True'";
        dt = objDa.return_DataTable(sqlQuery);
        dt = new DataView(dt, "Emp_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Emp_Name"].ToString() + "/" + dt.Rows[i]["User_Id"].ToString();
        }
        return txt;
    }

    protected void txtEmployeeName_TextChanged(object sender, EventArgs e)
    {
        Chk_SelectAll_Contact.Checked = false;
        try
        {
            string EmpId = string.Empty;
            DataTable dt = new DataTable();
            EmpId = txtEmployeeName.Text.Split('/')[1].ToString();
            hdnEmpId.Value = EmpId;
            string sqlQuery = @"Select ECG.Group_Id,EGM.Group_Name,EGM.Parent_Id from Set_EmployeeMaster SEM inner join Set_UserMaster SUMR on SEM.Emp_Id = SUMR.Emp_Id
                            LEFT JOIN Ems_FavoriteContact EFC ON EFC.User_Id = SUMR.User_Id 
                            LEFT JOIN Ems_ContactMaster ECM ON ECM.Trans_Id = EFC.Contact_Id
                            INNER JOIN Ems_Contact_Group ECG ON ECG.Contact_Id = ECM.Trans_Id
                            INNER JOIN Ems_GroupMaster EGM ON EGM.Group_Id = ECG.Group_Id
                            where SEM.IsActive = 'True' and SUMR.User_Id = convert(nvarchar(100), " + EmpId + ") group by ECG.Group_Id,EGM.Group_Name,,EGM.Parent_Id";
            dt = objDa.return_DataTable(sqlQuery);
            BindTreeByFilter(dt);
        }
        catch
        {

        }
    }
    protected void navTree_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
    {
        if (e != null && e.Node != null)
        {
            try
            {
                if (e.Node.Checked == true)
                {
                    CheckTreeNodeRecursive(e.Node, true);
                    try
                    {
                        SelectChild(e.Node);
                    }
                    catch (Exception)
                    {

                    }
                }
                else
                {
                    CheckTreeNodeRecursive(e.Node, false);
                    UnSelectChild(e.Node);
                }
            }
            catch (Exception)
            {

            }
        }


        if (Session["treeNode"] == null)
        {
            Session["treeNode"] = "0";
        }
        else if (Session["treeNode"].ToString() == "")
        {
            Session["treeNode"] = "0";
        }

        string UserId = string.Empty;
        if (ddlFilter.SelectedValue == "Other")
            UserId = hdnEmpId.Value;
        else
            UserId = Session["UserId"].ToString();

        DataTable dtContactList = new DataTable();

        if (ddlFilter.SelectedValue.ToString() == "All")
            dtContactList = objMailMarket.GetEmsContactListByUserId(Session["treeNode"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), "0");
        else
            dtContactList = objMailMarket.GetEmsContactListByUserId(Session["treeNode"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), UserId);

        //  Session["NewContact"] = objMailMarket.GetEmsContactListByUserId(Session["treeNode"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), UserId);


        dtContactList.Columns["CountryId"].ColumnName = "Country_Id";
        Session["NewContact"] = dtContactList;

        try
        {
            lblstep1selectedRecord.Text = "Total Record : " + ((DataTable)Session["NewContact"]).Rows.Count;
            lblstep1selectedRecord.Enabled = dtContactList.Rows.Count == 0 ? false : true;

            if (lblstep1selectedRecord.Enabled)
            {
                lblstep1selectedRecord.Enabled = (bool)Session["IsDownload"];
            }

        }
        catch
        {
            lblstep1selectedRecord.Text = "Total Record : 0";
            lblstep1selectedRecord.Enabled = false;
        }

    }
    private void SelectChild(TreeNode treeNode)
    {

        DataTable dtGrid = new DataTable();
        int i = 0;
        treeNode.Checked = true;
        Session["treeNode"] += treeNode.Value.ToString() + ",";
        string x = Session["treeNode"].ToString();
        while (i < treeNode.ChildNodes.Count)
        {
            treeNode.ChildNodes[i].Checked = true;
            SelectChild(treeNode.ChildNodes[i]);

            i++;
        }
        try
        {
            CheckParentNodes(treeNode.Parent);

        }
        catch
        {


        }
        navTree.DataBind();
    }
    protected void ddlFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strSql = string.Empty;
        string strCategoryId = "0";
        string strModelId = "0";

        if (txtModelNo.Text != "")
        {
            strModelId = hdnModelId.Value;
        }
        foreach (ListItem li in ChkProductCategory.Items)
        {
            if (li.Selected)
            {
                if (strCategoryId == "0")
                {
                    strCategoryId = li.Value;
                }
                else
                {
                    strCategoryId = strCategoryId + "," + li.Value; ;
                }
            }
        }

        //if (ddlcategorysearch.SelectedIndex > 0)
        //{
        //    strCategoryId = ddlcategorysearch.SelectedValue.Trim();
        //}
        string strproductId = "0";
        if (txtProductcode.Text.Trim() != "")
        {
            strproductId = hdnproductId.Value;
        }
        Chk_SelectAll_Contact.Checked = false;
        lblstep1selectedRecord.Text = "Total Record : 0";
        lblstep1selectedRecord.Enabled = false;
        txtEmployeeName.Visible = false;
        lblEmployee.Visible = false;
        navTree.DataSource = null;
        navTree.DataBind();
        navTree.Nodes.Clear();

        DataTable dt = new DataTable();
        if (ddlFilter.SelectedValue == "Other")
        {
            txtEmployeeName.Visible = true;
            lblEmployee.Visible = true;
        }
        else if (ddlFilter.SelectedValue == "All")
        {

            if (chkInvoiceRecordOnly.Checked)
            {
                if (strCategoryId == "0")
                {
                    strSql = "select distinct Ems_ContactMaster.Trans_Id as Contact_Id,Ems_ContactMaster.Code,Ems_ContactMaster.Name,Ems_ContactMaster.Field1,Ems_ContactMaster.Designation_Id,Ems_ContactMaster.Field4 as Country_Id,isnull( Sys_CountryMaster.Country_Name,'') as CountryName,isnull(Set_DesignationMaster.Designation,'') as desgName from Ems_ContactMaster left join Ems_FavoriteContact on Ems_FavoriteContact.contact_Id = ems_contactmaster.Trans_Id and Ems_FavoriteContact.is_active='True' left join Set_DesignationMaster on Ems_ContactMaster.Designation_Id=Set_DesignationMaster.Designation_Id left join Sys_CountryMaster on Ems_ContactMaster.Field4 =Sys_CountryMaster.Country_Id   where Ems_ContactMaster.Trans_Id in ((select distinct Supplier_Id from Inv_SalesInvoiceHeader inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id =Inv_SalesInvoiceDetail.Invoice_No left join Inv_Product_Category on Inv_SalesInvoiceDetail.Product_Id = Inv_Product_Category.ProductId left join Inv_ProductMaster on Inv_SalesInvoiceDetail.Product_Id = Inv_ProductMaster.ProductId   where Inv_SalesInvoiceHeader.IsActive='True' and (CASE WHEN " + strproductId + " > 0 THEN Inv_SalesInvoiceDetail.Product_Id ELSE 0 END = " + strproductId + ") and (CASE WHEN " + strModelId + " > 0 THEN inv_productmaster.modelno ELSE 0 END = " + strModelId + ") and (CASE WHEN " + strCategoryId + " > 0 THEN Inv_Product_Category.CategoryId ELSE 0 END = " + strCategoryId + ")) union all (select distinct Customer_Id from Inv_SalesInquiryHeader inner join Inv_SalesQuotationHeader on Inv_SalesInquiryHeader.SInquiryID =Inv_SalesQuotationHeader.SInquiry_No inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID left join Inv_Product_Category on Inv_SalesInqDetail.Product_Id = Inv_Product_Category.ProductId left join Inv_ProductMaster on Inv_SalesInqDetail.Product_Id = Inv_ProductMaster.ProductId  where Inv_SalesQuotationHeader.IsActive='True' and (CASE WHEN " + strproductId + " > 0 THEN Inv_SalesInqDetail.Product_Id ELSE 0 END = " + strproductId + ") and (CASE WHEN " + strModelId + " > 0 THEN inv_productmaster.modelno ELSE 0 END = " + strModelId + ") and (CASE WHEN " + strCategoryId + " > 0 THEN Inv_Product_Category.CategoryId ELSE 0 END = " + strCategoryId + ")  ))  and  Ems_ContactMaster.IsActive='True' and Ems_ContactMaster.IsEmail='True' and (CASE WHEN '0' <> '0' THEN Ems_FavoriteContact.User_Id ELSE '0' END ='0') order by Ems_ContactMaster.Name";
                }
                else
                {
                    strSql = "select distinct Ems_ContactMaster.Trans_Id as Contact_Id,Ems_ContactMaster.Code,Ems_ContactMaster.Name,Ems_ContactMaster.Field1,Ems_ContactMaster.Designation_Id,Ems_ContactMaster.Field4 as Country_Id,isnull( Sys_CountryMaster.Country_Name,'') as CountryName,isnull(Set_DesignationMaster.Designation,'') as desgName from Ems_ContactMaster left join Ems_FavoriteContact on Ems_FavoriteContact.contact_Id = ems_contactmaster.Trans_Id and Ems_FavoriteContact.is_active='True' left join Set_DesignationMaster on Ems_ContactMaster.Designation_Id=Set_DesignationMaster.Designation_Id left join Sys_CountryMaster on Ems_ContactMaster.Field4 =Sys_CountryMaster.Country_Id   where Ems_ContactMaster.Trans_Id in ((select distinct Supplier_Id from Inv_SalesInvoiceHeader inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id =Inv_SalesInvoiceDetail.Invoice_No left join Inv_Product_Category on Inv_SalesInvoiceDetail.Product_Id = Inv_Product_Category.ProductId left join Inv_ProductMaster on Inv_SalesInvoiceDetail.Product_Id = Inv_ProductMaster.ProductId  where Inv_SalesInvoiceHeader.IsActive='True' and (CASE WHEN " + strproductId + " > 0 THEN Inv_SalesInvoiceDetail.Product_Id ELSE 0 END = " + strproductId + ") and (CASE WHEN " + strModelId + " > 0 THEN inv_productmaster.modelno ELSE 0 END = " + strModelId + ") and  Inv_Product_Category.CategoryId in (" + strCategoryId + ")) union all (select distinct Customer_Id from Inv_SalesInquiryHeader inner join Inv_SalesQuotationHeader on Inv_SalesInquiryHeader.SInquiryID =Inv_SalesQuotationHeader.SInquiry_No inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID left join Inv_Product_Category on Inv_SalesInqDetail.Product_Id = Inv_Product_Category.ProductId left join Inv_ProductMaster on Inv_SalesInqDetail.Product_Id = Inv_ProductMaster.ProductId  where Inv_SalesQuotationHeader.IsActive='True' and (CASE WHEN " + strproductId + " > 0 THEN Inv_SalesInqDetail.Product_Id ELSE 0 END = " + strproductId + ") and (CASE WHEN " + strModelId + " > 0 THEN inv_productmaster.modelno ELSE 0 END = " + strModelId + ") and Inv_Product_Category.CategoryId in (" + strCategoryId + ")  ))  and  Ems_ContactMaster.IsActive='True' and Ems_ContactMaster.IsEmail='True' and (CASE WHEN '0' <> '0' THEN Ems_FavoriteContact.User_Id ELSE '0' END ='0') order by Ems_ContactMaster.Name";
                }
                //strSql = "select distinct Ems_ContactMaster.Trans_Id as Contact_Id,Ems_ContactMaster.Code,Ems_ContactMaster.Name,Ems_ContactMaster.Field1,Ems_ContactMaster.Designation_Id,Ems_ContactMaster.Field4 as Country_Id,isnull( Sys_CountryMaster.Country_Name,'') as CountryName,isnull(Set_DesignationMaster.Designation,'') as desgName  from Inv_SalesInvoiceHeader inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id =Inv_SalesInvoiceDetail.Invoice_No left join Ems_ContactMaster on Inv_SalesInvoiceHeader.Supplier_Id =Ems_ContactMaster.Trans_Id left join Inv_Product_Category on Inv_SalesInvoiceDetail.Product_Id = Inv_Product_Category.ProductId left join Ems_FavoriteContact on Ems_FavoriteContact.contact_Id = ems_contactmaster.Trans_Id left join Set_DesignationMaster on Ems_ContactMaster.Designation_Id=Set_DesignationMaster.Designation_Id left join Sys_CountryMaster on Ems_ContactMaster.Field4 =Sys_CountryMaster.Country_Id where Inv_SalesInvoiceHeader.IsActive='True' and Ems_ContactMaster.IsActive='True' and Ems_ContactMaster.IsEmail='True' and (Ems_ContactMaster.Field1<>'' and Ems_ContactMaster.Field1<>'0') and (CASE WHEN " + strproductId + " > 0 THEN Inv_SalesInvoiceDetail.Product_Id ELSE 0 END = " + strproductId + ") and (CASE WHEN " + strCategoryId + " > 0 THEN Inv_Product_Category.CategoryId ELSE 0 END = " + strCategoryId + ") and (CASE WHEN '0' <> '0' THEN Ems_FavoriteContact.User_Id ELSE '0' END ='0') order by Ems_ContactMaster.Name";
                dt = objDa.return_DataTable(strSql);
                Session["NewContact"] = dt;
                lblstep1selectedRecord.Text = "Total Record : " + ((DataTable)Session["NewContact"]).Rows.Count;

                lblstep1selectedRecord.Enabled = dt.Rows.Count == 0 ? false : true;

                if (lblstep1selectedRecord.Enabled)
                {
                    lblstep1selectedRecord.Enabled = (bool)Session["IsDownload"];
                }

            }
            else
            {
                BindTree();
            }
        }
        else if (ddlFilter.SelectedValue == "Personal")
        {

            if (chkInvoiceRecordOnly.Checked)
            {

                if (strCategoryId == "0")
                {
                    strSql = "select distinct Ems_ContactMaster.Trans_Id as Contact_Id,Ems_ContactMaster.Code,Ems_ContactMaster.Name,Ems_ContactMaster.Field1,Ems_ContactMaster.Designation_Id,Ems_ContactMaster.Field4 as Country_Id,isnull( Sys_CountryMaster.Country_Name,'') as CountryName,isnull(Set_DesignationMaster.Designation,'') as desgName from Ems_ContactMaster left join Ems_FavoriteContact on Ems_FavoriteContact.contact_Id = ems_contactmaster.Trans_Id and Ems_FavoriteContact.is_active='True' left join Set_DesignationMaster on Ems_ContactMaster.Designation_Id=Set_DesignationMaster.Designation_Id left join Sys_CountryMaster on Ems_ContactMaster.Field4 =Sys_CountryMaster.Country_Id   where Ems_ContactMaster.Trans_Id in ((select distinct Supplier_Id from Inv_SalesInvoiceHeader inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id =Inv_SalesInvoiceDetail.Invoice_No left join Inv_Product_Category on Inv_SalesInvoiceDetail.Product_Id = Inv_Product_Category.ProductId left join Inv_ProductMaster on Inv_SalesInvoiceDetail.Product_Id = Inv_ProductMaster.ProductId   where Inv_SalesInvoiceHeader.IsActive='True' and (CASE WHEN " + strproductId + " > 0 THEN Inv_SalesInvoiceDetail.Product_Id ELSE 0 END = " + strproductId + ") and (CASE WHEN " + strModelId + " > 0 THEN inv_productmaster.modelno ELSE 0 END = " + strModelId + ") and (CASE WHEN " + strCategoryId + " > 0 THEN Inv_Product_Category.CategoryId ELSE 0 END = " + strCategoryId + ")) union all (select distinct Customer_Id from Inv_SalesInquiryHeader inner join Inv_SalesQuotationHeader on Inv_SalesInquiryHeader.SInquiryID =Inv_SalesQuotationHeader.SInquiry_No inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID left join Inv_Product_Category on Inv_SalesInqDetail.Product_Id = Inv_Product_Category.ProductId left join Inv_ProductMaster on Inv_SalesInqDetail.Product_Id = Inv_ProductMaster.ProductId  where Inv_SalesQuotationHeader.IsActive='True' and (CASE WHEN " + strproductId + " > 0 THEN Inv_SalesInqDetail.Product_Id ELSE 0 END = " + strproductId + ") and (CASE WHEN " + strModelId + " > 0 THEN inv_productmaster.modelno ELSE 0 END = " + strModelId + ") and (CASE WHEN " + strCategoryId + " > 0 THEN Inv_Product_Category.CategoryId ELSE 0 END = " + strCategoryId + ")  ))  and  Ems_ContactMaster.IsActive='True' and Ems_ContactMaster.IsEmail='True' and (CASE WHEN " + Session["UserId"].ToString() + " <> '0' THEN Ems_FavoriteContact.User_Id ELSE '0' END ='" + Session["UserId"].ToString() + "') order by Ems_ContactMaster.Name";
                }
                else
                {
                    strSql = "select distinct Ems_ContactMaster.Trans_Id as Contact_Id,Ems_ContactMaster.Code,Ems_ContactMaster.Name,Ems_ContactMaster.Field1,Ems_ContactMaster.Designation_Id,Ems_ContactMaster.Field4 as Country_Id,isnull( Sys_CountryMaster.Country_Name,'') as CountryName,isnull(Set_DesignationMaster.Designation,'') as desgName from Ems_ContactMaster left join Ems_FavoriteContact on Ems_FavoriteContact.contact_Id = ems_contactmaster.Trans_Id and Ems_FavoriteContact.is_active='True' left join Set_DesignationMaster on Ems_ContactMaster.Designation_Id=Set_DesignationMaster.Designation_Id left join Sys_CountryMaster on Ems_ContactMaster.Field4 =Sys_CountryMaster.Country_Id   where Ems_ContactMaster.Trans_Id in ((select distinct Supplier_Id from Inv_SalesInvoiceHeader inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id =Inv_SalesInvoiceDetail.Invoice_No left join Inv_Product_Category on Inv_SalesInvoiceDetail.Product_Id = Inv_Product_Category.ProductId left join Inv_ProductMaster on Inv_SalesInvoiceDetail.Product_Id = Inv_ProductMaster.ProductId   where Inv_SalesInvoiceHeader.IsActive='True' and (CASE WHEN " + strproductId + " > 0 THEN Inv_SalesInvoiceDetail.Product_Id ELSE 0 END = " + strproductId + ") and (CASE WHEN " + strModelId + " > 0 THEN inv_productmaster.modelno ELSE 0 END = " + strModelId + ") and  Inv_Product_Category.CategoryId in (" + strCategoryId + ")) union all (select distinct Customer_Id from Inv_SalesInquiryHeader inner join Inv_SalesQuotationHeader on Inv_SalesInquiryHeader.SInquiryID =Inv_SalesQuotationHeader.SInquiry_No inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID left join Inv_Product_Category on Inv_SalesInqDetail.Product_Id = Inv_Product_Category.ProductId left join Inv_ProductMaster on Inv_SalesInqDetail.Product_Id = Inv_ProductMaster.ProductId  where Inv_SalesQuotationHeader.IsActive='True' and (CASE WHEN " + strproductId + " > 0 THEN Inv_SalesInqDetail.Product_Id ELSE 0 END = " + strproductId + ") and (CASE WHEN " + strModelId + " > 0 THEN inv_productmaster.modelno ELSE 0 END = " + strModelId + ") and Inv_Product_Category.CategoryId in (" + strCategoryId + ")  ))  and  Ems_ContactMaster.IsActive='True' and Ems_ContactMaster.IsEmail='True' and (CASE WHEN '" + Session["UserId"].ToString() + "' <> '0' THEN Ems_FavoriteContact.User_Id ELSE '0' END ='" + Session["UserId"].ToString() + "') order by Ems_ContactMaster.Name";
                }

                //strSql = "select distinct Ems_ContactMaster.Trans_Id as Contact_Id,Ems_ContactMaster.Code,Ems_ContactMaster.Name,Ems_ContactMaster.Field1,Ems_ContactMaster.Designation_Id,Ems_ContactMaster.Field4 as Country_Id,isnull( Sys_CountryMaster.Country_Name,'') as CountryName,isnull(Set_DesignationMaster.Designation,'') as desgName  from Inv_SalesInvoiceHeader inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id =Inv_SalesInvoiceDetail.Invoice_No left join Ems_ContactMaster on Inv_SalesInvoiceHeader.Supplier_Id =Ems_ContactMaster.Trans_Id left join Inv_Product_Category on Inv_SalesInvoiceDetail.Product_Id = Inv_Product_Category.ProductId left join Ems_FavoriteContact on Ems_FavoriteContact.contact_Id = ems_contactmaster.Trans_Id left join Set_DesignationMaster on Ems_ContactMaster.Designation_Id=Set_DesignationMaster.Designation_Id left join Sys_CountryMaster on Ems_ContactMaster.Field4 =Sys_CountryMaster.Country_Id where Inv_SalesInvoiceHeader.IsActive='True' and Ems_ContactMaster.IsActive='True' and Ems_ContactMaster.IsEmail='True' and (Ems_ContactMaster.Field1<>'' and Ems_ContactMaster.Field1<>'0') and (CASE WHEN " + strproductId + " > 0 THEN Inv_SalesInvoiceDetail.Product_Id ELSE 0 END = " + strproductId + ") and (CASE WHEN " + strCategoryId + " > 0 THEN Inv_Product_Category.CategoryId ELSE 0 END = " + strCategoryId + ") and (CASE WHEN " + Session["UserId"].ToString() + " <> '0' THEN Ems_FavoriteContact.User_Id ELSE '0' END =" + Session["UserId"].ToString() + ") order by Ems_ContactMaster.Name";
                dt = objDa.return_DataTable(strSql);
                Session["NewContact"] = dt;
                lblstep1selectedRecord.Text = "Total Record : " + ((DataTable)Session["NewContact"]).Rows.Count;
                lblstep1selectedRecord.Enabled = dt.Rows.Count == 0 ? false : true;

                if (lblstep1selectedRecord.Enabled)
                {
                    lblstep1selectedRecord.Enabled = (bool)Session["IsDownload"];
                }

            }
            else
            {
                strSql = @"Select ECG.Group_Id,EGM.Group_Name,EGM.Parent_Id from Set_EmployeeMaster SEM inner join Set_UserMaster SUMR on SEM.Emp_Id = SUMR.Emp_Id
                                LEFT JOIN Ems_FavoriteContact EFC ON EFC.User_Id = SUMR.User_Id 
                                LEFT JOIN Ems_ContactMaster ECM ON ECM.Trans_Id = EFC.Contact_Id
                                INNER JOIN Ems_Contact_Group ECG ON ECG.Contact_Id = ECM.Trans_Id
                                INNER JOIN Ems_GroupMaster EGM ON EGM.Group_Id = ECG.Group_Id
                                where SEM.IsActive = 'True' and EFC.Is_Active='True' and SUMR.User_Id = convert(nvarchar(100), '" + Session["UserId"].ToString() + "') group by ECG.Group_Id,EGM.Group_Name,EGM.Parent_Id";
                dt = objDa.return_DataTable(strSql);
                BindTreeByFilter(dt);
            }
        }
        else if (ddlFilter.SelectedValue == "0")
        {
            btnRefresh_Click(null, null);

            if (chkInvoiceRecordOnly.Checked)
            {
                if (strCategoryId == "0")
                {
                    strSql = "select distinct Ems_ContactMaster.Trans_Id as Contact_Id,Ems_ContactMaster.Code,Ems_ContactMaster.Name,Ems_ContactMaster.Field1,Ems_ContactMaster.Designation_Id,Ems_ContactMaster.Field4 as Country_Id,isnull( Sys_CountryMaster.Country_Name,'') as CountryName,isnull(Set_DesignationMaster.Designation,'') as desgName from Ems_ContactMaster left join Ems_FavoriteContact on Ems_FavoriteContact.contact_Id = ems_contactmaster.Trans_Id and Ems_FavoriteContact.is_active='True' left join Set_DesignationMaster on Ems_ContactMaster.Designation_Id=Set_DesignationMaster.Designation_Id left join Sys_CountryMaster on Ems_ContactMaster.Field4 =Sys_CountryMaster.Country_Id   where Ems_ContactMaster.Trans_Id in ((select distinct Supplier_Id from Inv_SalesInvoiceHeader inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id =Inv_SalesInvoiceDetail.Invoice_No left join Inv_Product_Category on Inv_SalesInvoiceDetail.Product_Id = Inv_Product_Category.ProductId left join Inv_ProductMaster on Inv_SalesInvoiceDetail.Product_Id = Inv_ProductMaster.ProductId   where Inv_SalesInvoiceHeader.IsActive='True' and (CASE WHEN " + strproductId + " > 0 THEN Inv_SalesInvoiceDetail.Product_Id ELSE 0 END = " + strproductId + ") and (CASE WHEN " + strModelId + " > 0 THEN inv_productmaster.modelno ELSE 0 END = " + strModelId + ") and (CASE WHEN " + strCategoryId + " > 0 THEN Inv_Product_Category.CategoryId ELSE 0 END = " + strCategoryId + ")) union all (select distinct Customer_Id from Inv_SalesInquiryHeader inner join Inv_SalesQuotationHeader on Inv_SalesInquiryHeader.SInquiryID =Inv_SalesQuotationHeader.SInquiry_No inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID left join Inv_Product_Category on Inv_SalesInqDetail.Product_Id = Inv_Product_Category.ProductId left join Inv_ProductMaster on Inv_SalesInqDetail.Product_Id = Inv_ProductMaster.ProductId  where Inv_SalesQuotationHeader.IsActive='True' and (CASE WHEN " + strproductId + " > 0 THEN Inv_SalesInqDetail.Product_Id ELSE 0 END = " + strproductId + ") and (CASE WHEN " + strModelId + " > 0 THEN inv_productmaster.modelno ELSE 0 END = " + strModelId + ") and (CASE WHEN " + strCategoryId + " > 0 THEN Inv_Product_Category.CategoryId ELSE 0 END = " + strCategoryId + ")  ))  and  Ems_ContactMaster.IsActive='True' and Ems_ContactMaster.IsEmail='True' and (CASE WHEN '0' <> '0' THEN Ems_FavoriteContact.User_Id ELSE '0' END ='0') order by Ems_ContactMaster.Name";
                }
                else
                {
                    strSql = "select distinct Ems_ContactMaster.Trans_Id as Contact_Id,Ems_ContactMaster.Code,Ems_ContactMaster.Name,Ems_ContactMaster.Field1,Ems_ContactMaster.Designation_Id,Ems_ContactMaster.Field4 as Country_Id,isnull( Sys_CountryMaster.Country_Name,'') as CountryName,isnull(Set_DesignationMaster.Designation,'') as desgName from Ems_ContactMaster left join Ems_FavoriteContact on Ems_FavoriteContact.contact_Id = ems_contactmaster.Trans_Id and Ems_FavoriteContact.is_active='True' left join Set_DesignationMaster on Ems_ContactMaster.Designation_Id=Set_DesignationMaster.Designation_Id left join Sys_CountryMaster on Ems_ContactMaster.Field4 =Sys_CountryMaster.Country_Id   where Ems_ContactMaster.Trans_Id in ((select distinct Supplier_Id from Inv_SalesInvoiceHeader inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id =Inv_SalesInvoiceDetail.Invoice_No left join Inv_Product_Category on Inv_SalesInvoiceDetail.Product_Id = Inv_Product_Category.ProductId left join Inv_ProductMaster on Inv_SalesInvoiceDetail.Product_Id = Inv_ProductMaster.ProductId  where Inv_SalesInvoiceHeader.IsActive='True' and (CASE WHEN " + strproductId + " > 0 THEN Inv_SalesInvoiceDetail.Product_Id ELSE 0 END = " + strproductId + ") and (CASE WHEN " + strModelId + " > 0 THEN inv_productmaster.modelno ELSE 0 END = " + strModelId + ") and  Inv_Product_Category.CategoryId in (" + strCategoryId + ")) union all (select distinct Customer_Id from Inv_SalesInquiryHeader inner join Inv_SalesQuotationHeader on Inv_SalesInquiryHeader.SInquiryID =Inv_SalesQuotationHeader.SInquiry_No inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID left join Inv_Product_Category on Inv_SalesInqDetail.Product_Id = Inv_Product_Category.ProductId left join Inv_ProductMaster on Inv_SalesInqDetail.Product_Id = Inv_ProductMaster.ProductId  where Inv_SalesQuotationHeader.IsActive='True' and (CASE WHEN " + strproductId + " > 0 THEN Inv_SalesInqDetail.Product_Id ELSE 0 END = " + strproductId + ") and (CASE WHEN " + strModelId + " > 0 THEN inv_productmaster.modelno ELSE 0 END = " + strModelId + ") and Inv_Product_Category.CategoryId in (" + strCategoryId + ")  ))  and  Ems_ContactMaster.IsActive='True' and Ems_ContactMaster.IsEmail='True' and (CASE WHEN '0' <> '0' THEN Ems_FavoriteContact.User_Id ELSE '0' END ='0') order by Ems_ContactMaster.Name";
                }
                // strSql = "select distinct Ems_ContactMaster.Trans_Id as Contact_Id,Ems_ContactMaster.Code,Ems_ContactMaster.Name,Ems_ContactMaster.Field1,Ems_ContactMaster.Designation_Id,Ems_ContactMaster.Field4 as Country_Id,isnull( Sys_CountryMaster.Country_Name,'') as CountryName,isnull(Set_DesignationMaster.Designation,'') as desgName  from Inv_SalesInvoiceHeader inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id =Inv_SalesInvoiceDetail.Invoice_No left join Ems_ContactMaster on Inv_SalesInvoiceHeader.Supplier_Id =Ems_ContactMaster.Trans_Id left join Inv_Product_Category on Inv_SalesInvoiceDetail.Product_Id = Inv_Product_Category.ProductId left join Ems_FavoriteContact on Ems_FavoriteContact.contact_Id = ems_contactmaster.Trans_Id left join Set_DesignationMaster on Ems_ContactMaster.Designation_Id=Set_DesignationMaster.Designation_Id left join Sys_CountryMaster on Ems_ContactMaster.Field4 =Sys_CountryMaster.Country_Id where Inv_SalesInvoiceHeader.IsActive='True' and Ems_ContactMaster.IsActive='True' and Ems_ContactMaster.IsEmail='True' and (Ems_ContactMaster.Field1<>'' and Ems_ContactMaster.Field1<>'0') and (CASE WHEN " + strproductId + " > 0 THEN Inv_SalesInvoiceDetail.Product_Id ELSE 0 END = " + strproductId + ") and (CASE WHEN " + strCategoryId + " > 0 THEN Inv_Product_Category.CategoryId ELSE 0 END = " + strCategoryId + ") and (CASE WHEN '0' <> '0' THEN Ems_FavoriteContact.User_Id ELSE '0' END ='0') order by Ems_ContactMaster.Name";
                dt = objDa.return_DataTable(strSql);
                Session["NewContact"] = dt;
                lblstep1selectedRecord.Text = "Total Record : " + ((DataTable)Session["NewContact"]).Rows.Count;
                lblstep1selectedRecord.Enabled = dt.Rows.Count == 0 ? false : true;

                if (lblstep1selectedRecord.Enabled)
                {
                    lblstep1selectedRecord.Enabled = (bool)Session["IsDownload"];
                }

            }


        }



    }

    protected void Chk_SelectAll_Contact_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_SelectAll_Contact.Checked == true)
        {
            CheckAllNodes(navTree.Nodes);
        }
        else
        {
            UncheckAllNodes(navTree.Nodes);
        }
        if (Session["treeNode"] == null)
        {
            Session["treeNode"] = "0";
        }
        else if (Session["treeNode"].ToString() == "")
        {
            Session["treeNode"] = "0";
        }

        string UserId = string.Empty;
        if (ddlFilter.SelectedValue == "Other")
            UserId = hdnEmpId.Value;
        else
            UserId = Session["UserId"].ToString();

        //string Brand_ID = string.Empty;
        //DataTable dtUser = ObjUserMaster.GetUserMasterByUserId(UserId, Session["CompId"].ToString());
        //if (dtUser.Rows.Count > 0)
        //{
        //    Brand_ID = dtUser.Rows[0]["Role_Id"].ToString();
        //}
        //else
        //{
        //    Brand_ID = Session["BrandId"].ToString();
        //}
        DataTable dtContactList = new DataTable();

        if (ddlFilter.SelectedValue.ToString() == "All")
        {
            dtContactList = objMailMarket.GetEmsContactListByUserId(Session["treeNode"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), "0");
        }
        else if (ddlFilter.SelectedValue.ToString() == "Personal")
        {
            dtContactList = objMailMarket.GetEmsContactListByUserId(Session["treeNode"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), UserId);
        }




        try
        {
            dtContactList.Columns["CountryId"].ColumnName = "Country_Id";


            Session["NewContact"] = dtContactList;


            lblstep1selectedRecord.Text = "Total Record : " + ((DataTable)Session["NewContact"]).Rows.Count;
            lblstep1selectedRecord.Enabled = dtContactList.Rows.Count == 0 ? false : true;
            if (lblstep1selectedRecord.Enabled)
            {
                lblstep1selectedRecord.Enabled = (bool)Session["IsDownload"];
            }

        }
        catch
        {
            lblstep1selectedRecord.Text = "Total Record : 0";
            lblstep1selectedRecord.Enabled = false;

        }
    }
    public void CheckAllNodes(TreeNodeCollection nodes)
    {
        foreach (TreeNode node in nodes)
        {
            node.Checked = true;
            Session["treeNode"] += node.Value.ToString() + ",";
            CheckChildren(node, true);
        }
    }

    public void UncheckAllNodes(TreeNodeCollection nodes)
    {
        foreach (TreeNode node in nodes)
        {
            node.Checked = false;
            CheckChildren(node, false);
        }
        Session["treeNode"] = "";
    }

    private void CheckChildren(TreeNode rootNode, bool isChecked)
    {
        foreach (TreeNode node in rootNode.ChildNodes)
        {
            CheckChildren(node, isChecked);
            node.Checked = isChecked;
            Session["treeNode"] += node.Value.ToString() + ",";
        }
    }

    #region InvoiceRecord
    protected void chkInvoiceRecordOnly_CheckedChanged(object sender, EventArgs e)
    {
        ddlFilter_SelectedIndexChanged(null, null);


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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetDistinctProductCode(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


        string[] txt = new string[dt.Rows.Count];


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["ProductCode"].ToString();
        }


        return txt;
    }


    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        hdnproductId.Value = "0";


        if (((TextBox)sender).Text != "")
        {
            DataTable dt = new DataTable();
            dt = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ((TextBox)sender).Text.ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                chkSelectAllcategory.Checked = false;

                foreach (ListItem li in ChkProductCategory.Items)
                {
                    li.Selected = chkSelectAllcategory.Checked;
                }
                txtModelNo.Text = "";

                hdnproductId.Value = dt.Rows[0]["ProductId"].ToString();

                if (((TextBox)sender).ID == "txtProductcode")
                {
                    txtProductName.Text = dt.Rows[0]["EProductName"].ToString();
                }
                else
                {
                    txtProductcode.Text = dt.Rows[0]["ProductCode"].ToString();
                }

                if (chkInvoiceRecordOnly.Checked)
                {
                    ddlFilter_SelectedIndexChanged(null, null);
                }

            }
            else
            {
                DisplayMessage("select product in suggestion only");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(((TextBox)sender));
                txtProductName.Text = "";
                txtProductcode.Text = "";
            }
        }
        else
        {
            txtProductName.Text = "";
            txtProductcode.Text = "";
            txtProductcode.Focus();
            if (chkInvoiceRecordOnly.Checked)
            {
                ddlFilter_SelectedIndexChanged(null, null);
            }
        }

    }
    #endregion
    protected void ddlcategorysearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtProductcode.Text = "";
        txtProductName.Text = "";
        txtModelNo.Text = "";
        if (chkInvoiceRecordOnly.Checked)
        {
            ddlFilter_SelectedIndexChanged(null, null);
        }
    }

    protected void lblstep1selectedRecord_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["NewContact"];

        dt = dt.DefaultView.ToTable(true, "code", "Name", "Field1", "CountryName", "desgName");
        dt.Columns["Field1"].ColumnName = "Email-Id";
        dt.Columns["CountryName"].ColumnName = "Country";
        dt.Columns["desgName"].ColumnName = "Designation";
        ExportTableData(dt, "ContactList");

    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void chkShowAll_CheckedChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        GetRadioList();
    }

    #region ProductModel

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListModelNo(string prefixText, int count, string contextKey)
    {
        Inv_ModelMaster ObjInvModelMaster = new Inv_ModelMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjInvModelMaster.GetModelMaster(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "True");
        DataTable dtTemp = dt.Copy();

        dt = new DataView(dt, "Model_No like '%" + prefixText.ToString() + "%'", "Model_No Asc", DataViewRowState.CurrentRows).ToTable();


        string[] txt = new string[dt.Rows.Count];


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Model_No"].ToString();
        }


        return txt;
    }

    protected void txtModelNo_TextChanged(object sender, EventArgs e)
    {
        if (txtModelNo.Text != "")
        {


            DataTable dt = new DataView(ObjInvModelMaster.GetModelMaster(StrCompId.ToString(), Session["BrandId"].ToString(), "True"), "Model_No='" + txtModelNo.Text.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                hdnModelId.Value = dt.Rows[0]["Trans_Id"].ToString();
                txtProductcode.Text = "";
                txtProductName.Text = "";
                chkSelectAllcategory.Checked = false;
                foreach (ListItem li in ChkProductCategory.Items)
                {
                    li.Selected = chkSelectAllcategory.Checked;
                }
                if (chkInvoiceRecordOnly.Checked)
                {
                    ddlFilter_SelectedIndexChanged(null, null);
                }
            }
            else
            {
                txtModelNo.Text = "";
                DisplayMessage("Select Model No");
                txtModelNo.Focus();
                //if (chkInvoiceRecordOnly.Checked)
                //{
                //    ddlFilter_SelectedIndexChanged(null, null);
                //}
            }
        }
        else
        {
            if (chkInvoiceRecordOnly.Checked)
            {
                ddlFilter_SelectedIndexChanged(null, null);
            }
        }
    }





    #endregion
}
