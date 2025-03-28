using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using PegasusDataAccess;
using System.Data.SqlClient;
using System.Net.Http;

/// <summary>
/// Summary description for Att_Employee_Leave
/// </summary>
public class Att_Employee_Leave
{
    DataAccessClass daClass = null;
    private string _strConString = string.Empty;
    Att_AttendanceLog objAttLog = null;
    public Att_Employee_Leave(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
        objAttLog = new Att_AttendanceLog(strConString);
        _strConString = strConString;
    }


    public int DeleteEmployeeLeaveByEmpIdandleaveTypeId(string empid, string leavetypeid)
    {


        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Leave_Type_Id", leavetypeid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("Set_Att_Employee_Leave_DeleteByEmpId_LeaveTypeId", paramList);
        return Convert.ToInt32(paramList[2].ParaValue);


    }
    public int DeleteEmployeeLeaveByEmpIdandleaveTypeIdIsActive(string empid, string leavetypeid, string strYear)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Leave_Type_Id", leavetypeid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Year", strYear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("Set_Att_Employee_Leave_DeleteByEmpId_LeaveTypeIdIsActive", paramList);
        return Convert.ToInt32(paramList[2].ParaValue);
    }
    public int DeleteEmployeeLeaveTransByEmpIdandleaveTypeId(string empid, string leavetypeid, string month, string year)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Leave_Type_Id", leavetypeid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@Month", month, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@Year", year, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[4] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("Att_Employee_Leave_Trans_DeleteByMonth_EmpId", paramList);
        return Convert.ToInt32(paramList[4].ParaValue);


    }

    public int DeleteEmployeeLeaveTransByEmpIdandleaveTypeId(string empid, string leavetypeid, string month, string year, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Leave_Type_Id", leavetypeid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@Month", month, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@Year", year, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[4] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("Att_Employee_Leave_Trans_DeleteByMonth_EmpId", paramList, ref trns);
        return Convert.ToInt32(paramList[4].ParaValue);


    }










    public int InsertEmployeeLeave(string CompanyId, string Emp_Id, string LeaveType_Id, string Total_Leave, string Paid_Leave, string Percentage_Of_Salary, string Shedule_Type, string ismonthcarry, string isyearcarry, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[24];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@LeaveType_Id", LeaveType_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Total_Leave", Total_Leave, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Paid_Leave", Paid_Leave, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Percentage_Of_Salary", Percentage_Of_Salary, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Shedule_Type", Shedule_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Is_MonthCarry", ismonthcarry, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);

        paramList[8] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[9] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


        paramList[15] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[21] = new PassDataToSql("@Is_YearCarry", isyearcarry, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@Is_Encash", false.ToString(), PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@Max_Days", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Set_Att_Employee_Leave_Insert", paramList);
        return Convert.ToInt32(paramList[20].ParaValue);
    }


    public int InsertEmployeeLeave(string CompanyId, string Emp_Id, string LeaveType_Id, string Total_Leave, string Paid_Leave, string Percentage_Of_Salary, string Shedule_Type, string ismonthcarry, string isyearcarry, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[24];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@LeaveType_Id", LeaveType_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Total_Leave", Total_Leave, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Paid_Leave", Paid_Leave, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Percentage_Of_Salary", Percentage_Of_Salary, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Shedule_Type", Shedule_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Is_MonthCarry", ismonthcarry, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);

        paramList[8] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[9] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


        paramList[15] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[21] = new PassDataToSql("@Is_YearCarry", isyearcarry, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@Is_Encash", false.ToString(), PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@Max_Days", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Set_Att_Employee_Leave_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[20].ParaValue);
    }


    public int InsertEmployeeLeaveTrans(string CompanyId, string Emp_Id, string leavetypeid, string Year, string Month, string Previous_Days, string Assign_Days, string Total_Days, string Used_Days, string Remaining_Days, string Pending_Days, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate)
    {

        if (Field1.Trim() == "")
        {


            DataTable dtEmpleave = GetEmployeeLeaveByEmpIdandLeaveTypeId(CompanyId, Emp_Id, leavetypeid);

            string PaidLeaves = string.Empty;
            double PaidLeave = 0;
            double TotalLeave = 0;
            double LeavePer = 0;
            if (dtEmpleave.Rows.Count > 0)
            {
                try
                {
                    PaidLeave = double.Parse(dtEmpleave.Rows[0]["Paid_Leave"].ToString());
                }
                catch
                {
                }

                try
                {
                    TotalLeave = double.Parse(dtEmpleave.Rows[0]["Total_Leave"].ToString());
                }
                catch
                {

                }
                try
                {
                    LeavePer = (PaidLeave / TotalLeave) * 100;
                }
                catch
                {
                }


                try
                {
                    PaidLeave = double.Parse(Assign_Days) * LeavePer / 100;
                    PaidLeaves = System.Math.Round(PaidLeave).ToString();
                }
                catch
                {

                }
            }


            Field1 = PaidLeaves.ToString();
            Field2 = PaidLeaves.ToString();

        }



        PassDataToSql[] paramList = new PassDataToSql[25];
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

        paramList[11] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[12] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


        paramList[18] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[24] = new PassDataToSql("@Leave_Type_Id", leavetypeid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Att_Employee_Leave_Trans_Insert", paramList);
        return Convert.ToInt32(paramList[23].ParaValue);
    }


    public int InsertEmployeeLeaveTrans(string CompanyId, string Emp_Id, string leavetypeid, string Year, string Month, string Previous_Days, string Assign_Days, string Total_Days, string Used_Days, string Remaining_Days, string Pending_Days, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
    {

        if (Field1.Trim() == "")
        {


            DataTable dtEmpleave = GetEmployeeLeaveByEmpId(CompanyId, Emp_Id);
            dtEmpleave = new DataView(dtEmpleave, "LeaveType_Id='" + leavetypeid + "'", "", DataViewRowState.CurrentRows).ToTable();

            string PaidLeaves = string.Empty;
            double PaidLeave = 0;
            double TotalLeave = 0;
            double LeavePer = 0;
            if (dtEmpleave.Rows.Count > 0)
            {
                try
                {
                    PaidLeave = double.Parse(dtEmpleave.Rows[0]["Paid_Leave"].ToString());
                }
                catch
                {
                }

                try
                {
                    TotalLeave = double.Parse(dtEmpleave.Rows[0]["Total_Leave"].ToString());
                }
                catch
                {

                }
                try
                {
                    LeavePer = (PaidLeave / TotalLeave) * 100;
                }
                catch
                {
                }


                try
                {
                    PaidLeave = double.Parse(Assign_Days) * LeavePer / 100;
                    PaidLeaves = System.Math.Round(PaidLeave).ToString();
                }
                catch
                {

                }
            }


            Field1 = PaidLeaves.ToString();
            Field2 = PaidLeaves.ToString();

        }



        PassDataToSql[] paramList = new PassDataToSql[25];
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

        paramList[11] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[12] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


        paramList[18] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[24] = new PassDataToSql("@Leave_Type_Id", leavetypeid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Att_Employee_Leave_Trans_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[23].ParaValue);
    }
    public int UpdateEmployeeLeaveTransaction(string CompanyId, string Emp_Id, string leavetypeid, string Year, string Month, string Previous_Days, string Assign_Days, string Total_Days, string Used_Days, string Remaining_Days, string Pending_Days, string RemainPaidLeave, string ModifiedBy, string ModifiedDate)
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
        paramList[10] = new PassDataToSql("@Encash_Days", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);



        paramList[11] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[14] = new PassDataToSql("@Leave_Type_Id", leavetypeid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[15] = new PassDataToSql("@Field2", RemainPaidLeave, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Att_Employee_Leave_Trans_Update", paramList);
        return Convert.ToInt32(paramList[13].ParaValue);
    }

    public int UpdateEmployeeLeaveTransaction(string CompanyId, string Emp_Id, string leavetypeid, string Year, string Month, string Previous_Days, string Assign_Days, string Total_Days, string Used_Days, string Remaining_Days, string Pending_Days, string RemainPaidLeave, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
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
        paramList[10] = new PassDataToSql("@Encash_Days", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);



        paramList[11] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[14] = new PassDataToSql("@Leave_Type_Id", leavetypeid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[15] = new PassDataToSql("@Field2", RemainPaidLeave, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Att_Employee_Leave_Trans_Update", paramList, ref trns);
        return Convert.ToInt32(paramList[13].ParaValue);
    }

    public int UpdateEmployeeLeaveTransactionByTransNo(string TransId, string CompanyId, string Emp_Id, string leavetypeid, string Year, string Month, string Previous_Days, string Assign_Days, string Total_Days, string Used_Days, string Remaining_Days, string Pending_Days, string RemainPaidDays, string ModifiedBy, string ModifiedDate)
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
        paramList[16] = new PassDataToSql("@Field2", RemainPaidDays, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Att_Employee_Leave_Trans_UpdateByTransNo", paramList);
        return Convert.ToInt32(paramList[13].ParaValue);
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

    public int UpdateEmployeeLeaveByTransNo(string Trans_No, string CompanyId, string Emp_Id, string LeaveType_Id, string Total_Leave, string Paid_Leave, string Percentage_Of_Salary, string Shedule_Type, string ismonthcarry, string isyearcarry, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[23];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@LeaveType_Id", LeaveType_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Total_Leave", Total_Leave, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Paid_Leave", Paid_Leave, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Percentage_Of_Salary", Percentage_Of_Salary, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Shedule_Type", Shedule_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Is_MonthCarry", ismonthcarry, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);

        paramList[8] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[9] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


        paramList[15] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[19] = new PassDataToSql("@Is_YearCarry", isyearcarry, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@Trans_No", Trans_No, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@Is_Encash", false.ToString(), PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@Max_Days", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Set_Att_Employee_Leave_Update", paramList);
        return Convert.ToInt32(paramList[20].ParaValue);
    }
    public DataTable GetEmployeeLeaveByCompanyId(string CompanyId)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Trans_No", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Att_Employee_Leave_SelectRow", paramList);

        return dtInfo;
    }
    public DataTable GetEmployeeLeaveByEmpId(string CompanyId, string EmpId)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Trans_No", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "4", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Att_Employee_Leave_SelectRow", paramList);

        return dtInfo;
    }


    public DataTable GetEmployeeLeaveByEmpIdandLeaveTypeId(string CompanyId, string EmpId, string strLeaveTypeId)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Trans_No", strLeaveTypeId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "5", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Att_Employee_Leave_SelectRow", paramList);

        return dtInfo;
    }


    public DataTable GetEmployeeLeaveByEmpIdandLeaveTypeId(string CompanyId, string EmpId, string strLeaveTypeId, ref SqlTransaction trns)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Trans_No", strLeaveTypeId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "5", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Att_Employee_Leave_SelectRow", paramList, ref trns);

        return dtInfo;
    }

    public DataTable GetEmployeeLeaveByTransId(string CompanyId, string TransNo)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Trans_No", TransNo, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Att_Employee_Leave_SelectRow", paramList);

        return dtInfo;
    }

    public DataTable GetEmployeeLeaveTransactionData(string empid, string LeaveTypeId, string month, string year)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];

        paramList[0] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Leave_Type_Id", LeaveTypeId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Year", year, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@Month", month, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("Att_Employee_Leave_Trans_Select", paramList);

        return dtInfo;
    }

    public DataTable GetEmployeeLeaveTransactionData(string empid, string LeaveTypeId, string month, string year, ref SqlTransaction trns)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];

        paramList[0] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Leave_Type_Id", LeaveTypeId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Year", year, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@Month", month, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("Att_Employee_Leave_Trans_Select", paramList, ref trns);

        return dtInfo;
    }

    public DataTable GetEmployeeLeaveTransactionDataByEmpId(string empid)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[1];

        paramList[0] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("Att_Employee_Leave_Trans_SelectByEmpId", paramList);

        return dtInfo;
    }

    public DataTable DashboardAppliedShortLeave(string Company_Id, string Brand_Id, string Location_Id, string Emp_Id)
    {
        DateTime txtFromdate = DateTime.Now.AddDays(-32);
        DateTime txtTodate = DateTime.Now.AddDays(-1);
        DataTable dtPartial = daClass.return_DataTable("Select   Att_PartialLeave_Request.Trans_Id ,  cast (Att_PartialLeave_Request.Request_Date_Time as Date) as Request_Date, Cast(Att_PartialLeave_Request.Partial_Leave_Date  as Date) as Partial_Leave_Date ,Att_PartialLeave_Request.Field5  as Method,case When Att_PartialLeave_Request.Partial_Leave_Type = 0  then 'PERSONAL' else  'OFFICIAL' end  as PartialType,Att_PartialLeave_Request.From_Time ,Att_PartialLeave_Request.To_Time ,dbo.MinutesToDuration(Att_PartialLeave_Request.Field4 )as duration,CASE WHEN  Att_PartialLeave_Request.Field1 ='Auto' then 'AUTO' else 'MANUAL' end as TransType,Att_PartialLeave_Request.Is_Confirmed From   Att_PartialLeave_Request  Where Is_Confirmed <>''   and emp_id =" + Emp_Id + " and (cast( Partial_Leave_Date as date)>='" + Convert.ToDateTime(txtFromdate).Date.ToString() + "' and cast( Partial_Leave_Date as date)<='" + Convert.ToDateTime(txtTodate).Date.ToString() + "') ");

        return dtPartial;


    }

    public DataTable GenerateDashboardShortLeave1(string Company_Id, string Brand_Id, string Location_Id, string Emp_Id)
    {

        DateTime txtFromdate = DateTime.Now.AddDays(-6);
        DateTime txtTodate = DateTime.Now.AddDays(-1);
        DataTable dtResult = new DataTable();

        dtResult.Columns.Add(new DataColumn("Date"));
        dtResult.Columns.Add(new DataColumn("InTime"));
        dtResult.Columns.Add(new DataColumn("OutTime"));
        dtResult.Columns.Add(new DataColumn("Duration"));
        dtResult.Columns.Add(new DataColumn("Type"));

        DataTable tTempResult = new DataTable();
        DateTime dtOndutyTime = new DateTime();
        DateTime dtOffdutyTime = new DateTime();
        DateTime dtinTime = new DateTime();
        DateTime dtoutTime = new DateTime();
        DataTable dtTemp = new DataTable();
        DataTable dtlog = new DataTable();
        DataTable dtlogtemp = new DataTable();
        //DataTable dtRegister = daClass.return_DataTable("select emp_id, att_date,timetable_id,onduty_time,OffDuty_Time,In_Time,out_time,LateMin,late_relaxation_min,earlymin,Early_Relaxation_Min from att_attendanceregister where emp_id in (" + Emp_Id + ") and (Is_Leave=0 and is_absent=0 and is_holiday=0 and Is_Week_Off=0) and (cast( att_Date as date)>='" + Convert.ToDateTime(txtFromdate).Date.ToString() + "' and cast( att_Date as date)<='" + Convert.ToDateTime(txtTodate).Date.ToString() + "')");


        DataTable dtRegister = daClass.return_DataTable("select emp_id, att_date,timetable_id,onduty_time,OffDuty_Time,In_Time,out_time,LateMin,late_relaxation_min,earlymin,Early_Relaxation_Min from att_attendanceregister where emp_id in (" + Emp_Id + ") and (Is_Leave=0 and is_absent=0 and is_holiday=0 and Is_Week_Off=0) and (cast( att_Date as date)>='" + Convert.ToDateTime(txtFromdate).Date.ToString() + "' and cast( att_Date as date)<='" + Convert.ToDateTime(txtTodate).Date.ToString() + "')");

        DataTable dtPartial = daClass.return_DataTable("Select  *   from  Att_PartialLeave_Request  where emp_id =" + Emp_Id + " and Field1='Auto' and (cast( Partial_Leave_Date as date)>='" + Convert.ToDateTime(txtFromdate).Date.ToString() + "' and cast( Partial_Leave_Date as date)<='" + Convert.ToDateTime(txtTodate).Date.ToString() + "') ");


        DataTable dtSchdule = daClass.return_DataTable("Select   Att_TimeTable.* From Att_ScheduleDescription  INNER JOIN  Att_TimeTable ON Att_TimeTable.TimeTable_Id = Att_ScheduleDescription.TimeTable_Id  where Att_ScheduleDescription.emp_id in (" + Emp_Id + ")  and (cast( att_Date as date)>='" + Convert.ToDateTime(txtFromdate).Date.ToString() + "' and cast( att_Date as date)<='" + Convert.ToDateTime(txtTodate).Date.ToString() + "')");


        DateTime dtFromdate = Convert.ToDateTime(txtFromdate).Date;
        DateTime dttoDate = Convert.ToDateTime(txtTodate).Date;

        dtFromdate = Convert.ToDateTime(txtFromdate).Date;
        dtlog = objAttLog.GetAttendanceLogByDateByEmpId1(Emp_Id, dtFromdate.AddDays(-1).ToString(), dttoDate.AddDays(1).ToString());


        while (dtFromdate <= dttoDate)
        {
            if (dtRegister.Rows.Count > 0)
            {
                dtTemp = new DataView(dtRegister, "att_date='" + dtFromdate.ToString() + "' and timetable_id<>0 and emp_id=" + Emp_Id + "", "", DataViewRowState.CurrentRows).ToTable();
            }
            else
            {
                DataTable dtTemp1 = new DataView(dtSchdule, "Att_Date= '" + dtFromdate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                dtTemp.Rows.Clear();
                DataRow row = dtTemp.NewRow();
              
            }





            for (int j = 0; j < dtTemp.Rows.Count; j++)
            {

                dtinTime = Convert.ToDateTime(dtTemp.Rows[j]["In_Time"].ToString());
                dtoutTime = Convert.ToDateTime(dtTemp.Rows[j]["out_time"].ToString());
                dtOndutyTime = Convert.ToDateTime(dtTemp.Rows[j]["onduty_time"].ToString());
                dtOffdutyTime = Convert.ToDateTime(dtTemp.Rows[j]["OffDuty_Time"].ToString());
                if (dtOndutyTime < dtOffdutyTime)
                {
                    dtOndutyTime = new DateTime(dtFromdate.Year, dtFromdate.Month, dtFromdate.Day, dtOndutyTime.Hour, dtOndutyTime.Minute, dtOndutyTime.Second);
                    dtOffdutyTime = new DateTime(dtFromdate.Year, dtFromdate.Month, dtFromdate.Day, dtOffdutyTime.Hour, dtOffdutyTime.Minute, dtOffdutyTime.Second);
                }
                else
                {
                    dtOndutyTime = new DateTime(dtFromdate.Year, dtFromdate.Month, dtFromdate.Day, dtOndutyTime.Hour, dtOndutyTime.Minute, dtOndutyTime.Second);
                    dtOffdutyTime = new DateTime(dtFromdate.AddDays(1).Year, dtFromdate.AddDays(1).Month, dtFromdate.AddDays(1).Day, dtOffdutyTime.Hour, dtOffdutyTime.Minute, dtOffdutyTime.Second);
                }
                //generate short leave for late in

                if (dtOndutyTime < dtinTime && dtTemp.Rows[j]["LateMin"].ToString().Trim() != "0")
                {
                    if (dtPartial.Rows.Count > 0)
                        tTempResult = new DataView(dtPartial, "Field5 = 'Direct In' and    Partial_Leave_Date  =   '" + dtFromdate + "' ", "", DataViewRowState.CurrentRows).ToTable();


                    if (tTempResult.Rows.Count == 0)
                    {
                        DataRow row = dtResult.NewRow();
                        row[0] = dtFromdate.ToString("dd-MMM-yyyy");
                        row[1] = dtOndutyTime.ToString("HH:mm").ToString();
                        row[2] = dtinTime.AddMinutes(-1).ToString("HH:mm").ToString();




                        DateTime t1Temp = Convert.ToDateTime(row[1]);
                        DateTime t2Temp = Convert.ToDateTime(row[2]);

                        if (t1Temp > t2Temp)
                        {
                            t2Temp = t2Temp.AddDays(1);
                        }

                        row[3] = (t2Temp - t1Temp).Hours + ":" + (t2Temp - t1Temp).Minutes;

                        row[4] = "Direct In".ToString();

                        dtResult.Rows.Add(row);
                    }


                }

                //generate short leave for Early out

                if (dtOffdutyTime > dtoutTime && dtTemp.Rows[j]["earlymin"].ToString().Trim() != "0")
                {
                    if (dtPartial.Rows.Count > 0)
                        tTempResult = new DataView(dtPartial, "Field5 = 'Direct Out' and    Partial_Leave_Date =   '" + dtFromdate + "' ", "", DataViewRowState.CurrentRows).ToTable();


                    if (tTempResult.Rows.Count == 0)
                    {
                        DataRow row = dtResult.NewRow();
                        row[0] = dtFromdate.ToString("dd-MMM-yyyy");
                        row[1] = dtoutTime.AddMinutes(1).ToString("HH:mm").ToString();
                        row[2] = dtOffdutyTime.ToString("HH:mm").ToString();
                        DateTime t1Temp = Convert.ToDateTime(row[1]);
                        DateTime t2Temp = Convert.ToDateTime(row[2]);

                        if (t1Temp > t2Temp)
                        {
                            t2Temp = t2Temp.AddDays(1);
                        }

                        row[3] = (t2Temp - t1Temp).Hours + ":" + (t2Temp - t1Temp).Minutes;

                        row[4] = "Direct Out".ToString();

                        dtResult.Rows.Add(row);
                    }

                }
            }

            //generating short leave which taken between duty time

            dtlogtemp = new DataView(dtlog, "event_date='" + dtFromdate.ToString() + "' and func_code='2'", "event_time", DataViewRowState.CurrentRows).ToTable();
            if (dtlogtemp.Rows.Count > 1)
            {
                //dtFromdate.ToString(), ,, "", "Auto generated", "", "Auto", "0", "", "", "In Between", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), ref trns);

                if (dtPartial.Rows.Count > 0)
                    tTempResult = new DataView(dtPartial, "Field5 = 'In Between' and   cast( Partial_Leave_Date as date) =   '" + dtFromdate + "' ", "", DataViewRowState.CurrentRows).ToTable();


                if (tTempResult.Rows.Count == 0)
                {
                    DataRow row = dtResult.NewRow();
                    row[0] = dtFromdate.ToString("dd-MMM-yyyy");
                    row[1] = Convert.ToDateTime(dtlogtemp.Rows[0]["Event_time"].ToString()).ToString("HH:mm").ToString();
                    row[2] = Convert.ToDateTime(dtlogtemp.Rows[dtlogtemp.Rows.Count - 1]["Event_time"].ToString()).ToString("HH:mm").ToString();



                    DateTime t1Temp = Convert.ToDateTime(row[1]);
                    DateTime t2Temp = Convert.ToDateTime(row[2]);

                    if (t1Temp > t2Temp)
                    {
                        t2Temp = t2Temp.AddDays(1);
                    }

                    row[3] = (t2Temp - t1Temp).Hours + ":" + (t2Temp - t1Temp).Minutes;

                    row[4] = "In Between".ToString();



                    dtResult.Rows.Add(row);
                }
            }

            dtFromdate = dtFromdate.AddDays(1);
        }





        return dtResult;
    }
    //public DataTable GenerateDashboardShortLeave(string Company_Id, string Brand_Id, string Location_Id, string Emp_Id)
    //{

    //    DateTime txtFromdate = DateTime.Now.AddDays(-6);
    //    DateTime txtTodate = DateTime.Now.AddDays(-1);
    //    DataTable dtResult = new DataTable();

    //    dtResult.Columns.Add(new DataColumn("Date"));
    //    dtResult.Columns.Add(new DataColumn("InTime"));
    //    dtResult.Columns.Add(new DataColumn("OutTime"));
    //    dtResult.Columns.Add(new DataColumn("Duration"));
    //    dtResult.Columns.Add(new DataColumn("Type"));








    //    DateTime dtFromdate = Convert.ToDateTime(txtFromdate).Date;
    //    DateTime dttoDate = Convert.ToDateTime(txtTodate).Date;

    //    dtFromdate = Convert.ToDateTime(txtFromdate).Date;
    //    //dtlog = objAttLog.GetAttendanceLogByDateByEmpId1(Emp_Id, dtFromdate.AddDays(-1).ToString(), dttoDate.AddDays(1).ToString());

    //    DataTable dtPartial = daClass.return_DataTable("Select  *   from  Att_PartialLeave_Request  where emp_id =" + Emp_Id + " and Field1='Auto' and (cast( Partial_Leave_Date as date)>='" + Convert.ToDateTime(txtFromdate).Date.ToString() + "' and cast( Partial_Leave_Date as date)<='" + Convert.ToDateTime(txtTodate).Date.ToString() + "') ");
    //    while (dtFromdate <= dttoDate)
    //    {
    //        DataTable dtShiftInfo = daClass.return_DataTable("Select  Att_TimeTable.OnDuty_Time ,Att_TimeTable.OffDuty_Time ,Att_TimeTable.Late_Min ,Att_TimeTable.Early_Min From  Att_ScheduleDescription   INNER JOIN  Att_TimeTable ON Att_TimeTable.TimeTable_Id = Att_ScheduleDescription.TimeTable_Id   Where   Emp_Id =  '" + Emp_Id + "'  and Att_Date  =  '" + dtFromdate.ToString() + "'  ");
    //        DataTable dtLog = daClass.return_DataTable("Select  * From   Att_AttendanceLog Where   Emp_Id =  '" + Emp_Id + "'  and Event_Date =  '" + txtFromdate.ToString() + "' Order by   Event_Time");


    //        if (dtShiftInfo.Rows.Count > 0)
    //        {
    //            int count = 0;
    //            DateTime tApply = objSys.getDateForInput(txtApplyDate.Text);
    //            bool bTwoDay = false;
    //            bool bParital = false;
    //            DateTime tOnTime = Convert.ToDateTime(dtShiftInfo.Rows[0]["OnDuty_Time"]);
    //            DateTime tOffTime = Convert.ToDateTime(dtShiftInfo.Rows[0]["OffDuty_Time"]);
    //            DateTime tAOnTime = Convert.ToDateTime(dtShiftInfo.Rows[0]["OnDuty_Time"]);
    //            DateTime tAOffTime = Convert.ToDateTime(dtShiftInfo.Rows[0]["OffDuty_Time"]);

    //            if (tOnTime > tOffTime)
    //            {
    //                bTwoDay = true;
    //            }
    //            tAOnTime = new DateTime(tApply.Year, tApply.Month, tApply.Day, tOnTime.Hour, tOnTime.Minute, 0);
    //            tOnTime = new DateTime(tApply.Year, tApply.Month, tApply.Day, tOnTime.Hour, tOnTime.Minute, 0).AddMinutes(Convert.ToInt16((dtShiftInfo.Rows[0]["Late_Min"])));

    //            if (bTwoDay)
    //            {
    //                DateTime tApply1 = tApply.AddDays(1);
    //                tAOffTime = new DateTime(tApply1.Year, tApply1.Month, tApply1.Day, tOffTime.Hour, tOffTime.Minute, 0);
    //                tOffTime = new DateTime(tApply1.Year, tApply1.Month, tApply1.Day, tOffTime.Hour, tOffTime.Minute, 0).AddMinutes(Convert.ToInt16((dtShiftInfo.Rows[0]["Early_Min"])) * -1);

    //            }
    //            else
    //            {
    //                tAOffTime = new DateTime(tApply.Year, tApply.Month, tApply.Day, tOffTime.Hour, tOffTime.Minute, 0);
    //                tOffTime = new DateTime(tApply.Year, tApply.Month, tApply.Day, tOffTime.Hour, tOffTime.Minute, 0).AddMinutes(Convert.ToInt16((dtShiftInfo.Rows[0]["Early_Min"])) * -1);

    //            }


    //            DataTable dtLogNormal = new DataView(dtLog, "func_code<>2", "", DataViewRowState.CurrentRows).ToTable();
    //            DataTable dtLogPartial = new DataView(dtLog, "func_code=2", "", DataViewRowState.CurrentRows).ToTable();

    //            count = dtLogNormal.Rows.Count;
    //            switch (count)
    //            {
    //                // if count 1  means we need to check late
    //                // if two then check late first and  then early
    //                case 1:
    //                    if (Convert.ToDateTime(dtLogNormal.Rows[0]["Event_Time"]) > tOnTime)
    //                    {



    //                        DataRow row = dtResult.NewRow();
    //                        row[0] = dtFromdate.ToString("dd-MMM-yyyy");
    //                        row[1] = tAOnTime.ToString("HH:mm");
    //                        row[2] = Convert.ToDateTime(dtLogNormal.Rows[0]["Event_Time"]).AddMinutes(-1).ToString("HH:mm");




    //                        DateTime t1Temp = Convert.ToDateTime(row[1]);
    //                        DateTime t2Temp = Convert.ToDateTime(row[2]);

    //                        if (t1Temp > t2Temp)
    //                        {
    //                            t2Temp = t2Temp.AddDays(1);
    //                        }

    //                        row[3] = (t2Temp - t1Temp).Hours + ":" + (t2Temp - t1Temp).Minutes;

    //                        row[4] = "Direct In".ToString();

    //                        dtResult.Rows.Add(row);
    //                    }

    //                    break;
    //                case 2:
    //                    if (Convert.ToDateTime(dtLogNormal.Rows[0]["Event_Time"]) > tOnTime)
    //                    {

    //                        DataRow row = dtResult.NewRow();
    //                        row[0] = dtFromdate.ToString("dd-MMM-yyyy");
    //                        row[1] = tAOnTime.ToString("HH:mm");
    //                        row[2] = Convert.ToDateTime(dtLogNormal.Rows[0]["Event_Time"]).AddMinutes(-1).ToString("HH:mm");




    //                        DateTime t1Temp = Convert.ToDateTime(row[1]);
    //                        DateTime t2Temp = Convert.ToDateTime(row[2]);

    //                        if (t1Temp > t2Temp)
    //                        {
    //                            t2Temp = t2Temp.AddDays(1);
    //                        }

    //                        row[3] = (t2Temp - t1Temp).Hours + ":" + (t2Temp - t1Temp).Minutes;

    //                        row[4] = "Direct In".ToString();

    //                        dtResult.Rows.Add(row);
    //                    }
    //                    else
    //                    {
    //                        if (Convert.ToDateTime(dtLogNormal.Rows[1]["Event_Time"]) < tOffTime)
    //                        {




    //                            DataRow row = dtResult.NewRow();
    //                            row[0] = dtFromdate.ToString("dd-MMM-yyyy");
    //                            row[1] = Convert.ToDateTime(dtLogNormal.Rows[1]["Event_Time"]).AddMinutes(1).ToString("HH:mm");
    //                            row[2] = tAOffTime.ToString("HH:mm");




    //                            DateTime t1Temp = Convert.ToDateTime(row[1]);
    //                            DateTime t2Temp = Convert.ToDateTime(row[2]);

    //                            if (t1Temp > t2Temp)
    //                            {
    //                                t2Temp = t2Temp.AddDays(1);
    //                            }

    //                            row[3] = (t2Temp - t1Temp).Hours + ":" + (t2Temp - t1Temp).Minutes;

    //                            row[4] = "Direct Out".ToString();

    //                            dtResult.Rows.Add(row);


    //                        }
    //                    }
    //                    break;
    //                default:
    //                    if (Convert.ToDateTime(dtLogNormal.Rows[0]["Event_Time"]) > tOnTime)
    //                    {


    //                        DataRow row = dtResult.NewRow();
    //                        row[0] = dtFromdate.ToString("dd-MMM-yyyy");
    //                        row[1] = tAOnTime.ToString("HH:mm");
    //                        row[2] = Convert.ToDateTime(dtLogNormal.Rows[0]["Event_Time"]).AddMinutes(-1).ToString("HH:mm");




    //                        DateTime t1Temp = Convert.ToDateTime(row[1]);
    //                        DateTime t2Temp = Convert.ToDateTime(row[2]);

    //                        if (t1Temp > t2Temp)
    //                        {
    //                            t2Temp = t2Temp.AddDays(1);
    //                        }

    //                        row[3] = (t2Temp - t1Temp).Hours + ":" + (t2Temp - t1Temp).Minutes;

    //                        row[4] = "Direct In".ToString();

    //                        dtResult.Rows.Add(row);
    //                    }
    //                    else
    //                    {
    //                        if (Convert.ToDateTime(dtLogNormal.Rows[dtLogNormal.Rows.Count - 1]["Event_Time"]) < tOffTime)
    //                        {

    //                            DataRow row = dtResult.NewRow();
    //                            row[0] = dtFromdate.ToString("dd-MMM-yyyy");
    //                            row[1] = Convert.ToDateTime(dtLogNormal.Rows[dtLogNormal.Rows.Count - 1]["Event_Time"]).AddMinutes(1).ToString("HH:mm");
    //                            row[2] = tAOffTime.ToString("HH:mm");




    //                            DateTime t1Temp = Convert.ToDateTime(row[1]);
    //                            DateTime t2Temp = Convert.ToDateTime(row[2]);

    //                            if (t1Temp > t2Temp)
    //                            {
    //                                t2Temp = t2Temp.AddDays(1);
    //                            }

    //                            row[3] = (t2Temp - t1Temp).Hours + ":" + (t2Temp - t1Temp).Minutes;

    //                            row[4] = "Direct Out".ToString();

    //                            dtResult.Rows.Add(row);
    //                        }
    //                    }
    //                    break;
    //            }

    //            if (!bParital)
    //            {
    //                if (dtLogPartial.Rows.Count == 2)
    //                {



    //                    DataRow row = dtResult.NewRow();
    //                    row[0] = dtFromdate.ToString("dd-MMM-yyyy");
    //                    row[1] = Convert.ToDateTime(dtLogPartial.Rows[0]["Event_Time"]).AddMinutes(1).ToString("HH:mm");
    //                    row[2] = Convert.ToDateTime(dtLogPartial.Rows[1]["Event_Time"]).AddMinutes(-1).ToString("HH:mm");




    //                    DateTime t1Temp = Convert.ToDateTime(row[1]);
    //                    DateTime t2Temp = Convert.ToDateTime(row[2]);

    //                    if (t1Temp > t2Temp)
    //                    {
    //                        t2Temp = t2Temp.AddDays(1);
    //                    }

    //                    row[3] = (t2Temp - t1Temp).Hours + ":" + (t2Temp - t1Temp).Minutes;

    //                    row[4] = "Between In Out".ToString();

    //                    dtResult.Rows.Add(row);

    //                }
    //            }



    //            dtFromdate = dtFromdate.AddDays(1);
    //        }





    //        return dtResult;
    //    }
    //}

    public DataTable Get_Attendance_Dashboard(string Company_Id, string Brand_Id, string Location_Id, string Emp_Id, string Log_Days, string OpType)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", Brand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", Location_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@LogDays", Log_Days, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Optype", OpType, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("Get_Attendance_Dashboard", paramList);
        return dtInfo;
    }
    public DataTable Get_Log_By_Date_EmpID(string Company_Id, string Brand_Id, string Location_Id, string Emp_Id, string From_Date, string To_Date, string Log_Days, string OpType)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", Brand_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", Location_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@From_Date", From_Date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@To_Date", To_Date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@LogDays", Log_Days, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Optype", OpType, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("Get_Log_By_Date_EmpID", paramList);
        return dtInfo;
    }

    //public DataTable GetEmployeeLeaveTransactionDataByEmpId(string empid)
    //{

    //    DataTable dtInfo = new DataTable();
    //    PassDataToSql[] paramList = new PassDataToSql[1];

    //    paramList[0] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

    //    dtInfo = daClass.Reuturn_Datatable_Search("Att_Employee_Leave_Trans_SelectByEmpId", paramList);
    //    //using (dtInfo = daClass.Reuturn_Datatable_Search("Att_Employee_Leave_Trans_SelectByEmpId", paramList, ref trns))
    //   return dtInfo;

    //}
}
