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

public partial class HR_Report_EmpDirectoryReport : BasePage
{
    EmployeeDirectoryReport objReport = null;
    EmployeeMaster objemp = null;
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    Arc_FileTransaction ObjFile = null;
    SystemParameter objSys = null;
    Att_Employee_Notification objEmpNotice = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        Session["AccordianId"] = "109";
        Session["HeaderText"] = "HR Report";

        objReport = new EmployeeDirectoryReport(Session["DBConnection"].ToString());
        objemp = new EmployeeMaster(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjFile = new Arc_FileTransaction(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
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

        DataTable dt = objemp.GetEmployeeMasterAllData(Session["CompId"].ToString());
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
        string Id = string.Empty;
        string EmpReport = string.Empty;




        if (Request.QueryString["DocString"] == "1")
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "151", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            Session["Document"] = "Document";

            //this code is created by jitendra upadhyay on 19-09-2014
            //this code for employee should not be showing in repot if not exists in employee notification for current report
            //code start
            EmpReport = Session["Querystring"].ToString();



            //code update on 27-09-2014

            DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(Session["Querystring"].ToString(), "19");


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
            //        if (Convert.ToBoolean(dtEmpNF.Rows[0]["Is_Rpt_DocumentExp"].ToString()))
            //        {
            //            Id += EmpReport.Split(',')[i].ToString() + ",";
            //        }
            //    }
            //}

            //code end
        }
        else
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "150", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            Id = Session["Querystring"].ToString();
            Session["Document"] = "Directory";
        }

        DataTable DtClaimRecord = new DataTable();
        DtClaimRecord = ObjFile.Get_FileTransaction(Session["CompId"].ToString(), "0");


        DataTable dt = new DataTable();
        dt.Columns.Add("EmpId");
        dt.Columns.Add("EmpName");
        dt.Columns.Add("Directory_Name");
        dt.Columns.Add("Directory_Created_Date");
        dt.Columns.Add("Document_Name");

        dt.Columns.Add("File_Name");
        dt.Columns.Add("File_Upload_Date");
        dt.Columns.Add("File_Expiry_Date");


        string Document = (string)Session["Document"];
        //int ClaimType = (int)Session["ClaimType"];
        foreach (string str in Id.Split(','))
        {
            if (str != "")
            {
                string EmpCode = string.Empty;
                EmpCode = GetEmployeeCode(str);
                DataTable dtClaimRecod = DtClaimRecord;

                //Comment by Ghanshyam Suthar on '17-May-2018'
                //if (Document.Trim() == "Document")
                //{
                //    dtClaimRecod = new DataView(dtClaimRecod, "Directory_Name='" +Session["CompId"].ToString()+"/Employee/"+str + "' and File_Expiry_Date<'" + DateTime.Now.AddDays(10).ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();
                //}
                //else
                //{
                //    dtClaimRecod = new DataView(dtClaimRecod, "Directory_Name='" + Session["CompId"].ToString() + "/Employee/" +str+ "'", "", DataViewRowState.CurrentRows).ToTable();
                //}
                //Close Comment by Ghanshyam Suthar on '17-May-2018'


                //Code by Ghanshyam Suthar on '17-May-2018'

                string Days_In_Month = DateTime.DaysInMonth(Convert.ToInt32(Session["Year"].ToString()), Convert.ToInt32(Session["Month"].ToString())).ToString();
                string Start_Date = new DateTime(Convert.ToInt32(Session["Year"].ToString()), Convert.ToInt32(Session["Month"].ToString()), 1).ToString();
                string Last_Date = new DateTime(Convert.ToInt32(Session["Year"].ToString()), Convert.ToInt32(Session["Month"].ToString()), Convert.ToInt32(Days_In_Month)).ToString();

                if (Document.Trim() == "Document")
                {
                    dtClaimRecod = new DataView(dtClaimRecod, "Directory_Name='" + Session["CompId"].ToString() + "/Employee/" + str + "' And File_Expiry_Date >='" + Convert.ToDateTime(Start_Date) + "' And File_Expiry_Date <='" + Convert.ToDateTime(Last_Date) + "' ", "", DataViewRowState.CurrentRows).ToTable();
                }
                else
                {
                    dtClaimRecod = new DataView(dtClaimRecod, "Directory_Name='Employee/" + str + "'", "", DataViewRowState.CurrentRows).ToTable();
                }
                //Code end by Ghanshyam Suthar on '17-May-2018'

                if (dtClaimRecod.Rows.Count > 0)
                {

                    for (int i = 0; i < dtClaimRecod.Rows.Count; i++)
                    {
                        string EmployeeName = GetEmployeeName(str);

                        DataRow dr = dt.NewRow();
                        dr[0] = str.ToString();
                        dr[1] = EmployeeName;
                        dr[2] = dtClaimRecod.Rows[i]["Directory_Name"].ToString();
                        dr[3] = dtClaimRecod.Rows[i]["CreatedDate"].ToString();
                        dr[4] = dtClaimRecod.Rows[i]["Document_Name"].ToString();
                        dr[5] = dtClaimRecod.Rows[i]["File_Name"].ToString();
                        dr[6] = dtClaimRecod.Rows[i]["File_Upload_Date"].ToString();
                        dr[7] = dtClaimRecod.Rows[i]["File_Expiry_Date"].ToString();

                        dt.Rows.Add(dr);
                    }
                }



            }
        }
        string CompanyName = "";
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

        objReport.SetImage(Imageurl);
        if (Document == "Document")
        {
            if (Session["lang"].ToString() == "1")
            {
                objReport.setTitleName(Resources.Attendance.Document_Expiry_Report);
                lblHeader.Text = Resources.Attendance.Document_Expiry_Report;
            }
            else if (Session["lang"].ToString() == "2")
            {
                objReport.setTitleName("وثيقة تقرير انتهاء الاشتراك");
                lblHeader.Text = "وثيقة تقرير انتهاء الاشتراك";
            }
        }
        else
        {
            if (Session["lang"].ToString() == "1")
            {
                objReport.setTitleName(Resources.Attendance.Employee_Directory_Report);
                lblHeader.Text = Resources.Attendance.Employee_Directory_Report;
            }
            else if (Session["lang"].ToString() == "2")
            {
                objReport.setTitleName("موظف تقرير الدليل");
                lblHeader.Text = "موظف تقرير الدليل";
            }
        }
        objReport.setcompanyname(CompanyName);
        objReport.setaddress(CompanyAddress);
        objReport.setUserName(Session["UserId"].ToString());
        objReport.SetHeader();
        objReport.DataSource = dt;
        objReport.DataMember = "EmpDirectory";
        ReportViewer1.Report = objReport;
        ReportToolbar1.ReportViewer = ReportViewer1;
    }
    public string GetEmployeeName(object empid)
    {
        string empname = string.Empty;

        DataTable dt = objemp.GetEmployeeMasterAllData(Session["CompId"].ToString());
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
}
