using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.IO;

public partial class GeneralLedger_ProfitAndLoss : System.Web.UI.Page
{
    SystemParameter objsys = null;
    LocationMaster ObjLocation = null;
    RoleMaster objRole = null;
    RoleDataPermission objRoleData = null;
    Common cmn = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Ac_Nature_Accounts objNature = null;
    CompanyMaster ObjCompany = null;
    Ac_ParameterMaster objAcParameter = null;

    string StrCompId = string.Empty;
    string StrBrandId = string.Empty;
    string strLocationId = string.Empty;
    string StrUserId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        objsys = new SystemParameter(Session["DBConnection"].ToString());
        ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objNature = new Ac_Nature_Accounts(Session["DBConnection"].ToString());
        ObjCompany = new CompanyMaster(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../GeneralLedger/ProfitAndLoss.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            //AllPageCode(clsPagePermission);
            //Check_Page_Permission Chk_Page_ = new Check_Page_Permission();
            //if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "284").ToString() == "False")
            //{
            //    Session.Abandon();
            //    Response.Redirect("~/ERPLogin.aspx");
            //}
            //AllPageCode();
            StrCompId = Session["CompId"].ToString();
            StrBrandId = Session["BrandId"].ToString();
            strLocationId = Session["LocId"].ToString();
            Calender_VoucherDate.Format = objsys.SetDateFormat();
            CalendarExtender1.Format = objsys.SetDateFormat();
            FillLocation();
            rbUnPost.Checked = true;
            rbPost.Checked = false;

            txtToDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());
        }

        //For Comment
        StrCompId = Session["CompId"].ToString();
        StrBrandId = Session["BrandId"].ToString();
        strLocationId = Session["LocId"].ToString();
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }
    protected void ExportToExcel(object sender, EventArgs e)
    {
        //btnExecute_Click(null, null);

        if (GVComplete.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;

            Response.AddHeader("content-disposition",
            "attachment;filename=Profit&LossData.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //GVTrailBalance.AllowPaging = false;
            //GVTrailBalance.DataBind();

            //Change the Header Row back to white color
            //GVComplete.HeaderRow.Style.Add("background-color", "#FFFFFF");

            //Apply style to Individual Cells
            //GVComplete.HeaderRow.Cells[0].Style.Add("background-color", "green");
            //GVComplete.HeaderRow.Cells[1].Style.Add("background-color", "green");
            //GVComplete.HeaderRow.Cells[2].Style.Add("background-color", "green");
            //GVTrailBalance.HeaderRow.Cells[3].Style.Add("background-color", "green");

            for (int i = 0; i < GVComplete.Rows.Count; i++)
            {
                GridViewRow row = GVComplete.Rows[i];

                //Change Color back to white
                row.BackColor = System.Drawing.Color.White;

                //Apply text style to each Row
                row.Attributes.Add("class", "textmode");

                //Apply style to Individual Cells of Alternating Row
                if (i % 2 != 0)
                {
                    // row.Cells[0].Style.Add("background-color", "#C2D69B");
                    // row.Cells[1].Style.Add("background-color", "#C2D69B");
                    // row.Cells[2].Style.Add("background-color", "#C2D69B");
                    //row.Cells[3].Style.Add("background-color", "#C2D69B");
                }
            }
            GVComplete.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { mso-number-format:\@; } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
        else
        {
            DisplayMessage("No Record Available");
        }
    }
    //public void AllPageCode()
    //{
    //    IT_ObjectEntry objObjectEntry = new IT_ObjectEntry();
    //    Common ObjComman = new Common();

    //    //New Code created by jitendra on 09-12-2014
    //    string strModuleId = string.Empty;
    //    string strModuleName = string.Empty;

    //    DataTable dtModule = objObjectEntry.GetModuleIdAndName("288");
    //    if (dtModule.Rows.Count > 0)
    //    {
    //        strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
    //        strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
    //    }
    //    else
    //    {
    //        Session.Abandon();
    //        Response.Redirect("~/ERPLogin.aspx");
    //    }

    //    //End Code
    //    Page.Title = objsys.GetSysTitle();
    //    StrUserId = Session["UserId"].ToString();
    //    StrCompId = Session["CompId"].ToString();
    //    StrBrandId = Session["BrandId"].ToString();
    //    strLocationId = Session["LocId"].ToString();
    //    Session["AccordianId"] = strModuleId;
    //    Session["HeaderText"] = strModuleName;
    //    if (Session["EmpId"].ToString() == "0")
    //    {
    //        //btnGetReport.Visible = true;
    //    }

    //    DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "288");
    //    foreach (DataRow DtRow in dtAllPageCode.Rows)
    //    {
    //        if (DtRow["Op_Id"].ToString() == "1")
    //        {
    //            //btnGetReport.Visible = true;
    //        }
    //    }
    //}
    protected void btnGetReport_Click(object sender, EventArgs e)
    {
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
            //objLocDept.InsertLocationDepartmentMaster(editid.Value, lstDepartmentSelect.Items[i].Value, "0", "", "", "", "", "", "", "", "", "True", DateTime.Now.ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        }

        if (strLocationId == "")
            strLocationId = Session["LocId"].ToString();


        DataTable DtFilter = new DataTable();
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

        DateTime DtFromdate = new DateTime();
        DateTime DttoDate = new DateTime();

        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            if (Convert.ToDateTime(txtFromDate.Text) > Convert.ToDateTime(txtToDate.Text))
            {
                DisplayMessage("from date should be less then to date ");
                txtFromDate.Focus();
                return;
            }

            DtFromdate = Convert.ToDateTime(txtFromDate.Text);
            DttoDate = Convert.ToDateTime(txtToDate.Text);
        }
        else
        {
            DtFromdate = Convert.ToDateTime(objVoucherHeader.GetVoucherRecord().Rows[0]["Voucher_Date"].ToString());
            DttoDate = DateTime.Now;
        }

        bool POstStatus = rbPost.Checked;

        AccountsDataset ObjAccountsDataset = new AccountsDataset();

        ObjAccountsDataset.EnforceConstraints = false;

        AccountsDatasetTableAdapters.sp_Ac_ChartOfAccount_TrialBalance_Report_UnPostTableAdapter adp = new AccountsDatasetTableAdapters.sp_Ac_ChartOfAccount_TrialBalance_Report_UnPostTableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        adp.Fill(ObjAccountsDataset.sp_Ac_ChartOfAccount_TrialBalance_Report_UnPost, Convert.ToInt32(Session["CompId"].ToString()), Convert.ToInt32(Session["LocId"].ToString()), DtFromdate, DttoDate, strLocationId, POstStatus);
        DtFilter = ObjAccountsDataset.sp_Ac_ChartOfAccount_TrialBalance_Report_UnPost;

        DtFilter = ObjAccountsDataset.sp_Ac_ChartOfAccount_TrialBalance_Report_UnPost;

        ArrayList objArr = new ArrayList();
        objArr.Add(DtFromdate.ToString(objsys.SetDateFormat()));
        objArr.Add(DttoDate.ToString(objsys.SetDateFormat()));
        objArr.Add(rbPost.Checked.ToString());

        Session["ArrDate"] = objArr;

        Session["dtTrialBalance"] = DtFilter;

        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Accounts_Report/TrialBalanceUnpostReport.aspx','window','width=1024');", true);

    }

    protected void btnExecute_Click(object sender, EventArgs e)
    {
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
        else
        {
            DisplayMessage("Need to Fill To Date");
            txtToDate.Focus();
            return;
        }

        //for Check Financial Year
        if (!Common.IsFinancialyearDateCheckOnly(Convert.ToDateTime(txtToDate.Text),Session["FinanceTodate"].ToString(), HttpContext.Current.Session["FinanceFromdate"].ToString()))
        {
            GVComplete.DataSource = null;
            GVComplete.DataBind();
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }

        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        string stNatureIds = "3,4";
        //DataTable dtNature = objNature.GetNatureAccountsAllTrue();
        //if (dtNature.Rows.Count > 0)
        //{
        //    for (int N = 0; N < dtNature.Rows.Count; N++)
        //    {
        //        if (stNatureIds == "")
        //        {
        //            stNatureIds = dtNature.Rows[N]["Trans_Id"].ToString();
        //        }
        //        else
        //        {
        //            stNatureIds = stNatureIds + "," + dtNature.Rows[N]["Trans_Id"].ToString();
        //        }
        //    }
        //}

        string strCurrencyId = string.Empty;
        string strCurrencyType = string.Empty;

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

        //Check Location Currency
        string strCurrencyIdNew = string.Empty;
        string strFlag = "True";
        if (strLocationId != "")
        {
            DataTable dtLocationData = ObjLocation.GetAllLocationMaster();
            if (dtLocationData.Rows.Count > 0)
            {
                dtLocationData = new DataView(dtLocationData, "Location_Id in (" + strLocationId + ")", "", DataViewRowState.CurrentRows).ToTable();

                for (int j = 0; j < dtLocationData.Rows.Count; j++)
                {
                    string strPresentCurrency = dtLocationData.Rows[j]["Field1"].ToString();

                    if (strCurrencyIdNew == "")
                    {
                        strCurrencyIdNew = strPresentCurrency;
                    }
                    else if (strCurrencyIdNew != "")
                    {
                        if (strCurrencyIdNew == strPresentCurrency)
                        {

                        }
                        else
                        {
                            strFlag = "False";
                            break;
                        }
                    }
                }
            }
        }

        if (strFlag == "True")
        {
            strCurrencyType = "1";
            string SelectedLocation = string.Empty;
            if (lstLocationSelect.Items.Count > 0)
            {
                SelectedLocation = lstLocationSelect.Items[0].Value.ToString();
            }
            else
            {
                SelectedLocation = Session["LocId"].ToString();
            }

            DataTable dtLocation = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), SelectedLocation);
            if (dtLocation.Rows.Count > 0)
            {
                strCurrencyId = dtLocation.Rows[0]["Field1"].ToString();
            }
        }
        else if (strFlag == "False")
        {
            strCurrencyType = "2";
            DataTable dtCompany = ObjCompany.GetCompanyMasterById(Session["CompId"].ToString());
            if (dtCompany.Rows.Count > 0)
            {
                strCurrencyId = dtCompany.Rows[0]["Currency_Id"].ToString();
            }
        }

        //For Account Information
        string strReceiveVoucher = string.Empty;
        string strPaymentVoucherAcc = string.Empty;
        string strNatureOfAccountIncome = string.Empty;
        string strNatureOfAccountExpenses = string.Empty;
        strNatureOfAccountIncome = "4";
        strNatureOfAccountExpenses = "3";


        DataTable dtAcParameter = objAcParameter.GetParameterMasterAllTrue(StrCompId);
        DataTable dtPaymentVoucher = new DataView(dtAcParameter, "Param_Name='Payment Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPaymentVoucher.Rows.Count > 0)
        {
            strPaymentVoucherAcc = dtPaymentVoucher.Rows[0]["Param_Value"].ToString();
        }
        DataTable dtReceiveVoucher = new DataView(dtAcParameter, "Param_Name='Receive Vouchers'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtReceiveVoucher.Rows.Count > 0)
        {
            strReceiveVoucher = dtReceiveVoucher.Rows[0]["Param_Value"].ToString();
        }
        //End

        if (txtToDate.Text != "")
        {
            if (stNatureIds != "" && stNatureIds != "0")
            {
                PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
                //DataTable dtTrailBalance = objDA.return_DataTable("select *, Cast((case when cb>0 then cb else 0 end) as Varchar) as Debit, Cast((case when cb<0 then abs(cb) else 0 end) as Varchar) as Credit   from dbo.fn_Ac_allAccounts_Balance('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + strLocationId + "','" + stNatureIds + "',0,'" + txtToDate.Text + "','" + strCurrencyType + "','" + strReceiveVoucher + "','" + strPaymentVoucherAcc + "',0,0,0)ab");
                DataTable dtBalance = objDA.return_DataTable("select nature_of_account,parent_id,account_no,name,cb as cb_actual, other_account_no,cast(abs(cb) as nvarchar)as cb,cast(cb_type as nvarchar)as cb_type from dbo.fn_Ac_allAccounts_Balance('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + strLocationId + "','" + stNatureIds + "',0,'" + txtToDate.Text + "','" + txtToDate.Text + "','" + strCurrencyType + "','" + strReceiveVoucher + "','" + strPaymentVoucherAcc + "',0,0,0, '" + Session["FinanceYearId"].ToString() + "')ab");

                //fillIncomeGrid();
                //fillExpensesGrid;
                if (chkShowZero.Checked == false)
                {
                    try
                    {
                        dtBalance = new DataView(dtBalance, "cb<>'0.000000'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch (Exception ex)
                    {

                    }
                }

                DataTable dtOpStock = objDA.return_DataTable("select dbo.Inv_GetStockDetail('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + strLocationId.ToString() + "','" + txtToDate.Text + "','0','" + Session["FinanceYearId"].ToString() + "',3)");
                string strOpeningStock = dtOpStock.Rows[0][0].ToString();


                DataTable dtClosingStock = objDA.return_DataTable("select dbo.Inv_GetStockDetail('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + strLocationId.ToString() + "','" + txtToDate.Text + "','0','" + Session["FinanceYearId"].ToString() + "',1)");
                string strClosingStock = dtClosingStock.Rows[0][0].ToString();

                DataRow tmp_row;
                tmp_row = dtBalance.NewRow();
                tmp_row["nature_of_account"] = 3;
                tmp_row["Name"] = "<b>" + "Opening Stock" + "</b>";
                tmp_row["cb"] = objsys.GetCurencyConversionForInv(strCurrency, strOpeningStock);
                tmp_row["cb_actual"] = tmp_row["cb"].ToString();
                tmp_row["cb_type"] = "DR";
                tmp_row["parent_id"] = -1;
                tmp_row["account_no"] = -1;
                tmp_row["other_account_no"] = 0;
                dtBalance.Rows.Add(tmp_row);

                tmp_row = dtBalance.NewRow();
                tmp_row["nature_of_account"] = 4;
                tmp_row["Name"] = "<b>" + "Closing Stock" + "</b>";
                tmp_row["cb"] = objsys.GetCurencyConversionForInv(strCurrency, strClosingStock);
                if (Convert.ToDouble(tmp_row["cb"]) < 0)
                {
                    tmp_row["cb_actual"] =  tmp_row["cb"].ToString();
                }
                else if (Convert.ToDouble(tmp_row["cb"]) > 0)
                {
                    tmp_row["cb_actual"] = "-" + tmp_row["cb"].ToString();
                }
              
                tmp_row["cb_type"] = "CR";
                tmp_row["parent_id"] = -1;
                tmp_row["account_no"] = -1;
                tmp_row["other_account_no"] = 0;
                dtBalance.Rows.Add(tmp_row);

                double TotalIncome = 0;
                double TotalExpenses = 0;

                foreach (DataColumn dc in dtBalance.Columns)
                {
                    dtBalance.Columns[dc.ColumnName.ToString()].ReadOnly = false;

                    if (dc.ColumnName.ToString() == "name")
                    {
                        dtBalance.Columns["name"].MaxLength = 1000;
                    }
                }

                if (dtBalance.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtBalance.Rows)
                    {
                        dt["cb"] = objsys.GetCurencyConversionForInv(strCurrency, dt["cb"].ToString());
                        //dt["Credit"] = objsys.GetCurencyConversionForInv(strCurrency, dt["Credit"].ToString());
                        dt["name"] = dt["name"].ToString().Replace(" ", "&nbsp;");

                        if (dt["nature_of_account"].ToString() == strNatureOfAccountIncome.ToString() && dt["account_no"].ToString() != "0")
                        {
                            TotalIncome += Convert.ToDouble(dt["cb_actual"]);
                        }

                        if (dt["nature_of_account"].ToString() == strNatureOfAccountExpenses.ToString() && dt["account_no"].ToString() != "0")
                        {
                            TotalExpenses += Convert.ToDouble(dt["cb_actual"]);
                        }

                        if (dt["cb"].ToString() == "0")
                        {
                            dt["cb"] = "";
                            dt["cb_type"] = "";
                        }


                        if (dt["parent_id"].ToString() == "0" && dt["account_no"].ToString() == "0")
                        {
                            dt["name"] = "<b>" + dt["name"] + "</b>";
                            dt["cb"] = "<b>" + dt["cb"] + "</b>";
                            dt["cb_type"] = "<b>" + dt["cb_type"] + "</b>";
                        }
                        else if (dt["parent_id"].ToString() != "0" && dt["account_no"].ToString() == "0")
                        {
                            dt["name"] = "<b>" + dt["name"] + "</b>";
                            dt["cb"] = "<b>" + dt["cb"] + "</b>";
                            dt["cb_type"] = "<b>" + dt["cb_type"] + "</b>";
                        }
                        else
                        {
                            dt["name"] = "<i>" + dt["name"] + "</i>";
                            dt["cb"] = "<i>" + dt["cb"] + "</i>";
                            dt["cb_type"] = "<i>" + dt["cb_type"] + "</i>";
                        }
                    }

                    TotalIncome = Math.Abs(TotalIncome);
                    TotalExpenses = Math.Abs(TotalExpenses);
                    double profit = 0;

                    if (dtBalance.Rows.Count > 0)
                    {
                        DataTable dtIncome = new DataView(dtBalance, "cb<>'0' and nature_of_account='" + strNatureOfAccountIncome + "'", "", DataViewRowState.CurrentRows).ToTable();
                        DataTable dtExpenses = new DataView(dtBalance, "cb<>'0' and nature_of_account='" + strNatureOfAccountExpenses + "'", "", DataViewRowState.CurrentRows).ToTable();
                        DataRow row;
                        profit = Math.Abs(TotalIncome) - Math.Abs(TotalExpenses);

                        if (profit > 0)
                        {
                            row = dtExpenses.NewRow();
                            row["Name"] = "<b>" + "By Profit" + "</b>";
                            row["cb"] = profit;
                            row["cb"] = "<b>" + objsys.GetCurencyConversionForInv(strCurrency, row["cb"].ToString()) + "<b>";
                            row["parent_id"] = 0;
                            row["account_no"] = 0;
                            row["other_account_no"] = 0;

                            dtExpenses.Rows.Add(row);
                            TotalExpenses += profit;
                        }
                        else if (profit < 0)
                        {
                            row = dtIncome.NewRow();
                            row["Name"] = "<b>" + "By Loss" + "</b>";
                            row["cb"] = Math.Abs(profit);
                            row["cb"] = "<b>" + objsys.GetCurencyConversionForInv(strCurrency, row["cb"].ToString()) + "<b>";
                            row["parent_id"] = 0;
                            row["account_no"] = 0;
                            row["other_account_no"] = 0;
                            dtIncome.Rows.Add(row);
                            TotalIncome += Math.Abs(profit);
                        }

                        if (dtExpenses.Rows.Count > dtIncome.Rows.Count)
                        {
                            //dtIncome.Rows.Count = dtExpenses.Rows.Count;
                            for (int counter = dtIncome.Rows.Count; counter < dtExpenses.Rows.Count; counter++)
                            {
                                row = dtIncome.NewRow();
                                row["Name"] = "&nbsp";
                                dtIncome.Rows.Add(row);
                            }
                        }
                        if (dtExpenses.Rows.Count < dtIncome.Rows.Count)
                        {
                            //dtIncome.Rows.Count = dtExpenses.Rows.Count;
                            for (int counter = dtExpenses.Rows.Count; counter < dtIncome.Rows.Count; counter++)
                            {
                                row = dtExpenses.NewRow();
                                row["Name"] = "&nbsp";
                                dtExpenses.Rows.Add(row);
                            }
                        }


                        DataTable dtLocation = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
                        if (dtLocation.Rows.Count > 0)
                        {
                            GVComplete.DataSource = dtLocation;
                            GVComplete.DataBind();
                        }

                        foreach (GridViewRow gvrC in GVComplete.Rows)
                        {
                            GridView gvIncome = (GridView)gvrC.FindControl("gvIncome");
                            GridView gvExpenses = (GridView)gvrC.FindControl("gvExpenses");

                            //fill income grid -------------
                            gvIncome.DataSource = dtIncome;
                            gvIncome.DataBind();

                            if (gvIncome.Rows.Count > 0)
                            {
                                Label lblgvCbTotal = (Label)gvIncome.FooterRow.FindControl("lblgvCbTotal");
                                // Label lblgvCreditTotal = (Label)gvIncome.FooterRow.FindControl("lblgvCreditTotal");

                                lblgvCbTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, TotalIncome.ToString());
                                //lblgvCreditTotal.Text = TotalCredit.ToString();
                                gvIncome.HeaderRow.Cells[1].Text = SystemParameter.GetCurrencySmbol(strCurrencyId, gvIncome.HeaderRow.Cells[1].Text + " ", Session["DBConnection"].ToString());
                                //gvIncome.HeaderRow.Cells[2].Text = SystemParameter.GetCurrencySmbol(strCurrencyId, gvIncome.HeaderRow.Cells[2].Text + " ");
                            }
                            //----------------------------

                            //fill income grid -------------
                            gvExpenses.DataSource = dtExpenses;
                            gvExpenses.DataBind();

                            if (gvExpenses.Rows.Count > 0)
                            {
                                Label lblgvCbTotal_1 = (Label)gvExpenses.FooterRow.FindControl("lblgvCbTotal_1");
                                // Label lblgvCreditTotal = (Label)gvIncome.FooterRow.FindControl("lblgvCreditTotal");

                                lblgvCbTotal_1.Text = objsys.GetCurencyConversionForInv(strCurrency, TotalExpenses.ToString());
                                //lblgvCreditTotal.Text = TotalCredit.ToString();
                                gvExpenses.HeaderRow.Cells[1].Text = SystemParameter.GetCurrencySmbol(strCurrencyId, gvExpenses.HeaderRow.Cells[1].Text + " ", Session["DBConnection"].ToString());
                                //gvExpenses.HeaderRow.Cells[2].Text = SystemParameter.GetCurrencySmbol(strCurrencyId, gvExpenses.HeaderRow.Cells[2].Text + " ");
                            }
                            //----------------------------                           
                        }
                    }
                }
            }
        }
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }

    #region Location
    //private void FillddlLocation()
    //{

    //    DataTable dtLoc = ObjLocation.GetLocationMaster(Session["CompId"].ToString());
    //    dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

    //    // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
    //    string LocIds = GetRoleDataPermission(Session["RoleId"].ToString(), "L");

    //    if (!GetStatus(Session["RoleId"].ToString()))
    //    {
    //        if (LocIds != "")
    //        {
    //            dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
    //        }


    //    }


    //    if (dtLoc.Rows.Count > 0)
    //    {
    //        ddlLocation.DataSource = null;
    //        ddlLocation.DataBind();
    //        ddlLocation.DataSource = dtLoc;
    //        ddlLocation.DataTextField = "Location_Name";
    //        ddlLocation.DataValueField = "Location_Id";
    //        ddlLocation.DataBind();
    //        ListItem li = new ListItem();
    //        li.Text = "All";
    //        li.Value = "0";
    //        ddlLocation.Items.Insert(0, li);
    //        try
    //        {
    //            ddlLocation.SelectedValue = Session["LocId"].ToString();
    //        }
    //        catch
    //        {
    //        }
    //    }
    //    else
    //    {
    //        try
    //        {
    //            ddlLocation.Items.Clear();

    //        }
    //        catch
    //        {

    //        }
    //    }
    //}

    #endregion

    #region LocationWork
    public void FillLocation()
    {
        lstLocation.Items.Clear();
        lstLocation.DataSource = null;
        lstLocation.DataBind();

        lstLocationSelect.Items.Clear();
        lstLocationSelect.DataSource = null;
        lstLocationSelect.DataBind();

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
            new PageControlCommon(Session["DBConnection"].ToString()).FillData((object)lstLocation, dtLoc, "Location_Name", "Location_Id");
        }
    }
    protected void btnPushDept_Click(object sender, EventArgs e)
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
        btnPushDept.Focus();

    }
    protected void btnPullDept_Click(object sender, EventArgs e)
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
        btnPullDept.Focus();
    }
    protected void btnPushAllDept_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstLocation.Items)
        {
            lstLocationSelect.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstLocationSelect.Items)
        {
            lstLocation.Items.Remove(DeptItem);
        }
        btnPushAllDept.Focus();
    }
    protected void btnPullAllDept_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lstLocationSelect.Items)
        {
            lstLocation.Items.Add(li);
        }
        foreach (ListItem DeptItem in lstLocation.Items)
        {
            lstLocationSelect.Items.Remove(DeptItem);
        }
        btnPullAllDept.Focus();
    }
    #endregion

}