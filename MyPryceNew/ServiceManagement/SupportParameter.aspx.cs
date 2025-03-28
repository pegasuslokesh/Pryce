using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;

public partial class ServiceManagement_SupportParameter : System.Web.UI.Page
{
    Set_ApplicationParameter objAppParam = null;
    SystemParameter ObjSysParam = null;
    Inv_ProductCategoryMaster ObjProductCateMaster = null;
    DataAccessClass objDa = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjProductCateMaster = new Inv_ProductCategoryMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "347", HttpContext.Current.Session["CompId"].ToString(),HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            FillProductCategory();
            if (objAppParam.GetApplicationParameterByParamName("Support_Email", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows.Count > 0)
            {
                GetParametervalue();
                AllPageCode();
            }
        }
    }
    private void FillProductCategory()
    {
        DataTable dsCategory = null;
        dsCategory = ObjProductCateMaster.GetProductCategoryTrueAllData(Session["CompId"].ToString());

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



    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        Common ObjComman = new Common(Session["DBConnection"].ToString());


        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("348", (DataTable)Session["ModuleName"]);
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
        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;


    }

    protected void btnSaveSMSEmail_Click(object sender, EventArgs e)
    {


        if (txtEmail.Text == "")
        {
            DisplayMessage("Enter Email-Id");
            txtEmail.Focus();
            TabContainer2.ActiveTabIndex = 0;
            return;
        }

        if (!IsValidEmail(txtEmail.Text))
        {
            DisplayMessage("Email is invalid");
            txtEmail.Text = "";
            txtEmail.Focus();
            TabContainer2.ActiveTabIndex = 0;
            return;
        }

        string strpassword = string.Empty;



        if (txtPasswordEmail.Text.Trim() == "")
        {


            try
            {
                strpassword = Common.Decrypt(objAppParam.GetApplicationParameterByParamName("Support_Email_Password", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Param_Value"].ToString());
            }
            catch
            {
                DisplayMessage("Enter Password");
                txtPasswordEmail.Focus();
                TabContainer2.ActiveTabIndex = 0;
                TabContainer2.ActiveTabIndex = 0;
                return;
            }

        }
        else
        {
            strpassword = txtPasswordEmail.Text;
        }





        if (txtSMTP.Text == "")
        {
            DisplayMessage("Enter (SMTP)Outgoing Mail Server");
            txtSMTP.Focus();
            TabContainer2.ActiveTabIndex = 0;
            return;
        }
        if (txtPort.Text == "")
        {
            DisplayMessage("Enter SMTP Port");
            txtPort.Focus();
            TabContainer2.ActiveTabIndex = 0;
            return;
        }
        if (txtPop3.Text == "")
        {
            DisplayMessage("Enter (POP3) Incoming Mail Server");
            txtPop3.Focus();
            TabContainer2.ActiveTabIndex = 0;
            return;
        }
        if (txtpopport.Text == "")
        {
            DisplayMessage("Enter POP3 Port");
            txtpopport.Focus();
            TabContainer2.ActiveTabIndex = 0;
            return;
        }
        if (txtpopport.Text == "")
        {
            DisplayMessage("Enter POP3 Port");
            txtpopport.Focus();
            TabContainer2.ActiveTabIndex = 0;
            return;
        }

        if (objAppParam.GetApplicationParameterByParamName("Support_Email", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows.Count > 0)
        {
            objAppParam.UpdateApplicationParameterMaster(Session["CompId"].ToString(), "Support_Email", txtEmail.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(Session["CompId"].ToString(), "Support_Email_Password", Common.Encrypt(strpassword), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(Session["CompId"].ToString(), "Support_Email_SMTP", txtSMTP.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(Session["CompId"].ToString(), "Support_Email_Port", txtPort.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(Session["CompId"].ToString(), "Support_Email_EnableSSL", chkEnableSSL.Checked.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(Session["CompId"].ToString(), "Support_Email_Server_In", txtPop3.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(Session["CompId"].ToString(), "Support_Email_Port_In", txtpopport.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
            objAppParam.UpdateApplicationParameterMaster(Session["CompId"].ToString(), "Support_Display_Text", txtDisplayText.Text, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        else
        {
            objAppParam.InsertApplicationParameterMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Support_Email", txtEmail.Text, "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.InsertApplicationParameterMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Support_Email_Password", Common.Encrypt(strpassword), "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.InsertApplicationParameterMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Support_Email_SMTP", txtSMTP.Text, "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.InsertApplicationParameterMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Support_Email_Port", txtPort.Text, "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.InsertApplicationParameterMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Support_Email_EnableSSL", chkEnableSSL.Checked.ToString(), "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.InsertApplicationParameterMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Support_Email_Server_In", txtPop3.Text, "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.InsertApplicationParameterMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Support_Email_Port_In", txtpopport.Text, "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objAppParam.InsertApplicationParameterMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Support_Display_Text", txtDisplayText.Text, "0", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


        }



        if (objAppParam.GetApplicationParameterByParamName("Job_Card_Terms", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows.Count > 0)
        {
            string strsql = "update Set_ApplicationParameter set Description='" + txtTerms.Content + "' where Company_Id=" + Session["CompId"].ToString() + " and Brand_Id=" + Session["BrandId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + " and Param_Name='Job_Card_Terms'";
            objDa.execute_Command(strsql);

        }
        else
        {
            objAppParam.InsertApplicationParameterMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Job_Card_Terms", "", "0", txtTerms.Content, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        }

        if (objAppParam.GetApplicationParameterByParamName("Final_acceptance_Terms", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows.Count > 0)
        {
            string strsql = "update Set_ApplicationParameter set Description='" + txtAcceptance.Content + "' where Company_Id=" + Session["CompId"].ToString() + " and Brand_Id=" + Session["BrandId"].ToString() + " and Location_Id=" + Session["LocId"].ToString() + " and Param_Name='Final_acceptance_Terms'";
            objDa.execute_Command(strsql);
        }
        else
        {
            objAppParam.InsertApplicationParameterMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Final_acceptance_Terms", "", "0", txtAcceptance.Content, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        }

        //here we saving category

        string strcategoryList = "0";

        foreach (ListItem li in ChkProductCategory.Items)
        {
            if (li.Selected)
            {


                strcategoryList = strcategoryList + "," + li.Value.ToString().Trim();

            }

        }


        if (objAppParam.GetApplicationParameterByParamName("Job_Card_Parts_Tools_Category", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows.Count > 0)
        {
            objAppParam.UpdateApplicationParameterMaster(Session["CompId"].ToString(), "Job_Card_Parts_Tools_Category", strcategoryList, "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        else
        {
            objAppParam.InsertApplicationParameterMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Job_Card_Parts_Tools_Category", strcategoryList, "0", txtTerms.Content, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        }



        DisplayMessage("Record Updated Successfully", "green");

    }
    bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    public void GetParametervalue()
    {
        txtEmail.Text = objAppParam.GetApplicationParameterByParamName("Support_Email", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Param_Value"].ToString();
        txtPasswordEmail.Text = Common.Decrypt(objAppParam.GetApplicationParameterByParamName("Support_Email_Password", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Param_Value"].ToString());
        txtSMTP.Text = objAppParam.GetApplicationParameterByParamName("Support_Email_SMTP", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Param_Value"].ToString();
        txtPort.Text = objAppParam.GetApplicationParameterByParamName("Support_Email_Port", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Param_Value"].ToString();
        chkEnableSSL.Checked = Convert.ToBoolean(objAppParam.GetApplicationParameterByParamName("Support_Email_EnableSSL", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Param_Value"].ToString());
        txtPop3.Text = objAppParam.GetApplicationParameterByParamName("Support_Email_Server_In", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Param_Value"].ToString();
        txtpopport.Text = objAppParam.GetApplicationParameterByParamName("Support_Email_Port_In", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Param_Value"].ToString();
        txtDisplayText.Text = objAppParam.GetApplicationParameterByParamName("Support_Display_Text", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Param_Value"].ToString();
        try
        {
            txtTerms.Content = objAppParam.GetApplicationParameterByParamName("Job_Card_Terms", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Description"].ToString();
        }
        catch
        {

        }
        //Final_acceptance_Terms

        try
        {
            txtAcceptance.Content = objAppParam.GetApplicationParameterByParamName("Final_acceptance_Terms", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows[0]["Description"].ToString();
        }
        catch
        {

        }

        //here we selection job card tools and parts category selected
        DataTable dtcategory = objAppParam.GetApplicationParameterByParamName("Job_Card_Parts_Tools_Category", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        if (dtcategory.Rows.Count > 0)
        {
            foreach (ListItem li in ChkProductCategory.Items)
            {

                if (dtcategory.Rows[0]["Param_Value"].ToString().Trim().Split(',').Contains(li.Value))
                {
                    li.Selected = true;
                }
                else
                {
                    li.Selected = false;
                }
            }
        }
    }
    protected void btnCancelSMSEmail_Click(object sender, EventArgs e)
    {
        if (objAppParam.GetApplicationParameterByParamName("Support_Email", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()).Rows.Count > 0)
        {
            GetParametervalue();
        }
    }
}