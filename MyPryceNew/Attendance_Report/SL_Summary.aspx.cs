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

public partial class Attendance_Report_SL_Summary : System.Web.UI.Page
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
        RptTimeCard.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "PL_Summary.repx");


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

            dtFilter = objDA.return_DataTable("Select Att_PartialLeave_Request.Is_Confirmed ,  Att_PartialLeave_Request.Trans_Id ,Set_LocationMaster.Location_Code ,Set_LocationMaster.Location_Name ,   Set_EmployeeMaster.Emp_Code ,Set_EmployeeMaster.Emp_Name ,Set_DepartmentMaster.Dep_Name ,Set_DesignationMaster.Designation ,Att_PartialLeave_Request.Field5  as Method,case When Att_PartialLeave_Request.Partial_Leave_Type = 0  then 'PERSONAL' else  'OFFICIAL' end  as PartialType,dbo.MinutesToDuration(Att_PartialLeave_Request.Field4 )as duration,Cast (Att_PartialLeave_Request.Field4 as decimal) as Field4,CASE WHEN  Att_PartialLeave_Request.Field1 ='Auto' then 'AUTO' else 'MANUAL' end as TransType,Cast('' as varchar(10)) as totalpl,  Cast('' as varchar(10)) as npl,Cast('' as varchar(10)) as nopl,Cast('' as varchar(10))  as noapl,Cast('' as varchar(10)) as nouapl,Cast('' as varchar(10))  as nppl,Cast('' as varchar(10))  as npapl,Cast('' as varchar(10)) as npuapl,Cast('' as varchar(10))  as dhr1,Cast('' as varchar(10))  as apl,Cast('' as varchar(10))  as dhr,Set_DepartmentMaster.Dep_Code as f1,'' as f2,'' as f3,'' as f4,'' as f5, '' as f6,'' as f7,'' as f8,'' as f9,'' as f10 From   Att_PartialLeave_Request INNER JOIN  Set_EmployeeMaster ON  Set_EmployeeMaster.Emp_Id = Att_PartialLeave_Request.Emp_Id  INNER JOIN Set_LocationMaster  On Set_LocationMaster.Location_Id = Set_EmployeeMaster.Location_Id  INNER JOIN   Set_DepartmentMaster  On Set_DepartmentMaster.Dep_Id = Set_EmployeeMaster.Department_Id  INNER JOIN   Set_DesignationMaster ON   Set_DesignationMaster.Designation_Id =  Set_EmployeeMaster.Designation_Id    where   Att_PartialLeave_Request.Emp_id  In  (" + Emplist + ")    and (Att_PartialLeave_Request.Partial_Leave_Date >= '" + FromDate + "' and Att_PartialLeave_Request.Partial_Leave_Date <= '" + ToDate + "') Order by Att_PartialLeave_Request.Emp_id,Att_PartialLeave_Request.Partial_Leave_Date             ");

            //PassDataToSql[] paramList = new PassDataToSql[4];
            //paramList[0] = new PassDataToSql("@FromDate", FromDate.ToString("yyyy-MM-dd"), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            //paramList[1] = new PassDataToSql("@ToDate", ToDate.ToString("yyyy-MM-dd"), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            //paramList[2] = new PassDataToSql("@Emp_Id", Emplist, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            //paramList[3] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);


            //dtFilter = objDA.Reuturn_Datatable_Search("sp_Att_NIC_Report", paramList);

            // Here We need to Update Logic

            DataTable dtFilter1 = new DataTable();

            if(dtFilter.Rows.Count >  0)
            {
                dtFilter1 = dtFilter.Clone();
                DataTable dtEmployee = dtFilter.DefaultView.ToTable(true, "Emp_Code");

                // Get Distinct Employee


                for (int count=0; count< dtEmployee.Rows.Count;   count++)
                {
                    DataTable dtTemp = new DataView(dtFilter, "Emp_Code =  '"+dtEmployee.Rows[count][0].ToString()  +"'", "", DataViewRowState.CurrentRows).ToTable();

                  

                    DataRow row = dtFilter1.NewRow();
                    for (int c = 0; c < dtTemp.Columns.Count; c++)
                    {
                        row[c] = dtTemp.Rows[0][c];
                    }

                    try
                    {
                        var totalpl = (decimal)dtTemp.Compute("SUM(Field4)", string.Empty);
                        row["totalpl"] = totalpl;
                    }
                    catch
                    {
                        row["totalpl"] = "0";
                    }

                    try
                    {
                        dtTemp = new DataView(dtFilter, "Emp_Code =  '" + dtEmployee.Rows[count][0].ToString() + "' and TransType = 'MANUAL' and   PartialType = 'PERSONAL'", "", DataViewRowState.CurrentRows).ToTable();
                        var totalnppl = (decimal)dtTemp.Compute("SUM(Field4)", string.Empty);
                        row["nppl"] = totalnppl;
                    }
                    catch
                    {
                        row["nppl"] = "0";
                    }



                    try
                    {
                        dtTemp = new DataView(dtFilter, "Is_Confirmed = 'Approved' and  Emp_Code =  '" + dtEmployee.Rows[count][0].ToString() + "' and TransType = 'MANUAL' and   PartialType = 'PERSONAL'", "", DataViewRowState.CurrentRows).ToTable();
                        var totalnpapl = (decimal)dtTemp.Compute("SUM(Field4)", string.Empty);
                        row["npapl"] = totalnpapl;
                    }
                    catch
                    {
                        row["npapl"] = "0";
                    }




                    try
                    {
                        dtTemp = new DataView(dtFilter, "Is_Confirmed = 'Rejected' and  Emp_Code =  '" + dtEmployee.Rows[count][0].ToString() + "' and TransType = 'MANUAL' and   PartialType = 'PERSONAL'", "", DataViewRowState.CurrentRows).ToTable();
                        var totalnpuapl = (decimal)dtTemp.Compute("SUM(Field4)", string.Empty);
                        row["npuapl"] = totalnpuapl;
                    }
                    catch
                    {
                        row["npuapl"] = "0";
                    }


                    try
                    {
                        var totalnpdhr1 = (Convert.ToDecimal(row["npapl"]) > 360 ? (Convert.ToDecimal(row["npapl"]) - 360) : 0) + Convert.ToDecimal(row["npuapl"]);
                        row["dhr1"] = totalnpdhr1;
                    }
                    catch
                    {
                        row["dhr1"] = "0";
                    }




                    try
                    {
                        dtTemp = new DataView(dtFilter, "Emp_Code =  '" + dtEmployee.Rows[count][0].ToString() + "' and TransType = 'MANUAL' and   PartialType = 'OFFICIAL'", "", DataViewRowState.CurrentRows).ToTable();
                        var totalnopl = (decimal)dtTemp.Compute("SUM(Field4)", string.Empty);
                        row["nopl"] = totalnopl;
                    }
                    catch
                    {
                        row["nopl"] = "0";
                    }

                    try
                    {
                        dtTemp = new DataView(dtFilter, "Is_Confirmed = 'Approved' and  Emp_Code =  '" + dtEmployee.Rows[count][0].ToString() + "' and TransType = 'MANUAL' and   PartialType = 'OFFICIAL'", "", DataViewRowState.CurrentRows).ToTable();
                        var totalnoapl = (decimal)dtTemp.Compute("SUM(Field4)", string.Empty);
                        row["noapl"] = totalnoapl;
                    }
                    catch
                    {
                        row["noapl"] = "0";
                    }


                    try
                    {
                        dtTemp = new DataView(dtFilter, "Is_Confirmed = 'Rejected' and  Emp_Code =  '" + dtEmployee.Rows[count][0].ToString() + "' and TransType = 'MANUAL' and   PartialType = 'OFFICIAL'", "", DataViewRowState.CurrentRows).ToTable();
                        var totalnouapl = (decimal)dtTemp.Compute("SUM(Field4)", string.Empty);
                        row["nouapl"] = totalnouapl;
                    }
                    catch
                    {
                        row["nouapl"] = "0";
                    }


                    try
                    {
                        dtTemp = new DataView(dtFilter, "Emp_Code =  '" + dtEmployee.Rows[count][0].ToString() + "' and TransType = 'AUTO' ", "", DataViewRowState.CurrentRows).ToTable();
                        var totalapl = (decimal)dtTemp.Compute("SUM(Field4)", string.Empty);
                        row["apl"] = totalapl;
                    }
                    catch
                    {
                        row["apl"] = "0";
                    }







                 


                    
                    var totaldh = Convert.ToDecimal(row["apl"]) + Convert.ToDecimal(row["nouapl"]) + Convert.ToDecimal(row["dhr1"]);
                    row["dhr"] = totaldh;



                    
                         row["totalpl"] = GetInHour(row["totalpl"].ToString());
                    row["nppl"] = GetInHour(row["nppl"].ToString());
                    row["npapl"] = GetInHour(row["npapl"].ToString());
                    row["npuapl"] = GetInHour(row["npuapl"].ToString());
                    row["dhr1"] = GetInHour(row["dhr1"].ToString());
                    row["nopl"] = GetInHour(row["nopl"].ToString());
                    row["noapl"] = GetInHour(row["noapl"].ToString());
                    row["nouapl"] = GetInHour(row["nouapl"].ToString());
                    row["apl"] = GetInHour(row["apl"].ToString());
                    row["dhr"] = GetInHour(row["dhr"].ToString());
                    dtFilter1.Rows.Add(row);


                }



            }




            XRLabel lblFromDate = (XRLabel)RptTimeCard.FindControl("lblFromDate", true);
            lblFromDate.Text = FromDate.ToString("dd-MM-yyyy");


            XRLabel lblToDate = (XRLabel)RptTimeCard.FindControl("lblToDate", true);
            lblToDate.Text = ToDate.ToString("dd-MM-yyyy");


            XRLabel lblUserName = (XRLabel)RptTimeCard.FindControl("lblUserName", true);
            lblUserName.Text = Session["UserId"].ToString();





            RptTimeCard.DataSource = dtFilter1;
            RptTimeCard.DataMember = "CustomSqlQuery";
            rptViewer.Report = RptTimeCard;
            rptToolBar.ReportViewer = rptViewer;

        }
    }

    public string GetInHour(string str)
    {
        string strTime = "00:00";
        try
        {
            if(Convert.ToInt16(str) >=  60)
            {
                int mHour = Convert.ToInt16(Math.Floor(Convert.ToDecimal(str) / 60));
                int mMin = Convert.ToInt16(str) - (mHour * 60);

                strTime = Convert.ToDecimal(mHour).ToString("00") + ":" + Convert.ToDecimal(mMin).ToString("00");
                
            }
            else
            {
                strTime = "00:" + Convert.ToDecimal(str).ToString("00");
            }
        }
        catch
        {

        }
        return strTime;
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