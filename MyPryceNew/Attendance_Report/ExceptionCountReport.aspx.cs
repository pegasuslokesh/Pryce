using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.IO;
using System.Web;
using DevExpress.XtraReports.UI;
using System.Configuration;

public partial class Attendance_Report_ExceptionCountReport : BasePage
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
    PageControlCommon objPageCmn = null;
    Att_Employee_Notification objEmpNotice = null;
    static string ExceptionOptions = "";
    static string LocationID = "";
    static string LocationList = "";
    static string EmpList = "";
    static string Depart = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        //txtFromDate.Attributes.Add("readonly", "readonly");
        //txtToDate.Attributes.Add("readonly", "readonly");

        if (radioLateIn.Checked == true || radioEarlyOut.Checked == true || radioLateInEalryOut.Checked==true)
        {
            divFrom.Attributes.Add("style", "display:block");
            divTo.Attributes.Add("style", "display:block");
        }
        else
        {
            divFrom.Attributes.Add("style", "display:none");
            divTo.Attributes.Add("style", "display:none");
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
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
         
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), Common.GetObjectIdbyPageURL("../Attendance_Report/ExceptionCountReport.aspx", Session["DBConnection"].ToString()), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
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
            txtNoOfOccurance.Text = "1";
            //DateTime date = System.DateTime.Now;
            //txtToDate.Text = GetDate(System.DateTime.Now.ToString());
            //txtFromDate.Text = GetDate(new DateTime(date.Year, date.Month, 1).ToString());

            //fillLocation();
            //LocationID = Session["LocId"].ToString();
            //ddlLocation.SelectedValue = LocationID;
            //LocationList = LocationID;
            //FillDepartment();
            //setEmpListPopup();
        }
    }

    public void fillGrid(string FromDate, string ToDate, string Location_Id, string Emp_Id, string Exception_type, string No_Of_Occurance, string l_value, string u_value)
    {
        try
        {
            if(l_value=="")
            {
                l_value = "0";
            }
            if (u_value == "")
            {
                u_value = "0";
            }


            DataTable dtInfo = new DataTable();
            PassDataToSql[] paramList = new PassDataToSql[8];
            paramList[0] = new PassDataToSql("@FromDate", FromDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
            paramList[1] = new PassDataToSql("@ToDate", ToDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
            paramList[2] = new PassDataToSql("@Location_Id", Location_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            paramList[3] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            paramList[4] = new PassDataToSql("@Exception_type", Exception_type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
            paramList[5] = new PassDataToSql("@No_Of_Occurance", No_Of_Occurance, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
            paramList[6] = new PassDataToSql("@l_value", l_value, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
            paramList[7] = new PassDataToSql("@u_value", u_value, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
            dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_ExceptionCountReport", paramList);
            if (dtInfo.Rows.Count > 0)
            {
                gvData.PageIndex = 0;
                gvData.DataSource = dtInfo;
                gvData.DataBind();

                Session["ExceptionCountData"] = dtInfo;
                lblTotalDataRecords.Text = "  (Total Records :" + dtInfo.Rows.Count + ")";
                DisplayMessage("Total Records :" + dtInfo.Rows.Count + "");
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
            DisplayMessage(err.Message);
        }
    }

    //public void setEmpListPopup()
    //{
    //    string where = "";
    //    if(Depart=="")
    //    {
    //        FillDepartment();
    //    }
    //    if(LocationID=="")
    //    {
    //      fillLocation();
    //    }

    //    if (LocationID != "0")
    //    {
            
    //        if(ddlDepartment.SelectedValue!="0")
    //        {
    //            where = "Location_Id=" + LocationID + " and IsActive='true' and Field2='false' and Department_Id<>0 and Department_Id="+ddlDepartment.SelectedValue+"";
    //        }
    //        else
    //        {
    //            where = "Location_Id=" + LocationID + " and IsActive='true' and Field2='false' and Department_Id<>0 and Department_Id in ("+Depart+")";
    //        }

    //    }
    //    else
    //    {
    //        if (ddlDepartment.SelectedValue != "0")
    //        {
    //            where = "IsActive='true' and Field2='false' and Department_Id<>0 and Department_Id=" + ddlDepartment.SelectedValue + "";
    //        }
    //        else
    //        {
    //            where = "IsActive='true' and Field2='false' and Department_Id<>0 and Department_Id in (" + Depart + ")";                
    //        }
    //    }
    //    DataTable EmpName_Id = daClass.return_DataTable("select Emp_Name, cast(Emp_Code as int) as Emp_Code,Emp_Id from Set_EmployeeMaster where " + where + "");
    //    GvEmployeeList.DataSource = EmpName_Id;
    //    GvEmployeeList.DataBind();
    //    lbltotalRecords.Text = "Total Records:" + EmpName_Id.Rows.Count;
    //    Session["EmpListData"] = EmpName_Id;
    //}

    //protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    LocationID = ddlLocation.SelectedValue.ToString();

    //    GvEmployeeList.DataSource = null;
    //    GvEmployeeList.DataBind();
    //    FillDepartment();
    //    setEmpListPopup();
    //    setLocationList(LocationID);

    //    EmpList = "";
    //}
    //public void fillLocation()
    //{
    //    DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
    //    try
    //    {
    //        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "Location_Name asc", DataViewRowState.CurrentRows).ToTable();
    //    }
    //    catch
    //    {
    //    }
    //    if (!Common.GetStatus())
    //    {
    //        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L");
    //        if (LocIds != "")
    //        {
    //            dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", " Location_Name asc", DataViewRowState.CurrentRows).ToTable();
    //        }
    //    }
    //    if (dtLoc.Rows.Count > 0)
    //    {
    //        ddlLocation.Items.Add(new System.Web.UI.WebControls.ListItem("All", "0"));
    //        for (int i = 0; i < dtLoc.Rows.Count; i++)
    //        {
    //            ddlLocation.Items.Add(new System.Web.UI.WebControls.ListItem(dtLoc.Rows[i]["Location_Name"].ToString(), dtLoc.Rows[i]["Location_Id"].ToString()));                
    //        }
    //    }
    //    else
    //    {
    //        ddlLocation.Items.Clear();
    //    }
    //}


    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    public void reset()
    {

        txtNoOfOccurance.Text = "1";
        radioAbsent.Checked = false;
        radioEarlyOut.Checked = false;
        radioLateIn.Checked = false;
        radioMissedCheckIn.Checked = false;
        radioMissedCheckOut.Checked = false;
        //LocationID = Session["LocId"].ToString();
        //ddlLocation.SelectedValue = Session["LocId"].ToString();
        //LocationList = LocationID;
        txtNoOfOccurance.Text = "";
        //DateTime date = System.DateTime.Now;
        //txtToDate.Text = GetDate(System.DateTime.Now.ToString());
        //txtFromDate.Text = GetDate(new DateTime(date.Year, date.Month, 1).ToString());
        txtLateEarlyFrom.Text = "0";
        txtlblLateEarlyTo.Text = "0";
        gvData.DataSource = null;
        gvData.DataBind();
        //GvEmployeeList.DataSource = null;
        //GvEmployeeList.DataBind();
        //setEmpListPopup();
        lblTotalDataRecords.Text = "";
        //EmpList = "";
    }

    //public void setEmpList()
    //{
    //    CheckBox chk;
    //    HiddenField hdn;
    //    EmpList = "";
    //    CheckBox chkEmp = GvEmployeeList.HeaderRow.FindControl("chkSelectAll") as CheckBox;
    //    for (int i = 0; i < GvEmployeeList.Rows.Count; i++)
    //    {
    //        chk = GvEmployeeList.Rows[i].FindControl("chkSelect") as CheckBox;
    //        hdn = GvEmployeeList.Rows[i].FindControl("hdnEmpId") as HiddenField;
    //        if (chk.Checked)
    //        {
    //            EmpList = EmpList + hdn.Value + ",";
    //        }
    //    }

    //}

    //public void setEmpList_WhenDepartmentSelected()
    //{
    //    CheckBox chk;
    //    HiddenField hdn;
    //    EmpList = "";
    //    CheckBox chkEmp = GvEmployeeList.HeaderRow.FindControl("chkSelectAll") as CheckBox;
    //    if(chkEmp.Checked)
    //    {
    //        for (int i = 0; i < GvEmployeeList.Rows.Count; i++)
    //        {
    //            hdn = GvEmployeeList.Rows[i].FindControl("hdnEmpId") as HiddenField;
    //            EmpList = EmpList + hdn.Value + ",";
    //        }
    //    }
    //    else
    //    {
    //        for (int i = 0; i < GvEmployeeList.Rows.Count; i++)
    //        {
    //            chk = GvEmployeeList.Rows[i].FindControl("chkSelect") as CheckBox;
    //            if(chk.Checked)
    //            {
    //                hdn = GvEmployeeList.Rows[i].FindControl("hdnEmpId") as HiddenField;
    //                EmpList = EmpList + hdn.Value + ",";
    //            }
    //        }

    //        if(EmpList=="")
    //        {
    //            for (int i = 0; i < GvEmployeeList.Rows.Count; i++)
    //            {
    //                hdn = GvEmployeeList.Rows[i].FindControl("hdnEmpId") as HiddenField;
    //                EmpList = EmpList + hdn.Value + ",";
    //            }
    //        }
    //    }
    //}

    //public void setLocationList(string locID)
    //{
       
    //    if (locID == "0")
    //    {
    //        LocationList = "";

    //        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
    //        try
    //        {
    //            dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "Location_Name asc", DataViewRowState.CurrentRows).ToTable();
    //        }
    //        catch
    //        {
    //        }
    //        for (int i = 0; i < dtLoc.Rows.Count; i++)
    //        {
    //            if (i == dtLoc.Rows.Count - 1)
    //            {
    //                LocationList = LocationList + dtLoc.Rows[i]["Location_Id"].ToString();
    //            }
    //            else
    //            {
    //                LocationList = LocationList + dtLoc.Rows[i]["Location_Id"].ToString() + ",";
    //            }
    //        }
    //    }
    //    else
    //    {
    //        LocationList = "";
    //        LocationList = ddlLocation.SelectedValue.ToString();
    //    }
    //}

    public void getExceptionOption()
    {
        ExceptionOptions = "";

        if (radioAbsent.Checked)
        {
            ExceptionOptions = "Absent";
            txtLateEarlyFrom.Text = "0";
            txtlblLateEarlyTo.Text = "0";
        }
        else if (radioMissedCheckIn.Checked)
        {
            ExceptionOptions = "MissedCheckIn";
            txtLateEarlyFrom.Text = "0";
            txtlblLateEarlyTo.Text = "0";
        }
        else if (radioMissedCheckOut.Checked)
        {
            ExceptionOptions = "MissedCheckOut";
            txtLateEarlyFrom.Text = "0";
            txtlblLateEarlyTo.Text = "0";
        }
        else if (radioLateIn.Checked)
        {
            ExceptionOptions = "LateIn";
        }
        else if (radioEarlyOut.Checked)
        {
            ExceptionOptions = "EarlyOut";
        }
        else if (radioLateInEalryOut.Checked)
        {
            ExceptionOptions = "LateInAndEarlyOut";
        }

        
    }

    protected void btnGetReport_Click(object sender, EventArgs e)
    {
        if (Session["EmpList"] == null)
        {
            //Response.Redirect("../Attendance_Report/AttendanceReport.aspx");
            string TARGET_URL = "../Attendance_Report/AttendanceReport.aspx";
            if (Page.IsCallback)
                DevExpress.Web.ASPxWebControl.RedirectOnCallback(TARGET_URL);
            else
                Response.Redirect(TARGET_URL);
        }

        getExceptionOption();

        if (ExceptionOptions == "")
        {
            DisplayMessage("Please Select An Exception Option");
            radioAbsent.Focus();
            return;
        }

        if (txtNoOfOccurance.Text.Trim() == "")
        {
            txtNoOfOccurance.Focus();
            return;
        }
        
        //EmpList = "";

        
        //try
        //{
        //    CheckBox chkEmp;
        //    chkEmp = GvEmployeeList.HeaderRow.FindControl("chkSelectAll") as CheckBox;
        //    if (chkEmp.Checked == true)
        //    {
        //        //EmpList = "0";
        //        setEmpList_WhenDepartmentSelected();
        //    }
        //    else
        //    {
        //        if (ddlDepartment.SelectedValue != "0")
        //        {
        //            setEmpList_WhenDepartmentSelected();
        //        }
        //        else
        //        {
        //            setEmpList();
        //        }

        //        if (EmpList != "")
        //        {
        //            int Last_pos = EmpList.LastIndexOf(",");
        //            EmpList = EmpList.Substring(0, Last_pos - 0);
        //            string[] count = EmpList.Split(',');
        //        }
        //        else
        //        {
        //            EmpList = "0";
        //        }
        //    }
        //}
        //catch (Exception err)
        //{
        //    gvData.DataSource = null;
        //    gvData.DataBind();
        //    lblTotalDataRecords.Text = " (No Employee Found)";
        //    DisplayMessage("No Employee Found");
        //    btnExportExcel.Visible = false;
        //    btnExport.Visible = false;
        //}

        if (radioLateIn.Checked == true || radioEarlyOut.Checked == true || radioLateInEalryOut.Checked==true)
        {
            if (txtLateEarlyFrom.Text.Trim() == "")
            {
                txtLateEarlyFrom.Focus();
                DisplayMessage("Enter Late In (in Minutes)");
                return;
            }
            if (txtlblLateEarlyTo.Text.Trim() == "")
            {
                txtlblLateEarlyTo.Focus();
                DisplayMessage("Enter Early Out (in Minutes)");
                return;
            }

            if (Convert.ToInt32(txtlblLateEarlyTo.Text) < Convert.ToInt32(txtLateEarlyFrom.Text))
            {
                DisplayMessage("Late In By/Early Out By To must be greater then From");
                txtlblLateEarlyTo.Text = "";
                return;
            }
        }


        //if (objSys.getDateForInput(txtFromDate.Text) > objSys.getDateForInput(txtToDate.Text))
        //{
        //    DisplayMessage("To Date must be greater than From Date");
        //    txtToDate.Focus();
        //    txtToDate.Text = "";
        //    return;
        //}

        //if (LocationList == "")
        //{
        //    setLocationList(ddlLocation.SelectedValue);
        //}


        //fillGrid(txtFromDate.Text, txtToDate.Text, LocationList, EmpList, ExceptionOptions, txtNoOfOccurance.Text, txtLateEarlyFrom.Text, txtlblLateEarlyTo.Text);
        fillGrid(Session["FromDate"].ToString(), Session["ToDate"].ToString(), Session["LocationID"].ToString(), Session["EmpList"].ToString(), ExceptionOptions, txtNoOfOccurance.Text, txtLateEarlyFrom.Text, txtlblLateEarlyTo.Text);
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

        DataTable dt_List_Sort = (DataTable)Session["ExceptionCountData"];
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
        Session["ExceptionCountData"] = dt_List_Sort;
    }


    private void ExportXtraReport(string extension)
    {
        XtraReport RptShift = new XtraReport();

        RptShift.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "Att_Exception_Count.repx");
        DataTable dtFilter = new DataTable();
        AttendanceDataSet rptdata = new AttendanceDataSet();
        rptdata.EnforceConstraints = false;
        AttendanceDataSetTableAdapters.sp_Att_ExceptionCountReport1TableAdapter  adp = new AttendanceDataSetTableAdapters.sp_Att_ExceptionCountReport1TableAdapter();
        adp.Connection.ConnectionString = Session["DBConnection"].ToString();
        //adp.Fill(rptdata.sp_Att_ExceptionCountReport1 ,Convert.ToDateTime(txtFromDate.Text),Convert.ToDateTime(txtToDate.Text), LocationList, EmpList, ExceptionOptions, Convert.ToInt16(txtNoOfOccurance.Text), Convert.ToInt16(txtLateEarlyFrom.Text), Convert.ToInt16(txtlblLateEarlyTo.Text));
        adp.Fill(rptdata.sp_Att_ExceptionCountReport1, Convert.ToDateTime(Session["FromDate"].ToString()), Convert.ToDateTime(Session["ToDate"].ToString()), Session["LocationID"].ToString(), Session["EmpList"].ToString(), ExceptionOptions, Convert.ToInt16(txtNoOfOccurance.Text), Convert.ToInt16(txtLateEarlyFrom.Text), Convert.ToInt16(txtlblLateEarlyTo.Text));

        string CompanyName = "";
        string CompanyAddress = "";
        string Imageurl = "";
        string BrandName = "";
        
        CompanyName = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "1");
        // Image Url
        Imageurl = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "2");
        // Get Brand Name
        BrandName = objAttendance.GetBrandName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["Lang"].ToString());
       
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
        //xrTitle.Text = getReportName()+ " From Date : "+txtFromDate.Text  +" To Date : "+ txtToDate.Text ;
        xrTitle.Text = getReportName() + " From Date : " + Session["FromDate"].ToString() + " To Date : " + Session["ToDate"].ToString();
        XRLabel xrCompName = (XRLabel)RptShift.FindControl("xrCompName", true);
        xrCompName.Text = CompanyName;
        XRLabel xrCompAddress = (XRLabel)RptShift.FindControl("xrCompAddress", true);
        xrCompAddress.Text = CompanyAddress;
      
        XRTableCell  xrEmpCode = (XRTableCell)RptShift.FindControl("xrEmpCode", true);
        xrEmpCode.Text = Resources.Attendance.Emp_Code;
        XRTableCell xrEmpName = (XRTableCell)RptShift.FindControl("xrEmpName", true);
        xrEmpName.Text = Resources.Attendance.Employee_Name;
        XRTableCell xrDepartmentName = (XRTableCell)RptShift.FindControl("xrDepartmentName", true);
        xrDepartmentName.Text = Resources.Attendance.Department_Name;
        XRTableCell xrDesingation = (XRTableCell)RptShift.FindControl("xrDesignation", true);
        xrDesingation.Text = Resources.Attendance.Designation;
        XRTableCell XrNoOfCurrence = (XRTableCell)RptShift.FindControl("XrNoOfCurrence", true);
        XrNoOfCurrence.Text = "No Of Occurrence";
        
        RptShift.DataSource = rptdata.sp_Att_ExceptionCountReport1 ;
        RptShift.DataMember = "sp_Att_ExceptionCountReport";
        RptShift.CreateDocument(true);

        string rptname = getReportName();
        if (extension==".pdf")
        {
            RptShift.ExportToPdf(@""+ ConfigurationManager.AppSettings["ReportPath"].ToString() + rptname + ".pdf");
            rptname = rptname + ".pdf";
            Response.ContentType = "application/pdf";
        }

        if(extension==".xls")
        {
            RptShift.ExportToXls(@"" + ConfigurationManager.AppSettings["ReportPath"].ToString() + rptname + ".xls");
            rptname = rptname + ".xls";
            Response.ContentType = "application/vnd.ms-excel";
        }

        DisplayMessage("Exported Successfully at "+ ConfigurationManager.AppSettings["ReportPath"].ToString()  + rptname);
        //Response.Redirect(ConfigurationManager.AppSettings["ReportPath"].ToString() + rptname);

       
        Response.AppendHeader("Content-Disposition", "attachment; filename="+ rptname);
        Response.TransmitFile(ConfigurationManager.AppSettings["ReportPath"].ToString() + rptname);
        Response.End();

    }
    protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        gvData.PageIndex = e.NewPageIndex;
        DataTable dt_list_grid = (DataTable)Session["ExceptionCountData"];
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

    protected void GvEmployeeList_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dt_List_Sort = (DataTable)Session["EmpListData"];
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
        objPageCmn.FillData((object)GvEmployeeList, dt_List_Sort, "", "");
        Session["EmpListData"] = dt_List_Sort;
    }

    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        ExportXtraReport(".xls");
    }

    public string getReportName()
    {
        string ExceptionOptions = "";
        if (radioAbsent.Checked)
        {
            ExceptionOptions = "ExceptionReport(Absent)"+Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString("hhmmyyss");
        }
        else if (radioMissedCheckIn.Checked)
        {
            ExceptionOptions = "ExceptionReport(MissedCheckIn)" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString("hhmmyyss");
        }
        else if (radioMissedCheckOut.Checked)
        {
            ExceptionOptions = "ExceptionReport(MissedCheckOut)" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString("hhmmyyss");
        }
        else if (radioLateIn.Checked)
        {
            ExceptionOptions = "ExceptionReport(LateIn)" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString("hhmmyyss");
        }
        else if (radioEarlyOut.Checked)
        {
            ExceptionOptions = "ExceptionReport(EarlyOut)" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString("hhmmyyss");
        }

        return ExceptionOptions;

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

    
    protected void lbtnEmpCode_Command(object sender, CommandEventArgs e)
    {
        string empId = e.CommandName.ToString();
        string locId = e.CommandArgument.ToString();

        //string fromdt = txtFromDate.Text;
        //string todt = txtToDate.Text;

        string fromdt = Session["FromDate"].ToString();
        string todt = Session["ToDate"].ToString();

        string l_value = txtLateEarlyFrom.Text;
        string u_value = txtlblLateEarlyTo.Text;
        string[] array = { empId, locId, fromdt, todt, "all", l_value, u_value,"no" };
        Session["EmpDataExceptionCount"] = array;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "EmployeeTrackingReport();", true);
    }

    protected void lBtnNoOfOccurance_Command(object sender, CommandEventArgs e)
    {
        string empId = e.CommandName.ToString();
        string locId = e.CommandArgument.ToString();
        string fromdt = Session["FromDate"].ToString();
        string todt = Session["ToDate"].ToString();
        getExceptionOption();
        string l_value = txtLateEarlyFrom.Text;
        string u_value = txtlblLateEarlyTo.Text;
        string[] array = { empId, locId, fromdt, todt, ExceptionOptions, l_value, u_value,"yes" };
        Session["EmpDataExceptionCount"] = array;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "EmployeeTrackingReport();", true);
    }

    //private void FillDepartment()
    //{
    //    Depart = "";
    //    ddlDepartment.Items.Clear();
    //    DataTable dt = ObjEmployeeMaster.GetEmployeeOrDepartment("0", "0", "0", "0", "0");

    //    if (ddlLocation.SelectedValue != "0")
    //    {
    //        dt = new DataView(dt, "Location_Id = '" + ddlLocation.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
    //    }

    //    string DepIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D");

    //    if (!Common.GetStatus())
    //    {
    //        if (DepIds != "")
    //        {
    //            dt = new DataView(dt, "Dep_Id in(" + DepIds.Substring(0, DepIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
    //        }
    //    }

    //    if (dt.Rows.Count > 0)
    //    {

    //        ddlDepartment.Items.Add(new System.Web.UI.WebControls.ListItem ("All", "0"));
    //        for (int i = 0; i < dt.Rows.Count; i++)
    //        {
    //            ddlDepartment.Items.Add(new System.Web.UI.WebControls.ListItem(dt.Rows[i]["DeptName"].ToString(), dt.Rows[i]["Dep_Id"].ToString()));
    //            if(i== dt.Rows.Count-1)
    //            {
    //                Depart = Depart + dt.Rows[i]["Dep_Id"].ToString();
    //            }
    //            else
    //            {
    //                Depart = Depart + dt.Rows[i]["Dep_Id"].ToString() + ",";
    //            }


    //        }

    //    }

    //}

    //protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    fillLocation();
    //    setEmpListPopup();
    //}
    protected void lnkback_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Attendance_Report/AttendanceReport.aspx");
    }
}