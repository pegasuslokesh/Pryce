using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;

public partial class Inventory_UnitMaster : BasePage
{
    #region defined Class Object
    DataAccessClass daClass = null;
    Inv_UnitMaster ObjInvUnitMaster = null;
    SystemParameter ObjSysPeram = null;
    Common cmn = null;
    Set_DocNumber objDocNo = null;
    UserMaster ObjUser = null;
    PageControlCommon objPageCmn = null;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        daClass = new DataAccessClass(Session["DBConnection"].ToString());
        ObjInvUnitMaster = new Inv_UnitMaster(Session["DBConnection"].ToString());
        ObjSysPeram = new SystemParameter(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Inventory/UnitMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

          
            AllPageCode(clsPagePermission);

           
            txtUnitCode.Text = GetDocumentNumber();

            FillConversionUnitDDL("");
            FillGridBin();
            FillGrid();

            //AllPageCode();
        }

    }

    #region AllPageCode
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();     
        imgBtnRestore.Visible = clsPagePermission.bRestore;
    }
    public string GetDocumentNumber()
    {
        string DocumentNo = string.Empty;

        DataTable Dt = objDocNo.GetDocumentNumberAll(Session["CompId"].ToString(), "11", "21");

        if (Dt.Rows.Count > 0)
        {
            if (Dt.Rows[0]["Prefix"].ToString() != "")
            {
                DocumentNo += Dt.Rows[0]["Prefix"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["CompId"].ToString()))
            {
                DocumentNo += Session["CompId"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["BrandId"].ToString()))
            {
                DocumentNo += Session["BrandId"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["LocationId"].ToString()))
            {
                DocumentNo += Session["LocId"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["DeptId"].ToString()))
            {
                DocumentNo += (string)Session["DepartmentId"];
            }

            if (Convert.ToBoolean(Dt.Rows[0]["EmpId"].ToString()))
            {
                DataTable Dtuser = ObjUser.GetUserMasterByUserId(Session["UserId"].ToString(), Session["LoginCompany"].ToString());
                DocumentNo += Dtuser.Rows[0]["Emp_Id"].ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["Year"].ToString()))
            {
                DocumentNo += DateTime.Now.Year.ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["Month"].ToString()))
            {
                DocumentNo += DateTime.Now.Month.ToString();
            }

            if (Convert.ToBoolean(Dt.Rows[0]["Day"].ToString()))
            {
                DocumentNo += DateTime.Now.Day.ToString();
            }

            if (Dt.Rows[0]["Suffix"].ToString() != "")
            {
                DocumentNo += Dt.Rows[0]["Suffix"].ToString();
            }
            if (DocumentNo != "")
            {
                DocumentNo += "-" + (Convert.ToInt32(ObjInvUnitMaster.GetUnitMasterAll(Session["CompId"].ToString().ToString()).Rows.Count) + 1).ToString();
            }
            else
            {
                DocumentNo += (Convert.ToInt32(ObjInvUnitMaster.GetUnitMasterAll(Session["CompId"].ToString().ToString()).Rows.Count) + 1).ToString();
            }
        }
        else
        {
            DocumentNo += (Convert.ToInt32(ObjInvUnitMaster.GetUnitMasterAll(Session["CompId"].ToString().ToString()).Rows.Count) + 1).ToString();
        }
        return DocumentNo;
    }
    #endregion


    #region System defined Function

    protected void btnBin_Click(object sender, EventArgs e)
    {
        FillGridBin();
        txtbinValue.Focus();
    }

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {

        //System.Threading.Thread.Sleep(20000);
        editid.Value = e.CommandArgument.ToString();
        DataTable dt = ObjInvUnitMaster.GetUnitMasterById(Session["CompId"].ToString().ToString(), editid.Value);
        if (dt.Rows.Count != 0)
        {
            txtUnitCode.Text = dt.Rows[0]["Unit_Code"].ToString();
            txtEUnitName.Text = dt.Rows[0]["Unit_Name"].ToString();
            txtConversion_Qty.Text = dt.Rows[0]["Coversion_Qty"].ToString();

            try
            {
                FillConversionUnitDDL(e.CommandArgument.ToString());
                if (dt.Rows[0]["Conversion_Unit"].ToString() != "0")
                {
                    ddlConversion_Unit.SelectedValue = dt.Rows[0]["Conversion_Unit"].ToString();
                }
                else
                {
                    ddlConversion_Unit.SelectedValue = "--Select--";
                }
            }
            catch
            {
                ddlConversion_Unit.SelectedValue = "--Select--";
            }

            txtlUnitName.Text = dt.Rows[0]["Unit_Name_L"].ToString();
            //btnNew_Click(null, null);
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        b = ObjInvUnitMaster.DeleteUnitMaster(Session["CompId"].ToString().ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
            FillConversionUnitDDL("");
            //FillGridBin();
            FillGrid();
            Reset();
            try
            {
                int i = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
                ((LinkButton)gvunitMaster.Rows[i].FindControl("IbtnDelete")).Focus();
            }
            catch
            {
                txtValue.Focus();
            }
        }
        else
        {
            DisplayMessage("Record Not Delete");
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
        //FillGridBin();
        FillConversionUnitDDL("");
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
        txtValue.Focus();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
        txtEUnitName.Focus();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FillGrid();
        FillGridBin();
        Reset();
        //btnList_Click(null, null);
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
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
            DataTable dtCust = (DataTable)Session["Unit"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            ViewState["dtFilter"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvunitMaster, view.ToTable(), "", "");
            //AllPageCode();
            txtValue.Focus();

        }

    }


    protected void gvunitMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvunitMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)ViewState["dtFilter"];
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvunitMaster, dt, "", "");

        //AllPageCode();
        gvunitMaster.BottomPagerRow.Focus();

    }
    protected void gvUnitMaster_OnSorting(object sender, GridViewSortEventArgs e)
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
        objPageCmn.FillData((object)gvunitMaster, dt, "", "");

        //AllPageCode();
        gvunitMaster.HeaderRow.Focus();

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        btnSave.Enabled = false;

        if (txtEUnitName.Text == "")
        {
            DisplayMessage("Enter Unit Name");
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtEUnitName);
            btnSave.Enabled = true;
            return;
        }
        if (txtConversion_Qty.Text == "")
        {
            txtConversion_Qty.Text = "1";
        }
        else
        {
            if (txtConversion_Qty.Text == "0")
            {
                txtConversion_Qty.Text = "1";
            }

        }


        string strConversionUnit = "";
        if (ddlConversion_Unit.SelectedValue == "--Select--")
        {
            strConversionUnit = "";
        }
        else
        {
            strConversionUnit = ddlConversion_Unit.SelectedValue.ToString();
        }

        int b = 0;



        if (editid.Value == "")
        {


            b = ObjInvUnitMaster.InsertUnitMaster(Session["CompId"].ToString().ToString(), txtEUnitName.Text, txtlUnitName.Text, txtUnitCode.Text, strConversionUnit, txtConversion_Qty.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
            if (b != 0)
            {
                DisplayMessage("Record Saved","green");
                Reset();
                FillGrid();
                txtEUnitName.Focus();
            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }
        else
        {
            b = ObjInvUnitMaster.UpdateUnitMaster(Session["CompId"].ToString().ToString(), editid.Value.ToString(), txtEUnitName.Text, txtlUnitName.Text, txtUnitCode.Text, strConversionUnit, txtConversion_Qty.Text, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            if (b != 0)
            {
                //btnList_Click(null, null);
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
        btnSave.Enabled = true;
    }

    protected void txtEUnitName_TextChanged(object sender, EventArgs e)
    {
        if (txtEUnitName.Text != "")
        {
            DataTable dtUnit = ObjInvUnitMaster.GetUnitMasterAll(Session["CompId"].ToString().ToString());
            dtUnit = new DataView(dtUnit, "unit_Name='" + txtEUnitName.Text.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtUnit.Rows.Count > 0)
            {
                if (dtUnit.Rows[0]["Unit_Id"].ToString() != editid.Value)
                {
                    if (Convert.ToBoolean(dtUnit.Rows[0]["IsActive"].ToString()))
                    {
                        DisplayMessage("Unit Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtEUnitName);


                    }
                    else
                    {

                        DisplayMessage("Unit Name Already Exists :- Go to Bin Tab");

                    }
                    txtEUnitName.Text = "";
                    txtEUnitName.Focus();
                }

            }
            else
            {
                txtlUnitName.Focus();
            }
        }
    }

    #region Bin Section
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
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + txtbinValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + txtbinValue.Text + "%'";
            }

            DataTable dtCust = (DataTable)Session["dtbinUnit"];

            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvUnitMasterBin, view.ToTable(), "", "");

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
        FillConversionUnitDDL("");
        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }
    protected void gvUnitMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvUnitMasterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtbinFilter"];
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvUnitMasterBin, dt, "", "");


        //AllPageCode();

        string temp = string.Empty;
        bool isselcted;

        for (int i = 0; i < gvUnitMasterBin.Rows.Count; i++)
        {
            Label lblconid = (Label)gvUnitMasterBin.Rows[i].FindControl("lblUnitId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvUnitMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }

        gvUnitMasterBin.BottomPagerRow.Focus();

    }
    protected void gvUnitMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtbinFilter"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvUnitMasterBin, dt, "", "");
        //AllPageCode();
        gvUnitMasterBin.HeaderRow.Focus();

    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvUnitMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvUnitMasterBin.Rows.Count; i++)
        {
            ((CheckBox)gvUnitMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvUnitMasterBin.Rows[i].FindControl("lblUnitId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvUnitMasterBin.Rows[i].FindControl("lblUnitId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvUnitMasterBin.Rows[i].FindControl("lblUnitId"))).Text.Trim().ToString())
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
        ((CheckBox)gvUnitMasterBin.HeaderRow.FindControl("chkgvSelectAll")).Focus();
    }
    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvUnitMasterBin.Rows[index].FindControl("lblUnitId");
        if (((CheckBox)gvUnitMasterBin.Rows[index].FindControl("chkgvSelect")).Checked)
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
        ((CheckBox)gvUnitMasterBin.Rows[index].FindControl("chkgvSelect")).Focus();
    }
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        DataTable dtUnit = (DataTable)Session["dtbinFilter"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Unit_Id"]))
                {
                    lblSelectedRecord.Text += dr["Unit_Id"] + ",";
                }
            }
            for (int i = 0; i < gvUnitMasterBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)gvUnitMasterBin.Rows[i].FindControl("lblUnitId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvUnitMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtBinFilter"];
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvUnitMasterBin, dtUnit1, "", "");

            //AllPageCode();
            ViewState["Select"] = null;
        }
        ImgbtnSelectAll.Focus();
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = ObjInvUnitMaster.DeleteUnitMaster(Session["CompId"].ToString().ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());
                }
            }
        }

        if (b != 0)
        {
            FillConversionUnitDDL("");
            FillGrid();
            FillGridBin();
            lblSelectedRecord.Text = "";
            ViewState["Select"] = null;
            DisplayMessage("Record Activated");
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in gvUnitMasterBin.Rows)
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
        Inv_UnitMaster ObjUnitMaster = new Inv_UnitMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(ObjUnitMaster.GetUnitMaster(HttpContext.Current.Session["CompId"].ToString()), "Unit_Name like '%" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Unit_Name"].ToString();
        }
        return txt;
    }
    #endregion

    #region User defined Function

    public void FillGrid()
    {
        DataTable dt = ObjInvUnitMaster.GetUnitMaster(Session["CompId"].ToString().ToString());
        dt = new DataView(dt, "", "Unit_Id desc", DataViewRowState.CurrentRows).ToTable();
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvunitMaster, dt, "", "");

        ViewState["dtFilter"] = dt;
        Session["Unit"] = dt;

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        //AllPageCode();
    }
    public void Reset()
    {
        txtUnitCode.Text = GetDocumentNumber();
        FillConversionUnitDDL("");
        txtConversion_Qty.Text = "";
        txtEUnitName.Text = "";
        txtlUnitName.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 3;
        ddlFieldName.SelectedIndex = 0;
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
        dt = ObjInvUnitMaster.GetUnitMasterInactive(Session["CompId"].ToString().ToString());
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvUnitMasterBin, dt, "", "");
        Session["dtbinFilter"] = dt;
        Session["dtbinUnit"] = dt;
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
    public string SetDecimal(string amount)
    {
        return ObjSysPeram.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), amount);
    }
    public void FillConversionUnitDDL(string strExceptId)
    {
        DataTable dt = ObjInvUnitMaster.GetUnitMaster(Session["CompId"].ToString().ToString());

        if (strExceptId != "")
        {
            string query = "Unit_Id<>'" + strExceptId + "'";
            dt = new DataView(dt, query, "", DataViewRowState.OriginalRows).ToTable();
        }

        if (dt.Rows.Count > 0)
        {
            ddlConversion_Unit.DataSource = null;
            ddlConversion_Unit.DataBind();


            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015


            objPageCmn.FillData((object)ddlConversion_Unit, dt, "Unit_Name", "Unit_Id");

        }
        else
        {
            try
            {
                ddlConversion_Unit.Items.Clear();
                ddlConversion_Unit.DataSource = null;
                ddlConversion_Unit.DataBind();
                ddlConversion_Unit.Items.Insert(0, "--Select--");
                ddlConversion_Unit.SelectedIndex = 0;
            }
            catch
            {
                ddlConversion_Unit.Items.Insert(0, "--Select--");
                ddlConversion_Unit.SelectedIndex = 0;
            }
        }
    }
    public string GetUnitName(string UnitId)
    {
        DataTable dt = new DataView(ObjInvUnitMaster.GetUnitMasterAll(Session["CompId"].ToString().ToString()), "Unit_Id='" + UnitId.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string strUnitName = "NA";
        if (dt.Rows.Count != 0)
        {
            strUnitName = dt.Rows[0]["Unit_Name"].ToString();
        }
        return strUnitName;
    }
    #endregion

    protected void txtUnitCode_TextChanged(object sender, EventArgs e)
    {
        if (txtUnitCode.Text != "")
        {
            DataTable dtUnit = ObjInvUnitMaster.GetUnitAll();
            dtUnit = new DataView(dtUnit, "Unit_Code='" + txtUnitCode.Text.Trim() + "'and Company_Id='" + Session["CompId"].ToString().ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtUnit.Rows.Count > 0)
            {
                if (dtUnit.Rows[0]["Unit_Id"].ToString() != editid.Value)
                {
                    if (Convert.ToBoolean(dtUnit.Rows[0]["IsActive"].ToString()))
                    {
                        DisplayMessage("Unit Code Already Exists");
                    }
                    else
                    {
                        DisplayMessage("Unit Code Already Exists :- Go to Bin Tab");
                    }
                    txtUnitCode.Text = GetDocumentNumber();
                    txtUnitCode.Focus();
                }
            }
            else
            {
                txtEUnitName.Focus();
            }
        }
    }
}
