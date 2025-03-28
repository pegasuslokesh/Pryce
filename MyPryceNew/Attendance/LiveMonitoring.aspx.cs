using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Attendance_LiveMonitoring : BasePage
{
    private string _strConString = string.Empty;
    Att_AttendanceLog objAttLog = null;
    SystemParameter objSys = null;
    Common cmn = null;
    LocationMaster ObjLocationMaster = null;
    EmployeeMaster objEmp = null;
    Set_ApplicationParameter objAppParam = null;
    Att_DeviceGroupMaster OBJDevicegroup = null;
    Att_DeviceMaster OBJDevice = null;
    PageControlCommon objPageCmn = null;
    static string Depart;
    static string locationids;

    protected void Page_Load(object sender, EventArgs e)
    {
        ddlSelectRecord.Focus();
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }

        _strConString = Session["DBConnection"].ToString();
        objAttLog = new Att_AttendanceLog(_strConString);
        objSys = new SystemParameter(_strConString);
        cmn = new Common(_strConString);
        ObjLocationMaster = new LocationMaster(_strConString);
        objEmp = new EmployeeMaster(_strConString);
        objAppParam = new Set_ApplicationParameter(_strConString);
        OBJDevicegroup = new Att_DeviceGroupMaster(_strConString);
        OBJDevice = new Att_DeviceMaster(_strConString);
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            
            FillDeviceGroup();
            FillDevice();
            fillLocation();
            FillddlDeaprtment();
            ddlLocation.SelectedValue = Session["LocId"].ToString();
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(_strConString);
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "119", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            //ddlSelectRecord.Focus();
            // GetLog("10");
            LogCount();
        }
        Page.Title = objSys.GetSysTitle();
        AllPageCode();
    }

    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());

        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("119", (DataTable)Session["ModuleName"]);
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
            gvTheGrid.Visible = true;
        }
        else
        {
            DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "119", HttpContext.Current.Session["CompId"].ToString());
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
                        if (DtRow["Op_Id"].ToString() == "2")
                        {
                            gvTheGrid.Visible = true;
                        }
                        if (DtRow["Op_Id"].ToString() == "1")
                        {
                            gvTheGrid.Visible = true;
                        }
                    }
                }
            }
            else
            {
                // Modified By Nitin Jain on 13/11/2014 to show log on Employee Level
                gvTheGrid.Visible = true;
                //Session.Abandon();
                //Response.Redirect("~/ERPLogin.aspx");
            }
        }
    }
    public string GetDate(object obj)
    {
        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());

        return Date.ToString(objSys.SetDateFormat());
    }

    //public void GetLog(string pagesize)
    //{
    //    DataTable dtlog = objAttLog.GetAttendanceLog(Session["CompId"].ToString());


    //    DataTable dtlogdata = new DataTable();

    //    dtlogdata = dtlog.Clone();


    //    if (dtlog.Rows.Count > 0)
    //    {

    //        if (pagesize == "all")
    //        {

    //            dtlogdata = dtlog;


    //        }
    //        else
    //        {
    //            dtlogdata = SelectTopDataRow(dtlog, int.Parse(pagesize));



    //        }
    //        gvTheGrid.DataSource = dtlogdata;
    //        gvTheGrid.DataBind();
    //    }
    //    else
    //    {
    //        gvTheGrid.DataSource = null;
    //        gvTheGrid.DataBind();
    //    }

    //}

    //public DataTable SelectTopDataRow(DataTable dt, int count)
    //{
    //    DataTable dtn = dt.Clone();
    //    if (dt.Rows.Count == 0)
    //    {
    //        return null;
    //    }

    //    if (dt.Rows.Count >= count)
    //    {
    //        for (int i = 0; i < count; i++)
    //        {
    //            dtn.ImportRow(dt.Rows[i]);
    //        }
    //    }
    //    else
    //    {
    //        for (int i = 0; i < dt.Rows.Count; i++)
    //        {
    //            dtn.ImportRow(dt.Rows[i]);
    //        }

    //    }
    //    return dtn;
    //}


    protected void ddlSelectRecord_SelectedIndexChanged(object sender, EventArgs e)
    {
        LogCount();

        // GetLog(Session["Size"].ToString());
        AllPageCode();
        ddlSelectRecord.Focus();
    }

    private void LogCount()
    {
        String strPageLevel = string.Empty;

        try
        {
            strPageLevel = objAppParam.GetApplicationParameterValueByParamName("Page Level", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        catch
        {
            strPageLevel = "Location";
        }



        DataTable dtEmp = objEmp.GetEmployeeMasterOnRole(Session["CompId"].ToString());


        if (strPageLevel == "Location")
        {

            //if(ddlLocation.SelectedValue=="0")
            //{
            //    dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //}
            //else
            //{
            //    dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + ddlLocation.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
            //}

            //if (Session["SessionDepId"] != null)
            //{
            //    dtEmp = new DataView(dtEmp, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

            //}
        }

        string EmpIds = string.Empty;
        //for (int i = 0; i < dtEmp.Rows.Count; i++)
        //{
        //    EmpIds += dtEmp.Rows[i]["Emp_Id"].ToString() + ",";

        //}
        //string[] count = EmpIds.Split(',');

        string RowCount = ddlSelectRecord.SelectedValue;
        //DataTable dtlog = objAttLog.GetAttendanceLogCount(Session["CompId"].ToString(), RowCount);
        //dtlog = new DataView(dtlog, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //if (Session["SessionDepId"] != null)
        //{
        //    dtlog = new DataView(dtlog, "Department_Id in(" + Session["SessionDepId"].ToString().Substring(0, Session["SessionDepId"].ToString().Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();

        //}
        DataTable dtlog = new DataTable();

        if (ddlLocation.SelectedValue == "0")
        {
            FillLocation_ids();
        }
        else
        {
            locationids = ddlLocation.SelectedValue;
        }

        if (dpDepartment.SelectedValue == "0")
        {
            fillDepartment_ids();
            string[] count = Depart.Split(',');
        }
        else
        {
            Depart = dpDepartment.SelectedValue;
        }

        dtlog = objAttLog.GetAttendanceLogCount(Session["CompId"].ToString(), RowCount, locationids, Depart, ddldeviceList.SelectedIndex > 0? ddldeviceList.SelectedValue:"0");

        
        if (dtlog.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvTheGrid, dtlog, "", "");
        }
        else
        {
            gvTheGrid.DataSource = null;
            gvTheGrid.DataBind();
        }

       

    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        //Session["Size"] = ddlSelectRecord.SelectedValue;
        //GetLog(ddlSelectRecord.SelectedValue);
        LogCount();
        AllPageCode();
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillddlDeaprtment();
        LogCount();
    }

    public void fillLocation()
    {
        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());

        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());
            if (LocIds != "")
            {
                dtLoc = new DataView(dtLoc, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        if (dtLoc.Rows.Count > 0)
        {
            ddlLocation.Items.Add(new ListItem("All", "0"));
            for (int i = 0; i < dtLoc.Rows.Count; i++)
            {
                ddlLocation.Items.Add(new ListItem(dtLoc.Rows[i]["Location_Name"].ToString(), dtLoc.Rows[i]["Location_Id"].ToString()));
                locationids = locationids + dtLoc.Rows[i]["Location_Id"].ToString() + ",";
            }
        }
        else
        {
            ddlLocation.Items.Clear();
        }
    }


    protected void dpDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        LogCount();
    }

    private void FillddlDeaprtment()
    {
        dpDepartment.Items.Clear();
        DataTable dt = objEmp.GetEmployeeOrDepartment("0", "0", "0", "0", "0");

        if (ddlLocation.SelectedValue != "0")
        {
            dt = new DataView(dt, "Location_Id = '" + ddlLocation.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else
        {
            dt = new DataView(dt, "Location_Id = '" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        string DepIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D",ddlLocation.SelectedValue=="0"?Session["LocId"].ToString(): ddlLocation.SelectedValue, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (DepIds != "")
            {
                dt = new DataView(dt, "Dep_Id in(" + DepIds.Substring(0, DepIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        dt = dt.DefaultView.ToTable(true, "DeptName", "Dep_Id");

        if (dt.Rows.Count > 0)
        {

            dpDepartment.Items.Add(new ListItem("All", "0"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dpDepartment.Items.Add(new ListItem(dt.Rows[i]["DeptName"].ToString(), dt.Rows[i]["Dep_Id"].ToString()));
                Depart = Depart + dt.Rows[i]["Dep_Id"].ToString() + ",";

            }

        }
        //else
        //{
        //    try
        //    {
        //        dpDepartment.Items.Clear();
        //        dpDepartment.DataSource = null;
        //        dpDepartment.DataBind();
        //        dpDepartment.Items.Insert(0, "All");
        //        dpDepartment.SelectedIndex = 0;
        //    }
        //    catch
        //    {
        //        dpDepartment.Items.Insert(0, "All");
        //        dpDepartment.SelectedIndex = 0;
        //    }
        //}
    }

    public void FillLocation_ids()
    {
        locationids = "";
        DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());

        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
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
                locationids = locationids + dtLoc.Rows[i]["Location_Id"].ToString() + ",";
            }
        }
    }

    public void fillDepartment_ids()
    {
        Depart = "";
        DataTable dt = objEmp.GetEmployeeOrDepartment("0", "0", "0", "0", "0");

        if (ddlLocation.SelectedValue != "0")
        {
            dt = new DataView(dt, "Location_Id = '" + ddlLocation.SelectedValue + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        string DepIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "D",ddlLocation.SelectedValue, HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (!Common.GetStatus(HttpContext.Current.Session["EmpId"].ToString()))
        {
            if (DepIds != "")
            {
                dt = new DataView(dt, "Dep_Id in(" + DepIds.Substring(0, DepIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Depart = Depart + dt.Rows[i]["Dep_Id"].ToString() + ",";
            }
        }
    }

    protected void ddlDeviceGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillDevice();
    }

    public void FillDeviceGroup()
    {
        ddlDeviceGroup.Items.Clear();
        DataTable dtDeviceGroup = OBJDevicegroup.GetHeaderAllTrueRecord("0", "0", "0");
        objPageCmn.FillData((DropDownList)ddlDeviceGroup, dtDeviceGroup, "Group_Name", "Group_Id");

    }


    protected void ddldeviceList_SelectedIndexChanged(object sender, EventArgs e)
    {
        LogCount();
    }

    public void FillDevice()
    {
        ddldeviceList.Items.Clear();
        string strdeviceId = string.Empty;
        //Add On 04-06-2015
        string strFLocId = string.Empty;
        DataTable dtDeviceDetail = new DataTable();
        DataTable dt = OBJDevice.GetDeviceMaster(Session["CompId"].ToString());
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["EmpId"].ToString());

        if (ddlDeviceGroup.SelectedIndex > 0)
        {
            dtDeviceDetail = OBJDevicegroup.GetDetailRecord(ddlDeviceGroup.SelectedValue);

            foreach (DataRow dr in dtDeviceDetail.Rows)
            {
                if (strdeviceId == "")
                {
                    strdeviceId = dr["Device_Id"].ToString();
                }
                else
                {
                    strdeviceId = strdeviceId + "," + dr["Device_Id"].ToString(); ;
                }
            }

            if (strdeviceId == "")
            {
                strdeviceId = "0";
            }

        }



        if (dt.Rows.Count > 0)
        {
            if (LocIds != "")
            {
                if (ddlDeviceGroup.SelectedIndex > 0)
                {

                    dt = new DataView(dt, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ") and Device_Id in (" + strdeviceId + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
                else
                {
                    dt = new DataView(dt, "Location_Id in(" + LocIds.Substring(0, LocIds.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
            else
            {
                if (ddlDeviceGroup.SelectedIndex > 0)
                {

                    dt = new DataView(dt, "Device_Id in (" + strdeviceId + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
        }
        objPageCmn.FillData((DropDownList)ddldeviceList, dt, "Device_Name", "Device_Id");



        AllPageCode();
    }
}
