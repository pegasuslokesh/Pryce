using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;

public partial class HR_HR_EmployeeBalance : System.Web.UI.Page
{
    LocationMaster ObjLocation = null;
    Common cmn = null;
    DataAccessClass ObjDa = null;
    SystemParameter objsys = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objsys = new SystemParameter(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            FillLocation();
            Calender_VoucherDate.Format = objsys.SetDateFormat();
            CalendarExtender1.Format = objsys.SetDateFormat();
        }
    }

    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }


    protected void Btn_Save_Click(object sender, EventArgs e)
    {

        if (txtFromDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtFromDate.Text);
            }
            catch
            {
                DisplayMessage("Enter valid date");
                txtFromDate.Focus();
                return;
            }
        }




        if (txtToDate.Text != "")
        {
            try
            {
                Convert.ToDateTime(txtToDate.Text);
            }
            catch
            {
                DisplayMessage("Enter valid date");
                txtToDate.Focus();
                return;
            }
        }





        string strCurrencyId = string.Empty;

        //for Selected Location
        string strLocationId = string.Empty;
        for (int i = 0; i < lstLocationSelect.Items.Count; i++)
        {
            if (strLocationId == "")
            {
                strLocationId = lstLocationSelect.Items[i].Value;
            }
            else
            {
                strLocationId = strLocationId + "," + lstLocationSelect.Items[i].Value;
            }
        }

        if (strLocationId != "")
        {

        }
        else
        {
            strLocationId = Session["LocId"].ToString();
        }



        string strsql = "select Pay_EmployeeSalaryStatement.Emp_Id, Set_EmployeeMaster.Department_Id, Set_EmployeeMaster.Emp_Code, Set_EmployeeMaster.Emp_Name, Set_EmployeeMaster.Emp_Name_L,REPLACE(CONVERT(CHAR(11), Set_EmployeeMaster.DOj, 106), ' ', '-') as DateOfjoining, sum( CAST( Pay_EmployeeSalaryStatement.Cr_cmp_amount as decimal(18,3)) ) as Credit_Amount, sum(CAST( Pay_EmployeeSalaryStatement.Dr_cmp_amount as decimal(18,3))) as Debit_Amount, (sum( CAST( Pay_EmployeeSalaryStatement.Cr_cmp_amount as decimal(18,3)) )-sum(CAST( Pay_EmployeeSalaryStatement.Dr_cmp_amount as decimal(18,3)))) as Balance from Pay_EmployeeSalaryStatement inner join Set_EmployeeMaster on Pay_EmployeeSalaryStatement.Emp_Id=Set_EmployeeMaster.emp_id ";


        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            strsql += " where Set_EmployeeMaster.Location_Id in (" + strLocationId + ") and Set_EmployeeMaster.IsActive='True' and Set_EmployeeMaster.Field2='False' and Trn_Date>='" + Convert.ToDateTime(txtFromDate.Text) + "' and   Trn_Date<='" + Convert.ToDateTime(txtToDate.Text) + "' and Pay_EmployeeSalaryStatement.Is_Opening_Balance='False' ";

        }
        else
        {
            strsql += " where Set_EmployeeMaster.Location_Id in (" + strLocationId + ") and Set_EmployeeMaster.IsActive='True' and Set_EmployeeMaster.Field2='False' and Pay_EmployeeSalaryStatement.Is_Opening_Balance='False'  ";

        }

        strsql += "group by Pay_EmployeeSalaryStatement.Emp_Id,Set_EmployeeMaster.Emp_Code, Set_EmployeeMaster.Emp_Name,Set_EmployeeMaster.Emp_Name_L, Set_EmployeeMaster.DOj,Set_EmployeeMaster.Department_Id";



        DataTable dt = new DataTable();
        dt = ObjDa.return_DataTable(strsql);

        dt.Columns.Add("Opening_balance");

        dt.Columns["Balance"].ReadOnly = false;
        dt.Columns["Opening_balance"].ReadOnly = false;
        dt.AcceptChanges();

        for (int i = 0;i<dt.Rows.Count;i++)
        {
            dt.Rows[i]["Balance"] = Common.GetAmountDecimal( (Convert.ToDouble(GetOpeningBalance(dt.Rows[i]["Emp_Id"].ToString(), txtFromDate.Text)) + Convert.ToDouble(dt.Rows[i]["Balance"].ToString())).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());
            dt.Rows[i]["Opening_balance"] = GetOpeningBalance(dt.Rows[i]["Emp_Id"].ToString(), txtFromDate.Text);


        }

        gvPayment.DataSource = dt;
        gvPayment.DataBind();


        if(dt.Rows.Count==0)
        {
            DisplayMessage("Record not found");
        }
        dt.Dispose();

    }

    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {

        gvPayment.DataSource = null;
        gvPayment.DataBind();

    }

    protected void lnkempStatement_Command(object sender, CommandEventArgs e)
    {

        string[] ObjArr = new string[3];

        ObjArr[0] = txtFromDate.Text;
        ObjArr[1] = txtToDate.Text;
        ObjArr[2] = e.CommandName.ToString() + "/" + e.CommandArgument.ToString();


        Session["Emp_Bal_Param"] = ObjArr;
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../HR/HR_EmployeeStatement.aspx')", true);
        
    }

    #region LocationWork
    public void FillLocation()
    {
        lstLocation.Items.Clear();
        lstLocation.DataSource = null;
        lstLocation.DataBind();
        //DataTable dtloc = new DataTable();
        //dtloc = ObjLocation.GetAllLocationMaster();

        DataTable dtLoc = ObjLocation.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        if (dtLoc.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 13-05-2015
            objPageCmn.FillData((object)lstLocation, dtLoc, "Location_Name", "Location_Id");
        }
    }

    protected void btnPushLoc_Click(object sender, EventArgs e)
    {
        if (lstLocation.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstLocation.Items)
            {
                if (li.Selected)
                {
                    lstLocationSelect.Items.Add(li);
                }
            }
            foreach (ListItem li in lstLocationSelect.Items)
            {
                lstLocation.Items.Remove(li);
            }
            lstLocationSelect.SelectedIndex = -1;
        }
        btnPushLoc.Focus();
    }
    protected void btnPullLoc_Click(object sender, EventArgs e)
    {
        if (lstLocationSelect.SelectedIndex >= 0)
        {
            foreach (ListItem li in lstLocationSelect.Items)
            {
                if (li.Selected)
                {
                    lstLocation.Items.Add(li);
                }
            }
            foreach (ListItem li in lstLocation.Items)
            {
                if (li.Selected)
                {
                    lstLocationSelect.Items.Remove(li);
                }
            }
            lstLocation.SelectedIndex = -1;
        }
        btnPullLoc.Focus();
    }
    protected void btnPushAllLoc_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstLocation.Items)
        {
            lstLocationSelect.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstLocationSelect.Items)
        {
            lstLocation.Items.Remove(DeptItem);
        }
        btnPushAllLoc.Focus();
    }
    protected void btnPullAllLoc_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstLocationSelect.Items)
        {
            lstLocation.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstLocation.Items)
        {
            lstLocationSelect.Items.Remove(DeptItem);
        }
        btnPullAllLoc.Focus();
    }
    #endregion


    public string GetOpeningBalance(string strEmpId, string strFromdate)
    {

        string strOpeningBalance = "0";
        DataTable dtTemp = new DataTable();

        if (strFromdate == "")
        {
            dtTemp = ObjDa.return_DataTable("select * from dbo.fn_Set_EmployeeOpeningBalance(" + strEmpId + ",null,0)");

        }
        else
        {
            dtTemp = ObjDa.return_DataTable("select * from dbo.fn_Set_EmployeeOpeningBalance(" + strEmpId + ",'" + objsys.getDateForInput(strFromdate).ToString() + "',0)");


        }


        if (dtTemp.Rows.Count > 0)
        {

            strOpeningBalance = Common.GetAmountDecimal((Convert.ToDouble(dtTemp.Rows[0]["Opening_Credit"].ToString()) - Convert.ToDouble(dtTemp.Rows[0]["Opening_Debit"].ToString())).ToString(), Session["DBConnection"].ToString(), Session["LocCurrencyId"].ToString());


        }
        else
        {

            strOpeningBalance = "0";
        }

        return strOpeningBalance;
    }
}