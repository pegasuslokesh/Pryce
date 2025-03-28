using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;

public partial class SystemSetUp_ProductMasterTax : System.Web.UI.Page
{
    CompanyMaster ObjCompanyMaster = null;
    UserMaster objUserMaster = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter ObjSysParam = null;
    IT_ObjectEntry objObjectEntry = null;
    Common cmn = null;
    ProductTaxMaster objProductTaxMaster = null;
    DataAccessClass objda = null;
    PageControlCommon objPageCmn = null;

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string StrUserId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjCompanyMaster = new CompanyMaster(Session["DBConnection"].ToString());
        objUserMaster = new UserMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objProductTaxMaster = new ProductTaxMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        objda = new DataAccessClass(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Session["ProTxMstr_Cat_ID"] = "";
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "374", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            StrUserId = Session["UserId"].ToString();
            StrCompId = Session["CompId"].ToString();
            StrBrandId = Session["BrandId"].ToString();
            Session["CHECKED_ITEMS"] = null;
            FillTax();
            Fill_Location();
            AllPageCode();
        }        
    }

    public void AllPageCode()
    {
        //new Code
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("374", (DataTable)Session["ModuleName"]);
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

        Page.Title = ObjSysParam.GetSysTitle();
        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;

        //old Code
        Page.Title = ObjSysParam.GetSysTitle();
        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;

        if (Session["EmpId"].ToString() == "0")
        {
            btnSave.Visible = true;
            gvTax.Columns[1].Visible = true;
            btnDelete.Visible = true;
            gvTaxCalculation.Columns[0].Visible = true;
            gvTax.Columns[2].Visible = true;
        }

        DataTable dtAllPageCode = cmn.GetAllPagePermission(StrUserId.ToString(), strModuleId, "374",Session["CompId"].ToString());
        foreach (DataRow DtRow in dtAllPageCode.Rows)
        {
            //Add

            if (DtRow["Op_Id"].ToString() == "1")
            {
                btnSave.Visible = true;
            }
            //Edit
            if (DtRow["Op_Id"].ToString() == "2")
            {
                gvTax.Columns[1].Visible = true;
            }
            //Delete
            if (DtRow["Op_Id"].ToString() == "3")
            {
                btnDelete.Visible = true;
                gvTaxCalculation.Columns[0].Visible = true;
            }
            //View
            if (DtRow["Op_Id"].ToString() == "5")
            {
                gvTax.Columns[2].Visible = true;
            }
        }
    }

    protected void FillTax()
    {
        string TaxQuery = "Select Trans_Id as Id, Tax_Name as Name from Sys_TaxMaster where isActive='true'";
        DataTable Taxdt = objda.return_DataTable(TaxQuery);
        if (Taxdt != null && Taxdt.Rows.Count > 0)
        {
            ddlTaxType.DataTextField = "Name";
            ddlTaxType.DataValueField = "Id";
            ddlTaxType.DataSource = Taxdt;
            ddlTaxType.DataBind();
            ddlTaxType.Items.Insert(0, new ListItem("--Select--", "0"));
        }
    }

    protected void Fill_Location()
    {
        DataTable Dt_Parameter = ObjSysParam.GetSysParameterByParamName("Tax System");

        if (Dt_Parameter != null && Dt_Parameter.Rows.Count > 0)
        {
            if (Dt_Parameter.Rows[0]["Param_Value"].ToString() == "Company")
            {
                Lbl_Location.Text = Resources.Attendance.Company;
                DataTable Dt_Company = new DataTable();
                DataTable dt = objUserMaster.GetUserMasterByUserId(Session["UserId"].ToString(), "");
                Dt_Company = ObjCompanyMaster.GetCompanyMaster();
                if (dt.Rows.Count != 0)
                {
                    if (dt.Rows[0]["Company_Id"].ToString() != "0" && dt.Rows[0]["Emp_Id"].ToString() != "0")
                    {
                        string CompanyId = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "C", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
                        Session["Company_Permission"] = CompanyId;
                        try
                        {
                            Dt_Company = new DataView(Dt_Company, "Company_Id in(" + CompanyId.Substring(0, CompanyId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        Dt_Company = ObjCompanyMaster.GetCompanyMaster();
                    }

                    if (Dt_Company != null && Dt_Company.Rows.Count > 0)
                    {
                        DDL_Location.DataSource = Dt_Company;
                        DDL_Location.DataTextField = "Company_Name";
                        DDL_Location.DataValueField = "Company_Id";
                        DDL_Location.DataBind();
                        //DDL_Location.Items.Insert(0, new ListItem("--Select--", "--Select--"));
                        //DDL_Location.Items.Insert(1, new ListItem("All", "0"));
                    }
                    else
                    {
                        DDL_Location.DataSource = null;
                        DDL_Location.DataBind();
                        //DDL_Location.Items.Insert(0, new ListItem("--Select--", "0"));
                    }
                    DDL_Location.SelectedValue = Session["LocId"].ToString();


                }
            }
            else if (Dt_Parameter.Rows[0]["Param_Value"].ToString() == "Location")
            {
                Lbl_Location.Text = Resources.Attendance.Location;
                DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
                try
                {
                    dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"] + "'", "", DataViewRowState.CurrentRows).ToTable();
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
                    DDL_Location.DataSource = dtLoc;
                    DDL_Location.DataTextField = "Location_Name";
                    DDL_Location.DataValueField = "Location_Id";
                    DDL_Location.DataBind();
                    //DDL_Location.Items.Insert(0, new ListItem("--Select--", "--Select--"));
                    //DDL_Location.Items.Insert(1, new ListItem("All", "0"));
                }
                else
                {
                    DDL_Location.DataSource = null;
                    DDL_Location.DataBind();
                    //DDL_Location.Items.Insert(0, new ListItem("--Select--", "0"));
                }
                DDL_Location.SelectedValue = Session["LocId"].ToString();
            }
            else if (Dt_Parameter.Rows[0]["Param_Value"].ToString() == "System")
            {
                Lbl_Location.Text = Resources.Attendance.System;
                Lbl_Location.Visible = false;
                DDL_Location.Visible = false;
            }
            else
            {
                Lbl_Location.Text = Resources.Attendance.Select;
                Lbl_Location.Visible = false;
                DDL_Location.Visible = false;
            }
        }
    }

    protected void ImgBtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        ArrayList userdetails = new ArrayList();

        if (ViewState["Select"] == null)
        {
            if (Session["CHECKED_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];

            ViewState["Select"] = 1;

            foreach (GridViewRow gvrow in gvTax.Rows)
            {
                Label ProductId = (Label)gvrow.FindControl("lblPId");
                string IdValue = string.Empty;
                IdValue = ProductId.Text;

                if (!userdetails.Contains(Convert.ToInt32(IdValue)))
                {
                    ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
                    userdetails.Add(Convert.ToInt32(IdValue));
                }
            }

            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;
        }
        else
        {
            foreach (GridViewRow gvrow in gvTax.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = false;
            }
            Session["CHECKED_ITEMS"] = null;
            ViewState["Select"] = null;
        }
    }

    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }

    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        ArrayList userdetails = new ArrayList();
        if (Session["CHECKED_ITEMS"] != null)
            userdetails = (ArrayList)Session["CHECKED_ITEMS"];

        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;

        Label ProductId = (Label)gvTax.Rows[index].FindControl("lblPId");

        if (((CheckBox)gvTax.Rows[index].FindControl("chkgvSelect")).Checked == true)
        {
            if (!userdetails.Contains(Convert.ToInt32(ProductId.Text)))
            {
                userdetails.Add(Convert.ToInt32(ProductId.Text));
            }
        }
        else
        {
            if (userdetails.Contains(Convert.ToInt32(ProductId.Text)))
            {
                userdetails.Remove(Convert.ToInt32(ProductId.Text));
            }

        }

        Session["CHECKED_ITEMS"] = userdetails;

    }

    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        ArrayList userdetails = new ArrayList();
        if (Session["CHECKED_ITEMS"] != null)
            userdetails = (ArrayList)Session["CHECKED_ITEMS"];

        CheckBox chkSelAll = ((CheckBox)gvTax.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvTax.Rows)
        {
            Label ProductId = (Label)gr.FindControl("lblPId");

            if (chkSelAll.Checked == true)
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = true;
                if (!userdetails.Contains(Convert.ToInt32(ProductId.Text)))
                {
                    userdetails.Add(Convert.ToInt32(ProductId.Text));
                }
            }
            else
            {
                ((CheckBox)gr.FindControl("chkgvSelect")).Checked = false;
                if (userdetails.Contains(Convert.ToInt32(ProductId.Text)))
                {
                    userdetails.Remove(Convert.ToInt32(ProductId.Text));
                }
            }
        }

        Session["CHECKED_ITEMS"] = userdetails;
    }

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        if (editid.Value != "0")
        {
            string Tax_According = string.Empty;
            string Tax_Company = string.Empty;
            string Tax_Location = string.Empty;
            if (Lbl_Location.Text == Resources.Attendance.Company && Lbl_Location.Visible == true)
            {
                Tax_According = "Company";
                Tax_Company = DDL_Location.SelectedValue.ToString();
                Tax_Location = "0";
            }
            else if (Lbl_Location.Text == Resources.Attendance.Location && Lbl_Location.Visible == true)
            {
                Tax_According = "Location";
                Tax_Company = "0";
                Tax_Location = DDL_Location.SelectedValue.ToString();
            }
            else if (Lbl_Location.Text == Resources.Attendance.System && Lbl_Location.Visible == false)
            {
                Tax_According = "System";
                Tax_Company = "0";
                Tax_Location = "0";
            }
            else if (Lbl_Location.Text == Resources.Attendance.Select && Lbl_Location.Visible == false)
            {
                Tax_Company = "0";
                Tax_Location = "0";                
            }
            DataTable dt = objProductTaxMaster.GetProductByProductId(Tax_Company, Tax_Location, Tax_According, editid.Value, "True", "0");
            btnSaveGST.Visible = true;
            
            if (dt.Rows.Count > 0)
            {
                gvTaxCalculation.DataSource = dt;
                gvTaxCalculation.DataBind();
                foreach (GridViewRow gr in gvTaxCalculation.Rows)
                {
                    TextBox TaxValue = (TextBox)gr.FindControl("txtTaxValue");
                    TaxValue.Enabled = true;
                    ImageButton ImgBtn = (ImageButton)gr.FindControl("btnDelete");
                    ImgBtn.Visible = true;
                }
                AllPageCode();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_GST()", true);
            }
            else
            {
                DisplayMessage("No Records are found");
                return;
            }
        }
    }

    protected void btnDetail_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        if (editid.Value != "0")
        {
            btnSaveGST.Visible = false;
            string Tax_According = string.Empty;
            string Tax_Company = string.Empty;
            string Tax_Location = string.Empty;
            if (Lbl_Location.Text == Resources.Attendance.Company && Lbl_Location.Visible == true)
            {
                Tax_According = "Company";
                Tax_Company = DDL_Location.SelectedValue.ToString();
                Tax_Location = "0";
            }
            else if (Lbl_Location.Text == Resources.Attendance.Location && Lbl_Location.Visible == true)
            {
                Tax_According = "Location";
                Tax_Company = "0";
                Tax_Location = DDL_Location.SelectedValue.ToString();
            }
            else if (Lbl_Location.Text == Resources.Attendance.System && Lbl_Location.Visible == false)
            {
                Tax_According = "System";
                Tax_Company = "0";
                Tax_Location = "0";
            }
            else if (Lbl_Location.Text == Resources.Attendance.Select && Lbl_Location.Visible == false)
            {
                Tax_Company = "0";
                Tax_Location = "0";
            }
            DataTable dt = objProductTaxMaster.GetProductByProductId(Tax_Company, Tax_Location, Tax_According, editid.Value, "True", "0");
            if (dt.Rows.Count > 0)
            {
                gvTaxCalculation.Columns[0].Visible = false;
                gvTaxCalculation.DataSource = dt;
                gvTaxCalculation.DataBind();                
                foreach (GridViewRow gr in gvTaxCalculation.Rows)
                {
                    TextBox TaxValue = (TextBox)gr.FindControl("txtTaxValue");
                    TaxValue.Enabled = false;
                    ImageButton ImgBtn = (ImageButton)gr.FindControl("btnDelete");
                    ImgBtn.Visible = false;
                }
                gvTaxCalculation.Columns[0].Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_GST()", true);
            }
            else
            {
                DisplayMessage("No Detail are available");
                return;
            }
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetProductCategoryList(string prefixText, int count, string contextKey)
    {
        Inv_ProductCategoryMaster objtaxMaster = new Inv_ProductCategoryMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objtaxMaster.GetProductCategoryTrueAllData(HttpContext.Current.Session["CompId"].ToString());
        DataView view = new DataView(dt);
        view.RowFilter = "Category_Name like '%" + prefixText + "%'";
        dt = view.ToTable();
        string[] Product = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Product[i] = dt.Rows[i]["Category_Name"].ToString() + "/" + dt.Rows[i]["Category_Id"].ToString();
        }
        return Product;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetProductNameList(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["ProTxMstr_Cat_ID"].ToString() == "")
        {
            Inv_ProductMaster objtaxMaster = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = objtaxMaster.Get_Inv_Product_Master_Search(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "", "", prefixText, "", "", "", "", "", "", "1");
            int dtrow = 0;
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                    dtrow = dt.Rows.Count;
            }
            string[] Product = new string[dtrow];
            for (int i = 0; i < dtrow; i++)
            {
                Product[i] = dt.Rows[i]["EProductName"].ToString() + "/" + dt.Rows[i]["ProductId"].ToString();
            }
            return Product;
        }
        else
        {
            Inv_ProductMaster objtaxMaster = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = objtaxMaster.Get_Inv_Product_Master_Search(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "", "", prefixText, "", "", "", HttpContext.Current.Session["ProTxMstr_Cat_ID"].ToString(), "", "", "2");
            int dtrow = 0;
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                    dtrow = dt.Rows.Count;
            }
            string[] Product = new string[dtrow];
            for (int i = 0; i < dtrow; i++)
            {
                Product[i] = dt.Rows[i]["EProductName"].ToString() + "/" + dt.Rows[i]["ProductId"].ToString();
            }
            return Product;
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetHSNCodeList(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["ProTxMstr_Cat_ID"].ToString() == "")
        {
            Inv_ProductMaster objtaxMaster = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = objtaxMaster.Get_Inv_Product_Master_Search(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "", "", prefixText, "", "", "", "", "", "", "3");
            int dtrow = 0;
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                    dtrow = dt.Rows.Count;
            }
            string[] Product = new string[dtrow];
            for (int i = 0; i < dtrow; i++)
            {
                Product[i] = dt.Rows[i]["HScode"].ToString();
            }
            return Product;
        }
        else
        {
            Inv_ProductMaster objtaxMaster = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = objtaxMaster.Get_Inv_Product_Master_Search(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), "", "", prefixText, "", "", "", HttpContext.Current.Session["ProTxMstr_Cat_ID"].ToString(), "", "", "4");
            int dtrow = 0;
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                    dtrow = dt.Rows.Count;
            }
            string[] Product = new string[dtrow];
            for (int i = 0; i < dtrow; i++)
            {
                Product[i] = dt.Rows[i]["HScode"].ToString();
            }
            return Product;
        }
        
    }

    protected void btnExecute_Click(object sender, EventArgs e)
    {
        AllPageCode();
        string CategoryId = string.Empty;
        string ProductId = string.Empty;
        string HSNCode = string.Empty;

        if (String.IsNullOrEmpty(txtProductCategory.Text))
            CategoryId = "0";
        else
            CategoryId = txtProductCategory.Text.Split('/')[1].ToString();

        if (String.IsNullOrEmpty(txtProductName.Text))
            ProductId = "0";
        else
        {
            string Product_Name = txtProductName.Text;
            ProductId = Product_Name.Substring(Product_Name.LastIndexOf('/') + 1);
        }
        

        if (String.IsNullOrEmpty(txtHSNCode.Text))
            HSNCode = "";
        else
            HSNCode = txtHSNCode.Text.ToString();

        DataTable dt = objProductTaxMaster.GetAllProductDetails(CategoryId, ProductId, HSNCode,DDL_Location.SelectedItem.Value.ToString());

        if (dt != null && dt.Rows.Count > 0)
        {
            Session["Dt_Filter_Tx_mstr"] = dt;
            gvTax.DataSource = dt;
            gvTax.DataBind();
            AllPageCode();            
            //lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        }
        else
        {
            Session["Dt_Filter_Tx_mstr"] = null;
            gvTax.DataSource = null;
            gvTax.DataBind();
            AllPageCode();
            btnDelete.Visible = false;
            DisplayMessage("Product not found");
        }
    }


    protected void btnApplyDefault_Click(object sender, EventArgs e)
    {
        string Tax_According = string.Empty;
        string Tax_Company = string.Empty;
        string Tax_Location = string.Empty;
        if (Lbl_Location.Text == Resources.Attendance.Company && Lbl_Location.Visible == true)
        {
            Tax_According = "Company";
            Tax_Company = DDL_Location.SelectedValue.ToString();
            Tax_Location = "0";
        }
        else if (Lbl_Location.Text == Resources.Attendance.Location && Lbl_Location.Visible == true)
        {
            Tax_According = "Location";
            Tax_Company = "0";
            Tax_Location = DDL_Location.SelectedValue.ToString();
        }
        else if (Lbl_Location.Text == Resources.Attendance.System && Lbl_Location.Visible == false)
        {
            Tax_According = "System";
            Tax_Company = "0";
            Tax_Location = "0";
        }
        else if (Lbl_Location.Text == Resources.Attendance.Select && Lbl_Location.Visible == false)
        {
            Tax_Company = "0";
            Tax_Location = "0";
        }
        ArrayList userdetails = new ArrayList();
        if (Session["CHECKED_ITEMS"] != null)
            userdetails = (ArrayList)Session["CHECKED_ITEMS"];

        string TaxValue = txtTaxValue.Text;
        if (String.IsNullOrEmpty(TaxValue))
            TaxValue = "0";

        if (double.Parse(TaxValue) == 0)
        {
            DisplayMessage("Please enter valid tax value");
            return;
        }

        btnSave.Enabled = false;

        if (ddlTaxType.SelectedIndex > 0)
        {
            if (userdetails.Count == 0)
            {
                string ProductId = txtProductName.Text.Substring(txtProductName.Text.LastIndexOf('/') + 1);
                userdetails.Add(ProductId);
            }

            if (userdetails.Count > 0)
            {
                for (int i = 0; i < userdetails.Count; i++)
                {
                    if (userdetails[i].ToString() != "")
                    {

                        objProductTaxMaster.DeleteProductTaxMasterByProductId( userdetails[i].ToString());


                        objProductTaxMaster.InsertProductTaxMaster(Tax_Company, Session["BrandId"].ToString(), "7", userdetails[i].ToString(), "6", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        objProductTaxMaster.InsertProductTaxMaster(Tax_Company, Session["BrandId"].ToString(), "7", userdetails[i].ToString(), "7", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        objProductTaxMaster.InsertProductTaxMaster(Tax_Company, Session["BrandId"].ToString(), "7", userdetails[i].ToString(), "8", "18.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                        objProductTaxMaster.InsertProductTaxMaster(Tax_Company, Session["BrandId"].ToString(), "8", userdetails[i].ToString(), "6", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        objProductTaxMaster.InsertProductTaxMaster(Tax_Company, Session["BrandId"].ToString(), "8", userdetails[i].ToString(), "7", "9.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        objProductTaxMaster.InsertProductTaxMaster(Tax_Company, Session["BrandId"].ToString(), "8", userdetails[i].ToString(), "8", "18.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


                        objProductTaxMaster.InsertProductTaxMaster(Tax_Company, Session["BrandId"].ToString(), "6", userdetails[i].ToString(), "4", "5.00", "", "Location", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());




                    }
                }
                DisplayMessage("Record Saved", "green");
                btnSave.Enabled = true;
                btnExecute_Click(null, null);
                txtTaxValue.Text = "0";
                ddlTaxType.SelectedValue = "0";
                //Reset();
            }
            else
            {
                DisplayMessage("Please Select at least one product");
                btnSave.Enabled = true;
                return;
            }
        }
        else
        {
            DisplayMessage("Please Select Tax Type");
            btnSave.Enabled = true;
            return;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string Tax_According = string.Empty;
        string Tax_Company = string.Empty;
        string Tax_Location = string.Empty;
        if (Lbl_Location.Text == Resources.Attendance.Company && Lbl_Location.Visible == true)
        {
            Tax_According = "Company";
            Tax_Company = DDL_Location.SelectedValue.ToString();
            Tax_Location = "0";
        }
        else if (Lbl_Location.Text == Resources.Attendance.Location && Lbl_Location.Visible == true)
        {
            Tax_According = "Location";
            Tax_Company = "0";
            Tax_Location = DDL_Location.SelectedValue.ToString();
        }
        else if (Lbl_Location.Text == Resources.Attendance.System && Lbl_Location.Visible == false)
        {
            Tax_According = "System";
            Tax_Company = "0";
            Tax_Location = "0";
        }
        else if (Lbl_Location.Text == Resources.Attendance.Select && Lbl_Location.Visible == false)
        {
            Tax_Company = "0";
            Tax_Location = "0";
        }
        ArrayList userdetails = new ArrayList();
        if (Session["CHECKED_ITEMS"] != null)
            userdetails = (ArrayList)Session["CHECKED_ITEMS"];

        string TaxValue = txtTaxValue.Text;
        if (String.IsNullOrEmpty(TaxValue))
            TaxValue = "0";

        if (double.Parse(TaxValue) == 0)
        {
            DisplayMessage("Please enter valid tax value");
            return;
        }

        btnSave.Enabled = false;

        if (ddlTaxType.SelectedIndex > 0)
        {
            if(userdetails.Count == 0)
            {
               string  ProductId = txtProductName.Text.Substring(txtProductName.Text.LastIndexOf('/') + 1);
                userdetails.Add(ProductId);
            }

            if (userdetails.Count > 0)
            {
                for (int i = 0; i < userdetails.Count; i++)
                {
                    if (userdetails[i].ToString() != "")
                    {
                        if(chkApply.Checked)
                        {


                            DataTable dt = objda.return_DataTable("Select * From Set_LocationMaster Where Field1 = (Select Field1 From Set_LocationMaster Where Location_Id ='"+ Tax_Location + "')");
                            for(int count=0; count <dt.Rows.Count; count ++)
                            {
                                int k = objProductTaxMaster.InsertProductTaxMaster(Tax_Company, Session["BrandId"].ToString(), dt.Rows[count]["Location_Id"].ToString(), userdetails[i].ToString(), ddlTaxType.SelectedValue, TaxValue, "", Tax_According, "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                            }


                        }
                        else
                        {
                            int k = objProductTaxMaster.InsertProductTaxMaster(Tax_Company, Session["BrandId"].ToString(), Tax_Location, userdetails[i].ToString(), ddlTaxType.SelectedValue, TaxValue, "", Tax_According, "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                        
                    }
                }
                DisplayMessage("Record Saved","green");
                btnSave.Enabled = true;
                btnExecute_Click(null, null);
                txtTaxValue.Text = "0";
                ddlTaxType.SelectedValue = "0";
                //Reset();
            }
            else
            {
                DisplayMessage("Please Select at least one product");
                btnSave.Enabled = true;
                return;
            }
        }
        else
        {
            DisplayMessage("Please Select Tax Type");
            btnSave.Enabled = true;
            return;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Reset();
    }

    protected void txtProductCategory_TextChanged(object sender, EventArgs e)
    {
        Session["ProTxMstr_Cat_ID"] = "";
        txtProductName.Text = "";
        txtHSNCode.Text = "";
        string categoryQuery = "Select * from Inv_Product_CategoryMaster";
        DataTable dt = objda.return_DataTable(categoryQuery);
        try
        {
            if (txtProductCategory.Text.Contains('/'))
            {
                dt = new DataView(dt, "Category_Name='" + txtProductCategory.Text.Split('/')[0] + "'", "", DataViewRowState.CurrentRows).ToTable();
                Hdn_Category_Trans_ID.Value = dt.Rows[0]["Category_ID"].ToString();
                Session["ProTxMstr_Cat_ID"] = dt.Rows[0]["Category_ID"].ToString();
                Hdn_Category_Name.Value= dt.Rows[0]["Category_Name"].ToString();
            }
            else
            {
                Session["ProTxMstr_Cat_ID"] = "";
                dt = null;
                Hdn_Category_Trans_ID.Value = "";
                Hdn_Category_Name.Value = "";
            }
        }
        catch
        {
            Session["ProTxMstr_Cat_ID"] = "";
        }

        if (dt == null || dt.Rows.Count == 0)
        {
            Session["ProTxMstr_Cat_ID"] = "";
            DisplayMessage("Product Category is not available");
            txtProductCategory.Text = string.Empty;
            return;
        }
    }

    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        txtHSNCode.Text = "";
        string productQuery = "Select * from Inv_ProductMaster";
        DataTable dt = objda.return_DataTable(productQuery);
        try
        {
            string Product_Name = txtProductName.Text;
            int Product = Product_Name.LastIndexOf("/");
            if (Product > 0)
                Product_Name = Product_Name.Substring(0, Product);

            if (txtProductName.Text.Contains('/'))
            {
                dt = new DataView(dt, "EProductName='" + Product_Name + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                dt = null;
            }
        }
        catch
        {

        }

        if (dt == null || dt.Rows.Count == 0)
        {
            DisplayMessage("Product is not available");
            txtProductName.Text = string.Empty;
            return;
        }
    }

    protected void txtHSNCode_TextChanged(object sender, EventArgs e)
    {
        txtProductName.Text = "";
        string productQuery = "Select * from Inv_ProductMaster";
        DataTable dt = objda.return_DataTable(productQuery);
        try
        {
           dt = new DataView(dt, "HSCode='" + txtHSNCode.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }

        if (dt == null || dt.Rows.Count == 0)
        {
            DisplayMessage("HSN Code is not available");
            txtHSNCode.Text = string.Empty;
            return;
        }
    }

    protected void Reset()
    {
        Session["ProTxMstr_Cat_ID"] = "";
        Session["CHECKED_ITEMS"] = null;
        ViewState["Select"] = null;

        txtProductCategory.Text = string.Empty;
        txtProductName.Text = string.Empty;
        txtHSNCode.Text = string.Empty;

        ddlTaxType.SelectedIndex = 0;
        txtTaxValue.Text = string.Empty;

        gvTax.DataSource = null;
        gvTax.DataBind();
    }
    
    protected void btnSaveGST_Click(object sender, EventArgs e)
    {


        bool IsUpdate = false;
        foreach (GridViewRow gr in gvTaxCalculation.Rows)
        {
            Label Trans_Id = (Label)gr.FindControl("lblTrans_Id");
            Label Product_Id = (Label)gr.FindControl("lblProduct_Id");
            Label Tax_Id = (Label)gr.FindControl("lblTax_Id");

            TextBox TaxValue = (TextBox)gr.FindControl("txtTaxValue");

            if (String.IsNullOrEmpty(TaxValue.Text))
                TaxValue.Text = "0";

            if (double.Parse(TaxValue.Text) > 100)
            {
                DisplayMessage("Please Enter Valid Percentage");
                return;
            }

            if (Trans_Id.Text != "0" && Product_Id.Text != "0")
            {
                string Tax_According = string.Empty;
                string Tax_Company = string.Empty;
                string Tax_Location = string.Empty;
                if (Lbl_Location.Text == Resources.Attendance.Company && Lbl_Location.Visible == true)
                {
                    Tax_According = "Company";
                    Tax_Company = DDL_Location.SelectedValue.ToString();
                    Tax_Location = "0";
                }
                else if (Lbl_Location.Text == Resources.Attendance.Location && Lbl_Location.Visible == true)
                {
                    Tax_According = "Location";
                    Tax_Company = "0";
                    Tax_Location = DDL_Location.SelectedValue.ToString();
                }
                else if (Lbl_Location.Text == Resources.Attendance.System && Lbl_Location.Visible == false)
                {
                    Tax_According = "System";
                    Tax_Company = "0";
                    Tax_Location = "0";
                }
                else if (Lbl_Location.Text == Resources.Attendance.Select && Lbl_Location.Visible == false)
                {
                    Tax_Company = "0";
                    Tax_Location = "0";
                }
                //int k = objProductTaxMaster.InsertProductTaxMaster(Tax_Company, Session["BrandId"].ToString(), Tax_Location, userdetails[i].ToString(), ddlTaxType.SelectedValue, TaxValue, "", Tax_According, "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                int k = objProductTaxMaster.UpdateProductTaxMaster(Trans_Id.Text, Product_Id.Text, Tax_Id.Text, TaxValue.Text, "", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            IsUpdate = true;
        }

        if (IsUpdate)
        {
            DisplayMessage("Record Updated", "green");
            ClosePopUp();
            return;
        }
        else
        {
            DisplayMessage("Sorry unable to update record!");
            return;
        }
    }    

    protected void btnDelete_Command(object sender, CommandEventArgs e)
    {
        string TransId  = e.CommandArgument.ToString();
        int k = 0;

        if (editid.Value != "0")
        {
            k = objProductTaxMaster.DeleteProductTaxMasterByTransId(TransId);
        }
        if (k > 0)
        {
            DisplayMessage("Record is deleted");
            DataTable dt = objProductTaxMaster.GetProductByProductId(editid.Value);
            gvTaxCalculation.DataSource = dt;
            gvTaxCalculation.DataBind();
            btnExecute_Click(null, null);
            ClosePopUp();
            return;
        }
        else
        {
            DisplayMessage("Record is not deleted");
            return;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (gvTax.Rows.Count == 0)
        {
            DisplayMessage("No Data Exists");
            return;
        }
        int k = 0;
        if (gvTax.Rows.Count != 0)
        {
            if (Session["CHECKED_ITEMS"] != null)
            {
                ArrayList userdetails = new ArrayList();
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];

                string Tax_According = string.Empty;
                string Tax_Company = string.Empty;
                string Tax_Location = string.Empty;
                if (Lbl_Location.Text == Resources.Attendance.Company && Lbl_Location.Visible == true)
                {
                    Tax_According = "Company";
                    Tax_Company = DDL_Location.SelectedValue.ToString();
                    Tax_Location = "0";
                }
                else if (Lbl_Location.Text == Resources.Attendance.Location && Lbl_Location.Visible == true)
                {
                    Tax_According = "Location";
                    Tax_Company = "0";
                    Tax_Location = DDL_Location.SelectedValue.ToString();
                }
                else if (Lbl_Location.Text == Resources.Attendance.System && Lbl_Location.Visible == false)
                {
                    Tax_According = "System";
                    Tax_Company = "0";
                    Tax_Location = "0";
                }
                else if (Lbl_Location.Text == Resources.Attendance.Select && Lbl_Location.Visible == false)
                {
                    Tax_Company = "0";
                    Tax_Location = "0";
                    Tax_According = "";
                }


                if (userdetails.Count > 0)
                {
                    for (int i = 0; i < userdetails.Count; i++)
                    {
                        if (userdetails[i].ToString() != "")
                        {
                            k = objProductTaxMaster.DeleteProductTaxMasterByProductId(Tax_Company, Tax_Location, Tax_According, userdetails[i].ToString(), "TRUE", "0");
                        }
                    }

                    DisplayMessage("Record Deleted");
                    Reset();
                    ClosePopUp();
                    return;
                }
            }
            else
            {
                DisplayMessage("Please Select Record ");
                gvTax.Focus();
                return;
            }
        }
    }

    protected void txtTaxValue_TextChanged(object sender, EventArgs e)
    {
        TextBox txt = sender as TextBox;
        double TaxValue = 0;
        if (String.IsNullOrEmpty(txt.Text))
            TaxValue = 0;
        else
            TaxValue = double.Parse(txt.Text);

        if (TaxValue > 100)
        {
            DisplayMessage("Please Enter Valid Percentage");
            return;
        }
    }
    
    protected void ClosePopUp()
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Hide_Model_GST()", true);

    }

    protected void gvTax_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTax.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["Dt_Filter_Tx_mstr"];
        objPageCmn.FillData((object)gvTax, dt, "", "");
        AllPageCode();
    }

    protected void gvTax_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["Dt_Filter_Tx_mstr"];
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
        Session["Dt_Filter_Tx_mstr"] = dt;
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvTax, dt, "", "");
        AllPageCode();
    }
}