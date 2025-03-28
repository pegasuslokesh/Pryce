using System;
using System.Web;
using DevExpress.XtraReports.UI;
using CatalogDataTableAdapters;
using System.Data;
using System.IO;
using PegasusDataAccess;
using System.Web.UI;
using System.Resources;
 

public partial class reportViewer : BasePage
{
    private CatalogData catalogDataSet;
    private DataTable reportsTable;
    private CatalogDataTableAdapters.sys_reportsTableAdapter reportsTableAdapter;
    XtraReport rep = new XtraReport();
    DataAccessClass objDa = null;
    Common ObjComman = null;
    DepartmentMaster objDep = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        objDep = new DepartmentMaster(Session["DBConnection"].ToString());

        DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension.RegisterExtensionGlobal(new CustomReportStorageWebExtension(Session["DBConnection"].ToString()));
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        rep.CreateDocument(false);
        //report.LoadLayout()
        setAdapterconnection();

        string reportId = string.Empty;
        if (Request.QueryString["ReportId"] != null)
        {
            reportId = Request.QueryString["ReportId"].ToString();
            reportsTable = new DataView(reportsTable, "reportId='" + reportId + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Please select a report');", true);
            return;
        }
        if (reportsTable.Rows.Count > 0)
        {
            byte[] reportData = (Byte[])reportsTable.Rows[0]["LayoutData"];

            //DataConnectionParametersBase rep1 = new MsSqlConnectionParameters
            //{
            //    ServerName = "DESKTOP-VNM6HAE",
            //    DatabaseName = "70510",
            //    UserName = "sa",
            //    Password = "123",
            //    AuthorizationType = MsSqlAuthorizationType.SqlServer
            //};

            //SqlDataSource objsql = new SqlDataSource(rep1);
            //rep.DataSource = objsql;
            MemoryStream ms = new MemoryStream(reportData);

            //XmlBuilder xml = new XmlBuilder();
            rep.LoadLayoutFromXml(ms);
            ((DevExpress.DataAccess.Sql.SqlDataSource)rep.DataSource).Connection.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();

            //try
            //{
            //    rep.DataSource = new SqlDataSource();
            //    ((DevExpress.DataAccess.Sql.SqlDataSource)rep.DataSource).Connection.ConnectionString = Session["DBConnection"].ToString();
            //}
            //catch (Exception ex)
            //{

            //}



            //rep.CreateDocument(true);
            //rep.PerformLayout();
            //rep.RequestParameters=true;
            string sql = string.Empty;
            if (HttpContext.Current.Session["UserId"].ToString() == "superadmin")
            {
                sql = "Select STUFF(( SELECT ',' + cast(location_id as varchar) FROM Set_locationMaster where Set_locationMaster.Company_Id = " + HttpContext.Current.Session["CompId"].ToString() + " and Set_locationMaster.IsActive='True' FOR XML PATH('') ), 1, 1, '') ";
            }
            else
            {
                sql = "Select STUFF(( SELECT ',' + cast(record_id as varchar) FROM Set_UserDataPermission inner join Set_locationMaster on Set_locationMaster.location_id= Set_UserDataPermission.record_id where Set_UserDataPermission.Company_Id = " + HttpContext.Current.Session["CompId"].ToString() + " and Set_UserDataPermission.User_Id = '" + HttpContext.Current.Session["UserId"].ToString() + "' and Set_UserDataPermission.IsActive = 'True' and Record_Type = 'L' and Set_locationMaster.IsActive='True' FOR XML PATH('') ), 1, 1, '') ";
            }

            string locationsIds = objDa.get_SingleValue(sql);

            try
            {
                rep.Parameters["Location"].LookUpSettings.FilterString = "[location_id] In(" + locationsIds + ")";
            }
            catch
            {
            }

            try
            {
                rep.Parameters["location_ids"].LookUpSettings.FilterString = "[location_id] In(" + locationsIds + ")";
            }
            catch
            {
            }

            try
            {
                rep.Parameters["report_id"].Value = reportId;
            }
            catch
            {
            }

            try
            {
                rep.Parameters["repCompId"].Value = Session["compId"].ToString();
            }
            catch
            {
            }

            try
            {
                rep.Parameters["repBrandId"].Value = Session["brandId"].ToString();
            }
            catch
            {
            }

            try
            {
                rep.Parameters["repLocId"].Value = Session["locId"].ToString();
                if (Request.QueryString["l"] != null)
                {
                    rep.Parameters["repLocId"].Value = Request.QueryString["l"].ToString();
                }
            }
            catch
            {
            }

            //here we are checking that current report is overtim ereport or not

            string strTransId = "", strSerial = "", strRefType = "", strProductId = "";
            try
            {
                if (Request.QueryString["t"] != null)
                {
                    try
                    {
                        rep.Parameters["repTransId"].Value = Request.QueryString["t"].ToString();
                        strTransId = Request.QueryString["t"].ToString();
                    }
                    catch
                    {

                    }
                    try
                    {
                        rep.Parameters["Trans_Id"].Value = Request.QueryString["t"].ToString();
                        strTransId = Request.QueryString["t"].ToString();
                    }
                    catch
                    {

                    }
                    try
                    {
                        rep.Parameters["Ref_Type"].Value = Request.QueryString["PageRef"].ToString();
                        strRefType = Request.QueryString["t"].ToString();
                    }
                    catch
                    {

                    }
                    try
                    {
                        rep.Parameters["Serial"].Value = Request.QueryString["Serial"].ToString();
                        strSerial = Request.QueryString["t"].ToString();
                    }
                    catch
                    {

                    }

                    try
                    {
                        rep.Parameters["Product_Code"].Value = Request.QueryString["ProductId"].ToString();
                        strProductId = Request.QueryString["t"].ToString();
                    }
                    catch
                    {

                    }
                    try
                    {
                        rep.Parameters["Location_Id"].Value = Request.QueryString["l"].ToString();

                    }
                    catch
                    {

                    }




                    string Imageurl = "";
                    string CompanyName_L = "";
                    string CompanyAddress = "";
                    string Companytelno = string.Empty;
                    string CompanyFaxno = string.Empty;
                    string CompanyWebsite = string.Empty;
                    string CompanyName = string.Empty;

                    string[] strParam = Common.ReportHeaderSetup("Location", Session["LocId"].ToString(), Session["DBConnection"].ToString());


                    CompanyName = strParam[0].ToString();
                    CompanyName_L = strParam[1].ToString();
                    CompanyAddress = strParam[2].ToString();
                    Companytelno = strParam[3].ToString();
                    CompanyFaxno = strParam[4].ToString();
                    CompanyWebsite = strParam[5].ToString();
                    Imageurl = "~/CompanyResource/" + Session["Compid"].ToString() + "/" + strParam[6].ToString();

                    Band reportHeader = rep.Bands[1];
                    try
                    {
                        (reportHeader.FindControl("xrLabel15", true) as XRControl).Text = CompanyAddress;
                    }
                    catch (Exception er) { }

                    try
                    {

                        (reportHeader.FindControl("xrLabel28", true) as XRControl).Text = Companytelno;
                    }
                    catch { }

                    try
                    {
                        (reportHeader.FindControl("xrLabel2", true) as XRControl).Text = CompanyName;

                    }
                    catch { }

                    try
                    {
                        (reportHeader.FindControl("xrLabel29", true) as XRControl).Text = CompanyFaxno;
                    }
                    catch { }

                    try
                    {
                        (reportHeader.FindControl("xrLabel32", true) as XRControl).Text = CompanyWebsite;
                    }
                    catch { }

                    try
                    {
                        (reportHeader.FindControl("xrPictureBox1", true) as XRPictureBox).ImageUrl = Imageurl;
                    }
                    catch { }

                }
            }
            catch (Exception ex)
            {
            }


            try
            {
                lblHeader.Text = reportsTable.Rows[0]["DisplayName"].ToString();
                rep.FindControl("reportName", true).Text = reportsTable.Rows[0]["DisplayName"].ToString();
            }
            catch
            {
            }

            try
            {
                rep.Parameters["CreatedBy"].Value = new EmployeeMaster(Session["DBConnection"].ToString()).GetEmployeeNameByEmployeeId(Session["EmpId"].ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString());
                //rep.Parameters["CreatedBy"].Visible = true;
            }
            catch
            {
            }
            var resourceManager = new ResourceManager(typeof(Resources.Attendance));

            //Set Header as caption as of the culture
            try
            {
                Band pgHeaderBand = rep.Bands["PageHeader1"];
                foreach (XRControl ctl in pgHeaderBand.Controls)
                {
                    if (ctl is XRTable)
                    {
                        foreach (XRTableCell col in ((XRTable)ctl).Rows[0].Cells)
                        {
                            string value = resourceManager.GetString(col.Text);
                            if (!string.IsNullOrEmpty(value))
                            {
                                col.Text = value;
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            // For Hide Serial No
            try
            {
                Band pgHeaderBand = rep.Bands["detailBand1"];
                if ((strSerial == "0" || strSerial == "") && (strTransId == "0" || strTransId == "") && (strRefType == "0" || strRefType == ""))
                {
                    try
                    {
                        (pgHeaderBand.FindControl("xrSerialNo", true) as XRControl).Visible = false;
                        (pgHeaderBand.FindControl("lblSerialNo", true) as XRControl).Visible = false;
                    }
                    catch { }
                    try
                    {
                        (pgHeaderBand.FindControl("barCode2", true) as XRControl).Visible = false;
                        (pgHeaderBand.FindControl("label7", true) as XRControl).Visible = false;
                    }
                    catch { }
                }
            }
            catch
            {
            }
            //

            try
            {
                Band rptHeaderBand = rep.Bands["reportHeaderBand1"];
                foreach (XRControl ctl in rptHeaderBand.Controls)
                {
                    if (ctl is XRLabel)
                    {
                        string value = resourceManager.GetString(((XRLabel)ctl).Text);
                        if (!string.IsNullOrEmpty(value))
                        {
                            ((XRLabel)ctl).Text = value;
                        }
                    }
                }
            }
            catch
            {

            }

            //set parameter caption as of selected culter
            try
            {

                foreach (DevExpress.XtraReports.Parameters.Parameter par in rep.Parameters)
                {
                    if (par.Type.Name == "DateTime")
                    {
                        par.Value = System.DateTime.Now.ToString();
                    }
                    string value = resourceManager.GetString(par.Description);
                    if (!string.IsNullOrEmpty(value))
                    {
                        string val = par.Type.GetType().ToString();
                        par.Description = value;
                    }
                }
            }
            catch
            {

            }

            try
            {

                if (GetOverTimeReportId() == Request.QueryString["ReportId"] && Session["otfromdate"] != null)
                {
                    rep.Parameters["Fromdate"].Value = Convert.ToDateTime(Session["otfromdate"].ToString()).Date.ToString();
                    rep.Parameters["ToDate"].Value = Convert.ToDateTime(Session["ottodate"].ToString()).Date.ToString();
                    rep.Parameters["Employee"].Value = Session["otemplist"].ToString().Split(',');
                    try
                    {
                        rep.Parameters["repdepName"].Value = "";
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }



            //set mobile layout if request came from mobile device
            ASPxWebDocumentViewer1.MobileMode = Request.Browser.IsMobileDevice;


            if (Session["lang"].ToString() == "2")
            {
                rep.RightToLeftLayout = RightToLeftLayout.Yes;
                rep.RightToLeft = RightToLeft.Yes;
            }
            else
            {
                rep.RightToLeftLayout = RightToLeftLayout.No;
                rep.RightToLeft = RightToLeft.No;
            }

            ASPxWebDocumentViewer1.OpenReport(rep);

            if (reportId == "218")
            {
                string path = Server.MapPath("~") + "\\temp\\" + lblHeader.Text + "_" + System.DateTime.Now.ToString("dd_MMM_yyyy_hh_mm_ss") + ".pdf";
                Session["mailAttachmentPath"] = path;

                using (System.Windows.Forms.PrintDialog printDialog = new System.Windows.Forms.PrintDialog())
                {
                    //printDialog.Document = path.ToString(); // Assign the PrintDocument to the dialog

                    //if (printDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    //{
                    //    path.Print(); // Print the document
                    //}
                }
            }
        }
    }

    public string GetOverTimeReportId()
    {
        string strObjectId = Common.GetObjectIdbyPageURL("../Attendance/OT_Request.aspx", Session["DBConnection"].ToString());
        return objDa.return_DataTable("select ReportId from sys_reports where objectid =" + strObjectId + "").Rows[0]["ReportId"].ToString();
    }

    public void setAdapterconnection()
    {
        catalogDataSet = new CatalogData();
        reportsTableAdapter = new sys_reportsTableAdapter();
        reportsTableAdapter.Connection.ConnectionString = HttpContext.Current.Session["DBConnection"].ToString();
        reportsTableAdapter.Fill(catalogDataSet.sys_reports);
        reportsTable = catalogDataSet.Tables["sys_reports"];
    }

    public string GetUserDepartment(string strLocId)
    {
        string strDepId = ObjComman.GetRoleDataPermission(Session["RoleId"].ToString(), "D", strLocId, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        return strDepId.Substring(0, strDepId.Length - 1);
    }

    protected void btnsendEmail_Click(object sender, EventArgs e)
    {
        string path = Server.MapPath("~") + "\\temp\\" + lblHeader.Text + "_" + System.DateTime.Now.ToString("dd_MMM_yyyy_hh_mm_ss") + ".pdf";
        Session["mailAttachmentPath"] = path;
        rep.ExportToPdf(path);
        Session["MailSubject"] = lblHeader.Text;
        Response.Redirect("SendReportAsMail.aspx");

    }


}