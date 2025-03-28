using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using System.IO;

public partial class Sales_Report_SalesFollowupReport : System.Web.UI.Page
{
    EmployeeMaster objEmployee = null;
    SalesFollowupPrint objFollowupPrint = null;
    PurchaseRequestHeader InvPr = null;
    InventoryDataSet ObjInvdataset = new InventoryDataSet();
    CompanyMaster Objcompany = null;
    Set_AddressChild Objaddress = null;
    BrandMaster ObjBrandMaster = null;
    LocationMaster ObjLocationMaster = null;
    SystemParameter ObjSysParam = null;
    Common cmn = null;
    string strCompId = string.Empty;
    string strBrandId = string.Empty;
    string strLocationId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        objEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        objFollowupPrint = new SalesFollowupPrint(Session["DBConnection"].ToString());
        InvPr = new PurchaseRequestHeader(Session["DBConnection"].ToString());
        Objcompany = new CompanyMaster(Session["DBConnection"].ToString());
        Objaddress = new Set_AddressChild(Session["DBConnection"].ToString());
        ObjBrandMaster = new BrandMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjSysParam = new SystemParameter(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());

        FillGrid();

        //GetReport();
        Page.Title = ObjSysParam.GetSysTitle();
        Session["AccordianId"] = "168";
        Session["HeaderText"] = "Production Report";
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }
    //public void GetReport()
    //{
    //    strCompId = Session["CompId"].ToString();
    //    strBrandId = Session["BrandId"].ToString();
    //    strLocationId = Session["LocId"].ToString();
    //    ObjInvdataset.EnforceConstraints = false;
    //    string CompanyName = "";
    //    string CompanyAddress = "";
    //    string Imageurl = "";
    //    string BrandName = "";
    //    string LocationName = "";
    //    string CompanyName_L = string.Empty;
    //    string Companytelno = string.Empty;
    //    string CompanyFaxno = string.Empty;
    //    string CompanyWebsite = string.Empty;



    //    DataTable DtCompany = Objcompany.GetCompanyMasterById(Session["CompId"].ToString());
    //    DataTable DtAddress = Objaddress.GetAddressChildDataByAddTypeAndAddRefId("Company", Session["CompId"].ToString());
    //    if (DtCompany.Rows.Count > 0)
    //    {
    //        CompanyName = DtCompany.Rows[0]["Company_Name"].ToString();
    //        CompanyName_L = DtCompany.Rows[0]["Company_Name_L"].ToString();
    //        Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtCompany.Rows[0]["Logo_Path"].ToString();


    //    }
    //    DataTable DtLocation = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString());
    //    if (DtLocation.Rows.Count > 0)
    //    {
    //        LocationName = DtLocation.Rows[0]["Location_Name"].ToString();
    //        Imageurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + DtLocation.Rows[0]["Field2"].ToString();


    //    }
    //    if (DtAddress.Rows.Count > 0)
    //    {
    //        CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
    //        if (DtAddress.Rows[0]["Address"].ToString() != "")
    //        {
    //            CompanyAddress = DtAddress.Rows[0]["Address"].ToString();
    //        }
    //        if (DtAddress.Rows[0]["Street"].ToString() != "")
    //        {
    //            if (CompanyAddress != "")
    //            {
    //                CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Street"].ToString();
    //            }
    //            else
    //            {
    //                CompanyAddress = DtAddress.Rows[0]["Street"].ToString();
    //            }
    //        }
    //        if (DtAddress.Rows[0]["Block"].ToString() != "")
    //        {
    //            if (CompanyAddress != "")
    //            {
    //                CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Block"].ToString();
    //            }
    //            else
    //            {
    //                CompanyAddress = DtAddress.Rows[0]["Block"].ToString();
    //            }

    //        }
    //        if (DtAddress.Rows[0]["Avenue"].ToString() != "")
    //        {
    //            if (CompanyAddress != "")
    //            {
    //                CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["Avenue"].ToString();
    //            }
    //            else
    //            {
    //                CompanyAddress = DtAddress.Rows[0]["Avenue"].ToString();
    //            }
    //        }

    //        if (DtAddress.Rows[0]["CityId"].ToString() != "")
    //        {
    //            if (CompanyAddress != "")
    //            {
    //                CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["CityId"].ToString();
    //            }
    //            else
    //            {
    //                CompanyAddress = DtAddress.Rows[0]["CityId"].ToString();
    //            }

    //        }
    //        if (DtAddress.Rows[0]["StateId"].ToString() != "")
    //        {


    //            if (CompanyAddress != "")
    //            {
    //                CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["StateId"].ToString();
    //            }
    //            else
    //            {
    //                CompanyAddress = DtAddress.Rows[0]["StateId"].ToString();
    //            }

    //        }
    //        if (DtAddress.Rows[0]["CountryId"].ToString() != "")
    //        {

    //            if (CompanyAddress != "")
    //            {
    //                CompanyAddress = CompanyAddress + "," + LocationName;
    //            }
    //            else
    //            {
    //                CompanyAddress = LocationName;
    //            }

    //        }
    //        if (DtAddress.Rows[0]["PinCode"].ToString() != "")
    //        {
    //            if (CompanyAddress != "")
    //            {
    //                CompanyAddress = CompanyAddress + "," + DtAddress.Rows[0]["PinCode"].ToString();
    //            }
    //            else
    //            {
    //                CompanyAddress = DtAddress.Rows[0]["PinCode"].ToString();
    //            }

    //        }
    //        Companytelno = DtAddress.Rows[0]["PhoneNo1"].ToString();

    //        if (DtAddress.Rows[0]["PhoneNo2"].ToString() != "")
    //        {

    //            if (Companytelno != "")
    //            {
    //                Companytelno = Companytelno + "," + DtAddress.Rows[0]["PhoneNo2"].ToString();
    //            }
    //            else
    //            {
    //                Companytelno = DtAddress.Rows[0]["PhoneNo2"].ToString();
    //            }
    //        }
    //        if (DtAddress.Rows[0]["MobileNo1"].ToString() != "")
    //        {
    //            if (Companytelno != "")
    //            {
    //                Companytelno = Companytelno + "," + DtAddress.Rows[0]["MobileNo1"].ToString();
    //            }
    //            else
    //            {
    //                Companytelno = DtAddress.Rows[0]["MobileNo1"].ToString();
    //            }
    //        }
    //        if (DtAddress.Rows[0]["MobileNo2"].ToString() != "")
    //        {
    //            if (Companytelno != "")
    //            {
    //                Companytelno = Companytelno + "," + DtAddress.Rows[0]["MobileNo2"].ToString();
    //            }
    //            else
    //            {
    //                Companytelno = DtAddress.Rows[0]["MobileNo2"].ToString();
    //            }
    //        }
    //        if (DtAddress.Rows[0]["FaxNo"].ToString() != "")
    //        {
    //            CompanyFaxno = DtAddress.Rows[0]["FaxNo"].ToString();
    //        }
    //        if (DtAddress.Rows[0]["WebSite"].ToString() != "")
    //        {
    //            CompanyWebsite = DtAddress.Rows[0]["WebSite"].ToString();
    //        }



    //    }
    //    DataTable DtBrand = ObjBrandMaster.GetBrandMasterById(Session["CompId"].ToString(), Session["BrandId"].ToString());
    //    if (DtBrand.Rows.Count > 0)
    //    {
    //        BrandName = DtBrand.Rows[0]["Brand_Name"].ToString();
    //    }
    //    DataTable dtemployee = objEmployee.GetEmployeeMasterAllData(Session["CompId"].ToString());

    //    try
    //    {
    //        dtemployee = new DataView(dtemployee, "Emp_Id=" + Session["EmpId"].ToString() + " ", "", DataViewRowState.CurrentRows).ToTable();
    //    }
    //    catch
    //    {
    //    }
    //    string signatureurl = string.Empty;
    //    if (dtemployee.Rows.Count > 0)
    //    {


    //        signatureurl = "~/CompanyResource/" + Session["CompId"].ToString() + "/" + dtemployee.Rows[0]["Field2"].ToString();

    //    }


    //         DataTable dt = (DataTable)Session["DtFilter"];
    //        objFollowupPrint.DataSource = dt;
    //        objFollowupPrint.DataMember = "DtSalesfollowUp";
    //        ReportViewer1.Report = objFollowupPrint;
    //        ReportToolbar1.ReportViewer = ReportViewer1;
    //        objFollowupPrint.setcompanyname(LocationName);
    //        objFollowupPrint.setSignature(signatureurl);
    //        objFollowupPrint.setCompanyArebicName(CompanyName_L);
    //        objFollowupPrint.setCompanyTelNo(Companytelno);
    //        objFollowupPrint.setCompanyFaxNo(CompanyFaxno);
    //        objFollowupPrint.setCompanyWebsite(CompanyWebsite);
    //        objFollowupPrint.setDateCriteria(Session["Parameter"].ToString());
    //        objFollowupPrint.SetImage(Imageurl);
    //        objFollowupPrint.setReportTitle("SALES FOLLOW UP REPORT");
    //}


    protected void btnEXport_Click(object sender, EventArgs e)
    {

        if (gvFollowupReport.Rows.Count > 0 || gvMonthlyFollowupReport.Rows.Count>0)
        {
            if (Request.QueryString["Type"].ToString().Trim() == "M")
            {
                Export(gvMonthlyFollowupReport);
               
            }
            else
            {
                Export(gvFollowupReport);
               
            }
        }
        else
        {
            DisplayMessage("Record Not Available");
        }


    }

    public void Export(GridView GvFollow)
    {

        Response.Clear();
        Response.Buffer = true;

        Response.AddHeader("content-disposition",
        "attachment;filename=FollowupReport.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);

        //GVTrailBalance.AllowPaging = false;
        //GVTrailBalance.DataBind();

        //Change the Header Row back to white color
        //GVComplete.HeaderRow.Style.Add("background-color", "#FFFFFF");

        //Apply style to Individual Cells
        //GVComplete.HeaderRow.Cells[0].Style.Add("background-color", "green");
        //GVComplete.HeaderRow.Cells[1].Style.Add("background-color", "green");
        //GVComplete.HeaderRow.Cells[2].Style.Add("background-color", "green");
        //GVTrailBalance.HeaderRow.Cells[3].Style.Add("background-color", "green");

        for (int i = 0; i < GvFollow.Rows.Count; i++)
        {
            GridViewRow row = GvFollow.Rows[i];

            //Change Color back to white
            row.BackColor = System.Drawing.Color.White;

            //Apply text style to each Row
            row.Attributes.Add("class", "textmode");

            //Apply style to Individual Cells of Alternating Row
            if (i % 2 != 0)
            {
                // row.Cells[0].Style.Add("background-color", "#C2D69B");
                // row.Cells[1].Style.Add("background-color", "#C2D69B");
                // row.Cells[2].Style.Add("background-color", "#C2D69B");
                //row.Cells[3].Style.Add("background-color", "#C2D69B");
            }
        }
        GvFollow.RenderControl(hw);

        //style to format numbers to string
        string style = @"<style> .textmode { mso-number-format:\@; } </style>";
        Response.Write(style);
        Response.Output.Write(sw.ToString());
        Response.Flush();
        Response.End();
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }

    public void FillGrid()
    {
        DataTable dt = (DataTable)Session["DtFilter"];

        PageControlCommon objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (Request.QueryString["Type"].ToString().Trim() == "M")
        {
            objPageCmn.FillData((object)gvFollowupReport, null, "", "");
            objPageCmn.FillData((object)gvMonthlyFollowupReport, dt, "", "");
        }
        else
        {
            objPageCmn.FillData((object)gvFollowupReport, dt, "", "");
            objPageCmn.FillData((object)gvMonthlyFollowupReport, null, "", "");
        }
    }

    protected void GvSalesOrderDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
        GridView gvProduct = (GridView)sender;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = "";
            cell.ColumnSpan = 1;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.Sales_Person;
            cell.ColumnSpan = 1;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.Sales_Inquiry;
            cell.ColumnSpan = 3;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);


            cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.Sales_Quotation;
            cell.ColumnSpan = 4;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);


            cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.Sales_Order;
            cell.ColumnSpan = 3;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);


            cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.Sales_Invoice;
            cell.ColumnSpan = 3;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.Sales_Return;
            cell.ColumnSpan = 3;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);

            gvProduct.Controls[0].Controls.Add(row);
        }
    }

    protected void GvSalesOrderDetailMonthly_RowCreated(object sender, GridViewRowEventArgs e)
    {
        GridView gvProduct = (GridView)sender;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableHeaderCell cell = new TableHeaderCell();
            cell.Text = "";
            cell.ColumnSpan = 1;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);
            cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.Sales_Person;
            cell.ColumnSpan = 1;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);


            cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.Month;
            cell.ColumnSpan = 1;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.Sales_Inquiry;
            cell.ColumnSpan = 3;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);


            cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.Sales_Quotation;
            cell.ColumnSpan = 4;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);


            cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.Sales_Order;
            cell.ColumnSpan = 3;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);


            cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.Sales_Invoice;
            cell.ColumnSpan = 3;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);

            cell = new TableHeaderCell();
            cell.Text = Resources.Attendance.Sales_Return;
            cell.ColumnSpan = 3;
            cell.HorizontalAlign = HorizontalAlign.Center;
            row.Controls.Add(cell);

            gvProduct.Controls[0].Controls.Add(row);
        }
    }
}