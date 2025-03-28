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
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.IO;
using System.Globalization;
using System.Drawing;

public partial class GeneralLedger_TrailBalance : System.Web.UI.Page
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
            //Check_Page_Permission Chk_Page_ = new Check_Page_Permission();
            //if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "329").ToString() == "False")
            //{
            //    Session.Abandon();
            //    Response.Redirect("~/ERPLogin.aspx");
            //}
            //AllPageCode();
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../GeneralLedger/TrailBalance.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            StrCompId = Session["CompId"].ToString();
            StrBrandId = Session["BrandId"].ToString();
            strLocationId = Session["LocId"].ToString();
            Calender_VoucherDate.Format = objsys.SetDateFormat();
            CalendarExtender1.Format = objsys.SetDateFormat();
            FillLocation();
            rbUnPost.Checked = true;
            rbPost.Checked = false;

            txtToDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());
            txtFromDate.Text = DateTime.Now.ToString(objsys.SetDateFormat());
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

        if (GVTrailBalance.Rows.Count > 0)
        {
            Response.Clear();
            Response.Buffer = true;

            Response.AddHeader("content-disposition",
            "attachment;filename=TrailBalanceData.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //GVTrailBalance.AllowPaging = false;
            //GVTrailBalance.DataBind();

            //Change the Header Row back to white color
            GVTrailBalance.HeaderRow.Style.Add("background-color", "#FFFFFF");

            //Apply style to Individual Cells
            GVTrailBalance.HeaderRow.Cells[0].Style.Add("background-color", "green");
            GVTrailBalance.HeaderRow.Cells[1].Style.Add("background-color", "green");
            GVTrailBalance.HeaderRow.Cells[2].Style.Add("background-color", "green");
            //GVTrailBalance.HeaderRow.Cells[3].Style.Add("background-color", "green");

            for (int i = 0; i < GVTrailBalance.Rows.Count; i++)
            {
                GridViewRow row = GVTrailBalance.Rows[i];

                //Change Color back to white
                row.BackColor = System.Drawing.Color.White;

                //Apply text style to each Row
                row.Attributes.Add("class", "textmode");

                //Apply style to Individual Cells of Alternating Row
                if (i % 2 != 0)
                {
                    row.Cells[0].Style.Add("background-color", "#C2D69B");
                    row.Cells[1].Style.Add("background-color", "#C2D69B");
                    row.Cells[2].Style.Add("background-color", "#C2D69B");
                    //row.Cells[3].Style.Add("background-color", "#C2D69B");
                }
            }
            GVTrailBalance.RenderControl(hw);

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
        else
        {
            DisplayMessage("Need to Fill From Date");
            txtFromDate.Focus();
            return;
        }

        //for Check Financial Year
        if (!Common.IsFinancialyearDateCheckOnly(Convert.ToDateTime(txtFromDate.Text),Session["FinanceTodate"].ToString(), HttpContext.Current.Session["FinanceFromdate"].ToString()))
        {
            GVTrailBalance.DataSource = null;
            GVTrailBalance.DataBind();
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }

        //With ToDate Check 
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
            DisplayMessage("Need to Fill From Date");
            txtToDate.Focus();
            return;
        }

        //for Check Financial Year
        if (!Common.IsFinancialyearDateCheckOnly(Convert.ToDateTime(txtToDate.Text),Session["FinanceTodate"].ToString(), HttpContext.Current.Session["FinanceFromdate"].ToString()))
        {
            GVTrailBalance.DataSource = null;
            GVTrailBalance.DataBind();
            DisplayMessage("Log In Financial year not allowing to perform this action");
            return;
        }


        string strCurrency = ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString();
        string stNatureIds = string.Empty;
        DataTable dtNature = objNature.GetNatureAccountsAllTrue();
        if (dtNature.Rows.Count > 0)
        {
            for (int N = 0; N < dtNature.Rows.Count; N++)
            {
                if (stNatureIds == "")
                {
                    stNatureIds = dtNature.Rows[N]["Trans_Id"].ToString();
                }
                else
                {
                    stNatureIds = stNatureIds + "," + dtNature.Rows[N]["Trans_Id"].ToString();
                }
            }
        }

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
                DataTable dtTrailBalance = objDA.return_DataTable("select *, Cast((case when Opening_bal>0 then Opening_bal else 0 end) as Varchar) as Open_Debit, Cast((case when Opening_bal<0 then abs(Opening_bal) else 0 end) as Varchar) as Open_Credit , CAST((Debit_bal) as Varchar) as Debit, CAST((Credit_bal) as Varchar) as Credit, Cast((case when cb>0 then cb else 0 end) as Varchar) as Closing_Debit, Cast((case when cb<0 then abs(cb) else 0 end) as Varchar) as Closing_Credit  from dbo.fn_Ac_allAccounts_Balance('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + strLocationId + "','" + stNatureIds + "','0','" + txtFromDate.Text + "','" + txtToDate.Text + "','" + strCurrencyType + "','" + strReceiveVoucher + "','" + strPaymentVoucherAcc + "',0,0,0, '" + Session["FinanceYearId"].ToString() + "')ab");

                if (dtTrailBalance.Rows.Count > 0)
                {
                    dtTrailBalance = new DataView(dtTrailBalance, "Open_Debit<>'0' or Open_Credit<>'0' or Debit<>'0' or Credit<>'0' or Closing_Debit<>'0' or Closing_Credit<>'0'", "", DataViewRowState.CurrentRows).ToTable();
                }

                DataTable dtStockDetail = objDA.return_DataTable("select dbo.Inv_GetStockDetail('" + Session["CompId"].ToString() + "','" + Session["BrandId"].ToString() + "','" + strLocationId.ToString() + "','" + txtToDate.Text + "','0','" + Session["FinanceYearId"].ToString() + "',3)");
                string strOpeningStock = dtStockDetail.Rows[0][0].ToString();

                DataRow row;
                row = dtTrailBalance.NewRow();
                row["Name"] = "<b>" + "Opening Stock" + "</b>";
                row["Open_Debit"] = objsys.GetCurencyConversionForInv(strCurrency, strOpeningStock);
                row["Closing_Debit"] = objsys.GetCurencyConversionForInv(strCurrency, strOpeningStock);
                row["cb_type"] = "DR";
                row["parent_id"] = -1;
                row["account_no"] = -1;
                row["other_account_no"] = 0;
                row["foreign_cb"] = 0;
                row["company_cb"] = 0;
                dtTrailBalance.Rows.Add(row);

                double TotalOpenDebit = 0;
                double TotalOpenCredit = 0;
                double TotalDebit = 0;
                double TotalCredit = 0;
                double TotalCloseDebit = 0;
                double TotalCloseCredit = 0;
                foreach (DataColumn dc in dtTrailBalance.Columns)
                {
                    dtTrailBalance.Columns[dc.ColumnName.ToString()].ReadOnly = false;

                    if (dc.ColumnName.ToString() == "name")
                    {
                        dtTrailBalance.Columns["name"].MaxLength = 1000;
                    }
                }

                if (dtTrailBalance.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtTrailBalance.Rows)
                    {
                        dt["Open_Debit"] = objsys.GetCurencyConversionForInv(strCurrency, dt["Open_Debit"].ToString());
                        dt["Open_Credit"] = objsys.GetCurencyConversionForInv(strCurrency, dt["Open_Credit"].ToString());
                        dt["Debit"] = objsys.GetCurencyConversionForInv(strCurrency, dt["Debit"].ToString());
                        dt["Credit"] = objsys.GetCurencyConversionForInv(strCurrency, dt["Credit"].ToString());
                        dt["Closing_Debit"] = objsys.GetCurencyConversionForInv(strCurrency, dt["Closing_Debit"].ToString());
                        dt["Closing_Credit"] = objsys.GetCurencyConversionForInv(strCurrency, dt["Closing_Credit"].ToString());

                        dt["name"] = dt["name"].ToString().Replace(" ", "&nbsp;");

                        if (dt["account_no"].ToString() != "0" && dt["parent_id"].ToString() != "0")
                        {
                            TotalOpenDebit += Convert.ToDouble(dt["Open_Debit"]);
                            TotalOpenCredit += Convert.ToDouble(dt["Open_Credit"]);
                            TotalDebit += Convert.ToDouble(dt["Debit"]);
                            TotalCredit += Convert.ToDouble(dt["Credit"]);
                            TotalCloseDebit += Convert.ToDouble(dt["Closing_Debit"]);
                            TotalCloseCredit += Convert.ToDouble(dt["Closing_Credit"]);
                        }


                        if (dt["Open_Debit"].ToString() == "0")
                        {
                            dt["Open_Debit"] = "";
                        }
                        if (dt["Open_Credit"].ToString() == "0")
                        {
                            dt["Open_Credit"] = "";
                        }
                        if (dt["Debit"].ToString() == "0")
                        {
                            dt["Debit"] = "";
                        }
                        if (dt["Credit"].ToString() == "0")
                        {
                            dt["Credit"] = "";
                        }
                        if (dt["Closing_Debit"].ToString() == "0")
                        {
                            dt["Closing_Debit"] = "";
                        }
                        if (dt["Closing_Credit"].ToString() == "0")
                        {
                            dt["Closing_Credit"] = "";
                        }

                        if (dt["parent_id"].ToString() == "0" && dt["account_no"].ToString() == "0")
                        {
                            dt["name"] = "<b>" + dt["name"] + "</b>";

                            dt["Open_Debit"] = "";
                            dt["Open_Credit"] = "";
                            dt["Debit"] = "";
                            dt["Credit"] = "";
                            dt["Closing_Debit"] = "";
                            dt["Closing_Credit"] = "";
                        }
                        else if (dt["parent_id"].ToString() != "0" && dt["account_no"].ToString() == "0")
                        {
                            dt["name"] = "<b>" + dt["name"] + "</b>";

                            dt["Open_Debit"] = "";
                            dt["Open_Credit"] = "";
                            dt["Debit"] = "";
                            dt["Credit"] = "";
                            dt["Closing_Debit"] = "";
                            dt["Closing_Credit"] = "";
                        }
                        else
                        {
                            dt["name"] = "<i>" + dt["name"] + "</i>";

                            dt["Open_Debit"] = "<i>" + dt["Open_Debit"] + "</i>";
                            dt["Open_Credit"] = "<i>" + dt["Open_Credit"] + "</i>";
                            dt["Debit"] = "<i>" + dt["Debit"] + "</i>";
                            dt["Credit"] = "<i>" + dt["Credit"] + "</i>";
                            dt["Closing_Debit"] = "<i>" + dt["Closing_Debit"] + "</i>";
                            dt["Closing_Credit"] = "<i>" + dt["Closing_Credit"] + "</i>";
                        }
                    }

                    GVTrailBalance.DataSource = dtTrailBalance;
                    GVTrailBalance.DataBind();

                    if (GVTrailBalance.Rows.Count > 0)
                    {
                        Label lblgvOpenDebitTotal = (Label)GVTrailBalance.FooterRow.FindControl("lblgvOpeningBalDTotal");
                        Label lblgvOpenCreditTotal = (Label)GVTrailBalance.FooterRow.FindControl("lblgvOpeningBalCTotal");
                        Label lblgvDebitTotal = (Label)GVTrailBalance.FooterRow.FindControl("lblgvDebitTotal");
                        Label lblgvCreditTotal = (Label)GVTrailBalance.FooterRow.FindControl("lblgvCreditTotal");
                        Label lblgvClosingDebitTotal = (Label)GVTrailBalance.FooterRow.FindControl("lblgvClosingBalDTotal");
                        Label lblgvClosingCreditTotal = (Label)GVTrailBalance.FooterRow.FindControl("lblgvClosingBalCTotal");

                        lblgvOpenDebitTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, TotalOpenDebit.ToString());
                        lblgvOpenCreditTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, TotalOpenCredit.ToString());
                        lblgvDebitTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, TotalDebit.ToString());
                        lblgvCreditTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, TotalCredit.ToString());
                        lblgvClosingDebitTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, TotalCloseDebit.ToString());
                        lblgvClosingCreditTotal.Text = objsys.GetCurencyConversionForInv(strCurrency, TotalCloseCredit.ToString());
                        GVTrailBalance.HeaderRow.Cells[1].Text = SystemParameter.GetCurrencySmbol(strCurrencyId, GVTrailBalance.HeaderRow.Cells[1].Text + " ", Session["DBConnection"].ToString());
                        GVTrailBalance.HeaderRow.Cells[2].Text = SystemParameter.GetCurrencySmbol(strCurrencyId, GVTrailBalance.HeaderRow.Cells[2].Text + " ", Session["DBConnection"].ToString());
                    }
                }
            }
        }
    }

    //public void ExportTableData(DataTable dtdata)
    //{
    //    Response.ClearContent();
    //    Response.Buffer = true;
    //    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "ProductList.xls"));
    //    Response.ContentType = "application/ms-excel";
    //    DataTable dt = dtdata.Copy();
    //    string str = string.Empty;
    //    foreach (DataColumn dtcol in dt.Columns)
    //    {
    //        Response.Write(str + dtcol.ColumnName);
    //        str = "\t";
    //    }
    //    Response.Write("\n");
    //    foreach (DataRow dr in dt.Rows)
    //    {
    //        str = "";
    //        for (int j = 0; j < dt.Columns.Count; j++)
    //        {
    //            Response.Write(str + Convert.ToString(dr[j]));
    //            str = "\t";
    //        }
    //        Response.Write("\n");
    //    }
    //    Response.End();
    //}
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }





    #region Location

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
            foreach (System.Web.UI.WebControls.ListItem li in lstLocation.Items)
            {
                if (li.Selected)
                {
                    lstLocationSelect.Items.Add(li);
                }
            }
            foreach (System.Web.UI.WebControls.ListItem li in lstLocationSelect.Items)
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
            foreach (System.Web.UI.WebControls.ListItem li in lstLocationSelect.Items)
            {
                if (li.Selected)
                {
                    lstLocation.Items.Add(li);
                }
            }
            foreach (System.Web.UI.WebControls.ListItem li in lstLocation.Items)
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
        foreach (System.Web.UI.WebControls.ListItem li in lstLocation.Items)
        {
            lstLocationSelect.Items.Add(li);
        }
        foreach (System.Web.UI.WebControls.ListItem DeptItem in lstLocationSelect.Items)
        {
            lstLocation.Items.Remove(DeptItem);
        }
        btnPushAllDept.Focus();
    }
    protected void btnPullAllDept_Click(object sender, EventArgs e)
    {
        foreach (System.Web.UI.WebControls.ListItem li in lstLocationSelect.Items)
        {
            lstLocation.Items.Add(li);
        }
        foreach (System.Web.UI.WebControls.ListItem DeptItem in lstLocation.Items)
        {
            lstLocationSelect.Items.Remove(DeptItem);
        }
        btnPullAllDept.Focus();
    }
    #endregion
}