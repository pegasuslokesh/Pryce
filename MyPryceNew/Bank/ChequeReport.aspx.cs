using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.Data;
using DevExpress.XtraExport;
using DevExpress.Web;
using PegasusDataAccess;

public partial class Bank_ChequeReport : System.Web.UI.Page
{
    SystemParameter objsys = null;
    LocationMaster ObjLocation = null;
    Common cmn = null;
    RoleMaster objRole = null;
    ChequeReport objChequeReport = null;
    Ac_Parameter_Location objAc_Parameter_Location = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    DataAccessClass objDa = null;
    bool isEditPermission = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        txtFromDate.Attributes.Add("readonly", "readonly");
        txtToDate.Attributes.Add("readonly", "readonly");
        objsys = new SystemParameter(Session["DBConnection"].ToString());
        ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objChequeReport = new ChequeReport(Session["DBConnection"].ToString());
        objAc_Parameter_Location = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Bank/ChequeReport.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            //Check_Page_Permission Chk_Page_ = new Check_Page_Permission();
            //if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), Common.GetObjectIdbyPageURL("../Bank/ChequeReport.aspx")).ToString() == "False")
            //{
            //    Session.Abandon();
            //    Response.Redirect("~/ERPLogin.aspx");
            //}

            var now = DateTime.Now;
            var first = new DateTime(now.Year, now.Month, 1);
            var last = first.AddMonths(1).AddDays(-1);

            txtFromDate.Text = GetDate(first.ToString());
            txtToDate.Text = GetDate(last.ToString());

            fillLocation();
            ddlLocation.SelectedValue = Session["LocId"].ToString();

            if (Request.QueryString.Count > 0)
            {
                if (Request.QueryString["SearchField"].ToString() != "")
                {

                    string[] strFields = Common.Decrypt(Request.QueryString["SearchField"].ToString()).Split(',');
                    
                    ddlLocation.SelectedValue = strFields[0].ToString();
                    txtFromDate.Text = strFields[1].ToString();
                    txtToDate.Text = strFields[1].ToString();
                    ddlIsReconsiled.SelectedValue= strFields[2].ToString();
                    ddlOption.SelectedValue= strFields[3].ToString();

                    //btnGetReport_Click(null, null);
                }
            }
        }
        fillGrid();
    }

    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {

    }


    protected void btnbind_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlFieldNameBin_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnbindBin_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void btnRefreshBin_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void imgBtnRestore_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {

    }

   
   
    public void fillLocation()
    {
        DataTable dtLoc = ObjLocation.GetLocationMaster(Session["CompId"].ToString());
        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        if (dtLoc.Rows.Count > 0)
        {
            //fillDropdown(ddlLocation, dtLoc, "Location_Name", "Location_Id");
            ddlLocation.Items.Add(new ListItem("--Select--", "0"));
            for (int i = 0; i < dtLoc.Rows.Count; i++)
            {
                ddlLocation.Items.Add(new ListItem(dtLoc.Rows[i]["Location_Name"].ToString(), dtLoc.Rows[i]["Location_Id"].ToString()));              
            }
        }
        else
        {
            ddlLocation.Items.Clear();
        }

    }


    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "" && strDate != "01-Jan-00 12:00:00 AM")
        {
            strNewDate = DateTime.Parse(strDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
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

    public void fillGrid()
    {
        if (txtToDate.Text.Trim() == "")
        {
            DisplayMessage("Please Enter All Dates");
            txtToDate.Focus();
            return;
        }

        if (ddlLocation.SelectedValue=="0")
        {
            DisplayMessage("Please select Location");
            return;
        }

        string acno = objAc_Parameter_Location.getBankAccounts(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue);
        DataTable dt_gridData = objChequeReport.getGridData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), acno, txtFromDate.Text, txtToDate.Text);

        if (ddlIsReconsiled.SelectedValue == "Reconcile")
        {
            dt_gridData = new DataView(dt_gridData, "Field2='True'", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (ddlIsReconsiled.SelectedValue == "NotReconcile")
        {
            dt_gridData = new DataView(dt_gridData, "Field2='False'", "", DataViewRowState.CurrentRows).ToTable();
        }

        if (ddlOption.SelectedValue=="Inward")
        {
            dt_gridData = new DataView(dt_gridData, "Debit_Amount>0", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (ddlOption.SelectedValue == "Outward")
        {
            dt_gridData = new DataView(dt_gridData, "Credit_Amount>0", "", DataViewRowState.CurrentRows).ToTable();
        }

        GvChequeData.DataSource = dt_gridData;
        GvChequeData.DataBind();
       
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        Btn_Add_Div.Attributes.Add("Class", "fa fa-minus");
        Div_Box_Add.Attributes.Add("Class", "box box-primary");
        fillGrid();
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
        Btn_Add_Div.Attributes.Add("Class", "fa fa-minus");
        Div_Box_Add.Attributes.Add("Class", "box box-primary");
        if (objsys.getDateForInput(txtFromDate.Text) > objsys.getDateForInput(txtToDate.Text))
        {
            DisplayMessage("To Date must be greater than From Date");
            txtToDate.Focus();
            txtToDate.Text = "";
            return ;
        }
        fillGrid();
    }

    protected void ddlOption_SelectedIndexChanged(object sender, EventArgs e)
    {
        Btn_Add_Div.Attributes.Add("Class", "fa fa-minus");
        Div_Box_Add.Attributes.Add("Class", "box box-primary");
        fillGrid();
    }

    protected void ddlIsReconsiled_SelectedIndexChanged(object sender, EventArgs e)
    {
        Btn_Add_Div.Attributes.Add("Class", "fa fa-minus");
        Div_Box_Add.Attributes.Add("Class", "box box-primary");
        fillGrid();
    }

    protected void Btn_Update_Reconcile_Click(object sender, EventArgs e)
    {
        DateTime strReconcileDate;
        
        DateTime.TryParse(txtChequeIssueDate.Text, out strReconcileDate);
        
        if (strReconcileDate == null)
        {
            DisplayMessage("Please enter valid date");
            return;
        }

        if (hdnVoucherDetailId.Value != "")
        {
            //Update Reconcile Voucher Detail
            string sql = "Update ac_voucher_detail set cheque_clear_date='" + strReconcileDate.ToString("dd-MMM-yyyy") + "',ModifiedBy='" + Session["UserId"].ToString() + "', ModifiedDate='" + DateTime.Now.ToString() +"' where trans_id='" + hdnVoucherDetailId.Value  + "'" ;
            objDa.execute_Command(sql);
        }

        fillGrid();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "myModal_Popup()", true);
    }

    protected void btnUpdateReconcileDate_Command(object sender, CommandEventArgs e)
    {
        string[] cmdArgue = e.CommandArgument.ToString().Split(';');


        hdnVoucherDetailId.Value = cmdArgue[0].ToString();

        if (cmdArgue[1].ToString() == "")
        {
            //button_delete_reconcile.Visible = false;
            txtChequeIssueDate.Text = GetDate(DateTime.Now.ToString()).ToString();
        }
        else
        {
           // button_delete_reconcile.Visible = true;
            txtChequeIssueDate.Text = cmdArgue[1].ToString();
        }

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "myModal_Popup()", true);
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        isEditPermission = clsPagePermission.bEdit;
        //IT_ObjectEntry objObjectEntry = new IT_ObjectEntry();
        //Common ObjComman = new Common();

        ////New Code created by jitendra on 09-12-2014
        //string strModuleId = string.Empty;
        //string strModuleName = string.Empty;

        ////BtnExportExcel.Visible = false;
        ////BtnExportPDF.Visible = false;
        //string stObjectId = string.Empty;
        //stObjectId = Common.GetObjectIdbyPageURL("../Bank/ChequeReport.aspx");
        //DataTable dtModule = objObjectEntry.GetModuleIdAndName(stObjectId);
        //if (dtModule.Rows.Count > 0)
        //{
        //    strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
        //    strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        //}
        //else
        //{
        //    Session.Abandon();
        //    Response.Redirect("~/ERPLogin.aspx");
        //}


        //    DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId,stObjectId);

        //    foreach (DataRow DtRow in dtAllPageCode.Rows)
        //    {
        //       //foreach (GridViewRow Row in GvCustomerInquiry.Rows)
        //        //{
        //        if (DtRow["Op_Id"].ToString() == "2")
        //        {
        //            try
        //            {
        //                isEditPermission = true;
        //            }
        //            catch
        //            {

        //            }

        //        }

        //    }
        }
    }