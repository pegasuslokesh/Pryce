using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using DevExpress.XtraReports.UI;
using System.Configuration;
using System.IO;
using System.Threading;

public partial class Attendance_Report_EmployeeTracking : System.Web.UI.Page
{
    DataAccessClass daClass = null;
    Att_LogReport RptShift = null;
    SystemParameter objSys = null;
    CompanyMaster objComp = null;
    LocationMaster ObjLocationMaster = null;
    Attendance objAttendance = null;
    Set_AddressChild ObjAddress = null;
    DepartmentMaster objDept = null;
    BrandMaster objBrand = null;
    Common cmn = null;
    EmployeeMaster ObjEmployeeMaster = null;
    Att_Employee_Notification objEmpNotice = null;
    LogProcess objLogProcess = null;
    PageControlCommon objPageCmn = null;
    static string EmpId;
    static string locationId;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        daClass = new DataAccessClass(Session["DBConnection"].ToString());
        RptShift = new Att_LogReport(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objComp = new CompanyMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        objDept = new DepartmentMaster(Session["DBConnection"].ToString());
        objBrand = new BrandMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjEmployeeMaster = new EmployeeMaster(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());
        objLogProcess = new LogProcess(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
          
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), Common.GetObjectIdbyPageURL("../attendance_report/EmployeeTracking.aspx", Session["DBConnection"].ToString()), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            if (Session["EmpList"] == null)
            {
                //Response.Redirect("../Attendance_Report/AttendanceReport.aspx");
                string TARGET_URL = "../Attendance_Report/AttendanceReport.aspx";
                if (Page.IsCallback)
                    DevExpress.Web.ASPxWebControl.RedirectOnCallback(TARGET_URL);
                else
                    Response.Redirect(TARGET_URL);
            }
            if (Session["EmpDataExceptionCount"] != null)
            {
                string[] array = (string[])Session["EmpDataExceptionCount"];

                string name_code = array[0].ToString().Split(',')[0].ToString();
                EmpId = array[0].ToString().Split(',')[1].ToString();
                locationId = array[1].ToString();
                //txtEmpList.Text= name_code;
                txtFromDate.Text = array[2].ToString();
                txtToDate.Text = array[3].ToString();
                hdnExceptionOption.Value = array[4].ToString();
                hdn_l_value.Value = array[5].ToString();
                hdn_u_value.Value = array[6].ToString();
                hdn_isNoOfOcc.Value = array[7].ToString();
                btnGetReport_Click(null, null);
                lbltrackrptValue.Text = "Tracked Data For(" + hdnExceptionOption.Value + "):";
                return;
            }

            if (Session["FromDate"] != null && Session["ToDate"] != null && Session["LocationID"] != null && Session["EmpList"] != null)
            {
                txtFromDate.Text = Session["FromDate"].ToString();
                txtToDate.Text = Session["ToDate"].ToString();
                hdn_isNoOfOcc.Value = "No";
                btnGetReport_Click(null, null);
            }
        }
    }

    public void fillGrid(string FromDate, string ToDate, string Location_Id, string Emp_Id)
    {
        try
        {
            DataTable dtInfo = new DataTable();
            PassDataToSql[] paramList = new PassDataToSql[7];
            paramList[0] = new PassDataToSql("@FromDate", FromDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
            paramList[1] = new PassDataToSql("@ToDate", ToDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
            paramList[2] = new PassDataToSql("@Location_Id", Location_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            paramList[3] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            paramList[4] = new PassDataToSql("@Exception_type", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            paramList[5] = new PassDataToSql("@l_value", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
            paramList[6] = new PassDataToSql("@u_value", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
            dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_EmployeeTracking", paramList);
            if (dtInfo.Rows.Count > 0)
            {
                gvData.DataSource = dtInfo;
                gvData.DataBind();

                Session["EmployeeTrackingData"] = dtInfo;
                
                DisplayMessage("Total Records :" + dtInfo.Rows.Count + "");
                lblTotalDataRecords.Text = "  (Total Records :" + dtInfo.Rows.Count + ")";
                btnExport.Visible = true;
                btnExportExcel.Visible = true;
            }
            else
            {
                gvData.DataSource = null;
                gvData.DataBind();
                lblTotalDataRecords.Text = " (No Record Found)";
                DisplayMessage("No Record Found");
                btnExportExcel.Visible = false;
                btnExport.Visible = false;
            }
        }
        catch (Exception err)
        {
            Response.Write("<script>alert('" + err + "')</script>");
        }
    }

    public void fillGrid(string FromDate, string ToDate, string Location_Id, string Emp_Id, string exception, string l_value, string u_value)
    {
        try
        {
            DataTable dtInfo = new DataTable();
            PassDataToSql[] paramList = new PassDataToSql[7];
            paramList[0] = new PassDataToSql("@FromDate", FromDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
            paramList[1] = new PassDataToSql("@ToDate", ToDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
            paramList[2] = new PassDataToSql("@Location_Id", Location_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            paramList[3] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            paramList[4] = new PassDataToSql("@Exception_type", exception, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            paramList[5] = new PassDataToSql("@l_value", l_value, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
            paramList[6] = new PassDataToSql("@u_value", u_value, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
            dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_EmployeeTracking", paramList);
            if (dtInfo.Rows.Count > 0)
            {
                gvData.DataSource = dtInfo;
                gvData.DataBind();

                Session["EmployeeTrackingData"] = dtInfo;
                DisplayMessage("Total Records :" + dtInfo.Rows.Count + "");
                lblTotalDataRecords.Text = "  (Total Records :" + dtInfo.Rows.Count + ")";
                btnExport.Visible = true;
                btnExportExcel.Visible = true;
            }
            else
            {
                gvData.DataSource = null;
                gvData.DataBind();
                lblTotalDataRecords.Text = " (No Record Found)";
                DisplayMessage("No Record Found");
                btnExportExcel.Visible = false;
                btnExport.Visible = false;
            }
        }
        catch (Exception err)
        {
            Response.Write("<script>alert('" + err + "')</script>");
        }
    }

    private void ExportXtraReport(string extension)
    {
        XtraReport RptShift = new XtraReport();

        RptShift.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "EmployeeTracking.repx");
        DataTable dtFilter = new DataTable();
        AttendanceDataSet rptdata = new AttendanceDataSet();
     
        rptdata.EnforceConstraints = false;
        AttendanceDataSetTableAdapters.sp_Att_EmployeeTrackingTableAdapter adp = new AttendanceDataSetTableAdapters.sp_Att_EmployeeTrackingTableAdapter();

        adp.Connection.ConnectionString = Session["DBConnection"].ToString();


        string fromdate = "";
        string todate = "";

        if (Session["EmpDataExceptionCount"] != null)
        {
            if (hdn_isNoOfOcc.Value == "yes")
            {
                adp.Fill(rptdata.sp_Att_EmployeeTracking, Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text), locationId, EmpId, hdnExceptionOption.Value, Convert.ToInt32(hdn_l_value.Value), Convert.ToInt32(hdn_u_value.Value));
                fromdate = txtFromDate.Text;
                todate = txtToDate.Text;
            }
            else
            {
                adp.Fill(rptdata.sp_Att_EmployeeTracking, Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text), locationId, EmpId, "", 0, 0);
                fromdate = txtFromDate.Text;
                todate = txtToDate.Text;
            }
        }
        else
        {
            adp.Fill(rptdata.sp_Att_EmployeeTracking, Convert.ToDateTime(Session["FromDate"].ToString()), Convert.ToDateTime(Session["ToDate"].ToString()), Session["LocationId"].ToString(), Session["EmpList"].ToString(), "", 0, 0);
            fromdate = Session["FromDate"].ToString();
            todate = Session["ToDate"].ToString();
        }

        

        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";

        CompanyName = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "1");
        Imageurl = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "2");

        DataTable DtAddress = ObjAddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());
        if (DtAddress.Rows.Count > 0)
        {
            CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
        }

        XRPictureBox xrPictureBox1 = (XRPictureBox)RptShift.FindControl("xrPictureBox1", true);
        try
        {
            xrPictureBox1.ImageUrl = Imageurl;
        }
        catch
        {

        }

        XRLabel xrTitle = (XRLabel)RptShift.FindControl("xrTitle", true);
        if (hdnExceptionOption.Value == "")
        {
            xrTitle.Text = "Employee Tracking Report";
        }
        else
        {
            xrTitle.Text = "Employee Tracking Report (" + hdnExceptionOption.Value + ")"; 
        }

        XRLabel xrFromDate = (XRLabel)RptShift.FindControl("xrFromDate", true);
        xrFromDate.Text = fromdate;

        XRLabel xrToDate = (XRLabel)RptShift.FindControl("xrToDate", true);
        xrToDate.Text = todate;


        XRLabel xrCompName = (XRLabel)RptShift.FindControl("xrCompName", true);
        xrCompName.Text = CompanyName;

        RptShift.DataSource = rptdata.sp_Att_EmployeeTracking;
        RptShift.DataMember = "sp_Att_EmployeeTracking";
        // RptShift.CreateDocument(true);
        string rptname = string.Empty;
        if (hdnExceptionOption.Value == "")
        {
            rptname = "Employee Tracking Report" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString("ddMMMyyyyhhmmss");
        }
        else
        {
            rptname = "Employee Tracking Report (" + hdnExceptionOption.Value + ")" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString("ddMMMyyyyhhmmss");
        }

        if (extension == ".pdf")
        {
            RptShift.ExportToPdf(@"" + ConfigurationManager.AppSettings["ReportPath"].ToString() + rptname + ".pdf");
            rptname = rptname + ".pdf";
            Response.ContentType = "application/pdf";
        }

        if (extension == ".xls")
        {
            RptShift.ExportToXls(@"" + ConfigurationManager.AppSettings["ReportPath"].ToString() + rptname + ".xls");
            rptname = rptname + ".xls";
            Response.ContentType = "application/vnd.ms-excel";
        }

        DisplayMessage("Exported Successfully at " + ConfigurationManager.AppSettings["ReportPath"].ToString() + rptname + "");

        Response.AppendHeader("Content-Disposition", "attachment; filename=" + rptname);
        Response.TransmitFile(ConfigurationManager.AppSettings["ReportPath"].ToString() + rptname);
        Response.End();

    }

    public void fillLocation()
    {
        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "Location_Name asc", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", " Location_Name asc", DataViewRowState.CurrentRows).ToTable();
            }
        }
        if (dtLoc.Rows.Count > 0)
        {
            gvLocation.DataSource = dtLoc;

            //ddlLocation.DataSource = dtLoc;
            //ddlLocation.DataTextField = "Location_Name";
            //ddlLocation.DataValueField = "Location_Id";
            gvLocation.DataBind();
        }
        else
        {
            gvLocation.DataSource = null;
            gvLocation.DataBind();
        }
    }

    public static string getLocationId()
    {
        return locationId;
    }

    public void setLocationId()
    {
        //locationId=ddlLocation.SelectedValue;
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    public void reset()
    {
        //ddlLocation.SelectedValue = Session["LocId"].ToString();
        DateTime date = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString());
        txtToDate.Text = GetDate(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
        txtFromDate.Text = GetDate(new DateTime(date.Year, date.Month, 1).ToString());
        gvData.DataSource = null;
        gvData.DataBind();
        lblTotalDataRecords.Text = "";
        EmpId = "";
        //txtEmpList.Text = "";
        hdn_isNoOfOcc.Value = "no";
        btnExport.Visible = false;
        btnExportExcel.Visible = false;
        Session["EmpDataExceptionCount"] = null;
        Btn_Div_ExceptionData.Attributes.Add("Class", "fa fa-plus");
        Div_ExceptionData.Attributes.Add("Class", "box box-primary collapsed-box");
        //I1.Attributes.Add("Class", "fa fa-minus");
        //Div1.Attributes.Add("Class", "box box-primary");
    }

    protected void btnGetReport_Click(object sender, EventArgs e)
    {
       
        if (objSys.getDateForInput(txtFromDate.Text) > objSys.getDateForInput(txtToDate.Text))
        {
            DisplayMessage("To Date must be greater than From Date");
            txtToDate.Focus();
            txtToDate.Text = "";
            return;
        }


        if(Session["EmpDataExceptionCount"] !=null)
        {
            if (hdn_isNoOfOcc.Value == "yes")
            {
                //fillGrid(txtFromDate.Text, txtToDate.Text, ddlLocation.SelectedValue, EmpId, hdnExceptionOption.Value, hdn_l_value.Value, hdn_u_value.Value);
                fillGrid(txtFromDate.Text, txtToDate.Text, locationId, EmpId, hdnExceptionOption.Value, hdn_l_value.Value, hdn_u_value.Value);
            }
            else
            {
                //fillGrid(txtFromDate.Text, txtToDate.Text, ddlLocation.SelectedValue, EmpId);
                fillGrid(Session["FromDate"].ToString(), Session["ToDate"].ToString(), locationId, EmpId);
            }
        }
        else
        {
            fillGrid(Session["FromDate"].ToString(), Session["ToDate"].ToString(), Session["LocationId"].ToString(), Session["EmpList"].ToString());
        }
      

        Btn_Div_ExceptionData.Attributes.Add("Class", "fa fa-minus");
        Div_ExceptionData.Attributes.Add("Class", "box box-primary");
      
    }

    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }

    protected void gvData_Sorting(object sender, GridViewSortEventArgs e)
    {
        Btn_Div_ExceptionData.Attributes.Add("Class", "fa fa-minus");
        Div_ExceptionData.Attributes.Add("Class", "box box-primary");

        DataTable dt_List_Sort = (DataTable)Session["EmployeeTrackingData"];
        string sortdir = "DESC";
        if (ViewState["SortDir"] != null)
        {
            sortdir = ViewState["SortDir"].ToString();
            if (sortdir == "ASC")
            {
                e.SortDirection = SortDirection.Descending;
                ViewState["SortDir"] = "DESC";
            }
            else
            {
                e.SortDirection = SortDirection.Ascending;
                ViewState["SortDir"] = "ASC";
            }
        }
        else
        {
            ViewState["SortDir"] = "DESC";
        }
        dt_List_Sort = (new DataView(dt_List_Sort, "", e.SortExpression + " " + ViewState["SortDir"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        objPageCmn.FillData((object)gvData, dt_List_Sort, "", "");
        Session["EmployeeTrackingData"] = dt_List_Sort;
    }

    protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvData.PageIndex = e.NewPageIndex;
        DataTable dt_list_grid = (DataTable)Session["EmployeeTrackingData"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvData, dt_list_grid, "", "");

        Btn_Div_ExceptionData.Attributes.Add("Class", "fa fa-minus");
        Div_ExceptionData.Attributes.Add("Class", "box box-primary");
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {

        ExportXtraReport(".pdf");
    }


    public override void VerifyRenderingInServerForm(Control control)
    {

    }


    protected void btnExportExcel_Click(object sender, EventArgs e)
    {

        ExportXtraReport(".xls");
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

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListGeneratedBy(string prefixText, int count, string contextKey)
    {

        try
        {
            EmployeeMaster ObjEmployeeMaster = new EmployeeMaster(HttpContext.Current.Session["DBConnection"].ToString());
            DataTable dtCon = ObjEmployeeMaster.GetEmployeeMasterDtlsByCompanyBrandLocationName(HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), locationId, prefixText.ToString());

            string[] filterlist = new string[dtCon.Rows.Count];
            if (dtCon.Rows.Count > 0)
            {
                for (int i = 0; i < dtCon.Rows.Count; i++)
                {
                    filterlist[i] = dtCon.Rows[i]["Emp_Name"].ToString() + "/" + dtCon.Rows[i]["Emp_Code"].ToString() + "," + dtCon.Rows[i]["Emp_Id"].ToString();
                }
            }
            return filterlist;
        }
        catch (Exception error)
        {

        }
        return null;
    }



   

    public string getID(string name)
    {
        string retval = "";
        try
        {
            if (name != "")
            {
                int start_pos = name.LastIndexOf(",") + 1;
                int last_pos = name.Length;
                string id = name.Substring(start_pos, last_pos - start_pos);

                int Last_pos_name_code = name.LastIndexOf(",");
                string empName_Code = name.Substring(0, Last_pos_name_code - 0);

                //txtEmpList.Text = empName_Code;

                int start_pos_Code = empName_Code.LastIndexOf("/") + 1;
                int last_pos_code = empName_Code.Length;
                string Emp_code = empName_Code.Substring(start_pos_Code, last_pos_code - start_pos_Code);


                int Last_pos_name = empName_Code.LastIndexOf("/");
                string empName = empName_Code.Substring(0, Last_pos_name - 0);


                if (start_pos != 0)
                {
                    DataTable dtEmployee = new DataTable();
                    //dtCampaign = HttpContext.Current.Session["EmpNameNId"] as DataTable;
                    dtEmployee = ObjEmployeeMaster.GetEmployeeMasterById(Session["CompId"].ToString(), id);

                    string filtertext = "Emp_name like'%" + empName.Trim() + "%'";
                    dtEmployee = new DataView(dtEmployee, filtertext, "", DataViewRowState.CurrentRows).ToTable();

                    if (dtEmployee.Rows.Count > 0)
                    {
                        retval = id;
                    }
                }
            }
        }
        catch (Exception error)
        {

        }
        return retval;
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        //locationId = ddlLocation.SelectedValue;
        Session["EmpDataExceptionCount"] = null;
        hdn_isNoOfOcc.Value = "no";
        Btn_Div_ExceptionData.Attributes.Add("Class", "fa fa-plus");
        Div_ExceptionData.Attributes.Add("Class", "box box-primary collapsed-box");
        //I1.Attributes.Add("Class", "fa fa-minus");
        //Div1.Attributes.Add("Class", "box box-primary");
        lblTotalDataRecords.Text = "";
    }

    protected void btnLogProcess_Click(object sender, EventArgs e)
    {
        //if(EmpId==null)
        //{
        //    txtEmpList.Text = "";
        //    txtEmpList.Focus();
        //    DisplayMessage("Enter Employee Name");
        //    return;
        //}
        if (txtFromDate.Text == "")
        {
            txtFromDate.Text = "";
            txtFromDate.Focus();
            DisplayMessage("Enter From date");
            return;
        }
        if (txtToDate.Text == "")
        {
            txtToDate.Text = "";
            txtToDate.Focus();
            DisplayMessage("Enter To date");
            return;
        }

        //objLogProcess.autoLogProcess(Session["CompId"].ToString(), Session["BrandId"].ToString(), ddlLocation.SelectedValue, EmpId, Session["UserId"].ToString(), "0", Convert.ToDateTime(txtFromDate.Text), Convert.ToDateTime(txtToDate.Text), Session["EmpId"].ToString());
        DisplayMessage("Log Process completed");
        //fillGrid(txtFromDate.Text, txtToDate.Text, ddlLocation.SelectedValue, EmpId);
        Btn_Div_ExceptionData.Attributes.Add("Class", "fa fa-minus");
        Div_ExceptionData.Attributes.Add("Class", "box box-primary");
        //I1.Attributes.Add("Class", "fa fa-minus");
        //Div1.Attributes.Add("Class", "box box-primary");
    }

    protected void lnkback_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Attendance_Report/AttendanceReport.aspx");
    }

}