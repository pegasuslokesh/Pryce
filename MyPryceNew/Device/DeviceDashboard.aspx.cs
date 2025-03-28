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
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PegasusDataAccess;

public partial class Device_DeviceMaster : BasePage
{
    Common cmn = null;
    SystemParameter objSys = null;
    Att_DeviceMaster objDevice = null;
    Ser_UserTransfer objSer = null;
    Set_ApplicationParameter objAppParam = null;
    Att_Device_Parameter objDeviceParam = null;
    LocationMaster ObjLocationMaster = null;
    RoleDataPermission objRoleData = null;
    RoleMaster objRole = null;
    Set_Location_Department objLocDept = null;
    Device_Operation_Lan objDeviceOp = null;
    DataAccessClass OBJda = null;
    PageControlCommon objPageCmn = null;
    string strPageLevel = string.Empty;



    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
     
        //AllPageCode();

        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objDevice = new Att_DeviceMaster(Session["DBConnection"].ToString());
        objSer = new Ser_UserTransfer(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objDeviceParam = new Att_Device_Parameter(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        objDeviceOp = new Device_Operation_Lan(Session["DBConnection"].ToString());
        OBJda = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        Page.Title = objSys.GetSysTitle();
        if (!IsPostBack)
        {
           
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), Common.GetObjectIdbyPageURL("../Device/DeviceDashboard.aspx", Session["DBConnection"].ToString()), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            Session["live_device"] = null;
            txtValue.Focus();
            FillGrid();
            //RefreshGrid();
            btnList_Click(null, null);
            FillddlLocation();
            string strConnectionstring = Session["DBConnection"].ToString();
            ThreadStart ts = delegate () { RefreshGrid(strConnectionstring); };
            Thread t = new Thread(ts);
            t.Start();

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

        if (!Common.GetStatus(Session["CompId"].ToString()))
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
    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());

        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("75", (DataTable)Session["ModuleName"]);
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
                //((ImageButton)Row.FindControl("btnEdit")).Visible = true;
                //((ImageButton)Row.FindControl("IbtnDelete")).Visible = true;
                //((ImageButton)Row.FindControl("lnkConnect")).Visible = true;
            }


        }
        else
        {
            DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "75",Session["CompId"].ToString());

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

                        }
                        foreach (GridViewRow Row in gvDevice.Rows)
                        {
                            if (DtRow["Op_Id"].ToString() == "2")
                            {
                                ((ImageButton)Row.FindControl("btnEdit")).Visible = true;
                            }
                            if (DtRow["Op_Id"].ToString() == "3")
                            {
                                ((ImageButton)Row.FindControl("IbtnDelete")).Visible = true;
                            }
                            if (DtRow["Op_Id"].ToString() == "5")
                            {
                                ((ImageButton)Row.FindControl("lnkConnect")).Visible = true;
                            }
                        }
                        if (DtRow["Op_Id"].ToString() == "4")
                        {

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

    //this event added for get device status means device is connected or not 

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



    protected void btnList_Click(object sender, EventArgs e)
    {
        txtValue.Focus();
        pnlMenuList.BackColor = System.Drawing.ColorTranslator.FromHtml("#ccddee");

        pnlMenuNew.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");
        pnlMenuBin.BackColor = System.Drawing.ColorTranslator.FromHtml("#90bde9");

        PnlList.Visible = true;
        PnlNewEdit.Visible = false;
        PnlBin.Visible = false;
    }



    protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
    {
        FillGrid();
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
    }

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
            DataTable dtCust = (DataTable)Session["Device"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Device_Mas"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";



            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvDevice, view.ToTable(), "", "");
            //AllPageCode();
        }
        txtValue.Focus();
    }





    public Boolean CheckIPValid(String strIP)
    {
        //  Split string by ".", check that array length is 3
        char chrFullStop = '.';
        string[] arrOctets = strIP.Split(chrFullStop);
        if (arrOctets.Length != 4)
        {
            return false;
        }
        //  Check each substring checking that the int value is less than 255 and that is char[] length is !> 2
        Int16 MAXVALUE = 255;
        Int32 temp; // Parse returns Int32
        foreach (String strOctet in arrOctets)
        {
            if (strOctet.Length > 3)
            {
                return false;
            }

            temp = int.Parse(strOctet);
            if (temp > MAXVALUE)
            {
                return false;
            }
        }
        return true;
    }





    protected void lnkConnect_Click(object sender, ImageClickEventArgs e)
    {
        int errorcode = 0;
        GridViewRow gvRow = ((GridViewRow)((ImageButton)sender).Parent.Parent);
        string port = gvDevice.DataKeys[gvRow.RowIndex]["Port"].ToString();
        string IP = gvDevice.DataKeys[gvRow.RowIndex]["IP_Address"].ToString();
        string DeviceId = gvDevice.DataKeys[gvRow.RowIndex]["Device_Id"].ToString();

        Device_Operation_Lan objDeviceOp = new Device_Operation_Lan(Session["DBConnection"].ToString());


        bool b = false;
        //b = objDeviceOp.Device_Connection(IP, Convert.ToInt32(port), 0);
        b = Common.IsAlive(IP);

        if (b == true)
        {

            DisplayMessage("Device Is Functional");

            ((Label)gvRow.FindControl("lblStatus")).Text = "Connected";
        }
        else
        {
            DisplayMessage("Unable to connect the device");

            ((Label)gvRow.FindControl("lblStatus")).Text = "Disconnected";

        }
    }

    //this funcation is call in another thread to refresh live devices
    protected void RefreshGrid(string strConnectionstring)
    {
        DataTable dtlogdetail = new DataTable();

        if (Session["live_device"] != null && ((DataTable)Session["live_device"]).Rows.Count > 0)
        {
            DataTable dt = (DataTable)Session["live_device"];
            int rowCount = 0;
            DataRow dRow;

            int userCount = 0;
            int fpCnt = 0;
            int faceCnt = 0;
            Ping myPing = new Ping();
            string strMaxLogTime = string.Empty;
            for (rowCount = 0; rowCount < dt.Rows.Count; rowCount++)
            {
                userCount = 0;
                fpCnt = 0;
                faceCnt = 0;
                try
                {
                    dRow = dt.Rows[rowCount];
                    string port = dRow["Port"].ToString();
                    string IP = dRow["IP_Address"].ToString();
                    bool device_status = false;
                    PingReply reply = myPing.Send(IP, 1000);

                    if (reply != null && reply.Status.ToString() == "Success")
                    {
                        dRow["Response_Time"] = reply.RoundtripTime.ToString();
                        device_status = objDeviceOp.Device_Connection(IP, Convert.ToInt32(port), 1);
                        if (device_status == true)
                        {
                            objDeviceOp.Device_Connection(IP, Convert.ToInt32(port), 0);
                            //objDeviceOp.sta_GetCapacityInfo(out userCount, out fpCnt, out faceCnt);
                            dRow["device_status"] = "Connected";
                            //dRow["User_Count"] = userCount.ToString();
                            //dRow["Finger_Count"] = fpCnt.ToString();
                            //dRow["Face_Count"] = faceCnt.ToString();
                        }
                        else
                        {
                            dRow["device_status"] = "Disconnected";
                            dRow["User_Count"] = "-";
                            dRow["Finger_Count"] = "-";
                            dRow["Face_Count"] = "-";
                        }
                    }
                    else
                    {
                        dRow["device_status"] = "Disconnected";
                        dRow["Response_Time"] = "-";
                    }


                    //here we are getting max log time for particular device

                    //Max_Log_Time


                    dtlogdetail = OBJda.return_DataTable_Using_Connectionstring("SELECT isnull(CAST(CAST(CAST(max(Att_AttendanceLog.Event_Time) AS date) AS varchar) + ' ' + SUBSTRING(CONVERT(varchar, max(Att_AttendanceLog.Event_Time), 108), 1, 5) AS datetime),''),count(*) from att_attendancelog where Device_Id =" + dRow["Device_Id"].ToString() + "",strConnectionstring);

                    if (dtlogdetail.Rows[0][1].ToString().Trim() != "0")
                    {
                        strMaxLogTime = dtlogdetail.Rows[0][0].ToString();
                    }
                    else
                    {
                        strMaxLogTime = "Log Not Found";
                    }

                    ////01-Jan-1900 0:0
                    //if (strMaxLogTime.Trim() == "1900-01-01 00:00:00.000" || strMaxLogTime.Trim() == "01-Jan-00 12:00:00 AM")
                    //{
                    //    strMaxLogTime = "Log Not Found";
                    //}
                    ////else
                    ////{
                    ////    strMaxLogTime = Convert.ToDateTime(strMaxLogTime).ToString(objSys.SetDateFormat())+" "+ Convert.ToDateTime(strMaxLogTime).Hour+":"+Convert.ToDateTime(strMaxLogTime).Minute;
                    ////}

                    dRow["Max_Log_Time"] = strMaxLogTime;

                }
                catch
                {

                }
                Session["live_device"] = dt;
                if (rowCount == dt.Rows.Count - 1)
                {
                    rowCount = -1;
                }
            }
        }


    }

    protected void UpdateTimer_Tick(object sender, EventArgs e)
    {
        FillGrid();
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }


    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {
        int b = 0;
        b = objDevice.DeleteDeviceMaster(Session["CompId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");


            FillGrid();
            //AllPageCode();
            //Reset();
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
    }
    protected void gvDevice_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDevice.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Device_Mas"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvDevice, dt, "", "");
        //AllPageCode();
    }
    protected void gvDevice_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Device_Mas"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Device_Mas"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvDevice, dt, "", "");
        //AllPageCode();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {


        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        FillGrid();

        btnList_Click(null, null);
    }

    public void FillGrid()
    {
        //Add On 04-06-2015
        DataTable dt = new DataTable();
        if (Session["live_device"] == null)
        {
            string strFLocId = string.Empty;
            DataTable dtLoc = ObjLocationMaster.GetLocationMaster(Session["CompId"].ToString());
            //dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            //string LocIds = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L");

            dt = objDevice.GetDeviceMaster(Session["CompId"].ToString());
            if (!Common.GetStatus(Session["CompId"].ToString()))
            {
                try
                {
                    string strLocationIDs = cmn.GetRoleDataPermission(Session["RoleId"].ToString(), "L", Session["UserId"].ToString(), Session["CompId"].ToString(), Session["EmpId"].ToString());
                    strLocationIDs = strLocationIDs.Substring(0, strLocationIDs.Length - 1);
                    if (strLocationIDs != "")
                    {
                        dt = new DataView(dt, "Location_Id in(" + strLocationIDs + ")", "", DataViewRowState.CurrentRows).ToTable();
                    }

                }
                catch
                {

                }
                
            }





            //dt = objDevice.GetDeviceMaster(Session["CompId"].ToString());
            //if (strPageLevel == "Company")
            //{
            //if (dt.Rows.Count > 0)
            //{
            //    if (strFLocId != "")
            //    {
            //        dt = new DataView(dt, "Location_Id in(" + strFLocId.Substring(0, strFLocId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
            //    }
            //}


        }

        if (Session["live_device"] == null)
        {
            dt.Columns.Add("Response_time");
            dt.Columns.Add("User_Count");
            dt.Columns.Add("Face_Count");
            dt.Columns.Add("Finger_Count");
            dt.Columns.Add("Max_Log_Time");
            dt.AcceptChanges();

            foreach (DataRow dRow in dt.Rows)
            {
                dRow["Response_time"] = "-";
                dRow["User_Count"] = "-";
                dRow["Face_Count"] = "-";
                dRow["Finger_Count"] = "-";
                dRow["Max_Log_Time"] = "";
            }


            Session["live_device"] = dt;
        }

        //DataTable dt = (DataTable)Session["live_device"];

        //Common Function add By Lokesh on 23-05-2015
        //cmn.FillData((object)gvDevice, (DataTable)Session["live_device"], "", "");

        // cmn.FillData((object)dlDevice, (DataTable)Session["live_device"], "", "");
        dlDevice.DataSource = (DataTable)Session["live_device"];
        dlDevice.DataBind();


        //AllPageCode();
        //Session["dtFilter_Device_Mas"] = dt;
        //Session["Device"] = dt;

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + ((DataTable)Session["live_device"]).Rows.Count.ToString() + "";
    }



    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDeviceName(string prefixText, int count, string contextKey)
    {
        Att_DeviceMaster objAtt_Device = new Att_DeviceMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objAtt_Device.GetDeviceMaster(HttpContext.Current.Session["CompId"].ToString()), "Device_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Device_Name"].ToString();
        }
        return txt;
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListIpAddress(string prefixText, int count, string contextKey)
    {
        Att_DeviceMaster objDevice = new Att_DeviceMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objDevice.GetDeviceMaster(HttpContext.Current.Session["CompId"].ToString()), "IP_Address like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["IP_Address"].ToString();
        }
        return txt;
    }



}
