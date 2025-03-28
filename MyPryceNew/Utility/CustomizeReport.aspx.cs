using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CatalogDataTableAdapters;
using PegasusDataAccess;
using System.Web;
using DevExpress.XtraReports.Web.ReportDesigner;
using DevExpress.XtraReports.UI;
using System.IO;

public partial class dummy : BasePage
{
    private CatalogData catalogDataSet;
    private static DataTable reportsTable;
    private CatalogDataTableAdapters.sys_reportsTableAdapter reportsTableAdapter;
    ModuleMaster objModule = null;
    Common cmn = null;
    DataAccessClass objDa = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objModule = new ModuleMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            try
            {
                DefaultReportDesignerContainer.RegisterDataSourceWizardConnectionStringsProvider<MyDataSourceWizardConnectionStringsProvider>(true);
            }
            catch
            {

            }

            //filling all data to report datatable
            fillAllReports();


            if (Session["UserId"].ToString().ToUpper() != "SUPERADMIN" && Session["UserId"].ToString() != "7572")
            {
                // filter the report according to permission
                reportsTable = setReprtPermission(reportsTable);
            }


            string application_id = string.Empty;
            application_id = objDa.get_SingleValue("select Param_Value from Sys_Parameter where Param_Name='Application_id'");



            //get all the list of modules
            DataTable dt = new DataTable();
            dt = objDa.return_DataTable("select distinct dbo.IT_ModuleMaster.Module_id,Module_name from dbo.IT_ModuleMaster left join dbo.IT_App_Mod_Object on dbo.IT_ModuleMaster.Module_Id=dbo.IT_App_Mod_Object.Module_Id where IT_App_Mod_Object.Application_Id=" + application_id + " order by Module_name asc");

            //get distinct modules of reports 
            DataTable dt_distinctfilteredModuleID = new DataView(reportsTable).ToTable(true, "ModuleId");
            DataTable dt_distinctfilteredObjectID = new DataView(reportsTable).ToTable(true, "ObjectId");

            string distinctModulesId = "", distinctObjectId = "";

            try
            {
                // used when user will click on navigation bar of some module report
                if (Request.QueryString["Nav_ModuleId"] != null)
                {
                    distinctModulesId = Request.QueryString["Nav_ModuleId"].ToString();
                    dt = new DataView(dt, "Module_Id in (" + distinctModulesId + ")", "", DataViewRowState.CurrentRows).ToTable();
                    lblHeader.Text = dt.Rows[0]["module_name"].ToString();
                }
                else
                {

                    for (int i = 0; i < dt_distinctfilteredModuleID.Rows.Count; i++)
                    {
                        if (dt_distinctfilteredModuleID.Rows.Count - 1 == i)
                        {
                            distinctModulesId += dt_distinctfilteredModuleID.Rows[i]["ModuleId"].ToString();
                        }
                        else
                        {
                            distinctModulesId += dt_distinctfilteredModuleID.Rows[i]["ModuleId"].ToString() + ",";
                        }
                    }
                }
                dt = new DataView(dt, "Module_Id in (" + distinctModulesId + ")", "", DataViewRowState.CurrentRows).ToTable();
                fillModules(dt);


            }
            catch
            {
            }

            //setting object name list
            if (Request.QueryString["Nav_ObjectId"] != null)
            {

                for (int i = 0; i < dt_distinctfilteredObjectID.Rows.Count; i++)
                {
                    if (dt_distinctfilteredObjectID.Rows[i]["ObjectId"].ToString() != "")
                    {
                        distinctObjectId += dt_distinctfilteredObjectID.Rows[i]["ObjectId"].ToString() + ",";
                    }
                }
                fillObjectList(Request.QueryString["Nav_ModuleId"].ToString(), Request.QueryString["Nav_ObjectId"].ToString(), distinctObjectId);
            }
            else
            {
                if (Session["ReportModuleId"] != null)
                {
                    ddlModule.SelectedValue = Session["ReportModuleId"].ToString();
                }
                ddlModule_slectedIndexChanged(null, null);
                //if (Session["ReportModuleId"] != null)
                //{
                //    if(Session["ReportObjectId"]!=null)
                //    {
                //        fillObjectList(Session["ReportModuleId"].ToString(), "0", Session["ReportObjectId"].ToString());
                //    }
                //    else
                //    {                        
                //        fillObjectList(Session["ReportModuleId"].ToString(), "0",distinctObjectId);
                //    }
                //}
                //else
                //{
                //    fillObjectList(distinctModulesId,"0", distinctObjectId);
                //}
            }


            //end 

            // setting module list to update report
            ddlModuleList.Items.Insert(0, new ListItem("Select", "0"));
            DataTable dt1 = objDa.return_DataTable("select distinct dbo.IT_ModuleMaster.Module_id,Module_name from dbo.IT_ModuleMaster left join dbo.IT_App_Mod_Object on dbo.IT_ModuleMaster.Module_Id=dbo.IT_App_Mod_Object.Module_Id where IT_App_Mod_Object.Application_Id=" + application_id + " order by Module_name asc");
            objPageCmn.FillData((object)ddlModuleList, dt1, "module_name", "module_id");

            if (Session["ReportModuleId"] != null)
            {
                ddlModule.SelectedValue = Session["ReportModuleId"].ToString();

                //fillObjectList(ddlModule.SelectedValue, "0", distinctObjectId);

                //setting object list to update report
                fillObjectListForUpdate(ddlModule.SelectedValue);

            }
            //setting report type
            getReportList();

            fillReportList();
            AllPageCode();
        }
    }
    protected void btnNewReport_Click(object sender, EventArgs e)
    {
        Session["DesignerTask"] = new DesignerTask
        {
            mode = ReportEdditingMode.NewReport,
        };
        Session["ReportModuleId"] = ddlModule.SelectedValue;
        Session["ReportObjectIDOfReport"] = ddlObject.SelectedValue;
        Session["ReportType"] = ddlReportType.SelectedValue;
        Response.Redirect("reportDesigner.aspx");

    }
    protected void btnEditReport_Click(object sender, EventArgs e)
    {
        ListItem selected = reportsList.SelectedItem;
        if (selected != null)
        {
            Session["DesignerTask"] = new DesignerTask
            {
                mode = ReportEdditingMode.ModifyReport,
                reportID = selected.Value
            };
            Session["ReportID"] = selected.Value;
            if (!string.IsNullOrEmpty(ddlObject.SelectedValue))
            {
                Session["ReportObjectIDOfReport"] = ddlObject.SelectedValue;
            }
            else
            {
                Session["ReportObjectIDOfReport"] = "0";
            }
            Response.Redirect("reportDesigner.aspx");
        }
    }
    protected void btnPreviewReport_Click(object sender, EventArgs e)
    {
        ListItem selected = reportsList.SelectedItem;
        if (selected != null)
        {
            Session["DesignerTask"] = new DesignerTask
            {
                mode = ReportEdditingMode.ModifyReport,
                reportID = selected.Value
            };
            Session["ReportID"] = selected.Value;
            string strCmd = string.Format("window.open('reportViewer.aspx?reportId=" + selected.Value + "','_blank','width=1024',false);");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "nothing", strCmd, true);
        }
    }
    protected void btnDeleteReport_Click(object sender, EventArgs e)
    {

        CustomizedReport cr = new CustomizedReport(HttpContext.Current.Session["DBConnection"].ToString());
        ListItem selected = reportsList.SelectedItem;
        if (selected == null)
        {
            DisplayMessage("Please select the report and try again");
            return;
        }
        cr.setisActiveFalse(selected.Value);
        if (selected.Value != "")
        {
            objDa.execute_Command("update it_objectentry set isactive='false' where page_url='../Utility/reportviewer.aspx?ReportId=" + selected.Value + "'");
        }
        Response.Redirect(Request.RawUrl);
    }
    protected void ddlModule_slectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlModule.SelectedValue == "0")
        {
            ddlObject.Items.Clear();
            ddlObject.Items.Insert(0, new ListItem("--Select--", "0"));
            ddlReportType.Items.Clear();
            ddlReportType.Items.Insert(0, new ListItem("--Select--", "0"));
            reportsList.DataSource = null;
            reportsList.Items.Clear();
            reportsList.DataBind();
            return;

        }
        reportsList.DataSource = null;
        reportsList.Items.Clear();
        reportsList.DataBind();

        string distinctObjectId = "";
        if (reportsTable == null)
        {
            fillAllReports();
        }
        DataTable dt_distinctFilteredObjectID = new DataView(reportsTable).ToTable(true, "ObjectId");
        for (int i = 0; i < dt_distinctFilteredObjectID.Rows.Count; i++)
        {
            if (dt_distinctFilteredObjectID.Rows[i]["ObjectId"].ToString() != "")
                distinctObjectId += dt_distinctFilteredObjectID.Rows[i]["ObjectId"].ToString() + ",";
        }
        Session["ReportObjectId"] = distinctObjectId;
        fillObjectList(ddlModule.SelectedValue, "0", distinctObjectId);
        ddlReportType.Items.Clear();
        ddlReportType.Items.Insert(0, new ListItem("--Select--", "0"));
        setReportType(reportsTable);
        fillObjectListForUpdate(ddlModule.SelectedValue);
        getReportList();
        //if (Session["UserId"].ToString().ToLower() == "superadmin" || Session["UserId"].ToString().ToLower() == "7572")
        fillReportList();
    }
    protected void getReportList()
    {
        if (ddlModule.SelectedValue != null && ddlModule.SelectedValue != "0" && ddlModule.SelectedValue != "")
        {
            if (reportsTable == null)
            {
                fillAllReports();
                reportsTable = setReprtPermission(reportsTable);
            }
            //DataTable dtModuleReports = new DataView(reportsTable, "ModuleId=" + ddlModule.SelectedValue + " and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
            DataTable dtModuleReports = new DataTable();
            if (Session["UserId"].ToString().ToLower() == "superadmin" || Session["UserId"].ToString().ToLower() == "7572")
            {
                dtModuleReports = new DataView(reportsTable, "ModuleId=" + ddlModule.SelectedValue + " and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                if (ddlObject.SelectedValue != "0")
                {
                    dtModuleReports = new DataView(reportsTable, "ModuleId=" + ddlModule.SelectedValue + " and objectId=" + ddlObject.SelectedValue + " and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                }
                else
                {
                    dtModuleReports = new DataView(reportsTable, "ModuleId=" + ddlModule.SelectedValue + " and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
            setReportType(dtModuleReports);
            Session["ReportModuleId"] = ddlModule.SelectedValue;
            Session["ReportType"] = ddlReportType.SelectedValue;
        }
        else
        {
            reportsList.DataSource = null;
            reportsList.Items.Clear();
            Session["ReportModuleId"] = null;
            Session["ReportType"] = null;
        }
    }
    protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillReportList();
    }
    public void fillReportList()
    {

        if (Session["ReportObjectId"] == null || Session["ReportObjectId"].ToString() == "")
        {
            Session["ReportObjectId"] = "0";
        }
        if (reportsTable != null)
        {
            if (reportsTable.Rows.Count > 0)
            {
                DataTable dtModuleReports = new DataTable();

                //if (ddlReportType.SelectedValue == "")
                //{
                //    dtModuleReports = new DataView(reportsTable, "ModuleId=" + ddlModule.SelectedValue + "  and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                //}
                //else
                //{
                //    dtModuleReports = new DataView(reportsTable, "ModuleId=" + ddlModule.SelectedValue + "  and ReportType='" + ddlReportType.SelectedValue + "' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                //}

                if (Session["UserId"].ToString().ToLower() == "superadmin" || Session["UserId"].ToString().ToLower() == "7572")
                {
                    if (ddlReportType.SelectedValue == "" || ddlReportType.SelectedValue == "0")
                    {
                        dtModuleReports = new DataView(reportsTable, "ModuleId=" + ddlModule.SelectedValue + " and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    else
                    {
                        dtModuleReports = new DataView(reportsTable, "ModuleId=" + ddlModule.SelectedValue + " and ReportType='" + ddlReportType.SelectedValue + "' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                }
                else
                {
                    if (ddlObject.SelectedValue.Trim() != "0")
                    {


                        if (ddlReportType.SelectedValue == "" || ddlReportType.SelectedValue == "0")
                        {
                            dtModuleReports = new DataView(reportsTable, "ModuleId=" + ddlModule.SelectedValue + " and objectId =" + ddlObject.SelectedValue + " and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        else
                        {
                            dtModuleReports = new DataView(reportsTable, "ModuleId=" + ddlModule.SelectedValue + " and objectId =" + ddlObject.SelectedValue + " and ReportType='" + ddlReportType.SelectedValue + "' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                        }
                    }
                    else
                    {
                        if (ddlReportType.SelectedValue == "" || ddlReportType.SelectedValue == "0")
                        {
                            dtModuleReports = new DataView(reportsTable, "ModuleId=" + ddlModule.SelectedValue + " and objectId  in (" + Session["ReportObjectId"].ToString() + ") and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                        }
                        else
                        {
                            dtModuleReports = new DataView(reportsTable, "ModuleId=" + ddlModule.SelectedValue + " and objectId  in (" + Session["ReportObjectId"].ToString() + ") and ReportType='" + ddlReportType.SelectedValue + "' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
                        }

                    }
                }

                reportsList.DataSource = dtModuleReports;
                reportsList.DataTextField = "reportName";
                reportsList.DataValueField = "ReportID";
                reportsList.DataBind();
                Session["ReportModuleId"] = ddlModule.SelectedValue;
                Session["ReportType"] = ddlReportType.SelectedValue;
            }
            else
            {
                reportsList.DataSource = null;
                reportsList.DataBind();
            }
        }
        else
        {
            fillAllReports();
            reportsTable = setReprtPermission(reportsTable);
            DataTable dtModuleReports = new DataTable();
            if (Session["UserId"].ToString().ToLower() == "superadmin" || Session["UserId"].ToString().ToLower() == "7572")
            {
                dtModuleReports = new DataView(reportsTable, "ModuleId=" + ddlModule.SelectedValue + " and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                dtModuleReports = new DataView(reportsTable, "ModuleId=" + ddlModule.SelectedValue + " and ObjectId=" + ddlObject.SelectedValue + " and ReportType='" + ddlReportType.SelectedValue + "' and IsActive='True'", "", DataViewRowState.CurrentRows).ToTable();
            }

            reportsList.DataSource = dtModuleReports;
            reportsList.DataTextField = "reportName";
            reportsList.DataValueField = "ReportID";
            reportsList.DataBind();
            Session["ReportModuleId"] = ddlModule.SelectedValue;
            Session["ReportType"] = ddlReportType.SelectedValue;
        }
    }
    protected void btnSaveChanges_Click(object sender, EventArgs e)
    {

        I1.Attributes.Add("Class", "fa fa-minus");
        div_update.Attributes.Add("Class", "box box-primary");
        string changedName = "", changedModule = "", changedType = "", changedObject = "";

        if (txtReportName.Text == "")
        {
            DisplayMessage("Please Enter Report Name");
            txtReportName.Focus();
            return;
        }

        if (txtReportName.Text.Trim() != "")
        {
            changedName = "DisplayName='" + txtReportName.Text.Trim() + "'";

            if (ddlModuleList.SelectedValue != "--Select--" || ddlChangeReportType.SelectedValue != "0")
            {
                changedName = changedName + ", ";
            }

        }


        if (ddlModuleList.SelectedValue != "--Select--")
        {
            changedModule = "ModuleId='" + ddlModuleList.SelectedValue + "'";
            if (ddlObjectName.SelectedValue != "0")
            {
                changedModule = changedModule + ", ";
            }
        }
        else
        {
            DisplayMessage("Please Select Module");
            ddlModuleList.Focus();
            return;
        }

        if (ddlObjectName.SelectedValue != "0")
        {
            changedObject = "ObjectId='" + ddlObjectName.SelectedValue + "'";
            if (ddlChangeReportType.SelectedValue != "0")
            {
                changedObject = changedObject + ", ";
            }
        }
        else
        {
            DisplayMessage("Please Select Module");
            ddlObjectName.Focus();
            return;
        }


        if (ddlChangeReportType.SelectedValue != "0")
        {
            changedType = "ReportType='" + ddlChangeReportType.SelectedValue + "'";
        }
        else
        {
            DisplayMessage("Please Select Report Type");
            ddlChangeReportType.Focus();
            return;
        }

        //used to update the report record
        string sqlQuery = "update sys_reports set " + changedName + " " + changedModule + " " + changedObject + " " + changedType + " where ReportId=" + reportsList.SelectedValue + "";
        objDa.return_DataTable(sqlQuery);



        string objectId = "0";
        objectId = Common.GetObjectIdbyPageURL("../Utility/reportviewer.aspx?ReportId=" + reportsList.SelectedValue, Session["DBConnection"].ToString());



        //used to update the name in object entry table
        sqlQuery = "update IT_ObjectEntry set Object_Name='" + txtReportName.Text + "' where Object_Id=" + objectId + "";
        objDa.return_DataTable(sqlQuery);


        if (ddlModuleList.SelectedValue != ddlModule.SelectedValue)
        {
            reportsTable = new DataView(reportsTable, "DisplayName='" + reportsList.SelectedItem + "'", "", DataViewRowState.CurrentRows).ToTable();

            // used to update the module id in object entry
            sqlQuery = "update IT_App_Mod_Object set module_id = '" + ddlModuleList.SelectedValue + "' where object_id = '" + objectId + "' and module_id = '" + ddlModule.SelectedValue + "' ";
            objDa.return_DataTable(sqlQuery);

            // used to update the user permission in role
            sqlQuery = "update Set_UserPermission set Module_Id = '" + ddlModuleList.SelectedValue + "' where  object_id = '" + objectId + "' and Module_Id = '" + ddlModule.SelectedValue + "'";
            objDa.return_DataTable(sqlQuery);
        }
        Response.Redirect(Request.RawUrl);

    }

    protected void reportsList_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblDeviceParameter.Text = "Report To Edit:" + reportsList.SelectedItem;
        txtReportName.Text = reportsList.SelectedItem.ToString();
        ddlChangeReportType.SelectedValue = ddlReportType.SelectedValue;
        ddlModuleList.SelectedValue = ddlModule.SelectedValue;
        //.Attributes["class"] = "hidden";
    }

    public void AllPageCode()
    {
        //New Code 
        string strModuleId = string.Empty;
        div_update.Visible = false;

        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());
        DataTable dtModule = objObjectEntry.GetModuleIdAndName(Common.GetObjectIdbyPageURL("../Utility/customizereport.aspx", Session["DBConnection"].ToString()), (DataTable)Session["ModuleName"]);
        Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());

        if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), Common.GetObjectIdbyPageURL("../utility/customizereport.aspx", Session["DBConnection"].ToString()), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }

        if (dtModule.Rows.Count > 0)
        {
            strModuleId = dtModule.Rows[0]["Module_Id"].ToString();
        }
        else
        {
            Session.Abandon();
            Response.Redirect("~/ERPLogin.aspx");
        }

        if (Session["EmpId"].ToString() == "0")
        {
            div_update.Visible = true;
            btnNewReport.Visible = true;
            btnEditReport.Visible = true;
            btnDeleteReport.Visible = true;
            btnPreviewReport.Visible = true;
            return;
        }
        DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, Common.GetObjectIdbyPageURL("../Utility/customizereport.aspx", Session["DBConnection"].ToString()), HttpContext.Current.Session["CompId"].ToString());
        if (dtAllPageCode.Rows.Count != 0)
        {
            if (dtAllPageCode.Rows[0][0].ToString() != "SuperAdmin")
            {
                foreach (DataRow DtRow in dtAllPageCode.Rows)
                {
                    if (DtRow["Op_Id"].ToString() == "2")
                    {
                        btnEditReport.Visible = true;
                    }

                    if (DtRow["Op_Id"].ToString() == "1")
                    {
                        btnNewReport.Visible = true;
                        div_update.Visible = true;
                    }

                    if (DtRow["Op_Id"].ToString() == "3")
                    {
                        btnDeleteReport.Visible = true;
                    }

                    if (DtRow["Op_Id"].ToString() == "5")
                    {
                        btnPreviewReport.Visible = true;
                    }

                }
            }
        }
    }

    public void setReportType(DataTable dt)
    {
        //ddlReportType.Items.Remove(ddlReportType.Items.FindByValue("TextToFind"));
        ddlReportType.Items.Clear();
        if (Request.QueryString["Nav_ModuleId"] != null)
        {
            dt = new DataView(dt, "ModuleId = '" + Request.QueryString["Nav_ModuleId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        if (Request.QueryString["Nav_ObjectId"] != null)
        {
            dt = new DataView(dt, "objectId = '" + Request.QueryString["Nav_ObjectId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        DataView view = new DataView(dt);

        dt = view.ToTable(true, "ReportType");

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            ddlReportType.Items.Add(new ListItem(dt.Rows[i]["ReportType"].ToString(), dt.Rows[i]["ReportType"].ToString()));
        }
        ddlReportType.Items.Insert(0, new ListItem("--Select--", ""));
    }

    public DataTable setReprtPermission(DataTable dt_report)
    {
        DataTable dt = new DataTable();
        string number = "";
        for (int i = 0; i < dt_report.Rows.Count; i++)
        {
            dt = objDa.return_DataTable("SELECT Set_RoleOpPermission.*, set_operationtype.Op_Type FROM Set_RoleOpPermission LEFT JOIN set_operationtype ON set_operationtype.Op_Id = Set_RoleOpPermission.Op_Id WHERE Ref_Id IN(SELECT TransId FROM Set_RolePermission WHERE Role_Id IN(SELECT CAST(Value AS int) FROM F_Split((SELECT role_id FROM set_usermaster WHERE company_id = '" + Session["CompId"].ToString() + "' AND user_id = '" + Session["UserId"].ToString() + "' ), ',')) and Object_Id = '" + Common.GetObjectIdbyPageURL("../Utility/reportviewer.aspx?ReportId=" + dt_report.Rows[i]["ReportId"].ToString(), Session["DBConnection"].ToString()) + "' and set_operationtype.Op_Type = 'view' )");

            if (dt != null)
            {
                if (dt.Rows.Count == 0)
                {
                    number += dt_report.Rows[i]["ReportId"].ToString() + ",";
                }
            }
        }

        dt_report = new DataView(dt_report, "ReportId not in (" + number + ")", "", DataViewRowState.CurrentRows).ToTable();

        return dt_report;
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','" + color + "','white');", true);
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

    protected void btnExportReport_Click(object sender, EventArgs e)
    {
        ListItem selected = reportsList.SelectedItem;
        if (selected != null)
        {
            string hg = "";
            try
            {
                DataTable dt = new DataView(reportsTable, "ReportId=" + selected.Value + "", "", DataViewRowState.CurrentRows).ToTable();
                XtraReport xr = new XtraReport();
                byte[] reportData = (Byte[])dt.Rows[0]["LayoutData"];
                dt = null;

                using (MemoryStream ms = new MemoryStream(reportData))
                {
                    xr.LoadLayout(ms);
                    hg = Server.MapPath("~") + "Report_Repx\\" + selected.Value + ".repx";
                    xr.SaveLayout(hg);
                }
                DownLoad(hg);
            }
            catch (Exception ex)
            {
            }
            DisplayMessage("Report Exported at " + hg);
        }
        else
        {
            DisplayMessage("Please select a report to export");
        }
    }

    public string DownLoad(string FName)
    {
        string path = FName;
        System.IO.FileInfo file = new System.IO.FileInfo(path);
        if (file.Exists)
        {
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + file.Name);
            Response.TransmitFile(path);
            Response.End();
        }
        return "";
    }

    protected void BrowseButton_Click(object sender, EventArgs e)
    {
        if (FileUploadControl.HasFile)
        {
            ListItem selected = reportsList.SelectedItem;
            if (selected == null)
            {
                DisplayMessage("Please select a report");
                return;
            }
            try
            {
                XtraReport r1 = new XtraReport();
                //  r1.LoadLayout(@"E:\E_DivyaPryce_svnReport_Repx55.repx");
                HttpPostedFile file = FileUploadControl.PostedFile;
                //byte[] bytes = new byte[file.ContentLength];

                r1.LoadLayout(file.InputStream);

                catalogDataSet = new CatalogData();
                reportsTableAdapter = new sys_reportsTableAdapter();
                reportsTableAdapter.Connection.ConnectionString = Session["DBConnection"].ToString();
                reportsTableAdapter.Fill(catalogDataSet.sys_reports);
                reportsTable = catalogDataSet.Tables["sys_reports"];

                DataRow row = reportsTable.Rows.Find(int.Parse(selected.Value));

                using (MemoryStream ms = new MemoryStream())
                {
                    r1.SaveLayoutToXml(ms);
                    //byte[] data = ms.GetBuffer();
                    row["LayoutData"] = ms.GetBuffer();
                }
                row["ModifiedBy"] = HttpContext.Current.Session["UserId"].ToString();
                row["ModifiedDate"] = DateTime.Now.ToString();

                //objDa.execute_Command("update sys_reports set layoutdata = "+ bytes + " where reportid =" + selected.Value + "");

                reportsTableAdapter.Update(catalogDataSet);
                catalogDataSet.AcceptChanges();
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {

            }

        }
        else
        {

        }
    }

    protected void ddlObject_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlObject.SelectedValue == "0")
        {
            ddlReportType.Items.Clear();
            ddlReportType.Items.Insert(0, new ListItem("--Select--", "0"));
            reportsList.DataSource = null;
            reportsList.Items.Clear();
            reportsList.DataBind();
           
        }
        Session["ReportObjectIDOfReport"] = ddlObject.SelectedValue;
        getReportList();
        fillReportList();
    }

    public void fillObjectList(string moduleIds, string selectedIndex = "0", string objectId = "0")
    {
        ddlObject.Items.Clear();
        DataTable dtObject = new DataTable();
        if (moduleIds == "" || moduleIds == "0")
        {
            dtObject = objDa.return_DataTable("select distinct IT_ObjectEntry.Object_Name,IT_ObjectEntry.Object_Id from IT_ObjectEntry inner join IT_App_Mod_Object on IT_ObjectEntry.Object_Id = IT_App_Mod_Object.Object_Id where IT_ObjectEntry.IsActive = 'true'  order by IT_ObjectEntry.Object_Name asc");
        }
        else
        {
            dtObject = objDa.return_DataTable("select distinct IT_ObjectEntry.Object_Name,IT_ObjectEntry.Object_Id from IT_ObjectEntry inner join IT_App_Mod_Object on IT_ObjectEntry.Object_Id = IT_App_Mod_Object.Object_Id where IT_ObjectEntry.IsActive = 'true'  and IT_App_Mod_Object.Module_Id in (" + moduleIds + ") order by IT_ObjectEntry.Object_Name asc");
        }

        if (objectId != "0" && objectId != "")
        {
            dtObject = new DataView(dtObject, "Object_Id in (" + objectId + ")", "", DataViewRowState.CurrentRows).ToTable();
        }
        ddlObject.DataSource = dtObject;
        ddlObject.DataTextField = "Object_Name";
        ddlObject.DataValueField = "Object_Id";
        ddlObject.DataBind();
        ddlObject.Items.Insert(0, new ListItem("--Select--", "0"));
        ddlObject.SelectedValue = selectedIndex;
    }

    public void fillObjectListForUpdate(string moduleIds)
    {
        // ddlObjectName.Items.Clear();
        DataTable dtObject = new DataTable();
        if (moduleIds == "" || moduleIds == "0")
        {
            dtObject = objDa.return_DataTable("select distinct IT_ObjectEntry.Object_Name,IT_ObjectEntry.Object_Id from IT_ObjectEntry inner join IT_App_Mod_Object on IT_ObjectEntry.Object_Id = IT_App_Mod_Object.Object_Id where IT_ObjectEntry.IsActive = 'true' and IT_ObjectEntry.ShowInNavigationMenu = 'true' order by IT_ObjectEntry.Object_Name asc");
        }
        else
        {
            dtObject = objDa.return_DataTable("select distinct IT_ObjectEntry.Object_Name,IT_ObjectEntry.Object_Id from IT_ObjectEntry inner join IT_App_Mod_Object on IT_ObjectEntry.Object_Id = IT_App_Mod_Object.Object_Id where IT_ObjectEntry.IsActive = 'true' and IT_ObjectEntry.ShowInNavigationMenu = 'true' and IT_App_Mod_Object.Module_Id in (" + moduleIds + ") order by IT_ObjectEntry.Object_Name asc");
        }

        ddlObjectName.DataSource = dtObject;
        ddlObjectName.DataTextField = "Object_Name";
        ddlObjectName.DataValueField = "Object_Id";
        ddlObjectName.DataBind();
        ddlObjectName.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    public void fillModules(DataTable dt)
    {
        ddlModule.DataSource = dt;
        ddlModule.DataTextField = "module_name";
        ddlModule.DataValueField = "module_id";
        ddlModule.DataBind();
        if (ddlModule.Items.Count > 1)
            ddlModule.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void ddlModuleList_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        div_update.Attributes.Add("Class", "box box-primary");
        fillObjectListForUpdate(ddlModuleList.SelectedValue);
    }
    public DataTable fillAllReports()
    {

        catalogDataSet = new CatalogData();
        reportsTableAdapter = new sys_reportsTableAdapter();
        reportsTableAdapter.Connection.ConnectionString = Session["DBConnection"].ToString();
        reportsTableAdapter.Fill(catalogDataSet.sys_reports);
        reportsTable = catalogDataSet.Tables["sys_reports"];
        return reportsTable;
    }
}