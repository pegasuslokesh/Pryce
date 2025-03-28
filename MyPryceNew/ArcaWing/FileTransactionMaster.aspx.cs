using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using PegasusDataAccess;
using System.Configuration;

public partial class ArcaWing_FileTransactionMaster : BasePage
{
    Arc_FileTransaction ObjFile = null;
    Arc_Directory_Master ObjDirectory = null;
    Arc_FileType ObjFileType =null;
    Common cmn = null;
    Document_Master ObjDocument = null;
    Ems_ContactMaster ObjContactMaster = null;
    Set_CustomerMaster objCustomer = null;
    SystemParameter objSys = null;
    EmployeeMaster objEmp = null;
    Inv_ProductMaster ObjProductMaster = null;
    Inv_UnitMaster ObjUnitMaster = null;
    Ems_Contact_Group objCG = null;
    Set_Suppliers objSupplier = null;
    UserMaster objUser = null;
    Prj_ProjectMaster objProjctMaster = null;
    Reminder objReminder = null;
    ReminderLogs objReminderlog = null;
    DataAccessClass Objda = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        ObjDocument = new Document_Master(Session["DBConnection"].ToString());
        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        objCustomer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjUnitMaster = new Inv_UnitMaster(Session["DBConnection"].ToString());
        objCG = new Ems_Contact_Group(Session["DBConnection"].ToString());
        objSupplier = new Set_Suppliers(Session["DBConnection"].ToString());
        objUser = new UserMaster(Session["DBConnection"].ToString());
        objProjctMaster = new Prj_ProjectMaster(Session["DBConnection"].ToString());
        objReminder = new Reminder(Session["DBConnection"].ToString());
        objReminderlog = new ReminderLogs(Session["DBConnection"].ToString());
        Objda = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        ObjFile = new Arc_FileTransaction(Session["DBConnection"].ToString());
        ObjDirectory = new Arc_Directory_Master(Session["DBConnection"].ToString());
        ObjFileType = new Arc_FileType(Session["DBConnection"].ToString());
        //AllPageCode();
        if (!IsPostBack)
        {
           

            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../ArcaWing/FileTransactionMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }


            

            AllPageCode(clsPagePermission);

            Session["CHECKED_ITEMS"] = null;
            BindFiletypeList();
            txtCalenderExtender.Format = objSys.SetDateFormat();

            //  rbtnUsergenerated.Checked = true;
            Session["FileGeneratedType"] = "0";
            // rbtnListUsergenerated.Checked = true;
            FillGrid();
            pnlGoButtton.Visible = true;
            btnGo.Visible = false;
            btnback.Visible = false;
            FillUser();
            if (Session["EmpId"].ToString().Trim() == "0")
            {
                lblLoginUser.Visible = true;
                //lblLoginUserColon.Visible = true;
                ddlLoginUserName.Visible = true;
                Session["LoginUserDirectory"] = null;
            }
            else
            {
                DataTable dtDirectory = ObjDirectory.getDirectoryMaster(HttpContext.Current.Session["CompId"].ToString(), "0");
                try
                {
                    dtDirectory = new DataView(dtDirectory, "Field1='0' and CreatedBy='" + Session["UserId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {

                }
                Session["LoginUserDirectory"] = dtDirectory;
            }
            gvFileMaster1.SettingsPager.PageSize = int.Parse(Session["GridSize"].ToString());

            //if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud")
            //{
            rbtnListUsergenerated.Visible = false;
            rbtnListSystemgenerated.Visible = false;
            rbtnUsergenerated.Visible = false;
            rbtnSystemgenerated.Visible = false;
            btnTreeView.Visible = false;
            //}
        }
        //cmn.FillData((object)gvFileMaster1, (DataTable)Session["ClaimRecord"], "", "");
        gvFileMaster1.DataSource = (DataTable)Session["ClaimRecord"];
        gvFileMaster1.DataBind();

    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        if (clsPagePermission.bEdit)
            gvFileMaster1.Columns[0].Visible = true;

        if (clsPagePermission.bDelete)
            gvFileMaster1.Columns[1].Visible = true;

        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanDownload.Value = clsPagePermission.bDownload.ToString().ToLower();
        btnTreeView.Visible = clsPagePermission.bView;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
    }
    void BindFiletypeList()
    {
        DataTable Dtfiletype = new DataTable();
        string Filetypeid = "0";
        Dtfiletype = ObjFileType.Get_FileType(Session["CompId"].ToString(), Filetypeid);
        if (Dtfiletype.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)ddlFiletype, Dtfiletype, "File_type", "Id");
        }
        else
        {
            ddlFiletype.Items.Insert(0, "--Select--");
            ddlFiletype.SelectedIndex = 0;
        }
    }
    public void FillGrid()
    {
        DataTable dt = ObjFile.Get_FileTransaction(Session["CompId"].ToString(), "0");
        if (Session["EmpId"].ToString() != "0")
        {
            dt = ObjFile.Get_FileTransaction_ByLoginEmpId(Session["CompId"].ToString(), Session["EmpId"].ToString());
        }
        //dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        if (rbtnListUsergenerated.Checked == true)
        {

            try
            {
                dt = new DataView(dt, "DirectoryType='0'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }
        else
        {
            if (ddlType.SelectedIndex == 1)
            {
                try
                {
                    dt = new DataView(dt, "DirectoryType_TransId in (" + lblSelectedRecordEmployee.Text.Substring(0, lblSelectedRecordEmployee.Text.Length - 1) + ") and DirectoryType_Name='Employee'", "", DataViewRowState.CurrentRows).ToTable();

                }
                catch
                {

                }
            }
            if (ddlType.SelectedIndex == 2)
            {
                try
                {
                    dt = new DataView(dt, "DirectoryType_TransId in (" + lblSelectedRecordProduct.Text.Substring(0, lblSelectedRecordProduct.Text.Length - 1) + ") and DirectoryType_Name='Product'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
            }
            if (ddlType.SelectedIndex == 3)
            {

                dt = ObjFile.Get_FileTransaction("0", "0");
                if (Session["EmpId"].ToString() != "0")
                {
                    dt = ObjFile.Get_FileTransaction_ByLoginEmpId("0", Session["EmpId"].ToString());
                }
                try
                {
                    dt = new DataView(dt, "DirectoryType_TransId in (" + lblSelectedRecordContact.Text.Substring(0, lblSelectedRecordContact.Text.Length - 1) + ") and DirectoryType_Name='Contact'", "", DataViewRowState.CurrentRows).ToTable();

                }
                catch
                {
                }
            }

            if (ddlType.SelectedIndex == 4)
            {
                try
                {
                    dt = new DataView(dt, "DirectoryType_TransId in (" + lblSelectedRecordCustomer.Text.Substring(0, lblSelectedRecordCustomer.Text.Length - 1) + ") and DirectoryType_Name='Customer'", "", DataViewRowState.CurrentRows).ToTable();

                }
                catch
                {
                }
            }
            if (ddlType.SelectedIndex == 5)
            {
                try
                {
                    dt = new DataView(dt, "DirectoryType_TransId in (" + lblSelectedRecordSupplier.Text.Substring(0, lblSelectedRecordSupplier.Text.Length - 1) + ") and DirectoryType_Name='Supplier'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }

            }
            if (ddlType.SelectedIndex == 6)
            {
                try
                {
                    dt = new DataView(dt, "DirectoryType_TransId in (" + lblSelectedRecordProject.Text.Substring(0, lblSelectedRecordProject.Text.Length - 1) + ") and DirectoryType_Name='Project'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }

            }
        }


        string Directorylist = string.Empty;

        foreach (DataRow dr in dt.Rows)
        {
            if (!Directorylist.Split(',').Contains(dr["Directory_Id"].ToString()))
            {
                Directorylist += dr["Directory_Id"].ToString() + ",";

            }
        }

        Session["DirectoryLIst"] = Directorylist;
        Session["ClaimRecord"] = dt;
        //Common Function add By Lokesh on 23-05-2015

        objPageCmn.FillData((object)gvFileMaster, dt, "", "");
        if (dt.Rows.Count > 0)
            pnlGoButtton.Visible = true;
        else
            pnlGoButtton.Visible = false;
        //AllPageCode();
        Session["dtFilter_File_Trans"] = dt;
        Session["File"] = dt;

        gvFileMaster1.DataSource = dt;
        gvFileMaster1.DataBind();

        if (dt.Rows.Count > 0)
        {
            // ReportPanel.Visible = true;
            chkGroupBy.Visible = true;
            btnReport.Visible = true;
        }
        else
        {
            //ReportPanel.Visible = false;
            chkGroupBy.Visible = false;
            btnReport.Visible = false;
        }
        //commented by jitendra because  filter panel exists in aspx gridview
        //01-june-2018
        //start
        //lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        //end
    }
    public void FillUser()
    {
        DataTable dt = objUser.GetUserMasterByUserIdByCompId(Session["UserId"].ToString(), "4",HttpContext.Current.Session["CompId"].ToString());
        try
        {
            dt = new DataView(dt, "Company_Id=" + Session["CompId"].ToString() + " or Company_Id=0 ", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)ddlLoginUserName, dt, "User_Id", "User_Id");
        }
        else
        {
            ddlLoginUserName.Items.Insert(0, "--Select--");
            ddlLoginUserName.SelectedIndex = 0;
        }
    }
    public void FillGridBin()
    {
        string FileTypeId = "0";

        DataTable dt = new DataTable();
        dt = ObjFile.Get_FileTransactionInActive(Session["CompId"].ToString(), FileTypeId);
        //dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
        //if (Session["EmpId"].ToString().Trim() != "0")
        //{
        //    try
        //    {
        //        dt = new DataView(dt, "CreatedBy='" + Session["UserId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //    }
        //    catch
        //    {
        //    }
        //}
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvFileMasterBin, dt, "", "");

        Session["dtbinFilter"] = dt;
        Session["dtbinFile"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        if (dt.Rows.Count == 0)
        {
            //imgBtnRestore.Visible = false;
            //ImgbtnSelectAll.Visible = false;
        }
        else
        {

            //AllPageCode();
        }
    }
    protected void gvFileMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //SaveAllGridChkbox(gvFileMaster);
        gvFileMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_File_Trans"];
        //if (HttpContext.Current.Session["EmpId"].ToString().Trim() != "0")
        //{
        //    try
        //    {
        //        dt = new DataView(dt, "CreatedBy='" + HttpContext.Current.Session["UserId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //    }
        //    catch
        //    {
        //    }
        //}
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvFileMaster, dt, "", "");
        // PopulateAllGridChkbox(gvFileMaster);
        //AllPageCode();
    }
    protected void gvFileMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_File_Trans"];
        if (rbtnListUsergenerated.Checked == true)
        {
            if (HttpContext.Current.Session["EmpId"].ToString().Trim() != "0")
            {
                //try
                //{
                //    dt = new DataView(dt, "CreatedBy='" + HttpContext.Current.Session["UserId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                //}
                //catch
                //{
                //}
            }
        }

        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_File_Trans"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvFileMaster, dt, "", "");
        //AllPageCode();
    }
    protected void gvFileMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveAllGridChkbox(gvFileMasterBin);
        gvFileMasterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvFileMasterBin, dt, "", "");
        //AllPageCode();
        PopulateAllGridChkbox(gvFileMasterBin);
    }
    protected void gvFileMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        string FileTypeid = "0";
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = ObjFile.Get_FileTransactionInActive(Session["CompId"].ToString(), FileTypeid);
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvFileMasterBin, dt, "", "");
        //AllPageCode();
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvFileMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvFileMasterBin.Rows)
        {

            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = false;
            }
        }
    }

    private void download(DataTable dt)
    {

        System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
        response.ClearContent();
        response.Clear();
        response.AddHeader("Content-Disposition", "attachment; filename=" + dt.Rows[0]["File_Name"].ToString() + ";");
        response.TransmitFile(Server.MapPath(dt.Rows[0]["File_Path"].ToString().Replace("~", "")));
        response.Flush();
        response.End();
    }


    protected void OnDownloadCommand(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        DataTable dt = new DataTable();

        dt = ObjFile.Get_FileTransaction_By_TransactionId(Session["CompId"].ToString(), editid.Value);
        download(dt);
        Reset();
    }
    protected void OnUpdateCommand(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((ImageButton)sender).Parent.Parent;
        if (((TextBox)gvrow.FindControl("txtfileexpirydate")).Text == "")
        {
            DisplayMessage("Enter Expiry Date");
            ((TextBox)gvrow.FindControl("txtfileexpirydate")).Focus();
            return;
        }
        try
        {
            Convert.ToDateTime(((TextBox)gvrow.FindControl("txtfileexpirydate")).Text);
        }
        catch
        {
            DisplayMessage("Invalid Date Format ,Try Again");
            ((TextBox)gvrow.FindControl("txtfileexpirydate")).Focus();
            return;
        }
        int b = ObjFile.UpdateExpiryDate_ByDirectoryId(Session["CompId"].ToString(), e.CommandArgument.ToString(), ((TextBox)gvrow.FindControl("txtfileexpirydate")).Text, Session["UserId"].ToString(), DateTime.Now.ToString());

        if (b != 0)
        {
            DisplayMessage("Record Updated Successfully", "green");
            Lbl_Tab_New.Text = Resources.Attendance.New;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        }
    }

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();


        DataTable dt = ObjFile.Get_FileTransaction_By_TransactionId(Session["CompId"].ToString(), editid.Value);

        if (dt.Rows.Count > 0)
        {

            try
            {
                txtDirectoryname.Text = dt.Rows[0]["Directory_Name"].ToString();
                txtDocumentName.Text = dt.Rows[0]["Document_Name"].ToString();
                hdnDirectoryId.Value = dt.Rows[0]["Directory_Id"].ToString();
                hdndocumentid.Value = dt.Rows[0]["Document_Master_Id"].ToString();
                ddlFiletype.SelectedValue = dt.Rows[0]["File_Type_id"].ToString();
                //txtFileName.Text = dt.Rows[0]["File_Name"].ToString();
                string FileName = dt.Rows[0]["File_Name"].ToString();
                string FilePath = dt.Rows[0]["File_Path"].ToString();
                string extension = Path.GetExtension(FilePath);
                string result = FileName.Substring(0, FileName.Length - extension.Length);
                txtFileName.Text = result.ToString();
            }
            catch
            {

            }
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
    }

    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        b = ObjFile.Delete_in_FileTransaction(Session["CompId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
            FillGrid();
            Reset();
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    public void Reset()
    {

        Session["CHECKED_ITEMS"] = null;
        txtFileName.Text = "";
        txtDirectoryname.Text = "";
        txtDocumentName.Text = "";
        ddlFiletype.SelectedIndex = 0;

        Lbl_Tab_New.Text = Resources.Attendance.New;

        ViewState["Select"] = null;
        editid.Value = "";
        txtExpiryDate.Text = "";

        txtDirectoryname.Focus();

        txtdesc.Text = "";



    }

    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtbinValue.Focus();
        FillGridBin();
    }




    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
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
            DataTable dtCust = (DataTable)Session["dtbinFile"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvFileMasterBin, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {
                //imgBtnRestore.Visible = false;
                //ImgbtnSelectAll.Visible = false;
            }
            else
            {
                //AllPageCode();
            }
        }
        txtbinValue.Focus();
    }

    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        FillGridBin();
        Session["CHECKED_ITEMS"] = null;
        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 1;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }

    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I3.Attributes.Add("Class", "fa fa-minus");
        Div3.Attributes.Add("Class", "box box-primary");
        int b = 0;
        ArrayList userdetail = new ArrayList();
        if (gvFileMasterBin.Rows.Count > 0)
        {
            SaveAllGridChkbox(gvFileMasterBin);
            if (Session["CHECKED_ITEMS"] != null)
            {
                userdetail = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetail.Count > 0)
                {
                    for (int j = 0; j < userdetail.Count; j++)
                    {
                        if (userdetail[j].ToString() != "")
                        {
                            b = ObjFile.Delete_in_FileTransaction(Session["CompId"].ToString(), userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                        }
                    }
                }

                if (b != 0)
                {

                    FillGrid();
                    FillGridBin();
                    lblSelectedRecord.Text = "";
                    ViewState["Select"] = null;
                    DisplayMessage("Record Activated");
                    Session["CHECKED_ITEMS"] = null;

                }
                else
                {
                    int fleg = 0;
                    foreach (GridViewRow Gvr in gvFileMasterBin.Rows)
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
            else
            {
                DisplayMessage("Please Select Record");
                gvFileMasterBin.Focus();
                return;
            }
        }
    }

    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["dtbinFilter"];
        ArrayList userdetails = new ArrayList();
        Session["CHECKED_ITEMS"] = null;

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["CHECKED_ITEMS"] != null)
                {
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                }

                if (!userdetails.Contains(dr["Trans_Id"]))
                {
                    userdetails.Add(dr["Trans_Id"]);
                }
            }
            foreach (GridViewRow GR in gvFileMasterBin.Rows)
            {
                ((CheckBox)GR.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails.Count > 0 && userdetails != null)
            {
                Session["CHECKED_ITEMS"] = userdetails;
            }
        }
        else
        {
            lblSelectedRecordEmployee.Text = "";
            DataTable dtProduct1 = (DataTable)Session["dtbinFilter"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvFileMasterBin, dtProduct1, "", "");
            ViewState["Select"] = null;
        }
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
    protected void txtDirectoryname_OnTextChanged(object sender, EventArgs e)
    {

        if (txtDirectoryname.Text != "")
        {
            if (rbtnUsergenerated.Checked == true)
            {

                DataTable dt = ObjDirectory.getDirectoryMaster(HttpContext.Current.Session["CompId"].ToString(), "0");
                try
                {
                    dt = new DataView(dt, "Directory_Name='" + txtDirectoryname.Text + "' ", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                //if (Session["EmpId"].ToString() == "0")
                //{
                //    dt = new DataView(dt, "CreatedBy='" + ddlLoginUserName.SelectedValue + "' ", "", DataViewRowState.CurrentRows).ToTable();

                //}
                if (dt.Rows.Count == 0)
                {
                    DisplayMessage("Select Directory Name in Suggestion Only");
                    txtDirectoryname.Text = "";
                    txtDirectoryname.Focus();
                    hdnDirectoryId.Value = "";
                    return;
                }
                else
                {
                    hdnDirectoryId.Value = dt.Rows[0]["Id"].ToString();
                }
            }
            else
            {
                if (ddlSystemDirectoryType.SelectedIndex == 0)
                {
                    DisplayMessage("Select Directory Type in DropDownlist");
                    ddlSystemDirectoryType.Focus();
                    txtDirectoryname.Text = "";
                    return;
                }

                DataTable dtDirectory = new DataTable();

                if (ddlSystemDirectoryType.SelectedIndex == 1)
                {
                    dtDirectory = objEmp.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());

                    //dtDirectory = new DataView(dtDirectory, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'  and Location_Id='" + HttpContext.Current.Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    //if (HttpContext.Current.Session["SessionDepId"] != null)
                    //{
                    //    dtDirectory = new DataView(dtDirectory, "Department_Id in(" + HttpContext.Current.Session["SessionDepId"].ToString().Substring(0, HttpContext.Current.Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

                    //}
                    try
                    {
                        dtDirectory = new DataView(dtDirectory, "Emp_Code='" + txtDirectoryname.Text.Split('/')[2].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dtDirectory.Rows.Count > 0)
                    {
                        hdnDirectoryId.Value = dtDirectory.Rows[0]["Emp_Id"].ToString();

                    }
                    else
                    {
                        DisplayMessage("Select Employee Name in Suggestion Only");
                        txtDirectoryname.Text = "";
                        txtDirectoryname.Focus();

                        return;
                    }


                }
                if (HttpContext.Current.Session["Sys_DirectoryType"].ToString() == "2")
                {

                    dtDirectory = ObjProductMaster.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), Session["locId"].ToString(), Session["FinanceYearId"].ToString());

                    dtDirectory = new DataView(dtDirectory, "ProductCode='" + txtDirectoryname.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtDirectory.Rows.Count > 0)
                    {
                        hdnDirectoryId.Value = dtDirectory.Rows[0]["ProductId"].ToString();
                    }
                    else
                    {
                        DisplayMessage("Select Product Name in Suggestion Only");
                        txtDirectoryname.Text = "";
                        txtDirectoryname.Focus();

                        return;
                    }

                }
                if (HttpContext.Current.Session["Sys_DirectoryType"].ToString() == "3")
                {

                    dtDirectory = Objda.return_DataTable("select Trans_Id from ems_contactmaster where isactive='True' and Name = '" + txtDirectoryname.Text.Split('/')[0].ToString() + "'");

                    if (dtDirectory.Rows.Count > 0)
                    {
                        hdnDirectoryId.Value = dtDirectory.Rows[0]["Trans_Id"].ToString();
                    }
                    else
                    {
                        DisplayMessage("Select Contact Name in Suggestion Only");
                        txtDirectoryname.Text = "";
                        txtDirectoryname.Focus();

                        return;
                    }

                }
                if (HttpContext.Current.Session["Sys_DirectoryType"].ToString() == "4")
                {

                    dtDirectory = objCustomer.GetCustomerAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());



                    dtDirectory = new DataView(dtDirectory, "Name='" + txtDirectoryname.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtDirectory.Rows.Count > 0)
                    {
                        hdnDirectoryId.Value = dtDirectory.Rows[0]["Customer_Id"].ToString();
                    }
                    else
                    {
                        DisplayMessage("Select Customer Name in Suggestion Only");
                        txtDirectoryname.Text = "";
                        txtDirectoryname.Focus();

                        return;
                    }


                }
                if (HttpContext.Current.Session["Sys_DirectoryType"].ToString() == "5")
                {

                    dtDirectory = objSupplier.GetSupplierAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());


                    dtDirectory = new DataView(dtDirectory, "Name='" + txtDirectoryname.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtDirectory.Rows.Count > 0)
                    {
                        hdnDirectoryId.Value = dtDirectory.Rows[0]["Supplier_Id"].ToString();

                    }
                    else
                    {
                        DisplayMessage("Select Supplier Name in Suggestion Only");
                        txtDirectoryname.Text = "";
                        txtDirectoryname.Focus();

                        return;
                    }
                }

                if (HttpContext.Current.Session["Sys_DirectoryType"].ToString() == "6")
                {

                    dtDirectory = objProjctMaster.GetAllRecordProjectMasteer();


                    dtDirectory = new DataView(dtDirectory, "Project_Name='" + txtDirectoryname.Text.Split('/')[0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtDirectory.Rows.Count > 0)
                    {
                        hdnDirectoryId.Value = dtDirectory.Rows[0]["Project_Id"].ToString();

                    }
                    else
                    {
                        DisplayMessage("Select Project Name in Suggestion Only");
                        txtDirectoryname.Text = "";
                        txtDirectoryname.Focus();

                        return;
                    }
                }






            }


        }
    }
    protected void txtDocumentName_OnTextChanged(object sender, EventArgs e)
    {
        if (txtDocumentName.Text != "")
        {
            DataTable dt = ObjDocument.getdocumentmaster(HttpContext.Current.Session["CompId"].ToString(), "0");
            try
            {
                dt = new DataView(dt, "Document_Name ='" + txtDocumentName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Select Document Name in Suggestion Only");
                txtDocumentName.Text = "";
                txtDocumentName.Focus();
                hdndocumentid.Value = "";
                return;
            }
            else
            {
                hdndocumentid.Value = dt.Rows[0]["Id"].ToString();

            }

        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int b = 0;
        if (rbtnSystemgenerated.Checked == true)
        {
            if (ddlSystemDirectoryType.SelectedIndex == 0)
            {
                DisplayMessage("Select Directory Type");
                ddlSystemDirectoryType.Focus();
                return;
            }
        }

        if (txtDirectoryname.Text.Trim() == "")
        {
            DisplayMessage("Select Directory Name");

            txtDirectoryname.Focus();
            return;
        }
        if (txtDocumentName.Text.Trim() == "")
        {
            DisplayMessage("Select Document Name");

            txtDocumentName.Focus();
            return;
        }




        if (UploadFile.HasFile == false)
        {
            DisplayMessage("Upload The File");
            UploadFile.Focus();
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
                DisplayMessage("Invalid Expiry Date");
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
            if (objSys.getDateForInput(txtExpiryDate.Text) < objSys.getDateForInput(currentdate))
            {
                txtExpiryDate.Focus();
                txtExpiryDate.Text = "";
                DisplayMessage("Expiry Date Must be Greater then or Equal to :" + currentdate);
                return;
            }
        }




        //this code for get the file path and after that save the record in arcawing folder

        //code start
        string filepath = string.Empty;

        try
        {
            string NewDirectory = string.Empty;

            if (rbtnUsergenerated.Checked == true)
            {
                NewDirectory = Server.MapPath(Session["CompId"].ToString() + "/" + Session["UserId"].ToString() + "/" + txtDirectoryname.Text + "/" + txtDocumentName.Text);
                try
                {
                    int i = CreateDirectoryIfNotExist(NewDirectory);
                }
                catch
                {
                }
                filepath = "~/" + "ArcaWing" + "/" + Session["CompId"].ToString() + "/" + Session["UserId"].ToString() + "/" + txtDirectoryname.Text + "/" + txtDocumentName.Text + "/" + UploadFile.FileName;

            }
            else
            {
                if (ddlSystemDirectoryType.SelectedIndex == 1)
                {
                    NewDirectory = Server.MapPath(Session["CompId"].ToString() + "/" + ddlSystemDirectoryType.SelectedValue + "/" + txtDirectoryname.Text.Split('/')[2].ToString() + "/" + txtDocumentName.Text);
                }
                else if (ddlSystemDirectoryType.SelectedIndex == 2)
                {
                    NewDirectory = Server.MapPath(Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + ddlSystemDirectoryType.SelectedValue + "/" + txtDirectoryname.Text + "/" + txtDocumentName.Text);
                }
                else if (ddlSystemDirectoryType.SelectedIndex == 3)
                {
                    NewDirectory = Server.MapPath(ddlSystemDirectoryType.SelectedValue + "/" + txtDirectoryname.Text.Split('/')[1].ToString() + "/" + txtDocumentName.Text);

                }
                else if (ddlSystemDirectoryType.SelectedIndex == 4 || ddlSystemDirectoryType.SelectedIndex == 5)
                {
                    NewDirectory = Server.MapPath(Session["CompId"].ToString() + "/" + ddlSystemDirectoryType.SelectedValue + "/" + txtDirectoryname.Text.Split('/')[1].ToString() + "/" + txtDocumentName.Text);

                }
                else if (ddlSystemDirectoryType.SelectedIndex == 6)
                {
                    NewDirectory = Server.MapPath(Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + Session["LocId"].ToString() + "/" + ddlSystemDirectoryType.SelectedValue + "/" + txtDirectoryname.Text.Split('/')[1].ToString() + "/" + txtDocumentName.Text);

                }
                else
                {
                    NewDirectory = Server.MapPath(Session["CompId"].ToString() + "/" + ddlSystemDirectoryType.SelectedValue + "/" + hdnDirectoryId.Value + "/" + txtDocumentName.Text);

                }

                try
                {
                    int i = CreateDirectoryIfNotExist(NewDirectory);
                }
                catch
                {
                }
                if (ddlSystemDirectoryType.SelectedIndex == 1)
                {
                    filepath = "~/" + "ArcaWing" + "/" + Session["CompId"].ToString() + "/" + ddlSystemDirectoryType.SelectedValue + "/" + txtDirectoryname.Text.Split('/')[2] + "/" + txtDocumentName.Text + "/" + UploadFile.FileName;
                }
                else if (ddlSystemDirectoryType.SelectedIndex == 2)
                {
                    filepath = "~/" + "ArcaWing" + "/" + Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + ddlSystemDirectoryType.SelectedValue + "/" + txtDirectoryname.Text + "/" + txtDocumentName.Text + "/" + UploadFile.FileName;
                }
                else if (ddlSystemDirectoryType.SelectedIndex == 3)
                {
                    filepath = "~/" + "ArcaWing/" + ddlSystemDirectoryType.SelectedValue + "/" + txtDirectoryname.Text.Split('/')[1].ToString() + "/" + txtDocumentName.Text + "/" + UploadFile.FileName;

                }
                else if (ddlSystemDirectoryType.SelectedIndex == 4 || ddlSystemDirectoryType.SelectedIndex == 5)
                {
                    filepath = "~/" + "ArcaWing/" + Session["CompId"].ToString() + "/" + ddlSystemDirectoryType.SelectedValue + "/" + txtDirectoryname.Text.Split('/')[1].ToString() + "/" + txtDocumentName.Text + "/" + UploadFile.FileName;

                }
                else if (ddlSystemDirectoryType.SelectedIndex == 6)
                {
                    filepath = "~/" + "ArcaWing/" + Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + Session["LocId"].ToString() + "/" + ddlSystemDirectoryType.SelectedValue + "/" + txtDirectoryname.Text.Split('/')[1].ToString() + "/" + txtDocumentName.Text + "/" + UploadFile.FileName;

                }
                else
                {
                    filepath = "~/" + "ArcaWing" + "/" + Session["CompId"].ToString() + "/" + ddlSystemDirectoryType.SelectedValue + "/" + hdnDirectoryId.Value + "/" + txtDocumentName.Text + "/" + UploadFile.FileName;

                }
            }

            if (!Directory.Exists(filepath))
            {
                UploadFile.SaveAs(Server.MapPath(filepath));
            }



        }
        catch
        {

        }
        //code end

        //here we get the file name

        //code start

        string NewfileName = string.Empty;
        string ext = string.Empty;
        string filename = UploadFile.FileName;

        if (txtFileName.Text != "")
        {
            try
            {
                ext = Path.GetExtension(filepath);

                NewfileName = txtFileName.Text + "" + ext;
            }
            catch
            {

            }
        }
        else
        {
            NewfileName = UploadFile.FileName;
        }
        try
        {
            NewfileName = NewfileName.Replace(',', '.');
        }
        catch
        {

        }

        //code end

        //this code for convert the file in bytes for save in database

        //code start
        Byte[] bytes = new Byte[0];
        try
        {
            bytes = FileToByteArray(Server.MapPath(filepath));
        }
        catch
        {
        }

        //code end

        if (editid.Value == "")
        {
            string FileTypeId = "0";

            DataTable dt = ObjFile.Get_FileTransaction(Session["CompId"].ToString(), FileTypeId);


            string UserId = string.Empty;

            if (rbtnUsergenerated.Checked == true)
            {
                if (Session["EmpId"].ToString() == "0")
                {
                    UserId = ddlLoginUserName.SelectedValue;
                }
                else
                {
                    UserId = Session["UserId"].ToString();

                }
            }
            else
            {
                UserId = Session["UserId"].ToString();
            }

            if (rbtnSystemgenerated.Checked == true)
            {
                CreateDirectory(ddlSystemDirectoryType.SelectedValue, hdnDirectoryId.Value, UserId);
            }


            if (rbtnSystemgenerated.Checked && ddlSystemDirectoryType.SelectedIndex == 3)
            {
                //for contact master we will insert 0 value in company id because this page is on systemlevel so
                b = ObjFile.Insert_In_FileTransaction("0", hdnDirectoryId.Value, hdndocumentid.Value, "0", NewfileName, DateTime.Now.ToString(), bytes, filepath, txtExpiryDate.Text, txtdesc.Text, "0", UploadFile.FileName, "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), UserId, DateTime.Now.ToString(), UserId, DateTime.Now.ToString());
            }
            else
            {
                b = ObjFile.Insert_In_FileTransaction(Session["CompId"].ToString(), hdnDirectoryId.Value, hdndocumentid.Value, "0", NewfileName, DateTime.Now.ToString(), bytes, filepath, txtExpiryDate.Text, txtdesc.Text, "0", UploadFile.FileName, "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), UserId, DateTime.Now.ToString(), UserId, DateTime.Now.ToString());
            }



            if (b != 0)
            {
                if (ddlSetReminder.SelectedValue == "Yes")
                {
                    DateTime before_4_days = Convert.ToDateTime(txtExpiryDate.Text);
                    string date_before_4_days = GetDate((before_4_days.AddDays(-4)).ToString());
                    string message = "Your Document ('" + txtDocumentName.Text + "')  and File Name('" + NewfileName + "') Will Expire After 4 days";
                    int reminder_id_4 = objReminder.insertData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Arc_File_Transaction", b.ToString(), message, "", date, "1", date_before_4_days, "Once", Session["EmpId"].ToString(), "On", "false", "false", "true", Session["UserId"].ToString(), Session["UserId"].ToString());
                    objReminderlog.insertLogData(reminder_id_4.ToString(), date_before_4_days, "", Session["UserId"].ToString(), Session["UserId"].ToString());
                    objReminder.Set_Url(reminder_id_4.ToString(), "../CRM/Reminder.aspx?ReminderID=" + reminder_id_4 + "");
                }

                DisplayMessage("Record Saved", "green");

                rbtnListUsergenerated.Checked = true;
                rbtnListSystemgenerated.Checked = false;

                rbtnListUsergenerated_OnCheckedChanged(null, null);


                Reset();
            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }
        else
        {
            string FileTransactionid = "0";
            DataTable dt = ObjFile.Get_FileTransaction(Session["CompId"].ToString(), FileTransactionid);

            //if (Session["EmpId"].ToString().Trim() != "0")
            //{
            //    try
            //    {
            //        dt = new DataView(dt, "CreatedBy='" + Session["UserId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //    }
            //    catch
            //    {
            //    }
            //}
            string FileTransaction = string.Empty;


            try
            {
                FileTransaction = (new DataView(dt, "Trans_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["File_Name"].ToString();
            }
            catch
            {
                FileTransaction = "";
            }

            dt = new DataView(dt, "File_Name='" + txtFileName.Text + "' and File_Name<>'" + FileTransaction + "'  ", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                DisplayMessage("File Name Already Exists");
                txtFileName.Focus();
                txtFileName.Text = "";
                return;

            }

            b = ObjFile.Update_In_FileTransaction(Session["CompId"].ToString(), editid.Value, hdnDirectoryId.Value, hdndocumentid.Value, ddlFiletype.SelectedValue, NewfileName, DateTime.Now.ToString(), bytes, filepath, DateTime.Now.ToString(), "", "0", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            if (b != 0)
            {
                DisplayMessage("Record Updated", "green");
                Reset();
                FillGrid();


            }
            else
            {
                DisplayMessage("Record Not Updated");
            }

        }
    }

    public byte[] FileToByteArray(string fileName)
    {
        byte[] buff = null;
        FileStream fs = new FileStream(fileName,
                                       FileMode.Open,
                                       FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);
        long numBytes = new FileInfo(fileName).Length;
        buff = br.ReadBytes((int)numBytes);
        return buff;
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        rbtnListUsergenerated.Checked = true;
        rbtnListSystemgenerated.Checked = false;
        rbtnListUsergenerated_OnCheckedChanged(null, null);
        FillGridBin();
        Reset();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    public void CreateDirectory(string DirectoryType, string EmployeeId, string UserId)
    {
        string DirectoryName = string.Empty;
        if (ddlSystemDirectoryType.SelectedIndex == 1)
        {
            DirectoryName = Session["CompId"].ToString() + "/" + DirectoryType + "/" + txtDirectoryname.Text.Split('/')[2].Trim();
        }
        else if (ddlSystemDirectoryType.SelectedIndex == 2)
        {
            DirectoryName = Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + DirectoryType + "/" + txtDirectoryname.Text;
        }
        else if (ddlSystemDirectoryType.SelectedIndex == 3)
        {
            DirectoryName = DirectoryType + "/" + txtDirectoryname.Text.Split('/')[1].Trim();
        }
        else if (ddlSystemDirectoryType.SelectedIndex == 4 || ddlSystemDirectoryType.SelectedIndex == 5)
        {
            DirectoryName = Session["CompId"].ToString() + "/" + DirectoryType + "/" + txtDirectoryname.Text.Split('/')[1].Trim();
        }
        else if (ddlSystemDirectoryType.SelectedIndex == 6)
        {
            DirectoryName = Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/" + Session["LocId"].ToString() + "/" + DirectoryType + "/" + txtDirectoryname.Text.Split('/')[1].Trim();
        }
        else
        {
            DirectoryName = Session["CompId"].ToString() + "/" + DirectoryType + "/" + EmployeeId;
        }

        DataTable DtDir = ObjDirectory.GetDirectoryMaster_By_DirectoryName(Session["compId"].ToString(), DirectoryName);

        if (DtDir.Rows.Count == 0)
        {
            int b = ObjDirectory.InsertDirectorymaster(Session["CompId"].ToString(), DirectoryName, "1", Session["BrandId"].ToString(), EmployeeId, DirectoryType, Session["LocId"].ToString(), false.ToString(), DateTime.Now.ToString(), true.ToString(), UserId, DateTime.Now.ToString(), UserId, DateTime.Now.ToString(),HttpContext.Current.Session["EmpId"].ToString());

            if (b != 0)
            {
                string NewDirectory = string.Empty;


                NewDirectory = Server.MapPath(DirectoryName);




                int i = CreateDirectoryIfNotExist(NewDirectory);

                hdnDirectoryId.Value = b.ToString();

            }
            else
            {
                //DisplayMessage("Record Not Saved");
            }
        }
        else
        {
            hdnDirectoryId.Value = DtDir.Rows[0]["Id"].ToString();
        }
    }
    protected void ddlSystemDirectoryType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Sys_DirectoryType"] = ddlSystemDirectoryType.SelectedIndex;
        txtDirectoryname.Text = "";
    }
    protected void ddlLoginUserName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlLoginUserName.SelectedIndex == 0)
        {
            txtDirectoryname.Text = "";
            Session["LoginUserDirectory"] = null;
            return;
        }
        else
        {
            txtDirectoryname.Text = "";
            DataTable dtDirectory = ObjDirectory.getDirectoryMaster(HttpContext.Current.Session["CompId"].ToString(), "0");


            try
            {
                dtDirectory = new DataView(dtDirectory, "Field1='" + HttpContext.Current.Session["FileGeneratedType"].ToString() + "' and CreatedBy='" + ddlLoginUserName.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            catch
            {
            }

            Session["LoginUserDirectory"] = dtDirectory;

        }



    }
    protected void rbtnUsergenerated_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtnUsergenerated.Checked == true)
        {

            Session["FileGeneratedType"] = "0";
            lblSystemDirectoryType.Visible = false;
            //lblSystemDirectoryTypecolon.Visible = false;
            ddlSystemDirectoryType.Visible = false;
            if (Session["EmpId"].ToString() == "0")
            {
                lblLoginUser.Visible = true;
                //lblLoginUserColon.Visible = true;
                ddlLoginUserName.Visible = true;
                ddlLoginUserName.SelectedIndex = 0;
                Session["LoginUserDirectory"] = null;
            }
            else
            {
                DataTable dtDirectory = ObjDirectory.getDirectoryMaster(HttpContext.Current.Session["CompId"].ToString(), "0");
                try
                {
                    dtDirectory = new DataView(dtDirectory, "Field1='0' and CreatedBy='" + Session["UserId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {

                }
                Session["LoginUserDirectory"] = dtDirectory;
                lblLoginUser.Visible = false;
                //lblLoginUserColon.Visible = false;
                ddlLoginUserName.Visible = false;
            }
            txtDirectoryname.Text = "";

        }
        if (rbtnSystemgenerated.Checked == true)
        {

            Session["FileGeneratedType"] = "1";
            Session["Sys_DirectoryType"] = "0";
            ddlSystemDirectoryType.SelectedIndex = 0;
            lblSystemDirectoryType.Visible = true;
            //lblSystemDirectoryTypecolon.Visible = true;
            ddlSystemDirectoryType.Visible = true;
            txtDirectoryname.Text = "";
            lblLoginUser.Visible = false;
            //lblLoginUserColon.Visible = false;
            Session["LoginUserDirectory"] = null;
            ddlLoginUserName.Visible = false;

        }
    }
    protected void rbtnListUsergenerated_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rbtnListUsergenerated.Checked == true)
        {
            //commented by jitendra because  filter panel exists in aspx gridview
            //01-june-2018
            //start
            //pnlSearchRecords.Visible = true;
            //txtValue.Text = "";
            //end
            panelFileGrid.Visible = true;

            panelSystemGeneratedType.Visible = false;

            ddlType.SelectedIndex = 0;
            ddlType_OnSelectedIndexChanged(null, null);
            FillGrid();

        }
        if (rbtnListSystemgenerated.Checked == true)
        {
            ddlType.SelectedIndex = 0;
            panelSystemGeneratedType.Visible = true;
            //commented by jitendra because  filter panel exists in aspx gridview
            //01-june-2018
            //start
            //pnlSearchRecords.Visible = false;
            //end
            panelFileGrid.Visible = false;
            //ReportPanel.Visible = false;
            chkGroupBy.Visible = false;
            btnReport.Visible = false;
        }


    }
    protected void ddlType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlType.SelectedIndex == 0)
        {
            pnlEmployee.Visible = false;
            panelCustomer.Visible = false;
            panelSupplier.Visible = false;
            panelContact.Visible = false;
            panelProduct.Visible = false;
            pnlGoButtton.Visible = true;
            btnGo.Visible = false;
            btnback.Visible = false;
            //ReportPanel.Visible = false;
            chkGroupBy.Visible = false;
            btnReport.Visible = false;
            pnlProject.Visible = false;
        }
        if (ddlType.SelectedIndex == 1)
        {
            pnlGoButtton.Visible = true;
            FillEmployee();
            lblSelectedRecordEmployee.Text = "";
            pnlEmployee.Visible = true;
            panelCustomer.Visible = false;
            panelSupplier.Visible = false;
            panelContact.Visible = false;
            panelProduct.Visible = false;
            //ReportPanel.Visible = false;
            chkGroupBy.Visible = false;
            btnReport.Visible = false;
            pnlProject.Visible = false;

        }
        if (ddlType.SelectedIndex == 2)
        {

            pnlGoButtton.Visible = true;
            FillProductList();
            lblSelectedRecordProduct.Text = "";
            pnlEmployee.Visible = false;
            panelCustomer.Visible = false;
            panelSupplier.Visible = false;
            panelContact.Visible = false;
            panelProduct.Visible = true;
            //ReportPanel.Visible = false;
            chkGroupBy.Visible = false;
            btnReport.Visible = false;
            pnlProject.Visible = false;



        }
        if (ddlType.SelectedIndex == 3)
        {
            pnlGoButtton.Visible = true;
            FillContactList();
            lblSelectedRecordContact.Text = "";
            pnlEmployee.Visible = false;
            panelCustomer.Visible = false;
            panelSupplier.Visible = false;
            panelContact.Visible = true;
            panelProduct.Visible = false;
            //ReportPanel.Visible = false;
            chkGroupBy.Visible = false;
            btnReport.Visible = false;
            pnlProject.Visible = false;
        }
        if (ddlType.SelectedIndex == 4)
        {
            pnlGoButtton.Visible = true;
            FillCustomerList();
            lblSelectedRecordCustomer.Text = "";
            pnlEmployee.Visible = false;
            panelCustomer.Visible = true;
            panelSupplier.Visible = false;
            panelContact.Visible = false;
            panelProduct.Visible = false;
            //ReportPanel.Visible = false;
            chkGroupBy.Visible = false;
            btnReport.Visible = false;
            pnlProject.Visible = false;
        }
        if (ddlType.SelectedIndex == 5)
        {
            pnlGoButtton.Visible = true;
            FillSupplierList();
            lblSelectedRecordSupplier.Text = "";
            pnlEmployee.Visible = false;
            panelCustomer.Visible = false;
            panelSupplier.Visible = true;
            panelContact.Visible = false;
            panelProduct.Visible = false;
            //ReportPanel.Visible = false;
            chkGroupBy.Visible = false;
            btnReport.Visible = false;
            pnlProject.Visible = false;

        }
        if (ddlType.SelectedIndex == 6)
        {
            pnlGoButtton.Visible = true;
            FillProjectList();
            lblSelectedRecordProject.Text = "";
            pnlEmployee.Visible = false;
            panelCustomer.Visible = false;
            panelSupplier.Visible = false;
            panelContact.Visible = false;
            panelProduct.Visible = false;
            //ReportPanel.Visible = false;
            chkGroupBy.Visible = false;
            btnReport.Visible = false;
            pnlProject.Visible = true;

        }

    }
    private void PopulateAllGridChkbox(GridView gvr)
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvr.Rows)
            {
                int index = (int)gvr.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    public void SaveAllGridChkbox(GridView GvCheckBox)
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in GvCheckBox.Rows)
        {
            index = (int)GvCheckBox.DataKeys[gvrow.RowIndex].Value;
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

    protected void btnGo_OnClick(object sender, EventArgs e)
    {
        if (ddlType.SelectedIndex == 1)
        {
            if (gvEmp.Rows.Count == 0)
            {
                DisplayMessage("Employee Not Found");
                return;
            }
            SaveAllGridChkbox(gvEmp);

            if (Session["CHECKED_ITEMS"] != null)
            {
                ArrayList Empids = new ArrayList();
                Empids = (ArrayList)Session["CHECKED_ITEMS"];
                if (Empids.Count > 0)
                {
                    foreach (int item in Empids)
                    {
                        if (!lblSelectedRecordEmployee.Text.Contains(item.ToString()))
                        {
                            lblSelectedRecordEmployee.Text += item.ToString() + ",";
                        }
                    }
                }
                else
                {
                    DisplayMessage("Select Employee First");
                    gvEmp.Focus();
                    return;
                }
            }
            if (lblSelectedRecordEmployee.Text == "")
            {
                DisplayMessage("Select Employee First");
                return;
            }
        }

        if (ddlType.SelectedIndex == 2)
        {
            if (gvProduct.Rows.Count == 0)
            {
                DisplayMessage("Product Not Found");
                return;
            }
            SaveAllGridChkbox(gvProduct);

            if (Session["CHECKED_ITEMS"] != null)
            {
                ArrayList Empids = new ArrayList();
                Empids = (ArrayList)Session["CHECKED_ITEMS"];
                if (Empids.Count > 0)
                {
                    foreach (int item in Empids)
                    {
                        if (!lblSelectedRecordProduct.Text.Contains(item.ToString()))
                        {
                            lblSelectedRecordProduct.Text += item.ToString() + ",";
                        }
                    }
                }
                else
                {
                    DisplayMessage("Select Product First");
                    gvEmp.Focus();
                    return;
                }
            }
            if (lblSelectedRecordProduct.Text == "")
            {
                DisplayMessage("Select Product First");
                return;
            }
        }



        if (ddlType.SelectedIndex == 3)
        {
            if (GvContact.Rows.Count == 0)
            {
                DisplayMessage("Contact Not Found");
                return;
            }
            SaveAllGridChkbox(GvContact);

            if (Session["CHECKED_ITEMS"] != null)
            {
                ArrayList Empids = new ArrayList();
                Empids = (ArrayList)Session["CHECKED_ITEMS"];
                if (Empids.Count > 0)
                {
                    foreach (int item in Empids)
                    {
                        if (!lblSelectedRecordContact.Text.Contains(item.ToString()))
                        {
                            lblSelectedRecordContact.Text += item.ToString() + ",";
                        }
                    }
                }
                else
                {
                    DisplayMessage("Select Contact First");
                    gvEmp.Focus();
                    return;
                }
            }
            if (lblSelectedRecordContact.Text == "")
            {
                DisplayMessage("Select Contact First");
                return;
            }
        }



        if (ddlType.SelectedIndex == 4)
        {
            if (GvCustomer.Rows.Count == 0)
            {
                DisplayMessage("Customer Not Found");
                return;
            }
            SaveAllGridChkbox(GvCustomer);

            if (Session["CHECKED_ITEMS"] != null)
            {
                ArrayList Empids = new ArrayList();
                Empids = (ArrayList)Session["CHECKED_ITEMS"];
                if (Empids.Count > 0)
                {
                    foreach (int item in Empids)
                    {
                        if (!lblSelectedRecordCustomer.Text.Contains(item.ToString()))
                        {
                            lblSelectedRecordCustomer.Text += item.ToString() + ",";
                        }
                    }
                }
                else
                {
                    DisplayMessage("Select Customer First");
                    gvEmp.Focus();
                    return;
                }
            }
            if (lblSelectedRecordCustomer.Text == "")
            {
                DisplayMessage("Select Customer First");
                return;
            }
        }

        if (ddlType.SelectedIndex == 5)
        {
            if (GvSupplier.Rows.Count == 0)
            {
                DisplayMessage("Supplier Not Found");
                return;
            }
            SaveAllGridChkbox(GvSupplier);

            if (Session["CHECKED_ITEMS"] != null)
            {
                ArrayList Empids = new ArrayList();
                Empids = (ArrayList)Session["CHECKED_ITEMS"];
                if (Empids.Count > 0)
                {
                    foreach (int item in Empids)
                    {
                        if (!lblSelectedRecordSupplier.Text.Contains(item.ToString()))
                        {
                            lblSelectedRecordSupplier.Text += item.ToString() + ",";
                        }
                    }
                }
                else
                {
                    DisplayMessage("Select Supplier First");
                    gvEmp.Focus();
                    return;
                }
            }
            if (lblSelectedRecordSupplier.Text == "")
            {
                DisplayMessage("Select Supplier First");
                return;
            }
        }

        if (ddlType.SelectedIndex == 6)
        {
            if (GvrProjectteam.Rows.Count == 0)
            {
                DisplayMessage("Project Not Found");
                return;
            }
            SaveAllGridChkbox(GvrProjectteam);

            if (Session["CHECKED_ITEMS"] != null)
            {
                ArrayList Empids = new ArrayList();
                Empids = (ArrayList)Session["CHECKED_ITEMS"];
                if (Empids.Count > 0)
                {
                    foreach (int item in Empids)
                    {
                        if (!lblSelectedRecordProject.Text.Contains(item.ToString()))
                        {
                            lblSelectedRecordProject.Text += item.ToString() + ",";
                        }
                    }
                }
                else
                {
                    DisplayMessage("Select Project First");
                    gvEmp.Focus();
                    return;
                }
            }
            if (lblSelectedRecordProject.Text == "")
            {
                DisplayMessage("Select Project First");
                return;
            }
        }



        FillGrid();

        if (gvFileMaster.Rows.Count > 0)
        {
            pnlEmployee.Visible = false;
            panelCustomer.Visible = false;
            panelSupplier.Visible = false;
            panelContact.Visible = false;
            panelProduct.Visible = false;
            //commented by jitendra because  filter panel exists in aspx gridview
            //01-june-2018
            //start
            //pnlSearchRecords.Visible = true;
            //end
            panelFileGrid.Visible = true;
            //ReportPanel.Visible = true;
            chkGroupBy.Visible = true;
            btnReport.Visible = true;
            pnlProject.Visible = false;

            panelSystemGeneratedType.Visible = false;
            lblSelectedRecordEmployee.Text = "";
            lblSelectedRecordSupplier.Text = "";
            lblSelectedRecordContact.Text = "";
            lblSelectedRecordCustomer.Text = "";
            lblSelectedRecordProduct.Text = "";
            lblSelectedRecordProject.Text = "";
            btnGo.Visible = false;
            btnback.Visible = true;
        }
        else
        {
            DisplayMessage("Record Not Found");
            lblSelectedRecordEmployee.Text = "";
            lblSelectedRecordSupplier.Text = "";
            lblSelectedRecordContact.Text = "";
            lblSelectedRecordCustomer.Text = "";
            lblSelectedRecordProduct.Text = "";
            lblSelectedRecordProject.Text = "";
            lblSelectedRecordProject.Text = "";
            return;
        }




    }
    protected void btnback_OnClick(object sender, EventArgs e)
    {

        if (ddlType.SelectedIndex == 1)
        {
            pnlEmployee.Visible = true;
            panelCustomer.Visible = false;
            panelSupplier.Visible = false;
            panelContact.Visible = false;
            panelProduct.Visible = false;
            //commented by jitendra because  filter panel exists in aspx gridview
            //01-june-2018
            //start
            //pnlSearchRecords.Visible = false;
            //end
            panelFileGrid.Visible = false;
            btnGo.Visible = true;
            btnback.Visible = false;
            //ReportPanel.Visible = false;
            chkGroupBy.Visible = false;
            btnReport.Visible = false;
            panelSystemGeneratedType.Visible = true;
            pnlProject.Visible = false;
        }
        if (ddlType.SelectedIndex == 2)
        {
            pnlEmployee.Visible = false;
            panelCustomer.Visible = false;
            panelSupplier.Visible = false;
            panelContact.Visible = false;
            panelProduct.Visible = true;
            //commented by jitendra because  filter panel exists in aspx gridview
            //01-june-2018
            //start
            //pnlSearchRecords.Visible = false;
            //end
            panelFileGrid.Visible = false;
            btnGo.Visible = true;
            btnback.Visible = false;
            //ReportPanel.Visible = false;
            chkGroupBy.Visible = false;
            btnReport.Visible = false;
            panelSystemGeneratedType.Visible = true;
            pnlProject.Visible = false;

        }
        if (ddlType.SelectedIndex == 3)
        {
            pnlEmployee.Visible = false;
            panelCustomer.Visible = false;
            panelSupplier.Visible = false;
            panelContact.Visible = true;
            panelProduct.Visible = false;
            //commented by jitendra because  filter panel exists in aspx gridview
            //01-june-2018
            //start
            //pnlSearchRecords.Visible = false;
            //end
            panelFileGrid.Visible = false;
            btnGo.Visible = true;
            btnback.Visible = false;
            //ReportPanel.Visible = false;
            chkGroupBy.Visible = false;
            btnReport.Visible = false;
            panelSystemGeneratedType.Visible = true;
            pnlProject.Visible = false;
        }
        if (ddlType.SelectedIndex == 4)
        {
            pnlEmployee.Visible = false;
            panelCustomer.Visible = true;
            panelSupplier.Visible = false;
            panelContact.Visible = false;
            panelProduct.Visible = false;
            //commented by jitendra because  filter panel exists in aspx gridview
            //01-june-2018
            //start
            //pnlSearchRecords.Visible = false;
            //end
            panelFileGrid.Visible = false;
            btnGo.Visible = true;
            btnback.Visible = false;
            //ReportPanel.Visible = false;
            chkGroupBy.Visible = false;
            btnReport.Visible = false;
            panelSystemGeneratedType.Visible = true;
            pnlProject.Visible = false;
        }
        if (ddlType.SelectedIndex == 5)
        {
            pnlEmployee.Visible = false;
            panelCustomer.Visible = false;
            panelSupplier.Visible = true;
            panelContact.Visible = false;
            panelProduct.Visible = false;
            //commented by jitendra because  filter panel exists in aspx gridview
            //01-june-2018
            //start
            //pnlSearchRecords.Visible = false;
            //end
            panelFileGrid.Visible = false;
            btnGo.Visible = true;
            btnback.Visible = false;
            //ReportPanel.Visible = false;
            chkGroupBy.Visible = false;
            btnReport.Visible = false;
            panelSystemGeneratedType.Visible = true;
            pnlProject.Visible = false;
        }
        if (ddlType.SelectedIndex == 6)
        {
            pnlEmployee.Visible = false;
            panelCustomer.Visible = false;
            panelSupplier.Visible = false;
            pnlProject.Visible = true;
            panelContact.Visible = false;
            panelProduct.Visible = false;
            //commented by jitendra because  filter panel exists in aspx gridview
            //01-june-2018
            //start
            //pnlSearchRecords.Visible = false;
            //end
            panelFileGrid.Visible = false;
            btnGo.Visible = true;
            btnback.Visible = false;
            //ReportPanel.Visible = false;
            chkGroupBy.Visible = false;
            btnReport.Visible = false;
            panelSystemGeneratedType.Visible = true;
        }

    }
    protected void btnReport_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["File"];
        if (dt.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }


        string Directorylist = string.Empty;

        foreach (DataRow dr in dt.Rows)
        {
            if (!Directorylist.Split(',').Contains(dr["Directory_Id"].ToString()))
            {
                Directorylist += dr["Directory_Id"].ToString() + ",";

            }
        }

        Session["DirectoryLIst"] = Directorylist;
        Session["ClaimRecord"] = dt;
        if (chkGroupBy.Checked == true)
        {
            Session["Document_Group"] = "YES";
        }
        else
        {
            Session["Document_Group"] = "NO";

        }

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/DirectoryReport.aspx')", true);



    }
    #region EmployeeList

    public void FillEmployee()
    {
        string loc = Session["LocId"].ToString();
        DataTable dtproduct = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());

        dtproduct = new DataView(dtproduct, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        string strDepId = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", Session["LocId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (strDepId == "")
        {
            strDepId = "0,";
        }


        dtproduct = new DataView(dtproduct, "Department_Id in(" + strDepId.Substring(0, strDepId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();



        Session["dtEmp"] = dtproduct;

        if (dtproduct.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmp, dtproduct, "", "");
            pnlGoButtton.Visible = true;
            btnGo.Visible = true;
            btnback.Visible = false;
        }
        else
        {
            pnlGoButtton.Visible = false;
            gvEmp.DataSource = null;
            gvEmp.DataBind();
            btnGo.Visible = false;
            btnback.Visible = false;

        }
        lblTotalRecordsEmployee.Text = Resources.Attendance.Total_Records + ": " + dtproduct.Rows.Count.ToString() + "";


    }
    protected void gvEmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveAllGridChkbox(gvEmp);
        gvEmp.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtEmp"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmp, dt, "", "");
        PopulateAllGridChkbox(gvEmp);
        //AllPageCode();
    }
    protected void btnRefreshEmployee_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        Session["CHECKED_ITEMS"] = null;
        FillEmployee();
        ddlOptionEmployee.SelectedIndex = 3;
        ddlFieldNameEmployee.SelectedIndex = 0;
        txtvalueEmployee.Text = "";
        txtvalueEmployee.Focus();
        lblSelectedRecordEmployee.Text = "";
        ViewState["SelectEmployee"] = null;
    }

    protected void btnBindEmployee_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        Session["CHECKED_ITEMS"] = null;

        if (ddlOptionEmployee.SelectedIndex != 0)
        {

            string condition = string.Empty;


            if (ddlOptionEmployee.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldNameEmployee.SelectedValue + ",System.String)='" + txtvalueEmployee.Text.Trim() + "'";
            }
            else if (ddlOptionEmployee.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNameEmployee.SelectedValue + ",System.String) like '" + txtvalueEmployee.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNameEmployee.SelectedValue + ",System.String) Like '%" + txtvalueEmployee.Text.Trim() + "%'";
            }
            DataTable dtCust = (DataTable)Session["dtEmp"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);

            lblTotalRecordsEmployee.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmp, view.ToTable(), "", "");
            if (view.ToTable().Rows.Count > 0)
                pnlGoButtton.Visible = true;
            else
                pnlGoButtton.Visible = false;
            //AllPageCode();
            btnRefreshEmployee.Focus();
        }
        txtvalueEmployee.Focus();
    }
    protected void ImgbtnSelectAllEmployee_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        DataTable dtUnit = (DataTable)Session["dtEmp"];
        ArrayList userdetails = new ArrayList();
        Session["CHECKED_ITEMS"] = null;

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["CHECKED_ITEMS"] != null)
                {
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                }

                if (!userdetails.Contains(dr["Emp_Id"]))
                {
                    userdetails.Add(dr["Emp_Id"]);
                }
            }
            foreach (GridViewRow GR in gvEmp.Rows)
            {
                ((CheckBox)GR.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails.Count > 0 && userdetails != null)
            {
                Session["CHECKED_ITEMS"] = userdetails;
            }
        }
        else
        {
            lblSelectedRecordEmployee.Text = "";
            DataTable dtProduct1 = (DataTable)Session["dtEmp"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmp, dtProduct1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void chkgvSelectAllEmployee_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvEmp.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvEmp.Rows)
        {

            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = false;
            }
        }

    }

    protected void gvEmp_Sorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        DataTable dt = (DataTable)Session["dtEmp"];
        string sortdir = "DESC";
        if (ViewState["SortDirBin"] != null)
        {
            sortdir = ViewState["SortDirBin"].ToString();
            if (sortdir == "ASC")
            {
                e.SortDirection = SortDirection.Descending;
                ViewState["SortDirBin"] = "DESC";
            }
            else
            {
                e.SortDirection = SortDirection.Ascending;
                ViewState["SortDirBin"] = "ASC";
            }
        }
        else
        {
            ViewState["SortDirBin"] = "DESC";
        }
        dt = (new DataView(dt, "", e.SortExpression + " " + ViewState["SortDirBin"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        Session["dtContact"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmp, dt, "", "");
        gvEmp.HeaderRow.Focus();
    }
    #endregion
    #region ProductLIst
    protected void btnBindProduct_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        Session["CHECKED_ITEMS"] = null;
        DataTable dtproduct = new DataTable();
        dtproduct = (DataTable)Session["dtProduct"];
        DataTable dtproductBrandSearch = new DataTable();
        DataTable dtproductCateSearch = new DataTable();

        if (ddlOptionproduct.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOptionproduct.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldNameProduct.SelectedValue + ",System.String)='" + txtValueProduct.Text.Trim() + "'";
            }
            else if (ddlOptionproduct.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNameProduct.SelectedValue + ",System.String) Like '" + txtValueProduct.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNameProduct.SelectedValue + ",System.String) like '%" + txtValueProduct.Text.Trim() + "%'";
            }
            DataView view = new DataView(dtproduct, condition, "", DataViewRowState.CurrentRows);

            lblTotalRecordProduct.Text = Resources.Attendance.Total_Records + " : " + (view.ToTable()).Rows.Count.ToString() + "";



            dtproduct = view.ToTable();
            if (dtproduct.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)gvProduct, dtproduct, "", "");
                pnlGoButtton.Visible = true;
            }
            else
            {
                pnlGoButtton.Visible = false;
                gvProduct.DataSource = null;
                gvProduct.DataBind();
            }
        }
        txtValueProduct.Focus();
    }
    protected void btnRefreshProduct_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        FillProductList();
        ddlFieldNameProduct.SelectedIndex = 1;
        ddlOptionproduct.SelectedIndex = 3;
        Session["CHECKED_ITEMS"] = null;

        lblSelectedRecordProduct.Text = "";
        txtValueProduct.Text = "";
        txtValueProduct.Focus();
        ViewState["SelectPrdouct"] = null;

    }
    private void FillProductList()
    {
        DataTable dtproduct = ObjProductMaster.GetProductMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString());


        DataTable dtproductBrandSearch = new DataTable();
        DataTable dtproductCateSearch = new DataTable();



        Session["dtProduct"] = dtproduct;
        Session["dtProductFilter"] = dtproduct;

        if (dtproduct.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvProduct, dtproduct, "", "");
            pnlGoButtton.Visible = true;
            btnGo.Visible = true;
            btnback.Visible = false;
        }
        else
        {
            pnlGoButtton.Visible = false;
            gvProduct.DataSource = null;
            gvProduct.DataBind();
            btnGo.Visible = false;
            btnback.Visible = false;

        }

        lblTotalRecordProduct.Text = Resources.Attendance.Total_Records + " : " + dtproduct.Rows.Count.ToString();
        //AllPageCode();
    }
    protected void ImgbtnSelectAllProduct_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        DataTable dtUnit = (DataTable)Session["dtProductFilter"];
        ArrayList userdetails = new ArrayList();
        Session["CHECKED_ITEMS"] = null;

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["CHECKED_ITEMS"] != null)
                {
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                }

                if (!userdetails.Contains(dr["ProductId"]))
                {
                    userdetails.Add(dr["ProductId"]);
                }
            }
            foreach (GridViewRow GR in gvProduct.Rows)
            {
                ((CheckBox)GR.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails.Count > 0 && userdetails != null)
            {
                Session["CHECKED_ITEMS"] = userdetails;
            }
        }
        else
        {
            lblSelectedRecordProduct.Text = "";
            DataTable dtProduct1 = (DataTable)Session["dtProductFilter"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvProduct, dtProduct1, "", "");
            //AllPageCode();
            ViewState["Select"] = null;
        }
    }

    protected void chkgvSelectAllProduct_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvProduct.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvProduct.Rows)
        {

            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = false;
            }
        }
    }
    public string GetItemType(string IT)
    {
        string retval = string.Empty;
        if (IT == "A")
        {
            retval = "Assemble";
        }
        if (IT == "K")
        {
            retval = "KIT";
        }
        if (IT == "NS")
        {
            retval = "Non Stockable";
        }
        if (IT == "S")
        {
            retval = "Stockable";
        }
        return retval;

    }
    public string GetUnitName(string UnitId)
    {
        string ProductUnit = string.Empty;
        if (UnitId.ToString() != "")
        {
            DataTable dtUnit = ObjUnitMaster.GetUnitMasterById(Session["CompId"].ToString(), UnitId.ToString());
            if (dtUnit.Rows.Count != 0)
            {
                ProductUnit = dtUnit.Rows[0]["Unit_Name"].ToString();
            }
            return ProductUnit;
        }
        else
        {
            return ProductUnit = "No Unit";
        }
    }
    protected void gvProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveAllGridChkbox(gvProduct);
        gvProduct.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtProductFilter"];
        gvProduct.DataSource = dt;
        gvProduct.DataBind();
        PopulateAllGridChkbox(gvProduct);
        //AllPageCode();
    }
    protected void gvProduct_Sorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        DataTable dt = (DataTable)Session["dtProductFilter"];
        string sortdir = "DESC";
        if (ViewState["SortDirBin"] != null)
        {
            sortdir = ViewState["SortDirBin"].ToString();
            if (sortdir == "ASC")
            {
                e.SortDirection = SortDirection.Descending;
                ViewState["SortDirBin"] = "DESC";
            }
            else
            {
                e.SortDirection = SortDirection.Ascending;
                ViewState["SortDirBin"] = "ASC";
            }
        }
        else
        {
            ViewState["SortDirBin"] = "DESC";
        }
        dt = (new DataView(dt, "", e.SortExpression + " " + ViewState["SortDirBin"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        Session["dtProductFilter"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvProduct, dt, "", "");
        gvProduct.HeaderRow.Focus();
    }
    #endregion
    #region ContactList
    public void FillContactList()
    {
        DataTable dtContact = ObjContactMaster.GetContactTrueAllData();
        if (dtContact.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvContact, dtContact, "", "");
            pnlGoButtton.Visible = true;
            //AllPageCode();
            btnGo.Visible = true;
            btnback.Visible = false;
        }
        else
        {
            pnlGoButtton.Visible = false;
            GvContact.DataSource = null;
            GvContact.DataBind();
            btnGo.Visible = false;
            btnback.Visible = false;

        }
        lblTotalRecordContact.Text = Resources.Attendance.Total_Records + " : " + dtContact.Rows.Count.ToString();

        Session["dtContact"] = dtContact;

    }
    protected void btnBindContact_Click(object sender, ImageClickEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        lblSelectedRecordContact.Text = "";
        if (ddlOptionContact.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOptionContact.SelectedIndex == 1)
            {
                condition = ddlFieldNameContact.SelectedValue + "='" + txtvalueContact.Text.Trim() + "'";
            }
            else if (ddlOptionContact.SelectedIndex == 2)
            {
                condition = ddlFieldNameContact.SelectedValue + " Like '" + txtvalueContact.Text.Trim() + "%'";
            }
            else
            {
                condition = ddlFieldNameContact.SelectedValue + " like '%" + txtvalueContact.Text.Trim() + "%'";
            }
            DataTable dtContact = (DataTable)Session["dtContact"];
            DataView view = new DataView(dtContact, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvContact, view.ToTable(), "", "");
            if (view.ToTable().Rows.Count > 0)
                pnlGoButtton.Visible = true;
            else
                pnlGoButtton.Visible = false;
            lblTotalRecordContact.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";

            txtvalueContact.Focus();
        }
        txtvalueContact.Focus();
    }
    protected void btnRefreshContact_Click(object sender, ImageClickEventArgs e)
    {
        FillContactList(); ;
        txtvalueContact.Text = "";
        ddlOptionContact.SelectedIndex = 3;
        ddlFieldNameContact.SelectedIndex = 0;
        Session["CHECKED_ITEMS"] = null;
        txtvalueContact.Focus();
        lblSelectedRecordContact.Text = "";
        ViewState["Contact"] = null;
    }
    protected void ImgbtnSelectAllContact_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["dtContact"];
        ArrayList userdetails = new ArrayList();
        Session["CHECKED_ITEMS"] = null;

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["CHECKED_ITEMS"] != null)
                {
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                }

                if (!userdetails.Contains(dr["Trans_Id"]))
                {
                    userdetails.Add(dr["Trans_Id"]);
                }
            }
            foreach (GridViewRow GR in GvContact.Rows)
            {
                ((CheckBox)GR.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails.Count > 0 && userdetails != null)
            {
                Session["CHECKED_ITEMS"] = userdetails;
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtContact"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvContact, dtUnit1, "", "");
            ViewState["Select"] = null;
        }

    }

    protected void GvContact_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveAllGridChkbox(GvContact);
        GvContact.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtContact"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvContact, dt, "", "");
        PopulateAllGridChkbox(GvContact);
    }
    protected void chkActiveAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvContact.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in GvContact.Rows)
        {

            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = false;
            }
        }
    }

    protected void GvContact_Sorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        DataTable dt = (DataTable)Session["dtContact"];
        string sortdir = "DESC";
        if (ViewState["SortDirBin"] != null)
        {
            sortdir = ViewState["SortDirBin"].ToString();
            if (sortdir == "ASC")
            {
                e.SortDirection = SortDirection.Descending;
                ViewState["SortDirBin"] = "DESC";
            }
            else
            {
                e.SortDirection = SortDirection.Ascending;
                ViewState["SortDirBin"] = "ASC";
            }
        }
        else
        {
            ViewState["SortDirBin"] = "DESC";
        }
        dt = (new DataView(dt, "", e.SortExpression + " " + ViewState["SortDirBin"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        Session["dtContact"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvContact, dt, "", "");
        GvContact.HeaderRow.Focus();
    }
    #endregion
    #region CustomerList
    private void FillCustomerList()
    {
        DataTable dtCustomer = objCustomer.GetCustomerAllTrueData(Session["CompId"].ToString(), Session["BrandId"].ToString());
        Session["dtCustomer"] = dtCustomer;

        if (dtCustomer != null && dtCustomer.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvCustomer, dtCustomer, "", "");
            pnlGoButtton.Visible = true;
            btnGo.Visible = true;
            btnback.Visible = false;
        }
        else
        {
            pnlGoButtton.Visible = false;
            GvCustomer.DataSource = null;
            GvCustomer.DataBind();
            btnGo.Visible = false;
            btnback.Visible = false;
        }
        lblTotalRecordCustomer.Text = Resources.Attendance.Total_Records + " : " + dtCustomer.Rows.Count.ToString() + "";
        //AllPageCode();
        ViewState["Customer"] = null;
    }
    protected void btnBindCustomer_Click(object sender, EventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        string condition = string.Empty;
        if (ddlOptionCustomer.SelectedIndex != 0)
        {
            if (ddlOptionCustomer.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldNameCustomer.SelectedValue + ",System.String)='" + txtvalueCustomer.Text.Trim() + "'";
            }
            else if (ddlOptionCustomer.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNameCustomer.SelectedValue + ",System.String) like '" + txtvalueCustomer.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNameCustomer.SelectedValue + ",System.String) Like '%" + txtvalueCustomer.Text.Trim() + "%'";
            }


            DataTable dtCust = (DataTable)Session["dtCustomer"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);


            lblTotalRecordCustomer.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvCustomer, view.ToTable(), "", "");
            if (view.ToTable().Rows.Count > 0)
                pnlGoButtton.Visible = true;
            else
                pnlGoButtton.Visible = false;
            lblSelectedRecordCustomer.Text = "";
        }
        txtvalueCustomer.Focus();
    }
    protected void btnRefreshCustomer_Click(object sender, EventArgs e)
    {
        FillCustomerList();
        txtvalueCustomer.Text = "";
        ddlOptionCustomer.SelectedIndex = 3;
        ddlFieldNameCustomer.SelectedIndex = 1;
        lblSelectedRecordCustomer.Text = "";
        txtvalueCustomer.Focus();
        ViewState["Customer"] = null;
        Session["CHECKED_ITEMS"] = null;
    }
    protected void ImgbtnSelectAllCustomer_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["dtCustomer"];
        ArrayList userdetails = new ArrayList();
        Session["CHECKED_ITEMS"] = null;

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["CHECKED_ITEMS"] != null)
                {
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                }

                if (!userdetails.Contains(dr["Customer_Id"]))
                {
                    userdetails.Add(dr["Customer_Id"]);
                }
            }
            foreach (GridViewRow GR in GvCustomer.Rows)
            {
                ((CheckBox)GR.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails.Count > 0 && userdetails != null)
            {
                Session["CHECKED_ITEMS"] = userdetails;
            }
        }
        else
        {
            lblSelectedRecordCustomer.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtCustomer"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvCustomer, dtUnit1, "", "");
            //AllPageCode();
            ViewState["Select"] = null;
        }
    }
    protected void chkCurrentCustomer_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvCustomer.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in GvCustomer.Rows)
        {

            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = false;
            }
        }
    }


    protected void GvCustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveAllGridChkbox(GvCustomer);
        GvCustomer.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtCustomer"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvCustomer, dt, "", "");
        PopulateAllGridChkbox(GvCustomer);
        //AllPageCode();
    }
    protected void GvCustomer_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objCustomer.GetCustomerAllTrueData(Session["CompId"].ToString(), Session["BrandId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtCustomer"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvCustomer, dt, "", "");
        lblSelectedRecordCustomer.Text = "";
        //AllPageCode();
        GvCustomer.HeaderRow.Focus();
    }
    #endregion
    #region SupplierList
    private void FillSupplierList()
    {
        DataTable dtSupplier = objSupplier.GetSupplierAllTrueData(Session["CompId"].ToString(), Session["BrandId"].ToString());



        Session["dtSupplier"] = dtSupplier;

        if (dtSupplier != null && dtSupplier.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvSupplier, dtSupplier, "", "");
            pnlGoButtton.Visible = true;
            btnGo.Visible = true;
            btnback.Visible = false;
        }
        else
        {
            pnlGoButtton.Visible = false;
            GvSupplier.DataSource = null;
            GvSupplier.DataBind();
            btnGo.Visible = false;
            btnback.Visible = false;
        }
        lblTotalRecordSupplier.Text = Resources.Attendance.Total_Records + " : " + dtSupplier.Rows.Count + "";
        //AllPageCode();

    }

    protected void btnBindSupplier_Click(object sender, EventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        string condition = string.Empty;
        if (ddlOptionSupplier.SelectedIndex != 0)
        {
            if (ddlOptionSupplier.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldNameSupplier.SelectedValue + ",System.String)='" + txtValueSupplier.Text.Trim() + "'";
            }
            else if (ddlOptionSupplier.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNameSupplier.SelectedValue + ",System.String) like '" + txtValueSupplier.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNameSupplier.SelectedValue + ",System.String) Like '%" + txtValueSupplier.Text.Trim() + "%'";
            }


            DataTable dtCust = (DataTable)Session["dtSupplier"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);

            lblTotalRecordSupplier.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvSupplier, view.ToTable(), "", "");
            if (view.ToTable().Rows.Count > 0)
                pnlGoButtton.Visible = true;
            else
                pnlGoButtton.Visible = false;
            lblSelectedRecordSupplier.Text = "";
        }
        txtValueSupplier.Focus();
    }
    protected void btnRefreshSupplier_Click(object sender, EventArgs e)
    {
        FillSupplierList();
        txtValueSupplier.Text = "";
        Session["CHECKED_ITEMS"] = null;
        ddlOptionSupplier.SelectedIndex = 3;
        ddlFieldNameSupplier.SelectedIndex = 1;
        lblSelectedRecordSupplier.Text = "";
        txtValueSupplier.Focus();
        ViewState["Supplier"] = null;
    }
    protected void ImgbtnSelectAllSupplier_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["dtSupplier"];
        ArrayList userdetails = new ArrayList();
        Session["CHECKED_ITEMS"] = null;

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["CHECKED_ITEMS"] != null)
                {
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                }

                if (!userdetails.Contains(dr["Supplier_Id"]))
                {
                    userdetails.Add(dr["Supplier_Id"]);
                }
            }
            foreach (GridViewRow GR in GvSupplier.Rows)
            {
                ((CheckBox)GR.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails.Count > 0 && userdetails != null)
            {
                Session["CHECKED_ITEMS"] = userdetails;
            }
        }
        else
        {
            lblSelectedRecordSupplier.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtSupplier"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvSupplier, dtUnit1, "", "");
            //AllPageCode();
            ViewState["Select"] = null;
        }
    }
    protected void chkCurrentSupplier_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvSupplier.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in GvSupplier.Rows)
        {

            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = false;
            }
        }
    }

    protected void GvSupplier_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveAllGridChkbox(GvSupplier);
        GvSupplier.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtSupplier"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvSupplier, dt, "", "");
        PopulateAllGridChkbox(GvSupplier);
        //AllPageCode();
    }
    protected void GvSupplier_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objSupplier.GetSupplierAllTrueData(Session["CompId"].ToString(), Session["BrandId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtSupplier"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvSupplier, dt, "", "");
        //AllPageCode();
        lblSelectedRecordSupplier.Text = "";
        GvSupplier.HeaderRow.Focus();
    }
    #endregion

    #region ProjectList
    private void FillProjectList()
    {
        DataTable dtProjectMAster = objProjctMaster.GetAllRecordProjectMasteer();



        ViewState["dtProject"] = dtProjectMAster;

        if (dtProjectMAster != null && dtProjectMAster.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvrProjectteam, dtProjectMAster, "", "");
            pnlGoButtton.Visible = true;
            btnGo.Visible = true;
            btnback.Visible = false;
        }
        else
        {
            pnlGoButtton.Visible = false;
            GvrProjectteam.DataSource = null;
            GvrProjectteam.DataBind();
            btnGo.Visible = false;
            btnback.Visible = false;
        }
        lblTotalRecordProject.Text = Resources.Attendance.Total_Records + " : " + dtProjectMAster.Rows.Count + "";
        //AllPageCode();

    }

    protected void btnBindProject_Click(object sender, EventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        string condition = string.Empty;
        if (ddlOptionProject.SelectedIndex != 0)
        {
            if (ddlOptionProject.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldNameProject.SelectedValue + ",System.String)='" + txtValueProject.Text.Trim() + "'";
            }
            else if (ddlOptionProject.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNameProject.SelectedValue + ",System.String) like '" + txtValueProject.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNameProject.SelectedValue + ",System.String) Like '%" + txtValueProject.Text.Trim() + "%'";
            }


            DataTable dtCust = (DataTable)ViewState["dtProject"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);

            lblTotalRecordProject.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvrProjectteam, view.ToTable(), "", "");
            if (view.ToTable().Rows.Count > 0)
                pnlGoButtton.Visible = true;
            else
                pnlGoButtton.Visible = false;
            lblSelectedRecordProject.Text = "";
        }
        txtValueProject.Focus();
    }
    protected void btnRefreshProject_Click(object sender, EventArgs e)
    {
        FillProjectList();
        txtValueProject.Text = "";
        Session["CHECKED_ITEMS"] = null;
        ddlOptionProject.SelectedIndex = 3;
        ddlFieldNameProject.SelectedIndex = 1;
        lblSelectedRecordProject.Text = "";
        txtValueProject.Focus();

    }
    protected void ImgbtnSelectAllProject_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)ViewState["dtProject"];
        ArrayList userdetails = new ArrayList();
        Session["CHECKED_ITEMS"] = null;

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (Session["CHECKED_ITEMS"] != null)
                {
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                }

                if (!userdetails.Contains(dr["Project_Id"]))
                {
                    userdetails.Add(dr["Project_Id"]);
                }
            }
            foreach (GridViewRow GR in GvrProjectteam.Rows)
            {
                ((CheckBox)GR.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails.Count > 0 && userdetails != null)
            {
                Session["CHECKED_ITEMS"] = userdetails;
            }
        }
        else
        {
            lblSelectedRecordProject.Text = "";
            DataTable dtUnit1 = (DataTable)ViewState["dtProject"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GvrProjectteam, dtUnit1, "", "");
            //AllPageCode();
            ViewState["Select"] = null;
        }
    }
    protected void chkCurrentProject_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GvrProjectteam.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in GvrProjectteam.Rows)
        {

            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = false;
            }
        }
    }

    protected void GvrProjectteam_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveAllGridChkbox(GvrProjectteam);
        GvrProjectteam.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)ViewState["dtProject"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvrProjectteam, dt, "", "");
        PopulateAllGridChkbox(GvrProjectteam);
        //AllPageCode();
    }
    protected void GvrProjectteam_Sorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objProjctMaster.GetAllRecordProjectMasteer();
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        ViewState["dtProject"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvrProjectteam, dt, "", "");
        //AllPageCode();
        lblSelectedRecordProject.Text = "";
        GvrProjectteam.HeaderRow.Focus();
    }
    public string Formatdate(object Date)
    {
        string newdate = "";
        string set_date = "";
        Date = Convert.ToDateTime(Date).ToString(objSys.GetSysParameterByParamName("Date_Format").Rows[0]["Param_Value"].ToString());
        newdate = Convert.ToString(Date);
        if (newdate == "01-01-1990" || newdate.ToString() == "01-Jan-1990")
        {
            set_date = "";
        }
        else
        {
            set_date = Convert.ToString(Date);
        }
        return set_date;
    }
    #endregion

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDirectoryName(string prefixText, int count, string contextKey)
    {
        EmployeeMaster objEmp = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataAccessClass ObjDa = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_ProductMaster ObjProductMaster = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        Set_CustomerMaster objCustomer = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString());
        Set_Suppliers objSupplier = new Set_Suppliers(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtDirectory = new DataTable();
        string[] txt = new string[dtDirectory.Rows.Count];
        Arc_Directory_Master objdir = new Arc_Directory_Master(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtFilter = new DataTable();
        dtDirectory = objdir.getDirectoryMaster(HttpContext.Current.Session["CompId"].ToString(), "0");

        if (HttpContext.Current.Session["FileGeneratedType"].ToString() == "0")
        {
            if (HttpContext.Current.Session["LoginUserDirectory"] != null)
            {
                dtDirectory = (DataTable)HttpContext.Current.Session["LoginUserDirectory"];
                dtFilter = dtDirectory.Copy();


                dtDirectory = new DataView(dtDirectory, "Directory_Name like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
                //if (dtDirectory.Rows.Count == 0)
                //{
                //    dtDirectory = dtFilter;
                //}

                txt = new string[dtDirectory.Rows.Count];
                for (int i = 0; i < dtDirectory.Rows.Count; i++)
                {
                    txt[i] = dtDirectory.Rows[i]["Directory_Name"].ToString();
                }
            }


        }
        else
        {



            if (HttpContext.Current.Session["Sys_DirectoryType"].ToString() == "0")
            {

            }
            else
            {



                if (HttpContext.Current.Session["Sys_DirectoryType"].ToString() == "1")
                {


                    dtDirectory = Common.GetEmployee(prefixText, HttpContext.Current.Session["CompId"].ToString(),HttpContext.Current.Session["BrandId"].ToString(),HttpContext.Current.Session["LocId"].ToString(),HttpContext.Current.Session["DBConnection"].ToString());

                    txt = new string[dtDirectory.Rows.Count];
                    for (int i = 0; i < dtDirectory.Rows.Count; i++)
                    {
                        txt[i] = "" + dtDirectory.Rows[i][1].ToString() + "/(" + dtDirectory.Rows[i][2].ToString() + ")/" + dtDirectory.Rows[i][0].ToString() + "";
                    }


                }
                if (HttpContext.Current.Session["Sys_DirectoryType"].ToString() == "2")
                {

                    dtDirectory = ObjProductMaster.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());



                    dtFilter = dtDirectory.Copy();
                    dtDirectory = new DataView(dtDirectory, "ProductCode like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();


                    txt = new string[dtDirectory.Rows.Count];
                    for (int i = 0; i < dtDirectory.Rows.Count; i++)
                    {
                        txt[i] = dtDirectory.Rows[i]["ProductCode"].ToString();
                    }
                }
                if (HttpContext.Current.Session["Sys_DirectoryType"].ToString() == "3")
                {

                    dtDirectory = ObjDa.return_DataTable("select Name,Code from ems_contactmaster where isactive='True' and Name like '%" + prefixText.ToString() + "%'");


                    dtFilter = dtDirectory.Copy();
                    //dtDirectory = new DataView(dtDirectory, "Name like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();

                    txt = new string[dtDirectory.Rows.Count];
                    for (int i = 0; i < dtDirectory.Rows.Count; i++)
                    {
                        txt[i] = dtDirectory.Rows[i]["Name"].ToString() + "/" + dtDirectory.Rows[i]["Code"].ToString();
                    }
                }
                if (HttpContext.Current.Session["Sys_DirectoryType"].ToString() == "4")
                {

                    dtDirectory = objCustomer.GetCustomerAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());


                    dtFilter = dtDirectory.Copy();
                    dtDirectory = new DataView(dtDirectory, "Name like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();

                    txt = new string[dtDirectory.Rows.Count];
                    for (int i = 0; i < dtDirectory.Rows.Count; i++)
                    {
                        txt[i] = dtDirectory.Rows[i]["Name"].ToString() + "/" + dtDirectory.Rows[i]["Customer_Code"].ToString();
                    }
                }
                if (HttpContext.Current.Session["Sys_DirectoryType"].ToString() == "5")
                {

                    dtDirectory = objSupplier.GetSupplierAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());

                    dtFilter = dtDirectory.Copy();
                    dtDirectory = new DataView(dtDirectory, "Name like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();

                    txt = new string[dtDirectory.Rows.Count];
                    for (int i = 0; i < dtDirectory.Rows.Count; i++)
                    {
                        txt[i] = dtDirectory.Rows[i]["Name"].ToString() + "/" + dtDirectory.Rows[i]["Supplier_Code"].ToString();
                    }
                }
            }
            if (HttpContext.Current.Session["Sys_DirectoryType"].ToString() == "6")
            {
                Prj_ProjectMaster objProjctMaster = new Prj_ProjectMaster(HttpContext.Current.Session["DBConnection"].ToString());

                dtDirectory = objProjctMaster.GetAllRecordProjectMasteer();


                dtFilter = dtDirectory.Copy();
                dtDirectory = new DataView(dtDirectory, "Project_Name like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();

                txt = new string[dtDirectory.Rows.Count];
                for (int i = 0; i < dtDirectory.Rows.Count; i++)
                {
                    txt[i] = dtDirectory.Rows[i]["Project_Name"].ToString() + "/" + dtDirectory.Rows[i]["Field7"].ToString();
                }
            }




        }




        return txt;



    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDocName(string prefixText, int count, string contextKey)
    {
        Document_Master objdoc = new Document_Master(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objdoc.getdocumentmaster(HttpContext.Current.Session["CompId"].ToString(), "0");
        DataTable dtFilter = new DataTable();
        dtFilter = dt.Copy();
        dt = new DataView(dt, "Document_Name like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();


        string[] txt = new string[0];

        if (dt != null)
        {
            txt = new string[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["Document_Name"].ToString();
            }
        }

        return txt;
    }
    public string GetDate(string Value)
    {
        string Newdate = string.Empty;
        if (Value != "")
        {
            Newdate = Convert.ToDateTime(Value).ToString(objSys.SetDateFormat());
        }

        return Newdate;
    }

    protected void Unnamed_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        DataTable dt = new DataTable();

        dt = ObjFile.Get_FileTransaction_By_TransactionId(Session["CompId"].ToString(), editid.Value);
        download(dt);
        Reset();
    }

    protected void ibtnDelete_Command(object sender, CommandEventArgs e)
    {

        int b = 0;
        b = ObjFile.Delete_in_FileTransaction(Session["CompId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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


    protected void btnTreeView_Click(object sender, EventArgs e)
    {
        string strCmd = string.Format("window.open('../ArcaWing/FileExplorer.aspx','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }
}