using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI;

public partial class WebUserControl_ContactNo : System.Web.UI.UserControl
{
    public delegate void parentHandler();
    public event parentHandler ExpandNumberDiv;

    LocationMaster objLocation = null;
    Country_Currency objCountryCurrency = null;
    CountryMaster ObjSysCountryMaster = null;
    ContactNoMaster objContactnoMaster = null;
    Common ObjComman = null;


    protected void Page_Load(object sender, EventArgs e)
    {
        //FillCountryCode();
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objCountryCurrency = new Country_Currency(Session["DBConnection"].ToString());
        ObjSysCountryMaster = new CountryMaster(Session["DBConnection"].ToString());
        objContactnoMaster = new ContactNoMaster(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
    }

    public void btnAddNum_Click(object sender, EventArgs e)
    {
        Btn_Div_Add_Num.Attributes.Add("Class", "fa fa-minus");
        Div_Add_Num.Attributes.Add("Class", "box box-primary");

        DataTable dt = new DataTable();
        string type, code, number, extension;
        CheckBox isdefault;

        if (txtNumber.Text.Trim() == "")
        {
            txtNumber.Focus();
            return;
        }

        dt.Columns.AddRange(new DataColumn[5] { new DataColumn("Type"), new DataColumn("Country_code"), new DataColumn("Phone_no"), new DataColumn(("Is_default"), typeof(bool)), new DataColumn("Extension_no") });

        if (GvNumber.Rows.Count > 0)
        {
            for (int i = 0; i < GvNumber.Rows.Count; i++)
            {
                type = (GvNumber.Rows[i].FindControl("lbltype") as Label).Text;
                code = (GvNumber.Rows[i].FindControl("lblCode") as Label).Text;
                number = (GvNumber.Rows[i].FindControl("lblNum") as Label).Text;
                isdefault = (GvNumber.Rows[i].FindControl("ChkDefault") as CheckBox);
                extension = (GvNumber.Rows[i].FindControl("lblExtension") as Label).Text;
                if (txtNumber.Text.Trim() == number)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('No Already Exists');", true);
                    txtNumber.Text = "";
                    txtNumber.Focus();
                    return;
                }
                dt.Rows.Add(type, code, number, isdefault.Checked, extension);
            }
        }

        if (GvNumber.Rows.Count == 0)
        {
            dt.Rows.Add(ddlContactType.SelectedItem, txtCountryCode.Text, txtNumber.Text, true, txtExtensionNumber.Text);
        }
        else
        {
            dt.Rows.Add(ddlContactType.SelectedItem, txtCountryCode.Text, txtNumber.Text, false, txtExtensionNumber.Text);
        }
            
        GvNumber.DataSource = dt;
        GvNumber.DataBind();
        Session["datatable_ContactNo"] = dt;
        txtNumber.Text = "";
        ddlContactType.SelectedIndex = 0;
        //ddlCountryCode_Phoneno1.SelectedValue = "+" + ViewState["CountryCode"].ToString();
        txtExtensionNumber.Text = "";
        dt = null;
        try
        {
            if (this.ExpandNumberDiv != null)
            {
                ExpandNumberDiv();
            }
        }
        catch (Exception ex)
        {

        }
        
    }

    protected void GvNumber_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        Btn_Div_Add_Num.Attributes.Add("Class", "fa fa-minus");
        Div_Add_Num.Attributes.Add("Class", "box box-primary");

        DataTable dt = new DataTable();
        dt.Columns.AddRange(new DataColumn[5] { new DataColumn("Type"), new DataColumn("Country_code"), new DataColumn("Phone_no"), new DataColumn(("Is_default"), typeof(bool)), new DataColumn("Extension_no") });
        string type, code, number, extension;
        CheckBox isdefault;

        for (int i = 0; i < GvNumber.Rows.Count; i++)
        {
            type = (GvNumber.Rows[i].FindControl("lbltype") as Label).Text;
            code = (GvNumber.Rows[i].FindControl("lblCode") as Label).Text;
            number = (GvNumber.Rows[i].FindControl("lblNum") as Label).Text;
            isdefault = (GvNumber.Rows[i].FindControl("ChkDefault") as CheckBox);
            extension = (GvNumber.Rows[i].FindControl("lblExtension") as Label).Text;
            dt.Rows.Add(type, code, number, isdefault.Checked, extension);
        }

        dt.Rows.RemoveAt(e.RowIndex);
        GvNumber.DataSource = dt;
        GvNumber.DataBind();
        Session["datatable_ContactNo"] = dt;
        dt = null;
        //ScriptManager.RegisterStartupScript(this, GetType(), "", "Open_Div_Mobile_Number();", true);
    }

    protected void ChkDefault_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;

        for (int i = 0; i < GvNumber.Rows.Count; i++)
        {
            chk = GvNumber.Rows[i].FindControl("ChkDefault") as CheckBox;

            if (i == index)
            {
                if (!chk.Checked)
                {
                    chk.Checked = false;
                }
            }
            else
            {
                chk.Checked = false;
            }
        }

        DataTable dt = new DataTable();
        dt.Columns.AddRange(new DataColumn[5] { new DataColumn("Type"), new DataColumn("Country_code"), new DataColumn("Phone_no"), new DataColumn(("Is_default"), typeof(bool)), new DataColumn("Extension_no") });
        string type, code, number, extension;
        CheckBox isdefault;

        for (int i = 0; i < GvNumber.Rows.Count; i++)
        {
            type = (GvNumber.Rows[i].FindControl("lbltype") as Label).Text;
            code = (GvNumber.Rows[i].FindControl("lblCode") as Label).Text;
            number = (GvNumber.Rows[i].FindControl("lblNum") as Label).Text;
            isdefault = (GvNumber.Rows[i].FindControl("ChkDefault") as CheckBox);
            extension = (GvNumber.Rows[i].FindControl("lblExtension") as Label).Text;
            dt.Rows.Add(type, code, number, isdefault.Checked, extension);
        }

        Session["datatable_ContactNo"] = dt;

        Btn_Div_Add_Num.Attributes.Add("Class", "fa fa-minus");
        Div_Add_Num.Attributes.Add("Class", "box box-primary");
        dt = null;
        //ScriptManager.RegisterStartupScript(this, GetType(), "", "Open_Div_Mobile_Number();", true);
    }

    public DataTable getDatatable()
    {
        DataTable dt = new DataTable();
        string type, code, number, extension;
        CheckBox isdefault;
        dt.Columns.AddRange(new DataColumn[5] { new DataColumn("Type"), new DataColumn("Country_code"), new DataColumn("Phone_no"), new DataColumn(("Is_default"), typeof(bool)), new DataColumn("Extension_no") });
        if (GvNumber.Rows.Count > 0)
        {
            for (int i = 0; i < GvNumber.Rows.Count; i++)
            {
                type = (GvNumber.Rows[i].FindControl("lbltype") as Label).Text;
                code = (GvNumber.Rows[i].FindControl("lblCode") as Label).Text;
                number = (GvNumber.Rows[i].FindControl("lblNum") as Label).Text;
                isdefault = (GvNumber.Rows[i].FindControl("ChkDefault") as CheckBox);
                extension = (GvNumber.Rows[i].FindControl("lblExtension") as Label).Text;
                dt.Rows.Add(type, code, number, isdefault.Checked, extension);
            }
        }
        Session["datatable_ContactNo"] = dt;
        return Session["datatable_ContactNo"] as DataTable;
    }

    public void setDatatable(DataTable dt)
    {
        try
        {
            GvNumber.DataSource = dt;
            GvNumber.DataBind();
        }
        catch(Exception erro)
        {

        }
    }

    public void FillGridData(string trans_id,string table_name)
    {
        DataTable dt_ContactNo = objContactnoMaster.getDataByPKID(trans_id, table_name);
        GvNumber.DataSource = dt_ContactNo;
        GvNumber.DataBind();
    }

    public void setNullToGV()
    {
        GvNumber.DataSource = null;
        GvNumber.DataBind();
    }

    public void setCountryCode(string data)
    {
        txtCountryCode.Text= data;
    }

    public void deleteRecordById(string trans_id, string table_name)
    {
        objContactnoMaster.deteteDate(table_name, trans_id);
    }

    // copy to parent


    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static string[] GetCompletionListStateName(string prefixText, int count, string contextKey)
    //{
    //    if (HttpContext.Current.Session["AddCtrl_Country_Id"].ToString() == "")
    //    {
    //        return null;
    //    }
    //    StateMaster objStateMaster = new StateMaster();
    //    DataTable dt = objStateMaster.GetAllStateByPrefixText(prefixText, HttpContext.Current.Session["AddCtrl_Country_Id"].ToString());
    //    string[] txt = new string[dt.Rows.Count];

    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        txt[i] = dt.Rows[i]["State_Name"].ToString();
    //    }
    //    return txt;
    //}

    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static string[] GetCompletionListCityName(string prefixText, int count, string contextKey)
    //{
    //    if (HttpContext.Current.Session["AddCtrl_State_Id"].ToString() == "")
    //    {
    //        return null;
    //    }
    //    CityMaster objCityMaster = new CityMaster();
    //    DataTable dt = objCityMaster.GetAllCityByPrefixText(prefixText, HttpContext.Current.Session["AddCtrl_State_Id"].ToString());
    //    string[] txt = new string[dt.Rows.Count];

    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        txt[i] = dt.Rows[i]["City_Name"].ToString();
    //    }
    //    return txt;
    //}

    //[System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    //public static string[] GetCompletionListContactNumber(string prefixText, int count, string contextKey)
    //{
    //    ContactNoMaster objContactNumMaster = new ContactNoMaster();

    //    DataTable dt = objContactNumMaster.getNumberList_PreText(prefixText);

    //    string[] txt = new string[dt.Rows.Count];

    //    for (int i = 0; i < dt.Rows.Count; i++)
    //    {
    //        txt[i] = dt.Rows[i]["Phone_no"].ToString();
    //    }
    //    return txt;
    //}

    //[System.Web.Services.WebMethod()]
    //[System.Web.Script.Services.ScriptMethod()]
    //public static string ddlCountry_IndexChanged(string CountryId)
    //{
    //    CountryMaster ObjSysCountryMaster = new CountryMaster();
    //    HttpContext.Current.Session["AddCtrl_Country_Id"] = CountryId;
    //    return "+" + ObjSysCountryMaster.GetCountryMasterById(CountryId).Rows[0]["Country_Code"].ToString();
    //}

    //[System.Web.Services.WebMethod()]
    //[System.Web.Script.Services.ScriptMethod()]
    //public static string txtCity_TextChanged(string stateId, string cityName)
    //{
    //    CityMaster ObjCityMaster = new CityMaster();

    //    DataTable dt_CityData = ObjCityMaster.GetAllCityByPrefixText(cityName, stateId);

    //    if (dt_CityData.Rows.Count > 0)
    //    {
    //        return dt_CityData.Rows[0]["Trans_Id"].ToString();
    //    }
    //    else
    //    {
    //        return "0";
    //    }
    //}

    //[System.Web.Services.WebMethod()]
    //[System.Web.Script.Services.ScriptMethod()]
    //public static string txtState_TextChanged(string CountryId, string StateName)
    //{
    //    StateMaster ObjStatemaster = new StateMaster();
    //    DataTable dt_stateData = ObjStatemaster.GetAllStateByPrefixText(StateName, CountryId);
    //    if (dt_stateData.Rows.Count > 0)
    //    {
    //        HttpContext.Current.Session["AddCtrl_State_Id"] = dt_stateData.Rows[0]["Trans_Id"].ToString();
    //        return dt_stateData.Rows[0]["Trans_Id"].ToString();

    //    }
    //    else
    //    {
    //        return "0";
    //    }
    //}

    //[System.Web.Services.WebMethod()]
    //[System.Web.Script.Services.ScriptMethod()]
    //public static int txtAddressNameNew_TextChanged(string AddressName)
    //{
    //    // return  1 when 'Address Name Already Exists' and 2 when 'Address Name Already In Deleted Section' and 0 when not present
    //    Set_AddressMaster AM = new Set_AddressMaster();
    //    DataTable dt = AM.GetAddressDataByAddressName(AddressName);
    //    if (dt.Rows.Count > 0)
    //    {
    //        return 1;
    //    }

    //    DataTable dt1 = AM.GetAddressFalseAllData();
    //    dt1 = new DataView(dt1, "Address_Name='" + AddressName + "'", "", DataViewRowState.CurrentRows).ToTable();
    //    if (dt1.Rows.Count > 0)
    //    {
    //        return 2;
    //    }

    //    return 0;
    //}

}