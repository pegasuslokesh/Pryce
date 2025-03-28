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

public partial class HR_Pay_Employee_LoanDetail : BasePage
{
    SystemParameter ObjSysParam = null;
    Pay_Employee_Loan ObjLoan = null;
    LocationMaster ObjLocationMaster = null;
    Common ObjComman = null;
    UserMaster objUser = null;
    PageControlCommon objPageCmn = null;

    string strCompId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        Session["AccordianId"] = "19";
        Session["HeaderText"] = "HR";
        strCompId = Session["CompId"].ToString();

        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        ObjLoan = new Pay_Employee_Loan(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        objUser = new UserMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = ObjComman.getPagePermission("../HR/Pay_Employee_LoanDetail.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            FillddlLocation();
            GridBind();
            //this code is created by jitendra upadhyay on 17-07-2014
            //this code for set the gridview page size according the system parameter
            try
            {
                GridViewLoan.PageSize = int.Parse(Session["GridSize"].ToString());
                GridViewLoanDetailrecord.PageSize = int.Parse(Session["GridSize"].ToString());
            }
            catch
            {

            }



        }
        Page.Title = ObjSysParam.GetSysTitle();

    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
    }

    public static string GetIntrest(object Interest)
    {
        SystemParameter Objsys = new SystemParameter(HttpContext.Current.Session["DBConnection"].ToString());

        if (Interest.ToString() == "")
        {
            Interest = "0%";
        }
        else
        {
            Interest = Interest.ToString() + '%';
        }
        return Interest.ToString();
    }

    public string GetDate(object obj)
    {

        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());

        return Date.ToString(ObjSysParam.SetDateFormat());
    }

    void GridBind()
    {

        DataTable dt = objUser.GetUserMasterByUserId(Session["UserId"].ToString(), "");
        bool IsSingleUser = false;
        try
        {
            IsSingleUser = Convert.ToBoolean(dt.Rows[0]["Field6"].ToString());
        }
        catch
        {
            IsSingleUser = false;
        }




        string strRecordType = "0";

        if (ddlLoanType.SelectedIndex > 0)
        {
            strRecordType = ddlLoanType.SelectedValue.Trim();
        }

        DataTable Dt = new DataTable();
        Dt = ObjLoan.GetRecord_From_PayEmployeeLoanByStatus(strCompId, "Approved");

        string strLocationId = string.Empty;

        if (ddlLocation.SelectedIndex == 0)
        {
            for (int j = 1; j < ddlLocation.Items.Count; j++)
            {
                if (strLocationId == "")
                {
                    strLocationId = ddlLocation.Items[j].Value;
                }
                else
                {
                    strLocationId = strLocationId + "," + ddlLocation.Items[j].Value;
                }
            }
        }
        else
        {
            strLocationId = ddlLocation.SelectedValue.Trim();
        }



        if (Dt.Rows.Count > 0)
        {
            if (!IsSingleUser)
            {
                Dt = new DataView(Dt, "Location_Id in (" + strLocationId + ") ", "", DataViewRowState.CurrentRows).ToTable();


            }
            else
            {
                Dt = new DataView(Dt, "Emp_id=" + Session["EmpId"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            }

            if (ddlLoanType.SelectedIndex > 0)
            {
                Dt = new DataView(Dt, "RecordType =" + strRecordType + "", "", DataViewRowState.CurrentRows).ToTable();
            }




            Dt.Columns.Add("Intrest_Amt");
            foreach (DataRow DTR in Dt.Rows)
            {
                DTR["Intrest_Amt"] = Convert.ToDouble(DTR["Gross_Amount"]) - Convert.ToDouble(DTR["Loan_Amount"]);
            }
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GridViewLoan, Dt, "", "");
            Session["dtFilter_Pay_Emp_Loan"] = Dt;
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + Dt.Rows.Count + "";
        }
        else
        {
            DataTable Dtclear = new DataTable();
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GridViewLoan, Dtclear, "", "");
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": 0";
        }

    }
    void Gridbind_LoanDetail(string LoanId)
    {
        DataTable dt = new DataTable();
        dt = ObjLoan.GetRecord_From_PayEmployeeLoanDetailByLoanId(LoanId);
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GridViewLoanDetailrecord, dt, "", "");
            Session["dtLoanDetail"] = dt;
            double Montly_Installment = Convert.ToDouble(dt.Compute("SUM(Montly_Installment)", string.Empty));
            double Employee_Paid = 0;
            try
            {
                Employee_Paid = Convert.ToDouble(dt.Compute("SUM(Employee_Paid)", "Is_Status = 'Paid'"));
            }
            catch
            {
                Employee_Paid = 0;
            }


            if (GridViewLoanDetailrecord.Rows.Count > 0)
            {
                Label M_Installment = (Label)GridViewLoanDetailrecord.FooterRow.FindControl("lblgvMonth_Installment");
                Label Paid = (Label)GridViewLoanDetailrecord.FooterRow.FindControl("lblgvPaid");
                Label Pending = (Label)GridViewLoanDetailrecord.FooterRow.FindControl("lblgvPending");
                M_Installment.Text = Montly_Installment.ToString();
                Paid.Text = Employee_Paid.ToString() + " (Paid)";
                Pending.Text = (Montly_Installment - Employee_Paid).ToString() + " (Pending)";
            }
        }
        else
        {
            DataTable Dtclear = new DataTable();
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)GridViewLoanDetailrecord, Dtclear, "", "");
        }

    }
    protected void GridViewLoan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewLoan.PageIndex = e.NewPageIndex;
        GridBind();

        GridViewLoan.HeaderRow.Focus();
    }
    protected void GridViewLoan_Sorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Pay_Emp_Loan"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Pay_Emp_Loan"] = dt;

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GridViewLoan, dt, "", "");

        GridViewLoan.HeaderRow.Focus();
    }
    protected void btnEdit_command(object sender, CommandEventArgs e)
    {

        GridViewRow row = (GridViewRow)((LinkButton)sender).Parent.Parent;
        hiddenid.Value = e.CommandArgument.ToString();
        DataTable Dt = new DataTable();
        Dt = ObjLoan.GetRecord_From_PayEmployeeLoan_usingLoanId(strCompId, hiddenid.Value, "Pending");


        Gridbind_LoanDetail(hiddenid.Value);

        Div_Details.Style.Add("display", "");

        Lblemployeeid.Visible = true;
        Lblemployeename.Visible = true;
        SetEmployeeId.Visible = true;
        setemployeename.Visible = true;
        lblempIdColon.Visible = true;
        lblEmpNameColon.Visible = true;
        setemployeename.Text = Dt.Rows[0]["Emp_Name"].ToString();
        SetEmployeeId.Text = Dt.Rows[0]["Emp_Code"].ToString();
        GridViewLoan.Rows[row.RowIndex].Cells[0].Focus();

    }
    protected void GridViewLoanDetailrecord_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridViewLoanDetailrecord.PageIndex = e.NewPageIndex;
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtLoanDetail"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GridViewLoanDetailrecord, dt, "", "");

    }
    protected void GridViewLoanDetailrecord_Sorting(object sender, GridViewSortEventArgs e)
    {
        HdfSortDetail.Value = HdfSortDetail.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtLoanDetail"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HdfSortDetail.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtLoanDetail"] = dt;

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)GridViewLoanDetailrecord, dt, "", "");
    }
    public string GetType(string Type)
    {
        if (Type == "1")
        {
            Type = "January";
        }
        if (Type == "2")
        {
            Type = "February";
        }
        if (Type == "3")
        {
            Type = "March";

        }
        if (Type == "4")
        {
            Type = "April";

        }
        if (Type == "5")
        {
            Type = "May";

        }
        if (Type == "6")
        {
            Type = "June";

        }
        if (Type == "7")
        {
            Type = "July";

        }
        if (Type == "8")
        {
            Type = "August";

        }
        if (Type == "9")
        {
            Type = "september";

        }
        if (Type == "10")
        {
            Type = "October";

        }
        if (Type == "11")
        {
            Type = "November";

        }
        if (Type == "12")
        {
            Type = "December";

        }
        return Type;
    }


    #region Filterrecord

    private void FillddlLocation()
    {
        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        string LocIds = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(Session["CompId"].ToString()))
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
            ListItem li = new ListItem("All", "0");
            ddlLocation.Items.Insert(0, li);

            ddlLocation.SelectedValue = Session["LocId"].ToString();
        }
        else
        {
            try
            {
                ddlLocation.Items.Clear();
                ddlLocation.DataSource = null;
                ddlLocation.DataBind();
                ListItem li = new ListItem("--Select--", "0");
                ddlLocation.Items.Insert(0, li);
                ddlLocation.SelectedIndex = 0;
            }
            catch
            {
                ListItem li = new ListItem("--Select--", "0");
                ddlLocation.Items.Insert(0, li);
                ddlLocation.SelectedIndex = 0;
            }
        }
    }


    protected void btnfilter_Click(object sender, EventArgs e)
    {
        GridBind();
        Div_Details.Style.Add("display", "none");
    }

    #endregion
}
