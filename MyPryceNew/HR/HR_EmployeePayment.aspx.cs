using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.Globalization;
using System.Collections;
using System.Data.SqlClient;

public partial class HR_HR_EmployeePayment : System.Web.UI.Page
{
    DepartmentMaster objDep = null;
    LocationMaster ObjLocationMaster = null;
    EmployeeParameter objEmpParam = null;
    Common ObjComman = null;
    SystemParameter ObjSys = null;
    DataAccessClass ObjDa = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    LocationMaster objLocation = null;
    Pay_AdvancePayment objAdvancePayment = null;
    Ac_Parameter_Location objAccParameterLocation = null;
    Set_DocNumber objDocNo = null;
    EmployeeMaster objEmp = null;
    Ac_ParameterMaster objAcParam = null;
    Set_Approval_Employee objApproalEmp = null;
    Ac_Finance_Year_Info objFinancialYearInfo = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        txtVoucherDate.Attributes.Add("readonly", "readonly");
        objDep = new DepartmentMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        ObjSys = new SystemParameter(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objAdvancePayment = new Pay_AdvancePayment(Session["DBConnection"].ToString());
        objAccParameterLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objDocNo = new Set_DocNumber(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objAcParam = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objApproalEmp = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objFinancialYearInfo = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = ObjComman.getPagePermission("../Hr/Hr_EmployeePayment.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
            FillddlLocationList();
            FillDepartment(ddl_Location, ddl_Department);
            FillDepartment(DDL_Location_New, DDL_Department_New);
            Fill_Grid_Data();
            Fill_Payment_Option();
            //FillddlDeaprtment();
          
            Fill_List_Voucher();
            txtChequeIssueDate.Text = Convert.ToDateTime(DateTime.Now).ToString(ObjSys.SetDateFormat());
        }
    }

    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        Btn_Save.Visible = clsPagePermission.bAdd;
        hdnCanPrint.Value = clsPagePermission.bPrint.ToString().ToLower();
    }
    
    #region Bin

    public void Fill_Grid_Data()
    {
        string EmployeeAccountName = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Employee Account").Rows[0]["Param_Value"].ToString());
        string EmployeeAccountId = EmployeeAccountName.Split('/')[1].ToString();
        DataTable dtFyInfo = objFinancialYearInfo.GetInfoByTransId(Session["CompId"].ToString(), Session["FinanceYearId"].ToString());
        DateTime dtFromDate = Convert.ToDateTime(dtFyInfo.Rows[0]["From_Date"].ToString());
        DataTable dt = new DataTable();

         
        dt = objVoucherDetail.GetAllEmployeeBalanceData(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), EmployeeAccountId, "0", dtFromDate.ToString(), DateTime.Now.ToString(), "1", Session["FinanceYearId"].ToString());
        Session["Emp_payment_Grid_Data_all"] = dt;
        FillGrid_Payment();
        //dt = ObjDa.return_DataTable("select ac_voucher_detail.other_account_no as Emp_Id,Set_EmployeeMaster.Department_Id,Set_EmployeeMaster.Emp_Code,Set_EmployeeMaster.Emp_Name,Set_EmployeeMaster.Emp_Name_L,REPLACE(CONVERT(CHAR(11), Set_EmployeeMaster.DOj, 106), ' ', '-') as DateOfjoining,sum(ac_voucher_detail.Credit_Amount) as Credit_Amount,sum(ac_voucher_detail.Debit_Amount) as Debit_Amount,(sum(ac_voucher_detail.Credit_Amount)-sum(ac_voucher_detail.Debit_Amount)) as Balance  from ac_voucher_detail inner join Set_EmployeeMaster on ac_voucher_detail.other_account_no=Set_EmployeeMaster.emp_id inner join Ac_Voucher_Header on ac_voucher_detail.Voucher_No=ac_voucher_header.Trans_Id  where Set_EmployeeMaster.Location_Id='"+Session["LocId"].ToString()+"' and ac_voucher_detail.Account_No='"+EmployeeAccountId+ "' and  ac_voucher_detail.other_Account_no<>'0' and ac_voucher_header.IsActive='True' and ac_voucher_header.ReconciledFromFinance='True' and ac_voucher_header.Field3<>'Pending'  group by ac_voucher_detail.other_account_no ,Set_EmployeeMaster.Department_Id,Set_EmployeeMaster.Emp_Code,Set_EmployeeMaster.Emp_Name,Set_EmployeeMaster.Emp_Name_L,Set_EmployeeMaster.DOj having (sum(ac_voucher_detail.Credit_Amount)-SUM(ac_voucher_detail.Debit_Amount))>0");
        //dt = ObjDa.return_DataTable("with tb as (select ac_voucher_detail.other_account_no as Emp_Id,Set_EmployeeMaster.Department_Id,Set_EmployeeMaster.Emp_Code,Set_EmployeeMaster.Emp_Name,Set_EmployeeMaster.Emp_Name_L,REPLACE(CONVERT(CHAR(11), Set_EmployeeMaster.DOj, 106), ' ', '-') as DateOfjoining,((select top 1 isnull( max(lcr_amount),0) from Ac_SubChartOfAccount where AccTransId='" + EmployeeAccountId + "' and other_account_no=ac_voucher_detail.other_account_no )+sum(ac_voucher_detail.Credit_Amount)) as Credit_Amount, (select top 1 isnull( max(ldr_amount),0) from Ac_SubChartOfAccount where AccTransId='" + EmployeeAccountId + "' and other_account_no=ac_voucher_detail.other_account_no )+ sum(ac_voucher_detail.Debit_Amount) as Debit_Amount from ac_voucher_detail inner join Set_EmployeeMaster on ac_voucher_detail.other_account_no=Set_EmployeeMaster.emp_id inner join Ac_Voucher_Header on ac_voucher_detail.Voucher_No=ac_voucher_header.Trans_Id where Set_EmployeeMaster.Location_Id='"+Session["LocId"].ToString()+ "' and ac_voucher_detail.Account_No='" + EmployeeAccountId + "' and ac_voucher_detail.other_Account_no<>'0' and ac_voucher_header.IsActive='True' and ac_voucher_header.ReconciledFromFinance='True' and ac_voucher_header.Field3<>'Pending' group by ac_voucher_detail.other_account_no ,Set_EmployeeMaster.Department_Id,Set_EmployeeMaster.Emp_Code,Set_EmployeeMaster.Emp_Name,Set_EmployeeMaster.Emp_Name_L,Set_EmployeeMaster.DOj ) select *,(Credit_Amount-Debit_Amount) as Balance from tb where (Credit_Amount-Debit_Amount)>0");
    }

    public void FillGrid_Payment()
    {

        if (Session["Emp_payment_Grid_Data_all"] != null)
        {
            DataTable dt = Session["Emp_payment_Grid_Data_all"] as DataTable;
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows.Count > 0)
                {
                    dt = new DataView(dt, "closing_final <> 0", "", DataViewRowState.CurrentRows).ToTable();
                }
                if (dt != null && DDL_Location_New.SelectedIndex > 0)
                {
                    dt = new DataView(dt, "Location_Id=" + DDL_Location_New.SelectedValue.Trim() + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                if (dt != null && DDL_Department_New.SelectedIndex > 0)
                {
                    dt = new DataView(dt, "Department_Id=" + DDL_Department_New.SelectedValue.Trim() + "", "", DataViewRowState.CurrentRows).ToTable();
                }
                if (dt != null && Ddl_Payment_Option.SelectedIndex > 0)
                {
                    dt = new DataView(dt, "Payment_Opt_Account_ID=" + Ddl_Payment_Option.SelectedValue.Trim() + "", "", DataViewRowState.CurrentRows).ToTable();
                }

                if (dt != null && chkEmployeeSelect.Checked == false)
                {
                    dt = new DataView(dt, "IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                }

                if (dt != null && chkEmployeeSelect.Checked == false)
                {
                    dt = new DataView(dt, "Field2='False'", "", DataViewRowState.CurrentRows).ToTable();
                }

                if (dt != null && chkEmployeeSelect.Checked == false)
                {
                    dt = new DataView(dt, "Emp_Type='On Role'", "", DataViewRowState.CurrentRows).ToTable();
                }

                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)gvPayment, dt, "", "");
                Session["dtBinDeduction"] = dt;
                Session["dtBinFilter"] = dt;
                lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";

                Session["CHECKED_ITEMS"] = null;
                if (dt.Rows.Count == 0)
                {

                    ImgbtnSelectAll.Visible = false;
                }
                else
                {
                  
                }
            }
            else
            {
                gvPayment.DataSource = null;
                gvPayment.DataBind();
                lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": 0";
            }
        }
        else
        {
            gvPayment.DataSource = null;
            gvPayment.DataBind();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": 0";
        }
    }


    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlbinOption.SelectedIndex != 0)
        {
            string condition = string.Empty;

            if (ddlbinOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + txtbinValue.Text.Trim() + "'";
            }
            else if (ddlbinOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + txtbinValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + txtbinValue.Text + "%'";
            }
            DataTable dtCust = (DataTable)Session["dtBinDeduction"];

            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtBinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvPayment, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {

                ImgbtnSelectAll.Visible = false;
            }
            else
            {
              
            }
        }
        txtbinValue.Focus();
    }

    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        Fill_Grid_Data();

        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
        txtbinValue.Focus();
    }


    private void PopulateCheckedValues()
    {
        ArrayList userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails != null && userdetails.Count > 0)
        {
            foreach (GridViewRow gvrow in gvPayment.Rows)
            {
                int index = (int)gvPayment.DataKeys[gvrow.RowIndex].Value;
                if (userdetails.Contains(index))
                {
                    CheckBox myCheckBox = (CheckBox)gvrow.FindControl("chkgvSelect");
                    myCheckBox.Checked = true;
                }
            }
        }
    }
    private void SaveCheckedValues()
    {
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvPayment.Rows)
        {
            index = (int)gvPayment.DataKeys[gvrow.RowIndex].Value;
            bool result = ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked;


            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
            }
            else
                userdetails.Remove(index);

        }
        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_ITEMS"] = userdetails;
    }
    protected void ImgbtnSelectAll_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        ArrayList userdetails = new ArrayList();
        DataTable dtDEPARTMENT = (DataTable)Session["dtbinFilter"];

        if (ViewState["Select"] == null)
        {
            Session["CHECKED_ITEMS"] = null;
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtDEPARTMENT.Rows)
            {
                //Allowance_Id

                // Check in the Session
                if (Session["CHECKED_ITEMS"] != null)
                    userdetails = (ArrayList)Session["CHECKED_ITEMS"];

                if (!userdetails.Contains(Convert.ToInt32(dr["Emp_Id"])))
                    userdetails.Add(Convert.ToInt32(dr["Emp_Id"]));

            }
            foreach (GridViewRow gvrow in gvPayment.Rows)
            {
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            if (userdetails != null && userdetails.Count > 0)
                Session["CHECKED_ITEMS"] = userdetails;
        }
        else
        {
            Session["CHECKED_ITEMS"] = null;
            DataTable dtAddressCategory1 = (DataTable)Session["dtbinFilter"];
            //Common Function add By Lokesh on 12-05-2015
            objPageCmn.FillData((object)gvPayment, dtAddressCategory1, "", "");
            ViewState["Select"] = null;
        }
    }

    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        double tot = 0;
        CheckBox chkSelAll = ((CheckBox)gvPayment.HeaderRow.FindControl("chkgvSelectAll"));
        bool result = false;
        if (chkSelAll.Checked == true)
        {
            result = true;
        }
        else
        {
            result = false;
        }
        ArrayList userdetails = new ArrayList();
        int index = -1;
        foreach (GridViewRow gvrow in gvPayment.Rows)
        {
            index = (int)gvPayment.DataKeys[gvrow.RowIndex].Value;
            // Check in the Session
            if (Session["CHECKED_ITEMS"] != null)
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];


            if (result)
            {
                if (!userdetails.Contains(index))
                    userdetails.Add(index);
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = true;
            }
            else
            {
                userdetails.Remove(index);
                ((CheckBox)gvrow.FindControl("chkgvSelect")).Checked = false;
            }
        }
        if (userdetails != null && userdetails.Count > 0)
            Session["CHECKED_ITEMS"] = userdetails;

        Set_Total_Amount();
    }

    protected void GvsalaryPlanBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        SaveCheckedValues();
        gvPayment.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtBinFilter"];
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvPayment, dt, "", "");

        PopulateCheckedValues();
  

    }
    protected void GvsalaryPlanBin_Sorting(object sender, GridViewSortEventArgs e)
    {
        SaveCheckedValues();
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtbinFilter"];
        //dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        //Common Function add By Lokesh on 12-05-2015
        objPageCmn.FillData((object)gvPayment, dt, "", "");

      
        PopulateCheckedValues();
    }

    protected void Btn_Bin_Click(object sender, EventArgs e)
    {
        Fill_Grid_Data();
    }
    #endregion


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

    #region Account
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountName(string prefixText, int count, string contextKey)
    {
        string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and IsActive='True'";
        DataAccessClass daclass = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCOA = daclass.return_DataTable(sql);

        string filtertext = "AccountName like '%" + prefixText + "%'";
        dtCOA = new DataView(dtCOA, filtertext, "", DataViewRowState.CurrentRows).ToTable();

        string[] txt = new string[dtCOA.Rows.Count];

        if (dtCOA.Rows.Count > 0)
        {
            for (int i = 0; i < dtCOA.Rows.Count; i++)
            {
                txt[i] = dtCOA.Rows[i]["AccountName"].ToString() + "/" + dtCOA.Rows[i]["Trans_Id"].ToString() + "";
            }
        }

        return txt;
    }
    protected void txtcmnAccount_textChnaged(object sender, EventArgs e)
    {
        if (((TextBox)sender).Text != "")
        {
            try
            {
                ((TextBox)sender).Text.Split('/')[0].ToString();

                string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and AccountName='" + ((TextBox)sender).Text.Split('/')[0].ToString() + "' and IsActive='True'";
                DataTable dtCOA = ObjDa.return_DataTable(sql);

                if (dtCOA.Rows.Count == 0)
                {
                    DisplayMessage("Choose account in suggestion only");
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Focus();
                }
            }
            catch
            {
                DisplayMessage("Choose account in suggestion only");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
            }
        }
    }

    private string GetLocationCode(string strLocationId)
    {
        string strLocationCode = string.Empty;
        if (strLocationId != "0" && strLocationId != "")
        {
            DataTable dtLocation = objLocation.GetLocationMasterByLocationId(strLocationId);
            if (dtLocation.Rows.Count > 0)
            {
                strLocationCode = dtLocation.Rows[0]["Location_Code"].ToString();
            }
        }
        return strLocationCode;
    }
    #endregion


    protected void Btn_Save_Click(object sender, EventArgs e)
    {
        if (txtVoucherDate.Text == "")
        {
            DisplayMessage("Enter Voucher Date");
            txtVoucherDate.Focus();
            return;
        }
        string strApprovalStatus = string.Empty;
        strApprovalStatus = "Approved";
        string strLocationCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();

        double PaidTotaamount = 0;
        double TotalSum = 0;
        int Msg = 0;
        string[] EmpDetails = new string[1];
        int EmpCount = -1;
        if (gvPayment.Rows.Count != 0)
        {
            SaveCheckedValues();
            if (Session["CHECKED_ITEMS"] != null)
            {
                ArrayList userdetails = new ArrayList();
                userdetails = (ArrayList)Session["CHECKED_ITEMS"];
                if (userdetails.Count == 0)
                {
                    DisplayMessage("Please select record");
                    return;
                }
                else if (Chk_Cheque_Payment.Checked == true && txtChequeIssueDate.Text == "")
                {
                    DisplayMessage("Please enter cheque issue date");
                    return;
                }
                else if (Chk_Cheque_Payment.Checked == true && txtChequeNo.Text == "")
                {
                    DisplayMessage("Please enter cheque number");
                    return;
                }
                else
                {
                    SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());

                    con.Open();
                    SqlTransaction trns;
                    trns = con.BeginTransaction();
                    try
                    {
                        string strNarration = txtNarration.Text;
                        if (!string.IsNullOrEmpty(strNarration))
                        {
                            strNarration += " ";
                        }
                        
                        foreach (GridViewRow gvr in gvPayment.Rows)
                        {
                            if (((CheckBox)gvr.FindControl("chkgvSelect")).Checked)
                            {
                                //string[] result = intarray.Select(x => x.ToString()).ToArray();
                                var strings = userdetails.ToArray();
                                var empidlist = string.Join(",", strings);
                                var arr = empidlist.Split(',');
                                if (EmpCount == -1)
                                    EmpDetails = new string[arr.Length];
                                EmpCount++;

                                try
                                {

                                    PaidTotaamount = Convert.ToDouble(((TextBox)gvr.FindControl("txtPayment")).Text);
                                }
                                catch
                                {
                                    PaidTotaamount = 0;

                                }
                                string str = gvPayment.DataKeys[gvr.RowIndex].Values[0].ToString();
                                string EmpDetail = str + "," + PaidTotaamount.ToString();
                                EmpDetails[EmpCount] = EmpDetail;

                                if (PaidTotaamount > 0)
                                {
                                    TotalSum += PaidTotaamount;
                                    objAdvancePayment.InsertEmployeeStatement(Session["compId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), ((Label)gvr.FindControl("lblEmpId")).Text, DateTime.Now.ToString(), PaidTotaamount.ToString(), "0", strLocationCurrencyId, PaidTotaamount.ToString(), "0", PaidTotaamount.ToString(), "0", "Employee Salary Payment", "0", "0", "Salary Payment", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), false.ToString(), ref trns);
                                }


                                strNarration += getEmployeeName(((Label)gvr.FindControl("lblEmpId")).Text, ref trns) + "(" + PaidTotaamount.ToString() + ")" + ",";


                            }
                        }


                        PaidTotaamount = TotalSum;

                        //For Bank Account
                        string strAccountId = string.Empty;
                        DataTable dtAccount = objAccParameterLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "BankAccount", ref trns);
                        if (dtAccount.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtAccount.Rows.Count; i++)
                            {
                                if (strAccountId == "")
                                {
                                    strAccountId = dtAccount.Rows[i]["Param_Value"].ToString();
                                }
                                else
                                {
                                    strAccountId = strAccountId + "," + dtAccount.Rows[i]["Param_Value"].ToString();
                                }
                            }
                        }
                        else
                        {
                            strAccountId = "0";
                        }

                        string strCurrencyId = objLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString(), ref trns).Rows[0]["Field1"].ToString();


                        //for Voucher Number
                        string strVoucherNumber = objDocNo.GetDocumentNo(true, Session["CompId"].ToString(), false, "160", "304", "0", ref trns, Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["DepartmentId"].ToString(), Session["EmpId"].ToString(), Session["TimeZoneId"].ToString());
                        if (strVoucherNumber != "")
                        {

                            int counter = objAcParam.GetCounterforVoucherNumber1(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "PV", Session["FinanceYearId"].ToString(), ref trns);

                            if (counter == 0)
                            {
                                strVoucherNumber = strVoucherNumber + "1";
                            }
                            else
                            {
                                strVoucherNumber = strVoucherNumber + (counter + 1);
                            }
                        }
                        string Cheque_Issue_Date = string.Empty;
                        if (Chk_Cheque_Payment.Checked == true)
                        {
                            Cheque_Issue_Date = Convert.ToDateTime(txtChequeIssueDate.Text).ToString();
                        }
                        else
                        {
                            Cheque_Issue_Date = "1/1/1800";
                        }
                        string Department_Id = string.Empty;
                        if (DDL_Department_New.SelectedIndex == 0)
                        {
                            Department_Id = "0";
                        }
                        else
                        {
                            Department_Id = DDL_Department_New.SelectedValue.ToString();
                        }
                        int MaxId = objVoucherHeader.InsertVoucherHeader(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["FinanceYearId"].ToString(), DDL_Location_New.SelectedValue.ToString(), Department_Id, "0", "0", "0", txtVoucherDate.Text, strVoucherNumber, txtVoucherDate.Text, "PV", Cheque_Issue_Date, "1/1/1800", txtChequeNo.Text, txtNarration.Text, strCurrencyId, "0.00", strNarration, false.ToString(), false.ToString(), true.ToString(), "PV", "", strApprovalStatus, "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        string strVMaxId = MaxId.ToString();


                        string strCompAmount = getConvertedAmount(strCurrencyId, Session["CurrencyId"].ToString(), PaidTotaamount.ToString());
                        string strLocAmount = getConvertedAmount(strCurrencyId, strCurrencyId, PaidTotaamount.ToString());
                        string strForeignAmount = PaidTotaamount.ToString();
                        string strForeignExchangerate = "1";



                        //str for Employee Id
                        //For Debit

                        //string strCompanyCrrValueDr = getConvertedAmount(ddlCurrency.SelectedValue, Session["CurrencyId"].ToString(), PaidTotaamount.ToString());
                        //string CompanyCurrDebit = strCompanyCrrValueDr.Trim().Split('/')[0].ToString();
                        if (EmpDetails.Length > 0)
                        {

                            string EmployeeAccountName = GetAccountNameByTransId(objAcParam.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), "Employee Account").Rows[0]["Param_Value"].ToString());
                            string EmployeeAccountId = EmployeeAccountName.Split('/')[1].ToString();

                            foreach (string EmpStr in EmpDetails)
                            {
                                string Emp_Id = EmpStr.Split(',')[0].ToString();
                                string Emp_Sal = EmpStr.Split(',')[1].ToString();
                                //if (strAccountId.Split(',').Contains(txtpaymentdebitaccount.Text.Split('/')[1].ToString()))
                                //{
                                //    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentdebitaccount.Text.Split('/')[1].ToString(), "0", "0", "EAP", "1/1/1800", "1/1/1800", "", strLocAmount, "0.00", "Employee Salary Payment", "", Session["EmpId"].ToString(), strCurrencyId, strForeignExchangerate, PaidTotaamount.ToString(), strCompAmount, "0.00", "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                //}
                                //else
                                {
                                    objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", EmployeeAccountId, Emp_Id, "0", "EAP", Cheque_Issue_Date, "1/1/1800", txtChequeNo.Text, Emp_Sal, "0.00", "Employee Salary Payment", "", Session["EmpId"].ToString(), strCurrencyId, strForeignExchangerate, Emp_Sal, strCompAmount, "0.00", "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                }
                            }
                        }

                        //For Credit


                        if (strAccountId.Split(',').Contains(txtpaymentCreditaccount.Text.Split('/')[1].ToString()))
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), "0", "0", "EAP", Cheque_Issue_Date, "1/1/1800", txtChequeNo.Text, "0.00", strLocAmount, "Employee Salary Payment", "", Session["EmpId"].ToString(), strCurrencyId, strForeignExchangerate, strLocAmount, "0.00", strCompAmount, "", "False", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                        else
                        {
                            objVoucherDetail.InsertVoucherDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), strVMaxId, "1", txtpaymentCreditaccount.Text.Split('/')[1].ToString(), "0", "0", "EAP", Cheque_Issue_Date, "1/1/1800", txtChequeNo.Text, "0.00", strLocAmount, "Employee Salary Payment", "", Session["EmpId"].ToString(), strCurrencyId, strForeignExchangerate, strLocAmount, "0.00", strCompAmount, "", "", "", "", "", "True", DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }


                        trns.Commit();
                        if (con.State == System.Data.ConnectionState.Open)
                        {
                            con.Close();
                        }
                        trns.Dispose();
                        con.Dispose();
                        Fill_Grid_Data();
                        ViewState["Select"] = null;
                        lblSelectedRecord.Text = "";
                        Txt_Total_Amount.Text = "0.00";
                        txtpaymentCreditaccount.Text = "";
                        txtVoucherDate.Text = "";
                        txtChequeIssueDate.Text = Convert.ToDateTime(DateTime.Now).ToString(ObjSys.SetDateFormat());
                        txtChequeNo.Text = "";
                        trCheque1.Visible = false;
                        Chk_Cheque_Payment.Checked = false;
                        DisplayMessage("Record Updated Successfully", "green");

                        Fill_List_Voucher();


                    }
                    catch (Exception ex)
                    {
                        DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));

                        trns.Rollback();
                        if (con.State == System.Data.ConnectionState.Open)
                        {

                            con.Close();
                        }
                        trns.Dispose();
                        con.Dispose();
                        return;


                    }



                }
            }
            else
            {
                DisplayMessage("Please select record");
                return;
            }
        }
        else
        {

            DisplayMessage("Record not found");
            return;


        }
    }

    protected string GetAccountNameByTransId(string strAccountNo)
    {
        string strAccountName = string.Empty;
        if (strAccountNo != "0" && strAccountNo != "")
        {
            string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and Trans_Id=" + strAccountNo + " and IsActive='True'";

            DataTable dtAccName = ObjDa.return_DataTable(sql);
            if (dtAccName.Rows.Count > 0)
            {
                strAccountName = dtAccName.Rows[0]["AccountName"].ToString() + "/" + strAccountNo;
            }
        }
        else
        {
            strAccountName = "";
        }
        return strAccountName;
    }


    protected void txtPayment_OnTextChanged(object sender, EventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((TextBox)sender).Parent.Parent;

        double Balance = 0;
        double Value = 0;
        try
        {
            Balance = Convert.ToDouble(((Label)gvrow.FindControl("lblBalance")).Text);
        }
        catch
        {


        }

        try
        {

            Value = Convert.ToDouble(((TextBox)gvrow.FindControl("txtPayment")).Text);
        }
        catch
        {


        }
        if (Value > Balance)
        {
            DisplayMessage("payment value should be equal or less then balance");
            ((TextBox)gvrow.FindControl("txtPayment")).Text = "0";
            ((TextBox)gvrow.FindControl("txtPayment")).Focus();
            Set_Total_Amount();
            return;
        }

        Set_Total_Amount();
    }
    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {
        Txt_Total_Amount.Text = "0.00";
        txtpaymentCreditaccount.Text = "";
        Fill_Grid_Data();
        Session["CHECKED_ITEMS"] = null;
      
        txtChequeIssueDate.Text = Convert.ToDateTime(DateTime.Now).ToString(ObjSys.SetDateFormat());
        txtChequeNo.Text = "";
        trCheque1.Visible = false;
        Chk_Cheque_Payment.Checked = false;
    }


    public double getEmployeeBalance(string strEmpId, ref SqlTransaction trns)
    {
        double Balance = 0;

        DataTable dt = ObjDa.return_DataTable("select Pay_EmployeeSalaryStatement.Emp_Id,Set_EmployeeMaster.Emp_Code,Set_EmployeeMaster.Emp_Name,Set_EmployeeMaster.Emp_Name_L,REPLACE(CONVERT(CHAR(11), Set_EmployeeMaster.DOj, 106), ' ', '-') as DateOfjoining, sum( CAST( Pay_EmployeeSalaryStatement.Cr_cmp_amount as decimal(18,3))  ) as Credit_Amount,sum(CAST( Pay_EmployeeSalaryStatement.Dr_cmp_amount as decimal(18,3))) as Debit_Amount,(sum( CAST( Pay_EmployeeSalaryStatement.Cr_cmp_amount as decimal(18,3))  )-sum(CAST( Pay_EmployeeSalaryStatement.Dr_cmp_amount as decimal(18,3)))) as Balance from Pay_EmployeeSalaryStatement inner join Set_EmployeeMaster on Pay_EmployeeSalaryStatement.Emp_Id=Set_EmployeeMaster.emp_id where Set_EmployeeMaster.Location_Id=" + Session["LocId"].ToString() + " and Pay_EmployeeSalaryStatement.Emp_Id=" + strEmpId + "  group by Pay_EmployeeSalaryStatement.Emp_Id,Set_EmployeeMaster.Emp_Code,Set_EmployeeMaster.Emp_Name,Set_EmployeeMaster.Emp_Name_L, Set_EmployeeMaster.DOj having (sum( CAST( Pay_EmployeeSalaryStatement.Cr_cmp_amount as decimal(18,3))  )-sum(CAST( Pay_EmployeeSalaryStatement.Dr_cmp_amount as decimal(18,3))))>0   ", ref trns);

        if (dt.Rows.Count > 0)
        {
            Balance = Convert.ToDouble(dt.Rows[0]["Balance"].ToString());

        }

        return Balance;

    }

    public string GetNarration(ref SqlTransaction trns)
    {
        string strNarration = "Employee Salary Payment - ";
        ArrayList userdetails = new ArrayList();
        userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails.Count > 0)
        {
            for (int j = 0; j < userdetails.Count; j++)
            {
                strNarration += getEmployeeName(userdetails[j].ToString(), ref trns) + "(" + getEmployeeBalance(userdetails[j].ToString(), ref trns) + ")" + ",";
            }
        }


        return strNarration;

    }


    public double GetBalance(ref SqlTransaction trns)
    {
        double TotalSum = 0;

        ArrayList userdetails = new ArrayList();
        userdetails = (ArrayList)Session["CHECKED_ITEMS"];
        if (userdetails.Count > 0)
        {
            for (int j = 0; j < userdetails.Count; j++)
            {
                TotalSum += getEmployeeBalance(userdetails[j].ToString(), ref trns);
            }
        }


        return TotalSum;

    }


    public string getConvertedAmount(string strFromCurrency, string strToCurrency, string strAmount)
    {
        string strconvertedAmount = string.Empty;
        double ExchangeRate = 0;
        try
        {
            ExchangeRate = Convert.ToDouble(SystemParameter.GetExchageRate(strFromCurrency, strToCurrency, Session["DBConnection"].ToString()));
        }
        catch
        {

        }

        if (strAmount == "")
        {
            strAmount = "0";
        }

        strconvertedAmount = ObjSys.GetCurencyConversionForInv(strToCurrency, (Convert.ToDouble(strAmount) * ExchangeRate).ToString());

        return strconvertedAmount;
    }


    public string getEmployeeName(string strEmpId, ref SqlTransaction trns)
    {
        string EmployeeName = string.Empty;

        DataTable Dt = objEmp.GetEmployeeMasterById(HttpContext.Current.Session["CompId"].ToString(), strEmpId, ref trns);
        if (Dt.Rows.Count > 0)
        {
            EmployeeName = Dt.Rows[0]["Emp_Name"].ToString();
        }

        return EmployeeName;
    }


    //protected void btnAllRefresh_Click(object sender, EventArgs e)
    //{
    //    dpDepartment.SelectedIndex = 0;
    //    Fill_Grid_Data();
    //    AllPageCode();

    //}
    //protected void dpDepartment_SelectedIndexChanged(object senderr, EventArgs e)
    //{

    //    Fill_Grid_Data();
    //    AllPageCode();


    //}
    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            strNewDate = DateTime.Parse(strDate).ToString(ObjSys.SetDateFormat());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }
    //private void FillddlDeaprtment()
    //{
    //    // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
    //    DataTable dt = objEmp.GetEmployeeOrDepartment("0", "0", "0", "0", "0");

    //    string DepIds = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D");

    //    if (!Common.GetStatus())
    //    {
    //        if (DepIds != "")
    //        {
    //            dt = new DataView(dt, "Dep_Id in(" + DepIds.Substring(0, DepIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
    //        }


    //    }

    //    dt = dt.DefaultView.ToTable(true, "DeptName", "Dep_Id");



    //    if (dt.Rows.Count > 0)
    //    {
    //        dpDepartment.DataSource = null;
    //        dpDepartment.DataBind();
    //        //Common Function add By Lokesh on 23-05-2015
    //        ObjComman.FillData((object)dpDepartment, dt, "DeptName", "Dep_Id");
    //    }
    //    else
    //    {
    //        try
    //        {
    //            dpDepartment.Items.Clear();
    //            dpDepartment.DataSource = null;
    //            dpDepartment.DataBind();
    //            dpDepartment.Items.Insert(0, "--Select--");
    //            dpDepartment.SelectedIndex = 0;
    //        }
    //        catch
    //        {
    //            dpDepartment.Items.Insert(0, "--Select--");
    //            dpDepartment.SelectedIndex = 0;
    //        }
    //    }
    //}

    private void Fill_Payment_Option()
    {
        DataTable Dt_Account_Parameter = objEmpParam.Get_Account_Employee_Payment("0", "2");

        Dt_Account_Parameter = Dt_Account_Parameter.DefaultView.ToTable(true, "AccountName", "Trans_Id");

        if (Dt_Account_Parameter != null && Dt_Account_Parameter.Rows.Count > 0)
        {
            Ddl_Payment_Option.DataSource = null;
            Ddl_Payment_Option.DataBind();
            objPageCmn.FillData((object)Ddl_Payment_Option, Dt_Account_Parameter, "AccountName", "Trans_Id");
        }
        else
        {
            try
            {
                Ddl_Payment_Option.Items.Clear();
                Ddl_Payment_Option.DataSource = null;
                Ddl_Payment_Option.DataBind();
                Ddl_Payment_Option.Items.Insert(0, "--Select--");
                Ddl_Payment_Option.SelectedIndex = 0;
            }
            catch
            {
                Ddl_Payment_Option.Items.Insert(0, "--Select--");
                Ddl_Payment_Option.SelectedIndex = 0;
            }
        }
        //if (Session["Emp_payment_Grid_Data_all"] != null)
        //{
        //    DataTable dt = Session["Emp_payment_Grid_Data_all"] as DataTable;
        //    if (dt.Rows.Count > 0)
        //    {
        //        dt = new DataView(dt, "closing_final <> 0", "", DataViewRowState.CurrentRows).ToTable();
        //    }
        //    if (dt != null && dpDepartment.SelectedIndex > 0)
        //    {
        //        dt = new DataView(dt, "Department_Id=" + dpDepartment.SelectedValue.Trim() + "", "", DataViewRowState.CurrentRows).ToTable();
        //    }
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        DataTable dt_Account_Id = dt.DefaultView.ToTable(true, "Payment_Opt_Account_ID");
        //        if (dt_Account_Id != null && dt_Account_Id.Rows.Count > 0)
        //        {
        //            string Account_Id = string.Empty;
        //            foreach (DataRow Dr in dt_Account_Id.Rows)
        //            {
        //                Account_Id = Account_Id + "," + Dr["Payment_Opt_Account_ID"].ToString();
        //            }
        //            DataTable Dt_Account_Parameter = objEmpParam.Get_Account_Employee_Payment(Account_Id, "1");
        //            if (Dt_Account_Parameter != null && Dt_Account_Parameter.Rows.Count > 0)
        //            {
        //                Ddl_Payment_Option.DataSource = null;
        //                Ddl_Payment_Option.DataBind();
        //                ObjComman.FillData((object)Ddl_Payment_Option, Dt_Account_Parameter, "AccountName", "Trans_Id");
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    Ddl_Payment_Option.Items.Clear();
        //                    Ddl_Payment_Option.DataSource = null;
        //                    Ddl_Payment_Option.DataBind();
        //                    Ddl_Payment_Option.Items.Insert(0, "--Select--");
        //                    Ddl_Payment_Option.SelectedIndex = 0;
        //                }
        //                catch
        //                {
        //                    Ddl_Payment_Option.Items.Insert(0, "--Select--");
        //                    Ddl_Payment_Option.SelectedIndex = 0;
        //                }
        //            }
        //        }
        //    }
        //}
    }

    protected void Ddl_Payment_Option_SelectedIndexChanged(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");

        if (Ddl_Payment_Option.SelectedIndex > 0)
            txtpaymentCreditaccount.Text = Ddl_Payment_Option.SelectedItem.ToString().Trim() + "/" + Ddl_Payment_Option.SelectedValue.Trim();
        else
            txtpaymentCreditaccount.Text = "";
        FillGrid_Payment();
        
    }

    protected void Img_Payment_Option_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        Ddl_Payment_Option.SelectedIndex = 0;
        Fill_Grid_Data();
       
    }

    public void Fill_List_Voucher()
    {
        DataTable Dt_Voucher = objVoucherDetail.Get_Employee_Payment_Voucher(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "0", "Employee Salary", "EAP", "0");
        if (Dt_Voucher != null && ddl_Location.SelectedIndex > 0)
        {
            Dt_Voucher = new DataView(Dt_Voucher, "Location_To=" + ddl_Location.SelectedValue.Trim() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (Dt_Voucher != null && ddl_Department.SelectedIndex > 0)
        {
            Dt_Voucher = new DataView(Dt_Voucher, "Department_Id=" + ddl_Department.SelectedValue.Trim() + "", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (Dt_Voucher != null && Dt_Voucher.Rows.Count > 0)
        {
            objPageCmn.FillData((object)Gv_Payment_Voucher, Dt_Voucher, "", "");
            Session["Dt_Voucher_List"] = Dt_Voucher;
            Lbl_TotalRecords.Text = Resources.Attendance.Total_Records + ": " + Dt_Voucher.Rows.Count.ToString() + "";
        }
        else
        {
            Gv_Payment_Voucher.DataSource = null;
            Gv_Payment_Voucher.DataBind();
            Lbl_TotalRecords.Text = Resources.Attendance.Total_Records + ": 0";
        }
       
    }

    protected string GetCurrencySymbol(string Amount, string CurrencyId)
    {
        return SystemParameter.GetCurrencySmbol(CurrencyId, SetDecimal(CurrencyId, Amount.ToString()), Session["DBConnection"].ToString());
    }

    public string SetDecimal(string amount, string CurrencyId)
    {
        return ObjSys.GetCurencyConversionForInv(CurrencyId, amount);
    }

    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        string SearchField = string.Empty;
        string SearchType = string.Empty;
        string SearchValue = string.Empty;

        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlFieldName.SelectedItem.Value == "Voucher_Date")
            {
                if (txtValueDate.Text != "")
                {
                    try
                    {
                        if (ddlOption.SelectedIndex == 1)
                        {
                            condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValueDate.Text.Trim() + "'";
                            SearchType = "Equal";
                        }
                        else if (ddlOption.SelectedIndex == 2)
                        {
                            condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValueDate.Text.Trim() + "'";
                            SearchType = "Equal";
                        }
                        else
                        {
                            condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValueDate.Text.Trim() + "'";
                            SearchType = "Equal";
                        }
                    }
                    catch
                    {
                        DisplayMessage("Enter date in format " + ObjSys.SetDateFormat() + "");
                        txtValueDate.Text = "";
                        txtValue.Text = "";
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueDate);
                        return;
                    }
                }
                else
                {
                    DisplayMessage("Enter date");
                    txtValueDate.Focus();
                    return;
                }
            }
            else if (ddlFieldName.SelectedItem.Value == "Voucher_Amount")
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) LIKE '%" + txtValue.Text.Trim() + "%'";
                SearchType = "Like";
            }
            else
            {
                if (ddlOption.SelectedIndex == 1)
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
                    SearchType = "Equal";
                }
                else if (ddlOption.SelectedIndex == 2)
                {
                    condition = "" + ddlFieldName.SelectedValue + " like '%" + txtValue.Text.Trim() + "%'";
                    SearchType = "Contains";
                }
                else
                {
                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
                    SearchType = "Like";
                }
            }
            DataTable Dt_Search = (DataTable)Session["Dt_Voucher_List"];
            DataView view = new DataView(Dt_Search, condition, "", DataViewRowState.CurrentRows);
            Session["HEP_Dt_Filter"] = view.ToTable();
            if (view.ToTable() != null && view.ToTable().Rows.Count > 0)
            {
                Gv_Payment_Voucher.DataSource = view.ToTable();
                Gv_Payment_Voucher.DataBind();
                Lbl_TotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            }
            else
            {
                Gv_Payment_Voucher.DataSource = null;
                Gv_Payment_Voucher.DataBind();
                Lbl_TotalRecords.Text = Resources.Attendance.Total_Records + ": 0";
            }
        }
    
        if (txtValue.Text != "")
            txtValue.Focus();
        else if (txtValueDate.Text != "")
            txtValueDate.Focus();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        txtValue.Text = "";
         DataTable Dt_Refresh = (DataTable)Session["Dt_Voucher_List"];
        if (Dt_Refresh != null && Dt_Refresh.Rows.Count > 0)
        {
            Gv_Payment_Voucher.DataSource = Dt_Refresh;
            Gv_Payment_Voucher.DataBind();
            Lbl_TotalRecords.Text = Resources.Attendance.Total_Records + ": " + Dt_Refresh.Rows.Count.ToString() + "";
        }
        else
        {
            Gv_Payment_Voucher.DataSource = null;
            Gv_Payment_Voucher.DataBind();
            Lbl_TotalRecords.Text = Resources.Attendance.Total_Records + ": 0";
        }
      
    }

    protected void Img_Emp_List_Select_All_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

    }

    protected void Btn_List_Click(object sender, EventArgs e)
    {

    }

    protected void Btn_New_Click(object sender, EventArgs e)
    {

    }

    protected void IbtnPrint_Command(object sender, CommandEventArgs e)
    {
        Session["EP_Voucher_No"] = e.CommandArgument.ToString();
        string strCmd = string.Format("window.open('../HR/HR_EmployeePayment_Print.aspx','window','width=1024, ');");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
    }

    protected void ddlFieldName_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");

        if (ddlFieldName.Text == "Voucher_Date")
        {
            txtValue.Visible = false;
            txtValueDate.Visible = true;
        }
        else
        {
            txtValue.Visible = true;
            txtValueDate.Visible = false;
        }
        txtValue.Text = "";
        txtValueDate.Text = "";
    }

    public void Set_Total_Amount()
    {
        double tot = 0;
        foreach (GridViewRow gvrow in gvPayment.Rows)
        {
            TextBox Txt_Payment = (TextBox)gvrow.FindControl("txtPayment");
            CheckBox Chk = (CheckBox)gvrow.FindControl("chkgvSelect");
            if (Chk.Checked == true)
            {
                if (Convert.ToDouble(Txt_Payment.Text) < 0)
                {
                    Chk.Checked = false;
                }
                else
                {
                    tot += Convert.ToDouble(Txt_Payment.Text);
                }
            }
        }
        Hdn_Total_Amount.Value = tot.ToString();
        Txt_Total_Amount.Text = tot.ToString();
    }

    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        Set_Total_Amount();
    }

    protected void ddl_Location_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDepartment(ddl_Location, ddl_Department);
        Fill_List_Voucher();
        ddl_Department.Focus();
       
    }

    protected void ddl_Department_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddl_Department.Focus();
        Fill_List_Voucher();
      
    }

    private void FillddlLocationList()
    {
        ddl_Location.Items.Clear();
        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string LocIds = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
        if (!Common.GetStatus(Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }
        objPageCmn.FillData((object)ddl_Location, dtLoc, "Location_Name", "Location_Id");
        objPageCmn.FillData((object)DDL_Location_New, dtLoc, "Location_Name", "Location_Id");
        ddl_Location.SelectedValue = Session["LocId"].ToString();
        DDL_Location_New.SelectedValue = Session["LocId"].ToString();
    }

    public void FillDepartment(DropDownList ddlLocation, DropDownList ddlDepartment)
    {
        ddlDepartment.Items.Clear();
        DataTable dtDepartment = new DataTable();
        string strDepId = string.Empty;
        string strLocId = Session["LocId"].ToString();

        if (ddlLocation.SelectedIndex > 0)
        {
            strLocId = ddlLocation.SelectedValue;
        }
        dtDepartment = objDep.GetDepartmentMaster();

        strDepId = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (strDepId == "")
        {
            strDepId = "0,";
        }
        dtDepartment = new DataView(dtDepartment, "Dep_Id in(" + strDepId.Substring(0, strDepId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
        objPageCmn.FillData((object)ddlDepartment, dtDepartment, "Dep_Name", "Dep_Id");
    }

    protected void DDL_Location_New_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDepartment(DDL_Location_New, DDL_Department_New);
        Fill_Grid_Data();
        ddl_Department.Focus();
       
    }

    protected void DDL_Department_New_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddl_Department.Focus();
        Fill_Grid_Data();
      
    }

    protected void Chk_Cheque_Payment_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Cheque_Payment.Checked == true)
        {
            trCheque1.Visible = true;
            RequiredFieldValidator2.ValidationGroup = "Save";
            RequiredFieldValidator4.ValidationGroup = "Save";
        }
        else
        {
            trCheque1.Visible = false;
            RequiredFieldValidator2.ValidationGroup = "Not_Save";
            RequiredFieldValidator4.ValidationGroup = "Not_Save";
        }
    }
    protected void Txt_Salary_Payment_Option_TextChanged(object sender, EventArgs e)
    {
        if (((TextBox)sender).Text != "")
        {
            try
            {
                ((TextBox)sender).Text.Split('/')[0].ToString();

                string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and AccountName='" + ((TextBox)sender).Text.Split('/')[0].ToString() + "' and IsActive='True' and Field1='False'";
                DataTable dtCOA = ObjDa.return_DataTable(sql);

                if (dtCOA.Rows.Count == 0)
                {
                    DisplayMessage("Choose account in suggestion only");
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Focus();
                }
            }
            catch
            {

                DisplayMessage("Choose account in suggestion only");
                ((TextBox)sender).Text = "";
                ((TextBox)sender).Focus();
            }
        }
        if (Btn_Div_Additional_Info.Attributes["class"].ToString() == "fa fa-plus")
        {
            Btn_Div_Additional_Info.Attributes.Add("Class", "fa fa-minus");
            Div_Additional_Info.Attributes.Add("Class", "box box-primary");
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListAccountName_Payment(string prefixText, int count, string contextKey)
    {
        string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + HttpContext.Current.Session["CompId"].ToString() + " and IsActive='True' and Field1='False'";
        DataAccessClass daclass = new DataAccessClass(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtCOA = daclass.return_DataTable(sql);

        string filtertext = "AccountName like '%" + prefixText + "%'";
        try
        {
            dtCOA = new DataView(dtCOA, filtertext, "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {

        }


        string[] txt = new string[dtCOA.Rows.Count];

        if (dtCOA.Rows.Count > 0)
        {
            for (int i = 0; i < dtCOA.Rows.Count; i++)
            {
                txt[i] = dtCOA.Rows[i]["AccountName"].ToString() + "/" + dtCOA.Rows[i]["Trans_Id"].ToString() + "/";
            }
        }

        return txt;
    }

    protected void Btn_Payment_Update_Click(object sender, EventArgs e)
    {
        int Check_count = 0;
        string Emp_Id = string.Empty;
        if (Txt_Salary_Payment_Option_M.Text == "")
        {
            DisplayMessage("Please enter salary payment option");
            return;
        }
        foreach (GridViewRow gvr in gvPayment.Rows)
        {
            if (((CheckBox)gvr.FindControl("chkgvSelect")).Checked)
            {
                Label lblEmpId = (Label)gvr.FindControl("lblEmpId");
                Emp_Id = Emp_Id + lblEmpId.Text + ",";
                Check_count++;
            }
        }
        if (Check_count == 0)
        {
            DisplayMessage("Please select at least one record");
            return;
        }
        string Salary_Payment_Option = string.Empty;
        if (Txt_Salary_Payment_Option_M.Text != "")
            Salary_Payment_Option = Txt_Salary_Payment_Option_M.Text.Split('/')[1].ToString();
        else
            Salary_Payment_Option = "0";
        int b = objEmpParam.Update_Employee_payment_Account(Session["CompId"].ToString(), Emp_Id, Salary_Payment_Option);
        if (b != 0)
        {
            DisplayMessage("Record update");
        }
        if (Btn_Div_Additional_Info.Attributes["class"].ToString() == "fa fa-plus")
        {
            Btn_Div_Additional_Info.Attributes.Add("Class", "fa fa-minus");
            Div_Additional_Info.Attributes.Add("Class", "box box-primary");
        }
    }

    //protected void Chk_Payment_Update_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (Chk_Payment_Update.Checked == true)
    //    {
    //        Div_Payment_Account.Visible = true;
    //        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Show_Modal_Salary()", true);
    //    }
    //    else
    //    {
    //        Div_Payment_Account.Visible = false;
    //    }
    //}

    protected void chkEmployeeSelect_CheckedChanged(object sender, EventArgs e)
    {
        FillGrid_Payment();
    }
}