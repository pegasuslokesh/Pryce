using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using PegasusDataAccess;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Att_Leave_Request
/// </summary>
public class Att_Leave_Request
{
    DataAccessClass daClass = null;
    Att_AttendanceLog objAttLog = null;
    Att_Employee_Leave objEmpleave = null;
    Set_ApplicationParameter objAppParam = null;
    Set_Approval_Employee objApproalEmp = null;
    LeaveMaster objleave = null;
    NotificationMaster Obj_Notifiacation = null;
    private string _strConString = string.Empty;

    public Att_Leave_Request(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
        objAttLog = new Att_AttendanceLog(strConString);
        objEmpleave = new Att_Employee_Leave(strConString);
        objAppParam = new Set_ApplicationParameter(strConString);
        objApproalEmp = new Set_Approval_Employee(strConString);
        objleave = new LeaveMaster(strConString);
        Obj_Notifiacation = new NotificationMaster(strConString);
        _strConString = strConString;
    }
    public bool IsPaidLeave(string Leave_Date, string Emp_Id)
    {
        bool IsPaid = false;
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Leave_Date", Leave_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_Leave_Request_Child_Leave", paramList);
        if (dtInfo.Rows.Count > 0)
        {
            if (Convert.ToBoolean(dtInfo.Rows[0]["Is_Paid"].ToString()))
            {
                IsPaid = true;
            }

        }

        return IsPaid;

    }

    public bool IsPaidLeave(string Leave_Date, string Emp_Id, ref SqlTransaction trns)
    {
        bool IsPaid = false;
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Leave_Date", Leave_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_Leave_Request_Child_Leave", paramList, ref trns);
        if (dtInfo.Rows.Count > 0)
        {
            if (Convert.ToBoolean(dtInfo.Rows[0]["Is_Paid"].ToString()))
            {
                IsPaid = true;
            }

        }

        return IsPaid;

    }



    public DataTable GetLeaveRequestChildData_By_Employeeidanddate(string Leave_Date, string Emp_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Leave_Date", Leave_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_Leave_Request_Child_Leave", paramList);
        return dtInfo;
    }

    public DataTable IsLeaveSalary(string Leave_Date, string Emp_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Leave_Date", Leave_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_Leave_Request_Child_Leave", paramList);

        return dtInfo;

    }

    public DataTable IsLeaveSalary(string Leave_Date, string Emp_Id, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Leave_Date", Leave_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_Leave_Request_Child_Leave", paramList, ref trns);

        return dtInfo;

    }


    public int DeleteLeaveRequestChildByRefId(string RefId)
    {

        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Ref_Id", RefId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Att_Leave_Request_Child_Delete", paramList);
        return Convert.ToInt32(paramList[1].ParaValue);
    }

    public int DeleteLeaveRequestChildByRefId(string RefId, ref SqlTransaction trns)
    {

        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Ref_Id", RefId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Att_Leave_Request_Child_Delete", paramList, ref trns);
        return Convert.ToInt32(paramList[1].ParaValue);
    }
    public int InsertLeaveRequestChild(string Ref_Id, string LeaveType_Id, string Leave_Date, string Is_Paid, string Type1, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate)
    {


        PassDataToSql[] paramList = new PassDataToSql[18];
        paramList[0] = new PassDataToSql("@Ref_Id", Ref_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@LeaveType_Id", LeaveType_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Leave_Date", Leave_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@Is_Paid", Is_Paid, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Type", Type1, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[5] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[6] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


        paramList[12] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Att_Leave_Request_Child_Insert", paramList);
        return Convert.ToInt32(paramList[17].ParaValue);
    }

    public int InsertLeaveRequestChild(string Ref_Id, string LeaveType_Id, string Leave_Date, string Is_Paid, string Type1, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
    {


        PassDataToSql[] paramList = new PassDataToSql[18];
        paramList[0] = new PassDataToSql("@Ref_Id", Ref_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@LeaveType_Id", LeaveType_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Leave_Date", Leave_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@Is_Paid", Is_Paid, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Type", Type1, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[5] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[6] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


        paramList[12] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Att_Leave_Request_Child_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[17].ParaValue);
    }

    public int InsertLeaveRequest(string CompanyId, string Leave_Type_Id, string Emp_Id, string Application_Date, string From_Date, string To_Date, string Is_Pending, string Is_Approved, string Is_Canceled, string Emp_Description, string Mgmt_Description, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate)
    {


        PassDataToSql[] paramList = new PassDataToSql[24];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Application_Date", Application_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@From_Date", From_Date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@To_Date", To_Date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Is_Pending", Is_Pending, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Is_Approved", Is_Approved, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Is_Canceled", Is_Canceled, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Emp_Description", Emp_Description, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Mgmt_Description", Mgmt_Description, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[10] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[11] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);


        paramList[17] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[23] = new PassDataToSql("@Leave_Type_Id", Leave_Type_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Att_Leave_Request_Insert", paramList);
        return Convert.ToInt32(paramList[22].ParaValue);
    }

    public int UpdateEmployeeLeaveTransactionByTransNo(string TransId, string CompanyId, string Emp_Id, string leavetypeid, string Year, string Month, string Previous_Days, string Assign_Days, string Total_Days, string Used_Days, string Remaining_Days, string Pending_Days, string RemainPaidDays, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[17];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Year", Year, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Month", Month, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Previous_Days", Previous_Days, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Assign_Days", Assign_Days, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Total_Days", Total_Days, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Used_Days", Used_Days, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Remaining_Days", Remaining_Days, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[9] = new PassDataToSql("@Pending_Days", Pending_Days, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Encash_Days", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[14] = new PassDataToSql("@Leave_Type_Id", leavetypeid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Trans_Id", TransId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Field2", RemainPaidDays, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Att_Employee_Leave_Trans_UpdateByTransNo", paramList, ref trns);
        return Convert.ToInt32(paramList[13].ParaValue);
    }
    public int UpdateEmployeeLeaveTransactionByTransNo(string TransId, string CompanyId, string Emp_Id, string leavetypeid, string Year, string Month, string Previous_Days, string Assign_Days, string Total_Days, string Used_Days, string Remaining_Days, string Pending_Days, string RemainPaidDays, string ModifiedBy, string ModifiedDate, string EncashDays, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[16];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Year", Year, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Month", Month, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Previous_Days", Previous_Days, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Assign_Days", Assign_Days, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Total_Days", Total_Days, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Used_Days", Used_Days, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Remaining_Days", Remaining_Days, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Pending_Days", Pending_Days, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[13] = new PassDataToSql("@Leave_Type_Id", leavetypeid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Trans_Id", TransId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field2", RemainPaidDays, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Att_Employee_Leave_Trans_UpdateByTransNo_New", paramList, ref trns);
        return Convert.ToInt32(paramList[12].ParaValue);
    }




    public int InsertLeaveRequest(string CompanyId, string Leave_Type_Id, string Emp_Id, string Application_Date, string From_Date, string To_Date, string Is_Pending, string Is_Approved, string Is_Canceled, string Emp_Description, string Mgmt_Description, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate, string Departure_Date, string Returning_Date, ref SqlTransaction trns)
    {


        PassDataToSql[] paramList = new PassDataToSql[26];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Application_Date", Application_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@From_Date", From_Date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@To_Date", To_Date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Is_Pending", Is_Pending, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Is_Approved", Is_Approved, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Is_Canceled", Is_Canceled, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Emp_Description", Emp_Description, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Mgmt_Description", Mgmt_Description, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[10] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[11] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);


        paramList[17] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[23] = new PassDataToSql("@Leave_Type_Id", Leave_Type_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@Departure_Date", Departure_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@Returning_Date", Returning_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Att_Leave_Request_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[22].ParaValue);
    }

    public int UpdateLeaveRequestByTransId(string Trans_Id, string CompanyId, string Is_Pending, string Is_Approved, string Is_Canceled, string Mgmt_Description, string IsActive, string ModifiedBy, string ModifiedDate)
    {

        PassDataToSql[] paramList = new PassDataToSql[10];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Is_Pending", Is_Pending, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Is_Approved", Is_Approved, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Is_Canceled", Is_Canceled, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Mgmt_Description", Mgmt_Description, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);


        paramList[5] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[9] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Att_Leave_Request_Update", paramList);
        return Convert.ToInt32(paramList[8].ParaValue);
    }


    public int UpdateLeaveRequestByTransId(string Trans_Id, string CompanyId, string Is_Pending, string Is_Approved, string Is_Canceled, string Mgmt_Description, string IsActive, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
    {

        PassDataToSql[] paramList = new PassDataToSql[10];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Is_Pending", Is_Pending, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Is_Approved", Is_Approved, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Is_Canceled", Is_Canceled, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Mgmt_Description", Mgmt_Description, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);


        paramList[5] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[9] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Att_Leave_Request_Update", paramList, ref trns);
        return Convert.ToInt32(paramList[8].ParaValue);
    }

    public DataTable GetLeaveRequestChildByTrandId(string TransId)
    {
        return daClass.return_DataTable("select * from Att_Leave_Request_Child where Ref_Id='" + TransId + "' and Is_Paid='True'");
    }

    public DataTable GetLeaveRequestChildByTrandId(string TransId, ref SqlTransaction trns)
    {
        return daClass.return_DataTable("select * from Att_Leave_Request_Child where Ref_Id='" + TransId + "' and Is_Paid='True'", ref trns);
    }
    public int UpdateLeaveRequestByEmpId(string CompanyId, string Leave_Type_Id, string Emp_Id, string Application_Date, string From_Date, string To_Date, string Is_Pending, string Is_Approved, string Is_Canceled, string Emp_Description, string Mgmt_Description, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[22];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Application_Date", Application_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@From_Date", From_Date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@To_Date", To_Date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Is_Pending", Is_Pending, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Is_Approved", Is_Approved, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Is_Canceled", Is_Canceled, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Emp_Description", Emp_Description, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Mgmt_Description", Mgmt_Description, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[21] = new PassDataToSql("@Leave_Type_Id", Leave_Type_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Att_Leave_Request_Update_ByEmpId", paramList);
        return Convert.ToInt32(paramList[20].ParaValue);
    }
    public int DeleteLeaveRequest(string CompanyId, string Trans_Id, string IsActive, string ModifiedBy, string ModifiedDate)
    {

        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Att_Leave_Request_RowStatus", paramList);
        return Convert.ToInt32(paramList[5].ParaValue);
    }
    public int DeleteLeaveRequest(string CompanyId, string Trans_Id, string IsActive, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
    {

        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Att_Leave_Request_RowStatus", paramList, ref trns);
        return Convert.ToInt32(paramList[5].ParaValue);
    }
    public DataTable GetLeaveRequestById(string CompanyId, string EmpId)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_Leave_Request_SelectRow", paramList);

        return dtInfo;
    }
    public bool IsLeaveOnDate(string Date, string EmpId)
    {
        bool isleave = false;
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Date", Date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_Leave_Request_IsLeave", paramList);

        if (dtInfo.Rows.Count > 0)
        {
            isleave = true;
        }
        DataTable dtLog = objAttLog.GetAttendanceLogByDateByEmpId1(EmpId, Date.ToString(), Date.ToString());

        if (dtLog.Rows.Count > 0)
        {
            isleave = false;
        }

        dtLog.Dispose();

        return isleave;
    }

    public string GetLeavetypeName(string Date, string EmpId, string strCompId)
    {

        string strLeaveName = "Leave";
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Date", Date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_Leave_Request_IsLeave", paramList);

        if (dtInfo.Rows.Count > 0)
        {
            strLeaveName = Common.GetleaveNameById(dtInfo.Rows[0]["Leave_Type_Id"].ToString(), _strConString, strCompId);
        }

        return strLeaveName;

    }

    public DataTable GetLeaveRequest(string CompanyId)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_Leave_Request_SelectRow", paramList);

        return dtInfo;
    }

    public DataTable GetLeaveRequest_ByID(string CompanyId, string Emp_ID)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", Emp_ID, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_Leave_Request_SelectRow", paramList);
        return dtInfo;
    }
    public DataTable GetLeaveEncashment_ByEmpId(string Emp_ID)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Emp_Id", Emp_ID, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_Leave_Encashment", paramList);
        return dtInfo;
    }
    public DataTable GetLeaveRequest_ByTrans_ID(string CompanyId, string Emp_ID, string Trans_ID)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", Emp_ID, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Trans_ID", @Trans_ID, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_Leave_Request_SelectRow_New", paramList);

        return dtInfo;
    }



    public DataTable GetLeaveRequest_ByTrans_ID(string CompanyId, string Emp_ID, string Trans_ID, ref SqlTransaction trns)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", Emp_ID, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Trans_ID", @Trans_ID, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_Leave_Request_SelectRow_New", paramList, ref trns);

        return dtInfo;
    }



    public DataTable GetLeaveRequest(string CompanyId, ref SqlTransaction trns)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_Leave_Request_SelectRow", paramList, ref trns);

        return dtInfo;
    }

    public string CheckLeaveValidation(string strCompanyId, string strBrandId, string strLocationId, string strEmpId, string strLeaveTypeId, DateTime dtFromdate, DateTime dtTodate, string strTransId, int ProbationMonth, bool IsProbationPeriod, DateTime DOJ, int remainingdayscount, int Applieddayscount, DataTable dtLeaveDetail, bool LeaveApprovalFunctionality, string strTimeZoneId, bool isLeaveExistsValidation = true)
    {
        Attendance objAttendance = new Attendance(_strConString);
        DataTable dtleavesalary = new DataTable();
        //Attendance objAttendance = new Attendance();
        //Att_Employee_Leave objEmpleave = new Att_Employee_Leave();
        //Set_ApplicationParameter objAppParam = new Set_ApplicationParameter();
        //Set_Approval_Employee objApproalEmp = new Set_Approval_Employee();

        dtLeaveDetail = new DataView(dtLeaveDetail, "Emp_Id=" + strEmpId + "", "", DataViewRowState.CurrentRows).ToTable();

        string strValidation = string.Empty;
        DataTable dtPostedList = new DataTable();
        DataTable dtPostedList1 = new DataTable();
        DataTable dtApprovalMaster = new DataTable();
        string strMaxLeavesingleAttempt = string.Empty;
        int FinancialYearMonth = 0;
        dtPostedList = objAttLog.Get_Pay_Employee_Attendance(strEmpId, dtFromdate.Month.ToString(), dtFromdate.Year.ToString());
        dtPostedList1 = objAttLog.Get_Pay_Employee_Attendance(strEmpId, dtTodate.Month.ToString(), dtTodate.Year.ToString());
        // DataTable dtLeaveSummary = objEmpleave.GetEmployeeLeaveTransactionDataByEmpId(strEmpId);
        try
        {
            FinancialYearMonth = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("FinancialYearStartMonth", strCompanyId, strBrandId, strLocationId));
        }
        catch
        {
            FinancialYearMonth = 1;
        }
        //int FinancialYearMonth = 0;
        //if (dt.Rows.Count > 0)
        //{
        //    FinancialYearMonth = int.Parse(dt.Rows[0]["Param_Value"].ToString());
        //}
        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();
        if (Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Month < FinancialYearMonth)
        {
            FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Year - 1, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }
        else
        {
            FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Year, FinancialYearMonth, 1);
            FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
        }


        if (dtPostedList.Rows.Count > 0 || dtPostedList1.Rows.Count > 0)
        {
            strValidation = "Log Posted For This Date Criteria";

        }
        //else if (!Common.IsFinancialyearDateCheckOnly(dtFromdate))
        //{
        //    strValidation = "Log In Financial year not allowing to perform this action";

        //}
        //else if (!Common.IsFinancialyearDateCheckOnly(dtTodate))
        //{
        //    strValidation = "Log In Financial year not allowing to perform this action";

        //}
        else if (strTransId == "" && daClass.return_DataTable("select Emp_Id from Att_Leave_Request where Emp_Id=" + strEmpId + " and Is_Pending='True' and Leave_Type_id=" + strLeaveTypeId + "").Rows.Count > 0)
        {

            strValidation = "you can not apply a new request because previous request is under process";


        }
        else if (strTransId != "" && daClass.return_DataTable("select Emp_Id from Att_Leave_Request where Emp_Id=" + strEmpId + " and Is_Pending='True' and Field2<>" + strTransId + " and Leave_Type_id=" + strLeaveTypeId + "").Rows.Count > 0)
        {

            strValidation = "you can not apply a new request because previous request is under process";

        }
        else if (!objAttendance.IsLeaveMaturity(strCompanyId, strEmpId, strLeaveTypeId, strTimeZoneId))
        {
            strValidation = "Employee not eligible";

        }
        else if (dtFromdate <= DOJ.AddMonths(ProbationMonth) && IsProbationPeriod == true)
        {
            strValidation = "You are not eligible for Leave request during Probation Period";

        }


        if (strValidation == "" && isLeaveExistsValidation)
        {
            string FullDay = objAttendance.GetLeaveApprovalStatus(strCompanyId, strEmpId, dtFromdate, dtTodate, "FullDay", 0, strBrandId, strLocationId, strTimeZoneId);

            if (FullDay != string.Empty && strTransId == "")
            {
                strValidation = FullDay.ToString();

            }
        }


        if (strValidation == "")
        {
            string HalfDay = objAttendance.GetLeaveApprovalStatus(strCompanyId, strEmpId, dtFromdate, dtTodate, "HalfDay", 0, strBrandId, strLocationId, strTimeZoneId);
            if (HalfDay != string.Empty)
            {
                strValidation = HalfDay.ToString();
            }
        }

        if (strValidation == "")
        {
            string PartialDay = objAttendance.GetLeaveApprovalStatus(strCompanyId, strEmpId, dtFromdate, dtTodate, "PartialLeave", 0, strBrandId, strLocationId, strTimeZoneId);
            if (PartialDay != string.Empty)
            {
                strValidation = PartialDay.ToString();
            }
        }

        if (strValidation == "")
        {
            dtApprovalMaster = objApproalEmp.getApprovalChainByObjectid(strCompanyId, strBrandId, strLocationId, "56", strEmpId);
            if (dtApprovalMaster.Rows.Count == 0)
            {
                strValidation = "Approval setup issue , please contact to your admin";

            }
        }


        if (strValidation == "")
        {
            //if negative balance flag was false then validation will work 
            if (!Common.LeaveNegativeBalance(strLeaveTypeId, _strConString))
            {
                if (Applieddayscount > remainingdayscount)
                {
                    strValidation = "Employee does not have Sufficient Leave";

                }
            }
        }



        if (strValidation == "")
        {
            while (dtFromdate <= dtTodate)
            {
                if (new DataView(dtLeaveDetail, "(From_Date<='" + dtFromdate + "' and To_Date>='" + dtFromdate + "') or (From_Date='" + dtFromdate + "') or ( To_Date='" + dtFromdate + "') ", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                {
                    strValidation = "Leave Already Applied between selected date criteria";
                    break;
                }
                dtFromdate = dtFromdate.AddDays(1);
            }




            //DataView dv = new DataView();
            //dv = dtLeaveDetail.DefaultView;
            //dv.RowFilter = "(From_Date>='" + dtFromdate + "' and To_Date<='" + dtFromdate + "') or (From_Date>='" + dtTodate + "' and To_Date<='" + dtTodate + "') ";
            //dtLeaveDetail = dv.ToTable();

            //if (dtLeaveDetail.Rows.Count > 0)
            //{
            //    strValidation = "Leave Already Applied between selected date criteria";

            //}

            //while (dtFromdate <= dtTodate)
            //{
            //    if (new DataView(dtLeaveDetail, "From_Date>=" + dtFromdate + " and To_Date<=" + dtTodate + "", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            //    {

            //    }

            //    dtFromdate = dtFromdate.AddDays(1);
            //}
        }

        if (strValidation == "")
        {

            strMaxLeavesingleAttempt = LeaveApplicableinSingleslot(strCompanyId, strLeaveTypeId, Applieddayscount, dtLeaveDetail);

            if (strMaxLeavesingleAttempt != "")
            {
                strValidation = strMaxLeavesingleAttempt;
            }
        }

        if (strValidation == "")
        {
            dtleavesalary = daClass.return_DataTable("select emp_id  from att_leavesalary where emp_id=" + strEmpId + " and F5='Pending' and Leave_Type_Id=" + strLeaveTypeId + "");

            if (dtleavesalary.Rows.Count > 0)
            {
                strValidation = "Leave salary request is pending";

            }
        }




        dtPostedList.Dispose();
        dtPostedList1.Dispose();
        dtApprovalMaster.Dispose();
        //dtLeaveSummary.Dispose();



        return strValidation;
    }


    public string LeaveApplicableinSingleslot(string strLeaveType, int strTotalDays, DataTable dtLeaveDetail, string strCompanyId)
    {

        string strSingleAttempt = string.Empty;
        string strMaxLeave = string.Empty;

        strMaxLeave = objleave.GetLeaveMasterById(strCompanyId, strLeaveType).Rows[0]["Field3"].ToString();

        if (strMaxLeave == "")
        {
            strMaxLeave = "0";
        }
        int LeaveApplied = 0;

        try
        {
            LeaveApplied = Convert.ToInt32(strTotalDays);
        }
        catch
        {

        }



        dtLeaveDetail = new DataView(dtLeaveDetail, "Leave_Type_id=" + strLeaveType + "", "", DataViewRowState.CurrentRows).ToTable();


        foreach (DataRow dr in dtLeaveDetail.Rows)
        {
            LeaveApplied += Convert.ToInt32(dr["LeaveCount"].ToString());
        }


        if (Convert.ToInt32(strMaxLeave) > 0)
        {
            if (LeaveApplied > Convert.ToInt32(strMaxLeave))
            {
                strSingleAttempt = "You Can apply " + strMaxLeave + " Leave in single attempt";
            }
        }


        return strSingleAttempt;

    }


    public string LeaveApplicableinSingleslot(string strCompanyId, string strLeaveType, int strTotalDays, DataTable dtLeaveDetail)
    {
        string strSingleAttempt = string.Empty;
        string strMaxLeave = string.Empty;

        strMaxLeave = objleave.GetLeaveMasterById(strCompanyId, strLeaveType).Rows[0]["Field3"].ToString();

        if (strMaxLeave == "")
        {
            strMaxLeave = "0";
        }
        int LeaveApplied = 0;

        try
        {
            LeaveApplied = Convert.ToInt32(strTotalDays);
        }
        catch
        {

        }



        dtLeaveDetail = new DataView(dtLeaveDetail, "Leave_Type_id=" + strLeaveType + "", "", DataViewRowState.CurrentRows).ToTable();


        foreach (DataRow dr in dtLeaveDetail.Rows)
        {
            LeaveApplied += Convert.ToInt32(dr["LeaveCount"].ToString());
        }


        if (Convert.ToInt32(strMaxLeave) > 0)
        {
            if (LeaveApplied > Convert.ToInt32(strMaxLeave))
            {
                strSingleAttempt = "You Can apply " + strMaxLeave + " Leave in single attempt";
            }
        }


        return strSingleAttempt;

    }


    public string[] SaveLeaveRequest(string strCompanyId, string strBrandId, string strLocationId, string strUserId, string strEmpId, DateTime dtFromDate, DateTime dtToDate, DateTime Rejoin_Date, int TotalDays, DataTable dtLeaveDetail, string hdnTransid, bool LeaveApprovalFunctionality, ref SqlTransaction trns, string strTimeZoneId, string strConString = "")
    {

        string[] strResult = new string[2];
        strResult[0] = "";
        bool Result = true;


        int LeaveCounter = 0;
        DataTable dtTemp = new DataTable();
        Set_Approval_Employee objApproalEmp;
        Att_Employee_Leave objEmpleave;
        Set_ApplicationParameter objAppParam;
        Set_Employee_Holiday objEmpHoliday;

        if (strConString.Length > 0)
        {

            objApproalEmp = new Set_Approval_Employee(strConString);
            objEmpleave = new Att_Employee_Leave(strConString);
            objAppParam = new Set_ApplicationParameter(strConString);
            objEmpHoliday = new Set_Employee_Holiday(strConString);
        }
        else
        {
            objApproalEmp = new Set_Approval_Employee(trns.Connection.ConnectionString);
            objEmpleave = new Att_Employee_Leave(trns.Connection.ConnectionString);
            objAppParam = new Set_ApplicationParameter(trns.Connection.ConnectionString);
            objEmpHoliday = new Set_Employee_Holiday(trns.Connection.ConnectionString);
        }




        string strsql = string.Empty;
        string strMaxId = string.Empty;
        bool IsPending = false;
        bool IsApproved = false;


        if (LeaveApprovalFunctionality)
        {
            IsPending = true;
            IsApproved = false;
        }
        else
        {
            IsPending = false;
            IsApproved = true;
        }


        dtTemp = dtLeaveDetail;


        if (hdnTransid != "")
        {
            strsql = "update Att_LeaveRequest_header set From_Date='" + dtFromDate.ToString() + "' , To_Date='" + dtToDate.ToString() + "',TotalDays='" + TotalDays + "',Field7='" + Rejoin_Date.ToString() + "' where Trans_Id=" + hdnTransid + "";
            //  strsql = "INSERT INTO [Att_LeaveRequest_header] ([Emp_Id] ,[From_Date] ,[To_Date] ,[TotalDays] ,[Is_Pending] ,[Is_Approved] ,[Is_Canceled] ,[Field1] ,[Field2] ,[Field3] ,[Field4] ,[Field5] ,[Field6] ,[Field7] ,[IsActive] ,[CreatedBy] ,[CreatedDate] ,[ModifiedBy] ,[ModifiedDate]) VALUES (" + hdnEmpId.Value + " ,'" + dtFromDate.ToString() + "' ,'" + dtToDate.ToString() + "' ," + TotalDays.ToString() + " ,'True' ,'False' ,'False' ,' ',' ',' ' ,' ' , ' ', '" + false.ToString() + "','" + DateTime.Now.ToString() + "' ,'" + true.ToString() + "' ,'" + Session["UserId"].ToString() + "' , '" + DateTime.Now.ToString() + "','" + Session["UserId"].ToString() + "' ,'" + DateTime.Now.ToString() + "')";

            daClass.execute_Command(strsql, ref trns);

            //this code for deleterecord from leave request and child table also

            DataTable dtleaveRquestDetail = daClass.return_DataTable("select Trans_id,Leave_Type_Id,Field3 from Att_Leave_Request where Field2='" + hdnTransid + "'", ref trns);


            foreach (DataRow drdetail in dtleaveRquestDetail.Rows)
            {

                int strPreviousDays = 0;
                //Commented by Ghanshyam Suthar on 06/12/2017
                //DataTable dtPendingDaysCount = objleaveReq.GetLeaveRequest(Session["CompId"].ToString(), ref trns);
                DataTable dtPendingDaysCount = GetLeaveRequest_ByTrans_ID(strCompanyId, "0", drdetail["Trans_Id"].ToString(), ref trns);
                //dtPendingDaysCount = new DataView(dtPendingDaysCount, "Trans_Id='" + drdetail["Trans_Id"].ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtPendingDaysCount.Rows.Count > 0)
                {
                    strPreviousDays = int.Parse(dtPendingDaysCount.Rows[0]["DaysCount"].ToString());
                }


                daClass.execute_Command("update Att_Employee_Leave_Trans set Pending_Days=(Pending_Days-" + strPreviousDays + "),Remaining_Days=(CAST(  Remaining_Days as float)+" + strPreviousDays + "),Field2  =  CAST(Field2 as int)+" + strPreviousDays + " where Leave_Type_Id=" + drdetail["Leave_Type_Id"].ToString() + " and Emp_Id=" + strEmpId + " and Field3='Open' and IsActive='True'", ref trns);

                DeleteLeaveRequest(strCompanyId, drdetail["Trans_Id"].ToString(), true.ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

            }
            //this code for delete record from approval transaction table
            try
            {
                objApproalEmp.Delete_Approval_Transaction("1", strCompanyId, strBrandId, strLocationId, "0", hdnTransid, ref trns);
            }
            catch
            {

            }

            strMaxId = hdnTransid;
        }
        else
        {
            strsql = "INSERT INTO [Att_LeaveRequest_header] ([Emp_Id] ,[From_Date] ,[To_Date] ,[TotalDays] ,[Is_Pending] ,[Is_Approved] ,[Is_Canceled] ,[Field1] ,[Field2] ,[Field3] ,[Field4] ,[Field5] ,[Field6] ,[Field7] ,[IsActive] ,[CreatedBy] ,[CreatedDate] ,[ModifiedBy] ,[ModifiedDate]) VALUES (" + strEmpId + " ,'" + dtFromDate.ToString() + "' ,'" + dtToDate.ToString() + "' ," + TotalDays.ToString() + " ,'" + IsPending.ToString() + "' ,'" + IsApproved.ToString() + "' ,'False' ,' ',' ',' ' ,' ' , ' ', '" + false.ToString() + "','" + Rejoin_Date.ToString() + "' ,'" + true.ToString() + "' ,'" + strUserId + "' , '" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString() + "','" + strUserId + "' ,'" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString() + "')";

            daClass.execute_Command(strsql, ref trns);

            strMaxId = daClass.return_DataTable("select max(Trans_id) from Att_LeaveRequest_header", ref trns).Rows[0][0].ToString();
        }
        int b = 0;

        DataTable dtLeaveSummary = new DataTable();
        //PassDataToSql[] paramList = new PassDataToSql[1];

        //paramList[0] = new PassDataToSql("@Emp_Id", strEmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        //dtLeaveSummary = daClass.Reuturn_Datatable_Search("Att_Employee_Leave_Trans_SelectByEmpId", paramList);




        foreach (DataRow dr in dtTemp.Rows)
        {
            strsql = "SELECT DISTINCT Att_Employee_Leave_Trans.Trans_Id, Att_Employee_Leave_Trans.Company_Id, Att_Employee_Leave_Trans.Emp_Id, Att_Employee_Leave_Trans.Leave_Type_Id,                             Att_Employee_Leave_Trans.Year, Att_Employee_Leave_Trans.Month,                                                                                        (select left(datename(month,dateadd(month, Att_Employee_Leave_Trans.Month - 1, 0)),3)) as MonthName                                                                                  , Att_Employee_Leave_Trans.Previous_Days, Att_Employee_Leave_Trans.Assign_Days,                             Att_Employee_Leave_Trans.Total_Days, Att_Employee_Leave_Trans.Used_Days, Att_Employee_Leave_Trans.Remaining_Days,                             Att_Employee_Leave_Trans.Pending_Days, Att_Employee_Leave_Trans.Encash_Days, Att_Employee_Leave_Trans.Field1, Att_Employee_Leave_Trans.Field2,                             Att_Employee_Leave_Trans.Field3, Att_Employee_Leave_Trans.Field4, Att_Employee_Leave_Trans.Field5, Att_Employee_Leave_Trans.Field6,                             Att_Employee_Leave_Trans.Field7, Att_Employee_Leave_Trans.IsActive, Att_Employee_Leave_Trans.CreatedBy, Att_Employee_Leave_Trans.CreatedDate,                             Att_Employee_Leave_Trans.ModifiedBy, Att_Employee_Leave_Trans.ModifiedDate, Att_LeaveMaster.Leave_Name, Set_Att_Employee_Leave.Is_MonthCarry,                             Set_Att_Employee_Leave.Is_YearCarry, Set_Att_Employee_Leave.Percentage_Of_Salary, Set_Att_Employee_Leave.Shedule_Type,Set_Att_Employee_Leave.Field4 AS IsRule,Set_Att_Employee_Leave.Field5 AS IsAuto, Set_Att_Employee_Leave.Total_Leave AS TotalAssignedLeave  FROM         Att_Employee_Leave_Trans LEFT OUTER JOIN                            Set_Att_Employee_Leave ON Att_Employee_Leave_Trans.Emp_Id = Set_Att_Employee_Leave.Emp_Id AND                             Att_Employee_Leave_Trans.Leave_Type_Id = Set_Att_Employee_Leave.LeaveType_Id and Set_Att_Employee_Leave.IsActive='True'  LEFT OUTER JOIN                            Att_LeaveMaster ON Att_Employee_Leave_Trans.Leave_Type_Id = Att_LeaveMaster.Leave_Id      WHERE     (Att_Employee_Leave_Trans.Emp_Id = '" + strEmpId + "') AND (Att_Employee_Leave_Trans.IsActive = 'True')      ";

            dtLeaveSummary = daClass.return_DataTable(strsql, ref trns);
            //DataTable dtLeaveSummary = objEmpleave.GetEmployeeLeaveTransactionDataByEmpId(strEmpId);





            DateTime FromDate = Convert.ToDateTime(dr["From_Date"].ToString());
            DateTime ToDate = Convert.ToDateTime(dr["To_Date"].ToString());
            string strleaveTypeId = dr["Leave_Type_id"].ToString();
            int dayscount = Convert.ToInt32(dr["LeaveCount"].ToString());
            string month = FromDate.Month.ToString();


            if (dr["Field3"].ToString().Trim() == "Monthly")
            {
                string months = string.Empty;
                string year = string.Empty;

                DateTime FromDate2 = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId);
                DateTime ToDate2 = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).AddMonths(2);
                while (FromDate2 <= ToDate2)
                {
                    months += FromDate2.Month.ToString() + ",";
                    FromDate2 = FromDate2.AddMonths(1);
                    string year1 = FromDate2.Year.ToString();
                    if (!year.Split(',').Contains(year1))
                    {
                        year += year1 + ",";
                    }
                }

                DataTable dt = objAppParam.GetApplicationParameterByParamName("FinancialYearStartMonth", strCompanyId, ref trns, strBrandId, strLocationId);
                int FinancialYearMonth = 0;

                if (dt.Rows.Count > 0)
                {
                    FinancialYearMonth = int.Parse(dt.Rows[0]["Param_Value"].ToString());
                }

                DateTime FinancialYearStartDate = new DateTime();
                DateTime FinancialYearEndDate = new DateTime();
                if (Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Month < FinancialYearMonth)
                {
                    FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Year - 1, FinancialYearMonth, 1);
                    FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
                }
                else
                {
                    FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Year, FinancialYearMonth, 1);
                    FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
                }

                string year4 = string.Empty;
                string months4 = string.Empty;

                months4 = months;
                if (FinancialYearStartDate.Year == FinancialYearEndDate.Year)
                {
                    year4 = FinancialYearStartDate.Year.ToString();
                }
                else
                {
                    year4 += FinancialYearStartDate.Year.ToString() + ",";
                    year4 += FinancialYearEndDate.Year.ToString() + ",";
                }

                DateTime DateFrm = dtFromDate;
                if (!months4.Split(',').Contains(DateFrm.Month.ToString()) && !year4.Split(',').Contains(DateFrm.Year.ToString()))
                {

                    strResult[0] = "You Cannot Request Leave for This Month";

                    //DisplayMessage("You Cannot Request Leave for This Month");
                    //trns.Rollback();
                    //if (con.State == System.Data.ConnectionState.Open)
                    //{

                    //    con.Close();
                    //}
                    //trns.Dispose();
                    //con.Dispose();
                    //return;
                }

                //
                //this code is modify by jitendra upadhyay on 17-09-2014
                //this code for get the  remaining leave for selected leave type
                //code start
                dtLeaveSummary = new DataView(dtLeaveSummary, "Field3='Open' and Leave_Type_Id='" + strleaveTypeId + "' ", "", DataViewRowState.CurrentRows).ToTable();
                //code end
                int remainingdays = 0;
                if (dtLeaveSummary.Rows.Count > 0)
                {
                    remainingdays = int.Parse(dtLeaveSummary.Rows[0]["Remaining_Days"].ToString());
                }

                if (dayscount > remainingdays && !Common.LeaveNegativeBalance(strleaveTypeId, _strConString) && dr["Field4"].ToString() == "false")
                {
                    strResult[0] = "You do not have Sufficient Leave";
                    //DisplayMessage("You do not have Sufficient Leave");
                    //trns.Rollback();
                    //if (con.State == System.Data.ConnectionState.Open)
                    //{

                    //    con.Close();
                    //}
                    //trns.Dispose();
                    //con.Dispose();
                    //return;
                }
                else
                {

                    b = InsertLeaveRequest(strCompanyId, strleaveTypeId, strEmpId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), FromDate.ToString(), ToDate.ToString(), true.ToString(), false.ToString(), false.ToString(), dr["Emp_Description"].ToString(), "", dr["Field1"].ToString(), strMaxId, dr["Field3"].ToString(), dr["Field4"].ToString(), dr["Field5"].ToString(), true.ToString(), dr["Field7"].ToString(), true.ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), dr["Departure_Date"].ToString(), dr["Returning_Date"].ToString(), ref trns);



                    DataTable dtLeave = objEmpleave.GetEmployeeLeaveTransactionData(strEmpId, strleaveTypeId, FromDate.Month.ToString(), FromDate.Year.ToString(), ref trns);

                    string TransNo = string.Empty;
                    int remain = 0;
                    double useddays = 0;
                    int totaldays = 0;
                    int TotalPaiddays = 0;
                    int RemainPaidDays = 0;
                    int PendingDays = 0;
                    if (dtLeave.Rows.Count > 0)
                    {

                        TransNo = dtLeave.Rows[0]["Trans_Id"].ToString();
                        remain = int.Parse(dtLeave.Rows[0]["Remaining_Days"].ToString());
                        totaldays = int.Parse(dtLeave.Rows[0]["Total_Days"].ToString());
                        TotalPaiddays = int.Parse(dtLeave.Rows[0]["Field1"].ToString());
                        RemainPaidDays = int.Parse(dtLeave.Rows[0]["Field2"].ToString());
                        PendingDays = int.Parse(dtLeave.Rows[0]["Pending_Days"].ToString());
                    }

                    remain = remain - dayscount;
                    useddays = totaldays - remain;

                    dayscount = PendingDays + dayscount;
                    int PaidRemain = 0;

                    if (!Common.LeaveNegativeBalance(strleaveTypeId, _strConString))
                    {
                        if (RemainPaidDays > 0)
                        {
                            if (RemainPaidDays > dayscount)
                            {
                                PaidRemain = RemainPaidDays - dayscount;
                            }
                            else
                            {
                                PaidRemain = 0;
                            }
                        }
                    }
                    else
                    {
                        if (TotalPaiddays > 0)
                        {
                            PaidRemain = RemainPaidDays - dayscount;
                        }

                    }

                    try
                    {
                        UpdateEmployeeLeaveTransactionByTransNo(TransNo, strCompanyId, strEmpId, strleaveTypeId, FromDate.Year.ToString(), FromDate.Month.ToString(), "0", "0", "0", useddays.ToString(), remain.ToString(), dayscount.ToString(), PaidRemain.ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                    }
                    catch
                    {
                        strResult[0] = "Leave not Assigned to employee";

                        //DisplayMessage("Leave Type Not Assigned to Employee");
                        //trns.Rollback();
                        //if (con.State == System.Data.ConnectionState.Open)
                        //{

                        //    con.Close();
                        //}
                        //trns.Dispose();
                        //con.Dispose();
                        //return;
                    }

                    while (FromDate <= ToDate)
                    {
                        LeaveCounter++;
                        bool IsPaid = false;
                        if (!Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Count_On_WeekOff", strCompanyId, strBrandId, strLocationId, ref trns)))
                        {
                            int counter = 0;
                            string[] WeekOffDays = objAppParam.GetApplicationParameterValueByParamName("Week Off Days", strCompanyId, strBrandId, strLocationId, ref trns).Split(',');
                            for (int j = 0; j < WeekOffDays.Length; j++)
                            {
                                //Code Commented on 22-08-2023 By Lokesh for Alpha Medicals
                                //if (WeekOffDays[j].ToString() == FromDate.DayOfWeek.ToString())
                                //{
                                //    counter = 1;
                                //}

                                if (FromDate.DayOfWeek.ToString() == "Friday")
                                {
                                    counter = 1;
                                }
                            }
                            if (objEmpHoliday.GetEmployeeHolidayOnDateAndEmpId(FromDate.ToString(), strEmpId, ref trns))
                            {
                                counter = 1;
                            }
                            if (counter == 0)
                            {

                                if (!Common.LeaveNegativeBalance(strleaveTypeId, _strConString))
                                {
                                    if (RemainPaidDays > 0)
                                    {
                                        IsPaid = true;
                                        RemainPaidDays--;
                                    }
                                }
                                else
                                {
                                    if (TotalPaiddays != 0)
                                    {
                                        IsPaid = true;
                                        RemainPaidDays--;
                                    }
                                }
                                InsertLeaveRequestChild(b.ToString(), strleaveTypeId, FromDate.ToString(), IsPaid.ToString(), "1", "", "", "", "", "", false.ToString(), Rejoin_Date.ToString(), true.ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

                            }

                        }
                        else
                        {
                            if (!Common.LeaveNegativeBalance(strleaveTypeId, _strConString))
                            {
                                if (RemainPaidDays > 0)
                                {
                                    IsPaid = true;
                                    RemainPaidDays--;
                                }
                            }
                            else
                            {
                                if (TotalPaiddays != 0)
                                {
                                    IsPaid = true;
                                    RemainPaidDays--;
                                }
                            }
                            InsertLeaveRequestChild(b.ToString(), strleaveTypeId, FromDate.ToString(), IsPaid.ToString(), "1", "", "", "", "", "", false.ToString(), Rejoin_Date.ToString(), true.ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

                        }


                        FromDate = FromDate.AddDays(1);

                    }

                }

            }

            else if (dr["Field3"].ToString().Trim() == "Yearly")
            {
                string year4 = string.Empty;
                int FinancialYearMonth = 0;
                try
                {
                    FinancialYearMonth = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("FinancialYearStartMonth", strCompanyId, strBrandId, strLocationId, ref trns));
                }
                catch
                {
                    FinancialYearMonth = 1;
                }

                DateTime FinancialYearStartDate = new DateTime();
                DateTime FinancialYearEndDate = new DateTime();
                if (Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Month < FinancialYearMonth)
                {

                    FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Year - 1, FinancialYearMonth, 1);

                    FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
                }
                else
                {
                    FinancialYearStartDate = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Year, FinancialYearMonth, 1);
                    FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);

                }


                if (FinancialYearStartDate.Year == FinancialYearEndDate.Year)
                {
                    year4 = FinancialYearStartDate.Year.ToString();


                }
                else
                {
                    year4 = FinancialYearStartDate.Year.ToString();


                }

                dtLeaveSummary = new DataView(dtLeaveSummary, "Leave_Type_Id='" + strleaveTypeId + "' and Field3='Open'", "", DataViewRowState.CurrentRows).ToTable();

                double remainingdays = 0;
                if (dtLeaveSummary.Rows.Count > 0)
                {
                    remainingdays = Convert.ToDouble(dtLeaveSummary.Rows[0]["Remaining_Days"].ToString());

                }
                if (dayscount > remainingdays && !Common.LeaveNegativeBalance(strleaveTypeId, _strConString) && dr["Field4"].ToString() == "false")
                {

                    strResult[0] = "You do not have Sufficient Leave";


                }
                else
                {



                    //this code is created by jitendra upadhyay on 28-03-2015
                    //here we delete record of selected employee by transid in leave request and leaverequestchild table and also from approval table and also delete from approval trasnaction

                    //code start

                    int strPreviousDays = 0;




                    b = InsertLeaveRequest(strCompanyId, strleaveTypeId, strEmpId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), FromDate.ToString(), ToDate.ToString(), IsPending.ToString(), IsApproved.ToString(), false.ToString(), dr["Emp_Description"].ToString(), "", dr["Field1"].ToString(), strMaxId, dr["Field3"].ToString(), dr["Field4"].ToString(), dr["Field5"].ToString(), true.ToString(), dr["Field7"].ToString(), true.ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), dr["Departure_Date"].ToString(), dr["Returning_Date"].ToString(), ref trns);




                    DataTable dtLeave = daClass.return_DataTable("select * from Att_Employee_Leave_Trans where Emp_Id=" + strEmpId + " and Leave_Type_Id=" + strleaveTypeId + " and Field3='Open' and IsActive='True'", ref trns);

                    //DataTable dtLeave = objEmpleave.GetEmployeeLeaveTransactionData(strEmpId, strleaveTypeId, "0", year4, ref trns);

                    string TransNo = string.Empty;
                    double remain = 0;
                    double useddays = 0;
                    int totaldays = 0;
                    double PaidRemain = 0;
                    double TotalPaiddays = 0;
                    double RemainPaidDays = 0;
                    int PendingDays = 0;
                    if (dtLeave.Rows.Count > 0)
                    {

                        TransNo = dtLeave.Rows[0]["Trans_Id"].ToString();
                        remain = Convert.ToDouble(dtLeave.Rows[0]["Remaining_Days"].ToString());
                        totaldays = int.Parse(dtLeave.Rows[0]["Total_Days"].ToString());
                        TotalPaiddays = Convert.ToDouble(dtLeave.Rows[0]["Field1"].ToString());
                        RemainPaidDays = Convert.ToDouble(dtLeave.Rows[0]["Field2"].ToString());
                        PendingDays = int.Parse(dtLeave.Rows[0]["Pending_Days"].ToString());
                        useddays = Convert.ToDouble(dtLeave.Rows[0]["Used_Days"].ToString());

                        if (hdnTransid != "")
                        {
                            PendingDays = PendingDays - strPreviousDays;
                            remain = remain + strPreviousDays;
                            RemainPaidDays = RemainPaidDays + strPreviousDays;
                            //PendingDays = 0;
                        }

                        //code end
                    }

                    remain = remain - dayscount;

                    if (LeaveApprovalFunctionality)
                    {
                        PendingDays = PendingDays + dayscount;
                    }
                    else
                    {
                        useddays = useddays + dayscount;
                    }

                    // useddays = totaldays;
                    // dayscount = dayscount + PendingDays;


                    if (!Common.LeaveNegativeBalance(strleaveTypeId, _strConString))
                    {

                        if (RemainPaidDays > 0)
                        {
                            if (RemainPaidDays > dayscount)
                            {
                                PaidRemain = RemainPaidDays - dayscount;
                            }
                            else
                            {
                                PaidRemain = 0;
                            }
                        }
                    }
                    else
                    {
                        if (TotalPaiddays > 0)
                        {
                            PaidRemain = RemainPaidDays - dayscount;
                        }
                    }





                    try
                    {
                        UpdateEmployeeLeaveTransactionByTransNo(TransNo, strCompanyId, strEmpId, strleaveTypeId, year4, "0", "0", "0", "0", useddays.ToString(), remain.ToString(), PendingDays.ToString(), PaidRemain.ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                    }
                    catch (Exception ex)
                    {
                        strResult[0] = "Leave not Assigned to employee";
                        //DisplayMessage("Leave not Assigned to employee");
                        //trns.Rollback();
                        //if (con.State == System.Data.ConnectionState.Open)
                        //{

                        //    con.Close();
                        //}
                        //trns.Dispose();
                        //con.Dispose();
                        //return;
                    }

                    while (FromDate <= ToDate)
                    {
                        LeaveCounter++;
                        bool IsPaid = false;

                        //DataTable dtLeave = daClass.return_DataTable("Select Param_Value From  Set_ApplicationParameter  Where  Company_Id = '"+ strCompanyId +"' and Brand_Id ='"+strBrandId +"' and Location_Id '"+ strLocationId +"' and   Param_Name='Leave_Count_On_WeekOff'Leave_Count_On_WeekOff", ref trns);
                        //if (!Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Count_On_WeekOff", strCompanyId, strBrandId, strLocationId, ref trns)))
                        if (!Convert.ToBoolean(daClass.return_DataTable("Select Param_Value From  Set_ApplicationParameter  Where  Company_Id = '" + strCompanyId + "' and Brand_Id ='" + strBrandId + "' and Location_Id ='" + strLocationId + "' and   Param_Name='Leave_Count_On_WeekOff'", ref trns).Rows[0][0].ToString()))
                        {
                            int counter = 0;

                            //daClass.return_DataTable("Select Param_Value From  Set_ApplicationParameter  Where  Company_Id = '" + strCompanyId + "' and Brand_Id ='" + strBrandId + "' and Location_Id '" + strLocationId + "' and   Param_Name='Week Off Days'", ref trns).Rows[0][0].ToString()
                            // string[] WeekOffDays = objAppParam.GetApplicationParameterValueByParamName("Week Off Days", strCompanyId, strBrandId, strLocationId, ref trns).Split(',');

                            string[] WeekOffDays = daClass.return_DataTable("Select Param_Value From  Set_ApplicationParameter  Where  Company_Id = '" + strCompanyId + "' and Brand_Id ='" + strBrandId + "' and Location_Id ='" + strLocationId + "' and   Param_Name='Week Off Days'", ref trns).Rows[0][0].ToString().Split(',');
                            for (int j = 0; j < WeekOffDays.Length; j++)
                            {
                                //Code Commented on 22-08-2023 By Lokesh for Alpha Medicals
                                //if (WeekOffDays[j].ToString() == FromDate.DayOfWeek.ToString())
                                //{
                                //    counter = 1;
                                //}
                                if (FromDate.DayOfWeek.ToString() == "Friday")
                                {
                                    counter = 1;
                                }
                            }
                            if (GetEmployeeHolidayOnDateAndEmpId(FromDate.ToString(), strEmpId, ref trns))
                            {
                                counter = 1;
                            }
                            if (counter == 0)
                            {
                                // if (!Common.LeaveNegativeBalance(strleaveTypeId, _strConString))
                                if (!Convert.ToBoolean(daClass.return_DataTable("select is_partial from Att_LeaveMaster where Leave_Id=" + strleaveTypeId + "", ref trns).Rows[0][0].ToString()))
                                {
                                    if (RemainPaidDays > 0)
                                    {
                                        IsPaid = true;
                                        RemainPaidDays--;
                                    }
                                }
                                else
                                {

                                    if (TotalPaiddays != 0)
                                    {
                                        IsPaid = true;
                                        RemainPaidDays--;
                                    }

                                }
                                InsertLeaveRequestChild(b.ToString(), strleaveTypeId, FromDate.ToString(), IsPaid.ToString(), "1", "", "", "", "", "", false.ToString(), Rejoin_Date.ToString(), true.ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

                            }
                        }
                        else
                        {
                            //if (!Common.LeaveNegativeBalance(strleaveTypeId, _strConString))
                            if (!Convert.ToBoolean(daClass.return_DataTable("select is_partial from Att_LeaveMaster where Leave_Id=" + strleaveTypeId + "", ref trns).Rows[0][0].ToString()))
                            {
                                if (RemainPaidDays > 0)
                                {
                                    IsPaid = true;
                                    RemainPaidDays--;
                                }
                                else
                                {
                                    if (TotalPaiddays != 0)
                                    {
                                        IsPaid = true;
                                    }
                                }
                            }
                            else
                            {
                                if (TotalPaiddays != 0)
                                {
                                    IsPaid = true;
                                    RemainPaidDays--;
                                }
                            }
                            InsertLeaveRequestChild(b.ToString(), strleaveTypeId, FromDate.ToString(), IsPaid.ToString(), "1", "", "", "", "", "", false.ToString(), Rejoin_Date.ToString(), true.ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                        }

                        FromDate = FromDate.AddDays(1);

                    }
                }
            }
        }


        //daClass.execute_Command("update Att_LeaveRequest_header set TotalDays='" + LeaveCounter.ToString() + "' where Trans_Id=" + strMaxId + "", ref trns);

        strResult[1] = strMaxId;

        return strResult;
    }
    public bool GetEmployeeHolidayOnDateAndEmpId(string date, string empid, ref SqlTransaction trns)
    {
        bool isholiday = false;
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Holiday_Date", date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);


        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Employee_Holiday_EmpId", paramList, ref trns);

        if (dtInfo.Rows.Count > 0)
        {
            isholiday = true;
        }
        return isholiday;
    }
    public string UpdateImagePath(string TransId, string FileName, string FilePath)
    {
        int a = daClass.execute_Command("Update Att_Leave_Request Set Field4='" + FileName + "',Field5='" + FilePath + "' where Trans_Id='" + TransId + "'", "");
        return (a.ToString());
    }




    public string InsertLeaveEnCashmentApproval(string strEmpId, string EmpPermission, DataTable dtApproval, string strMaxId, DataTable dtLeaveDetail, string strEmpName, string hdnTransid, string strEmpImage, DateTime dtFromDate, DateTime dtToDate, bool IsLeaveRequest, bool LeaveApprovalFunctionality, string strCompId, string strBrandId, string strLocId, string strTimeZoneId, string strUserid, string strAbsoluteUri, string strLoginEmpId)
    {
        Set_Approval_Employee objApproalEmp = new Set_Approval_Employee(_strConString);
        Set_ApprovalMaster ObjApproval = new Set_ApprovalMaster(_strConString);
        SendMailSms ObjSendMailSms = new SendMailSms(_strConString);
        EmployeeMaster objEmp = new EmployeeMaster(_strConString);
        SystemParameter objSys = new SystemParameter(_strConString);
        CompanyMaster objComp = new CompanyMaster(_strConString);
        Set_ApplicationParameter ObjParam = new Set_ApplicationParameter(_strConString);
        string strStatus = "Pending";
        if (IsLeaveRequest && !LeaveApprovalFunctionality)
        {
            strStatus = "Approved";
        }
        string strRequestEmpCode = Common.GetEmployeeCode(strEmpId, _strConString, strCompId);
        string strRequestEmpName = Common.GetEmployeeName(strEmpId, _strConString, strCompId);
        string strRequestDepName = Common.GetDepartmentName(strEmpId, _strConString, strCompId);
        string strRequestdoj = Common.GetEmployeeDateOfJoining(strEmpId, _strConString, strCompId);
        string strCompanyName = objComp.GetCompanyMasterById(strCompId).Rows[0]["Company_Name"].ToString();
        if (dtApproval.Rows.Count > 0)
        {
            for (int j = 0; j < dtApproval.Rows.Count; j++)
            {
                int cur_trans_id = 0;
                string PriorityEmpId = dtApproval.Rows[j]["Emp_Id"].ToString();
                string IsPriority = dtApproval.Rows[j]["Priority"].ToString();
                if (EmpPermission == "1")
                {
                    cur_trans_id = objApproalEmp.InsertApprovalTransaciton(dtApproval.Rows[j]["Approval_Id"].ToString(), strCompId, "0", "0", "0", strEmpId, strMaxId, PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), "01/01/1900", strStatus, "", "", "", "", "", "", "", "", true.ToString(), strUserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString());

                }
                else if (EmpPermission == "2")
                {
                    cur_trans_id = objApproalEmp.InsertApprovalTransaciton(dtApproval.Rows[j]["Approval_Id"].ToString(), strCompId, strBrandId, "0", "0", strEmpId, strMaxId, PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), "01/01/1900", strStatus, "", "", "", "", "", "", "", "", true.ToString(), strUserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString());

                }
                else if (EmpPermission == "3")
                {
                    cur_trans_id = objApproalEmp.InsertApprovalTransaciton(dtApproval.Rows[j]["Approval_Id"].ToString(), strCompId, strBrandId, strLocId, "0", strEmpId, strMaxId, PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), "01/01/1900", strStatus, "", "", "", "", "", "", "", "", true.ToString(), strUserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString());

                }
                else
                {
                    cur_trans_id = objApproalEmp.InsertApprovalTransaciton(dtApproval.Rows[j]["Approval_Id"].ToString(), strCompId, strBrandId, strLocId, "0", strEmpId, strMaxId, PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), "01/01/1900", strStatus, "", "", "", "", "", "", "", "", true.ToString(), strUserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString());
                }

                // Insert Notification For Leave by  ghanshyam suthar
                //Session["PriorityEmpId"] = PriorityEmpId;
                //Session["cur_trans_id"] = cur_trans_id;

                if (IsLeaveRequest && LeaveApprovalFunctionality)
                {
                    //Set_Notification(strEmpId, PriorityEmpId, dtLeaveDetail, strEmpName, hdnTransid, cur_trans_id.ToString(), strEmpImage, dtFromDate.ToString(objSys.SetDateFormat()), dtToDate.ToString(objSys.SetDateFormat()), strAbsoluteUri, strCompId, strBrandId, strLocId, strLoginEmpId, strTimeZoneId, strUserid);

                    //--------------------------------------

                    //code for only insert mode  so added this condition 
                    if (hdnTransid == "" && Convert.ToBoolean(ObjParam.GetApplicationParameterValueByParamName("Send_Email_OnLeaveRequest", strCompId, strBrandId, strLocId)))
                    {

                        string strPendingApproval = string.Empty;
                        //for Email Code
                        if ((IsPriority == "True" && ObjApproval.GetApprovalMasterById("1").Rows[0]["Approval_Type"].ToString() == "Priority") || (dtApproval.Rows[j]["Field1"].ToString().Trim() == "True" && ObjApproval.GetApprovalMasterById("1").Rows[0]["Approval_Type"].ToString() == "Hierarchy"))
                        {
                            if (PriorityEmpId != "" && PriorityEmpId != "0")
                            {
                                DataTable dtEmpDetail = objEmp.GetEmployeeMasterById(strCompId, PriorityEmpId);
                                if (dtEmpDetail.Rows.Count > 0)
                                {
                                    if (dtEmpDetail.Rows[0]["Email_Id"].ToString().Trim() != "")
                                    {
                                        //if hierarchy system then we will find next level name

                                        string strhigherHodName = string.Empty;
                                        if (ObjApproval.GetApprovalMasterById("1").Rows[0]["Approval_Type"].ToString() == "Hierarchy" && dtApproval.Rows.Count > 1)
                                        {
                                            strPendingApproval = "<br /> Pending Approval : " + Common.GetEmployeeName(dtApproval.Rows[1]["Emp_Id"].ToString(), _strConString, strCompId);
                                        }

                                        string MailMessage = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Dear " + Common.GetEmployeeName(PriorityEmpId, _strConString, strCompId) + "</h4> <hr /> Find below the pending leave application for your Approval <br /><br /> Employee Id : " + strRequestEmpCode + "<br /> Employee Name : " + strRequestEmpName + "<br /> Department : " + strRequestDepName + "<br /> Date of join :" + strRequestdoj + strPendingApproval + " " + Common.GetmailContentByLeaveTypeId(strMaxId, strEmpId, _strConString, strCompId) + "<br /><h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h3> HR Department</h3> <h2> " + strCompanyName + "</h2> </div> </body> </html>";
                                        //MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + Common.GetEmployeeName(PriorityEmpId) + "</h4><hr/><p>Request for Full Day Leave Type of '" + GetLeaveTypeName(ddlLeaveType.SelectedValue) + "' By '" +Common.GetEmployeeName(Session["EmpId"].ToString()) + "'<br />From Date '" + txtFromDate.Text + "' To Date '" + txtToDate.Text + "'<br /> and Given Reason is '" + txtDescription.Text + "'</p><h3>Thanks & Regards</h3><h3>" +Common.GetEmployeeName(Session["EmpId"].ToString()) + "</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";

                                        //MailMessage = "Working";

                                        try
                                        {
                                            ObjSendMailSms.SendApprovalMail(dtEmpDetail.Rows[0]["Email_Id"].ToString(), Common.GetMasterEmailId(_strConString, strCompId, strBrandId, strLocId), Common.GetMasterEmailPassword(_strConString, strCompId, strBrandId, strLocId), "Time Man: Full Day Leave Apply By '" + Common.GetEmployeeName(strEmpId, _strConString, strCompId) + "' From '" + dtFromDate.ToString(objSys.SetDateFormat()) + "' To '" + dtToDate.ToString(objSys.SetDateFormat()) + "'", MailMessage.ToString(), strCompId, "", "", strBrandId, strLocId);
                                        }
                                        catch
                                        {
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Set_Notification(strEmpId, PriorityEmpId, null, strEmpName, hdnTransid, cur_trans_id.ToString(), strEmpImage, dtFromDate.ToString(objSys.SetDateFormat()), dtToDate.ToString(objSys.SetDateFormat()), strAbsoluteUri, strCompId, strBrandId, strLocId, strLoginEmpId, strTimeZoneId, strUserid);
                }

            }

        }

        return "";
    }









    public string InsertLeaveApproval(string strEmpId, string EmpPermission, DataTable dtApproval, string strMaxId, DataTable dtLeaveDetail, string strEmpName, string hdnTransid, string strEmpImage, DateTime dtFromDate, DateTime dtToDate, bool IsLeaveRequest, bool LeaveApprovalFunctionality, string strCompId, string strBrandId, string strLocId, string strTimeZoneId, string strUserid, string strAbsoluteUri, string strLoginEmpId)
    {
        Set_Approval_Employee objApproalEmp = new Set_Approval_Employee(_strConString);
        Set_ApprovalMaster ObjApproval = new Set_ApprovalMaster(_strConString);
        SendMailSms ObjSendMailSms = new SendMailSms(_strConString);
        EmployeeMaster objEmp = new EmployeeMaster(_strConString);
        SystemParameter objSys = new SystemParameter(_strConString);
        CompanyMaster objComp = new CompanyMaster(_strConString);
        Set_ApplicationParameter ObjParam = new Set_ApplicationParameter(_strConString);
        string strStatus = "Pending";
        if (IsLeaveRequest && !LeaveApprovalFunctionality)
        {
            strStatus = "Approved";
        }
        string strRequestEmpCode = Common.GetEmployeeCode(strEmpId, _strConString, strCompId);
        string strRequestEmpName = Common.GetEmployeeName(strEmpId, _strConString, strCompId);
        string strRequestDepName = Common.GetDepartmentName(strEmpId, _strConString, strCompId);
        string strRequestdoj = Common.GetEmployeeDateOfJoining(strEmpId, _strConString, strCompId);
        string strCompanyName = objComp.GetCompanyMasterById(strCompId).Rows[0]["Company_Name"].ToString();
        if (dtApproval.Rows.Count > 0)
        {
            for (int j = 0; j < dtApproval.Rows.Count; j++)
            {
                int cur_trans_id = 0;
                string PriorityEmpId = dtApproval.Rows[j]["Emp_Id"].ToString();
                string IsPriority = dtApproval.Rows[j]["Priority"].ToString();
                if (EmpPermission == "1")
                {
                    cur_trans_id = objApproalEmp.InsertApprovalTransaciton(dtApproval.Rows[j]["Approval_Id"].ToString(), strCompId, "0", "0", "0", strEmpId, strMaxId, PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), "01/01/1900", strStatus, "", "", "", "", "", "", "", "", true.ToString(), strUserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString());

                }
                else if (EmpPermission == "2")
                {
                    cur_trans_id = objApproalEmp.InsertApprovalTransaciton(dtApproval.Rows[j]["Approval_Id"].ToString(), strCompId, strBrandId, "0", "0", strEmpId, strMaxId, PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), "01/01/1900", strStatus, "", "", "", "", "", "", "", "", true.ToString(), strUserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString());

                }
                else if (EmpPermission == "3")
                {
                    cur_trans_id = objApproalEmp.InsertApprovalTransaciton(dtApproval.Rows[j]["Approval_Id"].ToString(), strCompId, strBrandId, strLocId, "0", strEmpId, strMaxId, PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), "01/01/1900", strStatus, "", "", "", "", "", "", "", "", true.ToString(), strUserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString());

                }
                else
                {
                    cur_trans_id = objApproalEmp.InsertApprovalTransaciton(dtApproval.Rows[j]["Approval_Id"].ToString(), strCompId, strBrandId, strLocId, "0", strEmpId, strMaxId, PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), "01/01/1900", strStatus, "", "", "", "", "", "", "", "", true.ToString(), strUserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString());
                }

                // Insert Notification For Leave by  ghanshyam suthar
                //Session["PriorityEmpId"] = PriorityEmpId;
                //Session["cur_trans_id"] = cur_trans_id;

                if (IsLeaveRequest && LeaveApprovalFunctionality)
                {
                    Set_Notification(strEmpId, PriorityEmpId, dtLeaveDetail, strEmpName, hdnTransid, cur_trans_id.ToString(), strEmpImage, dtFromDate.ToString(objSys.SetDateFormat()), dtToDate.ToString(objSys.SetDateFormat()), strAbsoluteUri, strCompId, strBrandId, strLocId, strLoginEmpId, strTimeZoneId, strUserid);

                    //--------------------------------------

                    //code for only insert mode  so added this condition 
                    if (hdnTransid == "" && Convert.ToBoolean(ObjParam.GetApplicationParameterValueByParamName("Send_Email_OnLeaveRequest", strCompId, strBrandId, strLocId)))
                    {

                        string strPendingApproval = string.Empty;
                        //for Email Code
                        if ((IsPriority == "True" && ObjApproval.GetApprovalMasterById("1").Rows[0]["Approval_Type"].ToString() == "Priority") || (dtApproval.Rows[j]["Field1"].ToString().Trim() == "True" && ObjApproval.GetApprovalMasterById("1").Rows[0]["Approval_Type"].ToString() == "Hierarchy"))
                        {
                            if (PriorityEmpId != "" && PriorityEmpId != "0")
                            {
                                DataTable dtEmpDetail = objEmp.GetEmployeeMasterById(strCompId, PriorityEmpId);
                                if (dtEmpDetail.Rows.Count > 0)
                                {
                                    if (dtEmpDetail.Rows[0]["Email_Id"].ToString().Trim() != "")
                                    {
                                        //if hierarchy system then we will find next level name

                                        string strhigherHodName = string.Empty;
                                        if (ObjApproval.GetApprovalMasterById("1").Rows[0]["Approval_Type"].ToString() == "Hierarchy" && dtApproval.Rows.Count > 1)
                                        {
                                            strPendingApproval = "<br /> Pending Approval : " + Common.GetEmployeeName(dtApproval.Rows[1]["Emp_Id"].ToString(), _strConString, strCompId);
                                        }

                                        string MailMessage = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Dear " + Common.GetEmployeeName(PriorityEmpId, _strConString, strCompId) + "</h4> <hr /> Find below the pending leave application for your Approval <br /><br /> Employee Id : " + strRequestEmpCode + "<br /> Employee Name : " + strRequestEmpName + "<br /> Department : " + strRequestDepName + "<br /> Date of join :" + strRequestdoj + strPendingApproval + " " + Common.GetmailContentByLeaveTypeId(strMaxId, strEmpId, _strConString, strCompId) + "<br /><h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h3> HR Department</h3> <h2> " + strCompanyName + "</h2> </div> </body> </html>";
                                        //MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + Common.GetEmployeeName(PriorityEmpId) + "</h4><hr/><p>Request for Full Day Leave Type of '" + GetLeaveTypeName(ddlLeaveType.SelectedValue) + "' By '" +Common.GetEmployeeName(Session["EmpId"].ToString()) + "'<br />From Date '" + txtFromDate.Text + "' To Date '" + txtToDate.Text + "'<br /> and Given Reason is '" + txtDescription.Text + "'</p><h3>Thanks & Regards</h3><h3>" +Common.GetEmployeeName(Session["EmpId"].ToString()) + "</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";

                                        //MailMessage = "Working";

                                        try
                                        {
                                            ObjSendMailSms.SendApprovalMail(dtEmpDetail.Rows[0]["Email_Id"].ToString(), Common.GetMasterEmailId(_strConString, strCompId, strBrandId, strLocId), Common.GetMasterEmailPassword(_strConString, strCompId, strBrandId, strLocId), "Time Man: Full Day Leave Apply By '" + Common.GetEmployeeName(strEmpId, _strConString, strCompId) + "' From '" + dtFromDate.ToString(objSys.SetDateFormat()) + "' To '" + dtToDate.ToString(objSys.SetDateFormat()) + "'", MailMessage.ToString(), strCompId, "", "", strBrandId, strLocId);
                                        }
                                        catch
                                        {
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Set_Notification(strEmpId, PriorityEmpId, null, strEmpName, hdnTransid, cur_trans_id.ToString(), strEmpImage, dtFromDate.ToString(objSys.SetDateFormat()), dtToDate.ToString(objSys.SetDateFormat()), strAbsoluteUri, strCompId, strBrandId, strLocId, strLoginEmpId, strTimeZoneId, strUserid);
                }

            }

        }

        return "";
    }

    public string InsertLeaveApproval(string strCompanyId, string strBrandId, string strLocationId, string strUserId, string strEmpId, string EmpPermission, DataTable dtApproval, string strMaxId, DataTable dtLeaveDetail, string strEmpName, string hdnTransid, string strEmpImage, DateTime dtFromDate, DateTime dtToDate, bool IsLeaveRequest, bool LeaveApprovalFunctionality, ref SqlTransaction trns, string strTimeZoneId, string strAbsoluteUri)
    {
        Set_Approval_Employee objApproalEmp = new Set_Approval_Employee(_strConString);
        Set_ApprovalMaster ObjApproval = new Set_ApprovalMaster(_strConString);
        SendMailSms ObjSendMailSms = new SendMailSms(_strConString);
        EmployeeMaster objEmp = new EmployeeMaster(_strConString);
        SystemParameter objSys = new SystemParameter(_strConString);
        CompanyMaster objComp = new CompanyMaster(_strConString);


        Set_ApplicationParameter ObjParam = new Set_ApplicationParameter(_strConString);

        //string strRequestEmpCode = Common.GetEmployeeCode(strCompanyId, strEmpId, ref trns);
        //string strRequestEmpName = Common.GetEmployeeName(strCompanyId, strEmpId, ref trns);
        //string strRequestDepName = Common.GetDepartmentName(strCompanyId, strEmpId, ref trns);
        //string strRequestdoj = Common.GetEmployeeDateOfJoining(strCompanyId, strEmpId, ref trns);
        //string strCompanyName = objComp.GetCompanyMasterById(strCompanyId, ref trns).Rows[0]["Company_Name"].ToString();


        string strRequestEmpCode = "";
        string strRequestEmpName = "";
        string strRequestDepName = "";
        string strRequestdoj = "";
        string strCompanyName = "";


        string strStatus = "Pending";

        if (IsLeaveRequest && !LeaveApprovalFunctionality)
        {
            strStatus = "Approved";
        }


        if (dtApproval.Rows.Count > 0)
        {
            for (int j = 0; j < dtApproval.Rows.Count; j++)
            {
                int cur_trans_id = 0;
                string PriorityEmpId = dtApproval.Rows[j]["Emp_Id"].ToString();
                string IsPriority = dtApproval.Rows[j]["Priority"].ToString();
                if (EmpPermission == "1")
                {
                    cur_trans_id = objApproalEmp.InsertApprovalTransaciton(dtApproval.Rows[j]["Approval_Id"].ToString(), strCompanyId, "0", "0", "0", strEmpId, strMaxId, PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), "01/01/1900", strStatus, "", "", "", "", "", "", "", "", true.ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

                }
                else if (EmpPermission == "2")
                {
                    cur_trans_id = objApproalEmp.InsertApprovalTransaciton(dtApproval.Rows[j]["Approval_Id"].ToString(), strCompanyId, strBrandId, "0", "0", strEmpId, strMaxId, PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), "01/01/1900", strStatus, "", "", "", "", "", "", "", "", true.ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

                }
                else if (EmpPermission == "3")
                {
                    cur_trans_id = objApproalEmp.InsertApprovalTransaciton(dtApproval.Rows[j]["Approval_Id"].ToString(), strCompanyId, strBrandId, strLocationId, "0", strEmpId, strMaxId, PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), "01/01/1900", strStatus, "", "", "", "", "", "", "", "", true.ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

                }
                else
                {
                    cur_trans_id = objApproalEmp.InsertApprovalTransaciton(dtApproval.Rows[j]["Approval_Id"].ToString(), strCompanyId, strBrandId, strLocationId, "0", strEmpId, strMaxId, PriorityEmpId, IsPriority, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), "01/01/1900", strStatus, "", "", "", "", "", "", "", "", true.ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                }

                // Insert Notification For Leave by  ghanshyam suthar
                //Session["PriorityEmpId"] = PriorityEmpId;
                //Session["cur_trans_id"] = cur_trans_id;

                if (IsLeaveRequest && LeaveApprovalFunctionality)
                {
                    Set_Notification(strCompanyId, strBrandId, strLocationId, strUserId, strEmpId, PriorityEmpId, dtLeaveDetail, strEmpName, hdnTransid, cur_trans_id.ToString(), strEmpImage, dtFromDate.ToString(objSys.SetDateFormat(ref trns)), dtToDate.ToString(objSys.SetDateFormat(ref trns)), ref trns, strAbsoluteUri, strTimeZoneId);

                    //--------------------------------------

                    //code for only insert mode  so added this condition 
                    if (hdnTransid == "" && Convert.ToBoolean(ObjParam.GetApplicationParameterValueByParamName("Send_Email_OnLeaveRequest", strCompanyId, strBrandId, strLocationId)))
                    {

                        string strPendingApproval = string.Empty;
                        //for Email Code
                        if ((IsPriority == "True" && ObjApproval.GetApprovalMasterById("1").Rows[0]["Approval_Type"].ToString() == "Priority") || (dtApproval.Rows[j]["Field1"].ToString().Trim() == "True" && ObjApproval.GetApprovalMasterById("1", ref trns).Rows[0]["Approval_Type"].ToString() == "Hierarchy"))
                        {
                            if (PriorityEmpId != "" && PriorityEmpId != "0")
                            {
                                DataTable dtEmpDetail = objEmp.GetEmployeeMasterById(strCompanyId, PriorityEmpId, ref trns);
                                if (dtEmpDetail.Rows.Count > 0)
                                {
                                    if (dtEmpDetail.Rows[0]["Email_Id"].ToString().Trim() != "")
                                    {
                                        //if hierarchy system then we will find next level name

                                        string strhigherHodName = string.Empty;
                                        if (ObjApproval.GetApprovalMasterById("1", ref trns).Rows[0]["Approval_Type"].ToString() == "Hierarchy" && dtApproval.Rows.Count > 1)
                                        {
                                            strPendingApproval = "<br /> Pending Approval : " + Common.GetEmployeeName(strCompanyId, dtApproval.Rows[1]["Emp_Id"].ToString(), ref trns);
                                        }

                                        string MailMessage = "<html><head> <meta http-equiv='Content-Type' content='text/html; charset=utf-8' /> <title></title> <style> h4 { font-family: Arial, Helvetica, sans-serif; font-size: 16px; letter-spacing: 1px; margin-bottom: 30px; } h2 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; color: #990000; } h3 { font-family: Arial, Helvetica, sans-serif; font-size: 18px; letter-spacing: 1px; } p { font-family: Arial, Helvetica, sans-serif; line-height: 25px; margin-bottom: 70px; margin-left: 50px; font-size: 14px; } hr { border: #cccccc solid 1px; margin-top: -80px; width: 100%; float: left; } </style></head><body> <div> <h4> Dear " + Common.GetEmployeeName(strCompanyId, PriorityEmpId, ref trns) + "</h4> <hr /> Find below the pending leave application for your Approval <br /><br /> Employee Id : " + strRequestEmpCode + "<br /> Employee Name : " + strRequestEmpName + "<br /> Department : " + strRequestDepName + "<br /> Date of join :" + strRequestdoj + strPendingApproval + " " + Common.GetmailContentByLeaveTypeId(strMaxId, strEmpId, ref trns, strCompanyId) + "<br /><h4>Please note that this is an auto generate email and request not to reply to the sender </h4><br /><br /> <h3> Thanks & Regards</h3> <h3> HR Department</h3> <h2> " + strCompanyName + "</h2> </div> </body> </html>";
                                        //MailMessage = "<html><head><meta http-equiv='Content-Type content='text/html; charset=utf-8' /><title></title><style>h4 { font-family:Arial, Helvetica, sans-serif; font-size:16px; letter-spacing:1px;margin-bottom:30px;}h2 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px; color:#990000;}h3 { font-family:Arial, Helvetica, sans-serif; font-size:18px; letter-spacing:1px;}p { font-family:Arial, Helvetica, sans-serif; line-height:25px; margin-bottom:70px;margin-left:50px; font-size:14px;}hr {border:#cccccc solid 1px; margin-top:-80px; width:100%; float:left;}</style></head><body><div><h4>Dear " + Common.GetEmployeeName(PriorityEmpId) + "</h4><hr/><p>Request for Full Day Leave Type of '" + GetLeaveTypeName(ddlLeaveType.SelectedValue) + "' By '" +Common.GetEmployeeName(Session["EmpId"].ToString()) + "'<br />From Date '" + txtFromDate.Text + "' To Date '" + txtToDate.Text + "'<br /> and Given Reason is '" + txtDescription.Text + "'</p><h3>Thanks & Regards</h3><h3>" +Common.GetEmployeeName(Session["EmpId"].ToString()) + "</h3><h2>" + objComp.GetCompanyMasterById(Session["CompId"].ToString()).Rows[0]["Company_Name"].ToString() + "</h2></div></body></html>";
                                        try
                                        {
                                            ObjSendMailSms.SendApprovalMail(dtEmpDetail.Rows[0]["Email_Id"].ToString(), Common.GetMasterEmailId(ref trns, strCompanyId, strBrandId, strLocationId), Common.GetMasterEmailPassword(ref trns, strCompanyId, strBrandId, strLocationId), "Time Man: Full Day Leave Apply By '" + Common.GetEmployeeName(strCompanyId, strEmpId, ref trns) + "' From '" + dtFromDate.ToString(objSys.SetDateFormat(ref trns)) + "' To '" + dtToDate.ToString(objSys.SetDateFormat(ref trns)) + "'", MailMessage.ToString(), strCompanyId, "", "", strBrandId, strLocationId);

                                        }
                                        catch
                                        {
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Set_Notification(strCompanyId, strBrandId, strLocationId, "Superadmin", strEmpId, PriorityEmpId, null, strEmpName, hdnTransid, cur_trans_id.ToString(), strEmpImage, dtFromDate.ToString(objSys.SetDateFormat(ref trns)), dtToDate.ToString(objSys.SetDateFormat(ref trns)), ref trns, strAbsoluteUri, strTimeZoneId);
                }

            }

        }

        return "";
    }

    public static int GetLeaveDayCount(string strEmpId, DateTime dtFromdate, DateTime dtToDate, string strConString, string strCompanyId, string strBrandId, string strLocationId, string strTimeZoneId)
    {
        Set_ApplicationParameter objAppParam = new Set_ApplicationParameter(strConString);
        Set_Employee_Holiday objEmpHoliday = new Set_Employee_Holiday(strConString);
        string IsLeaveOnHoliday = string.Empty;
        string IsLeaveOnWeekOff = string.Empty;
        string WeekOffDays = string.Empty;
        int LeaveDays = 0;
        int days = 0;
        WeekOffDays = objAppParam.GetApplicationParameterValueByParamName("Week Off Days", strCompanyId, strBrandId, strLocationId);
        IsLeaveOnWeekOff = objAppParam.GetApplicationParameterValueByParamName("Leave_Count_On_WeekOff", strCompanyId, strBrandId, strLocationId);
        IsLeaveOnHoliday = objAppParam.GetApplicationParameterValueByParamName("Leave_Count_On_Holiday", strCompanyId, strBrandId, strLocationId);
        while (dtFromdate <= dtToDate)
        {
            if (IsLeaveOnWeekOff == "False")
            {
                foreach (string str in WeekOffDays.Split(','))
                {
                    string WeekOff = dtFromdate.DayOfWeek.ToString();
                    if (WeekOff == str)
                    {
                        LeaveDays += 1;
                    }
                }
            }
            if (IsLeaveOnHoliday == "False")
            {
                DataTable dtHoliday2 = objEmpHoliday.GetEmployeeHolidayMaster(strCompanyId, strTimeZoneId);
                // DateTime fromdate2 = objSys.getDateForInput(fromdate.ToString());

                DataTable dtHoliday1 = new DataView(dtHoliday2, "Holiday_Date='" + dtFromdate.ToString() + "' and Emp_Id='" + strEmpId + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtHoliday1.Rows.Count > 0)
                {
                    LeaveDays += 1;
                }
            }
            days += 1;


            dtFromdate = dtFromdate.AddDays(1);

        }
        days = days - LeaveDays;

        return days;
    }

    public static int GetleaveBalance(DataTable dtLeave, string strLeaveTypeId, string strEmpId)
    {
        int UsedLeave = 0;


        dtLeave = new DataView(dtLeave, "Leave_Type_id=" + strLeaveTypeId + " and Emp_Id=" + strEmpId + "", "", DataViewRowState.CurrentRows).ToTable();

        foreach (DataRow dr in dtLeave.Rows)
        {
            UsedLeave += Convert.ToInt32(dr["LeaveCount"].ToString());
        }


        return UsedLeave;
    }

    public static string GetRoundValue(string leaveCount)
    {
        string strRound = string.Empty;


        if (leaveCount.Contains("."))
        {
            leaveCount = leaveCount.Split('.')[0].ToString();
        }

        strRound = Math.Round(Convert.ToDouble(leaveCount), 0).ToString();


        return strRound;
    }



    public static string Get_Rejoin(string strCompanyId, string strBrandId, string strLocationId, string To_Date, string strEmpId, string strConString, string strTimeZoneId)
    {
        Set_ApplicationParameter objAppParam = new Set_ApplicationParameter(strConString);
        Set_Employee_Holiday objEmpHoliday = new Set_Employee_Holiday(strConString);
        SystemParameter ObjSys = new SystemParameter(strConString);
        Att_Leave_Request ObjLeaveRequest = new Att_Leave_Request(strConString);
        DataTable dtLeave = ObjLeaveRequest.GetLeaveRequest_ByID(strCompanyId, strEmpId);
        dtLeave = new DataView(dtLeave, "Is_Approved='True' and Brand_Id='" + strBrandId + "' and Location_Id='" + strLocationId + "' and Emp_Id=" + strEmpId + "", "", DataViewRowState.CurrentRows).ToTable();
        string IsLeaveOnHoliday = string.Empty;
        string IsLeaveOnWeekOff = string.Empty;
        string WeekOffDays = string.Empty;
        int Rejoin_Days = 0;
        string strGetRejoindate = string.Empty;

        DateTime Rejoin_Date = Convert.ToDateTime(To_Date).AddDays(1);
        strGetRejoindate = Rejoin_Date.ToString();
        WeekOffDays = objAppParam.GetApplicationParameterValueByParamName("Week Off Days", strCompanyId, strBrandId, strLocationId);
        IsLeaveOnWeekOff = objAppParam.GetApplicationParameterValueByParamName("Leave_Count_On_WeekOff", strCompanyId, strBrandId, strLocationId);
        IsLeaveOnHoliday = objAppParam.GetApplicationParameterValueByParamName("Leave_Count_On_Holiday", strCompanyId, strBrandId, strLocationId);
        if (IsLeaveOnWeekOff == "False")
        {
            foreach (string str in WeekOffDays.Split(','))
            {
                string WeekOff = Rejoin_Date.DayOfWeek.ToString();
                if (WeekOff == str)
                {
                    //Rejoin_Days += 1;
                    strGetRejoindate = Get_Rejoin(strCompanyId, strBrandId, strLocationId, Rejoin_Date.ToString(), strEmpId, strConString, strTimeZoneId);
                    //break;
                }
            }
        }
        if (IsLeaveOnHoliday == "False")
        {
            DataTable dtHoliday2 = objEmpHoliday.GetEmployeeHolidayMaster(strCompanyId, strTimeZoneId);
            DataTable dtHoliday1 = new DataView(dtHoliday2, "Holiday_Date='" + Rejoin_Date.ToString() + "' and Emp_Id='" + strEmpId + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (dtHoliday1.Rows.Count > 0)
            {
                //Rejoin_Days += 1;
                strGetRejoindate = Get_Rejoin(strCompanyId, strBrandId, strLocationId, Rejoin_Date.ToString(), strEmpId, strConString, strTimeZoneId);
                //break;
            }
        }

        if (dtLeave.Rows.Count > 0)
        {
            if (new DataView(dtLeave, "From_Date<='" + Convert.ToDateTime(Rejoin_Date) + "' and To_Date>='" + Convert.ToDateTime(Rejoin_Date) + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            {
                strGetRejoindate = Get_Rejoin(strCompanyId, strBrandId, strLocationId, Rejoin_Date.ToString(), strEmpId, strConString, strTimeZoneId);
            }
            if (new DataView(dtLeave, "From_Date<='" + Convert.ToDateTime(Rejoin_Date) + "' and To_Date>='" + Convert.ToDateTime(Rejoin_Date) + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
            {
                strGetRejoindate = Get_Rejoin(strCompanyId, strBrandId, strLocationId, Rejoin_Date.ToString(), strEmpId, strConString, strTimeZoneId);
            }
        }


        return Convert.ToDateTime(strGetRejoindate).ToString(ObjSys.SetDateFormat());
    }

    private void Set_Notification(string strEmpId, string PriorityEmpId, DataTable Dt_Leave_Grid, string strEmpName, string hdnTransid, string cur_trans_id, string strempImage, string strFromDate, string strTodate, string strAbsoluteUri, string strCompanyId, string strBrandId, string strlocationId, string strLoginEmpId, string strTimeZoneId, string struserid)
    {
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();
        //string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        string currentUrl = strAbsoluteUri.ToLower();
        string URL = strAbsoluteUri.Substring(currentUrl.IndexOf("/attendance"));

        int index = URL.LastIndexOf(".aspx");
        if (index > 0)
            URL = URL.Substring(0, index + 5);

        Dt_Request_Type = Obj_Notifiacation.Get_Request_Type(".." + URL, strEmpId, PriorityEmpId);
        string Request_URL = "../MasterSetUp/EmployeeApproval.aspx?Request_ID=" + Dt_Request_Type.Rows[0]["Request_Emp_ID"].ToString() + "&Request_Type=" + Dt_Request_Type.Rows[0]["Approval_Id"].ToString() + "";

        string Message = string.Empty;

        if (Dt_Leave_Grid != null)
        {
            for (int n = 0; n < Dt_Leave_Grid.Rows.Count; n++)
            {
                if (n == 0)
                {
                    Message = strEmpName + " applied " + Common.GetleaveNameById(Dt_Leave_Grid.Rows[n]["Leave_Type_id"].ToString(), _strConString, strCompanyId) + " For " + Dt_Leave_Grid.Rows[n]["LeaveCount"].ToString() + " Days.";
                }
                else
                {

                    Message = Message + " , " + Common.GetleaveNameById(Dt_Leave_Grid.Rows[n]["Leave_Type_id"].ToString(), _strConString, strCompanyId) + " For " + Dt_Leave_Grid.Rows[n]["LeaveCount"].ToString() + " Days.";
                }
            }
        }
        else
        {
            Message = strEmpName + " request for shift from '" + strFromDate + "' to '" + strTodate + "'";
        }



        if (hdnTransid == "")
        {
            // For Insert        
            Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(strCompanyId, strBrandId, strlocationId, strLoginEmpId, strEmpId, PriorityEmpId, Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", cur_trans_id, "False", strempImage, "", "", "", "", struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), "0", "0");
        }
        else
        {
            // For Update
            //Save_Notification = Obj_Notifiacation.UpdateNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEmpId.Value, Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "False", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString()).ToString(), hdnTransid.Value, "Set_Approval_Transaction", Session["cur_trans_id"].ToString());
            Save_Notification = Obj_Notifiacation.UpdateNotificationMaster_Trans(strCompanyId, strBrandId, strlocationId, strLoginEmpId, strEmpId, PriorityEmpId, Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", cur_trans_id, "False", strempImage, "", "", "", "", struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), hdnTransid, "14");
        }
    }

    private void Set_Notification(string strCompanyId, string strBrandId, string strLocationId, string strUserId, string strEmpId, string PriorityEmpId, DataTable Dt_Leave_Grid, string strEmpName, string hdnTransid, string cur_trans_id, string strempImage, string strFromDate, string strTodate, ref SqlTransaction trns, string strAbsoluteUri, string strTimeZoneId)
    {
        NotificationMaster Obj_Notifiacation = new NotificationMaster(_strConString);
        int Save_Notification = 0;
        DataTable Dt_Request_Type = new DataTable();
        //string currentUrl = HttpContext.Current.Request.Url.AbsoluteUri.ToLower();
        string currentUrl = strAbsoluteUri.ToLower();
        string URL = string.Empty;

        try
        {
            URL = strAbsoluteUri.Substring(currentUrl.IndexOf("/attendance"));
        }
        catch
        {
            URL = "/attendance/LeaveApproval.aspx";
        }

        int index = URL.LastIndexOf(".aspx");
        if (index > 0)
            URL = URL.Substring(0, index + 5);

        Dt_Request_Type = Obj_Notifiacation.Get_Request_Type(".." + URL, strEmpId, PriorityEmpId, ref trns);
        string Request_URL = "../MasterSetUp/EmployeeApproval.aspx?Request_ID=" + Dt_Request_Type.Rows[0]["Request_Emp_ID"].ToString() + "&Request_Type=" + Dt_Request_Type.Rows[0]["Approval_Id"].ToString() + "";

        string Message = string.Empty;

        if (Dt_Leave_Grid != null)
        {
            for (int n = 0; n < Dt_Leave_Grid.Rows.Count; n++)
            {
                if (n == 0)
                {
                    Message = strEmpName + " applied " + Common.GetleaveNameById(strCompanyId, Dt_Leave_Grid.Rows[n]["Leave_Type_id"].ToString(), ref trns) + " For " + Dt_Leave_Grid.Rows[n]["LeaveCount"].ToString() + " Days.";
                }
                else
                {

                    Message = Message + " , " + Common.GetleaveNameById(strCompanyId, Dt_Leave_Grid.Rows[n]["Leave_Type_id"].ToString(), ref trns) + " For " + Dt_Leave_Grid.Rows[n]["LeaveCount"].ToString() + " Days.";
                }
            }
        }
        else
        {
            Message = strEmpName + " request for shift from '" + strFromDate + "' to '" + strTodate + "'";
        }



        if (hdnTransid == "")
        {
            // For Insert        
            Save_Notification = Obj_Notifiacation.InsertNotificationMaster_Trans(strCompanyId, strBrandId, strLocationId, strEmpId, strEmpId, PriorityEmpId, Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", cur_trans_id, "False", strempImage, "", "", "", "", strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), "0", "0", ref trns);
        }
        else
        {
            // For Update
            //Save_Notification = Obj_Notifiacation.UpdateNotificationMaster_Trans(Session["CompId"].ToString(), Session["BrandId"].ToString(), Session["LocId"].ToString(), hdnEmpId.Value, Session["PriorityEmpId"].ToString(), Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "False", Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString()).ToString(), hdnTransid.Value, "Set_Approval_Transaction", Session["cur_trans_id"].ToString());
            Save_Notification = Obj_Notifiacation.UpdateNotificationMaster_Trans(strCompanyId, strBrandId, strLocationId, strEmpId, strEmpId, PriorityEmpId, Message, Dt_Request_Type.Rows[0]["notification_type_id"].ToString(), Request_URL, "Set_Approval_Transaction", cur_trans_id, "False", strempImage, "", "", "", "", strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), strUserId, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), hdnTransid, "14", ref trns);
        }
    }

    //New Updates Mearge From Alpha By Rahul Sharma On Date 16-04-2024
    public DataTable GetOTLeaveSummaryForAll(string CompanyId)
    {
        DataTable dtOt = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtOt = daClass.Reuturn_Datatable_Search("sp_Att_Leave_OT_SelectRow", paramList);
        return dtOt;
    }

    public int InsertOTRequest(string CompanyId, string LocationId, string LeaveTypeId, string AssignDate, string EmpId, string TotalHours, string TotalDays, string Description, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[15];
        paramList[0] = new PassDataToSql("@CompanyId", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@LocationID", LocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@LeaveTypeID", LeaveTypeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@AssignDate", @AssignDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@TotalHours", @TotalHours, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@TotalDays", TotalDays, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Description", Description, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Att_Leave_OT_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[14].ParaValue);
    }
    public DataTable EmployeeOTTrans(string EmpId, string CompanyId)
    {
        DataTable dtOt = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtOt = daClass.Reuturn_Datatable_Search("sp_Att_Leave_OT_SelectRow", paramList);
        return dtOt;
    }
    public int UpdateLeaveOTTrans(string CompanyId, string Emp_Id, string Remaining_Days, string OT_Days, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id ", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Remaining_Days", Remaining_Days, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@OT_Days", OT_Days, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Att_Leave_OT_Update", paramList, ref trns);
        return Convert.ToInt32(paramList[5].ParaValue);
    }
    public DataTable GetOTLeaveSummary(string EmpId, string CompanyId)
    {
        DataTable dtOt = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtOt = daClass.Reuturn_Datatable_Search("sp_Att_Leave_OT_SelectRow", paramList);
        return dtOt;
    }
    public DataTable GetEmplolyeeLeaveMovement(string Emp_Id)
    {
        DataTable dt = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dt = daClass.Reuturn_Datatable_Search("sp_Att_Employee_Leave_Movement", paramList);
        return dt;
    }
}
