using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DevExpress.XtraReports.UI;
using System.Configuration;
using System.IO;
using PegasusDataAccess;

public partial class CRM_EmployeeWorkHistory : System.Web.UI.Page
{
    FollowUp objFollowup = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter objSys = null;
    Attendance objAttendance = null;
    Set_AddressChild ObjAddress = null;
    Common cmn = null;
    IT_ObjectEntry objObjectEntry = null;
    EmployeeMaster ObjEmployeeMaster = null;
    UserMaster objUser = null;
    DataAccessClass ObjDa = null;
    PageControlCommon objPageCmn = null;

    public static string location_id, locationCondition, Depart;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        txtFromDt.Attributes.Add("readonly", "true");
        txtTodt.Attributes.Add("readonly", "true");
        var now = DateTime.Now;
        var first = new DateTime(now.Year, now.Month, 1);
        var last = first.AddMonths(1).AddDays(-1);

        objFollowup = new FollowUp(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objAttendance = new Attendance(Session["DBConnection"].ToString());
        ObjAddress = new Set_AddressChild(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        ObjEmployeeMaster = new EmployeeMaster(Session["DBConnection"].ToString());
        objUser = new UserMaster(Session["DBConnection"].ToString());
        ObjDa = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            txtFromDt.Text = first.ToString("dd-MMM-yyyy");
            txtTodt.Text = last.ToString("dd-MMM-yyyy");

            fillLocation();
            ddlLocation.SelectedValue = Session["LocId"].ToString();
            FillDepartment();
            fillUser();
            AllPageCode();
        }
  
        fillGrid();
    }

    private void fillGrid()
    {
        GvFollowupData.PageIndex = 0;

        location_id = ddlLocation.SelectedValue.ToString();


        string tableName = "";
        if (ddlReferenceType.SelectedValue != "All")
        {
            tableName = ddlReferenceType.SelectedValue;
        }


        DataTable dt_Data = objFollowup.getAllFollowupDate(Session["CompId"].ToString(), location_id, txtFromDt.Text, txtTodt.Text, tableName);
 
        if (ddlDepartment.SelectedIndex != 0)
        {
            dt_Data = new DataView(dt_Data, "Department_id='" + ddlDepartment.SelectedValue.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        if (ddlEmployee.SelectedIndex != 0)
        {
            dt_Data = new DataView(dt_Data, "Created_by_name='" + ddlEmployee.SelectedItem.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        

        GvFollowupData.DataSource = dt_Data;
        GvFollowupData.DataBind();
        //lblTotalRecords.Text = "Total Records:" + dt_followupData.Rows.Count;
        Session["SessionFollowupData"] = dt_Data;
    }

    public void fillLocation()
    {
        location_id = "";
        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        locationCondition = "Location_Id=";

        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        if (!Common.GetStatus(Session["EmpId"].ToString()))
        {
            string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        if (dtLoc.Rows.Count > 0)
        {

            for (int i = 0; i < dtLoc.Rows.Count; i++)
            {
                ddlLocation.Items.Add(new System.Web.UI.WebControls.ListItem(dtLoc.Rows[i]["Location_Name"].ToString(), dtLoc.Rows[i]["Location_Id"].ToString()));

                if (i == dtLoc.Rows.Count - 1)
                {
                    location_id = location_id + dtLoc.Rows[i]["Location_Id"].ToString();
                    locationCondition = locationCondition + dtLoc.Rows[i]["Location_Id"].ToString();
                }
                else
                {
                    location_id = location_id + dtLoc.Rows[i]["Location_Id"].ToString() + ",";
                    locationCondition = locationCondition + dtLoc.Rows[i]["Location_Id"].ToString() + " or Location_Id=";

                }
            }
            ddlLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", location_id));
        }
        else
        {
            ddlLocation.Items.Clear();
        }
    }


    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "FieldChange()", true);
        ddlReferenceType_SelectedIndexChanged(sender, e);
        //fillGrid();
    }

    protected void GvFollowupData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "FieldChange()", true);
        GvFollowupData.PageIndex = e.NewPageIndex;
        DataTable dtPaging = (DataTable)Session["SessionFollowupData"];
        objPageCmn.FillData((object)GvFollowupData, dtPaging, "", "");
    }

    protected void GvFollowupData_Sorting(object sender, GridViewSortEventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "FieldChange()", true);
        DataTable dt_Sorting = (DataTable)Session["SessionFollowupData"];
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

        dt_Sorting = (new DataView(dt_Sorting, "", e.SortExpression + " " + ViewState["SortDir"].ToString(), DataViewRowState.CurrentRows)).ToTable();
        Session["SessionFollowupData"] = dt_Sorting;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)GvFollowupData, dt_Sorting, "", "");

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

    //protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    //{
    //    ddlFieldName.SelectedIndex = 0;
    //    ddlReferenceType.SelectedIndex = 0;
    //    txtValue.Text = "";
    //    txtValueDate.Text = "";
    //    fillGrid();
    //}

    //protected void btnbind_Click(object sender, ImageClickEventArgs e)
    //{
    //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "FieldChange()", true);
    //    fillGrid();
    //    DataTable dtAdd = Session["SessionFollowupData"] as DataTable;

    //    if (ddlOption.SelectedIndex != 0)
    //    {
    //        string condition = string.Empty;

    //        if (ddlFieldName.SelectedItem.Value == "Created_date")
    //        {
    //            if (txtValueDate.Text.Trim() != "")
    //            {
    //                if (ddlOption.SelectedIndex == 1)
    //                {
    //                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + Convert.ToDateTime(txtValueDate.Text.Trim()) + "'";
    //                }
    //                else if (ddlOption.SelectedIndex == 2)
    //                {
    //                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + Convert.ToDateTime(txtValueDate.Text.Trim()) + "%'";
    //                }
    //                else
    //                {
    //                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '%" + Convert.ToDateTime(txtValueDate.Text.Trim()) + "%'";
    //                }
    //            }
    //            else
    //            {
    //                DisplayMessage("Enter Date");
    //                txtValueDate.Focus();
    //                return;
    //            }
    //        }
    //        else
    //        {
    //            if (txtValue.Text.Trim() != "")
    //            {
    //                if (ddlOption.SelectedIndex == 1)
    //                {
    //                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
    //                }
    //                else if (ddlOption.SelectedIndex == 2)
    //                {
    //                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
    //                }
    //                else
    //                {
    //                    condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
    //                }
    //            }
    //            else
    //            {
    //                DisplayMessage("Enter Some Value");
    //                txtValueDate.Focus();
    //            }
    //        }
    //        DataView view = new DataView(dtAdd, condition, "", DataViewRowState.CurrentRows);
    //        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
    //        cmn.FillData((object)GvFollowupData, view.ToTable(), "", "");
    //        Session["SessionFollowupData"] = view.ToTable();
    //        lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count + "";
    //    }
    //}

    public void DisplayMessage(string str,string color="orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
             ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','"+color+"','white');", true);
        }
    }
    public string GetArebicMessage(string EnglishMessage)
    {
        string ArebicMessage = string.Empty;
        DataTable dtres = (DataTable)ViewState["MessageDt"];
        if (dtres.Rows.Count != 0)
        {
            ArebicMessage = (new DataView(dtres, "Key='" + EnglishMessage + "'", "", DataViewRowState.CurrentRows).ToTable()).Rows[0]["Value"].ToString();
        }
        if (ArebicMessage == "")
        {
            ArebicMessage = EnglishMessage;
        }
        return ArebicMessage;
    }


    protected void lblRef_table_name_filtered_Command(object sender, CommandEventArgs e)
    {
        string name = e.CommandName.ToString();
        string id = e.CommandArgument.ToString();
        if (name == "Opportunity")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "RedirectOpportunity('" + id + "');", true);
        }
        if (name == "Quotation")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "RedirectQuotation('" + id + "');", true);
            //Response.Redirect("window.open('../Sales/SalesQuotation.aspx?FollowupID='" + id + "'','window','width=1024');");
        }

    }

    protected void ddlReferenceType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "FieldChange()", true);

        DropDownList ddl = (DropDownList)sender;

        if(ddl.ID != "ddlReferenceType")
        {
            ddlDepartment.Items.Clear();
            ddlEmployee.Items.Clear();
            FillDepartment();
            fillUser();
        }

        fillGrid();
    }

    public string getamt(string amt)
    {
        if (amt == "")
        {
            return "";
        }

        try
        {
            int x = Convert.ToInt32(amt);

            if (x == 0)
            {
                return "";
            }
            else
            {
                return amt;
            }
        }
        catch
        {
            return "";
        }
    }

    protected void GvFollowupData_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
    {
        fillGrid();
    }

    protected void ExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            ASPxGridViewExporter1.Landscape = true;
            ASPxGridViewExporter1.WriteCsvToResponse();
        }
        catch (Exception e1)
        {
            //SendTextToFile(e1.Message);
        }
    }

    protected void ExportPDF_Click(object sender, EventArgs e)
    {
        try
        {
            ASPxGridViewExporter1.Landscape = true;
            ASPxGridViewExporter1.WritePdfToResponse();
        }
        catch (Exception e1)
        {
            //SendTextToFile(e1.Message);
        }
    }

    protected void EmportReportPDF_Click(object sender, EventArgs e)
    {
        GridView gv = new GridView();
        XtraReport RptShift = new XtraReport();

        DataTable dtFilter = new DataTable();
        AttendanceDataSet rptdata = new AttendanceDataSet();
        rptdata.EnforceConstraints = false;
        AttendanceDataSetTableAdapters.sp_EmployeeWorkHistoryTableAdapter adp = new AttendanceDataSetTableAdapters.sp_EmployeeWorkHistoryTableAdapter();

        //adp.Fill(rptdata.sp_Att_ExceptionCountReport1 ,Convert.ToDateTime(txtFromDate.Text),Convert.ToDateTime(txtToDate.Text), LocationList, EmpList, ExceptionOptions, Convert.ToInt16(txtNoOfOccurance.Text), Convert.ToInt16(txtLateEarlyFrom.Text), Convert.ToInt16(txtlblLateEarlyTo.Text));
        adp.Fill(rptdata.sp_EmployeeWorkHistory, ddlLocation.SelectedValue.ToString(), Convert.ToDateTime(txtFromDt.Text), Convert.ToDateTime(txtTodt.Text));

        DataTable dt = rptdata.sp_EmployeeWorkHistory;

        dt.Columns["Emp_Code"].ColumnName = "Emp Code";
        dt.Columns["Created_by_name"].ColumnName = "Created By";
        dt.Columns["t_Campaign"].ColumnName = "Total Campaign";
        dt.Columns["t_Opportunity"].ColumnName = "Total Opportunity";
        dt.Columns["t_Followup"].ColumnName = "Total Followup";
        dt.Columns["t_Call"].ColumnName = "Total Call";
        dt.Columns["t_Visit"].ColumnName = "Total Visit";
        dt.Columns["t_Quote"].ColumnName = "Total Quote";
        dt.Columns["t_Order"].ColumnName = "Total Order";
        dt.Columns["t_Invoice"].ColumnName = "Total Invoice";
        //dt.Columns["location_name"].ColumnName = "Location";

        if(ddlDepartment.SelectedIndex!=0)
        {
            dt = new DataView(dt, "Department_Id='" + ddlDepartment.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (ddlEmployee.SelectedIndex != 0)
        {
            dt = new DataView(dt, "Emp_Id='" + ddlEmployee.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        dt.Columns.Remove("Emp_id");
        dt.Columns.Remove("Location_Id");
        dt.Columns.Remove("location_name");
        dt.Columns.Remove("Department_Id");


        gv.DataSource = dt;
        gv.AllowPaging = false;
        gv.DataBind();


        //try
        //{

        //    HttpContext.Current.Response.ContentType = "application/pdf";
        //    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=EmployeeWorkHistort.pdf");
        //    HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    StringWriter sw = new StringWriter();
        //    HtmlTextWriter hw = new HtmlTextWriter(sw);
        //    gv.RenderControl(hw);
        //    StringReader sr = new StringReader(sw.ToString());
        //    Document pdfDoc = new Document(PageSize.A3.Rotate(), 5f, 5f, 5f, 5f);

        //    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //    PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
        //    pdfDoc.Open();
        //    htmlparser.Parse(sr);
        //    pdfDoc.Close();
        //    HttpContext.Current.Response.Write(pdfDoc);
        //    HttpContext.Current.Response.End();

        //}
        //catch
        //{

        //}

        try
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;

            HttpContext.Current.Response.AddHeader("content-disposition",
            "attachment;filename=EmployeeWorkHistory.xls");
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            for (int i = 0; i < gv.Rows.Count; i++)
            {
                GridViewRow row = gv.Rows[i];
                row.BackColor = System.Drawing.Color.White;
                row.Attributes.Add("class", "textmode");

            }
            gv.RenderControl(hw);
            HttpContext.Current.Response.Output.Write(sw.ToString());
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
        catch
        { }
        //ExportXtraReport();
    }
    private void ExportXtraReport()
    {
        XtraReport RptShift = new XtraReport();

        RptShift.LoadLayout(ConfigurationManager.AppSettings["ReportPath"].ToString() + "EmployeeWorkReport.repx");
        DataTable dtFilter = new DataTable();
        AttendanceDataSet rptdata = new AttendanceDataSet();
        rptdata.EnforceConstraints = false;
        AttendanceDataSetTableAdapters.sp_EmployeeWorkHistoryTableAdapter adp = new AttendanceDataSetTableAdapters.sp_EmployeeWorkHistoryTableAdapter();

        //adp.Fill(rptdata.sp_Att_ExceptionCountReport1 ,Convert.ToDateTime(txtFromDate.Text),Convert.ToDateTime(txtToDate.Text), LocationList, EmpList, ExceptionOptions, Convert.ToInt16(txtNoOfOccurance.Text), Convert.ToInt16(txtLateEarlyFrom.Text), Convert.ToInt16(txtlblLateEarlyTo.Text));
        adp.Fill(rptdata.sp_EmployeeWorkHistory, ddlLocation.SelectedValue.ToString(), Convert.ToDateTime(txtFromDt.Text), Convert.ToDateTime(txtTodt.Text));

        string CompanyName = "";
        string Imageurl = "";
        string BrandName = "";

        CompanyName = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "1");
        // Image Url
        Imageurl = objAttendance.GetCompanyName(Session["CompId"].ToString(), Session["Lang"].ToString(), "2");
        // Get Brand Name
        BrandName = objAttendance.GetBrandName(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["Lang"].ToString());


        XRPictureBox xrPictureBox1 = (XRPictureBox)RptShift.FindControl("xrPictureBox1", true);
        try
        {
            xrPictureBox1.ImageUrl = Imageurl;
        }
        catch
        {

        }
        string dateT = System.DateTime.Now.ToString("hhmmyyss");
        XRLabel xrTitle = (XRLabel)RptShift.FindControl("xrTitle", true);
        //xrTitle.Text = getReportName()+ " From Date : "+txtFromDate.Text  +" To Date : "+ txtToDate.Text ;
        xrTitle.Text = "Employee Work History Report";
        XRLabel xrCompName = (XRLabel)RptShift.FindControl("xrCompName", true);
        xrCompName.Text = CompanyName;
        XRLabel rptBrand = (XRLabel)RptShift.FindControl("rptBrand", true);
        rptBrand.Text = BrandName;

        try
        {
            DataTable empDt = new EmployeeMaster(Session["DBConnection"].ToString()).GetEmployeeMasterById(Session["CompId"].ToString(), Session["EmpId"].ToString());
            XRLabel xrLabel10 = (XRLabel)RptShift.FindControl("xrLabel10", true);
            xrLabel10.Text = empDt.Rows[0]["Emp_Name"].ToString();
        }
        catch
        {

        }

        RptShift.DataSource = rptdata.sp_EmployeeWorkHistory;
        RptShift.DataMember = "sp_EmployeeWorkHistory";
        RptShift.CreateDocument(true);

        string rptname = "Employee Work History Report" + dateT;

        RptShift.ExportToPdf(@"" + ConfigurationManager.AppSettings["ReportPath"].ToString() + rptname + ".pdf");
        rptname = rptname + ".pdf";
        Response.ContentType = "application/pdf";

        DisplayMessage("Exported Successfully at " + ConfigurationManager.AppSettings["ReportPath"].ToString() + rptname);
        //Response.Redirect(ConfigurationManager.AppSettings["ReportPath"].ToString() + rptname);


        Response.AppendHeader("Content-Disposition", "attachment; filename=" + rptname);
        Response.TransmitFile(ConfigurationManager.AppSettings["ReportPath"].ToString() + rptname);
        Response.End();

    }

    public void AllPageCode()
    {
        ExportExcel.Visible = false;
        ExportPDF.Visible = false;
        EmportReportPDF.Visible = false;

        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        Common ObjComman = new Common(Session["DBConnection"].ToString());

        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;
        DataTable dtModule = objObjectEntry.GetModuleIdAndName(Common.GetObjectIdbyPageURL("../CRM/EmployeeWorkHistory.aspx", Session["DBConnection"].ToString()), (DataTable)Session["ModuleName"]);
        if (dtModule.Rows.Count > 0)
        {
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
            strModuleName = dtModule.Rows[0]["Module_Name"].ToString();
        }
        else
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }
        //End Code

        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;

        if (Session["EmpId"].ToString() == "0")
        {
            ExportExcel.Visible = true;
            ExportPDF.Visible = true; 
            EmportReportPDF.Visible = true;
        }

        DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, Common.GetObjectIdbyPageURL("../CRM/EmployeeWorkHistory.aspx", Session["DBConnection"].ToString()),Session["CompId"].ToString());

        foreach (DataRow DtRow in dtAllPageCode.Rows)
        {
            if (DtRow["Op_Id"].ToString() == "7")
            {
                ExportExcel.Visible = true;
                ExportPDF.Visible = true;
                EmportReportPDF.Visible = true;
            }
        }
    }

    public void fillUser()
    {
        if (locationCondition == "")
        {
            fillLocation();
            FillDepartment();
        }
        ddlEmployee.Items.Clear();

        string locId = "";
        if (ddlLocation.SelectedValue.Length >= 3)
        {
            locId = "0";
        }
        else
        {
            locId = ddlLocation.SelectedValue;
        }
        objPageCmn.FillUser(Session["CompId"].ToString(), "SUPERADMIN", ddlEmployee, objObjectEntry.GetModuleIdAndName("54", (DataTable)Session["ModuleName"]).Rows[0]["Module_Id"].ToString(), "54", locId, locationCondition);
    }

    public string getEmpList(string strBrandId, string strLocationId, string strdepartmentId)
    {
        string strEmpList = "0";

        DataTable dtEmp = ObjDa.return_DataTable(" SELECt STUFF((SELECT DISTINCT ',' + RTRIM(Set_EmployeeMaster.Emp_Id) FROM Set_EmployeeMaster where Brand_Id=" + strBrandId + " and  location_id =" + strLocationId + " and Department_Id in (" + strdepartmentId.Substring(0, strdepartmentId.Length - 1) + ") and IsActive='True' FOR xml PATH ('')), 1, 1, '')");

        if (dtEmp.Rows[0][0] != null)
        {
            strEmpList = dtEmp.Rows[0][0].ToString();
        }

        if (strEmpList == "")
        {
            strEmpList = "0";
        }
        return strEmpList;

    }

    public void FillEmployee(string strDeptId)
    {
        string strEmpId = string.Empty;
        string strLocationDept = string.Empty;
        string strLocId = Session["LocId"].ToString();

        if (ddlLocation.SelectedIndex > 0)
        {
            strLocId = ddlLocation.SelectedValue;

            strLocationDept = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

            if (strLocationDept == "")
            {
                strLocationDept = "0,";
            }

            strEmpId += getEmpList(Session["BrandId"].ToString(), strLocId, strLocationDept) + ",";
        }
        else
        {
            foreach (System.Web.UI.WebControls.ListItem li in ddlLocation.Items)
            {
                if (li.Value.Length >= 3)
                {
                    continue;
                }

                strLocId = li.Value;

                strLocationDept = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

                if (strLocationDept == "")
                {
                    strLocationDept = "0,";
                }

                strEmpId += getEmpList(Session["BrandId"].ToString(), strLocId, strLocationDept) + ",";

            }
        }

        DataTable dtEmp = ObjEmployeeMaster.GetEmployeeMasterOnRole(Session["CompId"].ToString());



        // Nitin Jasin , Get According to UserId to Get Records for Single User 
        DataTable dt = objUser.GetUserMasterByUserId(Session["UserId"].ToString(), "");
        bool IsSingleUser = false;
        try
        {
            IsSingleUser = Convert.ToBoolean(dt.Rows[0]["Field6"].ToString());
        }
        catch
        {
            IsSingleUser = false;
        }



        if (IsSingleUser == false)
        {


            dtEmp = new DataView(dtEmp, "Emp_Id in (" + strEmpId.Substring(0, strEmpId.ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

            if (strDeptId != "" && strDeptId != "0")
            {
                dtEmp = new DataView(dtEmp, "Department_Id ='" + strDeptId + "'", "", DataViewRowState.CurrentRows).ToTable();
            }

            ddlEmployee.DataSource = dtEmp;
            ddlEmployee.DataTextField = "Emp_Name";
            ddlEmployee.DataValueField = "Emp_Id";
            ddlEmployee.DataBind();

        }
        else
        {
            dt = new DataView(dtEmp, "Emp_Id='" + dt.Rows[0]["Emp_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            ddlEmployee.DataSource = dtEmp;
            ddlEmployee.DataTextField = "Emp_Name";
            ddlEmployee.DataValueField = "Emp_Id";
        }

        if (ddlDepartment.SelectedIndex != 0)
        {
            ddlEmployee.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select User--", "0"));
        }
    }

    public void FillDepartment()
    {

        ddlDepartment.Items.Clear();
        DataTable dt = ObjEmployeeMaster.GetEmployeeOrDepartment("0", "0", "0", "0", "0");

        if (ddlLocation.SelectedIndex != 0)
        {
            dt = new DataView(dt, "Location_Id = '" + ddlLocation.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            dt = dt.DefaultView.ToTable(false, "Dep_Id", "DeptName");
            DataView view = new DataView(dt);
            dt = view.ToTable(true, "Dep_Id", "DeptName");
        }

        string DepIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D",Session["LocId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(Session["EmpId"].ToString()))
        {
            if (DepIds != "")
            {
                dt = new DataView(dt, "Dep_Id in(" + DepIds.Substring(0, DepIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        if (dt.Rows.Count > 0)
        {

            ddlDepartment.Items.Add(new System.Web.UI.WebControls.ListItem("All", "0"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddlDepartment.Items.Add(new System.Web.UI.WebControls.ListItem(dt.Rows[i]["DeptName"].ToString(), dt.Rows[i]["Dep_Id"].ToString()));
            }
        }

    }


    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDepartment.SelectedIndex == 0)
        {
            fillUser();
        }
        else
        {
            FillEmployee(ddlDepartment.SelectedValue);
        }

    }

    protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillGrid();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        ddlLocation.Items.Clear();
        ddlDepartment.Items.Clear();
        ddlEmployee.Items.Clear();
        fillLocation();
        FillDepartment();
        fillUser();
        ddlLocation.SelectedValue = Session["LocId"].ToString();
        fillGrid();
    }


}