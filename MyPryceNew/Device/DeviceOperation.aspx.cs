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
using PryceDevicesLib;

public partial class Device_DeviceOperation : BasePage
{
    IAttDeviceOperation objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(TechType.PULL, "zkTecho");
    Att_DeviceMaster objDevice = null;
    Common cmn = null;
    UserMaster ObjUserMaster = null;
    SystemParameter objSys = null;
    //Device_Operation_Lan objDeviceOp = new Device_Operation_Lan();
    Set_ApplicationParameter objAppParam = null;
    LocationMaster ObjLocationMaster = null;
    RoleDataPermission objRoleData = null;
    RoleMaster objRole = null;
    Set_Location_Department objLocDept = null;
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
        ObjUserMaster = new UserMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        //Device_Operation_Lan objDeviceOp = new Device_Operation_Lan();
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        Page.Title = objSys.GetSysTitle();
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "76", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            FillddlLocation();
            FillGrid();
            pnlDeviceOp.Visible = false;
            if(Session["EmpId"].ToString().Trim()=="0")
            {
                btnInitialize.Enabled = true;
            }
            else
            {
                btnInitialize.Enabled = false;
            }
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
        string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["CompId"].ToString(),  HttpContext.Current.Session["Application_Id"].ToString());

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

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("76", (DataTable)Session["ModuleName"]);
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
            foreach (GridViewRow Row in gvDevice.Rows)
            {
                ((ImageButton)Row.FindControl("LnkDeviceOp")).Visible = true;

            }

        }
        else
        {
            DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "76",Session["CompId"].ToString());
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
                        if (DtRow["Op_Id"].ToString() == "1")
                        {
                            foreach (GridViewRow Row in gvDevice.Rows)
                            {
                                ((ImageButton)Row.FindControl("LnkDeviceOp")).Visible = true;

                            }

                        }
                        if (DtRow["Op_Id"].ToString() == "2")
                        {
                            foreach (GridViewRow Row in gvDevice.Rows)
                            {
                                ((ImageButton)Row.FindControl("LnkDeviceOp")).Visible = true;

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

        Session["dtFilter_Device_Opr"] = dt;
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

        DataTable dt = (DataTable)Session["dtFilter_Device_Opr"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvDevice, dt, "", "");
        AllPageCode();
    }
    protected void gvDevice_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Device_Opr"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Device_Opr"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvDevice, dt, "", "");
    }
    protected void LnkDeviceOp_Click(object sender, ImageClickEventArgs e)
    {
        int index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
        string ip = gvDevice.DataKeys[index]["IP_Address"].ToString();
        string deviceid = gvDevice.DataKeys[index]["Device_Id"].ToString();
        string port = gvDevice.DataKeys[index]["Port"].ToString();
        string strCommType = gvDevice.DataKeys[index]["Communication_Type"].ToString();
        string strMake = gvDevice.DataKeys[index]["field2"].ToString();

        objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(strCommType == "PULL" ? TechType.PULL : TechType.PUSH, strMake);
        bool b = false;


        b = objDeviceOp.Device_Connection(ip, port, 0);

        if (b == true)
        {

            DisplayMessage("Device Is Functional");




            pnlList.Visible = false;
            pnlDeviceOp.Visible = true;
            lblDeviceId.Text = deviceid;
            lblDeviceWithId.Text = gvDevice.DataKeys[index]["Device_Name"].ToString();

        }
        else
        {

            DisplayMessage("Unable to connect the device");


        }

        Session["IPForOp"] = ip;
        Session["PortForOp"] = port;
        Session["DeviceIdForOp"] = gvDevice.DataKeys[index]["Device_Id"].ToString();
        Session["deviceMake"] = strMake;
        Session["deviceCommunicationType"] = strCommType;
    }

    protected void lnkBackFromManage_Click(object sender, EventArgs e)
    {
        Session["IPforManage"] = "";
        Session["PortForManage"] = "";

        pnlList.Visible = true;
        pnlDeviceOp.Visible = false;
    }

    protected void btnClearLog_Click(object sender, EventArgs e)
    {
        objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(Session["deviceCommunicationType"].ToString() == "PULL" ? TechType.PULL : TechType.PUSH, Session["deviceMake"].ToString());
        if (objDeviceOp.ClearLog(Session["IPforOp"].ToString(), Session["PortForOp"].ToString()))
        {
            DisplayMessage("All Attendance Logs Have Been Cleared From Terminal");
        }
        else
        {
            DisplayMessage("Operation Failed");
        }
    }
    protected void btnClearadmin_Click(object sender, EventArgs e)
    {
        objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(Session["deviceCommunicationType"].ToString() == "PULL" ? TechType.PULL : TechType.PUSH, Session["deviceMake"].ToString());
        bool b = objDeviceOp.ClearAdminPrivilege(Session["IPforOp"].ToString(), Session["PortForOp"].ToString());
        if (b)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('Admin Privilege Cleared')", true);
        }
        else
        {
            DisplayMessage("Device PowerOFF Successfully!");
        }
    }
    protected void btnInitialize_Click(object sender, EventArgs e)
    {

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "show_modal()", true);
        //objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(Session["deviceCommunicationType"].ToString() == "PULL" ? TechType.PULL : TechType.PUSH, Session["deviceMake"].ToString());
        //if (objDeviceOp.Device_Connection(Session["IPForOp"].ToString(), Session["PortForOp"].ToString(), 0))
        //{
        //    bool b = false;
        //    b = objDeviceOp.InitializeDevice(Session["IPForOp"].ToString(),Session["PortForOp"].ToString());
        //    if (b)
        //    {
        //        DisplayMessage("Device Initialized");
        //    }
        //    else
        //    {
        //        DisplayMessage("Device PowerOFF Successfully!");
        //    }
        //}
        //else
        //{
        //    DisplayMessage("Unable To Connected Device");
        //}
    }
    protected void btnRestrat_Click(object sender, EventArgs e)
    {
        objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(Session["deviceCommunicationType"].ToString() == "PULL" ? TechType.PULL : TechType.PUSH, Session["deviceMake"].ToString());
        bool b = objDeviceOp.RestartDevice(Session["IPForOp"].ToString(), Session["PortForOp"].ToString());
        if (b)
        {
            DisplayMessage("Device Restarted Successfully");
        }
        else
        {
            DisplayMessage("Device PowerOFF Successfully!");
        }
    }
    protected void btnPowerOff_Click(object sender, EventArgs e)
    {
        //Code with class not DLL On 18-06-2015

        bool b = objDeviceOp.PowerOffDevice(Session["IPForOp"].ToString(), Session["PortForOp"].ToString());
        if (b)
        {
            DisplayMessage("Device Off Successfully");
        }
        else
        {
            DisplayMessage("Device PowerOFF Successfully!");
        }
    }
    protected void btnSynctime_Click(object sender, EventArgs e)
    {
        //Code with class not DLL On 18-06-2015
        objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(Session["deviceCommunicationType"].ToString() == "PULL" ? TechType.PULL : TechType.PUSH, Session["deviceMake"].ToString());
        bool b = objDeviceOp.SetDeviceTime(Session["IPForOp"].ToString(), Session["PortForOp"].ToString());
        if (b)
        {
            DisplayMessage("Device Time Set With System Time");
        }
        else
        {
            DisplayMessage("Sorry unalbable to sync Device time !");
        }


    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtUserName.Text == "")
        {
            DisplayMessage("Enter User Name");
            txtUserName.Focus();
            return;
        }
        else
        {
            txtUserName.Attributes.Add("Value", txtUserName.Text);
        }


        if (txtPassword.Text.Trim() == "")
        {
            DisplayMessage("Enter Password");
            txtPassword.Focus();
            return;
        }
        else
        {
            txtPassword.Attributes.Add("Value", txtPassword.Text);
        }

        string strCompanyId = string.Empty;
        if (Session["EmpId"].ToString() == "0")
        {
            strCompanyId = "0";
        }
        else
        {
            strCompanyId = Session["CompId"].ToString();
        }


        DataTable dtUser = ObjUserMaster.GetUserMasterByUserIdPass(txtUserName.Text.Trim(), Common.Encrypt(txtPassword.Text), strCompanyId);
        if (dtUser.Rows.Count == 0)
        {
            DisplayMessage("Invaild UserName or Password");
            return;
        }
        else
        {
            objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(Session["deviceCommunicationType"].ToString() == "PULL" ? TechType.PULL : TechType.PUSH, Session["deviceMake"].ToString());
            if (objDeviceOp.Device_Connection(Session["IPForOp"].ToString(), Session["PortForOp"].ToString(), 0))
            {
                bool b = false;
                b = objDeviceOp.InitializeDevice(Session["IPForOp"].ToString(), Session["PortForOp"].ToString());
                if (b)
                {
                    DisplayMessage("Device Initialized");
                }
                else
                {
                    DisplayMessage("Device PowerOFF Successfully!");
                }
            }
            else
            {
                DisplayMessage("Unable To Connected Device");
            }
        }

        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Close()", true);
    }
}
