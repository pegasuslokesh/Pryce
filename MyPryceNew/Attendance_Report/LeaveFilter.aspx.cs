using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PegasusDataAccess;

public partial class Attendance_Report_LeaveFilter : BasePage
{
    Common cmn = null;
    LocationMaster objLocation = null;
    LeaveMaster objleave = null;
    PageControlCommon objPageCmn = null;
    Att_Employee_Leave objEmpleave = null;
    Att_Leave_Request objleaveReq = null;
    HR_EmployeeDetail HR_EmployeeDetail = null;
    SystemParameter objSys = null;
    Set_ApplicationParameter objAppParam = null;
    DataAccessClass Objda = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        cmn = new Common(Session["DBConnection"].ToString());
        HR_EmployeeDetail = new HR_EmployeeDetail(Session["DBConnection"].ToString());
        objleaveReq = new Att_Leave_Request(Session["DBConnection"].ToString());
        objEmpleave = new Att_Employee_Leave(Session["DBConnection"].ToString());
        objLocation = new LocationMaster(Session["DBConnection"].ToString());
        objleave = new LeaveMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        Objda= new DataAccessClass(Session["DBConnection"].ToString());
        if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Allow Over Time Convert In Leaves", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())))
        {

            gvExportData.Columns[4].Visible = true;
        }
        else
        {
            gvExportData.Columns[4].Visible = false;
        
        }
        if (!IsPostBack)
        {
            Session["gvExportData"] = null;
            AllPageCode();
            fillLocation();
            FillLeaveType();
            FillYear(ddlYear);

            try
            {
                string year = DateTime.Now.Year.ToString();
                ddlYear.SelectedValue = year;
            }
            catch(Exception ex)
            {

            }
            ddlLocation.SelectedValue = Session["locId"].ToString();
        }
        if (Session["gvExportData"] != null)
        {
            try
            {   gvExportData.DataSource = Session["gvExportData"] as DataTable;
                gvExportData.DataBind();
            }
            catch
            {
            }
        }
    }

    private void FillYear(DropDownList ddl)
    {
        DataTable dt = new DataTable();
        dt = Objda.return_DataTable("SELECT DISTINCT YEAR(CreatedDate) AS Unique_Years FROM Att_Employee_Leave_Trans");
        if (dt.Rows.Count > 0)
        {
            objPageCmn.FillData((object)ddl, dt, "Unique_Years", "Unique_Years");
        }
    }
    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        DataTable dtModule = new DataTable();
        DataTable dtAllPageCode = new DataTable();
        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;
        if (Request.QueryString["Emp_Id"] == null)
        {
            dtModule = objObjectEntry.GetModuleIdAndName("56", (DataTable)Session["ModuleName"]);
        }
        else
        {
            dtModule = objObjectEntry.GetModuleIdAndName("52", (DataTable)Session["ModuleName"]);
        }
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
        Page.Title = objSys.GetSysTitle();
        if (Session["EmpId"].ToString() == "0")
        {
           
        }
        else
        {
            if (Request.QueryString["Emp_Id"] == null)
            {

                dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "56", HttpContext.Current.Session["CompId"].ToString());

            }
            else
            {
                dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "52", HttpContext.Current.Session["CompId"].ToString());

            }

            if (dtAllPageCode.Rows.Count == 0)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

            
        }
    }

    public void fillLocation()
    {
        DataTable dtLoc = objLocation.GetLocationMaster(Session["CompId"].ToString());

        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        string LocIds = "";
        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLoc.Rows.Count > 1)
                {
                    ddlLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", LocIds.Substring(0, LocIds.Length - 1)));
                }
            }
            else
            {
                ddlLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", Session["LocId"].ToString()));
            }
        }
        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.DataSource = dtLoc;
            ddlLocation.DataTextField = "Location_name";
            ddlLocation.DataValueField = "Location_Id";
            ddlLocation.DataBind();
            if (dtLoc.Rows.Count > 1 && LocIds != "")
            {
                ddlLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", LocIds.Substring(0, LocIds.Length - 1)));
            }
        }
        else
        {
            ddlLocation.Items.Clear();
        }      
        ddlLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
        dtLoc = null;
    }

    public void FillLeaveType()
    {
        //DataTable dt = new DataTable();
        //dt = objEmpleave.GetLeaveType();
        //ddlLeaveType.DataSource = dt;
        //ddlLeaveType.DataBind();
        //ddlLeaveType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Customer--", "0"));
    }

    protected void txtEmpName_textChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        string Emp_Name = txtEmpName.Text;
        int index = Emp_Name.IndexOf("/");
        if (index > 0)
            Emp_Name = Emp_Name.Substring(0, index);

        string empid = string.Empty;
        if (((TextBox)sender).ID.Trim() == "txtEmpName")
        {
            gvExportData.DataSource = null;
            gvExportData.DataBind();
        }

        if (((TextBox)sender).Text != "")
        {
            empid = ((TextBox)sender).Text.Split('/')[((TextBox)sender).Text.Split('/').Length - 1];
            string Emp_ID = HR_EmployeeDetail.GetEmployeeId(empid);
            if (Emp_ID == "0" || Emp_ID == "")
            {
                DisplayMessage("Employee not exists");
                txtEmpName.Focus();
                txtEmpName.Text = "";
                return;
            }

            DataTable dtEmp = objleaveReq.GetLeaveRequest_ByID(Session["CompId"].ToString(), Emp_ID);
            if (dtEmp.Rows.Count > 0)
            {
                dtEmp = new DataView(dtEmp, "Emp_Code='" + empid + "' and Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtEmp.Rows.Count > 0)
                {
                    empid = dtEmp.Rows[0]["Emp_Id"].ToString();

                    if (((TextBox)sender).ID.Trim() == "txtEmpName")
                    {
                        hdnEmpId.Value = empid;
                        //FillLeaveSummary(empid);                          
                    }
                }
                else
                {
                    DisplayMessage("Employee Not Exists");

                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Focus();
                    return;
                }




            }
        }

    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListEmployeeName(string prefixText, int count, string contextKey)
    {
        DataTable dt = Common.GetEmployee(prefixText, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());
        string[] str = new string[dt.Rows.Count];
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                str[i] = "" + dt.Rows[i][1].ToString() + "/(" + dt.Rows[i][2].ToString() + ")/" + dt.Rows[i][0].ToString() + "";
            }
           return str;
        }

        return str;
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    protected void btngo_Click(object sender, EventArgs e)
    { 

    }
    protected void btnFillgrid_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillLeaveSummary();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        hdnEmpId.Value = null;      
        txtEmpName.Text = "";
        ddlYear.SelectedValue = DateTime.Now.Year.ToString();
        Session["gvExportData"] = null;
        gvExportData.DataSource = null;
        gvExportData.DataBind();
    }
    protected void BtnExportPDF_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        try
        {
            DataTable dt = Session["gvExportData"] as DataTable;
            
            if (dt.Rows.Count > 0)
            {
                if (dt.Columns.Contains("Company_Id"))
                {
                    dt.Columns.Remove("Company_Id");
                    dt.Columns.Remove("Brand_Id");
                    dt.Columns.Remove("Location_Id");
                    dt.Columns.Remove("LeaveBalance");
                    dt.AcceptChanges();
                }
               
            }
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
            pdfDoc.AddHeader("Header", "Header Text");
            Font font13 = FontFactory.GetFont("ARIAL", 8);
            Font font18 = FontFactory.GetFont("ARIAL", 10);
            Font font21= FontFactory.GetFont("ARIAL", 21);
            try
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
                pdfDoc.Open();

                if (dt.Rows.Count > 0)
                {
                    PdfPTable PdfTable = new PdfPTable(1);
                    
                    PdfTable.TotalWidth = 800f; 
                    PdfTable.LockedWidth = true;
                    PdfPCell PdfPCell = new PdfPCell(new Phrase(new Chunk("Balance of Annual Leave", font21))){ HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
                    PdfPCell.Border = Rectangle.NO_BORDER;                    
                        PdfTable.AddCell(PdfPCell);
                    //DrawLine(writer, 25f, pdfDoc.Top - 30f, pdfDoc.PageSize.Width - 25f, pdfDoc.Top - 30f, new BaseColor(System.Drawing.Color.Red));                   
                    PdfTable.HorizontalAlignment = 0;
                    PdfTable.SpacingAfter = 10;                   
                    // STEP 2: Set the widths of the table columns                   
                    // STEP 3: Set the table width to not resize                   
                    pdfDoc.Add(PdfTable);

                    PdfTable = new PdfPTable(dt.Columns.Count);
                    PdfTable.SpacingBefore = 20f;
                    float[] columnWidths = new float[] { 280f, 380f, 350f, 450f, 350f, 300f, 300f, 350f };


                    PdfTable.SetWidths(columnWidths);
                    for (int columns = 0; columns <= dt.Columns.Count - 1; columns++)
                    {
                        PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Columns[columns].ColumnName, font18)));
                        PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                        PdfPCell.CellEvent = new LightBorderEvent();
                        PdfTable.AddCell(PdfPCell);
                    }

                    for (int rows = 0; rows <= dt.Rows.Count - 1; rows++)
                    {
                        for (int column = 0; column <= dt.Columns.Count - 1; column++)
                        {
                            PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), font13)));
                            PdfPCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                            PdfPCell.CellEvent = new LightBorderEvent();
                            PdfTable.AddCell(PdfPCell);
                        }
                    }
                    pdfDoc.Add(PdfTable);
                }
                pdfDoc.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=LeaveLocation_" + DateTime.Now.Date.Day.ToString() + DateTime.Now.Date.Month.ToString() + DateTime.Now.Date.Year.ToString() + DateTime.Now.Date.Hour.ToString() + DateTime.Now.Date.Minute.ToString() + DateTime.Now.Date.Second.ToString() + DateTime.Now.Date.Millisecond.ToString() + ".pdf");
                System.Web.HttpContext.Current.Response.Write(pdfDoc);
                Response.Flush();
                Response.End();
            }
            catch (DocumentException de)
            {
            }
            // System.Web.HttpContext.Current.Response.Write(de.Message)
            catch (IOException ioEx)
            {
            }
            // System.Web.HttpContext.Current.Response.Write(ioEx.Message)
            catch (Exception ex)
            {
            }
        }
        catch (Exception e1)
        {
            DisplayMessage(e1.ToString());
        }
    }

    public class LightBorderEvent : IPdfPCellEvent
    {
        public void CellLayout(PdfPCell cell, Rectangle position, PdfContentByte[] canvases)
        {
            PdfContentByte canvas = canvases[PdfPTable.LINECANVAS];

            // Set the border color and thickness
            canvas.SetColorStroke(BaseColor.LIGHT_GRAY);
            canvas.SetLineWidth(0.5f);

            // Draw the borders
            canvas.Rectangle(position.Left, position.Bottom, position.Width, position.Height);
            canvas.Stroke();
        }
    }
    protected void BtnExportExcel_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        try
        {
            DataTable dt = new DataTable();
            dt = Session["gvExportData"] as DataTable;

            if (gvExportData.Rows.Count > 0)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Customers");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=LeaveLocation" + DateTime.Now.ToString("ddmmyyyyHHMM") + ".xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            else
            {
                DisplayMessage("No Record Available");
            }
        }
        catch (Exception e1)
        {
            DisplayMessage(e1.ToString());
        }
    }
    public void FillLeaveSummary()
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        string Emp_Name = txtEmpName.Text;
        int index = Emp_Name.IndexOf("/");
        if (index > 0)
            Emp_Name = Emp_Name.Substring(0, index);
        string empName = string.Empty;
        empName = Emp_Name;
        string strQRL = string.Empty;

        if (ddlYear.SelectedIndex > 0 && ddlYear.SelectedValue != "--Select--" && ddlYear.SelectedValue != DateTime.Now.Year.ToString())
        {
            strQRL = "select EM.Company_Id, EM.Brand_Id, EM.Location_Id,EM.Emp_Code,EM.Emp_Name,(CONVERT(float, previous_days)) as CurrentBalance,(Sum(Cast(LT.Assign_Days as float)) * Sum(12) / (Sum(12))) as ThisYearAssignLeaves ,(Case when LT.OT_Leave is NULL then '0' else LT.OT_Leave End) as OT_Leave,(CONVERT(float, Used_Days) - CONVERT(float, Encash_Days)) as LeaveTaken, Encash_Days as EncashValue,((CONVERT(float, previous_days) + ((Case when LT.OT_Leave is null then '0' else LT.OT_Leave End)) +(Sum(Cast(LT.Assign_Days as float)) * Sum(12) / Sum(12))) -(Convert(float, used_Days))) as LeaveBalance,LT.Remaining_Days from Att_Employee_Leave_Trans as LT inner join Set_EmployeeMaster as EM on EM.Emp_Id = LT.Emp_Id  where LT.IsActive = 'True' and LT.Leave_Type_Id = '18'";
            if (ddlLocation.SelectedValue != null && ddlLocation.SelectedValue != "0")
            {
                strQRL += " And EM.Location_Id in (" + ddlLocation.SelectedValue + ")";
            }
            if (empName != null && empName != "")
            {
                strQRL += "And  EM.Emp_Id='" + hdnEmpId.Value + "'";
            }
            strQRL += "And Year='"+ddlYear.SelectedValue.ToString()+"' group by EM.Company_Id, EM.Brand_Id, EM.Location_Id, EM.Emp_Code, EM.Emp_Name,LT.Previous_Days,LT.Used_Days, LT.Encash_Days,LT.Remaining_Days,LT.OT_Leave";
        }
        else
        {
            strQRL = "select EM.Company_Id, EM.Brand_Id, EM.Location_Id,EM.Emp_Code,EM.Emp_Name,(CONVERT(float, previous_days)) as CurrentBalance,(Sum(Cast(LT.Assign_Days as float)) * Sum(MONTH(GetDate()) - 1) / (Sum(12))) as ThisYearAssignLeaves ,(Case when LT.OT_Leave is NULL then '0' else LT.OT_Leave End) as OT_Leave,(CONVERT(float, Used_Days) - CONVERT(float, Encash_Days)) as LeaveTaken, Encash_Days as EncashValue,((CONVERT(float, previous_days) + ((Case when LT.OT_Leave is null then '0' else LT.OT_Leave End)) +(Sum(Cast(LT.Assign_Days as float)) * Sum(MONTH(GetDate()) - 1) / Sum(12))) -(Convert(float, used_Days))) as LeaveBalance,LT.Remaining_Days from Att_Employee_Leave_Trans as LT inner join Set_EmployeeMaster as EM on EM.Emp_Id = LT.Emp_Id  where LT.IsActive = 'True' and LT.Leave_Type_Id = '18'";
            if (ddlLocation.SelectedValue != null && ddlLocation.SelectedValue != "0")
            {
                strQRL += " And EM.Location_Id in (" + ddlLocation.SelectedValue + ")";
            }
            if (empName != null && empName != "")
            {
                strQRL += "And  EM.Emp_Id='" + hdnEmpId.Value + "'";
            }
            strQRL += "And LT.Field3='Open' And  Year='"+DateTime.Now.Year.ToString()+"' group by EM.Company_Id, EM.Brand_Id, EM.Location_Id, EM.Emp_Code, EM.Emp_Name,LT.Previous_Days,LT.Used_Days, LT.Encash_Days,LT.Remaining_Days,LT.OT_Leave";
        }            
        DataTable dtLeaveSummary = objEmpleave.GetEmployeeLeaveTransactionAll(strQRL);
        Session["gvExportData"] = dtLeaveSummary;
        gvExportData.DataSource = dtLeaveSummary;
        lblTotalRecords.Text = "Total Records:" + dtLeaveSummary.Rows.Count.ToString() + "";      
        gvExportData.DataBind();
    }
}