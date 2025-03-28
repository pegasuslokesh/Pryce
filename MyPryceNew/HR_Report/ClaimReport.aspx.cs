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
using PegasusDataAccess;
using System.IO;

public partial class Reports_ClaimReport : BasePage
{
    EmployeeClaimreport_Currency Objreport = null;
    CompanyMaster Objcompany = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    Set_AddressChild Objaddress = null;
    Pay_Employee_claim objclaim = null;
    EmployeeParameter ObjSalary = null;
    SystemParameter objSys = null;
    Set_Approval_Employee objEmpApproval = null;
    Att_Employee_Notification objEmpNotice = null;
    string EmpId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Objreport = new EmployeeClaimreport_Currency(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objclaim = new Pay_Employee_claim(Session["DBConnection"].ToString());
        ObjSalary = new EmployeeParameter(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objEmpApproval = new Set_Approval_Employee(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "135", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
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
            ddlClaimType_OnSelectedIndexChanged(null, null);
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

    protected void ddlClaimType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string ClaimStatus = string.Empty;
        int ClaimType = Convert.ToInt32(ddlClaimType.SelectedValue);

        Session["ClaimType"] = ClaimType;
        if (ddlClaimType.SelectedValue == "1")
        {
            ClaimStatus = "Approved";
        }
        if (ddlClaimType.SelectedValue == "2")
        {
            ClaimStatus = "Cancelled";
        }
        if (ddlClaimType.SelectedValue == "3")
        {
            ClaimStatus = "Pending";
        }

        GetReport(ClaimStatus);

    }


    void GetReport(string ClaimStatus)
    {


        DataTable DtClaimRecord = new DataTable();

        DtClaimRecord = objclaim.GetRecord_From_PayEmployeeClaim(Session["CompId"].ToString(), "0", "0", Session["Month"].ToString(), Session["Year"].ToString(), ClaimStatus, "", "");




        Session["ClaimType"] = int.Parse(ddlClaimType.SelectedValue);



        DataTable dt = new DataTable();
        dt.Columns.Add("Employee_Id");
        dt.Columns.Add("Image");
        dt.Columns.Add("Month");
        dt.Columns.Add("Year");
        dt.Columns.Add("Emp_Id");
        dt.Columns.Add("Emp_Name");
        dt.Columns.Add("Claim_Name");
        dt.Columns.Add("Claim_Amount");
        dt.Columns.Add("Sum_Claim");
        dt.Columns.Add("Currency_Symbol");
        dt.Columns.Add("ApprovedBy");




        string Id = string.Empty;
        string EmpReport = string.Empty;


        //this code is created by jitendra upadhyay on 19-09-2014
        //this code for employee should not be showing in repot if not exists in employee notification for current report
        //code start
        EmpReport = Session["Querystring"].ToString();


        //code update on 27-09-2014

        DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(Session["Querystring"].ToString(), "23");


        foreach (DataRow dr in dtEmpNF.Rows)
        {
            Id += dr["Emp_Id"] + ",";
        }

        //code end

        //for (int i = 0; i < EmpReport.Split(',').Length - 1; i++)
        //{
        //    DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId(Session["CompId"].ToString(), EmpReport.Split(',')[i].ToString());

        //    if (dtEmpNF.Rows.Count > 0)
        //    {
        //        if (Convert.ToBoolean(dtEmpNF.Rows[0]["Is_Report4"].ToString()))
        //        {
        //            Id += EmpReport.Split(',')[i].ToString() + ",";
        //        }
        //    }
        //}

        //code end



        int ClaimType = (int)Session["ClaimType"];
        double SumClaim = 0;
        foreach (string str in Id.Split(','))
        {
            if (str != "")
            {
                DataTable dtClaimRecod = DtClaimRecord;
                dtClaimRecod = new DataView(dtClaimRecod, "Emp_Id=" + str + "", "", DataViewRowState.CurrentRows).ToTable();
                if (dtClaimRecod.Rows.Count > 0)
                {
                    DataTable dtsalary = ObjSalary.GetEmployeeParameterByEmpId(str, Session["CompId"].ToString());
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
                    string Claimamount = string.Empty;
                    for (int i = 0; i < dtClaimRecod.Rows.Count; i++)
                    {
                        string salary = string.Empty;
                        if (dtsalary.Rows.Count > 0)
                        {

                            salary = dtsalary.Rows[0]["Basic_Salary"].ToString();
                        }


                        Claimamount = Convert.ToDouble(dtClaimRecod.Rows[i]["Field2"].ToString()).ToString("0.000");


                       
                        //if (dtClaimRecod.Rows[i]["Value_Type"].ToString() == "2")
                        //{
                        //    string value = dtClaimRecod.Rows[i]["Value"].ToString();
                        //    Claimamount = (float.Parse(salary) * float.Parse(value) / 100).ToString("0.000");
                        //}
                        //else
                        //{
                        //    Claimamount = Convert.ToDouble(dtClaimRecod.Rows[i]["Value"]).ToString("0.000");
                        //}

                        int Month = Convert.ToInt32(dtClaimRecod.Rows[i]["Claim_Month"]);

                        DataRow dr = dt.NewRow();
                        dr[0] = str;
                        dr[1] = "";
                        dr[2] = Montharr[Month - 1].ToString();
                        dr[3] = dtClaimRecod.Rows[i]["Claim_Year"].ToString();
                        dr[4] = dtClaimRecod.Rows[i]["Emp_Code"].ToString();
                        dr[5] = dtClaimRecod.Rows[i]["Emp_Name"].ToString();
                        dr[6] = dtClaimRecod.Rows[i]["Claim_Name"].ToString();
                        //this code is modified on 02-04-2014 by jitendra upadhyay
                        //for convert the amount in selected currency by employee and  company
                        //for this work i create new function GetCurencyConversion in system parameter class where we pass the employeeid and amount for conversion 
                        //this function return the amount after currency conversion

                        //code start
                        dr[7] = objSys.GetCurencyConversion(str, Claimamount, HttpContext.Current.Session["LocCurrencyId"].ToString());

                        SumClaim += double.Parse(Claimamount);

                        dr[8] = objSys.GetCurencyConversion(str, SumClaim.ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());

                        dr["Currency_Symbol"] = objSys.GetCurencySymbol(str, HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                        if (dtClaimRecod.Rows[i]["Claim_Approved"].ToString() != "Pending")
                        {
                            string Approval = dtClaimRecod.Rows[i]["Approval_Description"].ToString();
                            Approval = Approval.Substring(Approval.LastIndexOf("-") + 1);
                            //dr["ApprovedBy"] = dtClaimRecod.Rows[i]["Approval_Description"].ToString().Substring(dtClaimRecod.Rows[i]["Approval_Description"].ToString().IndexOf('-') + 1);
                            dr["ApprovedBy"] = Approval;
                        }
                        else
                            dr["ApprovedBy"] = "";

                        DataTable dtApproved = new DataTable();
                        dtApproved = objEmpApproval.GetApprovalChild("0", "4");
                        try
                        {
                            dtApproved = new DataView(dtApproved, "Approval_Type='Claim'  and Request_Emp_Id=" + str + " and Priority='True' and Ref_id=" + DtClaimRecord.Rows[i]["Claim_Id"].ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        catch
                        {

                        }


                        if (dtApproved.Rows.Count > 0)
                        {
                            if (dtApproved.Rows[0]["Status"].ToString().Trim() != "Pending")
                            {
                                //string Description= dtApproved.Rows[0]["App_Description"].ToString();
                                //dr["ApprovedBy"] = Description.Substring(Description.LastIndexOf('-') + 1);
                            }
                        }
                        else
                        {
                            //dr["ApprovedBy"] = "";
                        }
                        //code end

                        //dr[7] = System.Math.Round(Claimamount, 3);
                        dt.Rows.Add(dr);
                    }
                }



            }
        }

        string CompanyName = "";
        string BrandName = string.Empty;
        string LocationName = string.Empty;
        string CompanyAddress = "";
        string Imageurl = "";

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
        DataTable DtLocation = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
        if (DtLocation.Rows.Count > 0)
        {
            if (Session["Lang"].ToString() == "1")
                LocationName = DtLocation.Rows[0]["Location_Name"].ToString();
            else
                LocationName = DtLocation.Rows[0]["Location_Name_L"].ToString();
        }
        DataTable DtBrand = ObjBrandMaster.GetBrandMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString());
        if (DtBrand.Rows.Count > 0)
        {
            if (Session["Lang"].ToString() == "1")
                BrandName = DtBrand.Rows[0]["Brand_Name"].ToString();
            else
                BrandName = DtBrand.Rows[0]["Brand_Name_L"].ToString();
        }
        Objreport.setDepartment(Session["DepartmentName"].ToString());
        Objreport.setBrandName(BrandName);
        Objreport.setLocation(LocationName);
        Objreport.SetImage(Imageurl);
        Objreport.SetCompanyName(CompanyName);
        Objreport.SetAddress(CompanyAddress);
        //Objreport.setSumClaimAmount(SumClaim);
        Objreport.setUserName(Session["UserId"].ToString());

        if (ClaimType == 1)
        {
            Objreport.setTitleName(Resources.Attendance.Claim_Approved_Report, Resources.Attendance.Approved_By);
        }
        if (ClaimType == 2)
        {
            Objreport.setTitleName(Resources.Attendance.Claim_Cancelled_Report, Resources.Attendance.Rejected_By);
        }
        if (ClaimType == 3)
        {
            Objreport.setTitleName(Resources.Attendance.Claim_Pending_Report, Resources.Attendance.Approved_By);
        }
        Objreport.SetHeaderName(Resources.Attendance.Created_By, Resources.Attendance.Employee_Name, Resources.Attendance.Employee_Code, Resources.Attendance.Claim_Name, Resources.Attendance.Claim_Amount, Resources.Attendance.Total);


        Objreport.DataSource = dt;
        Objreport.DataMember = "EmployeeClaim";
        ReportViewer1.Report = Objreport;
        ReportToolbar1.ReportViewer = ReportViewer1;

        //System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("ar-SA");
        //System.Threading.Thread.CurrentThread.CurrentCulture = ci;
        //System.Threading.Thread.CurrentThread.CurrentUICulture = ci;



    }



}
