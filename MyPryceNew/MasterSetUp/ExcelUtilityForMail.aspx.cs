using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.OleDb;
using PegasusDataAccess;
using System.Collections.Generic;
using ClosedXML.Excel;
using System.Data.SqlClient;
using System.Diagnostics;

public partial class MasterSetUp_ExcelUtilityForMail : System.Web.UI.Page
{
    DataAccessClass objDa = null;
    LocationMaster ObjLocMaster = null;
    SystemParameter objSys = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        ObjLocMaster = new LocationMaster(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
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
    public void DisplayMessage(string str, string color = "orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','" + color + "','white');", true);
    }
    protected void btnConnect_Click(object sender, EventArgs e)
    {

        DataTable dtEmpList = objDa.return_DataTable("select  [Company_Id] ,[Brand_Id] ,[Location_Id] ,[Emp_Id] ,[Emp_Code] ,[Civil_Id] ,[Emp_Name] ,[Emp_Name_L] ,[Emp_Image] ,[Department_Id] ,[Designation_Id] ,[Religion_Id] ,[Nationality_Id] ,[Qualification_Id] ,[DOB] ,[DOJ] ,[Emp_Type] ,[Termination_Date] ,[Gender] ,[Email_Id] ,[Phone_No] ,[Field1] ,[Field2] ,[Field3] ,[Field4] ,[Field5] ,[Field6] ,[Field7] ,[IsActive] ,[CreatedBy] ,[CreatedDate] ,[ModifiedBy] ,[ModifiedDate] ,[company_phone_no] ,[Pan] ,[FatherName] ,[IsMarried] ,[DLNo] ,isNull( [Device_Group_Id],0) as Device_Group_Id  from set_employeemaster where Company_Id=" + Session["CompId"].ToString() + "");
        string strResult = string.Empty;
        DataTable dt = new DataTable();
        DataTable dtTemp = new DataTable();
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
                dt = ConvetExcelToDataTable(Path, ddlTables.SelectedValue.Trim());
                dt.AcceptChanges();
                if (dt.Rows.Count == 0)
                {
                    DisplayMessage("Record not found");
                    return;
                }

                string strField6 = "False";
                string strIsActive = "True";
                string strUserId = "3000";

                //dt = AddColumn(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][0].ToString() != "" && dt.Rows[i][0].ToString() != null)
                    {
                        string inputStr = dt.Rows[i][0].ToString();
                        string outputStr = inputStr.Trim(new char[] { (char)39 });

                        if (objDa.return_DataTable("select * from ES_EmailMaster_Header where Email_Id='" + dt.Rows[i][0].ToString() + "'").Rows.Count == 0)
                        {
                            objDa.execute_Command("INSERT INTO [dbo].[ES_EmailMaster_Header]([Email_Id],[Field1],[Field2],[Field3],[Field4],[Field5],[Field6],[Field7],[IsActive],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate])VALUES('" + dt.Rows[i][0].ToString() + "','" + dt.Rows[i][1].ToString() + "','','','','','" + strField6 + "','" + DateTime.Now + "','" + strIsActive + "','" + strUserId + "','" + DateTime.Now + "','" + strUserId + "','" + DateTime.Now + "')");
                        }
                    }
                }


                //uploadEmpdetail.Visible = true;
                // dtTemp = dt.DefaultView.ToTable(true, "Action_Type", "Effectivedate", "Location", "Code", "Name", "Gender", "Dob", "Doj", "Department", "DeviceGroup", "Civil-id", "Email-Id", "Phone_No", "Designation", "Religion", "Qualification", "Nationality", "UserRole", "ManagerCode", "IsValid");
                //gvSelected.DataSource = dtTemp;
                //gvSelected.DataBind();
                //lbltotaluploadRecord.Text = "Total Record : " + (dtTemp.Rows.Count - 1).ToString();
                //Session["UploadEmpDtAll"] = dt;
                //Session["UploadEmpDt"] = dtTemp;
                //rbtnupdall.Checked = true;
                //rbtnupdInValid.Checked = false;
                //rbtnupdValid.Checked = false;
            }
        }
        else
        {
            DisplayMessage("File Not Found");
            return;
        }
        dt.Dispose();
    }
    protected void btnGetSheet_Click(object sender, EventArgs e)
    {
        //if (txtuploadReferenceNo.Text == "")
        //{
        //    DisplayMessage("Configure Document number");
        //    return;
        //}
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
        if (dt.Columns.Contains("Action_Type") && dt.Columns.Contains("Effectivedate") && dt.Columns.Contains("Location") && dt.Columns.Contains("Code") && dt.Columns.Contains("Name") && dt.Columns.Contains("Civil-id") && dt.Columns.Contains("Gender") && dt.Columns.Contains("Email-Id") && dt.Columns.Contains("Dob") && dt.Columns.Contains("Doj") && dt.Columns.Contains("Department") && dt.Columns.Contains("Designation") && dt.Columns.Contains("Religion") && dt.Columns.Contains("Qualification") && dt.Columns.Contains("Nationality") && dt.Columns.Contains("UserRole") && dt.Columns.Contains("DeviceGroup"))
        {

        }
        else
        {
            Result = false;
        }
        return Result;
    }
    public DataTable AddColumn(DataTable dt)
    {
        dt.Columns.Add("Email_Id");
        dt.Columns.Add("Country_Id");
        return dt;
    }
    public string GetcolumnValue(string strtablename, string strKeyfieldname, string strKeyfieldvalue, string strKeyFieldResult)
    {
        string strResult = "0";
        strKeyfieldvalue = strKeyfieldvalue.Replace("'", "");
        DataTable dt = objDa.return_DataTable("select " + strKeyFieldResult + " from " + strtablename + " where " + strKeyfieldname + "=N'" + strKeyfieldvalue.Trim() + "' and IsActive='True'");
        if (dt.Rows.Count > 0)
        {
            strResult = dt.Rows[0][0].ToString();
        }
        return strResult;
    }
    public bool IsemployeeTerminated(string strEmpId)
    {
        bool result = false;
        DataTable dt = objDa.return_DataTable("select emp_id from Set_EmployeeMaster where company_id=" + Session["CompId"].ToString() + " and field2='True' and Emp_id=" + strEmpId + "");
        if (dt.Rows.Count > 0)
        {
            result = true;
        }
        return result;
    }
    public string CheckDatevalidation(string strDateval)
    {
        string strDate = string.Empty;
        DateTime dt = new DateTime();
        int day = 0;
        int Month = 0;
        int year = 0;
        string strYear = string.Empty;
        try
        {
            strDateval = Convert.ToDateTime(strDateval).ToShortDateString();
            day = Convert.ToInt32(strDateval.ToString().Split('-')[0]);
            //Month = getMonthNumber(strDateval.ToString().Split('-')[1]);
            year = Convert.ToInt32(strDateval.ToString().Split('-')[2]);
            dt = new DateTime(year, Month, day);
            strDate = dt.ToShortDateString().ToString();
        }
        catch (Exception ex)
        {
            try
            {

                day = Convert.ToInt32(strDateval.ToString().Split('/')[0]);
                Month = Convert.ToInt32(strDateval.ToString().Split('/')[1]);
                year = Convert.ToInt32(strDateval.ToString().Split('/')[2]);
                dt = new DateTime(year, Month, day);
                strDate = dt.ToShortDateString().ToString();
            }
            catch
            {
                strDate = Convert.ToDateTime(strDateval).ToString(objSys.SetDateFormat());
            }
        }
        return strDate;
    }

}