using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using PryceDevicesLib;
using System.Collections.Generic;
using PegasusDataAccess;
using System.Web;

public partial class Device_DeleteUser : BasePage
{
    //public zkemkeeper.CZKEMClass axCZKEM1 = new zkemkeeper.CZKEMClass();
    IAttDeviceOperation objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(TechType.PULL, "zkTecho");
    Att_DeviceMaster objDevice = null;
    Common cmn = null;
    SystemParameter objSys = null;
    EmployeeMaster objEmp = null;
    //Device_Operation_Lan objDeviceOp = new Device_Operation_Lan();

    Set_ApplicationParameter objAppParam = null;
    LocationMaster ObjLocationMaster = null;
    RoleDataPermission objRoleData = null;
    RoleMaster objRole = null;
    Set_Location_Department objLocDept = null;
    DataAccessClass objda = null;
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
     
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        objda = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        Page.Title = objSys.GetSysTitle();
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "80", Session["CompId"].ToString(), Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            else
            {
                btnSaveSelected.Visible = true;
            }
            FillddlLocation();
            FillGrid();
        }

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
            strPageLevel = objAppParam.GetApplicationParameterValueByParamName("Page Level", Session["CompId"].ToString(),Session["BrandId"].ToString(),Session["LocId"].ToString());
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
        Session["dtFilter_Delete_Usr"] = dt;
        Session["Device"] = dt;
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    protected void gvDevice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDevice.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Delete_Usr"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvDevice, dt, "", "");
    }
    protected void gvDevice_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Delete_Usr"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Delete_Usr"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvDevice, dt, "", "");
    }
    protected void gvUser_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["DtDeviceUser"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["DtDeviceUser"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvUser, dt, "", "");
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

    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        bool bIs = false;
        bool IsDeviceSelected = false;

        DataTable dtEmp = objEmp.GetEmployeeMasterByCompanyOnly(Session["CompId"].ToString());

        //dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        DataTable dtUser = new DataTable();
        foreach (GridViewRow gvdevicerow in gvDevice.Rows)
        {
            if (((CheckBox)gvdevicerow.FindControl("chkSelectDevice")).Checked)
            {
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
                    List<clsUser> ObjUser = objDeviceOp.GetUser(IP, port);
                    if (ObjUser.Count > 0)
                    {
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
                            DataRow[] drEmp = dtEmp.Select("emp_code='" + cls.enrollNumber + "'");
                            if (drEmp.Length > 0)
                            {
                                clsVal.empId = drEmp[0]["emp_id"].ToString();
                                clsVal.Designation = drEmp[0]["Designation"].ToString();
                                clsVal.Department = drEmp[0]["Department"].ToString();
                            }

                            clsVal.Finger = "False";
                            clsVal.Face = "False";
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
                dtUser = new DataView(dtUser, "enrollNumber=''", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch
            {
                dtUser = null;
            }
        }
        if (dtUser != null && dtUser.Rows.Count > 0)
        {

            dtUser = new DataView(dtUser, "", "enrollNumber", DataViewRowState.CurrentRows).ToTable();


            Session["AllDtDeviceUser"] = dtUser;
            Session["DtDeviceUser"] = dtUser;
            Div_Delete.Visible = true;
            Div_Main.Visible = false;

            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvUser, dtUser, "", "");

            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dtUser.Rows.Count.ToString() + "";


            DisplayMessage(dtUser.Rows.Count.ToString() + " " + "Users Downloaded");



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
    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        //FillGrid();
        objPageCmn.FillData((object)gvUser, (DataTable)Session["AllDtDeviceUser"], "", "");

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + ((DataTable)Session["AllDtDeviceUser"]).Rows.Count.ToString() + "";

        ddlOption.SelectedIndex = 2;
        //ddlFieldName.SelectedIndex = 2;
        txtValue.Text = "";

    }
    protected void btnbind_Click(object sender, ImageClickEventArgs e)
    {
        if (txtValue.Text == "")
        {
            DisplayMessage("Please Fill Value");
            txtValue.Focus();
            return;
        }
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
            DataTable dtCust = (DataTable)Session["AllDtDeviceUser"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["DtDeviceUser"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";
            //Common Function add By Lokesh on 21-05-2015
            objPageCmn.FillData((object)gvUser, view.ToTable(), "", "");
            btnRefresh.Focus();
        }
        txtValue.Focus();
    }
    protected void btnDeleteSelected_Click(object sender, EventArgs e)
    {
        string DevicedwBackupNumber = string.Empty;
        DataTable dtFingertemp = new DataTable();
        List<clsUserFinger> objFingerInfo = new List<clsUserFinger> { };
        try
        {
            DevicedwBackupNumber = ConfigurationManager.AppSettings["DevicedwBackupNumber"].ToString();
        }
        catch
        {
            DevicedwBackupNumber = "12";
        }

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
        dtUser.Columns.Add("Communication_Type");
        dtUser.Columns.Add("Make");

        if (chkUser.Checked && chkFinger.Checked && chkFace.Checked)
        {
            optype = "5";
        }

        else if (chkUser.Checked && chkFinger.Checked && !chkFace.Checked)
        {
            optype = "6";
        }
        else if (chkUser.Checked && !chkFinger.Checked && chkFace.Checked)
        {
            optype = "7";
        }
        else if (chkFinger.Checked && chkFace.Checked)
        {
            optype = "4";
        }

        else if (chkFace.Checked)
        {
            optype = "3";
        }
        else if (chkFinger.Checked)
        {
            optype = "2";
        }
        else if (chkUser.Checked)
        {
            optype = "1";
        }

        for (int rowcount = 0; rowcount < gvUser.Rows.Count; rowcount++)
        {

            if (((CheckBox)gvUser.Rows[rowcount].FindControl("chkSel")).Checked)
            {
                string EnrollNo = gvUser.DataKeys[rowcount]["enrollNumber"].ToString();

                string Password = gvUser.DataKeys[rowcount]["password"].ToString();
                string EmpId = gvUser.DataKeys[rowcount]["empId"].ToString();
                string empname = gvUser.DataKeys[rowcount]["name"].ToString();
                string Privilege = gvUser.DataKeys[rowcount]["privilege"].ToString();
                string FingerTemplate = string.Empty;
                string DeviceId = gvUser.DataKeys[rowcount]["deviceId"].ToString();
                string ip = gvUser.DataKeys[rowcount]["IP"].ToString();
                string Port = gvUser.DataKeys[rowcount]["Port"].ToString();
                string iflag = string.Empty;
                string senabled = gvUser.DataKeys[rowcount]["enabled"].ToString();
                string CardNo = gvUser.DataKeys[rowcount]["cardNumber"].ToString();

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
                //gvDevice.DataKeys[rowcount]["Communication_Type"].ToString()
                dr["Communication_Type"] = "PUSH";
                //dr["Make"] = gvDevice.DataKeys[rowcount]["field2"].ToString();
                dr["Make"] = "a7Plus";
                dtUser.Rows.Add(dr);
            }
        }

        DataTable dtFinger = new DataTable();
        DataTable dtFace = new DataTable();

        DataTable dtDistinctDevice = dtUser.DefaultView.ToTable(true, "Device_Id");
        for (int devicecounter = 0; devicecounter < dtDistinctDevice.Rows.Count; devicecounter++)
        {
            DataTable dtUserByDevice = new DataView(dtUser, "Device_Id='" + dtDistinctDevice.Rows[devicecounter][0].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtUserByDevice.Rows.Count > 0)
            {
                string ip = dtUserByDevice.Rows[0]["IP"].ToString();
                string Port = dtUserByDevice.Rows[0]["Port"].ToString();
                //bool IsDeviceConnected = false;

                DataTable dtdevice = objda.return_DataTable("select Communication_Type,Field2 as sMake from att_Devicemaster where ip_address ='" + ip.ToString()+ "'");
                //here we are deleting multiple user 

                objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(dtdevice.Rows[0]["Communication_Type"].ToString() == "PULL" ? TechType.PULL : TechType.PUSH, dtdevice.Rows[0]["sMake"].ToString());

                if (!objDeviceOp.Device_Connection(ip, Port, 0))
                {
                    continue;
                }
                
                objFingerInfo = objDeviceOp.GetUserFinger(ip, Port);
                dtFingertemp = new Common(Session["DBConnection"].ToString()).ListToDataTable(objFingerInfo);
                bool b = false;
                //if (IsDeviceConnected)
                //{
                if (optype == "1")
                {
                    try
                    {

                        b = objDeviceOp.DelMultipleUser(ip,Port, dtUserByDevice, DevicedwBackupNumber);

                    }
                    catch
                    {

                    }
                }

                else if (optype == "2")
                {
                    try
                    {
                        b = objDeviceOp.DelMultipleUserFinger(ip, Port, dtUserByDevice, dtFingertemp);

                        // b = objDeviceOp.DelMultipleUser(ip, Convert.ToInt32(Port), dtUserByDevice,"11");
                    }
                    catch(Exception ex)
                    {

                    }



                }
                else if (optype == "3")
                {
                    try
                    {
                        b = objDeviceOp.DelMultipleUserFace(ip,Port, dtUserByDevice);

                    }
                    catch
                    {

                    }




                }
                else if (optype == "4")
                {

                    try
                    {

                        //b = objDeviceOp.DelMultipleUserFinger(ip, Convert.ToInt32(Port), dtUserByDevice);
                        b = objDeviceOp.DelMultipleUser(ip, Port, dtUserByDevice, "11");

                        // b = objDeviceOp.DelMultipleUserFace(ip, Convert.ToInt32(Port), dtUserByDevice);

                    }
                    catch
                    {
                    }


                }


                else if (optype == "5")
                {

                    try
                    {
                        //b = objDeviceOp.DelMultipleUserFinger(ip, Convert.ToInt32(Port), dtUserByDevice);
                        //b = objDeviceOp.DelMultipleUser(ip, Convert.ToInt32(Port), dtUserByDevice, "11");

                        //b = objDeviceOp.DelMultipleUserFace(ip, Convert.ToInt32(Port), dtUserByDevice);

                        // b = objDeviceOp.DelSingleUserFinger(ip, Convert.ToInt32(Port), Convert.ToInt32(EnrollNo), 12);


                        b = objDeviceOp.DelMultipleUser(ip, Port, dtUserByDevice, DevicedwBackupNumber);
                    }
                    catch
                    {
                    }

                }

                else if (optype == "6")
                {
                    try
                    {
                        //b = objDeviceOp.DelSingleUserFinger(ip, Convert.ToInt32(Port), Convert.ToInt32(EnrollNo), 12);
                        b = objDeviceOp.DelMultipleUser(ip, Port, dtUserByDevice, DevicedwBackupNumber);
                        // b = objDeviceOp.DelMultipleUserFinger(ip, Convert.ToInt32(Port), dtUserByDevice);
                        //b = objDeviceOp.DelMultipleUser(ip, Convert.ToInt32(Port), dtUserByDevice, "11");
                    }
                    catch
                    {
                    }

                }

                else if (optype == "7")
                {
                    try
                    {
                        // b = objDeviceOp.DelSingleUserFinger(ip, Convert.ToInt32(Port), Convert.ToInt32(EnrollNo), 12);
                        b = objDeviceOp.DelMultipleUser(ip,Port, dtUserByDevice, DevicedwBackupNumber);
                        //b = objDeviceOp.DelMultipleUserFace(ip, Convert.ToInt32(Port), dtUserByDevice);
                    }
                    catch
                    {
                    }

                }

                //}





                //}

            }
        }

        gvUser.DataSource = null;
        gvUser.DataBind();
        Div_Main.Visible = true;
        Div_Delete.Visible = false;

        DisplayMessage("Users Deleted");



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

        Div_Delete.Visible = false;
        Div_Main.Visible = true;
    }
    
}
