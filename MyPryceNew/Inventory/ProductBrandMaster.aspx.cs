using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using ClosedXML.Excel;
using PegasusDataAccess;

public partial class Inventory_ProductBrandMaster : BasePage
{
    #region defined Class Object
    Common cmn = null;
    Inv_ProductBrandMaster objProB = null;
    SystemParameter ObjSysPeram = null;
    PageControlCommon objPageCmn = null;
    DataAccessClass objDa = null;
    Set_ApplicationParameter objAppParam = null;
    #endregion

    protected string FuLogo_UploadFolderPath = "~/CompanyResource/Contact/";

    protected void Page_Load(object sender, EventArgs e)
    {


        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objProB = new Inv_ProductBrandMaster(Session["DBConnection"].ToString());
        ObjSysPeram = new SystemParameter(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objDa=new DataAccessClass(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Inventory/ProductBrandMaster.aspx",HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
         
            AllPageCode(clsPagePermission);
            ddlOption.SelectedIndex = 2;

            // FillGridBin();
            FillGrid();
            RootFolder();
            try
            {
                string ParmValue = objAppParam.GetApplicationParameterValueByParamName("ImageFileUploadSize", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                ASPxFileManager1.SettingsUpload.ValidationSettings.MaxFileSize = int.Parse(ParmValue) * 1000;
                Page.Form.Attributes.Add("enctype", "multipart/form-data");
            }
            catch(Exception ex)
            {

            }
        }

        //AllPageCode();
    }

    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnCSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        btnExportExcel.Visible = clsPagePermission.bDownload;
    }
    #endregion


    #region System defined Function
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();

        DataTable dt = objProB.GetProductBrandByPBrandId(Session["CompId"].ToString(), editid.Value);

        Lbl_Tab_New.Text = Resources.Attendance.Edit;

        txtBrandName.Text = dt.Rows[0]["Brand_Name"].ToString();
        txtLBrandName.Text = dt.Rows[0]["Brand_Name_L"].ToString();
        txtDescription.Text = dt.Rows[0]["Description"].ToString();
        try
        {
            string RegistrationCode = Common.Decrypt(objDa.get_SingleValue("Select registration_code from Application_Lic_Main"));
            imgProduct.ImageUrl = "~/CompanyResource/" + RegistrationCode + "/" + HttpContext.Current.Session["CompId"].ToString() + "/" + HttpContext.Current.Session["BrandId"].ToString() + "/ProductBrandLogo/" + dt.Rows[0]["Field1"].ToString();
            Session["FilePath"] = dt.Rows[0]["Field1"].ToString();
        }
        catch
        {
            imgProduct.ImageUrl = "~/CompanyResource/"+ HttpContext.Current.Session["CompId"].ToString() + "/" + HttpContext.Current.Session["BrandId"].ToString() + "/ProductBrandLogo/" + dt.Rows[0]["Field1"].ToString();
            Session["FilePath"] = dt.Rows[0]["Field1"].ToString();

        }
        txtUnsuscribeUrl.Content = dt.Rows[0]["Field2"].ToString();
        EditorHeader.Content = dt.Rows[0]["Field3"].ToString();
        EditorFooter.Content = dt.Rows[0]["Field4"].ToString();

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);

    }
    protected void gvProductBrand_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvProductBrand.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)ViewState["dtFilter"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProductBrand, dt, "", "");

        //AllPageCode();
        gvProductBrand.BottomPagerRow.Focus();
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
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
            DataTable dtBrand = (DataTable)Session["dtBrand"];
            DataView view = new DataView(dtBrand, condition, "", DataViewRowState.CurrentRows);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvProductBrand, view.ToTable(), "", "");


            ViewState["dtFilter"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
            //AllPageCode();
        }
        txtValue.Focus();
    }
    protected void gvProductBrand_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)ViewState["dtFilter"];
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
        ViewState["dtFilter"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProductBrand, dt, "", "");


        //AllPageCode();
        gvProductBrand.HeaderRow.Focus();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        int b = 0;
        b = objProB.DeleteProductBrandMaster(Session["CompId"].ToString(), editid.Value, "false", Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record  Not Deleted");
        }
        //FillGridBin();
        FillGrid();
        Reset();
        try
        {
            int i = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
            ((LinkButton)gvProductBrand.Rows[i].FindControl("IbtnDelete")).Focus();
        }
        catch
        {
            txtValue.Focus();
        }


    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValue.Focus();
    }
    protected void BtnCCancel_Click(object sender, EventArgs e)
    {
        Reset();
        txtValue.Focus();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();
        txtBrandName.Focus();
    }
    protected void btnCSave_Click(object sender, EventArgs e)
    {
        //Condition added to check brand name entered before save
        if (txtBrandName.Text == "" || txtBrandName.Text == null)
        {
            DisplayMessage("Enter Brand Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtBrandName);
            return;
        }
        int b = 0;
        if (editid.Value != "")
        {
            //Code to check whether the new name after edit does not exists
            DataTable dtPBrand = objProB.GetProductBrandByBrandName(Session["CompId"].ToString(), txtBrandName.Text);
            if (dtPBrand.Rows.Count > 0)
            {
                if (dtPBrand.Rows[0]["PBrandId"].ToString() != editid.Value)
                {
                    txtBrandName.Text = "";
                    DisplayMessage("Brand Name Already Exists");
                    txtBrandName.Focus();
                    return;
                }

            }
            string fn = string.Empty;
            if (Session["FilePath"] != null)
            {
                fn = Session["FilePath"].ToString();
            }
            b = objProB.UpdateProductBrandMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), editid.Value, txtBrandName.Text, txtLBrandName.Text, txtDescription.Text, fn, txtUnsuscribeUrl.Content, EditorHeader.Content, EditorFooter.Content, "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString());

            editid.Value = "";
            if (b != 0)
            {
                DisplayMessage("Record Updated", "green");
                Reset();
                FillGrid();

                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {
                DisplayMessage("Record  Not Updated");
            }
        }
        else
        {
            DataTable dtPro = objProB.GetProductBrandByBrandName(Session["CompId"].ToString(), txtBrandName.Text);
            if (dtPro.Rows.Count > 0)
            {
                txtBrandName.Text = "";
                DisplayMessage("Product Brand Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtBrandName);
                return;
            }
            string fname = string.Empty;
            if (Session["FilePath"] != null)
            {
                fname = Session["FilePath"].ToString();
            }
            b = objProB.InsertProductBrandMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), txtBrandName.Text, txtLBrandName.Text, txtDescription.Text, fname, txtUnsuscribeUrl.Content, EditorHeader.Content, EditorFooter.Content, "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            if (b != 0)
            {
                DisplayMessage("Record Saved","green");

                Reset();
                FillGrid();
                txtBrandName.Focus();
            }
            else
            {
                DisplayMessage("Record  Not Saved");
            }
        }
    }

    protected void txtBrandName_TextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt = objProB.GetProductBrandByBrandName(Session["CompId"].ToString(), txtBrandName.Text);
            if (dt.Rows.Count > 0)
            {
                txtBrandName.Text = "";
                DisplayMessage("Brand Name Already Exists");
                txtBrandName.Focus();
                return;
            }
            DataTable dt1 = objProB.GetProductBrandFalseAllData(Session["CompId"].ToString());
            dt1 = new DataView(dt1, "Brand_Name='" + txtBrandName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtBrandName.Text = "";
                DisplayMessage("Brand Name Already Exists - Go to Bin Tab");
                txtBrandName.Focus();
                return;
            }
        }
        else
        {
            DataTable dtTemp = objProB.GetProductBrandByPBrandId(Session["CompId"].ToString(), editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Brand_Name"].ToString() != txtBrandName.Text)
                {
                    DataTable dt = objProB.GetProductBrandByBrandName(Session["CompId"].ToString(), txtBrandName.Text);
                    if (dt.Rows.Count > 0)
                    {
                        txtBrandName.Text = "";
                        DisplayMessage("Brand Name Already Exists");
                        txtBrandName.Focus();
                        return;
                    }
                    DataTable dt1 = objProB.GetProductBrandFalseAllData(Session["CompId"].ToString());
                    dt1 = new DataView(dt1, "Brand_Name='" + txtBrandName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtBrandName.Text = "";
                        DisplayMessage("Brand Name Already Exists - Go to Bin Tab");
                        txtBrandName.Focus();
                        return;
                    }
                }
            }
        }
        txtLBrandName.Focus();
    }

    protected void btnloadimg_Click(object sender, EventArgs e)
    {
        try
        {

            if (FileUploadLogo.HasFile)
            {
                string path = Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString());
                if (!Directory.Exists(path))
                    CheckDirectory(path);
                string path1 = Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/" + Session["BrandId"].ToString());
                if (!Directory.Exists(path1))
                    CheckDirectory(path1);
                string path2 = Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/ProductBrandLogo");
                if (!Directory.Exists(path2))
                    CheckDirectory(path2);

                string path3;
                path3 = Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/ProductBrandLogo/");
                FileUploadLogo.SaveAs(path3 + txtBrandName.Text + System.IO.Path.GetExtension(FileUploadLogo.PostedFile.FileName));
                string path4;
                path4 = txtBrandName.Text + System.IO.Path.GetExtension(FileUploadLogo.PostedFile.FileName);
                imgProduct.ImageUrl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/ProductBrandLogo/" + path4;

                ViewState["FilePath"] = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/ProductBrandLogo/" + path4;
            }
            else
            {
                DisplayMessage("First Upload File");
                return;
            }//Has File end
        }
        catch
        {
            Exception ex = new Exception();
            DisplayMessage(ex.Message);
        }
    }//end load function
    public void CheckDirectory(string path)
    {
        if (path != "")
        {
            Directory.CreateDirectory(path);
        }
    }
    //function to show uploaded image

    public bool ValidateFileExtension()
    {
        string[] validFileTypes = { "bmp", "gif", "png", "jpg", "jpeg", "tiff", "png", "ico", "tif" };


        string exten = System.IO.Path.GetExtension(FileUploadLogo.FileName);
        bool isValidFile = false;
        for (int i = 0; i < validFileTypes.Length; i++)
        {
            if (exten.ToLower() == "." + validFileTypes[i])
            {
                isValidFile = true;
                break;
            }
        }
        return isValidFile;
    }



    #region Bin Section
    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillGridBin();
        txtValueBin.Focus();
    }
    protected void gvProductBrandMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvProductBrandMasterBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtInactive"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProductBrandMasterBin, dt, "", "");


        //AllPageCode();
        string temp = string.Empty;

        for (int i = 0; i < gvProductBrandMasterBin.Rows.Count; i++)
        {
            Label lblconid = (Label)gvProductBrandMasterBin.Rows[i].FindControl("lblProductBrandId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvProductBrandMasterBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
        gvProductBrandMasterBin.BottomPagerRow.Focus();
    }
    protected void gvProductBrandMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtInactive"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtInactive"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProductBrandMasterBin, dt, "", "");
        //AllPageCode();
        lblSelectedRecord.Text = "";
        gvProductBrandMasterBin.HeaderRow.Focus();
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objProB.GetProductBrandFalseAllData(Session["CompId"].ToString());
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvProductBrandMasterBin, dt, "", "");


        Session["dtPBrandBin"] = dt;
        Session["dtInactive"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        lblSelectedRecord.Text = "";
        if (dt.Rows.Count == 0)
        {
            //ImgbtnSelectAll.Visible = false;
            //imgBtnRestore.Visible = false;
        }
        else
        {
            //AllPageCode();
        }
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
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

            DataTable dtCust = (DataTable)Session["dtPBrandBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtInactive"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvProductBrandMasterBin, view.ToTable(), "", "");


            lblSelectedRecord.Text = "";
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
        txtValueBin.Focus();
    }

    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvProductBrandMasterBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < gvProductBrandMasterBin.Rows.Count; i++)
        {
            ((CheckBox)gvProductBrandMasterBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvProductBrandMasterBin.Rows[i].FindControl("lblProductBrandId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvProductBrandMasterBin.Rows[i].FindControl("lblProductBrandId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvProductBrandMasterBin.Rows[i].FindControl("lblProductBrandId"))).Text.Trim().ToString())
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
        chkSelAll.Focus();
    }
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvProductBrandMasterBin.Rows[index].FindControl("lblProductBrandId");
        if (((CheckBox)gvProductBrandMasterBin.Rows[index].FindControl("chkSelect")).Checked)
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
        ((CheckBox)gvProductBrandMasterBin.Rows[index].FindControl("chkSelect")).Focus();
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 1;
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        DataTable dtPbrand = (DataTable)Session["dtInactive"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPbrand.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["PBrandId"]))
                {
                    lblSelectedRecord.Text += dr["PBrandId"] + ",";
                }
            }
            for (int i = 0; i < gvProductBrandMasterBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)gvProductBrandMasterBin.Rows[i].FindControl("lblProductBrandId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvProductBrandMasterBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtInactive"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvProductBrandMasterBin, dtUnit1, "", "");
            //AllPageCode();
            ViewState["Select"] = null;
        }
        ImgbtnSelectAll.Focus();
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = objProB.DeleteProductBrandMaster(Session["CompId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
            btnRefreshBin_Click(null, null);
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in gvProductBrandMasterBin.Rows)
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

        txtValueBin.Focus();

    }
    #endregion

    #endregion

    #region User defined Function
    private void FillGrid()
    {
        DataTable dtBrand = objProB.GetProductBrandTrueAllData(Session["CompId"].ToString());
        dtBrand = new DataView(dtBrand, "", "PBrandId desc", DataViewRowState.CurrentRows).ToTable();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtBrand.Rows.Count + "";
        Session["dtBrand"] = dtBrand;
        ViewState["dtFilter"] = dtBrand;
        if (dtBrand != null && dtBrand.Rows.Count > 0)
        {//this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvProductBrand, dtBrand, "", "");


        }
        else
        {
            gvProductBrand.DataSource = null;
            gvProductBrand.DataBind();
        }
        //AllPageCode();
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
        return ArebicMessage;
    }
    public void Reset()
    {
        txtBrandName.Text = "";
        txtLBrandName.Text = "";
        txtDescription.Text = "";
        editid.Value = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        txtValueBin.Text = "";
        lblSelectedRecord.Text = "";
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValue);
        imgProduct.ImageUrl = "";
        txtUnsuscribeUrl.Content = null;
        EditorHeader.Content = null;
        EditorFooter.Content = null;
    }
    #endregion

    #region Auto Complete Function
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Inv_ProductBrandMaster ts = new Inv_ProductBrandMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ts.GetDistinctBrandName(HttpContext.Current.Session["CompId"].ToString(), prefixText);

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Brand_Name"].ToString();
        }
        return str;
    }
    #endregion
    protected void FuLogo_FileUploadComplete(object sender, EventArgs e)
    {
        if (FileUploadLogo.HasFile)
        {
            string ext = FileUploadLogo.FileName.Substring(FileUploadLogo.FileName.Split('.')[0].Length);
            if ((ext != ".png") && (ext != ".jpg") && (ext != ".jpge"))
            {
                DisplayMessage("Invalid File Type, Select Only .png, .jpg, .jpge extension file");
                return;
            }
            else
            {
                string path = Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString());
                if (!Directory.Exists(path))
                    CheckDirectory(path);
                string path1 = Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/" + Session["BrandId"].ToString());
                if (!Directory.Exists(path1))
                    CheckDirectory(path1);
                string path2 = Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/ProductBrandLogo");
                if (!Directory.Exists(path2))
                    CheckDirectory(path2);

                string path3;
                path3 = Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/ProductBrandLogo/");
                FileUploadLogo.SaveAs(path3 + txtBrandName.Text + System.IO.Path.GetExtension(FileUploadLogo.PostedFile.FileName));
            }
        }
    }

    #region Filetobytearray
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
    #endregion
    #region Upload
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string ASPxFileManager1_SelectedFileOpened(string fullname, string name)
    {
        try
        {
            DataAccessClass objda = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            string Date = DateTime.Now.ToString("yyyyMMddHHss");
            string fileExtension = Path.GetExtension(name);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(name) + Date;
            name = fileNameWithoutExtension + fileExtension;
            CompanyMaster objComp = new CompanyMaster(HttpContext.Current.Session["DBConnection"].ToString());
            string compid = (int.Parse(objComp.GetMaxCompanyId())).ToString();
           
            try
            {
                string RegistrationCode = Common.Decrypt(objda.get_SingleValue("Select registration_code from Application_Lic_Main"));
                fullname = fullname.Replace("Product_", "Product//Product_");
                FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(fullname), FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                long numBytes = new FileInfo(HttpContext.Current.Server.MapPath(fullname)).Length;
                Byte[] buffer = br.ReadBytes((int)numBytes);
                string fullPath ="~/CompanyResource/"+RegistrationCode+"/"+HttpContext.Current.Session["CompId"].ToString()+"/"+HttpContext.Current.Session["BrandId"].ToString()+"/ProductBrandLogo";
                string MapfullPath = HttpContext.Current.Server.MapPath(fullPath);
                if (Directory.Exists(MapfullPath))
                {
                    if (RegistrationCode != "" && RegistrationCode != null)
                    {
                        File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);

                    }
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(MapfullPath);
                        File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);
                    }
                    catch (Exception ex)
                    {

                    }
                }

            }
            catch (Exception ex)
            {
                FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(fullname), FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                long numBytes = new FileInfo(HttpContext.Current.Server.MapPath(fullname)).Length;
                Byte[] buffer = br.ReadBytes((int)numBytes);
                string fullPath = "~/CompanyResource/"+ HttpContext.Current.Session["CompId"].ToString() + "/" + HttpContext.Current.Session["BrandId"].ToString() + "/ProductBrandLogo";
                string MapfullPath = HttpContext.Current.Server.MapPath(fullPath);
                if (Directory.Exists(MapfullPath))
                {
                   File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(MapfullPath);
                        File.WriteAllBytes(HttpContext.Current.Server.MapPath(fullPath + "/" + name), buffer);
                    }
                    catch (Exception exp)
                    {

                    }
                }
            }
            HttpContext.Current.Session["FilePath"] = name;
            return "true";
        }
        catch (Exception err)
        {
            return "false";
        }
    }
    #region Filemanager
    public void RootFolder()
    {
        string User = HttpContext.Current.Session["UserId"].ToString();

        if (User == "superadmin")
        {
            ASPxFileManager1.Settings.RootFolder = "~\\Product";
            ASPxFileManager1.Settings.InitialFolder = "~\\Product";
            ASPxFileManager1.Settings.ThumbnailFolder = "~\\Product\\Thumbnail\\";
        }
        else
        {
            try
            {
                string RegistrationCode = Common.Decrypt(objDa.get_SingleValue("Select registration_code from Application_Lic_Main"));
                string folderPath = "~\\Product\\Product_" + RegistrationCode + "";
                string fullPath = Server.MapPath(folderPath);
                if (Directory.Exists(fullPath))
                {
                    //Console.WriteLine("The folder already exists.");
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(fullPath);
                        //Console.WriteLine("The folder has been created.");
                    }
                    catch (Exception ex)
                    {

                    }
                }
                if (Directory.Exists(folderPath + "\\Thumbnail"))
                {
                    //Console.WriteLine("The folder already exists.");
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(folderPath + "\\Thumbnail");
                        //Console.WriteLine("The folder has been created.");
                    }
                    catch (Exception ex)
                    {

                    }
                }
                ASPxFileManager1.Settings.RootFolder = folderPath;
                ASPxFileManager1.Settings.InitialFolder = folderPath;
                ASPxFileManager1.Settings.ThumbnailFolder = folderPath + "\\Thumbnail";
            }
            catch(Exception ex)
            {
                ASPxFileManager1.Settings.RootFolder = "~\\Product";
                ASPxFileManager1.Settings.InitialFolder = "~\\Product";
                ASPxFileManager1.Settings.ThumbnailFolder = "~\\Product\\Thumbnail\\";
            }
        }

    }
    #endregion
    #endregion
    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        if (ViewState["dtFilter"] != null)
        {
            DataTable dt = ViewState["dtFilter"] as DataTable;
            if (dt.Rows.Count > 0)
            {

                dt.Columns["PBrandId"].SetOrdinal(0);
                dt.Columns["Brand_Name"].SetOrdinal(1);
                dt.Columns["Brand_Name_L"].SetOrdinal(2);
                dt.Columns["Created_User"].SetOrdinal(3);
                dt.Columns["Modified_User"].SetOrdinal(4);             

                dt.Columns.RemoveAt(5);
                dt.Columns.RemoveAt(5);
                dt.Columns.RemoveAt(5);
                dt.Columns.RemoveAt(5);
                dt.Columns.RemoveAt(5);
                dt.Columns.RemoveAt(5);
                dt.Columns.RemoveAt(5);
                dt.Columns.RemoveAt(5);
                dt.Columns.RemoveAt(5);
                dt.Columns.RemoveAt(5);
                dt.Columns.RemoveAt(5);
                dt.Columns.RemoveAt(5);
                dt.Columns.RemoveAt(5);
                dt.Columns.RemoveAt(5);
                dt.Columns.RemoveAt(5);

                dt.Columns["PBrandId"].ColumnName = "ProductBrandId";
                dt.Columns["Brand_Name"].ColumnName = "BrandName";
                dt.Columns["Brand_Name_L"].ColumnName = "BrandNameLocal";
                dt.Columns["Created_User"].ColumnName = "CreatedBy";
                dt.Columns["Modified_User"].ColumnName = "ModifiedBy";


                ExportTableData(dt);
            }
        }
    }
    public void ExportTableData(DataTable dtdata)
    {
        string strFname = "ProductBrandMaster";
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
}
