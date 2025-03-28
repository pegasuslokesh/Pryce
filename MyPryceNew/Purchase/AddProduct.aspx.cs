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
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using System.IO;

public partial class Purchase_AddProduct : System.Web.UI.Page
{

    #region Class Object
    string StrCompId = string.Empty;
    string UserId = string.Empty;
    string StrBrandId = string.Empty;
    string StrLocationId = string.Empty;
    string strCurrencyId = string.Empty;

    SystemParameter ObjSysParam = null;
    Inv_UnitMaster ObjUnitMaster = null;

    Common cmn = null;
    Inv_Product_CompanyBrand ObjCompanyBrand = null;
    Inv_ProductMaster ObjProductMaster = null;
    PurchaseOrderHeader ObjPurchaseOrder = null;
    PurchaseOrderDetail ObjPurchaseOrderDetail = null;
    Inv_PurchaseInquiryHeader PbjIPurchaseHeader = null;
    Inv_PurchaseQuoteHeader objQuoteHeader = null;
    Inv_PurchaseQuoteDetail ObjQuoteDetail = null;
    Inv_PurchaseInquiryHeader ObjPIHeader = null;
    PageControlCommon objPageCmn = null;
    UserMaster ObjUser = null;

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjUnitMaster = new Inv_UnitMaster(Session["DBConnection"].ToString());

        cmn = new Common(Session["DBConnection"].ToString());
        ObjCompanyBrand = new Inv_Product_CompanyBrand(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjPurchaseOrder = new PurchaseOrderHeader(Session["DBConnection"].ToString());
        ObjPurchaseOrderDetail = new PurchaseOrderDetail(Session["DBConnection"].ToString());
        PbjIPurchaseHeader = new Inv_PurchaseInquiryHeader(Session["DBConnection"].ToString());
        objQuoteHeader = new Inv_PurchaseQuoteHeader(Session["DBConnection"].ToString());
        ObjQuoteDetail = new Inv_PurchaseQuoteDetail(Session["DBConnection"].ToString());
        ObjPIHeader = new Inv_PurchaseInquiryHeader(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        StrLocationId = Session["LocId"].ToString();
        ViewState["TransId"] = Request.QueryString["Trans_Id"].ToString();

        if (!IsPostBack)
        {
            
            DataTable dt = new DataTable();
            dt = ObjQuoteDetail.GetQuoteDetailByTrans_Id(StrCompId, StrBrandId, StrLocationId, ViewState["TransId"].ToString());
            if (dt != null && dt.Rows.Count > 0)
            {
                txtPDescription.Text = dt.Rows[0]["ProductDescription"].ToString();
                txtProductName.Text = dt.Rows[0]["SuggestedProductName"].ToString();
                ViewState["SuggestedProductName"] = txtProductName.Text;
                FillUnit();
                dt.Dispose();
            }
        }

    }

    # region User Defined Function

    public void FillUnit()
    {
        try
        {
            DataTable dt = ObjUnitMaster.GetUnitMasterAll(StrCompId.ToString());
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015

            objPageCmn.FillData((object)ddlUnit, dt, "Unit_Name", "Unit_Id");
           
        }
        catch
        {
            ddlUnit.Items.Insert(0, "--Select--");
            ddlUnit.SelectedIndex = 0;

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
    #endregion
    #region System Defined Function
    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        if (txtProductcode.Text != "")
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ObjProductMaster.SearchProductMasterByParameter(StrCompId, StrBrandId, StrLocationId, txtProductcode.Text.ToString());
                if (dt.Rows.Count != 0)
                {
                    DisplayMessage("Product Is already exists!");

                    txtProductcode.Text = "";
                    txtProductcode.Focus();
                    return;
                }

            }

            catch
            {

            }
        }
        else
        {
            DisplayMessage("Enter Product Id");
            txtProductcode.Focus();
            return;
           // System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductcode);
        }
    }
    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        if (txtProductName.Text != "")
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ObjProductMaster.SearchProductMasterByParameter(StrCompId, StrBrandId, StrLocationId, txtProductName.Text.ToString());

                if (dt.Rows.Count != 0)
                {
                    DisplayMessage("Product Name already exists!");
                    txtProductName.Text = "";
                    txtProductName.Focus();
                    return;
                }

            }
            catch
            {

            }
        }
        else
        {
            DisplayMessage("Enter Product Name");
            txtProductName.Focus();
            return;
           // System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductName);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {


        if (txtProductName.Text == "")
        {
            DisplayMessage("Enter Product Name");
            txtProductName.Focus();
            return;
        }
        if (txtProductcode.Text == "")
        {

            DisplayMessage("Enter Product Code");
            txtProductcode.Focus();
            return;
        }
        else
        {
            DataTable dt = new DataTable();
            dt = ObjProductMaster.SearchProductMasterByParameter(StrCompId, StrBrandId, StrLocationId, txtProductName.Text.ToString());

            if (dt != null && dt.Rows.Count != 0)
            {
                DisplayMessage("Product Name already exists!");
                txtProductName.Text = "";
                txtProductName.Focus();
                return;
            }
        }
        if (ddlUnit.SelectedIndex == 0)
        {
            DisplayMessage("Select Unit");
            ddlUnit.Focus();
            return;
        }
        if (txtPDescription.Text == "")
        {
            DisplayMessage("Enter Product Description");
            txtPDescription.Focus();
            return;
        }

        int b, i;
        b = ObjProductMaster.InsertProductMaster(StrCompId.ToString(), Session["BrandId"].ToString(), txtProductcode.Text.Trim().ToString(), "0", "0", "0", txtProductName.Text.Trim().ToString(), txtProductName.Text.Trim().ToString(), Session["CompId"].ToString(), ddlUnit.SelectedValue, "0", false.ToString(), false.ToString(), "0", false.ToString(), "0", "0", txtPDescription.Text.Trim().ToString(), "0", "0", "0", "", "0", "0", "0", "0", "0", "0", "0", "0", "", "URL", "0", "", "0", "0", "0", "", "", "", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(),"0","0", Session["LocCurrencyId"].ToString(),"","","");
        if (b != 0)
        {
            i= ObjCompanyBrand.InsertProductCompanyBrand(StrCompId.ToString(), b.ToString(), Session["BrandId"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
           if(i!= 0)
            {
            i = ObjProductMaster.UpdateProductIDInReference(ViewState["SuggestedProductName"].ToString(), b.ToString());
            }

            DisplayMessage("Record Saved","green");

           
           // in use ScriptManager.RegisterStartupScript(Page, typeof(Page), "close", "window.opener.location.href ='../Purchase/CompareQuatation.aspx?RPQId=" + Request.QueryString["RPQId"].ToString() + "';window.opener.location.reload();", true);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "close", "window.opener.location.href ='../Purchase/CompareQuatation.aspx?RPQId=" + HttpUtility.UrlEncode(Encrypt(Request.QueryString["RPQId"].ToString())) + "';window.close();", true);
         
        }
        else
        {
            DisplayMessage("Record Not Saved");
        }
    }
   


    #endregion
    #region Encryptanddecrypt
    private string Encrypt(string clearText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }
    #endregion
}
