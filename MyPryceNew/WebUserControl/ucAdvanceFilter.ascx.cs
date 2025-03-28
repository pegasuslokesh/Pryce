using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class WebUserControl_ucAdvanceFilter : System.Web.UI.UserControl
{
    public delegate void parentPageHandlerForFilter(string strFilterString);
    public event parentPageHandlerForFilter applyFilterOnParentPage;
    List<clsFilter> lstFilter = new List<clsFilter> { };
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlConditions.DataSource = Enum.GetNames(typeof(comparision));
            ddlConditions.DataBind();
        }
    }

    protected void btnApply_Click(object sender, EventArgs e)
    {
        
        if (this.applyFilterOnParentPage != null)
        {
           if(ViewState["lstFilter"]!=null)
             applyFilterOnParentPage(getFilterString());
        }
        //string strMessag = dlGrdCols.Visible == true ? "Table Columns visibility setting has been changed successfully" : "Contros setting has been changed successfully";
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "hideAdvfilterPopup()", true);

    }

    public void setDefaults(DataTable dt)
    {
        ddlFields.DataSource = dt.Columns;
        ddlFields.DataBind();

        //ddlFilter.DataSource = lstFilter;
        //ddlFilter.DataBind();
    }

    public enum comparision
    {
        Equal,
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        NotEqual,
        Contains, //for strings  
        StartsWith, //for strings  
        EndsWith //for strings  
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Record deleted successfully')", true);
        if (txtValue.Text == string.Empty)
        {
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "DisplayMsg('Please enter filter value')", true);
            return;
        }

        if (ViewState["lstFilter"] != null)
        {
            lstFilter = (List<clsFilter>)ViewState["lstFilter"];
        }
        clsFilter cls = new clsFilter();
        cls.sNo = lstFilter.Count() + 1;
        cls.fltField = ddlFields.SelectedValue;
        cls.fltCondition = ddlConditions.SelectedValue;
        cls.fltValue = txtValue.Text;
        cls.fltOperator = cls.sNo == 1 ? "" : ddlAndOr.SelectedValue;
        cls.fltString = cls.fltOperator + " " + cls.fltField + " " + cls.fltCondition + " " + cls.fltValue;
        lstFilter.Add(cls);
        ViewState["lstFilter"] = lstFilter;
        gvFltConditions.DataSource = lstFilter;
        gvFltConditions.DataBind();

        txtValue.Text = string.Empty;
        divAndOr.Visible = true;
        btnApplyFilter.Enabled = true;
        btnRemoveFilter.Enabled = true;
        //ddlConditions.SelectedIndex=0;
    }

    [Serializable]
    public class clsFilter
    {
        public int sNo { get; set; }
        public string fltOperator { get; set; }
        public string fltField { get; set; }
        public string fltCondition { get; set; }
        public string fltValue { get; set; }
        public string fltString { get; set; }
    }

    protected void fillTblConditions(List<clsFilter> lstCls)
    {

        foreach (clsFilter cls in lstCls)
        {

        }
    }


    protected void btnDelCondition_Command(object sender, CommandEventArgs e)
    {
        try
        {
            string strSno = e.CommandArgument.ToString();
            if (ViewState["lstFilter"] != null)
            {
                lstFilter = (List<clsFilter>)ViewState["lstFilter"];
                bool isReorderRequired = false;
                if (Int32.Parse(strSno)!=lstFilter.Count)
                {
                    isReorderRequired = true;
                }
                clsFilter cls = lstFilter.FirstOrDefault(w => w.sNo == Int32.Parse(strSno));
                if (cls != null)
                {
                    lstFilter.Remove(cls);
                    if (lstFilter.Count > 0)
                    {
                        if (isReorderRequired==true)
                        {
                            int cnt = 0;
                            foreach(clsFilter clsFlt in lstFilter)
                            {
                                cnt++;
                                clsFlt.sNo = cnt;
                                clsFlt.fltOperator = cls.sNo == 1 ? "" : clsFlt.fltOperator;
                                clsFlt.fltString = clsFlt.fltOperator + " " + clsFlt.fltField + " " + clsFlt.fltCondition + " " + clsFlt.fltValue;
                            }
                        }
                        ViewState["lstFilter"] = lstFilter;
                    }
                    else
                    {
                        divAndOr.Visible = false;
                        btnApplyFilter.Enabled = false;
                        btnRemoveFilter.Enabled = false;
                    }
                    gvFltConditions.DataSource = lstFilter;
                    gvFltConditions.DataBind();
                }

            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void btnRemoveFilter_Click1(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["lstFilter"] != null)
            {
                List<clsFilter> lstCls = (List<clsFilter>)ViewState["lstFilter"];
                lstCls.Clear();
                gvFltConditions.DataSource = lstCls;
                gvFltConditions.DataBind();
                btnRemoveFilter.Enabled = false;
                btnApplyFilter.Enabled = false;
                if (this.applyFilterOnParentPage != null)
                {
                    applyFilterOnParentPage("");
                }
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "hideAdvfilterPopup()", true);
            }
        }
        catch
        {

        }
    }
    protected string getFilterString()
    {
        StringBuilder strFltString = new StringBuilder();
        if (ViewState["lstFilter"] != null)
        {
            List<clsFilter> lstCls = (List<clsFilter>)ViewState["lstFilter"];
            foreach (clsFilter cls in lstCls)
            {
                switch (cls.fltCondition)
                {
                    case "EndsWith":
                        strFltString.Append(" " + cls.fltOperator + " " + cls.fltField + " " + " Like '%" + cls.fltValue + "'");
                        break;
                    case "Equal":
                        strFltString.Append(" " + cls.fltOperator + " " + cls.fltField + " " + " = '" + cls.fltValue + "'");
                        break;
                    case "Contains":
                        strFltString.Append(" " + cls.fltOperator + " " + cls.fltField + " " + " Like '%" + cls.fltValue + "%'");
                        break;
                    case "GreaterThan":
                        strFltString.Append(" " + cls.fltOperator + " " + cls.fltField + " " + " > '" + cls.fltValue + "'");
                        break;
                    case "GreaterThanOrEqual":
                        strFltString.Append(" " + cls.fltOperator + " " + cls.fltField + " " + " >= '" + cls.fltValue + "'");
                        break;
                    case "LessThan":
                        strFltString.Append(" " + cls.fltOperator + " " + cls.fltField + " " + " < '" + cls.fltValue + "'");
                        break;
                    case "LessThanOrEqual":
                        strFltString.Append(" " + cls.fltOperator + " " + cls.fltField + " " + " <= '" + cls.fltValue + "'");
                        break;
                    case "NotEqual":
                        strFltString.Append(" " + cls.fltOperator + " " + cls.fltField + " " + " <> '" + cls.fltValue + "'");
                        break;
                    case "StartsWith":
                        strFltString.Append(" " + cls.fltOperator + " " + cls.fltField + " " + " Like '" + cls.fltValue + "%'");
                        break;
                }

            }
        }
        return strFltString.ToString();
    }
}