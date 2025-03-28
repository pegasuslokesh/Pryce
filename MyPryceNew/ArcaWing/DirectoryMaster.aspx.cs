using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class ArcaWing_DirectoryMaster : BasePage
{
    Common cmn = null;
    Arc_Directory_Master objDir = null;
    Arc_FileTransaction objFile = null;
    SystemParameter objSys = null;
    EmployeeMaster objEmp = null;
    Arc_Directory_Privileges ObjDirPrivileges = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objDir = new Arc_Directory_Master(Session["DBConnection"].ToString());
        objFile = new Arc_FileTransaction(Session["DBConnection"].ToString());
        ObjDirPrivileges = new Arc_Directory_Privileges(Session["DBConnection"].ToString());
        //AllPageCode();
        if (!IsPostBack)
        {
           
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../ArcaWing/DirectoryMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            txtValue.Focus();

            FillGrid();
            FillEmployeeList();
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
    }
    public void FillGrid()
    {
        string Directoryid = "0";
        DataTable dt = objDir.getDirectoryMaster(Session["CompId"].ToString(), Directoryid);


        dt = new DataView(dt, " Field1='0'", "", DataViewRowState.CurrentRows).ToTable();


        if (Session["EmpId"].ToString() != "0")
        {
            try
            {
                dt = new DataView(dt, "CreatedBy='" + Session["UserId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }

        //get only user directory 




        //dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvDirMaster, dt, "", "");
        //AllPageCode();
        Session["dtFilter_Dir_Master"] = dt;
        Session["Dir"] = dt;

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
    }

    public void FillGridBin()
    {
        string Directoryid = "0";

        DataTable dt = new DataTable();
        dt = objDir.GetDirectoryMasterInActive(Session["CompId"].ToString(), Directoryid);
        if (Session["EmpId"].ToString() != "0")
        {
            try
            {
                dt = new DataView(dt, "CreatedBy='" + Session["UserId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }
        //dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvDirMasterBin, dt, "", "");

        Session["dtbinFilter"] = dt;
        Session["dtbinDir"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        if (dt.Rows.Count == 0)
        {
            //imgBtnRestore.Visible = false;
            //ImgbtnSelectAll.Visible = false;
        }
        else
        {
            //AllPageCode();
        }
    }
    protected void gvDirMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDirMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Dir_Master"];
        if (Session["EmpId"].ToString() != "0")
        {
            try
            {
                dt = new DataView(dt, "CreatedBy='" + Session["UserId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvDirMaster, dt, "", "");
        //AllPageCode();
    }
    protected void gvDirMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Dir_Master"];
        if (Session["EmpId"].ToString() != "0")
        {
            try
            {
                dt = new DataView(dt, "CreatedBy='" + Session["UserId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Dir_Master"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvDirMaster, dt, "", "");
        //AllPageCode();
    }
    private void SaveCheckedValuesemplog()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvDirMasterBin.Rows)
        {
            index = (int)gvDirMasterBin.DataKeys[gvrow.RowIndex].Value;
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
            foreach (GridViewRow gvrow in gvDirMasterBin.Rows)
            {
                int index = (int)gvDirMasterBin.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }

    protected void gvDirMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValuesemplog();
        gvDirMasterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvDirMasterBin, dt, "", "");
        //AllPageCode();
        PopulateCheckedValuesemplog();
    }
    protected void gvDirMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        Session["CHECKED_ITEMS"] = null;
        string Documentid = "0";
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objDir.GetDirectoryMasterInActive(Session["CompId"].ToString(), Documentid);
        //dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
        if (Session["EmpId"].ToString() != "0")
        {
            try
            {
                dt = new DataView(dt, "CreatedBy='" + Session["UserId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }

        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvDirMasterBin, dt, "", "");
        //AllPageCode();
    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvDirMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        foreach (GridViewRow gr in gvDirMasterBin.Rows)
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


    //protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    //{
    //    CheckBox chkSelAll = ((CheckBox)gvDirMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
    //    for (int i = 0; i < gvDirMasterBin.Rows.Count; i++)
    //    {
    //        ((CheckBox)gvDirMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
    //        if (chkSelAll.Checked)
    //        {
    //            if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvDirMasterBin.Rows[i].FindControl("lblDirId"))).Text.Trim().ToString()))
    //            {
    //                lblSelectedRecord.Text += ((Label)(gvDirMasterBin.Rows[i].FindControl("lblDirId"))).Text.Trim().ToString() + ",";
    //            }
    //        }
    //        else
    //        {
    //            string temp = string.Empty;
    //            string[] split = lblSelectedRecord.Text.Split(',');
    //            foreach (string item in split)
    //            {
    //                if (item != ((Label)(gvDirMasterBin.Rows[i].FindControl("lblDirId"))).Text.Trim().ToString())
    //                {
    //                    if (item != "")
    //                    {
    //                        temp += item + ",";
    //                    }
    //                }
    //            }
    //            lblSelectedRecord.Text = temp;
    //        }
    //    }
    //}
    //protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    //{
    //    string empidlist = string.Empty;
    //    string temp = string.Empty;
    //    int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
    //    Label lb = (Label)gvDirMasterBin.Rows[index].FindControl("lblDirId");
    //    if (((CheckBox)gvDirMasterBin.Rows[index].FindControl("chkgvSelect")).Checked)
    //    {
    //        empidlist += lb.Text.Trim().ToString() + ",";
    //        lblSelectedRecord.Text += empidlist;
    //    }
    //    else
    //    {
    //        empidlist += lb.Text.ToString().Trim();
    //        lblSelectedRecord.Text += empidlist;
    //        string[] split = lblSelectedRecord.Text.Split(',');
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
    //        lblSelectedRecord.Text = temp;
    //    }
    //}
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        int index = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;

        DataTable dt = objDir.GetDirectoryMaster_By_DirectoryId(Session["CompId"].ToString(), e.CommandArgument.ToString());
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["Field1"].ToString() == "1")
            {
                DisplayMessage("You Can not edit System Generated Directory");
                gvDirMaster.Rows[index].Cells[0].Focus();
                return;
            }

            editid.Value = e.CommandArgument.ToString();
            try
            {
                txtDirectName.Text = dt.Rows[0]["Directory_Name"].ToString();
                txtUserName.Text = dt.Rows[0]["Field3"].ToString();
            }
            catch
            {

            }

            txtDirectName.Enabled = false;
            txtparanetFolderName.Enabled = false;

            FillEmployeeList();
            DataTable dtempList = ObjDirPrivileges.getRecordbyDirectoryId(editid.Value);
            for (int i = 0; i < dtempList.Rows.Count; i++)
            {
                ListItem li = new ListItem();
                li.Value = dtempList.Rows[i]["Emp_Id"].ToString();
                li.Text = dtempList.Rows[i]["EmpDetailInfo"].ToString();
                if (lstEmployeeCategory.Items.FindByValue(li.Value) != null)
                {
                    lstEmployeeCategory.Items.Remove(li);
                }
                if (lstSelectEmployeeCategory.Items.FindByValue(li.Value) == null)
                {
                    lstSelectEmployeeCategory.Items.Add(li);
                }

            }
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
        txtUserName.Focus();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int index = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;

        DataTable dt = objDir.GetDirectoryMaster_By_DirectoryId(Session["CompId"].ToString(), e.CommandArgument.ToString());
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["Field1"].ToString() == "1")
            {
                DisplayMessage("You Can not delete System Generated Directory");
                gvDirMaster.Rows[index].Cells[1].Focus();
                return;
            }
            else
            {
                DataTable dtDirectory = objDir.getDirectoryMaster(Session["CompId"].ToString(), "0");
                try
                {
                    string NewDirectory = string.Empty;
                    NewDirectory = dt.Rows[0]["Directory_Name"].ToString() + "/";
                    dtDirectory = new DataView(dtDirectory, "Directory_Name like '" + NewDirectory + "%' and createdBy='" + dt.Rows[0]["CreatedBy"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (dtDirectory.Rows.Count > 0)
                    {
                        DisplayMessage("Child Is Exists of this Directory , You Can Not Delete");
                        return;
                    }
                }
                catch
                {
                }


            }
        }



        DataTable dtFile = objFile.Get_FileTransactionByCompanyId(Session["CompId"].ToString());
        {
            try
            {
                dtFile = new DataView(dtFile, "Directory_Id=" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }

        if (dtFile.Rows.Count > 0)
        {
            DisplayMessage("Directory In Use , You Can Not Delete");
            gvDirMaster.Rows[index].Cells[1].Focus();
            return;
        }


        int b = 0;
        b = objDir.DeleteDirectoryMaster(Session["CompId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    public void Reset()
    {
        txtDirectName.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        txtparanetFolderName.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtUserName.Text = "";
        txtUserName.Focus();
        FillEmployeeList();
        txtDirectName.Enabled = true;
        txtparanetFolderName.Enabled = true;
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtbinValue.Focus();
        FillGridBin();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
        txtValue.Focus();
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
            DataTable dtCust = (DataTable)Session["Dir"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Dir_Master"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvDirMaster, view.ToTable(), "", "");
            btnRefresh.Focus();
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
            DataTable dtCust = (DataTable)Session["dtbinDir"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvDirMasterBin, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {
                //imgBtnRestore.Visible = false;
                //ImgbtnSelectAll.Visible = false;
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
        FillGridBin();
        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        txtbinValue.Focus();
        lblSelectedRecord.Text = "";
    }

    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        ArrayList userdetail = new ArrayList();
        if (gvDirMasterBin.Rows.Count > 0)
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
                            b = objDir.DeleteDirectoryMaster(Session["CompId"].ToString(), userdetail[j].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

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
                    foreach (GridViewRow Gvr in gvDirMasterBin.Rows)
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
                gvDirMasterBin.Focus();
                return;
            }
        }
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["dtbinFilter"];
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

                if (!userdetails.Contains(dr["Id"]))
                {
                    userdetails.Add(dr["Id"]);
                }
            }
            foreach (GridViewRow GR in gvDirMasterBin.Rows)
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
            DataTable dtUnit1 = (DataTable)Session["dtBinFilter"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvDirMasterBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    //protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    //{

    //    DataTable dtUnit = (DataTable)Session["dtbinFilter"];

    //    if (ViewState["Select"] == null)
    //    {
    //        ViewState["Select"] = 1;
    //        foreach (DataRow dr in dtUnit.Rows)
    //        {
    //            if (!lblSelectedRecord.Text.Split(',').Contains(dr["Id"]))
    //            {
    //                lblSelectedRecord.Text += dr["Id"] + ",";
    //            }
    //        }
    //        for (int i = 0; i < gvDirMasterBin.Rows.Count; i++)
    //        {
    //            string[] split = lblSelectedRecord.Text.Split(',');
    //            Label lblconid = (Label)gvDirMasterBin.Rows[i].FindControl("lblDirId");
    //            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
    //            {
    //                if (lblSelectedRecord.Text.Split(',')[j] != "")
    //                {
    //                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
    //                    {
    //                        ((CheckBox)gvDirMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        lblSelectedRecord.Text = "";
    //        DataTable dtUnit1 = (DataTable)Session["dtBinFilter"];
    //        gvDirMasterBin.DataSource = dtUnit1;
    //        gvDirMasterBin.DataBind();
    //        ViewState["Select"] = null;
    //    }
    //}
    protected void txtparanetFolderName_OnTextChanged(object sender, EventArgs e)
    {
        if (txtparanetFolderName.Text != "")
        {
            DataTable dt = objDir.getDirectoryMaster(HttpContext.Current.Session["CompId"].ToString(), "0");
            try
            {
                dt = new DataView(dt, "Directory_Name='" + txtparanetFolderName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
            if (dt.Rows.Count == 0)
            {
                DisplayMessage("Select Parent Folder Name in Suggestion Only");
                txtparanetFolderName.Text = "";
                txtparanetFolderName.Focus();
                return;
            }


        }

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string FinalDirectory = string.Empty;
        int b = 0;
        if (txtDirectName.Text == "")
        {

            DisplayMessage("Enter Directory Name");
            txtDirectName.Focus();
            return;
        }
        if (txtparanetFolderName.Text != "")
        {
            FinalDirectory = txtparanetFolderName.Text + "/" + txtDirectName.Text;
        }
        else
        {
            FinalDirectory = txtDirectName.Text;
        }



        DataTable DtInactive = new DataTable();

        DtInactive = objDir.GetDirectoryMasterInActive(Session["CompId"].ToString(), "0");
        DtInactive = new DataView(DtInactive, "Directory_Name='" + FinalDirectory + "'", "", DataViewRowState.CurrentRows).ToTable();
        if (Session["EmpId"].ToString() != "0")
        {
            try
            {
                DtInactive = new DataView(DtInactive, "CreatedBy='" + Session["UserId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {

            }
        }
        if (DtInactive.Rows.Count > 0)
        {
            DisplayMessage("Already Exists - Go to bin section");
            txtparanetFolderName.Focus();

            return;
        }

        if (editid.Value == "")
        {
            DataTable dt = objDir.getDirectoryMaster(Session["CompId"].ToString(), "0");

            dt = new DataView(dt, "Directory_Name='" + FinalDirectory + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (Session["EmpId"].ToString() != "0")
            {
                try
                {
                    dt = new DataView(dt, "CreatedBy='" + Session["UserId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
            }
            if (dt.Rows.Count > 0)
            {
                DisplayMessage("Directory Name Already Exists");
                txtparanetFolderName.Focus();

                return;

            }


            b = objDir.InsertDirectorymaster(Session["CompId"].ToString(), FinalDirectory, "0", Session["BrandId"].ToString(), txtUserName.Text, "", Session["LocId"].ToString(), false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["EmpId"].ToString());

            if (b != 0)
            {
                foreach (ListItem li in lstSelectEmployeeCategory.Items)
                {

                    ObjDirPrivileges.InsertRecord(b.ToString(), li.Value);

                }
                string NewDirectory = Server.MapPath(Session["CompId"].ToString() + "/" + Session["UserId"].ToString() + "/" + FinalDirectory);

                int i = CreateDirectoryIfNotExist(NewDirectory);

                DisplayMessage("Record Saved", "green");
                FillGrid();
                Reset();

            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }
        else
        {
            //string Documentid = "0";
            DataTable dt = objDir.getDirectoryMaster(Session["CompId"].ToString(), "0");


            string DirectoryName = string.Empty;






            try
            {
                DirectoryName = (new DataView(dt, "Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Directory_Name"].ToString();
            }
            catch
            {
                DirectoryName = "";
            }
            dt = new DataView(dt, "Directory_Name='" + FinalDirectory + "' and Directory_Name<>'" + DirectoryName.ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
            if (Session["EmpId"].ToString() != "0")
            {
                try
                {
                    dt = new DataView(dt, "CreatedBy='" + Session["UserId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
            }
            if (dt.Rows.Count > 0)
            {
                DisplayMessage("Directory Name Already Exists");
                txtDirectName.Text = "";
                txtDirectName.Focus();
                return;

            }



            DataTable Dt = objDir.GetDirectoryMaster_By_DirectoryId(Session["CompId"].ToString(), editid.Value);
            if (Session["EmpId"].ToString().Trim() != "0")
            {
                try
                {
                    Dt = new DataView(Dt, "CreatedBy='" + Session["UserId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                catch
                {
                }
            }

            if (txtDirectName.Text != Dt.Rows[0]["Directory_Name"].ToString())
            {

                string Createdirectory = Server.MapPath(Session["CompId"].ToString() + "/" + txtDirectName.Text.Trim());
                string OLdDirectory = Server.MapPath(Session["CompId"].ToString() + "/" + Dt.Rows[0]["Directory_Name"].ToString());
                Directory.Move(OLdDirectory, Createdirectory);
            }

            b = objDir.UpdateDirectoryMaster(Session["CompId"].ToString(), editid.Value, txtDirectName.Text, "0", "0", txtUserName.Text, "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            ObjDirPrivileges.DeleteRecord(editid.Value);
            foreach (ListItem li in lstSelectEmployeeCategory.Items)
            {

                ObjDirPrivileges.InsertRecord(editid.Value, li.Value);

            }

            if (b != 0)
            {

                DisplayMessage("Record Updated", "green");
                Reset();
                FillGrid();
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
            }
            else
            {
                DisplayMessage("Record Not Updated");
            }
        }
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
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDirName(string prefixText, int count, string contextKey)
    {
        Arc_Directory_Master objdir = new Arc_Directory_Master(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objdir.getDirectoryMaster(HttpContext.Current.Session["CompId"].ToString(), "0");
        dt = new DataView(dt, "Directory_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Directory_Name"].ToString();
        }
        return txt;
    }
    public int CreateDirectoryIfNotExist(string NewDirectory)
    {
        int checkDirectory = 0;
        try
        {
            // Checking the existance of directory
            if (!Directory.Exists(NewDirectory))
            {
                //If No any such directory then creates the new one
                Directory.CreateDirectory(NewDirectory);
            }
            else
            {
                checkDirectory = 1;
            }
        }
        catch (IOException _err)
        {
            Response.Write(_err.Message);
        }
        return checkDirectory;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDirectoryName(string prefixText, int count, string contextKey)
    {
        Arc_Directory_Master objdir = new Arc_Directory_Master(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objdir.getDirectoryMaster(HttpContext.Current.Session["CompId"].ToString(), "0");
        DataTable dtFilter = new DataTable();
        dt = new DataView(dt, "Field1='0'", "", DataViewRowState.CurrentRows).ToTable();

        if (HttpContext.Current.Session["EmpId"].ToString().Trim() != "0")
        {
            try
            {
                dt = new DataView(dt, "CreatedBy='" + HttpContext.Current.Session["UserId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
            }
        }


        dtFilter = dt.Copy();

        dt = new DataView(dt, "Directory_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count == 0)
        {
            dt = dtFilter;
        }
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Directory_Name"].ToString();
        }
        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListUserName(string prefixText, int count, string contextKey)
    {
        Arc_Directory_Master objdir = new Arc_Directory_Master(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objdir.getDirectoryMaster(HttpContext.Current.Session["CompId"].ToString(), "0");
        DataTable dtFilter = new DataTable();

        dt = new DataView(dt, "Field3 like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable(true, "Field3");



        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Field3"].ToString();
        }
        return txt;
    }


    #region employeeListbox

    protected void btnPushAllCate_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstEmployeeCategory.Items)
        {
            lstSelectEmployeeCategory.Items.Add(li);


        }
        foreach (ListItem li in lstSelectEmployeeCategory.Items)
        {

            lstEmployeeCategory.Items.Remove(li);

        }
        btnPushAllCate.Focus();
    }
    protected void btnPullAllCate_Click(object sender, EventArgs e)
    {

        foreach (ListItem li in lstSelectEmployeeCategory.Items)
        {

            lstEmployeeCategory.Items.Add(li);


        }
        foreach (ListItem li in lstEmployeeCategory.Items)
        {

            lstSelectEmployeeCategory.Items.Remove(li);

        }

        btnPullAllCate.Focus();
    }
    protected void btnPushCate_Click(object sender, EventArgs e)
    {
        if (lstEmployeeCategory.SelectedIndex >= 0)
        {

            foreach (ListItem li in lstEmployeeCategory.Items)
            {
                if (li.Selected)
                {
                    lstSelectEmployeeCategory.Items.Add(li);

                }
            }
            foreach (ListItem li in lstSelectEmployeeCategory.Items)
            {
                lstEmployeeCategory.Items.Remove(li);
            }
            lstSelectEmployeeCategory.SelectedIndex = -1;
        }
        btnPushCate.Focus();
    }
    protected void btnPullCate_Click(object sender, EventArgs e)
    {
        if (lstSelectEmployeeCategory.SelectedIndex >= 0)
        {

            foreach (ListItem li in lstSelectEmployeeCategory.Items)
            {
                if (li.Selected)
                {
                    lstEmployeeCategory.Items.Add(li);

                }
            }
            foreach (ListItem li in lstEmployeeCategory.Items)
            {
                lstSelectEmployeeCategory.Items.Remove(li);
            }
            lstEmployeeCategory.SelectedIndex = -1;
        }
        btnPullCate.Focus();
    }



    public void FillEmployeeList()
    {
        DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(HttpContext.Current.Session["CompId"].ToString());

        DataTable dtTemp = dtEmp.Copy();




        try
        {
            if (Session["EmpId"].ToString() == "0")
            {
                dtEmp = new DataView(dtEmp, "EmpDetailInfo<>' ' and Emp_Name<>' '", "Emp_Name asc", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                dtEmp = new DataView(dtEmp, "EmpDetailInfo<>' ' and Emp_Name<>' ' and Emp_Id<>" + Session["EmpId"].ToString() + "", "Emp_Name asc", DataViewRowState.CurrentRows).ToTable();
                dtTemp = new DataView(dtTemp, "Emp_Id=" + Session["EmpId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        catch
        {
        }
        lstEmployeeCategory.DataSource = dtEmp;
        lstEmployeeCategory.DataTextField = "EmpDetailInfo";
        lstEmployeeCategory.DataValueField = "Emp_Id";
        lstEmployeeCategory.DataBind();
        lstSelectEmployeeCategory.DataSource = dtTemp;
        lstSelectEmployeeCategory.DataTextField = "EmpDetailInfo";
        lstSelectEmployeeCategory.DataValueField = "Emp_Id";
        lstSelectEmployeeCategory.DataBind();

    }
    #endregion
}
