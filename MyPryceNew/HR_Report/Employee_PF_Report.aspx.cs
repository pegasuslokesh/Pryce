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

public partial class HR_Report_Employee_PF_Report : BasePage
{

    EmployeePFReport objEmpPFDetials = new EmployeePFReport();
    Document_Master ObjDoc = null;
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    EmployeeMaster objEmpmaster = null;
    EmployeeMaster objEmp = null;
    Set_ApplicationParameter objAppParam = null;
    EmployeeParameter objempparam = null;
    Pay_Employee_Month objPayEmpMonth = null;
    SystemParameter objSys = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    Set_Deduction ObjDeduc = null;
    Pay_SalaryPlanDetail ObjSalaryPlan = null;
    DataAccessClass objDa = null;
  
    protected void Page_Load(object sender, EventArgs e)
    {
        ObjDoc = new Document_Master(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objEmpmaster = new EmployeeMaster(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objempparam = new EmployeeParameter(Session["DBConnection"].ToString());
        objPayEmpMonth = new Pay_Employee_Month(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjDeduc = new Set_Deduction(Session["DBConnection"].ToString());
        ObjSalaryPlan = new Pay_SalaryPlanDetail(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "146", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
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
        double Employee_PF = 0;
        double Employer_PF = 0;

        string strSalaryPlan = string.Empty;
        DataTable dtPF = new DataTable();

        dtPF.Columns.Add("EmpId");
        dtPF.Columns.Add("Empname");

        dtPF.Columns.Add("EmpBasicSal");
        dtPF.Columns.Add("EmployeeContributionPF");
        dtPF.Columns.Add("EmployerContributionPF");
        dtPF.Columns.Add("TotalAmount");
        dtPF.Columns.Add("EmpPFAccountNo");
        dtPF.Columns.Add("EmpCode");
        dtPF.Columns.Add("EmployeePer");
        dtPF.Columns.Add("EmployerPer");
        dtPF.Columns.Add("Currency_Symbol");

        string EmpIds = Session["Querystring"].ToString();
        try
        {

            Employee_PF = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employee_PF", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {
        }
        try
        {
            Employer_PF = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employer_PF", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {

        }



        foreach (string str in EmpIds.Split(','))
        {

            if ((str != ""))
            {
                DataRow dr = dtPF.NewRow();


                double basicsal = 0;
                string bankAcc = string.Empty;
                string EmployeeName = GetEmployeeName(str);
                string EmpCode = string.Empty;
                string EmployeeContributionPF = string.Empty;
                string EmployerContributionPF = string.Empty;
                DataTable dtemppara = new DataTable();
                dtemppara = objempparam.GetEmployeeParameterByEmpId(str, Session["CompId"].ToString());
                if (dtemppara.Rows.Count > 0)
                {

                    basicsal = Convert.ToDouble(dtemppara.Rows[0]["Basic_Salary"].ToString());
                    strSalaryPlan = dtemppara.Rows[0]["Salary_Plan_Id"].ToString();
                }




                DataTable dtEmpInfo = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), str);
                if (dtEmpInfo.Rows.Count > 0)
                {
                    bankAcc = dtEmpInfo.Rows[0]["Account_No"].ToString();
                    EmpCode = dtEmpInfo.Rows[0]["Emp_Code"].ToString();
                }

                DataTable dtEmpPay = new DataTable();
                dtEmpPay = objPayEmpMonth.GetAllRecordPostedEmpMonth(str, Session["Month"].ToString(), Session["Year"].ToString(), HttpContext.Current.Session["CompId"].ToString());

                EmployeeContributionPF = "0";
                if (dtEmpPay.Rows.Count > 0)
                {

                    EmployeeContributionPF = dtEmpPay.Rows[0]["Field1"].ToString();
                    EmployerContributionPF = dtEmpPay.Rows[0]["Field3"].ToString();
                }

                bool IsPF = false;


                bool IsEmpINPayroll = false;


                DataTable dt1 = objempparam.GetEmployeeParameterByEmpId(str, Session["CompId"].ToString());


                if (dt1.Rows.Count > 0)
                {
                    try
                    {
                        IsEmpINPayroll = Convert.ToBoolean(dt1.Rows[0]["Field6"].ToString());
                    }
                    catch
                    {

                    }
                    try
                    {
                        basicsal = double.Parse(dt1.Rows[0]["Basic_Salary"].ToString());
                    }
                    catch
                    {

                    }

                    try
                    {
                        IsPF = Convert.ToBoolean(dt1.Rows[0]["Field4"].ToString());
                    }
                    catch
                    {

                    }

                }



                string[] strPf = getEpfDeduction(str);


                DataTable dtemppayrollpost = objPayEmpMonth.GetAllRecordPostedEmpMonth(str, Session["Month"].ToString(), Session["Year"].ToString(),HttpContext.Current.Session["CompId"].ToString());
                if (dtemppayrollpost.Rows.Count > 0)
                {

                    dr["EmpId"] = str;
                    dr["Empname"] = GetEmployeeName(str);
                    dr["EmpBasicSal"] = strPf[4].ToString();


                    dr["EmployeeContributionPF"] = strPf[1].ToString();
                    dr["EmployerContributionPF"] = strPf[3].ToString();


                    double tot = 0;
                    tot = Convert.ToDouble(strPf[1].ToString()) + Convert.ToDouble(strPf[3].ToString());
                    dr["TotalAmount"] = tot.ToString();
                    dr["EmpPFAccountNo"] = bankAcc;
                    dr["EmpCode"] = EmpCode;

                    string EmployeePer = "";
                    string EmployerPer = "";
                    try
                    {
                        EmployeePer = objAppParam.GetApplicationParameterValueByParamName("Employee_PF", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                        EmployerPer = objAppParam.GetApplicationParameterValueByParamName("Employer_PF", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    }
                    catch
                    {

                    }

                    dr["EmployeePer"] = strPf[0].ToString();
                    dr["EmployerPer"] = strPf[2].ToString();
                    dr["Currency_Symbol"] = objSys.GetCurencySymbol(str, HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                    dtPF.Rows.Add(dr);
                }
                else
                {




                    if (IsPF == true)
                    {


                        dr["EmpId"] = str;
                        dr["Empname"] = GetEmployeeName(str);
                        dr["EmpBasicSal"] = strPf[4].ToString();


                        dr["EmployeeContributionPF"] = strPf[1].ToString();
                        dr["EmployerContributionPF"] = strPf[3].ToString();


                        double tot = 0;
                        tot = Convert.ToDouble(strPf[1].ToString()) + Convert.ToDouble(strPf[3].ToString());
                        dr["TotalAmount"] = tot.ToString();
                        dr["EmpPFAccountNo"] = bankAcc;
                        dr["EmpCode"] = EmpCode;

                        string EmployeePer = "";
                        string EmployerPer = "";
                        try
                        {
                            EmployeePer = objAppParam.GetApplicationParameterValueByParamName("Employee_PF", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                            EmployerPer = objAppParam.GetApplicationParameterValueByParamName("Employer_PF", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                        }
                        catch
                        {

                        }

                        dr["EmployeePer"] = strPf[0].ToString();
                        dr["EmployerPer"] = strPf[2].ToString();
                        dr["Currency_Symbol"] = objSys.GetCurencySymbol(str, HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString());
                        dtPF.Rows.Add(dr);

                    }
                }





            }
        }
        //this code is modified on 03-04-2014 by jitendra upadhyay
        //for convert the amount in selected currency by employee and  company
        //for this work i create new function GetCurencyConversion in system parameter class where we pass the employeeid and amount for conversion 
        //this function return the amount after currency conversion

        //code start
        for (int i = 0; i < dtPF.Rows.Count; i++)
        {

            dtPF.Rows[i]["EmpBasicSal"] = objSys.GetCurencyConversion(dtPF.Rows[i]["EmpId"].ToString(), dtPF.Rows[i]["EmpBasicSal"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtPF.Rows[i]["EmployeeContributionPF"] = objSys.GetCurencyConversion(dtPF.Rows[i]["EmpId"].ToString(), dtPF.Rows[i]["EmployeeContributionPF"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtPF.Rows[i]["EmployerContributionPF"] = objSys.GetCurencyConversion(dtPF.Rows[i]["EmpId"].ToString(), dtPF.Rows[i]["EmployerContributionPF"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString());
            dtPF.Rows[i]["TotalAmount"] = SystemParameter.GetCurrencySmbol(Common.GetEmployeeCurreny(dtPF.Rows[i]["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), objSys.GetCurencyConversionForInv(Common.GetEmployeeCurreny(dtPF.Rows[i]["EmpId"].ToString(), Session["DBConnection"].ToString(), HttpContext.Current.Session["LocCurrencyId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["LocId"].ToString()), dtPF.Rows[i]["TotalAmount"].ToString()), Session["DBConnection"].ToString());

        }

        //code end

        Session["dtPFReport"] = dtPF;

        string[] month = new string[13];
        month[0] = "-Select-";
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

        DataTable dtEmpPFRecod = dtPF;

        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        string CompanyContact = "";
        string EmpImageurl = "";
        string Mailid = "";
        string Websitename = "";
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
            CompanyContact = DtAddress.Rows[0]["PhoneNo1"].ToString();
            Mailid = DtAddress.Rows[0]["EmailId1"].ToString();
            Websitename = DtAddress.Rows[0]["WebSite"].ToString();

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
        objEmpPFDetials.setBrand(BrandName);
        objEmpPFDetials.setLocation(LocationName);
        objEmpPFDetials.setDepartment(Session["DepartmentName"].ToString());

        objEmpPFDetials.setcompanyAddress(CompanyAddress);
        objEmpPFDetials.setcompanyname(CompanyName);
        objEmpPFDetials.setUserName(Session["UserId"].ToString());
        objEmpPFDetials.setmailid(Mailid);
        objEmpPFDetials.setwebsite(Websitename);
        objEmpPFDetials.setHeader();
        objEmpPFDetials.setimage(Imageurl);
        objEmpPFDetials.setcontact(CompanyContact);
        objEmpPFDetials.setTitle(Resources.Attendance.PF_Report + ' ' + Resources.Attendance.For + ' ' + Session["MonthName"].ToString() + ' ' + Session["Year"].ToString() + "");
        objEmpPFDetials.DataSource = dtEmpPFRecod;
        objEmpPFDetials.DataMember = "dtEmployeePF";

        ReportViewer1.Report = objEmpPFDetials;

        ReportToolbar1.ReportViewer = ReportViewer1;

    }


  






    public string[] getEpfDeduction(string strEmpId)
    {
        DataTable dtDeduction = new DataTable();
        string strDeductionId = GetDeductionIdbyDeductionType("PF");

        dtDeduction = objDa.return_DataTable("select Applicable_Amount,Act_Deduction_Value from Pay_Employe_Deduction_Temp where month='" + Session["Month"].ToString() + "' and year='" + Session["Year"].ToString() + "' and Emp_id=" + strEmpId + " and Deduction_Id=" + strDeductionId + "");

        if (dtDeduction.Rows.Count == 0)
        {

            dtDeduction = objDa.return_DataTable("select Applicable_Amount  ,Act_Deduction_Value from Pay_Employe_Deduction_Temp where month='" + Session["Month"].ToString() + "' and year='" + Session["Year"].ToString() + "' and Emp_id=" + strEmpId + " and Deduction_Id=" + strDeductionId + "");


        }


        string[] str = new string[5];



        double ApplicableAmount = 0;
        double Employee_PF = 0;
        double Employer_PF = 0;
        double Employer_FPF = 0;
        double Pf_Applicable_Salary = 0;
        double PF_Inspection_Charges = 0;
        double PF_EDLI = 0;
        double PF_Admin_Charges = 0;


        try
        {
            ApplicableAmount = double.Parse(dtDeduction.Rows[0]["Applicable_Amount"].ToString());
        }
        catch
        {

        }

        try
        {

            Employee_PF = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employee_PF", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {
        }
        try
        {
            Employer_PF = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employer_PF", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {

        }

        try
        {
            Employer_FPF = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Employer_FPF", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {

        }

        try
        {
            Pf_Applicable_Salary = double.Parse(objAppParam.GetApplicationParameterValueByParamName("PF_Applicable_Salary", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {

        }

        try
        {
            PF_Inspection_Charges = double.Parse(objAppParam.GetApplicationParameterValueByParamName("PF_Inspection_Charges", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {

        }
        try
        {
            PF_EDLI = double.Parse(objAppParam.GetApplicationParameterValueByParamName("PF_EDLI", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {

        }
        try
        {
            PF_Admin_Charges = double.Parse(objAppParam.GetApplicationParameterValueByParamName("PF_Admin_Charges", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {

        }




        if (dtDeduction.Rows.Count>0)
        {


            str[0] = Employee_PF.ToString();
            str[1] = dtDeduction.Rows[0]["Act_Deduction_Value"].ToString();
            str[2] = (Employer_PF + Employer_FPF + PF_Inspection_Charges + PF_EDLI + PF_Admin_Charges).ToString();
            str[3] = ((ApplicableAmount * (Employer_PF + Employer_FPF + PF_Inspection_Charges + PF_EDLI + PF_Admin_Charges)) / 100).ToString();
            str[4] = ApplicableAmount.ToString();
        }
        else
        {
            str[0] = "0";
            str[1] = "0";
            str[2] = "0";
            str[3] = "0";
            str[4] = "0";

        }

        return str;
    }

    public string GetDeductionIdbyDeductionType(string strType)
    {
        string strDeductionId = "0";

        DataTable dt = ObjDeduc.GetDeductionTrueAll(Session["CompId"].ToString());

        dt = new DataView(dt, "Field1='" + strType + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {
            strDeductionId = dt.Rows[0]["Deduction_Id"].ToString();

        }
        return strDeductionId;
    }


    public double getApplicableAmountByDeductionId(string strBasicSalary, string strSalaryPlanId, string strDeductionId)
    {
        double strApplicableAmount = 0;
        double BasicSalary = 0;

        try
        {
            BasicSalary = Convert.ToDouble(strBasicSalary);
        }
        catch
        {

        }


        strApplicableAmount += BasicSalary;

        DataTable dt = ObjSalaryPlan.GetApplicableAllowance_By_headerId_and_deductionId(strSalaryPlanId, strDeductionId);

        foreach (DataRow dr in dt.Rows)
        {
            if (dr["Calculation_Method"].ToString() == "Fixed")
            {
                strApplicableAmount += Convert.ToDouble(dr["Value"].ToString());
            }
            else
            {
                try
                {
                    strApplicableAmount += (BasicSalary * Convert.ToDouble(dr["Value"].ToString())) / 100;
                }
                catch
                {

                }

            }

        }





        return strApplicableAmount;

    }



}
