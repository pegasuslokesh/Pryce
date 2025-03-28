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

public partial class Temp_Report_Settlement_payment : BasePage
{

    SettlementReport objxxtrarepot = null;
    EmployeeMaster objEmp = null;
    Document_Master ObjDoc = null;
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    Pay_Employee_Due_Payment objEmpDuePay = null;
    SystemParameter objSys = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    Att_Employee_Notification objEmpNotice = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objxxtrarepot = new SettlementReport(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjDoc = new Document_Master(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objEmpDuePay = new Pay_Employee_Due_Payment(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "148", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
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
            getrecoerd();

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
    public string GetEmployeeName(object empid)
    {

        string empname = string.Empty;

        DataTable dt = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
        dt = new DataView(dt, "Emp_Id='" + empid.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {

            empname = dt.Rows[0]["Emp_Name"].ToString();

            if (empname == "")
            {
                empname = "No Name";
            }

        }
        else
        {
            empname = "No Name";

        }

        return empname;



    }
    public string GetEmployeeCode(object empid)
    {

        string empname = string.Empty;

        DataTable dt = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
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

    public void getrecoerd()
    {
        string[] month = new string[13];
        month[0] = "--Select--";
        month[1] = "JAN'";
        month[2] = "FEB'";
        month[3] = "MAR'";
        month[4] = "APR'";
        month[5] = "MAY'";
        month[6] = "JUN'";
        month[7] = "JUL'";
        month[8] = "AUG'";
        month[9] = "SEP'";
        month[10] = "OCT'";
        month[11] = "NOV'";
        month[12] = "DEC'";

        DataTable dtreport = new DataTable();
        DataTable dtpaydueemp = new DataTable();

        dtpaydueemp.Columns.Add("EmpId");
        dtpaydueemp.Columns.Add("Empname");

        dtpaydueemp.Columns.Add("Type");
        dtpaydueemp.Columns.Add("Amount");
        dtpaydueemp.Columns.Add("Description");
        dtpaydueemp.Columns.Add("Month");
        dtpaydueemp.Columns.Add("Year");
        dtpaydueemp.Columns.Add("Currency_Symbol");
        dtpaydueemp.Columns.Add("Employee_Id");

        string TransNo = string.Empty;


        string EmpReport = string.Empty;
        string EmpIds = string.Empty;

        //this code is created by jitendra upadhyay on 19-09-2014
        //this code for employee should not be showing in repot if not exists in employee notification for current report
        //code start


        EmpReport = Session["Querystring"].ToString();
        //code update on 27-09-2014

        DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(Session["Querystring"].ToString(), "26");


        foreach (DataRow dr in dtEmpNF.Rows)
        {
            EmpIds += dr["Emp_Id"] + ",";
        }
        //for (int i = 0; i < EmpReport.Split(',').Length - 1; i++)
        //{
        //    DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId(Session["CompId"].ToString(), EmpReport.Split(',')[i].ToString());

        //    if (dtEmpNF.Rows.Count > 0)
        //    {
        //        if (Convert.ToBoolean(dtEmpNF.Rows[0]["Is_Report7"].ToString()))
        //        {
        //            EmpIds += EmpReport.Split(',')[i].ToString() + ",";
        //        }
        //    }
        //}


        //code end


        foreach (string str in EmpIds.Split(','))
        {

            if ((str != ""))
            {
                string CurrencySymbol = string.Empty;
                string EmployeeName = GetEmployeeName(str);
                CurrencySymbol = objSys.GetCurencySymbol(str, HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString());

                dtreport = objEmpDuePay.GetAllRecord_ByEmpId(str, HttpContext.Current.Session["CompId"].ToString());
                if (Session["Month"] != null && Session["Year"] != null)
                {
                    try
                    {
                        dtreport = new DataView(dtreport, "Month=" + Session["Month"].ToString() + " and Year=" + Session["Year"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                }
                if (dtreport.Rows.Count > 0)
                {
                    for (int i = 0; i < dtreport.Rows.Count; i++)
                    {
                        DataRow dr = dtpaydueemp.NewRow();
                        dr["Employee_Id"] = dtreport.Rows[i]["Emp_Id"].ToString();
                        dr[0] =GetEmployeeCode(dtreport.Rows[i]["Emp_Id"].ToString());
                        dr[1] = EmployeeName;

                        if (dtreport.Rows[i]["Type"].ToString() == "1")
                        {
                            dr[2] = Resources.Attendance.Addition;

                        }
                        else if (dtreport.Rows[i]["Type"].ToString() == "2")
                        {
                            dr[2] = Resources.Attendance.Subtraction;
                        }

                        dr[3] =objSys.GetCurencyConversion(str,dtreport.Rows[i]["Amount"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                      
                       

                        //this code is created by jitendra upadhyay on 03-04-2014
                        //this code is created for convert the amount in selected currency after split the string
                        //code start
                        try
                        {
                            string NewString = string.Empty;
                            string[] Currency = dtreport.Rows[i]["Description"].ToString().Split('=');
                            if (dtreport.Rows[i]["Field1"].ToString() == "Allowance" || dtreport.Rows[i]["Field1"].ToString() == "Deduction")
                            {
                                for (int j = 0; j < Currency.Length; j++)
                                {
                                    if (j == 3)
                                    {
                                        string[] CUR = Currency[j].ToString().Split(',');
                                        NewString = NewString + '='+CurrencySymbol+objSys.GetCurencyConversion(str, CUR[0].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString()) + ',' + CUR[1].ToString();
                                    }
                                    if (j == 4)
                                    {

                                        NewString = NewString + '=' + CurrencySymbol+objSys.GetCurencyConversion(str, Currency[j].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                                    }
                                    if (j != 3 && j != 4)
                                    {
                                        if (NewString == "")
                                        {
                                            NewString = Currency[j].ToString();
                                        }
                                        else
                                        {
                                            NewString = NewString + '=' + Currency[j].ToString();
                                        }
                                    }

                                }
                                dr[4] = NewString;
                            }
                            if (dtreport.Rows[i]["Field1"].ToString() == "Claim" || dtreport.Rows[i]["Field1"].ToString() == "Penalty")
                            {
                                for (int j = 0; j < Currency.Length; j++)
                                {
                                    if (j == 1)
                                    {
                                        string[] CUR = Currency[j].ToString().Split(',');
                                        NewString = NewString + '=' +CurrencySymbol+objSys.GetCurencyConversion(str, CUR[0].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString()) + ',' + CUR[1].ToString();
                                    }
                                    if (j == 2)
                                    {

                                        NewString = NewString + '=' + CurrencySymbol+objSys.GetCurencyConversion(str, Currency[j].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
                                    }
                                    if (j == 0)
                                    {

                                        NewString = Currency[j].ToString();

                                    }

                                }
                                dr[4] = NewString;
                            }
                        }
                        catch
                        {
                            dr[4] = dtreport.Rows[i]["Description"].ToString();
                        }
                     
                     //code end
                         
                        dr[5] = Session["MonthName"].ToString();
                        dr[6] = dtreport.Rows[i]["Year"].ToString();

                        dr["Currency_Symbol"] = CurrencySymbol;
                        dtpaydueemp.Rows.Add(dr);
                    }
                }


            }


        }






        
       
       
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
        objxxtrarepot.setBrandName(BrandName);
        objxxtrarepot.setLocationName(LocationName);
        objxxtrarepot.setDepartmentName(Session["DepartmentName"].ToString());
        objxxtrarepot.setcompanyAddress(CompanyAddress);
        objxxtrarepot.setUserName(Session["UserId"].ToString());
        objxxtrarepot.setcompanyname(CompanyName);
        objxxtrarepot.setmonth("Month");
        objxxtrarepot.setyear("Year");
        objxxtrarepot.setimage(Imageurl);
        objxxtrarepot.SetHeader(Resources.Attendance.Created_By, Resources.Attendance.Month, Resources.Attendance.Year, Resources.Attendance.Employee_Code, Resources.Attendance.Employee_Name, Resources.Attendance.Type, Resources.Attendance.Narration, Resources.Attendance.Amount, Resources.Attendance.Total);
        objxxtrarepot.settitle(Resources.Attendance.Adjustment_Report);        
        objxxtrarepot.DataSource = dtpaydueemp;
        objxxtrarepot.DataMember = "settlementdt";
        ReportViewer1.Report = objxxtrarepot;
        ReportToolbar1.ReportViewer = ReportViewer1;
      
      

    }


}
