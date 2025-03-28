using PegasusDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class GeneralLedger_CostCenter : System.Web.UI.Page
{
    Ac_CostOfCenter Acc = null;
    PageControlCommon objPageCmn = null;
    DataAccessClass objda = null;
    //This Page is Created By Rahul Sharma On Date 19-Dec-2023 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        hdnCanEdit.Value = true.ToString();
        hdnCanDelete.Value = true.ToString();
        hdnCanView.Value = true.ToString();
        Acc = new Ac_CostOfCenter(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objda=new DataAccessClass(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            FillGrid();
            FillGridBin();
        }
            
    }

    #region List Section 
    public void FillGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = Acc.GetCOCAllTrue(Session["CompId"].ToString());
            if (dt.Rows.Count > 0)
            {
                lblTotalRecords.Text = "Total Records:"+dt.Rows.Count+"";
                Session["CostOfCenter"] = dt;
                objPageCmn.FillData((object)GvCostOfCenter, dt, "", "");
            }
        }
        catch(Exception ex)
        {

        }
    }
    protected void btnbindrpt_Click(object sender, EventArgs e)
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
            DataTable dtAdd = (DataTable)Session["CostOfCenter"];
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
           
            objPageCmn.FillData((object)GvCostOfCenter, view.ToTable(), "", "");

        }
    }
 
    protected void btnRefreshReport_Click(object sender, EventArgs e)
    {
        FillGrid();
    }
    protected void GvCOC_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GvCostOfCenter.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["CostOfCenter"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvCostOfCenter, dt, "", "");
    }
    protected void GvCOC_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt = (DataTable)Session["CostOfCenter"];
        string sortdir = "DESC";
        if (ViewState["SortDir"] != null)
        {
            sortdir = ViewState["SortDir"].ToString();
            if (sortdir == "ASC")
            {
                e.SortDirection = SortDirection.Descending;
                ViewState["SortDir"] = "DESC";
            }
            else
            {
                e.SortDirection = SortDirection.Ascending;
                ViewState["SortDir"] = "ASC";
            }
        }
        else
        {
            ViewState["SortDir"] = "DESC";
        }

        dt = (new DataView(dt, "", e.SortExpression + " " + ViewState["SortDir"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        Session["CostOfCenter"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GvCostOfCenter, dt, "", "");
    }

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        hdnEditId.Value = e.CommandArgument.ToString();
        DataTable dt = new DataTable();
        dt = Acc.GetCOCByTransId(hdnEditId.Value, Session["CompId"].ToString());
        if (dt.Rows.Count > 0)
        {
            txtCenterName.Text = dt.Rows[0]["CenterName"].ToString();
            txtCenterNameL.Text = dt.Rows[0]["CenterNameL"].ToString();
            txtCOA.Text= dt.Rows[0]["AccountName"].ToString()+"/"+ dt.Rows[0]["AccountId"].ToString();

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
    }

    protected void IbtnDelete_Command(object sender,CommandEventArgs e)
    {
        string Trans_Id = e.CommandArgument.ToString();
        objda.execute_Command("Update Ac_CostCenter  set IsActive='0' where Trans_Id="+Trans_Id+" ");
        FillGrid();
        FillGridBin();
        DisplayMessage("Record Deleted");
    }
    protected void lnkViewDetail_Command(object sender,CommandEventArgs e)
    {
        hdnEditId.Value = e.CommandArgument.ToString();
        btnSave.Visible = false;
        DataTable dt = new DataTable();
        dt = Acc.GetCOCByTransId(hdnEditId.Value, Session["CompId"].ToString());
        if (dt.Rows.Count > 0)
        {
            txtCenterName.Text = dt.Rows[0]["CenterName"].ToString();
            txtCenterNameL.Text = dt.Rows[0]["CenterNameL"].ToString();
            txtCOA.Text = dt.Rows[0]["AccountName"].ToString() + "/" + dt.Rows[0]["AccountId"].ToString();

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }

    }
    #endregion
    #region Bin Section
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
       

    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = Acc.GetCOCAllFalse(Session["CompId"].ToString());
        if(dt.Rows.Count>0)
        {
            lblTotalRecordsBin.Text = "Total Records: "+dt.Rows.Count+"";
            Session["CostOfCenterBin"] = dt;
            objPageCmn.FillData((object)GVCostCenterBin, dt, "", "");
        }
    }
    protected void chkCurrent_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)GVCostCenterBin.HeaderRow.FindControl("chkCurrent"));
        for (int i = 0; i < GVCostCenterBin.Rows.Count; i++)
        {
            ((CheckBox)GVCostCenterBin.Rows[i].FindControl("chkSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(GVCostCenterBin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(GVCostCenterBin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(GVCostCenterBin.Rows[i].FindControl("lblgvTransId"))).Text.Trim().ToString())
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
    protected void btnbindBin_Click(object sender,EventArgs e)
    {
        if (ddlOptionBin.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOptionBin.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String)='" + txtValueBin.Text.Trim() + "'";
            }
            else if (ddlOptionBin.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) like '%" + txtValueBin.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) Like '" + txtValueBin.Text.Trim() + "%'";
            }
            DataTable dtAdd = (DataTable)Session["CostOfCenterBin"];
            DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);

            objPageCmn.FillData((object)GvCostOfCenter, view.ToTable(), "", "");
        }
    }
    protected void btnRefreshBin_Click(object sender,EventArgs e)
    {
        FillGridBin(); 
    }
    protected void imgBtnRestore_Click(object sender,EventArgs e)
    {
        foreach(GridViewRow gvr in GVCostCenterBin.Rows)
        {           
            CheckBox chk = (CheckBox)gvr.FindControl("chkSelect");
            Label TransId = (Label)gvr.FindControl("lblgvTransId");
            if (chk.Checked)
            {
                objda.execute_Command("Update Ac_CostCenter  set IsActive='1' where Trans_Id=" + TransId.Text + " ");               
            }
        }
        GVCostCenterBin.DataSource = null;
        GVCostCenterBin.DataBind();
        FillGrid();
        FillGridBin();
        DisplayMessage("Restore Successfully");
    }
    #endregion
    #region Auto Complete
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCostOfAccountName(string prefixText, int count, string contextKey)
    {
        Ac_ChartOfAccount objCOA = new Ac_ChartOfAccount(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt1 = objCOA.GetCOAAllTrueForCOA(HttpContext.Current.Session["CompId"].ToString());       
        DataTable dt = new DataView(dt1, "AccountName like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["AccountName"].ToString() + "/" + dt.Rows[i]["Trans_Id"].ToString() + "";
            }
        }
        else
        {
            if (dt1.Rows.Count > 0)
            {
                txt = new string[dt1.Rows.Count];
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    txt[i] = dt1.Rows[i]["AccountName"].ToString() + "/" + dt1.Rows[i]["Trans_Id"].ToString() + "";
                }
            }
        }
        return txt;
    }
    #endregion
    #region New Tab Work for Create New Center
    protected void btnSave_Click(object sender,EventArgs e)
    {
        if (txtCenterName.Text == "")
        {
            DisplayMessage("Please Add Center Name");
        }
        if (txtCOA.Text == "")
        {
            DisplayMessage("Please Add Account");
        }
        if (hdnEditId.Value == "")
        {
            try
            {
                int i = Acc.InsertCOA(Session["CompId"].ToString(), txtCOA.Text.Split('/')[1].ToString(), txtCenterName.Text, txtCenterNameL.Text, "", "", "", "", "", true.ToString(), "" + DateTime.Now.ToString("yyyy-MM-dd") + "", true.ToString(), "" + Session["UserId"].ToString() + "", "" + DateTime.Now.ToString("yyyy-MM-dd") + "", "" + Session["UserId"].ToString() + "", "" + DateTime.Now.ToString("yyyy-MM-dd") + "");
                DisplayMessage("Record Save");
                Reset();
            }
            catch (Exception ex)
            {
                DisplayMessage("Record not Save");
            }
        }
        else
        {
            int i = Acc.UpdateCOA(hdnEditId.Value, txtCOA.Text.Split('/')[1].ToString(), txtCenterName.Text, txtCenterNameL.Text);
            DisplayMessage("Record Updated");
            Reset();

        }

    }
    protected void btnCancel_Click(object sender,EventArgs e)
    {
        Reset();
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        
    }
    public void Reset()
    {
        hdnEditId.Value = "";
        txtCenterName.Text = "";
        txtCenterNameL.Text = "";
        txtCOA.Text = "";
        FillGrid();
        FillGridBin();
    }
    protected void btnReset_Click(object sender,EventArgs e)
    {
        txtCenterName.Text = "";
        txtCenterNameL.Text = "";
        txtCOA.Text = "";
    }
    #endregion
    #region Common Function
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
    #endregion
}