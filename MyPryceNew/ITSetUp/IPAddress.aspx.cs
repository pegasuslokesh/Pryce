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
using System.Text;
using System.Collections.Generic;

public partial class ITSetUp_NationalityMaster : BasePage
{
    #region Defined Class Object
    Common cmn = null;
    SystemParameter objSys = null;
    IT_ObjectEntry objObjectEntry = null;
    IPAddress objIpaddress = null;
    PageControlCommon objPageCmn = null;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objIpaddress = new IPAddress(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "9", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            Session["CHECKED_ITEMS"] = null;
            btnNew_Click(null, null);
            FillGridBin();
            FillGrid();

        }
        try
        {
            GvIpAddress.DataSource = Session["dtIpAddressData"] as DataTable;
            GvIpAddress.DataBind();
        }
        catch
        {

        }

        AllPageCode();
    }
    public void AllPageCode()
    {
        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("9", (DataTable)Session["ModuleName"]);
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
        //End Code

        Page.Title = objSys.GetSysTitle();
        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;


        if (Session["EmpId"].ToString() == "0")
        {
            btnCSave.Visible = true;
            GvIpAddress.Columns[0].Visible = true;
            GvIpAddress.Columns[1].Visible = true;
            imgBtnRestore.Visible = true;
            ImgbtnSelectAll.Visible = false;

        }
        else
        {
            DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "9", HttpContext.Current.Session["CompId"].ToString());
            if (dtAllPageCode.Rows.Count != 0)
            {
                if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
                {

                }
                else
                {
                    foreach (DataRow DtRow in dtAllPageCode.Rows)
                    {
                        if (DtRow["Op_Id"].ToString() == "1")
                        {
                            btnCSave.Visible = true;
                        }

                        if (DtRow["Op_Id"].ToString() == "2")
                        {
                            GvIpAddress.Columns[0].Visible = true;
                        }
                        if (DtRow["Op_Id"].ToString() == "3")
                        {
                            GvIpAddress.Columns[1].Visible = true;
                        }
                        if (DtRow["Op_Id"].ToString() == "4")
                        {
                            imgBtnRestore.Visible = true;
                            ImgbtnSelectAll.Visible = false;
                        }
                    }
                }
            }
        }
    }
    #region User Defined Funcation
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objIpaddress.GetAllInActiveData();
        objPageCmn.FillData((object)GvIpAddressBin, dt, "", "");
        Session["dtBinIpAddressData"] = dt;
        lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        lblSelectedRecord.Text = "";
        if (dt.Rows.Count > 0)
        {
            AllPageCode();
        }
    }
    private void FillGrid()
    {
        DataTable dtIpdata = objIpaddress.GetAllActiveData();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtIpdata.Rows.Count + "";
        Session["dtIpAddressData"] = dtIpdata;
        GvIpAddress.DataSource = dtIpdata;
        GvIpAddress.DataBind();
        AllPageCode();
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
        txtValueBin.Text = "";
        lblSelectedRecord.Text = "";
        txtIPAddress.Text = "";
        txtDescription.Text = "";
        ChkIsBlocked.Checked = false;
        hdnTrans_id.Value = "";
    }
    #endregion

    #region System Defined Funcation

    protected void btnNew_Click(object sender, EventArgs e)
    {


    }
    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Reset();

    }
    protected void btnCSave_Click(object sender, EventArgs e)
    {
        if (txtIPAddress.Text.Trim() == "")
        {
            DisplayMessage("Enter IP Address");
            return;
        }
        int ref_id = 0;

        if (hdnTrans_id.Value == "")
        {
            ref_id = objIpaddress.InsertIpAddress(txtIPAddress.Text, txtDescription.Text, ChkIsBlocked.Checked.ToString(), Session["UserId"].ToString(), Session["UserId"].ToString());
            if (ref_id != 0)
            {
                DisplayMessage("Record Inserted Successfully");
                FillGrid();
                Reset();
            }
        }
        else
        {
            ref_id = objIpaddress.UpdateIpAddress(hdnTrans_id.Value, txtIPAddress.Text, txtDescription.Text, ChkIsBlocked.Checked.ToString(), Session["UserId"].ToString(), Session["UserId"].ToString());
            if (ref_id != 0)
            {
                DisplayMessage("Record Updated Successfully", "green");
                FillGrid();
                Reset();
            }
        }

    }  
    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in GvIpAddressBin.Rows)
        {
            index = (int)GvIpAddressBin.DataKeys[gvrow.RowIndex].Value;
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
            foreach (GridViewRow gvrow in GvIpAddressBin.Rows)
            {
                int index = (int)GvIpAddressBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }
    protected void GvIpAddressBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        GvIpAddressBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtBinIpAddressData"];
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)GvIpAddressBin, dt, "", "");
        AllPageCode();
        PopulateCheckedValuesemplog();
    }
    protected void GvIpAddressBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objIpaddress.GetAllInActiveData();
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtBinFilter"] = dt;
        //Common Function add By Lokesh on 11-05-2015
        objPageCmn.FillData((object)GvIpAddressBin, dt, "", "");
        AllPageCode();
        lblSelectedRecord.Text = "";
        GvIpAddressBin.HeaderRow.Focus();
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        DataTable dt_ipdata = objIpaddress.GetIpaddressByTransId(e.CommandArgument.ToString());
        Lbl_Tab_New.Text = Resources.Attendance.Edit;
        txtIPAddress.Text = dt_ipdata.Rows[0]["ip_address"].ToString();
        txtDescription.Text = dt_ipdata.Rows[0]["description"].ToString();
        ChkIsBlocked.Checked = Convert.ToBoolean(dt_ipdata.Rows[0]["is_blocked"].ToString());
        hdnTrans_id.Value= e.CommandArgument.ToString();

    }  
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        int b = 0;
        b=objIpaddress.DeActivateIpAddressByTransId(e.CommandArgument.ToString(), Session["UserId"].ToString());
        
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
        FillGridBin();
        FillGrid();
        Reset();
    }   
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        FillGridBin();
        string condition = string.Empty;
        if (ddlOptionBin.SelectedIndex != 0)
        {
            if (ddlOptionBin.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String)='" + txtValueBin.Text + "'";
            }
            else if (ddlOptionBin.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) like '%" + txtValueBin.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) Like '" + txtValueBin.Text + "%'";
            }

            DataTable dtCust = (DataTable)Session["dtBinIpAddressData"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinIpAddressData"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 11-05-2015
            objPageCmn.FillData((object)GvIpAddressBin, view.ToTable(), "", "");

            lblSelectedRecord.Text = "";
            if (view.ToTable().Rows.Count == 0)
            {
                // imgBtnRestore.Visible = false;
                // ImgbtnSelectAll.Visible = false;
            }
            else
            {
                AllPageCode();
            }
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
        }
        btnRefreshBin.Focus();
    }
    protected void btnRestoreSelected_Click(object sender, EventArgs e)
    {
        int b = 0;
        ArrayList userdetail = new ArrayList();
        if (GvIpAddressBin.Rows.Count > 0)
        {
            SaveCheckedValuesemplog();
            if (Session["CHECKED_ITEMS"] != null)
            {
                userdetail = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetail.Count > 0)
                {
                    for (int j = 0; j < userdetail.Count; j++)
                    {
                        if (userdetail[j].ToString() != "")
                        {
                            b = objIpaddress.ActivateIpAddressByTransId(userdetail[j].ToString(), Session["UserId"].ToString());
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
                    foreach (GridViewRow Gvr in GvIpAddressBin.Rows)
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
                GvIpAddressBin.Focus();
                return;
            }
        }
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["dtBinIpAddressData"];
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

                if (!userdetails.Contains(dr["trans_id"]))
                {
                    userdetails.Add(dr["trans_id"]);
                }
            }
            foreach (GridViewRow GR in GvIpAddressBin.Rows)
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
            DataTable dtUnit1 = (DataTable)Session["dtBinIpAddressData"];
            //Common Function add By Lokesh on 11-05-2015
            objPageCmn.FillData((object)GvIpAddressBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {        
        CheckBox chkSelAll = ((CheckBox)GvIpAddressBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in GvIpAddressBin.Rows)
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
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        txtValueBin.Focus();
        Session["CHECKED_ITEMS"] = null;
        FillGrid();
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 0;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }
  
    #endregion


    protected void txtIPAddress_TextChanged(object sender, EventArgs e)
    {
        if(txtIPAddress.Text.Trim()!="")
        {
            DataTable dt_ipdata= objIpaddress.GetIpByIpAddress(txtIPAddress.Text);
            if(dt_ipdata.Rows.Count>0)
            {
                DisplayMessage("IP Address Already Exist");
                txtIPAddress.Text = "";
                txtIPAddress.Focus();
                return;
            }
        }
    }
}
