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
using System.Security.Cryptography;
using System.IO;
using System.Text;

public partial class Purchase_CompareQuatation : BasePage
{
    Inv_PurchaseQuoteHeader objQuoteHeader = null;
    Inv_PurchaseQuoteDetail ObjQuoteDetail = null;
    Inv_ProductMaster ObjProductMaster = null;
    Ems_ContactMaster objContact = null;
    CompanyMaster ObjCompanyMaster = null;
    Set_AddressChild objAddChild = null;
    Set_AddressMaster ObjAddMaster = null;
    SystemParameter ObjSysParam = null;
    LocationMaster objLocation = null;
    PageControlCommon objPageCmn = null;
    Common cmn = null;
    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string StrLocationId = string.Empty;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        objQuoteHeader = new Inv_PurchaseQuoteHeader(Session["DBConnection"].ToString());
        ObjQuoteDetail = new Inv_PurchaseQuoteDetail(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objContact = new Ems_ContactMaster(Session["DBConnection"].ToString());
        ObjCompanyMaster = new CompanyMaster(Session["DBConnection"].ToString());
        objAddChild = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjAddMaster = new Set_AddressMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        StrLocationId = Session["LocId"].ToString();
        ViewState["RequestId"] = Decrypt(HttpUtility.UrlDecode(Request.QueryString["RPQId"].ToString()));
        Session["Products"]= null;
        if (Request.QueryString["C"] != null)
        {
            if (Request.QueryString["C"].ToString() == "F")
            {
                btnConfirm.Visible = false;
            }
        }
        if (!IsPostBack)
        {
            fillDataList(Decrypt(HttpUtility.UrlDecode(Request.QueryString["RPQId"].ToString())));
        }

    }

   
   
    public void fillDataList(string RPQNo)
    {
        DataTable dtQuoteHeader = objQuoteHeader.GetQuoteHeaderAllDataByRPQ_No(StrCompId, StrBrandId, StrLocationId, RPQNo.Trim());
        string Id = dtQuoteHeader.Rows[0]["Trans_Id"].ToString();
        string strCurrencyId = dtQuoteHeader.Rows[0]["Field1"].ToString();


      

        DataTable dtPQDetail = GetRecord(ObjQuoteDetail.GetQuoteDetailByRPQ_No(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Id.ToString()));
        DataTable dtComp = ObjCompanyMaster.GetCompanyMasterById(Session["CompId"].ToString());
        lblCompanyName.Text = dtComp.Rows[0]["Company_Name"].ToString();
        try
        {
            string RefName = objAddChild.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString()).Rows[0]["Address_Name"].ToString();
            DataTable dtAdd = ObjAddMaster.GetAddressDataByAddressName(RefName);
            lblAddress.Text = dtAdd.Rows[0]["Address"].ToString();
            lblPhone.Text = dtAdd.Rows[0]["PhoneNo1"].ToString();
        }
        catch
        {


        }
        try
        {
            imgCompany.ImageUrl = "../CompanyResource" + "/" + dtComp.Rows[0]["Company_Id"].ToString().Trim() + "/" + dtComp.Rows[0]["Logo_Path"].ToString();
        }
        catch
        {
        }

        if (dtPQDetail.Rows.Count != 0)
        {
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvSupplier, dtPQDetail, "", "");

        }
        bool st = true;
        for (int i = 0; i < dtPQDetail.Rows.Count;i++)
        {
            if (dtPQDetail.Rows[i]["Product_Id"].ToString() == "0")
            {
                st = false;
            }
        }
        if (st == true)
        {
            btnConfirm.Visible = true;
        }
        else
        {
            btnConfirm.Visible = false;
        }
        if (Request.QueryString["C"] != null)
        {
            if (Request.QueryString["C"].ToString() == "F")
            {
                btnConfirm.Visible = false;
            }
        }

        try
        {
            gvSupplier.HeaderRow.Cells[5].Text = SystemParameter.GetCurrencySmbol(strCurrencyId, Resources.Attendance.Supplier, Session["DBConnection"].ToString());

        }
        catch
        {

        }
    }
    public DataTable GetRecord(DataTable dt)
    {
        DataTable dtreturn = new DataTable();
        dtreturn.Columns.Add("Trans_Id");
        dtreturn.Columns.Add("Product_Id");
        dtreturn.Columns.Add("ProductCode");
        dtreturn.Columns.Add("ProductName");
        dtreturn.Columns.Add("ReqQty");
       // dtreturn.Columns.Add("Field2");
        dtreturn.Rows.Add();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["Product_Id"].ToString() != "0")
            {
                DataTable dtTemp = new DataView(dtreturn, "Product_Id='" + dt.Rows[i]["Product_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtTemp.Rows.Count == 0)
                {
                    if (dtreturn.Rows.Count == 1)
                    {
                        dtreturn.Rows[0]["Product_Id"] = dt.Rows[i]["Product_Id"].ToString();
                        dtreturn.Rows[0]["ProductName"] = dt.Rows[i]["ProductName"].ToString();
                        dtreturn.Rows[0]["Trans_Id"] = dt.Rows[i]["Trans_Id"].ToString();
                        dtreturn.Rows[0]["ProductCode"] = dt.Rows[i]["ProductCode"].ToString();
                        dtreturn.Rows[0]["ReqQty"] = dt.Rows[i]["ReqQty"].ToString();
                    }
                    else
                    {

                        dtreturn.Rows[dtreturn.Rows.Count - 1]["Product_Id"] = dt.Rows[i]["Product_Id"].ToString();
                        dtreturn.Rows[dtreturn.Rows.Count - 1]["ProductName"] = dt.Rows[i]["ProductName"].ToString();
                        dtreturn.Rows[dtreturn.Rows.Count - 1]["Trans_Id"] = dt.Rows[i]["Trans_Id"].ToString();
                        dtreturn.Rows[dtreturn.Rows.Count - 1]["ProductCode"] = dt.Rows[i]["ProductCode"].ToString();
                        dtreturn.Rows[dtreturn.Rows.Count - 1]["ReqQty"] = dt.Rows[i]["ReqQty"].ToString();

                    }
                    dtreturn.Rows.Add();
                }
            }
            else
            {
                DataTable dtTemp = new DataView(dtreturn, "Product_Id='" + dt.Rows[i]["Product_Id"].ToString() + "' and ProductName='" + dt.Rows[i]["SuggestedProductName"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtTemp.Rows.Count == 0)
                {
                    if (dtreturn.Rows.Count == 1)
                    {
                        dtreturn.Rows[0]["Product_Id"] = dt.Rows[i]["Product_Id"].ToString();
                        dtreturn.Rows[0]["ProductName"] = dt.Rows[i]["ProductName"].ToString();
                        dtreturn.Rows[0]["Trans_Id"] = dt.Rows[i]["Trans_Id"].ToString();
                        dtreturn.Rows[0]["ProductCode"] = dt.Rows[i]["ProductCode"].ToString();
                        dtreturn.Rows[0]["ReqQty"] = dt.Rows[i]["ReqQty"].ToString();
                    }
                    else
                    {
                        dtreturn.Rows[dtreturn.Rows.Count - 1]["Product_Id"] = dt.Rows[i]["Product_Id"].ToString();
                        dtreturn.Rows[dtreturn.Rows.Count - 1]["ProductName"] = dt.Rows[i]["ProductName"].ToString();
                        dtreturn.Rows[dtreturn.Rows.Count - 1]["Trans_Id"] = dt.Rows[i]["Trans_Id"].ToString();
                        dtreturn.Rows[dtreturn.Rows.Count - 1]["ProductCode"] = dt.Rows[i]["ProductCode"].ToString();
                        dtreturn.Rows[dtreturn.Rows.Count - 1]["ReqQty"] = dt.Rows[i]["ReqQty"].ToString();

                    }
                    dtreturn.Rows.Add();
                }
            }
            
        }
        dtreturn.Rows.RemoveAt(dtreturn.Rows.Count - 1);

        return dtreturn;
    }
   
    protected void imgPrint_Click(object sender, ImageClickEventArgs e)
    {

        if (ViewState["RequestId"] != null)
        {
            fillDataList(ViewState["RequestId"].ToString());
        }

    }


    protected void gvSupplier1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label l1 = (Label)e.Row.FindControl("lblProductId");
            GridView dtlistSup = (GridView)e.Row.FindControl("gvSupplier");

            string Id = objQuoteHeader.GetQuoteHeaderAllDataByRPQ_No(StrCompId, StrBrandId, StrLocationId, Decrypt(HttpUtility.UrlDecode(Request.QueryString["RPQId"].ToString()))).Rows[0]["Trans_Id"].ToString();
            DataTable dtPQDetail = new DataTable();
            if (l1.Text.Trim() != "0")
            {
                dtPQDetail = new DataView(ObjQuoteDetail.GetQuoteDetailByRPQ_No(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Id.ToString()), "Product_Id='" + l1.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

            }
            else
            {
                Label lNm = (Label)e.Row.FindControl("txtProductName");
                dtPQDetail = new DataView(ObjQuoteDetail.GetQuoteDetailByRPQ_No(StrCompId.ToString(), StrBrandId.ToString(), StrLocationId.ToString(), Id.ToString()), "Product_Id='" + l1.Text.Trim() + "' and SuggestedProductName='" + lNm.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            dtPQDetail = new DataView(dtPQDetail, "", "Amount Asc", DataViewRowState.CurrentRows).ToTable();
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)dtlistSup, dtPQDetail, "", "");
        }
    }
    protected void rbtnSelect_CheckedChanged(object sender, EventArgs e)
    {
        RadioButton selectButton = (RadioButton)sender;
        GridView gvChildgv = (GridView)((RadioButton)sender).Parent.Parent.Parent.Parent;
        GridViewRow gvChildgvRow =(GridViewRow)((RadioButton)sender).Parent.Parent;
        
        foreach (GridViewRow rw in gvChildgv.Rows)
        {
            if (gvChildgvRow != rw)
            {
                ((RadioButton)rw.FindControl("rbtnSelect")).Checked = false;
            }



        }

    }
    protected void conOrder_Click(object sender, EventArgs e)
    {
        string s = "";
        DataTable dt = new DataTable();
        dt.Columns.Add("Qoute_ID", typeof(string));
        dt.Columns.Add("ProductID", typeof(string));
        dt.Columns.Add("Supplier_Id", typeof(string));
        dt.Columns.Add("Trans_Id", typeof(string));
        dt.Columns.Add("SuggestedProductName", typeof(string));
        int countSt = 0;       
        for (int i = 0; i < gvSupplier.Rows.Count; i++) // select status in drop downlist box of nested grid view
        {
             GridView childgrid = (GridView)gvSupplier.Rows[i].FindControl("gvSupplier");
             for (int m = 0; m < childgrid.Rows.Count; m++)
             {

                 RadioButton rdId = (RadioButton)childgrid.Rows[m].FindControl("rbtnSelect");
                 Label lbltran = (Label)childgrid.Rows[m].FindControl("lbltrans2");
                 if (rdId.Checked == true)
                 {
                     countSt++;
                     ObjQuoteDetail.UpdateQuoteDetailSupplierByTransID(lbltran.Text,"True");
                 }
                 else 
                 {
                     ObjQuoteDetail.UpdateQuoteDetailSupplierByTransID(lbltran.Text,"False");
                 }
             }
        }
        if (countSt < 1)
        {
            DisplayMessage("Please Select atleast One Supplier");
            return;
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.close();", true);
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
    public bool CheckSt(string TransID)
    {
        DataTable dt = new DataTable();
        dt = ObjQuoteDetail.GetQuoteDetailByTrans_Id(StrCompId, StrBrandId, StrLocationId, TransID);
        if (dt != null && dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["Field2"].ToString() == "True")
                return true;
            else
                return false;
        }
        return false;
    
    }
    protected string ProductCodeCheck(string Product_Id)
    {
        if (Product_Id == "0")
        {
            return "True";
        }
        else
        {
            return "False";
            
        }
    }
    protected void OpenProductAdd(string trans_id)
    {

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase/AddProduct.aspx?Trans_Id=" + trans_id + "');", true);
                                
    }

    protected void btnAddNewProduct_OnClick(object sender, CommandEventArgs e)
    {
        string trans_id = "";
        trans_id = e.CommandArgument.ToString();
        if (trans_id != "") 
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Purchase/AddProduct.aspx?Trans_Id=" + trans_id + "&Ref_Type=Purchase&RPQId=" + HttpUtility.UrlEncode(Encrypt(ViewState["RequestId"].ToString())) + "');", true);
        }
         
    }


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
    private string Decrypt(string cipherText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        cipherText = cipherText.Replace(" ", "+");
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }

    #endregion


    public string GetAmountDecimal(string Amount)
    {
        return ObjSysParam.GetCurencyConversionForInv(objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), Amount);

    }




}
