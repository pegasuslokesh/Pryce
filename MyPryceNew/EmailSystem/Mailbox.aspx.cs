using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Timers;
using System.IO;
using System.Data.OleDb;
using PegasusDataAccess;
using ClosedXML.Excel;
using System.Web;

public partial class EmailSystem_Mailbox : BasePage
{
    DataAccessClass da = null;
    ES_MailInboxHeader ObjMailBoxHeader = null;
    ES_SendMailHeader objSendMail = null;
    UserDetail objUserDetail = null;
    SystemParameter objSys = null;
    Ems_ContactMaster ObjContactMaster = null;
    ES_EmailMaster_Header objEmailHeader = null;
    PegasusPOP3.PegasusEmailMessage POPMessage = new PegasusPOP3.PegasusEmailMessage();
    CountryMaster objCountry = null;
    DesignationMaster objdesg = null;
    Ems_ContactMaster objContact = null;
    Ems_GroupMaster objContactGroup = null;
    Set_DocNumber ObjDocumentNo = null;
    ES_EmailMasterDetail objEmailDetail = null;
    Ems_Contact_Group objCG = null;
    PageControlCommon objPageCmn = null;
    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
       

        da = new DataAccessClass(Session["DBConnection"].ToString());
        ObjMailBoxHeader = new ES_MailInboxHeader(HttpContext.Current.Session["DBConnection_ES"].ToString());
        objSendMail = new ES_SendMailHeader(HttpContext.Current.Session["DBConnection_ES"].ToString());
        objUserDetail = new UserDetail(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objEmailHeader = new ES_EmailMaster_Header(Session["DBConnection"].ToString());
        objCountry = new CountryMaster(Session["DBConnection"].ToString());
        objdesg = new DesignationMaster(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objContactGroup = new Ems_GroupMaster(Session["DBConnection"].ToString());
        ObjDocumentNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objEmailDetail = new ES_EmailMasterDetail(Session["DBConnection"].ToString());
        objCG = new Ems_Contact_Group(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "265", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            Calender_FromDate.Format = objSys.SetDateFormat();
            Calender_ToDate.Format = objSys.SetDateFormat();
            Page.Title = objSys.GetSysTitle();
            GvEmail.PageSize = PageControlCommon.GetPageSize();
            FillUserEmailAccount();
            ddlFolder.Visible = true;
            //Li_Inbox.Visible = false;
            btnInbox_Click(null, null);
            Page.Form.Attributes.Add("enctype", "multipart/form-data");

           
        }
        if (Session["AddMailReffrence"] != null)
        {
            btngridviewoption_OnClick(null, null);
            txtvalueunregister.Text = "";
            Session["AddMailReffrence"] = null;
        }
        Allpagecode();
    }
    public void Allpagecode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        Common ObjComman = new Common(Session["DBConnection"].ToString());

        //New Code by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("265", (DataTable)Session["ModuleName"]);
        if (dtModule.Rows.Count > 0)
        {
            ViewState["ModuleId"] = dtModule.Rows[0]["Module_Id"].ToString();
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
            strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        }
        else
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }



        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
        if (Session["EmpId"].ToString() == "0")
        {
            lnkexportCountryList.Visible = true;
            lnkexportdesignationList.Visible = true;
            lnkExportContactCompanyList.Visible = true;
            lnkexportContactList.Visible = true;
            lnkexportContactGroup.Visible = true;
            btnConnect.Visible = true;
            btnExport.Visible = true;
            Li_Upload.Visible = true;
        }
        else
        {

            DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "265", HttpContext.Current.Session["CompId"].ToString());
            if (dtAllPageCode.Rows.Count == 0)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

            if (dtAllPageCode.Rows.Count != 0)
            {
                DataTable dt = new DataView(dtAllPageCode, "Op_Id=7", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count > 0)
                {
                    lnkexportCountryList.Visible = true;
                    lnkexportdesignationList.Visible = true;
                    lnkExportContactCompanyList.Visible = true;
                    lnkexportContactList.Visible = true;
                    lnkexportContactGroup.Visible = true;
                    btnExport.Visible = true;
                }
                else
                {
                    lnkexportCountryList.Visible = false;
                    lnkexportdesignationList.Visible = false;
                    lnkExportContactCompanyList.Visible = false;
                    lnkexportContactList.Visible = false;
                    lnkexportContactGroup.Visible = false;
                    btnExport.Visible = false;
                }

                dt = new DataView(dtAllPageCode, "Op_Id=8", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count > 0)
                {
                   
                    btnConnect.Visible = true;
                    Li_Upload.Visible = true;
                }
                else
                {
                   
                    btnConnect.Visible = false;
                }
            }
        }
    }
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
    protected void btnNew_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../EmailSystem/SendMail.aspx','','height=650,width=900,scrollbars=Yes');", true);


    }

    protected void TI_Elapsed(object sender, ElapsedEventArgs e)
    {
        FillInbox();
    }
    protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GvEmail, "Select$" + e.Row.RowIndex);
            e.Row.Attributes["style"] = "cursor:pointer";


            string imageUrl = DataBinder.Eval(e.Row.DataItem, "IsAttach").ToString();
            if (imageUrl.Equals("0"))
            {

                Image image = (Image)e.Row.FindControl("imgAttachment");
                image.Visible = false;
            }


            if (((Label)e.Row.FindControl("lblToOrFr")).Text != "")
            {
                try
                {


                    string Emailid = ((Label)e.Row.FindControl("lblToOrFr")).Text.Substring(0, ((Label)e.Row.FindControl("lblToOrFr")).Text.Length - 1);
                    DataTable dt = objEmailHeader.Get_EmailMasterHeaderTrueByEmailId(Emailid);

                    if (dt.Rows.Count > 0)
                    {
                        DataTable dtdetail = objEmailDetail.Get_EmailMasterDetailAllTrue();


                        dtdetail = new DataView(dtdetail, "Email_ref_Id=" + dt.Rows[0]["Trans_id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

                        if (dtdetail.Rows.Count == 0)
                        {
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#e8e8e8");
                        }

                    }
                }
                catch
                {

                }

            }
            //(Label)e.r
            //e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#");
        }
    }

    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int index = GvEmail.SelectedRow.RowIndex;
        ImageButton img = new ImageButton();
        img = (ImageButton)GvEmail.SelectedRow.Cells[0].FindControl("ImageReadUnRead");
        Label lbl = new Label();
        lbl = ((Label)GvEmail.SelectedRow.Cells[0].FindControl("imgAttachment1"));
        if (lbl.Text == "")
        {
            img.ImageUrl = "../images/readmailnew.png";
        }
        if (lbl.Text == "RE")
        {
            img.ImageUrl = "../images/replynew.png";
        }
        if (lbl.Text == "FW")
        {
            img.ImageUrl = "../images/forwardnew.png";
        }
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../EmailSystem/MailInfo.aspx?MailId=" + ((Label)GvEmail.Rows[index].FindControl("lblTransId")).Text.Trim() + "&&RefId=" + ViewState["BtnId"].ToString() + "','','height=650,width=900,scrollbars=Yes');", true);


    }
    public string GetdateFormate(string Date)
    {
        return Convert.ToDateTime(Date).ToString(objSys.SetDateFormat()) + "  " + Convert.ToDateTime(Date).ToLongTimeString();
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
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }

    public void FillInbox()
    {
        try
        {
            DataTable dt = new DataTable();
            if (ViewState["BtnId"].ToString() == "1")
            {
                if (ddlFolder.SelectedIndex == 0)
                    dt = new DataView(ObjMailBoxHeader.Get_Mail(Session["CompId"].ToString(), "0", "0", Session["EmpId"].ToString()), "Field4=' '", "Date Desc", DataViewRowState.CurrentRows).ToTable();
                else
                    dt = new DataView(ObjMailBoxHeader.Get_Mail(Session["CompId"].ToString(), "0", "0", Session["EmpId"].ToString()), "Field4='" + ddlFolder.SelectedValue + "'", "Date Desc", DataViewRowState.CurrentRows).ToTable();


                if (ddlFieldName.Text == "Trash")
                {
                    dt = new DataView(dt, "IsActive='False' and Field1='" + ddlEmailUser.SelectedItem.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else
                {
                    dt = new DataView(dt, "IsActive='True' and Field1='" + ddlEmailUser.SelectedItem.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }


                Session["DtInbox"] = dt;

                if (dt.Rows.Count > 0)
                {
                    DataTable dtread = new DataView((DataTable)Session["DtInbox"], "Field6='False'", "", DataViewRowState.CurrentRows).ToTable();

                    DataTable dtunread = new DataView((DataTable)Session["DtInbox"], "Field6='True'", "", DataViewRowState.CurrentRows).ToTable();


                    if (dtread.Rows.Count > 0)
                        Session["ReadMail"] = dtread.Rows.Count;
                    else
                        Session["ReadMail"] = 0;

                    if (dtunread.Rows.Count > 0)
                        Session["UnReadMail"] = dtunread.Rows.Count;
                    else
                        Session["UnReadMail"] = 0;
                }
                else
                {
                    Session["ReadMail"] = 0;
                    Session["UnReadMail"] = 0;
                }

            }
            else
            {
                string strstatus;

                if (ViewState["BtnId"].ToString() == "2")
                {
                    strstatus = "Status='S' and Field1='" + ddlEmailUser.SelectedItem.Text.Trim() + "'";
                    dt = new DataView(objSendMail.Get_Mail(Session["CompId"].ToString(), "0", "0", Session["EmpId"].ToString()), strstatus, "ModifiedDate Desc", DataViewRowState.CurrentRows).ToTable();
                    if (ddlFieldName.Text == "Trash")
                    {
                        dt = new DataView(dt, "IsActive='False'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    else
                    {
                        dt = new DataView(dt, "IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    dt.Columns["ModifiedDate"].ColumnName = "Date";
                    Session["DtInbox"] = dt;
                    if (dt.Rows.Count > 0)
                    {
                        DataTable dtread = new DataView((DataTable)Session["DtInbox"], "Field6='False'", "", DataViewRowState.CurrentRows).ToTable();

                        DataTable dtunread = new DataView((DataTable)Session["DtInbox"], "Field6='True'", "", DataViewRowState.CurrentRows).ToTable();


                        if (dtread.Rows.Count > 0)
                            Session["ReadMail"] = dtread.Rows.Count;
                        else
                            Session["ReadMail"] = 0;

                        if (dtunread.Rows.Count > 0)
                            Session["UnReadMail"] = dtunread.Rows.Count;
                        else
                            Session["UnReadMail"] = 0;
                    }
                    else
                    {
                        Session["ReadMail"] = 0;
                        Session["UnReadMail"] = 0;
                    }


                }
                else if (ViewState["BtnId"].ToString() == "3")
                {
                    strstatus = "Status='P'and Field1='" + ddlEmailUser.SelectedItem.Text.Trim() + "'";

                    dt = new DataView(objSendMail.Get_Mail(Session["CompId"].ToString(), "0", "0", Session["EmpId"].ToString()), strstatus, "ModifiedDate Desc", DataViewRowState.CurrentRows).ToTable();

                    if (ddlFieldName.Text == "Trash")
                    {
                        dt = new DataView(dt, "IsActive='False'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    else
                    {
                        dt = new DataView(dt, "IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    dt.Columns["ModifiedDate"].ColumnName = "Date";
                    Session["DtInbox"] = dt;
                    if (dt.Rows.Count > 0)
                    {
                        DataTable dtread = new DataView((DataTable)Session["DtInbox"], "Field6='False'", "", DataViewRowState.CurrentRows).ToTable();

                        DataTable dtunread = new DataView((DataTable)Session["DtInbox"], "Field6='True'", "", DataViewRowState.CurrentRows).ToTable();


                        if (dtread.Rows.Count > 0)
                            Session["ReadMail"] = dtread.Rows.Count;
                        else
                            Session["ReadMail"] = 0;

                        if (dtunread.Rows.Count > 0)
                            Session["UnReadMail"] = dtunread.Rows.Count;
                        else
                            Session["UnReadMail"] = 0;
                    }
                    else
                    {
                        Session["ReadMail"] = 0;
                        Session["UnReadMail"] = 0;
                    }

                }


                //here we wrote code for outbox 
                else if (ViewState["BtnId"].ToString() == "5")
                {
                    strstatus = "Status='O' and Field1='" + ddlEmailUser.SelectedItem.Text.Trim() + "'";

                    dt = new DataView(objSendMail.Get_Mail(Session["CompId"].ToString(), "0", "0", Session["EmpId"].ToString()), strstatus, "ModifiedDate Desc", DataViewRowState.CurrentRows).ToTable();

                    if (ddlFieldName.Text == "Trash")
                    {
                        dt = new DataView(dt, "IsActive='False'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    else
                    {
                        dt = new DataView(dt, "IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    dt.Columns["ModifiedDate"].ColumnName = "Date";
                    Session["DtInbox"] = dt;
                    if (dt.Rows.Count > 0)
                    {
                        DataTable dtread = new DataView((DataTable)Session["DtInbox"], "Field6='False'", "", DataViewRowState.CurrentRows).ToTable();

                        DataTable dtunread = new DataView((DataTable)Session["DtInbox"], "Field6='True'", "", DataViewRowState.CurrentRows).ToTable();


                        if (dtread.Rows.Count > 0)
                            Session["ReadMail"] = dtread.Rows.Count;
                        else
                            Session["ReadMail"] = 0;

                        if (dtunread.Rows.Count > 0)
                            Session["UnReadMail"] = dtunread.Rows.Count;
                        else
                            Session["UnReadMail"] = 0;
                    }
                    else
                    {
                        Session["ReadMail"] = 0;
                        Session["UnReadMail"] = 0;
                    }

                }
                //else if (ViewState["BtnId"] == "")
                //{
                //    dt = (DataTable)(Session["DtInbox"]);
                //    if (dt.Rows.Count > 0)
                //    {
                //        DataTable dtread = new DataView((DataTable)Session["DtInbox"], "Field6='False'", "", DataViewRowState.CurrentRows).ToTable();

                //        DataTable dtunread = new DataView((DataTable)Session["DtInbox"], "Field6='True'", "", DataViewRowState.CurrentRows).ToTable();


                //        if (dtread.Rows.Count > 0)
                //            Session["ReadMail"] = dtread.Rows.Count;
                //        else
                //            Session["ReadMail"] = 0;

                //        if (dtunread.Rows.Count > 0)
                //            Session["UnReadMail"] = dtunread.Rows.Count;
                //        else
                //            Session["UnReadMail"] = 0;
                //    }
                //    else
                //    {
                //        Session["ReadMail"] = 0;
                //        Session["UnReadMail"] = 0;
                //    }

                //}

            }
            Session["DtMailbox"] = dt;

            GvEmail.DataSource = dt;
            GvEmail.DataBind();
            if ((ViewState["BtnId"].ToString() == "2") || (ViewState["BtnId"].ToString() == "3") || (ViewState["BtnId"].ToString() == "5"))
            {
                try
                {
                    GvEmail.HeaderRow.Cells[5].Text = Resources.Attendance.To;
                }
                catch { }
            }
            if (ViewState["BtnId"].ToString() == "1")
            {
                try
                {
                    GvEmail.HeaderRow.Cells[5].Text = Resources.Attendance.From;
                }
                catch { }
            }

            lblTotalRecord.Text = Resources.Attendance.Total_Emails + " : " + dt.Rows.Count.ToString();
            lblReadMail.Text = Resources.Attendance.Read_Mail + " : " + Session["ReadMail"];
            lblUnReadMail.Text = Resources.Attendance.UnRead_Mail + " : " + Session["UnReadMail"];

        }
        catch
        {

        }


    }
    public string getAttachmentstatus(int str)
    {
        string str1 = string.Empty;
        if (str > 0)
        {
            str1 = "../Images/attachmentnew.png";
        }

        return str1;
    }
    public string getReadMailStatus(string str, string strReply)
    {
        string str1 = string.Empty;

        if (str == "True")
        {
            str1 = "../Images/newmailnew.png";

        }
        else
        {
            if (strReply == "RE")
            {
                str1 = "../Images/replynew.png";
            }
            else if (strReply == "FW")
            {
                str1 = "../Images/forwardnew.png";
            }
            else
            {
                str1 = "../Images/readmailnew.png";
            }

        }

        return str1;

    }

    protected void settxtValue(object sender, EventArgs e)
    {
        if ((ddlFieldName.Text == "Read Mail") || (ddlFieldName.Text == "UnRead Mail") || (ddlFieldName.Text == "Trash"))
        {

            txtValue.ReadOnly = true;
            if (ddlFieldName.Text == "Trash")
            {
                ddlDelete.SelectedIndex = 2;
                ddlDelete.Enabled = false;
            }
            else
            {
                ddlDelete.SelectedIndex = 0;
                ddlDelete.Enabled = true;
            }
        }
        else
        {
            txtValue.ReadOnly = false;
            ddlDelete.SelectedIndex = 0;
            ddlDelete.Enabled = true;
        }
        txtValue.Text = "";
    }

    protected void GvEmail_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["DtInbox"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["DtInbox"] = dt;
        GvEmail.DataSource = dt;
        GvEmail.DataBind();
        lblSelectedRecord.Text = "";
        GvEmail.HeaderRow.Focus();
    }

    protected void btnInbox_Click(object sender, EventArgs e)
    {

       
        ViewState["BtnId"] = "1";
        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtValue.Text = "";
        ddlFieldName.SelectedIndex = 0;
        ddlEmailUser.Visible = true;
        ddlMovetoFolder.Visible = true;
        lblMoveTo.Visible = true;
        imgMove.Visible = true;
        ddlMovetoFolder.SelectedIndex = 0;
        ddlFolder.Visible = true;
        //Li_Inbox.Visible = false;
        FillInbox();

        try
        {
            ddlFieldName.Items.RemoveAt(0);
            ListItem li = new ListItem();
            li.Text = "From";
            li.Value = "From";
            ddlFieldName.Items.Insert(0, li);
        }
        catch
        {

        }




    }
    protected void GvEmail_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvEmail.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["DtInbox"];
        GvEmail.DataSource = dt;
        GvEmail.DataBind();
        lblTotalRecord.Text = Resources.Attendance.Total_Emails + " : " + dt.Rows.Count.ToString();
        lblReadMail.Text = Resources.Attendance.Read_Mail + " : " + Session["ReadMail"];
        lblUnReadMail.Text = Resources.Attendance.UnRead_Mail + " : " + Session["UnReadMail"];

        if (lblSelectedRecord.Text != "")
        {
            foreach (GridViewRow gvrow in GvEmail.Rows)
            {
                string tarnsid = ((Label)gvrow.FindControl("lblTransId")).Text;
                if (lblSelectedRecord.Text.Split(',').Contains(tarnsid))
                {
                    ((CheckBox)gvrow.FindControl("chkSelect")).Checked = true;
                }

            }
        }


    }
    protected void btnSend_Click(object sender, EventArgs e)
    {

      
        ddlEmailUser.Visible = true;
        ViewState["BtnId"] = "2";
        ddlFolder.SelectedIndex = 0;
        ddlFolder.Visible = false;
        //Li_Inbox.Visible = true;
        FillInbox();
        try
        {
            GvEmail.HeaderRow.Cells[5].Text = Resources.Attendance.To;
        }
        catch { }

        try
        {
            ddlFieldName.Items.RemoveAt(0);
            ListItem li = new ListItem();
            li.Text = "To";
            li.Value = "To";
            ddlFieldName.Items.Insert(0, li);
        }
        catch
        {

        }

        ddlMovetoFolder.Visible = false;
        lblMoveTo.Visible = false;
        imgMove.Visible = false;


    }
    protected void btnDraft_Click(object sender, EventArgs e)
    {

      
        ddlEmailUser.Visible = true;
        ViewState["BtnId"] = "3";
        ddlFolder.SelectedIndex = 0;
        ddlFolder.Visible = false;
        //Li_Inbox.Visible = true;
        FillInbox();
        try
        {
            GvEmail.HeaderRow.Cells[5].Text = Resources.Attendance.To;
        }
        catch
        {
        }
        try
        {
            ddlFieldName.Items.RemoveAt(0);
            ListItem li = new ListItem();
            li.Text = "To";
            li.Value = "To";
            ddlFieldName.Items.Insert(0, li);
        }
        catch
        {

        }
        ddlMovetoFolder.Visible = false;
        lblMoveTo.Visible = false;
        imgMove.Visible = false;
    }


    protected void btnOutbox_Click(object sender, EventArgs e)
    {

       
        ddlEmailUser.Visible = true;
        ViewState["BtnId"] = "5";
        ddlFolder.SelectedIndex = 0;
        ddlFolder.Visible = false;
        //Li_Inbox.Visible = true;
        FillInbox();
        try
        {
            GvEmail.HeaderRow.Cells[5].Text = Resources.Attendance.To;
        }
        catch
        {
        }
        try
        {
            ddlFieldName.Items.RemoveAt(0);
            ListItem li = new ListItem();
            li.Text = "To";
            li.Value = "To";
            ddlFieldName.Items.Insert(0, li);
        }
        catch
        {

        }
        ddlMovetoFolder.Visible = false;
        lblMoveTo.Visible = false;
        imgMove.Visible = false;
    }

   
    protected void imgDeletedMails_Command(object sender, EventArgs e)
    {
        DataTable dtInbox = ObjMailBoxHeader.Get_Mail(Session["CompId"].ToString(), "0", "0", Session["EmpId"].ToString());

        dtInbox = new DataView(dtInbox, "Field1='" + ddlEmailUser.SelectedItem.Text.Trim() + "' and IsActive='False'", "", DataViewRowState.CurrentRows).ToTable();

        if (ddlFolder.SelectedIndex == 0)
            dtInbox = new DataView(dtInbox, "Field4=' '", "Date Desc", DataViewRowState.CurrentRows).ToTable();
        else
            dtInbox = new DataView(dtInbox, "Field4='" + ddlFolder.SelectedValue + "'", "Date Desc", DataViewRowState.CurrentRows).ToTable();

        Session["DtInbox"] = dtInbox;


        lblTotalRecord.Text = Resources.Attendance.Total_Emails + " : " + dtInbox.Rows.Count.ToString() + "";


        GvEmail.DataSource = dtInbox;
        GvEmail.DataBind();
        if ((ViewState["BtnId"].ToString() == "2") || (ViewState["BtnId"].ToString() == "3"))
        {
            try
            {
                GvEmail.HeaderRow.Cells[5].Text = Resources.Attendance.To;
            }
            catch { }
        }
        if (ViewState["BtnId"].ToString() == "1")
        {
            try
            {
                GvEmail.HeaderRow.Cells[5].Text = Resources.Attendance.From;
            }
            catch { }
        }


    }

    protected void UnReadMail_Click(object sender, EventArgs e)
    {
        DataTable dtInbox = ObjMailBoxHeader.Get_Mail(Session["CompId"].ToString(), "0", "0", Session["EmpId"].ToString());

        dtInbox = new DataView(dtInbox, "Field1='" + ddlEmailUser.SelectedItem.Text.Trim() + "' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();

        if (ddlFolder.SelectedIndex == 0)
            dtInbox = new DataView(dtInbox, "Field4=' '", "Date Desc", DataViewRowState.CurrentRows).ToTable();
        else
            dtInbox = new DataView(dtInbox, "Field4='" + ddlFolder.SelectedValue + "'", "Date Desc", DataViewRowState.CurrentRows).ToTable();

        Session["DtInbox"] = dtInbox;
        // condition = "Subject like '%" + txtValue.Text.Trim() + "%' or From like '%" + txtValue.Text.Trim() + "%' or CC like '%" + txtValue.Text.Trim() + "%'";
        string objSenderID = string.Empty;
        if (sender is Button)
        {
            Button b = (Button)sender;
            objSenderID = b.ID;
        }
        DataTable dt = new DataTable();
        if (objSenderID.ToString() == "lblReadMail")
        {
            dt = new DataView((DataTable)Session["DtInbox"], "Field6='False'", "", DataViewRowState.CurrentRows).ToTable(); ;

        }
        else
        {
            dt = new DataView((DataTable)Session["DtInbox"], "Field6='True'", "", DataViewRowState.CurrentRows).ToTable(); ;

        }
        //Session["DtInbox"] = dt;
        lblTotalRecord.Text = Resources.Attendance.Total_Emails + " : " + dtInbox.Rows.Count.ToString() + "";

        //if (dtInbox.Rows.Count > 0)
        //{
        //    DataTable dtread = new DataView((DataTable)dtInbox, "Field6='False'", "", DataViewRowState.CurrentRows).ToTable();

        //    DataTable dtunread = new DataView((DataTable)dtInbox, "Field6='True'", "", DataViewRowState.CurrentRows).ToTable();


        //    if (dtread.Rows.Count > 0)
        //        Session["ReadMail"] = dtread.Rows.Count;
        //    else
        //        Session["ReadMail"] = 0;

        //    if (dtunread.Rows.Count > 0)
        //        Session["UnReadMail"] = dtunread.Rows.Count;
        //    else
        //        Session["UnReadMail"] = 0;
        //}
        //else
        //{
        //    Session["ReadMail"] = 0;
        //    Session["UnReadMail"] = 0;
        //}
        //lblReadMail.Text = Resources.Attendance.Read_Mail + " : " + Session["ReadMail"];
        //lblUnReadMail.Text = Resources.Attendance.UnRead_Mail + " : " + Session["UnReadMail"];

        GvEmail.DataSource = dt;
        GvEmail.DataBind();
        if ((ViewState["BtnId"].ToString() == "2") || (ViewState["BtnId"].ToString() == "3"))
        {
            try
            {
                GvEmail.HeaderRow.Cells[5].Text = Resources.Attendance.To;
            }
            catch { }
        }
        if (ViewState["BtnId"].ToString() == "1")
        {
            try
            {
                GvEmail.HeaderRow.Cells[5].Text = Resources.Attendance.From;
            }
            catch { }
        }


    }

    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        string condition = string.Empty;
        DateTime FromDate;
        DateTime ToDate;
        DataTable dt;
        if (ddlFieldName.SelectedValue == "All")
        {
            if (ViewState["BtnId"].ToString() == "1")
            {
                DataTable dtInbox = ObjMailBoxHeader.Get_Mail(Session["CompId"].ToString(), "0", "0", Session["EmpId"].ToString());

                dtInbox = new DataView(dtInbox, "Field1='" + ddlEmailUser.SelectedItem.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (ddlFolder.SelectedIndex == 0)
                    dtInbox = new DataView(dtInbox, "Field4=' '", "Date Desc", DataViewRowState.CurrentRows).ToTable();
                else
                    dtInbox = new DataView(dtInbox, "Field4='" + ddlFolder.SelectedValue + "'", "Date Desc", DataViewRowState.CurrentRows).ToTable();


                Session["DtInbox"] = dtInbox;
                condition = "Subject like '%" + txtValue.Text.Trim() + "%' or From like '%" + txtValue.Text.Trim() + "%' or CC like '%" + txtValue.Text.Trim() + "%'";
                dt = new DataView((DataTable)Session["DtInbox"], condition, "", DataViewRowState.CurrentRows).ToTable(); ;
                Session["DtInbox"] = dt;

            }
            else
            {
                string strstatus;
                if (ViewState["BtnId"].ToString() == "2")
                {
                    strstatus = "Status='S' and  Field1='" + ddlEmailUser.SelectedItem.Text.Trim() + "'";
                }
                else
                {
                    strstatus = "Status='P' and  Field1='" + ddlEmailUser.SelectedItem.Text.Trim() + "'";
                }
                dt = new DataView(objSendMail.Get_Mail(Session["CompId"].ToString(), "0", "0", Session["EmpId"].ToString()), strstatus, "ModifiedDate Desc", DataViewRowState.CurrentRows).ToTable();
                dt.Columns["ModifiedDate"].ColumnName = "Date";
                Session["DtInbox"] = dt;
                condition = "Subject like '%" + txtValue.Text.Trim() + "%' or To like '%" + txtValue.Text.Trim() + "%' or CC like '%" + txtValue.Text.Trim() + "%'";
                //dt = new DataView((DataTable)Session["DtInbox"], condition, "", DataViewRowState.CurrentRows).ToTable(); ;
                //Session["DtInbox"] = dt;
            }
        }
        else
        {
            if (txtFromDate.Text != "")
            {
                try
                {
                    objSys.getDateForInput(txtFromDate.Text);
                }
                catch
                {
                    DisplayMessage("Enter From Date in format " + objSys.SetDateFormat() + "");
                    txtFromDate.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtFromDate);
                    return;
                }

                FromDate = Convert.ToDateTime(txtFromDate.Text);

                if (txtToDate.Text != "")
                {
                    try
                    {
                        objSys.getDateForInput(txtToDate.Text);
                    }
                    catch
                    {
                        DisplayMessage("Enter To Date in format " + objSys.SetDateFormat() + "");
                        txtToDate.Text = "";
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtToDate);
                        return;
                    }
                    ToDate = Convert.ToDateTime(txtToDate.Text);
                }
                else
                {
                    ToDate = Convert.ToDateTime(DateTime.Now.ToString(objSys.SetDateFormat()));
                }
                int result = DateTime.Compare(ToDate, FromDate);
                double diffdates;
                diffdates = (ToDate - FromDate).TotalDays;
                if (result < 0)
                {
                    DisplayMessage("ToDate Should be Greater than From Date");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtToDate);
                    return;
                }
                else
                {

                    ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
                    // FromDate = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day, 23, 59, 1);
                    condition = "Date>='" + FromDate.ToString() + "' and Date<='" + ToDate.ToString() + "'";
                    // condition = "Date <=" + ToDate + " and Date >=" + FromDate ;

                }
            }
            else
            {
                if (txtToDate.Text != "")
                {
                    DisplayMessage("Enter From Date");
                    txtFromDate.Focus();
                    return;
                }
            }
            if (condition != "")
            {
                condition = condition + " and ";
                if ((ddlFieldName.Text == "Read Mail") || (ddlFieldName.Text == "UnRead Mail") || (ddlFieldName.Text == "Trash"))
                {
                    if (ddlFieldName.Text == "Read Mail")
                        condition = condition + "Field6='False'";
                    if (ddlFieldName.Text == "UnRead Mail")
                        condition = condition + "Field6='True'";
                    if (ddlFieldName.Text == "Trash")
                        condition = condition + "IsActive='False'";
                }
                else
                {

                    condition = condition + "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
                }
            }
            else
            {
                if ((ddlFieldName.Text == "Read Mail") || (ddlFieldName.Text == "UnRead Mail") || (ddlFieldName.Text == "Trash"))
                {
                    if (ddlFieldName.Text == "Read Mail")
                        condition = "Field6='False'";
                    if (ddlFieldName.Text == "UnRead Mail")
                        condition = "Field6='True'";
                    if (ddlFieldName.Text == "Trash")
                        condition = "IsActive='False'";
                }
                else
                {

                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";


                }

            }
            // DataTable dt;
            if (ViewState["BtnId"].ToString() == "1")
            {
                dt = new DataView(ObjMailBoxHeader.Get_Mail(Session["CompId"].ToString(), "0", "0", Session["EmpId"].ToString()), "Field1='" + ddlEmailUser.SelectedItem.Text.Trim() + "'", "Date Desc", DataViewRowState.CurrentRows).ToTable();

                if (ddlFolder.SelectedIndex == 0)
                    dt = new DataView(dt, "Field4=' '", "Date Desc", DataViewRowState.CurrentRows).ToTable();
                else
                    dt = new DataView(dt, "Field4='" + ddlFolder.SelectedValue + "'", "Date Desc", DataViewRowState.CurrentRows).ToTable();



                Session["DtInbox"] = dt;
            }
            else
            {
                string strstatus;
                if (ViewState["BtnId"].ToString() == "2")
                {
                    strstatus = "Status='S' and IsActive='True' and  Field1='" + ddlEmailUser.SelectedItem.Text.Trim() + "'";
                }
                else
                {
                    strstatus = "Status='P' and IsActive='True' and Field1='" + ddlEmailUser.SelectedItem.Text.Trim() + "'";
                }
                dt = new DataView(objSendMail.Get_Mail(Session["CompId"].ToString(), "0", "0", Session["EmpId"].ToString()), strstatus, "ModifiedDate Desc", DataViewRowState.CurrentRows).ToTable();
                dt.Columns["ModifiedDate"].ColumnName = "Date";
                Session["DtInbox"] = dt;
            }
        }

        dt = new DataView((DataTable)Session["DtInbox"], condition, "", DataViewRowState.CurrentRows).ToTable();

        Session["DtInbox"] = dt;
        lblTotalRecord.Text = Resources.Attendance.Total_Emails + " : " + dt.Rows.Count.ToString() + "";

        if (dt.Rows.Count > 0)
        {
            DataTable dtread = new DataView((DataTable)Session["DtInbox"], "Field6='False'", "", DataViewRowState.CurrentRows).ToTable();

            DataTable dtunread = new DataView((DataTable)Session["DtInbox"], "Field6='True'", "", DataViewRowState.CurrentRows).ToTable();


            if (dtread.Rows.Count > 0)
                Session["ReadMail"] = dtread.Rows.Count;
            else
                Session["ReadMail"] = 0;

            if (dtunread.Rows.Count > 0)
                Session["UnReadMail"] = dtunread.Rows.Count;
            else
                Session["UnReadMail"] = 0;
        }
        else
        {
            Session["ReadMail"] = 0;
            Session["UnReadMail"] = 0;
        }
        lblReadMail.Text = Resources.Attendance.Read_Mail + " : " + Session["ReadMail"];
        lblUnReadMail.Text = Resources.Attendance.UnRead_Mail + " : " + Session["UnReadMail"];


        try
        {
            dt = new DataView(dt, "Field1='" + ddlEmailUser.SelectedItem.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        GvEmail.DataSource = dt;
        GvEmail.DataBind();
        if ((ViewState["BtnId"].ToString() == "2") || (ViewState["BtnId"].ToString() == "3"))
        {
            try
            {
                GvEmail.HeaderRow.Cells[5].Text = Resources.Attendance.To;
            }
            catch { }
        }
        if (ViewState["BtnId"].ToString() == "1")
        {
            try
            {
                GvEmail.HeaderRow.Cells[5].Text = Resources.Attendance.From;
            }
            catch { }
        }
        txtValue.Focus();
    }
    protected void ddlGridSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlGridSize.SelectedValue == "0")
        {
            GvEmail.PageSize = PageControlCommon.GetPageSize();
        }
        else
        {
            GvEmail.PageSize = Convert.ToInt32(ddlGridSize.SelectedValue.ToString());
        }
        GvEmail.DataSource = (DataTable)(Session["DtInbox"]);
        GvEmail.DataBind();
        //FillInbox();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        txtValue.Text = "";
        txtValue.ReadOnly = false;
        txtFromDate.Text = "";
        txtToDate.Text = "";
        ddlFieldName.SelectedIndex = 0;
        ddlDelete.SelectedIndex = 0;
        ddlDelete.Enabled = true;
        FillInbox();
        lblSelectedRecord.Text = "";
    }

    public string GetlblEmail(string TransId)
    {

        DataTable dt = new DataTable();
        if (ViewState["BtnId"].ToString() == "1")
        {
            string s = string.Empty;
            dt = new DataView((DataTable)Session["DtInbox"], "Trans_Id='" + TransId + "'", "", DataViewRowState.CurrentRows).ToTable();
            try
            {
                s = dt.Rows[0]["From"].ToString();
            }
            catch
            {
            }

            string[] words = s.Split('"');
            int upperbound = words.GetUpperBound(0);
            int lowerbound = words.GetLowerBound(0);
            if (upperbound > 0)
            {
                if (words[upperbound - 1] != "")
                    return words[upperbound - 1];
                else
                {
                    if (words[upperbound] != "")
                        return words[upperbound];
                    else
                        return s;
                }
            }
            else
                return s;

        }
        else
        {

            dt = new DataView((DataTable)Session["DtInbox"], "Trans_Id='" + TransId + "'", "", DataViewRowState.CurrentRows).ToTable();
            string s = dt.Rows[0]["To"].ToString();
          
            return s;


        }


    }
    protected void ddlEmailUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["UserEmailSelect"] = ddlEmailUser.SelectedValue.ToString();

        if (ViewState["BtnId"].ToString() == "4")
        {
            btnRefreshRecord_Click(null, null);
        }
        else
        {
            FillInbox();
        }

    }
    protected void btnSetting_Click(object sender, EventArgs e)
    {
        //ResetEmail();
        ddlEmailUser.Visible = false;
        ddlFolder.Visible = false;
        Email_Config.setUserID("","",true);
        //Li_Inbox.Visible = true;
    }
    

    #region delete

    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvEmail.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GvEmail.Rows.Count; i++)
        {
            ((CheckBox)GvEmail.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GvEmail.Rows[i].FindControl("lblTransId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GvEmail.Rows[i].FindControl("lblTransId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GvEmail.Rows[i].FindControl("lblTransId"))).Text.Trim().ToString())
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
        Label lb = (Label)GvEmail.Rows[index].FindControl("lblTransId");
        if (((CheckBox)GvEmail.Rows[index].FindControl("chkSelect")).Checked)
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
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        string deleteType = string.Empty;
        if (ddlDelete.SelectedValue.ToString() == "0")
        {
            DisplayMessage("Select Delete Type");
            ddlDelete.Focus();
            return;
        }
        else
        {
            if (ddlDelete.SelectedValue.ToString() == "1")
            {
                deleteType = "1";
            }
            if (ddlDelete.SelectedValue.ToString() == "2")
            {
                deleteType = "2";
            }

        }

        if (lblSelectedRecord.Text != "")
        {
            if (deleteType == "1")
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {

                        if (ViewState["BtnId"].ToString() == "1")
                        {
                            b = ObjMailBoxHeader.MailInbox_Delete(lblSelectedRecord.Text.Split(',')[j].Trim(), Session["CompId"].ToString(), "0", "0", Session["UserId"].ToString(), DateTime.Now.ToString(), "False");
                        }
                        else if ((ViewState["BtnId"].ToString() == "2") || (ViewState["BtnId"].ToString() == "3"))
                        {

                            b = objSendMail.SendMail_Delete(lblSelectedRecord.Text.Split(',')[j].Trim(), StrCompId.ToString(), StrBrandId.ToString(), strLocationId.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "False");
                        }
                    }
                }
            }
            if (deleteType == "2")
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {

                        if (ViewState["BtnId"].ToString() == "1")
                        {
                            b = ObjMailBoxHeader.MailInbox_DeletePermanent(lblSelectedRecord.Text.Split(',')[j].Trim(), StrCompId, "0", "0");
                        }
                        else if ((ViewState["BtnId"].ToString() == "2") || (ViewState["BtnId"].ToString() == "3"))
                        {
                            b = objSendMail.SendMail_DeletePermanent(lblSelectedRecord.Text.Split(',')[j].Trim(), StrCompId, strLocationId, StrBrandId);

                        }
                    }
                }
            }
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in GvEmail.Rows)
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
                DisplayMessage("Please Select Mail at least One Mail");
            }
            else
            {
                DisplayMessage("Mail Not Deleted");
            }
        }


        if (b != 0)
        {

            lblSelectedRecord.Text = "";
            ViewState["Select"] = null;
            DisplayMessage("Mail Deleted Successfully!");
            FillInbox();
            //GvEmail.DataSource = Session["DtInbox"];
            //GvEmail.DataBind();
        }
    }
    #endregion
    #region MoveTofolder
    protected void IbtnMove_Command(object sender, CommandEventArgs e)
    {
        if (ddlMovetoFolder.SelectedIndex == 0)
        {
            DisplayMessage("First Select Folder");
            return;
        }

        int b = 0;
        if (lblSelectedRecord.Text != "")
        {


            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {

                    if (ViewState["BtnId"].ToString() == "1")
                    {

                        if (ddlMovetoFolder.SelectedIndex == 1)
                        {
                            b = ObjMailBoxHeader.MailInbox_MoveMailtoFolder(lblSelectedRecord.Text.Split(',')[j].Trim(), Session["CompId"].ToString(), Session["EmpId"].ToString(), " ");

                        }
                        else
                        {
                            b = ObjMailBoxHeader.MailInbox_MoveMailtoFolder(lblSelectedRecord.Text.Split(',')[j].Trim(), Session["CompId"].ToString(), Session["EmpId"].ToString(), ddlMovetoFolder.SelectedValue);

                        }

                    }


                }
            }
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in GvEmail.Rows)
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
                DisplayMessage("Please Select at least One Mail");
            }
            else
            {
                DisplayMessage("Mail Not Moved");
            }
        }







        if (b != 0)
        {
            DisplayMessage("Mail Successfully moved");
            btnRefresh_Click(null, null);
        }
    }

    #endregion

    #region Multiple Email

    //protected void IbtnDeleteEmail_Command(object sender, CommandEventArgs e)
    //{
    //    objUserDetail.delete(e.CommandArgument.ToString(), Session["CompId"].ToString());

    //    ResetEmail();

    //}
    //protected void IbtnEditEmail_Command(object sender, CommandEventArgs e)
    //{
    //    DataTable dt = objUserDetail.GetbyTransId(e.CommandArgument.ToString(), Session["CompId"].ToString());
    //    if (dt.Rows.Count != 0)
    //    {
    //        ViewState["Trans_Id"] = dt.Rows[0]["Trans_Id"].ToString();
    //        txtEmail.Text = dt.Rows[0]["Email"].ToString();

    //        txtPasswordEmail.Attributes.Add("Value", dt.Rows[0]["Password"].ToString());
    //        txtSMTP.Text = dt.Rows[0]["Field1"].ToString();
    //        txtPort.Text = dt.Rows[0]["Field2"].ToString();
    //        txtPop3.Text = dt.Rows[0]["Field3"].ToString();
    //        txtpopport.Text = dt.Rows[0]["Field4"].ToString();
    //        chkEnableSSL.Checked = Convert.ToBoolean(dt.Rows[0]["Field5"].ToString());
    //        txtEmailSignature.Content = dt.Rows[0]["Signature"].ToString();
    //        chkDefault.Checked = Convert.ToBoolean(dt.Rows[0]["IsDefault"].ToString());
    //    }
    //}
    //protected void ResetEmail()
    //{

    //    txtPasswordEmail.Attributes.Add("Value", "");
    //    txtEmail.Text = "";
    //    txtEmailSignature.Content = "";
    //    txtPop3.Text = "";
    //    txtpopport.Text = "";
    //    ViewState["Trans_Id"] = null;
    //    txtPort.Text = "";
    //    txtSMTP.Text = "";
    //    chkEnableSSL.Checked = false;
    //    chkDefault.Checked = false;
    //    gvEmailPassuser.DataSource = objUserDetail.GetbyUserId(Session["Userid"].ToString(), Session["CompId"].ToString());
    //    gvEmailPassuser.DataBind();
    //    FillUserEmailAccount();
    //}
    //protected void ImgLogoAdd_Click(object sender, EventArgs e)
    //public void CheckDirectory(string path)
    //{
    //    if (path != "")
    //    {
    //        Directory.CreateDirectory(path);
    //    }
    //}
    //protected void btnSaveSMSEmail_Click(object sender, EventArgs e)
    //{
    //    if (txtEmail.Text == "")
    //    {
    //        DisplayMessage("Enter EmailId");
    //        txtEmail.Focus();
    //        return;
    //    }
    //    if (txtPasswordEmail.Text == "")
    //    {
    //        DisplayMessage("Enter Password");
    //        txtPasswordEmail.Focus();
    //        return;
    //    }
    //    if (txtSMTP.Text == "")
    //    {
    //        DisplayMessage("Enter (SMTP)Outgoing Mail Server");
    //        txtSMTP.Focus();
    //        return;
    //    }
    //    if (txtPort.Text == "")
    //    {
    //        DisplayMessage("Enter SMTP Port");
    //        txtPort.Focus();
    //        return;
    //    }
    //    if (txtPop3.Text == "")
    //    {
    //        DisplayMessage("Enter (POP3) Incoming Mail Server");
    //        txtPop3.Focus();
    //        return;
    //    }
    //    if (txtpopport.Text == "")
    //    {
    //        DisplayMessage("Enter POP3 Port");
    //        txtpopport.Focus();
    //        return;
    //    }
    //    if (txtpopport.Text == "")
    //    {
    //        DisplayMessage("Enter POP3 Port");
    //        txtpopport.Focus();
    //        return;
    //    }
    //    string isdefault = false.ToString();
    //    if (chkDefault.Checked)
    //    {
    //        isdefault = true.ToString();
    //    }
    //    if (ViewState["Trans_Id"] == null)
    //    {
    //        objUserDetail.insert(Session["CompId"].ToString(), Session["UserId"].ToString(), txtEmail.Text, txtPasswordEmail.Text.Trim(), txtEmailSignature.Content.ToString(), isdefault.ToString(), txtSMTP.Text.Trim(), txtPort.Text.Trim(), txtPop3.Text.Trim(), txtpopport.Text.Trim(), chkEnableSSL.Checked.ToString(), "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
    //    }
    //    else
    //    {
    //        objUserDetail.Update(ViewState["Trans_Id"].ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), txtEmail.Text, txtPasswordEmail.Text.Trim(), txtEmailSignature.Content.ToString(), isdefault.ToString(), txtSMTP.Text.Trim(), txtPort.Text.Trim(), txtPop3.Text.Trim(), txtpopport.Text.Trim(), chkEnableSSL.Checked.ToString(), "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

    //    }
    //    ResetEmail();
    //}

    //protected void btnCancelSMSEmail_Click(object sender, EventArgs e)
    //{
    //    ResetEmail();
    //}

    #endregion

    #region Searchpanel
    protected void btnSearch_Click(object sender, EventArgs e)
    {
      
        ViewState["BtnId"] = "4";
        //ResetEmail();
        ddlEmailUser.Visible = true;
        btnRefreshRecord_Click(null, null);
        ddlFolder.Visible = false;
        //Li_Inbox.Visible = true;
    }

    protected void ddlContactoremailOption_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlContactoremailOption.SelectedIndex == 0)
        {
            txtEmailAdressSearch.Visible = true;
            txtContactNameSearch.Visible = false;
        }
        else
        {
            txtEmailAdressSearch.Visible = false;
            txtContactNameSearch.Visible = true;
        }


    }
    protected void txtEmailAdressSearch_OnTextChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        if (txtEmailAdressSearch.Text != "")
        {
            DataTable DtEmail = new DataView(objEmailHeader.Get_EmailMasterHeaderAllTrue(), "Email_Id='" + txtEmailAdressSearch.Text.Trim() + "'", "Email_Id Asc", DataViewRowState.CurrentRows).ToTable();
            if (DtEmail.Rows.Count == 0)
            {
                DisplayMessage("Email Address Not Exists");
                txtEmailAdressSearch.Text = "";
                txtEmailAdressSearch.Focus();
                return;
            }

        }

    }
    protected void txtContactNameSearch_OnTextChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        if (txtContactNameSearch.Text != "")
        {
            string[] ContactName = txtContactNameSearch.Text.Split('/');

            DataTable DtContact = ObjContactMaster.GetContactByContactName(ContactName[0].ToString().Trim());

            if (DtContact.Rows.Count > 0)
            {

                hdnContactMailId.Value = DtContact.Rows[0]["Field1"].ToString();
            }
            else
            {
                DisplayMessage("Contact Name Not Exists");
                txtContactNameSearch.Text = "";
                txtContactNameSearch.Focus();
                return;
            }
        }


    }
    protected void btnbindRecord_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        string strstatus = string.Empty;

        strstatus = "Status='S' and  Field1='" + ddlEmailUser.SelectedItem.Text.Trim() + "'";


        DataTable dt = new DataView(objSendMail.Get_Mail(Session["CompId"].ToString(), "0", "0", Session["EmpId"].ToString()), strstatus, "ModifiedDate Desc", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {

            if (ddlPageType.SelectedIndex != 0)
            {

                dt = new DataView(dt, "Ref_Type='" + ddlPageType.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            if (ddlContactoremailOption.SelectedIndex == 0)
            {
                if (txtEmailAdressSearch.Text != "")
                {

                    dt = new DataView(dt, "To like '%" + txtEmailAdressSearch.Text + "%' or Cc like '%" + txtEmailAdressSearch.Text + "%' or BCc like '%" + txtEmailAdressSearch.Text + "%' ", "", DataViewRowState.CurrentRows).ToTable();
                }

            }
            else
            {
                if (txtContactNameSearch.Text != "" && hdnContactMailId.Value != "")
                {
                    dt = new DataView(dt, "To like '%" + hdnContactMailId.Value + "%' or Cc like '%" + hdnContactMailId.Value + "%' or BCc like '%" + hdnContactMailId.Value + "%' ", "", DataViewRowState.CurrentRows).ToTable();


                }

            }

        }



        Session["DtInbox"] = dt;
        if (dt.Rows.Count > 0)
        {
            GvEmailSearch.DataSource = dt;
            GvEmailSearch.DataBind();
        }
        else
        {
            GvEmailSearch.DataSource = null;
            GvEmailSearch.DataBind();

        }
        lblTotalRecordsSearch.Text = Resources.Attendance.Total_Emails + " : " + dt.Rows.Count.ToString() + "";

    }
    protected void btnRefreshRecord_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        ddlPageType.SelectedIndex = 0;
        ddlContactoremailOption.SelectedIndex = 0;
        txtEmailAdressSearch.Text = "";
        txtEmailAdressSearch.Visible = true;
        txtContactNameSearch.Text = "";
        txtContactNameSearch.Visible = false;

        string strstatus = string.Empty;
        try
        {
            strstatus = "Status='S' and  Field1='" + ddlEmailUser.SelectedItem.Text.Trim() + "'";




            DataTable dt = new DataView(objSendMail.Get_Mail(Session["CompId"].ToString(), "0", "0", Session["EmpId"].ToString()), strstatus, "ModifiedDate Desc", DataViewRowState.CurrentRows).ToTable();
            Session["DtInbox"] = dt;
            GvEmailSearch.DataSource = dt;
            GvEmailSearch.DataBind();
            lblTotalRecordsSearch.Text = Resources.Attendance.Total_Emails + " : " + dt.Rows.Count.ToString() + "";
        }

        catch
        {
        }


    }

    protected void GvEmailSearch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        int index = GvEmailSearch.SelectedRow.RowIndex;
        ImageButton img = new ImageButton();
        img = (ImageButton)GvEmailSearch.SelectedRow.Cells[0].FindControl("ImageReadUnRead");
        Label lbl = new Label();
        lbl = ((Label)GvEmailSearch.SelectedRow.Cells[0].FindControl("imgAttachment1"));
        if (lbl.Text == "")
        {
            img.ImageUrl = "../images/readmailnew.png";
        }
        if (lbl.Text == "RE")
        {
            img.ImageUrl = "../images/replynew.png";
        }
        if (lbl.Text == "FW")
        {
            img.ImageUrl = "../images/forwardnew.png";
        }
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../EmailSystem/MailInfo.aspx?MailId=" + ((Label)GvEmailSearch.Rows[index].FindControl("lblTransId")).Text.Trim() + "&&RefId=" + ViewState["BtnId"].ToString() + "','','height=650,width=900,scrollbars=Yes');", true);


    }
    protected void GvEmailSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvEmailSearch.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["DtInbox"];
        GvEmailSearch.DataSource = dt;
        GvEmailSearch.DataBind();
        lblTotalRecordsSearch.Text = Resources.Attendance.Total_Emails + " : " + dt.Rows.Count.ToString();

    }
    protected void GvEmailSearch_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GvEmailSearch, "Select$" + e.Row.RowIndex);
            e.Row.Attributes["style"] = "cursor:pointer";


            string imageUrl = DataBinder.Eval(e.Row.DataItem, "IsAttach").ToString();
            if (imageUrl.Equals("0"))
            {

                Image image = (Image)e.Row.FindControl("imgAttachment");
                image.Visible = false;
            }
        }
    }

    protected void GvEmailSearch_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["DtSearch"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["DtInbox"] = dt;
        GvEmailSearch.DataSource = dt;
        GvEmailSearch.DataBind();

        GvEmailSearch.HeaderRow.Focus();
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetEmailList(string prefixText, int count, string contextKey)
    {
        ES_EmailMaster_Header objEmailHeader = new ES_EmailMaster_Header(System.Web.HttpContext.Current.Session["DBConnection"].ToString());
        DataTable DtEmail = new DataView(objEmailHeader.Get_EmailMasterHeaderAllTrue(), "Email_Id Like('%" + prefixText + "%')", "Email_Id Asc", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[DtEmail.Rows.Count];

        if (DtEmail.Rows.Count > 0)
        {
            for (int i = 0; i < DtEmail.Rows.Count; i++)
            {
                txt[i] += DtEmail.Rows[i]["Email_Id"].ToString();
            }

        }

        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContact(string prefixText, int count, string contextKey)
    {
        //Set_CustomerMaster objcustomer = new Set_CustomerMaster();


        //DataTable dtCustomer = objcustomer.GetCustomerAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["CompId"].ToString());
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(System.Web.HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCustomer = ObjContactMaster.GetContactTrueAllData();

        DataTable dtMain = new DataTable();
        dtMain = dtCustomer.Copy();


        string filtertext = "Name like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "Name asc", DataViewRowState.CurrentRows).ToTable();
        //if (dtCon.Rows.Count == 0)
        //{
        //    dtCon = dtCustomer;
        //}
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Trans_Id"].ToString();
            }
        }
        return filterlist;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCountryName(string prefixText, int count, string contextKey)
    {
        CountryMaster objCountryMaster = new CountryMaster(System.Web.HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objCountryMaster.GetCountryMaster(), "Country_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Country_Name"].ToString() + "/" + dt.Rows[i]["Country_Id"].ToString();
        }
        return txt;
    }
    #endregion


    #region Upload
    protected void btnupload_Click(object sender, EventArgs e)
    {
       
        //ResetEmail();
        ddlEmailUser.Visible = false;

        ddlFolder.Visible = false;
        //Li_Inbox.Visible = true;
        Allpagecode();
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        if (rbtnCreatecontact.Checked)
        {

            dt = objEmailHeader.Get_Emailall_ByOpType("8");
            if (dt.Rows.Count > 0)
            {

                ExportTableData(dt, "EmailList");
            }
        }
        else
        {

            dt = objEmailHeader.Get_Emailall_ByOpType("9");
            if (dt.Rows.Count > 0)
            {

                ExportTableData(dt, "EmailList");


            }
        }
        Allpagecode();

    }
    protected void rbtncontact_OnCheckedChanged(object sender, EventArgs e)
    {
        btnuploadoption.Visible = true;
        btngridviewoption.Visible = true;

    }
    protected void btnuploadoption_OnClick(object sender, EventArgs e)
    {

        rbtnCreatecontact.Enabled = false;
        rbtnexistcontact.Enabled = false;
        btnuploadoption.Visible = false;
        btngridviewoption.Visible = false;
        int counter = 0;
        Common ObjComman = new Common(Session["DBConnection"].ToString());
        pnlgridunregisterwithexistscontact.Visible = false;
        pnluploadEmaillist.Visible = true;
        Allpagecode();

        DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), ViewState["ModuleId"].ToString(), "265", HttpContext.Current.Session["CompId"].ToString());
        if (dtAllPageCode.Rows.Count != 0)
        {

            DataTable dt = new DataView(dtAllPageCode, "Op_Id=7", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                counter = 1;
            }
        }
        if (rbtnCreatecontact.Checked)
        {
            if (counter != 0)
            {
                lnkexportCountryList.Visible = true;
                lnkexportdesignationList.Visible = true;
                lnkExportContactCompanyList.Visible = true;
                lnkexportContactGroup.Visible = true;
                btnExport.Visible = true;
            }

            lnkexportContactList.Visible = false;

        }
        else
        {

            if (counter != 0)
            {
                lnkexportCountryList.Visible = true;
                lnkexportContactList.Visible = true;
                btnExport.Visible = true;
            }

            lnkexportdesignationList.Visible = false;
            lnkExportContactCompanyList.Visible = false;

            lnkexportContactGroup.Visible = false;
        }

    }
    protected void btngridviewoption_OnClick(object sender, EventArgs e)
    {
        rbtnCreatecontact.Enabled = false;
        rbtnexistcontact.Enabled = false;
        pnlgridunregisterwithexistscontact.Visible = true;
        pnluploadEmaillist.Visible = false;
        btnuploadoption.Visible = false;
        btngridviewoption.Visible = false;

        DataTable dt = new DataTable();
        if (rbtnCreatecontact.Checked)
        {

            dt = objEmailHeader.Get_Emailall_ByOpType("8");
            if (dt.Rows.Count > 0)
            {
                ViewState["dtEmailList"] = dt;

                gvUnregisteredEmailexistcontact.DataSource = dt;
                gvUnregisteredEmailexistcontact.DataBind();
                try
                {
                    gvUnregisteredEmailexistcontact.Columns[0].Visible = true;
                    gvUnregisteredEmailexistcontact.Columns[1].Visible = false;
                    gvUnregisteredEmailexistcontact.Columns[2].Visible = true;
                    gvUnregisteredEmailexistcontact.Columns[3].Visible = true;
                    gvUnregisteredEmailexistcontact.Columns[4].Visible = false;
                    gvUnregisteredEmailexistcontact.Columns[5].Visible = false;
                    gvUnregisteredEmailexistcontact.Columns[6].Visible = true;

                }
                catch
                {
                }
            }

            btnSave_UnregisterEmail.Visible = false;
        }
        else
        {

            dt = objEmailHeader.Get_Emailall_ByOpType("9");
            if (dt.Rows.Count > 0)
            {
                ViewState["dtEmailList"] = dt;
                gvUnregisteredEmailexistcontact.DataSource = dt;
                gvUnregisteredEmailexistcontact.DataBind();
                try
                {
                    gvUnregisteredEmailexistcontact.Columns[0].Visible = true;
                    gvUnregisteredEmailexistcontact.Columns[1].Visible = true;
                    gvUnregisteredEmailexistcontact.Columns[2].Visible = false;
                    gvUnregisteredEmailexistcontact.Columns[3].Visible = true;
                    gvUnregisteredEmailexistcontact.Columns[4].Visible = true;
                    gvUnregisteredEmailexistcontact.Columns[5].Visible = true;
                    gvUnregisteredEmailexistcontact.Columns[6].Visible = false;
                }
                catch
                {
                }
            }

            btnSave_UnregisterEmail.Visible = true;
        }
        lblTotalRecordsUnRegisEmail.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count + "";
        Allpagecode();
    }
    protected void lnkexportCountryList_OnClick(object sender, EventArgs e)
    { //for get country list
        DataTable dt = objCountry.GetCountryMaster();
        try
        {
            dt = dt.DefaultView.ToTable(true, "Country_Id", "Country_Name", "Country_Name_L");
            dt.Select("Country_Name", "Asc");
        }
        catch
        {
        }
        if (dt.Rows.Count > 0)
        {
            ExportTableData(dt, "CountryList");
        }

    }
    protected void lnkexportdesignationList_OnClick(object sender, EventArgs e)
    {
        //for get designation list

        DataTable dt = objdesg.GetDesignationMaster();
        try
        {
            dt = dt.DefaultView.ToTable(true, "Designation_Id", "Designation", "Designation_L");
            dt = new DataView(dt, "", "Designation asc", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (dt.Rows.Count > 0)
        {
            ExportTableData(dt, "DesignationList");
        }
    }
    protected void lnkExportContactCompanyList_OnClick(object sender, EventArgs e)
    {
        //for get contact company

        DataTable dt = objContact.GetContactTrueAllData();
        try
        {
            dt = new DataView(dt, "Status='Company'", "", DataViewRowState.CurrentRows).ToTable();
            dt.Columns["Field1"].ColumnName = "EmailId";
            dt = dt.DefaultView.ToTable(true, "Trans_Id", "Code", "Name", "EmailId", "CountryName", "Designation");
            dt = new DataView(dt, "", "Name asc", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (dt.Rows.Count > 0)
        {
            ExportTableData(dt, "Contact Company List");
        }
    }
    protected void lnkexportContactList_OnClick(object sender, EventArgs e)
    {
        //for get contact list

        DataTable dt = objContact.GetContactTrueAllData();
        try
        {

            dt.Columns["Field1"].ColumnName = "EmailId";
            dt = dt.DefaultView.ToTable(true, "Trans_Id", "Code", "Name", "Status", "C_Name", "EmailId", "CountryName", "Designation");

            dt = new DataView(dt, "", "Name asc", DataViewRowState.CurrentRows).ToTable();

        }
        catch
        {
        }
        if (dt.Rows.Count > 0)
        {
            ExportTableData(dt, "Contact List");
        }

    }
    protected void lnkexportContactGroup_OnClick(object sender, EventArgs e)
    {
        //for get contact group

        DataTable dt = objContactGroup.GetGroupMasterTrueAllData();

        try
        {
            dt = new DataView(dt, "Group_Name<>'Customer' and Group_Name<>'Supplier'", "", DataViewRowState.CurrentRows).ToTable();
            dt = dt.DefaultView.ToTable(true, "Group_Id", "Group_Name");
            dt = new DataView(dt, "", "Group_Name asc", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (dt.Rows.Count > 0)
        {
            ExportTableData(dt, "Contact Group List");
        }
    }

    public void ExportTableData(DataTable Dt_Data,string FName)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(Dt_Data, FName);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + FName + ".xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }
    
    protected void btnConnect_Click(object sender, EventArgs e)
    {
        if (fileLoad.HasFile)
        {
            string Path = string.Empty;
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
                Path = Server.MapPath("~/Temp/" + fileLoad.FileName);
                DataTable dt = ConvetExcelToDataTable(Path);

                if (dt != null)
                {
                    int b = 0;
                    gvunregisteredEmaillist.DataSource = dt;
                    gvunregisteredEmaillist.DataBind();

                    ViewState["dtEmailList"] = dt;

                    foreach (GridViewRow gvrow in gvunregisteredEmaillist.Rows)
                    {
                        pnlgridrecord.Visible = true;
                        pnluploadEmaillist.Visible = false;
                        rbtnCreatecontact.Enabled = false;
                        rbtnexistcontact.Enabled = false;
                        btnsavefinalemail.Visible = true;
                        btnResetEmailListall.Visible = true;

                        try
                        {
                            if (rbtnCreatecontact.Checked)
                            {

                                if (gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[4].Text != "" && gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[4].Text != "0")
                                {
                                    try
                                    {
                                        gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[4].Text = objContact.GetContactTrueById(gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[4].Text).Rows[0]["Name"].ToString();
                                    }
                                    catch
                                    {
                                    }
                                }
                                if (gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[5].Text != "" && gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[5].Text != "0")
                                {
                                    try
                                    {
                                        gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[5].Text = objCountry.GetCountryMasterById(gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[5].Text).Rows[0]["Country_Name"].ToString();
                                    }
                                    catch
                                    {
                                    }
                                }
                                if (gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[6].Text != "" && gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[6].Text != "0")
                                {
                                    try
                                    {

                                        gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[6].Text = objdesg.GetDesignationMasterById(gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[6].Text).Rows[0]["Designation"].ToString();
                                    }
                                    catch
                                    {
                                    }
                                }
                                if (gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[7].Text != "" && gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[7].Text != "0")
                                {
                                    try
                                    {
                                        gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[7].Text = objContactGroup.GetGroupMasterByGroupId(gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[7].Text).Rows[0]["Group_Name"].ToString();
                                    }
                                    catch
                                    {
                                    }
                                }

                                if (gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[8].Text != "" && gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[8].Text != "0")
                                {
                                    try
                                    {
                                        gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[8].Text = objContactGroup.GetGroupMasterByGroupId(gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[8].Text).Rows[0]["Group_Name"].ToString();
                                    }
                                    catch
                                    {
                                    }
                                }
                                if (gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[9].Text != "" && gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[9].Text != "0")
                                {
                                    try
                                    {
                                        gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[9].Text = objContactGroup.GetGroupMasterByGroupId(gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[9].Text).Rows[0]["Group_Name"].ToString();
                                    }
                                    catch
                                    {
                                    }
                                }
                                if (gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[10].Text != "" && gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[10].Text != "0")
                                {
                                    try
                                    {
                                        gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[10].Text = objContactGroup.GetGroupMasterByGroupId(gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[10].Text).Rows[0]["Group_Name"].ToString();
                                    }
                                    catch
                                    {
                                    }
                                }
                                if (gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[11].Text != "" && gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[11].Text != "0")
                                {
                                    try
                                    {
                                        gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[11].Text = objContactGroup.GetGroupMasterByGroupId(gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[11].Text).Rows[0]["Group_Name"].ToString();
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                            else
                            {
                                if (gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[2].Text != "" && gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[2].Text != "0")
                                {
                                    try
                                    {
                                        gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[2].Text = objContact.GetContactTrueById(gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[2].Text).Rows[0]["Name"].ToString();
                                    }
                                    catch
                                    {
                                    }
                                }
                                if (gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[3].Text != "" && gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[3].Text != "0")
                                {
                                    try
                                    {
                                        gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[3].Text = objCountry.GetCountryMasterById(gvunregisteredEmaillist.Rows[gvrow.RowIndex].Cells[3].Text).Rows[0]["Country_Name"].ToString();
                                    }
                                    catch
                                    {
                                    }
                                }

                            }
                        }
                        catch
                        {
                            gvunregisteredEmaillist.DataSource = null;
                            gvunregisteredEmaillist.DataBind();
                            DisplayMessage("Upload only unregistered Email list excel file");
                            btnResetEmailList_Click(null,null);
                            return;
                        }
                    }
                }
                else
                {
                    DisplayMessage("Record Not Found");
                    return;
                }
            }
        }
        else
        {
            DisplayMessage("File Not Found");
            return;
        }
        Allpagecode();
        fileLoad.FileContent.Dispose();
    }
    protected void btnResetEmailList_Click(object sender, EventArgs e)
    {
        lnkexportCountryList.Visible = false;
        lnkexportdesignationList.Visible = false;
        lnkExportContactCompanyList.Visible = false;
        lnkexportContactList.Visible = false;
        lnkexportContactGroup.Visible = false;
        btnExport.Visible = false;
        rbtnCreatecontact.Checked = false;
        rbtnexistcontact.Checked = false;
        pnluploadEmaillist.Visible = false;
        pnlgridrecord.Visible = false;
        gvunregisteredEmaillist.DataSource = null;
        gvunregisteredEmaillist.DataBind();
        rbtnCreatecontact.Enabled = true;
        rbtnexistcontact.Enabled = true;
        ViewState["dtEmailList"] = null;
        btnsavefinalemail.Visible = false;
        btnResetEmailListall.Visible = false;
        btnuploadoption.Visible = false;
        btngridviewoption.Visible = false;
        gvUnregisteredEmailexistcontact.DataSource = null;
        gvUnregisteredEmailexistcontact.DataBind();
        pnlgridunregisterwithexistscontact.Visible = false;
        Allpagecode();
    }
    public DataTable ConvetExcelToDataTable(string path)
    {
        DataTable dt = new DataTable();
        string strcon = string.Empty;
        if (Path.GetExtension(path) == ".xls")
        {
            strcon = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + path + "; Extended Properties =\"Excel 8.0;HDR=YES;\"";
        }
        else if (Path.GetExtension(path) == ".xlsx")
        {
            strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 12.0;HDR=YES;iMEX=1\"";
        }
        try
        {
            OleDbConnection oledbConn = new OleDbConnection(strcon);
            oledbConn.Open();
            DataTable Sheets = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string strquery = "select * from [" + Sheets.Rows[0]["Table_Name"].ToString() + "] ";
            OleDbCommand com = new OleDbCommand(strquery, oledbConn);
            DataSet ds = new DataSet();
            OleDbDataAdapter oledbda = new OleDbDataAdapter(com);
            oledbda.Fill(ds, Sheets.Rows[0]["Table_Name"].ToString());
            oledbConn.Close();
            dt = ds.Tables[0];
        }
        catch 
        {
            DisplayMessage("Excel file should in correct format");
        }
        return dt;
    }
    protected void btnsaveemail_Click(object sender, EventArgs e)
    {
        DataAccessClass da = new DataAccessClass(Session["DBConnection"].ToString());
        string docno = string.Empty;
        docno = ObjDocumentNo.GetDocumentNo(true, "0", false, "8", "19", "0", HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(),"0", HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        int counter = 0;
        DataTable dt = new DataTable();

        if (ViewState["dtEmailList"] != null)
        {
            dt = (DataTable)ViewState["dtEmailList"];

            if (rbtnCreatecontact.Checked)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[2].ToString() != " " && dr[3].ToString() != " ")
                    {

                        int b = 0;

                        b = ObjContactMaster.InsertContactMaster(docno, dr[2].ToString(), dr[2].ToString(), "", "0", dr[6].ToString(), "0", dr[4].ToString(), true.ToString(), true.ToString(), dr[3].ToString(), false.ToString(), dr[1].ToString(), "", "", dr[5].ToString(), "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "0");


                        if (b != 0)
                        {
                            counter++;
                            ObjContactMaster.UpdateContactMaster(b.ToString(), docno + b.ToString());


                            string sql = "update ES_EmailMaster_Header set Field1='" + dr[5].ToString() + "' where Trans_Id=" + dr[0].ToString() + "";
                            da.execute_Command(sql);

                            //  objEmailDetail.ES_EmailMasterDetail_DeleteByEmailRefIdandEmailRefType(b.ToString(), "Contact");

                            objEmailDetail.ES_EmailMasterDetail_Insert(b.ToString(), "Contact", dr[0].ToString(), true.ToString(), "", "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                            if (dr[7].ToString() != " " && dr[7].ToString() != "0")
                            {
                                objCG.InsertContactGroup(b.ToString(), dr[7].ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), docno + b.ToString(), "", "", "", "", true.ToString(), DateTime.Now.ToString());

                            }
                            if (dr[8].ToString() != " " && dr[8].ToString() != "0")
                            {
                                objCG.InsertContactGroup(b.ToString(), dr[8].ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), docno + b.ToString(), "", "", "", "", true.ToString(), DateTime.Now.ToString());

                            }
                            if (dr[9].ToString() != " " && dr[9].ToString() != "0")
                            {
                                objCG.InsertContactGroup(b.ToString(), dr[9].ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), docno + b.ToString(), "", "", "", "", true.ToString(), DateTime.Now.ToString());

                            }
                            if (dr[10].ToString() != " " && dr[10].ToString() != "0")
                            {
                                objCG.InsertContactGroup(b.ToString(), dr[10].ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), docno + b.ToString(), "", "", "", "", true.ToString(), DateTime.Now.ToString());

                            }
                            if (dr[11].ToString() != "" && dr[11].ToString() != "0")
                            {
                                objCG.InsertContactGroup(b.ToString(), dr[11].ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), docno + b.ToString(), "", "", "", "", true.ToString(), DateTime.Now.ToString());

                            }


                        }
                    }

                }
            }
            else
            {

                foreach (DataRow dr in dt.Rows)
                {

                    bool isdefault = true;
                    if (dr[2].ToString() != "0" && dr[3].ToString() != "0" && dr[2].ToString() != " " && dr[3].ToString() != " ")
                    {
                        counter++;
                        DataTable dtExists = objEmailDetail.Get_EmailMasterDetailAllTrue();
                        try
                        {
                            dtExists = new DataView(dtExists, "Ref_Id=" + dr[2].ToString() + " and Email_Ref_Type='Contact' and Is_Default='True'", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {
                        }
                        if (dtExists.Rows.Count > 0)
                        {
                            isdefault = false;
                        }

                        string sql = "update ES_EmailMaster_Header set Field1='" + dr[3].ToString() + "' where Trans_Id=" + dr[0].ToString() + "";
                        da.execute_Command(sql);

                        if (isdefault)
                        {
                            sql = "update Ems_ContactMaster set Field1='" + dr[1].ToString() + "' ,Field4='" + dr[3].ToString() + "' where Trans_Id=" + dr[2].ToString() + "";
                        }
                        else
                        {
                            sql = "update Ems_ContactMaster set Field4='" + dr[3].ToString() + "' where Trans_Id=" + dr[2].ToString() + "";
                        }
                        da.execute_Command(sql);
                        objEmailDetail.ES_EmailMasterDetail_Insert(dr[2].ToString(), "Contact", dr[0].ToString(), isdefault.ToString(), "", "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                    }

                }



            }

        }
        else
        {

        }
        if (counter != 0)
        {
            DisplayMessage(counter.ToString() + " Record Inserted");
            btnResetEmailList_Click(null, null);
        }
        Allpagecode();
    }
    protected void gvUnregisteredEmailexistcontact_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvUnregisteredEmailexistcontact.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)ViewState["dtEmailList"];
        gvUnregisteredEmailexistcontact.DataSource = dt;
        gvUnregisteredEmailexistcontact.DataBind();
        lblTotalRecordsUnRegisEmail.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count + "";
    }
    protected void btnsaveexistcontactgridrecord_Click(object sender, EventArgs e)
    {
    }
    protected void btnresetexistscontactgridrecord_Click(object sender, EventArgs e)
    {
        btnResetEmailList_Click(null, null);
        txtvalueunregister.Text = "";
        ddlOptionunregister.SelectedIndex = 2;

    }

    protected void IbtnDeleteMail_Command(object sender, CommandEventArgs e)
    {
        DataAccessClass da = new DataAccessClass(Session["DBConnection"].ToString());
        string sql = "delete from ES_EmailMaster_Header where Trans_Id=" + e.CommandArgument.ToString() + "";
        da.execute_Command(sql);
        DisplayMessage("Record Deleted");
        FillGridoption();
        txtvalueunregister.Text = "";
        Allpagecode();
    }

    protected void chkHeaderSelect_OnCheckedChanged(object sender, EventArgs e)
    {

        foreach (GridViewRow gvrow in gvUnregisteredEmailexistcontact.Rows)
        {
            if (((CheckBox)gvUnregisteredEmailexistcontact.HeaderRow.FindControl("chkHeaderSelect")).Checked)
            {
                ((CheckBox)gvrow.FindControl("chkselctemail")).Checked = true;

            }
            else
            {
                ((CheckBox)gvrow.FindControl("chkselctemail")).Checked = false;

            }
        }
    }



    protected void btnSave_UnregisterEmail_Click(object sender, EventArgs e)
    {

        bool isdefault = true;


        int counter = 0;


        foreach (GridViewRow gvrow in gvUnregisteredEmailexistcontact.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkselctemail")).Checked)
            {
                if (((TextBox)gvrow.FindControl("txtcontactname")).Text == "")
                {
                    DisplayMessage("Enter Contact Name");
                    ((TextBox)gvrow.FindControl("txtcontactname")).Focus();
                    return;
                }
                else
                {
                    try
                    {
                        ((TextBox)gvrow.FindControl("txtcontactname")).Text.Split('/')[1].ToString();
                    }
                    catch
                    {
                        DisplayMessage("Choose Contact in suggestion only");
                        ((TextBox)gvrow.FindControl("txtcontactname")).Focus();
                        return;

                    }
                }


                if (((TextBox)gvrow.FindControl("txtCountryName")).Text == "")
                {
                    DisplayMessage("Enter Country Name");
                    ((TextBox)gvrow.FindControl("txtCountryName")).Focus();
                    return;
                }
                else
                {
                    try
                    {
                        ((TextBox)gvrow.FindControl("txtCountryName")).Text.Split('/')[1].ToString();
                    }
                    catch
                    {
                        DisplayMessage("Choose  Country in suggestion only");
                        ((TextBox)gvrow.FindControl("txtCountryName")).Focus();
                        return;

                    }
                }

                counter = 1;
            }

        }


        if (counter == 0)
        {
            DisplayMessage("Select at Least one Email-Id");
            return;
        }

        foreach (GridViewRow gvrow in gvUnregisteredEmailexistcontact.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkselctemail")).Checked)
            {
                isdefault = true;



                DataTable dtExists = objEmailDetail.Get_EmailMasterDetailAllTrue();
                try
                {
                    dtExists = new DataView(dtExists, "Ref_Id=" + ((TextBox)gvrow.FindControl("txtcontactname")).Text.Split('/')[1].ToString() + " and Email_Ref_Type='Contact' and Is_Default='True'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (dtExists.Rows.Count > 0)
                {
                    isdefault = false;
                }
                string sql = "update ES_EmailMaster_Header set Field1='" + ((TextBox)gvrow.FindControl("txtCountryName")).Text.Split('/')[1].ToString() + "' where Trans_Id=" + ((HiddenField)gvrow.FindControl("hdntransid")).Value + "";
                da.execute_Command(sql);

                if (isdefault)
                {
                    sql = "update Ems_ContactMaster set Field1='" + ((Label)gvrow.FindControl("lblEmailId")).Text + "' ,Field4='" + ((TextBox)gvrow.FindControl("txtCountryName")).Text.Split('/')[1].ToString() + "' where Trans_Id=" + ((TextBox)gvrow.FindControl("txtcontactname")).Text.Split('/')[1].ToString() + "";
                }
                else
                {
                    sql = "update Ems_ContactMaster set Field4='" + ((TextBox)gvrow.FindControl("txtCountryName")).Text.Split('/')[1].ToString() + "' where Trans_Id=" + ((TextBox)gvrow.FindControl("txtcontactname")).Text.Split('/')[1].ToString() + "";
                }
                da.execute_Command(sql);
                objEmailDetail.ES_EmailMasterDetail_Insert(((TextBox)gvrow.FindControl("txtcontactname")).Text.Split('/')[1].ToString(), "Contact", ((HiddenField)gvrow.FindControl("hdntransid")).Value, isdefault.ToString(), "", "", "", "", "", "False", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }


        DisplayMessage("Record Saved", "green");
        FillGridoption();
        txtvalueunregister.Text = "";
        Allpagecode();
    }
    protected void btnbindunRegisEmail_Click(object sender, EventArgs e)
    {
        if (ddlOptionunregister.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOptionunregister.SelectedIndex == 1)
            {
                condition = ddlFieldNameunregis.SelectedValue + "='" + txtvalueunregister.Text.Trim() + "'";
            }
            else if (ddlOptionunregister.SelectedIndex == 2)
            {
                condition = ddlFieldNameunregis.SelectedValue + " Like '%" + txtvalueunregister.Text.Trim() + "%'";
            }
            else
            {
                condition = ddlFieldNameunregis.SelectedValue + " like '" + txtvalueunregister.Text.Trim() + "%'";
            }
            DataTable dt = (DataTable)ViewState["dtEmailList"];


            if (dt != null)
            {
                dt = new DataView(dt, condition, "", DataViewRowState.CurrentRows).ToTable();

                gvUnregisteredEmailexistcontact.DataSource = dt;
                gvUnregisteredEmailexistcontact.DataBind();

                lblTotalRecordsUnRegisEmail.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count + "";

                //view.ToTable();
            }
        }
        Allpagecode();
        txtvalueunregister.Focus();
    }
    protected void btnRefreshunRegisEmail_Click(object sender, EventArgs e)
    {
        txtvalueunregister.Text = "";
        ddlOptionunregister.SelectedIndex = 2;
        FillGridoption();
        Allpagecode();

    }
    public void FillGridoption()
    {
        DataTable dt = new DataTable();
        if (rbtnCreatecontact.Checked)
        {

            dt = objEmailHeader.Get_Emailall_ByOpType("8");
            if (dt.Rows.Count > 0)
            {
                ViewState["dtEmailList"] = dt;

                gvUnregisteredEmailexistcontact.DataSource = dt;
                gvUnregisteredEmailexistcontact.DataBind();
            }

            btnSave_UnregisterEmail.Visible = false;
        }
        else
        {

            dt = objEmailHeader.Get_Emailall_ByOpType("9");
            if (dt.Rows.Count > 0)
            {
                ViewState["dtEmailList"] = dt;
                gvUnregisteredEmailexistcontact.DataSource = dt;
                gvUnregisteredEmailexistcontact.DataBind();
            }
            btnSave_UnregisterEmail.Visible = true;

        }
        lblTotalRecordsUnRegisEmail.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count + "";
        Allpagecode();
    }
    protected void btnEditEmail_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((ImageButton)sender).Parent.Parent;
        string strCmd = string.Format("window.open('../EMS/ContactMaster.aspx?EmailId=" + ((Label)gvrow.FindControl("lblEmailId")).Text + "&&CountryId=" + ((HiddenField)gvrow.FindControl("hdnCountryid")).Value + "&&CountryName=" + ((Label)gvrow.FindControl("lblcountryName")).Text + "','window','width=1024');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);

    }
    public string GetcountryNamebycountryId(string Id)
    {
        string countryname = string.Empty;
        if (Id != "")
        {
            try
            {
                countryname = objCountry.GetCountryMasterById(Id).Rows[0]["Country_Name"].ToString();
            }
            catch
            {
            }
        }
        return countryname;
    }

    #endregion

    protected void FileUploadComplete(object sender, EventArgs e)
    {
        if (fileLoad.HasFile)
        {
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                string path = Server.MapPath("~/Temp/" + fileLoad.FileName);
                fileLoad.SaveAs(path);
            }
        }
    }
    //protected void FuLogo_FileUploadComplete(object sender, EventArgs e)
    //{
    //    if (FileUploadImage.HasFile)
    //    {
    //        string ext = FileUploadImage.FileName.Substring(FileUploadImage.FileName.Split('.')[0].Length);
    //        if ((ext != ".png") && (ext != ".jpg") && (ext != ".jpge"))
    //        {
    //            DisplayMessage("Invalid File Type, Select Only .png, .jpg, .jpge extension file");
    //            return;
    //        }
    //        else
    //        {
    //            string path = Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/Signature/" + Session["UserId"].ToString());
    //            if (!Directory.Exists(path))
    //            {
    //                CheckDirectory(path);
    //            }
    //            string filepath = "../CompanyResource/" + Session["CompId"].ToString() + "/Signature/" + Session["UserId"].ToString() + "/" + Guid.NewGuid() + FileUploadImage.FileName;
    //            FileUploadImage.SaveAs(Server.MapPath(filepath));
    //            txtEmailSignature.Content = txtEmailSignature.Content + "<img src='" + filepath + "' />";
    //        }
    //    }        
    //}
}
