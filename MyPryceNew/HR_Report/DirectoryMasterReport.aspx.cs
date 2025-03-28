﻿using System;
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

public partial class HR_DirectoryMasterReport : BasePage
{
    string strCompId = string.Empty;
    string StrBrandId = string.Empty;
    string StrLocId = string.Empty;
  
    Common cmn = null;
    Arc_Directory_Master objDir = null;
    SystemParameter objSys = null;
    EmployeeMaster objEmp = null;
    Arc_FileTransaction ObjFile = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objDir = new Arc_Directory_Master(Session["DBConnection"].ToString());
        ObjFile = new Arc_FileTransaction(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "89", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
        }

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        strCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        StrLocId = Session["LocId"].ToString(); 
       
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        AllPageCode();
        if (!IsPostBack)
        {
            txtValue.Focus();

            
            FillGrid();
          

        }
        Page.Title = objSys.GetSysTitle();
    }

    public void AllPageCode()
    {

        Page.Title = objSys.GetSysTitle();
        strCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        StrLocId = Session["LocId"].ToString(); 
        Session["AccordianId"] = "17";
        Session["HeaderText"] = "ArcaWing";
        DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), "17", "62", HttpContext.Current.Session["CompId"].ToString());
        if (dtAllPageCode.Rows.Count != 0)
        {
            if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
            {
                //btnSave.Visible = true;
                foreach (GridViewRow Row in gvDirMaster.Rows)
                {
                    //((ImageButton)Row.FindControl("btnEdit")).Visible = true;
                    //((ImageButton)Row.FindControl("IbtnDelete")).Visible = true;
                }
                //imgBtnRestore.Visible = true;
                //ImgbtnSelectAll.Visible = false;
            }
            else
            {
                foreach (DataRow DtRow in dtAllPageCode.Rows)
                {
                    if (Convert.ToBoolean(DtRow["Op_Add"].ToString()))
                    {
                        //btnSave.Visible = true;
                    }
                    foreach (GridViewRow Row in gvDirMaster.Rows)
                    {
                        if (Convert.ToBoolean(DtRow["Op_Edit"].ToString()))
                        {
                            //((ImageButton)Row.FindControl("btnEdit")).Visible = true;
                        }
                        if (Convert.ToBoolean(DtRow["Op_Delete"].ToString()))
                        {
                            //((ImageButton)Row.FindControl("IbtnDelete")).Visible = true;
                        }
                    }
                    if (Convert.ToBoolean(DtRow["Op_Restore"].ToString()))
                    {
                        //imgBtnRestore.Visible = true;
                        //ImgbtnSelectAll.Visible = false;
                    }
                    if (Convert.ToBoolean(DtRow["Op_View"].ToString()))
                    {

                    }
                    if (Convert.ToBoolean(DtRow["Op_Print"].ToString()))
                    {

                    }
                    if (Convert.ToBoolean(DtRow["Op_Download"].ToString()))
                    {

                    }
                    if (Convert.ToBoolean(DtRow["Op_Upload"].ToString()))
                    {

                    }
                }
            }
        }
    }
    public void FillGrid()
    {
        string Directoryid = "0";
        DataTable dt = objDir.getDirectoryMaster(strCompId, Directoryid);
        //dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        gvDirMaster.DataSource = dt;
        gvDirMaster.DataBind();
        AllPageCode();
        Session["dtFilter_DMR"] = dt;
        Session["Dir"] = dt;

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

    }
    protected void gvDepMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDirMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_DMR"];
        gvDirMaster.DataSource = dt;
        gvDirMaster.DataBind();
        AllPageCode();

    }
    protected void gvDepMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_DMR"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_DMR"] = dt;
        gvDirMaster.DataSource = dt;
        gvDirMaster.DataBind();
        AllPageCode();
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvDirMaster.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvDirMaster.Rows.Count; i++)
        {
            ((CheckBox)gvDirMaster.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvDirMaster.Rows[i].FindControl("lblDirId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvDirMaster.Rows[i].FindControl("lblDirId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvDirMaster.Rows[i].FindControl("lblDirId"))).Text.Trim().ToString())
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
        Label lb = (Label)gvDirMaster.Rows[index].FindControl("lblDirId");
        if (((CheckBox)gvDirMaster.Rows[index].FindControl("chkgvSelect")).Checked)
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
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }



    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        FillGrid();
       
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
        txtValue.Focus();
    }

    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["dtFilter_DMR"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Id"]))
                {
                    lblSelectedRecord.Text += dr["Id"] + ",";
                }
            }
            for (int i = 0; i < gvDirMaster.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)gvDirMaster.Rows[i].FindControl("lblDirId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvDirMaster.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtFilter_DMR"];
            gvDirMaster.DataSource = dtUnit1;
            gvDirMaster.DataBind();
            ViewState["Select"] = null;
        }
    }

    protected void btnbind_Click(object sender, ImageClickEventArgs e)
    {
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
            DataTable dtCust = (DataTable)Session["Dir"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_DMR"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            gvDirMaster.DataSource = view.ToTable();
            gvDirMaster.DataBind();
            AllPageCode();
        }
        txtValue.Focus();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        DataTable DtReport = new DataTable();
        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            DtReport = ObjFile.Get_FileTransaction(Session["CompId"].ToString(), "0");
            if (DtReport.Rows.Count > 0)
            {


                Session["Querystring"] = lblSelectedRecord.Text;
                Session["ClaimRecord"] = DtReport;
                ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR_Report/DirectoryReport.aspx')", true);
     
                
            }
            else
            {
                DisplayMessage("Record Not Found");
                return;
            }
              

        }

       
         
        
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in gvDirMaster.Rows)
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
                //DisplayMessage("");
            }
        }
    }
}
