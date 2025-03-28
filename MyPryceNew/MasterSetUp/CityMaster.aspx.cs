using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;

public partial class MasterSetUp_CityMaster : BasePage
{
    StateMaster objstateMaster = null;
    CityMaster objCityMaster = null;
    Common ObjComman = null;
    SystemParameter objSys = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objstateMaster = new StateMaster(Session["DBConnection"].ToString());
        objCityMaster = new CityMaster(Session["DBConnection"].ToString());
        ObjComman = new Common(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        FillGrid();
        if (!IsPostBack)
        {
            fillCountry();
            fillGridBin();
            AllPageCode();
        }

    }

    public void FillGrid()
    {
        DataTable dt_CityData = objCityMaster.GetAllIsActiveTrueCities();
        GvCityMaster.DataSource = dt_CityData;
        GvCityMaster.DataBind();
    }
    public void fillCountry()
    {
        CountryMaster objCountry = new CountryMaster(Session["DBConnection"].ToString());
        DataTable dsCountry = null;
        dsCountry = objCountry.GetCountryMaster();
        if (dsCountry.Rows.Count > 0)
        {
            objPageCmn.FillData((object)ddlCountry, dsCountry, "Country_Name", "Country_Id");
        }
        else
        {
            ddlCountry.Items.Insert(0, "--Select--");
            ddlCountry.SelectedIndex = 0;
        }
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string FillState(string country_id,string State_id)
    {
        string result = "";
        if (country_id != "--Select--")
        {
            DataTable dt_Statedata = new StateMaster(HttpContext.Current.Session["DBConnection"].ToString()).GetAllStateByCountryId(country_id);
            if (dt_Statedata.Rows.Count > 0)
            {
               

                DataView view = new DataView(dt_Statedata);
                dt_Statedata = view.ToTable(true, "Trans_Id", "State_Name");
                string statename = "";
                if(State_id!="0")
                {
                    foreach (DataRow dr in dt_Statedata.Rows)
                    {
                        if (dr["Trans_Id"].ToString() == State_id)
                        {
                            statename = dr["State_Name"].ToString();
                            dr.Delete();
                            break;
                        }
                    }
                    DataRow dr1;
                    dr1 = dt_Statedata.NewRow();
                    dr1["Trans_Id"] = State_id;
                    dr1["State_Name"] = statename;
                    dt_Statedata.Rows.InsertAt(dr1, 0);
                }

                using (StringWriter sw = new StringWriter())
                {
                    dt_Statedata.WriteXml(sw);
                    result = sw.ToString();
                }
                return result;              
            }
        }
        return "no"; 
    }   

    protected void btnbindBin_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        fillGridBin();
        string condition = string.Empty;
        if (ddlOptionBin.SelectedIndex != 0)
        {
            if (ddlOptionBin.SelectedIndex == 1)
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String)='" + txtValueBin.Text.Trim() + "'";
            }
            else if (ddlOptionBin.SelectedIndex == 2)
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) like '%" + txtValueBin.Text.Trim() + "%'";
            }
            else
            {
                condition = "convert(" + ddlFieldNameBin.SelectedValue + ",System.String) Like '" + txtValueBin.Text.Trim() + "%'";
            }


            DataTable dtCust = (DataTable)Session["gvCityMasterBin_data"];
            DataView view = new DataView(dtCust, condition, "", DataViewRowState.CurrentRows);
            Session["gvCityMasterBin_data"] = view.ToTable();
            lblTotalRecordsBin.Text = Resources.Attendance.Total_Records + " : " + view.ToTable().Rows.Count.ToString() + "";
            //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
            objPageCmn.FillData((object)gvCityMasterBin, view.ToTable(), "", "");

            lblSelectedRecord.Text = "";
            if (view.ToTable().Rows.Count == 0)
            {
                fillGridBin();
            }
            System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(btnRefreshBin);
        }
        txtValueBin.Focus();
    }

    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static CityMaster GetCityByTrans_Id(string trans_id)
    {
        CityMaster cm = new CityMaster(HttpContext.Current.Session["DBConnection"].ToString());
        DataTable dtInfo = cm.GetCityByTrans_Id(trans_id);
        cm.TransId = dtInfo.Rows[0]["Trans_Id"].ToString();
        cm.Country_Id = dtInfo.Rows[0]["Country_Id"].ToString();
        cm.State_Id = dtInfo.Rows[0]["State_Id"].ToString();
        cm.State_Name = dtInfo.Rows[0]["State_Name"].ToString();
        cm.City_Name = dtInfo.Rows[0]["City_Name"].ToString();
        cm.City_Name_Local = dtInfo.Rows[0]["City_Name_Local"].ToString();
        return cm;
    }
    [System.Web.Services.WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static int DeleteByTrans_Id(string trans_id)
    {
        CityMaster cm = new CityMaster(HttpContext.Current.Session["DBConnection"].ToString());        
        string addressCount = "";
        addressCount = cm.daClass.get_SingleValue("select count(trans_id) from set_addressmaster where cityId='" + trans_id + "'");
        addressCount = addressCount == "@NOTFOUND@" ? "" : addressCount;
        if (addressCount != "" && addressCount != "0")
        {
            return 0;
        }
        cm.SetCityIsActiveFalse(trans_id, HttpContext.Current.Session["UserId"].ToString());
        return 1;
    }

    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkSelAll = ((CheckBox)gvCityMasterBin.HeaderRow.FindControl("chkgvSelectAll"));
        for (int i = 0; i < gvCityMasterBin.Rows.Count; i++)
        {
            ((CheckBox)gvCityMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = chkSelAll.Checked;
            if (chkSelAll.Checked)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(((HiddenField)(gvCityMasterBin.Rows[i].FindControl("hdnTrans_Id"))).Value.Trim().ToString()))
                {
                    lblSelectedRecord.Text += ((HiddenField)(gvCityMasterBin.Rows[i].FindControl("hdnTrans_Id"))).Value.Trim().ToString() + ",";
                }
            }
            else
            {
                string temp = string.Empty;
                string[] split = lblSelectedRecord.Text.Split(',');
                foreach (string item in split)
                {
                    if (item != ((HiddenField)(gvCityMasterBin.Rows[i].FindControl("hdnTrans_Id"))).Value.Trim().ToString())
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

    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        string empidlist = string.Empty;
        string temp = string.Empty;
        int index = ((GridViewRow)((CheckBox)sender).Parent.Parent).RowIndex;
        HiddenField lb = (HiddenField)gvCityMasterBin.Rows[index].FindControl("hdnTrans_Id");
        if (((CheckBox)gvCityMasterBin.Rows[index].FindControl("chkgvSelect")).Checked)
        {
            empidlist += lb.Value.Trim().ToString() + ",";
            lblSelectedRecord.Text += empidlist;
        }
        else
        {
            empidlist += lb.Value.ToString().Trim();
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

    protected void gvCityMasterBin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCityMasterBin.PageIndex = e.NewPageIndex;

        DataTable dt = (DataTable)Session["gvCityMasterBin_data"];
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvCityMasterBin, dt, "", "");


        string temp = string.Empty;

        for (int i = 0; i < gvCityMasterBin.Rows.Count; i++)
        {
            Label lblconid = (Label)gvCityMasterBin.Rows[i].FindControl("gvlblTransId");
            string[] split = lblSelectedRecord.Text.Split(',');

            for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
            {
                if (lblSelectedRecord.Text.Split(',')[j] != "")
                {
                    if (lblconid.Text.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                    {
                        ((CheckBox)gvCityMasterBin.Rows[i].FindControl("chkSelect")).Checked = true;
                    }
                }
            }
        }
    }

    protected void gvCityMasterBin_Sorting(object sender, GridViewSortEventArgs e)
    {
        ViewState["Select"] = null;
        HDFSortbin.Value = HDFSortbin.Value == "ASC" ? "DESC" : "ASC";
        DataTable dt = (DataTable)Session["gvCityMasterBin_data"];

        DataView dv = new DataView(dt);
        string Query = "" + e.SortExpression + " " + HDFSortbin.Value + "";
        dv.Sort = Query;
        dt = dv.ToTable();
        Session["gvCityMasterBin_data"] = dt;
        //this function for bind control by function in common class by jitendra upadhyay on 05-05-2015
        objPageCmn.FillData((object)gvCityMasterBin, dt, "", "");
        lblSelectedRecord.Text = "";
    }

    protected void btnRefreshBin_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        fillGridBin();
    }

    protected void imgBtnRestore_Click(object sender, EventArgs e)
    {
        I1.Attributes.Add("Class", "fa fa-minus");
        Div1.Attributes.Add("Class", "box box-primary");
        int b = 0;
        DataTable dt = (DataTable)Session["gvCityMasterBin_data"];

        if (gvCityMasterBin.Rows.Count != 0)
        {
            if (lblSelectedRecord.Text != "")
            {
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        b = objCityMaster.SetCityIsActiveTrue(lblSelectedRecord.Text.Split(',')[j].Trim().ToString(), Session["UserId"].ToString());
                    }
                }
            }

            if (b != 0)
            {
                FillGrid();
                fillGridBin();

                lblSelectedRecord.Text = "";
                DisplayMessage("Record Activated");
            }
            else
            {
                int fleg = 0;
                foreach (GridViewRow Gvr in gvCityMasterBin.Rows)
                {
                    CheckBox chk = (CheckBox)Gvr.FindControl("chkSelect");
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
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(txtValueBin);
    }

    protected void ImgbtnSelectAll_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dtCitymasterBin = (DataTable)Session["gvCityMasterBin_data"];

        if (ViewState["Select"] == null)
        {
            ViewState["Select"] = 1;
            foreach (DataRow dr in dtCitymasterBin.Rows)
            {
                if (!lblSelectedRecord.Text.Split(',').Contains(dr["Trans_ID"]))
                {
                    lblSelectedRecord.Text += dr["Trans_ID"] + ",";
                }
            }
            for (int i = 0; i < gvCityMasterBin.Rows.Count; i++)
            {
                string[] split = lblSelectedRecord.Text.Split(',');
                HiddenField lblconid = (HiddenField)gvCityMasterBin.Rows[i].FindControl("hdnTrans_Id");
                for (int j = 0; j < lblSelectedRecord.Text.Split(',').Length; j++)
                {
                    if (lblSelectedRecord.Text.Split(',')[j] != "")
                    {
                        if (lblconid.Value.Trim().ToString() == lblSelectedRecord.Text.Split(',')[j].Trim().ToString())
                        {
                            ((CheckBox)gvCityMasterBin.Rows[i].FindControl("chkgvSelect")).Checked = true;
                        }
                    }
                }
            }
        }
        else
        {
            lblSelectedRecord.Text = "";
            objPageCmn.FillData((object)gvCityMasterBin, dtCitymasterBin, "", "");
            ViewState["Select"] = null;
        }
    }

    private void fillGridBin()
    {
        DataTable dt_state = objCityMaster.GetAllIsActiveFalseCities();
        gvCityMasterBin.DataSource = dt_state;
        gvCityMasterBin.DataBind();
        Session["gvCityMasterBin_data"] = dt_state;
        lblTotalRecordsBin.Text = "Total Records : " + dt_state.Rows.Count;
    }

    public void DisplayMessage(string str,string color="orange")
    {
        if (Session["lang"] == null)
        {
            Session["lang"] = "1";
        }
        if (Session["lang"].ToString() == "1")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
        }
        else if (Session["lang"].ToString() == "2")
        {
             ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + GetArebicMessage(str) + "','"+color+"','white');", true);
        }
    }
    public string GetArebicMessage(string EnglishMessage)
    {
        string ArebicMessage = string.Empty;
        DataTable dtres = (DataTable)Session["MessageDt"];
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!checkValidation())
        {
            return;
        }

        if (hdnTrans_id.Value == "")
        {
            objCityMaster.InsertCity(hdnState_Id.Value, txtCityName.Text, txtCityNameLocal.Text, Session["UserId"].ToString(), Session["UserId"].ToString()).ToString();
            DisplayMessage("Record Saved","green");
            reset();
            FillGrid();
        }
        else
        {
            objCityMaster.UpdateCity(hdnTrans_id.Value, hdnState_Id.Value, txtCityName.Text, txtCityNameLocal.Text, Session["UserId"].ToString());
            DisplayMessage("Record Updated", "green");
            FillGrid();
            reset();
        }
    }

    public bool checkValidation()
    {
        if(hdnState_Id.Value=="")
        {
            DisplayMessage("Please Select State");
            return false;
        }


        if (txtCityName.Text.Trim() == "")
        {
            DisplayMessage("Please Enter City Name");
            return false;
        }

        return true;

    }
    private void reset()
    {
        hdnTrans_id.Value = "";
        ddlCountry.SelectedIndex = 0;
        ddlState.SelectedIndex = 0;
        txtCityName.Text = "";
        txtCityNameLocal.Text = "";
    }

    protected void btn_fillGrid_Click(object sender, EventArgs e)
    {
        FillGrid();
        fillGridBin();
    }


    protected void btnReset_Click(object sender, EventArgs e)
    {
        reset();
    }

    protected void DownloadSampleExcel_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Country_Name");
        dt.Columns.Add("State_Name");
        dt.Columns.Add("City_Name");
        dt.Columns.Add("City_Name_Local");
        ExportTableData(dt);
    }

    public void ExportTableData(DataTable dtdata)
    {
        string strFname = "CityMaster";
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
    protected void FileUploadComplete(object sender, EventArgs e)
    {
        int fileType = 0;

        if (fileLoad.HasFile)
        {
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
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

                string path = Server.MapPath("~/Temp/" + fileLoad.FileName);
                fileLoad.SaveAs(path);
                Import(path, fileType);
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
            //Provider=Microsoft.ACE.OLEDB.12.0;Data Source=d:/abc.mdb;Persist Security Info=False
            strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Persist Security Info=False";
        }


        Session["cnn"] = strcon;

        OleDbConnection conn = new OleDbConnection(strcon);
        conn.Open();

        DataTable tables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
        ddlTables.DataSource = tables;

        ddlTables.DataTextField = "TABLE_NAME";
        ddlTables.DataValueField = "TABLE_NAME";
        ddlTables.DataBind();

        conn.Close();


    }

    protected void btnGetSheet_Click(object sender, EventArgs e)
    {
        int fileType = 0;

        if (fileLoad.HasFile)
        {
            string Path = string.Empty;
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
                Path = Server.MapPath("~/Temp/" + fileLoad.FileName);

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

    protected void btnConnect_Click(object sender, EventArgs e)
    {
        string strResult = string.Empty;
        DataTable dt = new DataTable();
        DataTable dtTemp = new DataTable();
        CountryMaster objCM = new CountryMaster(HttpContext.Current.Session["DBConnection"].ToString());

        string strDateVal = string.Empty;
        if (ddlTables == null)
        {
            DisplayMessage("Sheet not found");
            return;
        }
        else if (ddlTables.Items.Count == 0)
        {
            DisplayMessage("Sheet not found");
            return;
        }

        string strEmpId = string.Empty;




        if (fileLoad.HasFile)
        {
            string Path = string.Empty;
            string ext = fileLoad.FileName.Substring(fileLoad.FileName.Split('.')[0].Length);
            if ((ext != ".xls") && (ext != ".xlsx") && (ext != ".mdb") && (ext != ".accdb"))
            {
                DisplayMessage("Invalid File Type, Select Only .xls, .xlsx, .mdb, .accdb extension file");
                return;
            }
            else
            {
                fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
                Path = Server.MapPath("~/Temp/" + fileLoad.FileName);

                //need to check
                dt = ConvetExcelToDataTable(Path, ddlTables.SelectedValue.Trim());

                dt.AcceptChanges();

                if (dt.Rows.Count == 0)
                {
                    DisplayMessage("Record not found");
                    return;
                }


                if (!CheckSheetValidation(dt))
                {
                    DisplayMessage("Upload valid excel sheet");
                    return;
                }

                if (!dt.Columns.Contains("IsValid"))
                {
                    dt.Columns.Add("IsValid");

                }

             
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["IsValid"] = "True";

                    if (dt.Rows[i][0].ToString() == "")
                    {
                        dt.Rows[i]["IsValid"] = "";
                        continue;
                    }

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {

                        if (dt.Columns[j].ColumnName.Trim() == "Country_Name")
                        {
                            if (dt.Rows[i]["Country_Name"].ToString().Trim() == "")
                            {
                                dt.Rows[i]["IsValid"] = "False(Country_Name - Enter Value)";
                                break;
                            }
                            else
                            {
                                string country_id = objCM.getCountryIdByName(dt.Rows[i]["Country_Name"].ToString().Trim());
                                if (country_id == "")
                                {
                                    dt.Rows[i]["IsValid"] = "False(Country_Name - Does not exist in Database)";
                                    break;
                                }
                            }
                        }

                        if (dt.Columns[j].ColumnName.Trim() == "State_Name")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i]["IsValid"] = "False(State_Name - Enter Value)";
                                break;
                            }
                            string state_Id = objstateMaster.GetStateIdByStateNameNCountryName(dt.Rows[i][j].ToString().Trim(), dt.Rows[i][j-1].ToString().Trim());
                            if(state_Id=="")
                            {
                                dt.Rows[i]["IsValid"] = "False(State_Name - Does Not Contains in Country)";
                                break;
                            }
                        }

                        if (dt.Columns[j].ColumnName.Trim() == "City_Name")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i]["IsValid"] = "False(City_Name - Enter Value)";
                                break;
                            }

                            DataTable dt_CityDuplicacy = objCityMaster.GetCityDataByCountry_State_CityName(dt.Rows[i][j - 2].ToString(),dt.Rows[i][j - 1].ToString(), dt.Rows[i][j].ToString());

                            if (dt_CityDuplicacy.Rows.Count > 0)
                            {
                                dt.Rows[i]["IsValid"] = "False(City_Name - Already exists in this States)";
                                break;
                            }
                        }

                        if (dt.Columns[j].ColumnName.Trim() == "City_Name_Local")
                        {
                            if (dt.Rows[i][j].ToString().Trim() == "")
                            {
                                dt.Rows[i]["IsValid"] = "False(City_Name_Local - Enter Value)";
                                break;
                            }
                        }
                    }

                }


                uploadEmpdetail.Visible = true;

                dtTemp = dt.DefaultView.ToTable(true, "Country_Name", "State_Name", "City_Name", "City_Name_Local", "IsValid");

                gvSelected.DataSource = dtTemp;
                gvSelected.DataBind();
                lbltotaluploadRecord.Text = "Total Record : " + (dtTemp.Rows.Count).ToString();
                Session["UploadEmpDtAll"] = dt;
                Session["UploadEmpDt"] = dtTemp;
                rbtnupdall.Checked = true;
                rbtnupdInValid.Checked = false;
                rbtnupdValid.Checked = false;
            }
        }
        else
        {
            DisplayMessage("File Not Found");
            return;
        }

        dt.Dispose();

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

    public bool CheckSheetValidation(DataTable dt)
    {
        bool Result = true;

        if (!dt.Columns.Contains("Country_Name") && dt.Columns.Contains("State_Name") && dt.Columns.Contains("State_Name_Local"))
        {
            Result = false;
        }
        return Result;
    }

   
    protected void rbtnupdall_OnCheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["UploadEmpDt"];


        if (rbtnupdValid.Checked)
        {
            dt = new DataView(dt, "IsValid='True' or IsValid=''", "", DataViewRowState.CurrentRows).ToTable();
        }

        if (rbtnupdInValid.Checked)
        {
            dt = new DataView(dt, "IsValid<>'True'", "", DataViewRowState.CurrentRows).ToTable();
        }

        gvSelected.DataSource = dt;
        gvSelected.DataBind();
        lbltotaluploadRecord.Text = "Total Record : " + (dt.Rows.Count).ToString();
    }

    protected void btnUploadEmpInfo_Click(object sender, EventArgs e)
    {

        if (gvSelected.Rows.Count == 0)
        {
            DisplayMessage("Record not found");
            return;
        }
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());
        con.Open();
        SqlTransaction trns;

        trns = con.BeginTransaction();
        try
        {

            dt = (DataTable)Session["UploadEmpDtAll"];


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["IsValid"].ToString().Trim() != "True")
                {
                    continue;
                }

                CountryMaster objCM = new CountryMaster(HttpContext.Current.Session["DBConnection"].ToString());
                DataTable dt_CityDuplicacy = objCityMaster.GetCityDataByCountry_State_CityName(dt.Rows[i]["Country_Name"].ToString().Trim(), dt.Rows[i]["State_Name"].ToString().Trim(), dt.Rows[i]["City_Name"].ToString().Trim(), ref trns);

                if(dt_CityDuplicacy.Rows.Count>0)
                {
                    continue;
                }

                string state_Id = objstateMaster.GetStateIdByStateNameNCountryName(dt.Rows[i]["State_Name"].ToString().Trim(), dt.Rows[i]["Country_Name"].ToString().Trim(),ref trns);


                if (state_Id != "")
                {
                    objCityMaster.InsertCity(state_Id, dt.Rows[i]["City_Name"].ToString().Trim(), dt.Rows[i]["City_Name_Local"].ToString().Trim(), Session["UserId"].ToString(), Session["UserId"].ToString(), ref trns).ToString();
                }
            }


            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();

            DisplayMessage("Record Updated Successfully", "green");
            FillGrid();
            //btnResetEmpInfo_Click(null, null);
            //rbtnupdateoption.Checked = true;
            //rbtnReportoption.Checked = false;
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


    protected void btnResetEmpInfo_Click(object sender, EventArgs e)
    {
        gvSelected.DataSource = null;
        gvSelected.DataBind();
        Session["UploadEmpDt"] = null;
        Session["UploadEmpDtAll"] = null;
        ddlTables.Items.Clear();
        //need to get document number 
        rbtnupdall.Checked = true;
        rbtnupdValid.Checked = false;
        rbtnupdInValid.Checked = false;
        uploadEmpdetail.Visible = false;
        //rbtnupdateoption.Checked = true;
        //rbtnReportoption.Checked = false;
    }
    protected void btndownloadInvalid_Click(object sender, EventArgs e)
    {
        //this event for download inavid record excel sheet 

        if (Session["UploadEmpDt"] == null)
        {
            DisplayMessage("Record Not found");
            return;
        }

        DataTable dt = (DataTable)(Session["UploadEmpDt"]);

        dt = new DataView(dt, "IsValid<>'True'", "", DataViewRowState.CurrentRows).ToTable();

        ExportTableData(dt);
    }

    public void AllPageCode()
    {
        IT_ObjectEntry objObjectEntry = new IT_ObjectEntry(HttpContext.Current.Session["DBConnection"].ToString());
        Common ObjComman = new Common(HttpContext.Current.Session["DBConnection"].ToString());


        //New Code created by jitendra on 09-12-2014
        string strModuleId = string.Empty;
        string strModuleName = string.Empty;

        DataTable dtModule = objObjectEntry.GetModuleIdAndName(Common.GetObjectIdbyPageURL("../MasterSetup/CityMaster.aspx", Session["DBConnection"].ToString()), (DataTable)Session["ModuleName"]);
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

        Page.Title = objSys.GetSysTitle();


        Session["AccordianId"] = strModuleId;
        Session["HeaderText"] = strModuleName;

        if (Session["EmpId"].ToString() == "0")
        {
            btnSave.Visible = true;
            GvCityMaster.Columns[0].Visible = true;
            GvCityMaster.Columns[1].Visible = true;
            imgBtnRestore.Visible = true;
            btnGetSheet.Visible = true;
            btnConnect.Visible = true;
            return;
        }

        DataTable dtAllPageCode = ObjComman.GetAllPagePermission(Session["UserId"].ToString(), strModuleId, Common.GetObjectIdbyPageURL("../MasterSetup/CityMaster.aspx", Session["DBConnection"].ToString()), HttpContext.Current.Session["CompId"].ToString());

        foreach (DataRow DtRow in dtAllPageCode.Rows)
        {
            if (DtRow["Op_Id"].ToString() == "1")
            {
                btnSave.Visible = true;
                btnGetSheet.Visible = true;
                btnConnect.Visible = true;
            }

            if (DtRow["Op_Id"].ToString() == "2")
            {
                GvCityMaster.Columns[0].Visible = true;
            }
            if (DtRow["Op_Id"].ToString() == "3")
            {
                GvCityMaster.Columns[1].Visible = true;
            }

            if (DtRow["Op_Id"].ToString() == "4")
            {
                imgBtnRestore.Visible = true;
            }

        }
    }

}