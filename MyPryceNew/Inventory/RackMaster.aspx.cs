using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data.SqlClient;

public partial class Inventory_RackMaster : BasePage
{
    #region defined Class Object
    DataAccessClass daClass = null;
    Inv_RackMaster ObjRackMaster = null;
    SystemParameter ObjSysPeram = null;
    Common cmn = null;
    ArrayList arr = new ArrayList();
    Inv_ProductMaster ObjProductMaster = null;
    Inv_RackDetail ObjRackDetail = null;
    PageControlCommon objPageCmn = null;
    string SortExpression = "";

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {


        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        daClass = new DataAccessClass(Session["DBConnection"].ToString());
        ObjRackMaster = new Inv_RackMaster(Session["DBConnection"].ToString());
        ObjSysPeram = new SystemParameter(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjRackDetail = new Inv_RackDetail(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Inventory/RackMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
          
            AllPageCode(clsPagePermission);

            ddlOption.SelectedIndex = 2;

            FillPerentDDl();
            FillProductCheckBoxlist();
            FillGrid();
            Session["DtProduct"] = null;
        }
        //AllPageCode();
        BindTreeView();

    }

    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        btnSave.Visible = clsPagePermission.bAdd;
        btnRestoreSelected.Visible = clsPagePermission.bRestore;
    }
    #endregion


    #region System defined Function
    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillGridBin();
        txtValueBin.Focus();
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtPbrand = (DataTable)Session["dtInactive"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtPbrand.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Rack_Id"]))
                {
                    lblSelectedRecord.Text += dr["Rack_Id"] + ",";
                }
            }
            for (int i = 0; i < gvRackMasterBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)gvRackMasterBin.Rows[i].FindControl("lblRackId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvRackMasterBin.Rows[i].FindControl("chkSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtInactive"];
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvRackMasterBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        btnSave.Enabled = false;
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        if (txtRackName.Text == "")
        {
            DisplayMessage("Enter Rack Name");
            txtRackName.Focus();
            btnSave.Enabled = true;
            return;
        }
        else
        {
            DataTable dt = ObjRackMaster.GetRackMasterByRackName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtRackName.Text);
            if (dt.Rows.Count != 0)
            {
                if (dt.Rows[0]["Rack_Id"].ToString() != editid.Value)
                {
                    DisplayMessage("Rack Name Already Exists");
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRackName);
                    btnSave.Enabled = true;
                    return;
                }
            }
        }
        string ddlValues;
        int b = 0;

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            if (editid.Value == "")
            {
                if (ddlPerentRack.SelectedIndex == 0)
                {
                    ddlValues = "0";
                }
                else
                {
                    ddlValues = ddlPerentRack.SelectedValue.ToString();
                }
                b = ObjRackMaster.InsertRackMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ddlValues, txtRackName.Text, txtLRackName.Text, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                if (b != 0)
                {

                    foreach (GridViewRow gvrow in gvProduct.Rows)
                    {
                        ObjRackDetail.InsertRackDetail(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), b.ToString(), ((Label)gvrow.FindControl("lblProductId")).Text, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);

                    }
                    DisplayMessage("Record Saved","green");
                    txtRackName.Focus();
                }
                else
                {
                    DisplayMessage("Record Not Saved");
                }


            }
            else
            {
                if (ddlPerentRack.SelectedIndex == 0)
                {
                    ddlValues = "0";
                }
                else
                {
                    ddlValues = ddlPerentRack.SelectedValue.ToString();
                }
                b = ObjRackMaster.UpdateRackMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value.ToString(), ddlValues, txtRackName.Text, txtLRackName.Text, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                if (b != 0)
                {
                    ObjRackDetail.DeleteRackDetail(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), editid.Value.ToString(), ref trns);

                    foreach (GridViewRow gvrow in gvProduct.Rows)
                    {
                        ObjRackDetail.InsertRackDetail(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), editid.Value.ToString(), ((Label)gvrow.FindControl("lblProductId")).Text, "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), ref trns);

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
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            FillGrid();
            Reset(1);
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
            btnSave.Enabled = true;
            return;

        }
        btnSave.Enabled = true;
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();
        FillParentDropDownListExcludingChildren(editid.Value); //For filling up parent ddl excluding children for current node, so that it cannot be moved to its children

        DataTable dt = ObjRackMaster.GetRackMasterTruebyId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value);
        if (dt.Rows.Count != 0)
        {
            txtRackName.Text = dt.Rows[0]["Rack_Name"].ToString();
            txtLRackName.Text = dt.Rows[0]["Rack_Name_L"].ToString();

            if (dt.Rows[0]["ParentRackId"].ToString() == "" || dt.Rows[0]["ParentRackId"].ToString() == "0")
            {
                ddlPerentRack.SelectedIndex = 0;
            }
            else
            {
                DataTable dtp = ObjRackMaster.GetRackMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                DataView dtv = new DataView(dtp);
                dtv.RowFilter = "Rack_Id='" + dt.Rows[0]["ParentRackId"].ToString() + "'";
                dtp = dtv.ToTable();
                if (dtp.Rows.Count != 0)
                {
                    ddlPerentRack.SelectedValue = dtp.Rows[0]["Rack_Id"].ToString();
                }
                else
                {
                    ddlPerentRack.SelectedIndex = 0;
                }
            }

            DataTable dtRackDetail = ObjRackDetail.SelectRowRackDetailRack(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString().ToString(), Session["LocId"].ToString().ToString(), editid.Value);

            if (dtRackDetail.Rows.Count > 0)
            {
                Session["DtProduct"] = dtRackDetail.DefaultView.ToTable(true, "Product_Id");
            }
            else
            {
                Session["DtProduct"] = null;
            }

            objPageCmn.FillData((object)gvProduct, (DataTable)Session["DtProduct"], "", "");

            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);

        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset(0);
        txtRackName.Focus();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Reset(1);
        FillGrid();
        txtValue.Focus();
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }
    protected void gvRackMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRackMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)ViewState["dtFilter"];
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvRackMaster, dt, "", "");
        //AllPageCode();
        gvRackMaster.BottomPagerRow.Focus();
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        string condition = string.Empty;
        if (ddlOption.SelectedIndex != 0)
        {
            if (ddlFieldName.SelectedValue == "Rack_name_L" && txtValue.Text == "")
            {
                condition = ddlFieldName.SelectedValue + " " + "is null";
            }
            else
            {
                if (ddlOption.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text + "'";
                }
                else if (ddlOption.SelectedIndex == 2)
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text + "%'";
                }
                else
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text + "%'";
                }
            }

            TreeViewRack.Visible = false;
            gvRackMaster.Visible = true;
            btnGridView.ToolTip = Resources.Attendance.Tree_View; //Show grid if tree view is current shown

            DataTable dtCust = (DataTable)Session["Rack"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            ViewState["dtFilter"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvRackMaster, view.ToTable(), "", "");
            //AllPageCode();
            txtValue.Focus();
        }
    }
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        TreeViewRack.Visible = false;
        gvRackMaster.Visible = true;
        btnGridView.ToolTip = Resources.Attendance.Tree_View; ;
        BindTreeView(); //Update TreeView on Refresh        
        FillGrid();
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 1;
        txtValue.Text = "";
        txtValue.Focus();
    }
    protected void btnGridView_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (TreeViewRack.Visible == true)//To show grid view
        {
            TreeViewRack.Visible = false;
            gvRackMaster.Visible = true;
            btnGridView.ToolTip = Resources.Attendance.Tree_View;
        }
        else //To show tree view
        {
            btnGridView.ToolTip = Resources.Attendance.Grid_View;
            gvRackMaster.Visible = false;
            TreeViewRack.Visible = true;
            trdel2.Visible = false;
            trdel.Visible = false;
            trgv.Visible = false;
            BindTreeView();
            FillGrid();
            txtValue.Text = "";
        }
        btnGridView.Focus();
    }
    protected void btnTreeView_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (TreeViewRack.Visible == true)
        {
            TreeViewRack.Visible = false;
            gvRackMaster.Visible = true;
            btnGridView.ToolTip = Resources.Attendance.Tree_View;
        }
        else
        {
            btnGridView.ToolTip = Resources.Attendance.Grid_View;
            gvRackMaster.Visible = false;
            TreeViewRack.Visible = true;
            trdel2.Visible = false;
            trdel.Visible = false;
            trgv.Visible = false;
        }
        btnTreeView.Focus();
    }
    protected void TreeViewRack_SelectedNodeChanged(object sender, EventArgs e)
    {
        CommandEventArgs CmdEvntArgs = new CommandEventArgs("", (object)TreeViewRack.SelectedValue.ToString());
        btnEdit_Command(sender, CmdEvntArgs);
    }
    protected void btnDeleteNode_Click(object sender, EventArgs e)
    {
        if (!rbtnmovechild.Checked && !rdbtndelchild.Checked && !trdel.Visible)
        {
            //Lower most node delete code
            int b = 0;
            b = ObjRackMaster.DeleteRackMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DeleteNodeValue"].ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            if (b != 0)
            {
                ObjRackMaster.UpdateParentId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DeleteNodeValue"].ToString());
                DisplayMessage("Record Deleted");
                FillGridBin(); //Update Bin tab
                FillGrid();
                FillPerentDDl();
                BindTreeView(); //Update tree view if Record has been Deleted And Moved To Bin
                trgv.Visible = false;
                trdel.Visible = false;
                trdel2.Visible = false;
                Reset(1);
            }
            else
            {
                DisplayMessage("Record Not Deleted");
            }
        }
        else if (rbtnmovechild.Checked && !rdbtndelchild.Checked)
        {
            //Move child node to selected node
            DataTable dt1 = GetRackMasterByParentId(Session["DeleteNodeValue"].ToString());
            string[] ChildrenNodeId = new string[dt1.Rows.Count];
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                ChildrenNodeId[i] = dt1.Rows[i]["Rack_Id"].ToString();
            }

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                if (ddlgroup0.SelectedValue.ToString() == "--Select--")
                    ObjRackMaster.UpdateParentIdbyrackId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ChildrenNodeId[i], "");
                else
                    ObjRackMaster.UpdateParentIdbyrackId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ChildrenNodeId[i], ddlgroup0.SelectedValue.ToString());
            }

            int b = 0;
            b = ObjRackMaster.DeleteRackMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DeleteNodeValue"].ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            if (b != 0)
            {
                ObjRackMaster.UpdateParentId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DeleteNodeValue"].ToString());
                DisplayMessage("Record Deleted");
                FillGridBin(); //Update Bin Tab
                FillGrid();
                FillPerentDDl();
                BindTreeView();//Update tree view if Record has been Deleted And Moved To Bin
                trgv.Visible = false;
                trdel.Visible = false;
                trdel2.Visible = false;
            }
            else
            {
                DisplayMessage("Record Not Deleted");
            }

            Reset(1);

        }
        else if (!rbtnmovechild.Checked && rdbtndelchild.Checked)
        {
            //Delete all children of that node
            FindChildNode(Session["DeleteNodeValue"].ToString());
            for (int i = 0; i < arr.Count; i++)
            {
                ObjRackMaster.DeleteRackMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), arr[i].ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                ObjRackMaster.UpdateParentId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), arr[i].ToString());
            }

            DisplayMessage("Record Deleted");
            FillGridBin(); //Update bin tab
            FillGrid();
            FillPerentDDl();
            BindTreeView(); //Update tree view if Record has been Deleted And Moved To Bin
            trgv.Visible = false;
            trdel.Visible = false;
            trdel2.Visible = false;

            Reset(1);
        }
        txtValue.Focus();
    }
    protected void rdbtndelchild_CheckedChanged(object sender, EventArgs e)
    {
        btnDeleteNode.Text = Resources.Attendance.Delete_Child_Also.ToString();
        trdel2.Visible = true;
        ddlgroup0.Visible = false;
        rdbtndelchild.Focus();
    }
    protected void rbtnmovechild_CheckedChanged(object sender, EventArgs e)
    {
        btnDeleteNode.Text = Resources.Attendance.Move_Child.ToString();
        trdel2.Visible = true;
        ddlgroup0.Visible = true;
        rbtnmovechild.Focus();
    }
    protected void btnCancelDelete_Click(object sender, EventArgs e)
    {
        trgv.Visible = false;
        trdel.Visible = false;
        trdel2.Visible = false;
        ddlgroup0.Visible = false;
        rbtnmovechild.Checked = false;
        rdbtndelchild.Checked = false;
        Session["DeleteNodeValue"] = "";
        btnDeleteNode.Text = Resources.Attendance.Delete.ToString();
        txtValue.Focus();
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)//delete(in grid) button click event modified
    {
        //Code to show information of Category to delete 
        DataTable dtable = ObjRackMaster.GetRackMasterTruebyId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), e.CommandArgument.ToString());
        lblDelRackId.Text = /*Resources.Attendance.Category_Id.ToString() + " : " + */dtable.Rows[0]["Rack_Id"].ToString();
        lblDelRackName.Text = /*Resources.Attendance.Category_Name.ToString() + " : " + */dtable.Rows[0]["Rack_Name"].ToString();
        if (dtable.Rows[0]["ParentRackId"].ToString() != "0")
        {
            DataTable dtable2 = ObjRackMaster.GetRackMasterTruebyId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtable.Rows[0]["ParentRackId"].ToString());
            try
            {
                lblDelParentrack.Text = /*Resources.Attendance.Parent_Category.ToString() + " : " + */dtable2.Rows[0]["Rack_Name"].ToString();
                rowDelParentRack.Visible = true;
            }
            catch
            { }
        }
        else
        {
            rowDelParentRack.Visible = false;
            lblDelParentrack.Text = /*Resources.Attendance.Parent_Category.ToString() + " : ---";*/ "---";
        }
        //For modal dialog box
        btnCancelDelete_Click(null, null);

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Delete_Rack_Popup()", true);
        //Check before delete if child is present
        DataTable dt = GetRackMasterByParentId(e.CommandArgument.ToString());
        if (dt.Rows.Count > 0)
        {
            trgv.Visible = true;
            trdel.Visible = true;
            rbtnmovechild.Visible = true;
            rdbtndelchild.Visible = true;
            FillMoveChildDropDownList(e.CommandArgument.ToString());
            Session["DeleteNodeValue"] = e.CommandArgument.ToString();
        }
        else
        {
            trgv.Visible = true;
            trdel2.Visible = true;
            Session["DeleteNodeValue"] = e.CommandArgument.ToString();
        }

    }
    protected void gvRackMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRackMasterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtInactive"];
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvRackMasterBin, dt, "", "");
        //AllPageCode();
        string temp = string.Empty;

        for (int i = 0; i < gvRackMasterBin.Rows.Count; i++)
        {
            Label lblconid = (Label)gvRackMasterBin.Rows[i].FindControl("lblRackId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvRackMasterBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
        gvRackMasterBin.BottomPagerRow.Focus();
    }
    protected void gvRackMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        SortExpression = e.SortExpression;
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = ObjRackMaster.GetRackMasterFalseAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtInactive"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvRackMasterBin, dt, "", "");
        lblSelectedRecord.Text = "";
        //AllPageCode();
        gvRackMasterBin.HeaderRow.Focus();
    }
    protected void gvRackMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        SortExpression = e.SortExpression;
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["dtFilter"];
        DataView dv = new DataView(dt);
        string Query = "" + SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        ViewState["dtFilter"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvRackMaster, dt, "", "");
        //AllPageCode();
        gvRackMaster.HeaderRow.Focus();

    }
    //Search panel on Bin Tab
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        BindTreeView(); //Update TreeView on Refresh
        //Update Bin Tab
        FillGridBin();
        txtValueBin.Text = "";
        ddlOptionBin.SelectedIndex = 2;
        ddlFieldNameBin.SelectedIndex = 1;
        lblSelectedRecord.Text = "";
        txtValueBin.Focus();
    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        string condition = string.Empty;
        if (ddlOptionBin.SelectedIndex != 0)
        {
            //if (ddlFieldNameBin.SelectedValue == "Rack_Name_L" && txtValueBin.Text == "")
            //{
            //    condition = ddlFieldNameBin.SelectedValue + " " + "is null";
            //}
            //else
            //{

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
                //    }
            }
            //}

            DataTable dtCust = (DataTable)Session["RackBin"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtInactive"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvRackMasterBin, view.ToTable(), "", "");
            lblSelectedRecord.Text = "";
            if (view.ToTable().Rows.Count == 0)
            {
                //ImgbtnSelectAll.Visible = false;
                //btnRestoreSelected.Visible = false;
            }
            else
            {
                //AllPageCode();
            }

        }
        txtValueBin.Focus();
    }
    protected void txtRackName_TextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt = ObjRackMaster.GetRackMasterByRackName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtRackName.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                txtRackName.Text = "";
                DisplayMessage("Rack Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRackName);

            }
            else
            {
                DataTable dt1 = ObjRackMaster.GetRackMasterFalseAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                dt1 = new DataView(dt1, "Rack_Name='" + txtRackName.Text + "'", "Rack_Name Asc", DataViewRowState.CurrentRows).ToTable();
                if (dt1.Rows.Count > 0)
                {
                    txtRackName.Text = "";
                    DisplayMessage("Rack Name Already Exists - Go to Bin Tab");
                    txtRackName.Focus();

                }
            }
        }
        else
        {
            DataTable dtTemp = ObjRackMaster.GetRackMasterTruebyId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Rack_Name"].ToString() != txtRackName.Text.Trim())
                {
                    DataTable dt = ObjRackMaster.GetRackMasterByRackName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtRackName.Text.Trim());
                    if (dt.Rows.Count > 0)
                    {
                        txtRackName.Text = "";
                        DisplayMessage("Rack Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtRackName);
                    }
                    else
                    {
                        DataTable dt1 = ObjRackMaster.GetRackMasterFalseAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
                        dt1 = new DataView(dt1, "Rack_Name='" + txtRackName.Text + "'", "Rack_Name Asc", DataViewRowState.CurrentRows).ToTable();
                        if (dt1.Rows.Count > 0)
                        {
                            txtRackName.Text = "";
                            DisplayMessage("Rack Name Already Exists - Go to Bin Tab");
                            txtRackName.Focus();

                        }
                    }
                }
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtLRackName);
    }
    protected void btnRestoreSelected_Click(object sender, CommandEventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int Msg = 0;
        DataTable dt = ObjRackMaster.GetRackMasterFalseAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        if (gvRackMasterBin.Rows.Count != 0)
        {
            if (lblSelectedRecord.Text != "")
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        Msg = ObjRackMaster.DeleteRackMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        if (Msg != 0)
                        {
                            ObjRackMaster.UpdateParentId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim().ToString().ToString());
                        }
                    }
                }
            }

            if (Msg != 0)
            {
                lblSelectedRecord.Text = "";
                DisplayMessage("Record Activated");
                btnRefreshBin_Click(null, null);

            }
            else
            {
                int fleg = 0;
                foreach (GridViewRow Gvr in gvRackMasterBin.Rows)
                {
                    CheckBox chk = (CheckBox)Gvr.FindControl("chkSelect");
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
        txtValueBin.Focus();
    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvRackMasterBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < gvRackMasterBin.Rows.Count; i++)
        {
            ((CheckBox)gvRackMasterBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvRackMasterBin.Rows[i].FindControl("lblRackId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvRackMasterBin.Rows[i].FindControl("lblRackId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvRackMasterBin.Rows[i].FindControl("lblRackId"))).Text.Trim().ToString())
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
        chkSelAll.Focus();
    }
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvRackMasterBin.Rows[index].FindControl("lblRackId");
        if (((CheckBox)gvRackMasterBin.Rows[index].FindControl("chkSelect")).Checked)
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
        ((CheckBox)gvRackMasterBin.Rows[index].FindControl("chkSelect")).Focus();
    }
    protected void btnClosePanel_Click(object sender, EventArgs e)
    {
        Reset(1);
        txtValue.Focus();
    }
    #endregion

    #region Auto Complete Function
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Inv_RackMaster ObjRackMaster = new Inv_RackMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjRackMaster.GetDistinctRackName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), prefixText);
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Rack_Name"].ToString() + "";
        }
        return txt;
    }
    #endregion

    #region User defined Function
    public void FillGrid()
    {
        DataTable dt = ObjRackMaster.GetRackMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        if (dt.Rows.Count > 0)
        {   //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvRackMaster, dt, "", "");
        }
        else
        {
            gvRackMaster.DataSource = null;
            gvRackMaster.DataBind();
        }

        Session["Rack"] = dt;
        ViewState["dtFilter"] = dt;
        //AllPageCode();
        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
    }
    public void FillPerentDDl()
    {
        try
        {
            DataTable dt = ObjRackMaster.GetRackMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
            dt = new DataView(dt, "", "Rack_Name Asc", DataViewRowState.CurrentRows).ToTable();



            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015


            objPageCmn.FillData((object)ddlPerentRack, dt, "Rack_Name", "Rack_Id");

        }
        catch
        {
            ddlPerentRack.Items.Insert(0, "--Select--");
            ddlPerentRack.SelectedIndex = 0;
        }
    }
    public void Reset(int RC)
    {
        txtRackName.Text = "";
        txtLRackName.Text = "";
        ddlPerentRack.SelectedIndex = 0;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        editid.Value = "";
        ddlFieldName.SelectedIndex = 1;
        ddlOption.SelectedIndex = 2;
        txtValue.Text = "";
        ddlFieldNameBin.SelectedIndex = 1;
        ddlOptionBin.SelectedIndex = 2;
        txtValueBin.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        if (RC == 1)
        {

            //txtRackName.Text = "";
            txtValue.Focus();
        }

        else
        {
            //txtRackName.Text = "";
            txtRackName.Focus();
        }
        FillProductCheckBoxlist();
        //delete functionality and BIN tab 
        btnDeleteNode.Text = Resources.Attendance.Delete.ToString();
        Session["DeleteNodeValue"] = "";
        arr.Clear();
        TreeViewRack.Visible = false;
        gvRackMaster.Visible = true;
        FillGrid();
        FillPerentDDl();
        btnGridView.ToolTip = Resources.Attendance.Tree_View;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ddlProductSerach_SelectedIndexChanged(null, null);
        objPageCmn.FillData((object)gvProduct, null, "", "");
        Session["DtProduct"] = null;

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
    public string GetParent(object Pid)
    {
        string retval = String.Empty;
        if (Pid.ToString() != "")
        {
            DataTable dt = ObjRackMaster.GetRackMasterTruebyId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Pid.ToString());
            if (dt.Rows.Count != 0)
            {
                retval = dt.Rows[0]["Rack_Name"].ToString();
            }
            return retval;
        }
        else
        {
            return retval;
        }
    }
    //To apply tree view, delete options and BIN tab
    private void BindTreeView()//fucntion to fill up TreeView according to parent child nodes
    {
        TreeViewRack.Nodes.Clear();
        DataTable dt = new DataTable();

        string x = "ParentRackId=" + "0" + "";


        dt = ObjRackMaster.GetRackMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        dt = new DataView(dt, x, "Rack_Name asc", DataViewRowState.OriginalRows).ToTable();
        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn = new TreeNode();
            tn.Text = dt.Rows[i]["Rack_Name"].ToString();
            tn.Value = dt.Rows[i]["Rack_Id"].ToString();
            TreeViewRack.Nodes.Add(tn);
            FillChild((dt.Rows[i]["Rack_Id"].ToString()), tn);
            i++;
        }
        TreeViewRack.DataBind();
    }
    private void FillChild(string index, TreeNode tn)//fill up child nodes and respective child nodes of them 
    {
        DataTable dt = new DataTable();
        dt = GetRackMasterByParentId(index);

        int i = 0;
        while (i < dt.Rows.Count)
        {
            TreeNode tn1 = new TreeNode();
            tn1.Text = dt.Rows[i]["Rack_Name"].ToString();
            tn1.Value = dt.Rows[i]["Rack_Id"].ToString();
            tn.ChildNodes.Add(tn1);
            FillChild((dt.Rows[i]["Rack_Id"].ToString()), tn1);
            i++;
        }
        TreeViewRack.DataBind();
    }
    public DataTable GetRackMasterByParentId(string ParentId) //Function to get entries of same ProductId
    {
        DataTable dt = ObjRackMaster.GetRackMasterTruebyId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ParentId);
        string query = "ParentRackId='" + ParentId + "'";
        dt = ObjRackMaster.GetRackMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        dt = new DataView(dt, query, "", DataViewRowState.OriginalRows).ToTable();
        return dt;
    }
    public void FillMoveChildDropDownList(string strExceptId) //Function to fill up items in drop down list of New Parent after delete
    {
        DataTable dt = ObjRackMaster.GetRackMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        string query = "Rack_Id not in(";

        FindChildNode(strExceptId);

        for (int i = 0; i < arr.Count; i++)
        {
            query += "'" + arr[i].ToString() + "',";
        }
        query = query.Substring(0, query.Length - 1).ToString() + ")";
        dt = new DataView(dt, query, "", DataViewRowState.OriginalRows).ToTable();


        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015


        objPageCmn.FillData((object)ddlgroup0, dt, "Rack_Name", "Rack_Id");


        arr.Clear();
    }
    private void FindChildNode(string p)  //Function to find child nodes and child of child nodes and so on
    {
        arr.Add(p);
        DataTable dt = GetRackMasterByParentId(p.ToString());
        if (dt != null && dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                FindChildNode(dt.Rows[i]["Rack_Id"].ToString());
            }
        }
        else
        {
            return;
        }
    }
    public void FillGridBin()//Function to fill up Inactive items grid in Bin Tab...
    {
        DataTable dt = new DataTable();
        dt = ObjRackMaster.GetRackMasterFalseAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvRackMasterBin, dt, "", "");

        Session["RackBin"] = dt;
        Session["dtInactive"] = dt;
        if (dt != null)
        {
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + dt.Rows.Count.ToString() + "";
        }
        else
        {
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + "0";
        }
        lblSelectedRecord.Text = "";
        if (dt.Rows.Count == 0)
        {
            //ImgbtnSelectAll.Visible = false;
            //btnRestoreSelected.Visible = false;
        }
        else
        {
            //AllPageCode();
        }
    }
    public void FillParentDropDownListExcludingChildren(string strExceptId)
    {
        DataTable dt = ObjRackMaster.GetRackMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        string query = "Rack_Id not in(";

        FindChildNode(strExceptId);

        for (int i = 0; i < arr.Count; i++)
        {
            if (arr[i].ToString() == "")
            {
                arr[i] = "0";
            }

            query += "'" + arr[i].ToString() + "',";
        }
        query = query.Substring(0, query.Length - 1).ToString() + ")";
        dt = new DataView(dt, query, "", DataViewRowState.OriginalRows).ToTable();



        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015


        objPageCmn.FillData((object)ddlPerentRack, dt, "Rack_Name", "Rack_Id");


        arr.Clear();
    }

    public String GetParentRack(string RackId)
    {
        string ParentRackName = string.Empty;
        DataTable dt = ObjRackMaster.GetRackMasterTruebyId(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), RackId);
        try
        {
            ParentRackName = dt.Rows[0]["Rack_Name"].ToString();
        }
        catch
        {
        }
        return ParentRackName;
    }


    public void FillProductCheckBoxlist()
    {
        DataTable dt = new DataView(ObjProductMaster.GetProductMasterTrueAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), "0", "0"), "", "EProductName Asc", DataViewRowState.CurrentRows).ToTable();

        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015

        objPageCmn.FillData((object)ChkProduct, dt, "EProductName", "ProductId");
    }
    #endregion

    #region ProductFilter
    protected void ddlProductSerach_SelectedIndexChanged(object sender, EventArgs e)
    {

        txtSearchProductName.Text = "";
        txtProductId.Text = "";
        if (ddlProductSerach.SelectedIndex == 0)
        {
            txtProductId.Visible = true;
            txtSearchProductName.Visible = false;
        }
        else
        {
            txtProductId.Visible = false;
            txtSearchProductName.Visible = true;
        }

    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = dt.Rows[i]["EProductName"].ToString();
            }
        }
        return str;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
    {
        Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = PM.GetDistinctProductCode(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(), HttpContext.Current.Session["LocId"].ToString());


        string[] txt = new string[dt.Rows.Count];


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["ProductCode"].ToString();
        }


        return txt;
    }



    protected void txtProductCode_TextChanged(object sender, EventArgs e)
    {
        if (txtProductId.Text != "")
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtProductId.Text.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {

                    hdnProductId.Value = dt.Rows[0]["ProductId"].ToString();
                }
                else
                {
                    DisplayMessage("Product Not Found");
                    txtProductId.Text = "";
                    txtProductId.Focus();
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
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductId);
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductId);
    }

    protected void txtProductName_TextChanged(object sender, EventArgs e)
    {
        if (txtSearchProductName.Text != "")
        {
            try
            {
                DataTable dt = new DataTable();
                dt = ObjProductMaster.SearchProductMasterByParameter(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), txtSearchProductName.Text.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {

                    hdnProductId.Value = dt.Rows[0]["ProductId"].ToString();
                }
                else
                {
                    DisplayMessage("Product Not Found");
                    txtProductId.Text = "";
                    txtProductId.Focus();
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
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductId);
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtProductId);
    }






    protected void btnProductSave_Click(object sender, EventArgs e)
    {
        if (txtProductId.Text.Trim() == "" && txtSearchProductName.Text.Trim() == "")
        {
            DisplayMessage("Enter Product Id or Name");
            txtProductId.Focus();
            return;
        }


        DataTable dt = new DataTable();


        if (Session["DtProduct"] == null)
        {
            dt.Columns.Add("Product_Id", typeof(int));
        }
        else
        {
            dt = (DataTable)Session["DtProduct"];
            if (new DataView(dt, "Product_Id=" + hdnProductId.Value + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            {
                DisplayMessage("Product Id already exists");
                if (ddlProductSerach.SelectedIndex == 0)
                {
                    txtProductId.Focus();
                }
                else
                {
                    txtSearchProductName.Focus();
                }
                return;
            }
        }

        dt.Rows.Add(hdnProductId.Value);
        txtProductId.Text = "";
        txtSearchProductName.Text = "";

        if (ddlProductSerach.SelectedIndex == 0)
        {
            txtProductId.Focus();
        }
        else
        {
            txtSearchProductName.Focus();
        }
        Session["DtProduct"] = dt;

        objPageCmn.FillData((object)gvProduct, dt, null, null);
    }



    protected void IbtnPDDelete_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = (DataTable)Session["DtProduct"];

        dt = new DataView(dt, "Product_Id<>" + e.CommandArgument.ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        Session["DtProduct"] = dt;

        objPageCmn.FillData((object)gvProduct, dt, null, null);

    }

    protected void ImgbtnRefresh_Click(object sender, EventArgs e)
    {
        ddlProductSerach.SelectedIndex = 0;
        ddlProductSerach_SelectedIndexChanged(null, null);
        txtProductId.Focus();
    }

    protected string GetProductName(string strProductId)
    {
        string strProductName = string.Empty;
        if (strProductId != "0" && strProductId != "")
        {
            DataTable dtPName = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), strProductId, HttpContext.Current.Session["FinanceYearId"].ToString());
            if (dtPName.Rows.Count > 0)
            {
                strProductName = dtPName.Rows[0]["EProductName"].ToString();
            }
        }
        else
        {
            strProductName = "";
        }
        return strProductName;
    }
    public string ProductCode(string ProductId)
    {
        string ProductName = string.Empty;
        DataTable dt = ObjProductMaster.GetProductMasterById(Session["CompId"].ToString().ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        if (dt.Rows.Count != 0)
        {
            ProductName = dt.Rows[0]["ProductCode"].ToString();
        }
        else
        {
            ProductName = "0";
        }
        return ProductName;
    }


    #endregion
}
