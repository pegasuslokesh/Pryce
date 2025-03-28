using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Reports_LoanReport : BasePage
{
    SystemParameter objSys = null;
    EmployeeLoanReport_Currency Objreport = null;
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    Pay_Employee_Loan ObjLoan = null;
    EmployeeMaster objEmpmaster = null;
    Set_Approval_Employee objEmpApproval = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        Objreport = new EmployeeLoanReport_Currency(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjLoan = new Pay_Employee_Loan(Session["DBConnection"].ToString());
        objEmpmaster = new EmployeeMaster(Session["DBConnection"].ToString());
        objEmpApproval = new Set_Approval_Employee(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "141", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
        }
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        Session["AccordianId"] = "109";
        Session["HeaderText"] = "HR Report";
        if (Session["Querystring"] == null)
        {
            //Response.Redirect("~/HR_Report/GeneratePayrollReport.aspx");
            string TARGET_URL = "../HR_Report/GeneratePayrollReport.aspx";
            if (Page.IsCallback)
                DevExpress.Web.ASPxWebControl.RedirectOnCallback(TARGET_URL);
            else
                Response.Redirect(TARGET_URL);
        }
        else
        {
            GetReport();
        }
        Page.Title = objSys.GetSysTitle();
    }
    protected void DocumentViewer1_CacheReportDocument(object sender, DevExpress.XtraReports.Web.CacheReportDocumentEventArgs e)
    {
        e.Key = Guid.NewGuid().ToString();
        Page.Session[e.Key] = e.SaveDocumentToMemoryStream();
    }

    protected void DocumentViewer1_RestoreReportDocumentFromCache(object sender, DevExpress.XtraReports.Web.RestoreReportDocumentFromCacheEventArgs e)
    {
        Stream stream = Page.Session[e.Key] as Stream;
        if (stream != null)
            e.RestoreDocumentFromStream(stream);
    }
    public string GetEmployeeCode(object empid)
    {
        string empname = string.Empty;

        DataTable dt = objEmpmaster.GetEmployeeMasterAllData(Session["CompId"].ToString());
        dt = new DataView(dt, "Emp_Id='" + empid.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {
            empname = dt.Rows[0]["Emp_Code"].ToString();
            if (empname == "")
            {
                empname = "No Code";
            }
        }
        else
        {
            empname = "No Code";
        }
        return empname;
    }
    void GetReport()
    {
        DataTable DtClaimRecord = new DataTable();
        DtClaimRecord = ObjLoan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved");
        string startadate = Session["Month"].ToString() + "/" + "1" + "/" + Session["Year"].ToString();
        int DaysInmonth = DateTime.DaysInMonth(Convert.ToInt32(Session["Year"].ToString()), Convert.ToInt32(Session["Month"].ToString().ToString()));
        DateTime ToDate = new DateTime();
        ToDate = new DateTime(Convert.ToInt32(Session["Year"].ToString()), Convert.ToInt32(Session["Month"].ToString()), DaysInmonth, 23, 59, 1);


        DtClaimRecord = new DataView(DtClaimRecord, "Loan_Approval_Date>='" + startadate + "' and Loan_Approval_Date <='" + ToDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        DataTable dt = new DataTable();
        dt.Columns.Add("EmpId");
        dt.Columns.Add("EmpName");
        dt.Columns.Add("Month");
        dt.Columns.Add("Loan_Name");
        dt.Columns.Add("Loan_Amount");
        dt.Columns.Add("Loan_Duration");
        dt.Columns.Add("Loan_Interest");
        dt.Columns.Add("Gross_Amount");
        dt.Columns.Add("Company_Name");
        dt.Columns.Add("Year");
        dt.Columns.Add("Monthly_Installmet");
        dt.Columns.Add("Loan_Approval_Date");
        dt.Columns.Add("Employee_Id");
        dt.Columns.Add("Currency_Symbol");
        dt.Columns.Add("ApprovedBy");



        string Id = (string)Session["Querystring"];

        foreach (string str in Id.Split(','))
        {
            if (str != "")
            {
                DataTable dtClaimRecord = DtClaimRecord;
                dtClaimRecord = new DataView(dtClaimRecord, "Emp_Id=" + str + "", "", DataViewRowState.CurrentRows).ToTable();
                if (dtClaimRecord.Rows.Count > 0)
                {
                   
                    string[] Montharr = new string[12];


                    for (int i = 0; i < dtClaimRecord.Rows.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        DataTable dtApproved = new DataTable();
                        dtApproved = objEmpApproval.GetApprovalChild("0","5");
                        try
                        {
                            dtApproved = new DataView(dtApproved, "Approval_Type='Loan' and Status='Approved' and Request_Emp_Id=" + str + " and Priority='True' and Ref_id=" + dtClaimRecord.Rows[i]["Loan_Id"].ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {

                        }

                        if (dtApproved.Rows.Count > 0)
                        {
                            DataTable dt_Emp_By_Approved = objEmpApproval.getApprovalTransByRef_IDandApprovalId(dtApproved.Rows[0]["Ref_Id"].ToString(), dtApproved.Rows[0]["Approval_Id"].ToString());
                            if (dt_Emp_By_Approved.Rows.Count > 0)
                                dr["ApprovedBy"] = dt_Emp_By_Approved.Rows[0]["Description"].ToString().Substring(dt_Emp_By_Approved.Rows[0]["Description"].ToString().IndexOf('-') + 1);
                            else
                                dr["ApprovedBy"] = "";
                        }
                        else
                        {
                            dr["ApprovedBy"] = "";
                        }
                        dr["Employee_Id"] = str;
                        dr[0] = GetEmployeeCode(dtClaimRecord.Rows[i]["Emp_Id"].ToString());
                        dr[1] = dtClaimRecord.Rows[i]["Emp_Name"].ToString();
                        dr[2] = Session["MonthName"].ToString();
                        dr[3] = dtClaimRecord.Rows[i]["Loan_Name"].ToString();
                        dr[4] = dtClaimRecord.Rows[i]["Loan_Amount"].ToString();
                        dr[5] = dtClaimRecord.Rows[i]["Loan_Duration"].ToString();
                        dr[6] = dtClaimRecord.Rows[i]["Loan_Interest"].ToString();
                        dr[7] = dtClaimRecord.Rows[i]["Gross_Amount"].ToString();
                        dr[8] = "Pegasus Limited";
                        dr[9] = (string)Session["Year"];
                        dr[10] = dtClaimRecord.Rows[i]["Monthly_Installment"].ToString();
                        dr[11] = dtClaimRecord.Rows[i]["Loan_Approval_Date"].ToString();
                        dr["Currency_Symbol"] = objSys.GetCurencySymbol(str, HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString());


                        dt.Rows.Add(dr);
                    }
                }



            }
        }




        //this code is modified on 03-04-2014 by jitendra upadhyay
        //for convert the amount in selected currency by employee and  company
        //for this work i create new function GetCurencyConversion in system parameter class where we pass the employeeid and amount for conversion 
        //this function return the amount after currency conversion

        //code start
        for (int i = 0; i < dt.Rows.Count; i++)
        {


            dt.Rows[i]["Loan_Amount"] = objSys.GetCurencyConversion(dt.Rows[i]["Employee_Id"].ToString(), dt.Rows[i]["Loan_Amount"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dt.Rows[i]["Gross_Amount"] = objSys.GetCurencyConversion(dt.Rows[i]["Employee_Id"].ToString(), dt.Rows[i]["Gross_Amount"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dt.Rows[i]["Monthly_Installmet"] = objSys.GetCurencyConversion(dt.Rows[i]["Employee_Id"].ToString(), dt.Rows[i]["Monthly_Installmet"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());


        }

        //code end


        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        string BrandName = string.Empty;
        string LocationName = string.Empty;

        DataTable DtCompany = Objcompany.GetCompanyMasterById(Session["CompId"].ToString());
        DataTable DtAddress = Objaddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());
        if (DtCompany.Rows.Count > 0)
        {
            if (Session["Lang"].ToString() == "1")
            {
                CompanyName = DtCompany.Rows[0]["Company_Name"].ToString();
            }
            else
            {
                CompanyName = DtCompany.Rows[0]["Company_Name_L"].ToString();
            }
            Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Logo_Path"].ToString();


        }
        if (DtAddress.Rows.Count > 0)
        {
            CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
        }
        DataTable DtBrand = ObjBrandMaster.GetBrandMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString());
        if (DtBrand.Rows.Count > 0)
        {
            if (Session["Lang"].ToString() == "1")
                BrandName = DtBrand.Rows[0]["Brand_Name"].ToString();
            else
                BrandName = DtBrand.Rows[0]["Brand_Name_L"].ToString();
        }

        DataTable DtLocation = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
        if (DtLocation.Rows.Count > 0)
        {
            if (Session["Lang"].ToString() == "1")
                LocationName = DtLocation.Rows[0]["Location_Name"].ToString();
            else
                LocationName = DtLocation.Rows[0]["Location_Name_L"].ToString();
        }
        Objreport.setBrandName(BrandName);
        Objreport.setLocationName(LocationName);
        Objreport.setDepartmentName(Session["DepartmentName"].ToString());
        Objreport.SetImage(Imageurl);

        Objreport.setTitleName(Resources.Attendance.Loan_Approval_Report);
        Objreport.SetHeaderName(Resources.Attendance.Created_By, Resources.Attendance.Employee_Name, Resources.Attendance.Employee_Code, Resources.Attendance.Loan_Name, Resources.Attendance.Loan_Amount, Resources.Attendance.Duration, Resources.Attendance.Interest, Resources.Attendance.Gross_Amount, Resources.Attendance.Installment, Resources.Attendance.Approved_By, Resources.Attendance.Approval_Date);
        Objreport.setcompanyname(CompanyName);
        Objreport.setaddress(CompanyAddress);
        Objreport.setUserName(Session["UserId"].ToString());
        Objreport.DataSource = dt;
        Objreport.DataMember = "EmpLoan";
        ReportViewer1.Report = Objreport;
        ReportToolbar1.ReportViewer = ReportViewer1;





    }

}
