using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PegasusDataAccess;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for HR_EmployeeDetail
/// </summary>
public class HR_EmployeeDetail
{
    DataAccessClass daClass = null;    
    private string _Emp_Id;
    private string _Month;
    private string _Year;
    private string _TotalDays;
    private string _PresentDays;
    private string _AbsentDays;
    private string _WeekOffDays;
    private string _LeaveDays;
    private string _HolidayDays;
    private string _LateMin;
    private string _EarlyMin;
    private string _PartialMin;
    private string _CreatBy;
    private string _ModifyBy;
    private DateTime _CreatDate;
    private DateTime _ModifyDate;
    private string _Normal_OTMin;
    private string _WeekOff_OTMin;
    private string _Holiday_OTMin;

    public HR_EmployeeDetail(string strConString)
    {
        daClass = new DataAccessClass(strConString);
    }
    public string Emp_Id
    {
        get
        {
            return _Emp_Id;
        }
        set
        {
            _Emp_Id = value;
        }
    }
    public string Month
    {
        get
        {
            return _Month;
        }
        set
        {
            _Month = value;
        }
    }
    public string Year
    {
        get
        {
            return _Year;
        }
        set
        {
            _Year = value;
        }
    }
    public string TotalDays
    {
        get
        {
            return _TotalDays;
        }
        set
        {
            _TotalDays = value;
        }
    }
    public string PresentDays
    {
        get
        {
            return _PresentDays;
        }
        set
        {
            _PresentDays = value;
        }
    }
    public string AbsentDays
    {
        get
        {
            return _AbsentDays;
        }
        set
        {
            _AbsentDays = value;
        }
    }
    public string LeaveDays
    {
        get
        {
            return _LeaveDays;
        }
        set
        {
            _LeaveDays = value;
        }
    }
    public string HolidayDays
    {
        get
        {
            return _HolidayDays;
        }
        set
        {
            _HolidayDays = value;
        }
    }
    public string WeekOffDays
    {
        get
        {
            return _WeekOffDays;
        }
        set
        {
            _WeekOffDays = value;
        }
    }
    public string LateMin
    {
        get
        {
            return _LateMin;
        }
        set
        {
            _LateMin = value;
        }
    }
    public string EarlyMin
    {
        get
        {
            return _EarlyMin;
        }
        set
        {
            _EarlyMin = value;
        }
    }
    public string PartialMin
    {
        get
        {
            return _PartialMin;
        }
        set
        {
            _PartialMin = value;
        }
    }

    public string ModifyBy
    {
        get
        {
            return _ModifyBy;
        }
        set
        {
            _ModifyBy = value;
        }
    }
    public string Normal_OTMin
    {
        get
        {
            return _Normal_OTMin;
        }
        set
        {
            _Normal_OTMin = value;
        }
    }
    public string WeekOff_OTMin
    {
        get
        {
            return _WeekOff_OTMin;
        }
        set
        {
            _WeekOff_OTMin = value;
        }
    }
    public string Holiday_OTMin
    {
        get
        {
            return _Holiday_OTMin;
        }
        set
        {
            _Holiday_OTMin = value;
        }
    }
    public DateTime ModifyDate
    {
        get
        {
            return _ModifyDate;
        }
        set
        {
            _ModifyDate = value;
        }
    }
    public DateTime CreateDate
    {
        get
        {
            return _CreatDate;
        }
        set
        {
            _CreatDate = value;
        }
    }
    public string CreatBy
    {
        get
        {
            return _CreatBy;
        }
        set
        {
            _CreatBy = value;
        }
    }


    public int InsertORUpdateRecord(string Trans_Id, string Emp_Id, string Month, string Year, string TotalDays, string Present, string Holiday, string WeekOff, string Absent, string Leave, string Late, string Early, string Partial, string NormalOT, string WeekOffOT, string HolidayOT, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string Is_Active, string Created_By, string Created_Date, string Modified_By, string Modified_Date)
    {
        PassDataToSql[] paramList = new PassDataToSql[30];
        paramList[0] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Month", Month, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Year", Year, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Total_Days", TotalDays, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Present_Days", Present, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Holiday_Days", Holiday, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@WeekOff_Days", WeekOff, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Absent_Days", Absent, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Leave_Days", Leave, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Late_Min", Late, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Early_Min", Early, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Partial_Min", Partial, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Is_Post", false.ToString(), PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@Is_Active", Is_Active, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@Created_By", Created_By, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@Created_Date", Created_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@Modified_By", Modified_By, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@Modified_Date", Modified_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@Reference_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        paramList[27] = new PassDataToSql("@Normal_OTMin", NormalOT, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@WeekOff_OTMin", WeekOffOT, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[29] = new PassDataToSql("@Holiday_OTMin", HolidayOT, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_HR_EmployeeDetail_InsertUpdate", paramList);
        return Convert.ToInt32(paramList[26].ParaValue);
    }
    public DataTable GetAllTrueRecord()
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_HR_EmployeeDetail_SelectRow", paramList);
        return dtInfo;
    }


    public DataTable GetAllTrueRecord(ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_HR_EmployeeDetail_SelectRow", paramList,ref trns);
        return dtInfo;
    }
   
    public DataTable GetAllFalseRecord()
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_HR_EmployeeDetail_SelectRow", paramList);
        return dtInfo;
    }

    public DataTable GetAllRecord()
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_HR_EmployeeDetail_SelectRow", paramList);
        return dtInfo;
    }

    public DataTable GetRecord_By_TransId(string Candidate_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Trans_Id", Candidate_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@optype", "4", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_HR_EmployeeDetail_SelectRow", paramList);
        return dtInfo;
    }
    public DataTable GetRecord_By_EmpId(string Candidate_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Trans_Id", Candidate_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@optype", "5", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_HR_EmployeeDetail_SelectRow", paramList);
        return dtInfo;
    }

    public DataTable GetNonPostedRecord(string EmpId, string Month, string Year)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Month", Month, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Year", Year, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("Get_Pay_Employe_Month_Temp", paramList);
        return dtInfo;
    }

    public int UpdatePayrllSalaryRecord(string Emp_Id, string Month, string Year, string Present, string Holiday, string WeekOff, string Absent, string Leave, string Late, string Early, string Partial, string Normal_OT, string WeekOff_OT, string Holiday_OT)
    {
        PassDataToSql[] paramList = new PassDataToSql[15];
        paramList[0] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Month", Month, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Year", Year, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Work_Sal", Present, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Holiday_Sal", Holiday, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Weekoff_Sal", WeekOff, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Absent_Sal", Absent, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Leave_Sal", Leave, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Late_MinSal", Late, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Early_MinSal", Early, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Partial_MinSal", Partial, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Reference_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        paramList[12] = new PassDataToSql("@Normal_OTSal", Normal_OT, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@WeekOff_OTSal", WeekOff_OT, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Holiday_OTSal", Holiday_OT, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Pay_Employee_Due_Payment_Temp_Update", paramList);
        return Convert.ToInt32(paramList[11].ParaValue);
    }

    public DataTable GetFieldName()
    {
        string query = "select Distinct(Column_Name),'1'  Nec  from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME In ('HR_EmployeeDetail') and Column_name  in ('Emp_Id','Month','Year','Total_Days','Present_Days','Holiday_Days','WeekOff_Days','Absent_Days','Leave_Days','Late_Min','Early_Min','Partial_Min','Normal_OTMin','WeekOff_OTMin','Holiday_OTMin')";
        DataTable dt = daClass.return_DataTable(query);
        return dt;
    }
    public string GetEmployeeId(string EmpCode)
    {
        try
        {
            string query = "select Emp_Id from Set_EmployeeMaster where Emp_Code=" + EmpCode + "";
            string EmpId = daClass.get_SingleValue(query);
            return EmpId;
        }
        catch
        {
            return "0";
        }
    }

    public string GetEmployeeCode(string EmpID)
    {
        try
        {
            string query = "select Emp_Code from Set_EmployeeMaster where Emp_Id=" + EmpID + "";
            string EmpId = daClass.get_SingleValue(query);
            return EmpId;
        }
        catch
        {
            return "0";
        }
    }

    public string GetEmpEmailID(string EmpID)
    {
        try
        {
            string query = "select Email_Id from Set_EmployeeMaster where Emp_Id=" + EmpID + "";
            string EmpEmailId = daClass.get_SingleValue(query);
            return EmpEmailId;
        }
        catch
        {
            return "0";
        }
    }

    public DataTable GetEmpName_Code(string EmpID)
    {
        try
        {
            DataTable dt = new DataTable();
            string query = "select Emp_Name,Emp_Code from Set_EmployeeMaster where Emp_Id=" + EmpID + "";
            dt = daClass.return_DataTable(query);
            return dt;
        }
        catch
        {
            return null;
        }
    }

    public DataTable GetEmpName_CodeByUserID(string UserID)
    {
        try
        {
            DataTable dt = new DataTable();
            string query = "select Set_EmployeeMaster.Emp_Name from Set_EmployeeMaster inner join Set_UserMaster on Set_UserMaster.Emp_Id=Set_EmployeeMaster.Emp_Id where Set_UserMaster.User_Id = '"+UserID+"'";
            dt = daClass.return_DataTable(query);
            return dt;
        }
        catch
        {
            return null;
        }
    }
}
