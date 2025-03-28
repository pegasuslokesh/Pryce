using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Data.SqlClient;
using PegasusDataAccess;

public partial class MasterSetUp_Leave_Opening_Balance : BasePage
{

    Common cmn = null;
    Att_Employee_Leave objEmpleave = null;
    EmployeeMaster ObjEmployee = null;
    LeaveMaster Objleave = null;
    DataAccessClass objDa = null;
    PageControlCommon objPageCmn = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        cmn = new Common(Session["DBConnection"].ToString());
        objEmpleave = new Att_Employee_Leave(Session["DBConnection"].ToString());
        ObjEmployee = new EmployeeMaster(Session["DBConnection"].ToString());
        Objleave = new LeaveMaster(Session["DBConnection"].ToString());
        objDa = new DataAccessClass(Session["DBConnection"].ToString());
        objPageCmn = new PageControlCommon(Session["DBConnection"].ToString());
        if (!IsPostBack)
        {
            Session["dtleaveOpeningBalance"] = null;
            fileLoad.Dispose();

        }
    }

    protected void Btn_upload_Click(object sender, EventArgs e)
    {
        string strEmpId = string.Empty;
        string strLeaveTypeId = string.Empty;

        DataTable dt = new DataTable();

        if (fileLoad.HasFile)
        {

            if (fileLoad.FileName.Contains("Shift"))
            {
                DisplayMessage("File not found");
                return;
            }

            fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
            string Path = Server.MapPath("~/Temp/" + fileLoad.FileName);
            dt = ConvetExcelToDataTable(Path);
            Session["dtleaveOpeningBalance"] = dt;
        }
        DataTable dtValid = dt.Clone();
        dtValid.Columns.Add("IsValid");
        DataTable dtInValid = dt.Clone();
        dtInValid.Columns.Add("IsValid");
        DataTable dtEmployee = new DataTable();
        DataTable dtleaveType = new DataTable();
        DataTable dtEmpLeaveTrans = new DataTable();
        DataTable dtEmpLeaveTrans1 = new DataTable();
        int LogPostedCount = 0;
        int maxLeaveBalace = 0;

        string stryear = string.Empty;
        for (int i = 0; i < dt.Rows.Count; i++)
        {



            try
            {

                dtEmployee = ObjEmployee.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), dt.Rows[i]["Code"].ToString().Trim());

                if (dtEmployee.Rows.Count <= 0)
                {
                    dtValid.ImportRow(dt.Rows[i]);
                    dtValid.Rows[i]["IsValid"] = "Invalid Employee Code";
                    continue;
                }
                else
                {
                    strEmpId = dtEmployee.Rows[0]["Emp_Id"].ToString();

                }


                dtleaveType = Objleave.GetLeaveMasterByLeaveName(Session["CompId"].ToString(), dt.Rows[i]["LeaveName"].ToString().Trim());

                if (dtleaveType.Rows.Count <= 0)
                {
                    dtValid.ImportRow(dt.Rows[i]);
                    dtValid.Rows[i]["IsValid"] = "Invalid Leave Type";
                    continue;
                }
                else
                {
                    strLeaveTypeId = dtleaveType.Rows[0]["Leave_Id"].ToString();
                }



                Convert.ToInt32(dt.Rows[i]["Year"].ToString());
                Convert.ToInt32(dt.Rows[i]["Previous_Leave"].ToString());
                Convert.ToInt32(dt.Rows[i]["Assign_Leave"].ToString());
                Convert.ToInt32(dt.Rows[i]["Total_Leave"].ToString());
                Convert.ToInt32(dt.Rows[i]["Paid_Leave"].ToString());
                Convert.ToInt32(dt.Rows[i]["total_leave_balance"].ToString());
                Convert.ToInt32(dt.Rows[i]["total_paid_leave_balance"].ToString());




                try
                {
                    maxLeaveBalace = int.Parse(objDa.return_DataTable("select Field4 from att_leavemaster where Leave_id=" + strLeaveTypeId + "").Rows[0][0].ToString());
                }
                catch
                {

                }



                if (maxLeaveBalace > 0)
                {

                    if (Convert.ToInt32(dt.Rows[i]["total_leave_balance"].ToString()) > maxLeaveBalace)
                    {
                        dtValid.ImportRow(dt.Rows[i]);
                        dtValid.Rows[i]["IsValid"] = "Leave balance is exceeded";
                        continue;
                    }

                }


                stryear = dt.Rows[i]["Year"].ToString();


                LogPostedCount = Convert.ToInt32(objDa.return_DataTable("select count(*) from pay_employee_attendance where emp_id=" + strEmpId + " and year='" + stryear + "' and isactive='True'").Rows[0][0].ToString());




                dtEmpLeaveTrans = objEmpleave.GetEmployeeLeaveTransactionDataByEmpId(strEmpId);

                dtEmpLeaveTrans1 = new DataView(dtEmpLeaveTrans, "Leave_Type_Id='" + strLeaveTypeId + "' and year='" + stryear + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtEmpLeaveTrans1.Rows.Count > 0)
                {
                    if (dtEmpLeaveTrans1.Rows[0]["Field3"].ToString() == "Close")
                    {
                        dtValid.ImportRow(dt.Rows[i]);
                        dtValid.Rows[i]["IsValid"] = "Financial year is already closed";
                        continue;
                    }
                    if (Convert.ToDouble(dtEmpLeaveTrans1.Rows[0]["Used_Days"].ToString()) > 0 || Convert.ToInt32(dtEmpLeaveTrans1.Rows[0]["Pending_Days"].ToString()) > 0)
                    {
                        dtValid.ImportRow(dt.Rows[i]);
                        dtValid.Rows[i]["IsValid"] = "Leave already used";
                        continue;
                    }
                    if (Convert.ToBoolean(dtEmpLeaveTrans1.Rows[0]["IsRule"].ToString()) && LogPostedCount > 0)
                    {
                        dtValid.ImportRow(dt.Rows[i]);
                        dtValid.Rows[i]["IsValid"] = "log posted for financial year";
                        continue;
                    }

                }


                dtValid.ImportRow(dt.Rows[i]);
                dtValid.Rows[i]["IsValid"] = "True";

            }
            catch (Exception ex)
            {
                dtValid.ImportRow(dt.Rows[i]);
                dtValid.Rows[i]["IsValid"] = ex.Message.ToString();
            }

        }


        Session["dtleaveOpeningBalance"] = dtValid;


        if (rbtnValid.Checked)
        {
            dtValid = new DataView(dtValid, "IsValid='True'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else if (rbtninValid.Checked)
        {
            dtValid = new DataView(dtValid, "IsValid<>'True'", "", DataViewRowState.CurrentRows).ToTable();
        }



        objPageCmn.FillData((object)gvValidEmployee, dtValid, "", "");
        lblvalidRecord.Text = "Total Record :" + dtValid.Rows.Count.ToString();
        fileLoad.Dispose();
        //objPageCmn.FillData((object)gvInvalidEmployee, dtInValid, "", "");
        //lblInvalidRecord.Text = "Total Invalid Record :" + dtInValid.Rows.Count.ToString();
    }

    protected void btn_Save_Click(object sender, EventArgs e)
    {
        DateTime dtTodayDate = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        SqlConnection con = new SqlConnection(HttpContext.Current.Session["DBConnection"].ToString());

        if (gvValidEmployee.Rows.Count == 0)
        {
            DisplayMessage("Record not found");
            return;
        }


        Btn_upload_Click(null, null);

        string stryear = string.Empty;
        string strPreviousLeave = string.Empty;
        string strAssignLeave = string.Empty;
        string strEmpCode = string.Empty;
        string strLeaveType = string.Empty;
        string strTotalLeave = string.Empty;
        string strPaidLeave = string.Empty;
        string strleaveBalance = string.Empty;
        string strpaidLeavebalance = string.Empty;
        string strIsRule = string.Empty;
        string strIsyearCarry = string.Empty;
        string strIsAuto = string.Empty;
        string strEmpId = string.Empty;
        string strLeaveTypeId = string.Empty;
        DataTable dtEmpLeave = objEmpleave.GetEmployeeLeaveByCompanyId(Session["CompId"].ToString());
        DataTable dtEmpLeave1 = new DataTable();
        int counter = 0;

        con.Open();
        SqlTransaction trns;
        trns = con.BeginTransaction();

        try
        {
            foreach (GridViewRow gvrow in gvValidEmployee.Rows)
            {


                if (gvrow.Cells[12].Text.ToString() != "True")
                {
                    continue;
                }

                strEmpCode = gvrow.Cells[0].Text.ToString();
                strLeaveType = gvrow.Cells[1].Text.ToString();

                strEmpId = ObjEmployee.GetEmployeeMasterByEmpCode(Session["CompId"].ToString(), strEmpCode, ref trns).Rows[0]["Emp_Id"].ToString();
                strLeaveTypeId = Objleave.GetLeaveMasterByLeaveName(Session["CompId"].ToString(), strLeaveType, ref trns).Rows[0]["Leave_Id"].ToString();

                stryear = gvrow.Cells[2].Text.ToString();
                strPreviousLeave = gvrow.Cells[3].Text.ToString();
                strAssignLeave = gvrow.Cells[4].Text.ToString();
                strTotalLeave = gvrow.Cells[5].Text.ToString();
                strPaidLeave = gvrow.Cells[6].Text.ToString();
                strleaveBalance = gvrow.Cells[7].Text.ToString();
                strpaidLeavebalance = gvrow.Cells[8].Text.ToString();
                strIsRule = ((CheckBox)gvrow.Cells[9].Controls[0]).Checked.ToString();
                strIsyearCarry = ((CheckBox)gvrow.Cells[10].Controls[0]).Checked.ToString();
                strIsAuto = ((CheckBox)gvrow.Cells[11].Controls[0]).Checked.ToString();


                dtEmpLeave1 = new DataView(dtEmpLeave, "Emp_Id=" + strEmpId + " and LeaveType_Id=" + strLeaveTypeId + " and Isactive='True'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtEmpLeave1.Rows.Count > 0)
                {
                    objDa.execute_Command("delete from Set_Att_Employee_Leave  where Trans_no=" + dtEmpLeave1.Rows[0]["Trans_No"].ToString() + "", ref trns);
                }

                objDa.execute_Command("delete from Att_Employee_Leave_Trans where emp_id=" + strEmpId + " and Leave_Type_Id=" + strLeaveTypeId + " and Year=" + stryear + "", ref trns);



                objEmpleave.InsertEmployeeLeave(Session["CompId"].ToString(), strEmpId, strLeaveTypeId, (Convert.ToInt32(strTotalLeave) - Convert.ToInt32(strPreviousLeave)).ToString(), (Convert.ToInt32(strPaidLeave) - Convert.ToInt32(strPreviousLeave)).ToString(), "100", "Yearly", true.ToString(), strIsyearCarry, "", "", "", strIsRule.ToString(), strIsAuto.ToString(), true.ToString(), dtTodayDate.ToString(), true.ToString(), Session["UserId"].ToString(), dtTodayDate.ToString(), Session["UserId"].ToString(), dtTodayDate.ToString(), ref trns);

                if (((CheckBox)gvrow.Cells[9].Controls[0]).Checked)
                {
                    objEmpleave.InsertEmployeeLeaveTrans(Session["CompId"].ToString(), strEmpId, strLeaveTypeId, stryear, "0", strPreviousLeave, strAssignLeave, strTotalLeave, "0", strPreviousLeave, "0", strPaidLeave.ToString(), strpaidLeavebalance.ToString(), "Open", "", "", true.ToString(), dtTodayDate.ToString(), true.ToString(), Session["UserId"].ToString(), dtTodayDate.ToString(), Session["UserId"].ToString(), dtTodayDate.ToString(), ref trns);
                }
                else
                {
                    objEmpleave.InsertEmployeeLeaveTrans(Session["CompId"].ToString(), strEmpId, strLeaveTypeId, stryear, "0", strPreviousLeave, strAssignLeave, strTotalLeave, "0", strTotalLeave, "0", strPaidLeave.ToString(), strpaidLeavebalance.ToString(), "Open", "", "", true.ToString(), dtTodayDate.ToString(), true.ToString(), Session["UserId"].ToString(), dtTodayDate.ToString(), Session["UserId"].ToString(), dtTodayDate.ToString(), ref trns);
                }

                counter++;
            }



            trns.Commit();
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }
            trns.Dispose();
            con.Dispose();

            string Message = string.Empty;



            Message += counter.ToString() + " Record Inserted ";


            DisplayMessage(Message);

            objPageCmn.FillData((object)gvValidEmployee, null, "", "");
            lblvalidRecord.Text = "Total Valid Record : 0";

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

            //if (dt.Columns.Contains("Rack_Name"))
            //{
            //    DataTable dtRack = dt.DefaultView.ToTable(true, "ProductId", "ProductCode", "EProductName", "Unit_Name", "SystemQuantity", "PhysicalQuantity");
            //    dtRack.Columns.Add("UnitCost");
            //    for (int i = 0; i < dtRack.Rows.Count; i++)
            //    {
            //        DataTable dtTemp = new DataTable();

            //        dtTemp = new DataView(dt, "ProductId='" + dtRack.Rows[i]["ProductId"] + "'", "", DataViewRowState.CurrentRows).ToTable();
            //        object sumObject;
            //        sumObject = dtTemp.Compute("Sum(PhysicalQuantity)", "");
            //        dtRack.Rows[i]["PhysicalQuantity"] = sumObject.ToString();
            //        dtRack.Rows[i]["SystemQuantity"] = dtTemp.Rows[0]["SystemQuantity"].ToString();
            //        dtRack.Rows[i]["UnitCost"] = dtTemp.Rows[0]["UnitCost"].ToString();

            //    }
            //    dt = null;
            //    dt = dtRack;
            //}
            //DataTable dtUnit = objUnit.GetUnitMaster(StrCompId);
            //dt.Columns.Add("UnitId");
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    try
            //    {

            //        dt.Rows[i]["UnitId"] = new DataView(dtUnit, "Unit_Name='" + dt.Rows[i]["Unit_Name"] + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Unit_Id"].ToString();
            //    }
            //    catch
            //    {
            //        dt.Rows[i]["UnitId"] = ObjProductMaster.GetProductMasterById(StrCompId, StrBrandId, StrLocationId, HttpContext.Current.Session["FinanceYearId"].ToString()).Rows[0]["UnitId"].ToString();
            //    }

            //}



        }
        catch
        {
            DisplayMessage("Excel Format should be in .xls");

        }
        return dt;
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
            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('');", true);
        }
    }
    protected void FUAll_FileUploadComplete(object sender, EventArgs e)
    {
        if (fileLoad.HasFile)
        {
            fileLoad.SaveAs(Server.MapPath("~/Temp/" + fileLoad.FileName));
            string Path = Server.MapPath("~/Temp/" + fileLoad.FileName);
        }
    }

    protected void rbtnall_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dtleave = new DataTable();

        if (Session["dtleaveOpeningBalance"] == null)
        {
            return;
        }

        dtleave = (DataTable)Session["dtleaveOpeningBalance"];

        if (rbtnValid.Checked)
        {
            dtleave = new DataView(dtleave, "IsValid='True'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else if (rbtninValid.Checked)
        {

            dtleave = new DataView(dtleave, "IsValid<>'True'", "", DataViewRowState.CurrentRows).ToTable();
        }


        objPageCmn.FillData((object)gvValidEmployee, dtleave, "", "");
        lblvalidRecord.Text = "Total Record :" + dtleave.Rows.Count.ToString();

    }
}