using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.IO;
using ClosedXML.Excel;
using System.Net.NetworkInformation;
using PryceDevicesLib;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

public partial class Device_DownloadUser : BasePage
{
    IAttDeviceOperation objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(TechType.PULL, "zkTecho");
    Att_DeviceMaster objDevice = null;
    Common cmn = null;
    SystemParameter objSys = null;
    EmployeeMaster objEmp = null;
    Ping myPing = new Ping();
    //Device_Operation_Lan objDeviceOp = new Device_Operation_Lan();
    EmployeeInformation objEmpInfo = null;
    EmployeeParameter objEmpParam = null;
    Set_ApplicationParameter objAppParam = null;
    LocationMaster ObjLocationMaster = null;
    RoleDataPermission objRoleData = null;
    RoleMaster objRole = null;
    Set_Location_Department objLocDept = null;
    Att_AttendanceLog objAttlog = null;
    Att_Employee_Notification objEmpNotice = null;
    hr_laborLaw_leave ObjLabourLeavedetail = null;
    Att_Employee_Leave objEmpleave = null;
    LeaveMaster_deduction ObjLeavededuction = null;
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
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        //Device_Operation_Lan objDeviceOp = new Device_Operation_Lan();
        objEmpInfo = new EmployeeInformation(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        objAttlog = new Att_AttendanceLog(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());
        ObjLabourLeavedetail = new hr_laborLaw_leave(Session["DBConnection"].ToString());
        objEmpleave = new Att_Employee_Leave(Session["DBConnection"].ToString());
        ObjLeavededuction = new LeaveMaster_deduction(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        Page.Title = objSys.GetSysTitle();

        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Device/Downloaduser.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            FillddlLocation();
            FillGrid();
            pnlDeviceOp.Visible = false;

        }
        //AllPageCode();
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


    protected void lnkViewDetail_Command(object sender, CommandEventArgs e)
    {
        GridViewRow gvrow = (GridViewRow)((LinkButton)sender).Parent.Parent;

        string port = ((Label)gvrow.FindControl("lblport")).Text;
        string IP = ((Label)gvrow.FindControl("lblip")).Text;
        string DeviceId = ((Label)gvrow.FindControl("lblDeviceId1")).Text;
        string strCommunicationType = ((Label)gvrow.FindControl("lblCommunicationType")).Text;
        string strMake = ((Label)gvrow.FindControl("lblMake")).Text;
        objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(strCommunicationType == "PULL" ? TechType.PULL : TechType.PUSH, strMake);

        int adminCnt = 0;
        int userCount = 0;
        int fpCnt = 0;
        int recordCnt = 0;
        int pwdCnt = 0;
        int oplogCnt = 0;
        int faceCnt = 0;

        bool b = false;

        // b = Common.IsAlive(IP);
        b = objDeviceOp.Device_Connection(IP, port, 0);

        if (b == true)
        {

            clsDeviceCapacity objdeviceCapacity = objDeviceOp.GetCapacityInfo(IP, port);
            ((Label)gvrow.FindControl("lblUserCount")).Text = objdeviceCapacity.userCount.ToString();
            ((Label)gvrow.FindControl("lblLogCount")).Text = objdeviceCapacity.logCount.ToString();
            ((Label)gvrow.FindControl("lblFaceCount")).Text = objdeviceCapacity.faceCount.ToString();
            ((Label)gvrow.FindControl("lblFingerCount")).Text = objdeviceCapacity.fingerCount.ToString();
            ((Label)gvrow.FindControl("lblPasswordCount")).Text = objdeviceCapacity.passwordCount.ToString();

            gvrow.BackColor = System.Drawing.ColorTranslator.FromHtml("#32CD32");
            ((Label)gvrow.FindControl("lblStatus")).Text = "Connected";
        }
        else
        {
            ((Label)gvrow.FindControl("lblUserCount")).Text = "0";
            ((Label)gvrow.FindControl("lblLogCount")).Text = "0";
            ((Label)gvrow.FindControl("lblFaceCount")).Text = "0";
            ((Label)gvrow.FindControl("lblFingerCount")).Text = "0";
            ((Label)gvrow.FindControl("lblPasswordCount")).Text = "0";

            gvrow.BackColor = System.Drawing.ColorTranslator.FromHtml("#FA8072");
            ((Label)gvrow.FindControl("lblStatus")).Text = "Disconnected";

        }


    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSaveSelected.Visible = clsPagePermission.bHavePermission;
        btnSaveSelected_1.Visible = clsPagePermission.bHavePermission;
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
        objPageCmn.FillData((object)gvSourceDevice, dt, "", "");
        objPageCmn.FillData((object)gvDestinationDevice, dt, "", "");
        objPageCmn.FillData((object)listEmpDevice, dt, "DeviceName_Ip", "Device_Id");
        Session["dtFilter_Download_U"] = dt;
        Session["Device"] = dt;

        //AllPageCode();
    }

    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    protected void gvDevice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDevice.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Download_U"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvDevice, dt, "", "");
    }
    protected void gvDevice_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Download_U"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Download_U"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvDevice, dt, "", "");
    }


    private class clsUserList : clsUser
    {
        public string deviceId { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
        public string empId { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string DeviceName { get; set; }
        public string Finger { get; set; }
        public string Face { get; set; }
        public string CommunicationType { get; set; }
        public string deviceMake { get; set; }
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        int adminCnt = 0;
        int userCount = 0;
        int fpCnt = 0;
        int recordCnt = 0;
        int pwdCnt = 0;
        int oplogCnt = 0;
        int faceCnt = 0;
        DataTable dtFingertemp = new DataTable();
        DataTable dtFaceTemp = new DataTable();

        if (((Button)sender).ID == "btnDownloadAllUser")
        {
            rbtnAll.Checked = true;
            rbtnNew.Checked = false;
        }
        else
        {
            rbtnAll.Checked = false;
            rbtnNew.Checked = true;
        }

        List<clsUserFinger> objFingerInfo = new List<clsUserFinger> { };
        List<clsUserFace> objFaceInfo = new List<clsUserFace> { };

        bool bIs = false;
        bool IsDeviceSelected = false;

        DataTable dtEmp = objEmp.GetEmployeeMasterByCompanyOnly(Session["CompId"].ToString());

        //dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        DataTable dtUser = new DataTable();
        foreach (GridViewRow gvdevicerow in gvDevice.Rows)
        {
            if (((CheckBox)gvdevicerow.FindControl("chkSelectDevice")).Checked)
            {
                //objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(TechType.PUSH, "a7Plus");
                IsDeviceSelected = true;
                IsDeviceSelected = true;
                int index = gvdevicerow.RowIndex;
                string port = gvDevice.DataKeys[index]["Port"].ToString();
                string IP = gvDevice.DataKeys[index]["IP_Address"].ToString();
                string DeviceId = gvDevice.DataKeys[index]["Device_Id"].ToString();
                string DeviceName = gvDevice.DataKeys[index]["Device_Name"].ToString();
                string strCommunicationType = gvDevice.DataKeys[index]["Communication_Type"].ToString();
                string strMake = gvDevice.DataKeys[index]["field2"].ToString();
                objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(strCommunicationType == "PULL" ? TechType.PULL : TechType.PUSH, strMake);

                bIs = objDeviceOp.Device_Connection(IP, port, 0);
                if (bIs)
                {


                    DataTable dtUserTemp = new DataTable();

                    if (chkDownloadFace.Checked || chkDownloadFinger.Checked)
                    {
                        clsDeviceCapacity objdevicecapacity = objDeviceOp.GetCapacityInfo(IP, port);

                        if (objdevicecapacity.userCount == 0)
                        {
                            continue;
                        }

                        if (objdevicecapacity.fingerCount > 0 && chkDownloadFinger.Checked)
                        {
                            objFingerInfo = objDeviceOp.GetUserFinger(IP, port);
                        }

                        if (objdevicecapacity.faceCount > 0 && chkDownloadFace.Checked)
                        {

                            objFaceInfo = objDeviceOp.GetUserFace(IP, port);
                        }
                    }


                    List<clsUser> ObjUser = objDeviceOp.GetUser(IP, port);

                    if (ObjUser.Count > 0)
                    {

                        Session["FingerData"] = objFingerInfo;
                        Session["FaceData"] = objFaceInfo;
                        Session["DeviceUser"] = ObjUser;


                        List<clsUserList> objuserlist = new List<clsUserList> { };
                        List<clsUserFinger> objuseFinger = new List<clsUserFinger> { };
                        List<clsUserFace> objuseFace = new List<clsUserFace> { };
                        foreach (clsUser cls in ObjUser)
                        {
                            clsUserList clsVal = new clsUserList();
                            clsVal.enrollNumber = cls.enrollNumber;
                            clsVal.name = cls.name;
                            clsVal.cardNumber = cls.cardNumber;
                            clsVal.privilege = cls.privilege;
                            clsVal.enabled = cls.enabled;
                            clsVal.password = cls.password;
                            clsVal.deviceId = DeviceId;
                            clsVal.IP = IP;
                            clsVal.Port = port;
                            clsVal.DeviceName = DeviceName;
                            clsVal.empId = "0";
                            clsVal.Designation = "";
                            clsVal.Department = "";
                            clsVal.CommunicationType = strCommunicationType;
                            clsVal.deviceMake = strMake;
                            DataRow[] drEmp = dtEmp.Select("emp_code='" + cls.enrollNumber + "'");
                            if (drEmp.Length > 0)
                            {
                                clsVal.empId = drEmp[0]["emp_id"].ToString();
                                clsVal.Designation = drEmp[0]["Designation"].ToString();
                                clsVal.Department = drEmp[0]["Department"].ToString();
                            }

                            clsVal.Finger = "False";
                            clsVal.Face = "False";

                            objuseFinger = objFingerInfo.Where(w => w.enrollNumber == cls.enrollNumber).ToList();

                            if (objuseFinger.Count > 0)
                            {
                                if (objuseFinger[0].name != null)
                                {
                                    clsVal.name = objuseFinger[0].name;
                                }
                                if (objuseFinger.Where(w => w.tmpData != null).ToList().Count > 0)
                                {
                                    clsVal.Finger = "True";
                                }
                            }

                            objuseFace = objFaceInfo.Where(w => w.enrollNumber == cls.enrollNumber).ToList();

                            if (objuseFace.Count > 0)
                            {
                                clsVal.Face = "True";
                            }

                            objuserlist.Add(clsVal);
                        }

                        dtUserTemp = new Common(Session["DBConnection"].ToString()).ListToDataTable(objuserlist);
                        dtUser.Merge(dtUserTemp);
                    }
                }
            }
        }

        if (rbtnNew.Checked)
        {
            try
            {
                dtUser = new DataView(dtUser, "empid='0'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
                dtUser = null;
            }
        }
        if (dtUser != null && dtUser.Rows.Count > 0)
        {


            Session["DtDeviceUser"] = dtUser;
            Session["DtDeviceUserFilter"] = dtUser;
            //Common Function add By Lokesh on 23-05-2015
            dtUser = new DataView(dtUser, "", "enrollNumber asc", DataViewRowState.CurrentRows).ToTable();
            objPageCmn.FillData((object)gvUser, dtUser, "", "");
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtUser.Rows.Count.ToString() + "";
            DisplayMessage(dtUser.Rows.Count.ToString() + " " + "Users Downloaded");
            pnlDeviceOp.Visible = true;
            Btn_Div_General_Info.Attributes.Add("Class", "fa fa-plus");
            Div_Device_Download.Attributes.Add("Class", "box box-warning box-solid collapsed-box");
            gvUser.Columns[6].Visible = false;
            gvUser.Columns[7].Visible = false;
            if (chkDownloadFace.Checked)
            {
                gvUser.Columns[6].Visible = true;
            }
            if (chkDownloadFinger.Checked)
            {
                gvUser.Columns[7].Visible = true;
            }

        }
        else
        {
            if (!IsDeviceSelected)
            {
                DisplayMessage("Please Select Device");
            }
            else
            {
                DisplayMessage("User does not exists");

            }
        }
    }

    protected void btnexport_Click(object sender, EventArgs e)
    {
        //GetReport();


        if (gvUser.Rows.Count > 0)
        {
            gvUser.Columns[0].Visible = false;

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=DeviceInformation.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                divexport.RenderControl(hw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
            gvUser.Columns[0].Visible = true;

        }
        else
        {
            DisplayMessage("No Record Available");
        }



    }


    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the runtime error "  
        //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
    }



    protected void btnSaveSelected_Click(object sender, EventArgs e)
    {
        bool Isoldcode = true;
        string strButtonId = ((Button)sender).ID;

        if (strButtonId == "btnSaveSelected_1")
        {
            Isoldcode = false;
        }
        string strEmpId = string.Empty;
        lblSelectedRecord.Text = "";
        int TotalUser_Affected = 0;
        int TotalFace_Affected = 0;
        int TotalFinger_Affected = 0;
        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();
        DataTable dt = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());
        int FinancialYearMonth = 0;
        if (dt.Rows.Count > 0)
        {
            FinancialYearMonth = int.Parse(dt.Rows[0]["Param_Value"].ToString());
        }

        if (DateTime.Now.Month < FinancialYearMonth)
        {
            FinancialYearStartDate = new DateTime(DateTime.Now.Year - 1, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        else
        {
            FinancialYearStartDate = new DateTime(DateTime.Now.Year, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }

        string strLabourLaw = string.Empty;

        strLabourLaw = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), Session["LocId"].ToString()).Rows[0]["Field3"].ToString();



        bool Result = false;


        foreach (GridViewRow gvrow in gvUser.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkSel")).Checked)
            {
                Result = true;
                break;
            }
        }


        if (!Result)
        {
            DisplayMessage("Select at least one record");
            return;
        }

        DataTable dtdeviceinfo = new DataTable();
        dtdeviceinfo.Columns.Add("Device_id");
        dtdeviceinfo.Columns.Add("Emp_Code");
        dtdeviceinfo.Columns.Add("Face");
        dtdeviceinfo.Columns.Add("Finger");
        DataTable dtFingertemp = new DataTable();
        DataTable dtFaceTemp = new DataTable();
        DataTable dtFacTep = new DataTable();
        DataTable dtEmpDt = new DataTable();
        DataTable dtFtemp = new DataTable();
        string optype = "1";
        DataTable dtUser = new DataTable();
        dtUser.Columns.Add("sdwEnrollNumber");
        dtUser.Columns.Add("sName");
        dtUser.Columns.Add("sPassWord");
        dtUser.Columns.Add("iPrivilege");
        dtUser.Columns.Add("sTmpData");
        dtUser.Columns.Add("sCardNumber");
        dtUser.Columns.Add("Emp_Id");
        dtUser.Columns.Add("IP");
        dtUser.Columns.Add("Port");
        dtUser.Columns.Add("Device_Id");
        dtUser.Columns.Add("sEnabled");
        dtUser.Columns.Add("sCommunication_Type");
        dtUser.Columns.Add("sMake");
        for (int rowcount = 0; rowcount < gvUser.Rows.Count; rowcount++)
        {
            if (((CheckBox)gvUser.Rows[rowcount].FindControl("chkSel")).Checked)
            {
                string EnrollNo = gvUser.DataKeys[rowcount]["enrollNumber"].ToString();

                string Password = gvUser.DataKeys[rowcount]["password"].ToString();
                string EmpId = gvUser.DataKeys[rowcount]["EmpId"].ToString();
                string empname = gvUser.DataKeys[rowcount]["name"].ToString();
                string Privilege = gvUser.DataKeys[rowcount]["privilege"].ToString();
                string FingerTemplate = string.Empty;
                string DeviceId = gvUser.DataKeys[rowcount]["deviceId"].ToString();
                string ip = gvUser.DataKeys[rowcount]["IP"].ToString();
                string Port = gvUser.DataKeys[rowcount]["Port"].ToString();

                string iflag = string.Empty;
                string senabled = gvUser.DataKeys[rowcount]["enabled"].ToString();
                string CardNo = gvUser.DataKeys[rowcount]["cardNumber"].ToString();
                string strCommunicationType = gvUser.DataKeys[rowcount]["CommunicationType"].ToString();
                string strMake = gvUser.DataKeys[rowcount]["deviceMake"].ToString();

                DataRow dr = dtUser.NewRow();
                dr["sdwEnrollNumber"] = EnrollNo;
                dr["sName"] = empname;
                dr["sPassword"] = Password;
                dr["iPrivilege"] = Privilege;
                dr["sEnabled"] = senabled;
                dr["sCardNumber"] = CardNo;
                dr["Emp_Id"] = EmpId;
                dr["Device_Id"] = DeviceId;
                dr["IP"] = ip;
                dr["Port"] = Port;
                dr["sEnabled"] = senabled;
                dr["sCommunication_Type"] = strCommunicationType;
                dr["sMake"] = strMake;
                dtUser.Rows.Add(dr);
            }
        }

        DataTable dtFinger = new DataTable();
        DataTable dtFace = new DataTable();
        DataTable dtDistinctDevice = new DataTable();
        try
        {
            dtDistinctDevice = dtUser.DefaultView.ToTable(true, "Device_Id", "sCommunication_Type", "sMake");
        }
        catch (Exception ex)
        {

        }
        for (int devicecounter = 0; devicecounter < dtDistinctDevice.Rows.Count; devicecounter++)
        {
            objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(dtDistinctDevice.Rows[devicecounter]["sCommunication_Type"].ToString() == "PULL" ? TechType.PULL : TechType.PUSH, dtDistinctDevice.Rows[devicecounter]["sMake"].ToString());

            DataTable dtUserByDevice = new DataView(dtUser, "Device_Id='" + dtDistinctDevice.Rows[devicecounter][0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtUserByDevice.Rows.Count > 0)
            {
                string ip = dtUserByDevice.Rows[0]["IP"].ToString();
                string Port = dtUserByDevice.Rows[0]["Port"].ToString();
                bool IsDeviceConnected = false;

                List<clsUserFinger> objFingerInfo = new List<clsUserFinger> { };
                List<clsUserFace> objFaceInfo = new List<clsUserFace> { };



                if (objDeviceOp.Device_Connection(ip, Port, 0))
                {
                    IsDeviceConnected = true;
                    if (chkFinger.Checked && chkFace.Checked)
                    {
                        optype = "4";

                        // objFingerInfo = objDeviceOp.GetUserFinger(ip, Port);
                        try
                        {
                            if (chkDownloadFinger.Checked)
                                objFingerInfo = (List<clsUserFinger>)Session["FingerData"];
                            else
                            {
                                if (chkFinger.Checked)
                                {
                                    objFingerInfo = objDeviceOp.GetUserFinger(ip, Port);
                                }
                            }
                        }
                        catch
                        {
                            objFingerInfo = new List<clsUserFinger> { };
                        }

                        if (Isoldcode)
                        {
                            // objFaceInfo = objDeviceOp.GetUserFace(ip, Port);

                            try
                            {
                                if (chkDownloadFace.Checked)
                                    objFaceInfo = (List<clsUserFace>)Session["FaceData"];
                                else
                                {
                                    if (chkFace.Checked)
                                    {
                                        objFaceInfo = objDeviceOp.GetUserFace(ip, Port);
                                    }
                                }
                            }
                            catch
                            {
                                objFaceInfo = new List<clsUserFace> { };
                            }
                        }
                        else
                        {
                            objDeviceOp.Device_Connection(ip, Port, 0);
                        }
                    }

                    else if (chkFace.Checked)
                    {
                        optype = "3";
                        if (Isoldcode)
                        {
                            objFaceInfo = objDeviceOp.GetUserFace(ip, Port);
                            //dtFaceTemp = objDeviceOp.GetUserFace(ip, Convert.ToInt32(Port));
                        }

                    }
                    else if (chkFinger.Checked)
                    {
                        optype = "2";
                        objFingerInfo = objDeviceOp.GetUserFinger(ip, Port);
                    }
                }

                string strLocationId = objDa.return_DataTable("select Location_Id from att_devicemaster where device_id=" + dtDistinctDevice.Rows[devicecounter][0].ToString() + "").Rows[0]["Location_Id"].ToString();

                // Get Employee From Table .......................
                DataTable dtEmpD = objEmp.GetEmployeeMasterByCompanyOnly(Session["CompId"].ToString());
                for (int rowcount = 0; rowcount < dtUserByDevice.Rows.Count; rowcount++)
                {
                    objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
                    //if (rowcount % 50 == 0)
                    //{
                    //    ;
                    //}

                    string EnrollNo = dtUserByDevice.Rows[rowcount]["sdwEnrollNumber"].ToString();
                    string DeviceEmpName = dtUserByDevice.Rows[rowcount]["sName"].ToString();
                    string Password = dtUserByDevice.Rows[rowcount]["sPassword"].ToString();
                    //string EmpId = dtUserByDevice.Rows[rowcount]["Emp_Id"].ToString();
                    string EmpId = dtUserByDevice.Rows[rowcount]["sdwEnrollNumber"].ToString();
                    string strEmpcode = dtUserByDevice.Rows[rowcount]["sdwEnrollNumber"].ToString();
                    string Privilege = dtUserByDevice.Rows[rowcount]["iPrivilege"].ToString();
                    string DeviceId = dtUserByDevice.Rows[rowcount]["Device_Id"].ToString();
                    string senabled = dtUserByDevice.Rows[rowcount]["sEnabled"].ToString();
                    string CardNo = dtUserByDevice.Rows[rowcount]["sCardNumber"].ToString();
                    DataRow drdeviceinfo = dtdeviceinfo.NewRow();
                    drdeviceinfo["Device_id"] = DeviceId;
                    drdeviceinfo["Emp_Code"] = EmpId;
                    drdeviceinfo["Face"] = "False";
                    drdeviceinfo["Finger"] = "False";

                    string FingerTemplate = string.Empty;
                    string iflag = string.Empty;
                    string faceindex = string.Empty;
                    string facedata = string.Empty;
                    string facelength = string.Empty;
                    string fingerindex = string.Empty;
                    if (EmpId.Trim() != "")
                    {
                        try
                        {
                            Convert.ToInt32(EmpId);
                        }
                        catch
                        {
                            continue;
                        }


                        // DataTable dtEmpD = objEmp.GetEmployeeMaster(Session["CompId"].ToString());
                        dtEmpDt = new DataView(dtEmpD, "Emp_Code='" + EmpId + "'", "", DataViewRowState.CurrentRows).ToTable();
                        TotalUser_Affected++;
                        if (dtEmpDt.Rows.Count == 0)
                        {

                            if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud")
                            {
                                MasterDataAccess objMDa = new MasterDataAccess(ConfigurationManager.ConnectionStrings["PegaConnection1"].ToString());
                                MasterDataAccess.clsMasterCompany clsMasterCmp = objMDa.getMasterCompanyInfo(Session["RegistrationCode"].ToString(), ConfigurationManager.AppSettings["masterDbApiBaseAddress"].ToString());
                                int attEmpCount = Convert.ToInt32(objDa.return_DataTable("select count(*) from set_employeemaster where company_id = '" + Session["CompId"].ToString() + "'").Rows[0][0].ToString());
                                if ((attEmpCount + 1) > Convert.ToInt32(clsMasterCmp.no_of_employee.ToString()))
                                {
                                    DisplayMessage("Maximum Employees is exceeded so please update your license");

                                    UC_LicenseInfo.GetLicenseInformation(clsMasterCmp.registration_code, clsMasterCmp.license_key == "" ? "Demo" : clsMasterCmp.license_key, clsMasterCmp.email, Convert.ToDateTime(clsMasterCmp.expiry_date).ToString("dd-MMM-yyyy"), clsMasterCmp.att_device_count.ToString(), clsMasterCmp.user.ToString(), clsMasterCmp.no_of_employee.ToString(), clsMasterCmp.version_type, clsMasterCmp.product_code, ConfigurationManager.AppSettings["master_product_id"].ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_UpdateLicense_Open()", true);
                                    //DisplayMessage("Modal_UpdateLicense_Open()");
                                    return;
                                }
                            }

                            int b = 0;
                            int c = 0;
                            int d = 0;
                            int f = 0;
                            b = objEmp.InsertEmployeeMaster(Session["CompId"].ToString(), DeviceEmpName, DeviceEmpName, EmpId, "", Session["BrandId"].ToString(), strLocationId, "0", "1", "1", "1", "1", "1", "01/01/2000", "01/01/2000", "On Role", DateTime.Now.ToString(), "Male", "Employee", false.ToString(), "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "", "", "", "", false.ToString(), "", "", "", "", "");

                            objEmpParam.DeleteEmployeeParameterByEmpId(b.ToString());
                            objEmpParam.InsertEmployeeParameterOnEmployeeInsert(Session["CompId"].ToString(), b.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "0", Session["BrandId"].ToString(), Session["LocId"].ToString(), Session["TimeZoneId"].ToString());

                            bool IsIndemnity = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsIndemnity", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
                            int IndemnityYear = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("IndemnityYear", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString()));
                            string strIndemnityDays = objAppParam.GetApplicationParameterValueByParamName("IndemnityDayas", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

                            objEmpParam.DeleteEmployeeParameterByEmpIdNew(b.ToString());
                            c = objEmpParam.InsertEmpParameterNew(b.ToString(), Session["CompId"].ToString(), "IsIndemnity", IsIndemnity.ToString(), "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                            d = objEmpParam.InsertEmpParameterNew(b.ToString(), Session["CompId"].ToString(), "IndemnityYear", IndemnityYear.ToString(), "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                            f = objEmpParam.InsertEmpParameterNew(b.ToString(), Session["CompId"].ToString(), "IndemnityDayas", strIndemnityDays, "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                            //Insert For Notification
                            int N = 0;
                            objEmpNotice.DeleteEmployeeNotificationByEmpId(b.ToString());
                            DataTable dtNoti = objEmpNotice.GetAllNotification_By_NOtificationType("Report");
                            if (dtNoti.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtNoti.Rows.Count; i++)
                                {
                                    N = objEmpNotice.InsertEmployeeNotification(b.ToString(), dtNoti.Rows[i]["Notification_Id"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                }
                            }


                        }
                    }

                    strEmpId = GetEmpId(EmpId);

                    if (!lblSelectedRecord.Text.Split(',').Contains(EmpId))
                    {
                        lblSelectedRecord.Text += EmpId + ",";
                    }

                    DataTable dtEmployeeInfo = objEmpInfo.GetEmployeeAccessInfoByEmpId(strEmpId);
                    if (dtEmployeeInfo.Rows.Count > 0)
                    {
                        //here we will update passwordif exists
                        //because when user have only password in case it will not updating in database
                        if (Password != "")
                        {
                            objDa.execute_Command("update set_employeeinformation set Template2='" + Password + "' where emp_id=" + strEmpId + "");
                        }
                        if (CardNo.Trim() != "" && CardNo.Trim() != "0")
                        {
                            objDa.execute_Command("update set_employeeinformation set CardNo='" + CardNo.Trim() + "' where emp_id=" + strEmpId + "");
                        }

                    }
                    else
                    {
                        objEmpInfo.InsertEmployeeInformation(strEmpId, CardNo.Trim(), "0", Password, "", "", "", true.ToString(), true.ToString(), "0", DateTime.Now.ToString(), DateTime.Now.ToString(), "", "0", "1", true.ToString());
                    }

                    if (IsDeviceConnected)
                    {
                        if (optype == "2")
                        {
                            if (objFingerInfo.Count > 0)
                            {
                                List<clsUserFinger> objuseFinger = objFingerInfo.Where(w => w.enrollNumber == EmpId && w.tmpData != null).ToList();

                                //dtFtemp = new DataView(dtFingertemp, "sdwEnrollNumber='" + EmpId + "'", "", DataViewRowState.CurrentRows).ToTable();

                                //Updated Code Start On 25-06-2015
                                if (objuseFinger.Count > 0)
                                {

                                    drdeviceinfo["Finger"] = "True";
                                    objEmpInfo.DeleteEmployeeInformationByEmpId(strEmpId);
                                    foreach (clsUserFinger cls in objuseFinger)
                                    {
                                        FingerTemplate = cls.tmpData;
                                        iflag = "1";
                                        fingerindex = cls.fingerIndex.ToString();
                                        senabled = "True";
                                        Privilege = cls.privilege;
                                        try
                                        {
                                            objEmpInfo.InsertEmployeeInformation(strEmpId, CardNo, Privilege, Password, FingerTemplate, facedata, faceindex, false.ToString(), false.ToString(), facelength, DateTime.Now.ToString(), DateTime.Now.ToString(), "", fingerindex, iflag, senabled);
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        TotalFinger_Affected++;
                                        //objEmpInfo.UpdateAccessControlFingerInfo(GetEmpId(EmpId), CardNo, Password, Privilege, FingerTemplate, facedata, faceindex, facelength, fingerindex, iflag, senabled, false.ToString(), false.ToString());
                                    }
                                }
                                //Updated Code End
                            }
                        }
                        else if (optype == "3")
                        {


                            if (Isoldcode)
                            {
                                if (objFaceInfo.Count > 0)
                                {


                                    List<clsUserFace> objuseFace = objFaceInfo.Where(w => w.enrollNumber == EmpId).ToList();


                                    //dtFacTep = new DataView(dtFaceTemp, "sUSERID='" + EmpId + "'", "", DataViewRowState.CurrentRows).ToTable();

                                    foreach (clsUserFace cls in objuseFace)
                                    {
                                        iflag = "1";
                                        faceindex = cls.faceIndex;
                                        facedata = cls.tmpData;
                                        facelength = cls.length; ;
                                        senabled = cls.enabled;
                                        Privilege = cls.privilege;
                                        drdeviceinfo["Face"] = "True";
                                        TotalFace_Affected++;

                                        objEmpInfo.UpdateAccessControlFaceInfo(strEmpId, CardNo, Password, Privilege, FingerTemplate, facedata, faceindex, facelength, fingerindex, iflag, senabled, false.ToString(), false.ToString(), Session["TimeZoneId"].ToString());
                                    }
                                }
                            }
                            else
                            {
                                List<clsUserFace> objuserface = objDeviceOp.GetSingleUserFace(ip, Port, Convert.ToInt32(strEmpcode));
                                //dtFacTep = objDeviceOp.GetSingleUserFace(ip,Port,strEmpcode);

                                foreach (clsUserFace cls in objuserface)
                                {

                                    try
                                    {
                                        iflag = "1";
                                        faceindex = cls.faceIndex;
                                        facedata = cls.tmpData;
                                        facelength = cls.length;
                                        senabled = cls.enabled;
                                        Privilege = cls.privilege;
                                        drdeviceinfo["Face"] = "True";
                                        TotalFace_Affected++;
                                    }
                                    catch
                                    {
                                    }

                                    objEmpInfo.UpdateAccessControlFaceInfo(strEmpId, CardNo, Password, Privilege, FingerTemplate, facedata, faceindex, facelength, fingerindex, iflag, senabled, false.ToString(), false.ToString(), Session["TimeZoneId"].ToString());

                                }
                            }

                        }
                        else if (optype == "4")
                        {

                            if (objFingerInfo.Count > 0)
                            {
                                List<clsUserFinger> objuseFinger = objFingerInfo.Where(w => w.enrollNumber == EmpId).ToList();

                                //dtFtemp = new DataView(dtFingertemp, "sdwEnrollNumber='" + EmpId + "'", "", DataViewRowState.CurrentRows).ToTable();

                                //Updated Code Start On 25-06-2015
                                if (objuseFinger.Count > 0)
                                {

                                    drdeviceinfo["Finger"] = "True";
                                    objEmpInfo.DeleteEmployeeInformationByEmpId(strEmpId);
                                    foreach (clsUserFinger cls in objuseFinger)
                                    {
                                        FingerTemplate = cls.tmpData;
                                        iflag = "1";
                                        fingerindex = cls.fingerIndex.ToString();
                                        senabled = "True";
                                        Privilege = cls.privilege;
                                        objEmpInfo.InsertEmployeeInformation(strEmpId, CardNo, Privilege, Password, FingerTemplate, facedata, faceindex, false.ToString(), false.ToString(), facelength, DateTime.Now.ToString(), DateTime.Now.ToString(), "", fingerindex, iflag, senabled);
                                        TotalFinger_Affected++;
                                        //objEmpInfo.UpdateAccessControlFingerInfo(GetEmpId(EmpId), CardNo, Password, Privilege, FingerTemplate, facedata, faceindex, facelength, fingerindex, iflag, senabled, false.ToString(), false.ToString());
                                    }
                                }
                                //Updated Code End
                            }



                            // DataTable dtFaceTemp = objDeviceOp.GetUserFace(ip, Convert.ToInt32(Port));
                            if (Isoldcode)
                            {
                                if (objFaceInfo.Count > 0)
                                {


                                    List<clsUserFace> objuseFace = objFaceInfo.Where(w => w.enrollNumber == EmpId).ToList();


                                    //dtFacTep = new DataView(dtFaceTemp, "sUSERID='" + EmpId + "'", "", DataViewRowState.CurrentRows).ToTable();

                                    foreach (clsUserFace cls in objuseFace)
                                    {
                                        iflag = "1";
                                        faceindex = cls.faceIndex;
                                        facedata = cls.tmpData;
                                        facelength = cls.length; ;
                                        senabled = cls.enabled;
                                        Privilege = cls.privilege;
                                        drdeviceinfo["Face"] = "True";
                                        TotalFace_Affected++;

                                        objEmpInfo.UpdateAccessControlFaceInfo(strEmpId, CardNo, Password, Privilege, FingerTemplate, facedata, faceindex, facelength, fingerindex, iflag, senabled, false.ToString(), false.ToString(), Session["TimeZoneId"].ToString());
                                    }
                                }
                            }
                            else
                            {
                                List<clsUserFace> objuserface = objDeviceOp.GetSingleUserFace(ip, Port, Convert.ToInt32(strEmpcode));
                                //dtFacTep = objDeviceOp.GetSingleUserFace(ip,Port,strEmpcode);

                                foreach (clsUserFace cls in objuserface)
                                {

                                    try
                                    {
                                        iflag = "1";
                                        faceindex = cls.faceIndex;
                                        facedata = cls.tmpData;
                                        facelength = cls.length;
                                        senabled = cls.enabled;
                                        Privilege = cls.privilege;
                                        drdeviceinfo["Face"] = "True";
                                        TotalFace_Affected++;
                                    }
                                    catch
                                    {
                                    }

                                    objEmpInfo.UpdateAccessControlFaceInfo(strEmpId, CardNo, Password, Privilege, FingerTemplate, facedata, faceindex, facelength, fingerindex, iflag, senabled, false.ToString(), false.ToString(), Session["TimeZoneId"].ToString());

                                }
                            }
                        }

                    }
                    dtdeviceinfo.Rows.Add(drdeviceinfo);
                }

                if (!Isoldcode && IsDeviceConnected)
                {
                    objDeviceOp.Device_Connection(ip, Port, 1);
                }
            }
        }

        updateFaceFingerInfo(dtdeviceinfo);

        DisplayMessage("Affected User : " + TotalUser_Affected + ",Affected Finger : " + TotalFinger_Affected + ",Affected Face : " + TotalFace_Affected + "");
        //if (((Button)sender).ID == "btnUpload")
        //{
        //    ExportTableData(UploadOperation());
        //}

    }


    public void ExportTableData(DataTable dtdata)
    {
        string strFname = "Useruploadinformation";
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dtdata, strFname);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + strFname + ".xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }

    public void updateFaceFingerInfo(DataTable dtDeviceinfo)
    {
        if (dtDeviceinfo.Rows.Count > 0)
        {
            foreach (GridViewRow gvrow in gvUser.Rows)
            {
                if (((CheckBox)gvrow.FindControl("chkSel")).Checked)
                {
                    if (new DataView(dtDeviceinfo, "Device_Id=" + ((Label)gvrow.FindControl("lblDevId")).Text + " and Emp_Code=" + ((Label)gvrow.FindControl("lblsdwenrollNo")).Text + " and Face='True'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                    {
                        ((Label)gvrow.FindControl("lblFace")).Text = "True";
                    }
                    else
                    {
                        ((Label)gvrow.FindControl("lblFace")).Text = "False";
                    }

                    if (new DataView(dtDeviceinfo, "Device_Id=" + ((Label)gvrow.FindControl("lblDevId")).Text + " and Emp_Code=" + ((Label)gvrow.FindControl("lblsdwenrollNo")).Text + " and Finger='True'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                    {
                        ((Label)gvrow.FindControl("lblFinger")).Text = "True";
                    }
                    else
                    {
                        ((Label)gvrow.FindControl("lblFinger")).Text = "False";
                    }
                }
            }
        }

    }


    public string GetFaceImage(string strStatus)
    {
        string url = string.Empty;
        if (strStatus == "True")
        {
            url = "../Images/Face.jpg";
        }
        else
        {
            url = "../Images/Blank.png";
        }
        return url;
    }


    public string GetFingerImage(string strStatus)
    {
        string url = string.Empty;
        if (strStatus == "True")
        {
            url = "../Images/Finger.png";
        }
        else
        {
            url = "../Images/Blank.png";
        }
        return url;
    }

    //Updated Code For Download Log On 29-06-2015
    protected void btnDownloadLog_Click(object sender, EventArgs e)
    {
        //Get Selected Employees
        //string strEmpIds = string.Empty;
        //if (strEmpIds == "")
        //{
        foreach (GridViewRow gvr in gvUser.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvr.FindControl("chkSel");
            Label lblEmpId = (Label)gvr.FindControl("lblEnrollNo");
            Label lblDevId = (Label)gvr.FindControl("lblDevId");
            Label lblIP = (Label)gvr.FindControl("lblIp");
            Label lblPort = (Label)gvr.FindControl("lblPort");

            bool Connect = false;
            if (chkSelect.Checked)
            {
                //strEmpIds += "'" + lblEmpId.Text.Trim().ToString() + "'" + ",";                    
                Connect = objDeviceOp.Device_Connection(lblIP.Text, lblPort.Text, 0);

                if (Connect == true)
                {
                    DataTable dtLog = new DataTable();
                    List<clsUserLog> objuserlog = objDeviceOp.GetUserLog(lblIP.Text, lblPort.Text);

                    if (objuserlog.Count > 0)
                    {
                        objuserlog = objuserlog.Where(w => w.enrollNumber == lblEmpId.Text).ToList();
                        //dtLog = new DataView(dtLog, "sdwEnrollNumber='" + lblEmpId.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                        int b = 0;
                        if (objuserlog.Count > 0)
                        {
                            foreach (clsUserLog cls in objuserlog)
                            {
                                b = objAttlog.InsertAttendanceLog(Session["CompId"].ToString(), GetEmpId(cls.enrollNumber), lblDevId.Text, Convert.ToDateTime(cls.logTime).ToString("MM/dd/yyyy"), Convert.ToDateTime(cls.logTime).ToString(), cls.inOutMode, "In", "By Device", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                            }
                        }
                    }
                }
            }
        }
        DisplayMessage("Log Saved");
        //}
    }
    public string GetEmpId(string empcode)
    {
        string empId = "0";
        DataTable dt = objDa.return_DataTable("select Emp_Id from Set_EmployeeMaster where Company_Id=" + Session["CompId"].ToString() + " and Emp_Code='" + empcode.ToString().Trim() + "'");

        if (dt.Rows.Count > 0)
        {
            empId = dt.Rows[0]["Emp_Id"].ToString();
        }
        return empId;
    }
    protected void chkSelAll_CheckedChanged(object sender, EventArgs e)
    {
        bool chk = ((CheckBox)gvUser.HeaderRow.FindControl("chkSelAll")).Checked;
        foreach (GridViewRow gvrow in gvUser.Rows)
        {
            ((CheckBox)gvrow.FindControl("chkSel")).Checked = chk;
        }
    }
    protected void gvUser_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvUser.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["DtDeviceUser"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvUser, dt, "", "");
    }
    protected void chkSelAll_CheckedChanged1(object sender, EventArgs e)
    {
        bool b = ((CheckBox)sender).Checked;
        foreach (GridViewRow gvrow in gvDevice.Rows)
        {
            ((CheckBox)gvrow.FindControl("chkSelectDevice")).Checked = b;
        }
    }
    protected void lnkBackFromManage_Click(object sender, EventArgs e)
    {
        Session["IPforManage"] = "";
        Session["PortForManage"] = "";
        pnlDeviceOp.Visible = false;
        foreach (GridViewRow gvrow in gvDevice.Rows)
        {
            ((CheckBox)gvDevice.HeaderRow.FindControl("chkSelAll")).Checked = false;
            ((CheckBox)gvrow.FindControl("chkSelectDevice")).Checked = false;
        }
    }
    public void SaveLeave(string Edit, string LeaveTypeId, string EmpId, string SchType, string AssignLeave, string PaidLeave, string IsYearCarry, string PrevSchduleType, string PrevAssignLeave, string TransNo, string IsRule, DateTime FinancialYearStartDate, DateTime FinancialYearEndDate)
    {


        //code commneted by jitendra on 24-05-2017

        //new code wrote for proper leave assigning according date of joining

        /// means employee joined in january but hr forgot to assign leave in that case system assigning leave according current month instead of joining month and year 


        //code start

        DateTime dtJoining = new DateTime();
        DataTable dtEmp = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), EmpId);
        if (dtEmp.Rows.Count > 0)
        {
            dtJoining = Convert.ToDateTime(dtEmp.Rows[0]["DOJ"].ToString());

        }
        else
        {
            return;
        }


        if (dtJoining > DateTime.Now)
        {
            return;
        }






        double TotalAssignLeave = Convert.ToDouble(AssignLeave);
        double TotalpaidLeave = Convert.ToDouble(PaidLeave);

        if (SchType == "Yearly")
        {

            if (dtJoining >= FinancialYearStartDate)
            {
                int month = 1 + (FinancialYearEndDate.Month - dtJoining.Month) + 12 * (FinancialYearEndDate.Year - dtJoining.Year);

                if (Convert.ToBoolean(IsRule) == false)
                {
                    TotalAssignLeave = (TotalAssignLeave / 12) * month;

                    TotalAssignLeave = Math.Round(TotalAssignLeave);

                    TotalpaidLeave = (TotalpaidLeave / 12) * month;

                    TotalpaidLeave = Math.Round(TotalpaidLeave);
                }
            }

        }

        //here we are deleting previous row for same leave type




        if (SchType == "Monthly")
        {
            if (TransNo.Trim() != "")
            {

                objEmpleave.DeleteEmployeeLeaveTransByEmpIdandleaveTypeId(EmpId, LeaveTypeId, DateTime.Now.Month.ToString(), FinancialYearStartDate.Year.ToString());

            }


            objEmpleave.InsertEmployeeLeaveTrans(Session["CompId"].ToString(), EmpId, LeaveTypeId, FinancialYearStartDate.Year.ToString(), DateTime.Now.Month.ToString(), "0", TotalAssignLeave.ToString(), TotalAssignLeave.ToString(), "0", TotalAssignLeave.ToString(), "0", TotalpaidLeave.ToString(), TotalpaidLeave.ToString(), "Open", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        }
        else
        {

            int totalLogPostedCount = Convert.ToInt32(objDa.return_DataTable("SELECT COUNT(*) from pay_employee_attendance where Emp_Id='" + EmpId + "' and DATEADD(year, pay_employee_attendance.year-1900, DATEADD(month, pay_employee_attendance.Month-1, DATEADD(day, 1-1, 0)))>='" + FinancialYearStartDate + "' and DATEADD(year, pay_employee_attendance.year-1900, DATEADD(month, pay_employee_attendance.Month-1, DATEADD(day, 1-1, 0)))<='" + FinancialYearEndDate + "'").Rows[0][0].ToString());

            double Remainingdays = 0;



            Remainingdays = ((Convert.ToDouble(AssignLeave) / 12) * totalLogPostedCount);


            if (Remainingdays.ToString().Contains("."))
            {
                Remainingdays = Convert.ToDouble(Remainingdays.ToString().Split('.')[0].ToString());
            }



            if (TransNo.Trim() != "")
            {

                objEmpleave.DeleteEmployeeLeaveTransByEmpIdandleaveTypeId(EmpId, LeaveTypeId, "0", FinancialYearStartDate.Year.ToString());

            }

            if (Convert.ToBoolean(IsRule) == true)
            {
                objEmpleave.InsertEmployeeLeaveTrans(Session["CompId"].ToString(), EmpId, LeaveTypeId, FinancialYearStartDate.Year.ToString(), "0", "0", TotalAssignLeave.ToString(), TotalAssignLeave.ToString(), "0", Remainingdays.ToString(), "0", TotalpaidLeave.ToString(), TotalpaidLeave.ToString(), "Open", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
            else
            {
                objEmpleave.InsertEmployeeLeaveTrans(Session["CompId"].ToString(), EmpId, LeaveTypeId, FinancialYearStartDate.Year.ToString(), "0", "0", TotalAssignLeave.ToString(), TotalAssignLeave.ToString(), "0", TotalAssignLeave.ToString(), "0", TotalpaidLeave.ToString(), TotalpaidLeave.ToString(), "Open", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            }
        }

        SystemLog.SaveSystemLog("Employee Master : Leave", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Full Day Leave Saved", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["DBConnection"].ToString());
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        lnkBackFromManage_Click(null, null);
        Div_Device_Download.Visible = true;
        Div_Sync.Visible = false;

        foreach (GridViewRow gvrow in gvSourceDevice.Rows)
        {

            ((CheckBox)gvrow.FindControl("chkSelectDevice")).Checked = false;

        }
        foreach (GridViewRow gvrow in gvDestinationDevice.Rows)
        {
            ((CheckBox)gvDestinationDevice.HeaderRow.FindControl("chkSelAll")).Checked = false;
            ((CheckBox)gvrow.FindControl("chkSelectDevice")).Checked = false;

        }

        rbtnAppend.Checked = false;
        rbtnOverWrite.Checked = true;
        chkFace.Checked = false;
        chkFinger.Checked = false;
        Btn_Div_General_Info.Attributes.Add("Class", "fa fa-minus");
        Div_Device_Download.Attributes.Add("Class", "box box-warning box-solid");
        chkDownloadFace.Checked = false;
        chkDownloadFinger.Checked = false;
    }
    #region synchronization

    protected void btnsync_Click(object sender, EventArgs e)
    {
        Div_Device_Download.Visible = false;
        Div_Sync.Visible = true;
    }

    protected void chkSelAllDestinationDevice_CheckedChanged1(object sender, EventArgs e)
    {
        bool b = ((CheckBox)sender).Checked;
        foreach (GridViewRow gvrow in gvDestinationDevice.Rows)
        {
            ((CheckBox)gvrow.FindControl("chkSelectDevice")).Checked = b;
        }
    }


    protected void chkSelectDevice_CheckedChanged(object sender, EventArgs e)
    {
        GridViewRow GvRow1 = (GridViewRow)((CheckBox)sender).Parent.Parent;
        bool b = ((CheckBox)sender).Checked;
        foreach (GridViewRow gvrow in gvSourceDevice.Rows)
        {
            if (gvrow.RowIndex != GvRow1.RowIndex)
            {
                ((CheckBox)gvrow.FindControl("chkSelectDevice")).Checked = false;
            }
        }
    }

    #endregion    
    #region Filter
    protected void btnbind_Click(object sender, ImageClickEventArgs e)
    {
        if (ddlOption.SelectedIndex != 0)
        {
            string condition = string.Empty;


            if (ddlOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String)='" + txtValue.Text.Trim() + "'";
            }
            else if (ddlOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) like '%" + txtValue.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldName.SelectedValue + ",System.String) Like '" + txtValue.Text.Trim() + "%'";
            }
            DataTable dtCust = (DataTable)Session["DtDeviceUser"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["DtDeviceUserFilter"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvUser, view.ToTable(), "", "");
            //AllPageCode();
        }
        txtValue.Focus();
    }
    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {//need to write code for refreshcode

        objPageCmn.FillData((object)gvUser, (DataTable)Session["DtDeviceUser"], "", "");
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + ((DataTable)Session["DtDeviceUser"]).Rows.Count.ToString() + "";
    }



    protected void gvUser_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["DtDeviceUserFilter"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["DtDeviceUserFilter"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvUser, dt, "", "");
    }
    #endregion
    #region Upload
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        //lblSelectedRecord.Text = "";

        //bool Isdeviceselected = false;

        //foreach (ListItem li in listEmpDevice.Items)
        //{
        //    if (li.Selected)
        //    {
        //        Isdeviceselected = true;
        //        break;
        //    }
        //}

        //if (!Isdeviceselected)
        //{
        //    DisplayMessage("Select at least one device");
        //    return;
        //}

        //foreach (GridViewRow gvrow in gvUser.Rows)
        //{
        //    if (((CheckBox)gvrow.FindControl("chkSel")).Checked)
        //    {
        //        if (!lblSelectedRecord.Text.Split(',').Contains(((Label)gvrow.FindControl("lblsdwenrollNo")).Text))
        //        {
        //            lblSelectedRecord.Text += ((Label)gvrow.FindControl("lblsdwenrollNo")).Text + ",";
        //        }
        //    }
        //}


        //if (lblSelectedRecord.Text.Trim() == "")
        //{
        //    DisplayMessage("Select at least one User");
        //    return;
        //}

        //ExportTableData(UploadOperation());

        //btnSaveSelected_Click(sender, e);
    }

    //public DataTable UploadOperation()
    //{
    //    DataTable dtResult = new DataTable();
    //    dtResult.Columns.Add("Result");
    //    DataTable dtuser = new DataTable();
    //    DataTable dtDbFinger = new DataTable();
    //    DataTable dtDbFace = new DataTable();
    //    if (lblSelectedRecord.Text != "")
    //    {
    //        dtuser = objEmp.GetEmployeeMasterWithDeviceData(Session["CompId"].ToString());
    //        string strselectedempIdList = lblSelectedRecord.Text.Substring(0, lblSelectedRecord.Text.Length - 1);
    //        dtuser = new DataView(dtuser, "Emp_Code in (" + strselectedempIdList + ")", "", DataViewRowState.CurrentRows).ToTable();
    //        if (chkFinger.Checked == true)
    //        {
    //            dtDbFinger = new DataView(dtuser, "Template3 <>''", "", DataViewRowState.CurrentRows).ToTable();
    //        }
    //        if (chkFace.Checked == true)
    //        {
    //            dtDbFace = new DataView(dtuser, "Template4 <>''", "", DataViewRowState.CurrentRows).ToTable();
    //        }


    //        //for user
    //        dtuser.Columns["Emp_Name"].ColumnName = "Name";
    //        dtuser.Columns["Emp_Code"].ColumnName = "UserID";
    //        dtuser.Columns["Template1"].ColumnName = "Privilege";
    //        dtuser.Columns["Template2"].ColumnName = "Password";
    //        dtuser.Columns["CardNo"].ColumnName = "CardNumber";
    //        dtuser.Columns["sEnabled"].ColumnName = "Enabled";

    //        //for finger
    //        dtDbFinger.Columns["Emp_Code"].ColumnName = "sdwEnrollNumber";
    //        dtDbFinger.Columns["Emp_Name"].ColumnName = "sName";
    //        dtDbFinger.Columns["idwFingerIndex"].ColumnName = "idwFingerIndex";
    //        dtDbFinger.Columns["Template2"].ColumnName = "sPassword";
    //        dtDbFinger.Columns["Template1"].ColumnName = "iPrivilege";
    //        dtDbFinger.Columns["Template3"].ColumnName = "sTmpData";
    //        dtDbFinger.Columns["sEnabled"].ColumnName = "sEnabled";
    //        dtDbFinger.Columns["iFlag"].ColumnName = "iFlag";

    //        //for face

    //        dtDbFace.Columns["Emp_Code"].ColumnName = "sUserID";
    //        dtDbFace.Columns["Emp_Name"].ColumnName = "sName";
    //        dtDbFace.Columns["Template2"].ColumnName = "sPassword";
    //        dtDbFace.Columns["Template1"].ColumnName = "iPrivilege";
    //        dtDbFace.Columns["Template5"].ColumnName = "iFaceIndex";
    //        dtDbFace.Columns["Template4"].ColumnName = "sTmpData";
    //        dtDbFace.Columns["Template8"].ColumnName = "iLength";
    //        dtDbFace.Columns["sEnabled"].ColumnName = "bEnabled";

    //    }
    //    foreach (ListItem li in listEmpDevice.Items)
    //    {
    //        if (li.Selected)
    //        {
    //            DataRow drresult = dtResult.NewRow();

    //            DataTable dt = objDevice.GetDeviceMasterById(Session["CompId"].ToString(), li.Value);
    //            string IP = dt.Rows[0]["IP_Address"].ToString();
    //            int port = Convert.ToInt32(dt.Rows[0]["Port"]);
    //            if (objDeviceOp.Device_Connection(IP,port.ToString(), 0))
    //            {
    //                objDeviceOp.UploadCardUserFpFace(dtuser, IP, port, dtDbFinger, dtDbFace, true);

    //                drresult[0] = dt.Rows[0]["Device_Name"].ToString() + "= Uploaded User:" + dtuser.Rows.Count + ",Uploaded Face:" + dtDbFace.Rows.Count + ",Uploaded Finger:" + dtDbFinger.Rows.Count + "";
    //            }
    //            else
    //            {
    //                drresult[0] = dt.Rows[0]["Device_Name"].ToString() + "= Disconnected";

    //            }

    //            dtResult.Rows.Add(drresult);
    //        }
    //    }

    //    return dtResult;
    //}

    #endregion


    protected void btnUserInfo_Click(object sender, EventArgs e)
    {
        Session["DeviceList"] = null;

        string strDeviceId = string.Empty;

        foreach (GridViewRow gvrow in gvDevice.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkSelectDevice")).Checked)
            {
                strDeviceId += ((Label)gvrow.FindControl("lblDeviceId1")).Text + ",";
            }
        }

        Session["DeviceList"] = strDeviceId;
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "New", "window.open('../Attendance_Report/Device_Information_Report.aspx?Type=1','window','width=1024');", true);

    }


    protected void btnconnectdevice_Click(object sender, EventArgs e)
    {

        int adminCnt = 0;
        int userCount = 0;
        int fpCnt = 0;
        int recordCnt = 0;
        int pwdCnt = 0;
        int oplogCnt = 0;
        int faceCnt = 0;

        bool IsConencted = false;
        foreach (GridViewRow gvrow in gvDevice.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkSelectDevice")).Checked)
            {

                ((Label)gvrow.FindControl("lblUserCount")).Text = "0";
                ((Label)gvrow.FindControl("lblLogCount")).Text = "0";
                ((Label)gvrow.FindControl("lblFaceCount")).Text = "0";
                ((Label)gvrow.FindControl("lblFingerCount")).Text = "0";
                ((Label)gvrow.FindControl("lblPasswordCount")).Text = "0";
                gvrow.BackColor = System.Drawing.ColorTranslator.FromHtml("#FA8072");
                ((Label)gvrow.FindControl("lblStatus")).Text = "Disconnected";
                int index = gvrow.RowIndex;
                string port = gvDevice.DataKeys[index]["Port"].ToString();
                string IP = gvDevice.DataKeys[index]["IP_Address"].ToString();
                string strCommunicationType = gvDevice.DataKeys[index]["Communication_Type"].ToString();
                string strMake = gvDevice.DataKeys[index]["field2"].ToString();
                objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(strCommunicationType == "PULL" ? TechType.PULL : TechType.PUSH, strMake);
                PingReply reply = null;
                if (strCommunicationType == "PULL")
                {
                    reply = myPing.Send(IP, 1000);
                }
                else
                {
                    reply = myPing.Send("127.0.0.1", 1000);
                }

                if (reply != null && reply.Status.ToString() == "Success")
                {

                    IsConencted = objDeviceOp.Device_Connection(IP, port, 0);

                    if (IsConencted)
                    {

                        clsDeviceCapacity objdevicecapacity = objDeviceOp.GetCapacityInfo(IP, port);

                        ((Label)gvrow.FindControl("lblUserCount")).Text = objdevicecapacity.userCount.ToString();
                        ((Label)gvrow.FindControl("lblLogCount")).Text = objdevicecapacity.logCount.ToString();
                        ((Label)gvrow.FindControl("lblFaceCount")).Text = objdevicecapacity.faceCount.ToString();
                        ((Label)gvrow.FindControl("lblFingerCount")).Text = objdevicecapacity.fingerCount.ToString();
                        ((Label)gvrow.FindControl("lblPasswordCount")).Text = objdevicecapacity.passwordCount.ToString();

                        gvrow.BackColor = System.Drawing.ColorTranslator.FromHtml("#32CD32");
                        ((Label)gvrow.FindControl("lblStatus")).Text = "Connected";
                    }
                }
            }
        }
    }
}