using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class Attendance_Report_EmployeeLeaveMovement : System.Web.UI.Page
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
        fillLocation();
        ddlLocation.SelectedValue = Session["locId"].ToString();
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
            gvEmpLeaves.DataSource = null;
            gvEmpLeaves.DataBind();
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
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }

    protected void btnFillgrid_Click(object sender, EventArgs e)
    {
        if (txtEmpName.Text =="")
        {
            DisplayMessage("Please Enter Employee Name");
            return;
        }
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillLeaveSummary();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        hdnEmpId.Value = null;
        txtEmpName.Text = "";
        Session["gvEmpLeaves"] = null;
        gvEmpLeaves.DataSource = null;
        gvEmpLeaves.DataBind();
        lblTotalRecords.Text = "Total Record:0";
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
    protected void BtnExportPDF_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        try
        {
            DataTable dt = Session["gvEmpLeaves"] as DataTable;
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
                    PdfPCell PdfPCell = new PdfPCell(new Phrase(new Chunk("Balance of Anual Leave", font21))) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER };
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
                Response.AddHeader("content-disposition", "attachment; filename=EmployeeLeaveMovement" + DateTime.Now.Date.Day.ToString() + DateTime.Now.Date.Month.ToString() + DateTime.Now.Date.Year.ToString() + DateTime.Now.Date.Hour.ToString() + DateTime.Now.Date.Minute.ToString() + DateTime.Now.Date.Second.ToString() + DateTime.Now.Date.Millisecond.ToString() + ".pdf");
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
            dt = Session["gvEmpLeaves"] as DataTable;

            if (gvEmpLeaves.Rows.Count > 0)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Customers");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=EmployeeLeaveMovement" + DateTime.Now.ToString("ddmmyyyyHHMM") + ".xlsx");
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
    public void FillLeaveSummary()
    {
        DataTable dt = new DataTable();
        if (hdnEmpId.Value != "")
        {
            dt = objleaveReq.GetEmplolyeeLeaveMovement(hdnEmpId.Value);

            string MonthNumber = Convert.ToString(int.Parse(DateTime.Now.Month.ToString()) - 1);
            if (MonthNumber != "0")
            {
                dt = new DataView(dt,"MonthNumber<='" + MonthNumber + "'", "", DataViewRowState.CurrentRows).ToTable();

            }            
            if (dt.Rows.Count > 0)
            {
                Session["gvEmpLeaves"] = dt;
                gvEmpLeaves.DataSource = dt;
                lblTotalRecords.Text = "Total Records:" + dt.Rows.Count.ToString() + "";
                // gvEmpLeaves.AutoGenerateColumns = true;
                gvEmpLeaves.DataBind();
            }
        }       
    }
    protected void gvEmpLeaves_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            try {

             
                

            }
            catch(Exception ex)
            {

            }
            }
    }

    protected void gvEmpLeaves_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            // Create the footer row
            GridView gv = (GridView)sender;
            GridViewRow footerRow = new GridViewRow(0, 0, DataControlRowType.Footer, DataControlRowState.Insert);


            // Initialize variables to hold the totals
            double totalThisYearAssignLeaves = 0;
            int totalLeaveBalance = 0;
            int totalLeaveEncashment = 0;
            int totalOTLeave = 0;

            // Loop through the GridView rows
            DataTable dt = Session["gvEmpLeaves"] as DataTable;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                totalThisYearAssignLeaves += Convert.ToDouble(dt.Rows[i]["ThisYearAssignLeaves"].ToString());
                totalLeaveBalance += Convert.ToInt32(dt.Rows[i]["Leave_Taken"].ToString());
                totalLeaveEncashment += Convert.ToInt32(dt.Rows[i]["Leave_Encashment"].ToString());
                totalOTLeave += Convert.ToInt32(dt.Rows[i]["OT_Leave"].ToString());
            }




            // Create the cells for the footer row
            TableCell totalLabelCell = new TableCell();
            totalLabelCell.Text = "Total:";
            totalLabelCell.Font.Bold = true;

            TableCell totalThisYearAssignLeavesCell = new TableCell();
            Label totalThisYearAssignLeavesLabel = new Label();
            totalThisYearAssignLeavesLabel.ID = "TotalThisYearAssignLeaves";
            totalThisYearAssignLeavesLabel.Text = totalThisYearAssignLeaves.ToString();
            totalThisYearAssignLeavesCell.Controls.Add(totalThisYearAssignLeavesLabel);
            totalThisYearAssignLeavesLabel.Font.Bold = true;


            TableCell totalLeaveTakenCell = new TableCell();
            Label totalLeaveTakenLabel = new Label();
            totalLeaveTakenLabel.ID = "TotalLeaveTaken";
            totalLeaveTakenLabel.Text = totalLeaveBalance.ToString();
            totalLeaveTakenLabel.Font.Bold = true;
            totalLeaveTakenCell.Controls.Add(totalLeaveTakenLabel);
            totalLeaveTakenLabel.Font.Bold = true;

            TableCell totalLeaveEncashmentCell = new TableCell();
            Label totalLeaveEncashmentLabel = new Label();
            totalLeaveEncashmentLabel.ID = "TotalLeaveEncashment";
            totalLeaveEncashmentLabel.Text = totalLeaveEncashment.ToString();
            totalLeaveEncashmentCell.Controls.Add(totalLeaveEncashmentLabel);
            totalLeaveEncashmentLabel.Font.Bold = true;

            TableCell totalOTLeaveCell = new TableCell();
            Label totalOTLeaveLabel = new Label();
            totalOTLeaveLabel.ID = "TotalOTLeave";
            totalOTLeaveLabel.Text = totalOTLeave.ToString(); ;
            totalOTLeaveCell.Controls.Add(totalOTLeaveLabel);
            totalOTLeaveLabel.Font.Bold = true;
            

            // Add the cells to the footer row
            footerRow.Cells.Add(totalLabelCell);
            footerRow.Cells.Add(totalThisYearAssignLeavesCell);
            footerRow.Cells.Add(totalLeaveTakenCell);
            footerRow.Cells.Add(totalLeaveEncashmentCell);
            footerRow.Cells.Add(totalOTLeaveCell);

            footerRow.CssClass = "bold-text";
            // Add the footer row to the GridView
            gv.Controls[0].Controls.Add(footerRow);
        }
    }



}