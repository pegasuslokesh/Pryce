using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using PryceDevicesLib;
using System.Web;

public partial class Device_UploadUser : BasePage
{
    EmployeeMaster objEmp = null;
    Common cmn = null;
    Att_DeviceMaster objDevice = null;
    SystemParameter objSys = null;
    IAttDeviceOperation objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(TechType.PULL, "zkTecho");
    //Device_Operation_Lan objDeviceOp = new Device_Operation_Lan();
    Set_ApplicationParameter objAppParam = null;
    LocationMaster ObjLocationMaster = null;
    RoleDataPermission objRoleData = null;
    RoleMaster objRole = null;
    Set_Location_Department objLocDept = null;
    string strPageLevel = string.Empty;
    PegasusDataAccess.DataAccessClass objDa = null;
    Att_DeviceGroupMaster ObjDeviceGroup = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objDevice = new Att_DeviceMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        //Device_Operation_Lan objDeviceOp = new Device_Operation_Lan();
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        objRoleData = new RoleDataPermission(Session["DBConnection"].ToString());
        objRole = new RoleMaster(Session["DBConnection"].ToString());
        objLocDept = new Set_Location_Department(Session["DBConnection"].ToString());
        objDa = new PegasusDataAccess.DataAccessClass(Session["DBConnection"].ToString());
        ObjDeviceGroup = new Att_DeviceGroupMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());

        Page.Title = objSys.GetSysTitle();
        if (!IsPostBack)
        {
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), "79", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            FillddlLocation();
            FillGrid();
            pnlFailedRec.Visible = false;
            pnlDestDevice.Visible = false;
            FillDeviceGroupDDL();
        }
        AllPageCode();
    }


    public void FillDeviceGroupDDL()
    {
        DataTable dt = ObjDeviceGroup.GetHeaderAllTrueRecord(Session["CompId"].ToString(), Session["BRandId"].ToString(), Session["LocId"].ToString());
        //this function for bind control by function in common class by jitendra upadhyay on 06-05-2015
        objPageCmn.FillData((object)ddlDeviceGroup, dt, "Group_Name", "Group_Id");
    }


    protected void gvDevice_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {


    }

    protected void gvEmp_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtEmpUpload"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtEmpUpload"] = dt;
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmp, dt, "", "");
    }

    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(Session["DBConnection"].ToString());

        //New Code 
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName("79", (DataTable)Session["ModuleName"]);
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
            btnNext.Visible = true;
        }
        else
        {
            DataTable dtAllPageCode = cmn.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, "79",Session["CompId"].ToString());
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
                            btnNext.Visible = true;
                        }

                        if (DtRow["Op_Id"].ToString() == "2")
                        {
                            btnNext.Visible = true;
                        }

                        if (DtRow["Op_Id"].ToString() == "8")
                        {
                            btnNext.Visible = true;
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
    protected void btnUploadUser_Click(object sender, EventArgs e)
    {
        DataTable dtuser = new DataTable();
        DataTable dtFailedRec = new DataTable();

        DataTable dtAllUser = new DataTable();
        dtAllUser.Columns.Add("Name");
        dtAllUser.Columns.Add("UserID");
        dtAllUser.Columns.Add("Privilege");
        dtAllUser.Columns.Add("Password");
        dtAllUser.Columns.Add("CardNumber");
        dtAllUser.Columns.Add("Enabled");
        DataRow dr;

        DataTable dtFinger = new DataTable();

        dtFinger.Columns.Add("sdwEnrollNumber");
        dtFinger.Columns.Add("sName");
        dtFinger.Columns.Add("idwFingerIndex");
        dtFinger.Columns.Add("sTmpData");
        dtFinger.Columns.Add("iPrivilege");

        dtFinger.Columns.Add("sPassword");
        dtFinger.Columns.Add("sEnabled");

        dtFinger.Columns.Add("iFlag");

        DataTable dtFace = new DataTable();
        dtFace.Columns.Add("sUserID");
        dtFace.Columns.Add("sName");
        dtFace.Columns.Add("sPassword");
        dtFace.Columns.Add("iPrivilege");
        dtFace.Columns.Add("iFaceIndex");
        dtFace.Columns.Add("sTmpData");
        dtFace.Columns.Add("iLength");
        dtFace.Columns.Add("bEnabled");

        DataTable dtDbFinger = new DataTable();
        DataTable dtDbFace = new DataTable();
        if (lblSelectRecd.Text != "")
        {
            dtuser = objEmp.GetEmployeeMasterWithDeviceData(Session["CompId"].ToString());
            string strselectedempIdList = lblSelectRecd.Text.Substring(0, lblSelectRecd.Text.Length - 1);
            if (chkuploadFaceFinger.Checked)
            {
                dtuser = new DataView(dtuser, "Emp_Code in (" + strselectedempIdList + ") and (Template3 <>'' or Template4 <>'')", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                dtuser = new DataView(dtuser, "Emp_Code in (" + strselectedempIdList + ")", "", DataViewRowState.CurrentRows).ToTable();
            }
            if (chkUploadFP.Checked == true)
            {
                dtDbFinger = new DataView(dtuser, "Template3 <>''", "", DataViewRowState.CurrentRows).ToTable();
            }
            if (chkUploadFace.Checked == true)
            {
                dtDbFace = new DataView(dtuser, "Template4 <>''", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        if (dtuser.Rows.Count == 0)
        {
            DisplayMessage("Face Or Finger is not available");
            return;
        }


        bool b = false;
        bool Connect = false;
        dtFailedRec = dtuser.Clone();
        bool IsDeviceSelected = false;
        string strEmpCode = string.Empty;
        foreach (GridViewRow gvdevicerow in gvDevice.Rows)
        {
            if (((CheckBox)gvdevicerow.FindControl("chkSelectDevice")).Checked)
            {
                objDeviceOp = PryceDevicesLib.DeviceOperation.getDeviceLibObj(gvDevice.DataKeys[gvdevicerow.RowIndex]["Communication_Type"].ToString() == "PULL" ? TechType.PULL : TechType.PUSH, gvDevice.DataKeys[gvdevicerow.RowIndex]["field2"].ToString());
                DataTable dt = objDevice.GetDeviceMasterById(Session["CompId"].ToString(), gvDevice.DataKeys[gvdevicerow.RowIndex]["Device_Id"].ToString());

                string IP = dt.Rows[0]["IP_Address"].ToString();
                int port = Convert.ToInt32(dt.Rows[0]["Port"]);

                Connect = objDeviceOp.Device_Connection(IP, port.ToString(), 0);
                IsDeviceSelected = true;
                //DataRowCollection
                DataRow[] drUser;
                DataRow[] drUserFp;
                DataRow[] drUserFace;

                if (Connect == true)
                {


                    string strAdminVal = string.Empty;

                    if (gvDevice.DataKeys[gvdevicerow.RowIndex]["field2"].ToString().Trim() == "a7Plus")
                    {
                        strAdminVal = "MANAGER";
                    }
                    else
                    {
                        strAdminVal = "3";
                    }



                    for (int i = 0; i < dtuser.Rows.Count; i++)
                    {
                        b = false;

                        if (strEmpCode == dtuser.Rows[i]["Emp_Code"].ToString())
                        {
                            continue;
                        }
                        else
                        {
                            strEmpCode = dtuser.Rows[i]["Emp_Code"].ToString();
                        }
                        dr = dtAllUser.NewRow();
                        int emplength = dtuser.Rows[i]["Emp_Name"].ToString().Trim().Length;
                        dr["Name"] = dtuser.Rows[i]["Emp_Name"].ToString().Trim().Length > 22 ? dtuser.Rows[i]["Emp_Name"].ToString().Trim().Substring(0, 22) : dtuser.Rows[i]["Emp_Name"].ToString().Trim();
                        dr["UserID"] = dtuser.Rows[i]["Emp_Code"].ToString();
                        if (chkuploadadmin.Checked)
                        {
                            dr["Privilege"] = strAdminVal;
                        }
                        else
                        {
                            dr["Privilege"] = dtuser.Rows[i]["Template1"].ToString();
                        }
                        dr["Password"] = dtuser.Rows[i]["Template2"].ToString();
                        dr["CardNumber"] = dtuser.Rows[i]["CardNo"].ToString();
                        dr["Enabled"] = dtuser.Rows[i]["sEnabled"].ToString();
                        dtAllUser.Rows.Add(dr);

                    }




                    if (chkUploadFP.Checked == true)
                    {
                        for (int i = 0; i < dtDbFinger.Rows.Count; i++)
                        {
                            b = false;
                            DataRow dr1 = dtFinger.NewRow();
                            dr1["sdwEnrollNumber"] = dtDbFinger.Rows[i]["Emp_Code"].ToString();
                            dr1["sName"] = dtDbFinger.Rows[i]["Emp_Name"].ToString().Trim().Length > 22 ? dtDbFinger.Rows[i]["Emp_Name"].ToString().Trim().Substring(0, 22) : dtDbFinger.Rows[i]["Emp_Name"].ToString().Trim();
                            dr1["idwFingerIndex"] = dtDbFinger.Rows[i]["idwFingerIndex"].ToString();
                            dr1["sPassword"] = dtDbFinger.Rows[i]["Template2"].ToString();
                            if (chkuploadadmin.Checked)
                            {
                                dr1["iPrivilege"] = strAdminVal;
                            }
                            else
                            {
                                dr1["iPrivilege"] = dtDbFinger.Rows[i]["Template1"].ToString();
                            }

                            dr1["sTmpData"] = dtDbFinger.Rows[i]["Template3"].ToString();
                            dr1["sEnabled"] = dtDbFinger.Rows[i]["sEnabled"].ToString();
                            dr1["iFlag"] = dtDbFinger.Rows[i]["iFlag"].ToString();
                            dtFinger.Rows.Add(dr1);

                        }
                    }

                    if (chkUploadFace.Checked)
                    {
                        for (int i = 0; i < dtDbFace.Rows.Count; i++)
                        {
                            b = false;
                            DataRow dr2 = dtFace.NewRow();
                            dr2["sUserID"] = dtDbFace.Rows[i]["Emp_Code"].ToString();
                            dr2["sName"] = dtDbFace.Rows[i]["Emp_Name"].ToString().Trim().Length > 22 ? dtDbFace.Rows[i]["Emp_Name"].ToString().Trim().Substring(0, 22) : dtDbFace.Rows[i]["Emp_Name"].ToString().Trim();

                            dr2["sPassword"] = dtDbFace.Rows[i]["Template2"].ToString();
                            if (chkuploadadmin.Checked)
                            {
                                dr2["iPrivilege"] = strAdminVal;
                            }
                            else
                            {
                                dr2["iPrivilege"] = dtDbFace.Rows[i]["Template1"].ToString();
                            }
                            dr2["iFaceIndex"] = dtDbFace.Rows[i]["Template5"].ToString();
                            dr2["sTmpData"] = dtDbFace.Rows[i]["Template4"].ToString();
                            dr2["iLength"] = dtDbFace.Rows[i]["Template8"].ToString();
                            dr2["bEnabled"] = dtDbFace.Rows[i]["sEnabled"].ToString();

                            dtFace.Rows.Add(dr2);
                        }

                        try
                        {

                        }
                        catch
                        {

                        }
                    }
                    b = objDeviceOp.UploadCardUserFpFace(IP, port.ToString(), dtAllUser, dtFinger, dtFace, true);
                }
                else
                {
                    DisplayMessage("Unable to connect the device");
                }
            }
        }
        if (!IsDeviceSelected)
        {
            DisplayMessage("Please Select Device");
        }
        if (dtAllUser.Rows.Count > 0)
        {
            DisplayMessage("User Uploaded Successfully");
            chkuploadadmin.Checked = false;
        }

    }
    //}
    protected void btnBackToList_Click(object sender, EventArgs e)
    {
        gvFailedRecord.DataSource = null;
        gvFailedRecord.DataBind();
        pnlDestDevice.Visible = false;
        pnlFailedRec.Visible = false;
        pnlList.Visible = true;

    }
    public void FillDeviceGrid()
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
        }
        else if (strPageLevel == "Location")
        {
            if (dt.Rows.Count > 0)
            {
                dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
        }

        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvDevice, dt, "", "");

        Session["dtFilter_Upload_User"] = dt;
        Session["Device"] = dt;
    }

    protected void chkSelAll_CheckedChanged1(object sender, EventArgs e)
    {
        bool b = ((CheckBox)sender).Checked;
        foreach (GridViewRow gvrow in gvDevice.Rows)
        {
            ((CheckBox)gvrow.FindControl("chkSelectDevice")).Checked = b;
        }
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        //Add Code On 29-05-2015
        string empidlist = string.Empty;
        string temp = string.Empty;

        if (lblSelectRecd.Text == "")
        {
            foreach (GridViewRow gvr in gvEmp.Rows)
            {
                CheckBox chkSelect = (CheckBox)gvr.FindControl("chkgvSelect");
                Label lblEmpId = (Label)gvr.FindControl("lblEmpId");

                if (chkSelect.Checked)
                {
                    empidlist += "'" + lblEmpId.Text.Trim().ToString() + "'" + ",";
                    lblSelectRecd.Text = empidlist;
                }
            }
        }
        //End Code

        if (lblSelectRecd.Text == "")
        {
            DisplayMessage("Please select at least one employee");
            return;
        }
        else
        {
            pnlDestDevice.Visible = true;
            pnlList.Visible = false;


            DataTable dt = objDevice.GetDeviceMaster(Session["CompId"].ToString());
            dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "' and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

            if (ddlDeviceGroup.SelectedIndex > 0)
            {
                string strDevicelist = string.Empty;
                DataTable dtDetail = ObjDeviceGroup.GetDetailRecord(ddlDeviceGroup.SelectedValue);

                foreach (DataRow dr in dtDetail.Rows)
                {
                    strDevicelist += dr["Device_Id"] + ",";
                }

                if (strDevicelist != "")
                {
                    dt = new DataView(dt, "Device_Id in (" + strDevicelist.Substring(0, strDevicelist.Length - 1) + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
                else
                {
                    dt = new DataView(dt, "Device_Id=0", "", DataViewRowState.CurrentRows).ToTable();
                }
            }

            if (dt.Rows.Count > 0)
            {
                //Common Function add By Lokesh on 23-05-2015
                objPageCmn.FillData((object)gvDevice, dt, "", "");
            }
            else
            {
                DisplayMessage("No device exists");
            }
        }
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        pnlDestDevice.Visible = false;
        pnlFailedRec.Visible = false;
        pnlList.Visible = true;
        chkUploadFace.Checked = false;
        chkUploadFP.Checked = false;
        FillGrid();
        lblSelectRecd.Text = "";
    }
    public void FillGrid()
    {
        string strLocId = string.Empty;


        foreach (ListItem li in listEmpLocation.Items)
        {
            if (li.Selected)
            {
                if (strLocId == "")
                {
                    strLocId = li.Value;
                }
                else
                {
                    strLocId = strLocId + "," + li.Value;
                }
            }
        }

        if (strLocId == "")
        {
            strLocId = Session["LocId"].ToString();
        }

        string sql = string.Empty;
        if (ddlDeviceGroup.SelectedIndex > 0)
        {
            sql = "select set_employeemaster.Company_Id,set_employeemaster.Brand_Id,set_employeemaster.Location_Id,set_employeemaster.Department_Id,  set_employeemaster.emp_id,cast( set_employeemaster.emp_code as int) as Emp_code,set_employeemaster.Emp_Name,set_employeemaster.Emp_Name_L,set_employeemaster.Email_Id,set_employeemaster.Phone_No,case when set_employeeInformation.Template3<>'' then 'True' else 'False' end as Is_Finger,case when set_employeeInformation.Template4<>'' then 'True' else 'False' end as Is_Face   from set_employeemaster left join set_employeeInformation on set_employeemaster.emp_id =set_employeeInformation.emp_id  where company_id=" + Session["CompId"].ToString() + "  AND Field2 = 'False' and emp_type='On Role' and device_group_Id=" + ddlDeviceGroup.SelectedValue.Trim() + " AND IsActive = 'True' order by cast( set_employeemaster.emp_code as int)";
        }
        else
        {
            sql = "select set_employeemaster.Company_Id,set_employeemaster.Brand_Id,set_employeemaster.Location_Id,set_employeemaster.Department_Id,  set_employeemaster.emp_id,cast( set_employeemaster.emp_code as int) as Emp_code,set_employeemaster.Emp_Name,set_employeemaster.Emp_Name_L,set_employeemaster.Email_Id,set_employeemaster.Phone_No,case when set_employeeInformation.Template3<>'' then 'True' else 'False' end as Is_Finger,case when set_employeeInformation.Template4<>'' then 'True' else 'False' end as Is_Face   from set_employeemaster left join set_employeeInformation on set_employeemaster.emp_id =set_employeeInformation.emp_id  where company_id=" + Session["CompId"].ToString() + "  AND Field2 = 'False' and emp_type='On Role' AND IsActive = 'True' order by cast( set_employeemaster.emp_code as int)";
        }
        DataTable dtEmp = objDa.return_DataTable(sql);

        if (dtEmp != null && dtEmp.Rows.Count > 0)
        {
            dtEmp = dtEmp.DefaultView.ToTable(true, "Company_Id", "Brand_Id", "Location_Id", "Department_Id", "emp_id", "Emp_code", "Emp_Name", "Emp_Name_L", "Email_Id", "Phone_No", "Is_Finger", "Is_Face");

            if (dtEmp.Rows.Count > 0)
            {
                dtEmp = new DataView(dtEmp, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id in (" + strLocId + ")", "", DataViewRowState.CurrentRows).ToTable();
            }

            //Session["dtEmpUpload"] = dtGrid;
            Session["dtEmpUpload"] = dtEmp;
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmp, dtEmp, "", "");
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + dtEmp.Rows.Count.ToString() + "";

        }
        else
        {
            Session["dtEmpUpload"] = null;
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmp, null, "", "");
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : 0";
        }
        AllPageCode();
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


    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        if (ddlOption1.SelectedIndex != 0)
        {
            string condition = string.Empty;
            if (ddlOption1.SelectedIndex == 1)
            {
                condition = "convert(" + ddlField1.SelectedValue + ",System.String)='" + txtValue1.Text.Trim() + "'";
            }
            else if (ddlOption1.SelectedIndex == 2)
            {
                condition = "convert(" + ddlField1.SelectedValue + ",System.String) like '%" + txtValue1.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlField1.SelectedValue + ",System.String) Like '" + txtValue1.Text.Trim() + "%'";

            }
            DataTable dtEmp = (DataTable)Session["dtEmpUpload"];
            DataView view = new DataView(dtEmp, condition, "", DataViewRowState.CurrentRows);
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmp, view.ToTable(), "", "");
            Session["dtEmpUpload"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
        }
        AllPageCode();
        txtValue1.Focus();
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        lblSelectRecd.Text = "";
        ddlField1.SelectedIndex = 1;
        ddlOption1.SelectedIndex = 2;
        txtValue1.Text = "";
        FillGrid();
    }
    protected void gvEmp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmp.PageIndex = e.NewPageIndex;
        DataTable dtEmp = (DataTable)Session["dtEmpUpload"];
        //Common Function add By Lokesh on 23-05-2015
        objPageCmn.FillData((object)gvEmp, dtEmp, "", "");
        string temp = string.Empty;

        for (int i = 0; i < gvEmp.Rows.Count; i++)
        {
            Label lblconid = (Label)gvEmp.Rows[i].FindControl("lblEmpId");
            string[] split = lblSelectRecd.Text.Split(',');

            for (int j = 0; j < lblSelectRecd.Text.Split(',').Length; j++)
            {
                if (lblSelectRecd.Text.Split(',')[j] != "")
                {
                    if ("'" + lblconid.Text.Trim().ToString() + "'" == lblSelectRecd.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvEmp.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }



    }
    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvEmp.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvEmp.Rows.Count; i++)
        {
            ((CheckBox)gvEmp.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectRecd.Text.Split(',').Contains(((Label)(gvEmp.Rows[i].FindControl("lblEmpId"))).Text.Trim().ToString()))
                {
                    lblSelectRecd.Text += "'" + ((Label)(gvEmp.Rows[i].FindControl("lblEmpId"))).Text.Trim().ToString() + "'" + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectRecd.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvEmp.Rows[i].FindControl("lblEmpId"))).Text.Trim().ToString())
                    {
                        if (item != "")
                        {
                            temp += item + ",";
                        }
                    }
                }
                lblSelectRecd.Text = temp;
            }
        }
    }
    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtProduct = (DataTable)Session["dtEmpUpload"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtProduct.Rows)
            {
                if (!lblSelectRecd.Text.Split(',').Contains("'" + dr["Emp_Code"] + "'"))
                {
                    lblSelectRecd.Text += "'" + dr["Emp_Code"] + "'" + ",";
                }
            }
            for (int i = 0; i < gvEmp.Rows.Count; i++)
            {
                string[] split = lblSelectRecd.Text.Split(',');
                Label lblconid = (Label)gvEmp.Rows[i].FindControl("lblEmpId");
                for (int j = 0; j < lblSelectRecd.Text.Split(',').Length; j++)
                {
                    if (lblSelectRecd.Text.Split(',')[j] != "")
                    {
                        if ("'" + lblconid.Text.Trim().ToString() + "'" == lblSelectRecd.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvEmp.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectRecd.Text = "";
            DataTable dtProduct1 = (DataTable)Session["dtEmpUpload"];
            //Common Function add By Lokesh on 23-05-2015
            objPageCmn.FillData((object)gvEmp, dtProduct1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void btnbindLOCEmp_Click(object sender, EventArgs e)
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
            listEmpLocation.DataSource = null;
            listEmpLocation.DataBind();
            listEmpLocation.DataSource = dtLoc;
            listEmpLocation.DataTextField = "Location_Name";
            listEmpLocation.DataValueField = "Location_Id";
            listEmpLocation.DataBind();
            listEmpLocation.SelectedValue = Session["LocId"].ToString();
        }
        else
        {
            try
            {
                listEmpLocation.Items.Clear();
                listEmpLocation.DataSource = null;
                listEmpLocation.DataBind();
                ListItem li = new ListItem("--Select--", "0");
                listEmpLocation.Items.Insert(0, li);
                listEmpLocation.SelectedIndex = 0;
            }
            catch
            {
                ListItem li = new ListItem("--Select--", "0");
                listEmpLocation.Items.Insert(0, li);
                listEmpLocation.SelectedIndex = 0;
            }
        }
    }

    protected void ddlDeviceGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
}
