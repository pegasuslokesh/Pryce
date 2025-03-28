using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.Data.SqlClient;

public partial class Device_DeviceGroupMaster : BasePage
{
    #region defined Class Object
    DataAccessClass daClass = null;
    SystemParameter ObjSysPeram = null;
    Att_DeviceMaster ObjDevice = null;
    Common cmn = null;
    Set_DocNumber objDocNo = null;
    UserMaster ObjUser = null;
    Att_DeviceGroupMaster ObjDeviceGroup = null;
    PageControlCommon objPageCmn = null;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        daClass = new DataAccessClass(Session["DBConnection"].ToString());
        ObjSysPeram = new SystemParameter(Session["DBConnection"].ToString());
        ObjDevice = new Att_DeviceMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        ObjDeviceGroup = new Att_DeviceGroupMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
                       
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), Common.GetObjectIdbyPageURL("../Device/DeviceGroupMaster.aspx", Session["DBConnection"].ToString()), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            FillGridBin();
            FillGrid();
            btnList_Click(null, null);
            FillDeviceList();
        }
        AllPageCode();
    }

    #region AllPageCode
    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        Common ObjComman = new Common(Session["DBConnection"].ToString());


        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName(Common.GetObjectIdbyPageURL("../Device/DeviceGroupMaster.aspx", Session["DBConnection"].ToString()), (DataTable)Session["ModuleName"]);
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



        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;
        if (Session["EmpId"].ToString() == "0")
        {
            btnSave.Visible = true;
            foreach (GridViewRow Row in gvDeviceGroup.Rows)
            {
                ((ImageButton)Row.FindControl("btnEdit")).Visible = true;
                ((ImageButton)Row.FindControl("IbtnDelete")).Visible = true;
            }
            imgBtnRestore.Visible = true;
            ImgbtnSelectAll.Visible = false;

        }
        DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, Common.GetObjectIdbyPageURL("../Device/DeviceGroupMaster.aspx", Session["DBConnection"].ToString()),Session["CompId"].ToString());

        foreach (DataRow DtRow in dtAllPageCode.Rows)
        {


            if (DtRow["Op_Id"].ToString() == "1")
            {
                btnSave.Visible = true;
            }
            foreach (GridViewRow Row in gvDeviceGroup.Rows)
            {
                if (DtRow["Op_Id"].ToString() == "2")
                {
                    ((ImageButton)Row.FindControl("btnEdit")).Visible = true;
                }
                if (DtRow["Op_Id"].ToString() == "3")
                {
                    ((ImageButton)Row.FindControl("IbtnDelete")).Visible = true;
                }
            }
            if (DtRow["Op_Id"].ToString() == "4")
            {
                imgBtnRestore.Visible = true;
                ImgbtnSelectAll.Visible = false;
            }
        }
    }
    #endregion


    #region System defined Function
    public void FillDeviceList()
    {
        chkDeviceList.Items.Clear();
        DataTable dtDevice = ObjDevice.GetDeviceMaster(Session["CompId"].ToString());
        dtDevice = new DataView(dtDevice, "", "Device_Name", DataViewRowState.CurrentRows).ToTable();

        chkDeviceList.DataSource = dtDevice;
        chkDeviceList.DataTextField = "Device_Name";
        chkDeviceList.DataValueField = "Device_Id";
        chkDeviceList.DataBind();
        dtDevice.Dispose();
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = true;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
        txtValue.Focus();
        FillGrid();
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = false;
        PnlNewEdit.Visible = true;
        PnlBin.Visible = false;
        txtGroupName.Focus();
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlNewEdit.Visible = false;
        PnlBin.Visible = true;
        PnlList.Visible = false;
        FillGridBin();
        txtbinValue.Focus();
    }

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {

        //System.Threading.Thread.Sleep(20000);
        editid.Value = e.CommandArgument.ToString();
        DataTable dt = ObjDeviceGroup.GetHeaderRecord_ByGroupId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value);
        if (dt.Rows.Count != 0)
        {
            txtGroupName.Text = dt.Rows[0]["Group_Name"].ToString();
            txtGroupNameLOcal.Text = dt.Rows[0]["Group_Name_Local"].ToString();

            DataTable dtDetail = ObjDeviceGroup.GetDetailRecord(editid.Value);

            foreach (ListItem li in chkDeviceList.Items)
            {
                if (new DataView(dtDetail,"Device_Id="+li.Value+"", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                {
                    li.Selected = true;
                }
                else
                {
                    li.Selected = false;
                }
            }

            btnNew_Click(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        b = ObjDeviceGroup.UpdateHeaderRecordActiveStatus(e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
            FillGridBin();
            FillGrid();
            Reset();
        }
        else
        {
            DisplayMessage("Record Not Delete");
        }
    }
    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        FillGrid();
        FillGridBin();
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
        txtValue.Focus();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FillGrid();
        FillGridBin();
        Reset();
        btnList_Click(null, null);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
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
            DataTable dtCust = (DataTable)Session["Unit"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            ViewState["dtFilter"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvDeviceGroup, view.ToTable(), "", "");
            AllPageCode();
            txtValue.Focus();

        }

    }


    protected void gvDeviceGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDeviceGroup.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)ViewState["dtFilter"];
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvDeviceGroup, dt, "", "");

        AllPageCode();
        gvDeviceGroup.BottomPagerRow.Focus();

    }
    protected void gvDeviceGroup_OnSorting(object sender, GridViewSortEventArgs e)
    {

        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["dtFilter"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        ViewState["dtFilter"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvDeviceGroup, dt, "", "");

        AllPageCode();
        gvDeviceGroup.HeaderRow.Focus();

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {

            int b = 0;

            if (editid.Value == "")
            {
                b = ObjDeviceGroup.InsertHeaderRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LOcId"].ToString(), txtGroupName.Text.Trim(), txtGroupNameLOcal.Text.Trim(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);


                if (b != 0)
                {

                    foreach (ListItem li in chkDeviceList.Items)
                    {
                        if (li.Selected)
                        {
                            ObjDeviceGroup.InsertDetailRecord(b.ToString(), li.Value, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }


                    DisplayMessage("Record Saved", "green");
                }
                else
                {
                    DisplayMessage("Record Not Saved");
                }
            }
            else
            {
                b = ObjDeviceGroup.UpdateHeaderRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LOcId"].ToString(), editid.Value.ToString(), txtGroupName.Text.Trim(), txtGroupNameLOcal.Text.Trim(), true.ToString(), DateTime.Now.ToString(), ref trns);
                if (b != 0)
                {
                    ObjDeviceGroup.DeleteDetailRecord(editid.Value, ref trns);

                    foreach (ListItem li in chkDeviceList.Items)
                    {
                        if (li.Selected)
                        {
                            ObjDeviceGroup.InsertDetailRecord(editid.Value, li.Value, true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
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
            if (editid.Value != "")
            {
                btnList_Click(null, null);
            }
            FillGrid();
            Reset();
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
            return;

        }

    }

    //protected void txtEUnitName_TextChanged(object sender, EventArgs e)
    //{
    //    if (txtEUnitName.Text != "")
    //    {
    //        DataTable dtUnit = ObjInvUnitMaster.GetUnitMasterAll(StrCompId.ToString());
    //        dtUnit = new DataView(dtUnit, "unit_Name='" + txtEUnitName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
    //        if (dtUnit.Rows.Count > 0)
    //        {
    //            if (dtUnit.Rows[0]["Unit_Id"].ToString() != editid.Value)
    //            {
    //                if (Convert.ToBoolean(dtUnit.Rows[0]["IsActive"].ToString()))
    //                {
    //                    DisplayMessage("Unit Name Already Exists");
    //                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtEUnitName);


    //                }
    //                else
    //                {

    //                    DisplayMessage("Unit Name Already Exists :- Go to Bin Tab");

    //                }
    //                txtEUnitName.Text = "";
    //                txtEUnitName.Focus();
    //            }

    //        }
    //        else
    //        {
    //            txtlUnitName.Focus();
    //        }
    //    }
    //}

    #region Bin Section
    protected void btnbinbind_Click(object sender, ImageClickEventArgs e)
    {
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

            DataTable dtCust = (DataTable)Session["dtbinGroup"];

            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilterGroup"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvDeviceGroupBin, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {
                //imgBtnRestore.Visible = false;
                //ImgbtnSelectAll.Visible = false;
            }
            else
            {
                AllPageCode();
            }
        }
        txtbinValue.Focus();
    }
    protected void btnbinRefresh_Click(object sender, ImageClickEventArgs e)
    {
        FillGrid();
        FillGridBin();
        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }
    protected void gvDeviceGroupBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDeviceGroupBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilterGroup"];
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvDeviceGroupBin, dt, "", "");


        AllPageCode();

        string temp = string.Empty;
        bool isselcted;

        for (int i = 0; i < gvDeviceGroupBin.Rows.Count; i++)
        {
            Label lblconid = (Label)gvDeviceGroupBin.Rows[i].FindControl("lblGroupId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvDeviceGroupBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }

        gvDeviceGroupBin.BottomPagerRow.Focus();

    }
    protected void gvDeviceGroupBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtbinFilterGroup"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilterGroup"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvDeviceGroupBin, dt, "", "");
        AllPageCode();
        gvDeviceGroupBin.HeaderRow.Focus();

    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvDeviceGroupBin.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvDeviceGroupBin.Rows.Count; i++)
        {
            ((CheckBox)gvDeviceGroupBin.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvDeviceGroupBin.Rows[i].FindControl("lblGroupId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvDeviceGroupBin.Rows[i].FindControl("lblGroupId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvDeviceGroupBin.Rows[i].FindControl("lblGroupId"))).Text.Trim().ToString())
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
        ((CheckBox)gvDeviceGroupBin.HeaderRow.FindControl("chkgvSelectAll")).Focus();
    }
    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvDeviceGroupBin.Rows[index].FindControl("lblGroupId");
        if (((CheckBox)gvDeviceGroupBin.Rows[index].FindControl("chkgvSelect")).Checked)
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
        ((CheckBox)gvDeviceGroupBin.Rows[index].FindControl("chkgvSelect")).Focus();
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["dtbinFilterGroup"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Group_Id"]))
                {
                    lblSelectedRecord.Text += dr["Group_Id"] + ",";
                }
            }
            for (int i = 0; i < gvDeviceGroupBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)gvDeviceGroupBin.Rows[i].FindControl("lblGroupId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvDeviceGroupBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtbinFilterGroup"];
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvDeviceGroupBin, dtUnit1, "", "");

            AllPageCode();
            ViewState["Select"] = null;
        }
        ImgbtnSelectAll.Focus();
    }
    protected void imgBtnRestore_Click(object sender, ImageClickEventArgs e)
    {
        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = ObjDeviceGroup.UpdateHeaderRecordActiveStatus(lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString());
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
            int fleg = 0;
            foreach (GridViewRow Gvr in gvDeviceGroupBin.Rows)
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
        txtbinValue.Focus();
    }
    #endregion

    #endregion

    #region Auto Complete Function
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Att_DeviceGroupMaster ObjDeviceGroup = new Att_DeviceGroupMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(ObjDeviceGroup.GetHeaderAllTrueRecord(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), "Group_Name like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Group_Name"].ToString();
        }
        return txt;
    }
    #endregion

    #region User defined Function

    public void FillGrid()
    {
        DataTable dt = ObjDeviceGroup.GetHeaderAllTrueRecord(Session["CompId"].ToString(), Session["BRandId"].ToString(), Session["LocId"].ToString());

        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvDeviceGroup, dt, "", "");
        ViewState["dtFilter"] = dt;
       

        if (dt == null)
        {
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": 0";
        }
        else
        {
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        }
        AllPageCode();
    }
    public void Reset()
    {
        txtGroupName.Text = "";
        txtGroupNameLOcal.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        chkselectalldevice.Checked = false;
        foreach (ListItem li in chkDeviceList.Items)
        {
            li.Selected = false;
        }
        txtGroupName.Focus();
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
        dt = ObjDeviceGroup.GetHeaderAllFalseRecord(Session["CompId"].ToString(), Session["BRandId"].ToString(), Session["LocId"].ToString());
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvDeviceGroupBin, dt, "", "");
        Session["dtbinFilterGroup"] = dt;
        Session["dtbinGroup"] = dt;

        if (dt == null)
        {
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": 0";
        }
        else
        {
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        }



        AllPageCode();

    }

    #endregion


    protected void txtGroupName_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();

        if (txtGroupName.Text != "")
        {
            dt = ObjDeviceGroup.GetHeaderRecordAll(Session["CompId"].ToString(), Session["BRandId"].ToString(), Session["LocId"].ToString());

            dt = new DataView(dt, "Group_Name='" + txtGroupName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString()))
                {
                    DisplayMessage("Group name already exists");
                    txtGroupName.Text = "";
                    txtGroupName.Focus();
                    return;
                }
                else
                {
                    DisplayMessage("Group name already exists in bin section");
                    txtGroupName.Text = "";
                    txtGroupName.Focus();
                    return;
                }
            }
        }

    }

    protected void chkselectalldevice_CheckedChanged(object sender, EventArgs e)
    {

        foreach (ListItem li in chkDeviceList.Items)
        {
            li.Selected = chkselectalldevice.Checked;
        }

    }
}