using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;



//created by divya parakh 16/3/2018
public partial class WebUserControl_FileUpload : System.Web.UI.UserControl
{
    Document_Master ObjDocument = null;
    Arc_Directory_Master objDirectorymaster = null;
    Arc_FileTransaction ObjFile = null;
    Common cmn = null;
    SystemParameter ObjSysParam = null;
    Reminder objReminder = null;
    ReminderLogs objReminderlog = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        ObjDocument = new Document_Master(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objReminder = new Reminder(Session["DBConnection"].ToString());
        objReminderlog = new ReminderLogs(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objDirectorymaster = new Arc_Directory_Master(Session["DBConnection"].ToString());
        ObjFile = new Arc_FileTransaction(Session["DBConnection"].ToString());

        txtExpiryDate.Attributes.Add("readonly", "true");
        if (!Page.IsPostBack)
        {
            BindDocumentList();
        }
       // AllPageCode();
    }
  


    public void SetreminerColor()
    {
        foreach (GridViewRow gvrow in gvFileMaster.Rows)
        {
            if (objReminder.getdataByref_tableName_PKID("Arc_File_Transaction", ((Label)gvrow.FindControl("lblTrans_id")).Text).Rows.Count > 0)
            {
                gvrow.BackColor = System.Drawing.ColorTranslator.FromHtml("#e8e8e8");
            }
            else
            {
                gvrow.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
            }
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
    public void setID(string ID, string module, string replaceDir, string strDirectoryType, string strDocumentNo = "",string strRefName="")
    {
        resetfile();
        Session["DirectoryType"]  = strDirectoryType;
        Session["TransID"]  = ID;
        Session["DirModuleName"] = module;
        Session["DirReplace"] = replaceDir;
        Session["DocumentNo"] = strDocumentNo;
        lblRefname.Text = strRefName;
        string DirectoryName = string.Empty;
        if (strDocumentNo == "")
        {
            DirectoryName = module + "/" + Session["TransID"].ToString();
        }
        else
        {
            DirectoryName = module + "/" + strDocumentNo;
        }
        DataTable dt = objDirectorymaster.GetDirectoryMaster_By_DirectoryName(Session["CompId"].ToString(), DirectoryName);
        if (dt.Rows.Count > 0)
        {
            FillArcawingGrid(dt.Rows[0]["Id"].ToString());
        }
        else
        {
            CreateDirectory(" ", Session["TransID"].ToString());
        }
    }
    void BindDocumentList()
    {
        DataTable dtdocument = new DataTable();
        string Documentid = "0";
        dtdocument = ObjDocument.getdocumentmaster(Session["CompId"].ToString(), Documentid);
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)ddlDocumentName, dtdocument, "Document_name", "Id");
    }
    protected void ImgButtonDocumentAdd_Click(object sender, EventArgs e)
    {

        if (ddlDocumentName.SelectedIndex == 0)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DispMsg('Select Document Name')", true);
            ddlDocumentName.Focus();
            return;
        }


        if (txtDocumentName.Text == "")
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DispMsg('Enter Document Name')", true);
            txtDocumentName.Focus();
            return;
        }

        if (Session["FileName"] == null)
        {

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DispMsg('Select a File')", true);
            return;
        }


        if (txtExpiryDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtExpiryDate.Text);
            }
            catch
            {
                //DisplayMessage("Invalid Expiry Date");
                txtExpiryDate.Focus();
                return;
            }
        }
        else
        {
            txtExpiryDate.Text = DateTime.Now.AddYears(20).ToString("dd-MMM-yyyy");
        }

        string date = System.DateTime.Now.ToString();
        string currentdate = System.DateTime.Now.ToString("dd-MMM-yyyy");

        if (ddlSetReminder.SelectedValue == "Yes")
        {
            DateTime check_4 = Convert.ToDateTime(currentdate);
            currentdate = GetDate((check_4.AddDays(4)).ToString());
            if (ObjSysParam.getDateForInput(txtExpiryDate.Text) < ObjSysParam.getDateForInput(currentdate))
            {
                txtExpiryDate.Focus();
                txtExpiryDate.Text = "";
                lblmessage.Text = "Expiry Date Must be Greater then or Equal to :" + currentdate;
                lblmessage.Visible = true;
                return;
            }
        }

        string filepath = string.Empty;

        try
        {
            string DirectoryName = string.Empty;
            if (Session["DocumentNo"].ToString() == "")
            {
                DirectoryName = Session["DirModuleName"].ToString() + "/" + Session["TransID"].ToString() + "/" + ddlDocumentName.SelectedItem.Text;
            }
            else
            {
                DirectoryName = Session["DirModuleName"].ToString() + "/" + Session["DocumentNo"].ToString() + "/" + ddlDocumentName.SelectedItem.Text;
            }

            string NewDirectory = Server.MapPath(DirectoryName);
            NewDirectory = NewDirectory.Replace(Session["DirReplace"].ToString(), "ArcaWing");
            try
            {
                int i = CreateDirectoryIfNotExist(NewDirectory);
            }
            catch
            {

            }
            if (Session["DocumentNo"].ToString() == "")
            {
                filepath = "~/ArcaWing/" + Session["DirModuleName"].ToString()  + "/" + Session["TransID"].ToString()  + "/" + ddlDocumentName.SelectedItem.Text + "/" + Session["FileName"].ToString();
            }
            else
            {
                filepath = "~/ArcaWing/" + Session["DirModuleName"].ToString() + "/" + Session["DocumentNo"].ToString() + "/" + ddlDocumentName.SelectedItem.Text + "/" + Session["FileName"].ToString();
            }
            UploadFile.SaveAs(Server.MapPath(filepath));
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DispMsg('File Uploaded Successfully')", true);
        }
        catch (Exception ex)
        {
            //DisplayMessage("file not found");
            return;
        }

        string filename = string.Empty;
        string ext = string.Empty;

        try
        {
            ext = Path.GetExtension(filepath);
        }
        catch
        {

        }
        filename = txtDocumentName.Text + "" + ext;

        Byte[] bytes = new Byte[0];

        string ExistDirectoryName = string.Empty;

        if (Session["DocumentNo"].ToString() == "")
        {
            ExistDirectoryName = Session["DirModuleName"].ToString() + "/" + Session["TransID"].ToString();
        }
        else
        {
            ExistDirectoryName = Session["DirModuleName"].ToString() + "/" + Session["DocumentNo"].ToString();
        }
        int fileUploadId = 0;
        DataTable dt = objDirectorymaster.GetDirectoryMaster_By_DirectoryName(Session["CompId"].ToString(), ExistDirectoryName);
        if (dt.Rows.Count > 0)
        {

            //id directory type is 0 then we will insert 0 company id because contact page is on systemlevel
            //so we added if else condition
            if (Session["DirectoryType"].ToString().Trim() == "Contact")
            {
                fileUploadId = ObjFile.Insert_In_FileTransaction("0", dt.Rows[0]["Id"].ToString(), ddlDocumentName.SelectedValue, "0", filename, DateTime.Now.ToString(), bytes, filepath, txtExpiryDate.Text, "", "0", UploadFile.FileName, "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                fileUploadId = ObjFile.Insert_In_FileTransaction(Session["CompId"].ToString(), dt.Rows[0]["Id"].ToString(), ddlDocumentName.SelectedValue, "0", filename, DateTime.Now.ToString(), bytes, filepath, txtExpiryDate.Text, "", "0", UploadFile.FileName, "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            FillArcawingGrid(dt.Rows[0]["Id"].ToString());
            ddlDocumentName.SelectedIndex = 0;
            //txtfilename.Text = "";
        }
        Session["FileName"] = null;
        UploadFile.FileContent.Dispose();


        if (ddlSetReminder.SelectedValue == "Yes")
        {
            DateTime before_4_days = Convert.ToDateTime(txtExpiryDate.Text);
            string date_before_4_days = GetDate((before_4_days.AddDays(-4)).ToString());
            string message = "Your Document ('" + txtDocumentName.Text + "') Generated For ('" + Session["DirModuleName"].ToString() + "') Will Expire After 4 days";
            int reminder_id_4 = objReminder.insertData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Arc_File_Transaction", fileUploadId.ToString(), message, "", date, "1", date_before_4_days, "Once", Session["EmpId"].ToString(), "On", "false", "false", "true", Session["UserId"].ToString(), Session["UserId"].ToString());
            objReminderlog.insertLogData(reminder_id_4.ToString(), date_before_4_days, "", Session["UserId"].ToString(), Session["UserId"].ToString());
            objReminder.Set_Url(reminder_id_4.ToString(), "../CRM/Reminder.aspx?ReminderID=" + reminder_id_4 + "");

            currentdate = System.DateTime.Now.ToString("dd-MMM-yyyy");
            DateTime check_7 = Convert.ToDateTime(currentdate);
            currentdate = GetDate((check_7.AddDays(7)).ToString());
            if (ObjSysParam.getDateForInput(txtExpiryDate.Text) > ObjSysParam.getDateForInput(currentdate))
            {
                DateTime before_7_Days = Convert.ToDateTime(txtExpiryDate.Text);
                string date_before_7_Days = GetDate((before_7_Days.AddDays(-7)).ToString());
                message = "Your Document ('" + txtDocumentName.Text + "') Generated For ('" + Session["DirModuleName"].ToString() + "') Will Expire After 7 days";
                int reminder_id_7 = objReminder.insertData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Arc_File_Transaction", fileUploadId.ToString(), message, "", date, "1", date_before_7_Days, "Once", Session["EmpId"].ToString(), "On", "false", "false", "true", Session["UserId"].ToString(), Session["UserId"].ToString());
                objReminderlog.insertLogData(reminder_id_7.ToString(), date_before_7_Days, "", Session["UserId"].ToString(), Session["UserId"].ToString());
                objReminder.Set_Url(reminder_id_7.ToString(), "../CRM/Reminder.aspx?ReminderID=" + reminder_id_7 + "");
            }
        }

        lblmessage.Visible = false;
        txtDocumentName.Text = "";
        txtExpiryDate.Text = "";
        ddlSetReminder.SelectedIndex = 0;
    }
    public void FillArcawingGrid(string DirectoryId)
    {
        DataTable dt = ObjFile.Get_FileTransaction_By_DirectoryidandObjectId(Session["CompId"].ToString(),"0", DirectoryId);
        gvFileMaster.DataSource = dt;
        gvFileMaster.DataBind();
        SetreminerColor();
    }
    public int CreateDirectoryIfNotExist(string NewDirectory)
    {
        int checkDirectory = 0;
        try
        {
            // Checking the existance of directory
            if (!Directory.Exists(NewDirectory))
            {
                //If No any such directory then creates the new one
                Directory.CreateDirectory(NewDirectory);
            }
            else
            {
                checkDirectory = 1;
            }
        }
        catch (IOException _err)
        {
            Response.Write(_err.Message);
        }
        return checkDirectory;
    }
    protected void FUAll_FileUploadComplete(object sender, EventArgs e)
    {
        if (UploadFile.HasFile)
        {
            Session["FileName"] = UploadFile.FileName;
        }
        else
        {
            Session["FileName"] = null;
        }

    }
    public void OnDownloadDocumentCommand1(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataTable();
        dt = ObjFile.Get_FileTransaction_By_TransactionId(Session["CompId"].ToString(), e.CommandArgument.ToString());
        downloadfile(dt);
        resetfile();
        Page page = new Page();
    }
    public void downloadfile(DataTable dt)
    {
        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        response.ClearContent();
        response.Clear();
        response.AddHeader("Content-Disposition", "attachment; filename=" + dt.Rows[0]["File_Name"].ToString() + ";");
        response.TransmitFile(Server.MapPath(dt.Rows[0]["File_Path"].ToString().Replace("~/", "~//")));
        response.Flush();
        response.End();

    }
    protected void IbtnDeleteDocument_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        b = ObjFile.Delete_in_FileTransaction(Session["CompId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        objReminder.setIsActiveFalseByRef_table_name_n_pk("Arc_File_Transaction", e.CommandArgument.ToString());

        if (b != 0)
        {
            string ExistDirectoryName = string.Empty;

            if (Session["DocumentNo"].ToString() == "")
            {
                ExistDirectoryName = Session["DirModuleName"].ToString() + "/" + Session["TransID"].ToString();
            }
            else
            {
                ExistDirectoryName = Session["DirModuleName"].ToString() + "/" + Session["DocumentNo"].ToString();
            }
            DataTable dt = objDirectorymaster.GetDirectoryMaster_By_DirectoryName(Session["CompId"].ToString(), ExistDirectoryName);
            if (dt.Rows.Count > 0)
            {
                FillArcawingGrid(dt.Rows[0]["Id"].ToString());
            }
        }
        else
        {
            //DisplayMessage("Record Not Deleted");
        }

    }
    public void CreateDirectory(string Projectcode, string ProjectId)
    {
        string DirectoryName = string.Empty;
        if (Session["DocumentNo"].ToString() == "")
        {
            DirectoryName = Session["DirModuleName"].ToString() + "/" + ProjectId;
        }
        else
        {
            DirectoryName = Session["DirModuleName"].ToString() + "/" + Session["DocumentNo"].ToString();
        }

        DataTable DtDir = objDirectorymaster.getDirectoryMasterByCompanyid(Session["compId"].ToString());

        try
        {
            DtDir = new DataView(DtDir, "Directory_Name='" + DirectoryName + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }

        if (DtDir.Rows.Count == 0)
        {
            int b = objDirectorymaster.InsertDirectorymaster(Session["CompId"].ToString(), DirectoryName, "1", Session["BrandId"].ToString(), ProjectId, Session["DirectoryType"].ToString(), Session["LocId"].ToString(), false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["EmpId"].ToString());

            if (b != 0)
            {
                string NewDirectory = string.Empty;
                NewDirectory = Server.MapPath(DirectoryName);
                NewDirectory = NewDirectory.Replace(Session["DirReplace"].ToString() , "ArcaWing");
                int i = CreateDirectoryIfNotExist(NewDirectory);
            }
            else
            {
                //DisplayMessage("Record Not Saved");
            }
        }
    }
    public void resetfile()
    {
        gvFileMaster.DataSource = null;
        gvFileMaster.DataBind();
        Session["DirReplace"] = "";
        Session["DocumentNo"] = "";
        Session["DirModuleName"] = "";
        Session["DirectoryType"] = "";
        Session["TransID"] = "";
        lblmessage.Visible = false;
        txtDocumentName.Text = "";
        ddlDocumentName.SelectedIndex = 0;
        ddlSetReminder.SelectedIndex = 0;
        txtExpiryDate.Text = "";
    }
}