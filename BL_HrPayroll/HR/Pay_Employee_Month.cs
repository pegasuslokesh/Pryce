using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using PegasusDataAccess;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Pay_Employee_Month
/// </summary>
public class Pay_Employee_Month
{
    DataAccessClass da = null;

    public Pay_Employee_Month(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        da = new DataAccessClass(strConString);
    }

    public int Insert_Pay_Employee_Month_Temp(string strCompanyId, string strEmployeeId, string strmonth, string stryear, string WorkMinSal, string NorOTMinSal, string WeekOtMinSal, string HolidayOtMinSal, string LeaveDaySal, string WeekOfSal, string HolidaysSal, string AbsantSal, string LateMinPe, string EarlyMinPen, string PatialViolPen, string strEmpPenalty, string strTotalClaim, string strEmpLoan, string strTotalAllow, string strTotalDeduc, string Employee_PF, string Employer_PF, string Employee_ESIC, string Employer_ESIC)
    {
        PassDataToSql[] param = new PassDataToSql[25];
        param[0] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Emp_Id", strEmployeeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Month", strmonth, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Year", stryear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[4] = new PassDataToSql("@Worked_Min_Salary", WorkMinSal, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Normal_OT_Min_Salary", NorOTMinSal, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Week_Off_OT_Min_Salary", WeekOtMinSal, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[7] = new PassDataToSql("@Holiday_OT_Min_Salary", HolidayOtMinSal, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[8] = new PassDataToSql("@Leave_Days_Salary", LeaveDaySal, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[9] = new PassDataToSql("@Week_Off_Salary", WeekOfSal, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[10] = new PassDataToSql("@Holidays_Salary", HolidaysSal, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[11] = new PassDataToSql("@Absent_Salary", AbsantSal, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[12] = new PassDataToSql("@Late_Min_Penalty", LateMinPe, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[13] = new PassDataToSql("@Early_Min_Penalty", EarlyMinPen, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[14] = new PassDataToSql("@Patial_Violation_Penalty", PatialViolPen, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[15] = new PassDataToSql("@Employee_Penalty", strEmpPenalty, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[16] = new PassDataToSql("@Employee_Claim", strTotalClaim, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[17] = new PassDataToSql("@Emlployee_Loan", strEmpLoan, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[18] = new PassDataToSql("@Total_Allowance", strTotalAllow, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[19] = new PassDataToSql("@Total_Deduction", strTotalDeduc, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[20] = new PassDataToSql("@Employee_PF", Employee_PF, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[21] = new PassDataToSql("@Employer_PF", Employer_PF, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[22] = new PassDataToSql("@Employee_ESIC", Employee_ESIC, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[23] = new PassDataToSql("@Employer_ESIC", Employer_ESIC, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[24] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);


        da.execute_Sp("sp_Pay_Employe_Month_Temp_Insert", param);
        return Convert.ToInt32(param[24].ParaValue);

        //return Convert.ToInt32(param[3].ParaValue);

    }


    public int Insert_Pay_Employee_Month_Temp(string strCompanyId, string strEmployeeId, string strmonth, string stryear, string WorkMinSal, string NorOTMinSal, string WeekOtMinSal, string HolidayOtMinSal, string LeaveDaySal, string WeekOfSal, string HolidaysSal, string AbsantSal, string LateMinPe, string EarlyMinPen, string PatialViolPen, string strEmpPenalty, string strTotalClaim, string strEmpLoan, string strTotalAllow, string strTotalDeduc, string Employee_PF, string Employer_PF, string Employee_ESIC, string Employer_ESIC,ref SqlTransaction trns)
    {
        PassDataToSql[] param = new PassDataToSql[25];
        param[0] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Emp_Id", strEmployeeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Month", strmonth, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Year", stryear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[4] = new PassDataToSql("@Worked_Min_Salary", WorkMinSal, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Normal_OT_Min_Salary", NorOTMinSal, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Week_Off_OT_Min_Salary", WeekOtMinSal, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[7] = new PassDataToSql("@Holiday_OT_Min_Salary", HolidayOtMinSal, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[8] = new PassDataToSql("@Leave_Days_Salary", LeaveDaySal, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[9] = new PassDataToSql("@Week_Off_Salary", WeekOfSal, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[10] = new PassDataToSql("@Holidays_Salary", HolidaysSal, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[11] = new PassDataToSql("@Absent_Salary", AbsantSal, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[12] = new PassDataToSql("@Late_Min_Penalty", LateMinPe, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[13] = new PassDataToSql("@Early_Min_Penalty", EarlyMinPen, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[14] = new PassDataToSql("@Patial_Violation_Penalty", PatialViolPen, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[15] = new PassDataToSql("@Employee_Penalty", strEmpPenalty, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[16] = new PassDataToSql("@Employee_Claim", strTotalClaim, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[17] = new PassDataToSql("@Emlployee_Loan", strEmpLoan, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[18] = new PassDataToSql("@Total_Allowance", strTotalAllow, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[19] = new PassDataToSql("@Total_Deduction", strTotalDeduc, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[20] = new PassDataToSql("@Employee_PF", Employee_PF, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[21] = new PassDataToSql("@Employer_PF", Employer_PF, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[22] = new PassDataToSql("@Employee_ESIC", Employee_ESIC, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[23] = new PassDataToSql("@Employer_ESIC", Employer_ESIC, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[24] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);


        da.execute_Sp("sp_Pay_Employe_Month_Temp_Insert", param,ref trns);
        return Convert.ToInt32(param[24].ParaValue);

        //return Convert.ToInt32(param[3].ParaValue);

    }

    public int DeleteEmpMonthTemp_By_EmpId_MonthandYear(string CompanyId, string strEmpId, string strMonth, string strYear)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Emp_Id", strEmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Month", strMonth, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Year", strYear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[4] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        da.execute_Sp("sp_Pay_Employe_Month_Temp_Delete", paramList);
        return Convert.ToInt32(paramList[3].ParaValue);
    }

    public int DeleteEmpMonthTemp_By_EmpId_MonthandYear(string CompanyId, string strEmpId, string strMonth, string strYear, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Emp_Id", strEmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Month", strMonth, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Year", strYear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[4] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        da.execute_Sp("sp_Pay_Employe_Month_Temp_Delete", paramList, ref trns);
        return Convert.ToInt32(paramList[3].ParaValue);
    }


    public int DeletePayrollByMonthYearEmpID(string strEmpId, string strMonth, string strYear)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Emp_Id", strEmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Month", strMonth, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Year", strYear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        da.execute_Sp("sp_Pay_Employe_Month_Delete", paramList);
        return Convert.ToInt32(paramList[2].ParaValue);
    }


    public DataTable GetPenaltyClaim(string strEmp_Id, string strCompanyId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Emp_Id", strEmp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Trans_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Month", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Year", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[4] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = da.Reuturn_Datatable_Search("sp_Pay_Employe_Month_Temp_Select_Row", paramList);
        return dtInfo;
    }

    public DataTable GetPayEmpMonthTemp_By_EmployeeId(string strEmp_Id, string strCompanyId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Emp_Id", strEmp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Trans_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Month", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Year", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[4] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = da.Reuturn_Datatable_Search("sp_Pay_Employe_Month_Temp_Select_Row", paramList);
        return dtInfo;
    }

    public DataTable GetPayEmpMonthTemp_By_EmployeeId(string strEmp_Id,ref SqlTransaction trns, string strCompanyId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Emp_Id", strEmp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Trans_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Month", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Year", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[4] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = da.Reuturn_Datatable_Search("sp_Pay_Employe_Month_Temp_Select_Row", paramList, ref trns);
        return dtInfo;
    }

    // Get all records
    public DataTable GetPayTempData(string CompanyId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Emp_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Month", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Year", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[4] = new PassDataToSql("@optype", "5", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = da.Reuturn_Datatable_Search("sp_Pay_Employe_Month_Temp_Select_Row", paramList);
        return dtInfo;
    }

    public DataTable GetPayData(string CompanyId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Emp_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Month", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Year", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[4] = new PassDataToSql("@optype", "6", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = da.Reuturn_Datatable_Search("sp_Pay_Employe_Month_Temp_Select_Row", paramList);
        return dtInfo;
    }
    public DataTable GetallTemprecords_By_EmployeeId(string strEmp_Id, string strCompanyId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Emp_Id", strEmp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Trans_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Month", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Year", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[4] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = da.Reuturn_Datatable_Search("sp_Pay_Employe_Month_Temp_Select_Row", paramList);
        return dtInfo;
    }
    public DataTable GetallTemprecords_By_EmployeeId(string strEmp_Id, ref SqlTransaction trns, string strCompanyId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Emp_Id", strEmp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Trans_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Month", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Year", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[4] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = da.Reuturn_Datatable_Search("sp_Pay_Employe_Month_Temp_Select_Row", paramList, ref trns);
        return dtInfo;
    }
    public DataTable GetRecordByEmpIdMonthYear(string strEmp_Id, string strmonth, string stryear, string strCompanyId)
    {
        DataTable dtInfo1 = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Trans_Id",strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", strEmp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@month", strmonth, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@year", stryear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo1 = da.Reuturn_Datatable_Search("sp_Pay_Employe_Month_Temp_Select_Row", paramList);
        return dtInfo1;
    }

    public DataTable GetRecordByEmpIdMonthYear(string strEmp_Id, string strmonth, string stryear,ref SqlTransaction trns, string strCompanyId)
    {
        DataTable dtInfo1 = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Trans_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", strEmp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@month", strmonth, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@year", stryear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo1 = da.Reuturn_Datatable_Search("sp_Pay_Employe_Month_Temp_Select_Row", paramList, ref trns);
        return dtInfo1;
    }

    // Get Records of Prev_Balance and extra Fields..
    public DataTable GetRecord_PrvBal_Fields(string strEmp_Id, string strmonth, string stryear, string strCompanyId)
    {
        DataTable dtInfo1 = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Trans_Id",strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", strEmp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@month", strmonth, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@year", stryear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@optype", "4", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo1 = da.Reuturn_Datatable_Search("sp_Pay_Employe_Month_Temp_Select_Row", paramList);
        return dtInfo1;
    }

    // Updeate Records Prev_Monthly_Balance and extra Fields

    public int UpdateRecord_PrvBal_fields_By_TransId(string transid, string prvmonthbalance, string field1, string field2, string field3, string field4, string field5, string field6, string field7, string field8, string field9, string field10, string strCompanyId)
    {
        PassDataToSql[] param = new PassDataToSql[17];
        param[0] = new PassDataToSql("@Compny_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Trans_Id", transid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Emp_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@month", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[4] = new PassDataToSql("@year", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[5] = new PassDataToSql("@Prve_bal_monthy", Getdata(prvmonthbalance), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Field1", field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[7] = new PassDataToSql("@Field2", field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[8] = new PassDataToSql("@Field3", field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        param[9] = new PassDataToSql("@Field4", field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[10] = new PassDataToSql("@Field5", field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[11] = new PassDataToSql("@Field6", field6, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[12] = new PassDataToSql("@Field7", field7, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        param[13] = new PassDataToSql("@Field8", field8, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[14] = new PassDataToSql("@Field9", field9, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[15] = new PassDataToSql("@Field10", field10, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[16] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        da.execute_Sp("sp_Pay_Employe_Month_Temp_update_prvbal_fields", param);
        return Convert.ToInt32(param[16].ParaValue);
    }



    public int UpdateRecord_PrvBal_fields_By_TransId(string transid, string prvmonthbalance, string field1, string field2, string field3, string field4, string field5, string field6, string field7, string field8, string field9, string field10,ref SqlTransaction trns, string strCompanyId)
    {
        PassDataToSql[] param = new PassDataToSql[17];
        param[0] = new PassDataToSql("@Compny_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Trans_Id", transid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Emp_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@month", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[4] = new PassDataToSql("@year", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[5] = new PassDataToSql("@Prve_bal_monthy", Getdata(prvmonthbalance), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Field1", field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[7] = new PassDataToSql("@Field2", field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[8] = new PassDataToSql("@Field3", field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        param[9] = new PassDataToSql("@Field4", field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[10] = new PassDataToSql("@Field5", field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[11] = new PassDataToSql("@Field6", field6, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[12] = new PassDataToSql("@Field7", field7, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        param[13] = new PassDataToSql("@Field8", field8, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[14] = new PassDataToSql("@Field9", field9, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[15] = new PassDataToSql("@Field10", field10, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[16] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        da.execute_Sp("sp_Pay_Employe_Month_Temp_update_prvbal_fields", param,ref trns);
        return Convert.ToInt32(param[16].ParaValue);
    }

    public int UpdateRecord_Pay_Employee_Month(string strCompanyId, string strEmployeeId, string strmonth, string stryear, string strEmpPenalty, string strTotalClaim, string strEmpLoan, string strTotalAllow, string strTotalDeduc, string TotalPenalty, string TotalClaim, string Employee_PF, string Employer_PF, string Employee_ESIC, string Employer_ESIC, string Employer_FPF, string Employer_AdminCharges, string Employer_EDLI, string Employer_Inspection_Charges)
    {
        PassDataToSql[] param = new PassDataToSql[20];
        param[0] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Emp_Id", strEmployeeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Month", strmonth, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Year", stryear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[4] = new PassDataToSql("@Employee_Penalty", Getdata(strEmpPenalty), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[5] = new PassDataToSql("@Employee_Claim", Getdata(strTotalClaim), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Emlployee_Loan", Getdata(strEmpLoan), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[7] = new PassDataToSql("@Total_Allowance", Getdata(strTotalAllow), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[8] = new PassDataToSql("@Total_Deduction", Getdata(strTotalDeduc), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[9] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        param[10] = new PassDataToSql("@Field1", TotalPenalty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[11] = new PassDataToSql("@Field2", TotalClaim, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[12] = new PassDataToSql("@Employee_PF", Employee_PF, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[13] = new PassDataToSql("@Employer_PF", Employer_PF, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[14] = new PassDataToSql("@Employee_ESIC", Employee_ESIC, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[15] = new PassDataToSql("@Employer_ESIC", Employer_ESIC, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[16] = new PassDataToSql("@Field3", Employer_FPF, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[17] = new PassDataToSql("@Field4", Employer_AdminCharges, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[18] = new PassDataToSql("@Field5", Employer_EDLI, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[19] = new PassDataToSql("@Field6", Employer_Inspection_Charges, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        da.execute_Sp("sp_Pay_Employe_Month_Temp_Update", param);
        return Convert.ToInt32(param[9].ParaValue);
    }

    public int UpdateRecord_Pay_Employee_Month(string strCompanyId, string strEmployeeId, string strmonth, string stryear, string strEmpPenalty, string strTotalClaim, string strEmpLoan, string strTotalAllow, string strTotalDeduc, string TotalPenalty, string TotalClaim, string Employee_PF, string Employer_PF, string Employee_ESIC, string Employer_ESIC, string Employer_FPF, string Employer_AdminCharges, string Employer_EDLI, string Employer_Inspection_Charges,ref SqlTransaction trns)
    {
        PassDataToSql[] param = new PassDataToSql[20];
        param[0] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Emp_Id", strEmployeeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Month", strmonth, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Year", stryear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[4] = new PassDataToSql("@Employee_Penalty", Getdata(strEmpPenalty), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[5] = new PassDataToSql("@Employee_Claim", Getdata(strTotalClaim), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Emlployee_Loan", Getdata(strEmpLoan), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[7] = new PassDataToSql("@Total_Allowance", Getdata(strTotalAllow), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[8] = new PassDataToSql("@Total_Deduction", Getdata(strTotalDeduc), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[9] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        param[10] = new PassDataToSql("@Field1", TotalPenalty, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[11] = new PassDataToSql("@Field2", TotalClaim, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[12] = new PassDataToSql("@Employee_PF", Employee_PF, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[13] = new PassDataToSql("@Employer_PF", Employer_PF, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[14] = new PassDataToSql("@Employee_ESIC", Employee_ESIC, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[15] = new PassDataToSql("@Employer_ESIC", Employer_ESIC, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[16] = new PassDataToSql("@Field3", Employer_FPF, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[17] = new PassDataToSql("@Field4", Employer_AdminCharges, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[18] = new PassDataToSql("@Field5", Employer_EDLI, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[19] = new PassDataToSql("@Field6", Employer_Inspection_Charges, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        da.execute_Sp("sp_Pay_Employe_Month_Temp_Update", param, ref trns);
        return Convert.ToInt32(param[9].ParaValue);
    }


    // Update Attendance and salary records

    public int UpdateAttendenceRecord_By_EmpId_Monthandyear(string strEmployeeId, string strmonth, string stryear, string WorkMinSal, string NorOTMinSal, string WeekOtMinSal, string HolidayOtMinSal, string LeaveDaySal, string WeekOfSal, string HolidaysSal, string AbsantSal, string LateMinPen, string EarlyMinPen, string PatialViolPen, string strCompanyId)
    {
        PassDataToSql[] param = new PassDataToSql[16];
        param[0] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Emp_Id", strEmployeeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Month", strmonth, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Year", stryear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[4] = new PassDataToSql("@Worked_Min_Salary", Getdata(WorkMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Normal_OT_Min_Salary", Getdata(NorOTMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Week_Off_OT_Min_Salary", Getdata(WeekOtMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[7] = new PassDataToSql("@Holiday_OT_Min_Salary", Getdata(HolidayOtMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[8] = new PassDataToSql("@Leave_Days_Salary", Getdata(LeaveDaySal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[9] = new PassDataToSql("@Week_Off_Salary", Getdata(WeekOfSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[10] = new PassDataToSql("@Holidays_Salary", Getdata(HolidaysSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[11] = new PassDataToSql("@Absent_Salary", Getdata(AbsantSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[12] = new PassDataToSql("@Late_Min_Penalty", Getdata(LateMinPen), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[13] = new PassDataToSql("@Early_Min_Penalty", Getdata(EarlyMinPen), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[14] = new PassDataToSql("@Patial_Violation_Penalty", Getdata(PatialViolPen), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[15] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        da.execute_Sp("sp_Pay_Employe_Month_Temp_Attendance_Salary_Update", param);
        return Convert.ToInt32(param[15].ParaValue);
    }

    public int UpdateAttendenceRecord_By_EmpId_Monthandyear(string strEmployeeId, string strmonth, string stryear, string WorkMinSal, string NorOTMinSal, string WeekOtMinSal, string HolidayOtMinSal, string LeaveDaySal, string WeekOfSal, string HolidaysSal, string AbsantSal, string LateMinPen, string EarlyMinPen, string PatialViolPen, ref SqlTransaction trns, string strCompanyId)
    {
        PassDataToSql[] param = new PassDataToSql[16];
        param[0] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Emp_Id", strEmployeeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Month", strmonth, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Year", stryear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[4] = new PassDataToSql("@Worked_Min_Salary", Getdata(WorkMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Normal_OT_Min_Salary", Getdata(NorOTMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Week_Off_OT_Min_Salary", Getdata(WeekOtMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[7] = new PassDataToSql("@Holiday_OT_Min_Salary", Getdata(HolidayOtMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[8] = new PassDataToSql("@Leave_Days_Salary", Getdata(LeaveDaySal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[9] = new PassDataToSql("@Week_Off_Salary", Getdata(WeekOfSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[10] = new PassDataToSql("@Holidays_Salary", Getdata(HolidaysSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[11] = new PassDataToSql("@Absent_Salary", Getdata(AbsantSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[12] = new PassDataToSql("@Late_Min_Penalty", Getdata(LateMinPen), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[13] = new PassDataToSql("@Early_Min_Penalty", Getdata(EarlyMinPen), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[14] = new PassDataToSql("@Patial_Violation_Penalty", Getdata(PatialViolPen), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[15] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        da.execute_Sp("sp_Pay_Employe_Month_Temp_Attendance_Salary_Update", param, ref trns);
        return Convert.ToInt32(param[15].ParaValue);
    }

    public int UpdateRecord_Pay_Employee_Penalty_claim(string CompanyId, string strEmployeeId, string strEmpPenalty, string strTotalClaim)
    {
        PassDataToSql[] param = new PassDataToSql[5];

        param[0] = new PassDataToSql("@Emp_Id", strEmployeeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Employee_Penalty", strEmpPenalty, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Employee_Claim", strTotalClaim, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        param[4] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        //param[0] = new PassDataToSql("@Company_id",CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        //param[0] = new PassDataToSql("@Company_id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        da.execute_Sp("sp_Pay_Employe_Month_Temp_Update_penalty_claim", param);
        return Convert.ToInt32(param[3].ParaValue);
    }


    //  records inserted of post payroll into Pay Emp Month Table
    public int Insert_posted_Pay_Emp_Month(string strCompanyId, string strEmployeeId, string strmonth, string stryear, string WorkMinSal, string NorOTMinSal, string WeekOtMinSal, string HolidayOtMinSal, string LeaveDaySal, string WeekOfSal, string HolidaysSal, string AbsantSal, string LateMinPe, string EarlyMinPen, string PatialViolPen, string strEmpPenalty, string strTotalClaim, string strEmpLoan, string strTotalAllow, string strTotalDeduc, string strprvmonthbal, string postdate, string Employee_Pf, string Employer_PF, string Employee_ESIC, string Employer_ESIC, string field1, string field2, string field3, string field4, string field5, string field6, string field7, string field8, string field9, string field10, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate)
    {
        PassDataToSql[] param = new PassDataToSql[42];
        param[0] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Emp_Id", strEmployeeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Month", strmonth, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Year", stryear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[4] = new PassDataToSql("@Worked_Min_Salary", Getdata(WorkMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Normal_OT_Min_Salary", Getdata(NorOTMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Week_Off_OT_Min_Salary", Getdata(WeekOtMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[7] = new PassDataToSql("@Holiday_OT_Min_Salary", Getdata(HolidayOtMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[8] = new PassDataToSql("@Leave_Days_Salary", Getdata(LeaveDaySal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[9] = new PassDataToSql("@Week_Off_Salary", Getdata(WeekOfSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[10] = new PassDataToSql("@Holidays_Salary", Getdata(HolidaysSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[11] = new PassDataToSql("@Absent_Salary", Getdata(AbsantSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[12] = new PassDataToSql("@Late_Min_Penalty", Getdata(LateMinPe), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[13] = new PassDataToSql("@Early_Min_Penalty", Getdata(EarlyMinPen), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[14] = new PassDataToSql("@Patial_Violation_Penalty", Getdata(PatialViolPen), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[15] = new PassDataToSql("@Employee_Penalty", Getdata(strEmpPenalty), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[16] = new PassDataToSql("@Employee_Claim", Getdata(strTotalClaim), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[17] = new PassDataToSql("@Emlployee_Loan", Getdata(strEmpLoan), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[18] = new PassDataToSql("@Total_Allowance", Getdata(strTotalAllow), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[19] = new PassDataToSql("@Total_Deduction", Getdata(strTotalDeduc), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[20] = new PassDataToSql("@Previous_Month_Balance", Getdata(strprvmonthbal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[21] = new PassDataToSql("@Posted_Date", postdate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        param[22] = new PassDataToSql("@Field1", field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[23] = new PassDataToSql("@Field2", field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[24] = new PassDataToSql("@Field3", field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[25] = new PassDataToSql("@Field4", field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[26] = new PassDataToSql("@Field5", field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[27] = new PassDataToSql("@Field6", field6, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[28] = new PassDataToSql("@Field7", field7, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[29] = new PassDataToSql("@Field8", field8, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[30] = new PassDataToSql("@Field9", field9, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[31] = new PassDataToSql("@Field10", field10, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);


        param[32] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        param[33] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[34] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[35] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[36] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


        param[37] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        param[38] = new PassDataToSql("@Employee_PF", Employee_Pf, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[39] = new PassDataToSql("@Employer_PF", Employer_PF, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[40] = new PassDataToSql("@Employee_ESIC", Employee_ESIC, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[41] = new PassDataToSql("@Employer_ESIC", Employer_ESIC, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        da.execute_Sp("sp_Pay_Employe_Month_Insert", param);
        return Convert.ToInt32(param[37].ParaValue);

        //return Convert.ToInt32(param[3].ParaValue);

    }

    public int Insert_posted_Pay_Emp_Month(string strCompanyId, string strEmployeeId, string strmonth, string stryear, string WorkMinSal, string NorOTMinSal, string WeekOtMinSal, string HolidayOtMinSal, string LeaveDaySal, string WeekOfSal, string HolidaysSal, string AbsantSal, string LateMinPe, string EarlyMinPen, string PatialViolPen, string strEmpPenalty, string strTotalClaim, string strEmpLoan, string strTotalAllow, string strTotalDeduc, string strprvmonthbal, string postdate, string Employee_Pf, string Employer_PF, string Employee_ESIC, string Employer_ESIC, string field1, string field2, string field3, string field4, string field5, string field6, string field7, string field8, string field9, string field10, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate, ref SqlTransaction trns)
    {
        PassDataToSql[] param = new PassDataToSql[42];
        param[0] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Emp_Id", strEmployeeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Month", strmonth, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Year", stryear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[4] = new PassDataToSql("@Worked_Min_Salary", Getdata(WorkMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Normal_OT_Min_Salary", Getdata(NorOTMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Week_Off_OT_Min_Salary", Getdata(WeekOtMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[7] = new PassDataToSql("@Holiday_OT_Min_Salary", Getdata(HolidayOtMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[8] = new PassDataToSql("@Leave_Days_Salary", Getdata(LeaveDaySal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[9] = new PassDataToSql("@Week_Off_Salary", Getdata(WeekOfSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[10] = new PassDataToSql("@Holidays_Salary", Getdata(HolidaysSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[11] = new PassDataToSql("@Absent_Salary", Getdata(AbsantSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[12] = new PassDataToSql("@Late_Min_Penalty", Getdata(LateMinPe), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[13] = new PassDataToSql("@Early_Min_Penalty", Getdata(EarlyMinPen), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[14] = new PassDataToSql("@Patial_Violation_Penalty", Getdata(PatialViolPen), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[15] = new PassDataToSql("@Employee_Penalty", Getdata(strEmpPenalty), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[16] = new PassDataToSql("@Employee_Claim", Getdata(strTotalClaim), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[17] = new PassDataToSql("@Emlployee_Loan", Getdata(strEmpLoan), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[18] = new PassDataToSql("@Total_Allowance", Getdata(strTotalAllow), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[19] = new PassDataToSql("@Total_Deduction", Getdata(strTotalDeduc), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[20] = new PassDataToSql("@Previous_Month_Balance", Getdata(strprvmonthbal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[21] = new PassDataToSql("@Posted_Date", postdate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        param[22] = new PassDataToSql("@Field1", field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[23] = new PassDataToSql("@Field2", field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[24] = new PassDataToSql("@Field3", field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[25] = new PassDataToSql("@Field4", field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[26] = new PassDataToSql("@Field5", field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[27] = new PassDataToSql("@Field6", field6, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[28] = new PassDataToSql("@Field7", field7, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[29] = new PassDataToSql("@Field8", field8, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[30] = new PassDataToSql("@Field9", field9, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[31] = new PassDataToSql("@Field10", field10, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);


        param[32] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        param[33] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[34] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[35] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[36] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


        param[37] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        param[38] = new PassDataToSql("@Employee_PF", Employee_Pf, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[39] = new PassDataToSql("@Employer_PF", Employer_PF, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[40] = new PassDataToSql("@Employee_ESIC", Employee_ESIC, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[41] = new PassDataToSql("@Employer_ESIC", Employer_ESIC, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        da.execute_Sp("sp_Pay_Employe_Month_Insert", param, ref trns);
        return Convert.ToInt32(param[37].ParaValue);

        //return Convert.ToInt32(param[3].ParaValue);

    }

    public int Update_posted_Pay_Emp_Month(string strCompanyId, string strEmployeeId, string strmonth, string stryear, string WorkMinSal, string NorOTMinSal, string WeekOtMinSal, string HolidayOtMinSal, string LeaveDaySal, string WeekOfSal, string HolidaysSal, string AbsantSal, string LateMinPe, string EarlyMinPen, string PatialViolPen, string strEmpPenalty, string strTotalClaim, string strEmpLoan, string strTotalAllow, string strTotalDeduc, string strprvmonthbal, string postdate, string field1, string field2, string field3, string field4, string field5, string field6, string field7, string field8, string field9, string field10, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate)
    {
        PassDataToSql[] param = new PassDataToSql[38];
        param[0] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Emp_Id", strEmployeeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Month", strmonth, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Year", stryear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[4] = new PassDataToSql("@Worked_Min_Salary", Getdata(WorkMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Normal_OT_Min_Salary", Getdata(NorOTMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Week_Off_OT_Min_Salary", Getdata(WeekOtMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[7] = new PassDataToSql("@Holiday_OT_Min_Salary", Getdata(HolidayOtMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[8] = new PassDataToSql("@Leave_Days_Salary", Getdata(LeaveDaySal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[9] = new PassDataToSql("@Week_Off_Salary", Getdata(WeekOfSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[10] = new PassDataToSql("@Holidays_Salary", Getdata(HolidaysSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[11] = new PassDataToSql("@Absent_Salary", Getdata(AbsantSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[12] = new PassDataToSql("@Late_Min_Penalty", Getdata(LateMinPe), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[13] = new PassDataToSql("@Early_Min_Penalty", Getdata(EarlyMinPen), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[14] = new PassDataToSql("@Patial_Violation_Penalty", Getdata(PatialViolPen), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[15] = new PassDataToSql("@Employee_Penalty", Getdata(strEmpPenalty), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[16] = new PassDataToSql("@Employee_Claim", Getdata(strTotalClaim), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[17] = new PassDataToSql("@Emlployee_Loan", Getdata(strEmpLoan), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[18] = new PassDataToSql("@Total_Allowance", Getdata(strTotalAllow), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[19] = new PassDataToSql("@Total_Deduction", Getdata(strTotalDeduc), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[20] = new PassDataToSql("@Previous_Month_Balance", Getdata(strprvmonthbal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[21] = new PassDataToSql("@Posted_Date", postdate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        param[22] = new PassDataToSql("@Field1", field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[23] = new PassDataToSql("@Field2", field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[24] = new PassDataToSql("@Field3", field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[25] = new PassDataToSql("@Field4", field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[26] = new PassDataToSql("@Field5", field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[27] = new PassDataToSql("@Field6", field6, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[28] = new PassDataToSql("@Field7", field7, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[29] = new PassDataToSql("@Field8", field8, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[30] = new PassDataToSql("@Field9", field9, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[31] = new PassDataToSql("@Field10", field10, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);


        param[32] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        param[33] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[34] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[35] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[36] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


        param[37] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        da.execute_Sp("sp_Pay_Employe_Month_Update", param);
        return Convert.ToInt32(param[37].ParaValue);

        //return Convert.ToInt32(param[3].ParaValue);

    }

    public int Update_posted_Pay_Emp_Month(string strCompanyId, string strEmployeeId, string strmonth, string stryear, string WorkMinSal, string NorOTMinSal, string WeekOtMinSal, string HolidayOtMinSal, string LeaveDaySal, string WeekOfSal, string HolidaysSal, string AbsantSal, string LateMinPe, string EarlyMinPen, string PatialViolPen, string strEmpPenalty, string strTotalClaim, string strEmpLoan, string strTotalAllow, string strTotalDeduc, string strprvmonthbal, string postdate, string field1, string field2, string field3, string field4, string field5, string field6, string field7, string field8, string field9, string field10, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate, ref SqlTransaction trns)
    {
        PassDataToSql[] param = new PassDataToSql[38];
        param[0] = new PassDataToSql("@Company_Id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Emp_Id", strEmployeeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Month", strmonth, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Year", stryear, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[4] = new PassDataToSql("@Worked_Min_Salary", Getdata(WorkMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Normal_OT_Min_Salary", Getdata(NorOTMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Week_Off_OT_Min_Salary", Getdata(WeekOtMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[7] = new PassDataToSql("@Holiday_OT_Min_Salary", Getdata(HolidayOtMinSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[8] = new PassDataToSql("@Leave_Days_Salary", Getdata(LeaveDaySal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[9] = new PassDataToSql("@Week_Off_Salary", Getdata(WeekOfSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[10] = new PassDataToSql("@Holidays_Salary", Getdata(HolidaysSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[11] = new PassDataToSql("@Absent_Salary", Getdata(AbsantSal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[12] = new PassDataToSql("@Late_Min_Penalty", Getdata(LateMinPe), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[13] = new PassDataToSql("@Early_Min_Penalty", Getdata(EarlyMinPen), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[14] = new PassDataToSql("@Patial_Violation_Penalty", Getdata(PatialViolPen), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[15] = new PassDataToSql("@Employee_Penalty", Getdata(strEmpPenalty), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[16] = new PassDataToSql("@Employee_Claim", Getdata(strTotalClaim), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[17] = new PassDataToSql("@Emlployee_Loan", Getdata(strEmpLoan), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[18] = new PassDataToSql("@Total_Allowance", Getdata(strTotalAllow), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[19] = new PassDataToSql("@Total_Deduction", Getdata(strTotalDeduc), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[20] = new PassDataToSql("@Previous_Month_Balance", Getdata(strprvmonthbal), PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[21] = new PassDataToSql("@Posted_Date", postdate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        param[22] = new PassDataToSql("@Field1", field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[23] = new PassDataToSql("@Field2", field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[24] = new PassDataToSql("@Field3", field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[25] = new PassDataToSql("@Field4", field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[26] = new PassDataToSql("@Field5", field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[27] = new PassDataToSql("@Field6", field6, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[28] = new PassDataToSql("@Field7", field7, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[29] = new PassDataToSql("@Field8", field8, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[30] = new PassDataToSql("@Field9", field9, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[31] = new PassDataToSql("@Field10", field10, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);


        param[32] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        param[33] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[34] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[35] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[36] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


        param[37] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        da.execute_Sp("sp_Pay_Employe_Month_Update", param, ref trns);
        return Convert.ToInt32(param[37].ParaValue);

        //return Convert.ToInt32(param[3].ParaValue);

    }


    //Get all posted records from Pay Emp Month Table

    public DataTable GetAllRecordPostedEmpMonth(string Empid, string month, string year, string strCompanyId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Emp_Id", Empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Month", month, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Year", year, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@company_id", strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = da.Reuturn_Datatable_Search("sp_Pay_Employe_Month_Select_Row", paramList);
        return dtInfo;
    }

    public DataTable GetAllRecordPostedEmpMonth(string Empid, string month, string year, ref SqlTransaction trns, string strCompanyId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Emp_Id", Empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Month", month, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Year", year, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@company_id",strCompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = da.Reuturn_Datatable_Search("sp_Pay_Employe_Month_Select_Row", paramList, ref trns);
        return dtInfo;
    }

    public DataTable GetAllRecordPostedByCompanyId(string CompanyId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Emp_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Month", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Year", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@company_id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = da.Reuturn_Datatable_Search("sp_Pay_Employe_Month_Select_Row", paramList);
        return dtInfo;
    }
    public int RollBackTransaction(string companyId, string EmployeeId, string Month, string Year)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", companyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Month", Month, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Year", Year, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@RefferenceId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[4] = new PassDataToSql("@Emp_Id", EmployeeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        da.execute_Sp("sp_Pay_Employe_Month_RollBackPayroll", paramList);
        return Convert.ToInt32(paramList[3].ParaValue);
    }
    public int RollBackEmpTransaction(string EmployeeId)
    {
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Employee_Id", EmployeeId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Reference_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        da.execute_Sp("sp_Pay_EmpRollback", paramList);
        return Convert.ToInt32(paramList[1].ParaValue);
    }
    public string Getdata(string type)
    {
        if (type == "")
        {
            type = "0";
        }
        return type;
    }
}
