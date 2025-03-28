using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PegasusDataAccess;
using System.Data;

public partial class ITSetUp_ExcelConfiguration : System.Web.UI.Page
{
    DataAccessClass objDa = null;
    Common cmn = null;
    ObjectMaster objObject = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        cmn = new Common(Session["DBConnection"].ToString());
        objObject = new ObjectMaster(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            FillObjectname();
        }
    }

    public void FillObjectname()
    {
        DataTable dt = objDa.return_DataTable("select distinct IT_ObjectEntry.object_name,IT_ObjectEntry.object_id from  IT_ObjectEntry inner join IT_Object_Table_Info on IT_ObjectEntry.Object_Id= IT_Object_Table_Info.Object_Id where IT_ObjectEntry.IsActive='True' ");
        objPageCmn.FillData((object)ddlObjectname, dt, "Object_Name", "Object_Id");
    }

    public void FillTableName(DropDownList ddl)
    {
        DataTable dt = objDa.return_DataTable("select sys.tables.name ,sys.tables.object_id from IT_Object_Table_Info inner join sys.tables on IT_Object_Table_Info.Table_name=sys.tables.name where IT_Object_Table_Info.object_id=" + ddlObjectname.SelectedValue.Trim() + "");
        objPageCmn.FillData((object)ddl, dt, "name", "Object_Id");
    }
    
    public void fillFieldname(string strTableObjectId, DropDownList ddl)
    {
        DataTable dt = objDa.return_DataTable("select name from sys.columns where object_id=" + strTableObjectId + " order by column_id");
        ddl.DataSource = dt;
        ddl.DataTextField = "name";
        ddl.DataValueField = "name";
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
            DataTable dtexcelHeader = objDa.return_DataTable("select IT_Object_ExcelConfig_Header.Operation_Type,IT_Object_ExcelConfig_Header.Error_Action,IT_Object_ExcelConfig_Header.Consistency_Action,IT_Object_ExcelConfig_Header.Base_Table,IT_Object_ExcelConfig_Header.Error_Message,sys.tables.object_id from IT_Object_ExcelConfig_Header  inner join sys.tables on IT_Object_ExcelConfig_Header.base_table= sys.tables.name  WHERE IT_Object_ExcelConfig_Header.Object_Id=" + ddlObjectname.SelectedValue + "");
            if (dtexcelHeader.Rows.Count > 0)
            {
                ddlOperationType.SelectedValue = dtexcelHeader.Rows[0]["Operation_Type"].ToString().Trim();
                ddlConsistency.SelectedValue = dtexcelHeader.Rows[0]["Error_Action"].ToString().Trim();
                ddlForeingExcetion.SelectedValue = dtexcelHeader.Rows[0]["Consistency_Action"].ToString().Trim();
                ddlTableName.SelectedValue = dtexcelHeader.Rows[0]["object_id"].ToString().Trim();
                ddlTableName_OnSelectedIndexChanged(null, null);
                DataTable dtDetail = objDa.return_DataTable("select IT_Object_ExcelConfig_Detail.* from IT_Object_ExcelConfig_Header inner join IT_Object_ExcelConfig_Detail on IT_Object_ExcelConfig_Header.Trans_Id= IT_Object_ExcelConfig_Detail.Header_Id where IT_Object_ExcelConfig_Header.Object_Id=" + ddlObjectname.SelectedValue + " order by IT_Object_ExcelConfig_Detail.sort_order");
                objPageCmn.FillData((object)gvExcelConfig, dtDetail, "", "");
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
            DataTable dtDetail = objDa.return_DataTable("select IT_Object_ExcelConfig_Detail.*,sys.tables.object_id as FObject_Id from IT_Object_ExcelConfig_Header inner join IT_Object_ExcelConfig_Detail on IT_Object_ExcelConfig_Header.Trans_Id= IT_Object_ExcelConfig_Detail.Header_Id     left join sys.tables on IT_Object_ExcelConfig_Detail.Foreign_Table= sys.tables.name where IT_Object_ExcelConfig_Header.Object_Id=" + ddlObjectname.SelectedValue + " and IT_Object_ExcelConfig_Detail.Field_Name='" + ddlFieldname.SelectedItem.Text.Trim() + "'");
            if (dtDetail.Rows.Count > 0)
            {
                ddlFieldType.SelectedValue = dtDetail.Rows[0]["Field_Type"].ToString();
                txtFieldCaption.Text = dtDetail.Rows[0]["Field_Caption"].ToString().Trim();
                txtdefaultValue.Text = dtDetail.Rows[0]["Default_Value"].ToString().Trim();
                chkIsRequired.Checked = Convert.ToBoolean(dtDetail.Rows[0]["Is_Required"].ToString().Trim());
                txtSortOrder.Text = dtDetail.Rows[0]["Sort_Order"].ToString().Trim();
                chkIsForeignKey.Checked = Convert.ToBoolean(dtDetail.Rows[0]["Is_Foreign_Key"].ToString().Trim());
                if (chkIsForeignKey.Checked)
                {
                    chkIsForeignKey_OnCheckedChanged(null, null);
                    ddlForeignTable.SelectedValue = dtDetail.Rows[0]["FObject_Id"].ToString().Trim();
                    ddlForeignTable_OnSelectedIndexChanged(null, null);
                    ddlForeignKeyField.SelectedValue = dtDetail.Rows[0]["Foreign_Key_Field"].ToString().Trim();
                    ddlForeignValueField.SelectedValue = dtDetail.Rows[0]["Foreign_Value_Field"].ToString().Trim();
                }
                else
                {
                    ListItem Li = new ListItem();
                    Li.Text = "--Select---";
                    Li.Value = "";
                    ddlForeignTable.Items.Clear();
                    ddlForeignKeyField.Items.Clear();
                    ddlForeignValueField.Items.Clear();
                    ddlForeignTable.Items.Insert(0, Li);
                    ddlForeignKeyField.Items.Insert(0, Li);
                    ddlForeignValueField.Items.Insert(0, Li);
                }
                chkAutoInsert.Checked = Convert.ToBoolean(dtDetail.Rows[0]["Is_Auto_Insert"].ToString().Trim());
                chkAutoInsert_OnCheckedChanged(null, null);
                try
                {
                    ddlautoupdatesorce.SelectedValue = dtDetail.Rows[0]["Auto_Update_Source"].ToString().Trim();
                }
                catch
                {
                }
                try
                {
                    ddlautoupdateVariable.SelectedValue = dtDetail.Rows[0]["Auto_Update_Variable"].ToString().Trim();
                }
                catch
                {
                }
                chkIsVisible.Checked = Convert.ToBoolean(dtDetail.Rows[0]["Is_Visible"].ToString().Trim());
                chkIsDuplicaate.Checked = Convert.ToBoolean(dtDetail.Rows[0]["Is_Duplicate"].ToString().Trim());
                txtSuggestedValue.Text = dtDetail.Rows[0]["Suggested_Value"].ToString().Trim();
            }
        }
    }
    protected void chkIsForeignKey_OnCheckedChanged(object sender, EventArgs e)
    {
        if (ddlObjectname.SelectedIndex == 0)
        {
            if (chkIsForeignKey.Checked)
            {
                DisplayMessage("Please Select Object Name");
                chkIsForeignKey.Checked = false;
                ddlObjectname.Focus();
            }
        }
        else if (ddlFieldname.SelectedIndex == 0)
        {
            DisplayMessage("Please Select Field Name");
            ddlFieldname.Focus();
        }
        else
        {
            ddlForeignTable.Items.Clear();
            ddlForeignKeyField.Items.Clear();
            ddlForeignValueField.Items.Clear();

            if (chkIsForeignKey.Checked)
            {
                FillTableName(ddlForeignTable);
            }
        }
    }
    protected void ddlForeignTable_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlForeignKeyField.Items.Clear();
        ddlForeignValueField.Items.Clear();
        if (ddlForeignTable.SelectedIndex > 0)
        {
            fillFieldname(ddlForeignTable.SelectedValue, ddlForeignKeyField);
            fillFieldname(ddlForeignTable.SelectedValue, ddlForeignValueField);
        }
    }
    protected void chkAutoInsert_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkAutoInsert.Checked)
        {
            ddlautoupdatesorce.Enabled = true;
            ddlautoupdateVariable.Enabled = true;
        }
        else
        {
            ddlautoupdatesorce.Enabled = false;
            ddlautoupdateVariable.Enabled = false;
        }
    }
    #endregion
    #region saveandreset
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
            objDa.execute_Command("UPDATE [IT_Object_ExcelConfig_Header] SET [Operation_Type] ='" + ddlOperationType.SelectedValue + "' ,[Error_Message] =' ' ,[Error_Action] ='" + ddlConsistency.SelectedValue + "' ,[Base_Table] ='" + ddlTableName.SelectedItem.Text.Trim() + "',[Consistency_Action] ='" + ddlForeingExcetion.SelectedValue + "' WHERE Object_Id=" + ddlObjectname.SelectedValue + "");
        }
        strHeaderId = objDa.return_DataTable("select * from IT_Object_ExcelConfig_Header where object_id=" + ddlObjectname.SelectedValue + "").Rows[0]["Trans_Id"].ToString();
        if (ddlFieldname.SelectedIndex > 0)
        {
            //here we are checking that record exist  or not for sleected object if exisr then we will update otherwise inserty
            string strForeingTable = string.Empty;
            if (ddlForeignTable.SelectedIndex > 0)
            {
                strForeingTable = ddlForeignTable.SelectedItem.Text.Trim();
            }
            //here we are checking for detail section
            DataTable dtDetail = objDa.return_DataTable("select IT_Object_ExcelConfig_Detail.* from IT_Object_ExcelConfig_Header inner join IT_Object_ExcelConfig_Detail on IT_Object_ExcelConfig_Header.Trans_Id= IT_Object_ExcelConfig_Detail.Header_Id where IT_Object_ExcelConfig_Header.Object_Id=" + ddlObjectname.SelectedValue + " and IT_Object_ExcelConfig_Detail.Field_Name='" + ddlFieldname.SelectedItem.Text.Trim() + "'");
            if (dtDetail.Rows.Count == 0)
            {
                //insert in detail
                objDa.execute_Command("INSERT INTO  [IT_Object_ExcelConfig_Detail] ([Header_Id] ,[Field_Name] ,[Field_Type] ,[Field_Caption] ,[Field_Caption_User],[Default_Value] ,[Is_Required],[Is_Required_User] ,[Sort_Order] ,[Sort_Order_User],[Is_Foreign_Key] ,[Foreign_Table] ,[Foreign_Key_Field] ,[Foreign_Value_Field] ,[Is_Auto_Insert] ,[Auto_Update_Source] ,[Auto_Update_Variable] ,[Is_Visible] ,[Is_Visible_User],[Suggested_Value] ,[Is_Duplicate],[Is_Duplicate_User]) VALUES (" + strHeaderId + " ,'" + ddlFieldname.SelectedItem.Text + "' ,'" + ddlFieldType.SelectedItem.Text + "' , '" + txtFieldCaption.Text + "','" + txtFieldCaption.Text + "','" + txtdefaultValue.Text + "', '" + chkIsRequired.Checked.ToString() + "','" + chkIsRequired.Checked.ToString() + "','" + txtSortOrder.Text + "','" + txtSortOrder.Text + "' , '" + chkIsForeignKey.Checked.ToString() + "','" + strForeingTable + "' , '" + ddlForeignKeyField.SelectedValue.Trim() + "', '" + ddlForeignValueField.SelectedValue + "', '" + chkAutoInsert.Checked.ToString() + "', '" + ddlautoupdatesorce.SelectedValue + "', '" + ddlautoupdateVariable.SelectedValue + "','" + chkIsVisible.Checked.ToString() + "','" + chkIsVisible.Checked.ToString() + "'  ,'" + txtSuggestedValue.Text + "' ,'" + chkIsDuplicaate.Checked.ToString() + "','" + chkIsDuplicaate.Checked.ToString() + "')");
            }
            else
            {
                strTransId = dtDetail.Rows[0]["Trans_id"].ToString();
                //updaate in detail
                objDa.execute_Command("UPDATE [IT_Object_ExcelConfig_Detail] SET [Field_Name] = '" + ddlFieldname.SelectedValue.Trim() + "',[Field_Type] ='" + ddlFieldType.SelectedItem.Text + "' ,[Field_Caption] ='" + txtFieldCaption.Text.Trim() + "',[Field_Caption_User] ='" + txtFieldCaption.Text.Trim() + "' ,[Default_Value] ='" + txtdefaultValue.Text.Trim() + "' ,[Is_Required] ='" + chkIsRequired.Checked.ToString() + "',[Is_Required_User] ='" + chkIsRequired.Checked.ToString() + "' ,[Sort_Order] ='" + txtSortOrder.Text.Trim() + "',[Sort_Order_User] ='" + txtSortOrder.Text.Trim() + "' ,[Is_Foreign_Key] ='" + chkIsForeignKey.Checked.ToString() + "' ,[Foreign_Table] ='" + strForeingTable + "' ,[Foreign_Key_Field] = '" + ddlForeignKeyField.SelectedValue + "',[Foreign_Value_Field] ='" + ddlForeignValueField.SelectedValue + "' ,[Is_Auto_Insert] ='" + chkAutoInsert.Checked.ToString() + "' ,[Auto_Update_Source] ='" + ddlautoupdatesorce.SelectedValue + "' ,[Auto_Update_Variable] = '" + ddlautoupdateVariable.SelectedValue + "',[Is_Visible] ='" + chkIsVisible.Checked.ToString() + "',[Is_Visible_User] ='" + chkIsVisible.Checked.ToString() + "' ,[Suggested_Value] ='" + txtSuggestedValue.Text.Trim() + "' ,[Is_Duplicate] ='" + chkIsDuplicaate.Checked.ToString() + "',[Is_Duplicate_User] ='" + chkIsDuplicaate.Checked.ToString() + "' WHERE Trans_Id=" + strTransId + "");
            }
        }
        DataTable DtGridView = objDa.return_DataTable("select IT_Object_ExcelConfig_Detail.* from IT_Object_ExcelConfig_Header inner join IT_Object_ExcelConfig_Detail on IT_Object_ExcelConfig_Header.Trans_Id= IT_Object_ExcelConfig_Detail.Header_Id where IT_Object_ExcelConfig_Header.Object_Id=" + ddlObjectname.SelectedValue + " order by IT_Object_ExcelConfig_Detail.sort_order");
        objPageCmn.FillData((object)gvExcelConfig, DtGridView, "", "");
        ResetDetail();
        ddlFieldname.SelectedIndex = 0;
        DisplayMessage("Record Updated Successfully", "green");
    }
    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
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
        txtdefaultValue.Text = "";
        chkIsRequired.Checked = false;
        txtSortOrder.Text = "";
        chkIsForeignKey.Checked = false;
        ListItem Li = new ListItem();
        Li.Text = "--Select---";
        Li.Value = "";
        ddlForeignTable.Items.Clear();
        ddlForeignKeyField.Items.Clear();
        ddlForeignValueField.Items.Clear();
        ddlForeignTable.Items.Insert(0, Li);
        ddlForeignKeyField.Items.Insert(0, Li);
        ddlForeignValueField.Items.Insert(0, Li);
        chkAutoInsert.Checked = false;
        chkIsDuplicaate.Checked = false;
        ddlautoupdatesorce.SelectedIndex = 0;
        ddlautoupdateVariable.SelectedIndex = 0;
        chkIsVisible.Checked = false;
        txtSuggestedValue.Text = "";
        chkIsDuplicaate.Checked = true;
    }
    #endregion
}