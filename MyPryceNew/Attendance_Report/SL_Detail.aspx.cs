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
using DevExpress.XtraReports.UI;
using PegasusDataAccess;

public partial class Attendance_Report_SL_Detail : System.Web.UI.Page
{
    XtraReport RptTimeCard = new XtraReport();
    //Att_AbsentReport RptShift = new Att_AbsentReport();
    SystemParameter objSys = null;
    CompanyMaster objComp = null;
    Attendance objAttendance = null;
    Set_AddressChild ObjAddress = null;
    BrandMaster objBrand = null;
    LocationMaster objLoc = null;
    Att_Employee_Notification objEmpNotice = null;
    PegasusDataAccess.DataAccessClass objDA = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        RptTimeCard.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "PL_Detail.repx");


        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objBrand = new BrandMaster(Session["DBConnection"].ToString());
        objLoc = new LocationMaster(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());
        objDA = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
        GetReport();

    }

    public void GetReport()
    {
        DateTime FromDate = new DateTime();
        DateTime ToDate = new DateTime();
        string Emplist = string.Empty;
        string EmpReport = string.Empty;
        if (Session["EmpList"] == null)
        {
            //Response.Redirect("../Attendance_Report/AttendanceReport.aspx");
            string TARGET_URL = "../Attendance_Report/AttendanceReport.aspx";
            if (Page.IsCallback)
                DevExpress.Web.ASPxWebControl.RedirectOnCallback(TARGET_URL);
            else
                Response.Redirect(TARGET_URL);
        }
        else
        {
            FromDate = objSys.getDateForInput(Session["FromDate"].ToString());
            ToDate = objSys.getDateForInput(Session["ToDate"].ToString());

            EmpReport = Session["EmpList"].ToString();

            DataTable dtEmpNF = objEmpNotice.GetEmployeeNotificationByEmpId_and_NotificationId(Session["EmpList"].ToString(), "14");

            foreach (DataRow dr in dtEmpNF.Rows)
            {
                Emplist += dr["Emp_Id"] + ",";
            }



            DataTable dtFilter = new DataTable();



            Emplist = Emplist.Substring(0, Emplist.Length - 1);

            dtFilter = objDA.return_DataTable("Select   Att_PartialLeave_Request.Trans_Id ,Set_LocationMaster.Location_Code ,Set_LocationMaster.Location_Name ,   Set_EmployeeMaster.Emp_Code ,Set_EmployeeMaster.Emp_Name ,Set_DepartmentMaster.Dep_Code ,Set_DepartmentMaster.Dep_Name ,Set_DesignationMaster.Designation ,Att_PartialLeave_Request.Request_Date_Time, Att_PartialLeave_Request.Partial_Leave_Date ,Att_PartialLeave_Request.Field5  as Method,case When Att_PartialLeave_Request.Partial_Leave_Type = 0  then 'PERSONAL' else  'OFFICIAL' end  as PartialType,Att_PartialLeave_Request.From_Time ,Att_PartialLeave_Request.To_Time ,dbo.MinutesToDuration(Att_PartialLeave_Request.Field4 )as duration,CASE WHEN  Att_PartialLeave_Request.Field1 ='Auto' then 'AUTO' else 'MANUAL' end as TransType,Att_PartialLeave_Request.Is_Confirmed From   Att_PartialLeave_Request INNER JOIN  Set_EmployeeMaster ON  Set_EmployeeMaster.Emp_Id = Att_PartialLeave_Request.Emp_Id INNER JOIN Set_LocationMaster  On Set_LocationMaster.Location_Id = Set_EmployeeMaster.Location_Id INNER JOIN   Set_DepartmentMaster  On Set_DepartmentMaster.Dep_Id = Set_EmployeeMaster.Department_Id INNER JOIN   Set_DesignationMaster ON   Set_DesignationMaster.Designation_Id =  Set_EmployeeMaster.Designation_Id  where   Att_PartialLeave_Request.Emp_id  In  (" + Emplist + ")    and (Att_PartialLeave_Request.Partial_Leave_Date >= '" + FromDate + "' and Att_PartialLeave_Request.Partial_Leave_Date <= '" + ToDate + "') Order by Att_PartialLeave_Request.Emp_id,Att_PartialLeave_Request.Partial_Leave_Date             ");

            //PassDataToSql[] paramList = new PassDataToSql[4];
            //paramList[0] = new PassDataToSql("@FromDate", FromDate.ToString("yyyy-MM-dd"), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            //paramList[1] = new PassDataToSql("@ToDate", ToDate.ToString("yyyy-MM-dd"), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            //paramList[2] = new PassDataToSql("@Emp_Id", Emplist, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            //paramList[3] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);


            //dtFilter = objDA.Reuturn_Datatable_Search("sp_Att_NIC_Report", paramList);

            // Here We need to Update Logic

           


            XRLabel lblFromDate = (XRLabel)RptTimeCard.FindControl("lblFromDate", true);
            lblFromDate.Text = FromDate.ToString("dd-MM-yyyy");


            XRLabel lblToDate = (XRLabel)RptTimeCard.FindControl("lblToDate", true);
            lblToDate.Text = ToDate.ToString("dd-MM-yyyy");


            XRLabel lblUserName = (XRLabel)RptTimeCard.FindControl("lblUserName", true);
            lblUserName.Text = Session["UserId"].ToString();





            RptTimeCard.DataSource = dtFilter;
            RptTimeCard.DataMember = "CustomSqlQuery";
            rptViewer.Report = RptTimeCard;
            rptToolBar.ReportViewer = rptViewer;

        }
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

}