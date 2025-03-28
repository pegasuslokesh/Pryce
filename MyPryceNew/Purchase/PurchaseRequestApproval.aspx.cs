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
using System.Threading;
using System.ComponentModel;

public partial class Purchase_PurchaseRequestApproval : BasePage
{
    PurchaseRequestHeader ObjInvPurchaseRequest = null;
    PurchaseRequestDetail ObjPurchaseRequestDetail = null;
    SystemParameter ObjSysParam = null;
    Common cmn = null;
    Inv_ProductMaster ObjProductMaster = null;
    Inv_UnitMaster objUnit = null;
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
        else
        {
            StrUserId = Session["UserId"].ToString();
        }

        ObjInvPurchaseRequest = new PurchaseRequestHeader(Session["DBConnection"].ToString());
        ObjPurchaseRequestDetail = new PurchaseRequestDetail(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        objUnit = new Inv_UnitMaster(Session["DBConnection"].ToString());


        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "74", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            txtDescription.Visible = false;
            FillRequestGrid();
        }
        AllPageCode();
    }
    public void FillRequestGrid()
    {
        DataTable dt = new DataView(ObjInvPurchaseRequest.GetPurchaseRequestHeaderTrueAll(StrCompId, StrBrandId, strLocationId), "Status='Pending'", "", DataViewRowState.CurrentRows).ToTable();
        gvPurchaseRequest.DataSource = dt;
        gvPurchaseRequest.DataBind();
        lblTotalRecords.Text = Resources.Attendance.Total_Records__0 + dt.Rows.Count;
        Session["DtRecord"] = dt;
        Session["dtFilter_Pur_Req_A"] = dt;
        AllPageCode();
    }


    protected void btnApprove_Command(object sender, CommandEventArgs e)
    {
        int b = 0;


        b = ObjInvPurchaseRequest.UpdatePurchaseRequestHeaderApproval(e.CommandArgument.ToString(), StrCompId.ToString(), StrBrandId.ToString(), strLocationId.ToString(), "Approved", true.ToString(), false.ToString(), true.ToString(), StrUserId.ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            FillRequestGrid();
        }
        IbtnBack_Command(null, null);

    }
    protected void IbtnReject_Command(object sender, CommandEventArgs e)
    {

        int b = 0;

        b = ObjInvPurchaseRequest.UpdatePurchaseRequestHeaderApproval(e.CommandArgument.ToString(), StrCompId.ToString(), StrBrandId.ToString(), strLocationId.ToString(), "Rejected", false.ToString(), false.ToString(), true.ToString(), StrUserId.ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            FillRequestGrid();
        }
        IbtnBack_Command(null, null);

    }
    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        txtValue.Text = "";
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 3;
        txtValue.Focus();
        FillRequestGrid();
    }
    protected internal void IbtnDetail_Command(object sender, CommandEventArgs e)
    {
        EditId.Value = e.CommandArgument.ToString();
        DataTable dt = ObjPurchaseRequestDetail.GetPurchaseRequestDetailbyRequestId(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString());
        gvProductRequest.DataSource = dt;
        gvProductRequest.DataBind();
        foreach (GridViewRow Row in gvPurchaseRequest.Rows)
        {
            if (((ImageButton)Row.FindControl("IbtnDetail")).CommandArgument.ToString() != e.CommandArgument.ToString())
            {
                Row.Visible = false;
            }
            else
            {
                ((ImageButton)Row.FindControl("IbtnBack")).Visible = true;
            }
        }
        lblDescription.Text = Resources.Attendance.Description + " : ";
        txtDescription.Visible = true;
        txtDescription.Text = ObjInvPurchaseRequest.GetPurchaseRequestTrueAllByReqId(StrCompId, StrBrandId, strLocationId, e.CommandArgument.ToString()).Rows[0]["TermCondition"].ToString();
        ((ImageButton)sender).Visible = false;
        gvPurchaseRequest.HeaderRow.Cells[0].Text = Resources.Attendance.Back;
        pnlSearchRecords.Visible = false;
    }
    protected void IbtnBack_Command(object sender, CommandEventArgs e)
    {
        pnlSearchRecords.Visible = true;
        FillRequestGrid();
        gvProductRequest.DataSource = null;
        gvProductRequest.DataBind();
        pnlSearchRecords.Visible = true;
        gvPurchaseRequest.HeaderRow.Cells[0].Text = Resources.Attendance.Detail;
        lblDescription.Text = "";
        txtDescription.Text = "";
        txtDescription.Visible = false;
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
            DataTable dtCust = (DataTable)Session["DtPurchaseRequest"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Pur_Req_A"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            gvPurchaseRequest.DataSource = view.ToTable();
            gvPurchaseRequest.DataBind();
            AllPageCode();

            //btnbind.Focus();
            //btnRefresh.Focus();

        }
        txtValue.Focus();
    }

    protected void gvPurchaseRequest_Sorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Pur_Req_A"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Pur_Req_A"] = dt;
        gvPurchaseRequest.DataSource = dt;
        gvPurchaseRequest.DataBind();
        AllPageCode();
        gvPurchaseRequest.HeaderRow.Focus();

    }

    protected void gvPurchaseRequest_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPurchaseRequest.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Pur_Req_A"];
        gvPurchaseRequest.DataSource = dt;
        gvPurchaseRequest.DataBind();
        AllPageCode();
    }

    #region AllPageCode
    public void AllPageCode()
    {
        Page.Title = ObjSysParam.GetSysTitle();
        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        Session["AccordianId"] = "12";
        Session["HeaderText"] = "Purchase";
        DataTable dtAllPageCode = cmn.GetAllPagePermission(StrUserId.ToString(), "12", "74", HttpContext.Current.Session["CompId"].ToString());
        if (dtAllPageCode.Rows.Count != 0)
        {
            if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
            {
                foreach (GridViewRow Row in gvPurchaseRequest.Rows)
                {
                    ((ImageButton)Row.FindControl("IbtnApprove")).Visible = true;
                    ((ImageButton)Row.FindControl("IbtnReject")).Visible = true;
                    ((ImageButton)Row.FindControl("IbtnDetail")).Visible = true;
                }
            }
            else
            {
                foreach (DataRow DtRow in dtAllPageCode.Rows)
                {
                    if (Convert.ToBoolean(DtRow["Op_Add"].ToString()))
                    {
                    }
                    foreach (GridViewRow Row in gvPurchaseRequest.Rows)
                    {
                        if (Convert.ToBoolean(DtRow["Op_View"].ToString()))
                        {
                            ((ImageButton)Row.FindControl("IbtnDetail")).Visible = true;
                        }
                        if (Convert.ToBoolean(DtRow["Op_Add"].ToString()))
                        {
                            ((ImageButton)Row.FindControl("IbtnApprove")).Visible = true;
                            ((ImageButton)Row.FindControl("IbtnReject")).Visible = true;

                        }
                    }

                    if (Convert.ToBoolean(DtRow["Op_Restore"].ToString()))
                    {
                    }

                    if (Convert.ToBoolean(DtRow["Op_Print"].ToString()))
                    {

                    }
                }

            }
        }
        else
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }
    }
    #endregion

    public string ProductName(string ProductId, string TransId)
    {
        string ProductName = string.Empty;
        DataTable dt = new DataTable();
        try
        {
            dt = ObjProductMaster.GetProductMasterById(StrCompId.ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
        }
        catch
        {
        }
        if (dt.Rows.Count != 0)
        {
            ProductName = dt.Rows[0]["EProductName"].ToString();
        }
        else
        {


            DataTable DtPurchaseDetail = new DataTable();


            DtPurchaseDetail = ObjPurchaseRequestDetail.GetPurchaseRequestDetailbyTransIdandRequestId(StrCompId, StrBrandId, strLocationId, EditId.Value, TransId);
            try
            {
                ProductName = DtPurchaseDetail.Rows[0]["SuggestedProductName"].ToString();
            }
            catch
            {
            }



        }

        return ProductName;

    }
    public string ProductCode(string ProductId)
    {
        string ProductName = string.Empty;

        DataTable dt = ObjProductMaster.GetProductMasterById(StrCompId.ToString(), Session["BrandId"].ToString(), ProductId.ToString(), HttpContext.Current.Session["FinanceYearId"].ToString());
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
    public string UnitName(string UnitId)
    {
        string UnitName = string.Empty;
        DataTable dt = objUnit.GetUnitMasterById(StrCompId.ToString(), UnitId.ToString());
        if (dt.Rows.Count != 0)
        {
            UnitName = dt.Rows[0]["Unit_Name"].ToString();
        }
        return UnitName;
    }
    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            strNewDate = DateTime.Parse(strDate).ToString(HttpContext.Current.Session["DateFormat"].ToString());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }

}
