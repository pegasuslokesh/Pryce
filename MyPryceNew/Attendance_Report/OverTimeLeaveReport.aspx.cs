using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class Attendance_Report_OverTimeLeaveReport : BasePage
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
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), Request.QueryString["Emp_Id"] == null ? "621" : "621", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }

            Session["gvExportOTLeaves"] = null;
            fillLocation();
            AllPageCode();
            ddlLocation.SelectedValue = Session["locId"].ToString();
        }
        if (Session["gvExportOTLeaves"] != null)
        {
            try
            {
                gvExportData.DataSource = Session["gvExportOTLeaves"] as DataTable;
                gvExportData.DataBind();
            }
            catch
            {
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
    public string GetDate(object obj)
    {

        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());



        return Date.ToString(objSys.SetDateFormat());

    }
    protected void FillLeaveSummary()
    {
        if (txtEmpName.Text == "")
        {
            DataTable dtOTLeaveSummary = objleaveReq.GetOTLeaveSummaryForAll(HttpContext.Current.Session["CompId"].ToString());
            if (dtOTLeaveSummary.Rows.Count > 0)
            {
                Session["gvExportOTLeaves"] = dtOTLeaveSummary;
                lblTotalRecords.Text = "Total Records:" + dtOTLeaveSummary.Rows.Count.ToString() + "";
                objPageCmn.FillData((object)gvExportData, dtOTLeaveSummary, "", "");

            }
        }
        else
        {
            DataTable dtOTLeaveSummary = objleaveReq.GetOTLeaveSummary(hdnEmpId.Value, HttpContext.Current.Session["CompId"].ToString());
            if (dtOTLeaveSummary.Rows.Count > 0)
            {
                Session["gvExportOTLeaves"] = dtOTLeaveSummary;
                lblTotalRecords.Text = "Total Records:" + dtOTLeaveSummary.Rows.Count.ToString() + "";
                objPageCmn.FillData((object)gvExportData, dtOTLeaveSummary, "", "");
            }

        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        hdnEmpId.Value = null;
        txtEmpName.Text = "";
        Session["gvExportOTLeaves"] = null;
        gvExportData.DataSource = null;
        gvExportData.DataBind();
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
            dtModule = objObjectEntry.GetModuleIdAndName("621", (DataTable)Session["ModuleName"]);
        }
        else
        {
            dtModule = objObjectEntry.GetModuleIdAndName("621", (DataTable)Session["ModuleName"]);
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

                dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "621", HttpContext.Current.Session["CompId"].ToString());

            }
            else
            {
                dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "621", HttpContext.Current.Session["CompId"].ToString());

            }

            if (dtAllPageCode.Rows.Count == 0)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }


        }
    }
    protected void BtnExportPDF_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        try
        {
            DataTable dt = Session["gvExportOTLeaves"] as DataTable;
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
            pdfDoc.AddHeader("Header", "Header Text");
            Font font13 = FontFactory.GetFont("ARIAL", 11);
            Font font18 = FontFactory.GetFont("ARIAL", 12);
            Font font21 = FontFactory.GetFont("ARIAL", 21);
            try
            {
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
                pdfDoc.Open();

                if (dt.Rows.Count > 0)
                {
                    PdfPTable PdfTable = new PdfPTable(1);

                    PdfTable.TotalWidth = 700f;
                    PdfTable.LockedWidth = true;
                    PdfPCell PdfPCell = new PdfPCell(new Phrase(new Chunk("Balance of OverTime Leave", font21))) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
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
                    for (int columns = 0; columns <= dt.Columns.Count - 1; columns++)
                    {
                        PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Columns[columns].ColumnName, font18)));
                        PdfTable.AddCell(PdfPCell);
                    }

                    for (int rows = 0; rows <= dt.Rows.Count - 1; rows++)
                    {
                        for (int column = 0; column <= dt.Columns.Count - 1; column++)
                        {
                            PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows][column].ToString(), font13)));
                            PdfTable.AddCell(PdfPCell);
                        }
                    }
                    pdfDoc.Add(PdfTable);
                }
                pdfDoc.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment; filename=OverTime_" + DateTime.Now.Date.Day.ToString() + DateTime.Now.Date.Month.ToString() + DateTime.Now.Date.Year.ToString() + DateTime.Now.Date.Hour.ToString() + DateTime.Now.Date.Minute.ToString() + DateTime.Now.Date.Second.ToString() + DateTime.Now.Date.Millisecond.ToString() + ".pdf");
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
    protected void BtnExportExcel_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        try
        {
            DataTable dt = new DataTable();
            dt = Session["gvExportOTLeaves"] as DataTable;

            if (gvExportData.Rows.Count > 0)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Customers");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=OverTimeLeave" + DateTime.Now.ToString("ddmmyyyyHHMM") + ".xlsx");
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
}