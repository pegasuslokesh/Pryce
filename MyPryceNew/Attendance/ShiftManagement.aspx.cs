using ClosedXML.Excel;
using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Attendance_ShiftManagement : BasePage
{
    Set_ApplicationParameter objAppParam = null;
    Set_ApplicationParameter objparam = null;
    Common cmn = null;
    SystemParameter objSys = null;
    Att_ShiftManagement objShift = null;
    Att_TimeTable objTimeTable = null;
    Att_ShiftDescription objShiftDesc = null;
    Att_ScheduleMaster objSch = null;
    DataTable DtTime = new DataTable();
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        //AllPageCode();

        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objparam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objShift = new Att_ShiftManagement(Session["DBConnection"].ToString());
        objTimeTable = new Att_TimeTable(Session["DBConnection"].ToString());
        objShiftDesc = new Att_ShiftDescription(Session["DBConnection"].ToString());
        objSch = new Att_ScheduleMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Common.clsPagePermission clsPagePermission = cmn.getPagePermission("../Attendance/ShiftManagement.aspx", HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["UserId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString());
            if (clsPagePermission.bHavePermission == false)
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
            }
            AllPageCode(clsPagePermission);
            objPageCmn.fillLocationWithAllOption(ddlLocation);
            objPageCmn.fillLocation(ddlLocNew);
            //fillLocation();
            txtValue.Focus();
            FillGridBin();
            FillGrid();
            btnList_Click(null, null);
            btnAddTime.Visible = false;
            btnClearAll.Visible = false;
            btnDelete.Visible = false;
            Session["DtTime"] = null;
        }
        CalendarExtender2.Format = objSys.SetDateFormat();
        this.RegisterPostBackControl();
    }
    private void RegisterPostBackControl()
    {
        ScriptManager.GetCurrent(this).RegisterPostBackControl(Btn_Download_Excel);
    }
    public void AllPageCode(Common.clsPagePermission clsPagePermission)
    {
        hdnCanView.Value = clsPagePermission.bView.ToString().ToLower();
        hdnCanEdit.Value = clsPagePermission.bEdit.ToString().ToLower();
        hdnCanDelete.Value = clsPagePermission.bDelete.ToString().ToLower();
        btnSave.Visible = clsPagePermission.bAdd;
       // imgBtnRestore.Visible = clsPagePermission.bRestore;
    }
    public string GetDate(object obj)
    {
        DateTime Date = new DateTime();
        Date = Convert.ToDateTime(obj.ToString());
        return Date.ToString(objSys.SetDateFormat());
    }
    protected void btnList_Click(object sender, EventArgs e)
    {
        Session["DtTime"] = null;
        ddlFieldName.Focus();
    }
    protected void txtShiftName_OnTextChanged(object sender, EventArgs e)
    {
        if (editid.Value == "")
        {
            DataTable dt = objShift.GetShiftMasterByShiftName(Session["CompId"].ToString().ToString(), txtShiftName.Text.Trim());
            // Modified By Nitin On 10/11/2014 to get on Brand Level
            dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt.Rows.Count > 0)
            {
                txtShiftName.Text = "";
                DisplayMessage("Shift Name Already Exists");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtShiftName);
                return;
            }
            DataTable dt1 = objShift.GetShiftMasterInactive(Session["CompId"].ToString().ToString());
            // Modified By Nitin On 10/11/2014 to get on Brand Level
            dt1 = new DataView(dt1, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            dt1 = new DataView(dt1, "Shift_Name='" + txtShiftName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                txtShiftName.Text = "";
                DisplayMessage("Shift Name Already Exists - Go to Bin Tab");
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtShiftName);
                return;
            }
            txtShiftNameL.Focus();
        }
        else
        {
            DataTable dtTemp = objShift.GetShiftMasterById(Session["CompId"].ToString().ToString(), editid.Value);
            // Modified By Nitin On 10/11/2014 to get on Brand Level
            dtTemp = new DataView(dtTemp, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtTemp.Rows.Count > 0)
            {
                if (dtTemp.Rows[0]["Shift_Name"].ToString() != txtShiftName.Text)
                {
                    DataTable dt = objShift.GetShiftMaster(Session["CompId"].ToString().ToString());
                    // Modified By Nitin On 10/11/2014 to get on Brand Level
                    dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    dt = new DataView(dt, "Shift_Name='" + txtShiftName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt.Rows.Count > 0)
                    {
                        txtShiftName.Text = "";
                        DisplayMessage("Shift Name Already Exists");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtShiftName);
                        return;
                    }
                    DataTable dt1 = objShift.GetShiftMaster(Session["CompId"].ToString().ToString());
                    // Modified By Nitin On 10/11/2014 to get on Brand Level
                    dt1 = new DataView(dt1, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    dt1 = new DataView(dt1, "Shift_Name='" + txtShiftName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dt1.Rows.Count > 0)
                    {
                        txtShiftName.Text = "";
                        DisplayMessage("Shift Name Already Exists - Go to Bin Tab");
                        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtShiftName);
                        return;
                    }
                }
            }
            txtShiftNameL.Focus();
        }
    }
    protected void btnBin_Click(object sender, EventArgs e)
    {
        Session["DtTime"] = null;
        ddlbinFieldName.Focus();
        PnlViewShift.Visible = false;
        FillGridBin();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();

        ddlOption.SelectedIndex = 2;
        //ddlFieldName.SelectedIndex = 2;
        txtValue.Text = "";
        ddlbinFieldName.Focus();
    }

    protected void btnbind_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
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
            DataTable dtCust = (DataTable)Session["Shift"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtFilter_Shift_Mng"] = view.ToTable();
            lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 21-05-2015
            objPageCmn.FillData((object)gvShift, view.ToTable(), "", "");
            //AllPageCode();
            btnRefresh.Focus();
        }
        txtValue.Focus();
    }
    protected void btnbinbind_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        if (txtbinValue.Text.Trim().ToString() == "")
        {
            txtbinValue.Text = "";
            DisplayMessage("Please Fill Value");
            txtbinValue.Focus();
            return;

        }
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
            DataTable dtCust = (DataTable)Session["dtbinShift"];


            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["dtbinFilter"] = view.ToTable();
            lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + view.ToTable().Rows.Count.ToString() + "";

            //Common Function add By Lokesh on 21-05-2015
            objPageCmn.FillData((object)gvShiftBin, view.ToTable(), "", "");

            if (view.ToTable().Rows.Count == 0)
            {
                //imgBtnRestore.Visible = false;
                //ImgbtnSelectAll.Visible = false;
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
        ddlbinFieldName.Focus();
    }

    protected void gvShiftBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        gvShiftBin.PageIndex = e.NewPageIndex;
        if (HDFSortbin.Value == "")
            FillGridBin();
        else
        {
            DataTable dt = (DataTable)Session["dtbinFilter"];
            //Common Function add By Lokesh on 22-05-2015
            objPageCmn.FillData((object)gvShiftBin, dt, "", "");
            //AllPageCode();
        }

        string temp = string.Empty;
        bool isselcted;

        for (int i = 0; i < gvShiftBin.Rows.Count; i++)
        {
            Label lblconid = (Label)gvShiftBin.Rows[i].FindControl("lblShiftId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvShiftBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                    }
                }
            }
        }

    }

    protected void gvShiftView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvShiftView.PageIndex = e.NewPageIndex;
        DataTable dt = (DataTable)Session["ViewShiftDt"];
        if (dt.Rows.Count > 0)
        {
            //Common Function add By Lokesh on 21-05-2015
            objPageCmn.FillData((object)gvShiftView, dt, "", "");
        }
    }
    protected void gvShiftBin_OnSorting(object sender, GridViewSortEventArgs e)
    {
        lblSelectedRecord.Text = "";
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = objShift.GetShiftMasterInactive(Session["CompId"].ToString());
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtbinFilter"] = dt;
        //Common Function add By Lokesh on 21-05-2015
        objPageCmn.FillData((object)gvShiftBin, dt, "", "");
        //AllPageCode();
        gvShiftBin.HeaderRow.Focus();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        PanelShiftAss.Visible = false;

    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        Session["DtTime"] = null;
        int TransId = 0;

        DataTable dtScatch = (DataTable)Session["dtScatch"];
        int flag = 0;

        for (int j = 0; j < chkTimeTableList.Items.Count; j++)
        {

            if (chkTimeTableList.Items[j].Selected)
            {
                flag = 1;

            }
        }
        if (flag == 0)
        {

            DisplayMessage("Please select a Time Table");


        }
        int flag1 = 0;

        for (int j = 0; j < chkDayUnderPeriod.Items.Count; j++)
        {

            if (chkDayUnderPeriod.Items[j].Selected)
            {
                flag1 = 1;

            }

        }


        if (flag1 == 0)
        {

            DisplayMessage("Please select a Day for Time Table");


        }
        string strMsg = string.Empty;

        DataTable dtShiftDes = objShiftDesc.GetShiftDescriptionByShiftId(editid.Value);

        if (dtShiftDes.Rows.Count == 0)
        {
            for (int i = 0; i < chkDayUnderPeriod.Items.Count; i++)
            {


                string CycleType = string.Empty;
                string CycleDay = string.Empty;

                if (ddlCycleUnit.SelectedIndex == 1)
                {
                    CycleType = chkDayUnderPeriod.Items[i].Value.Split('-')[0].ToString();
                    CycleDay = chkDayUnderPeriod.Items[i].Value.Split('-')[1].ToString();

                }
                else
                {
                    CycleType = chkDayUnderPeriod.Items[i].Value.Split('-')[0].ToString() + "-" + chkDayUnderPeriod.Items[i].Value.Split('-')[1].ToString();
                    CycleDay = chkDayUnderPeriod.Items[i].Value.Split('-')[2].ToString();
                }
                if (chkDayUnderPeriod.Items[i].Selected == false)
                {
                    for (int j = 0; j < chkTimeTableList.Items.Count; j++)
                    {

                        if (chkTimeTableList.Items[j].Selected)
                        {

                            DataTable dt = objShiftDesc.GetShiftDescriptionByShiftId(editid.Value);

                            dt = new DataView(dt, "Cycle_Type='" + CycleType + "' and Cycle_Day='" + CycleDay + "'", "", DataViewRowState.CurrentRows).ToTable();

                            if (dt.Rows.Count == 0)
                            {
                                // TransId=objShiftDesc.InsertShiftDescription(editid.Value, CycleType, CycleDay,"", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

                                // objShiftDesc.InsertShift_TimeTable(TransId.ToString(), chkTimeTableList.Items[j].Value, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

                            }

                        }
                    }


                }
                else
                {
                    for (int j = 0; j < chkTimeTableList.Items.Count; j++)
                    {
                        string Value = chkTimeTableList.Items[j].Value.ToString();
                        if (chkTimeTableList.Items[j].Selected)
                        {
                            DataTable dtShift = objShiftDesc.GetShiftDescriptionByShiftId(editid.Value);

                            dtShift = new DataView(dtShift, "Cycle_Type='" + CycleType + "' and Cycle_Day = '" + CycleDay + "'", "", DataViewRowState.CurrentRows).ToTable();

                            DataTable dtDisTimeTable = dtShift.DefaultView.ToTable(true, "TimeTable_Id");

                            if (dtDisTimeTable.Rows.Count > 0)
                            {
                                int f = 0;
                                for (int k = 0; k < dtDisTimeTable.Rows.Count; k++)
                                {
                                    DataTable dtin = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString(), dtDisTimeTable.Rows[k]["TimeTable_Id"].ToString());
                                    DateTime dtintime = Convert.ToDateTime(dtin.Rows[0]["OnDuty_Time"]);
                                    DateTime dtouttime = Convert.ToDateTime(dtin.Rows[0]["OffDuty_Time"]);


                                    DataTable dtin1 = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString(), chkTimeTableList.Items[j].Value);
                                    DateTime dtintime1 = Convert.ToDateTime(dtin1.Rows[0]["OnDuty_Time"]);
                                    DateTime dtouttime1 = Convert.ToDateTime(dtin1.Rows[0]["OffDuty_Time"]);

                                    if (ISOverLapTimeTable(dtintime1, dtouttime1, dtintime, dtouttime))
                                    {

                                        f = 1;
                                        strMsg += chkDayUnderPeriod.Items[i].Text + ",";
                                    }
                                    else
                                    {


                                    }

                                }
                                if (f == 0)
                                {
                                    TransId = int.Parse(dtShift.Rows[0]["Trans_Id"].ToString());
                                    //TransId = objShiftDesc.InsertShiftDescription(editid.Value, CycleType, CycleDay, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                    objShiftDesc.InsertShift_TimeTable(TransId.ToString(), chkTimeTableList.Items[j].Value, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                                                    }
                                                            }

                            else
                            {

                                TransId = objShiftDesc.InsertShiftDescription(editid.Value, CycleType, CycleDay, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
                                objShiftDesc.InsertShift_TimeTable(TransId.ToString(), chkTimeTableList.Items[j].Value, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                            }

                        }



                    }



                }



            }


        }
        else
        {
            DataTable dtDisTimeTable = new DataTable();
            for (int i = 0; i < chkDayUnderPeriod.Items.Count; i++)
            {

                string CycleType = string.Empty;
                string CycleDay = string.Empty;
                if (ddlCycleUnit.SelectedIndex == 1)
                {
                    CycleType = chkDayUnderPeriod.Items[i].Value.Split('-')[0].ToString();
                    CycleDay = chkDayUnderPeriod.Items[i].Value.Split('-')[1].ToString();

                }
                else
                {
                    CycleType = chkDayUnderPeriod.Items[i].Value.Split('-')[0].ToString() + "-" + chkDayUnderPeriod.Items[i].Value.Split('-')[1].ToString();
                    CycleDay = chkDayUnderPeriod.Items[i].Value.Split('-')[2].ToString();
                }
                if (chkDayUnderPeriod.Items[i].Selected)
                {
                    for (int j = 0; j < chkTimeTableList.Items.Count; j++)
                    {
                        if (chkTimeTableList.Items[j].Selected)
                        {

                            DataTable dtShift = objShiftDesc.GetShiftDescriptionByShiftId(editid.Value);

                            dtShift = new DataView(dtShift, "Cycle_Type = '" + CycleType + "'  and Cycle_Day='" + CycleDay + "' and convert(TimeTable_Id,System.String) <> ''", "", DataViewRowState.CurrentRows).ToTable();


                            dtDisTimeTable = dtShift.DefaultView.ToTable(true, "TimeTable_Id");

                            if (dtDisTimeTable.Rows.Count > 0)
                            {

                                for (int k = 0; k < dtDisTimeTable.Rows.Count; k++)
                                {
                                    DataTable dtin = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString(), dtDisTimeTable.Rows[k]["TimeTable_Id"].ToString());
                                    DateTime dtintime = Convert.ToDateTime(dtin.Rows[0]["OnDuty_Time"]);
                                    DateTime dtouttime = Convert.ToDateTime(dtin.Rows[0]["OffDuty_Time"]);


                                    DataTable dtin1 = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString(), chkTimeTableList.Items[j].Value);
                                    DateTime dtintime1 = Convert.ToDateTime(dtin1.Rows[0]["OnDuty_Time"]);
                                    DateTime dtouttime1 = Convert.ToDateTime(dtin1.Rows[0]["OffDuty_Time"]);

                                    DataTable dt1 = new DataView(dtShift, "TimeTable_Id='" + dtDisTimeTable.Rows[k]["TimeTable_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                    if (dt1.Rows.Count > 0)
                                    {

                                        if (ISOverLapTimeTable(dtintime1, dtouttime1, dtintime, dtouttime))
                                        {
                                            strMsg += chkDayUnderPeriod.Items[i].Text + ",";
                                            break;

                                        }
                                        else
                                        {
                                            dtShift = objShiftDesc.GetShiftDescriptionByShiftId(editid.Value);

                                            dtShift = new DataView(dtShift, "Cycle_Type = '" + CycleType + "'  and Cycle_Day='" + CycleDay + "'", "", DataViewRowState.CurrentRows).ToTable();



                                            DataTable dtDisTimeTable1 = new DataTable();
                                            dtDisTimeTable1 = dtShift.DefaultView.ToTable(true, "TimeTable_Id");

                                            DataTable dtTimeTab = new DataTable();
                                            dtTimeTab = new DataView(dtDisTimeTable1, "TimeTable_Id='" + chkTimeTableList.Items[j].Value + "'", "", DataViewRowState.CurrentRows).ToTable();

                                            if (dtTimeTab.Rows.Count == 0)
                                            {
                                                TransId = int.Parse(dtShift.Rows[0]["Trans_Id"].ToString());
                                                //TransId = objShiftDesc.InsertShiftDescription(editid.Value, CycleType, CycleDay, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                objShiftDesc.InsertShift_TimeTable(TransId.ToString(), chkTimeTableList.Items[j].Value, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                            }




                                        }
                                    }

                                }


                                //if (dtShift.Rows.Count > 0)
                                //{
                                //    for (int m = 0; m < dtShift.Rows.Count; m++)
                                //    {
                                //        objShiftDesc.DeleteShiftDescriptionByTransId(dtShift.Rows[m]["Trans_Id"].ToString());
                                //        objShiftDesc.DeleteShift_TimeTableByRefId(dtShift.Rows[m]["Trans_Id"].ToString());

                                //    }

                                //   TransId= objShiftDesc.InsertShiftDescription(editid.Value, CycleType, CycleDay, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                //    objShiftDesc.InsertShift_TimeTable(TransId.ToString(), chkTimeTableList.Items[j].Value, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

                            }
                            else
                            {

                                dtShift = objShiftDesc.GetShiftDescriptionByShiftId(editid.Value);
                                dtShift = new DataView(dtShift, "Cycle_Type='" + CycleType + "' and Cycle_Day = '" + CycleDay + "' and  convert(TimeTable_Id,System.String) <> '' ", "", DataViewRowState.CurrentRows).ToTable();
                                dtDisTimeTable = dtShift.DefaultView.ToTable(true, "TimeTable_Id");

                                if (dtDisTimeTable.Rows.Count > 0)
                                {

                                    for (int k = 0; k < dtDisTimeTable.Rows.Count; k++)
                                    {
                                        DataTable dtin = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString(), dtDisTimeTable.Rows[k]["TimeTable_Id"].ToString());
                                        DateTime dtintime = Convert.ToDateTime(dtin.Rows[0]["OnDuty_Time"]);
                                        DateTime dtouttime = Convert.ToDateTime(dtin.Rows[0]["OffDuty_Time"]);


                                        DataTable dtin1 = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString(), chkTimeTableList.Items[j].Value);
                                        DateTime dtintime1 = Convert.ToDateTime(dtin1.Rows[0]["OnDuty_Time"]);
                                        DateTime dtouttime1 = Convert.ToDateTime(dtin1.Rows[0]["OffDuty_Time"]);

                                        DataTable dt1 = new DataView(dtShift, "TimeTable_Id='" + dtDisTimeTable.Rows[k]["TimeTable_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                        if (dt1.Rows.Count > 0)
                                        {

                                            if (ISOverLapTimeTable(dtintime1, dtouttime1, dtintime, dtouttime))
                                            {
                                                strMsg += chkDayUnderPeriod.Items[i].Text + ",";
                                                break;

                                            }
                                            else
                                            {
                                                dtShift = objShiftDesc.GetShiftDescriptionByShiftId(editid.Value);

                                                dtShift = new DataView(dtShift, "Cycle_Type = '" + CycleType + "'  and Cycle_Day='" + CycleDay + "'", "", DataViewRowState.CurrentRows).ToTable();



                                                DataTable dtDisTimeTable1 = new DataTable();
                                                dtDisTimeTable1 = dtShift.DefaultView.ToTable(true, "TimeTable_Id");

                                                DataTable dtTimeTab = new DataTable();
                                                dtTimeTab = new DataView(dtDisTimeTable1, "TimeTable_Id='" + chkTimeTableList.Items[j].Value + "'", "", DataViewRowState.CurrentRows).ToTable();

                                                if (dtTimeTab.Rows.Count == 0)
                                                {
                                                    TransId = int.Parse(dtShift.Rows[0]["Trans_Id"].ToString());

                                                    //TransId= objShiftDesc.InsertShiftDescription(editid.Value, CycleType, CycleDay, "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                                                    objShiftDesc.InsertShift_TimeTable(TransId.ToString(), chkTimeTableList.Items[j].Value, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                                }




                                            }
                                        }

                                    }

                                }

                                else
                                {
                                    dtShift = objShiftDesc.GetShiftDescriptionByShiftId(editid.Value);

                                    dtShift = new DataView(dtShift, "Cycle_Type = '" + CycleType + "'  and Cycle_Day='" + CycleDay + "'", "", DataViewRowState.CurrentRows).ToTable();

                                    if (dtShift.Rows.Count == 0)
                                    {
                                        TransId = objShiftDesc.InsertShiftDescription(editid.Value, CycleType, CycleDay, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

                                        objShiftDesc.InsertShift_TimeTable(TransId.ToString(), chkTimeTableList.Items[j].Value, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());


                                    }
                                    else
                                    {
                                        TransId = int.Parse(dtShift.Rows[0]["Trans_Id"].ToString());

                                        objShiftDesc.InsertShift_TimeTable(TransId.ToString(), chkTimeTableList.Items[j].Value, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

                                    }

                                }


                            }

                        }


                    }

                }
            }


        }








        if (editid.Value == "")
        {

            chkTimeTableList.DataSource = null;
            chkTimeTableList.DataBind();
            DisplayMessage("Record Saved", "green");

        }

        else
        {
            if (strMsg != "")
            {
                DisplayMessage(strMsg + " " + "Overlapped Days!");
            }
            else
            {
                chkTimeTableList.DataSource = null;
                chkTimeTableList.DataBind();
                DisplayMessage("Record Updated", "green");
            }
        }
        chkTimeTableList.DataSource = null;
        chkTimeTableList.DataBind();
        Button1_Click(null, null);

    }
    protected void btnCancelPanel_Click1(object sender, EventArgs e)
    {
        Session["DtTime"] = null;
        chkTimeTableList.DataSource = null;
        chkTimeTableList.DataBind();
        PanelShiftAss.Visible = false;
        //    TrShiftName.Visible = false;
        PanView.Visible = false;
        ViewShift(editid.Value);
        PanelShiftAss.Visible = false;
        //  TrShiftName.Visible = false;
        PanelShiftAss.Visible = false;
        Div_Main.Visible = true;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Session["DtTime"] = null;
        for (int t = 0; t < chkTimeTableList.Items.Count; t++)
        {
            chkTimeTableList.Items[t].Selected = false;
        }
        for (int j = 0; j < chkDayUnderPeriod.Items.Count; j++)
        {
            chkDayUnderPeriod.Items[j].Selected = false;
        }

    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (chkTimeTableList.SelectedItem == null)
        {

            DisplayMessage("Select Atleast One TimeTable");
            //btnshowpopup_ModalPopupExtender.Show();
            return;
        }
        else
        {
            PanelShiftAss.Visible = false;
        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        DataTable dttimetable1 = objShiftDesc.GetShiftDescriptionByShiftId(editid.Value);

        if (dttimetable1.Rows.Count == 0)
        {
            DisplayMessage("Please Save Record First");
            return;

        }

        // DataTable dt = objTimeTable.GetTimeTableMaster(Session["CompId"].ToString());

        //DataTable dtTime1 = new DataTable();
        //dtTime1.Columns.Add("EDutyTime");
        //dtTime1.Columns.Add("TimeTableId");

        //for (int i = 0; i < dt.Rows.Count; i++)
        //{
        //    DataRow dr = dtTime1.NewRow();

        //    dr["EDutyTime"] = dt.Rows[i]["ETimeTableName"] + "(" + Convert.ToDateTime(dt.Rows[i]["OnDutyTime"]).ToString("HH:mm") + "-" + Convert.ToDateTime(dt.Rows[i]["OffDutyTime"]).ToString("HH:mm") + ")";
        //    dr["TimeTableId"] = dt.Rows[i]["TimeTableId"].ToString();
        //    dtTime1.Rows.Add(dr);

        //}


        //chkTimeTableList.DataSource = dtTime1;
        //chkTimeTableList.DataTextField = "EDutyTime";
        //chkTimeTableList.DataValueField = "TimeTableId";
        //chkTimeTableList.DataBind();

        editid.Value = txtShiftId.Text;




        GetDays();

        foreach (DataListItem dl in dlView.Items)
        {
            DataList GvTime = (DataList)dl.FindControl("GvTime");

            Label lblDay = (Label)dl.FindControl("lblDays");
            Label lblTime = (Label)dl.FindControl("lblTime");

            string strDaysValue = "";
            strDaysValue = lblDay.Text.Split(',')[0];


            if (strDaysValue == "Thu ")
            {
                strDaysValue = "Thursday";
            }
            else if (strDaysValue == "Fri ")
            {
                strDaysValue = "Friday";
            }
            else if (strDaysValue == "Sat ")
            {
                strDaysValue = "Saturday";
            }
            else if (strDaysValue == "Sun ")
            {
                strDaysValue = "Sunday";
            }
            else if (strDaysValue == "Mon ")
            {
                strDaysValue = "Monday";
            }
            else if (strDaysValue == "Tue ")
            {
                strDaysValue = "Tuesday";
            }
            else if (strDaysValue == "Wed ")
            {
                strDaysValue = "Wednesday";
            }


            //DataTable dtTime = objShiftDesc.GetShiftDiscriptionByVal(editid.Value, strDaysValue);
            //if (dtTime.Rows.Count > 0)
            //{
            //    GvTime.DataSource = dtTime;
            //    GvTime.DataBind();
            //}
        }

        Session["IsView"] = true;
        PanView.Visible = true;

        DataTable dttimetable = objShiftDesc.GetShiftDescriptionByShiftId(editid.Value);
        if (dttimetable.Rows.Count > 0)
        {
            for (int i = 0; i < chkTimeTableList.Items.Count; i++)
            {
                for (int j = 0; j < dttimetable.Rows.Count; j++)
                {
                    if (chkTimeTableList.Items[i].Value == dttimetable.Rows[j]["TimeTable_Id"].ToString())
                    {
                        chkTimeTableList.Items[i].Selected = true;
                    }
                }
            }
        }


    }
    protected void GetDays()
    {

        string[] weekdays = new string[8];
        weekdays[1] = "Monday";
        weekdays[2] = "Tuesday";
        weekdays[3] = "Wednesday";
        weekdays[4] = "Thursday";
        weekdays[5] = "Friday";
        weekdays[6] = "Saturday";
        weekdays[7] = "Sunday";

        int days = Convert.ToInt32(ddlCycleUnit.SelectedValue);
        int cycleno = Convert.ToInt32(txtCycleNo.Text);

        DateTime newappfromdt = Convert.ToDateTime(txtApplyFrom.Text);
        string appfromdt = newappfromdt.ToShortDateString();
        string appfromday = Convert.ToDateTime(newappfromdt).DayOfWeek.ToString().Substring(0, 3);

        DataTable dtfordays = new DataTable();
        dtfordays.Columns.Add("days");
        dtfordays.Columns.Add("dayno");
        int totaldays = days * cycleno;
        int colrept = 0;
        if (totaldays % 7 > 0)
        {
            colrept = 1;
        }
        chkDayUnderPeriod.RepeatColumns = totaldays / 7 + colrept;

        if (ddlCycleUnit.SelectedValue == "1")
        {
            for (int i = 1; i <= totaldays; i++)
            {
                dtfordays.Rows.Add(dtfordays.NewRow());
                dtfordays.Rows[dtfordays.Rows.Count - 1]["days"] = "Days " + i.ToString() + " , " + appfromdt + " , " + appfromday;
                dtfordays.Rows[dtfordays.Rows.Count - 1]["dayno"] = "Days " + i.ToString();
                appfromdt = (Convert.ToDateTime(appfromdt).AddDays(1)).ToShortDateString().ToString();
                appfromday = Convert.ToDateTime(appfromdt).DayOfWeek.ToString().Substring(0, 3);
            }
        }
        else if (ddlCycleUnit.SelectedValue == "7")
        {
            //Added by Fatima
            appfromday = Convert.ToDateTime(appfromdt).DayOfWeek.ToString();
            for (int j = 1; j <= weekdays.Length; j++)
            {
                if (weekdays[j].Equals(appfromday))
                {
                    appfromday = Convert.ToDateTime(appfromdt).ToShortDateString();
                    for (int i = 1; i <= totaldays; i++)
                    {

                        dtfordays.Rows.Add(dtfordays.NewRow());
                        dtfordays.Rows[dtfordays.Rows.Count - 1]["days"] = weekdays[j % 7].ToString().Substring(0, 3) + " , " + appfromdt;
                        dtfordays.Rows[dtfordays.Rows.Count - 1]["dayno"] = weekdays[j % 7].ToString();
                        appfromdt = (Convert.ToDateTime(appfromdt).AddDays(1)).ToShortDateString().ToString();
                        j++;
                    }
                    break;
                }
            }

        }
        else
        {
            string month = Convert.ToDateTime(appfromdt.ToString()).Month.ToString();
            DateTime startdate = new DateTime(Convert.ToDateTime(appfromdt.ToString()).Year, Convert.ToDateTime(appfromdt.ToString()).Month, 1);
            string stdate = Convert.ToDateTime(startdate.ToString()).ToShortDateString().ToString();

            string startday = Convert.ToDateTime(startdate.ToString()).DayOfWeek.ToString().Substring(0, 3);

            for (int i = 1; i <= totaldays; i++)
            {
                int dayno = 1;
                if (i % 31 == 0)
                {
                    dayno = 31;
                }
                else
                {
                    dayno = i % 31;
                }


                dtfordays.Rows.Add(dtfordays.NewRow());
                if (month.Equals(Convert.ToDateTime(stdate).Month.ToString()))
                {
                    dtfordays.Rows[dtfordays.Rows.Count - 1]["days"] = dayno.ToString() + " Days" + " ,( " + stdate + " , " + startday + " )";
                    dtfordays.Rows[dtfordays.Rows.Count - 1]["dayno"] = dayno.ToString() + " Days";
                    stdate = (Convert.ToDateTime(stdate).AddDays(1)).ToShortDateString().ToString();
                    startday = Convert.ToDateTime(stdate).DayOfWeek.ToString().Substring(0, 3);
                    if (dayno == 31)
                        month = Convert.ToDateTime(stdate).Month.ToString();
                }
                else
                {
                    dtfordays.Rows[dtfordays.Rows.Count - 1]["days"] = dayno.ToString() + " Days";
                    dtfordays.Rows[dtfordays.Rows.Count - 1]["dayno"] = dayno.ToString() + " Days";
                    if (dayno == 31)
                        month = Convert.ToDateTime(stdate).Month.ToString();
                }
                //dtfordays.Rows[dtfordays.Rows.Count - 1]["days"] = dayno.ToString() + " Days" + " , " + stdate + " , " + startday;
                //dtfordays.Rows[dtfordays.Rows.Count - 1]["dayno"] = dayno.ToString() + " Days";
                // stdate = (Convert.ToDateTime(stdate).AddDays(1)).ToShortDateString().ToString();
                //startday = Convert.ToDateTime(stdate).DayOfWeek.ToString().Substring(0, 3);
            }
        }

        //Common Function add By Lokesh on 21-05-2015
        objPageCmn.FillData((object)dlView, dtfordays, "", "");
    }
    public bool ISOverLapTimeTable(DateTime dtintime1, DateTime dtouttime1, DateTime dtintime, DateTime dtouttime)
    {
        bool isoverlap = false;
        if (dtintime >= dtintime1 && dtintime <= dtouttime1)
        {
            isoverlap = true;

        }
        else if (dtouttime >= dtintime1 && dtouttime <= dtouttime1)
        {
            isoverlap = true;

        }

        else if (dtintime1 >= dtintime && dtintime1 <= dtouttime)
        {
            isoverlap = true;

        }

        else if (dtouttime1 >= dtintime && dtouttime1 <= dtouttime)
        {
            isoverlap = true;

        }
        else if (dtintime1 == dtintime && dtouttime1 == dtouttime)
        {
            isoverlap = true;

        }
        return isoverlap;
    }
    protected void chkTimeTableList_SelectedIndexChanged(object sender, EventArgs e)
    {


        bool isoverlap = false;



        CheckBoxList list = (CheckBoxList)sender;
        string[] control = Request.Form.Get("__EVENTTARGET").Split('$');
        int idx = control.Length - 1;
        string timetableid = string.Empty;
        try
        {
            timetableid = list.Items[Int32.Parse(control[idx])].Value;
        }
        catch (Exception ex)
        {
            return;
        }

        if (list.Items[Int32.Parse(control[idx])].Selected)
        {
            DataTable dtin = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString(), timetableid);

            DateTime dtintime = Convert.ToDateTime(dtin.Rows[0]["OnDuty_Time"]);
            DateTime dtouttime = Convert.ToDateTime(dtin.Rows[0]["OffDuty_Time"]);
            DateTime OnDutyTime = Convert.ToDateTime(dtin.Rows[0]["OnDuty_Time"]);
            DateTime OffDutyTime = Convert.ToDateTime(dtin.Rows[0]["OffDuty_Time"]);
            if (dtintime > dtouttime)
            {
                dtouttime = dtouttime.AddHours(24);
            }

            if (OnDutyTime > OffDutyTime)
            {
                OffDutyTime = OffDutyTime.AddHours(24);
            }

            for (int i = 0; i < chkTimeTableList.Items.Count; i++)
            {
                if (chkTimeTableList.Items[i].Selected && chkTimeTableList.Items[i].Value != timetableid)
                {
                    DataTable dtin1 = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString(), chkTimeTableList.Items[i].Value);
                    DateTime dtintime1 = Convert.ToDateTime(dtin1.Rows[0]["OnDuty_Time"]);
                    DateTime dtouttime1 = Convert.ToDateTime(dtin1.Rows[0]["OffDuty_Time"]);

                    DateTime OnDutyTime1 = Convert.ToDateTime(dtin1.Rows[0]["OnDuty_Time"]);
                    DateTime OffDutyTime1 = Convert.ToDateTime(dtin1.Rows[0]["OffDuty_Time"]);


                    if (dtintime1 > dtouttime1)
                    {
                        dtouttime1 = dtouttime1.AddHours(24);
                    }

                    if (OnDutyTime1 > OffDutyTime1)
                    {
                        OffDutyTime1 = OffDutyTime1.AddHours(24);
                    }

                    if (dtintime >= dtintime1 && dtintime <= dtouttime1)
                    {
                        isoverlap = true;
                        break;
                    }
                    if (dtouttime >= dtintime1 && dtouttime <= dtouttime1)
                    {
                        isoverlap = true;
                        break;
                    }

                    if (dtintime1 >= dtintime && dtintime1 <= dtouttime)
                    {
                        isoverlap = true;
                        break;
                    }

                    if (dtouttime1 >= dtintime && dtouttime1 <= dtouttime)
                    {
                        isoverlap = true;
                        break;
                    }
                }
            }
        }
        if (isoverlap)
        {
            list.Items[Int32.Parse(control[idx])].Selected = false;
            DisplayMessage("Time Overlaped");
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        int b = 0;

        if (txtShiftName.Text == "")
        {
            DisplayMessage("Enter Shift Name");
            txtShiftName.Focus();
            return;
        }
        if (txtCycleNo.Text == "")
        {
            DisplayMessage("Enter Cycle No.");
            txtCycleNo.Focus();
            return;
        }
        else
        {

        }



        if (txtApplyFrom.Text == "")
        {
            DisplayMessage("Enter Apply Date");
            txtApplyFrom.Focus();
            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtApplyFrom.Text);
            }
            catch
            {
                DisplayMessage("Enter Correct To Date Format dd-MMM-yyyy");
                txtApplyFrom.Text = "";
                txtApplyFrom.Focus();
                return;
            }
        }

        if (editid.Value == "")
        {
            DataTable dt1 = objShift.GetShiftMaster(Session["CompId"].ToString());
            dt1 = new DataView(dt1, "Shift_Name='" + txtShiftName.Text + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("Shift Name Already Exists");
                txtShiftName.Focus();
                return;
            }

            b = objShift.InsertShiftMaster(Session["CompId"].ToString(), txtShiftName.Text, txtShiftNameL.Text, Session["BrandId"].ToString(), txtCycleNo.Text, ddlCycleUnit.SelectedValue, txtApplyFrom.Text, "", "", "", "", ddlLocNew.SelectedValue.Trim(), true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
            if (b != 0)
            {
                DisplayMessage("Record Saved", "green");
                editid.Value = b.ToString();
                btnAddTime.Visible = true;
                btnClearAll.Visible = true;
                btnDelete.Visible = true;
                btnAddTime.Focus();
                ViewShift(editid.Value);
                PnlViewShift.Visible = true;
                // Reset();
                hdn_locID.Value = ddlLocNew.SelectedValue.Trim();
            }
            else
            {
                DisplayMessage("Record Not Saved");
                Reset();
            }
        }
        else
        {
            string ShiftTypeName = string.Empty;
            DataTable dt1 = objShift.GetShiftMaster(Session["CompId"].ToString());
            try
            {
                ShiftTypeName = new DataView(dt1, "Shift_Id='" + editid.Value + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Shift_Name"].ToString();
            }
            catch
            {
                ShiftTypeName = "";
            }
            dt1 = new DataView(dt1, "Shift_Name='" + txtShiftName.Text + "' and Shift_Name<>'" + ShiftTypeName + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                DisplayMessage("Shift Name Already Exists");
                txtShiftName.Focus();
                return;
            }
            b = objShift.UpdateShiftMaster(editid.Value, Session["CompId"].ToString(), txtShiftName.Text, txtShiftNameL.Text, Session["BrandId"].ToString(), txtCycleNo.Text, ddlCycleUnit.SelectedValue, txtApplyFrom.Text, "", "", "", "", ddlLocNew.SelectedValue.Trim(), true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

            if (b != 0)
            {
                DisplayMessage("Record Updated", "green");
                objShiftDesc.DeleteShiftDescriptionByShiftId(editid.Value);
                objShiftDesc.DeleteShift_TimeTableByShiftId(editid.Value);

                ViewShift(editid.Value);
                Reset();
                btnAddTime.Visible = true;
                btnClearAll.Visible = true;
                btnDelete.Visible = true;
            }
            else
            {
                DisplayMessage("Record Not Updated");
                Reset();
            }
        }
    }
    public void SubDtScatch()
    {
        string WeekOff = string.Empty;
        WeekOff = objparam.GetApplicationParameterValueByParamName("Week Off Days", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        string[] weekdays = new string[8];
        weekdays[1] = "Monday";
        weekdays[2] = "Tuesday";
        weekdays[3] = "Wednesday";
        weekdays[4] = "Thursday";
        weekdays[5] = "Friday";
        weekdays[6] = "Saturday";
        weekdays[7] = "Sunday";

        string[] arr = Convert.ToDateTime(txtApplyFrom.Text).ToString("dd-MM-yyyy").Split('-');
        DataTable dtScatch = new DataTable();
        dtScatch.Columns.Add(new DataColumn("Date"));
        dtScatch.Columns.Add(new DataColumn("Day"));
        dtScatch.Columns.Add(new DataColumn("Cycle_Type"));
        dtScatch.Columns.Add(new DataColumn("Cycle_Day"));
        DateTime dtApply = new DateTime(Convert.ToInt16(arr[2]), Convert.ToInt16(arr[1]), Convert.ToInt16(arr[0]));

        int index = ddlCycleUnit.SelectedIndex;

        int TotalDays = 1;
        switch (index)
        {
            case 0:
                TotalDays = TotalDays * Convert.ToInt16(txtCycleNo.Text) * 7;
                break;
            case 1:
                TotalDays = TotalDays * Convert.ToInt16(txtCycleNo.Text);
                break;
            case 2:
                TotalDays = TotalDays * Convert.ToInt16(txtCycleNo.Text) * 31;
                break;
        }
        int a = 1;
        int j = 0;
        for (int k = 1; k <= TotalDays; k++)
        {
            DataRow row = dtScatch.NewRow();
            row[0] = dtApply.ToShortDateString();
            row[1] = dtApply.DayOfWeek.ToString();

            if (index == 0)
            {
                for (int i = 1; i <= 7; i++)
                {

                    if (weekdays[i].ToString() == row[1].ToString())
                    {
                        foreach (string str in WeekOff.Split(','))
                        {
                            if (weekdays[i].ToString() == str)
                            {
                                break;
                            }
                        }

                        row[2] = "Week-" + a.ToString();
                        row[3] = i.ToString();
                    }
                }
                if (k % 7 == 0)
                {
                    a = a + 1;

                }
            }
            else if (index == 2)
            {
                if (k % 31 == 0)
                {

                    j = 1;
                }
                else
                {
                    j = j + 1;
                }
                row[2] = "Month-" + a.ToString();
                row[3] = j.ToString();
                if (k % 31 == 0)
                {
                    a = a + 1;
                }
            }
            else
            {
                row[2] = "Day";
                row[3] = k.ToString();
            }


            dtScatch.Rows.Add(row);

            dtApply = dtApply.AddDays(1);
        }
        //for (int r = 0; r < dtScatch.Rows.Count; r++)
        //{
        //    foreach (string str in WeekOff.Split(','))
        //    {
        //        if (dtScatch.Rows[r]["Day"].ToString() == str)
        //        {

        //        }
        //        else
        //        {
        //        }
        //    }
        //}
        Session["dtScatch"] = dtScatch;


    }

    protected void btnAddShift_Click(object sender, EventArgs e)
    {
        if (txtShiftName.Text == "")
        {
            DisplayMessage("Enter Shift Name");
            txtShiftName.Focus();
            return;
        }

        if (txtCycleNo.Text == "")
        {
            DisplayMessage("Enter Cycle No.");
            txtCycleNo.Focus();
            return;
        }
        if (txtApplyFrom.Text == "")
        {
            DisplayMessage("Enter Apply Date");
            txtApplyFrom.Focus();
            return;
        }
        else
        {

            try
            {
                Convert.ToDateTime(txtApplyFrom.Text);
            }
            catch
            {
                DisplayMessage("Enter Correct To Date Format dd-MMM-yyyy");
                txtApplyFrom.Text = "";
                txtApplyFrom.Focus();
                return;
            }

        }

        SubDtScatch();
        int b = 0;

        //DataTable dt = objTimeTable.GetTimeTableMaster(Session["CompId"].ToString());
        //// Modified By Nitin On 10/11/2014 to get on Brand Level
        //dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //DataTable dtTime1 = new DataTable();
        //dtTime1.Columns.Add("EDutyTime");
        //dtTime1.Columns.Add("TimeTableId");

        //for (int i = 0; i < dt.Rows.Count; i++)
        //{
        //    DataRow dr = dtTime1.NewRow();

        //    dr["EDutyTime"] = dt.Rows[i]["TimeTable_Name"] + "(" + Convert.ToDateTime(dt.Rows[i]["OnDuty_Time"]).ToString("HH:mm") + "-" + Convert.ToDateTime(dt.Rows[i]["OffDuty_Time"]).ToString("HH:mm") + ")";
        //    dr["TimeTableId"] = dt.Rows[i]["TimeTable_Id"].ToString();
        //    dtTime1.Rows.Add(dr);

        //}

        //chkTimeTableList.DataSource = dtTime1;
        //chkTimeTableList.DataTextField = "EDutyTime";
        //chkTimeTableList.DataValueField = "TimeTableId";
        //chkTimeTableList.DataBind();


        if (editid.Value != "")
        {
            PnlViewShift.Visible = false;
            b = objShift.UpdateShiftMaster(editid.Value, Session["CompId"].ToString(), txtShiftName.Text, txtShiftNameL.Text, Session["BrandId"].ToString(), txtCycleNo.Text, ddlCycleUnit.SelectedValue, txtApplyFrom.Text, "", "", "", "", hdn_locID.Value, true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());

            DisplayMessage("Record Updated", "green");



            chkTimeTableList.DataSource = null;
            chkTimeTableList.DataBind();
            txtTimeTable.Text = "";
            for (int i = 0; i < chkTimeTableList.Items.Count; i++)
            {

                chkTimeTableList.Items[i].Selected = false;

            }




            for (int i = 0; i < chkDayUnderPeriod.Items.Count; i++)
            {

                //  chkDayUnderPeriod.Items[i].Remove();
                chkDayUnderPeriod.Items[i].Selected = true;


            }




        }
        else
        {
            string shiftid = "";
            b = objShift.InsertShiftMaster(Session["CompId"].ToString(), txtShiftName.Text, txtShiftNameL.Text, Session["BrandId"].ToString(), txtCycleNo.Text, ddlCycleUnit.SelectedValue, txtApplyFrom.Text, "", "", "", "", Session["LocId"].ToString(), true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
            editid.Value = shiftid;




            DisplayMessage("Record Saved", "green");
            PnlViewShift.Visible = true;

            btnAddTime.Visible = true;

            btnClearAll.Visible = true;

        }
        if (b != 0)
        {

            FillGrid();
            FillGridBin();

            chkTimeTableList.Enabled = true;
            chkDayUnderPeriod.Enabled = true;

            PanelShiftAss.Visible = true;
            Div_Main.Visible = false;

            // TrShiftName.Visible = true;
            btnshowpopup_Click(null, null);

            lblShiftNameIs.Text = txtShiftName.Text;
            txtShiftId.Text = editid.Value;

            btnNext.Visible = false;
            btnOk.Visible = true;
            btnBack.Visible = false;
            btnCancelPanel.Visible = true;

            PnlViewShift.Visible = false;


        }

    }
    protected void btnshowpopup_Click(object sender, ImageClickEventArgs e)
    {
        bindchecklist();

        if (Session["IsView"] != null)
        {
            Session["IsView"] = null;
        }
    }
    private void FindAndCheckChkTimeTable(string timetableid)
    {
        for (int i = 0; i < chkTimeTableList.Items.Count; i++)
        {
            if (chkTimeTableList.Items[i].Value == timetableid)
            {
                chkTimeTableList.Items[i].Selected = false;
                break;
            }
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int flag = 0;
        foreach (GridViewRow gvr in gvShiftNew.Rows)
        {
            CheckBox chk = (CheckBox)gvr.FindControl("ChkDay");
            if (chk.Checked)
            {
                flag = 1;
            }
        }

        if (flag == 0)
        {
            DisplayMessage("Please First Select Date in List");
            return;
        }
        foreach (GridViewRow gvr in gvShiftNew.Rows)
        {
            CheckBox chk = (CheckBox)gvr.FindControl("ChkDay");
            HiddenField CycleType = (HiddenField)gvr.FindControl("hdnCycle_Type");
            HiddenField CycleDay = (HiddenField)gvr.FindControl("hdnCycle_Day");
            if (chk.Checked)
            {
                objShiftDesc.DeleteShiftDescriptionByCycleDayCycleDay(editid.Value, CycleType.Value, CycleDay.Value);

            }
        }
        ViewShift(editid.Value);
        DisplayMessage("Record Deleted");



    }
    protected void btnClearAll_OnClick(object sender, EventArgs e)
    {

        PnlViewShift.Visible = false;
        objShiftDesc.DeleteShiftDescriptionByShiftId(editid.Value);
        objShiftDesc.DeleteShift_TimeTableByShiftId(editid.Value);

        gvShiftView.DataSource = null;
        gvShiftView.DataBind();
        btnAddTime.Visible = false;
        btnClearAll.Visible = false;
        btnDelete.Visible = false;
    }

    protected void btnSelectAll_OnClick(object sender, EventArgs e)
    {

        for (int j = 0; j < chkDayUnderPeriod.Items.Count; j++)
        {
            chkDayUnderPeriod.Items[j].Selected = true;
        }

    }
    public void bindchecklist()
    {

        string[] weekdays = new string[8];
        weekdays[1] = "Monday";
        weekdays[2] = "Tuesday";
        weekdays[3] = "Wednesday";
        weekdays[4] = "Thursday";
        weekdays[5] = "Friday";
        weekdays[6] = "Saturday";
        weekdays[7] = "Sunday";


        //Update On 25-06-2015 For Week Off Parameter
        bool strWeekOffParam = true;
        DataTable dtWeekOffParam = objparam.GetApplicationParameterByCompanyId("AddWeekOffInShift", Session["CompId"].ToString());
        dtWeekOffParam = new DataView(dtWeekOffParam, "Param_Name='AddWeekOffInShift'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtWeekOffParam.Rows.Count > 0)
        {
            strWeekOffParam = Convert.ToBoolean(dtWeekOffParam.Rows[0]["Param_Value"].ToString());
        }
        else
        {
            strWeekOffParam = true;
        }

        string WeekOff = string.Empty;
        if (strWeekOffParam == true)
        {
            WeekOff = objparam.GetApplicationParameterValueByParamName("Week Off Days", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }

        int days = Convert.ToInt32(ddlCycleUnit.SelectedValue);
        int cycleno = Convert.ToInt32(txtCycleNo.Text);

        DateTime newappfromdt = Convert.ToDateTime(txtApplyFrom.Text);
        string appfromdt = newappfromdt.ToShortDateString();
        string appfromday = Convert.ToDateTime(newappfromdt).DayOfWeek.ToString().Substring(0, 3);

        DataTable dtfordays = new DataTable();
        dtfordays.Columns.Add("days");
        dtfordays.Columns.Add("dayno");
        int totaldays = days * cycleno;
        int colrept = 0;
        if (totaldays % 7 > 0)
        {
            colrept = 1;
        }
        //chkDayUnderPeriod.RepeatColumns = totaldays / 7 + colrept;

        if (ddlCycleUnit.SelectedValue == "1")
        {
            for (int i = 1; i <= totaldays; i++)
            {

                dtfordays.Rows.Add(dtfordays.NewRow());
                dtfordays.Rows[dtfordays.Rows.Count - 1]["days"] = "Day " + "-" + i.ToString();
                dtfordays.Rows[dtfordays.Rows.Count - 1]["dayno"] = "Day " + "-" + i.ToString();
                appfromdt = (Convert.ToDateTime(appfromdt).AddDays(1)).ToShortDateString().ToString();
                appfromday = Convert.ToDateTime(appfromdt).DayOfWeek.ToString().Substring(0, 3);
            }
        }
        else if (ddlCycleUnit.SelectedValue == "7")
        {
            appfromday = Convert.ToDateTime(appfromdt).DayOfWeek.ToString();
            int CycleType = 0;
            for (int j = 1; j < weekdays.Length; j++)
            {
                if (weekdays[j].Equals(appfromday))
                {

                    appfromday = Convert.ToDateTime(appfromdt).ToShortDateString();
                    for (int i = 0; i < totaldays; i++)
                    {

                        if (i % 7 == 0)
                        {
                            CycleType += 1;
                        }


                        if (WeekOff == "No")
                        {
                            dtfordays.Rows.Add(dtfordays.NewRow());
                            dtfordays.Rows[dtfordays.Rows.Count - 1]["days"] = "Week-" + CycleType.ToString() + "-" + weekdays[j].ToString();
                            dtfordays.Rows[dtfordays.Rows.Count - 1]["dayno"] = "Week-" + CycleType.ToString() + "-" + (j).ToString();
                        }
                        else
                        {
                            bool WF = false;
                            foreach (string str in WeekOff.Split(','))
                            {
                                if (weekdays[j].ToString() == str)
                                {
                                    WF = true;
                                }

                            }
                            if (WF == false)
                            {
                                dtfordays.Rows.Add(dtfordays.NewRow());
                                dtfordays.Rows[dtfordays.Rows.Count - 1]["days"] = "Week-" + CycleType.ToString() + "-" + weekdays[j].ToString();
                                dtfordays.Rows[dtfordays.Rows.Count - 1]["dayno"] = "Week-" + CycleType.ToString() + "-" + (j).ToString();
                                // break;
                            }
                        }
                        appfromdt = (Convert.ToDateTime(appfromdt).AddDays(1)).ToShortDateString().ToString();
                        j++;
                        if (j == 8)
                        {
                            j = 1;
                        }
                    }
                    break;
                }
            }
        }
        else
        {
            string month = Convert.ToDateTime(appfromdt.ToString()).Month.ToString();
            DateTime startdate = new DateTime(Convert.ToDateTime(appfromdt.ToString()).Year, Convert.ToDateTime(appfromdt.ToString()).Month, 1);

            startdate = Convert.ToDateTime(txtApplyFrom.Text);
            string stdate = Convert.ToDateTime(startdate.ToString()).ToShortDateString().ToString();

            string startday = Convert.ToDateTime(startdate.ToString()).DayOfWeek.ToString().Substring(0, 3);
            int CycleType = 1;
            int dayno = 1;
            for (int i = 1; i <= totaldays; i++)
            {
                if (i % 31 == 0)
                {
                    dayno = 31;
                }
                else
                {
                    dayno = i % 31;
                }

                dtfordays.Rows.Add(dtfordays.NewRow());
                if (month.Equals(Convert.ToDateTime(stdate).Month.ToString()))
                {
                    dtfordays.Rows[dtfordays.Rows.Count - 1]["days"] = "Month-" + CycleType.ToString() + "-" + dayno.ToString();
                    dtfordays.Rows[dtfordays.Rows.Count - 1]["dayno"] = "Month-" + CycleType.ToString() + "-" + dayno.ToString();
                    stdate = (Convert.ToDateTime(stdate).AddDays(1)).ToShortDateString().ToString();
                    startday = Convert.ToDateTime(stdate).DayOfWeek.ToString().Substring(0, 3);
                    if (dayno == 31)
                        month = Convert.ToDateTime(stdate).Month.ToString();
                }
                else
                {
                    dtfordays.Rows[dtfordays.Rows.Count - 1]["days"] = "Month-" + CycleType.ToString() + "-" + dayno.ToString();
                    dtfordays.Rows[dtfordays.Rows.Count - 1]["dayno"] = "Month-" + CycleType.ToString() + "-" + dayno.ToString();
                    stdate = (Convert.ToDateTime(stdate).AddDays(1)).ToShortDateString().ToString();
                    startday = Convert.ToDateTime(stdate).DayOfWeek.ToString().Substring(0, 3);
                    if (dayno == 31)
                        month = Convert.ToDateTime(stdate).Month.ToString();
                }

                if (i % 31 == 0)
                {
                    CycleType = CycleType + 1;
                }
            }
        }

        //Week Off Code New By Nitin On 27-08-2014
        DataTable dtWeekOff = new DataTable();
        for (int w = 0; w < dtfordays.Rows.Count; w++)
        {
            foreach (string str in WeekOff.Split(','))
            {
                string sp = dtfordays.Rows[w]["days"].ToString();
                string[] s = sp.Split('-');
                string spv = s[0].ToString() + "-" + s[1].ToString() + "-";
                string wof = spv + str;
                if (dtfordays.Rows[w]["days"].ToString() == wof)
                {
                    dtfordays.Rows[w].Delete();
                    dtfordays.AcceptChanges();
                }
            }
        }
        //...........................................


        //Common Function add By Lokesh on 21-05-2015
        objPageCmn.FillData((object)chkDayUnderPeriod, dtfordays, "days", "dayno");

        for (int j = 0; j < chkDayUnderPeriod.Items.Count; j++)
        {
            chkDayUnderPeriod.Items[j].Selected = true;
        }
    }
    protected void btnDelete_Command(object sender, CommandEventArgs e)
    {

    }
    protected void btnView_Command(object sender, CommandEventArgs e)
    {
        DataTable dt = objShift.GetShiftMasterById(Session["CompId"].ToString(), e.CommandArgument.ToString());
        if (dt.Rows.Count > 0)
        {
            txtShiftName.Text = dt.Rows[0]["Shift_Name"].ToString();
            txtShiftNameL.Text = dt.Rows[0]["Shift_Name_L"].ToString();

            txtApplyFrom.Text = Convert.ToDateTime(dt.Rows[0]["Apply_From"]).ToString(objSys.SetDateFormat());
            txtCycleNo.Text = dt.Rows[0]["Cycle_No"].ToString();
            ddlCycleUnit.SelectedValue = dt.Rows[0]["Cycle_Unit"].ToString();
            Lbl_Tab_New.Text = Resources.Attendance.Edit;

            btnAddTime.Visible = false;
            btnClearAll.Visible = false;
            btnDelete.Visible = false;
            btnSave.Visible = false;

            ViewShift(e.CommandArgument.ToString());
            Lbl_Tab_New.Text = Resources.Attendance.View;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
    }
    protected void btnEdit_Command(object sender, CommandEventArgs e)
    {
        DataTable dtShift = new DataTable();
        dtShift = objSch.GetSheduleDescription();
        dtShift = new DataView(dtShift, "Shift_Id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dtShift.Rows.Count > 0)
        {
            //DisplayMessage("You cannot edit this shift");
            //return;
        }

        editid.Value = e.CommandArgument.ToString();
        DataTable dt = objShift.GetShiftMasterById(Session["CompId"].ToString(), editid.Value);
        if (dt.Rows.Count > 0)
        {
            txtShiftName.Text = dt.Rows[0]["Shift_Name"].ToString();
            txtShiftNameL.Text = dt.Rows[0]["Shift_Name_L"].ToString();

            txtApplyFrom.Text = Convert.ToDateTime(dt.Rows[0]["Apply_From"]).ToString(objSys.SetDateFormat());
            txtCycleNo.Text = dt.Rows[0]["Cycle_No"].ToString();
            ddlCycleUnit.SelectedValue = dt.Rows[0]["Cycle_Unit"].ToString();
            ddlLocNew.SelectedValue = dt.Rows[0]["Field5"].ToString();
            hdn_locID.Value = dt.Rows[0]["Field5"].ToString();
           
            Lbl_Tab_New.Text = Resources.Attendance.Edit;

            btnAddTime.Visible = true;
            btnClearAll.Visible = true;
            btnDelete.Visible = true;

            ViewShift(editid.Value);

            Lbl_Tab_New.Text = Resources.Attendance.Edit;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_Edit_Active()", true);
        }
        //AllPageCode();
    }
    protected void ChkAllDay_CheckedChanged(object sender, EventArgs e)
    {
        if (((CheckBox)(sender)).Checked)
        {
            foreach (GridViewRow gvr in gvShiftView.Rows)
            {
                CheckBox chk = (CheckBox)gvr.FindControl("ChkDay");
                chk.Checked = true;
            }
        }
        else
        {
            foreach (GridViewRow gvr in gvShiftView.Rows)
            {
                CheckBox chk = (CheckBox)gvr.FindControl("ChkDay");
                chk.Checked = false;
            }
        }
    }
    protected void ChkAllDay_CheckedChanged1(object sender, EventArgs e)
    {
        if (((CheckBox)(sender)).Checked)
        {
            foreach (GridViewRow gvr in gvShiftNew.Rows)
            {
                CheckBox chk = (CheckBox)gvr.FindControl("ChkDay");
                chk.Checked = true;
            }
        }
        else
        {
            foreach (GridViewRow gvr in gvShiftNew.Rows)
            {
                CheckBox chk = (CheckBox)gvr.FindControl("ChkDay");
                chk.Checked = false;
            }
        }

    }
    public string WriteDays(object o)
    {
        DateTime dt = Convert.ToDateTime(o.ToString());

        return dt.ToString("dd-MMM-yyyy") + "   " + dt.DayOfWeek;
    }
    public void ViewShift(string shiftId)
    {
        string WOff = string.Empty;
        PnlViewShift.Visible = true;
        DataTable dttimetable1 = objShiftDesc.GetShiftDescriptionByShiftId(shiftId);
        SubDtScatch();
        DataTable dtScatch = (DataTable)Session["dtScatch"];

        DataTable dt = objTimeTable.GetTimeTableMaster(Session["CompId"].ToString());

        DataTable dtTime1 = new DataTable();
        dtTime1.Columns.Add("EDutyTime");
        dtTime1.Columns.Add("TimeTable_Id");
        dtTime1.Columns.Add("Cycle_Type");
        dtTime1.Columns.Add("Cycle_Day");

        string WeekOff = string.Empty;
        WeekOff = objparam.GetApplicationParameterValueByParamName("Week Off Days", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());

        for (int i = 0; i < dtScatch.Rows.Count; i++)
        {
            DataTable dtTemp = new DataView(dttimetable1, "Cycle_Day = '" + dtScatch.Rows[i]["Cycle_Day"].ToString() + "' and  Cycle_Type='" + dtScatch.Rows[i]["Cycle_Type"].ToString() + "' and convert(TimeTable_Id,System.String) <> ''", "", DataViewRowState.CurrentRows).ToTable();
            DataRow dr = dtTime1.NewRow();
            string sTime = string.Empty;
            if (dtTemp.Rows.Count > 0)
            {

                for (int k = 0; k < dtTemp.Rows.Count; k++)
                {

                    DataTable dtTempTime = new DataView(dt, "TimeTable_Id = '" + dtTemp.Rows[k]["TimeTable_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtTempTime.Rows.Count > 0)
                    {
                        sTime = sTime + dtTempTime.Rows[0]["TimeTable_Name"].ToString() + "(" + dtTempTime.Rows[0]["OnDuty_Time"].ToString() + "-" + dtTempTime.Rows[0]["OffDuty_Time"].ToString() + ")" + ",";
                    }

                }
                dr["TimeTable_Id"] = sTime;
                dr["EDutyTime"] = Convert.ToDateTime(dtScatch.Rows[i][0].ToString()).ToString("dd-MMM-yyyy");
                dr["Cycle_Type"] = dtScatch.Rows[i]["Cycle_Type"].ToString();
                dr["Cycle_Day"] = dtScatch.Rows[i]["Cycle_Day"].ToString();
                dtTime1.Rows.Add(dr);
            }
            else
            {
                dr["TimeTable_Id"] = "IsOff";
                dr["EDutyTime"] = Convert.ToDateTime(dtScatch.Rows[i][0].ToString()).ToString("dd-MMM-yyyy");
                dr["Cycle_Type"] = dtScatch.Rows[i]["Cycle_Type"].ToString();
                dr["Cycle_Day"] = dtScatch.Rows[i]["Cycle_Day"].ToString();
                dtTime1.Rows.Add(dr);
            }
        }

        int max = 0;
        for (int i = 0; i < dttimetable1.Rows.Count; i++)
        {
            DataTable dt2 = objShiftDesc.GetShift_TimeTableByRefId(dttimetable1.Rows[i]["Trans_Id"].ToString());

            if (dt2.Rows.Count > 0)
            {
                if (dt2.Rows.Count > max)
                {

                    max = dt2.Rows.Count;

                }
            }


        }
        string[] weekdays = new string[8];
        weekdays[1] = "Monday";
        weekdays[2] = "Tuesday";
        weekdays[3] = "Wednesday";
        weekdays[4] = "Thursday";
        weekdays[5] = "Friday";
        weekdays[6] = "Saturday";
        weekdays[7] = "Sunday";

        DataTable dtdata = new DataTable();
        dtdata.Columns.Add("Day");

        dtdata.Columns.Add("Cycle_Type");
        dtdata.Columns.Add("Cycle_Day");
        if (max > 0)
        {
            int max2 = max;
            max2 = 2 * max2;
            for (int i = 0; i < max2; i++)
            {
                dtdata.Columns.Add("t" + (i + 1));

            }

        }

        string sTime1 = "";
        for (int i = 0; i < dtScatch.Rows.Count; i++)
        {
            DataTable dtTemp = new DataView(dttimetable1, "Cycle_Day = '" + dtScatch.Rows[i]["Cycle_Day"].ToString() + "' and  Cycle_Type='" + dtScatch.Rows[i]["Cycle_Type"].ToString() + "' and convert(TimeTable_Id,System.String) <> ''", "", DataViewRowState.CurrentRows).ToTable();
            DataRow dr = dtdata.NewRow();

            if (WeekOff != "No")
            {
                if (ddlCycleUnit.SelectedValue == "7")
                {
                    foreach (string str in WeekOff.Split(','))
                    {
                        if (weekdays[int.Parse(dtScatch.Rows[i]["Cycle_Day"].ToString())].ToString() == str && ddlCycleUnit.SelectedValue == "7")
                        {
                            WOff = "1";
                            continue;
                        }
                    }
                }
                else if (ddlCycleUnit.SelectedValue == "31")
                {

                }

            }

            if (dtTemp.Rows.Count > 0)
            {
                if (ddlCycleUnit.SelectedValue == "7")
                {
                    dr["Day"] = dtScatch.Rows[i]["Cycle_Type"].ToString() + "-" + weekdays[int.Parse(dtScatch.Rows[i]["Cycle_Day"].ToString())];
                }
                else
                {
                    dr["Day"] = dtScatch.Rows[i]["Cycle_Type"].ToString() + "-" + dtScatch.Rows[i]["Cycle_Day"].ToString();


                }

                dr["Cycle_Type"] = dtScatch.Rows[i]["Cycle_Type"].ToString();
                dr["Cycle_Day"] = dtScatch.Rows[i]["Cycle_Day"].ToString();

                for (int k = 0; k < dtTemp.Rows.Count; k++)
                {


                    if (k == 0)
                    {
                        DataTable dtTempTime = new DataView(dt, "TimeTable_Id = '" + dtTemp.Rows[k]["TimeTable_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtTempTime.Rows.Count > 0)
                        {
                            sTime1 = dtTempTime.Rows[0]["TimeTable_Name"].ToString();
                        }
                        dr["t" + (k + 1).ToString()] = sTime1;
                    }
                    else
                    {
                        if (k % 2 != 0)
                        {

                            DataTable dtTempTime = new DataView(dt, "TimeTable_Id = '" + dtTemp.Rows[k]["TimeTable_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtTempTime.Rows.Count > 0)
                            {
                                sTime1 = dtTempTime.Rows[0]["TimeTable_Name"].ToString();
                            }
                            dr["t" + (k + 2).ToString()] = sTime1;
                        }
                        else
                        {
                            DataTable dtTempTime = new DataView(dt, "TimeTable_Id = '" + dtTemp.Rows[k]["TimeTable_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtTempTime.Rows.Count > 0)
                            {
                                sTime1 = dtTempTime.Rows[0]["TimeTable_Name"].ToString();
                            }
                            dr["t" + (k + 3).ToString()] = sTime1;
                        }
                    }
                }
                dtdata.Rows.Add(dr);
                dr = dtdata.NewRow();
                for (int k = 0; k < dtTemp.Rows.Count; k++)
                {

                    if (k == 0)
                    {
                        DataTable dtTempTime = new DataView(dt, "TimeTable_Id = '" + dtTemp.Rows[k]["TimeTable_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                        if (dtTempTime.Rows.Count > 0)
                        {
                            dr["t" + (k + 1).ToString()] = dtTempTime.Rows[0]["OnDuty_Time"].ToString();

                            dr["t" + (k + 2).ToString()] = dtTempTime.Rows[0]["OffDuty_Time"].ToString();
                        }
                    }
                    else
                    {



                        if (k % 2 != 0)
                        {
                            DataTable dtTempTime = new DataView(dt, "TimeTable_Id = '" + dtTemp.Rows[k]["TimeTable_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtTempTime.Rows.Count > 0)
                            {

                                dr["t" + (k + 2).ToString()] = dtTempTime.Rows[0]["OnDuty_Time"].ToString();

                                dr["t" + (k + 3).ToString()] = dtTempTime.Rows[0]["OffDuty_Time"].ToString();
                            }

                        }
                        else
                        {
                            DataTable dtTempTime = new DataView(dt, "TimeTable_Id = '" + dtTemp.Rows[k]["TimeTable_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                            if (dtTempTime.Rows.Count > 0)
                            {

                                dr["t" + (k + 3).ToString()] = dtTempTime.Rows[0]["OnDuty_Time"].ToString();

                                dr["t" + (k + 4).ToString()] = dtTempTime.Rows[0]["OffDuty_Time"].ToString();
                            }
                        }

                    }


                }
                if (WOff != "1")
                {
                    dtdata.Rows.Add(dr);
                }


            }
            else
            {

                if (ddlCycleUnit.SelectedValue == "7")
                {
                    dr["Day"] = dtScatch.Rows[i]["Cycle_Type"].ToString() + "-" + weekdays[int.Parse(dtScatch.Rows[i]["Cycle_Day"].ToString())];
                }
                else
                {
                    dr["Day"] = dtScatch.Rows[i]["Cycle_Type"].ToString() + "-" + dtScatch.Rows[i]["Cycle_Day"].ToString();


                }
                try
                {
                    dr["t1"] = "Off";
                }
                catch
                {

                }
                if (WOff != "1")
                {
                    dtdata.Rows.Add(dr);
                }
                WOff = "";
            }

        }





        Session["ViewShiftDt"] = dtTime1;
        //Common Function add By Lokesh on 21-05-2015
        objPageCmn.FillData((object)gvShiftView, dtTime1, "", "");
        gvShiftView.Visible = false;
        //Common Function add By Lokesh on 21-05-2015
        objPageCmn.FillData((object)gvShiftNew, dtdata, "", "");
        try
        {
            gvShiftNew.HeaderRow.Cells[2].Visible = false;
            gvShiftNew.HeaderRow.Cells[3].Visible = false;
            for (int i = 4; i < gvShiftNew.HeaderRow.Cells.Count; i++)
            {
                gvShiftNew.HeaderRow.Cells[i].Text = "";
            }
            for (int i = 4; i < gvShiftNew.HeaderRow.Cells.Count; i++)
            {
                gvShiftNew.HeaderRow.Cells[i].BorderStyle = BorderStyle.None;
            }
        }
        catch
        {


        }



        try
        {
            int count = gvShiftNew.HeaderRow.Cells.Count - 2;

            count = count / 2;
            count = count + 2;
            gvShiftNew.HeaderRow.Cells[count].Text = "Time Table";

        }
        catch
        {
        }

    }


    protected void gvShiftNew_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[3].Visible = false;

            CheckBox chb = (CheckBox)e.Row.FindControl("chkDay");

            HiddenField hdCycleDay = (HiddenField)e.Row.FindControl("hdnCycle_Type");

            if (hdCycleDay.Value == "")
            {
                chb.Visible = false;

            }
        }
    }
    protected void IbtnDelete_Command(object sender, CommandEventArgs e)
    {

        DataTable dtShift = new DataTable();
        dtShift = objSch.GetSheduleDescription();
        dtShift = new DataView(dtShift, "Shift_Id='" + e.CommandArgument.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dtShift.Rows.Count > 0)
        {
            DisplayMessage("You cannot delete this shift");
            return;
        }
        int b = 0;
        b = objShift.DeleteShiftMaster(Session["CompId"].ToString(), e.CommandArgument.ToString(), false.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
        if (b != 0)
        {
            DisplayMessage("Record Deleted");
            FillGridBin();
            FillGrid();
            Reset();
        }
        else
        {
            DisplayMessage("Record Not Deleted");
        }
    }
    protected void gvShift_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvShift.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["dtFilter_Shift_Mng"];
        //Common Function add By Lokesh on 21-05-2015
        objPageCmn.FillData((object)gvShift, dt, "", "");
        //AllPageCode();
    }
    protected void gvShift_OnSorting(object sender, GridViewSortEventArgs e)
    {
        HDFSort.Value = HDFSort.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtFilter_Shift_Mng"];
        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSort.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["dtFilter_Shift_Mng"] = dt;
        //Common Function add By Lokesh on 21-05-2015
        objPageCmn.FillData((object)gvShift, dt, "", "");
        //AllPageCode();
        gvShift.HeaderRow.Focus();
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListShiftName(string prefixText, int count, string contextKey)
    {
        Att_ShiftManagement objAtt_Shift = new Att_ShiftManagement(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dt = new DataView(objAtt_Shift.GetShiftMaster(HttpContext.Current.Session["CompId"].ToString()), "Shift_Name like '" + prefixText.ToString() + "%'", "", DataViewRowState.CurrentRows).ToTable();
        // Modified By Nitin On 10/11/2014 for filter on brand Level
        dt = new DataView(dt, "Brand_Id='" + HttpContext.Current.Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        string[] txt = new string[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            txt[i] = dt.Rows[i]["Shift_Name"].ToString();
        }
        return txt;
    }


    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Reset();
        dlView.DataSource = null;
        dlView.DataBind();
        PnlViewShift.Visible = false;
        txtShiftName.Focus();
        //AllPageCode();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        FillGrid();
        FillGridBin();
        if (editid.Value != "")
        {
            btnCancelPanel_Click1(null, null);
        }
        dlView.DataSource = null;
        dlView.DataBind();
        PnlViewShift.Visible = false;
        Reset();
        Session["DtTime"] = null;
        ddlFieldName.Focus();
        PnlViewShift.Visible = false;
        Lbl_Tab_New.Text = Resources.Attendance.New;
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active()", true);
    }

    public void FillGrid()
    {

        DataTable dt = objShift.GetShiftMaster(Session["CompId"].ToString(), ddlLocation.SelectedValue);
        // Modified By Nitin On 10/11/2014 to get on Brand Level
        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        //Common Function add By Lokesh on 21-05-2015
        objPageCmn.FillData((object)gvShift, dt, "", "");
        //AllPageCode();
        Session["dtFilter_Shift_Mng"] = dt;
        Session["Shift"] = dt;
        lblTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
    }
    public void FillGridBin()
    {
        DataTable dt = new DataTable();
        dt = objShift.GetShiftMasterInactive(Session["CompId"].ToString());
        // Modified By Nitin On 10/11/2014 to get on Brand Level
        dt = new DataView(dt, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

        //Common Function add By Lokesh on 21-05-2015
        objPageCmn.FillData((object)gvShiftBin, dt, "", "");

        Session["dtbinFilter"] = dt;
        Session["dtbinShift"] = dt;
        lblbinTotalRecords.Text = Resources.Attendance.Total_Records + ": " + dt.Rows.Count.ToString() + "";
        if (dt.Rows.Count == 0)
        {
            //imgBtnRestore.Visible = false;
            ////ImgbtnSelectAll.Visible = false;
        }
        else
        {

            //AllPageCode();
        }

    }


    protected void chkgvSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvShiftBin.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvShiftBin.Rows.Count; i++)
        {
            ((CheckBox)gvShiftBin.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((Label)(gvShiftBin.Rows[i].FindControl("lblShiftId"))).Text.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((Label)(gvShiftBin.Rows[i].FindControl("lblShiftId"))).Text.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((Label)(gvShiftBin.Rows[i].FindControl("lblShiftId"))).Text.Trim().ToString())
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
        Label lb = (Label)gvShiftBin.Rows[index].FindControl("lblShiftId");
        if (((CheckBox)gvShiftBin.Rows[index].FindControl("chkgvSelect")).Checked)
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
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Shift_Id"]))
                {
                    lblSelectedRecord.Text += dr["Shift_Id"] + ",";
                }
            }
            for (int i = 0; i < gvShiftBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                Label lblconid = (Label)gvShiftBin.Rows[i].FindControl("lblShiftId");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvShiftBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            DataTable dtUnit1 = (DataTable)Session["dtBinFilter"];
            //Common Function add By Lokesh on 21-05-2015
            objPageCmn.FillData((object)gvShiftBin, dtUnit1, "", "");
            ViewState["Select"] = null;
        }
    }
    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I2.Attributes.Add("Class", "fa fa-minus");
        Div2.Attributes.Add("Class", "box box-primary");
        int b = 0;
        lblSelectedRecord.Text = "";
        for (int i = 0; i < gvShiftBin.Rows.Count; i++)
        {
             bool bSelect =   ((CheckBox)gvShiftBin.Rows[i].FindControl("chkgvSelect")).Checked ;
            if (bSelect)
            {
              
                    lblSelectedRecord.Text += ((Label)(gvShiftBin.Rows[i].FindControl("lblShiftId"))).Text.Trim().ToString() + ",";
               
            }
        }
            if (lblSelectedRecord.Text != "")
        {
            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    b = objShift.DeleteShiftMaster(Session["CompId"].ToString(), lblSelectedRecord.Text.Split(',')[j].Trim(), true.ToString(), Session["UserId"].ToString().ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString());
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
            foreach (GridViewRow Gvr in gvShiftBin.Rows)
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
        txtShiftName.Text = "";
        txtShiftNameL.Text = "";
        txtCycleNo.Text = "";
        ddlCycleUnit.SelectedIndex = 0;
        txtApplyFrom.Text = "";
        Lbl_Tab_New.Text = Resources.Attendance.New;
        lblSelectedRecord.Text = "";
        ViewState["Select"] = null;
        editid.Value = "";
        txtValue.Text = "";
        hdn_locID.Value = "";
        ddlOption.SelectedIndex = 2;
        ddlFieldName.SelectedIndex = 0;
        btnDelete.Visible = false;
        btnAddTime.Visible = false;
        btnClearAll.Visible = false;
    }

    // Modified By Nitin On 13/11/2014..................
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListTimeTableName(string prefixText, int count, string contextKey)
    {
        DataTable dt = Common.GetTimeTable(prefixText, HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["DBConnection"].ToString());

        string[] str = new string[dt.Rows.Count];
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            str[i] = "(" + dt.Rows[i][4].ToString() + "/" + dt.Rows[i][2].ToString() + "";
        }
        return str;
    }

    protected void txtTimeTable_textChanged(object sender, EventArgs e)
    {
        string TimeTableid = string.Empty;
        DataTable dtTimeTable = new DataTable();
        dtTimeTable.Clear();
        if (txtTimeTable.Text != "")
        {
            TimeTableid = txtTimeTable.Text.Split('/')[txtTimeTable.Text.Split('/').Length - 1];

            dtTimeTable = objTimeTable.GetTimeTableMaster(Session["CompId"].ToString());
            try
            {
                dtTimeTable = new DataView(dtTimeTable, "TimeTable_Id='" + TimeTableid + "'", "", DataViewRowState.CurrentRows).ToTable();
            }
            catch (Exception Ex)
            {
                dtTimeTable.Clear();
            }
            if (dtTimeTable.Rows.Count > 0)
            {
                TimeTableid = dtTimeTable.Rows[0]["TimeTable_Id"].ToString();
                ViewState["TimeTable_Id"] = TimeTableid;
            }
            else
            {
                DisplayMessage("Time Table  Not Exists");
                txtShiftName.Text = "";
                txtShiftName.Focus();
                return;
            }
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (txtTimeTable.Text == "")
        {
            DisplayMessage("Select Time Table");
            txtTimeTable.Focus();
            return;
        }
        DataTable dtTime1 = new DataTable();
        dtTime1.Columns.Add("EDutyTime");
        dtTime1.Columns.Add("TimeTableId");
        DataRow dr = dtTime1.NewRow();
        try
        {
            dr["EDutyTime"] = txtTimeTable.Text.Split('/')[txtTimeTable.Text.Split('/').Length - 2];
            dr["TimeTableId"] = txtTimeTable.Text.Split('/')[txtTimeTable.Text.Split('/').Length - 1];
        }
        catch (Exception Ex)
        {
        }
        dtTime1.Rows.Add(dr);
        if (Session["DtTime"] != null)
        {
            DataTable FilterDt = new DataTable();
            DtTime = (DataTable)Session["DtTime"];
            FilterDt = new DataView(DtTime, "TimeTableId='" + dr["TimeTableId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (FilterDt.Rows.Count > 0)
            {
                DisplayMessage("Time Table Already Added");
                txtTimeTable.Text = "";
                txtTimeTable.Focus();
                return;
            }

        }
        DtTime.Merge(dtTime1);
        txtTimeTable.Text = "";
        //Common Function add By Lokesh on 21-05-2015
        objPageCmn.FillData((object)chkTimeTableList, DtTime, "EDutyTime", "TimeTableId");
        Session["DtTime"] = DtTime;
    }


    //-------------------- Add by ghanshyam Suthar on 15-01-2018------------------------------

    protected void Btn_Upload_Click(object sender, EventArgs e)
    {

    }

    public void ExportTableData(DataTable Dt_Data, string FName)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(Dt_Data, FName);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + FName + ".xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }

    protected void FUExcel_FileUploadComplete(object sender, EventArgs e)
    {
        if (FU_Shift_Upload.HasFile)
        {
            string ext = FU_Shift_Upload.FileName.Substring(FU_Shift_Upload.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                FU_Shift_Upload.SaveAs(Server.MapPath("~/Temp/" + FU_Shift_Upload.FileName));
            }
        }
    }

    protected void btnGetSheet_Click(object sender, EventArgs e)
    {
        int fileType = 0;

        if (FU_Shift_Upload.HasFile)
        {
            string Path = string.Empty;
            string ext = FU_Shift_Upload.FileName.Substring(FU_Shift_Upload.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                FU_Shift_Upload.SaveAs(Server.MapPath("~/Temp/" + FU_Shift_Upload.FileName));
                Path = Server.MapPath("~/Temp/" + FU_Shift_Upload.FileName);

                if (ext == ".xls")
                {
                    fileType = 0;
                }
                else if (ext == ".xlsx")
                {
                    fileType = 1;
                }
                else if (ext == ".mdb")
                {
                    fileType = 2;
                }
                else if (ext == ".accdb")
                {
                    fileType = 3;
                }

                Import(Path, fileType);
            }
        }
    }

    public void Import(String path, int fileType)
    {
        string strcon = string.Empty;
        if (fileType == 1)
        {
            Session["filetype"] = "excel";
            strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 12.0;HDR=YES;iMEX=1\"";
        }
        else if (fileType == 0)
        {
            Session["filetype"] = "excel";
            strcon = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 8.0;HDR=YES;iMEX=1\"";
        }
        else
        {
            Session["filetype"] = "access";
            strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Persist Security Info=False";
        }


        Session["cnn"] = strcon;

        OleDbConnection conn = new OleDbConnection(strcon);
        conn.Open();

        DataTable tables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
        DDl_Excel_Sheet.DataSource = tables;

        DDl_Excel_Sheet.DataTextField = "TABLE_NAME";
        DDl_Excel_Sheet.DataValueField = "TABLE_NAME";
        DDl_Excel_Sheet.DataBind();
        conn.Close();
    }

    protected void btnConnect_Click(object sender, EventArgs e)
    {
        if (DDl_Excel_Sheet == null)
        {
            DisplayMessage("Sheet not found");
            return;
        }
        else if (DDl_Excel_Sheet.Items.Count == 0)
        {
            DisplayMessage("Sheet not found");
            return;
        }
        DataTable Dt_Sheet = new DataTable();
        if (FU_Shift_Upload.HasFile)
        {
            string Path = string.Empty;
            string ext = FU_Shift_Upload.FileName.Substring(FU_Shift_Upload.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                FU_Shift_Upload.SaveAs(Server.MapPath("~/Temp/" + FU_Shift_Upload.FileName));
                Path = Server.MapPath("~/Temp/" + FU_Shift_Upload.FileName);
                Dt_Sheet = ConvetExcelToDataTable(Path, DDl_Excel_Sheet.SelectedValue.Trim());

                if (Dt_Sheet.Rows.Count == 0)
                {
                    DisplayMessage("Record not found");
                    return;
                }
                Dt_Sheet.Columns.Add("Remark");
                Dt_Sheet.Columns.Add("Is_Valid");
                Dt_Sheet.AcceptChanges();

                //DataTable Dt_Duplicate = Dt_Sheet.Clone();
                // Dt_Duplicate.Clear();

                DataTable DT_TimeTable = objTimeTable.GetTimeTableMaster(Session["CompId"].ToString());
                DataTable Dt_Temp_Invalid = new DataTable();
                Dt_Temp_Invalid.Clear();
                for (int Sheet_Row = 0; Sheet_Row < Dt_Sheet.Rows.Count; Sheet_Row++)
                {
                    if (Dt_Sheet.Rows[Sheet_Row]["Remark"].ToString() == "")
                    {
                        Dt_Sheet.Rows[Sheet_Row]["Remark"] = "";
                        Dt_Sheet.Rows[Sheet_Row]["Is_Valid"] = "Valid";
                    }
                    for (int D_Sheet_Row = Dt_Sheet.Rows.Count - 1; D_Sheet_Row > Sheet_Row; D_Sheet_Row--)
                    {
                        if (Dt_Sheet.Rows[Sheet_Row]["Shift_Name"].ToString() == Dt_Sheet.Rows[D_Sheet_Row]["Shift_Name"].ToString())
                        {
                            Dt_Sheet.Rows[D_Sheet_Row]["Remark"] = "Shift Name Already Exists";
                            Dt_Sheet.Rows[D_Sheet_Row]["Is_Valid"] = "InValid";
                            break;
                        }
                    }

                    for (int Sheet_Col = 2; Sheet_Col < Dt_Sheet.Columns.Count - 2; Sheet_Col++)
                    {
                        if (Dt_Sheet.Rows[Sheet_Row][Sheet_Col].ToString().Trim() != "")
                        {
                            for (int j = 0; j < Dt_Sheet.Rows.Count; j++)
                            {
                                if (Dt_Sheet.Rows[Sheet_Row][Sheet_Col].ToString().Count(x => (x == '|')) > 0)
                                {
                                    for (int i = 0; i < Dt_Sheet.Rows[Sheet_Row][Sheet_Col].ToString().Count(x => (x == '|')); i++)
                                    {

                                        try
                                        {
                                            Dt_Temp_Invalid = new DataView(DT_TimeTable, "TimeTable_Name='" + Dt_Sheet.Rows[Sheet_Row][Sheet_Col].ToString().Split('|')[i].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                        }
                                        catch (Exception Ex)
                                        {
                                            Dt_Temp_Invalid.Clear();
                                        }
                                        if (Dt_Temp_Invalid.Rows.Count == 0)
                                        {
                                            Dt_Sheet.Rows[Sheet_Row]["Remark"] = "Invalid Time Table";
                                            Dt_Sheet.Rows[Sheet_Row]["Is_Valid"] = "InValid";
                                            break;
                                        }
                                        bool isoverlap = false;
                                        for (int k = Dt_Sheet.Rows[Sheet_Row][Sheet_Col].ToString().Count(x => (x == '|')); k > i; k--)
                                        {
                                            try
                                            {

                                                DataTable dtin = new DataView(DT_TimeTable, "TimeTable_Name='" + Dt_Sheet.Rows[Sheet_Row][Sheet_Col].ToString().Split('|')[i].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();

                                                DateTime dtintime = Convert.ToDateTime(dtin.Rows[0]["OnDuty_Time"]);
                                                DateTime dtouttime = Convert.ToDateTime(dtin.Rows[0]["OffDuty_Time"]);
                                                DateTime OnDutyTime = Convert.ToDateTime(dtin.Rows[0]["OnDuty_Time"]);
                                                DateTime OffDutyTime = Convert.ToDateTime(dtin.Rows[0]["OffDuty_Time"]);
                                                if (dtintime > dtouttime)
                                                {
                                                    dtouttime = dtouttime.AddHours(24);
                                                }

                                                if (OnDutyTime > OffDutyTime)
                                                {
                                                    OffDutyTime = OffDutyTime.AddHours(24);
                                                }


                                                DataTable dtin1 = new DataView(DT_TimeTable, "TimeTable_Name='" + Dt_Sheet.Rows[Sheet_Row][Sheet_Col].ToString().Split('|')[k].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                                DateTime dtintime1 = Convert.ToDateTime(dtin1.Rows[0]["OnDuty_Time"]);
                                                DateTime dtouttime1 = Convert.ToDateTime(dtin1.Rows[0]["OffDuty_Time"]);

                                                DateTime OnDutyTime1 = Convert.ToDateTime(dtin1.Rows[0]["OnDuty_Time"]);
                                                DateTime OffDutyTime1 = Convert.ToDateTime(dtin1.Rows[0]["OffDuty_Time"]);


                                                if (dtintime1 > dtouttime1)
                                                {
                                                    dtouttime1 = dtouttime1.AddHours(24);
                                                }

                                                if (OnDutyTime1 > OffDutyTime1)
                                                {
                                                    OffDutyTime1 = OffDutyTime1.AddHours(24);
                                                }

                                                if (dtintime >= dtintime1 && dtintime <= dtouttime1)
                                                {
                                                    isoverlap = true;
                                                    break;
                                                }
                                                if (dtouttime >= dtintime1 && dtouttime <= dtouttime1)
                                                {
                                                    isoverlap = true;
                                                    break;
                                                }

                                                if (dtintime1 >= dtintime && dtintime1 <= dtouttime)
                                                {
                                                    isoverlap = true;
                                                    break;
                                                }

                                                if (dtouttime1 >= dtintime && dtouttime1 <= dtouttime)
                                                {
                                                    isoverlap = true;
                                                    break;
                                                }
                                            }
                                            catch
                                            {

                                            }
                                        }
                                        if (isoverlap)
                                        {
                                            Dt_Sheet.Rows[i]["Remark"] = "Time Table Overlaped";
                                            Dt_Sheet.Rows[i]["Is_Valid"] = "InValid";
                                        }
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        Dt_Temp_Invalid = new DataView(DT_TimeTable, "TimeTable_Name='" + Dt_Sheet.Rows[Sheet_Row][Sheet_Col].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                    }
                                    catch (Exception Ex)
                                    {
                                        Dt_Temp_Invalid.Clear();
                                    }
                                    if (Dt_Temp_Invalid.Rows.Count == 0)
                                    {
                                        Dt_Sheet.Rows[Sheet_Row]["Remark"] = "Invalid Time Table";
                                        Dt_Sheet.Rows[Sheet_Row]["Is_Valid"] = "InValid";
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }

                Div_Upload_Grid.Visible = true;
                GV_Sheet_Upload.DataSource = Dt_Sheet;
                GV_Sheet_Upload.DataBind();
                lbltotaluploadRecord.Text = "Total Record : " + (Dt_Sheet.Rows.Count).ToString();
                Session["Dt_Sheet_Upload"] = Dt_Sheet;
            }
        }
        else
        {
            DisplayMessage("File Not Found");
            return;
        }
    }

    public DataTable ConvetExcelToDataTable(string path, string strtableName)
    {
        DataTable dt = new DataTable();
        try
        {
            OleDbConnection cnn = new OleDbConnection(Session["cnn"].ToString());
            OleDbDataAdapter adp = new OleDbDataAdapter("", "");
            adp.SelectCommand.CommandText = "Select *  From [" + strtableName.ToString() + "]";
            adp.SelectCommand.Connection = cnn;
            try
            {
                adp.Fill(dt);
            }
            catch (Exception)
            {
            }
        }
        catch
        {
            DisplayMessage("Excel file should in correct format");
        }
        return dt;
    }

    protected void Btn_Upload_Sheet_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            Shift_Name_Save(ref trns);
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();

        }
        catch (Exception ex)
        {
            DisplayMessage(Common.ConvertErrorMessage(ex.Message.ToString(), ex));

            trns.Rollback();
            if (con.State == System.Data.ConnectionState.Open)
            {

                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            return;
        }
    }

    public void Shift_Name_Save(ref SqlTransaction trns)
    {
        int b = 0;
        // Convert.ToDateTime(txtApplyFrom.Text);
        int Rows_affected = 0;
        DataTable Shift_Name = Session["Dt_Sheet_Upload"] as DataTable;
        Shift_Name = new DataView(Shift_Name, "Is_Valid='Valid'  and Remark=''", "", DataViewRowState.CurrentRows).ToTable();
        DataTable Dt_Shift_Database = objShift.GetShiftMaster(Session["CompId"].ToString());
        string Cycle_No = ((Shift_Name.Columns.Count - 4) / 7).ToString();
        for (int i = 0; i < Shift_Name.Rows.Count; i++)
        {
            DataTable dt1 = new DataTable();
            dt1 = new DataView(Dt_Shift_Database, "Shift_Name='" + Shift_Name.Rows[i]["Shift_Name"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dt1.Rows.Count > 0)
            {
                string ShiftTypeName = string.Empty;
                try
                {
                    ShiftTypeName = new DataView(dt1, "Shift_Id='" + dt1.Rows[0]["Shift_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Shift_Name"].ToString();
                }
                catch
                {
                    ShiftTypeName = "";
                }
                dt1 = new DataView(Dt_Shift_Database, "Shift_Name='" + Shift_Name.Rows[i]["Shift_Name"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dt1.Rows.Count > 0)
                {
                    b = objShift.UpdateShiftMaster(dt1.Rows[0]["Shift_Id"].ToString(), Session["CompId"].ToString(), Shift_Name.Rows[i]["Shift_Name"].ToString().Trim(), Shift_Name.Rows[i]["Shift_Name"].ToString().Trim(), Session["BrandId"].ToString(), Cycle_No, "7", Shift_Name.Rows[i]["Apply_From_Date"].ToString().Trim(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                }
                if (b != 0)
                {
                    objShiftDesc.DeleteShiftDescriptionByShiftId(dt1.Rows[0]["Shift_Id"].ToString());
                    objShiftDesc.DeleteShift_TimeTableByShiftId(dt1.Rows[0]["Shift_Id"].ToString());
                    Add_Time(b.ToString(), ref trns, Shift_Name.Rows[i]["Shift_Name"].ToString().Trim(), Cycle_No, Convert.ToDateTime(Shift_Name.Rows[i]["Apply_From_Date"].ToString().Trim()).ToString());
                    Rows_affected++;
                }
            }
            else
            {
                b = objShift.InsertShiftMaster(Session["CompId"].ToString(), Shift_Name.Rows[i]["Shift_Name"].ToString().Trim(), Shift_Name.Rows[i]["Shift_Name"].ToString().Trim(), Session["BrandId"].ToString(), Cycle_No, "7", Convert.ToDateTime(Shift_Name.Rows[i]["Apply_From_Date"].ToString().Trim()).ToString(), "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                if (b != 0)
                {
                    Add_Time(b.ToString(), ref trns, Shift_Name.Rows[i]["Shift_Name"].ToString().Trim(), Cycle_No, Convert.ToDateTime(Shift_Name.Rows[i]["Apply_From_Date"].ToString().Trim()).ToString());
                    Rows_affected++;
                }
            }
        }
        btnBackToMapData_Click(null, null);
        DisplayMessage(Rows_affected + " Row Affected");
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "LI_List_Active_UPDATE()", true);
    }

    public void Add_Time(string Ref_ID, ref SqlTransaction trns, string Shift_Name, string Cycle_No, string Apply_From_Date)
    {
        //SubDtScatch();
        int b = 0;
        if (Ref_ID != "")
        {
            b = objShift.UpdateShiftMaster(Ref_ID, Session["CompId"].ToString(), Shift_Name, Shift_Name, Session["BrandId"].ToString(), Cycle_No, "7", Apply_From_Date, "", "", "", "", HttpContext.Current.Session["LocId"].ToString(), true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
        }
        else
        {
            string shiftid = "";
            b = objShift.InsertShiftMaster(Session["CompId"].ToString(), Shift_Name, Shift_Name, Session["BrandId"].ToString(), Cycle_No, "7", Apply_From_Date, "", "", "", "", HttpContext.Current.Session["LocId"].ToString(),  true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
            Ref_ID = shiftid;
        }
        if (b != 0)
        {
            Shift_Save(b.ToString(), ref trns, Shift_Name, Cycle_No, Apply_From_Date);
        }
    }

    public void Shift_Save(string Ref_ID, ref SqlTransaction trns, string Shift_Name, string Cycle_No, string Apply_From_Date)
    {

        bool strWeekOffParam = true;
        DataTable dtWeekOffParam = objparam.GetApplicationParameterByCompanyId("AddWeekOffInShift", Session["CompId"].ToString());
        dtWeekOffParam = new DataView(dtWeekOffParam, "Param_Name='AddWeekOffInShift'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtWeekOffParam.Rows.Count > 0)
        {
            strWeekOffParam = Convert.ToBoolean(dtWeekOffParam.Rows[0]["Param_Value"].ToString());
        }
        else
        {
            strWeekOffParam = true;
        }

        string WeekOff = string.Empty;
        if (strWeekOffParam == true)
        {
            WeekOff = objparam.GetApplicationParameterValueByParamName("Week Off Days", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }

        string strMsg = string.Empty;
        int TransId = 0;
        DataTable dtShiftDes = objShiftDesc.GetShiftDescriptionByShiftId(Ref_ID, ref trns);
        if (dtShiftDes.Rows.Count == 0)
        {
            DataTable Dt_Shift = Session["Dt_Sheet_Upload"] as DataTable;
            Dt_Shift = new DataView(Dt_Shift, "Shift_Name='" + Shift_Name + "' and Is_Valid='Valid'  and Remark=''", "", DataViewRowState.CurrentRows).ToTable();

            for (int i = 2; i < Dt_Shift.Columns.Count - 2; i++)
            {
                string CycleType = Dt_Shift.Columns[i].ColumnName.ToString().Split('-')[0].ToString();
                string CycleDay = string.Empty;
                if (Dt_Shift.Columns[i].ColumnName.ToString().Split('-')[2].ToString() == "Monday")
                    CycleDay = "1";
                else if (Dt_Shift.Columns[i].ColumnName.ToString().Split('-')[2].ToString() == "Tuesday")
                    CycleDay = "2";
                else if (Dt_Shift.Columns[i].ColumnName.ToString().Split('-')[2].ToString() == "Wednesday")
                    CycleDay = "3";
                else if (Dt_Shift.Columns[i].ColumnName.ToString().Split('-')[2].ToString() == "Thursday")
                    CycleDay = "4";
                else if (Dt_Shift.Columns[i].ColumnName.ToString().Split('-')[2].ToString() == "Friday")
                    CycleDay = "5";
                else if (Dt_Shift.Columns[i].ColumnName.ToString().Split('-')[2].ToString() == "Saturday")
                    CycleDay = "6";
                else if (Dt_Shift.Columns[i].ColumnName.ToString().Split('-')[2].ToString() == "Sunday")
                    CycleDay = "7";

                string Cycle_Type_Day = CycleType + "-" + Dt_Shift.Columns[i].ColumnName.ToString().Split('-')[1].ToString();
                if (WeekOff != Dt_Shift.Columns[i].ColumnName.ToString().Split('-')[2].ToString())
                {
                    if (Dt_Shift.Rows[0][i].ToString().Trim() != "")
                    {
                        if (Dt_Shift.Rows[0][i].ToString().Count(x => (x == '|')) > 0)
                        {
                            for (int j = 0; j <= Dt_Shift.Rows[0][i].ToString().Count(x => (x == '|')); j++)
                            {
                                string Value = Dt_Shift.Rows[0][i].ToString().Split('|')[j].ToString().Trim();
                                if (Value != "")
                                {
                                    DataTable dtShift = objShiftDesc.GetShiftDescriptionByShiftId(Ref_ID, ref trns);
                                    dtShift = new DataView(dtShift, "Cycle_Type='" + Cycle_Type_Day + "' and Cycle_Day = '" + CycleDay + "'", "", DataViewRowState.CurrentRows).ToTable();
                                    DataTable dtDisTimeTable = dtShift.DefaultView.ToTable(true, "TimeTable_Id");
                                    string Time_Table_ID = string.Empty;
                                    DataTable Dt_Time_ID = objTimeTable.GetTimeTableMasterByTimeTableName(Session["CompId"].ToString(), Value, ref trns);
                                    if (Dt_Time_ID != null && Dt_Time_ID.Rows.Count > 0)
                                        Time_Table_ID = Dt_Time_ID.Rows[0]["TimeTable_Id"].ToString();

                                    if (dtDisTimeTable.Rows.Count > 0)
                                    {
                                        int f = 0;
                                        for (int k = 0; k < dtDisTimeTable.Rows.Count; k++)
                                        {
                                            DataTable dtin = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString(), dtDisTimeTable.Rows[k]["TimeTable_Id"].ToString(), ref trns);
                                            DateTime dtintime = Convert.ToDateTime(dtin.Rows[0]["OnDuty_Time"]);
                                            DateTime dtouttime = Convert.ToDateTime(dtin.Rows[0]["OffDuty_Time"]);
                                            DataTable dtin1 = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString(), Time_Table_ID, ref trns);
                                            DateTime dtintime1 = Convert.ToDateTime(dtin1.Rows[0]["OnDuty_Time"]);
                                            DateTime dtouttime1 = Convert.ToDateTime(dtin1.Rows[0]["OffDuty_Time"]);
                                            if (ISOverLapTimeTable(dtintime1, dtouttime1, dtintime, dtouttime))
                                            {
                                                f = 1;
                                                strMsg += Dt_Shift.Rows[0][i].ToString() + ",";
                                            }
                                        }
                                        if (f == 0)
                                        {
                                            TransId = int.Parse(dtShift.Rows[0]["Trans_Id"].ToString());
                                            objShiftDesc.InsertShift_TimeTable(TransId.ToString(), Time_Table_ID, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                        }
                                    }
                                    else
                                    {
                                        TransId = objShiftDesc.InsertShiftDescription(Ref_ID, Cycle_Type_Day, CycleDay, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                        objShiftDesc.InsertShift_TimeTable(TransId.ToString(), Time_Table_ID, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                    }
                                }
                            }
                        }
                        else
                        {
                            string Value = Dt_Shift.Rows[0][i].ToString().Trim();
                            if (Value != "")
                            {
                                DataTable dtShift = objShiftDesc.GetShiftDescriptionByShiftId(Ref_ID, ref trns);
                                dtShift = new DataView(dtShift, "Cycle_Type='" + Cycle_Type_Day + "' and Cycle_Day = '" + CycleDay + "'", "", DataViewRowState.CurrentRows).ToTable();
                                DataTable dtDisTimeTable = dtShift.DefaultView.ToTable(true, "TimeTable_Id");
                                string Time_Table_ID = string.Empty;
                                DataTable Dt_Time_ID = objTimeTable.GetTimeTableMasterByTimeTableName(Session["CompId"].ToString(), Value, ref trns);
                                if (Dt_Time_ID != null && Dt_Time_ID.Rows.Count > 0)
                                    Time_Table_ID = Dt_Time_ID.Rows[0]["TimeTable_Id"].ToString();

                                if (dtDisTimeTable.Rows.Count > 0)
                                {
                                    int f = 0;
                                    for (int k = 0; k < dtDisTimeTable.Rows.Count; k++)
                                    {
                                        DataTable dtin = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString(), dtDisTimeTable.Rows[k]["TimeTable_Id"].ToString(), ref trns);
                                        DateTime dtintime = Convert.ToDateTime(dtin.Rows[0]["OnDuty_Time"]);
                                        DateTime dtouttime = Convert.ToDateTime(dtin.Rows[0]["OffDuty_Time"]);
                                        DataTable dtin1 = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString(), Time_Table_ID, ref trns);
                                        DateTime dtintime1 = Convert.ToDateTime(dtin1.Rows[0]["OnDuty_Time"]);
                                        DateTime dtouttime1 = Convert.ToDateTime(dtin1.Rows[0]["OffDuty_Time"]);
                                        if (ISOverLapTimeTable(dtintime1, dtouttime1, dtintime, dtouttime))
                                        {
                                            f = 1;
                                            strMsg += Dt_Shift.Rows[0][i].ToString() + ",";
                                        }
                                    }
                                    if (f == 0)
                                    {
                                        TransId = int.Parse(dtShift.Rows[0]["Trans_Id"].ToString());
                                        objShiftDesc.InsertShift_TimeTable(TransId.ToString(), Time_Table_ID, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                    }
                                }
                                else
                                {
                                    TransId = objShiftDesc.InsertShiftDescription(Ref_ID, Cycle_Type_Day, CycleDay, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                    objShiftDesc.InsertShift_TimeTable(TransId.ToString(), Time_Table_ID, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            DataTable dtDisTimeTable = new DataTable();
            DataTable Dt_Shift = Session["Dt_Sheet_Upload"] as DataTable;
            Dt_Shift = new DataView(Dt_Shift, "Shift_Name='" + Shift_Name + "' and Is_Valid='Valid'  and Remark=''", "", DataViewRowState.CurrentRows).ToTable();

            for (int i = 2; i < Dt_Shift.Columns.Count - 2; i++)
            {
                string CycleType = Dt_Shift.Columns[i].ColumnName.ToString().Split('-')[0].ToString();
                string CycleDay = string.Empty;
                if (Dt_Shift.Columns[i].ColumnName.ToString().Split('-')[2].ToString() == "Monday")
                    CycleDay = "1";
                else if (Dt_Shift.Columns[i].ColumnName.ToString().Split('-')[2].ToString() == "Tuesday")
                    CycleDay = "2";
                else if (Dt_Shift.Columns[i].ColumnName.ToString().Split('-')[2].ToString() == "Wednesday")
                    CycleDay = "3";
                else if (Dt_Shift.Columns[i].ColumnName.ToString().Split('-')[2].ToString() == "Thursday")
                    CycleDay = "4";
                else if (Dt_Shift.Columns[i].ColumnName.ToString().Split('-')[2].ToString() == "Friday")
                    CycleDay = "5";
                else if (Dt_Shift.Columns[i].ColumnName.ToString().Split('-')[2].ToString() == "Saturday")
                    CycleDay = "6";
                else if (Dt_Shift.Columns[i].ColumnName.ToString().Split('-')[2].ToString() == "Sunday")
                    CycleDay = "7";

                string Cycle_Type_Day = CycleType + "-" + Dt_Shift.Columns[i].ColumnName.ToString().Split('-')[1].ToString();

                if (WeekOff != Dt_Shift.Columns[i].ColumnName.ToString().Split('-')[2].ToString())
                {
                    if (Dt_Shift.Rows[0][i].ToString().Trim() != "")
                    {
                        DataTable dtShift = objShiftDesc.GetShiftDescriptionByShiftId(Ref_ID, ref trns);
                        dtShift = new DataView(dtShift, "Cycle_Type = '" + Cycle_Type_Day + "'  and Cycle_Day='" + CycleDay + "' and convert(TimeTable_Id,System.String) <> ''", "", DataViewRowState.CurrentRows).ToTable();
                        dtDisTimeTable = dtShift.DefaultView.ToTable(true, "TimeTable_Id");

                        string Time_Table_ID = string.Empty;
                        DataTable Dt_Time_ID = objTimeTable.GetTimeTableMasterByTimeTableName(Session["CompId"].ToString(), Dt_Shift.Rows[0][i].ToString().Trim(), ref trns);
                        if (Dt_Time_ID != null && Dt_Time_ID.Rows.Count > 0)
                            Time_Table_ID = Dt_Time_ID.Rows[0]["TimeTable_Id"].ToString();


                        if (dtDisTimeTable.Rows.Count > 0)
                        {
                            for (int k = 0; k < dtDisTimeTable.Rows.Count; k++)
                            {
                                DataTable dtin = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString(), dtDisTimeTable.Rows[k]["TimeTable_Id"].ToString(), ref trns);
                                DateTime dtintime = Convert.ToDateTime(dtin.Rows[0]["OnDuty_Time"]);
                                DateTime dtouttime = Convert.ToDateTime(dtin.Rows[0]["OffDuty_Time"]);
                                DataTable dtin1 = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString(), Time_Table_ID, ref trns);
                                DateTime dtintime1 = Convert.ToDateTime(dtin1.Rows[0]["OnDuty_Time"]);
                                DateTime dtouttime1 = Convert.ToDateTime(dtin1.Rows[0]["OffDuty_Time"]);
                                DataTable dt1 = new DataView(dtShift, "TimeTable_Id='" + dtDisTimeTable.Rows[k]["TimeTable_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                if (dt1.Rows.Count > 0)
                                {
                                    if (ISOverLapTimeTable(dtintime1, dtouttime1, dtintime, dtouttime))
                                    {
                                        strMsg += Dt_Shift.Rows[0][i].ToString() + ",";
                                        break;
                                    }
                                    else
                                    {
                                        dtShift = objShiftDesc.GetShiftDescriptionByShiftId(Ref_ID, ref trns);
                                        dtShift = new DataView(dtShift, "Cycle_Type = '" + Cycle_Type_Day + "'  and Cycle_Day='" + CycleDay + "'", "", DataViewRowState.CurrentRows).ToTable();
                                        DataTable dtDisTimeTable1 = new DataTable();
                                        dtDisTimeTable1 = dtShift.DefaultView.ToTable(true, "TimeTable_Id");
                                        DataTable dtTimeTab = new DataTable();
                                        dtTimeTab = new DataView(dtDisTimeTable1, "TimeTable_Id='" + Time_Table_ID + "'", "", DataViewRowState.CurrentRows).ToTable();
                                        if (dtTimeTab.Rows.Count == 0)
                                        {
                                            TransId = int.Parse(dtShift.Rows[0]["Trans_Id"].ToString());
                                            objShiftDesc.InsertShift_TimeTable(TransId.ToString(), Time_Table_ID, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            dtShift = objShiftDesc.GetShiftDescriptionByShiftId(Ref_ID, ref trns);
                            dtShift = new DataView(dtShift, "Cycle_Type='" + Cycle_Type_Day + "' and Cycle_Day = '" + CycleDay + "' and  convert(TimeTable_Id,System.String) <> '' ", "", DataViewRowState.CurrentRows).ToTable();
                            dtDisTimeTable = dtShift.DefaultView.ToTable(true, "TimeTable_Id");

                            if (dtDisTimeTable.Rows.Count > 0)
                            {
                                for (int k = 0; k < dtDisTimeTable.Rows.Count; k++)
                                {
                                    DataTable dtin = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString(), dtDisTimeTable.Rows[k]["TimeTable_Id"].ToString(), ref trns);
                                    DateTime dtintime = Convert.ToDateTime(dtin.Rows[0]["OnDuty_Time"]);
                                    DateTime dtouttime = Convert.ToDateTime(dtin.Rows[0]["OffDuty_Time"]);
                                    DataTable dtin1 = objTimeTable.GetTimeTableMasterById(Session["CompId"].ToString(), Time_Table_ID, ref trns);
                                    DateTime dtintime1 = Convert.ToDateTime(dtin1.Rows[0]["OnDuty_Time"]);
                                    DateTime dtouttime1 = Convert.ToDateTime(dtin1.Rows[0]["OffDuty_Time"]);
                                    DataTable dt1 = new DataView(dtShift, "TimeTable_Id='" + dtDisTimeTable.Rows[k]["TimeTable_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                                    if (dt1.Rows.Count > 0)
                                    {
                                        if (ISOverLapTimeTable(dtintime1, dtouttime1, dtintime, dtouttime))
                                        {
                                            strMsg += Dt_Shift.Rows[0][i].ToString() + ",";
                                            break;
                                        }
                                        else
                                        {
                                            dtShift = objShiftDesc.GetShiftDescriptionByShiftId(Ref_ID, ref trns);
                                            dtShift = new DataView(dtShift, "Cycle_Type = '" + Cycle_Type_Day + "'  and Cycle_Day='" + CycleDay + "'", "", DataViewRowState.CurrentRows).ToTable();
                                            DataTable dtDisTimeTable1 = new DataTable();
                                            dtDisTimeTable1 = dtShift.DefaultView.ToTable(true, "TimeTable_Id");
                                            DataTable dtTimeTab = new DataTable();
                                            dtTimeTab = new DataView(dtDisTimeTable1, "TimeTable_Id='" + Time_Table_ID + "'", "", DataViewRowState.CurrentRows).ToTable();

                                            if (dtTimeTab.Rows.Count == 0)
                                            {
                                                TransId = int.Parse(dtShift.Rows[0]["Trans_Id"].ToString());
                                                objShiftDesc.InsertShift_TimeTable(TransId.ToString(), Time_Table_ID, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                dtShift = objShiftDesc.GetShiftDescriptionByShiftId(Ref_ID, ref trns);
                                dtShift = new DataView(dtShift, "Cycle_Type = '" + Cycle_Type_Day + "'  and Cycle_Day='" + CycleDay + "'", "", DataViewRowState.CurrentRows).ToTable();
                                if (dtShift.Rows.Count == 0)
                                {
                                    TransId = objShiftDesc.InsertShiftDescription(Ref_ID, Cycle_Type_Day, CycleDay, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                    objShiftDesc.InsertShift_TimeTable(TransId.ToString(), Time_Table_ID, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                }
                                else
                                {
                                    TransId = int.Parse(dtShift.Rows[0]["Trans_Id"].ToString());
                                    objShiftDesc.InsertShift_TimeTable(TransId.ToString(), Time_Table_ID, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString()).ToString(), ref trns);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    protected void btnBackToMapData_Click(object sender, EventArgs e)
    {
        Div_Upload_Grid.Visible = false;
        GV_Sheet_Upload.DataSource = null;
        GV_Sheet_Upload.DataBind();
        lbltotaluploadRecord.Text = "";
        DDl_Excel_Sheet.Items.Clear();
        FU_Shift_Upload.Dispose();
        FU_Shift_Upload.Attributes.Clear();
    }

    protected void Btn_Download_Excel_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Shift_View_Show()", true);
        DateTime Cur_Date = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),HttpContext.Current.Session["TimeZoneId"].ToString());
        string Current_Date = Cur_Date.ToString("dd-MMM-yyyy");
        string strdatecell = string.Empty;
        DataTable dt = new DataTable();

        dt.Columns.Add("Shift_Name", typeof(string));
        dt.Columns.Add("Apply_From_Date", typeof(string));

        for (int i = 1; i <= Convert.ToInt32(DDl_Cycle_Of_Week.SelectedValue); i++)
        {
            dt.Columns.Add("Week-" + i + "-Sunday", typeof(string));
            dt.Columns.Add("Week-" + i + "-Monday", typeof(string));
            dt.Columns.Add("Week-" + i + "-Tuesday", typeof(string));
            dt.Columns.Add("Week-" + i + "-Wednesday", typeof(string));
            dt.Columns.Add("Week-" + i + "-Thursday", typeof(string));
            dt.Columns.Add("Week-" + i + "-Friday", typeof(string));
            dt.Columns.Add("Week-" + i + "-Saturday", typeof(string));
            dt.AcceptChanges();
        }
        if (DDl_Cycle_Of_Week.SelectedValue.ToString() == "1")
        {
            dt.Rows.Add("Morning Shift", Current_Date, "", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time");
            dt.Rows.Add("Day Shift", Current_Date, "", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time");
            dt.Rows.Add("Evening Shift", Current_Date, "", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time");
            dt.Rows.Add("Night Shift", Current_Date, "", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time");
            ExportTableData(dt, "Shift Management");
        }
        else if (DDl_Cycle_Of_Week.SelectedValue.ToString() == "2")
        {
            dt.Rows.Add("Morning Shift", Current_Date, "", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time");
            dt.Rows.Add("Day Shift", Current_Date, "", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time");
            dt.Rows.Add("Evening Shift", Current_Date, "", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time");
            dt.Rows.Add("Night Shift", Current_Date, "", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time");
            ExportTableData(dt, "Shift Management");
        }
        else if (DDl_Cycle_Of_Week.SelectedValue.ToString() == "3")
        {
            dt.Rows.Add("Morning Shift", Current_Date, "", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time");
            dt.Rows.Add("Day Shift", Current_Date, "", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time");
            dt.Rows.Add("Evening Shift", Current_Date, "", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time");
            dt.Rows.Add("Night Shift", Current_Date, "", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time");
            ExportTableData(dt, "Shift Management");
        }
        else if (DDl_Cycle_Of_Week.SelectedValue.ToString() == "4")
        {
            dt.Rows.Add("Morning Shift", Current_Date, "", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time", "Morning Shift Time");
            dt.Rows.Add("Day Shift", Current_Date, "", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time", "Day Shift Time");
            dt.Rows.Add("Evening Shift", Current_Date, "", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time", "Evening Shift Time");
            dt.Rows.Add("Night Shift", Current_Date, "", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time", "Night Shift Time");
            ExportTableData(dt, "Shift Management");
        }
    }

    protected void Lnk_Demo_Shift_Upload_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "Modal_Shift_View_Show()", true);
    }

    protected void Rbt_All_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["Dt_Sheet_Upload"];


        if (Rbt_Valid.Checked)
        {
            dt = new DataView(dt, "Is_Valid='Valid' or Is_Valid=''", "", DataViewRowState.CurrentRows).ToTable();
        }

        if (Rbt_Invalid.Checked)
        {
            dt = new DataView(dt, "Is_Valid<>'Valid'", "", DataViewRowState.CurrentRows).ToTable();
        }

        GV_Sheet_Upload.DataSource = dt;
        GV_Sheet_Upload.DataBind();
        lbltotaluploadRecord.Text = "Total Record : " + (dt.Rows.Count).ToString();
    }
    public void fillLocation()
    {

        ddlLocation.Items.Clear();

        DataTable dtLoc = new LocationMaster(Session["DBConnection"].ToString()).GetLocationMaster(Session["CompId"].ToString());

        try
        {
            dtLoc = new DataView(dtLoc, "Brand_Id='" + Session["BrandId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
        }
        catch
        {
        }
        ddlLocation.DataSource = dtLoc;
        ddlLocation.DataTextField = "Location_Name";
        ddlLocation.DataValueField = "Location_Id";
        ddlLocation.DataBind();
        ddlLocation.Items.Insert(0, new ListItem("All", "0"));
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        FillGrid();
    }
}
//--------------------------------------------------



