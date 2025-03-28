using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using PegasusDataAccess;
using ClosedXML.Excel;
using System.IO;

public partial class Attendance_Deactivate_Employee : System.Web.UI.Page
{
    DataAccessClass Objda = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
        {
            Response.Redirect("~/ERPLogin.aspx");
        }
        Objda = new DataAccessClass(Session["DBConnection"].ToString());

        if (!IsPostBack)
        {
            
            Check_Page_Permission Chk_Page_ = new Check_Page_Permission(Session["DBConnection"].ToString());
            if (Chk_Page_.CheckPagePermission(Session["UserId"].ToString(), Common.GetObjectIdbyPageURL("../Attendance/Deactivate_Employee.aspx", Session["DBConnection"].ToString()), HttpContext.Current.Session["CompId"].ToString(), HttpContext.Current.Session["Application_Id"].ToString()).ToString() == "False")
            {
                Session.Abandon();
                Response.Redirect("~/ERPLogin.aspx");
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

    public void DisplayMessage(string str,string color="orange")
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "", "showAlert('" + str + "','"+color+"','white');", true);
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
        DataTable dtExcelupload = new DataTable();

        dtExcelupload.Columns.Add("Action_Type");
        dtExcelupload.Columns.Add("Effectivedate", typeof(DateTime));
        dtExcelupload.Columns.Add("Location");
        dtExcelupload.Columns.Add("Code");
        dtExcelupload.Columns.Add("Name");
        dtExcelupload.Columns.Add("Gender");
        dtExcelupload.Columns.Add("Dob",typeof(DateTime));
        dtExcelupload.Columns.Add("Doj", typeof(DateTime));
        dtExcelupload.Columns.Add("Department");
        dtExcelupload.Columns.Add("DeviceGroup");
        dtExcelupload.Columns.Add("Civil-id");
        dtExcelupload.Columns.Add("Email-Id");
        dtExcelupload.Columns.Add("Phone_No");
        dtExcelupload.Columns.Add("Designation");
        dtExcelupload.Columns.Add("Religion");
        dtExcelupload.Columns.Add("Qualification");
        dtExcelupload.Columns.Add("Nationality");
        dtExcelupload.Columns.Add("UserRole");
        div_upload_button.Visible = false;
        div_Record_count.Visible = false;
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
        DataTable dt = new DataTable();

        DataTable dtEmpList = new DataTable();
        string strEmpCodeList = string.Empty;

        string strsql = string.Empty;

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


                try
                {

                    if (dt.Columns.Contains("EMPLOYEE_NUMBER") && dt.Columns.Contains("NAME") && dt.Columns.Contains("HIRE_DATE") && dt.Columns.Contains("JOB_NAME") && dt.Columns.Contains("LOCATION") && dt.Columns.Contains("ORGANIZATION_NAME") && dt.Columns.Contains("DOB"))
                    {
                        dtExcelupload.Rows.Add("", null, "*", "*", "*", "*(M/F)", null, null, "*", "", "", "", "", "", "", "", "", "");

                        foreach (DataRow dr in dt.Rows)
                        {
                            strEmpCodeList += dr["EMPLOYEE_NUMBER"].ToString() + ",";


                            //generating excel sheet which is not exist in our database but exist  at client side

                            if (Objda.return_DataTable("select emp_code from set_employeemaster where emp_code='" + dr["EMPLOYEE_NUMBER"].ToString() + "'").Rows.Count == 0)
                            {
                                dtExcelupload.Rows.Add("New Hire/Update", DateTime.Now.Date.ToString("dd-MMM-yyyy"), dr["LOCATION"].ToString(), dr["EMPLOYEE_NUMBER"].ToString(), dr["NAME"].ToString(), "M", Convert.ToDateTime(dr["DOB"].ToString()).Date.ToString("dd-MMM-yyyy"), Convert.ToDateTime(dr["HIRE_DATE"].ToString()).Date.ToString("dd-MMM-yyyy"), dr["ORGANIZATION_NAME"].ToString(), "", "", "", "", dr["JOB_NAME"].ToString(), "", "", "", "");
                            }

                        }


                        //generating excel sheet which is exist in our database but not at client side
                        strsql = "select set_locationmaster.Location_Name,set_employeemaster.emp_id,set_departmentmaster.Dep_Name,Emp_code, emp_name,doj,set_designationmaster.designation from set_employeemaster left join set_locationmaster on set_employeemaster.location_id = set_locationmaster.location_id left join set_designationmaster on set_employeemaster.designation_id =set_designationmaster.designation_id left join set_departmentmaster on set_employeemaster.department_id = set_departmentmaster.Dep_Id    where (set_employeemaster.isactive='True' and set_employeemaster.Field2='False')  and emp_code  not in (" + strEmpCodeList.Substring(0, strEmpCodeList.Length - 1) + ") order by set_locationmaster.Location_Name,set_departmentmaster.Dep_Name,cast(emp_code as int)";
                        dtEmpList = Objda.return_DataTable(strsql);

                        foreach (DataRow dr in dtEmpList.Rows)
                        {
                            dtExcelupload.Rows.Add("Termination", DateTime.Now.Date.ToString("dd-MMM-yyyy"), dr["Location_Name"].ToString(), dr["Emp_code"].ToString(), dr["emp_name"].ToString(), "", null, null, "", "", "", "", "", "", "", "", "", "");
                        }



                        //generating excel sheet which is deactivated in our database but activated at client side
                        strsql = "select set_locationmaster.Location_Name,set_employeemaster.emp_id,set_departmentmaster.Dep_Name,Emp_code, emp_name,doj,set_designationmaster.designation from set_employeemaster left join set_locationmaster on set_employeemaster.location_id = set_locationmaster.location_id left join set_designationmaster on set_employeemaster.designation_id =set_designationmaster.designation_id left join set_departmentmaster on set_employeemaster.department_id = set_departmentmaster.Dep_Id    where (set_employeemaster.isactive='False' or set_employeemaster.Field2='True')  and emp_code   in (" + strEmpCodeList.Substring(0, strEmpCodeList.Length - 1) + ") order by set_locationmaster.Location_Name,set_departmentmaster.Dep_Name,cast(emp_code as int)";
                        dtEmpList = Objda.return_DataTable(strsql);

                        foreach (DataRow dr in dtEmpList.Rows)
                        {
                            dtExcelupload.Rows.Add("Reverse Termination", DateTime.Now.Date.ToString("dd-MMM-yyyy"), "", dr["Emp_code"].ToString(), dr["emp_name"].ToString(), "", null, null, "", "", "", "", "", "", "", "", "", "");
                        }

                        if (dtExcelupload.Rows.Count > 1)
                        {
                            div_upload_button.Visible = true;
                            div_Record_count.Visible = true;

                            lblTotalrecord.Text = "Total Records : " + dtExcelupload.Rows.Count.ToString();
                            gvEmpList.DataSource = dtExcelupload;
                            gvEmpList.DataBind();
                            Session["dtEmpList"] = dtExcelupload;
                        }
                        else
                        {
                            DisplayMessage("Record not found");
                            return;
                        }

                    }
                    else
                    {
                        DisplayMessage("Upload valid sheet");
                        return;
                    }
                }
                catch(Exception ex)
                {
                    DisplayMessage(ex.Message.ToString());
                    div_upload_button.Visible = false;
                    div_Record_count.Visible = false;
                    gvEmpList.DataSource = null;
                    gvEmpList.DataBind();
                    return;
                }


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


    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvrow in gvEmpList.Rows)
        {
            if (((CheckBox)gvrow.FindControl("chkgvselect")).Checked)
            {
                Objda.execute_Command("update set_employeemaster set isactive='False',ModifiedDate='" + DateTime.Now.ToString() + "' where emp_id=" + ((HiddenField)gvrow.FindControl("hdnempid")).Value + "");
            }

        }

        DisplayMessage("Record deactivated successfully");

    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ddlTables.Items.Clear();
        lblTotalrecord.Text = "";
        gvEmpList.DataSource = null;
        gvEmpList.DataBind();
        div_upload_button.Visible = false;
        div_Record_count.Visible = false;
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["dtEmpList"];
        //dt.Columns["Location_Name"].ColumnName = "Location";
        //dt.Columns["Dep_Name"].ColumnName = "Department";
        //dt.Columns["Emp_Code"].ColumnName = "Code";
        //dt.Columns["Emp_Name"].ColumnName = "Name";
        //dt.Columns["designation"].ColumnName = "Designation";

        //dt = dt.DefaultView.ToTable(true, "Location", "Department", "Code", "Name", "Designation");

        ExportTableData(dt);
    }


    public void ExportTableData(DataTable dtdata)
    {
        string strFname = "EmployeeList";
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

}