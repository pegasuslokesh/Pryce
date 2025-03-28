using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Web;

public partial class ITSetUp_ObjectEntry : BasePage
{
    Common cmn = null;
    SystemParameter objSys = null;
    ObjectMaster objObject = null;
    IT_ApplicationMaster objApp = null;
    IT_ModuleMaster objModule = null;
    IT_App_Op_Permission obj_OP_Permission = null;
    ModuleMaster objModule1 = null;
    IT_ObjectEntry objObjectEntry = null;
    IT_Object_Table objObjectTable = null;
    Set_ApprovalMaster ObjApproval = null;
    NotificationMaster Obj_Notification = null;
    PageControlCommon objPageCmn = null;
    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        //AllPageCode();
        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objObject = new ObjectMaster(Session["DBConnection"].ToString());
        objApp = new IT_ApplicationMaster(Session["DBConnection"].ToString());
        objModule = new IT_ModuleMaster(Session["DBConnection"].ToString());
        obj_OP_Permission = new IT_App_Op_Permission(Session["DBConnection"].ToString());
        objModule1 = new ModuleMaster(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        objObjectTable = new IT_Object_Table(Session["DBConnection"].ToString());
        ObjApproval = new Set_ApprovalMaster(Session["DBConnection"].ToString());
        Obj_Notification = new NotificationMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../ITSetup/ObjectEntry.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            txtValue.Focus();

            //FillGridBin();
            FillGrid();
            FillApplicationDDL();
            //btnList_Click(null, null);
            Fillopeartion();
            FillModule();
            FillAppprovalType();
            NotifiactionType();
        }
    }

    private void FillAppprovalType()
    {
        DataTable dtBrand = ObjApproval.GetApprovalMaster();
        objPageCmn.FillData((object)ddlApprovalType, dtBrand, "Approval_Name", "Approval_Id");
        dtBrand.Dispose();
    }

    private void NotifiactionType()
    {
        DataTable dtNotifcation = Obj_Notification.GetNotificationMaster(StrCompId, StrBrandId, strLocationId, "True", DateTime.Now.ToString());

        objPageCmn.FillData((object)DdlNotificationType, dtNotifcation, "Type", "Trans_id");


        dtNotifcation.Dispose();
    }

    public void FillModule()
    {
        DataTable dt = objModule.GetModuleMaster();
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)ddlModule, dt, "Module_Name", "Module_Id");
        }
        else
        {
            ddlModule.Items.Clear();
        }
    }
    public void Fillopeartion()
    {
        DataTable DtOpType = objObject.GetOpType();
        if (DtOpType.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)chkOpeartionLIst, DtOpType, "Op_Type", "Op_Id");
        }
        else
        {
            chkOpeartionLIst.DataSource = null;
            chkOpeartionLIst.DataBind();
        }
    }
    public void FillApplicationDDL()
    {
        DataTable dt = objApp.GetApplicationMaster();
        if (dt.Rows.Count > 0)
        {
            chkApplication.DataSource = null;
            chkApplication.DataBind();
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)chkApplication, dt, "Application_Name", "Application_Id");
        }
        else
        {
            try
            {
                chkApplication.DataSource = null;
                chkApplication.DataBind();
            }
            catch
            {
                chkApplication.DataSource = null;
                chkApplication.DataBind();
            }
        }
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = true;
        imgBtnRestore.Visible = true;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
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
        //FillGridBin();

        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
    }


    protected void txtObjectName_TextChanged(object sender, EventArgs e)
    {


        if (editid.Value == "")
        {
            DataTable dt = objObject.GetObjectMasterByObjectName(txtObjectName.Text);
            if (dt.Rows.Count > 0)
            {
                txtObjectName.Text = "";
                DisplayMessage("Object Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtObjectName);
                return;
            }
            DataTable dt1 = objObject.GetObjectMasterInactive();
            dt1 = new DataView(dt1, "Object_Name='" + txtObjectName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtObjectName.Text = "";
                DisplayMessage("Object Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtObjectName);
                return;
            }
        }
        else
        {
            DataTable dtTemp = objObject.GetObjectMasterById(editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Object_Name"].ToString() != txtObjectName.Text)
                {
                    DataTable dt = objObject.GetObjectMasterByObjectName(txtObjectName.Text);
                    if (dt.Rows.Count > 0)
                    {
                        txtObjectName.Text = "";
                        DisplayMessage("Object Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtObjectName);
                        return;
                    }
                    DataTable dt1 = objObject.GetObjectMasterInactive();
                    dt1 = new DataView(dt1, "Object_Name='" + txtObjectName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtObjectName.Text = "";
                        DisplayMessage("Object Name Already Exists - Go to Bin Tab");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtObjectName);
                        return;
                    }
                }
            }
        }
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtObjectNameL);


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
            DataTable dtCust = (DataTable)Session["Object"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Obj_ent"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvObjectMaster, view.ToTable(), "", "");
            //AllPageCode();
        }
        txtValue.Focus();
    }
    protected void btnbinbind_Click(object sender, ImageClickEventArgs e)
    {
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
            DataTable dtCust = (DataTable)Session["dtbinObject"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvObjectMasterBin, view.ToTable(), "", "");
            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
                ImgbtnSelectAll.Visible = false;
            }
            else
            {
                //AllPageCode();
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
    }

    protected void gvObjectMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        gvObjectMasterBin.PageIndex = e.NewPageIndex;
        if (HDFSortbin.Value == "")
            FillGridBin();
        else
        {
            DataTable dt = (DataTable)Session["dtbinFilter"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvObjectMasterBin, dt, "", "");
            //AllPageCode();
        }
        string temp = string.Empty;
        bool isselcted;

        for (int i = 0; i < gvObjectMasterBin.Rows.Count; i++)
        {
            Label lblconid = (Label)gvObjectMasterBin.Rows[i].FindControl("lblObjectId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvObjectMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }

    }
    protected void gvObjectMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objObject.GetObjectMasterInactive();
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvObjectMasterBin, dt, "", "");
        //AllPageCode();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int b = 0;



        if (txtObjectName.Text == "")
        {
            DisplayMessage("Enter Object Name");
            txtObjectName.Focus();
            return;
        }

        //if (ddlApplication.SelectedIndex == 0)
        //{
        //    DisplayMessage("Select Application");
        //    ddlApplication.Focus();
        //    return;

        //}

        if (ddlModule.SelectedIndex == 0)
        {
            DisplayMessage("Select Module");
            ddlModule.Focus();
            return;

        }



        if (txtOrderNo.Text == "")
        {
            txtOrderNo.Text = "0";

        }

        string strAppprovalId = "0";


        if (ddlApprovalType.SelectedIndex > 0)
        {
            strAppprovalId = ddlApprovalType.SelectedValue.ToString();
        }

        string strNotification_ID = "0";
        if (DdlNotificationType.SelectedIndex > 0)
        {
            strNotification_ID = DdlNotificationType.SelectedValue.ToString();
        }


        if (editid.Value == "")
        {

            DataTable dt1 = objObject.GetObjectMaster();

            dt1 = new DataView(dt1, "Object_Name='" + txtObjectName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("Object name already exists");
                txtObjectName.Focus();
                return;

            }


            b = objObject.InsertObjectMaster(txtObjectName.Text, txtObjectNameL.Text.Trim(), txtPageUrl.Text, txtOrderNo.Text, "F", "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), strAppprovalId, strNotification_ID, ChkShowInNavigationMenu.Checked.ToString());


            if (b != 0)
            {


                //here we are inserting record in object table 


                string StrTo = string.Empty;
                foreach (string str in txtTablename.Text.Split(';'))
                {
                    if (str.ToString() != "")
                    {
                        if (!StrTo.Split(';').Contains(str))
                        {
                            objObjectTable.InsertRecord(b.ToString(), str, "", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString());
                            StrTo += str + ";";
                        }
                    }
                }



                foreach (ListItem item in chkApplication.Items)
                {
                    if (item.Selected == true)
                    {
                        objObject.InsertObjectModuleMaster(item.Value, ddlModule.SelectedValue, b.ToString());
                    }


                }

                try
                {
                    objObject.InsertObjectModuleMaster("0", ddlModule.SelectedValue, b.ToString());
                    obj_OP_Permission.DeleteRecord(b.ToString());
                }
                catch
                {
                }
                foreach (ListItem item in chkOpeartionLIst.Items)
                {
                    try
                    {
                        if (item.Selected == true)
                        {
                            obj_OP_Permission.insertRecord(b.ToString(), item.Value.ToString());
                        }
                    }
                    catch
                    {
                    }
                }

                DisplayMessage("Record Saved","green");
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


            DataTable dt2 = objObject.GetObjectMaster();

            dt2 = new DataView(dt2, "Object_Name='" + txtObjectName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt2.Rows.Count > 0)
            {
                if (dt2.Rows[0]["Object_Id"].ToString() != editid.Value && editid.Value != "")
                {
                    DisplayMessage("Object Name Already Exists");
                    txtObjectName.Focus();
                    return;
                }
            }

            b = objObject.UpdateObjectMaster(editid.Value, txtObjectName.Text, txtObjectNameL.Text.Trim(), txtPageUrl.Text, txtOrderNo.Text, "F", "True", Session["UserId"].ToString(), DateTime.Now.ToString(), strAppprovalId, strNotification_ID, ChkShowInNavigationMenu.Checked.ToString());

            objObjectTable.DeleteRecordByobjectId(editid.Value);

            string StrTo = string.Empty;
            foreach (string str in txtTablename.Text.Split(';'))
            {
                if (str.ToString() != "")
                {
                    if (!StrTo.Split(';').Contains(str))
                    {
                        objObjectTable.InsertRecord(editid.Value, str, "", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString());
                        StrTo += str + ";";
                    }
                }
            }

            try
            {
                objObject.Delete_It_App_Mod_Object_By_Objectid(editid.Value);
            }
            catch
            {

            }

            foreach (ListItem item in chkApplication.Items)
            {
                if (item.Selected == true)
                {
                    objObject.InsertObjectModuleMaster(item.Value, ddlModule.SelectedValue, editid.Value);
                }
            }
            try
            {
                objObject.InsertObjectModuleMaster("0", ddlModule.SelectedValue, editid.Value);
                obj_OP_Permission.DeleteRecord(editid.Value);
            }
            catch
            {
            }
            foreach (ListItem item in chkOpeartionLIst.Items)
            {
                try
                {
                    if (item.Selected == true)
                    {
                        obj_OP_Permission.insertRecord(editid.Value, item.Value.ToString());
                    }
                }
                catch
                {
                }
            }

            if (b != 0)
            {
                //btnList_Click(null, null);
                DisplayMessage("Record Updated", "green");
                Reset();
                FillGrid();
                Lbl_New_tab.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);

            }
            else
            {
                DisplayMessage("Record Not Updated");
            }
        }
    }



    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();


        DataTable dt = objObject.GetObjectMasterById(editid.Value);
        if (dt.Rows.Count > 0)
        {
            //btnNew_Click(null, null);
            Lbl_New_tab.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "on_View_tab_position()", true);


            txtObjectName.Text = dt.Rows[0]["Object_Name"].ToString();
            txtObjectNameL.Text = dt.Rows[0]["Object_Name_L"].ToString();

            txtOrderNo.Text = dt.Rows[0]["Order_Appear"].ToString();

            txtPageUrl.Text = dt.Rows[0]["Page_Url"].ToString();

            try
            {
                ChkShowInNavigationMenu.Checked = Convert.ToBoolean(dt.Rows[0]["ShowInNavigationMenu"].ToString());
            }
            catch
            {
                ChkShowInNavigationMenu.Checked = false;
            }

            try
            {
                ddlApprovalType.SelectedValue = dt.Rows[0]["Approval_Id"].ToString();
            }
            catch
            {
                ddlApprovalType.SelectedIndex = 0;
            }
            try
            {
                DdlNotificationType.SelectedValue = dt.Rows[0]["notification_type_id"].ToString();
            }
            catch
            {
                DdlNotificationType.SelectedIndex = 0;
            }
            try
            {
                Fillopeartion();
                DataTable DtopPermission = obj_OP_Permission.GetRecord(dt.Rows[0]["Object_Id"].ToString());
                if (DtopPermission.Rows.Count > 0)
                {
                    foreach (ListItem item in chkOpeartionLIst.Items)
                    {
                        for (int i = 0; i < DtopPermission.Rows.Count; i++)
                        {
                            if (item.Value == DtopPermission.Rows[i]["Op_Id"].ToString())
                            {
                                item.Selected = true;
                                break;
                            }
                        }
                    }

                }
            }
            catch
            {
            }

            try
            {
                foreach (ListItem item in chkApplication.Items)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (item.Value == dt.Rows[i]["Application_Id"].ToString())
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
            }
            catch
            {

            }


            try
            {
                ddlModule.SelectedValue = dt.Rows[0]["Module_Id"].ToString();
            }
            catch
            {

            }

            txtTablename.Text = "";


            DataTable dtObjecttable = objObjectTable.GetRecordByObjectId(editid.Value);


            foreach (DataRow dr in dtObjecttable.Rows)
            {

                txtTablename.Text += dr["Table_name"].ToString() + ";";
            }

        }





    }

    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        b = objObject.DeleteObjectMaster(e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
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
    protected void gvObjectMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvObjectMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Obj_ent"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvObjectMaster, dt, "", "");
        //AllPageCode();
    }
    protected void gvObjectMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Obj_ent"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Obj_ent"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvObjectMaster, dt, "", "");
        //AllPageCode();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListObjectName(string prefixText, int count, string contextKey)
    {
        ObjectMaster objObjectMaster = new ObjectMaster(System.Web.HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objObjectMaster.GetObjectMaster(), "Object_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();

        dt = dt.DefaultView.ToTable(true, "Object_Name");

        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Object_Name"].ToString();
        }
        return txt;
    }


    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }



    //protected void ddlApplication_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (ddlApplication.SelectedIndex != 0)
    //    {
    //        string moduleids = string.Empty;
    //        DataTable dt = objModule.GetModuleMaster();
    //        DataTable DtModuleApp = new DataTable();



    //        DtModuleApp = objModule1.GetModuleObjectByApplicatonId(ddlApplication.SelectedValue);


    //        for (int i = 0; i < DtModuleApp.Rows.Count; i++)
    //        {
    //            moduleids += DtModuleApp.Rows[i]["Module_Id"].ToString()  + ",";
    //        }
    //        try
    //        {
    //            dt = new DataView(dt, "Module_Id in (" + moduleids.Substring(0, moduleids.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
    //        }
    //        catch
    //        {
    //            dt = new DataTable();
    //        }
    //        //dt = new DataView(dt, "Application_Id='" + ddlApplication.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
    //        if (dt.Rows.Count > 0)
    //        {

    //            ddlModule.DataSource = null;
    //            ddlModule.DataBind();
    //            ddlModule.DataSource = dt;
    //            ddlModule.DataTextField = "Module_Name";
    //            ddlModule.DataValueField = "Module_Id";
    //            ddlModule.DataBind();

    //            ListItem li = new ListItem("--Select--", "0");
    //            ddlModule.Items.Insert(0, li);
    //            ddlModule.SelectedIndex = 0;

    //        }
    //        else
    //        {
    //            try
    //            {
    //                ddlModule.Items.Clear();
    //                ddlModule.DataSource = null;
    //                ddlModule.DataBind();
    //                ListItem li = new ListItem("--Select--", "0");
    //                ddlModule.Items.Insert(0, li);
    //                ddlModule.SelectedIndex = 0;
    //            }
    //            catch
    //            {
    //                ListItem li = new ListItem("--Select--", "0");
    //                ddlModule.Items.Insert(0, li);
    //                ddlModule.SelectedIndex = 0;

    //            }
    //        }



    //    }
    //    else
    //    {
    //        try
    //        {
    //            ddlModule.Items.Clear();
    //            ddlModule.DataSource = null;
    //            ddlModule.DataBind();
    //            ListItem li = new ListItem("--Select--", "0");
    //            ddlModule.Items.Insert(0, li);
    //            ddlModule.SelectedIndex = 0;
    //        }
    //        catch
    //        {
    //            ListItem li = new ListItem("--Select--", "0");
    //            ddlModule.Items.Insert(0, li);
    //            ddlModule.SelectedIndex = 0;

    //        }
    //    }


    //}



    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FillGrid();
        //FillGridBin();
        Reset();
        //btnList_Click(null, null);
        Lbl_New_tab.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }

    public void FillGrid()
    {
        DataTable dt = objObject.GetObjectMaster();
        if (dt.Rows.Count > 0)
        {
            dt = dt.DefaultView.ToTable(true, "Object_Id", "Object_Name", "Object_Name_L", "Module_Name");
        }

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvObjectMaster, dt, "", "");
        //AllPageCode();
        Session["dtFilter_Obj_ent"] = dt;
        Session["Object"] = dt;

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objObject.GetObjectMasterInactive();

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvObjectMasterBin, dt, "", "");

        Session["dtbinFilter"] = dt;
        Session["dtbinObject"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        if (dt.Rows.Count == 0)
        {
            imgBtnRestore.Visible = false;
            ImgbtnSelectAll.Visible = false;
        }
        else
        {

            //AllPageCode();
        }

    }


    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvObjectMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvObjectMasterBin.Rows.Count; i++)
        {
            ((CheckBox)gvObjectMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvObjectMasterBin.Rows[i].FindControl("lblObjectId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvObjectMasterBin.Rows[i].FindControl("lblObjectId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvObjectMasterBin.Rows[i].FindControl("lblObjectId"))).Text.Trim().ToString())
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
        Label lb = (Label)gvObjectMasterBin.Rows[index].FindControl("lblObjectId");
        if (((CheckBox)gvObjectMasterBin.Rows[index].FindControl("chkgvSelect")).Checked)
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
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["dtbinFilter"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Object_Id"]))
                {
                    lblSelectedRecord.Text += dr["Object_Id"] + ",";
                }
            }
            for (int i = 0; i < gvObjectMasterBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)gvObjectMasterBin.Rows[i].FindControl("lblObjectId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvObjectMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtBinFilter"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvObjectMasterBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
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
                    b = objObject.DeleteObjectMaster(lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());

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
            foreach (GridViewRow Gvr in gvObjectMasterBin.Rows)
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


    public void Reset()
    {


        txtTablename.Text = "";
        txtObjectName.Text = "";
        txtObjectNameL.Text = "";
        FillApplicationDDL();
        ddlModule.SelectedIndex = 0;
        ddlApprovalType.SelectedIndex = 0;
        DdlNotificationType.SelectedIndex = 0;
        Session["empimgpath"] = null;
        txtOrderNo.Text = "";
        txtPageUrl.Text = "";
        Fillopeartion();
        Lbl_New_tab.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        ChkShowInNavigationMenu.Checked = true;
    }



    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetContactList(string prefixText, int count, string contextKey)
    {

        DataAccessClass objDa = new DataAccessClass(System.Web.HttpContext.Current.Session["DBConnection"].ToString());
        DataTable DtEmail = objDa.return_DataTable("select name from sys.tables");

        try
        {
            DtEmail = new DataView(DtEmail, "name Like('%" + prefixText + "%')", "name Asc", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
            DtEmail = new DataTable();
        }

        string[] txt = new string[DtEmail.Rows.Count];

        if (DtEmail.Rows.Count > 0)
        {
            for (int i = 0; i < DtEmail.Rows.Count; i++)
            {
                txt[i] += DtEmail.Rows[i]["name"].ToString() + ";";
            }

        }

        return txt;
    }

}
