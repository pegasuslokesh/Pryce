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

public partial class TempReport_Employee_Allowance_Report : BasePage
{
   // XtraReport1 objxxtrarepot = new XtraReport1();
    Allowance_Report objAlowaReport = null;
    Document_Master ObjDoc = null;
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    Pay_Employe_Allowance objpayrollall = null;
    EmployeeMaster objEmp = null;
    Set_Allowance ObjAllow = null;
    SystemParameter objSys = null;
    Pay_Employee_Due_Payment objEmpDuePay = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objAlowaReport = new Allowance_Report(Session["DBConnection"].ToString());
        ObjDoc = new Document_Master(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objpayrollall = new Pay_Employe_Allowance(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjAllow = new Set_Allowance(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objEmpDuePay = new Pay_Employee_Due_Payment(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
           Check_Page_Permission  Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "137", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
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



        DataTable dtReport = new DataTable();
        DataTable dtEmpAllowance = new DataTable();
         dtEmpAllowance.Columns.Add("Employee_Id");
        dtEmpAllowance.Columns.Add("EmpId");
        dtEmpAllowance.Columns.Add("EmpName");

        dtEmpAllowance.Columns.Add("ActAmount");
        dtEmpAllowance.Columns.Add("GivenAmount");
        dtEmpAllowance.Columns.Add("Allowance");
        dtEmpAllowance.Columns.Add("Month");
        dtEmpAllowance.Columns.Add("Year");
        dtEmpAllowance.Columns.Add("Currency_Symbol");

        string EmpIds = Session["Querystring"].ToString();
        foreach (string str in EmpIds.Split(','))
        {

            if ((str != ""))
            {
                string EmployeeName = GetEmployeeName(str);

                dtReport = objpayrollall.GetPostedAllowanceAll(str, Session["Month"].ToString(), Session["Year"].ToString());



                for (int i = 0; i < dtReport.Rows.Count; i++)
                {
                    DataRow dr = dtEmpAllowance.NewRow();
                    dr["Employee_Id"]=str;
                    dr["EmpId"] =GetEmployeeCode(dtReport.Rows[i]["Emp_Id"].ToString());
                    dr["EmpName"] = EmployeeName;
                    dr["Month"] = Session["MonthName"].ToString();

                    dr["year"] = Session["Year"].ToString();
                    dr["ActAmount"] = dtReport.Rows[i]["Allowance_Value"].ToString();

                    //this code is creeated on 01-04-2014 by jitendra upadhyay and divya mam
                    //this code for showing the actual amount with previous month adjustment
                    //code start
                    int Lastmonth = 0;
                    int LastYear = 0;

                  
                 DataTable   dtDueamount = objEmpDuePay.GetAllRecord_ByEmpId(str, HttpContext.Current.Session["CompId"].ToString());
                 if (Session["Month"].ToString() == "1")
                    {
                        LastYear = Convert.ToInt32(Session["Year"].ToString()) - 1;
                        Lastmonth = 12;
                    }
                    else
                    {
                        Lastmonth = Convert.ToInt32(Session["Month"].ToString()) - 1;
                        LastYear = Convert.ToInt32(Session["Year"].ToString());
                    }

                    try
                    {
                        dtDueamount = new DataView(dtDueamount, "Month=" + Lastmonth + " and Year=" + LastYear + " and Field1='Allowance'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    catch
                    {
                    }
                    if (dtDueamount.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtDueamount.Rows.Count;j++)
                        {
                            string[] AllowaceId = dtDueamount.Rows[j]["Field2"].ToString().Split('=');
                            if (dtReport.Rows[i]["Allowance_Id"].ToString() == AllowaceId[1].ToString())
                            {
                                dr["ActAmount"] = (float.Parse(dtReport.Rows[i]["Allowance_Value"].ToString()) + float.Parse(dtDueamount.Rows[j]["Amount"].ToString())).ToString();
                                break;
                            }
                        }

                    }

                    //code end
                    









                    dr["GivenAmount"] = dtReport.Rows[i]["Act_Allowance_Value"].ToString();

                    if (dtReport.Rows[i]["Allowance_Id"].ToString() != "0" && dtReport.Rows[i]["Allowance_Id"].ToString() != "")
                    {

                        DataTable dtAllowance = ObjAllow.GetAllowanceTruebyId(Session["CompId"].ToString(), dtReport.Rows[i]["Allowance_Id"].ToString());

                        dtAllowance = new DataView(dtAllowance, "Allowance_Id=" + dtReport.Rows[i]["Allowance_Id"].ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtAllowance.Rows.Count > 0)
                        {
                            dr["Allowance"] = dtAllowance.Rows[0]["Allowance"].ToString();
                        }
                        else
                        {
                            dr["Allowance"] = "";
                        }

                    }
                    dr["Currency_Symbol"] = objSys.GetCurencySymbol(str, HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString());

                    dtEmpAllowance.Rows.Add(dr);
                }

            }

        }




        //this code is modified on 03-04-2014 by jitendra upadhyay
        //for convert the amount in selected currency by employee and  company
        //for this work i create new function GetCurencyConversion in system parameter class where we pass the employeeid and amount for conversion 
        //this function return the amount after currency conversion

        //code start
        for (int i = 0; i < dtEmpAllowance.Rows.Count; i++)
        {
            dtEmpAllowance.Rows[i]["ActAmount"] = objSys.GetCurencyConversion(dtEmpAllowance.Rows[i]["Employee_Id"].ToString(), dtEmpAllowance.Rows[i]["ActAmount"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtEmpAllowance.Rows[i]["GivenAmount"] = objSys.GetCurencyConversion(dtEmpAllowance.Rows[i]["Employee_Id"].ToString(), dtEmpAllowance.Rows[i]["GivenAmount"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
      
        }

        //code end






        Session["dtRecordEmp"] = dtEmpAllowance;








        DataTable dtEmpAllow = (DataTable)Session["dtRecordEmp"];
        

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
            if(Session["Lang"].ToString()=="1")
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
        objAlowaReport.setBrandName(BrandName);
        objAlowaReport.setLocationName(LocationName);
        objAlowaReport.setDepartmentName(Session["DepartmentName"].ToString());
        objAlowaReport.setcompanyAddress(CompanyAddress);
        objAlowaReport.setUserName(Session["UserId"].ToString());
        objAlowaReport.setcompanyname(CompanyName);        
        objAlowaReport.setimage(Imageurl);
        objAlowaReport.setUserName(Session["UserId"].ToString());
        objAlowaReport.settitle(Resources.Attendance.Allowance_Report);
        objAlowaReport.DataSource = dtEmpAllow;
        objAlowaReport.DataMember = "dtEmpAllowance";
        objAlowaReport.SetHeaderName(Resources.Attendance.Created_By, Resources.Attendance.Month, Resources.Attendance.Year, Resources.Attendance.Employee_Name, Resources.Attendance.Employee_Code, Resources.Attendance.Allowance_Name, Resources.Attendance.Allowance_Value, Resources.Attendance.Paid_Amount, Resources.Attendance.Total);
        ReportViewer1.Report = objAlowaReport;
        ReportToolbar1.ReportViewer = ReportViewer1;



    }
}
