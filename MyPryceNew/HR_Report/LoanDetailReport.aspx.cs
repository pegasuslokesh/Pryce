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

public partial class HR_Report_LoanDetailReport : BasePage
{
    SystemParameter objSys = null;
    Pay_Employee_Loan ObjLoan = null;
    EmployeeLoanDetailReport Objreport = null;
    EmployeeLoanInstallmentReport_Currency ObjReportInstallmet = null;
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    EmployeeMaster objEmpmaster = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    Att_Employee_Notification objEmpNotice = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        Session["AccordianId"] = "109";
        Session["HeaderText"] = "HR Report";
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        ObjLoan = new Pay_Employee_Loan(Session["DBConnection"].ToString());
        Objreport = new EmployeeLoanDetailReport(Session["DBConnection"].ToString());
        ObjReportInstallmet = new EmployeeLoanInstallmentReport_Currency(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objEmpmaster = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());
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
            getReport();

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
    void getReport()
    {
        Session["QuerystringLoan"] = Request.QueryString["LoanType"].ToString();
        if (!IsPostBack)
        {
            if (Session["QuerystringLoan"].ToString() == "1")
            {
                Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
                if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "142", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
                {
                    Session.Abandon();
                    Response.Redirect("~/ERPLogin.aspx");
                }
            }
            else
            {
                Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
                if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "143", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
                {
                    Session.Abandon();
                    Response.Redirect("~/ERPLogin.aspx");
                }
            }
        }



        string[] Montharr = new string[12];


        Montharr[0] = "January";
        Montharr[1] = "February";
        Montharr[2] = "March";
        Montharr[3] = "April";
        Montharr[4] = "May";
        Montharr[5] = "June";
        Montharr[6] = "July";
        Montharr[7] = "August";
        Montharr[8] = "September";
        Montharr[9] = "October";
        Montharr[10] = "November";
        Montharr[11] = "December";
        DataTable DtClaimRecord = new DataTable();



        string EmpReport = string.Empty;
        string EmpIds = string.Empty;

        //this code is created by jitendra upadhyay on 19-09-2014
        //this code for employee should not be showing in repot if not exists in employee notification for current report
        //code start

        //code update on 27-09-2014

        DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(Session["Querystring"].ToString(), "25");


        foreach (DataRow dr in dtEmpNF.Rows)
        {
            EmpIds += dr["Emp_Id"] + ",";
        }

        //code end

        //if (Session["QuerystringLoan"].ToString() == "1")
        //{
        //    EmpReport = Session["Querystring"].ToString();
        //    for (int i = 0; i < EmpReport.Split(',').Length - 1; i++)
        //    {
        //        DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId(Session["CompId"].ToString(), EmpReport.Split(',')[i].ToString());

        //        if (dtEmpNF.Rows.Count > 0)
        //        {
        //            if (Convert.ToBoolean(dtEmpNF.Rows[0]["Is_Report6"].ToString()))
        //            {
        //                EmpIds += EmpReport.Split(',')[i].ToString() + ",";
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    EmpIds = Session["Querystring"].ToString();

        //}

        //code end


        string[] Empid = EmpIds.Split(',');



      
        DtClaimRecord.Columns.Add("Empid");
        DtClaimRecord.Columns.Add("EmpName");
        DtClaimRecord.Columns.Add("Loan_Name");
        DtClaimRecord.Columns.Add("Installment");
        DtClaimRecord.Columns.Add("PaidAmount");
        DtClaimRecord.Columns.Add("Status");
        DtClaimRecord.Columns.Add("Month");
        DtClaimRecord.Columns.Add("Year");
        DtClaimRecord.Columns.Add("Loan_Id");
        DtClaimRecord.Columns.Add("Loan_Amount");
        DtClaimRecord.Columns.Add("Loan_Duration");
        DtClaimRecord.Columns.Add("Loan_Interest");
        DtClaimRecord.Columns.Add("Gross_Amount");
        DtClaimRecord.Columns.Add("Sum_PaidAmount");
        DtClaimRecord.Columns.Add("Employee_Id");
        DtClaimRecord.Columns.Add("Currency_Symbol");


        for (int i = 0; i < Empid.Length - 1; i++)
        {

            DataTable dtloanmaster = new DataTable();
            DataTable dtloandetail = new DataTable();
            dtloanmaster = ObjLoan.GetRecord_From_PayEmployeeLoanByStatus(Session["CompId"].ToString(), "Approved");


            dtloanmaster = new DataView(dtloanmaster, "Emp_id=" + Empid[i].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
            if (dtloanmaster.Rows.Count > 0)
            {
                for (int k = 0; k < dtloanmaster.Rows.Count; k++)
                {

                    string loanid = dtloanmaster.Rows[k]["Loan_id"].ToString();

                    dtloandetail = ObjLoan.GetRecord_From_PayEmployeeLoanDetailByLoanId(loanid);
                    if (dtloandetail.Rows.Count > 0)
                    {
                        if (Session["QuerystringLoan"].ToString() == "2")
                        {
                            dtloandetail = new DataView(dtloandetail, "Month=" + Session["Month"].ToString() + " and Year=" + Session["Year"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                        }

                        for (int j = 0; j < dtloandetail.Rows.Count; j++)
                        {
                            int month = Convert.ToInt32(dtloandetail.Rows[j]["Month"].ToString());
                            DataRow Tablerow = DtClaimRecord.NewRow();
                            Tablerow["Employee_Id"] = dtloanmaster.Rows[k]["Emp_Id"].ToString();
                            Tablerow[0] =GetEmployeeCode(dtloanmaster.Rows[k]["Emp_Id"].ToString());
                            Tablerow[1] = dtloanmaster.Rows[k]["Emp_Name"].ToString();
                            Tablerow[2] = dtloanmaster.Rows[k]["Loan_Name"].ToString();

                            double installment = 0;


                            try
                            {
                                installment = double.Parse(dtloandetail.Rows[j]["Montly_Installment"].ToString()) +  double.Parse(dtloandetail.Rows[j]["Previous_Balance"].ToString());
                            }
                            catch
                            {
                                installment = double.Parse(dtloandetail.Rows[j]["Montly_Installment"].ToString());

                            }

                             Tablerow[3]=installment.ToString();


                            Tablerow[4] = dtloandetail.Rows[j]["Employee_Paid"].ToString();
                            Tablerow[5] = dtloandetail.Rows[j]["Is_Status"].ToString();
                            Tablerow[6] = Montharr[month - 1].ToString();
                            Tablerow[7] = dtloandetail.Rows[j]["Year"].ToString();
                            Tablerow[8] = dtloanmaster.Rows[k]["Loan_Id"].ToString();
                            Tablerow[9] = dtloanmaster.Rows[k]["Loan_Amount"].ToString();
                            Tablerow[10] = dtloanmaster.Rows[k]["Loan_Duration"].ToString();
                            Tablerow[11] = dtloanmaster.Rows[k]["Loan_Interest"].ToString();
                            Tablerow[12] = dtloanmaster.Rows[k]["Gross_Amount"].ToString();
                            Tablerow["Currency_Symbol"] = objSys.GetCurencySymbol(dtloanmaster.Rows[k]["Emp_Id"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString());

                            DtClaimRecord.Rows.Add(Tablerow);
                        }
                    }
                }




            }



        }



       DataTable dtClaimRecord = DtClaimRecord;

      



        
        string LoanType = (string)Session["QuerystringLoan"];
        double sumPaidamount = 0;
        //dtClaimRecord.Columns.Add("Sum_PaidAmount");
        if (dtClaimRecord.Rows.Count > 0)
        {
            for (int i = 0; i < dtClaimRecord.Rows.Count; i++)
            {
                DataRow dr = dtClaimRecord.NewRow();

                sumPaidamount += Convert.ToDouble(dtClaimRecord.Rows[i]["PaidAmount"]);
                dr[13] = sumPaidamount;

            }
        }
       
        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        Objreport.setsum(sumPaidamount);
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
            Imageurl ="~/CompanyResource/"+Session["CompId"].ToString()+"/"+DtCompany.Rows[0]["Logo_Path"].ToString();


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

        //this code is modified on 03-04-2014 by jitendra upadhyay
        //for convert the amount in selected currency by employee and  company
        //for this work i create new function GetCurencyConversion in system parameter class where we pass the employeeid and amount for conversion 
        //this function return the amount after currency conversion

        //code start
        for (int i = 0; i < dtClaimRecord.Rows.Count; i++)
        {
            dtClaimRecord.Rows[i]["Installment"] = objSys.GetCurencyConversion(dtClaimRecord.Rows[i]["Employee_Id"].ToString(), dtClaimRecord.Rows[i]["Installment"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtClaimRecord.Rows[i]["PaidAmount"] = objSys.GetCurencyConversion(dtClaimRecord.Rows[i]["Employee_Id"].ToString(), dtClaimRecord.Rows[i]["PaidAmount"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtClaimRecord.Rows[i]["Loan_Amount"] = objSys.GetCurencyConversion(dtClaimRecord.Rows[i]["Employee_Id"].ToString(), dtClaimRecord.Rows[i]["Loan_Amount"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtClaimRecord.Rows[i]["Gross_Amount"] = objSys.GetCurencyConversion(dtClaimRecord.Rows[i]["Employee_Id"].ToString(), dtClaimRecord.Rows[i]["Gross_Amount"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtClaimRecord.Rows[i]["Sum_PaidAmount"] = objSys.GetCurencyConversion(dtClaimRecord.Rows[i]["Employee_Id"].ToString(), dtClaimRecord.Rows[i]["Sum_PaidAmount"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            

        }

        //code end





        if (LoanType == "1")
        {
            Objreport.setBrandName(BrandName);
            Objreport.setLocationName(LocationName);
            Objreport.setDepartmentName(Session["DepartmentName"].ToString());
           
            Objreport.SetImage(Imageurl);
            if (Session["lang"].ToString() == "1")
            {
                Objreport.setTitleName("Loan Detail Report");
            }
            else if (Session["lang"].ToString() == "2")
            {
                Objreport.setTitleName("تقرير قرض التفاصيل");
            }

            Objreport.setaddress(CompanyAddress);
            Objreport.setcompanyname(CompanyName);
            Objreport.setUserName(Session["UserId"].ToString());
            if (Session["lang"].ToString() == "1")
            {
                lblHeader.Text = "Loan Detail Report";
            }
            else if (Session["lang"].ToString() == "2")
            {
                lblHeader.Text = "تقرير قرض التفاصيل";
            }
            Objreport.SetHeader(Resources.Attendance.Created_By, Resources.Attendance.Month, Resources.Attendance.Year, Resources.Attendance.Employee_Code, Resources.Attendance.Employee_Name, Resources.Attendance.Loan_Id, Resources.Attendance.Loan_Name, Resources.Attendance.Loan_Amount, Resources.Attendance.Interest, Resources.Attendance.Duration, Resources.Attendance.Gross_Amount, Resources.Attendance.Monthly_Installment, Resources.Attendance.Paid_Amount, Resources.Attendance.Status, Resources.Attendance.Total_Amount);
            Objreport.DataSource = dtClaimRecord;
            Objreport.DataMember = "LoanDetail";
            ReportViewer1.Report = Objreport;
            ReportToolbar1.ReportViewer = ReportViewer1;
                        
        }
        else
        {
            ObjReportInstallmet.setBrandName(BrandName);
            ObjReportInstallmet.setLocationName(LocationName);
            ObjReportInstallmet.setDepartmentName(Session["DepartmentName"].ToString());
            ObjReportInstallmet.SetImage(Imageurl);

            if (Session["lang"].ToString() == "1")
            {
                ObjReportInstallmet.setTitleName("Monthly Installment Report");
            }
            else if (Session["lang"].ToString() == "2")
            {
                ObjReportInstallmet.setTitleName("تقرير القسط الشهري");
            }
            ObjReportInstallmet.setaddress(CompanyAddress);
            ObjReportInstallmet.setcompanyname(CompanyName);
            ObjReportInstallmet.setUserName(Session["UserId"].ToString());

            if (Session["lang"].ToString() == "1")
            {
                lblHeader.Text = "Monthly Installment Report";
            }
            else if (Session["lang"].ToString() == "2")
            {
                lblHeader.Text = "تقرير القسط الشهري";
            }
            ObjReportInstallmet.SetHeaderName(Resources.Attendance.Created_By, Resources.Attendance.Employee_Code, Resources.Attendance.Employee_Name, Resources.Attendance.Monthly_Installment, Resources.Attendance.Paid_Amount, Resources.Attendance.Status);

            ObjReportInstallmet.DataSource = dtClaimRecord;
            ObjReportInstallmet.DataMember = "LoanDetail";
            ReportViewer1.Report = ObjReportInstallmet;
            ReportToolbar1.ReportViewer = ReportViewer1;
                    }   
    }
}
