using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Inventory_Magic_Stock_Analysis : System.Web.UI.Page
{
    Inv_StockDetail objStockDetail = null;
    Common cmn = null;
    Inv_ProductMaster objProductMaster = null;
    SystemParameter ObjSysParam = null;
    Inv_ProductLedger ObjProductLadger = null;
    Inventory_Common objinvCommon = null;
    Inv_ModelMaster ObjModelMaster = null;
    LocationMaster ObjLocationMaster = null;
    RoleMaster objRole = null;
    RoleDataPermission objRoleData = null;
    Set_Location_Department objLocDept = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objStockDetail = new Inv_StockDetail(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjProductLadger = new Inv_ProductLedger(Session["DBConnection"].ToString());
        objinvCommon = new Inventory_Common(Session["DBConnection"].ToString());
        ObjModelMaster = new Inv_ModelMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());


        if (!IsPostBack)
        {
            //ProductId
            //Type
            //Contact
            FillRefTye();
            FillddlLocation();
            getProductDetail(Request.QueryString["ProductId"].ToString(), Request.QueryString["Type"].ToString(), Request.QueryString["Contact"].ToString());

        }
    }

    public void getProductDetail(string strProductId, string strtype, string ContactId)
    {
        if (objProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), strProductId, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows.Count == 0)
        {
            return;
        }


        //first we get product info from product master table 
        lblproductid.Text = objProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), strProductId, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["ProductCode"].ToString();
        lblProductName.Text = objProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), strProductId, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["EProductName"].ToString();
        lblModel.Text = getModelNo(objProductMaster.GetProductMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString(), strProductId, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["ModelNo"].ToString());



        //then we get product stock and product current qty in sales order ,purchase order ,delivery etc 


        objPageCmn.FillData((object)gvStockInfo, objinvCommon.GetProductDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strProductId, HttpContext.Current.Session["FinanceYearId"].ToString()), "", "");

        DataTable dt = new DataView(ObjProductLadger.GetProductLedgerAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "ProductId=" + strProductId + "", "", DataViewRowState.CurrentRows).ToTable();


        if (strtype.Trim() == "SALES")
        {
            Ddlreftype.SelectedValue = "SI";

        }
        else if (strtype == "PURCHASE")
        {
            Ddlreftype.SelectedValue = "PG";

            //here we modify code for show cost price according the login user permission
            //code created by jitendra on 13-08-2016
            try
            {
                gvProductLadger.Columns[3].Visible = Convert.ToBoolean(Inventory_Common.CheckUserPermission("24","20", Session["DBConnection"].ToString(), HttpContext.Current.Session["EmpId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()));

                gvProductLadger.Columns[5].Visible = Convert.ToBoolean(Inventory_Common.CostPermission(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LOcId"].ToString(), Session["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()));
            }
            catch
            {

            }
        }

        FillGrid();




    }


    public void FillGrid()
    {

        DataTable dt = new DataTable();

        if (ddlLocation.SelectedIndex == 0)
        {
            dt = new DataView(ObjProductLadger.GetProductLedgerByCompanyIdandBrandId(Session["CompId"].ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "ProductId=" + Request.QueryString["ProductId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

            string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

            if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
            {
                if (LocIds != "")
                {
                    dt = new DataView(dt, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
        }
        else
        {
            dt = new DataView(ObjProductLadger.GetProductLedgerAll(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, HttpContext.Current.Session["FinanceYearId"].ToString()), "ProductId=" + Request.QueryString["ProductId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

        }

        //FOR FILTER BY REF TYPE 
        if (Ddlreftype.SelectedIndex > 0)
        {
            dt = new DataView(dt, "TransType='" + Ddlreftype.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }



        if (txtValue.Text != "")
        {
            if (ddlOption.SelectedIndex != 0)
            {
                string condition = string.Empty;
                if (ddlOption.SelectedIndex == 1)
                {
                    if (ddlFieldName.SelectedValue == "AlternateId")
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + "1" + ",System.String)='" + txtValue.Text.Trim() + "'";
                        condition = condition + " or " + "convert(" + ddlFieldName.SelectedValue + "2" + ",System.String)='" + txtValue.Text.Trim() + "'";
                        condition = condition + " or " + "convert(" + ddlFieldName.SelectedValue + "3" + ",System.String)='" + txtValue.Text.Trim() + "'";

                    }
                    else
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
                    }
                }
                else if (ddlOption.SelectedIndex == 2)
                {
                    if (ddlFieldName.SelectedValue == "AlternateId")
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + "1" + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
                        condition = condition + " or " + "convert(" + ddlFieldName.SelectedValue + "2" + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
                        condition = condition + " or " + "convert(" + ddlFieldName.SelectedValue + "3" + ",System.String) like '%" + txtValue.Text.Trim() + "%'";

                    }
                    else
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
                    }
                }
                else
                {
                    if (ddlFieldName.SelectedValue == "AlternateId")
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + "1" + ",System.String) like '" + txtValue.Text.Trim() + "%'";
                        condition = condition + " or " + "convert(" + ddlFieldName.SelectedValue + "2" + ",System.String) like '" + txtValue.Text.Trim() + "%'";
                        condition = condition + " or " + "convert(" + ddlFieldName.SelectedValue + "3" + ",System.String) like '" + txtValue.Text.Trim() + "%'";

                    }
                    else
                    {
                        condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '%" + txtValue.Text.Trim() + "%'";
                    }
                }

                DataView view = new DataView(dt, condition, "", DataViewRowState.CurrentRows);

                dt = view.ToTable();
                btnRefresh.Focus();
            }
        }



        try
        {

            dt = new DataView(dt, "", "ModifiedDate desc", DataViewRowState.CurrentRows).ToTable().Rows.Cast<System.Data.DataRow>().Take(30).CopyToDataTable();
        }
        catch
        {
        }




        objPageCmn.FillData((object)gvProductLadger, dt, "", "");

        Session["dtStockAnalysis"] = dt;
    }

    public string getModelNo(string ModelId)
    {
        string modelNo = string.Empty;
        if (ModelId == "")
        {
            ModelId = "0";
        }
        DataTable dtModel = ObjModelMaster.GetModelMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), ModelId, "True");
        if (dtModel.Rows.Count != 0)
        {

            modelNo = dtModel.Rows[0]["Model_No"].ToString();
        }
        return modelNo;
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
    public string SetDecimal(string amount, string CurrencyID)
    {
        if (amount == "")
        {
            amount = "0";
        }
        return ObjSysParam.GetCurencyConversionForInv(CurrencyID, amount);

    }

    protected void btnbind_Click(object sender, EventArgs e)
    {
        FillGrid();
        txtValue.Focus();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        txtValue.Text = "";
        ddlFieldName.SelectedIndex = 0;
        ddlOption.SelectedIndex = 3;
        ddlLocation.SelectedValue = Session["LocId"].ToString();
        getProductDetail(Request.QueryString["ProductId"].ToString(), Request.QueryString["Type"].ToString(), Request.QueryString["Contact"].ToString());
    }

    public void FillRefTye()
    {
        DataTable dt = ObjProductLadger.GetProductLedgerAll();

        dt = new DataView(dt, "", "", DataViewRowState.CurrentRows).ToTable(true, "Ref_Type", "TransType");

        Ddlreftype.DataSource = dt;
        Ddlreftype.DataTextField = "Ref_Type";
        Ddlreftype.DataValueField = "TransType";
        Ddlreftype.DataBind();

        Ddlreftype.Items.Insert(0, "ALL");
    }

    private void FillddlLocation()
    {

        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }


        }


        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.DataSource = null;
            ddlLocation.DataBind();

            ddlLocation.DataSource = dtLoc;
            ddlLocation.DataTextField = "Location_Name";
            ddlLocation.DataValueField = "Location_Id";
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, "All");
            ddlLocation.SelectedValue = Session["LocId"].ToString();

        }
        else
        {
            try
            {
                ddlLocation.Items.Clear();
                ddlLocation.DataSource = null;
                ddlLocation.DataBind();
                ListItem li = new ListItem(Resources.Attendance.__Select__, "0");
                ddlLocation.Items.Insert(0, li);
                ddlLocation.SelectedIndex = 0;
            }
            catch
            {
                ListItem li = new ListItem(Resources.Attendance.__Select__, "0");
                ddlLocation.Items.Insert(0, li);
                ddlLocation.SelectedIndex = 0;
            }
        }
    }




    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListContact(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster objcontact = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());


        DataTable dtContact = objcontact.GetContactTrueAllData();


        DataTable dtMain = new DataTable();
        dtMain = dtContact.Copy();


        string filtertext = "Name like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "", DataViewRowState.CurrentRows).ToTable();

        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString();
            }
        }
        return filterlist;
    }


    public bool IsObjectPermission(string ModuelId, string ObjectId)
    {
        bool Result = false;

        if (Session["EmpId"].ToString() == "0")
        {
            Result = true;
        }
        else
        {
            if (cmn.GetAllPagePermission(Session["UserId"].ToString(), ModuelId, ObjectId, HttpContext.Current.Session["CompId"].ToString()).Rows.Count > 0)
            {
                Result = true;
            }
        }
        return Result;
    }


    protected void lnkRef_Click(object sender, EventArgs e)
    {
        LinkButton myButton = (LinkButton)sender;

        string[] arguments = myButton.CommandArgument.ToString().Split(new char[] { ',' });
        string RefId = arguments[0];
        string RefType = arguments[1];
        string RefTypeId = arguments[2].Trim();
        string LocationId = arguments[3].Trim();
        if (RefId != "" && RefId != "")
        {
            //    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('ProductLedgerRefTypeView.aspx?RefType=" + RefType + "&RefId=" + RefId + "','s','height=650,width=900,scrollbars=Yes');", true);


            if (RefType == "Purchase Return")
            {
                if (IsObjectPermission("143", "53"))
                {

                    string strCmd = string.Format("window.open('../Purchase/PurchaseReturn.aspx?Id=" + RefTypeId + "&LocId=" + LocationId + "','window','width=1024, ');");
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", strCmd, true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }
            }
            else if (RefType == "Purchase Goods")
            {
                if (IsObjectPermission("143", "58"))
                {
                    string strCmd = string.Format("window.open('../Purchase/PurchaseGoodsRec.aspx?Id=" + RefTypeId + "&LocId=" + LocationId + "','window','width=1024, ');");
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", strCmd, true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }
            }
            else if (RefType == "Sales Return")
            {
                if (IsObjectPermission("144", "120"))
                {
                    string strCmd = string.Format("window.open('../Sales/SalesReturn.aspx?Id=" + RefTypeId + "&LocId=" + LocationId + "','window','width=1024, ');");
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", strCmd, true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }
            }
            else if (RefType == "Sales Invoice")
            {
                if (IsObjectPermission("144", "92"))
                {
                    string strCmd = string.Format("window.open('../Sales/SalesInvoice.aspx?Id=" + RefTypeId + "&LocId=" + LocationId + "','window','width=1024, ');");

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "New", strCmd, true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }
                //SalesInvoiceDetailnew(RefId);
            }
            else if (RefType == "Transfer Out")
            {
                if (IsObjectPermission("142", "94"))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/Transferoutreport.aspx?TransId=" + RefTypeId + "&&Type=TO&LocId=" + LocationId + "','window','width=1024');", true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }

            }
            else if (RefType == "Transfer IN")
            {
                if (IsObjectPermission("142", "118"))
                {
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Inventory_Report/Transferoutreport.aspx?TransId=" + RefTypeId + "&&Type=TI&LocId=" + LocationId + "','window','width=1024');", true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }
            }

            else if (RefType == "Stock Adjustment")
            {
                if (IsObjectPermission("142", "131"))
                {
                    string strCmd = string.Format("window.open('../Inventory/StockAdjustment.aspx?Id=" + RefTypeId + "&LocId=" + LocationId + "','window','width=1024, ');");
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", strCmd, true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }

            }

            else if (RefType == "Delivery Voucher")
            {
                if (IsObjectPermission("144", "327"))
                {
                    string strCmd = string.Format("window.open('../Sales/DeliveryVoucher.aspx?Id=" + RefTypeId + "&LocId=" + LocationId + "','window','width=1024, ');");
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", strCmd, true);
                }
                else
                {
                    DisplayMessage("You have no permission for view detail");
                    return;
                }
            }
        }
        else
        {
            //myButton.Attributes.Add("
            DisplayMessage("No Data");
            return;
        }


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
        DataTable dtres = (DataTable)Session["MessageDt"];
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



}