using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using PegasusDataAccess;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using System.Web.Script.Services;

public partial class Inventory_ModelMaster : BasePage
{
    #region defined Class Object
    Set_ApplicationParameter objAppParam = null;
    Inv_ProductCategoryMaster ObjProductCateMaster = null;
    Inv_ProductCategoryDetail ObjProductCategoryDetail = null;
    Inv_ModelMaster ObjInvModelMaster = null;
    CurrencyMaster ObjCurrencyMaster = null;
    Inv_Model_Category_Product objInvModelCategoryProduct = null;
    BillOfMaterial ObjInvBOM = null;
    SystemParameter ObjSysPeram = null;
    Set_DocNumber objDocNo = null;
    UserMaster ObjUser = null;
    Common cmn = null;
    Set_Suppliers ObjSupplierMaster = null;
    Inv_Product_Category ObjProductCate = null;
    Inv_Model_Suppliers objModelSupplier = null;
    Inv_Model_Category objModelCategory = null;
    SystemParameter ObjSysParam = null;
    DataAccessClass objDa = null;
    //For Arcawing
    Arc_Directory_Master objDir = null;
    Arc_FileTransaction ObjFile = null;
    Document_Master ObjDocument = null;
    Inv_ProductMaster ObjProductMaster = null;
    PageControlCommon objPageCmn = null;
    net.webservicex.www.CurrencyConvertor obj = new net.webservicex.www.CurrencyConvertor();
    net.webservicex.www.Currency Currency = new net.webservicex.www.Currency();


    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjProductCateMaster = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());
        ObjProductCategoryDetail = new Inv_ProductCategoryDetail(Session["DBConnection"].ToString());
        ObjInvModelMaster = new Inv_ModelMaster(Session["DBConnection"].ToString());
        ObjCurrencyMaster = new CurrencyMaster(Session["DBConnection"].ToString());
        objInvModelCategoryProduct = new Inv_Model_Category_Product(Session["DBConnection"].ToString());
        ObjInvBOM = new BillOfMaterial(Session["DBConnection"].ToString());
        ObjSysPeram = new SystemParameter(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjSupplierMaster = new Set_Suppliers(Session["DBConnection"].ToString());
        ObjProductCate = new Inv_Product_Category(Session["DBConnection"].ToString());
        objModelSupplier = new Inv_Model_Suppliers(Session["DBConnection"].ToString());
        objModelCategory = new Inv_Model_Category(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        //For Arcawing
        ObjDocument = new Document_Master(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objDir = new Arc_Directory_Master(Session["DBConnection"].ToString());
        ObjFile = new Arc_FileTransaction(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Inventory/ModelMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

            AllPageCode(clsPagePermission);
            Session["dtArcaWing"] = null;
            txtModelNo.Text = GetDocumentNumber();
            FillProductCategory();
            FillModelCategory();
            //FillGridBin();
            FillGrid(true);
            FillCurrency();
            //btnList_Click(null, null);
            //AllPageCode();

            //if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud")
            //{
            //    File_Manager_Model.Visible = false;
            //}

            Session["ModelCategoryId"] = null;
            Session["dtModelRelatedProduct"] = null;
            txtsalePrice1.Text = "4";
            txtSalesprice2.Text = "3";
            txtsalesprice3.Text = "2";

            RootFolder();
            try
            {
                string ParmValue = objAppParam.GetApplicationParameterValueByParamName("ImageFileUploadSize", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

                ASPxFileManager1.SettingsUpload.ValidationSettings.MaxFileSize = int.Parse(ParmValue) * 1000;
            }
            catch(Exception ex)
            {

            }
        }
        // //AllPageCode();
    }
    protected override PageStatePersister PageStatePersister
    {
        get
        {
            return new SessionPageStatePersister(Page);
        }
    }
    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        hdnCanUpload.Value = clsPagePermission.bUpload.ToString().ToLower();
        btnSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
    }
    #endregion
    #region
    public string GetDocumentNumber()
    {
        string DocumentNo = string.Empty;

        DataTable Dt = objDocNo.GetDocumentNumberAll(Session["CompId"].ToString(), "11", "217");

        if (Dt.Rows.Count > 0)
        {
            if (Dt.Rows[0]["Prefix"].ToString() != "")
            {
                DocumentNo += Dt.Rows[0]["Prefix"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["CompId"].ToString()))
            {
                DocumentNo += Session["Compid"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["BrandId"].ToString()))
            {
                DocumentNo += Session["BrandId"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["LocationId"].ToString()))
            {
                DocumentNo += Session["LocId"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["DeptId"].ToString()))
            {
                DocumentNo += (string)Session["DepartmentId"];
            }

            if (Convert.ToBoolean(Dt.Rows[0]["EmpId"].ToString()))
            {
                DataTable Dtuser = ObjUser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["LoginCompany"].ToString());
                DocumentNo += Dtuser.Rows[0]["Emp_Id"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["Year"].ToString()))
            {
                DocumentNo += DateTime.Now.Year.ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["Month"].ToString()))
            {
                DocumentNo += DateTime.Now.Month.ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["Day"].ToString()))
            {
                DocumentNo += DateTime.Now.Day.ToString();
            }

            if (Dt.Rows[0]["Suffix"].ToString() != "")
            {
                DocumentNo += Dt.Rows[0]["Suffix"].ToString();
            }
            if (DocumentNo != "")
            {
                DocumentNo += "-" + (Convert.ToInt32(ObjInvModelMaster.GetModelMasterByCompanyandBrandId(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString()).Rows.Count) + 1).ToString();
            }
            else
            {
                DocumentNo += (Convert.ToInt32(ObjInvModelMaster.GetModelMasterByCompanyandBrandId(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString()).Rows.Count) + 1).ToString();
            }
        }
        else
        {
            DocumentNo += (Convert.ToInt32(ObjInvModelMaster.GetModelMasterByCompanyandBrandId(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString()).Rows.Count) + 1).ToString();
        }
        return DocumentNo;
    }

    #endregion
    #region System defined Function
    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillGridBin();
        txtbinValue.Focus();
    }
    protected void btnView_Command(object sender, EventArgs e)
    {
        btnEdit_Command(sender, (CommandEventArgs)e);
        btnSave.Visible = false;
    }

    public string GetImageUrl(string image_name)
    {
        string Emp_Image = string.Empty;
        try
        {
           
            string RegistrationCode = Common.Decrypt(objDa.get_SingleValue("Select registration_code from Application_Lic_Main"));

            if (File.Exists(Server.MapPath("~/CompanyResource/" + RegistrationCode + "/" + Session["CompId"].ToString() + "/Model/" + image_name)) == true)
            {
                Emp_Image = "~/CompanyResource/" + RegistrationCode + "/" + Session["CompId"].ToString() + "/Model/" + image_name;
            }
            else
            {
                Emp_Image = "../Bootstrap_Files/dist/img/NoImage.jpg";
            }
            return Emp_Image;
        }
        catch(Exception ex)
        {
            if (File.Exists(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/Model/" + image_name)) == true)
            {
                Emp_Image = "~/CompanyResource/" + Session["CompId"].ToString() + "/Model/" + image_name;
            }
            else
            {
                Emp_Image = "../Bootstrap_Files/dist/img/NoImage.jpg";
            }
            return Emp_Image;
        }
    }

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        DataTable dt = ObjInvModelMaster.GetModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), editid.Value, "True");
        DataTable dtPro = new DataTable();
        DataTable dtCatPro = new DataTable();
        if (dt.Rows.Count > 0)
        {
            txtEModelName.Text = dt.Rows[0]["Model_Name"].ToString();
            txtlModelName.Text = dt.Rows[0]["Model_Name_L"].ToString();
            txtModelNo.Text = dt.Rows[0]["Model_No"].ToString();
            txtBasicPrice.Text = dt.Rows[0]["Field1"].ToString();
            txtCostPrice.Text = dt.Rows[0]["Cost_Price"].ToString();
            txtDescription.Content = dt.Rows[0]["Description"].ToString();
            txtHeader.Content = dt.Rows[0]["Field5"].ToString();
            txtFooter.Content = dt.Rows[0]["Field6"].ToString();
            txtModelDecription.Text = dt.Rows[0]["Field3"].ToString();
            imgProduct.ImageUrl = GetImageUrl(dt.Rows[0]["Field2"].ToString());
            Session["Image"] = dt.Rows[0]["Field2"].ToString();
            txtsalePrice1.Text = dt.Rows[0]["Sales_Price_1"].ToString();
            txtSalesprice2.Text = dt.Rows[0]["Sales_Price_2"].ToString();
            txtsalesprice3.Text = dt.Rows[0]["Sales_Price_3"].ToString();

            txtPrefixName.Text = dt.Rows[0]["SnoPrefix"].ToString();
            txtSuffixName.Text = dt.Rows[0]["SnoSuffix"].ToString();
            txtSnoStartFrom.Text = dt.Rows[0]["SnoStartFrom"].ToString();

            try
            {
                chkIsLabel.Checked = Convert.ToBoolean(dt.Rows[0]["IsLabel"].ToString());
            }
            catch
            {
                chkIsLabel.Checked = false;
            }
            FillCurrency();
            try
            {
                ddlCurrency.SelectedValue = dt.Rows[0]["Field4"].ToString();
            }
            catch
            {
            }

            //here we select record from label model

            //code start

            chkSelectedItems.Items.Clear();
            string sql = "select * from Inv_Model_LabelSize where model_id=" + editid.Value + " order by Trans_Id";
            DataTable dtlabel = objDa.return_DataTable(sql);
            for (int i = 0; i < dtlabel.Rows.Count; i++)
            {
                string ItemTaxt = string.Empty;
                if (dtlabel.Rows[i]["Field1"].ToString().Trim() != "")
                {
                    ItemTaxt = dtlabel.Rows[i]["Field3"].ToString().Trim() + dtlabel.Rows[i]["Width"].ToString() + "X" + dtlabel.Rows[i]["Height"].ToString() + " " + "(mm) - " + dtlabel.Rows[i]["gap"].ToString() + " (gap) - " + dtlabel.Rows[i]["Field1"].ToString().Trim();

                }
                else
                {
                    ItemTaxt = dtlabel.Rows[i]["Field3"].ToString().Trim() + dtlabel.Rows[i]["Width"].ToString() + "X" + dtlabel.Rows[i]["Height"].ToString() + " " + "(mm) - " + dtlabel.Rows[i]["gap"].ToString() + " (gap)";


                }
                string ItemValue = dtlabel.Rows[i]["Trans_Id"].ToString();
                chkSelectedItems.Items.Add(new ListItem(ItemTaxt, ItemValue));
            }
            //code end


            FillProductCategory();
            DataTable dtProductCat = objInvModelCategoryProduct.SelectModelCategoryProductRow(editid.Value);

            if (dtProductCat.Rows.Count > 0)
            {
                dtProductCat = dtProductCat.DefaultView.ToTable(true, "CategoryId", "categoryName", "ProductId", "ProductCode", "ProductName");

                gvRelatedProduct.DataSource = dtProductCat;
                gvRelatedProduct.DataBind();

                Session["dtModelRelatedProduct"] = dtProductCat;


            }

            DataTable dtModelCategory = objModelCategory.GetDataModelId(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), editid.Value.ToString());

            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            objPageCmn.FillData((object)lstSelectProductCategory, dtModelCategory, "Category_Name", "CategoryId");


            //DataTable dtModelCat = new DataView(dtModelCategory,"CategoryId not in '" + lstSelectProductCategory.Items.

            for (int i = 0; i < dtModelCategory.Rows.Count; i++)
            {
                ListItem li = new ListItem();
                li.Value = dtModelCategory.Rows[i]["CategoryId"].ToString();
                li.Text = dtModelCategory.Rows[i]["Category_Name"].ToString();
                //ObjProductCateMaster.GetProductCategoryByCategoryId(Session["Compid"].ToString().ToString(), dtModelCategory.Rows[i]["CategoryId"].ToString()).Rows[0]["Category_Name"].ToString();
                // lstSelectProductCategory.Items.Add(li);
                lstProductCategory.Items.Remove(li);
            }

            DataTable dtSupplier = objModelSupplier.GetModelSuppliersByModelId(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), editid.Value);
            dtSupplier.Columns.Add("Name");

            DataTable dtMod = new DataTable();
            dtMod.Columns.Add("Supplier_Id");
            dtMod.Columns.Add("Name");
            dtMod.Columns.Add("ModelSupplierCode");
            for (int i = 0; i < dtSupplier.Rows.Count; i++)
            {
                try
                {
                    dtSupplier.Rows[i]["Name"] = ObjSupplierMaster.GetSupplierAllDataBySupplierId(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), dtSupplier.Rows[i]["Supplier_Id"].ToString()).Rows[0]["Name"].ToString();
                }
                catch
                {

                }
                dtMod.Rows.Add(i);
                dtMod.Rows[i]["Supplier_Id"] = dtSupplier.Rows[i]["Supplier_Id"].ToString();
                dtMod.Rows[i]["Name"] = dtSupplier.Rows[i]["Name"].ToString();
                dtMod.Rows[i]["ModelSupplierCode"] = dtSupplier.Rows[i]["ModelSupplierCode"].ToString();
            }


            try
            {
                divLastSerialNo.Visible = true;
                DataTable dtSerialConfig = objDa.return_DataTable("select max(cast(substring(SerialNo,"+ (Convert.ToInt16(dt.Rows[0]["SnoPrefix"].ToString().Length)+1) + ",len(SerialNo)) as int)) as lastSno from Inv_StockBatchMaster where Company_Id='"+ Session["Compid"].ToString() + "' and SerialNo like '" + dt.Rows[0]["SnoPrefix"].ToString() + "%'   and ISNUMERIC(substring(SerialNo," + (Convert.ToInt32 (dt.Rows[0]["SnoPrefix"].ToString().Length)+1) + ",len(SerialNo))) =1");
                if (dtSerialConfig.Rows.Count > 0 && !string.IsNullOrEmpty(dtSerialConfig.Rows[0]["lastSno"].ToString()))
                {
                    lblLastSerial.Text = int.Parse(dtSerialConfig.Rows[0]["lastSno"].ToString()).ToString();
                }
                else
                {
                    lblLastSerial.Text = "-";
                }
 
            }
            catch
            {

            }

            Session["dtModelSupplierCode"] = dtMod;
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvModelSupplierCode, dtSupplier, "", "");

            //for get record of arcwing

            //code start




            if (sender is LinkButton)
            {
                Lbl_Tab_New.Text = Resources.Attendance.Edit;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            }
            else
            {
                try
                {
                    if (((LinkButton)sender).ID == "btnEdit")
                    {
                        Lbl_Tab_New.Text = Resources.Attendance.Edit;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                    }
                    else
                    {
                        Lbl_Tab_New.Text = Resources.Attendance.View;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                        gvModelSupplierCode.Visible = Convert.ToBoolean(Inventory_Common.CheckUserPermission("24", "20", Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()));
                    }
                }
                catch
                {
                    if (((ImageButton)sender).ID == "btnEdit")
                    {
                        Lbl_Tab_New.Text = Resources.Attendance.Edit;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                    }
                    else
                    {
                        Lbl_Tab_New.Text = Resources.Attendance.View;
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
                        gvModelSupplierCode.Visible = Convert.ToBoolean(Inventory_Common.CheckUserPermission("24", "20", Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()));
                    }
                }

            }
        }

    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        //here we set validation that if it is i used then we can not delete 

        if (ObjInvBOM.BOM_ByModelId(Session["CompId"].ToString(), e.CommandArgument.ToString()).Rows.Count > 0)
        {
            DisplayMessage("Model is in use , you can not delete !");
            return;
        }

        int b = 0;
        int f = 0;
        b = ObjInvModelMaster.DeleteModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            //f = objInvModelCategoryProduct.DeleteModelProductCategory(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), e.CommandArgument.ToString());
            //if (f != 0)
            //{
            DisplayMessage("Record Deleted");
            FillGrid();
            Reset();
            try
            {
                int i = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
                ((LinkButton)gvModelMaster.Rows[i].FindControl("IbtnDelete")).Focus();
            }
            catch
            {
                txtValue.Focus();
            }

        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
    }

    protected void imbBtnGrid_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        dtlistProduct.Visible = false;
        gvModelMaster.Visible = true;
        FillGrid(true);
        imgBtnDatalist.Visible = true;
        imbBtnGrid.Visible = false;
        txtValue.Focus();


    }

    protected void imgBtnDatalist_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        dtlistProduct.Visible = true;
        gvModelMaster.Visible = false;
        imgBtnDatalist.Visible = false;
        FillGrid(false);
        imbBtnGrid.Visible = true;
        txtValue.Focus();
        //AllPageCode();

    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (ddlcategorysearch.SelectedIndex != 0 || txtSupplierSearch.Text != "")
        {
            btngo_Click(null, null);
        }
        else
        {
            FillGrid();
        }
        FillGridBin();
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
        txtValue.Focus();
        //AllPageCode();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
        txtModelNo.Focus();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FillGrid();
        //FillGridBin();
        Reset();
        //btnList_Click(null, null);
        editid.Value = "";
        FillModelCategory();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
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
            DataTable dtCust = (DataTable)Session["Model"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["ModelFilter"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvModelMaster, view.ToTable(), "", "");
            objPageCmn.FillData((object)dtlistProduct, view.ToTable(), "", "");
            ////AllPageCode();
            txtValue.Focus();
            //AllPageCode();
        }
    }
    protected void gvModelMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvModelMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["ModelFilter"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvModelMaster, dt, "", "");
        ////AllPageCode();
        gvModelMaster.BottomPagerRow.Focus();
    }
    protected void gvModelMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["ModelFilter"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["Model"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvModelMaster, dt, "", "");


        ////AllPageCode();
        gvModelMaster.HeaderRow.Focus();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtSnoStartFrom.Text))
        {
            int sNoStratFrom = 0;
            int.TryParse(txtSnoStartFrom.Text, out sNoStratFrom);
            if (sNoStratFrom == 0)
            {
                DisplayMessage("Please check Serial Start From Value");
                return;
            }
        }

        btnSave.Enabled = false;
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        bool IsLabel = false;

        foreach (ListItem li in chkSelectedItems.Items)
        {
            chkIsLabel.Checked = true;
            break;
        }


        if (txtModelNo.Text == "")
        {
            DisplayMessage("Enter Model No");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtModelNo);
            btnSave.Enabled = true;
            return;
        }
        if (txtEModelName.Text == "")
        {
            DisplayMessage("Enter Model Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtEModelName);
            btnSave.Enabled = true;
            return;
        }
        if (txtModelDecription.Text == "")
        {
            DisplayMessage("Enter Description");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtModelDecription);
            btnSave.Enabled = true;
            return;
        }

        if (txtCostPrice.Text == "")
        {
            txtCostPrice.Text = "0.000";
        }

        if (txtBasicPrice.Text == "")
        {
            txtBasicPrice.Text = "0.000";
        }
        if (ddlCurrency.SelectedIndex == 0)
        {
            DisplayMessage("Select Currency");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(ddlCurrency);
            btnSave.Enabled = true;
            return;

        }
        string ExchangeRate = string.Empty;
        string LocalPrice = string.Empty;


        //updated on 30-11-2015 for currency conversion by jitendra upadhyay
        ExchangeRate = SystemParameter.GetExchageRate(ddlCurrency.SelectedValue, Session["CurrencyId"].ToString(), Session["DBConnection"].ToString());


        //here we get local price= exchagerate*model_Price

        if (txtBasicPrice.Text != "" && txtBasicPrice.Text != "0")
        {
            try
            {
                LocalPrice = ObjSysParam.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), (float.Parse(txtBasicPrice.Text) * float.Parse(ExchangeRate)).ToString());
            }
            catch
            {
                LocalPrice = "0";
            }
        }
        else
        {
            LocalPrice = "0";
        }

        int b = 0;
        int d = 0;
        if (Session["Image"] == null)
        {
            Session["Image"] = "";
        }


        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {

            DataTable dtcatid = new DataTable();
            if (editid.Value == "")
            {

                string ModelId = string.Empty;
                b = ObjInvModelMaster.InsertModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), txtModelNo.Text.Trim(), txtEModelName.Text, txtlModelName.Text, txtDescription.Content.Trim(), LocalPrice, chkIsLabel.Checked.ToString(), txtsalePrice1.Text, txtSalesprice2.Text, txtsalesprice3.Text, txtBasicPrice.Text.Trim(), Session["Image"].ToString(), txtModelDecription.Text.Trim(), ddlCurrency.SelectedValue.ToString(), txtHeader.Content, txtFooter.Content, DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), txtPrefixName.Text, txtSuffixName.Text, txtSnoStartFrom.Text, txtCostPrice.Text, ref trns);

                if (b != 0)
                {
                    //here we insert all label size 
                    //code start

                    int j = 0;
                    foreach (ListItem li in chkSelectedItems.Items)
                    {
                        string Narration = string.Empty;
                        string strPerforation = string.Empty;
                        string Width = "0";
                        string Height = "0";
                        string gap = "0";
                        string[] str;
                        if (li.Text.Contains('/'))
                        {
                            Narration = li.Text.Split('/')[0].ToString() + "/";
                            str = li.Text.Split('/')[1].ToString().Split(' ');
                        }
                        else
                        {
                            str = li.Text.Split(' ');
                        }

                        string[] strWh = str[0].Replace("X", ",").Split(',');
                        Width = strWh[0].ToString();
                        Height = strWh[1].ToString();
                        gap = str[3].ToString();


                        string ItemValue = string.Empty;
                        try
                        {
                            ItemValue = ((float.Parse(Width.Trim()) + float.Parse(gap.Trim())) * (float.Parse(Height.Trim()) + float.Parse(gap.Trim()))).ToString();
                        }
                        catch
                        {
                        }
                        try
                        {

                            if (str.Length == 7)
                            {
                                strPerforation = str[6].ToString();
                            }
                            else if (str.Length == 8)
                            {

                                strPerforation = str[6].ToString() + " " + str[7].ToString();
                            }

                        }
                        catch
                        {

                        }

                        j++;
                        string sql = "insert into Inv_Model_LabelSize values(" + Session["CompId"].ToString() + "," + Session["BrandId"].ToString() + "," + b.ToString() + "," + Height + "," + Width + "," + ItemValue + "," + gap + ",'" + strPerforation.Trim() + "','" + j.ToString() + "','" + Narration.ToString() + "',' ',' ', ' ',' ','" + true.ToString() + "','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString() + "','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString() + "')";
                        objDa.execute_Command(sql, ref trns);
                    }

                    //code end



                    //Code For Inserting Model Related Product Category
                    foreach (GridViewRow gvrow in gvRelatedProduct.Rows)
                    {
                        string CategoryId = ((Label)gvrow.FindControl("lblgvCategoryid")).Text;
                        string ProductId = ((Label)gvrow.FindControl("lblgvProductid")).Text;


                        d = objInvModelCategoryProduct.InsertModelProductCategory(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), b.ToString(), ((Label)gvrow.FindControl("lblgvProductid")).Text, ((Label)gvrow.FindControl("lblgvCategoryid")).Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);



                    }

                    //Code For inserting Model Category
                    foreach (ListItem li in lstSelectProductCategory.Items)
                    {
                        try
                        {
                            objModelCategory.InsertModelCategory(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), b.ToString(), li.Value.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        catch
                        {

                        }


                    }
                    DataTable dtSupplier = (DataTable)Session["dtModelSupplierCode"];
                    if (dtSupplier != null)
                    {
                        for (int i = 0; i < dtSupplier.Rows.Count; i++)
                        {
                            objModelSupplier.InsertModelSuppliers(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), b.ToString(), dtSupplier.Rows[i]["Supplier_Id"].ToString(), dtSupplier.Rows[i]["ModelSupplierCode"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }

                    //for arcawing

                    //code start


                    //code end

                    DisplayMessage("Record Saved", "green");
                    txtModelNo.Focus();
                }

                else
                {
                    DisplayMessage("Record Not Saved");
                }
            }
            else
            {
                b = ObjInvModelMaster.UpdateModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString().ToString(), editid.Value.ToString(), txtModelNo.Text.Trim(), txtEModelName.Text.Trim(), txtlModelName.Text.Trim(), txtDescription.Content.Trim(), LocalPrice, chkIsLabel.Checked.ToString(), txtsalePrice1.Text, txtSalesprice2.Text, txtsalesprice3.Text, txtBasicPrice.Text.Trim(), Session["Image"].ToString(), txtModelDecription.Text.Trim(), ddlCurrency.SelectedValue.ToString(), txtHeader.Content, txtFooter.Content, DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), txtPrefixName.Text, txtSuffixName.Text, txtSnoStartFrom.Text, txtCostPrice.Text, ref trns);
                if (b != 0)
                {
                    string sql = string.Empty;



                    //here we insert all label size 

                    //code start

                    //first delete record by model id
                    sql = "select * from Inv_ProductMaster where ModelNo=" + editid.Value + "";
                    if (objDa.return_DataTable(sql, ref trns).Rows.Count == 0)
                    {
                        sql = "delete from Inv_Model_LabelSize where Model_Id=" + editid.Value + "";
                        objDa.execute_Command(sql, ref trns);
                    }
                    int k = 0;
                    foreach (ListItem li in chkSelectedItems.Items)
                    {

                        string Narration = string.Empty;
                        k++;
                        string strPerforation = string.Empty;
                        string Width = "0";
                        string Height = "0";
                        string gap = "0";

                        string[] str;
                        if (li.Text.Contains('/'))
                        {
                            Narration = li.Text.Split('/')[0].ToString() + "/";
                            str = li.Text.Split('/')[1].ToString().Split(' ');
                        }
                        else
                        {
                            str = li.Text.Split(' ');
                        }
                        string[] strWh = str[0].Replace("X", ",").Split(',');
                        Width = strWh[0].ToString();
                        Height = strWh[1].ToString();
                        gap = str[3].ToString();

                        sql = "select * from Inv_Model_LabelSize where Model_Id=" + editid.Value + " and Trans_Id=" + li.Value + "";
                        if (objDa.return_DataTable(sql, ref trns).Rows.Count == 0)
                        {
                            string ItemValue = string.Empty;
                            try
                            {
                                ItemValue = ((float.Parse(Width.Trim()) + float.Parse(gap.Trim())) * (float.Parse(Height.Trim()) + float.Parse(gap.Trim()))).ToString();
                            }
                            catch
                            {
                            }

                            try
                            {

                                if (str.Length == 7)
                                {
                                    strPerforation = str[6].ToString();
                                }
                                else if (str.Length == 8)
                                {

                                    strPerforation = str[6].ToString() + " " + str[7].ToString();
                                }

                            }
                            catch
                            {

                            }



                            sql = "insert into Inv_Model_LabelSize values(" + Session["CompId"].ToString() + "," + Session["BrandId"].ToString() + "," + editid.Value + "," + Height + "," + Width + "," + ItemValue + "," + gap + ",'" + strPerforation + "','" + k.ToString() + "','" + Narration.ToString() + "',' ',' ', ' ',' ','" + true.ToString() + "','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString() + "','" + Session["UserId"].ToString() + "','" + DateTime.Now.ToString() + "')";

                            objDa.execute_Command(sql, ref trns);
                        }

                    }

                    //code end



                    int j;
                    j = objInvModelCategoryProduct.DeleteModelProductCategory(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString().ToString(), editid.Value.ToString(), ref trns);

                    //for insert related product category

                    foreach (GridViewRow gvrow in gvRelatedProduct.Rows)
                    {
                        string CategoryId = ((Label)gvrow.FindControl("lblgvCategoryid")).Text;
                        string ProductId = ((Label)gvrow.FindControl("lblgvProductid")).Text;


                        objInvModelCategoryProduct.InsertModelProductCategory(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), editid.Value, ((Label)gvrow.FindControl("lblgvProductid")).Text, ((Label)gvrow.FindControl("lblgvCategoryid")).Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);


                    }

                    objModelCategory.DeleteModelCategory(Session["Compid"].ToString().ToString(), editid.Value.ToString(), ref trns);

                    //Code For inserting Model Category
                    foreach (ListItem li in lstSelectProductCategory.Items)
                    {
                        try
                        {
                            objModelCategory.InsertModelCategory(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), editid.Value.ToString(), li.Value.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                        }
                        catch
                        {

                        }
                    }
                    //Code for Inserting Model Suppliers

                    objModelSupplier.DeleteModelSuppliers(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString().ToString(), editid.Value.ToString(), ref trns);

                    DataTable dtSupplier = (DataTable)Session["dtModelSupplierCode"];
                    if (dtSupplier != null)
                    {
                        for (int i = 0; i < dtSupplier.Rows.Count; i++)
                        {
                            objModelSupplier.InsertModelSuppliers(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), editid.Value.ToString(), dtSupplier.Rows[i]["Supplier_Id"].ToString(), dtSupplier.Rows[i]["ModelSupplierCode"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                    //btnList_Click(null, null);
                    DisplayMessage("Record Updated", "green");
                    Lbl_Tab_New.Text = Resources.Attendance.New;
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                }
                else
                {
                    DisplayMessage("Record Not Updated");
                }
            }
            //here we commit transaction when all data insert and update proper 
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            Reset();
            FillGrid();
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
            btnSave.Enabled = true;
            return;
        }
        btnSave.Enabled = true;
    }
    protected void txtEModelName_TextChanged(object sender, EventArgs e)
    {
        if (txtEModelName.Text != "")
        {
            DataTable dtModel = ObjInvModelMaster.GetModelMasterByModelName(Session["Compid"].ToString(), Session["BrandId"].ToString(), txtEModelName.Text);

            // dtModel = new DataView(dtModel, "Model_Name='" + txtEModelName.Text.Trim() + "'and Company_Id='" + Session["Compid"].ToString().ToString() + "'and Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtModel.Rows.Count > 0)
            {
                if (dtModel.Rows[0]["Trans_Id"].ToString() != editid.Value)
                {
                    if (Convert.ToBoolean(dtModel.Rows[0]["IsActive"].ToString()))
                    {
                        DisplayMessage("Model Name Already Exists");
                    }
                    else
                    {

                        DisplayMessage("Model Name Already Exists :- Go to Bin Tab");
                    }
                    txtEModelName.Text = "";
                    txtEModelName.Focus();
                }
            }
            else
            {
                txtlModelName.Focus();
            }
        }
    }
    protected void txtModelNo_TextChanged(object sender, EventArgs e)
    {
        if (txtModelNo.Text != "")
        {
            if (txtModelNo.Text.Contains('*'))
            {
                DisplayMessage("Invalid Model Number");
                txtModelNo.Text = "";
                txtModelNo.Focus();
                return;
            }

            DataTable dtModel = ObjInvModelMaster.GetModelMaster();
            dtModel = new DataView(dtModel, "Model_No='" + txtModelNo.Text.Trim() + "'and Company_Id='" + Session["Compid"].ToString().ToString() + "'and Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtModel.Rows.Count > 0)
            {
                if (dtModel.Rows[0]["Trans_Id"].ToString() != editid.Value)
                {
                    if (Convert.ToBoolean(dtModel.Rows[0]["IsActive"].ToString()))
                    {
                        DisplayMessage("Model No Already Exists");
                    }
                    else
                    {
                        DisplayMessage("Model No Already Exists :- Go to Bin Tab");
                    }
                    txtModelNo.Text = GetDocumentNumber();
                    txtModelNo.Focus();
                }
            }
            else
            {
                txtEModelName.Focus();
            }
        }
    }



    #region filemanager



    protected void btnRemove_OnClick(object sender, EventArgs e)
    {
        if (Session["Image"] != null)
        {
            Session["Image"] = "";
        }


        try
        {

            imgProduct.ImageUrl = "../Bootstrap_Files/dist/img/NoImage.jpg";
        }
        catch
        {

        }

    }


    #endregion
    #region Bin Section
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
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + txtbinValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + txtbinValue.Text + "%'";
            }
            DataTable dtModel = (DataTable)Session["dtbinModel"];


            DataView view = new DataView(dtModel, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvModelMasterBin, view.ToTable(), "", "");



            if (view.ToTable().Rows.Count == 0)
            {
                //imgBtnRestore.Visible = false;
                //ImgbtnSelectAll.Visible = false;
            }
            else
            {
                ////AllPageCode();
            }
        }
        txtbinValue.Focus();
    }
    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGridBin();

        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }
    protected void gvModelMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvModelMasterBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtbinFilter"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvModelMasterBin, dt, "", "");

        ////AllPageCode();

        string temp = string.Empty;

        for (int i = 0; i < gvModelMasterBin.Rows.Count; i++)
        {
            Label lblconid = (Label)gvModelMasterBin.Rows[i].FindControl("lblModelId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvModelMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }

        gvModelMasterBin.BottomPagerRow.Focus();

    }
    protected void gvModelMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtbinFilter"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvModelMasterBin, dt, "", "");


        ////AllPageCode();
        gvModelMasterBin.HeaderRow.Focus();
    }
    //Er
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvModelMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvModelMasterBin.Rows.Count; i++)
        {
            ((CheckBox)gvModelMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvModelMasterBin.Rows[i].FindControl("lblModelId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvModelMasterBin.Rows[i].FindControl("lblModelId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvModelMasterBin.Rows[i].FindControl("lblModelId"))).Text.Trim().ToString())
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
        ((CheckBox)gvModelMasterBin.HeaderRow.FindControl("chkgvSelectAll")).Focus();
    }
    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvModelMasterBin.Rows[index].FindControl("lblModelId");
        if (((CheckBox)gvModelMasterBin.Rows[index].FindControl("chkgvSelect")).Checked)
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
        ((CheckBox)gvModelMasterBin.Rows[index].FindControl("chkgvSelect")).Focus();
    }
    //ER
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        DataTable dtModel = (DataTable)Session["dtbinFilter"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtModel.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Trans_Id"]))
                {
                    lblSelectedRecord.Text += dr["Trans_Id"] + ",";
                }
            }
            for (int i = 0; i < gvModelMasterBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)gvModelMasterBin.Rows[i].FindControl("lblModelId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvModelMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtModel1 = (DataTable)Session["dtBinFilter"];
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvModelMasterBin, dtModel1, "", "");


            // //AllPageCode();
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
                    b = ObjInvModelMaster.DeleteModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
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
        }
        else
        {
            int flag = 0;
            foreach (GridViewRow Gvr in gvModelMasterBin.Rows)
            {
                CheckBox chk = (CheckBox)Gvr.FindControl("chkgvSelect");
                if (chk.Checked)
                {
                    flag = 1;
                }
                else
                {
                    flag = 0;
                }
            }
            if (flag == 0)
            {
                DisplayMessage("Please Select Record");
            }
            else
            {
                DisplayMessage("Record Not Activated");
            }
        }
        txtbinValue.Focus();
    }
    #endregion
    #endregion
    #region Auto Complete Function
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Inv_ModelMaster ObjModelMaster = new Inv_ModelMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(ObjModelMaster.GetModelMaster(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "True"), "Model_Name like '%" + prefixText.ToString() + "%'", "Model_Name Asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Model_Name"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList1(string prefixText, int count, string contextKey)
    {
        Inv_ModelMaster ObjModelMaster = new Inv_ModelMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(ObjModelMaster.GetModelMaster(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "True"), "Model_No like '%" + prefixText.ToString() + "%'", "Model_No Asc", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Model_No"].ToString();
        }
        return txt;
    }
    #endregion
    #region User defined Function

    public void FillGrid(bool IsGridview = true)
    {
        DataTable dt = ObjInvModelMaster.GetModelMaster(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString(), "True");
        if (IsGridview)
        {
            objPageCmn.FillData((object)gvModelMaster, dt, "", "");

        }
        else
        {
            objPageCmn.FillData((object)dtlistProduct, dt, "", "");
        }
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

        Session["Model"] = dt;
        Session["ModelFilter"] = dt;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        //AllPageCode();
        // //AllPageCode();
    }
    public void Reset()
    {
        txtModelNo.Text = GetDocumentNumber();
        txtEModelName.Text = "";
        txtCostPrice.Text = "0";
        txtlModelName.Text = "";
        Session["ModelCategoryId"] = null;
        txtBasicPrice.Text = "";
        ChkProductCategory.Items.Clear();
        ChkProductChildCategory.Items.Clear();
        FillProductCategory();
        txtsalePrice1.Text = "4";
        txtSalesprice2.Text = "3";
        txtsalesprice3.Text = "2";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 3;
        ddlFieldName.SelectedIndex = 0;
        txtDescription.Content = "";
        txtHeader.Content = "";
        txtFooter.Content = "";
        txtModelDecription.Text = "";
        Session["Image"] = null;
        imgProduct.ImageUrl = "../Bootstrap_Files/dist/img/NoImage.jpg";
        FillCurrency();
        chkIsLabel.Checked = false;
        lstSelectProductCategory.Items.Clear();
        FillModelCategory();
        Session["dtModelSupplierCode"] = null;
        Session["dtModelRelatedProduct"] = null;
        gvRelatedProduct.DataSource = null;
        gvRelatedProduct.DataBind();
        gvModelSupplierCode.DataSource = null;
        gvModelSupplierCode.DataBind();
        txtSupplierSearch.Text = "";
        lblLastSerial.Text = "-";

        //if (Session["EmpId"].ToString() == "0")
        //{
        //    btnSave.Visible = true;

        //}
        //else
        //{
        //    DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), ViewState["ModuleId"].ToString(), "217");
        //    try
        //    {
        //        dtAllPageCode = new DataView(dtAllPageCode, "Op_Id=1", "", DataViewRowState.CurrentRows).ToTable();

        //    }
        //    catch
        //    {
        //    }
        //    if (dtAllPageCode.Rows.Count > 0)
        //    {
        //        btnSave.Visible = true;
        //    }
        //}
        txtwidth.Text = "";
        txtHeight.Text = "";
        txtgap.Text = "";
        chkSelectedItems.Items.Clear();
        txtPrefixName.Text = string.Empty;
        txtSuffixName.Text = string.Empty;
        txtSnoStartFrom.Text = string.Empty;
        gvModelSupplierCode.Visible = true;

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
        DataTable dtres = (DataTable)ViewState["MessageDt"];
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
    public void FillGridBin()
    {

        DataTable dt = new DataTable();
        dt = ObjInvModelMaster.GetModelMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), "False");
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvModelMasterBin, dt, "", "");

        Session["dtbinFilter"] = dt;
        Session["dtbinModel"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        if (dt.Rows.Count == 0)
        {
            //imgBtnRestore.Visible = false;
            //ImgbtnSelectAll.Visible = false;
        }
        else
        {

            // //AllPageCode();
        }
    }


    public void FillCurrency()
    {
        try
        {
            DataTable dt = ObjCurrencyMaster.GetCurrencyMaster();

            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            objPageCmn.FillData((object)ddlCurrency, dt, "Currency_Name", "Currency_Id");

        }
        catch
        {
            ddlCurrency.Items.Insert(0, "--Select--");
            ddlCurrency.SelectedIndex = 0;

        }
        if (ddlCurrency.Items.Count > 0)
        {
            try
            {
                ddlCurrency.SelectedValue = Session["CurrencyId"].ToString();

            }
            catch
            {
            }
        }
    }

    #endregion
    #region Model Category
    protected void btnPushAllCate_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstProductCategory.Items)
        {
            lstSelectProductCategory.Items.Add(li);


        }
        foreach (ListItem li in lstSelectProductCategory.Items)
        {

            lstProductCategory.Items.Remove(li);

        }
        btnPushAllCate.Focus();
    }
    protected void btnPullAllCate_Click(object sender, EventArgs e)
    {

        foreach (ListItem li in lstSelectProductCategory.Items)
        {

            lstProductCategory.Items.Add(li);


        }
        foreach (ListItem li in lstProductCategory.Items)
        {

            lstSelectProductCategory.Items.Remove(li);

        }

        btnPullAllCate.Focus();
    }
    protected void btnPushCate_Click(object sender, EventArgs e)
    {
        if (lstProductCategory.SelectedIndex >= 0)
        {

            foreach (ListItem li in lstProductCategory.Items)
            {
                if (li.Selected)
                {
                    lstSelectProductCategory.Items.Add(li);

                }
            }
            foreach (ListItem li in lstSelectProductCategory.Items)
            {
                lstProductCategory.Items.Remove(li);
            }
            lstSelectProductCategory.SelectedIndex = -1;
        }
        btnPushCate.Focus();
    }
    protected void btnPullCate_Click(object sender, EventArgs e)
    {
        if (lstSelectProductCategory.SelectedIndex >= 0)
        {

            foreach (ListItem li in lstSelectProductCategory.Items)
            {
                if (li.Selected)
                {
                    lstProductCategory.Items.Add(li);

                }
            }
            foreach (ListItem li in lstProductCategory.Items)
            {
                lstSelectProductCategory.Items.Remove(li);
            }
            lstProductCategory.SelectedIndex = -1;
        }
        btnPullCate.Focus();
    }
    private void FillModelCategory()
    {
        DataTable dsCategory = null;
        dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(Session["Compid"].ToString().ToString());
        if (dsCategory.Rows.Count > 0)
        {
            ddlcategorysearch.Items.Clear();

            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            objPageCmn.FillData((object)ddlcategorysearch, dsCategory, "Category_Name", "Category_Id");

        }
        else
        {
            ddlcategorysearch.Items.Add("No Category Available Here");
        }
    }
    #endregion
    #region Suppliers

    protected void txtSuppliers_OnTextChanged(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["dtModelSupplierCode"];
        if (dt != null)
        {
            if (txtSuppliers.Text != "")
            {
                try
                {
                    string strSupplierId = "";
                    strSupplierId = (txtSuppliers.Text.Split('/'))[txtSuppliers.Text.Split('/').Length - 1];
                    string query = "Supplier_Id = '" + strSupplierId + "'";
                    dt = new DataView(dt, query, "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        DisplayMessage("Supplier Name Already Exists");
                        txtSuppliers.Text = "";
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
                    }
                    else
                    {

                        DataTable dt1 = ObjSupplierMaster.GetSupplierAllTrueData(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString());
                        dt1 = new DataView(dt1, "Supplier_Id='" + strSupplierId + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dt1.Rows.Count > 0)
                        {
                            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductSupplierCode);
                        }
                        else
                        {
                            DisplayMessage("Invalid Supplier Name");
                            txtSuppliers.Text = "";
                            txtSuppliers.Focus();
                            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
                        }
                    }
                }
                catch
                {
                    DisplayMessage("Invalid Supplier Name");
                    txtSuppliers.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
                }
            }
        }
        else
        {
            if (txtSuppliers.Text != "")
            {
                string strSupplierId = "";
                strSupplierId = (txtSuppliers.Text.Split('/'))[txtSuppliers.Text.Split('/').Length - 1];
                DataTable dt1 = ObjSupplierMaster.GetSupplierAllTrueData(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString());
                try
                {
                    dt1 = new DataView(dt1, "Supplier_Id='" + strSupplierId + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
                if (dt1.Rows.Count > 0)
                {
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductSupplierCode);
                }
                else
                {
                    DisplayMessage("Invalid Supplier Name");

                    txtSuppliers.Focus();
                    txtSuppliers.Text = "";
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
                }
            }
        }

    }


    //Supplier :- Start
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList_Supplier(string prefixText, int count, string contextKey)
    {
        Set_Suppliers ObjSupplier = new Set_Suppliers(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtSupplier = ObjSupplier.GetSupplierAllTrueData(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString());


        string filtertext = "Name like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtSupplier, filtertext, "Name asc", DataViewRowState.CurrentRows).ToTable();

        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Supplier_Id"].ToString();
            }
        }
        return filterlist;
    }
    //Supplier :- End

    protected void gvModelSupplierCode_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvModelSupplierCode.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtModelSupplierCode"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvModelSupplierCode, dt, "", "");


    }
    protected void IbtnAddProductSupplierCode_Click(object sender, EventArgs e)
    {
        if (txtSuppliers.Text == "")
        {
            DisplayMessage("Select Supplier");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
            return;

        }
        if (txtSuppliers.Text != "")
        {
            DataTable dt = (DataTable)Session["dtModelSupplierCode"];
            if (dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add("Supplier_Id");
                dt.Columns.Add("Name");
                dt.Columns.Add("ModelSupplierCode");
            }

            string strSupplierId = "";
            string strSupplierName = "";
            if (txtSuppliers.Text != "")
            {
                strSupplierId = (txtSuppliers.Text.Split('/'))[txtSuppliers.Text.Split('/').Length - 1];
                strSupplierName = txtSuppliers.Text.Split('/')[0];
            }

            dt.Rows.Add(strSupplierId, strSupplierName, txtProductSupplierCode.Text);
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvModelSupplierCode, dt, "", "");


            Session["dtModelSupplierCode"] = dt;

            txtProductSupplierCode.Text = "";
            txtSuppliers.Text = "";
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
        }
        else
        {
            DisplayMessage("Please Select Supplier First");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSuppliers);
            return;
        }
    }
    protected void IbtnDeleteSupplier_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtModelSupplierCode"];
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                var rows = dt.Select("Supplier_Id ='" + e.CommandArgument.ToString() + "'");
                foreach (var row in rows)
                    row.Delete();
                //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
                objPageCmn.FillData((object)gvModelSupplierCode, dt, "", "");


                Session["dtModelSupplierCode"] = dt;
            }
        }

    }


    #endregion
    #region FilterSearch
    protected void btnResetSearch_Click(object sender, EventArgs e)
    {
        ddlcategorysearch.SelectedIndex = 0;
        txtSupplierSearch.Text = "";
        FillGrid();
        txtValue.Focus();
        ////AllPageCode();

    }
    protected void btngo_Click(object sender, EventArgs e)
    {

        DataTable dtModel = ObjInvModelMaster.GetModelMasterByCompanyandBrandId(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString());
        DataTable dtSearch = new DataTable();
        DataTable dt = new DataTable();
        string supplierName = string.Empty;
        string stCount = string.Empty;
        string strSupplierId = string.Empty;
        string strCount = string.Empty;
        int i = 0;
        int j = 0;

        supplierName = txtSupplierSearch.Text.Trim();

        if (ddlcategorysearch.SelectedIndex != 0)
        {

            dt = objModelCategory.GetModelByCategoryId(Session["Compid"].ToString(), Session["BrandId"].ToString(), ddlcategorysearch.SelectedValue.ToString());
            if (dt.Rows.Count > 0)
            {
                dtSearch = dt;
            }
            else
            {
                dtSearch = null;
            }

        }

        if (txtSupplierSearch.Text != "")
        {
            if (dtSearch != null)
            {
                strSupplierId = (supplierName.ToString().Split('/'))[supplierName.ToString().Split('/').Length - 1];
                DataTable dtSupp = objModelSupplier.GetModelSuppliersBySupplierId(Session["Compid"].ToString(), Session["BrandId"].ToString(), strSupplierId);


                for (j = 0; j < dtSupp.Rows.Count; j++)
                {
                    if (stCount == "")
                    {
                        stCount = dtSupp.Rows[j]["ModelId"].ToString();
                    }
                    else
                    {
                        stCount = stCount + "," + dtSupp.Rows[j]["ModelId"].ToString();
                    }
                }
                if (stCount != "")
                {
                    if (dtSearch.Rows.Count > 0)
                    {
                        dtSupp = new DataView(dtSearch, "ModelId in (" + stCount + ")", "", DataViewRowState.CurrentRows).ToTable();
                    }
                }

                if (dtSupp.Rows.Count > 0)
                {

                    dtSearch = dtSupp;
                }
                else
                {
                    dtSearch = null;
                }

            }
        }

        if (dtSearch != null)
        {
            for (i = 0; i < dtSearch.Rows.Count; i++)
            {
                if (strCount == "")
                {
                    strCount = dtSearch.Rows[i]["ModelId"].ToString();
                }
                else
                {
                    strCount = strCount + "," + dtSearch.Rows[i]["ModelId"].ToString();
                }
            }
        }
        else
        {
            strCount = "0";
        }

        if (strCount != "")
        {
            dtModel = new DataView(dtModel, "Trans_Id in (" + strCount + ")", "Model_Name asc", DataViewRowState.CurrentRows).ToTable();

        }


        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvModelMaster, dtModel, "", "");
        objPageCmn.FillData((object)dtlistProduct, dtModel, "", "");
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtModel.Rows.Count.ToString() + "";
        Session["Model"] = dtModel;
        //AllPageCode();
    }
    protected void txtSupplierSearch_OnTextChanged(object sender, EventArgs e)
    {
        string SupplierName = string.Empty;
        SupplierName = txtSupplierSearch.Text.Trim();
        if (SupplierName.ToString() != "")
        {
            try
            {
                string strSupplierId = "";
                strSupplierId = (SupplierName.ToString().Split('/'))[SupplierName.ToString().Split('/').Length - 1];
                string query = "Supplier_Id = '" + strSupplierId + "'";

                DataTable dt1 = ObjSupplierMaster.GetSupplierAllTrueData(Session["Compid"].ToString().ToString(), Session["BrandId"].ToString());
                dt1 = new DataView(dt1, "Supplier_Id='" + strSupplierId + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt1.Rows.Count > 0)
                {
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btngo);
                }
                else
                {
                    DisplayMessage("Invalid Supplier Name");
                    txtSupplierSearch.Text = "";
                    txtSupplierSearch.Focus();
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSupplierSearch);
                }
            }
            catch
            {
                DisplayMessage("Invalid Supplier Name");
                txtSupplierSearch.Text = "";
                txtSupplierSearch.Focus();
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSupplierSearch);
            }
        }
        else
        {

            DisplayMessage("Select Supplier Name");

            txtSupplierSearch.Focus();
            txtSupplierSearch.Text = "";
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtSupplierSearch);
        }



    }

    public bool IsAlphaNumeric(string inputString)
    {
        Regex r = new Regex("^[a-zA-Z0-9]+$");
        if (r.IsMatch(inputString))
            return true;
        else
            return false;
    }
    public string GetCurrencySymbol(string Amount, string CurrencyId)
    {
        string Amountwithsymbol = string.Empty;
        try
        {
            Amountwithsymbol = SystemParameter.GetCurrencySmbol(CurrencyId, GetAmountDecimal(Amount.ToString()), Session["DBConnection"].ToString());
        }
        catch
        {
            Amountwithsymbol = Amount;
        }

        return Amountwithsymbol;

    }
    public string GetAmountDecimal(string Amount)
    {

        if (ddlCurrency.SelectedIndex <= 0)
        {
            return ObjSysParam.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), Amount);
        }
        else
        {
            return ObjSysParam.GetCurencyConversionForInv(ddlCurrency.SelectedValue, Amount);
        }
    }

    #endregion
    #region Forlabelmodel

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string ItemTaxt = string.Empty;
        if (txtwidth.Text == "")
        {
            DisplayMessage("Enter Width");

            Focus();
            return;
        }
        if (txtHeight.Text == "")
        {
            DisplayMessage("Enter Heigth");

            Focus();
            return;
        }
        if (txtgap.Text == "")
        {
            DisplayMessage("Enter Gap");

            Focus();
            return;
        }

        if (ddlPerforation.SelectedIndex != 0)
        {

            ItemTaxt = txtwidth.Text.Trim() + "X" + txtHeight.Text.Trim() + " " + "(mm) - " + txtgap.Text.Trim() + " (gap) - " + ddlPerforation.SelectedValue;
        }
        else
        {
            ItemTaxt = txtwidth.Text.Trim() + "X" + txtHeight.Text.Trim() + " " + "(mm) - " + txtgap.Text.Trim() + " (gap)";
        }


        if (txtNarration.Text != "")
        {
            ItemTaxt = txtNarration.Text + "/" + ItemTaxt;
        }

        //string ItemValue = ((float.Parse(txtwidth.Text.Trim()) + float.Parse(txtgap.Text.Trim())) * (float.Parse(txtHeight.Text.Trim()) + float.Parse(txtgap.Text.Trim()))).ToString();
        string ItemValue = "0";

        //if (chkSelectedItems.Items.FindByText(ItemTaxt) == null || chkSelectedItems.Items.FindByValue(ItemValue) == null)
        //{
        chkSelectedItems.Items.Add(new ListItem(ItemTaxt, ItemValue));
        //}
        //chkSelectedItems.SelectedValue = ItemValue;

        txtHeight.Text = "";
        txtwidth.Text = "";
        txtgap.Text = "";
        ddlPerforation.SelectedIndex = 0;
        chkIsLabel.Checked = true;
        txtNarration.Text = "";
    }
    protected void imgDelete_Click(object sender, EventArgs e)
    {
        //here we check that product id id created or not in item master page
        //if created then user can not delete 

        if (editid.Value != "")
        {
            string sql = "select * from Inv_ProductMaster where ModelNo=" + editid.Value + "";
            if (objDa.return_DataTable(sql).Rows.Count > 0)
            {
                DisplayMessage("This record is in use, you can not delete !");
                return;
            }
        }
        if (chkSelectedItems.SelectedItem != null)
        {
            chkSelectedItems.Items.RemoveAt(chkSelectedItems.SelectedIndex);
        }
        else
        {
            DisplayMessage("Select Item");
        }
    }
    #endregion

    #region relatedProduct

    protected void txtproductcategorysearch_OnTextChanged(object sender, EventArgs e)
    {
        DataTable dsCategory = null;

        if (txtproductcategorysearch.Text != "")
        {

            ChkProductChildCategory.Items.Clear();

            txtproductsearch.Text = "";

            dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(Session["CompId"].ToString());

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
    protected void ChkProductCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        CheckBoxList liItem = ((CheckBoxList)sender);


        ChkProductChildCategory.Items.Clear();
        txtproductsearch.Text = "";
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
    protected void imgsearchproductcategory_OnClick(object sender, ImageClickEventArgs e)
    {
        DataTable dsCategory = null;

        if (txtproductcategorysearch.Text != "")
        {


            ChkProductChildCategory.Items.Clear();

            txtproductsearch.Text = "";

            dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(Session["CompId"].ToString());


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
    protected void ImgRefreshProductcategory_OnClick(object sender, EventArgs e)
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
    protected void ImgRefreshProduct_OnClick(object sender, EventArgs e)
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
    protected void gvRelatedProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRelatedProduct.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtModelRelatedProduct"];
        objPageCmn.FillData((object)gvRelatedProduct, dt, "", "");
    }
    protected void IbtnDeleteProduct_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = new DataView((DataTable)Session["dtModelRelatedProduct"], "ProductId<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
        objPageCmn.FillData((object)gvRelatedProduct, dt, "", "");
        Session["dtModelRelatedProduct"] = dt;
    }
    private void FillProductCategory()
    {
        DataTable dsCategory = null;
        dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(Session["CompId"].ToString());

        if (dsCategory.Rows.Count > 0)
        {
            dsCategory = new DataView(dsCategory, "", "Category_Name Asc", DataViewRowState.CurrentRows).ToTable();


            objPageCmn.FillData((object)lstProductCategory, dsCategory, "Category_Name", "Category_Id");

            ChkProductCategory.Items.Clear();
            ChkProductCategory.DataSource = dsCategory;
            ChkProductCategory.DataTextField = "Category_Name";
            ChkProductCategory.DataValueField = "Category_Id";
            ChkProductCategory.DataBind();
        }
    }

    protected void ChkProductChildCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        string strCategoryName = string.Empty;
        string strCategoryid = string.Empty;
        DataTable dt = new DataTable();

        foreach (ListItem li in ChkProductChildCategory.Items)
        {
            if (li.Selected == true)
            {
                dt = (DataTable)Session["dtModelRelatedProduct"];
                if (dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("CategoryId");
                    dt.Columns.Add("categoryName");
                    dt.Columns.Add("ProductId");
                    dt.Columns.Add("ProductCode");
                    dt.Columns.Add("ProductName");
                }
                else
                {
                    if (new DataView(dt, "ProductId=" + li.Value + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                    {
                        li.Selected = false;
                        continue;
                    }
                }

                DataTable dtProductCate = ObjProductCate.GetDataProductId(Session["CompId"].ToString(), Session["BrandId"].ToString(), li.Value);

                if (dtProductCate.Rows.Count > 0)
                {
                    strCategoryid = dtProductCate.Rows[0]["CategoryId"].ToString();
                    strCategoryName = dtProductCate.Rows[0]["ProductCategoryName"].ToString();
                }

                dt.Rows.Add(strCategoryid, strCategoryName, li.Value, ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), li.Value, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["ProductCode"].ToString(), ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), li.Value, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["EProductName"].ToString());
                Session["dtModelRelatedProduct"] = dt;
                li.Selected = false;
                //ChkProductChildCategory.Items.Remove(li);
            }
        }


        objPageCmn.FillData((object)gvRelatedProduct, dt, "", "");


    }

    #endregion

    protected void btnloadimg_Click(object sender, EventArgs e)
    {
        if (fugProduct.HasFile)
        {
            if (!Directory.Exists(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/Model/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/Model/"));
            }
            imgProduct.ImageUrl = "../Bootstrap_Files/dist/img/NoImage.jpg";

            fugProduct.SaveAs(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/Model/" + fugProduct.FileName));
            imgProduct.ImageUrl = "~/CompanyResource/" + Session["CompId"].ToString() + "/Model/" + fugProduct.FileName;

            Session["Image"] = fugProduct.FileName;
        }
    }

    protected void FuLogo_FileUploadComplete(object sender, EventArgs e)
    {
        if (fugProduct.HasFile)
        {
            string ext = fugProduct.FileName.Substring(fugProduct.FileName.Split('.')[0].Length);
            if ((ext != ".png") && (ext != ".jpg") && (ext != ".jpge"))
            {
                DisplayMessage("Invalid File Type, Select Only .png, .jpg, .jpge extension file");
                return;
            }
            else
            {
                if (!Directory.Exists(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/Model/")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/Model/"));
                }
                imgProduct.ImageUrl = null;

                fugProduct.SaveAs(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/Model/" + fugProduct.FileName));
                imgProduct.ImageUrl = "~/CompanyResource/" + Session["CompId"].ToString() + "/Model/" + fugProduct.FileName;

                Session["Image"] = fugProduct.FileName;
            }
        }
    }


    #region ArchivingModule
    protected void btnFileUpload_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;
        FUpload1.setID(e.CommandArgument.ToString(), Session["CompId"].ToString() + "/" + Session["BrandId"].ToString() + "/Model", "Inventory", "Model", e.CommandName.ToString(), e.CommandName.ToString());
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Open_FileUpload()", true);
    }
    #endregion


    protected void ASPxFileManager1_SelectedFileOpened(object source, DevExpress.Web.FileManagerFileOpenedEventArgs e)
    {
        Byte[] buffer = FileToByteArray(Server.MapPath(e.File.FullName.ToString()));
        try
        {
            File.WriteAllBytes(Server.MapPath("~/CompanyResource/" + Session["CompId"].ToString() + "/Model/" + e.File.Name), buffer);
        }
        catch
        {
        }
        imgProduct.ImageUrl = e.File.FullName.ToString();
        Session["Image"] = e.File.Name;
        //btnNew_Click(null, null);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);

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

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string ASPxFileManager1_SelectedFileOpened(string fullname, string name)
    {
        try
        {
            string Date = DateTime.Now.ToString("yyyyMMddHHss");
            string fileExtension = Path.GetExtension(name);
            DataAccessClass objda = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(name) + Date;
            name = fileNameWithoutExtension + fileExtension;
         
            try
            {
                string RegistrationCode = Common.Decrypt(objda.get_SingleValue("Select registration_code from Application_Lic_Main"));
                fullname = fullname.Replace("Product_", "Product//Product_");
                FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(fullname), FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                long numBytes = new FileInfo(HttpContext.Current.Server.MapPath(fullname)).Length;
                Byte[] buffer = br.ReadBytes((int)numBytes);
                string fullPath = "~/CompanyResource/"+ RegistrationCode + "/" + HttpContext.Current.Session["CompId"].ToString() + "/Model";
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
            catch
            {
                FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(fullname), FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                long numBytes = new FileInfo(HttpContext.Current.Server.MapPath(fullname)).Length;
                Byte[] buffer = br.ReadBytes((int)numBytes);
                string fullPath = "~/CompanyResource/"+ HttpContext.Current.Session["CompId"].ToString() + "/Model";
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
                    catch (Exception ex)
                    {

                    }
                }
            }
            HttpContext.Current.Session["Image"] = name;
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
}
