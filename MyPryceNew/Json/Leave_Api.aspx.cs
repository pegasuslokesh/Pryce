using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json.Converters;
using System.Configuration;
using Newtonsoft.Json.Serialization;

using PegasusDataAccess;
using System.IO;
using System.Xml;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public partial class Leave_Api : System.Web.UI.Page
{
    DataAccessClass objDA = null;
    Att_Leave_Request ObjLeave = null;
    SystemParameter objSys = null;
    Set_ApplicationParameter objAppParam = null;
    Att_Employee_Leave objEmpleave = null;
    Set_Approval_Employee objApproalEmp = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        objDA = new DataAccessClass(Session["DBConnection"].ToString());
        ObjLeave = new Att_Leave_Request(Session["DBConnection"].ToString());
        objSys = new SystemParameter(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());
        objEmpleave = new Att_Employee_Leave(Session["DBConnection"].ToString());

        objApproalEmp = new Set_Approval_Employee(Session["DBConnection"].ToString());
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        string registration_code;
        DataTable dtResult = new DataTable();
        dtResult.Columns.Add(new DataColumn("result"));
        string s = string.Empty;
        DataRow row = dtResult.NewRow();
        string strSQL = string.Empty;
        string Emp_Code = string.Empty;
        string Leave_type = string.Empty;
        string strLeave_Date = string.Empty;
        string strleaveTypeId = string.Empty;
        string strEmpId = string.Empty;
        string strMaxId = string.Empty;
        string strLeaveId = string.Empty;
        DataTable dtLeaveDetail = new DataTable();
        DataTable dtEmp = new DataTable();
        bool IsProbationPeriod = false;
        int ProbationMonth = 0;
        DateTime DOJ = new DateTime();
        DataTable dtLeaveSummary = new DataTable();
        int RemainingDays = 0;
        string strValidation = string.Empty;
        string MaxLeaveId = string.Empty;
        string EmpPermission = string.Empty;
        DataTable dtApproval = new DataTable();
        EmpPermission = objSys.Get_Approval_Parameter_By_Name("Leave").Rows[0]["Approval_Level"].ToString();
        try
        {
            registration_code = Request.Form["Query"].ToString();
            JObject results = JObject.Parse(registration_code);
            
            int counter = 0;


            foreach (var result in results["result"])
            {

                Emp_Code = (string)result["Emp_Code"];
                Leave_type = (string)result["Leave_Type"];
                strLeave_Date = (string)result["Leave_Date"];
                try
                {
                    Convert.ToDateTime(strLeave_Date);
                }
                catch
                {
                    continue;
                }


                dtEmp = GetEmpIdbyEmployeeCode(Emp_Code);

                if (dtEmp.Rows.Count <= 0)
                {
                    continue;
                }
                else
                {
                    strEmpId = dtEmp.Rows[0]["Emp_Id"].ToString();
                    //Session["CompId"] = dtEmp.Rows[0]["Company_Id"].ToString();
                    //Session["BrandId"] = dtEmp.Rows[0]["Brand_Id"].ToString();
                    //Session["LOcId"] = dtEmp.Rows[0]["Location_Id"].ToString();
                    //Session["EmpId"] = dtEmp.Rows[0]["Emp_Id"].ToString();
                }


                strleaveTypeId = GetLeaveIdbyLeaveType(Leave_type);

                if (strleaveTypeId == "0")
                {
                    continue;
                }


                dtLeaveSummary = objEmpleave.GetEmployeeLeaveTransactionDataByEmpId(strEmpId);

                dtLeaveSummary = new DataView(dtLeaveSummary, "Leave_Type_Id='" + strleaveTypeId.Trim() + "' and Field3='Open'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtLeaveSummary.Rows.Count <= 0)
                {
                    continue;
                }
                else
                {
                    RemainingDays = Convert.ToInt32(dtLeaveSummary.Rows[0]["Remaining_Days"].ToString());
                }


                IsProbationPeriod = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsProbationPeriod", dtEmp.Rows[0]["Company_Id"].ToString(), dtEmp.Rows[0]["Brand_Id"].ToString(), dtEmp.Rows[0]["Location_Id"].ToString()));
                if (IsProbationPeriod == true)
                {
                    ProbationMonth = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("ProbationPeriod", dtEmp.Rows[0]["Company_Id"].ToString(), dtEmp.Rows[0]["Brand_Id"].ToString(), dtEmp.Rows[0]["Location_Id"].ToString()));
                }
                else
                {
                    ProbationMonth = 0;
                }

                DOJ = Convert.ToDateTime(dtEmp.Rows[0]["DOJ"].ToString());

                dtLeaveDetail = GetLeaveDatatable();

            
                strValidation = ObjLeave.CheckLeaveValidation(dtEmp.Rows[0]["Company_Id"].ToString(), dtEmp.Rows[0]["Brand_Id"].ToString(), dtEmp.Rows[0]["Location_Id"].ToString(), strEmpId, strleaveTypeId, Convert.ToDateTime(strLeave_Date), Convert.ToDateTime(strLeave_Date), "", ProbationMonth, IsProbationPeriod, DOJ, RemainingDays, 1, dtLeaveDetail, Common.LeaveApprovalFunctionality(strleaveTypeId, Session["DBConnection"].ToString()), HttpContext.Current.Session["TimeZoneId"].ToString());

                if (strValidation != "")
                {
                    continue;
                }

                dtLeaveDetail = FillLeaveDatatable(dtEmp.Rows[0]["Company_Id"].ToString(), dtEmp.Rows[0]["Brand_Id"].ToString(), dtEmp.Rows[0]["Location_Id"].ToString(),strEmpId, dtLeaveDetail, strleaveTypeId, strLeave_Date, strLeave_Date);


                dtApproval = objApproalEmp.getApprovalChainByObjectid(dtEmp.Rows[0]["Company_Id"].ToString(), dtEmp.Rows[0]["Brand_Id"].ToString(), dtEmp.Rows[0]["Location_Id"].ToString(), "52", strEmpId);


                SqlConnection con = new SqlConnection(Session["DBConnection"].ToString());
                con.Open();
                SqlTransaction trns;
                trns = con.BeginTransaction();

                try
                {

                    MaxLeaveId = ObjLeave.SaveLeaveRequest(dtEmp.Rows[0]["Company_Id"].ToString(), dtEmp.Rows[0]["Brand_Id"].ToString(), dtEmp.Rows[0]["Location_Id"].ToString(),"Superadmin",strEmpId, Convert.ToDateTime(strLeave_Date), Convert.ToDateTime(strLeave_Date), Convert.ToDateTime(dtLeaveDetail.Rows[0]["Field7"].ToString()), 1, dtLeaveDetail, "", Convert.ToBoolean(dtLeaveDetail.Rows[0]["Is_Approval"].ToString()), ref trns, HttpContext.Current.Session["TimeZoneId"].ToString())[1];
                    ObjLeave.InsertLeaveApproval(dtEmp.Rows[0]["Company_Id"].ToString(), dtEmp.Rows[0]["Brand_Id"].ToString(), dtEmp.Rows[0]["Location_Id"].ToString(), "Superadmin",strEmpId, EmpPermission, dtApproval, MaxLeaveId.ToString(), dtLeaveDetail, Common.GetEmployeeName(dtEmp.Rows[0]["Company_Id"].ToString(), strEmpId, ref trns), "", Common.GetEmployeeImage(dtEmp.Rows[0]["Company_Id"].ToString(),strEmpId, ref trns), Convert.ToDateTime(strLeave_Date), Convert.ToDateTime(strLeave_Date), true, Convert.ToBoolean(dtLeaveDetail.Rows[0]["Is_Approval"].ToString()), ref trns, HttpContext.Current.Session["TimeZoneId"].ToString(), HttpContext.Current.Request.Url.AbsoluteUri.ToString());

                    counter++;
                    trns.Commit();
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                    }
                    trns.Dispose();
                    con.Dispose();
                }
                catch(Exception ex)
                {
                    row[0] = ex.ToString();
                    trns.Rollback();
                    if (con.State == System.Data.ConnectionState.Open)
                    {

                        con.Close();
                    }
                    trns.Dispose();
                    con.Dispose();
                }
                
            }


            row[0] = counter.ToString() + " row affected";

        }
        catch (Exception Ex)
        {
            row[0] = Ex.ToString();
        }
        dtResult.Rows.Add(row);
        Common objcmn = new Common(Session["DBConnection"].ToString());


        Response.Write(DataTableToJSONWithJSONNet(dtResult));

    }


    public DataTable GetLeaveDatatable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Trans_id", typeof(float));
        dt.Columns.Add("Leave_Type_id");
        dt.Columns.Add("From_date", typeof(DateTime));
        dt.Columns.Add("To_Date", typeof(DateTime));
        dt.Columns.Add("Field1");
        dt.Columns.Add("Emp_Description");
        dt.Columns.Add("Field3");
        dt.Columns.Add("LeaveCount", typeof(float));
        dt.Columns.Add("Field7", typeof(DateTime));
        dt.Columns.Add("Emp_Id");
        dt.Columns.Add("Is_Approval", typeof(bool));
        return dt;
    }


    public DataTable FillLeaveDatatable( string strCompId,string strBrandId,string strLOcId,string strEmpId, DataTable dtLeaveDetail, string strShiftId, string strFromdate, string strToDate)
    {
        dtLeaveDetail.Rows.Add();
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][0] = dtLeaveDetail.Rows.Count + 1;
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][1] = strShiftId;
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][2] = strFromdate;
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][3] = strToDate;
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][4] = "0";
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][5] = "Leave Requested from shift upload functionality";
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][6] = "Yearly";
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][7] = "1";
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][8] = Att_Leave_Request.Get_Rejoin(strCompId,strBrandId,strLOcId,strFromdate, strEmpId, Session["DBConnection"].ToString(), HttpContext.Current.Session["TimeZoneId"].ToString());
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][9] = strEmpId;
        dtLeaveDetail.Rows[dtLeaveDetail.Rows.Count - 1][10] = Common.LeaveApprovalFunctionality(strShiftId, Session["DBConnection"].ToString());

        return dtLeaveDetail;
    }


    public string IsLeaveOnDate(string Date, string EmpId)
    {
        string LeaveTRansId = "0";
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Date", Date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = objDA.Reuturn_Datatable_Search("sp_Att_Leave_Request_IsLeave", paramList);

        if (dtInfo.Rows.Count > 0)
        {
            LeaveTRansId = dtInfo.Rows[0]["Trans_Id"].ToString();
        }

        dtInfo.Dispose();

        return LeaveTRansId;
    }



    public DataTable GetEmpIdbyEmployeeCode(string strEmpCode)
    {
        string strEmpId = string.Empty;

        DataTable dtEmp = objDA.return_DataTable("select Company_id,Brand_Id,Location_Id,emp_id,Doj from set_employeemaster where Emp_Code='" + strEmpCode + "' and IsActive='True' and Field2='False' and emp_type='On Role'");


        return dtEmp;

    }


    public string GetLeaveIdbyLeaveType(string strLeaveTypeName)
    {
        string strLeaveId = string.Empty;

        DataTable dtLeave = objDA.return_DataTable("select Leave_Id from Att_LeaveMaster where leave_name='" + strLeaveTypeName.Trim() + "' and IsActive='True'");

        if (dtLeave.Rows.Count > 0)
        {
            strLeaveId = dtLeave.Rows[0][0].ToString();
        }
        else
        {
            strLeaveId = "0";
        }

        return strLeaveId;

    }



    public string DataTableToJSONWithJSONNet(DataTable table)
    {

        string json = JsonConvert.SerializeObject(table, Newtonsoft.Json.Formatting.Indented);
        return json;
    }

}