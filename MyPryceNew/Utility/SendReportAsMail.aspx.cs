using Newtonsoft.Json;
using System;
using System.Data;
using System.Web;
using System.Web.UI;

public partial class _SendReportAsMail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void ddlPosted_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void btnbind_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlFieldNameBin_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnbindBin_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void btnRefreshBin_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void imgBtnRestore_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        string data = "  <p>Hi @Customer</p>< p > We are happy to inform you that your order has been out of delivery with following details:</ p >< p >   < table class='table table-striped table-bordered' style='width: 100%'><thead><tr><td>Customer</td><td>Order No</td><td>Order Date</td><td>Ref.No.</td><td>Product ID</td><td>Product Name</td><td>Order Qty</td><td>Delivered Qty</td><td>Current Stock</td><td>Balanced Qty</td><td>Expected Delivery Date</td></tr></thead><tbody></tbody></table></p>";
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCon = ObjContactMaster.GetAllContactAsPerFilterText(prefixText);
        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Filtertext"].ToString();
            }
        }
        return filterlist;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string getEmpData(string CustID)
    {
        Ems_ContactMaster CM = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        using (DataTable dt = CM.getContactEmailFromCustId(CustID))
        {
            if (dt.Rows.Count > 0)
            {
                return JsonConvert.SerializeObject(dt);
            }
            else
            {
                return "";
            }
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string setEmailAddress(string emails)
    {
        HttpContext.Current.Session["hdnEmailIDS"] = emails;
        return "true";
    }
}
