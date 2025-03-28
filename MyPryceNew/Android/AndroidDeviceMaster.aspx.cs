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
using System.IO;

public partial class Android_AndroidDeviceMaster : BasePage
{
    #region defind Class Object
    Common cmn = null;
    SystemParameter objSys = null;
    Set_EmployeeGroup_Master objEmpGroup = null;
    Set_Group_Employee objGroupEmp = null;
    EmployeeMaster objEmp = null;
    And_DeviceMaster objDevice = null;
    Att_ShiftDescription objShiftDesc = null;
    Ser_UserTransfer objSer = null;
    Att_ScheduleMaster objSch = null;
    UserMaster objUser = null;
    And_Device_Group objDeviceGroup = null;
    Set_ApplicationParameter objAppParam = null;
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }


        //AllPageCode();
        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objEmpGroup = new Set_EmployeeGroup_Master(Session["DBConnection"].ToString());
        objGroupEmp = new Set_Group_Employee(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objDevice = new And_DeviceMaster(Session["DBConnection"].ToString());
        objShiftDesc = new Att_ShiftDescription(Session["DBConnection"].ToString());
        objSer = new Ser_UserTransfer(Session["DBConnection"].ToString());
        objSch = new Att_ScheduleMaster(Session["DBConnection"].ToString());
        objUser = new UserMaster(Session["DBConnection"].ToString());
        objDeviceGroup = new And_Device_Group(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            

            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Android/AndroidDeviceMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            txtValue.Focus();

            FillGridBin();
            FillGrid();
            pnlEmpDevice.Visible = false;

            btnList_Click(null, null);
            btnDelete.Visible = false;
        }
        Page.Title = objSys.GetSysTitle();
        //AllPageCode();
    }

    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bEdit;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }
    #endregion

    #region System defind Funcation

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string SaveGroupIds = string.Empty;
        string EmpIds = string.Empty;

        //for (int i = 0; i < lbxGroupSal.Items.Count; i++)
        //{
        //    if (lbxGroupSal.Items[i].Selected)
        //    {
        //        objDeviceGroup.DeleteDeviceGroupMaster(Session["CompId"].ToString(), editid.Value);


        //        SaveGroupIds += lbxGroupSal.Items[i].Value + ",";

        //    }

        //}
        DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());

        dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + SaveGroupIds.Substring(0, SaveGroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

        for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
        {

            objDevice.DeleteEmployeeDeviceMasterByEmpId(Session["CompId"].ToString(), editid.Value, dtEmpInGroup.Rows[i]["Emp_Id"].ToString());




        }

        for (int i = 0; i < lbxGroupSal.Items.Count; i++)
        {
            lbxGroupSal.Items[i].Selected = false;

        }
        DisplayMessage("Record Deleted");

    }

    protected void txtDeviceName_OnTextChanged(object sender, EventArgs e)
    {

        if (editid.Value == "")
        {
            DataTable dt = objDevice.GetAndroidDeviceMasterByDeviceName(Session["CompId"].ToString().ToString(), txtDeviceName.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                txtDeviceName.Text = "";
                DisplayMessage("Device Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeviceName);
                return;
            }
            DataTable dt1 = objDevice.GetAndroidDeviceMasterInactive(Session["CompId"].ToString().ToString());
            dt1 = new DataView(dt1, "Device_Name='" + txtDeviceName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtDeviceName.Text = "";
                DisplayMessage("Device Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeviceName);
                return;
            }

        }
        else
        {
            DataTable dtTemp = objDevice.GetAndroidDeviceMasterById(Session["CompId"].ToString().ToString(), editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Device_Name"].ToString() != txtDeviceName.Text)
                {
                    DataTable dt = objDevice.GetAndroidDeviceMaster(Session["CompId"].ToString().ToString());
                    dt = new DataView(dt, "Device_Name='" + txtDeviceName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtDeviceName.Text = "";
                        DisplayMessage("Device Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeviceName);
                        return;
                    }
                    DataTable dt1 = objDevice.GetAndroidDeviceMaster(Session["CompId"].ToString().ToString());
                    dt1 = new DataView(dt1, "Device_Name='" + txtDeviceName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtDeviceName.Text = "";
                        DisplayMessage("Device Name Already Exists - Go to Bin Tab");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeviceName);
                        return;
                    }
                }
            }

        }
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        txtValue.Focus();
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = true;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;



    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        txtDeviceName.Focus();
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = false;
        PnlNewEdit.Visible = true;
        PnlBin.Visible = false;
        txtDeviceId.Text = objDevice.GetDeviceId().ToString();
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtbinValue.Focus();
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlNewEdit.Visible = false;
        PnlBin.Visible = true;
        PnlList.Visible = false;
        FillGridBin();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        FillGridBin();

        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
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
            DataTable dtCust = (DataTable)Session["Device"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Android"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            gvDevice.DataSource = view.ToTable();
            gvDevice.DataBind();
            //AllPageCode();


        }
        txtValue.Focus();

    }
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
            else if (ddlbinOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + txtbinValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + txtbinValue.Text + "%'";
            }
            DataTable dtCust = (DataTable)Session["dtbinDevice"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            gvDeviceBin.DataSource = view.ToTable();
            gvDeviceBin.DataBind();


            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
                
            }
            else
            {
                //AllPageCode();
            }
        }
        txtbinValue.Focus();
    }



    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        FillGrid();
        FillGridBin();

        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
    }

    protected void gvDeviceBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        gvDeviceBin.PageIndex = e.NewPageIndex;
        if (HDFSortbin.Value == "")
            FillGridBin();
        else
        {
            DataTable dt = (DataTable)Session["dtbinFilter"];
            gvDeviceBin.DataSource = dt;
            gvDeviceBin.DataBind();
            //AllPageCode();
        }
        string temp = string.Empty;
        bool isselcted;

        for (int i = 0; i < gvDeviceBin.Rows.Count; i++)
        {
            Label lblconid = (Label)gvDeviceBin.Rows[i].FindControl("lblDeviceId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvDeviceBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }

    }
    protected void gvDeviceBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objDevice.GetAndroidDeviceMasterInactive(Session["CompId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        gvDeviceBin.DataSource = dt;
        gvDeviceBin.DataBind();
        //AllPageCode();

    }
    protected void ImgbtnSelectAll_Clickary(object sender, ImageClickEventArgs e)
    {
        DataTable dtProduct = (DataTable)Session["dtEmp"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtProduct.Rows)
            {

            }

        }
        else
        {
            DataTable dtProduct1 = (DataTable)Session["dtEmp"];
            ViewState["Select"] = null;
        }



    }

    //protected void chkgvEmpSelectAll_CheckedChanged(object sender, EventArgs e)
    //{
    //    CheckBox chkSelAll = ((CheckBox)gvEmployee.HeaderRow.FindControl("chkgvSelectAll"));
    //    for (int i = 0; i < gvEmployee.Rows.Count; i++)
    //    {
    //        ((CheckBox)gvEmployee.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
    //        if (chkSelAll.Checked)
    //        {
    //            if (!lblSelectRecord.Text.Split(',').Contains(((Label)(gvEmployee.Rows[i].FindControl("lblEmpId"))).Text.Trim().ToString()))
    //            {
    //                lblSelectRecord.Text += ((Label)(gvEmployee.Rows[i].FindControl("lblEmpId"))).Text.Trim().ToString() + ",";
    //            }
    //        }
    //        else
    //        {
    //            string temp = string.Empty;
    //            string[] split = lblSelectRecord.Text.Split(',');
    //            foreach (string item in split)
    //            {
    //                if (item != ((Label)(gvEmployee.Rows[i].FindControl("lblEmpId"))).Text.Trim().ToString())
    //                {
    //                    if (item != "")
    //                    {
    //                        temp += item + ",";
    //                    }
    //                }
    //            }
    //            lblSelectRecord.Text = temp;
    //        }
    //    }

    //    DataTable dtEmp1 = objEmp.GetEmployeeMaster(Session["CompId"].ToString());

    //    dtEmp1 = new DataView(dtEmp1, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

    //    if (Session["SessionDepId"] != null)
    //    {

    //        dtEmp1 = new DataView(dtEmp1, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

    //    }
    //    try
    //    {

    //        dtEmp1 = new DataView(dtEmp1, "Emp_Id in (" + lblSelectRecord.Text + ")", "", DataViewRowState.CurrentRows).ToTable();
    //    }
    //    catch
    //    {
    //        dtEmp1 = new DataTable();
    //    }


    //}
    //protected void chkgvEmpSelect_CheckedChanged(object sender, EventArgs e)
    //{
    //    string empidlist = string.Empty;
    //    string temp = string.Empty;
    //    int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
    //    Label lb = (Label)gvEmployee.Rows[index].FindControl("lblEmpId");
    //    if (((CheckBox)gvEmployee.Rows[index].FindControl("chkgvSelect")).Checked)
    //    {
    //        empidlist += lb.Text.Trim().ToString() + ",";
    //        lblSelectRecord.Text += empidlist;

    //    }

    //    else
    //    {

    //        empidlist += lb.Text.ToString().Trim();
    //        lblSelectRecord.Text += empidlist;
    //        string[] split = lblSelectRecord.Text.Split(',');
    //        foreach (string item in split)
    //        {
    //            if (item != empidlist)
    //            {
    //                if (item != "")
    //                {
    //                    if (item != "")
    //                    {
    //                        temp += item + ",";
    //                    }
    //                }
    //            }
    //        }
    //        lblSelectRecord.Text = temp;
    //    }









    //}


    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvDeviceBin.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvDeviceBin.Rows.Count; i++)
        {
            ((CheckBox)gvDeviceBin.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvDeviceBin.Rows[i].FindControl("lblDeviceId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvDeviceBin.Rows[i].FindControl("lblDeviceId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvDeviceBin.Rows[i].FindControl("lblDeviceId"))).Text.Trim().ToString())
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
        Label lb = (Label)gvDeviceBin.Rows[index].FindControl("lblDeviceId");
        if (((CheckBox)gvDeviceBin.Rows[index].FindControl("chkgvSelect")).Checked)
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

    protected void btnarybind_Click1(object sender, ImageClickEventArgs e)
    {


    }
    protected void lbxGroupSal_SelectedIndexChanged(object sender, EventArgs e)
    {
        string GroupIds = string.Empty;
        string EmpIds = string.Empty;
        for (int i = 0; i < lbxGroupSal.Items.Count; i++)
        {
            if (lbxGroupSal.Items[i].Selected == true)
            {
                GroupIds += lbxGroupSal.Items[i].Value + ",";

            }

        }
        if (GroupIds != "")
        {
            DataTable dtEmp = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString());

            dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            DataTable dtEmpInGroup = objGroupEmp.GetGroup_EmployeeById(Session["CompId"].ToString());

            dtEmpInGroup = new DataView(dtEmpInGroup, "Group_Id in(" + GroupIds.Substring(0, GroupIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

            for (int i = 0; i < dtEmpInGroup.Rows.Count; i++)
            {
                if (!EmpIds.Split(',').Contains(dtEmpInGroup.Rows[i]["Emp_Id"].ToString()))
                {
                    EmpIds += dtEmpInGroup.Rows[i]["Emp_Id"].ToString() + ",";
                }
            }
            if (EmpIds != "")
            {
                dtEmp = new DataView(dtEmp, "Emp_Id in(" + EmpIds.Substring(0, EmpIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                dtEmp = new DataTable();
            }
            if (dtEmp.Rows.Count > 0)
            {
                Session["dtEmp4"] = dtEmp;
                gvEmployeeSal.DataSource = dtEmp;
                gvEmployeeSal.DataBind();

            }
            else
            {
                Session["dtEmp4"] = null;
                gvEmployeeSal.DataSource = dtEmp;
                gvEmployeeSal.DataBind();
            }
        }
        else
        {
            gvEmployeeSal.DataSource = null;
            gvEmployeeSal.DataBind();

        }
    }

    protected void EmpGroupSal_CheckedChanged(object sender, EventArgs e)
    {


        pnlGroupSal.Visible = false;
        btnDelete.Visible = false;
        string GroupIds = string.Empty;
        string EmpIds = string.Empty;
        DataTable dtEmp = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString());

        dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();




        if (dtEmp.Rows.Count > 0)
        {
            Session["dtEmp"] = dtEmp;
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";



            DataTable dtEmpInGrp = objDevice.GetEmployeeDeviceMasterById(Session["CompId"].ToString(), editid.Value);



            if (dtEmpInGrp.Rows.Count > 0)
            {

            }
        }

    }

    protected void gvEmployeeSal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployeeSal.PageIndex = e.NewPageIndex;
        gvEmployeeSal.DataSource = (DataTable)Session["dtEmp4"];
        gvEmployeeSal.DataBind();
        //AllPageCode();
    }
    protected void btnaryRefresh_Click1(object sender, ImageClickEventArgs e)
    {

    }




    protected void btnSaveEmp_Click(object sender, EventArgs e)
    {
        int b = 0;

        // Modified By Nitin jain on 20/11/2014 to select grid values without Page Refresh

        ArrayList userdetails = new ArrayList();
        if (Session["CHECKED_ITEMS"] != null)
        {
            userdetails = (ArrayList)Session["CHECKED_ITEMS"];
            if (userdetails.Count == 0)
            {
                DisplayMessage("Select Employee First");
                return;
            }
        }
        else
        {
            DisplayMessage("Select Employee First");
            return;
        }
        for (int i = 0; i < userdetails.Count; i++)
        {



        }



        if (b != 0)
        {
            DisplayMessage("Record Saved", "green");
            Reset();
            btnList_Click(null, null);
            pnlEmpDevice.Visible = false;
        }
    }

    protected void btnResetEmp_Click(object sender, EventArgs e)
    {

        DataTable dtGroup = objEmpGroup.GetEmployeeGroup_Master(Session["CompId"].ToString());
        dtGroup = new DataView(dtGroup, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtGroup.Rows.Count > 0)
        {
            lbxGroupSal.DataSource = dtGroup;
            lbxGroupSal.DataTextField = "Group_Name";
            lbxGroupSal.DataValueField = "Group_Id";

            lbxGroupSal.DataBind();

        }

        DataTable dtEmp = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString());

        dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (Session["SessionDepId"] != null)
        {
            dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

        }
        if (dtEmp.Rows.Count > 0)
        {
            Session["dtEmp"] = dtEmp;

        }
        EmpGroupSal_CheckedChanged(null, null);





    }


    protected void btnCancelEmp_Click(object sender, EventArgs e)
    {
        pnlEmpDevice.Visible = false;
        PnlNewEdit.Visible = true;



    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        int b = 0;



        if (txtDeviceName.Text == "")
        {
            DisplayMessage("Enter Device Name");
            txtDeviceName.Focus();
            return;
        }


        if (editid.Value == "")
        {

            DataTable dt1 = objDevice.GetAndroidDeviceMaster(Session["CompId"].ToString());

            DataTable dt2 = new DataView(dt1, "Device_Name='" + txtDeviceName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt2.Rows.Count > 0)
            {
                DisplayMessage("Device Name Already Exists");
                txtDeviceName.Focus();
                return;

            }




            b = objDevice.InsertAndroidDeviceMaster(Session["CompId"].ToString(), txtDeviceName.Text, Session["BrandId"].ToString(), Session["LocId"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            if (b != 0)
            {
                editid.Value = b.ToString();
                DisplayMessage("Record Saved", "green");
                FillGrid();


                pnlEmpDevice.Visible = true;
                PnlNewEdit.Visible = false;
                Reset();
                btnList_Click(null, null);
                Panel1.Visible = false;
                EmpGroupSal_CheckedChanged(null, null);

            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }
        else
        {
            string DeviceTypeName = string.Empty;
            DataTable dt1 = objDevice.GetAndroidDeviceMaster(Session["CompId"].ToString());
            try
            {
                DeviceTypeName = new DataView(dt1, "Device_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Device_Name"].ToString();
            }
            catch
            {
                DeviceTypeName = "";
            }
            dt1 = new DataView(dt1, "Device_Name='" + txtDeviceName.Text + "' and Device_Name<>'" + DeviceTypeName + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("Device Name Already Exists");
                txtDeviceName.Focus();
                return;

            }






            b = objDevice.UpdateAndroidDeviceMaster(editid.Value, Session["CompId"].ToString(), txtDeviceName.Text, Session["BrandId"].ToString(), Session["LocId"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            if (b != 0)
            {
                //btnList_Click(null, null);
                DisplayMessage("Record Updated", "green");
                //Reset();
                FillGrid();
                pnlEmpDevice.Visible = true;
                PnlNewEdit.Visible = false;
                EmpGroupSal_CheckedChanged(null, null);
                Reset();
                btnList_Click(null, null);
                Panel1.Visible = false;
            }
            else
            {
                DisplayMessage("Record Not Updated");
            }

        }
    }

    protected void lnkConnect_Click(object sender, ImageClickEventArgs e)
    {


        int errorcode = 0;
        int index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
        string port = gvDevice.DataKeys[index]["Port"].ToString();
        string IP = gvDevice.DataKeys[index]["IP_Address"].ToString();
        string DeviceId = gvDevice.DataKeys[index]["Device_Id"].ToString();

        //Device_Operation_Lan objDeviceOp = new Device_Operation_Lan(Session["DBConnection"].ToString());


        //bool b = false;
        //b = objDeviceOp.Device_Connection(IP, Convert.ToInt32(port), 0);


        //if (b == true)
        //{

        //    DisplayMessage("Device Is Functional");



        //}
        //else
        //{



        //    DisplayMessage("Unable to connect the device");


        //}
    }

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();




        DataTable dt = objDevice.GetAndroidDeviceMasterById(Session["CompId"].ToString(), editid.Value);
        if (dt.Rows.Count > 0)
        {

            txtDeviceName.Text = dt.Rows[0]["Device_Name"].ToString();






            btnNew_Click(null, null);
            txtDeviceId.Text = dt.Rows[0]["Device_Id"].ToString();

            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);

        }



    }

    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {




        int b = 0;
        b = objDevice.DeleteAndroidDeviceMaster(Session["CompId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");

            FillGridBin();
            FillGrid();
            Reset();
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
    }
    protected void gvDevice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDevice.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Android"];
        gvDevice.DataSource = dt;
        gvDevice.DataBind();
        //AllPageCode();

    }
    protected void gvDevice_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Android"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Android"] = dt;
        gvDevice.DataSource = dt;
        gvDevice.DataBind();
        //AllPageCode();
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
    }


    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["dtbinFilter"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Device_Id"]))
                {
                    lblSelectedRecord.Text += dr["Device_Id"] + ",";
                }
            }
            for (int i = 0; i < gvDeviceBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)gvDeviceBin.Rows[i].FindControl("lblDeviceId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvDeviceBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtBinFilter"];
            gvDeviceBin.DataSource = dtUnit1;
            gvDeviceBin.DataBind();
            ViewState["Select"] = null;
        }
    }


    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        Session["CHECKED_ITEMS"] = string.Empty;
        // Modified By Nitin jain on 20/11/2014 to select grid values without Page Refresh
        SaveCheckedValuesemplog();
        ArrayList userdetails = new ArrayList();
        if (Session["CHECKED_ITEMS"] != null)
        {
            userdetails = (ArrayList)Session["CHECKED_ITEMS"];
            if (userdetails.Count == 0)
            {
                DisplayMessage("Select Employee First");
                return;
            }
        }
        else
        {
            DisplayMessage("Select Employee First");
            return;
        }


        for (int i = 0; i < userdetails.Count; i++)
        {

            lblSelectedRecord.Text += userdetails[i].ToString() + ",";

        }
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = objDevice.DeleteAndroidDeviceMaster(Session["CompId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
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
            foreach (GridViewRow Gvr in gvDeviceBin.Rows)
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

    #endregion

    #region User Defined Function
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }

    public void FillGrid()
    {

        DataTable dtEmpDev = new DataTable();
        dtEmpDev = objUser.GetUserMasterByUserIdByCompId(Session["UserId"].ToString(), "2",HttpContext.Current.Session["CompId"].ToString());
        string DeviceIds = string.Empty;
        if (Session["UserId"].ToString().ToLower().Trim() != "superadmin")
        {
            if (dtEmpDev.Rows.Count > 0)
            {
                for (int i = 0; i < dtEmpDev.Rows.Count; i++)
                {
                    if (dtEmpDev.Rows[i]["Field1"].ToString() != "")
                    {
                        DeviceIds += dtEmpDev.Rows[i]["Field1"].ToString() + ",";
                    }
                }
            }

        }


        DataTable dt = objDevice.GetAndroidDeviceMaster(Session["CompId"].ToString());
        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (DeviceIds != "")
        {
            try
            {
                dt = new DataView(dt, "Device_Id in(" + DeviceIds + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }
        }
        else
        {
            if (Session["UserId"].ToString().ToLower().Trim() != "superadmin")
            {
                dt = new DataTable();
            }
        }
        gvDevice.DataSource = dt;
        gvDevice.DataBind();
        //AllPageCode();
        Session["dtFilter_Android"] = dt;
        Session["Device"] = dt;

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

    }
    public void FillGridBin()
    {

        DataTable dtEmpDev = new DataTable();
        dtEmpDev = objUser.GetUserMasterByUserIdByCompId(Session["UserId"].ToString(), "2",HttpContext.Current.Session["CompId"].ToString());
        string DeviceIds = string.Empty;
        if (Session["UserId"].ToString().ToLower().Trim() != "superadmin")
        {
            if (dtEmpDev.Rows.Count > 0)
            {
                for (int i = 0; i < dtEmpDev.Rows.Count; i++)
                {
                    if (dtEmpDev.Rows[i]["Field1"].ToString() != "")
                    {
                        DeviceIds += dtEmpDev.Rows[i]["Field1"].ToString() + ",";
                    }
                }
            }

        }

        DataTable dt = new DataTable();
        dt = objDevice.GetAndroidDeviceMasterInactive(Session["CompId"].ToString());
        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (DeviceIds != "")
        {
            try
            {
                dt = new DataView(dt, "Device_Id in(" + DeviceIds + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }
        }
        else
        {
            if (Session["UserId"].ToString().ToLower().Trim() != "superadmin")
            {
                dt = new DataTable();
            }
        }








        gvDeviceBin.DataSource = dt;
        gvDeviceBin.DataBind();


        Session["dtbinFilter"] = dt;
        Session["dtbinDevice"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        if (dt.Rows.Count == 0)
        {
            imgBtnRestore.Visible = false;
        }
        else
        {

            //AllPageCode();
        }

    }
    public void Reset()
    {



        txtDeviceName.Text = "";


        txtDeviceId.Text = objDevice.GetDeviceId().ToString();



        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;



    }



    private int GetMinuteDiff(string greatertime, string lesstime)
    {

        if (greatertime == "__:__:__" || greatertime == "")
        {
            return 0;
        }
        if (lesstime == "__:__:__" || lesstime == "")
        {
            return 0;
        }
        int retval = 0;
        int actTimeHour = Convert.ToInt32(greatertime.Split(':')[0]);
        int ondutyhour = Convert.ToInt32(lesstime.Split(':')[0]);
        int actTimeMinute = Convert.ToInt32(greatertime.Split(':')[1]);
        int ondutyMinute = Convert.ToInt32(lesstime.Split(':')[1]);
        int totalActTimeMinute = actTimeHour * 60 + actTimeMinute;
        int totalOnDutyTimeMinute = ondutyhour * 60 + ondutyMinute;
        if (totalActTimeMinute - totalOnDutyTimeMinute < 0)
        {
            retval = 1440 + (totalActTimeMinute - totalOnDutyTimeMinute);
        }
        else
        {
            retval = (totalActTimeMinute - totalOnDutyTimeMinute);
        }
        return retval;
    }
    #endregion

    #region Auto Complete Function

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListIpAddress(string prefixText, int count, string contextKey)
    {
        Att_DeviceMaster objDevice = new Att_DeviceMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objDevice.GetDeviceMaster(HttpContext.Current.Session["CompId"].ToString()), "IP_Address like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["IP_Address"].ToString();
        }
        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDeviceName(string prefixText, int count, string contextKey)
    {
        And_DeviceMaster objAtt_Device = new And_DeviceMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objAtt_Device.GetAndroidDeviceMaster(HttpContext.Current.Session["CompId"].ToString()), "Device_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Device_Name"].ToString();
        }
        return txt;
    }

    #endregion
    //
    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvDeviceBin.Rows)
        {
            index = (int)gvDeviceBin.DataKeys[gvrow.RowIndex].Value;
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
            foreach (GridViewRow gvrow in gvDeviceBin.Rows)
            {
                int index = (int)gvDeviceBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    //
}
