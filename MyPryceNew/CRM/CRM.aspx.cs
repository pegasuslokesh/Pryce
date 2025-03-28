using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;

public partial class CRM_CRM : System.Web.UI.Page
{
    Common cmn = null;
    Set_CustomerMaster ObjCoustmer = null;
    SystemParameter objSys = null;
    Sys_AreaMaster objAreamaster = null;
    Ems_Contact_Group objCG = null;
    IT_ObjectEntry objObjectEntry = null;
    LocationMaster ObjLocation = null;
    Ems_GroupMaster objGroup = null;
    DataAccessClass objDa = null;
    CompanyMaster ObjCompany = null;
    Ac_ParameterMaster objAcParameter = null;
    Ac_Voucher_Detail objVoucherDetail = null;
    EmployeeMaster objEmployee = null;
    Ac_Voucher_Header objVoucherHeader = null;
    Set_CustomerMaster objcustomermaster = null;
    CurrencyMaster ObjCurrencyMaster = null;
    Ac_ChartOfAccount objCOA = null;
    CustomerAgeingEstatement objCustomerAgeingStatement = null;
    Ac_Ageing_Detail ObjAgeingDetail = null;
    Ac_Parameter_Location objAcParamLocation = null;
    Ac_AccountMaster objAcMaster = null;
    PageControlCommon objPageCmn = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        cmn = new Common(Session["DBConnection"].ToString());
        ObjCoustmer = new Set_CustomerMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objAreamaster = new Sys_AreaMaster(Session["DBConnection"].ToString());
        objCG = new Ems_Contact_Group(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        ObjLocation = new LocationMaster(Session["DBConnection"].ToString());
        objGroup = new Ems_GroupMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        ObjCompany = new CompanyMaster(Session["DBConnection"].ToString());
        objAcParameter = new Ac_ParameterMaster(Session["DBConnection"].ToString());
        objVoucherDetail = new Ac_Voucher_Detail(Session["DBConnection"].ToString());
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objVoucherHeader = new Ac_Voucher_Header(Session["DBConnection"].ToString());
        objcustomermaster = new Set_CustomerMaster(Session["DBConnection"].ToString());
        ObjCurrencyMaster = new CurrencyMaster(Session["DBConnection"].ToString());
        objCOA = new Ac_ChartOfAccount(Session["DBConnection"].ToString());
        objCustomerAgeingStatement = new CustomerAgeingEstatement(Session["DBConnection"].ToString());
        ObjAgeingDetail = new Ac_Ageing_Detail(Session["DBConnection"].ToString());
        objAcParamLocation = new Ac_Parameter_Location(Session["DBConnection"].ToString());
        objAcMaster = new Ac_AccountMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
           
            ViewState["grouplist"] = "";
            //navTree.Attributes.Add("onclick", "OnTreeClick(event)");
            btnList_Click(null, null);
            FillLocation();
            Calender_VoucherDate.Format = objSys.SetDateFormat();
            CalendarExtender1.Format = objSys.SetDateFormat();
            FillGroup();
            FillGrade();
            FillsalesEmployee();
            if (Request.QueryString["contactInfo"] != null)
            {
                fillGridData(Request.QueryString["contactInfo"].ToString());
            }
            else
            {
                fillGridData();
            }
            //BindTree();

            //navTree.Attributes.Add("onclick", "postBackByObject()");
            AllPageCode();
        }
        //isselectall = chkSelectAll.Checked;

        try
        {
            DataTable dt = Session["Grid_Data_CRM"] as DataTable;
            GvCustomerDetails.DataSource = dt;
            GvCustomerDetails.DataBind();
            //Chart1.DataSource = null;
            //Chart1.DataBind();
        }
        catch
        {

        }

        getAgeingStatement();
    }

    public void FillGrade()
    {
        DataTable dt = objDa.return_DataTable("select distinct Grade from Set_CustomerMaster where Brand_id=" + Session["BrandId"].ToString() + " and (   Grade is not null and Grade<>'' and Grade<>'Select')");
        ddlGrade.DataSource = dt;
        ddlGrade.DataTextField = "Grade";
        ddlGrade.DataValueField = "Grade";
        ddlGrade.DataBind();
        ddlGrade.Items.Insert(0, "--Select--");
    }


    public void FillsalesEmployee()
    {
        DataTable dt = objDa.return_DataTable("select distinct Set_EmployeeMaster.Emp_Name+'('+Set_EmployeeMaster.Emp_Code+')' as emp_name, Set_EmployeeMaster.emp_id from Set_CustomerMaster inner join Set_EmployeeMaster on Set_CustomerMaster.Handled_By_Emp =  Set_EmployeeMaster.emp_id  where Set_CustomerMaster.Brand_Id = " + Session["BrandId"].ToString() + " ");
        dt = new DataView(dt, "", "Emp_Name", DataViewRowState.CurrentRows).ToTable();

        ddlEmployee.DataSource = dt;
        ddlEmployee.DataTextField = "Emp_Name";
        ddlEmployee.DataValueField = "emp_id";
        ddlEmployee.DataBind();
        ddlEmployee.Items.Insert(0, "--Select--");
    }



    public void AllPageCode()
    {
        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("276", (DataTable)Session["ModuleName"]);
        if (dtModule.Rows.Count > 0)
        {
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
            strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        }
        else
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }
        //End Code

        Page.Title = objSys.GetSysTitle();

        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;


        if (Session["EmpId"].ToString() == "0")
        {



        }
        DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "276",Session["CompId"].ToString());
        if (dtAllPageCode.Rows.Count != 0)
        {
            if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
            {

            }
            else
            {
                foreach (DataRow DtRow in dtAllPageCode.Rows)
                {
                    if (DtRow["Op_Id"].ToString() == "1")
                    {

                    }

                    if (DtRow["Op_Id"].ToString() == "4")
                    {

                    }
                }
            }
        }
    }
    protected void btnList_Click(object sender, EventArgs e)
    {

        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = true;
        PnlNewEdit.Visible = false;

    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        PnlList.Visible = false;
        PnlNewEdit.Visible = true;
    }

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
        DataTable dtres = (DataTable)Session["MessageDt"];
        if (dtres.Rows.Count != 0)
        {
            try
            {
                ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
            }
            catch
            {
                ArebicMessage = EnglishMessage;
            }
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }


    protected void rbtnFilter_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bool IsSeelct = false;
        string strvalue = string.Empty;
        foreach (ListItem li in rbtnFilter.Items)
        {
            if (li.Selected)
            {
                IsSeelct = true;
                strvalue = li.Value;
                break;
            }
        }



        if (strvalue.Trim() != "CP" && strvalue.Trim() != "BD" && strvalue.Trim() != "AD")
        {
            trDate.Visible = true;
        }
        else
        {
            trDate.Visible = false;
        }


        if (strvalue.Trim() == "SQ")
        {
            trStatus.Visible = true;
        }
        else
        {
            trStatus.Visible = false;
        }

        if (strvalue.Trim() == "FS")
        {
            trOpeningBalance.Visible = true;
        }
        else
        {
            trOpeningBalance.Visible = false;
        }

        if (strvalue.Trim()=="FAS" || strvalue.Trim() == "FS")
        {
            trCurrency.Visible = true;
        }
        else
        {
            trCurrency.Visible = false;
        }

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

        if (!Common.GetStatus(Session["CompId"].ToString()))
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
    public bool IsObjectPermission(string ModuelId, string ObjectId)
    {
        bool Result = false;
        if (Session["EmpId"].ToString() == "0")
        {
            Result = true;
        }
        else
        {
            if (cmn.GetAllPagePermission(Session["UserId"].ToString(), ModuelId, ObjectId,Session["CompId"].ToString()).Rows.Count > 0)
            {
                Result = true;
            }
        }
        return Result;
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

    #region Auto Complete Method
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCustomer(string prefixText, int count, string contextKey)
    {
        Set_CustomerMaster objcustomer = new Set_CustomerMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dtCustomer = new DataTable();

        //if (grouplist != "")
        //{
        //    dtCustomer = objcustomer.GetCustomerDataByGroupIds(grouplist);
        //}
        //else
        //{
        dtCustomer = objcustomer.GetCustomerDataByGroupIds("");
        //}

        DataTable dtMain = new DataTable();
        dtMain = dtCustomer.Copy();

        string filtertext = "Name like '%" + prefixText + "%'";
        DataTable dtCon = new DataView(dtMain, filtertext, "Name Asc", DataViewRowState.CurrentRows).ToTable();

        string[] filterlist = new string[dtCon.Rows.Count];
        if (dtCon.Rows.Count > 0)
        {
            for (int i = 0; i < dtCon.Rows.Count; i++)
            {
                filterlist[i] = dtCon.Rows[i]["Name"].ToString() + "/" + dtCon.Rows[i]["Trans_Id"].ToString();
            }
        }
        return filterlist;
    }

    #endregion


    protected void btnGetReport_Click(object sender, EventArgs e)
    {
        objPageCmn.FillData((object)gvCustomerDetail, null, "", "");
        objPageCmn.FillData((object)GVCStatement, null, "", "");

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

        DateTime ToDate = new DateTime();

        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            ToDate = Convert.ToDateTime(txtToDate.Text);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
        }


        if (txtCustomerName.Text.Trim() == "")
        {
            DisplayMessage("Customer Name is blank");
            txtCustomerName.Focus();
            return;
        }
        else
        {
            try
            {
                txtCustomerName.Text.Split('/')[1].ToString();
            }
            catch
            {
                DisplayMessage("Select Customer in suggestion only");
                txtCustomerName.Focus();
                return;
            }

        }


        bool IsSeelct = false;
        string strvalue = string.Empty;
        foreach (ListItem li in rbtnFilter.Items)
        {
            if (li.Selected)
            {
                IsSeelct = true;
                strvalue = li.Value;
                break;
            }
        }

        if (!IsSeelct)
        {
            DisplayMessage("Select Filter");
            rbtnFilter.Focus();
            return;
        }


        //for Check Financial Year
        int fyear_id = 0;
        

        if (strvalue.Trim() == "FS" || strvalue.Trim()=="FAS")
        {

           

            if (ddlCurrency.SelectedValue=="0")
            {
                DisplayMessage("Please select valid currency");
                ddlCurrency.Focus();
                return;
            }

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

            fyear_id = Common.getFinancialYearId(Convert.ToDateTime(txtFromDate.Text), Session["DBConnection"].ToString(),Session["CompId"].ToString());

            if (fyear_id == 0)
            {
                GVCStatement.DataSource = null;
                GVCStatement.DataBind();
                DisplayMessage("Please enter valid date, This date(" + txtFromDate.Text + ") not belongs to any financial year");
                return;
            }

        }



        string strsql = string.Empty;

        DataTable dtFilter = new DataTable();

        if (strvalue.Trim() == "CP")
        {
            strsql = "select Ems_ContactMaster.Code ,Ems_ContactMaster.Name,Set_DesignationMaster.Designation,Ems_ContactMaster.Field2 as MobileNo,Ems_ContactMaster.Field1 as EmailId  from Ems_ContactMaster  left join Set_DesignationMaster on Ems_ContactMaster.Designation_Id=Set_DesignationMaster.Designation_Id  where Ems_ContactMaster.IsActive='True' and Ems_ContactMaster.Company_Id='" + txtCustomerName.Text.Split('/')[1].ToString() + "' and Ems_ContactMaster.Status='Individual' order by Ems_ContactMaster.Name";
        }
        else if (strvalue.Trim() == "AD")
        {
            strsql = "SELECT dbo.Set_AddressMaster.Address_Name,Set_AddressMaster.Address,Set_AddressMaster.PhoneNo1 as PhoneNo,Set_AddressMaster.MobileNo1,Set_AddressMaster.MobileNo2,Set_AddressMaster.EmailId1 as EmailId ,Set_AddressMaster.FaxNo,Set_AddressMaster.WebSite FROM dbo.Set_AddressMaster INNER JOIN dbo.Set_AddressChild ON dbo.Set_AddressMaster.Trans_Id = dbo.Set_AddressChild.Ref_Id where Set_AddressChild.Add_Type='Customer' and Set_AddressChild.Add_Ref_Id='" + txtCustomerName.Text.Split('/')[1].ToString() + "' and Set_AddressMaster.IsActive='True' order by dbo.Set_AddressMaster.Address_Name";

        }
        else if (strvalue.Trim() == "BD")
        {
            strsql = "select Set_BankMaster.Bank_Name,Contact_BankDetail.Account_No,Contact_BankDetail.Branch_Code,Contact_BankDetail.Branch_Address,Contact_BankDetail.IFSC_Code,Contact_BankDetail.MICR_Code,Contact_BankDetail.IBAN_NUMBER from Contact_BankDetail inner join Set_BankMaster on Contact_BankDetail.Bank_Id=Set_BankMaster.Bank_Id where Contact_BankDetail.Contact_Id='" + txtCustomerName.Text.Split('/')[1].ToString() + "' and Contact_BankDetail.Group_Id=1 order by Set_BankMaster.Bank_Name";

        }
        else if (strvalue.Trim() == "CD")
        {
            strsql = " select SM_Call_Logs.Trans_Id  as VoucherId,SM_Call_Logs.Call_No as Voucher_No,CONVERT(VARCHAR(11),SM_Call_Logs.Call_Date,106)  as VoucherDate,(select Ems_ContactMaster.Name from Ems_ContactMaster where Ems_ContactMaster.Trans_Id=SM_Call_Logs.Contact_Person) as Contact_Person,SM_Call_Logs.Contact_No,SM_Call_Logs.Email_Id,SM_Call_Logs.Call_Type, SM_Call_Logs.Call_Detail ,case when SM_Call_Logs.Status=' ' then 'Open' else SM_Call_Logs.Status end as Status  from SM_Call_Logs  where SM_Call_Logs.Customer_Name='" + txtCustomerName.Text.Split('/')[1].ToString() + "' order by SM_Call_Logs.Call_Date";

        }

        else if (strvalue.Trim() == "TD")
        {
            strsql = " select SM_Ticket_Master.Trans_Id as VoucherId,SM_Ticket_Master.Ticket_No as Voucher_No,CONVERT(VARCHAR(11),SM_Ticket_Master.Ticket_Date,106) as VoucherDate,SM_Ticket_Master.Ref_Type, case when SM_Ticket_Master.Ref_Type='Call' then (select SM_Call_Logs.Call_No from SM_Call_Logs where SM_Call_Logs.Trans_Id=SM_Ticket_Master.Ref_No) else ' ' end as RefferenceNo,SM_Ticket_Master.Schedule_Date,SM_Ticket_Master.Task_Type,SM_Ticket_Master.Status,SM_Ticket_Master.Description from SM_Ticket_Master where SM_Ticket_Master.Customer_Name='" + txtCustomerName.Text.Split('/')[1].ToString() + "' order by SM_Ticket_Master.Ticket_Date";

        }
        else if (strvalue.Trim() == "WO")
        {
            strsql = " select sm_workorder.Trans_Id as VoucherId,sm_workorder.Work_Order_No as Voucher_No,CONVERT(VARCHAR(11),sm_workorder.Work_Order_Date,106) as VoucherDate, sm_workorder.Ref_Type as ReferenceType, sm_workorder.ref_id as RefferenceNo, sm_workorder.Start_Time as StartTime, sm_workorder.End_Time as EndTime, sm_workorder.Remarks, sm_workorder.Status, sm_workorder.Engineer_Remarks as EngineerRemarks, set_employeemaster.Emp_Name as HandledEmpName from sm_workorder left join set_employeemaster on set_employeemaster.Emp_Id= sm_workorder.Handled_Emp_Id where sm_workorder.Customer_Id='" + txtCustomerName.Text.Split('/')[1].ToString() + "' order by sm_workorder.Work_Order_Date ";
        }



        else if (strvalue.Trim() == "SINQ")
        {
            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {

                strsql = " select Inv_SalesInquiryHeader.SInquiryID as Trans_Id,inv_salesinquiryheader.SInquiryNo as VoucherNo ,CONVERT(VARCHAR(11),inv_salesinquiryheader.IDate,106) as VoucherDate,inv_salesinquiryheader.Remark,(select CAST( cast(sum(Inv_SalesInqDetail.EstimatedUnitPrice*Inv_SalesInqDetail.Quantity) as numeric(18,3))as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code  from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=inv_salesinquiryheader.Currency_Id)   from Inv_SalesInqDetail  where Inv_SalesInqDetail.SInquiryID=Inv_SalesInquiryHeader.SInquiryID)  as Amount,Set_EmployeeMaster.Emp_Name as CreatedBy,  case when (select  top 1  Inv_SalesQuotationHeader.SQuotation_No from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID) IS not null then 'Quotation Created'   when (select top 1 Inv_SalesQuotationHeader.SQuotation_No from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID) IS null then 'Open'  end as Status , Set_LocationMaster.Location_Name from inv_salesinquiryheader left join Set_UserMaster on inv_salesinquiryheader.CreatedBy=Set_UserMaster.User_Id left join Set_EmployeeMaster on Set_UserMaster.Emp_Id=Set_EmployeeMaster.Emp_Id inner join Ems_ContactMaster on Inv_SalesInquiryHeader.Customer_Id=Ems_ContactMaster.Trans_Id   inner join Set_LocationMaster on Inv_SalesInquiryHeader.Location_Id=Set_LocationMaster.Location_Id  where Inv_SalesInquiryHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInquiryHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.IsActive='True' and Inv_SalesInquiryHeader.Customer_Id=" + txtCustomerName.Text.Split('/')[1].ToString() + " and Inv_SalesInquiryHeader.IDate>='" + txtFromDate.Text + "' and Inv_SalesInquiryHeader.IDate<='" + ToDate.ToString() + "' order by Inv_SalesInquiryHeader.IDate";
            }
            else
            {
                strsql = " select Inv_SalesInquiryHeader.SInquiryID as Trans_Id,inv_salesinquiryheader.SInquiryNo as VoucherNo ,CONVERT(VARCHAR(11),inv_salesinquiryheader.IDate,106) as VoucherDate,inv_salesinquiryheader.Remark,(select CAST( cast(sum(Inv_SalesInqDetail.EstimatedUnitPrice*Inv_SalesInqDetail.Quantity) as numeric(18,3))as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code  from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=inv_salesinquiryheader.Currency_Id)   from Inv_SalesInqDetail  where Inv_SalesInqDetail.SInquiryID=Inv_SalesInquiryHeader.SInquiryID)  as Amount,Set_EmployeeMaster.Emp_Name as CreatedBy,case when (select  top 1  Inv_SalesQuotationHeader.SQuotation_No from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID) IS not null then 'Quotation Created'   when (select top 1 Inv_SalesQuotationHeader.SQuotation_No from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID) IS null then 'Open'  end as Status, Set_LocationMaster.Location_Name from inv_salesinquiryheader left join Set_UserMaster on inv_salesinquiryheader.CreatedBy=Set_UserMaster.User_Id left join Set_EmployeeMaster on Set_UserMaster.Emp_Id=Set_EmployeeMaster.Emp_Id inner join Ems_ContactMaster on Inv_SalesInquiryHeader.Customer_Id=Ems_ContactMaster.Trans_Id   inner join Set_LocationMaster on Inv_SalesInquiryHeader.Location_Id=Set_LocationMaster.Location_Id  where Inv_SalesInquiryHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInquiryHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.IsActive='True' and Inv_SalesInquiryHeader.Customer_Id=" + txtCustomerName.Text.Split('/')[1].ToString() + " order by Inv_SalesInquiryHeader.IDate";
            }

        }


        else if (strvalue.Trim() == "SQ")
        {
            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {

                strsql = " select inv_salesquotationheader.SQuotation_Id as Trans_Id,Inv_SalesQuotationHeader.SQuotation_No as VoucherNo,CONVERT(VARCHAR(11),Inv_SalesQuotationHeader.Quotation_Date,106) as VoucherDate,Inv_SalesInquiryHeader.SInquiryNo,CAST(cast((inv_salesquotationheader.Amount+inv_salesquotationheader.TaxValue-inv_salesquotationheader.DiscountValue) as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=Inv_SalesQuotationHeader.Currency_Id) as Amount,Set_EmployeeMaster.Emp_Name as CreatedBy,Inv_SalesQuotationHeader.Status,Set_LocationMaster.Location_Name  from inv_salesquotationheader inner join Inv_SalesInquiryHeader on Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID inner join Ems_ContactMaster on Inv_SalesInquiryHeader.Customer_Id=Ems_ContactMaster.Trans_Id left join Set_UserMaster on Inv_SalesQuotationHeader.CreatedBy=Set_UserMaster.User_Id left join Set_EmployeeMaster on Set_UserMaster.Emp_Id=Set_EmployeeMaster.Emp_Id  inner join Set_LocationMaster on Inv_SalesQuotationHeader.Location_Id=Set_LocationMaster.Location_Id where  Inv_SalesQuotationHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesQuotationHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesInquiryHeader.Customer_Id=" + txtCustomerName.Text.Split('/')[1].ToString() + "  and inv_salesquotationheader.Quotation_Date>='" + txtFromDate.Text + "' and inv_salesquotationheader.Quotation_Date<='" + ToDate.ToString() + "' ";
            }
            else
            {
                strsql = "select inv_salesquotationheader.SQuotation_Id as Trans_Id,Inv_SalesQuotationHeader.SQuotation_No as VoucherNo,CONVERT(VARCHAR(11),Inv_SalesQuotationHeader.Quotation_Date,106) as VoucherDate,Inv_SalesInquiryHeader.SInquiryNo,CAST(cast((inv_salesquotationheader.Amount+inv_salesquotationheader.TaxValue-inv_salesquotationheader.DiscountValue) as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=Inv_SalesQuotationHeader.Currency_Id) as Amount,Set_EmployeeMaster.Emp_Name as CreatedBy,Inv_SalesQuotationHeader.Status,Set_LocationMaster.Location_Name  from inv_salesquotationheader inner join Inv_SalesInquiryHeader on Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID inner join Ems_ContactMaster on Inv_SalesInquiryHeader.Customer_Id=Ems_ContactMaster.Trans_Id left join Set_UserMaster on Inv_SalesQuotationHeader.CreatedBy=Set_UserMaster.User_Id left join Set_EmployeeMaster on Set_UserMaster.Emp_Id=Set_EmployeeMaster.Emp_Id  inner join Set_LocationMaster on Inv_SalesQuotationHeader.Location_Id=Set_LocationMaster.Location_Id where  Inv_SalesQuotationHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesQuotationHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesInquiryHeader.Customer_Id=" + txtCustomerName.Text.Split('/')[1].ToString() + "  ";
            }


            if (ddlPosted.SelectedIndex > 0)
            {
                strsql = strsql + "  and Inv_SalesQuotationHeader.Status='" + ddlPosted.SelectedValue.Trim() + "' order by inv_salesquotationheader.Quotation_Date ";

            }
            else
            {
                strsql = strsql + " order by inv_salesquotationheader.Quotation_Date";
            }

        }


        else if (strvalue.Trim() == "SO")
        {
            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {

                strsql = " SELECT  SH.[Trans_Id] ,SH.[SalesOrderNo]  as VoucherNo  , convert(varchar(11), SH.[SalesOrderDate] ,106) as Voucherdate       ,                        case when SH.SOfromTransType='Q' then 'Sales Quotation'                           when SH.SOfromTransType='D' then 'Direct' end as Transfer_Type,                                      case when SH.SOfromTransNo<>' ' and SH.SOfromTransNo<>0 then (select Inv_SalesQuotationHeader.SQuotation_No from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader.SQuotation_Id=SH.SOfromTransNo)                           end as Transfer_No                         ,SH.[Remark]                      ,cast(cast(SH.[NetAmount] as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=SH.Currency_Id) as       [NetAmount]                         ,Set_EmployeeMaster.Emp_Name as CreatedBy ,case       when  (select top 1 Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.SIFromTransType='S'	and Inv_SalesInvoiceDetail.SIFromTransNo =SH.Trans_Id) IS not null then 'Invoice Created'     when  (select top 1 Inv_SalesDeliveryVoucher_Header.Voucher_No from Inv_SalesDeliveryVoucher_Header where Inv_SalesDeliveryVoucher_Header.SalesOrder_Id =SH.Trans_Id) IS not null then 'Delivery Voucher Created'        when (select top 1 Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.SIFromTransType='S'	and Inv_SalesInvoiceDetail.SIFromTransNo =SH.Trans_Id) IS null then 'Open'               end as Status    ,Set_LocationMaster.Location_Name     FROM Inv_SalesOrderHeader as SH      left join Set_UserMaster on SH.CreatedBy=Set_UserMaster.User_Id left join Set_EmployeeMaster on Set_UserMaster.Emp_Id=Set_EmployeeMaster.Emp_Id inner join Set_LocationMaster on sh.Location_Id=Set_LocationMaster.Location_Id  Where SH.Company_Id = " + Session["CompId"].ToString() + " and SH.Brand_Id=" + Session["BrandId"].ToString() + " and SH.Location_Id in (" + strLocationId + ") and SH.IsActive='True' and sh.CustomerId=" + txtCustomerName.Text.Split('/')[1].ToString() + "    and SH.SalesOrderDate>='" + txtFromDate.Text + "' and SH.SalesOrderDate<='" + ToDate.ToString() + "'  order by SH.SalesOrderDate ";
            }
            else
            {
                strsql = " SELECT  SH.[Trans_Id] ,SH.[SalesOrderNo]  as VoucherNo  , convert(varchar(11), SH.[SalesOrderDate] ,106) as Voucherdate       ,                        case when SH.SOfromTransType='Q' then 'Sales Quotation'                           when SH.SOfromTransType='D' then 'Direct' end as Transfer_Type,                                      case when SH.SOfromTransNo<>' ' and SH.SOfromTransNo<>0 then (select Inv_SalesQuotationHeader.SQuotation_No from Inv_SalesQuotationHeader where Inv_SalesQuotationHeader.SQuotation_Id=SH.SOfromTransNo)                           end as Transfer_No                         ,SH.[Remark]                      ,cast(cast(SH.[NetAmount] as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=SH.Currency_Id) as       [NetAmount]                         ,Set_EmployeeMaster.Emp_Name as CreatedBy ,case       when  (select top 1 Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.SIFromTransType='S'	and Inv_SalesInvoiceDetail.SIFromTransNo =SH.Trans_Id) IS not null then 'Invoice Created'     when  (select top 1 Inv_SalesDeliveryVoucher_Header.Voucher_No from Inv_SalesDeliveryVoucher_Header where Inv_SalesDeliveryVoucher_Header.SalesOrder_Id =SH.Trans_Id) IS not null then 'Delivery Voucher Created'        when (select top 1 Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.SIFromTransType='S'	and Inv_SalesInvoiceDetail.SIFromTransNo =SH.Trans_Id) IS null then 'Open'               end as Status    ,Set_LocationMaster.Location_Name     FROM Inv_SalesOrderHeader as SH      left join Set_UserMaster on SH.CreatedBy=Set_UserMaster.User_Id left join Set_EmployeeMaster on Set_UserMaster.Emp_Id=Set_EmployeeMaster.Emp_Id inner join Set_LocationMaster on sh.Location_Id=Set_LocationMaster.Location_Id  Where SH.Company_Id = " + Session["CompId"].ToString() + " and SH.Brand_Id=" + Session["BrandId"].ToString() + " and SH.Location_Id in (" + strLocationId + ") and SH.IsActive='True' and sh.CustomerId=" + txtCustomerName.Text.Split('/')[1].ToString() + "     order by SH.SalesOrderDate ";
            }
        }


        else if (strvalue.Trim() == "SINV")
        {
            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {

                strsql = " select Inv_SalesInvoiceHeader.Trans_Id,Inv_SalesInvoiceHeader.Invoice_No as VoucherNo,convert(varchar(11),Inv_SalesInvoiceHeader.Invoice_Date,106) as Voucherdate,(SELECT STUFF((SELECT Distinct ',' + RTRIM(Inv_salesOrderHeader.SalesOrderNo)                    FROM Inv_salesOrderHeader where Inv_salesOrderHeader.Trans_id in (select Distinct Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.Invoice_No=Inv_SalesInvoiceHeader.Trans_Id) FOR XML PATH('')),1,1,'') ) as   SalesOrder_No,Set_EmployeeMaster.Emp_Name as SalesPerson,cast(cast(Inv_SalesInvoiceHeader.GrandTotal as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=Inv_SalesInvoiceHeader.Currency_Id) as InvoiceAmount     ,case when (select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Inv_SalesInvoiceHeader.CreatedBy)=0 then Inv_SalesInvoiceHeader.CreatedBy                else (select Set_EmployeeMaster.Emp_Name from Set_EmployeeMaster where Set_EmployeeMaster.Emp_Id=(select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Inv_SalesInvoiceHeader.CreatedBy)) end as CreatedBy    ,Set_LocationMaster.Location_Name      from Inv_SalesInvoiceHeader inner join Set_EmployeeMaster on Set_EmployeeMaster.emp_id=Inv_SalesInvoiceHeader.SalesPerson_Id inner join Ems_ContactMaster on Inv_SalesInvoiceHeader.Supplier_Id=Ems_ContactMaster.Trans_Id  inner join Set_LocationMaster on Inv_SalesInvoiceHeader.Location_Id=Set_LocationMaster.Location_Id  where Inv_SalesInvoiceHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInvoiceHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInvoiceHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInvoiceHeader.IsActive='True' and Inv_SalesInvoiceHeader.Supplier_Id=" + txtCustomerName.Text.Split('/')[1].ToString() + " and Inv_SalesInvoiceHeader.Invoice_Date>='" + txtFromDate.Text + "' and Inv_SalesInvoiceHeader.Invoice_Date<='" + ToDate.ToString() + "'  order by Inv_SalesInvoiceHeader.Invoice_Date ";
            }
            else
            {
                strsql = " select Inv_SalesInvoiceHeader.Trans_Id,Inv_SalesInvoiceHeader.Invoice_No as VoucherNo,convert(varchar(11),Inv_SalesInvoiceHeader.Invoice_Date,106) as Voucherdate,(SELECT STUFF((SELECT Distinct ',' + RTRIM(Inv_salesOrderHeader.SalesOrderNo)                    FROM Inv_salesOrderHeader where Inv_salesOrderHeader.Trans_id in (select Distinct Inv_SalesInvoiceDetail.SIFromTransNo from Inv_SalesInvoiceDetail where Inv_SalesInvoiceDetail.Invoice_No=Inv_SalesInvoiceHeader.Trans_Id) FOR XML PATH('')),1,1,'') ) as   SalesOrder_No,Set_EmployeeMaster.Emp_Name as SalesPerson,cast(cast(Inv_SalesInvoiceHeader.GrandTotal as numeric(18,3)) as nvarchar(max))+' '+(select Sys_CurrencyMaster.Currency_Code from Sys_CurrencyMaster where Sys_CurrencyMaster.Currency_ID=Inv_SalesInvoiceHeader.Currency_Id) as InvoiceAmount     ,case when (select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Inv_SalesInvoiceHeader.CreatedBy)=0 then Inv_SalesInvoiceHeader.CreatedBy                else (select Set_EmployeeMaster.Emp_Name from Set_EmployeeMaster where Set_EmployeeMaster.Emp_Id=(select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Inv_SalesInvoiceHeader.CreatedBy)) end as CreatedBy    ,Set_LocationMaster.Location_Name      from Inv_SalesInvoiceHeader inner join Set_EmployeeMaster on Set_EmployeeMaster.emp_id=Inv_SalesInvoiceHeader.SalesPerson_Id inner join Ems_ContactMaster on Inv_SalesInvoiceHeader.Supplier_Id=Ems_ContactMaster.Trans_Id  inner join Set_LocationMaster on Inv_SalesInvoiceHeader.Location_Id=Set_LocationMaster.Location_Id  where Inv_SalesInvoiceHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesInvoiceHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesInvoiceHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInvoiceHeader.IsActive='True' and Inv_SalesInvoiceHeader.Supplier_Id=" + txtCustomerName.Text.Split('/')[1].ToString() + "  order by Inv_SalesInvoiceHeader.Invoice_Date  ";
            }
        }



        else if (strvalue.Trim() == "SR")
        {
            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {

                strsql = " select Inv_SalesReturnHeader.Trans_Id,Inv_SalesReturnHeader.Return_No as VoucherNo,convert(varchar(11),Inv_SalesReturnHeader.Return_Date,106) as Voucherdate,Inv_SalesReturnHeader.Invoice_No,convert(varchar(11),Inv_SalesReturnHeader.Invoice_Date,106) as Invoice_Date,cast(Inv_SalesReturnHeader.GrandTotal as Numeric(18,3)) as GrandTotal,Inv_SalesReturnHeader.Remark,case when (select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Inv_SalesReturnHeader.CreatedBy)=0 then Inv_SalesReturnHeader.CreatedBy                else (select Set_EmployeeMaster.Emp_Name from Set_EmployeeMaster where Set_EmployeeMaster.Emp_Id=(select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Inv_SalesReturnHeader.CreatedBy)) end as CreatedBy    ,Set_LocationMaster.Location_Name  from Inv_SalesReturnHeader  inner join Set_LocationMaster on Inv_SalesReturnHeader.Location_Id=Set_LocationMaster.Location_Id where Inv_SalesReturnHeader.IsActive='True' and Inv_SalesReturnHeader.Location_Id in (" + strLocationId + ") and Inv_SalesReturnHeader.Customer_Id =" + txtCustomerName.Text.Split('/')[1].ToString() + " and  Inv_SalesReturnHeader.Return_Date>='" + txtFromDate.Text + "' and Inv_SalesReturnHeader.Return_Date<='" + ToDate.ToString() + "'  order by Inv_SalesReturnHeader.Return_Date ";
            }
            else
            {
                strsql = " select Inv_SalesReturnHeader.Trans_Id,Inv_SalesReturnHeader.Return_No as VoucherNo,convert(varchar(11),Inv_SalesReturnHeader.Return_Date,106) as Voucherdate,Inv_SalesReturnHeader.Invoice_No,convert(varchar(11),Inv_SalesReturnHeader.Invoice_Date,106) as Invoice_Date,cast(Inv_SalesReturnHeader.GrandTotal as Numeric(18,3)) as GrandTotal,Inv_SalesReturnHeader.Remark,case when (select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Inv_SalesReturnHeader.CreatedBy)=0 then Inv_SalesReturnHeader.CreatedBy                else (select Set_EmployeeMaster.Emp_Name from Set_EmployeeMaster where Set_EmployeeMaster.Emp_Id=(select Set_UserMaster.Emp_Id from Set_UserMaster where Set_UserMaster.User_Id=Inv_SalesReturnHeader.CreatedBy)) end as CreatedBy    ,Set_LocationMaster.Location_Name  from Inv_SalesReturnHeader  inner join Set_LocationMaster on Inv_SalesReturnHeader.Location_Id=Set_LocationMaster.Location_Id where Inv_SalesReturnHeader.IsActive='True' and Inv_SalesReturnHeader.Location_Id in (" + strLocationId + ") and Inv_SalesReturnHeader.Customer_Id =" + txtCustomerName.Text.Split('/')[1].ToString() + "   order by Inv_SalesReturnHeader.Return_Date  ";
            }
        }
        else if (strvalue.Trim() == "FAS") {
            try
            {
                HDFcurrentCustomerID.Value = txtCustomerName.Text.Split('/')[1].ToString();
                HDFCurrencyID.Value = ddlCurrency.SelectedValue;
                ViewState["CustomerAddressForStatement"] = null;
                ViewState["EmailFooterForStatement"] = null;
                ViewState["DtRvInvoiceStatement"] = null;
                getAgeingStatement();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "fnOpenModalAgeing_Detail()", true);
            }
            catch (Exception ex)
            {

            }
        }
        else if (strvalue.Trim() == "FS")
        {
            txtOpeningBalance.Text = "0";
            txtClosingBalance.Text = "0";

            string strCurrencyId = string.Empty;
            string strCurrencyType = string.Empty;

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




            string strReceiveVoucherAcc = Ac_ParameterMaster.GetCustomerAccountNo(Session["CompId"].ToString(), Session["DBConnection"].ToString());
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


            string strSta = string.Empty;
            txtOpeningBalance.Text = "0";

            string strOtherAcId = "0";
            if (txtCustomerName.Text != "")
            {
                //For Opening Balance Start
                if (txtFromDate.Text != "")
                {
                    DateTime dtFromdate = Convert.ToDateTime(txtFromDate.Text);
                    if (dtFromdate.Day == 1 && dtFromdate.Month == 1)
                    {
                        dtFromdate = new DateTime(dtFromdate.Year - 1, 12, 31, 23, 59, 1);
                    }
                    else if (dtFromdate.Day != 1 && dtFromdate.Month == 1)
                    {
                        dtFromdate = new DateTime(dtFromdate.Year, dtFromdate.Month, dtFromdate.Day - 1, 23, 59, 1);
                    }
                    else if (dtFromdate.Day != 1 && dtFromdate.Month != 1)
                    {
                        dtFromdate = new DateTime(dtFromdate.Year, dtFromdate.Month, dtFromdate.Day - 1, 23, 59, 1);
                    }
                    else if (dtFromdate.Day == 1 && dtFromdate.Month != 1)
                    {
                        int daysInMonth = DateTime.DaysInMonth(dtFromdate.Year, dtFromdate.Month - 1);
                        dtFromdate = new DateTime(dtFromdate.Year, dtFromdate.Month - 1, daysInMonth, 23, 59, 1);
                    }

                    strOtherAcId = objAcMaster.GetCustomerAccountByCurrency(txtCustomerName.Text.Split('/')[1].ToString(), ddlCurrency.SelectedValue.ToString()).ToString();
                    //Get Opening Balance
                    double OpeningBalance = 0;
                    PegasusDataAccess.DataAccessClass objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
                    DataTable dtOpeningBalance = objDA.return_DataTable("select (dbo.Ac_GetBalance('" + Session["CompId"].ToString() + "', '" + Session["BrandId"].ToString() + "','" + strLocationId + "', '" + dtFromdate + "', '0','" + strReceiveVoucherAcc + "','" + strOtherAcId + "','" + strCurrencyType + "','" + fyear_id.ToString() + "')) OpeningBalance");
                    if (dtOpeningBalance.Rows.Count > 0)
                    {
                        OpeningBalance = Convert.ToDouble(dtOpeningBalance.Rows[0][0].ToString());
                        if (OpeningBalance < 0)
                        {
                            strSta = "Credit";
                        }
                        else if (OpeningBalance > 0)
                        {
                            strSta = "Debit";
                        }
                        else
                        {
                            OpeningBalance = 0;
                        }
                    }

                    if (OpeningBalance != 0)
                    {
                        string DOB = OpeningBalance.ToString();
                        txtOpeningBalance.Text = objSys.GetCurencyConversionForInv(ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), DOB);
                    }
                    else
                    {
                        string DOB = OpeningBalance.ToString();
                        txtOpeningBalance.Text = objSys.GetCurencyConversionForInv(ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), DOB);
                    }
                }
            }



            dtFilter = objVoucherDetail.GetAllStatementData(Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, strReceiveVoucherAcc, strOtherAcId, txtFromDate.Text, txtToDate.Text, strCurrencyType);


            objPageCmn.FillData((object)GVCStatement, dtFilter, "", "");

            string strStatus = "False";
            string strBalanceA = string.Empty;
            string strBalanceF = string.Empty;
            double debitamount = 0;
            double creditamount = 0;
            double debitTotalamount = 0;
            double creditTotalamount = 0;

            foreach (GridViewRow gvr in GVCStatement.Rows)
            {
                HiddenField hdngvDetailId = (HiddenField)gvr.FindControl("hdnDetailId");
                Label lblgvDebitAmount = (Label)gvr.FindControl("lblgvDebitAmount");
                Label lblgvCreditAmount = (Label)gvr.FindControl("lblgvCreditAmount");
                lblgvDebitAmount.Text = objSys.GetCurencyConversionForInv(ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), lblgvDebitAmount.Text);
                lblgvCreditAmount.Text = objSys.GetCurencyConversionForInv(ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), lblgvCreditAmount.Text);
                debitamount = Convert.ToDouble(lblgvDebitAmount.Text);
                creditamount = Convert.ToDouble(lblgvCreditAmount.Text);
                Label lblgvBalance = (Label)gvr.FindControl("lblgvBalance");
                //Label lblgvFregnAmount = (Label)gvr.FindControl("lblgvForeignAmount");
                //Label lblgvFregnBalance = (Label)gvr.FindControl("lblgvFVBalance");




                if (strStatus == "False")
                {
                    //string strCredit = "-";
                    if (strSta == "Credit")
                    {
                        lblgvBalance.Text = txtOpeningBalance.Text;
                        lblgvBalance.Text = objSys.GetCurencyConversionForInv(ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), lblgvBalance.Text);
                        strStatus = "True";
                    }
                    else if (strSta == "Debit")
                    {
                        //lblgvBalance.Text = strCredit + "" + txtOpeningBalance.Text;
                        lblgvBalance.Text = txtOpeningBalance.Text;
                        lblgvBalance.Text = objSys.GetCurencyConversionForInv(ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), lblgvBalance.Text);
                        strStatus = "True";
                    }
                    else
                    {
                        lblgvBalance.Text = "0";
                        lblgvBalance.Text = objSys.GetCurencyConversionForInv(ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), lblgvBalance.Text);
                        strStatus = "True";
                    }
                }
                else
                {
                    lblgvBalance.Text = strBalanceA;
                }

                if (debitamount != 0)
                {
                    lblgvBalance.Text = (Convert.ToDouble(lblgvBalance.Text) + Convert.ToDouble(debitamount.ToString())).ToString();
                    strBalanceA = lblgvBalance.Text;
                    lblgvBalance.Text = objSys.GetCurencyConversionForInv(ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), lblgvBalance.Text);
                }
                else if (creditamount != 0)
                {
                    lblgvBalance.Text = (Convert.ToDouble(lblgvBalance.Text) - Convert.ToDouble(creditamount.ToString())).ToString();
                    strBalanceA = lblgvBalance.Text;
                    lblgvBalance.Text = objSys.GetCurencyConversionForInv(ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), lblgvBalance.Text);
                }

                debitTotalamount += debitamount;
                creditTotalamount += creditamount;
                txtClosingBalance.Text = strBalanceA;
                txtClosingBalance.Text = objSys.GetCurencyConversionForInv(ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), txtClosingBalance.Text);
            }


            if (GVCStatement.Rows.Count > 0)
            {
                Label lblgvDebitTotal = (Label)GVCStatement.FooterRow.FindControl("lblgvDebitTotal");
                Label lblgvCreditTotal = (Label)GVCStatement.FooterRow.FindControl("lblgvCreditTotal");
                Label lblgvBalanceTotal = (Label)GVCStatement.FooterRow.FindControl("lblgvBalanceTotal");

                lblgvDebitTotal.Text = debitTotalamount.ToString();
                lblgvDebitTotal.Text = objSys.GetCurencyConversionForInv(ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), lblgvDebitTotal.Text);
                lblgvCreditTotal.Text = creditTotalamount.ToString();
                lblgvCreditTotal.Text = objSys.GetCurencyConversionForInv(ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), lblgvCreditTotal.Text);
                lblgvBalanceTotal.Text = strBalanceA;
                lblgvBalanceTotal.Text = objSys.GetCurencyConversionForInv(ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), lblgvBalanceTotal.Text);

                GVCStatement.HeaderRow.Cells[7].Text = SystemParameter.GetCurrencySmbol(strCurrencyId, GVCStatement.HeaderRow.Cells[7].Text, Session["DBConnection"].ToString());
            }



        }
        else if (strvalue.Trim() == "PH")
        {
            objPageCmn.FillData((object)gvCustomerDetail, GetProductHistory(strLocationId), "", "");
        }

        if (strsql.Trim() != "")
        {

            dtFilter = objDa.return_DataTable(strsql);
            objPageCmn.FillData((object)gvCustomerDetail, dtFilter, "", "");
        }

    }

    protected string GetDate(string strDate)
    {
        string strNewDate = string.Empty;
        if (strDate != "")
        {
            strNewDate = DateTime.Parse(strDate).ToString(objSys.SetDateFormat());
        }
        else
        {
            strNewDate = "";
        }
        return strNewDate;
    }

    protected string GetEmployeeNameByEmpCode(string strEmployeeCode)
    {
        string strEmpName = string.Empty;
        if (strEmployeeCode != "0" && strEmployeeCode != "")
        {
            if (strEmployeeCode == "superadmin")
            {
                strEmpName = "Admin";
            }
            else
            {
                DataTable dtEmp = objEmployee.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), strEmployeeCode);
                if (dtEmp.Rows.Count > 0)
                {
                    strEmpName = dtEmp.Rows[0]["Emp_Name"].ToString();
                }
            }
        }
        else
        {
            strEmpName = "";
        }
        return strEmpName;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

        txtCustomerName.Text = "";
        foreach (ListItem li in rbtnFilter.Items)
        {
            li.Selected = false;
        }
        lstLocationSelect.Items.Clear();
        trDate.Visible = false;
        txtCustomerName.Focus();
        txtFromDate.Text = "";
        txtToDate.Text = "";
        objPageCmn.FillData((object)gvCustomerDetail, null, "", "");
        trStatus.Visible = false;
        trOpeningBalance.Visible = false;
        txtOpeningBalance.Text = "0";
        txtClosingBalance.Text = "0";

    }

    protected void lblgvVoucherNo_Click(object sender, EventArgs e)
    {
        string strVoucherType = string.Empty;
        LinkButton myButton = (LinkButton)sender;

        string[] arguments = myButton.CommandArgument.ToString().Split(new char[] { ',' });
        string RefId = arguments[0];
        string RefType = arguments[1];
        string Trans_Id = arguments[2].Trim();
        string LocationId = arguments[3].Trim();

        DataTable dtVoucherHeader = objVoucherHeader.GetVoucherHeader();
        if (dtVoucherHeader.Rows.Count > 0)
        {
            dtVoucherHeader = new DataView(dtVoucherHeader, "Trans_Id='" + Trans_Id + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtVoucherHeader.Rows.Count > 0)
            {
                strVoucherType = dtVoucherHeader.Rows[0]["Voucher_Type"].ToString();
            }
        }

        if (RefId == "0" && RefId != "")
        {
            if (IsObjectPermission("160", "184"))
            {
                string strCmd = string.Format("window.open('../VoucherEntries/VoucherDetail.aspx?Id=" + Trans_Id + "&LocId=" + LocationId + "','window','width=1024, ');");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
            }
            else
            {
                DisplayMessage("You have no permission for view detail");
                return;
            }
        }
        else if (RefId != "0" && RefId != "")
        {

            if (RefType == "SINV")
            {
                if (IsObjectPermission("144", "92"))
                {
                    string strCmd = string.Format("window.open('../Sales/SalesInvoice.aspx?Id=" + RefId + "&LocId=" + LocationId + "','window','width=1024, ');");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
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
        AllPageCode();

    }





    public DataTable GetProductHistory(string strLocationId)
    {
        DataTable DtFilter = new DataTable();


        DtFilter.Columns.Add("ProductCode");
        DtFilter.Columns.Add("ProductName");
        DtFilter.Columns.Add("UnitName");
        DtFilter.Columns.Add("Inquiry_Qty", typeof(double));
        DtFilter.Columns.Add("Inquiry_Amount", typeof(double));
        DtFilter.Columns.Add("Quotation_Qty", typeof(double));
        DtFilter.Columns.Add("Quotation_Amount", typeof(double));
        DtFilter.Columns.Add("Order_Qty", typeof(double));
        DtFilter.Columns.Add("Order_Amount", typeof(double));
        DtFilter.Columns.Add("Invoice_Qty", typeof(double));
        DtFilter.Columns.Add("Invoice_Amount", typeof(double));
        DtFilter.Columns.Add("Return_Qty", typeof(double));
        DtFilter.Columns.Add("Return_Amount", typeof(double));


        DateTime ToDate = new DateTime();

        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            ToDate = Convert.ToDateTime(txtToDate.Text);
            ToDate = new DateTime(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 1);
        }

        string strsql = string.Empty;




        if (txtFromDate.Text != "" && txtToDate.Text != "")
        {
            strsql = "select Inv_ProductMaster.ProductId,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName as ProductName ,Inv_UnitMaster.Unit_Name  , cast(  SUM( Inv_SalesInvoiceDetail.Quantity) as Numeric(18,3)) as SalesQuantity, cast( sum(((Inv_SalesInvoiceDetail.UnitPrice-Inv_SalesInvoiceDetail.DiscountV+Inv_SalesInvoiceDetail.TaxP)*Inv_SalesInvoiceDetail.Quantity)) as Numeric(18,3)) as SalesAmount    from Inv_SalesInvoiceHeader inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id=Inv_SalesInvoiceDetail.Invoice_No  inner join Inv_ProductMaster on Inv_SalesInvoiceDetail.Product_Id=Inv_ProductMaster.ProductId inner join Inv_UnitMaster on Inv_ProductMaster.UnitId=Inv_UnitMaster.Unit_Id where Inv_SalesInvoiceHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInvoiceHeader.Supplier_Id=" + txtCustomerName.Text.Split('/')[1].ToString() + " and Inv_SalesInvoiceHeader.IsActive='True' and Inv_SalesInvoiceHeader.Invoice_Date>='" + txtFromDate.Text + "' and Inv_SalesInvoiceHeader.Invoice_Date<='" + ToDate.ToString() + "'  group by Inv_ProductMaster.ProductId,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName,Inv_UnitMaster.Unit_Name   order by  cast(  SUM( Inv_SalesInvoiceDetail.Quantity) as Numeric(18,3)) desc";

        }
        else
        {
            strsql = " select Inv_ProductMaster.ProductId,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName as ProductName ,Inv_UnitMaster.Unit_Name  ,  cast(  SUM( Inv_SalesInvoiceDetail.Quantity) as Numeric(18,3)) as SalesQuantity, cast( sum(((Inv_SalesInvoiceDetail.UnitPrice-Inv_SalesInvoiceDetail.DiscountV+Inv_SalesInvoiceDetail.TaxP)*Inv_SalesInvoiceDetail.Quantity)) as Numeric(18,3)) as SalesAmount    from Inv_SalesInvoiceHeader inner join Inv_SalesInvoiceDetail on Inv_SalesInvoiceHeader.Trans_Id=Inv_SalesInvoiceDetail.Invoice_No  inner join Inv_ProductMaster on Inv_SalesInvoiceDetail.Product_Id=Inv_ProductMaster.ProductId inner join Inv_UnitMaster on Inv_ProductMaster.UnitId=Inv_UnitMaster.Unit_Id where Inv_SalesInvoiceHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInvoiceHeader.Supplier_Id=" + txtCustomerName.Text.Split('/')[1].ToString() + " and Inv_SalesInvoiceHeader.IsActive='True'  group by Inv_ProductMaster.ProductId,Inv_ProductMaster.ProductCode,Inv_ProductMaster.EProductName,Inv_UnitMaster.Unit_Name   order by  cast(SUM( Inv_SalesInvoiceDetail.Quantity) as Numeric(18,3)) desc  ";
        }

        DataTable dt = objDa.return_DataTable(strsql);



        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow dr = DtFilter.NewRow();

            dr["ProductCode"] = dt.Rows[i]["ProductCode"].ToString();
            dr["ProductName"] = dt.Rows[i]["ProductName"].ToString();
            dr["UnitName"] = dt.Rows[i]["Unit_Name"].ToString();


            //get all sales inquiry

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {

                strsql = "select  isnull( sum (Inv_SalesInqDetail.Quantity),0) from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID  where Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.Customer_Id=" + txtCustomerName.Text.Split('/')[1].ToString() + " and Inv_SalesInquiryHeader.IDate>='" + txtFromDate.Text + "' and Inv_SalesInquiryHeader.IDate<='" + ToDate.ToString() + "' and Inv_SalesInquiryHeader.IsActive='True' and Inv_SalesInqDetail.Product_Id=" + dt.Rows[i]["ProductId"].ToString() + " ";

            }
            else
            {
                strsql = "select isnull( sum (Inv_SalesInqDetail.Quantity),0) from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID  where Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.Customer_Id=" + txtCustomerName.Text.Split('/')[1].ToString() + "  and Inv_SalesInquiryHeader.IsActive='True'  and Inv_SalesInqDetail.Product_Id=" + dt.Rows[i]["ProductId"].ToString() + "  ";

            }


            dr["Inquiry_Qty"] = SetDecimal(objDa.return_DataTable(strsql).Rows[0][0].ToString());



            //inquiry amount



            //get all sales inquiry

            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {

                strsql = "select  isnull( sum( (Inv_SalesInqDetail.Quantity*Inv_SalesInqDetail.EstimatedUnitPrice)),0)  from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID  where Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.Customer_Id=" + txtCustomerName.Text.Split('/')[1].ToString() + " and Inv_SalesInquiryHeader.IDate>='" + txtFromDate.Text + "' and Inv_SalesInquiryHeader.IDate<='" + ToDate.ToString() + "' and Inv_SalesInquiryHeader.IsActive='True'     and Inv_SalesInqDetail.Product_Id=" + dt.Rows[i]["ProductId"].ToString() + " ";

            }
            else
            {
                strsql = "select isnull( sum( (Inv_SalesInqDetail.Quantity*Inv_SalesInqDetail.EstimatedUnitPrice)),0) from Inv_SalesInquiryHeader inner join Inv_SalesInqDetail on Inv_SalesInquiryHeader.SInquiryID=Inv_SalesInqDetail.SInquiryID  where Inv_SalesInquiryHeader.Location_Id in (" + strLocationId + ") and Inv_SalesInquiryHeader.Customer_Id=" + txtCustomerName.Text.Split('/')[1].ToString() + "  and Inv_SalesInquiryHeader.IsActive='True'  and Inv_SalesInqDetail.Product_Id=" + dt.Rows[i]["ProductId"].ToString() + "  ";

            }


            dr["Inquiry_Amount"] = SetDecimal(objDa.return_DataTable(strsql).Rows[0][0].ToString());



            //get all sales quotation 


            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {


                strsql = " select isnull( sum( Inv_SalesQuotationDetail.Quantity),0)  from inv_salesquotationheader inner join Inv_SalesInquiryHeader on Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID    inner join Inv_SalesQuotationDetail on Inv_SalesQuotationHeader.SQuotation_Id=Inv_SalesQuotationDetail.SQuotation_Id    where  Inv_SalesQuotationHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesQuotationHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesInquiryHeader.Customer_Id=" + txtCustomerName.Text.Split('/')[1].ToString() + "  and inv_salesquotationheader.Quotation_Date>='" + txtFromDate.Text + "' and inv_salesquotationheader.Quotation_Date<='" + ToDate.ToString() + "' and Inv_SalesQuotationDetail.Product_Id=" + dt.Rows[i]["ProductId"].ToString() + " ";
            }
            else
            {
                strsql = "select  isnull( sum( Inv_SalesQuotationDetail.Quantity),0)  from inv_salesquotationheader inner join Inv_SalesInquiryHeader on Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID   inner join Inv_SalesQuotationDetail on Inv_SalesQuotationHeader.SQuotation_Id=Inv_SalesQuotationDetail.SQuotation_Id    where  Inv_SalesQuotationHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesQuotationHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesInquiryHeader.Customer_Id=" + txtCustomerName.Text.Split('/')[1].ToString() + "  and Inv_SalesQuotationDetail.Product_Id=" + dt.Rows[i]["ProductId"].ToString() + "   ";
            }



            dr["Quotation_Qty"] = SetDecimal(objDa.return_DataTable(strsql).Rows[0][0].ToString());




            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {


                strsql = " select  isnull( sum( (Inv_SalesQuotationDetail.UnitPrice-Inv_SalesQuotationDetail.DiscountValue+Inv_SalesQuotationDetail.TaxValue)*Inv_SalesQuotationDetail.Quantity),0)  from inv_salesquotationheader   inner join Inv_SalesQuotationDetail on Inv_SalesQuotationHeader.SQuotation_Id=Inv_SalesQuotationDetail.SQuotation_Id        inner join Inv_SalesInquiryHeader on Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID  where  Inv_SalesQuotationHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesQuotationHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesInquiryHeader.Customer_Id=" + txtCustomerName.Text.Split('/')[1].ToString() + "  and inv_salesquotationheader.Quotation_Date>='" + txtFromDate.Text + "' and inv_salesquotationheader.Quotation_Date<='" + ToDate.ToString() + "'  and Inv_SalesQuotationDetail.Product_Id=" + dt.Rows[i]["ProductId"].ToString() + "  ";
            }
            else
            {
                strsql = "select  isnull( sum( (Inv_SalesQuotationDetail.UnitPrice-Inv_SalesQuotationDetail.DiscountValue+Inv_SalesQuotationDetail.TaxValue)*Inv_SalesQuotationDetail.Quantity),0)  from inv_salesquotationheader  inner join Inv_SalesQuotationDetail on Inv_SalesQuotationHeader.SQuotation_Id=Inv_SalesQuotationDetail.SQuotation_Id       inner join Inv_SalesInquiryHeader on Inv_SalesQuotationHeader.SInquiry_No=Inv_SalesInquiryHeader.SInquiryID  where  Inv_SalesQuotationHeader.Company_Id=" + Session["CompId"].ToString() + " and Inv_SalesQuotationHeader.Brand_Id=" + Session["BrandId"].ToString() + " and Inv_SalesQuotationHeader.Location_Id in (" + strLocationId + ") and Inv_SalesQuotationHeader.IsActive='True' and Inv_SalesInquiryHeader.Customer_Id=" + txtCustomerName.Text.Split('/')[1].ToString() + "   and Inv_SalesQuotationDetail.Product_Id=" + dt.Rows[i]["ProductId"].ToString() + "   ";
            }

            dr["Quotation_Amount"] = SetDecimal(objDa.return_DataTable(strsql).Rows[0][0].ToString());



            //sales order detail 


            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {

                strsql = " SELECT  isnull( sum(Inv_SalesOrderDetail.Quantity),0)    FROM Inv_SalesOrderHeader as SH    inner join Inv_SalesOrderDetail on SH.Trans_Id=Inv_SalesOrderDetail.SalesOrderNo      Where SH.Company_Id = " + Session["CompId"].ToString() + " and SH.Brand_Id=" + Session["BrandId"].ToString() + " and SH.Location_Id in (" + strLocationId + ") and SH.IsActive='True' and sh.CustomerId=" + txtCustomerName.Text.Split('/')[1].ToString() + "    and SH.SalesOrderDate>='" + txtFromDate.Text + "' and SH.SalesOrderDate<='" + ToDate.ToString() + "' and Inv_SalesOrderDetail.Product_Id=" + dt.Rows[i]["ProductId"].ToString() + " ";
            }
            else
            {
                strsql = " SELECT  isnull( sum(Inv_SalesOrderDetail.Quantity),0)   FROM Inv_SalesOrderHeader as SH  inner join Inv_SalesOrderDetail on SH.Trans_Id=Inv_SalesOrderDetail.SalesOrderNo       Where SH.Company_Id = " + Session["CompId"].ToString() + " and SH.Brand_Id=" + Session["BrandId"].ToString() + " and SH.Location_Id in (" + strLocationId + ") and SH.IsActive='True' and sh.CustomerId=" + txtCustomerName.Text.Split('/')[1].ToString() + " and Inv_SalesOrderDetail.Product_Id=" + dt.Rows[i]["ProductId"].ToString() + "    ";
            }




            dr["Order_Qty"] = SetDecimal(objDa.return_DataTable(strsql).Rows[0][0].ToString());



            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {


                strsql = " SELECT  isnull( sum( (Inv_SalesOrderDetail.UnitPrice-Inv_SalesOrderDetail.DiscountV+Inv_SalesOrderDetail.TaxV)*Inv_SalesOrderDetail.Quantity),0)    FROM Inv_SalesOrderHeader as SH    inner join Inv_SalesOrderDetail on SH.Trans_Id=Inv_SalesOrderDetail.SalesOrderNo        Where SH.Company_Id = " + Session["CompId"].ToString() + " and SH.Brand_Id=" + Session["BrandId"].ToString() + " and SH.Location_Id in (" + strLocationId + ") and SH.IsActive='True' and sh.CustomerId=" + txtCustomerName.Text.Split('/')[1].ToString() + "    and SH.SalesOrderDate>='" + txtFromDate.Text + "' and SH.SalesOrderDate<='" + ToDate.ToString() + "'  and Inv_SalesOrderDetail.Product_Id=" + dt.Rows[i]["ProductId"].ToString() + "";
            }
            else
            {
                strsql = " SELECT  isnull( sum( (Inv_SalesOrderDetail.UnitPrice-Inv_SalesOrderDetail.DiscountV+Inv_SalesOrderDetail.TaxV)*Inv_SalesOrderDetail.Quantity),0)   FROM Inv_SalesOrderHeader as SH   inner join Inv_SalesOrderDetail on SH.Trans_Id=Inv_SalesOrderDetail.SalesOrderNo      Where SH.Company_Id = " + Session["CompId"].ToString() + " and SH.Brand_Id=" + Session["BrandId"].ToString() + " and SH.Location_Id in (" + strLocationId + ") and SH.IsActive='True' and sh.CustomerId=" + txtCustomerName.Text.Split('/')[1].ToString() + " and Inv_SalesOrderDetail.Product_Id=" + dt.Rows[i]["ProductId"].ToString() + "     ";
            }


            dr["Order_Amount"] = SetDecimal(objDa.return_DataTable(strsql).Rows[0][0].ToString());



            //sales invoice

            dr["Invoice_Qty"] = SetDecimal(dt.Rows[i]["SalesQuantity"].ToString());
            dr["Invoice_Amount"] = SetDecimal(dt.Rows[i]["SalesAmount"].ToString());



            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {

                strsql = " select isnull( sum(Inv_SalesReturnDetail.ReturnQty),0) from Inv_SalesReturnHeader  inner join Inv_SalesReturnDetail on Inv_SalesReturnHeader.Trans_Id=Inv_SalesReturnDetail.Return_No   where Inv_SalesReturnHeader.IsActive='True' and Inv_SalesReturnHeader.Location_Id in (" + strLocationId + ") and Inv_SalesReturnHeader.Customer_Id =" + txtCustomerName.Text.Split('/')[1].ToString() + " and  Inv_SalesReturnHeader.Return_Date>='" + txtFromDate.Text + "' and Inv_SalesReturnHeader.Return_Date<='" + ToDate.ToString() + "' and Inv_SalesReturnDetail.Product_Id=" + dt.Rows[i]["ProductId"].ToString() + "  ";
            }
            else
            {
                strsql = " select isnull( sum(Inv_SalesReturnDetail.ReturnQty),0)  from Inv_SalesReturnHeader inner join Inv_SalesReturnDetail on Inv_SalesReturnHeader.Trans_Id=Inv_SalesReturnDetail.Return_No  where Inv_SalesReturnHeader.IsActive='True' and Inv_SalesReturnHeader.Location_Id in (" + strLocationId + ") and Inv_SalesReturnHeader.Customer_Id =" + txtCustomerName.Text.Split('/')[1].ToString() + "  and Inv_SalesReturnDetail.Product_Id=" + dt.Rows[i]["ProductId"].ToString() + " ";
            }

            dr["Return_Qty"] = SetDecimal(objDa.return_DataTable(strsql).Rows[0][0].ToString());


            if (txtFromDate.Text != "" && txtToDate.Text != "")
            {

                strsql = " select  isnull( sum( ((Inv_SalesReturnDetail.UnitPrice-Inv_SalesReturnDetail.DiscountV+Inv_SalesReturnDetail.TaxV)*Inv_SalesReturnDetail.ReturnQty)),0) from Inv_SalesReturnHeader inner join Inv_SalesReturnDetail on Inv_SalesReturnHeader.Trans_Id=Inv_SalesReturnDetail.Return_No  where Inv_SalesReturnHeader.IsActive='True' and Inv_SalesReturnHeader.Location_Id in (" + strLocationId + ") and Inv_SalesReturnHeader.Customer_Id =" + txtCustomerName.Text.Split('/')[1].ToString() + " and  Inv_SalesReturnHeader.Return_Date>='" + txtFromDate.Text + "' and Inv_SalesReturnHeader.Return_Date<='" + ToDate.ToString() + "' and Inv_SalesReturnDetail.Product_Id=" + dt.Rows[i]["ProductId"].ToString() + "  ";
            }
            else
            {
                strsql = " select isnull( sum( ((Inv_SalesReturnDetail.UnitPrice-Inv_SalesReturnDetail.DiscountV+Inv_SalesReturnDetail.TaxV)*Inv_SalesReturnDetail.ReturnQty)),0)  from Inv_SalesReturnHeader inner join Inv_SalesReturnDetail on Inv_SalesReturnHeader.Trans_Id=Inv_SalesReturnDetail.Return_No  where Inv_SalesReturnHeader.IsActive='True' and Inv_SalesReturnHeader.Location_Id in (" + strLocationId + ") and Inv_SalesReturnHeader.Customer_Id =" + txtCustomerName.Text.Split('/')[1].ToString() + " and Inv_SalesReturnDetail.Product_Id=" + dt.Rows[i]["ProductId"].ToString() + "  ";
            }

            dr["Return_Amount"] = SetDecimal(objDa.return_DataTable(strsql).Rows[0][0].ToString());



            DtFilter.Rows.Add(dr);
        }

        return DtFilter;
    }

    public string SetDecimal(string amount)
    {

        return objSys.GetCurencyConversionForInv(ObjLocation.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field1"].ToString(), amount);
    }

    protected void fillDdlCurrencyByCustomer(string strCustomerId)
    {
        try
        {
            ddlCurrency.Items.Clear();
            using (DataTable _dt = objAcMaster.GetAc_AccountMasterByCustomerId(strCustomerId))
            {
                if (_dt.Rows.Count > 0)
                {
                    ddlCurrency.DataSource = _dt;
                    ddlCurrency.DataTextField = "Currency_Name";
                    ddlCurrency.DataValueField = "Currency_id";
                    ddlCurrency.DataBind();
                }
                ddlCurrency.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
        catch (Exception ex)
        {
            ddlCurrency.Items.Insert(0, new ListItem("Select", "0"));
        }

    }
    public void FillGroup()
    {
        DataTable DtGroup = objGroup.GetGroupMasterTrueAllData();
        DtGroup = new DataView(DtGroup, "", "Group_Name Asc", DataViewRowState.CurrentRows).ToTable();

        if (DtGroup.Rows.Count > 0)
        {
            ddlGroupSearch.DataSource = DtGroup;
            ddlGroupSearch.DataTextField = "Group_Name";
            ddlGroupSearch.DataValueField = "Group_Id";
            ddlGroupSearch.DataBind();

        }
        ddlGroupSearch.Items.Insert(0, new ListItem("Select", "0"));


    }

    protected void ddlGroupSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlGroupSearch.SelectedIndex == 0)
        {
            ViewState["grouplist"] = "";
        }
        else
        {
            ViewState["grouplist"] = ddlGroupSearch.SelectedValue;
        }
        if (Request.QueryString["contactInfo"] == null)
        {
            fillGridData();
        }
        else
        {
            fillGridData(Request.QueryString["contactInfo"].ToString());
        }
    }

    public void fillGridData(string CustomerID = "0")
    {
        if (ddlGroupSearch.SelectedIndex == 0)
        {
            ViewState["grouplist"] = "";
        }
        else
        {
            ViewState["grouplist"] = ddlGroupSearch.SelectedValue;
        }

        DataTable dt_customerData = objcustomermaster.GetCustomerDataForCRMGrid(Session["CompId"].ToString(), Session["BrandId"].ToString(), ViewState["grouplist"].ToString(), CustomerID);

        if (ddlGrade.SelectedIndex > 0)
        {
            dt_customerData = new DataView(dt_customerData, "grade='" + ddlGrade.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        if (ddlEmployee.SelectedIndex > 0)
        {
            dt_customerData = new DataView(dt_customerData, "Handled_By_Emp='" + ddlEmployee.SelectedValue.Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        GvCustomerDetails.DataSource = dt_customerData;
        GvCustomerDetails.DataBind();
        Session["Grid_Data_CRM"] = dt_customerData;

        if (ddlGroupSearch.SelectedValue == "" || ddlGroupSearch.SelectedValue == "" || ddlGroupSearch.SelectedValue == "" || ddlGroupSearch.SelectedValue == "" || ddlGroupSearch.SelectedValue == "")
        {

        }
    }

    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        btnNext.Enabled = false;
        btnPrev.Enabled = true;
        div_next.Attributes["style"] = "display:block";
        div_prev.Attributes["style"] = "display:none";

        txtCustomerName.Text = e.CommandName.ToString() + "/" + e.CommandArgument.ToString();
        fillDdlCurrencyByCustomer(e.CommandArgument.ToString());
    }


    protected void btnPrev_Click(object sender, EventArgs e)
    {
        btnNext.Enabled = true;
        btnPrev.Enabled = false;
        div_next.Attributes["style"] = "display:none";
        div_prev.Attributes["style"] = "display:block";
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        btnNext.Enabled = false;
        btnPrev.Enabled = true;
        div_next.Attributes["style"] = "display:block";
        div_prev.Attributes["style"] = "display:none";
    }

    protected void lbtnMore_Command(object sender, CommandEventArgs e)
    {
        string data = "";
        DataTable dt_ContactNodata = new ContactNoMaster(Session["DBConnection"].ToString()).getDataByPKID(e.CommandArgument.ToString(), "Ems_ContactMaster");

        if (dt_ContactNodata.Rows.Count > 0)
        {
            foreach (DataRow dr in dt_ContactNodata.Rows)
            {
                data = data + "<b>" + dr["Type"].ToString() + "</b>:" + dr["Phone_no"].ToString() + " <br/>";
            }
        }

        if (data.Trim() != "")
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "Modal_Number_Open('" + data + "');", true);
        }

    }

    protected void lbtnTotalSales_Command(object sender, CommandEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "Modal_chart_Open();", true);
    }

    protected void lnkEditDetail_Command(object sender, CommandEventArgs e)
    {
        Session["ContactID"] = "";
        Session["ContactID"] = e.CommandArgument.ToString();
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "Modal_crm_agreement_Open();", true);
        agreement.fillcommonThings(ddlGroupSearch.SelectedItem.ToString(), ddlGroupSearch.SelectedValue.ToString(), e.CommandName.ToString(), e.CommandArgument.ToString());
    }

    // used for its child page
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetContactListCustomer(string prefixText, int count, string contextKey)
    {
        try
        {
            Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
            string id = string.Empty;

            if (HttpContext.Current.Session["ContactID"] != null)
            {
                id = HttpContext.Current.Session["ContactID"].ToString();
            }
            else
            {
                id = "0";
            }

            DataTable dtCon = ObjContactMaster.GetAllContactAsPerFilterText(prefixText, id);

            string[] filterlist = new string[dtCon.Rows.Count];
            if (dtCon.Rows.Count > 0)
            {
                for (int i = 0; i < dtCon.Rows.Count; i++)
                {
                    filterlist[i] = dtCon.Rows[i]["filtertext"].ToString();
                }
                return filterlist;
            }
            else
            {
                DataTable dtcon = ObjContactMaster.GetContactTrueById(id);
                string[] filterlistcon = new string[dtcon.Rows.Count];
                for (int i = 0; i < dtcon.Rows.Count; i++)
                {
                    filterlistcon[i] = dtcon.Rows[i]["Name"].ToString() + "/" + dtcon.Rows[i]["Trans_Id"].ToString();
                }
                return filterlistcon;
            }
        }
        catch (Exception error)
        {

        }
        return null;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmailMaster(string prefixText, int count, string contextKey)
    {
        ES_EmailMaster_Header Email = new ES_EmailMaster_Header(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = Email.GetDistinctEmailId(prefixText);

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = dt.Rows[i]["Email_Id"].ToString();
        }
        return str;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static int save_email(string emailAddress, string countryId, string contactId)
    {
        ES_EmailMaster_Header objEmailHeader = new ES_EmailMaster_Header(HttpContext.Current.Session["DBConnection"].ToString());
        ES_EmailMasterDetail objEmailDetail = new ES_EmailMasterDetail(HttpContext.Current.Session["DBConnection"].ToString());

        int emailHeaderID = objEmailHeader.ES_EmailMasterHeader_Insert(emailAddress, countryId, "", "", "", "", "False", DateTime.Now.ToString(), "True", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString());
        objEmailDetail.ES_EmailMasterDetail_Insert(contactId, "Contact", emailHeaderID.ToString(), "true", "", "", "", "", "", "False", DateTime.Now.ToString(), "True", HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString());
        return 1;
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static int Check_email(string emailAddress)
    {
        ES_EmailMaster_Header objEmailHeader = new ES_EmailMaster_Header(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt_email = objEmailHeader.GetDistinctEmailId(emailAddress);
        if (dt_email.Rows.Count > 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListStateName(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["AddCtrl_Country_Id"].ToString() == "")
        {
            return null;
        }
        StateMaster objStateMaster = new StateMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objStateMaster.GetAllStateByPrefixText(prefixText, HttpContext.Current.Session["AddCtrl_Country_Id"].ToString());
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["State_Name"].ToString();
        }
        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCityName(string prefixText, int count, string contextKey)
    {
        if (HttpContext.Current.Session["AddCtrl_State_Id"].ToString() == "" || HttpContext.Current.Session["AddCtrl_State_Id"].ToString() == "0")
        {
            return null;
        }
        CityMaster objCityMaster = new CityMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = objCityMaster.GetAllCityByPrefixText(prefixText, HttpContext.Current.Session["AddCtrl_State_Id"].ToString());
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["City_Name"].ToString();
        }
        return txt;
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static void ddlCountry_IndexChanged(string CountryId)
    {
        HttpContext.Current.Session["AddCtrl_Country_Id"] = CountryId;
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string txtState_TextChanged(string CountryId, string StateName)
    {
        StateMaster ObjStatemaster = new StateMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt_stateData = ObjStatemaster.GetAllStateByPrefixText(StateName, CountryId);
        if (dt_stateData.Rows.Count > 0)
        {
            HttpContext.Current.Session["AddCtrl_State_Id"] = dt_stateData.Rows[0]["Trans_Id"].ToString();
            return dt_stateData.Rows[0]["Trans_Id"].ToString();
        }
        else
        {
            HttpContext.Current.Session["AddCtrl_State_Id"] = "0";
            return "0";
        }
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string txtCity_TextChanged(string stateId, string cityName)
    {
        CityMaster ObjCityMaster = new CityMaster(HttpContext.Current.Session["DBConnection"].ToString());

        DataTable dt_CityData = ObjCityMaster.GetAllCityByPrefixText(cityName, stateId);

        if (dt_CityData.Rows.Count > 0)
        {
            return dt_CityData.Rows[0]["Trans_Id"].ToString();
        }
        else
        {
            return "0";
        }
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string ContactName_textchange(string contactName)
    {
        Ems_ContactMaster ObjContactMaster = new Ems_ContactMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string id = string.Empty;
        if (HttpContext.Current.Session["ContactID"] != null)
        {
            id = HttpContext.Current.Session["ContactID"].ToString();
        }
        else
        {
            id = "0";
        }
        DataTable dtCon = ObjContactMaster.GetAllContactAsPerFilterText(contactName, id);

        if (dtCon.Rows.Count > 0)
        {
            return dtCon.Rows[0]["Filtertext"].ToString();
        }
        else
        {
            return "0";
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductCode(string prefixText, int count, string contextKey)
    {
        try
        {
            if (HttpContext.Current.Session["CRM_Agreement_ProductCategoryId"] == null)
            {
                HttpContext.Current.Session["CRM_Agreement_ProductCategoryId"] = "0";
            }
            Inv_Product_Category PC = new Inv_Product_Category(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = PC.GetProductcodeByCategoryId_PreText(HttpContext.Current.Session["CRM_Agreement_ProductCategoryId"].ToString(), prefixText);
            string[] txt = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["ProductCode"].ToString();
            }
            return txt;
        }
        catch (Exception err)
        {
            return null;
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListProductName(string prefixText, int count, string contextKey)
    {
        try
        {
            Inv_ProductMaster PM = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dt = PM.GetDistinctProductName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), prefixText, HttpContext.Current.Session["FinanceYearId"].ToString(),HttpContext.Current.Session["LocId"].ToString());
            string[] txt = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                txt[i] = dt.Rows[i]["EProductName"].ToString();
            }
            return txt;
        }
        catch
        {
            return null;
        }
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static void CategoryChange(string categoryId)
    {
        HttpContext.Current.Session["CRM_Agreement_ProductCategoryId"] = categoryId;
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string productcode_change(string productCode)
    {
        Inv_ProductMaster ipm = new Inv_ProductMaster(HttpContext.Current.Session["DBConnection"].ToString());
        string productId = ipm.GetProductIdbyProductCode(productCode,HttpContext.Current.Session["BrandId"].ToString());

        if (productId != "")
        {
            string productName = ipm.GetProductNamebyProductId(productId);
            return productName;
        }

        return "";
    }

    protected void lnkFollowup_Command(object sender, CommandEventArgs e)
    {
        Session["CustBal_CustID"] = "0";
        Session["CustBal_CustID"] = e.CommandArgument.ToString();
        FollowupUC.newBtnCall();
        FollowupUC.GetFollowupDocumentNumber();
        FollowupUC.SetGeneratedByName();
        FollowupUC.ResetFollowupType();
        DataTable dt = ObjCoustmer.GetCustomerAllDataByCustomerId(Session["CompId"].ToString(), Session["BrandId"].ToString(), e.CommandArgument.ToString());
        FollowupUC.fillHeader(dt, hdnFollowupTableName.Value);
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Followup_Open();showNewTab();", true);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["contactInfo"] == null)
        {
            fillGridData();
        }
        else
        {
            fillGridData(Request.QueryString["contactInfo"].ToString());
        }
    }

    protected void btnSearchRefresh_Click(object sender, EventArgs e)
    {
        ddlGroupSearch.SelectedIndex = 0;
        ddlGrade.SelectedIndex = 0;
        ddlEmployee.SelectedIndex = 0;
        if (Request.QueryString["contactInfo"] == null)
        {
            fillGridData();
        }
        else
        {
            fillGridData(Request.QueryString["contactInfo"].ToString());
        }
    }

    protected void getAgeingStatement()
    {
        if (HDFcurrentCustomerID.Value == "0")
        {
            return;
        }

        string strLocationId = string.Empty;
        if (lstLocationSelect.Items.Count > 0)
        {
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
        }
        else
        {
            strLocationId = Session["LocId"].ToString();
        }
        string strCustomerId = HDFcurrentCustomerID.Value;
        string strOtherAcId = objAcMaster.GetCustomerAccountByCurrency(HDFcurrentCustomerID.Value.ToString(), HDFCurrencyID.Value.ToString()).ToString();
        DataTable dt = new DataTable();
        if (ViewState["DtRvInvoiceStatement"] == null)
        {
            AccountsDataset ObjAccountDataset = new AccountsDataset();
            ObjAccountDataset.EnforceConstraints = false;
            AccountsDatasetTableAdapters.Sp_Ac_InvoiceAgeingStatementTableAdapter adp = new AccountsDatasetTableAdapters.Sp_Ac_InvoiceAgeingStatementTableAdapter();
            adp.Connection.ConnectionString = Session["DBConnection"].ToString();
            adp.Fill(ObjAccountDataset.Sp_Ac_InvoiceAgeingStatement, Session["CompId"].ToString(), Session["BrandId"].ToString(), strLocationId, "RV", strOtherAcId, (Convert.ToDateTime(txtToDate.Text)).Date);
            ViewState["DtRvInvoiceStatement"] = ObjAccountDataset.Sp_Ac_InvoiceAgeingStatement;
        }

        dt = (DataTable)ViewState["DtRvInvoiceStatement"];

        int CurrencydecimalCount = 0;
        int.TryParse(ObjCurrencyMaster.GetCurrencyMasterById(HDFCurrencyID.Value).Rows[0]["decimal_count"].ToString(), out CurrencydecimalCount);

        //set ageing days detail in footer of the print
        Ac_Ageing_Detail.clsAgeingDaysDetail clsAging = new Ac_Ageing_Detail.clsAgeingDaysDetail();
        clsAging = ObjAgeingDetail.getAgingDayDetail(dt, CurrencydecimalCount == 0 ? 2 : CurrencydecimalCount);

        string strCompanyName = string.Empty;
        string strCompanyLogoUrl = Session["CompanyLogoUrl"].ToString();
        strCompanyName = Session["CompName"].ToString();


        objCustomerAgeingStatement.DataSource = dt;
        ViewState["DtRvInvoiceStatement"] = dt;
        objCustomerAgeingStatement.DataMember = "sp_Ac_InvoiceAgeingStatement";

        //ReportToolbar1.ReportViewer = ReportViewer1;
        objCustomerAgeingStatement.setcompanyname(strCompanyName);
        objCustomerAgeingStatement.SetImage(strCompanyLogoUrl);
        objCustomerAgeingStatement.setStatementDate(txtToDate.Text);
        string strEmailFooter = string.Empty;
        string strCustomerAddress = string.Empty;
        try
        {
            if (ViewState["CustomerAddressForStatement"] == null)
            {
                ViewState["CustomerAddressForStatement"] = objDa.get_SingleValue("SELECT Set_AddressMaster.Address FROM Set_AddressMaster WHERE Set_AddressMaster.Trans_Id = (SELECT TOP 1 Set_AddressChild.Ref_Id FROM Set_AddressChild inner join ac_accountMaster on ac_accountMaster.ref_id=Set_AddressChild.Add_Ref_Id WHERE (Set_AddressChild.Add_Type = 'Contact' or Set_AddressChild.Add_Type = 'Customer') AND ac_accountMaster.trans_id = " + HDFcurrentCustomerID.Value + ")");
            }
            if (ViewState["EmailFooterForStatement"] == null)
            {
                ViewState["EmailFooterForStatement"] = objAcParamLocation.GetParameterValue_By_ParameterName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), "Finance_Email_Statement_Footer").Rows[0]["Param_Value"].ToString();
            }
        }
        catch
        { }
        strCustomerAddress = ViewState["CustomerAddressForStatement"] == null ? "" : ViewState["CustomerAddressForStatement"].ToString();
        strEmailFooter = ViewState["EmailFooterForStatement"] == null ? "" : ViewState["EmailFooterForStatement"].ToString();

        strCustomerAddress = strCustomerAddress == "@NOTFOUND@" ? "" : strCustomerAddress;
        objCustomerAgeingStatement.setCustomerAddress(strCustomerAddress);
        objCustomerAgeingStatement.setFooterNote(strEmailFooter);
        objCustomerAgeingStatement.setAgeingDaysDetail(clsAging);
        objCustomerAgeingStatement.CreateDocument();
        ReportViewer1.OpenReport(objCustomerAgeingStatement);
    }


    protected void txtCustomerName_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtCustomerName.Text))
        {
            try
            {
                string strCustomerId = txtCustomerName.Text.Split('/')[1].ToString();
                fillDdlCurrencyByCustomer(strCustomerId);
            }
            catch
            {

            }
        }
    }
}