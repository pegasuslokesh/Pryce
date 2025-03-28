using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Data.SqlClient;

public partial class ITSetUp_UserExcelConfiguration : System.Web.UI.Page
{
    hr_laborLaw_config ObjLabourLaw = null;
    Inv_ParameterMaster objInvParam = null;
    Ems_ContactCompanyBrand EMS_CompanyBrand = null;
    Inv_Product_CompanyBrand ObjCompanyBrand = null;
    DataAccessClass objDa = null;
    Common cmn = null;
    Ac_Finance_Year_Info ObjFinance = null;
    Ac_FinancialYear_Detail ObjFinancedetail = null;
    ObjectMaster objObject = null;
    UserMaster ObjUser = null;
    Set_ApplicationParameter objAppParam = null;
    EmployeeMaster objEmp = null;
    HR_Indemnity_Master objIndemnity = null;
    Set_Emp_SalaryIncrement objEmpSalInc = null;
    EmployeeParameter objEmpParam = null;
    Att_Employee_Notification objEmpNotice = null;
    UserDataPermission objUserDataPerm = null;
    Ser_UserTransfer objSer = null;
    Att_DeviceMaster Objdevice = null;
    LocationMaster ObjLocationMaster = null;
    hr_laborLaw_leave ObjLabourLeavedetail = null;
    Att_Employee_Leave objEmpleave = null;
    LeaveMaster_deduction ObjLeavededuction = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        ObjLabourLaw = new hr_laborLaw_config(Session["DBConnection"].ToString());
        objInvParam = new Inv_ParameterMaster(Session["DBConnection"].ToString());
        EMS_CompanyBrand = new Ems_ContactCompanyBrand(Session["DBConnection"].ToString());
        ObjCompanyBrand = new Inv_Product_CompanyBrand(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        ObjFinance = new Ac_Finance_Year_Info(Session["DBConnection"].ToString());
        ObjFinancedetail = new Ac_FinancialYear_Detail(Session["DBConnection"].ToString());
        objObject = new ObjectMaster(Session["DBConnection"].ToString());
        ObjUser = new UserMaster(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        objIndemnity = new HR_Indemnity_Master(Session["DBConnection"].ToString());
        objEmpSalInc = new Set_Emp_SalaryIncrement(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());
        objUserDataPerm = new UserDataPermission(Session["DBConnection"].ToString());
        objSer = new Ser_UserTransfer(Session["DBConnection"].ToString());
        Objdevice = new Att_DeviceMaster(Session["DBConnection"].ToString());
        ObjLocationMaster = new LocationMaster(Session["DBConnection"].ToString());
        ObjLabourLeavedetail = new hr_laborLaw_leave(Session["DBConnection"].ToString());
        objEmpleave = new Att_Employee_Leave(Session["DBConnection"].ToString());
        ObjLeavededuction = new LeaveMaster_deduction(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            FillObjectname();
        }
        this.RegisterPostBackControl();
    }
    private void RegisterPostBackControl()
    {
        ScriptManager.GetCurrent(this).RegisterPostBackControl(btnDemo);
    }
    public void FillTableName(DropDownList ddl)
    {
        DataTable dt = objDa.return_DataTable("select sys.tables.name ,sys.tables.object_id from IT_Object_Table_Info inner join sys.tables on IT_Object_Table_Info.Table_name=sys.tables.name where IT_Object_Table_Info.object_id=" + ddlObjectname.SelectedValue.Trim() + "");
        objPageCmn.FillData((object)ddl, dt, "name", "Object_Id");
    }
    public void FillObjectname()
    {
        DataTable dt = objDa.return_DataTable("select distinct IT_ObjectEntry.object_name,IT_ObjectEntry.object_id from  IT_ObjectEntry inner join IT_Object_Table_Info on IT_ObjectEntry.Object_Id= IT_Object_Table_Info.Object_Id where IT_ObjectEntry.IsActive='True' ");
        objPageCmn.FillData((object)ddlObjectname, dt, "Object_Name", "Object_Id");
    }
    public void fillFieldname(string strTableObjectId, DropDownList ddl)
    {
        DataTable dt = objDa.return_DataTable("select IT_Object_ExcelConfig_Detail.Field_Caption_User as FieldName,IT_Object_ExcelConfig_Detail.Trans_Id from IT_Object_ExcelConfig_Header inner join IT_Object_ExcelConfig_Detail on IT_Object_ExcelConfig_Header.Trans_Id= IT_Object_ExcelConfig_Detail.Header_Id where IT_Object_ExcelConfig_Header.Object_Id=" + ddlObjectname.SelectedValue + " and IT_Object_ExcelConfig_Detail.Is_Visible='True'  order by IT_Object_ExcelConfig_Detail.sort_order");
        ddl.DataSource = dt;
        ddl.DataTextField = "FieldName";
        ddl.DataValueField = "Trans_Id";
        ddl.DataBind();
        ddl.Items.Insert(0, "--Select--");
        //objPageCmn.FillData((object)ddlFieldname, dt, "name", "name");
    }
    #region ClickEventList
    protected void ddlObjectname_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ResetDetail();
        ddlFieldname.Items.Clear();
        ddlTableName.Items.Clear();
        objPageCmn.FillData((object)gvExcelConfig, null, "", "");
        if (ddlObjectname.SelectedIndex > 0)
        {
            FillTableName(ddlTableName);
            DataTable dtexcelHeader = objDa.return_DataTable("select IT_Object_ExcelConfig_Header.Operation_Type,IT_Object_ExcelConfig_Header.Error_Action,IT_Object_ExcelConfig_Header.Consistency_Action,IT_Object_ExcelConfig_Header.Base_Table,IT_Object_ExcelConfig_Header.Error_Message,sys.tables.object_id from IT_Object_ExcelConfig_Header  inner join sys.tables on IT_Object_ExcelConfig_Header.base_table= sys.tables.name  WHERE IT_Object_ExcelConfig_Header.Object_Id=" + ddlObjectname.SelectedValue + " ");
            if (dtexcelHeader.Rows.Count > 0)
            {
                ddlOperationType.SelectedValue = dtexcelHeader.Rows[0]["Operation_Type"].ToString().Trim();
                ddlConsistency.SelectedValue = dtexcelHeader.Rows[0]["Error_Action"].ToString().Trim();
                ddlForeingExcetion.SelectedValue = dtexcelHeader.Rows[0]["Consistency_Action"].ToString().Trim();
                ddlTableName.SelectedValue = dtexcelHeader.Rows[0]["object_id"].ToString().Trim();
                ddlTableName_OnSelectedIndexChanged(null, null);
                BindGridView(ddlObjectname.SelectedValue.Trim());
            }
        }
    }
    protected void ddlTableName_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlFieldname.Items.Clear();
        if (ddlTableName.SelectedIndex > 0)
        {
            fillFieldname(ddlTableName.SelectedValue, ddlFieldname);
        }
    }
    protected void ddlFieldname_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txtFieldCaption.Text = "";
        txtSortOrder.Text = "0";
        ResetDetail();
        if (ddlFieldname.SelectedIndex > 0)
        {
            txtFieldCaption.Text = ddlFieldname.SelectedValue.Trim();
            txtSortOrder.Text = (gvExcelConfig.Rows.Count + 1).ToString();
            //heer we will get all saved setting for this field if exists
            DataTable dtDetail = objDa.return_DataTable("select IT_Object_ExcelConfig_Detail.*,sys.tables.object_id as FObject_Id from IT_Object_ExcelConfig_Header inner join IT_Object_ExcelConfig_Detail on IT_Object_ExcelConfig_Header.Trans_Id= IT_Object_ExcelConfig_Detail.Header_Id     left join sys.tables on IT_Object_ExcelConfig_Detail.Foreign_Table= sys.tables.name where IT_Object_ExcelConfig_Header.Object_Id=" + ddlObjectname.SelectedValue + " and IT_Object_ExcelConfig_Detail.Trans_Id='" + ddlFieldname.SelectedValue.Trim() + "'");
            if (dtDetail.Rows.Count > 0)
            {
                //ddlFieldType.SelectedValue = dtDetail.Rows[0]["Field_Type"].ToString();
                txtFieldCaption.Text = dtDetail.Rows[0]["Field_Caption_User"].ToString().Trim();
                chkIsRequired.Checked = Convert.ToBoolean(dtDetail.Rows[0]["Is_Required_User"].ToString().Trim());
                if (Convert.ToBoolean(dtDetail.Rows[0]["Is_Required"].ToString().Trim()))
                {
                    chkIsRequired.Enabled = false;
                    chkIsVisible.Enabled = false;
                }
                else
                {
                    chkIsRequired.Enabled = true;
                    chkIsVisible.Enabled = true;
                }
                txtSortOrder.Text = dtDetail.Rows[0]["Sort_Order_User"].ToString().Trim();
                chkIsVisible.Checked = Convert.ToBoolean(dtDetail.Rows[0]["Is_Visible_User"].ToString().Trim());
                chkIsDuplicaate.Checked = Convert.ToBoolean(dtDetail.Rows[0]["Is_Duplicate_User"].ToString().Trim());
            }
        }
    }
    #endregion
    #region saveandreset
    protected void btnDefaultSetting_OnClick(object sender, EventArgs e)
    {
        if (ddlObjectname.SelectedIndex <= 0)
        {
            DisplayMessage("Select Object Name");
            ddlObjectname.Focus();
            return;
        }
        int b = objDa.execute_Command("UPDATE [IT_Object_ExcelConfig_Detail] SET [Field_Caption_User] = [Field_Caption],[Is_Required_User] =[Is_Required] ,[Sort_Order_User] =[Sort_Order],[Is_Visible_User] =[Is_Visible] ,[Is_Duplicate_User] =[Is_Duplicate]  WHERE Is_Visible='True'  and  Header_id in (select IT_Object_ExcelConfig_Header.Trans_Id from IT_Object_ExcelConfig_Header where IT_Object_ExcelConfig_Header.Object_Id= " + ddlObjectname.SelectedValue.Trim() + ")");
        DisplayMessage(b.ToString() + " record affected");
        BindGridView(ddlObjectname.SelectedValue.Trim());
    }
    protected void btnsave_OnClick(object sender, EventArgs e)
    {
        if (ddlObjectname.Items == null)
        {
            DisplayMessage("Select Object Name");
            ddlObjectname.Focus();
            return;
        }
        else
        {
            if (ddlObjectname.SelectedIndex <= 0)
            {
                DisplayMessage("Select Object Name");
                ddlObjectname.Focus();
                return;
            }
        }
        if (ddlTableName.SelectedIndex <= 0)
        {
            DisplayMessage("Select Table Name");
            ddlTableName.Focus();
            return;
        }
        //heer we are checking that caption name is already exist or not 
        string strHeaderId = string.Empty;
        string strTransId = string.Empty;
        if (objDa.return_DataTable("select * from IT_Object_ExcelConfig_Header where object_id=" + ddlObjectname.SelectedValue + "").Rows.Count == 0)
        {
            //insert in header
            objDa.execute_Command("INSERT INTO  [IT_Object_ExcelConfig_Header] ([Object_Id] ,[Operation_Type] ,[Error_Message] ,[Error_Action] ,[Base_Table],[Consistency_Action]) VALUES ('" + ddlObjectname.SelectedValue + "' ,'" + ddlOperationType.SelectedValue + "',' ' ,'" + ddlConsistency.SelectedValue + "' ,'" + ddlTableName.SelectedItem.Text.Trim() + "','" + ddlForeingExcetion.SelectedValue + "')");
        }
        else
        {
            //update
            objDa.execute_Command("UPDATE [IT_Object_ExcelConfig_Header] SET [Operation_Type] ='" + ddlOperationType.SelectedValue + "' ,[Error_Message] =' ' ,[Error_Action] ='" + ddlConsistency.SelectedValue + "' ,[Base_Table] ='" + ddlTableName.SelectedItem.Text.Trim() + "',[Consistency_Action]='" + ddlForeingExcetion.SelectedValue.Trim() + "' WHERE Object_Id=" + ddlObjectname.SelectedValue + "");
        }
        strHeaderId = objDa.return_DataTable("select * from IT_Object_ExcelConfig_Header where object_id=" + ddlObjectname.SelectedValue + "").Rows[0]["Trans_Id"].ToString();
        if (ddlFieldname.SelectedIndex > 0)
        {
            //heer we are checking that caption name already exist or not 
            if (objDa.return_DataTable("select IT_Object_ExcelConfig_Detail.Field_Caption_User  from IT_Object_ExcelConfig_Header inner join IT_Object_ExcelConfig_Detail on IT_Object_ExcelConfig_Header.Trans_Id= IT_Object_ExcelConfig_Detail.Header_Id where IT_Object_ExcelConfig_Header.Object_Id=" + ddlObjectname.SelectedValue + " and IT_Object_ExcelConfig_Detail.Field_Caption_User='" + txtFieldCaption.Text.Trim() + "' and  IT_Object_ExcelConfig_Detail.Trans_Id<>" + ddlFieldname.SelectedValue.Trim() + " and IT_Object_ExcelConfig_Detail.Is_Visible='True'").Rows.Count > 0)
            {
                DisplayMessage("Field Caption should be unique");
                txtFieldCaption.Focus();
                return;
            }
            objDa.execute_Command("UPDATE [IT_Object_ExcelConfig_Detail] SET [Field_Caption_User] = '" + txtFieldCaption.Text + "',[Is_Required_User] ='" + chkIsRequired.Checked.ToString() + "' ,[Sort_Order_User] ='" + txtSortOrder.Text.Trim() + "',[Is_Visible_User] ='" + chkIsVisible.Checked.ToString() + "' ,[Is_Duplicate_User] ='" + chkIsDuplicaate.Checked.ToString() + "' WHERE Trans_Id=" + ddlFieldname.SelectedValue.Trim() + "");
        }
        ResetDetail();
        ddlFieldname.SelectedIndex = 0;
        DisplayMessage("Record Updated Successfully", "green");
    }
    public void BindGridView(string strObjectId)
    {
        DataTable DtGridView = objDa.return_DataTable("select IT_Object_ExcelConfig_Detail.Field_Caption_User as Field_Caption,IT_Object_ExcelConfig_Detail.Sort_Order_User as Sorting_Order,IT_Object_ExcelConfig_Detail.Is_Required_User as Is_Required,IT_Object_ExcelConfig_Detail.is_visible_user as Is_Visible ,IT_Object_ExcelConfig_Detail.is_duplicate_user as Is_Duplicate   from IT_Object_ExcelConfig_Header inner join IT_Object_ExcelConfig_Detail on IT_Object_ExcelConfig_Header.Trans_Id= IT_Object_ExcelConfig_Detail.Header_Id where IT_Object_ExcelConfig_Header.Object_Id=" + strObjectId + " and IT_Object_ExcelConfig_Detail.Is_Visible='True'  order by IT_Object_ExcelConfig_Detail.sort_order_User");
        objPageCmn.FillData((object)gvExcelConfig, DtGridView, "", "");
    }
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    protected void btnreset_OnClick(object sender, EventArgs e)
    {
        ResetDetail();
        try
        {
            ddlFieldname.SelectedIndex = 0;
        }
        catch
        {
        }
    }
    public void ResetDetail()
    {
        txtFieldCaption.Text = "";
        chkIsRequired.Checked = false;
        txtSortOrder.Text = "";
        chkIsDuplicaate.Checked = false;
        GridView1.DataSource = null;
        GridView1.DataBind();
        chkIsVisible.Checked = false;
        if (ddlObjectname.SelectedValue.Trim() != "--Select--")
            BindGridView(ddlObjectname.SelectedValue.Trim());
    }
    #endregion
    #region uploadanddownloadFunctionality
    protected void btnDemo_OnClick(object sender, EventArgs e)
    {
        if (ddlObjectname.SelectedIndex == 0)
        {
            DisplayMessage("Please Select Object Name");
        }
        else
        {
            DataTable DtExport = new DataTable();
            DataTable dt = objDa.return_DataTable("select IT_Object_ExcelConfig_Detail.Field_Caption_User, IT_Object_ExcelConfig_Detail.Field_Type,IT_Object_ExcelConfig_Detail.Suggested_Value from IT_Object_ExcelConfig_Header inner join IT_Object_ExcelConfig_Detail on IT_Object_ExcelConfig_Header.Trans_Id= IT_Object_ExcelConfig_Detail.Header_Id where IT_Object_ExcelConfig_Header.Object_Id=" + ddlObjectname.SelectedValue + " and IT_Object_ExcelConfig_Detail.Is_Visible_User='True'  order by IT_Object_ExcelConfig_Detail.Sort_Order_User");
            foreach (DataRow dr in dt.Rows)
            {
                Addcolumn(DtExport, dr["Field_Caption_User"].ToString(), dr["Field_Type"].ToString());
            }
            ExportTableData(CreateDemoData(DtExport, dt, 5));
        }
    }
    public DataTable Addcolumn(DataTable dt, string Caption, string dataType)
    {
        DataColumn DC = new DataColumn();
        DC.ColumnName = Caption;
        if (dataType == "Number")
        {
            DC.DataType = typeof(Int32);
        }
        if (dataType == "Decimal")
        {
            DC.DataType = typeof(float);
        }
        if (dataType == "String")
        {
            DC.DataType = typeof(string);
        }
        if (dataType == "DateTime" || dataType == "Date")
        {
            DC.DataType = typeof(DateTime);
        }
        if (dataType == "Boolean")
        {
            DC.DataType = typeof(bool);
        }
        dt.Columns.Add(DC);
        return dt;
    }
    public DataTable CreateDemoData(DataTable dt, DataTable dtExport, int RowsNo)
    {
        DataTable dtTemp = new DataTable();
        dtTemp.Columns.Add("ColumnName");
        dtTemp.Columns.Add("Counter", typeof(int));
        int counter = 0;
        for (int i = 0; i < RowsNo; i++)
        {
            DataRow dr = dt.NewRow();
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                if (dt.Columns[j].DataType == typeof(Int32) || dt.Columns[j].DataType == typeof(float))
                {
                    dr[j] = i + 1;
                }
                if (dt.Columns[j].DataType == typeof(string))
                {
                    dr[j] = dt.Columns[j].ColumnName + "_" + (i + 1).ToString();
                    DataTable dtSuggestedvalue = new DataView(dtExport, "Field_Caption_User='" + dt.Columns[j].ColumnName.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtSuggestedvalue.Rows[0]["Suggested_Value"].ToString() != "")
                    {
                        if (dtTemp == null)
                        {
                            dtTemp.Rows[0]["ColumnName"] = dt.Columns[j].ColumnName.ToString();
                            dtTemp.Rows[0]["Counter"] = 0;
                        }
                        if (new DataView(dtTemp, "ColumnName='" + dt.Columns[j].ColumnName.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count == 0)
                        {
                            DataRow drNew = dtTemp.NewRow();
                            drNew[0] = dt.Columns[j].ColumnName.ToString();
                            drNew[1] = 0;
                            dtTemp.Rows.Add(drNew);
                        }
                        for (int k = 0; k < dtTemp.Rows.Count; k++)
                        {
                            if (dtTemp.Rows[k][0].ToString() == dt.Columns[j].ColumnName.ToString())
                            {
                                if (Convert.ToInt32(dtTemp.Rows[k][1].ToString()) >= dtSuggestedvalue.Rows[0]["Suggested_Value"].ToString().Split('|').Length)
                                {
                                    dtTemp.Rows[k][1] = 0;
                                }
                                dr[j] = dtSuggestedvalue.Rows[0]["Suggested_Value"].ToString().Split('|')[Convert.ToInt32(dtTemp.Rows[k][1].ToString())].ToString().Split('-')[0].ToString();
                                dtTemp.Rows[k][1] = Convert.ToInt32(dtTemp.Rows[k][1].ToString()) + 1;
                                break;
                            }
                        }
                    }
                }
                if (dt.Columns[j].DataType == typeof(DateTime))
                {
                    dr[j] = DateTime.Now.ToString();
                }
                if (dt.Columns[j].DataType == typeof(bool))
                {
                    dr[j] = true;
                }
            }
            dt.Rows.Add(dr);
        }
        return dt;
    }
    public void ExportTableData(DataTable dtdata)
    {
        Response.ClearContent();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "" + ddlObjectname.SelectedItem.Text + ".xls"));
        Response.ContentType = "application/ms-excel";
        DataTable dt = dtdata.Copy();
        string str = string.Empty;
        foreach (DataColumn dtcol in dt.Columns)
        {
            Response.Write(str + dtcol.ColumnName);
            str = "\t";
        }
        Response.Write("\n");
        foreach (DataRow dr in dt.Rows)
        {
            str = "";
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                Response.Write(str + Convert.ToString(dr[j]));
                str = "\t";
            }
            Response.Write("\n");
        }
        Response.End();
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        Session["dtInsertSheet"] = null;
        if (ddlObjectname.SelectedIndex <= 0)
        {
            DisplayMessage("Select object Name");
            ddlObjectname.Focus();
            return;
        }
        if (fileLoad.HasFile)
        {
            fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
            string Path = Server.MapPath("~/Temp/" + fileLoad.FileName);
            DataTable dt = ConvetExcelToDataTable(Path);
            //dt =    dt.DefaultView.ToTable(true, "Dep_Code", "Department_Name", "Dep_Name_L");
            //ExportTableData(dt);
            DataTable dtFinalSheet = InsertRecord(dt);
            Session["dtInsertSheet"] = dtFinalSheet;
            dt.Columns.Add("Is Valid", typeof(string));
            dt.Columns.Add("Error Message", typeof(string));
            dt.AcceptChanges();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 117)
                {

                }

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].ColumnName == "Is Valid" || dt.Columns[j].ColumnName == "Error Message" || dt.Rows[i]["Is Valid"].ToString().Trim() == "False")
                    {
                        continue;
                    }
                    dt.Rows[i]["Is Valid"] = checkFieldContraints(dt.Columns[j].ColumnName, dt.Rows[i][j].ToString())[0].ToString();
                    dt.Rows[i]["Error Message"] = checkFieldContraints(dt.Columns[j].ColumnName, dt.Rows[i][j].ToString())[1].ToString();
                }
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        else
        {
            DisplayMessage("File Not Found");
            return;
        }
    }
    protected void btnFinalSheet_click(object sender, EventArgs e)
    {
        if (GridView1.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }
        ExportTableData((DataTable)Session["dtInsertSheet"]);
    }
    public DataTable CreateDatatable(DataTable dt)
    {
        DataTable dtCelllDetail = objDa.return_DataTable("select IT_Object_ExcelConfig_Detail.Field_Name from IT_Object_ExcelConfig_Header inner join IT_Object_ExcelConfig_Detail on IT_Object_ExcelConfig_Header.Trans_Id= IT_Object_ExcelConfig_Detail.Header_Id where IT_Object_ExcelConfig_Header.Object_Id=" + ddlObjectname.SelectedValue + "  order by IT_Object_ExcelConfig_Detail.Sort_Order");
        for (int i = 0; i < dtCelllDetail.Rows.Count; i++)
        {
            DataColumn DC = new DataColumn();
            DC.ColumnName = dtCelllDetail.Rows[i]["Field_Name"].ToString();
            DC.DataType = typeof(string);
            dt.Columns.Add(DC);
        }
        DataColumn DC1 = new DataColumn();
        DC1.ColumnName = "Is_Valid";
        DC1.DataType = typeof(bool);
        dt.Columns.Add(DC1);
        DataColumn DC2 = new DataColumn();
        DC2.ColumnName = "Is_Consistent";
        DC2.DataType = typeof(bool);
        dt.Columns.Add(DC2);
        return dt;
    }
    public DataTable InsertRecord(DataTable dtuploadExcel)
    {
        DataTable dtCelllDetail = objDa.return_DataTable("select IT_Object_ExcelConfig_Detail.* from IT_Object_ExcelConfig_Header inner join IT_Object_ExcelConfig_Detail on IT_Object_ExcelConfig_Header.Trans_Id= IT_Object_ExcelConfig_Detail.Header_Id where IT_Object_ExcelConfig_Header.Object_Id=" + ddlObjectname.SelectedValue + "  order by IT_Object_ExcelConfig_Detail.Sort_Order");
        DataTable dt = new DataTable();
        CreateDatatable(dt);
        for (int i = 0; i < dtuploadExcel.Rows.Count; i++)
        {
            DataRow dr = dt.NewRow();
            dr[dtCelllDetail.Rows.Count + 1] = "True";
            for (int k = 0; k < dtCelllDetail.Rows.Count; k++)
            {
                if (!Convert.ToBoolean(dtCelllDetail.Rows[k]["Is_Visible_User"].ToString()))
                {
                    if (dtCelllDetail.Rows[k]["Auto_Update_Source"].ToString() != "")
                    {
                        if (dtCelllDetail.Rows[k]["Auto_Update_Variable"].ToString().Trim() == "CompId")
                        {
                            dr[k] = Session["CompId"].ToString();
                        }
                        if (dtCelllDetail.Rows[k]["Auto_Update_Variable"].ToString().Trim() == "BrandId")
                        {
                            dr[k] = Session["BrandId"].ToString();
                        }
                        if (dtCelllDetail.Rows[k]["Auto_Update_Variable"].ToString().Trim() == "LocId")
                        {
                            dr[k] = Session["LocId"].ToString();
                        }
                        if (dtCelllDetail.Rows[k]["Auto_Update_Variable"].ToString().Trim() == "UserId")
                        {
                            dr[k] = Session["UserId"].ToString();
                        }
                    }
                    else if ((dtCelllDetail.Rows[k]["Field_Type"].ToString().Trim() == "Date" || dtCelllDetail.Rows[k]["Field_Type"].ToString().Trim() == "DateTime") && dtCelllDetail.Rows[k]["Default_Value"].ToString().Trim() == "")
                    {
                        dr[k] = DateTime.Now.ToString();
                    }
                    else
                    {
                        dr[k] = dtCelllDetail.Rows[k]["Default_Value"].ToString().Trim();
                    }
                    if (dr[dtCelllDetail.Rows.Count].ToString() == "" || dr[dtCelllDetail.Rows.Count].ToString() == "True")
                    {
                        dr[dtCelllDetail.Rows.Count] = "True";
                    }
                }
                else
                {
                    //entry for user side 
                    for (int j = 0; j < dtuploadExcel.Columns.Count; j++)
                    {
                        if (dtCelllDetail.Rows[k]["Field_Caption_User"].ToString().Trim() == dtuploadExcel.Columns[j].ColumnName.Trim())
                        {
                            //if foreing key then we will get data from foreing table 
                            if (Convert.ToBoolean(dtCelllDetail.Rows[k]["Is_Foreign_Key"].ToString().Trim()))
                            {
                                dr[k] = GetForeignValue(dtCelllDetail.Rows[k]["Foreign_Table"].ToString().Trim(), dtCelllDetail.Rows[k]["Foreign_Key_Field"].ToString().Trim(), dtCelllDetail.Rows[k]["Foreign_Value_Field"].ToString().Trim(), dtuploadExcel.Rows[i][j].ToString());
                                if (dr[dtCelllDetail.Rows.Count + 1].ToString() == "True")
                                {
                                    if (dr[k].ToString().Trim() == "" && Convert.ToBoolean(dtCelllDetail.Rows[k]["Is_Required_User"].ToString().Trim()) == true)
                                    {
                                        dr[dtCelllDetail.Rows.Count + 1] = "False";
                                    }
                                }
                                if (dr[k].ToString().Trim() == "")
                                {
                                    dr[k] = dtCelllDetail.Rows[k]["Default_Value"].ToString().Trim();
                                }
                            }
                            else
                            {
                                dr[k] = dtuploadExcel.Rows[i][j].ToString();
                            }
                            if (dr[dtCelllDetail.Rows.Count].ToString() == "" || dr[dtCelllDetail.Rows.Count].ToString() == "True")
                            {
                                dr[dtCelllDetail.Rows.Count] = checkFieldContraints(dtuploadExcel.Columns[j].ColumnName.Trim(), dtuploadExcel.Rows[i][j].ToString())[0].ToString();
                            }
                            break;
                        }
                    }
                }
            }
            dt.Rows.Add(dr);
        }
        return dt;
    }
    public DataTable ConvetExcelToDataTable(string path)
    {
        DataTable dt = new DataTable();
        string strcon = string.Empty;
        if (Path.GetExtension(path) == ".xls")
        {
            //strcon = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", path);
            strcon = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 8.0;HDR=YES;iMEX=1\"";
        }
        else if (Path.GetExtension(path) == ".xlsx")
        {
            //strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 12.0;HDR=YES;iMEX=1\"";
            strcon = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + @"'" + path + "';" + "Extended Properties=\"Excel 12.0;HDR=YES;iMEX=1\"";
        }
        try
        {
            OleDbConnection oledbConn = new OleDbConnection(strcon);
            oledbConn.Open();
            DataTable Sheets = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string strquery = "select * from [" + Sheets.Rows[0]["Table_Name"].ToString() + "] ";
            //string strquery = " select ProductCode, count(*) from (   select ProductCode,PhysicalQuantity from [" + Sheets.Rows[0]["Table_Name"].ToString() + "] where PhysicalQuantity<>0 group by  ProductId,ProductCode,PhysicalQuantity having count(PhysicalQuantity)>1 ) tb group by ProductCode having count(*)>1";
            OleDbCommand com = new OleDbCommand(strquery, oledbConn);
            DataSet ds = new DataSet();
            OleDbDataAdapter oledbda = new OleDbDataAdapter(com);
            oledbda.Fill(ds, Sheets.Rows[0]["Table_Name"].ToString());
            oledbConn.Close();
            dt = ds.Tables[0];
        }
        catch
        {
            DisplayMessage("Excel Format should be in .xlsx");
        }
        return dt;
    }
    public string[] checkFieldContraints(string captionName, string strValue)
    {
        string[] objArr = new string[2];
        objArr[0] = "True";
        objArr[1] = "";
        //fisrt  we will check that caption name is exist or not 
        DataTable dtConfigDetail = objDa.return_DataTable("select IT_Object_ExcelConfig_Detail.* from IT_Object_ExcelConfig_Header inner join IT_Object_ExcelConfig_Detail on IT_Object_ExcelConfig_Header.Trans_Id= IT_Object_ExcelConfig_Detail.Header_Id where IT_Object_ExcelConfig_Header.Object_Id=" + ddlObjectname.SelectedValue + " and Field_caption_User='" + captionName.Trim() + "' ");
        // DataTable dtConfigDetail = objDa.return_DataTable("select * from IT_Object_ExcelConfig_Detail where Field_caption_User='" + captionName.Trim() + "'");
        if (dtConfigDetail.Rows.Count == 0)
        {
            objArr[0] = "False";
            objArr[1] = "Field Missmatch";
        }
        //if (Convert.ToBoolean(dtConfigDetail.Rows[0]["Is_Foreign_Key"].ToString().Trim()) && Convert.ToBoolean(dtConfigDetail.Rows[0]["Is_Required_User"].ToString().Trim()))
        //{
        if (Convert.ToBoolean(dtConfigDetail.Rows[0]["Is_Foreign_Key"].ToString().Trim()) && strValue.Trim() != "")
        {
            if (GetForeignValue(dtConfigDetail.Rows[0]["Foreign_Table"].ToString().Trim(), dtConfigDetail.Rows[0]["Foreign_Key_Field"].ToString().Trim(), dtConfigDetail.Rows[0]["Foreign_Value_Field"].ToString().Trim(), strValue).Trim() == "")
            {
                objArr[0] = "False";
                objArr[1] = "Consistency Issue";
            }
        }
        if (Convert.ToBoolean(dtConfigDetail.Rows[0]["Is_Required_User"].ToString().Trim()))
        {
            if (strValue.Trim() == "")
            {
                objArr[0] = "False";
                objArr[1] = "Required Field is blank";
            }
        }
        if (dtConfigDetail.Rows[0]["Suggested_Value"].ToString().Trim() != "")
        {
            if (strValue.Trim() == "")
            {
                objArr[0] = "False";
                objArr[1] = "Suggested Value  Missmatch";
            }
            else
            {
                string[] str = dtConfigDetail.Rows[0]["Suggested_Value"].ToString().Trim().Split('|');
                bool ValidVal = false;
                foreach (string strval in str)
                {
                    if (strval.Split('-')[0].Contains(strValue))
                    {
                        ValidVal = true;
                        break;
                    }
                }
                if (!ValidVal)
                {
                    objArr[0] = "False";
                    objArr[1] = "Suggested Value  Missmatch";
                }
            }
        }
        //here we are checking datatype isssue
        if (!checkdataTypeContraints(dtConfigDetail.Rows[0]["Field_Type"].ToString().Trim(), strValue))
        {
            objArr[0] = "False";
            objArr[1] = "Datat Type Conversion Issue";
        }
        //here we are checking 
        if (objArr[0].ToString() == "False")
        {
            objArr[1] = objArr[1] + " For " + captionName;
        }
        return objArr;
    }
    public bool checkdataTypeContraints(string dataType, string strValue)
    {
        bool result = false;
        try
        {
            if (dataType == "Number")
            {
                Convert.ToInt32(strValue);
            }
            if (dataType == "Decimal")
            {
                Convert.ToDouble(strValue);
            }
            if (dataType == "DateTime" || dataType == "Date")
            {
                Convert.ToDateTime(strValue);
            }
            if (dataType == "Boolean")
            {
                Convert.ToBoolean(strValue);
            }
            result = true;
        }
        catch
        {
        }
        return result;
    }
    public string GetForeignValue(string strtablename, string strKeyField, string strValueField, string strvalue)
    {
        string strval = string.Empty;
        string strsql = "";
        if (strvalue.Trim() != "")
        {
            strsql = "select " + strKeyField + " from " + strtablename + " where " + strValueField + " = " + "N'" + strvalue.Replace("'", "") + "'";
            if (objDa.return_DataTable(strsql).Rows.Count > 0)
            {
                strval = objDa.return_DataTable(strsql).Rows[0][0].ToString();
            }
        }
        return strval;
    }
    protected void btnInsert_click(object sender, EventArgs e)
    {

        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();

        DataTable dtFinance = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", Session["CompId"].ToString(),HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        int FinancialYearMonth = 0;
        if (dtFinance.Rows.Count > 0)
        {
            FinancialYearMonth = int.Parse(dtFinance.Rows[0]["Param_Value"].ToString());
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


        string Normal_OT_Method = string.Empty;
        string Assign_Min = string.Empty;
        string Is_OverTime = string.Empty;
        string Effective_Work_Cal_Method = string.Empty;
        string Is_Partial_Enable = string.Empty;
        string Partial_Leave_Mins = string.Empty;
        string Partial_Leave_Day = string.Empty;
        string Field1 = string.Empty;
        string Field2 = string.Empty;
        string Field12 = string.Empty;
        string Field8 = string.Empty;
        string Field9 = string.Empty;
        string Field10 = string.Empty;
        string strGender = string.Empty;
        string strLabourLaw = string.Empty;
        try
        {
            Assign_Min = objAppParam.GetApplicationParameterValueByParamName("Work Day Min", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        catch
        {
            Assign_Min = "540";
        }
        try
        {
            Effective_Work_Cal_Method = objAppParam.GetApplicationParameterValueByParamName("Effective Work Calculation Method", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        catch
        {
            Effective_Work_Cal_Method = "InOut";
        }
        try
        {
            Is_OverTime = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())).ToString();
        }
        catch
        {
            Is_OverTime = false.ToString();
        }
        try
        {
            Normal_OT_Method = objAppParam.GetApplicationParameterValueByParamName("Over Time Calculation Method", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        catch
        {
            Normal_OT_Method = "Work Hour";
        }
        try
        {
            Is_Partial_Enable = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_Leave_Enable", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())).ToString();
        }
        catch
        {
            Is_Partial_Enable = false.ToString();
        }
        try
        {
            Partial_Leave_Mins = objAppParam.GetApplicationParameterValueByParamName("Total Partial Leave Minutes", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        catch
        {
            Partial_Leave_Mins = "240";
        }
        try
        {
            Partial_Leave_Day = objAppParam.GetApplicationParameterValueByParamName("Partial Leave Minute Use In A Day", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        catch
        {
            Partial_Leave_Day = "60";
        }
        try
        {
            Field1 = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Is_Late_Penalty", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())).ToString();
        }
        catch
        {
            Field1 = false.ToString();
        }
        try
        {
            Field2 = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Is_Early_Penalty", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString())).ToString();
        }
        catch
        {
            Field2 = false.ToString();
        }
        try
        {
            Field12 = objAppParam.GetApplicationParameterValueByParamName("Half_Day_Count", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        }
        catch
        {
        }
        Field8 = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Duration", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        Field9 = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Fresher_From", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        Field10 = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Fresher_To", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
        bool IsIndemnity = false;
        int IndemnityYear = 0;
        int indenmitydays = 10;
        try
        {
            IsIndemnity = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsIndemnity", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {
        }
        try
        {
            IndemnityYear = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("IndemnityYear", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {
        }
        try
        {
            indenmitydays = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("IndemnityDayas", Session["CompId"].ToString(), HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString()));
        }
        catch
        {
        }
        DataTable dtNf = objEmpNotice.GetAllNotification_By_NOtificationType("Report");
        SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
        if (GridView1.Rows.Count == 0)
        {
            DisplayMessage("Record Not Found");
            return;
        }
        DataTable dtEmployee = new DataTable();
        DataTable dt = (DataTable)Session["dtInsertSheet"];
        string strConsistencyAction = string.Empty;
        string strBaseTable = string.Empty;
        string strForeignException = string.Empty;
        //heere we are checking the user parameter for selected page or object
        DataTable dtexcelHeader = objDa.return_DataTable("select IT_Object_ExcelConfig_Header.Operation_Type,IT_Object_ExcelConfig_Header.Error_Action,IT_Object_ExcelConfig_Header.Consistency_Action,IT_Object_ExcelConfig_Header.Base_Table,IT_Object_ExcelConfig_Header.Error_Message,sys.tables.object_id from IT_Object_ExcelConfig_Header  inner join sys.tables on IT_Object_ExcelConfig_Header.base_table= sys.tables.name  WHERE IT_Object_ExcelConfig_Header.Object_Id=" + ddlObjectname.SelectedValue + "");
        //if selcted action is   skip then we will skip error  record and allow remaining
        strConsistencyAction = dtexcelHeader.Rows[0]["Error_Action"].ToString().Trim();
        strBaseTable = dtexcelHeader.Rows[0]["Base_Table"].ToString().Trim();
        strForeignException = dtexcelHeader.Rows[0]["Consistency_Action"].ToString().Trim();
        if (strConsistencyAction == "Rollback")
        {
            if (new DataView((DataTable)Session["dtInsertSheet"], "Is_Valid='False'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            {
                DisplayMessage("InValid Record Found , you can not proceed");
                return;
            }
        }
        //here we are checking that  duplicaate record found or not 
        //here we can check  that record is duplicate or not according the user configureation 
        DataTable dColumndetail = objDa.return_DataTable("select IT_Object_ExcelConfig_Detail.* from IT_Object_ExcelConfig_Header inner join IT_Object_ExcelConfig_Detail on IT_Object_ExcelConfig_Header.Trans_Id= IT_Object_ExcelConfig_Detail.Header_Id where IT_Object_ExcelConfig_Header.Object_Id=" + ddlObjectname.SelectedValue + "  order by IT_Object_ExcelConfig_Detail.Sort_Order");
        string strDuplicateRecord = string.Empty;
        string strDuplicateCaption = string.Empty;
        for (int j = 0; j < dt.Rows.Count; j++)
        {
            strDuplicateRecord = "";
            strDuplicateCaption = "Duplicate record found in sheet for ";
            for (int k = 0; k < dColumndetail.Rows.Count; k++)
            {
                if (!Convert.ToBoolean(dColumndetail.Rows[k]["Is_Duplicate_User"].ToString()))
                {
                    if (Convert.ToBoolean(dColumndetail.Rows[k]["Is_Visible_User"].ToString()))
                    {
                        strDuplicateCaption += dColumndetail.Rows[k]["Field_Caption_User"].ToString() + " = " + "" + dt.Rows[j][k].ToString().Replace("'", "") + "" + " , ";
                    }
                    if (strDuplicateRecord.Trim() == "")
                    {
                        strDuplicateRecord = dColumndetail.Rows[k]["Field_Name"].ToString() + " = " + "'" + dt.Rows[j][k].ToString().Replace("'", "") + "'";
                    }
                    else
                    {
                        strDuplicateRecord = strDuplicateRecord + " and " + dColumndetail.Rows[k]["Field_Name"].ToString() + " = " + "'" + dt.Rows[j][k].ToString().Replace("'", "") + "'";
                    }
                }
            }
            if (strDuplicateRecord.Trim() != "")
            {
                string strsql = "select count(*) from " + strBaseTable + " where " + strDuplicateRecord;
                //checking in database
                if (objDa.return_DataTable(strsql).Rows[0][0].ToString() != "0")
                {
                    //dt.Rows[j]["Is_Valid"] = "False";
                    if (strConsistencyAction == "Rollback")
                    {
                        DisplayMessage("Duplicate record found in database ,you can check in sheet at  Row no. =" + (j + 1).ToString());
                        return;
                    }
                }
                //then we will check in uploaded sheet
                DataTable DtTemp = new DataView(dt, strDuplicateRecord, "", DataViewRowState.CurrentRows).ToTable();
                if (DtTemp == null)
                {
                    continue;
                }
                if (DtTemp.Rows.Count > 1)
                {
                    //dt.Rows[j]["Is_Valid"] = "False";
                    if (strConsistencyAction == "Rollback")
                    {
                        DisplayMessage(strDuplicateCaption);
                        return;
                    }
                }
            }
        }

        DataTable dtdevice = Objdevice.GetDeviceMaster(Session["CompId"].ToString());
        DataTable dtdevice1 = new DataTable();
        double maxid = 0;
        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();
        try
        {
            string strColumnName = string.Empty;
            strColumnName = "Insert Into " + strBaseTable + " ( ";
            DataTable dtCelllDetail = objDa.return_DataTable("select IT_Object_ExcelConfig_Detail.Field_Name,IT_Object_ExcelConfig_Detail.Field_Type,IT_Object_ExcelConfig_Detail.Is_Foreign_Key,IT_Object_ExcelConfig_Detail.Field_Caption_User,IT_Object_ExcelConfig_Detail.Is_Required_User , IT_Object_ExcelConfig_Detail.Is_Duplicate_User,IT_Object_ExcelConfig_Detail.Is_Visible_User from IT_Object_ExcelConfig_Header inner join IT_Object_ExcelConfig_Detail on IT_Object_ExcelConfig_Header.Trans_Id= IT_Object_ExcelConfig_Detail.Header_Id where IT_Object_ExcelConfig_Header.Object_Id=" + ddlObjectname.SelectedValue + "  order by IT_Object_ExcelConfig_Detail.Sort_Order", ref trns);
            for (int i = 0; i < dtCelllDetail.Rows.Count; i++)
            {
                strColumnName += dtCelllDetail.Rows[i]["Field_Name"].ToString() + ",";
            }
            strColumnName = strColumnName.Substring(0, strColumnName.Length - 1) + " ) "; ;
            string strvalueField = "";
            int counter = 0;
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                strDuplicateRecord = "";
                if (strConsistencyAction.Trim() == "Skip" && dt.Rows[j]["Is_Valid"].ToString() == "False")
                {
                    continue;
                }
                strvalueField = " Values ( ";
                for (int k = 0; k < dtCelllDetail.Rows.Count; k++)
                {
                    if (strConsistencyAction.Trim() == "Skip")
                    {
                        if (!Convert.ToBoolean(dtCelllDetail.Rows[k]["Is_Duplicate_User"].ToString()))
                        {
                            if (strDuplicateRecord.Trim() == "")
                            {
                                strDuplicateRecord = dtCelllDetail.Rows[k]["Field_Name"].ToString() + " = " + "'" + dt.Rows[j][k].ToString().Replace("'", "") + "'";
                            }
                            else
                            {
                                //strDuplicateRecord = dtCelllDetail + " and " + dColumndetail.Rows[k]["Field_Name"].ToString() + " = " + "'" + dt.Rows[j][k].ToString().Replace("'", "") + "'";
                                strDuplicateRecord = strDuplicateRecord + " and " + dColumndetail.Rows[k]["Field_Name"].ToString() + " = " + "'" + dt.Rows[j][k].ToString().Replace("'", "") + "'";
                            }
                        }
                    }
                    if (dtCelllDetail.Rows[k]["Field_Type"].ToString().Trim() == "String")
                    {
                        strvalueField += "N'" + dt.Rows[j][k].ToString().Replace("'", "") + "'" + ",";
                    }
                    else
                    {
                        strvalueField += "'" + dt.Rows[j][k].ToString().Replace("'", "") + "'" + ",";
                    }
                }
                if (strConsistencyAction.Trim() == "Skip")
                {
                    if (strDuplicateRecord.Trim() != "")
                    {
                        string strsql = "select count(*) from " + strBaseTable + " where " + strDuplicateRecord;
                        //checking in database
                        if (objDa.return_DataTable(strsql, ref trns).Rows[0][0].ToString() != "0")
                        {
                            continue;
                        }
                    }
                }
                strvalueField = strvalueField.Substring(0, strvalueField.Length - 1) + " )  ";
                objDa.execute_Command(strColumnName + strvalueField, ref trns);
                //if base table is set_employeemaster then we will insert default parameter for employee
                if (ddlTableName.SelectedItem.Text.ToUpper() == "SET_EMPLOYEEMASTER")
                {
                    maxid = Convert.ToDouble(objDa.return_DataTable("select max(emp_id) from set_employeemaster", ref trns).Rows[0][0].ToString());
                    dtEmployee = objDa.return_DataTable("select Emp_Code,Field4,DOJ,Brand_Id,Location_Id,Gender from set_employeemaster where emp_id=" + maxid.ToString() + "", ref trns);
                    if (dtEmployee.Rows.Count > 0)
                    {

                        if (dtEmployee.Rows[0]["Field4"].ToString().Trim() != "0" && dtEmployee.Rows[0]["Field4"].ToString().Trim() != "")
                        {
                            CreateUser(maxid.ToString(), dtEmployee.Rows[0]["Brand_Id"].ToString(), dtEmployee.Rows[0]["Location_Id"].ToString(), dtEmployee.Rows[0]["Emp_Code"].ToString(), dtEmployee.Rows[0]["Field4"].ToString(), ref trns);
                            objDa.execute_Command("update set_employeemaster set Field4='' where emp_id=" + maxid.ToString() + "", ref trns);
                        }
                        objEmp.InsertEmployeeLocationTransfer(maxid.ToString(), Session["LocId"].ToString(), Session["LocId"].ToString(), DateTime.Now.ToString(), "Employee Created", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), ref trns);
                        int Indemnity = objIndemnity.InsertIndemnityRecord("0", Session["CompId"].ToString(), maxid.ToString(), Convert.ToDateTime(dtEmployee.Rows[0]["DOJ"].ToString()).AddYears(IndemnityYear).ToString(), "Pending", "", "", "", "", "", "", "", "", "", "", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        SystemLog.SaveSystemLog("Employee Master", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Employee Saved", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        InsertEmployeeParameterOnEmployeeInsert(Session["CompId"].ToString(), maxid.ToString(), Convert.ToDateTime(dtEmployee.Rows[0]["DOJ"].ToString()), Normal_OT_Method, Assign_Min, Is_OverTime, Effective_Work_Cal_Method, Is_Partial_Enable, Partial_Leave_Mins, Partial_Leave_Day, Field1, Field2, Field12, Field8, Field9, Field10, ref trns);
                        objEmpParam.InsertEmpParameterNew(maxid.ToString(), Session["CompId"].ToString(), "IsIndemnity", IsIndemnity.ToString(), "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        // Indemnity Duration
                        objEmpParam.InsertEmpParameterNew(maxid.ToString(), Session["CompId"].ToString(), "IndemnityYear", IndemnityYear.ToString(), "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        objEmpParam.InsertEmpParameterNew(maxid.ToString(), Session["CompId"].ToString(), "IndemnityDayas", indenmitydays.ToString(), "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        foreach (DataRow dr in dtNf.Rows)
                        {
                            try
                            {
                                objEmpNotice.InsertEmployeeNotification(maxid.ToString(), dr["Notification_Id"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                            }
                            catch
                            {
                            }
                        }


                        dtdevice1 = new DataView(dtdevice, "Location_Id=" + dtEmployee.Rows[0]["Location_Id"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

                        foreach (DataRow dr in dtdevice1.Rows)
                        {
                            objSer.InsertUserTransfer(maxid.ToString(), dr["Device_Id"].ToString(), false.ToString(), DateTime.Now.ToString(), "1/1/1900", "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
                        }

                        //inserting labour law


                        strLabourLaw = ObjLocationMaster.GetLocationMasterById(Session["CompId"].ToString(), dtEmployee.Rows[0]["Location_Id"].ToString()).Rows[0]["Field3"].ToString();


                        if (dtEmployee.Rows[0]["Gender"].ToString() == "M")
                        {
                            strGender = "Male";
                        }
                        else
                        {
                            strGender = "Female";
                        }

                        if (strLabourLaw.Trim() != "0" && strLabourLaw.Trim() != "")
                        {

                            DataTable dtleavedetail = ObjLabourLeavedetail.GetRecord_By_LaborLawId(strLabourLaw, ref trns);

                            dtleavedetail = new DataView(dtleavedetail, "Gender='" + strGender + "' or Gender='Both'", "", DataViewRowState.CurrentRows).ToTable();


                            foreach (DataRow dr in dtleavedetail.Rows)
                            {
                                objEmpleave.InsertEmployeeLeave(Session["CompId"].ToString(), maxid.ToString(), dr["Leave_Type_Id"].ToString(), dr["Total_Leave_days"].ToString(), dr["Paid_Leave_days"].ToString(), "100", dr["schedule_type"].ToString(), true.ToString(), dr["is_yearcarry"].ToString(), "", "", "", dr["is_rule"].ToString(), dr["is_auto"].ToString(), true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                                SaveLeave("No", dr["Leave_Type_Id"].ToString(), maxid.ToString(), dr["schedule_type"].ToString(), dr["Total_Leave_days"].ToString(), dr["Paid_Leave_days"].ToString(), dr["is_yearcarry"].ToString(), "", "", "", dr["is_rule"].ToString(), FinancialYearStartDate, FinancialYearEndDate, ref trns);

                                //here we are checking that any deduction slab exists or not for selected leave type

                                DataTable dtdeduction = ObjLeavededuction.GetRecordbyLeaveTypeId(dr["Leave_Type_Id"].ToString(), ref trns).DefaultView.ToTable(true, "Trans_Id", "DaysFrom", "Daysto", "Deduction_Percentage");

                                foreach (DataRow childrow in dtdeduction.Rows)
                                {
                                    ObjLeavededuction.InsertRecord(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), dr["Leave_Type_Id"].ToString(), maxid.ToString(), childrow["DaysFrom"].ToString(), childrow["Daysto"].ToString(), childrow["Deduction_Percentage"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                                }
                            }
                        }


                    }
                }
                else if (ddlTableName.SelectedItem.Text.ToUpper() == "INV_PRODUCTMASTER")
                {
                    maxid = Convert.ToDouble(objDa.return_DataTable("Select max(ProductId) from Inv_ProductMaster", ref trns).Rows[0][0].ToString());
                    ObjCompanyBrand.InsertProductCompanyBrand(Session["CompId"].ToString(), maxid.ToString(), Session["BrandId"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                }
                else if (ddlTableName.SelectedItem.Text.ToUpper() == "EMS_CONTACTMASTER")
                {
                    maxid = Convert.ToDouble(objDa.return_DataTable("Select max(Trans_Id) from Ems_ContactMaster", ref trns).Rows[0][0].ToString());
                    EMS_CompanyBrand.InsertContactCompanyBrand(maxid.ToString(), Session["CompId"].ToString(), Session["BrandId"].ToString(), "True", Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString());
                }
                else if (ddlTableName.SelectedItem.Text.ToUpper() == "SET_LOCATIONMASTER")
                {
                    maxid = Convert.ToDouble(objDa.return_DataTable("Select max(Location_Id) from Set_LocationMaster", ref trns).Rows[0][0].ToString());
                    string FinanceId = ObjFinance.GetInfoByStatus(Session["CompId"].ToString(), ref trns).Rows[0]["Trans_Id"].ToString();
                    ObjFinancedetail.InsertFinancialYearDetail(Session["CompId"].ToString(), Session["BrandId"].ToString(), maxid.ToString(), FinanceId, "Open", "", "Open", "", DateTime.Now.ToString(), "", "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), Session["userId"].ToString(), DateTime.Now.ToString(), ref trns);
                    DataTable dtParam = objAppParam.GetApplicationParameterByCompanyId("", Session["CompId"].ToString(), ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                    dtParam = new DataView(dtParam, "Brand_Id='" + Session["BrandId"].ToString() + "'  and Location_Id='" + Session["LocId"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtParam.Rows.Count == 0)
                    {
                        dtParam = objAppParam.GetApplicationParameterByCompanyId("", "1", ref trns, HttpContext.Current.Session["BrandId"].ToString(), HttpContext.Current.Session["LocId"].ToString());
                        dtParam = new DataView(dtParam, "Brand_Id='1'  and Location_Id='1'", "", DataViewRowState.CurrentRows).ToTable();
                    }
                    if (dtParam.Rows.Count > 0)
                    {
                        string strFinancialMonth = string.Empty;
                        string strworkdayMinute = string.Empty;
                        string strYearlyHalfDay = string.Empty;
                        string strweekoffday = string.Empty;
                        DataTable dtLabourlaw = ObjLabourLaw.GetRecordbyTRans_Id(Session["CompId"].ToString(), "0", ref trns);
                        if (dtLabourlaw.Rows.Count > 0)
                        {
                            strFinancialMonth = dtLabourlaw.Rows[0]["fy_start_month"].ToString();
                            strworkdayMinute = dtLabourlaw.Rows[0]["work_day_minutes"].ToString();
                            strYearlyHalfDay = dtLabourlaw.Rows[0]["yearly_halfday"].ToString();
                            strweekoffday = dtLabourlaw.Rows[0]["week_off_day"].ToString();
                        }
                        string strParamValue = string.Empty;
                        for (int i = 0; i < dtParam.Rows.Count; i++)
                        {
                            if (dtParam.Rows[i]["Param_Name"].ToString().Trim() == "FinancialYearStartMonth")
                            {
                                strParamValue = strFinancialMonth;
                            }
                            else if (dtParam.Rows[i]["Param_Name"].ToString().Trim() == "Work Day Min")
                            {
                                strParamValue = strworkdayMinute;
                            }
                            else if (dtParam.Rows[i]["Param_Name"].ToString().Trim() == "Half_Day_Count")
                            {
                                strParamValue = strYearlyHalfDay;
                            }
                            else if (dtParam.Rows[i]["Param_Name"].ToString().Trim() == "Week Off Days")
                            {
                                strParamValue = strweekoffday;
                            }
                            else
                            {
                                strParamValue = dtParam.Rows[i]["Param_Value"].ToString();
                            }
                            objAppParam.InsertApplicationParameterMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), maxid.ToString(), dtParam.Rows[i]["Param_Name"].ToString(), strParamValue, dtParam.Rows[i]["Param_Cat_Id"].ToString(), dtParam.Rows[i]["Description"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                    DataTable dtInvParam = objInvParam.GetParameterMasterAllTrue(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), ref trns);
                    if (dtInvParam.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtInvParam.Rows.Count; i++)
                        {
                            objInvParam.InsertParameterMaster(Session["CompId"].ToString(), Session["BrandId"].ToString(), maxid.ToString(), dtInvParam.Rows[i]["ParameterName"].ToString(), dtInvParam.Rows[i]["ParameterValue"].ToString(), dtInvParam.Rows[i]["Field1"].ToString(), dtInvParam.Rows[i]["Field2"].ToString(), dtInvParam.Rows[i]["Field3"].ToString(), dtInvParam.Rows[i]["Field4"].ToString(), dtInvParam.Rows[i]["Field5"].ToString(), dtInvParam.Rows[i]["Field6"].ToString(), dtInvParam.Rows[i]["Field7"].ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
                        }
                    }
                }
                counter++;
            }
            DisplayMessage(counter.ToString() + " Records Inserted successfully");
            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();
            ResetDetail();
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


    public void SaveLeave(string Edit, string LeaveTypeId, string EmpId, string SchType, string AssignLeave, string PaidLeave, string IsYearCarry, string PrevSchduleType, string PrevAssignLeave, string TransNo, string IsRule, DateTime FinancialYearStartDate, DateTime FinancialYearEndDate, ref SqlTransaction trns)
    {


        //code commneted by jitendra on 24-05-2017

        //new code wrote for proper leave assigning according date of joining

        /// means employee joined in january but hr forgot to assign leave in that case system assigning leave according current month instead of joining month and year 


        //code start

        DateTime dtJoining = new DateTime();
        DataTable dtEmp = objEmp.GetEmployeeMasterById(Session["CompId"].ToString(), EmpId, ref trns);
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

                objEmpleave.DeleteEmployeeLeaveTransByEmpIdandleaveTypeId(EmpId, LeaveTypeId, DateTime.Now.Month.ToString(), FinancialYearStartDate.Year.ToString(), ref trns);

            }


            objEmpleave.InsertEmployeeLeaveTrans(Session["CompId"].ToString(), EmpId, LeaveTypeId, FinancialYearStartDate.Year.ToString(), DateTime.Now.Month.ToString(), "0", TotalAssignLeave.ToString(), TotalAssignLeave.ToString(), "0", TotalAssignLeave.ToString(), "0", TotalpaidLeave.ToString(), TotalpaidLeave.ToString(), "Open", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
        }
        else
        {

            int totalLogPostedCount = Convert.ToInt32(objDa.return_DataTable("SELECT COUNT(*) from pay_employee_attendance where Emp_Id='" + EmpId + "' and DATEADD(year, pay_employee_attendance.year-1900, DATEADD(month, pay_employee_attendance.Month-1, DATEADD(day, 1-1, 0)))>='" + FinancialYearStartDate + "' and DATEADD(year, pay_employee_attendance.year-1900, DATEADD(month, pay_employee_attendance.Month-1, DATEADD(day, 1-1, 0)))<='" + FinancialYearEndDate + "'", ref trns).Rows[0][0].ToString());

            double Remainingdays = 0;



            Remainingdays = ((Convert.ToDouble(AssignLeave) / 12) * totalLogPostedCount);


            if (Remainingdays.ToString().Contains("."))
            {
                Remainingdays = Convert.ToDouble(Remainingdays.ToString().Split('.')[0].ToString());
            }



            if (TransNo.Trim() != "")
            {

                objEmpleave.DeleteEmployeeLeaveTransByEmpIdandleaveTypeId(EmpId, LeaveTypeId, "0", FinancialYearStartDate.Year.ToString(), ref trns);

            }

            if (Convert.ToBoolean(IsRule) == true)
            {
                objEmpleave.InsertEmployeeLeaveTrans(Session["CompId"].ToString(), EmpId, LeaveTypeId, FinancialYearStartDate.Year.ToString(), "0", "0", TotalAssignLeave.ToString(), TotalAssignLeave.ToString(), "0", Remainingdays.ToString(), "0", TotalpaidLeave.ToString(), TotalpaidLeave.ToString(), "Open", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }
            else
            {
                objEmpleave.InsertEmployeeLeaveTrans(Session["CompId"].ToString(), EmpId, LeaveTypeId, FinancialYearStartDate.Year.ToString(), "0", "0", TotalAssignLeave.ToString(), TotalAssignLeave.ToString(), "0", TotalAssignLeave.ToString(), "0", TotalpaidLeave.ToString(), TotalpaidLeave.ToString(), "Open", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
            }
        }

        ///SystemLog.SaveSystemLog("Employee Master : Leave", DateTime.Now.ToString(), Session["CompId"].ToString(), Session["UserId"].ToString(), "Full Day Leave Saved", "", "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
    }


    public void InsertEmployeeParameterOnEmployeeInsert(string CompanyId, string Emp_Id, DateTime JoinDate, string Normal_OT_Method, string Assign_Min, string Is_OverTime, string Effective_Work_Cal_Method, string Is_Partial_Enable, string Partial_Leave_Mins, string Partial_Leave_Day, string Field1, string Field2, string Field12, string Field8, string Field9, string Field10, ref SqlTransaction trns)
    {
        string Basic_Salary = "0";
        string Salary_Type = "Monthly";
        string Currency_Id = Session["CurrencyId"].ToString();
        string Normal_OT_Type = "2";
        string Normal_OT_Value = "100";
        string Normal_HOT_Type = "2";
        string Normal_HOT_Value = "100";
        string Normal_WOT_Type = "2";
        string Normal_WOT_Value = "100";
        string Is_Partial_Carry = false.ToString();
        string Field3 = string.Empty;
        Field3 = true.ToString();
        string Field4 = false.ToString();
        string Field5 = false.ToString();
        string Field6 = false.ToString();
        string Field7 = DateTime.Now.ToString();
        string Field11 = string.Empty;
        Field11 = "Fresher";
        double IncrementPer = 0;
        try
        {
            IncrementPer = double.Parse(Field10);
        }
        catch
        {
        }
        double BasicSal = 0;
        double IncrementValue = 0;
        try
        {
            IncrementValue = (BasicSal * IncrementPer) / 100;
        }
        catch
        {
        }
        double IncrementSalary = 0;
        int Duration = 0;
        try
        {
            Duration = int.Parse(Field8);
        }
        catch
        {
        }
        IncrementSalary = BasicSal + IncrementValue;
        DateTime IncrementDate = JoinDate.AddMonths(Duration);
        objEmpSalInc.DeleteEmpSalaryIncrementByEmpId(Emp_Id, ref trns);
        //objEmpSalInc.InsertEmpSalaryIncrement(Session["CompId"].ToString(), hdnEmpIdSal.Value, BasicSal.ToString(), ddlCategory1.SelectedValue, IncrementDate.Month.ToString(), IncrementDate.Year.ToString(), txtIncrementPerTo1.Text, IncrementValue.ToString(), IncrementSalary.ToString(), Duration.ToString(), txtIncrementPerFrom1.Text, txtIncrementPerTo1.Text, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
        objEmpSalInc.InsertEmpSalaryIncrement(HttpContext.Current.Session["CompId"].ToString(), Emp_Id, BasicSal.ToString(), "Fresher", IncrementDate.Month.ToString(), IncrementDate.Year.ToString(), Field10, IncrementValue.ToString(), IncrementSalary.ToString(), Duration.ToString(), Field9, Field10, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), HttpContext.Current.Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);
        objEmpParam.InsertEmployeeParameter(Session["CompId"].ToString(), Emp_Id, "0", Salary_Type, Currency_Id, Assign_Min, Effective_Work_Cal_Method, Is_OverTime, Normal_OT_Method, Normal_OT_Type, Normal_OT_Value, Normal_HOT_Type, Normal_HOT_Value, Normal_WOT_Type, Normal_WOT_Value, Is_Partial_Enable, Partial_Leave_Mins, Partial_Leave_Day, Is_Partial_Carry, Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10, Field11, Field12, true.ToString(), "", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "0", true.ToString(), "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", ref trns, "0");
    }
    public void CreateUser(string strEmpId, string strbrandId, string strLocationId, string strEmpCode, string strRoleId, ref SqlTransaction trns)
    {
        DataTable dt = new DataTable();
        string strsql = string.Empty;
        strsql = "select emp_id from set_usermaster where emp_id=" + strEmpId + "";
        dt = objDa.return_DataTable(strsql);
        if (dt.Rows.Count == 0)
        {
            ObjUser.InsertUserMaster(Session["CompId"].ToString(), strEmpCode, Common.Encrypt(strEmpCode), strEmpId, strRoleId, false.ToString(), "0", "", "", "1", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), "false", ref trns);
            objUserDataPerm.InsertUserDataPermission(strEmpCode, Session["CompId"].ToString(), "C", Session["CompId"].ToString(), "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objUserDataPerm.InsertUserDataPermission(strEmpCode, Session["CompId"].ToString(), "B", strbrandId, "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());
            objUserDataPerm.InsertUserDataPermission(strEmpCode, Session["CompId"].ToString(), "L", strLocationId, "0", true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        }
    }
    #endregion
    protected void FUExcel_FileUploadComplete(object sender, EventArgs e)
    {
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
                fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
            }
        }
    }
}