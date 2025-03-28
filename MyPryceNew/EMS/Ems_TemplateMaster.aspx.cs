using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.IO;
using System.Drawing;
using PegasusDataAccess;
using System.Text;
using System.Configuration;

public partial class EMS_Ems_TemplateMaster : System.Web.UI.Page
{
    protected string FuLogoUploadFolderPath = "~/CompanyResource/Template/";
    Ems_TemplateMaster objTemplate = null;
    EMS_TemplateMaster_Reference objTemMasterRef = null;
    SystemParameter ObjSysParam = null;
    Common cmn = null;
    Inv_ModelMaster ObjInvModelMaster = null;
    Ems_GroupMaster objGroup = null;
    Ems_Contact_Group objCG = null;
    Inv_ProductCategoryMaster ObjProductCateMaster = null;
    Inv_ProductBrandMaster objProB = null;
    Arc_Directory_Master objDir = null;
    Arc_FileTransaction ObjFile = null;
    Ems_MailMarketing objMailMarket = null;
    PageControlCommon objPageCmn = null;
    Inv_ProductMaster ObjProductMaster = null;
    string StrCompId = string.Empty;
    string strBrandId = string.Empty;
    string StrUserId = string.Empty;
    public void ConvertHtmlToImage()
    {
        string text = Editor1.Content;
        Bitmap bitmap = new Bitmap(1, 1);
        Font font = new Font("Arial", 25, FontStyle.Regular, GraphicsUnit.Pixel);
        Graphics graphics = Graphics.FromImage(bitmap);
        int width = (int)graphics.MeasureString(text, font).Width;
        int height = (int)graphics.MeasureString(text, font).Height;
        bitmap = new Bitmap(bitmap, new Size(width, height));
        graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.White);
        graphics.DrawString(text, font, new SolidBrush(Color.FromArgb(255, 0, 0)), 0, 0);
        graphics.Flush();
        string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".jpg";
        bitmap.Save(Server.MapPath("~/Images/") + fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
    }
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }
    private byte[] StreamFile(string p)
    {
        throw new NotImplementedException();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objTemplate = new Ems_TemplateMaster(Session["DBConnection"].ToString());
        objTemMasterRef = new EMS_TemplateMaster_Reference(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjInvModelMaster = new Inv_ModelMaster(Session["DBConnection"].ToString());
        objGroup = new Ems_GroupMaster(Session["DBConnection"].ToString());
        objCG = new Ems_Contact_Group(Session["DBConnection"].ToString());
        ObjProductCateMaster = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());
        objProB = new Inv_ProductBrandMaster(Session["DBConnection"].ToString());
        objMailMarket = new Ems_MailMarketing(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objDir = new Arc_Directory_Master(Session["DBConnection"].ToString());
        ObjFile = new Arc_FileTransaction(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Session["ImageUpload"] = "";
            Session["Template_Upload"] = "";
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../EMS/Ems_TemplateMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            StrCompId = Session["CompId"].ToString();
            strBrandId = Session["BrandId"].ToString();
            StrUserId = Session["UserId"].ToString();
            txtValue.Focus();
            FillGrid();
            FillGridBin();
            BindTree();
            FillProductCategory();
            btnList_Click(null, null);
            Session["AutoProductList"] = null;
            TabContainer1.ActiveTabIndex = 0;
            FillProductBrand();
            Session["ddlTemplatesearchValue"] = "0";
        }
        navTree.Attributes.Add("onclick", "postBackByObject()");

    }


    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }



    public void FillProductBrand()
    {
        DataTable dtProductBrand = objProB.GetProductBrandTrueAllData(Session["CompId"].ToString());
        objPageCmn.FillData((object)ddlProductBrand, dtProductBrand, "Brand_Name", "PBrandId");
    }
    public void FillGrid()
    {
        DataTable dt = objTemplate.GetRecord("0", "", "2");
        if (ddlTemplatesearch.SelectedValue.Trim() == "CG" && txtAdvanceFilterValue.Text != "")
        {
            dt = objTemplate.GetRecord(hdnAdvanceFilterValue.Value, "", "8");
        }
        else if (ddlTemplatesearch.SelectedValue.Trim() == "MO" && txtAdvanceFilterValue.Text != "")
        {
            dt = new DataView(dt, "Field2='" + hdnAdvanceFilterValue.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else if (ddlTemplatesearch.SelectedValue.Trim() == "PB" && txtAdvanceFilterValue.Text != "")
        {
            dt = new DataView(dt, "Field3='" + hdnAdvanceFilterValue.Value + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else if (ddlTemplatesearch.SelectedValue.Trim() == "PC" && txtAdvanceFilterValue.Text != "")
        {
            dt = objTemplate.GetRecord(hdnAdvanceFilterValue.Value, "", "9");
        }
        else if ((ddlTemplatesearch.SelectedValue.Trim() == "PI" || ddlTemplatesearch.SelectedValue.Trim() == "PN") && txtAdvanceFilterValue.Text != "")
        {
            dt = objTemplate.GetRecord(hdnAdvanceFilterValue.Value, "", "10");
        }
        gvTemplateMaster.DataSource = dt;
        gvTemplateMaster.DataBind();
        //  AllPageCode();
        Session["dtFilter_EMS_Temp"] = dt;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
    }

    public void FillGridBin()
    {
        string FileTypeId = "0";
        DataTable dt = new DataTable();
        dt = objTemplate.GetRecord("0", "", "3");
        gvTemplateMasterBin.DataSource = dt;
        gvTemplateMasterBin.DataBind();
        Session["dtbinFilter"] = dt;
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
    protected void gvTemplateMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTemplateMaster.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_EMS_Temp"];
        gvTemplateMaster.DataSource = dt;
        gvTemplateMaster.DataBind();

    }
    protected void gvTemplateMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_EMS_Temp"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_EMS_Temp"] = dt;
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
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvTemplateMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvTemplateMasterBin.Rows.Count; i++)
        {
            ((CheckBox)gvTemplateMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvTemplateMasterBin.Rows[i].FindControl("lblFileId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvTemplateMasterBin.Rows[i].FindControl("lblFileId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvTemplateMasterBin.Rows[i].FindControl("lblFileId"))).Text.Trim().ToString())
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
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        DataTable dt = objTemplate.GetRecord(editid.Value, "", "4");
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["Field1"].ToString() == "")
            {
                hdnimageurl.Value = "NoImage.jpg";
            }
            else
            {
                hdnimageurl.Value = dt.Rows[0]["Field1"].ToString();
            }
            if (dt.Rows[0]["Field2"].ToString() != "" && dt.Rows[0]["Field2"].ToString() != "0")
            {
                txtModelNo.Text = dt.Rows[0]["Model_No"].ToString();
                hdnModelId.Value = dt.Rows[0]["Field2"].ToString();
            }
            else
            {
                txtModelNo.Text = "";
                hdnModelId.Value = "0";
            }
            if (dt.Rows[0]["Field3"].ToString() != "" && dt.Rows[0]["Field3"].ToString() != "0")
            {
                ddlProductBrand.SelectedValue = dt.Rows[0]["Field3"].ToString();
            }
            else
            {
                ddlProductBrand.SelectedIndex = 0;
            }
            Session["FileName"] = dt.Rows[0]["Field4"].ToString();
            imgLogo.ImageUrl = getImageUrl(hdnimageurl.Value);
            txtTemplateName.Text = dt.Rows[0]["Template_Name"].ToString();
            Editor1.Content = dt.Rows[0]["Template_Content"].ToString();
            btnNew_Click(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
        string PCChked = string.Empty;
        string ProductChked = string.Empty;
        BindTree();
        FillProductCategory();
        DataTable dtCG = objTemMasterRef.GetTemplateMaster_ReferenceByTemplateId(editid.Value.ToString());
        for (int i = 0; i < dtCG.Rows.Count; i++)
        {
            //if (dtCG.Rows[i]["Ref_Type"].ToString() == "CG")
            //{
            //    navTree.FindNode(dtCG.Rows[i]["Ref_Id"].ToString()).Checked = true;
            //}
            if (dtCG.Rows[i]["Ref_Type"].ToString() == "PC")
            {
                if (PCChked == "")
                {
                    PCChked = dtCG.Rows[i]["Ref_Id"].ToString();
                }
                else
                {
                    PCChked = PCChked + "," + dtCG.Rows[i]["Ref_Id"].ToString();
                }
                ChkProductCategory.Items.FindByValue(dtCG.Rows[i]["Ref_Id"].ToString()).Selected = true;
            }
            if (dtCG.Rows[i]["Ref_Type"].ToString() == "IM")
            {
                if (ProductChked == "")
                {
                    ProductChked = dtCG.Rows[i]["Ref_Id"].ToString();
                }
                else
                {
                    ProductChked = ProductChked + "," + dtCG.Rows[i]["Ref_Id"].ToString();
                }
            }
        }
        //
        DataTable dtPro = new DataTable();
        DataTable dtCatPro = new DataTable();
        string[] split = PCChked.Split(',');
        string[] splitProduct = ProductChked.Split(',');
        for (int j = 0; j < split.Length; j++)
        {
            if (split[j] != "")
            {
                dtPro = ObjInvModelMaster.GetProductNameByCategoryID(split[j]);
                if (dtPro.Rows.Count > 0)
                {
                    dtCatPro.Merge(dtPro);
                }
            }
        }
        try
        {
            dtCatPro = new DataView(dtCatPro, "", "EProductName Asc", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (dtCatPro.Rows.Count > 0)
        {
            ChkProductChildCategory.Items.Clear();
            ChkProductChildCategory.DataSource = dtCatPro;
            ChkProductChildCategory.DataTextField = "TemplateProductName";
            ChkProductChildCategory.DataValueField = "ProductID";
            ChkProductChildCategory.DataBind();
        }
        //
        for (int j = 0; j < splitProduct.Length; j++)
        {
            if (splitProduct[j] != "")
            {
                ChkProductChildCategory.Items.FindByValue(splitProduct[j]).Selected = true;
            }
        }
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        DataTable dt = objMailMarket.GetRecordHeader("0", "1");
        try
        {
            dt = new DataView(dt, "Template_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (dt.Rows.Count > 0)
        {
            DisplayMessage("Template in use ,you can not delete");
            return;
        }
        b = objTemplate.CRUDRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString(), "", "", "", "", "", "", "", "", "", false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "3");
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
        Editor1.Content = "";
        txtTemplateName.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        Session["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtTemplateName.Focus();
        ChkProductChildCategory.Items.Clear();
        FillProductCategory();
        BindTree();
        Editor1.Content = "";
        hdnimageurl.Value = "";
        imgLogo.ImageUrl = getImageUrl("Test");
        txtproductcategorysearch.Text = "";
        txtproductsearch.Text = "";
        Session["AutoProductList"] = null;
        Session["FileName"] = "";
        TabContainer1.ActiveTabIndex = 0;
        Session["Template_Upload"] = "";
        Session["ImageUpload"] = "";
        txtModelNo.Text = "";
        ddlProductBrand.SelectedIndex = 0;
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        txtValue.Focus();
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlList.Visible = true;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlList.Visible = false;
        PnlNewEdit.Visible = true;
        PnlBin.Visible = false;
        txtTemplateName.Focus();
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
        ddlFieldName.SelectedIndex = 1;
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
            DataTable dtCust = (DataTable)Session["dtFilter_EMS_Temp"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_EMS_Temp"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            gvTemplateMaster.DataSource = view.ToTable();
            gvTemplateMaster.DataBind();
            // AllPageCode();

        }
        txtValue.Focus();
    }
    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

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
    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        FillGrid();
        FillGridBin();
        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 1;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

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
                    b = objTemplate.CRUDRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), userdetail[j].ToString(), "", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "4");
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
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        DataTable dtUnit = (DataTable)Session["dtbinFilter"];
        if (Session["Select"] == null)
        {
            Session["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Trans_Id"]))
                {
                    lblSelectedRecord.Text += dr["Trans_Id"] + ",";
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string strModelId = "0";
        string strProuctBrandId = "0";
        if (txtModelNo.Text.Trim() != "")
        {
            strModelId = hdnModelId.Value.Trim();
        }
        if (ddlProductBrand.SelectedIndex > 0)
        {
            strProuctBrandId = ddlProductBrand.SelectedValue;
        }
        int flag = 0;
        foreach (TreeNode tn in navTree.Nodes)
        {
            if (tn.Checked == true)
            {
                flag = 1;
                break;
            }
        }
        if (flag == 0)
        {
            DisplayMessage("Select Atleast One Contact Group");
            TabContainer1.ActiveTabIndex = 0;
            return;
        }
        if (txtTemplateName.Text == "")
        {
            DisplayMessage("Enter Template Name");
            txtTemplateName.Focus();
            return;
        }
        if (Editor1.Content == "")
        {
            DisplayMessage("Load File to View Uploaded Content");
            TabContainer1.ActiveTabIndex = 2;
            UploadTemplate.Focus();
            return;
        }

        if (Session["FileName"] == null)
        {
            Session["FileName"] = "";
        }
        //try
        //{
        //    ConvertHtmlToImage();
        //}
        //catch
        //{
        //}
        //url = "~/Images/NoImage.png";
        if (hdnimageurl.Value == "")
        {
            hdnimageurl.Value = "NoImage.jpg";
        }
        int b = 0;
        int c = 0;
        if (editid.Value == "")
        {
            b = objTemplate.CRUDRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", txtTemplateName.Text, Editor1.Content, hdnimageurl.Value, strModelId, strProuctBrandId, Session["FileName"].ToString(), "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "1");
            if (b != 0)
            {
                foreach (ListItem li in ChkProductChildCategory.Items)
                {
                    if (li.Selected)
                    {
                        try
                        {
                            //objModelCategory.InsertModelCategory(StrCompId.ToString(), b.ToString(), li.Value.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                            DataTable dtProduct = ObjProductMaster.GetProductCodebyProductId(li.Value.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
                            if (dtProduct.Rows.Count > 0)
                            {
                                CreateDirectory(dtProduct.Rows[0]["ProductCode"].ToString(), dtProduct.Rows[0]["Company_Id"].ToString(), dtProduct.Rows[0]["Brand_Id"].ToString());
                                string DirectoryName = dtProduct.Rows[0]["Company_Id"].ToString() + "/Product/" + dtProduct.Rows[0]["Brand_Id"].ToString() + "/" + dtProduct.Rows[0]["ProductCode"].ToString();
                                DataTable DtDir = objDir.GetDirectoryMaster_By_DirectoryName(dtProduct.Rows[0]["Company_Id"].ToString(), DirectoryName);
                                if (DtDir.Rows.Count > 0)
                                {
                                    //ObjFile.Delete_in_FileTransactionForProductMaster(dtProduct.Rows[0]["Company_Id"].ToString(), DtDir.Rows[0]["Id"].ToString(), "0");
                                    //RecordSavedInArcaWing(DtDir.Rows[0]["Id"].ToString());
                                    c = ObjFile.Insert_In_FileTransaction(dtProduct.Rows[0]["Company_Id"].ToString(), DtDir.Rows[0]["Id"].ToString(), "1", "0", Session["FileName"].ToString(), DateTime.Now.ToString(), (byte[])Session["FileData"], "", DateTime.Now.ToString(), "", "0", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                }
                                objTemMasterRef.InsertTemplateMaster_Reference(b.ToString(), li.Value.ToString(), "IM", c.ToString(), "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString()); ;
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                foreach (TreeNode tn in navTree.Nodes)
                {
                    if (tn.Checked == true)
                    {
                        objTemMasterRef.InsertTemplateMaster_Reference(b.ToString(), tn.Value.ToString(), "CG", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString()); ;
                        foreach (TreeNode ObjNode in tn.ChildNodes)
                        {
                            if (ObjNode.Checked)
                            {
                                objTemMasterRef.InsertTemplateMaster_Reference(b.ToString(), ObjNode.Value.ToString(), "CG", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString()); ;
                                childNodeSave(ObjNode, b.ToString());
                            }
                        }
                    }
                }
                foreach (ListItem li in ChkProductCategory.Items)
                {
                    if (li.Selected)
                    {
                        try
                        {
                            //objModelCategory.InsertModelCategory(StrCompId.ToString(), b.ToString(), li.Value.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                            objTemMasterRef.InsertTemplateMaster_Reference(b.ToString(), li.Value.ToString(), "PC", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString()); ;
                        }
                        catch
                        {
                        }
                    }
                }
                //objTemMasterRef.InsertTemplateMaster_Reference(b,
                DisplayMessage("Record Saved", "green");
                FillGrid();
                Reset();
            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }
        else
        {
            b = objTemplate.CRUDRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value, txtTemplateName.Text, Editor1.Content, hdnimageurl.Value, strModelId, strProuctBrandId, Session["FileName"].ToString(), "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "2");
            if (b != 0)
            {
                objTemMasterRef.DeleteTemplateMaster_Reference(editid.Value.ToString());
                foreach (ListItem li in ChkProductChildCategory.Items)
                {
                    if (li.Selected)
                    {
                        try
                        {
                            objTemMasterRef.InsertTemplateMaster_Reference(editid.Value.ToString(), li.Value.ToString(), "IM", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString()); ;
                            DataTable dtProduct = ObjProductMaster.GetProductCodebyProductId(li.Value.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
                            if (dtProduct.Rows.Count > 0)
                            {
                                CreateDirectory(dtProduct.Rows[0]["ProductCode"].ToString(), dtProduct.Rows[0]["Company_Id"].ToString(), dtProduct.Rows[0]["Brand_Id"].ToString());
                                string DirectoryName = dtProduct.Rows[0]["Company_Id"].ToString() + "/Product/" + dtProduct.Rows[0]["Brand_Id"].ToString() + "/" + dtProduct.Rows[0]["ProductCode"].ToString();
                                DataTable DtDir = objDir.GetDirectoryMaster_By_DirectoryName(dtProduct.Rows[0]["Company_Id"].ToString(), DirectoryName);
                                if (DtDir.Rows.Count > 0)
                                {
                                    //ObjFile.Delete_in_FileTransactionForProductMaster(dtProduct.Rows[0]["Company_Id"].ToString(), DtDir.Rows[0]["Id"].ToString(), "0");
                                    //RecordSavedInArcaWing(DtDir.Rows[0]["Id"].ToString());
                                    c = ObjFile.Insert_In_FileTransaction(dtProduct.Rows[0]["Company_Id"].ToString(), DtDir.Rows[0]["Id"].ToString(), "1", "0", Session["FileName"].ToString(), DateTime.Now.ToString(), (byte[])Session["FileData"], "", DateTime.Now.ToString(), "", "0", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                }
                                //objTemMasterRef.InsertTemplateMaster_Reference(b.ToString(), li.Value.ToString(), "IM", c.ToString(), "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString()); ;
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                foreach (TreeNode tn in navTree.Nodes)
                {
                    if (tn.Checked == true)
                    {
                        objTemMasterRef.InsertTemplateMaster_Reference(editid.Value.ToString(), tn.Value.ToString(), "CG", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString()); ;
                        foreach (TreeNode ObjNode in tn.ChildNodes)
                        {
                            if (ObjNode.Checked)
                            {
                                objTemMasterRef.InsertTemplateMaster_Reference(editid.Value.ToString(), ObjNode.Value.ToString(), "CG", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString()); ;
                                childNodeSave(ObjNode, editid.Value.ToString());
                            }
                        }
                    }
                }
                foreach (ListItem li in ChkProductCategory.Items)
                {
                    if (li.Selected)
                    {
                        try
                        {
                            objTemMasterRef.InsertTemplateMaster_Reference(editid.Value.ToString(), li.Value.ToString(), "PC", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString()); ;
                        }
                        catch
                        {
                        }
                    }
                }
                btnList_Click(null, null);
                DisplayMessage("Record Updated", "green");
                Reset();
                FillGrid();

                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {
                DisplayMessage("Record Not Updated");
            }
        }
    }
    public void childNodeSave(TreeNode ModuleNode, string strTransId)
    {
        foreach (TreeNode ObjNode in ModuleNode.ChildNodes)
        {
            if (ObjNode.Checked)
            {
                objTemMasterRef.InsertTemplateMaster_Reference(strTransId, ObjNode.Value.ToString(), "CG", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString()); ;
                childNodeSave(ObjNode, strTransId);
            }
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();

    }
    protected void btnTemplate_Click(object sender, EventArgs e)
    {
        //if (Session["Template_Upload"].ToString() != "")
        //{
        //    if (txtTemplateName.Text == "")
        //    {
        //        DisplayMessage("Enter Template Name");
        //        txtTemplateName.Focus();
        //        return;
        //    }
        if (UploadTemplate.HasFile == false)
        {
            DisplayMessage("Please Upload an Image");
            UploadTemplate.Focus();
            return;
        }
        //    if (UploadTemplate.PostedFile.ContentType.Trim() != "text/html")
        //    {
        //        DisplayMessage("File extension Should be in Html format");
        //        UploadTemplate.Focus();
        //        return;
        //    }
        //    string FilePath = string.Empty;
        //    string UploadFileName = UploadTemplate.PostedFile.FileName;
        //    Session["FileName"] = UploadFileName.ToString();
        string path = string.Empty;
        path = Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString());
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        //    FilePath = "~/ArcaWing/Template/" + UploadFileName;
        //    UploadTemplate.SaveAs(Server.MapPath(FilePath));
        //    //Stream fs = UploadTemplate.PostedFile.InputStream;
        //    //BinaryReader br = new BinaryReader(fs);
        //    //Byte[] bytes = br.ReadBytes((Int32)fs.Length);
        //    Byte[] bytes = new Byte[0];
        //    try
        //    {
        //        bytes = FileToByteArray(Server.MapPath(FilePath));
        //    }
        //    catch
        //    {
        //    }
        //    Session["FileData"] = bytes;
        //    string Html = File.ReadAllText(Server.MapPath(FilePath));
        //    Editor1.Content = Html.ToString();
        //    UploadTemplate.FileContent.Dispose();
        //}

        ddlProductBrand_SelectedIndexChanged(null, null);
        string data = Editor1.Content;
        string FilePath = string.Empty;
        string UploadFileName = UploadTemplate.PostedFile.FileName;
        Session["FileName"] = UploadFileName.ToString();
        FilePath = Server.MapPath(UploadTemplate.FileName);
        UploadTemplate.SaveAs(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/" + UploadTemplate.FileName));
        int value = data.IndexOf("@I");
        var aStringBuilder = new StringBuilder(data);
        aStringBuilder.Remove(value, 2);
        aStringBuilder.Insert(value, ConfigurationManager.AppSettings["appPath"].ToString() + "/CompanyResource/" + Session["CompId"].ToString() + "/" + UploadTemplate.FileName + "");
        data = aStringBuilder.ToString();
        Editor1.Content = data;
        UploadTemplate.Dispose();
        UploadTemplate = null;
    }
    public byte[] FileToByteArray(string fileName)
    {
        byte[] buff = null;
        FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);
        long numBytes = new FileInfo(fileName).Length;
        buff = br.ReadBytes((int)numBytes);
        return buff;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        FillGrid();
        FillGridBin();
        FillProductCategory();
        BindTree();
        Reset();
        btnList_Click(null, null);

        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    #region Treebinding
    protected void navTree_SelectedNodeChanged1(object sender, EventArgs e)
    {
        try
        {
            if (navTree.SelectedNode.Checked == true)
            {
                SelectChild(navTree.SelectedNode);
            }
            else
            {
                UnSelectChild(navTree.SelectedNode);
            }
        }
        catch (Exception)
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
    private void SelectChild(TreeNode treeNode)
    {
        int i = 0;
        treeNode.Checked = true;
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
    public void CheckParentNodes(TreeNode startNode)
    {
        startNode.Checked = true;
        if (startNode.Parent != null)
        {
            CheckParentNodes(startNode.Parent);
        }
    }
    private void UnSelectChild(TreeNode treeNode)
    {
        int i = 0;
        treeNode.Checked = false;
        bool b = false;
        string value = treeNode.Value;
        while (i < treeNode.ChildNodes.Count)
        {
            treeNode.ChildNodes[i].Checked = false;
            UnSelectChild(treeNode.ChildNodes[i]);
            i++;
        }
        navTree.DataBind();
    }
    void selectContact(TreeNode treenode)
    {
        treenode.Checked = true;
    }
    public void BindTree()
    {
        DataTable dtCG = new DataTable();
        if (editid.Value != "")
        {
            dtCG = objTemMasterRef.GetTemplateMaster_ReferenceByTemplateId(editid.Value.ToString());
        }
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
            if (editid.Value != "")
            {
                if (new DataView(dtCG, "Ref_Type='CG' and Ref_Id=" + datarow["Group_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                {
                    tn.Checked = true;
                }
            }
            FillChild(dtContactGroup, tn.Value, tn);
            navTree.Nodes.Add(tn);
        }
        navTree.DataBind();
        navTree.CollapseAll();
        return;
    }
    private void FillChild(DataTable dtContactGroup, string index, TreeNode tn)//fill up child nodes and respective child nodes of them 
    {
        DataTable dtCG = new DataTable();
        if (editid.Value != "")
        {
            dtCG = objTemMasterRef.GetTemplateMaster_ReferenceByTemplateId(editid.Value.ToString());
        }
        DataTable dt = new DataTable();
        dt = new DataView(objGroup.GetGroupMasterByParentId(index), "", "Group_Name Asc", DataViewRowState.CurrentRows).ToTable();
        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn1 = new TreeNode();
            tn1.Text = dt.Rows[i]["Group_Name"].ToString();
            tn1.Value = dt.Rows[i]["Group_Id"].ToString();
            tn1.SelectAction = TreeNodeSelectAction.Expand;
            if (editid.Value != "")
            {
                if (new DataView(dtCG, "Ref_Type='CG' and Ref_Id=" + dt.Rows[i]["Group_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                {
                    tn1.Checked = true;
                }
            }
            tn.ChildNodes.Add(tn1);
            //foreach (DataRow Dr in dtContactGroup.Rows)
            //{
            //    if (dt.Rows[i]["Group_Id"].ToString() == Dr["Group_Id"].ToString())
            //    {
            //        tn1.Checked = true;
            //    }
            //}
            FillChild(dtContactGroup, (dt.Rows[i]["Group_Id"].ToString()), tn1);
            i++;
        }
        navTree.DataBind();
    }
    #endregion
    #region Product category
    private void FillProductCategory()
    {
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
        }
        if (dtCat.Rows.Count > 0)
        {
            Session["AutoProductList"] = dtCat;
            dtCat = new DataView(dtCat, "", "EProductName Asc", DataViewRowState.CurrentRows).ToTable();
            ChkProductChildCategory.DataSource = dtCat;
            ChkProductChildCategory.DataTextField = "TemplateProductName";
            ChkProductChildCategory.DataValueField = "ProductID";
            ChkProductChildCategory.DataBind();
        }
        else
        {
            Session["AutoProductList"] = null;
        }
    }
    public void CreateDirectory(string ProductCode, string CompId, string BrandId)
    {
        string DirectoryName = CompId.ToString() + "/Product/" + BrandId.ToString() + "/" + ProductCode;
        DataTable DtDir = objDir.getDirectoryMasterByCompanyid(CompId.ToString());
        try
        {
            DtDir = new DataView(DtDir, "Directory_Name='" + DirectoryName + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (DtDir.Rows.Count == 0)
        {
            int b = objDir.InsertDirectorymaster(CompId.ToString(), DirectoryName, "1", "0", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (b != 0)
            {
                string NewDirectory = string.Empty;
                NewDirectory = Server.MapPath(DirectoryName);
                NewDirectory = NewDirectory.Replace("EMS", "ArcaWing");
                int i = CreateDirectoryIfNotExist(NewDirectory);
            }
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
    #endregion
    #region Uploadimage
    protected void btnUploadImage_Click(object sender, EventArgs e)
    {
        if (Session["ImageUpload"].ToString() != "")
        {
            if (FULogoPath.HasFile)
            {
                if (!Directory.Exists(Server.MapPath("~/CompanyResource/Template")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/CompanyResource/Template"));
                }
                string path = Server.MapPath("~/CompanyResource/Template/") + FULogoPath.FileName;
                FULogoPath.SaveAs(path);
                hdnimageurl.Value = FULogoPath.FileName;
                Session["ImageUpload"] = FULogoPath.FileName;
            }
            imgLogo.ImageUrl = getImageUrl(FULogoPath.FileName);
        }
    }
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
    #endregion
    #region searchproductcategoryandproductname
    protected void imgsearchproductcategory_OnClick(object sender, ImageClickEventArgs e)
    {
        DataTable dsCategory = null;
        if (txtproductcategorysearch.Text != "")
        {
            ChkProductChildCategory.Items.Clear();
            txtproductsearch.Text = "";
            dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(StrCompId.ToString());
            if (dsCategory.Rows.Count > 0)
            {
                try
                {
                    dsCategory = new DataView(dsCategory, "Category_Name like '%" + txtproductcategorysearch.Text + "%'", "Category_Name Asc", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
            }
            ChkProductCategory.Items.Clear();
            ChkProductCategory.DataSource = dsCategory;
            ChkProductCategory.DataTextField = "Category_Name";
            ChkProductCategory.DataValueField = "Category_Id";
            ChkProductCategory.DataBind();
            ImgRefreshProductcategory.Focus();
        }
        else
        {
            DisplayMessage("Enter Product Category Name");
            txtproductcategorysearch.Focus();
            return;
        }
    }
    protected void ImgRefreshProductcategory_OnClick(object sender, ImageClickEventArgs e)
    {
        FillProductCategory();
        Session["AutoProductList"] = null;
        ChkProductChildCategory.Items.Clear();
        txtproductcategorysearch.Text = "";
        txtproductcategorysearch.Focus();
        txtproductsearch.Text = "";
    }
    protected void imgsearchproduct_OnClick(object sender, ImageClickEventArgs e)
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
        if (txtproductsearch.Text != "")
        {
            if (dtCat.Rows.Count > 0)
            {
                dtCat = new DataView(dtCat, "TemplateProductName like '%" + txtproductsearch.Text + "%'", "EProductName Asc", DataViewRowState.CurrentRows).ToTable();
                ChkProductChildCategory.DataSource = dtCat;
                ChkProductChildCategory.DataTextField = "TemplateProductName";
                ChkProductChildCategory.DataValueField = "ProductID";
                ChkProductChildCategory.DataBind();
            }
        }
        else
        {
            DisplayMessage("Enter Product Name");
            txtproductsearch.Focus();
            return;
        }
    }
    protected void ImgRefreshProduct_OnClick(object sender, ImageClickEventArgs e)
    {
        txtproductsearch.Text = "";
        txtproductsearch.Focus();
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
        }
        if (dtCat.Rows.Count > 0)
        {
            dtCat = new DataView(dtCat, "", "EProductName Asc", DataViewRowState.CurrentRows).ToTable();
            ChkProductChildCategory.DataSource = dtCat;
            ChkProductChildCategory.DataTextField = "TemplateProductName";
            ChkProductChildCategory.DataValueField = "ProductID";
            ChkProductChildCategory.DataBind();
        }
    }
    protected void txtproductcategorysearch_OnTextChanged(object sender, EventArgs e)
    {
        DataTable dsCategory = null;
        if (txtproductcategorysearch.Text != "")
        {
            ChkProductChildCategory.Items.Clear();
            txtproductsearch.Text = "";
            dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(StrCompId.ToString());
            if (dsCategory.Rows.Count > 0)
            {
                try
                {
                    dsCategory = new DataView(dsCategory, "Category_Name like '%" + txtproductcategorysearch.Text + "%'", "Category_Name Asc", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
            }
            ChkProductCategory.Items.Clear();
            ChkProductCategory.DataSource = dsCategory;
            ChkProductCategory.DataTextField = "Category_Name";
            ChkProductCategory.DataValueField = "Category_Id";
            ChkProductCategory.DataBind();
            ImgRefreshProductcategory.Focus();
        }
        else
        {
            DisplayMessage("Enter Product Category Name");
            txtproductcategorysearch.Focus();
            return;
        }
    }
    protected void txtproductsearch_OnTextChanged(object sender, EventArgs e)
    {
        if (txtproductsearch.Text != "")
        {
            bool b = false;
            DataTable dtProduct = new DataTable();
            DataTable dtCat = new DataTable();
            foreach (ListItem li in ChkProductCategory.Items)
            {
                if (li.Selected == true)
                {
                    dtProduct = ObjInvModelMaster.GetProductNameByCategoryID(li.Value);
                    dtCat.Merge(dtProduct);
                    b = true;
                }
            }
            if (!b)
            {
                DisplayMessage("First Select Product Category");
                txtproductsearch.Text = "";
                return;
            }
            if (txtproductsearch.Text != "")
            {
                if (dtCat.Rows.Count > 0)
                {
                    dtCat = new DataView(dtCat, "TemplateProductName like '%" + txtproductsearch.Text + "%'", "EProductName Asc", DataViewRowState.CurrentRows).ToTable();
                    ChkProductChildCategory.DataSource = dtCat;
                    ChkProductChildCategory.DataTextField = "TemplateProductName";
                    ChkProductChildCategory.DataValueField = "ProductID";
                    ChkProductChildCategory.DataBind();
                }
            }
            else
            {
                DisplayMessage("Enter Product Name");
                txtproductsearch.Focus();
                return;
            }
        }
    }
    #endregion
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCategoryname(string prefixText, int count, string contextKey)
    {
        Inv_ProductCategoryMaster ObjProductCateMaster = new Inv_ProductCategoryMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(ObjProductCateMaster.GetProductCategoryTrueAllData(HttpContext.Current.Session["CompId"].ToString()), "Category_Name like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Category_Name"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        Inv_ProductCategoryMaster ObjProductCateMaster = new Inv_ProductCategoryMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string[] txt = new string[0];
        DataTable dt = new DataTable();
        if (HttpContext.Current.Session["AutoProductList"] != null)
        {
            dt = (DataTable)HttpContext.Current.Session["AutoProductList"];
            if (dt.Rows.Count > 0)
            {
                dt = new DataView(dt, "TemplateProductName like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
                txt = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    txt[i] = dt.Rows[i]["TemplateProductName"].ToString();
                }
            }
        }
        return txt;
    }
    protected void FuLogo_FileUploadComplete(object sender, EventArgs e)
    {
        if (FULogoPath.HasFile)
        {
            if (!Directory.Exists(Server.MapPath("~/CompanyResource/Template")))
            {
                Directory.CreateDirectory(Server.MapPath("~/CompanyResource/Template"));
            }
            string path = Server.MapPath("~/CompanyResource/Template/") + FULogoPath.FileName;
            FULogoPath.SaveAs(path);
            hdnimageurl.Value = FULogoPath.FileName;
            Session["ImageUpload"] = FULogoPath.FileName;
        }
    }
    protected void UploadTemplate_FileUploadComplete(object sender, EventArgs e)
    {
        if (UploadTemplate.HasFile)
        {
            if (!Directory.Exists(Server.MapPath("~/ArcaWing/Template")))
            {
                Directory.CreateDirectory(Server.MapPath("~/ArcaWing/Template"));
            }
            string FilePath = string.Empty;
            string UploadFileName = UploadTemplate.PostedFile.FileName;
            Session["FileName"] = UploadFileName.ToString();
            string path = string.Empty;
            path = Server.MapPath("~/ArcaWing/Template");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            FilePath = "~/ArcaWing/Template/" + UploadFileName;
            UploadTemplate.SaveAs(Server.MapPath(FilePath));
            Session["Template_Upload"] = UploadFileName;
        }
    }
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListFilter(string prefixText, int count, string contextKey)
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
            DataTable dt = new DataView(ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), "True"), "Model_No='" + txtModelNo.Text.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count != 0)
            {
                hdnModelId.Value = dt.Rows[0]["Trans_Id"].ToString();
            }
            else
            {
                txtModelNo.Text = "";
                DisplayMessage("Select Model No");
                txtModelNo.Focus();
            }
        }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        if (ddlTemplatesearch.SelectedIndex <= 0)
        {
            DisplayMessage("Select Filter Option");
            ddlTemplatesearch.Focus();
            return;
        }
        if (txtAdvanceFilterValue.Text.Trim() == "")
        {
            DisplayMessage("Enter Filter Value");
            txtAdvanceFilterValue.Focus();
            return;
        }
        FillGrid();

    }
    protected void btnResetSreach_Click(object sender, EventArgs e)
    {
        ddlTemplatesearch.SelectedIndex = 0;
        Session["ddlTemplatesearchValue"] = "0";
        txtAdvanceFilterValue.Text = "";
        hdnAdvanceFilterValue.Value = "0";
        FillGrid();

    }
    protected void ddlTemplatesearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtAdvanceFilterValue.Text = "";
        Session["ddlTemplatesearchValue"] = ddlTemplatesearch.SelectedValue;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAdvanceFilter(string prefixText, int count, string contextKey)
    {
        DataAccessClass Objda = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        Ems_GroupMaster ObjGroup = new Ems_GroupMaster(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_ModelMaster ObjInvModelMaster = new Inv_ModelMaster(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_ProductBrandMaster objProB = new Inv_ProductBrandMaster(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_ProductCategoryMaster ObjProductCateMaster = new Inv_ProductCategoryMaster(HttpContext.Current.Session["DBConnection"].ToString());
        Inv_ProductMaster ObjproductM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataTable();
        if (HttpContext.Current.Session["ddlTemplatesearchValue"].ToString() == "0")
        {
        }
        else if (HttpContext.Current.Session["ddlTemplatesearchValue"].ToString() == "CG")
        {
            //for business group
            dt = Objda.return_DataTable("select Group_Name,Group_Id from Ems_GroupMaster where Group_Name like '%" + prefixText + "%' and IsActive='True'");
            // dt = ObjGroup.GetGroupMasterTrueAllData();
            // dt = new DataView(dt, "Group_Name like '%" + prefixText.ToString() + "%'", "Group_Name", DataViewRowState.CurrentRows).ToTable().DefaultView.ToTable(true, "Group_Name");
        }
        else if (HttpContext.Current.Session["ddlTemplatesearchValue"].ToString() == "MO")
        {
            //for model
            dt = Objda.return_DataTable("select Model_No,Trans_Id from Inv_ModelMaster where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and IsActive='True' and Model_No like '%" + prefixText + "%'");
            //dt = ObjInvModelMaster.GetModelMaster(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "True");
            // dt = new DataView(dt, "Model_No like '%" + prefixText.ToString() + "%'", "Model_No", DataViewRowState.CurrentRows).ToTable().DefaultView.ToTable(true, "Model_No");
        }
        else if (HttpContext.Current.Session["ddlTemplatesearchValue"].ToString() == "PB")
        {
            //for product brand
            //dt = objProB.GetProductBrandTrueAllData(HttpContext.Current.Session["CompId"].ToString());
            //dt = new DataView(dt, "Brand_Name like '%" + prefixText.ToString() + "%'", "Brand_Name", DataViewRowState.CurrentRows).ToTable().DefaultView.ToTable(true, "Brand_Name");
            dt = Objda.return_DataTable("select Brand_Name,PBrandId from Inv_ProductBrandMaster where IsActive='True' and Brand_Name like '%" + prefixText + "%'");
        }
        else if (HttpContext.Current.Session["ddlTemplatesearchValue"].ToString() == "PC")
        {
            //for product category
            //dt = ObjProductCateMaster.GetProductCategoryTrueAllData(HttpContext.Current.Session["CompId"].ToString());
            // dt = new DataView(dt, "Category_Name like '%" + prefixText.ToString() + "%'", "Category_Name", DataViewRowState.CurrentRows).ToTable().DefaultView.ToTable(true, "Category_Name");
            dt = Objda.return_DataTable("select Category_Name,Category_Id from Inv_Product_CategoryMaster where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and IsActive='True' and Category_Name like '%" + prefixText + "%'");
        }
        else if (HttpContext.Current.Session["ddlTemplatesearchValue"].ToString() == "PI")
        {
            //for product
            //dt = ObjproductM.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
            //dt = new DataView(dt, "ProductCode like '%" + prefixText.ToString() + "%'", "ProductCode", DataViewRowState.CurrentRows).ToTable().DefaultView.ToTable(true, "ProductCode");
            dt = Objda.return_DataTable("select ProductCode,ProductId from Inv_ProductMaster where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and IsActive='True' and ProductCode like '%" + prefixText + "%'");
        }
        else if (HttpContext.Current.Session["ddlTemplatesearchValue"].ToString() == "PN")
        {
            //for product
            //dt = ObjproductM.GetProductMasterTrueAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
            //dt = new DataView(dt, "EProductName like '%" + prefixText.ToString() + "%'", "EProductName", DataViewRowState.CurrentRows).ToTable().DefaultView.ToTable(true, "EProductName");
            dt = Objda.return_DataTable("select EProductName,ProductId from Inv_ProductMaster where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and IsActive='True' and EProductName like '%" + prefixText + "%'");
        }
        string[] txt = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i][0].ToString();
        }
        return txt;
    }
    protected void txtAdvanceFilterValue_TextChanged(object sender, EventArgs e)
    {
        DataAccessClass Objda = new DataAccessClass(Session["DBConnection"].ToString());
        DataTable dt = new DataTable();
        if (txtAdvanceFilterValue.Text != "")
        {
            if (ddlTemplatesearch.SelectedIndex <= 0)
            {
                DisplayMessage("Select Filter Option");
                txtAdvanceFilterValue.Text = "";
                ddlTemplatesearch.Focus();
                return;
            }
            else if (HttpContext.Current.Session["ddlTemplatesearchValue"].ToString() == "CG")
            {
                //for business group
                dt = Objda.return_DataTable("select Group_Name,Group_Id from Ems_GroupMaster where Group_Name ='" + txtAdvanceFilterValue.Text.Trim() + "' and IsActive='True'");
            }
            else if (HttpContext.Current.Session["ddlTemplatesearchValue"].ToString() == "MO")
            {
                //for model
                dt = Objda.return_DataTable("select Model_No,Trans_Id from Inv_ModelMaster where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and IsActive='True' and Model_No ='" + txtAdvanceFilterValue.Text.Trim() + "'");
            }
            else if (HttpContext.Current.Session["ddlTemplatesearchValue"].ToString() == "PB")
            {
                //for product brand
                dt = Objda.return_DataTable("select Brand_Name,PBrandId from Inv_ProductBrandMaster where IsActive='True' and Brand_Name ='" + txtAdvanceFilterValue.Text.Trim() + "'");
            }
            else if (HttpContext.Current.Session["ddlTemplatesearchValue"].ToString() == "PC")
            {
                //for product category
                dt = Objda.return_DataTable("select Category_Name,Category_Id from Inv_Product_CategoryMaster where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and IsActive='True' and Category_Name ='" + txtAdvanceFilterValue.Text.Trim() + "'");
            }
            else if (HttpContext.Current.Session["ddlTemplatesearchValue"].ToString() == "PI")
            {
                //for product
                dt = Objda.return_DataTable("select ProductCode,ProductId from Inv_ProductMaster where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and IsActive='True' and ProductCode ='" + txtAdvanceFilterValue.Text.Trim() + "'");
            }
            else if (HttpContext.Current.Session["ddlTemplatesearchValue"].ToString() == "PN")
            {
                //for product
                dt = Objda.return_DataTable("select EProductName,ProductId from Inv_ProductMaster where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Brand_Id=" + HttpContext.Current.Session["BrandId"].ToString() + " and IsActive='True' and EProductName ='" + txtAdvanceFilterValue.Text.Trim() + "'");
            }
            if (dt.Rows.Count > 0)
            {
                hdnAdvanceFilterValue.Value = dt.Rows[0][1].ToString();
            }
            else
            {
                DisplayMessage("Select Value in suggestion only");
                txtAdvanceFilterValue.Text = "";
                txtAdvanceFilterValue.Focus();
                return;
            }
        }
    }
    protected void ddlProductBrand_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (DataTable dt = objProB.GetTemplateHeaderNFooter(Session["CompId"].ToString(), ddlProductBrand.SelectedValue))
            {
                if (dt.Rows.Count > 0)
                {
                    Editor1.Content = dt.Rows[0]["Field3"].ToString() + dt.Rows[0]["Field4"].ToString();
                }
                else
                {
                    Editor1.Content = null;
                }
            }
        }
        catch
        { }
    }
}