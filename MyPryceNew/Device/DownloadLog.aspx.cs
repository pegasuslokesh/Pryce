using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using PegasusDataAccess;
using System.Threading;
using PryceDevicesLib;
using System.Collections.Generic;

public partial class Device_DownloadLog : BasePage
{
    Att_DeviceMaster objDevice = null;
    Common cmn = null;
    SystemParameter objSys = null;
    Att_AttendanceLog objAttlog = null;
    IAttDeviceOperation objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(TechType.PULL, "zkTecho");
    //Device_Operation_Lan objDeviceOp = new Device_Operation_Lan();
    EmployeeInformation objEmpInfo = null;
    EmployeeMaster objEmp = null;
    Set_ApplicationParameter objAppParam = null;
    //Device_Operation_Lan objDeviceCon = new Device_Operation_Lan();
    EmployeeParameter objEmpParam = null;
    Ser_UserTransfer objSer = null;
    LocationMaster ObjLocationMaster = null;
    RoleDataPermission objRoleData = null;
    RoleMaster objRole = null;
    Set_Location_Department objLocDept = null;
    DataAccessClass objDa = null;
    PageControlCommon objPageCmn = null;
    string strPageLevel = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        objDevice = new Att_DeviceMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objAttlog = new Att_AttendanceLog(Session["DBConnection"].ToString());
        objEmpInfo = new EmployeeInformation(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        objSer = new Ser_UserTransfer(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        Page.Title = objSys.GetSysTitle();

        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "77", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            FillddlLocation();
            FillGrid();
            pnlDeviceOp.Visible = false;
        }
        AllPageCode();
    }
    #region LocationFilter
    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
    private void FillddlLocation()
    {

        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        // DataTable dt = objEmp.GetEmployeeOrDepartment(Session["Company"].ToString(), Session["Brand"].ToString(), Session["Location"].ToString(), Session["Dept"].ToString(), "0");
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.DataSource = null;
            ddlLocation.DataBind();
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)ddlLocation, dtLoc, "Location_Name", "Location_Id");
        }
        else
        {
            try
            {
                ddlLocation.Items.Clear();
                ddlLocation.DataSource = null;
                ddlLocation.DataBind();
                ddlLocation.Items.Insert(0, "--Select--");
                ddlLocation.SelectedIndex = 0;
            }
            catch
            {
                ddlLocation.Items.Insert(0, "--Select--");
                ddlLocation.SelectedIndex = 0;
            }
        }
    }
    #endregion
    protected void gvDevice_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{

        //    string port = gvDevice.DataKeys[e.Row.RowIndex]["Port"].ToString();
        //    string IP = gvDevice.DataKeys[e.Row.RowIndex]["IP_Address"].ToString();
        //    string DeviceId = gvDevice.DataKeys[e.Row.RowIndex]["Device_Id"].ToString();

        //    Device_Operation_Lan objDeviceOp = new Device_Operation_Lan();


        //    bool b = false;
        //    // b = objDeviceOp.Device_Connection(IP, Convert.ToInt32(port), 0);

        //    b = Common.IsAlive(IP);
        //    if (b == true)
        //    {
        //        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#32CD32");
        //        //e.Row.Cells[11].BackColor = System.Drawing.Color.Green;
        //        ((Label)e.Row.FindControl("lblStatus")).Text = "Connected";
        //        //((Label)e.Row.FindControl("lblStatus")).BackColor = System.Drawing.Color.Green;
        //    }
        //    else
        //    {
        //        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FA8072");
        //        //e.Row.Cells[11].BackColor = System.Drawing.Color.Red;
        //        ((Label)e.Row.FindControl("lblStatus")).Text = "Disconnected";
        //        //((Label)e.Row.FindControl("lblStatus")).BackColor = System.Drawing.Color.Red;
        //    }
        //}

    }
    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());

        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("77", (DataTable)Session["ModuleName"]);
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

            try
            {
                gvDevice.Columns[0].Visible = true;
            }
            catch
            {

            }
            //foreach (GridViewRow Row in gvDevice.Rows)
            //{
            //    ((CheckBox)Row.FindControl("chkdevice")).Visible = true;
            //  //  ((ImageButton)Row.FindControl("LnkDeviceOp")).Visible = true;
            //}
        }
        else
        {
            DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "77", Session["CompId"].ToString());
            if (dtAllPageCode.Rows.Count == 0)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            if (dtAllPageCode.Rows.Count != 0)
            {
                if (dtAllPageCode.Rows[0][0].ToString() == "SuperAdmin")
                {

                }
                else
                {
                    foreach (DataRow DtRow in dtAllPageCode.Rows)
                    {
                        if (DtRow["Op_Id"].ToString() == "7")
                        {
                            try
                            {
                                gvDevice.Columns[0].Visible = true;
                            }
                            catch
                            {

                            }
                        }
                    }
                }
            }
        }
        //else
        //{
        //    Session.Abandon();
        //    Response.Redirect("~/ERPLogin.aspx");
        //}
    }
    public void FillGrid()
    {
        //Add On 04-06-2015
        string strFLocId = string.Empty;
        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
        //dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(Session["EmpId"].ToString()))
        {
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

                if (dtLoc.Rows.Count > 0)
                {
                    for (int i = 0; i < dtLoc.Rows.Count; i++)
                    {
                        strFLocId += dtLoc.Rows[i]["Location_Id"].ToString() + ",";
                    }
                }
            }
        }
        //End

        //Update On 02-06-2015 For According to Company & Location
        try
        {
            strPageLevel = objAppParam.GetApplicationParameterValueByParamName("Page Level", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        }
        catch
        {
            strPageLevel = "Location";
        }

        DataTable dt = objDevice.GetDeviceMaster(Session["CompId"].ToString());
        if (strPageLevel == "Company")
        {
            if (dt.Rows.Count > 0)
            {
                if (strFLocId != "")
                {
                    dt = new DataView(dt, "Location_Id in(" + strFLocId.Substring(0, strFLocId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
            Div_location.Visible = true;
        }
        else if (strPageLevel == "Location")
        {
            if (dt.Rows.Count > 0)
            {
                dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        if (ddlLocation.SelectedIndex > 0)
        {
            dt = new DataView(dt, "Location_Id=" + ddlLocation.SelectedValue + "", "", DataViewRowState.CurrentRows).ToTable();
        }

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvDevice, dt, "", "");

        Session["dtFilter_Download_Log"] = dt;
        Session["Device"] = dt;
        AllPageCode();
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    protected void gvDevice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDevice.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["dtFilter_Download_Log"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvDevice, dt, "", "");
    }
    protected void gvLog_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLog.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtDownloadLog"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvLog, dt, "", "");
        AllPageCode();
    }
    protected void gvDevice_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Download_Log"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Download_Log"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvDevice, dt, "", "");
        AllPageCode();
    }
    public string GetEmployeeCode(object empid)
    {

        string empname = string.Empty;

        DataTable dt = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
        dt = new DataView(dt, "Emp_Id='" + empid.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {

            empname = dt.Rows[0]["Emp_Code"].ToString();

            if (empname == "")
            {
                empname = "No Code";
            }

        }
        else
        {
            empname = "No Code";

        }

        return empname;



    }
    public DataTable FilterLog(DataTable dt, string strDeviceId, string MaxLogDate, DataTable dtEmp)
    {
        DataTable dtLog = dt;

        DataTable dtAttLog = new DataTable();

        // dtEmp = dtEmp.DefaultView.ToTable(true, "Emp_Id");
        string EmpIds = string.Empty;


        for (int i = 0; i < dtEmp.Rows.Count; i++)
        {
            EmpIds += (dtEmp.Rows[i]["Emp_Code"].ToString()) + ",";
        }

        if (EmpIds != "")
        {
            dtLog = new DataView(dt, "sdwEnrollNumber in(" + EmpIds + ")", "", DataViewRowState.CurrentRows).ToTable();
        }

        //string tFrom = string.Empty;
        if (MaxLogDate != "")
        {
            dtAttLog = objAttlog.GetAttendanceLog(Session["CompId"].ToString(), strDeviceId, MaxLogDate.ToString());

            //MaxLogDate = Convert.ToDateTime(MaxLogDate).ToString();

            //dtAttLog = new DataView(dtAttLog, "Event_Time  > '" + tFrom + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        //int CompareLogsDays = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("CompareLogs", Session["CompId"].ToString()));
        // CompareLogsDays = CompareLogsDays + 1;
        // string tFrom = DateTime.Now.AddDays(CompareLogsDays * -1).ToString();

        DataTable dtFilter = new DataTable();
        DataTable dtFinal = new DataTable();
        dtFinal = dtLog.Clone();
        string strEmpId = string.Empty;
        if (dtAttLog.Rows.Count > 0)
        {
            for (int j = 0; j < dtLog.Rows.Count; j++)
            {
                strEmpId = "0";
                DataRow[] drEmp = dtEmp.Select("emp_code='" + dtLog.Rows[j]["sdwEnrollNumber"].ToString() + "'");
                strEmpId = drEmp[0]["emp_id"].ToString();



                dtFilter = new DataView(dtAttLog, "Emp_Id='" + strEmpId + "' and Event_Time='" + dtLog.Rows[j]["LDateTime"].ToString() + "' ", "", DataViewRowState.CurrentRows).ToTable();

                if (dtFilter.Rows.Count == 0)
                {
                    dtFinal.ImportRow(dtLog.Rows[j]);
                }
            }
        }
        else
        {
            dtFinal = dtLog;
        }
        return dtFinal;
    }
    public string GetEmpId(string empcode)
    {
        string empId = string.Empty;


        DataTable dt = objEmp.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), empcode);

        if (dt.Rows.Count > 0)
            empId = dt.Rows[0]["Emp_Id"].ToString();
        else
            empId = "0";
        //    DataTable dt = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
        //dt = new DataView(dt, "Emp_Code='" + empcode.ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //if (dt.Rows.Count > 0)
        //{
        //    empId = dt.Rows[0]["Emp_Id"].ToString();
        //}
        return empId;
    }
    public int InsertLoginDatabse(DataTable dtEmp, DataTable dtLog, string strDeviceId)
    {

        DataTable dtLogDetail = dtLog.Copy();
        string tFrom = string.Empty;
        int counter = 0;
        string strEmpId = string.Empty;
        DataTable dtEmpList = new DataTable();
        int b = 0;
        if (dtLog.Rows.Count > 0)
        {
            dtEmpList = dtLogDetail.DefaultView.ToTable(true, "enrollNumber");

            for (int j = 0; j < dtEmpList.Rows.Count; j++)
            {
                DataRow[] drEmp = dtEmp.Select("emp_code='" + dtEmpList.Rows[j]["enrollNumber"].ToString() + "'");

                if (drEmp.Length > 0)
                {
                    strEmpId = drEmp[0]["emp_id"].ToString();

                    string sql = "select isnull(max(event_time),'')as event_time from Att_AttendanceLog where Att_AttendanceLog.Company_Id = '" + Session["CompId"].ToString() + "' AND Att_AttendanceLog.IsActive = 'True'  AND Att_AttendanceLog.Device_Id='" + strDeviceId + "'  and  Att_AttendanceLog.Emp_Id=" + strEmpId + "";
                    tFrom = objDa.get_SingleValue(sql).ToString();

                    if (tFrom != "1/1/1900 12:00:00 AM")
                    {
                        tFrom = Convert.ToDateTime(tFrom).ToString();
                    }
                    else
                    {
                        tFrom = "";
                    }

                    if (tFrom != "")
                    {
                        dtLog = new DataView(dtLogDetail, "LDateTime  > '" + tFrom + "' and enrollNumber  = '" + dtEmpList.Rows[j]["enrollNumber"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    else
                    {
                        dtLog = new DataView(dtLogDetail, "enrollNumber  = '" + dtEmpList.Rows[j]["enrollNumber"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    }

                    for (int i = 0; i < dtLog.Rows.Count; i++)
                    {

                        b = objAttlog.InsertAttendanceLog(Session["CompId"].ToString(), strEmpId, Session["DevId"].ToString(), Convert.ToDateTime(dtLog.Rows[i]["LDateTime"].ToString()).ToString(), dtLog.Rows[i]["logTime"].ToString(), dtLog.Rows[i]["inOutMode"].ToString(), "In", "By Device", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                        if (b != 0)
                        {
                            counter++;
                        }


                    }
                }
            }
        }
        return counter;

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //    DataTable dtLog = new DataTable();
        //    dtLog = (DataTable)Session["dtDownloadLog"];
        //    int b = 0;
        //    if (dtLog.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dtLog.Rows.Count; i++)
        //        {

        //            b = objAttlog.InsertAttendanceLog(Session["CompId"].ToString(), GetEmpId(dtLog.Rows[i]["sdwEnrollNumber"].ToString()), Session["DevId"].ToString(), Convert.ToDateTime(dtLog.Rows[i]["sTimeString"].ToString()).ToString("MM/dd/yyyy"), Convert.ToDateTime(dtLog.Rows[i]["sTimeString"].ToString()).ToString(), dtLog.Rows[i]["idwInOutMode"].ToString(), "In", "By Device", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        //        }
        //    }

        //    Session["dtDownloadLog"] = null;
        //    gvLog.DataSource = null;
        //    gvLog.DataBind();
        //    pnlList.Visible = true;
        //    pnlDeviceOp.Visible = false;
        //    DisplayMessage("Log Saved");
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Session["dtDownloadLog"] = null;
        gvLog.DataSource = null;
        gvLog.DataBind();
        pnlList.Visible = true;
        pnlDeviceOp.Visible = false;
        AllPageCode();
    }
    protected void lnkBackFromManage_Click(object sender, EventArgs e)
    {
        Session["IPforManage"] = "";
        Session["PortForManage"] = "";

        pnlList.Visible = true;
        pnlDeviceOp.Visible = false;
    }
    #region download
    public static bool IsAlive(string IP, int Socket)
    {
        //bool result = false;


        try
        {
            TcpClient client = new TcpClient(IP, Socket);
            return true;
        }
        catch (Exception ex)
        {
            //MessageBox.Show("Error pinging host:'" + aIP + ":4370'");
            return false;
        }




    }
    protected void btnDownload_OnClick(object sender, EventArgs e)
    {
        FillLogStatus(null);

        DataTable dtStatus = new DataTable();
        dtStatus.Columns.Add("Status");
        dtStatus.Columns.Add("STime");

        int savedlogcount = 0;
        bool Connect = true;
        DataTable dtEmp = objEmp.GetEmployeeMasterAllData(Session["CompId"].ToString());
        DataTable dtLog = new DataTable();
        DataTable dtTemp = new DataTable();

        int counter = 0;



        dtStatus.Rows.Add("Log Download Process Started", DateTime.Now.ToString());
        FillLogStatus(dtStatus);

        foreach (GridViewRow gvrow in gvDevice.Rows)
        {

            if (((CheckBox)gvrow.FindControl("chkdevice")).Checked)
            {
                //objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(TechType.PUSH, "a7Plus");
                counter++;

                int index = gvrow.RowIndex;
                string ip = gvDevice.DataKeys[index]["IP_Address"].ToString();
                string deviceid = gvDevice.DataKeys[index]["Device_Id"].ToString();
                string port = gvDevice.DataKeys[index]["Port"].ToString();
                string strCommType = gvDevice.DataKeys[index]["Communication_Type"].ToString();
                string strMake = gvDevice.DataKeys[index]["field2"].ToString();

                Session["DevId"] = deviceid;

                objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(strCommType == "PULL" ? TechType.PULL : TechType.PUSH, strMake);

                //txtlogStatus.Text += Environment.NewLine + gvDevice.DataKeys[index]["Device_Name"].ToString() + " : device connecting  " + " " + DateTime.Now.ToString();


                dtStatus.Rows.Add(gvDevice.DataKeys[index]["Device_Name"].ToString() + " : device connecting", DateTime.Now.ToString());

                FillLogStatus(dtStatus);

                Connect = objDeviceOp.Device_Connection(ip, port, 0);

                if (!Connect)
                {
                    dtStatus.Rows.Add(gvDevice.DataKeys[index]["Device_Name"].ToString() + " : device disconnected ", DateTime.Now.ToString());
                    FillLogStatus(dtStatus);
                    continue;
                }

                dtStatus.Rows.Add(gvDevice.DataKeys[index]["Device_Name"].ToString() + " : device connected", DateTime.Now.ToString());
                FillLogStatus(dtStatus);

                //if (Connect == true)
                //{
                lblDeviceId.Text = deviceid;
                lblDeviceWithId.Text = gvDevice.DataKeys[index]["Device_Name"].ToString();
                List<clsUserLog> objuserlog = objDeviceOp.GetUserLog(ip, port);
                dtLog = cmn.ListToDataTable(objuserlog);
                if (dtLog.Rows.Count > 0)
                {

                    dtStatus.Rows.Add(gvDevice.DataKeys[index]["Device_Name"].ToString() + " : device downloading log...", DateTime.Now.ToString());
                    FillLogStatus(dtStatus);

                    //string tFrom = string.Empty;
                    ////string maxLogDate = string.Empty;
                    //string sql = "select isnull(max(event_time),'')as event_time from Att_AttendanceLog where Att_AttendanceLog.Company_Id = '" + Session["CompId"].ToString() + "' AND Att_AttendanceLog.IsActive = 'True'  AND Att_AttendanceLog.Device_Id='" + deviceid + "'";
                    //tFrom = objDa.get_SingleValue(sql).ToString();

                    //if (tFrom != "1/1/1900 12:00:00 AM")
                    //{
                    //    tFrom = Convert.ToDateTime(tFrom).ToString();
                    //}
                    //else
                    //{
                    //    tFrom = "";
                    //}

                    //if (tFrom != "")
                    //{
                    //    dtLog = new DataView(dtLog, "LDateTime  > '" + tFrom + "'", "", DataViewRowState.CurrentRows).ToTable();
                    //}

                    dtStatus.Rows.Add(gvDevice.DataKeys[index]["Device_Name"].ToString() + " : downloaded log = " + dtLog.Rows.Count.ToString(), DateTime.Now.ToString());
                    FillLogStatus(dtStatus);
                    //

                    //dtLog = FilterLog(dtLog, deviceid, tFrom, dtEmp);
                    //For Save Log
                    savedlogcount = InsertLoginDatabse(dtEmp, dtLog, deviceid);
                    dtStatus.Rows.Add(gvDevice.DataKeys[index]["Device_Name"].ToString() + " : Log saved = " + savedlogcount.ToString(), DateTime.Now.ToString());
                    FillLogStatus(dtStatus);
                    dtTemp.Merge(dtLog);
                }
                else
                {
                    dtStatus.Rows.Add(gvDevice.DataKeys[index]["Device_Name"].ToString() + " : No Log Found", DateTime.Now.ToString());
                    FillLogStatus(dtStatus);
                }

                //}
                //else
                //{
                //    continue;
                //}


            }

        }

        if (counter == 0)
        {

            DisplayMessage("Select atleast one device");

            return;
        }

        if (dtTemp.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvLog, dtTemp, "", "");
            Session["dtDownloadLog"] = dtTemp;
            dtTemp.Dispose();
            dtLog.Dispose();
            Session["dtDownloadLog"] = null;
            gvLog.DataSource = null;
            gvLog.DataBind();
            pnlList.Visible = true;
            pnlDeviceOp.Visible = false;
            if (savedlogcount > 0)
            {
                DisplayMessage("Log Saved");
            }
            else
            {
                DisplayMessage("Log Not Saved");
            }

            dtStatus.Rows.Add("Log Download Process Finished", DateTime.Now.ToString());

            FillLogStatus(dtStatus);

        }
        else
        {
            DisplayMessage("No Log Data Exists");

            return;
        }




        //bool Connect = false;


        //// 
        //bool IsAddNewUser = false;
        //string BrandId = string.Empty;
        //string CompanyId = string.Empty;
        //string LocationId = string.Empty;
        //string DeptId = string.Empty;
        //DeptId = "0";
        //LocationId = "0";
        //BrandId = "0";
        //CompanyId = Session["CompId"].ToString();
        //BrandId = Session["BrandId"].ToString();
        //LocationId = Session["LocId"].ToString();
        //DataTable dtEmp = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString());
        //if (IsAddNewUser)
        //{
        //    DataTable dtUserTemp = objDeviceCon.GetUser(ip, Convert.ToInt32(port));
        //    DataTable dtUser = new DataTable();
        //    DataTable dtFaceTemp = objDeviceCon.GetUserFace(ip, Convert.ToInt32(port));
        //    DataTable dtFingertemp = objDeviceCon.GetUserFinger(ip, Convert.ToInt32(port));
        //    if (dtUserTemp.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dtUserTemp.Rows.Count; i++)
        //        {
        //            dtUser = new DataView(dtEmp, "Emp_Code='" + dtUserTemp.Rows[i]["sdwEnrollNumber"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        //            if (dtUser.Rows.Count == 0)
        //            {
        //                string EnrollNo = dtUserTemp.Rows[i]["sdwEnrollNumber"].ToString();
        //                string DeviceEmpName = dtUserTemp.Rows[i]["sName"].ToString();
        //                string Password = dtUserTemp.Rows[i]["sPassword"].ToString();
        //                string EmpCode = dtUserTemp.Rows[i]["sdwEnrollNumber"].ToString();

        //                string Privilege = dtUserTemp.Rows[i]["iPrivilege"].ToString();
        //                string DeviceId = deviceid; ;
        //                string senabled = dtUserTemp.Rows[i]["bEnabled"].ToString();
        //                string CardNo = dtUserTemp.Rows[i]["sCardNumber"].ToString();

        //                int b = 0;
        //                b = objEmp.InsertEmployeeMaster(CompanyId, DeviceEmpName, DeviceEmpName, EmpCode, "", BrandId, LocationId, DeptId, "1", "1", "1", "1", "1", DateTime.Now.ToString(), DateTime.Now.ToString(), "On Role", DateTime.Now.ToString(), "Male", "", false.ToString(), "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), "SuperAdmin", DateTime.Now.ToString(), "SuperAdmin", DateTime.Now.ToString(), "", "");
        //                if (b != 0)
        //                {
        //                    objEmpParam.DeleteEmployeeParameterByEmpId(b.ToString());
        //                    objEmpParam.InsertEmployeeParameterOnEmployeeInsert(CompanyId, b.ToString(), "SuperAdmin", DateTime.Now.ToString());

        //                    int c = 0;
        //                    int d = 0;
        //                    int f = 0;
        //                    bool IsIndemnity = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsIndemnity", Session["CompId"].ToString()));
        //                    int IndemnityYear = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("IndemnityYear", Session["CompId"].ToString()));
        //                    string IndemnityDays = objAppParam.GetApplicationParameterValueByParamName("IndemnityDayas", Session["CompId"].ToString());

        //                    objEmpParam.DeleteEmployeeParameterByEmpIdNew(b.ToString());
        //                    c = objEmpParam.InsertEmpParameterNew(b.ToString(), Session["CompId"].ToString(), "IsIndemnity", IsIndemnity.ToString(), "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        //                    d = objEmpParam.InsertEmpParameterNew(b.ToString(), Session["CompId"].ToString(), "IndemnityYear", IndemnityYear.ToString(), "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        //                    f = objEmpParam.InsertEmpParameterNew(b.ToString(), Session["CompId"].ToString(), "Indemenity_SalaryCalculationType", IndemnityDays.ToString(), "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());


        //                    //Add New Code for Device On 16-04-2015
        //                    DataTable dtDevice = new DataTable();
        //                    string EmpSync = objAppParam.GetApplicationParameterValueByParamName("Employee Synchronization", Session["CompId"].ToString());
        //                    if (EmpSync == "Company")
        //                    {
        //                        dtDevice = objDevice.GetDeviceMaster(Session["CompId"].ToString());

        //                        if (dtDevice.Rows.Count > 0)
        //                        {
        //                            objSer.DeleteUserTransfer(b.ToString());
        //                            for (int D = 0; D < dtDevice.Rows.Count; D++)
        //                            {
        //                                objSer.InsertUserTransfer(b.ToString(), dtDevice.Rows[D]["Device_Id"].ToString(), false.ToString(), DateTime.Now.ToString(), "1/1/1900", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        dtDevice = objDevice.GetDeviceMaster(Session["CompId"].ToString());
        //                        dtDevice = new DataView(dtDevice, "Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        //                        if (dtDevice.Rows.Count > 0)
        //                        {
        //                            objSer.DeleteUserTransfer(b.ToString());
        //                            for (int D = 0; D < dtDevice.Rows.Count; D++)
        //                            {
        //                                objSer.InsertUserTransfer(b.ToString(), dtDevice.Rows[D]["Device_Id"].ToString(), false.ToString(), DateTime.Now.ToString(), "1/1/1900", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        //                            }
        //                        }
        //                    }
        //                }

        //                string FingerTemplate = string.Empty;
        //                string iflag = string.Empty;
        //                string faceindex = string.Empty;
        //                string facedata = string.Empty;
        //                string facelength = string.Empty;
        //                string fingerindex = string.Empty;

        //                //

        //                if (dtFingertemp.Rows.Count > 0)
        //                {
        //                    dtFingertemp = new DataView(dtFingertemp, "sdwEnrollNumber='" + EmpCode + "'", "", DataViewRowState.CurrentRows).ToTable();
        //                    try
        //                    {
        //                        FingerTemplate = dtFingertemp.Rows[0]["sTmpData"].ToString();
        //                        iflag = dtFingertemp.Rows[0]["iFlag"].ToString();
        //                        fingerindex = dtFingertemp.Rows[0]["idwFingerIndex"].ToString();
        //                        senabled = dtFingertemp.Rows[0]["sEnabled"].ToString();
        //                    }
        //                    catch
        //                    {

        //                    }
        //                    objEmpInfo.UpdateAccessControlFingerInfo(b.ToString(), CardNo, Password, Privilege, FingerTemplate, facedata, faceindex, facelength, fingerindex, iflag, senabled, false.ToString(), false.ToString());
        //                }

        //                //
        //                if (dtFaceTemp.Rows.Count > 0)
        //                {
        //                    dtFaceTemp = new DataView(dtFaceTemp, "sUSERID='" + EmpCode + "'", "", DataViewRowState.CurrentRows).ToTable();

        //                    try
        //                    {
        //                        faceindex = dtFaceTemp.Rows[0]["iFaceIndex"].ToString();
        //                        facedata = dtFaceTemp.Rows[0]["sTmpData"].ToString();
        //                        facelength = dtFaceTemp.Rows[0]["iLength"].ToString();
        //                        senabled = dtFaceTemp.Rows[0]["bEnabled"].ToString();
        //                    }
        //                    catch
        //                    {

        //                    }
        //                    objEmpInfo.UpdateAccessControlFaceInfo(b.ToString(), CardNo, Password, Privilege, FingerTemplate, facedata, faceindex, facelength, fingerindex, iflag, senabled, false.ToString(), false.ToString());
        //                }
        //                //
        //            }
        //        }
        //    }
        //}
        //




        pnlList.Visible = true;
        pnlDeviceOp.Visible = false;
        //Session["IPForOp"] = ip;
        //Session["PortForOp"] = port;
        //Session["DeviceIdForOp"] = gvDevice.DataKeys[index]["Device_Id"].ToString();

    }
    public void FillLogStatus(DataTable dtStatus)
    {

        objPageCmn.FillData((object)gvLogStatus, dtStatus, "", "");

    }
    #endregion
    protected void chkheaderdevice_OnCheckedChanged(object sender, EventArgs e)
    {
        bool ChkStatus = ((CheckBox)gvDevice.HeaderRow.FindControl("chkheaderdevice")).Checked;


        foreach (GridViewRow gvr in gvDevice.Rows)
        {

            ((CheckBox)gvr.FindControl("chkdevice")).Checked = ChkStatus;



        }



    }
}
