using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;

public partial class Purchase_Purchase_Bill_Adjustment : System.Web.UI.Page
{

    Ems_ContactMaster ObjContactMaster = null;
    SystemParameter ObjSysParam = null;
    PurchaseInvoice ObjPurchaseInvoice = null;
    Common cmn = null;
    DataAccessClass ObjDa = null;
    SystemParameter ObjSys = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        ObjContactMaster = new Ems_ContactMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjPurchaseInvoice = new PurchaseInvoice(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        ObjSys = new SystemParameter(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "384", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            txtSupplierName_AutoCompleteExtender.Enabled = true;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Div_Add_Open()", true);
            AllPageCode();
        }

    }

    protected void btnList_Click(object sender,EventArgs e)
    {
        AllPageCode();

    }

    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());

        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("384", (DataTable)Session["ModuleName"]);
        if (dtModule.Rows.Count > 0)
        {
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
            strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        }
        else
        {
            //Session.Abandon();
            //Response.Redirect("~/ERPLogin.aspx");
        }
        //End Code

        Page.Title = ObjSys.GetSysTitle();

        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
        if (Session["EmpId"].ToString() == "0")
        {

            Btn_Save.Visible = true;

        }
        else
        {
            DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["userId"].ToString(), strModuleId, "384", HttpContext.Current.Session["CompId"].ToString());
            if (dtAllPageCode.Rows.Count != 0)
            {

                foreach (DataRow DtRow in dtAllPageCode.Rows)
                {
                    if (DtRow["Op_Id"].ToString() == "1")
                    {
                        Btn_Save.Visible = true;

                    }
                }

            }
            else
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
        }
    }

    public void fillGrid(bool IsRefreshInvoiceGridView)
    {
        DataTable dt = new DataTable();
        DataTable dtTemp = new DataTable();

        dt = ObjPurchaseInvoice.GetPurchaseInvoiceTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());


        if (IsRefreshInvoiceGridView)
        {
            dtTemp = new DataView(dt, "Post = 'True' and SupplierId = " + txtSupplierName.Text.Split('/')[1].ToString() + " and Field4='Final'", "", DataViewRowState.CurrentRows).ToTable();

            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)GvPurchaseInvoice, dtTemp, "", "");
        }

        dtTemp = new DataView(dt, "SupplierId = " + txtSupplierName.Text.Split('/')[1].ToString() + " and Field4='Running' and (Field5=' ' or Field5='0')", "", DataViewRowState.CurrentRows).ToTable();
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvRunningInvoiceNonAdjusted, dtTemp, "", "");
        objPageCmn.FillData((object)gvRunningInvoiceAdjusted, null, "", "");
        dt.Dispose();
        dtTemp.Dispose();
    }




    public void FillAdjustedGrid(string StrInvoiceId)
    {

        DataTable dt = new DataTable();

        dt = ObjPurchaseInvoice.GetPurchaseInvoiceTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

        dt = new DataView(dt, "SupplierId = " + txtSupplierName.Text.Split('/')[1].ToString() + " and Field5='" + StrInvoiceId + "'", "", DataViewRowState.CurrentRows).ToTable();


        objPageCmn.FillData((object)gvRunningInvoiceAdjusted, dt, "", "");
        dt.Dispose();

    }
   
    protected void Btn_Save_Click(object sender, EventArgs e)
    {


        foreach (GridViewRow gvrow in gvRunningInvoiceNonAdjusted.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkgvSelect")).Checked)
            {

                ObjDa.execute_Command("update Inv_PurchaseInvoiceHeader set Field5='" + hdnTransId.Value + "' where Transid=" + ((Label)gvrow.FindControl("lblTransId")).Text + "");

            }

        }

        DisplayMessage("Purchase Invoice adjusted successfully");
        fillGrid(false);
        FillAdjustedGrid(hdnTransId.Value);

    }

    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {
        txtSupplierName.Text = "";

        objPageCmn.FillData((object)gvRunningInvoiceNonAdjusted, null, "", "");
        objPageCmn.FillData((object)gvRunningInvoiceAdjusted, null, "", "");
        objPageCmn.FillData((object)GvPurchaseInvoice, null, "", "");
        txtSupplierName.Focus();
    }

    protected void btnGetRecord_OnClick(object sender, EventArgs e)
    {

        fillGrid(true);
        AllPageCode();
    }


    protected void chkgvSelect_CheckedChangedDefault(object sender, EventArgs e)
    {
        foreach (GridViewRow gvr in GvPurchaseInvoice.Rows)
        {
            CheckBox chk = (CheckBox)gvr.FindControl("chkgvSelect");
            chk.Checked = false;
        }

        CheckBox chk1 = (CheckBox)sender;

        chk1.Checked = true;

        GridViewRow Gvrow = (GridViewRow)((CheckBox)sender).Parent.Parent;

        FillAdjustedGrid(((Label)Gvrow.FindControl("lblTransId")).Text);

        hdnTransId.Value = ((Label)Gvrow.FindControl("lblTransId")).Text;

    }

    public string GetDateFormat(string Date)
    {
        try
        {
            return Convert.ToDateTime(Date).ToString(HttpContext.Current.Session["DateFormat"].ToString()).ToString();
        }
        catch
        {
            return "";
        }
    }

    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        ObjDa.execute_Command("update Inv_PurchaseInvoiceHeader set Field5='0' where TransID=" + e.CommandArgument.ToString() + "");

        DisplayMessage("Purchase Invoice cancelled successfully");
        fillGrid(false);
        FillAdjustedGrid(hdnTransId.Value);
    }

    #region Auto Complete Method
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Set_Suppliers ObjSupplier = new Set_Suppliers(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtSupplier = ObjSupplier.GetAutoCompleteSupplierAll(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText);

        string[] filterlist = new string[dtSupplier.Rows.Count];
        if (dtSupplier.Rows.Count > 0)
        {
            for (int i = 0; i < dtSupplier.Rows.Count; i++)
            {
                filterlist[i] = dtSupplier.Rows[i]["Filtertext"].ToString();
            }
        }
        return filterlist;
    }

    #endregion
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    protected void txtSupplierName_TextChanged(object sender, EventArgs e)
    {
        if (txtSupplierName.Text != "")
        {
            try
            {
                txtSupplierName.Text.Split('/')[1].ToString();
            }
            catch
            {
                DisplayMessage("Enter Supplier Name");
                txtSupplierName.Text = "";
                txtSupplierName.Focus();
                return;
            }

            DataTable dt = ObjContactMaster.GetContactByContactName(txtSupplierName.Text.Trim().Split('/')[0].ToString());
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Select Supplier Name");
                txtSupplierName.Text = "";
                txtSupplierName.Focus();

            }
        }
        else
        {
            DisplayMessage("Select Supplier Name");
            txtSupplierName.Focus();
        }
    }
}