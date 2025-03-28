using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using PryceDevicesLib;

public partial class Device_DeviceMaster : BasePage
{
    IAttDeviceOperation objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(TechType.PULL, "zkTecho");
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
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        Page.Title = objSys.GetSysTitle();
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Device/DeviceMaster.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);

            txtValue.Focus();
            FillGrid();
            FillddlLocation();
            txtPortNumber.Text = "4370";
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
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        btnSave.Visible = clsPagePermission.bAdd;
        imgBtnRestore.Visible = clsPagePermission.bRestore;
        hdnCanTest.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
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


    protected void txtDeviceName_OnTextChanged(object sender, EventArgs e)
    {

        if (editid.Value == "")
        {
            DataTable dt = objDevice.GetDeviceMasterByDeviceName(Session["CompId"].ToString().ToString(), txtDeviceName.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                txtDeviceName.Text = "";
                DisplayMessage("Device Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeviceName);
                return;
            }
            DataTable dt1 = objDevice.GetDeviceMasterInactive(Session["CompId"].ToString().ToString());
            dt1 = new DataView(dt1, "Device_Name='" + txtDeviceName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtDeviceName.Text = "";
                DisplayMessage("Device Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeviceName);
                return;
            }
            txtDeviceNameL.Focus();
        }
        else
        {
            DataTable dtTemp = objDevice.GetDeviceMasterById(Session["CompId"].ToString().ToString(), editid.Value);
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Device_Name"].ToString() != txtDeviceName.Text)
                {
                    DataTable dt = objDevice.GetDeviceMaster(Session["CompId"].ToString().ToString());
                    dt = new DataView(dt, "Device_Name='" + txtDeviceName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtDeviceName.Text = "";
                        DisplayMessage("Device Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeviceName);
                        return;
                    }
                    DataTable dt1 = objDevice.GetDeviceMaster(Session["CompId"].ToString().ToString());
                    dt1 = new DataView(dt1, "Device_Name='" + txtDeviceName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtDeviceName.Text = "";
                        DisplayMessage("Device Name Already Exists - Go to Bin Tab");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtDeviceName);
                        return;
                    }
                }
            }
            txtDeviceNameL.Focus();
        }
    }

    protected void btnBin_Click(object sender, EventArgs e)
    {
        txtbinValue.Focus();
        FillGridBin();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();

        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        txtValue.Text = "";
    }

    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
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
    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (ddlbinOption.SelectedIndex != 0)
        {
            string condition = string.Empty;


            if (ddlbinOption.SelectedIndex == 1)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String)='" + txtbinValue.Text.Trim() + "'";
            }
            else if (ddlbinOption.SelectedIndex == 2)
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) like '%" + txtbinValue.Text + "%'";
            }
            else
            {
                condition = "convert(" + ddlbinFieldName.SelectedValue + ",System.String) Like '" + txtbinValue.Text + "%'";
            }
            DataTable dtCust = (DataTable)Session["dtbinDevice"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvDeviceBin, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {
                imgBtnRestore.Visible = false;
            }
            else
            {
                //AllPageCode();
            }
        }
        txtbinValue.Focus();
    }

    protected void btnbinRefresh_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        FillGridBin();
        ddlbinOption.SelectedIndex = 2;
        ddlbinFieldName.SelectedIndex = 0;
        txtbinValue.Text = "";
        lblSelectedRecord.Text = "";
    }

    protected void gvDeviceBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        gvDeviceBin.PageIndex = e.NewPageIndex;
        if (HDFSortbin.Value == "")
            FillGridBin();
        else
        {
            DataTable dt = (DataTable)Session["dtbinFilter"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvDeviceBin, dt, "", "");
            //AllPageCode();
        }
        string temp = string.Empty;
        bool isselcted;

        for (int i = 0; i < gvDeviceBin.Rows.Count; i++)
        {
            Label lblconid = (Label)gvDeviceBin.Rows[i].FindControl("lblDeviceId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvDeviceBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }

    }
    protected void gvDeviceBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objDevice.GetDeviceMasterInactive(Session["CompId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvDeviceBin, dt, "", "");
        //AllPageCode();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int b = 0;



        if (txtDeviceName.Text == "")
        {
            DisplayMessage("Enter Device Name");
            txtDeviceName.Focus();
            return;
        }

        if (txtIPAddress.Text == "")
        {
            DisplayMessage("Enter IP Address");
            txtIPAddress.Focus();
            return;
        }
        else
        {
            txtIPAddress.Text = txtIPAddress.Text.Trim();
        }
        //else
        //{
        //    if (!CheckIPValid(txtIPAddress.Text))
        //    {
        //        DisplayMessage("IP Address is not valid");
        //        txtIPAddress.Focus();
        //        return;
        //    }
        //}

        if (txtPortNumber.Text == "")
        {
            DisplayMessage("Enter Port Number");
            txtPortNumber.Focus();
            return;
        }








        if (editid.Value == "")
        {

            DataTable dt1 = objDevice.GetDeviceMaster(Session["CompId"].ToString());

            //Here we are cheking license
            DateTime dtTemp;

            if (ConfigurationManager.AppSettings["ApplicationType"].ToString().Trim().ToLower() == "cloud")
            {
                MasterDataAccess objMDa = new MasterDataAccess(ConfigurationManager.ConnectionStrings["PegaConnection1"].ToString());
                MasterDataAccess.clsMasterCompany clsMasterCmp = objMDa.getMasterCompanyInfo(Session["RegistrationCode"].ToString(), ConfigurationManager.AppSettings["masterDbApiBaseAddress"].ToString());
                Lic_Common objLicCmn = new Lic_Common(Session["DBConnection"].ToString());
                int deviceLicCount = objLicCmn.getAttDeviceLicCount();
                if ((dt1.Rows.Count + 1) > deviceLicCount)
                {
                    DisplayMessage("Please update your license");
                    UC_LicenseInfo.GetLicenseInformation(clsMasterCmp.registration_code, clsMasterCmp.license_key == "" ? "Demo" : clsMasterCmp.license_key, clsMasterCmp.email, Convert.ToDateTime(clsMasterCmp.expiry_date).ToString("dd-MMM-yyyy"), clsMasterCmp.att_device_count.ToString(), clsMasterCmp.user.ToString(), clsMasterCmp.no_of_employee.ToString(), clsMasterCmp.version_type, clsMasterCmp.product_code, ConfigurationManager.AppSettings["master_product_id"].ToString());
                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_UpdateLicense_Open()", true);
                    //DisplayMessage("Modal_UpdateLicense_Open()");
                    return;
                }
            }


            //

            DataTable dt2 = new DataView(dt1, "Device_Name='" + txtDeviceName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt2.Rows.Count > 0)
            {
                DisplayMessage("Device Name Already Exists");
                txtDeviceName.Focus();
                return;

            }
            DataTable dt3 = new DataView(dt1, "IP_Address='" + txtIPAddress.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt3.Rows.Count > 0)
            {
                DisplayMessage("IP Address Already Exists");
                txtIPAddress.Focus();
                return;

            }

            b = objDevice.InsertDeviceMaster(Session["CompId"].ToString(), txtDeviceName.Text, txtDeviceNameL.Text, Session["BrandId"].ToString(), Session["LocId"].ToString(), txtIPAddress.Text, ddlCommType.Text, txtPortNumber.Text, chkMasterdevice.Checked.ToString(), ddlMake.SelectedItem.Value, txtCommunicationKey.Text.Trim(), "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            if (b != 0)
            {

                DataTable dtParam1 = objDeviceParam.GetDeviceParameterByCompanyId("", Session["CompId"].ToString());
                if (dtParam1.Rows.Count > 0)
                {
                    DisplayMessage("Device Setup Successfully");
                }
                else
                {
                    DataTable dtDeviceParam = objDeviceParam.GetDeviceParameterByCompanyId("", "1");
                    for (int i = 0; i < dtDeviceParam.Rows.Count; i++)
                    {
                        if (i == 3)
                        {
                            objDeviceParam.InsertDeviceParameterMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtDeviceParam.Rows[i]["Param_Name"].ToString(), Session["CompId"].ToString(), dtDeviceParam.Rows[i]["Param_Description"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                        else if (i == 4)
                        {
                            objDeviceParam.InsertDeviceParameterMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtDeviceParam.Rows[i]["Param_Name"].ToString(), Session["BrandId"].ToString(), dtDeviceParam.Rows[i]["Param_Description"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                        else if (i == 5)
                        {
                            objDeviceParam.InsertDeviceParameterMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtDeviceParam.Rows[i]["Param_Name"].ToString(), Session["LocId"].ToString(), dtDeviceParam.Rows[i]["Param_Description"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }
                        else
                        {
                            objDeviceParam.InsertDeviceParameterMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dtDeviceParam.Rows[i]["Param_Name"].ToString(), dtDeviceParam.Rows[i]["Param_Value"].ToString(), dtDeviceParam.Rows[i]["Param_Description"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }

                    }
                }

                EmployeeMaster objEmp = new EmployeeMaster(Session["DBConnection"].ToString());

                DataTable dtEmp = new DataTable();

                string EmpSync = string.Empty;
                EmpSync = objAppParam.GetApplicationParameterValueByParamName("Employee Synchronization", Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString());

                if (EmpSync == "Company")
                {
                    dtEmp = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString());

                    if (dtEmp.Rows.Count > 0)
                    {


                        for (int i = 0; i < dtEmp.Rows.Count; i++)
                        {
                            objSer.InsertUserTransfer(dtEmp.Rows[i]["Emp_Id"].ToString(), b.ToString(), false.ToString(), DateTime.Now.ToString(), "1/1/1900", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                        }


                    }


                }
                else
                {
                    dtEmp = objEmp.GetEmployeeMasterAll(Session["CompId"].ToString());
                    dtEmp = new DataView(dtEmp, "Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (dtEmp.Rows.Count > 0)
                    {

                        for (int i = 0; i < dtEmp.Rows.Count; i++)
                        {
                            objSer.InsertUserTransfer(dtEmp.Rows[i]["Emp_Id"].ToString(), b.ToString(), false.ToString(), DateTime.Now.ToString(), "1/1/1900", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                        }
                    }
                }

                DisplayMessage("Record Saved", "green");
                FillGrid();
                Reset();
            }
            else
            {
                DisplayMessage("Record Not Saved");
            }
        }
        else
        {
            string DeviceTypeName = string.Empty;
            DataTable dt1 = objDevice.GetDeviceMaster(Session["CompId"].ToString());
            try
            {
                DeviceTypeName = new DataView(dt1, "Device_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Device_Name"].ToString();
            }
            catch
            {
                DeviceTypeName = "";
            }
            dt1 = new DataView(dt1, "Device_Name='" + txtDeviceName.Text + "' and Device_Name<>'" + DeviceTypeName + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("Device Name Already Exists");
                txtDeviceName.Focus();
                return;

            }
            DataTable dt3 = objDevice.GetDeviceMaster(Session["CompId"].ToString());
            dt3 = new DataView(dt3, "IP_Address='" + txtIPAddress.Text + "' and Device_Id<>'" + editid.Value + "' ", "", DataViewRowState.CurrentRows).ToTable();
            if (dt3.Rows.Count > 0)
            {
                DisplayMessage("IP Address Already Exists");
                txtIPAddress.Focus();
                return;

            }





            b = objDevice.UpdateDeviceMaster(editid.Value, Session["CompId"].ToString(), txtDeviceName.Text, txtDeviceNameL.Text, Session["BrandId"].ToString(), Session["LocId"].ToString(), txtIPAddress.Text, ddlCommType.Text, txtPortNumber.Text, chkMasterdevice.Checked.ToString(), ddlMake.SelectedItem.Value, txtCommunicationKey.Text.Trim(), "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

            if (b != 0)
            {
                DisplayMessage("Record Updated", "green");
                Lbl_Tab_New.Text = Resources.Attendance.New;
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
                Reset();
                FillGrid();


            }
            else
            {
                DisplayMessage("Record Not Updated");
            }

        }
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

    protected void lnkConnect_Click(object sender, EventArgs e)
    {


        int errorcode = 0;
        GridViewRow gvRow = ((GridViewRow)((LinkButton)sender).Parent.Parent);
        string port = gvDevice.DataKeys[gvRow.RowIndex]["Port"].ToString();
        string IP = gvDevice.DataKeys[gvRow.RowIndex]["IP_Address"].ToString();
        string DeviceId = gvDevice.DataKeys[gvRow.RowIndex]["Device_Id"].ToString();
        string strCommunicationType = gvDevice.DataKeys[gvRow.RowIndex]["Communication_Type"].ToString();
        string strMake = gvDevice.DataKeys[gvRow.RowIndex]["field2"].ToString();
        objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(strCommunicationType == "PULL" ? TechType.PULL : TechType.PUSH, strMake);


        bool b = false;
        b = objDeviceOp.Device_Connection(IP, port, 0);
        //b = Common.IsAlive(IP);

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
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }

    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        editid.Value = e.CommandArgument.ToString();




        DataTable dt = objDevice.GetDeviceMasterById(Session["CompId"].ToString(), editid.Value);
        if (dt.Rows.Count > 0)
        {

            txtDeviceName.Text = dt.Rows[0]["Device_Name"].ToString();
            txtCommunicationKey.Text = dt.Rows[0]["Field3"].ToString().Trim();
            txtDeviceNameL.Text = dt.Rows[0]["Device_Name_L"].ToString();
            chkMasterdevice.Checked = Convert.ToBoolean(dt.Rows[0]["Field1"].ToString());


            txtIPAddress.Text = dt.Rows[0]["IP_Address"].ToString();

            txtPortNumber.Text = dt.Rows[0]["Port"].ToString();
            try
            {
                ddlCommType.SelectedValue = dt.Rows[0]["Communication_Type"].ToString();
            }
            catch
            {

            }

            try
            {
                ddlMake.SelectedValue = dt.Rows[0]["field2"].ToString();
            }
            catch
            {

            }
            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }



    }

    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {




        int b = 0;
        b = objDevice.DeleteDeviceMaster(Session["CompId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
            FillGrid();
            ////AllPageCode();
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
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {

        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
        Reset();
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
        //dtPageLevel = new DataView(dtPageLevel, "Param_Name='Page Level'", "", DataViewRowState.CurrentRows).ToTable();
        //if (dtPageLevel.Rows.Count > 0)
        //{
        //    strPageLevel = dtPageLevel.Rows[0]["Param_Value"].ToString();
        //}
        //else
        //{
        //    strPageLevel = "Location";
        //}

        DataTable dt = new DataTable();
        dt = objDevice.GetDeviceMaster(Session["CompId"].ToString());
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

        //AllPageCode();
        Session["dtFilter_Device_Mas"] = dt;
        Session["Device"] = dt;

        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
    }
    public void FillGridBin()
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
        DataTable dtPageLevel = objAppParam.GetApplicationParameterByCompanyId("Page Level", Session["CompId"].ToString());
        dtPageLevel = new DataView(dtPageLevel, "Param_Name='Page Level'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtPageLevel.Rows.Count > 0)
        {
            strPageLevel = dtPageLevel.Rows[0]["Param_Value"].ToString();
        }
        else
        {
            strPageLevel = "Location";
        }

        DataTable dt = new DataTable();
        dt = objDevice.GetDeviceMasterInactive(Session["CompId"].ToString());
        if (strPageLevel == "Company")
        {
            if (dt.Rows.Count > 0)
            {
                if (strFLocId != "")
                {
                    dt = new DataView(dt, "Location_Id in(" + strFLocId.Substring(0, strFLocId.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
            }
        }
        else if (strPageLevel == "Location")
        {
            if (dt.Rows.Count > 0)
            {
                dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvDeviceBin, dt, "", "");

        Session["dtbinFilter"] = dt;
        Session["dtbinDevice"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        if (dt.Rows.Count == 0)
        {
            imgBtnRestore.Visible = false;
        }
        else
        {

            //AllPageCode();
        }

    }



    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvDeviceBin.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvDeviceBin.Rows.Count; i++)
        {
            ((CheckBox)gvDeviceBin.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvDeviceBin.Rows[i].FindControl("lblDeviceId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvDeviceBin.Rows[i].FindControl("lblDeviceId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvDeviceBin.Rows[i].FindControl("lblDeviceId"))).Text.Trim().ToString())
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
                lblSelectedRecord.Text = temp;
            }
        }
    }
    protected void chkgvSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        Label lb = (Label)gvDeviceBin.Rows[index].FindControl("lblDeviceId");
        if (((CheckBox)gvDeviceBin.Rows[index].FindControl("chkgvSelect")).Checked)
        {
            empidlist += lb.Text.Trim().ToString() + ",";
            lblSelectedRecord.Text += empidlist;

        }

        else
        {

            empidlist += lb.Text.ToString().Trim();
            lblSelectedRecord.Text += empidlist;
            string[] split = lblSelectedRecord.Text.Split(',');
            foreach (string item in split)
            {
                if (item != empidlist)
                {
                    if (item != "")
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
            }
            lblSelectedRecord.Text = temp;
        }
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtUnit = (DataTable)Session["dtbinFilter"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtUnit.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Device_Id"]))
                {
                    lblSelectedRecord.Text += dr["Device_Id"] + ",";
                }
            }
            for (int i = 0; i < gvDeviceBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)gvDeviceBin.Rows[i].FindControl("lblDeviceId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvDeviceBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtBinFilter"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvDeviceBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = objDevice.DeleteDeviceMaster(Session["CompId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString().ToString(), DateTime.Now.ToString());

                }
            }
        }

        if (b != 0)
        {

            FillGrid();
            FillGridBin();
            lblSelectedRecord.Text = "";
            ViewState["Select"] = null;
            DisplayMessage("Record Activated");
        }
        else
        {
            int fleg = 0;
            foreach (GridViewRow Gvr in gvDeviceBin.Rows)
            {
                CheckBox chk = (CheckBox)Gvr.FindControl("chkgvSelect");
                if (chk.Checked)
                {
                    fleg = 1;
                }
                else
                {
                    fleg = 0;
                }
            }
            if (fleg == 0)
            {
                DisplayMessage("Please Select Record");
            }
            else
            {
                DisplayMessage("Record Not Activated");
            }
        }

    }

    public void Reset()
    {
        txtDeviceName.Text = "";
        txtCommunicationKey.Text = "";
        txtDeviceNameL.Text = "";
        chkMasterdevice.Checked = false;
        ddlCommType.SelectedIndex = 0;
        txtIPAddress.Text = "";
        txtPortNumber.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
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




    protected void ddlMake_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMake.SelectedValue.Trim() == "zkTecho")
        {
            ddlCommType.SelectedValue = "Ethernet";
            txtPortNumber.Text = "4370";
            lblDeviceCode.Text = Resources.Attendance.IP_Address;
        }
        else
        {
            ddlCommType.SelectedValue = "PUSH";
            txtPortNumber.Text = "8041";
            lblDeviceCode.Text = Resources.Attendance.Device_Id;
        }
    }
}
