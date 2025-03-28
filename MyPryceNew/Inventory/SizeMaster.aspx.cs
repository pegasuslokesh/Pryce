using DocumentFormat.OpenXml.Spreadsheet;
using PegasusDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterSetUp_SizeMaster : System.Web.UI.Page
{
    //This Page Created By Rahul Sharma on Date 19-06-2024
    PageControlCommon objPageCmn = null;
    Inv_SizeMaster objSize = null;
    DataAccessClass objDa = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objSize = new Inv_SizeMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            hdnCanEdit.Value = "true";
            hdnCanDelete.Value = "true";
            FillGrid();
            FillGridBin();
        }
    }
    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        string condition = string.Empty;
        if (ddlOption.SelectedIndex != 0)
        {
            if (ddlFieldName.SelectedValue == "Size_Name_L" && txtValue.Text == "")
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


            //Show grid if tree view is current shown

            DataTable dtCust = (DataTable)Session["dtFilter_Size__Master"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            ViewState["dtFilter"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
            objPageCmn.FillData((object)gvSizeMaster, view.ToTable(), "", "");
            //AllPageCode();
            txtValue.Focus();
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        FillGrid();
        txtValue.Text = "";
    }
    protected void gvSizeMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSizeMaster.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Size__Master"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvSizeMaster, dt, "", "");


    }
    protected void gvSizeMaster_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Size__Master"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Size__Master"] = dt;
        gvSizeMaster.DataSource = dt;
        gvSizeMaster.DataBind();

    }
    //this Is Save Function 
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtSizeCode.Text == "")
        {
            DisplayMessage("Please enter Size Code");
            return;
        }
        if (txtSizeName.Text == "")
        {
            DisplayMessage("Please enter Size Name");
            return;
        }
        if (editId.Value == "")
        {
            int i = 0;
            // Ensure that IsActive is passed as boolean, not string
            bool isActive = true; // Assuming IsActive should be true by default

            // Convert Session variables to string explicitly if not already done
            string companyId = Session["CompId"].ToString();
            string userId = Session["UserId"].ToString();
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            // Call the InsertSizeMaster method with corrected parameters
            i = objSize.InsertSizeMaster(companyId,
                                            txtSizeCode.Text.Trim(),
                                            txtSizeName.Text.Trim(),
                                            txtSizeNameL.Text.Trim(),
                                            "", "", "", "", // Assuming empty strings for Field1 to Field4
                                            "1900-01-01", // Field5
                                            isActive.ToString(),
                                            userId,
                                            currentDate,
                                            userId,
                                            currentDate);

            // Handle the returned value 'i' as needed

            if (i != 0)
            {
                DisplayMessage("Size Save Successfully");
                FillGrid();
                btnReset_Click(null, null);
            }
            else
            {
                DisplayMessage("Size Not Save");
            }
        }
        else
        {
            int i = 0;
            // Ensure that IsActive is passed as boolean, not string
            bool isActive = true; // Assuming IsActive should be true by default

            // Convert Session variables to string explicitly if not already done
            string editIdValue = editId.Value.ToString(); // Assuming editId is a control that holds the ID
            string companyId = Session["CompId"].ToString();
            string userId = Session["UserId"].ToString();
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            // Call the UpdateSizeMaster method with corrected parameters
            i = objSize.UpdateSizeMaster(editIdValue,
                                            companyId,
                                            txtSizeCode.Text.Trim(),
                                            txtSizeName.Text.Trim(),
                                            txtSizeNameL.Text.Trim(),
                                            "", "", "", "", // Assuming empty strings for Field1 to Field4
                                            "1900-01-01", // Field5
                                            isActive.ToString(),
                                            userId,
                                            currentDate);

            // Handle the returned value 'i' as needed

            if (i != 0)
            {
                DisplayMessage("Size Update Successfully");
                FillGrid();
                btnReset_Click(null, null);
            }
            else
            {
                DisplayMessage("Size Not Update");
            }
        }

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtSizeCode.Text = "";
        txtSizeName.Text = "";
        txtSizeNameL.Text = "";


    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editId.Value = e.CommandArgument.ToString();
        if (editId.Value != "")
        {
            DataTable dt = objSize.GetSizeMasterById(Session["CompId"].ToString(), editId.Value.ToString());
            if (dt.Rows.Count > 0)
            {
                txtSizeCode.Text = dt.Rows[0]["Size_Code"].ToString();
                txtSizeName.Text = dt.Rows[0]["Size_Name"].ToString();
                txtSizeNameL.Text = dt.Rows[0]["Size_Name_L"].ToString();
            }
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }

    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        string Trans_Id = e.CommandArgument.ToString();
        if (Trans_Id != "")
        {
            int i = 0;

            // Convert Session variables to string explicitly if not already done
            string companyId = Session["CompId"].ToString();
            string userId = Session["UserId"].ToString();
            DateTime currentDate = DateTime.Now;

            // Assuming Trans_Id is passed as a variable
            int transId = // provide value for Trans_Id here;

            // Call the DeleteSizeMaster method with corrected parameters
             i = objSize.DeleteSizeMaster(
                companyId,
                Trans_Id,
                false.ToString(), // Assuming this parameter is an integer and is 0 by default for delete operation
                userId,
                currentDate.ToString("yyyy-MM-dd")
            );

            // Handle the returned value 'i' as needed


            if (i != 0)
            {
                DisplayMessage("Size Deleted");
                FillGrid();
                FillGridBin();
            }
            else
            {
                DisplayMessage("Size Not Delete");
            }
        }

    }

    protected void btnList_Click(object sender, EventArgs e)
    {

    }
    protected void btnNew_Click(object sender, EventArgs e)
    {

    }
    protected void btnBin_Click(object sender, EventArgs e)
    {

    }
    public void DisplayMessage(string str, string color = "orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','" + color + "','white');", true);
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
    public void FillGrid()
    {
        DataTable dt = new DataTable();
        dt = objSize.GetSizeMaster(Session["CompId"].ToString());
        if (dt.Rows.Count > 0)
        {
            Session["dtFilter_Size__Master"] = dt;
            objPageCmn.FillData((object)gvSizeMaster, dt, "", "");
            lblTotalRecords.Text = "Total Records " + dt.Rows.Count.ToString() + "";
        }
        else
        {
            gvSizeMaster.DataSource = null;
            gvSizeMaster.DataBind();

        }
    }

    #region Bin Section
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvSizeMasterBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < gvSizeMasterBin.Rows.Count; i++)
        {
            ((CheckBox)gvSizeMasterBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvSizeMasterBin.Rows[i].FindControl("lblgvBinSizeId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvSizeMasterBin.Rows[i].FindControl("lblgvBinSizeId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvSizeMasterBin.Rows[i].FindControl("lblgvBinSizeId"))).Text.Trim().ToString())
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
        Label lb = (Label)gvSizeMasterBin.Rows[index].FindControl("lblgvBinSizeId");
        if (((CheckBox)gvSizeMasterBin.Rows[index].FindControl("chkSelect")).Checked)
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
        ((CheckBox)gvSizeMasterBin.Rows[index].FindControl("chkSelect")).Focus();
    }
    protected void gvSizeMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSizeMasterBin.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtSizeInactive"];
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvSizeMasterBin, dt, "", "");
        //AllPageCode();
        string temp = string.Empty;

        for (int i = 0; i < gvSizeMasterBin.Rows.Count; i++)
        {
            Label lblconid = (Label)gvSizeMasterBin.Rows[i].FindControl("lblRackId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvSizeMasterBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
        gvSizeMasterBin.BottomPagerRow.Focus();
    }
    protected void gvSizeMasterBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        string SortExpression = e.SortExpression;
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objSize.GetDeletedSizeMaster(Session["CompId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtSizeInactive"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)gvSizeMasterBin, dt, "", "");
        lblSelectedRecord.Text = "";
        //AllPageCode();
        gvSizeMasterBin.HeaderRow.Focus();
    }

    protected void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objSize.GetDeletedSizeMaster(Session["CompId"].ToString());
        try
        {
            if (dt.Rows.Count > 0)
            {
                objPageCmn.FillData((object)gvSizeMasterBin, dt, "", "");
                lblTotalRecordsBin.Text = "Total Records : " + dt.Rows.Count.ToString() + "";
                Session["dtSizeInactive"] = dt;
            }
        }
        catch
        {


        }

    }
    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
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


            DataTable dtCust = (DataTable)Session["dtSizeInactive"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtSizeInactive"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";

            objPageCmn.FillData((object)gvSizeMasterBin, view.ToTable(), "", "");
            lblSelectedRecord.Text = "";

        }
        txtValueBin.Focus();
    }
    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        FillGridBin();
        txtValueBin.Text = "";

    }
    protected void btnRestoreSelected_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int Msg = 0;
        if (gvSizeMasterBin.Rows.Count != 0)
        {
            foreach (GridViewRow row in gvSizeMasterBin.Rows)
            {
                CheckBox chkFiled = (CheckBox)row.FindControl("chkSelect");
                Label TransId = (Label)row.FindControl("lblgvBinSizeId");

                if (chkFiled.Checked)
                {
                    string Trans_Id = TransId.Text.ToString();
                    if (Trans_Id != "")
                    {
                        int i = 0;

                        // Convert Session variables to string explicitly if not already done
                        string companyId = Session["CompId"].ToString();
                        string userId = Session["UserId"].ToString();
                        DateTime currentDate = DateTime.Now;

                        // Assuming Trans_Id is passed as a variable
                        int transId = // provide value for Trans_Id here;

                         // Call the DeleteSizeMaster method with corrected parameters
                         i = objSize.DeleteSizeMaster(
                            companyId,
                            Trans_Id,
                            true.ToString(), // Assuming this parameter is an integer and is 0 by default for delete operation
                            userId,
                            currentDate.ToString("yyyy-MM-dd")
                        );
                    }
                }
            }
            DisplayMessage("Record Updated");
            FillGridBin();
            FillGrid();
        }
        txtValueBin.Focus();
    }
    #endregion




    protected void txtSizeCode_TextChanged(object sender, EventArgs e)
    {
        if (txtSizeCode.Text != "")
        {
            DataTable dt = objDa.return_DataTable("Select * from Set_SizeMaster where Size_Code='" + txtSizeCode.Text + "'");
            if (dt.Rows.Count > 0)
            {
                txtSizeCode.Text = "";
                txtSizeCode.Focus();
                DisplayMessage("Size Code is already exist");
                return;
            }
        }
        else
        {
            DisplayMessage("Please enter Size Code");
            return;
        }
    }
}