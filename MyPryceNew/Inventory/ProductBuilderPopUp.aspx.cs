using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Threading;

public partial class Inventory_ProductBuilderPopUp : System.Web.UI.Page
{

    #region Defined Class Object
    BillOfMaterial ObjInvBOM = null;
    Inv_ProductMaster ObjInvProductMaster = null;
    Inv_ModelMaster ObjInvModelMaster = null;
    Inv_OptionCategoryMaster ObjOpCate = null;
    PageControlCommon objPageCmn = null;
    Common cmn = null;
    SystemParameter ObjSysParam = null;
    string StrCompId = string.Empty;
    string StrUserId = string.Empty;
    string StrBrandId = string.Empty;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        StrCompId = Session["CompId"].ToString();
        StrUserId = Session["UserId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        Session["AccordianId"] = "11";
        Session["HeaderText"] = "Inventory";


        ObjInvBOM = new BillOfMaterial(Session["DBConnection"].ToString());
        ObjInvProductMaster = new Inv_ProductMaster(Session["DBConnection"].ToString());
        ObjInvModelMaster = new Inv_ModelMaster(Session["DBConnection"].ToString());
        ObjOpCate = new Inv_OptionCategoryMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        Page.Title = ObjSysParam.GetSysTitle();

        AllPageCode();


    }
    #region AllPageCode
    public void AllPageCode()
    {
        Page.Title = ObjSysParam.GetSysTitle();
        StrUserId = Session["UserId"].ToString();
        StrCompId = Session["CompId"].ToString();
        Session["AccordianId"] = "11";
        Session["HeaderText"] = "Inventory";
        DataTable dtAllPageCode = cmn.GetAllPagePermission(StrUserId.ToString(), "11", "28", HttpContext.Current.Session["CompId"].ToString());
        if (dtAllPageCode.Rows.Count == 0)
        {

            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }
    }
    #endregion


    #region System Defined Function:-Event







    protected void txtModelNo_TextChanged(object sender, EventArgs e)
    {
        if (txtModelNo.Text != "")
        {
            try
            {
                DataTable dt = new DataView(ObjInvModelMaster.GetModelMaster(StrCompId.ToString(), StrBrandId, "True"), "Model_No='" + txtModelNo.Text.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count != 0)
                {
                    lblPrice.Text = SystemParameter.GetCurrencySmbol(dt.Rows[0]["Field4"].ToString(), Resources.Attendance.Price, Session["DBConnection"].ToString());
                    fillDataByModel(dt);
                    foreach (GridViewRow OPRow in gvOptionCategory.Rows)
                    {

                        RadioButtonList RdoList = (RadioButtonList)OPRow.FindControl("rdoOption");
                        foreach (ListItem li in RdoList.Items)
                        {
                            if (li.Selected)
                            {
                                rdoOption_SelectedIndexChanged(((object)RdoList), e);
                            }
                        }
                    }
                }
                else
                {
                    txtModelNo.Text = "";
                    DisplayMessage("Select Model Name");
                    txtModelNo.Focus();

                }
            }
            catch
            {

            }
        }


    }

    protected void txtModelName_TextChanged(object sender, EventArgs e)
    {
        if (txtModelName.Text != "")
        {
            try
            {
                DataTable dt = new DataView(ObjInvModelMaster.GetModelMaster(StrCompId.ToString(), StrBrandId, "True"), "Model_Name='" + txtModelName.Text.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count != 0)
                {
                    lblPrice.Text = SystemParameter.GetCurrencySmbol(dt.Rows[0]["Field4"].ToString(), Resources.Attendance.Price, Page.Title = ObjSysParam.GetSysTitle());



                    fillDataByModel(dt);
                    foreach (GridViewRow OPRow in gvOptionCategory.Rows)
                    {

                        RadioButtonList RdoList = (RadioButtonList)OPRow.FindControl("rdoOption");
                        foreach (ListItem li in RdoList.Items)
                        {
                            if (li.Selected)
                            {
                                rdoOption_SelectedIndexChanged(((object)RdoList), e);
                            }
                        }
                    }
                }
                else
                {
                    txtModelNo.Text = "";
                    DisplayMessage("Select Model Name");
                    txtModelNo.Focus();

                }
            }
            catch
            {

            }
        }


    }


    protected void gvOptionCategory_DataBound(object sender, EventArgs e)
    {
        foreach (GridViewRow GvOptrow in gvOptionCategory.Rows)
        {
            try
            {
                string ModelId = ViewState["ModelId"].ToString();
                RadioButtonList RdoList = (RadioButtonList)GvOptrow.FindControl("rdoOption");
                Label lblOpCatiD = (Label)GvOptrow.FindControl("lblOptionCategoryId");
                DataTable dtOption = new DataView(ObjInvBOM.BOM_ByModelId(StrCompId.ToString(), ModelId.ToString()), "OptionCategoryId='" + lblOpCatiD.Text.Trim() + "'", "TransId Asc", DataViewRowState.CurrentRows).ToTable();
                foreach (DataRow row in dtOption.Rows)
                {
                    var txt = string.Empty;
                    string ProductName = string.Empty;
                    bool b = false;
                    if (row["SubProductId"].ToString() == "0")
                    {

                        txt = row["ShortDescription"].ToString();
                    }
                    else
                    {
                        ProductName = new DataView(ObjInvProductMaster.GetProductMasterTrueAll(StrCompId.ToString(), Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["FinanceYearId"].ToString()), "ProductId = '" + row["SubProductId"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["EProductName"].ToString();
                        txt = ProductName.ToString() + "," + row["ShortDescription"].ToString();
                    }
                    try
                    {
                        if (Convert.ToBoolean(row["PDefault"].ToString()))
                        {
                            b = true;
                        }
                    }
                    catch
                    {

                    }
                    var val = row["OptionId"].ToString();
                    var item = new ListItem(txt, val);
                    item.Selected = b;
                    RdoList.Items.Add(item);

                }
            }
            catch
            {

            }


        }
    }
    protected void txtProductPartNo_TextChanged(object sender, EventArgs e)
    {
        if (txtProductPartNo.Text != "")
        {

            bool b = true;

            string str = string.Empty;
            string ProductPartNo = string.Empty;
            string ProductId = string.Empty;

            for (int i = txtProductPartNo.Text.Length; i > 0; i--)
            {
                if (b)
                {
                    if (!txtProductPartNo.Text[i - 1].ToString().Contains("-"))
                    {

                        str = txtProductPartNo.Text[i - 1].ToString() + str;


                    }
                    else
                    {
                        b = false;

                    }
                }
                else
                {

                    ProductId = txtProductPartNo.Text[i - 1].ToString() + ProductId;


                }
            }

            try
            {
                txtProductPartNo.Text = "";
                txtOptionPartNo.Text = "";
                txtProductPartNo.Text = ProductPartNo.Trim();
                txtOptionPartNo.Text = str.Trim();

                DataTable dt = new DataView(ObjInvModelMaster.GetModelMaster(StrCompId.ToString(), StrBrandId, "True"), "Model_No='" + ProductId.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt.Rows.Count != 0)
                {
                    fillDataByModel(dt);
                    fillOptionCategorygrid();
                    // fillpart();
                    tblgrid.Visible = true;
                }
            }
            catch
            {

            }

            txtOptionPartNo_TextChanged(null, null);


        }
    }
    protected void txtOptionPartNo_TextChanged(object sender, EventArgs e)
    {


        DataTable dt = new DataTable();
        DataTable dtTemp = new DataTable();
        fillOptionCategorygrid();
        string ModelId = string.Empty;
        if (txtModelNo.Text != "0")
        {
            ModelId = new DataView(ObjInvModelMaster.GetModelMaster(StrCompId.ToString(), StrBrandId, "True"), "Model_No like '" + txtModelNo.Text.Trim() + "%'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Trans_Id"].ToString();
        }
        bool b = true;
        if (txtOptionPartNo.Text != "")
        {
            for (int i = 0; i < txtOptionPartNo.Text.Length; i++)
            {
                char c = txtOptionPartNo.Text[i];

                dtTemp = new DataView(ObjInvBOM.BOM_ByModelId(StrCompId.ToString(), ModelId.Trim()), "OptionId='" + c.ToString() + "'and Field1='" + (i + 1) + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtTemp.Rows.Count != 0)
                {
                    dt.Merge(dtTemp);
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        dtTemp = null;
                        dtTemp = new DataView(dt, "OptionCategoryId='" + dt.Rows[j]["OptionCategoryId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtTemp.Rows.Count == 2)
                        {
                            b = false;
                        }
                    }

                }
                else
                {
                    dtTemp = new DataView(ObjInvBOM.BOM_ByModelId(StrCompId.ToString(), ModelId.Trim()), "Field1='" + (i + 1) + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtTemp.Rows.Count != 0)
                    {

                        if (c != '0')
                        {
                            b = false;
                        }
                        else
                        {
                            dt.Rows.Add();

                        }
                    }

                    else
                    {
                        b = false;
                    }
                }
            }

            if (!b)
            {
                DisplayMessage("Invalid Part No");
                txtOptionPartNo.Text = "";
                txtPrice.Text = "0";
                txtOptionPartNo.Focus();
                fillOptionCategorygrid();
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    RadioButtonList RdoList = (RadioButtonList)gvOptionCategory.Rows[i].FindControl("rdoOption");
                    for (int j = 0; j < RdoList.Items.Count; j++)
                    {
                        if (RdoList.Items[j].Value == dt.Rows[i]["OptionId"].ToString())
                        {
                            RdoList.Items[j].Selected = true;
                            txtDesc.Text += "," + RdoList.Items[j].Text.Trim();
                            txtPrice.Text = (Convert.ToInt32(txtPrice.Text) + float.Parse(dt.Rows[i]["UnitPrice"].ToString()) * float.Parse(dt.Rows[i]["Quantity"].ToString())).ToString();
                        }




                    }


                }
                txtPrice.Text = ObjSysParam.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), txtPrice.Text.Trim());


            }
        }
    }



    protected void btnReset_Click(object sender, EventArgs e)
    {

        tblgrid.Visible = false;
        txtModelName.Text = "";
        txtModelNo.Text = "";
        txtProductPartNo.Text = "";
        txtOptionPartNo.Text = "";
        gvOptionCategory.DataSource = null;
        gvOptionCategory.DataBind();
        txtModelNo.Focus();
        txtDesc.Text = "";
        txtPrice.Text = "";

    }
    public void somethingToRunInThread()
    {
        System.Windows.Forms.Clipboard.SetText(txtProductPartNo.Text.Trim() + "-" + txtOptionPartNo.Text.Trim());
    }

    protected void copy_to_clipboard()
    {
        Thread clipboardThread = new Thread(somethingToRunInThread);
        clipboardThread.SetApartmentState(ApartmentState.STA);
        clipboardThread.IsBackground = false;
        clipboardThread.Start();
    }

    protected void btnCopyPartNo_Click(object sender, EventArgs e)
    {
        copy_to_clipboard();
    }


    #endregion

    #region AutoComplete Method
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListModelNo(string prefixText, int count, string contextKey)
    {
        Inv_ModelMaster ObjInvModelMaster = new Inv_ModelMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjInvModelMaster.GetModelMaster(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "True");
        DataTable dtTemp = dt.Copy();

        dt = new DataView(dt, "Model_No like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count == 0)
        {
            dt = dtTemp.Copy();

        }

        string[] txt = new string[dt.Rows.Count];


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Model_No"].ToString();
        }


        return txt;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListModelName(string prefixText, int count, string contextKey)
    {
        Inv_ModelMaster ObjInvModelMaster = new Inv_ModelMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = ObjInvModelMaster.GetModelMaster(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), "True");
        DataTable dtTemp = dt.Copy();

        dt = new DataView(dt, "Model_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        if (dt.Rows.Count == 0)
        {
            dt = dtTemp.Copy();

        }

        string[] txt = new string[dt.Rows.Count];


        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Model_Name"].ToString();
        }


        return txt;
    }

    #endregion


    #region User Defined Function


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


    public void fillOptionCategorygrid()
    {
        try
        {
            string ModelId = ViewState["ModelId"].ToString();

            DataTable dt = ObjInvBOM.BOM_ByModelId(StrCompId.ToString(), ModelId.ToString());
            dt = new DataView(dt, "", "Field1 Asc", DataViewRowState.CurrentRows).ToTable(true, "OptionCategoryId");
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvOptionCategory, dt, "", "");



            if (dt.Rows.Count == 0)
            {

                txtOptionPartNo.Text = "";
                txtDesc.Text = "";
            }
            else
            {
                if (txtOptionPartNo.Text == "")
                {
                    txtOptionPartNo.Text = "0".PadRight(dt.Rows.Count, '0');

                }
            }
        }
        catch
        {

        }
    }

    public string GetOpCateName(string OpCatId)
    {
        string OpCateName = string.Empty;
        try
        {
            OpCateName = ObjOpCate.GetOptionCategoryTruebyId(StrCompId.ToString(), OpCatId.ToString()).Rows[0]["EName"].ToString();
        }
        catch
        {

        }
        return OpCateName;
    }




    //Updated


    protected void rdoOption_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow Row = (GridViewRow)((RadioButtonList)sender).Parent.Parent;

        Label lblCategoryId = (Label)Row.FindControl("lblOptionCategoryId");
        RadioButtonList RdoList = (RadioButtonList)Row.FindControl("rdoOption");

        string PartNo = string.Empty;
        string Desc = string.Empty;
        float Price = 0;
        string OldPartOption = string.Empty;
        string value = string.Empty;
        string ModelId = ViewState["ModelId"].ToString();
        string strsno = string.Empty;
        DataTable dtOption = new DataView(ObjInvBOM.BOM_ByModelId(StrCompId.ToString(), ModelId.ToString()), "OptionCategoryId='" + lblCategoryId.Text.Trim() + "'", "TransId  Asc", DataViewRowState.CurrentRows).ToTable();

        for (int i = 0; i < RdoList.Items.Count; i++)
        {

            if (RdoList.Items[i].Selected)
            {
                DataTable dtOptionTemp = new DataView(dtOption, "OptionId='" + RdoList.Items[i].Value.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                string UnitPrice = dtOptionTemp.Rows[0]["UnitPrice"].ToString();
                Price += float.Parse(UnitPrice) * float.Parse(dtOptionTemp.Rows[0]["Quantity"].ToString());
                Desc += "," + dtOptionTemp.Rows[0]["ShortDescription"].ToString();

                strsno = new DataView(dtOption, "OptionId='" + RdoList.Items[i].Value + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Field1"].ToString();
                OldPartOption = txtOptionPartNo.Text[Convert.ToInt32(strsno) - 1].ToString();

                txtOptionPartNo.Text = txtOptionPartNo.Text.Remove(Convert.ToInt32(strsno) - 1, 1);
                txtOptionPartNo.Text = txtOptionPartNo.Text.Insert(Convert.ToInt32(strsno) - 1, RdoList.Items[i].Value);


                try
                {
                    dtOptionTemp = new DataView(dtOption, "OptionId='" + OldPartOption.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    txtDesc.Text = txtDesc.Text.Replace("," + dtOptionTemp.Rows[0]["ShortDescription"].ToString(), "");
                    txtPrice.Text = (float.Parse(txtPrice.Text.ToString()) - (float.Parse(dtOptionTemp.Rows[0]["UnitPrice"].ToString()) * float.Parse(dtOptionTemp.Rows[0]["Quantity"].ToString()))).ToString();
                }
                catch
                {

                }
            }



        }
        if (txtPrice.Text == "")
        {
            txtPrice.Text = "0";
        }

        txtPrice.Text = (float.Parse(txtPrice.Text.ToString()) + Price).ToString();
        txtPrice.Text = ObjSysParam.GetCurencyConversionForInv(Session["CurrencyId"].ToString(), txtPrice.Text.Trim());
        txtDesc.Text += Desc;


    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        tblgrid.Visible = false;
        btnReset_Click(null, null);

        AllPageCode();
    }
    public void fillDataByModel(DataTable dt)
    {
        try
        {
            txtModelNo.Text = dt.Rows[0]["Model_No"].ToString();

            txtModelName.Text = dt.Rows[0]["Model_Name"].ToString();
            txtProductPartNo.Text = dt.Rows[0]["Model_No"].ToString();
            txtPrice.Text = dt.Rows[0]["Field1"].ToString();
            txtDesc.Text = dt.Rows[0]["Field3"].ToString();
            txtOptionPartNo.Focus();
            ViewState["ModelId"] = dt.Rows[0]["Trans_Id"].ToString();

            fillOptionCategorygrid();
            // fillpart();
            tblgrid.Visible = true;
        }
        catch
        {
        }
    }
    #endregion



}
