using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using PegasusDataAccess;
using System.Data.SqlClient;


/// <summary>
/// Summary description for Attendance
/// </summary>
public class Attendance
{
    DataAccessClass daClass = null;
    Set_ApplicationParameter objAppParam = null;
    EmployeeParameter objEmpParam = null;
    Att_HalfDay_Request objHalfDay = null;
    Att_Leave_Request objleaveReq = null;
    CompanyMaster objComp = null;
    BrandMaster objBrand = null;
    LocationMaster objLoc = null;
    Att_OverTime_Request objOTRequest = null;
    Att_Employee_HalfDay objEmpHalfDay = null;
    Att_PartialLeave_Request objPartial = null;
    SystemParameter objSys = null;
    Set_Employee_Holiday objEmpHoliday = null;
    Att_ScheduleMaster objEmpSch = null;
    LeaveMaster objleave = null;
    private string _strConString = string.Empty;
    public Attendance(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
        objAppParam = new Set_ApplicationParameter(strConString);
        objEmpParam = new EmployeeParameter(strConString);
        objHalfDay = new Att_HalfDay_Request(strConString);
        objleaveReq = new Att_Leave_Request(strConString);
        objComp = new CompanyMaster(strConString);
        objBrand = new BrandMaster(strConString);
        objLoc = new LocationMaster(strConString);
        objOTRequest = new Att_OverTime_Request(strConString);
        objEmpHalfDay = new Att_Employee_HalfDay(strConString);
        objPartial = new Att_PartialLeave_Request(strConString);
        objSys = new SystemParameter(strConString);
        objEmpHoliday = new Set_Employee_Holiday(strConString);
        objEmpSch = new Att_ScheduleMaster(strConString);
        objleave = new LeaveMaster(strConString);
        _strConString = strConString;
    }
    public DataTable GetEmployeeList(string CompanyId, string BrandId, string LocationId, string OpType)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", LocationId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        // paramList[3] = new PassDataToSql("@Departemnt_Ids", DepartmentIds, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Op_Type", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Get_Set_EmployeeMaster_SelectRow", paramList);
        return dtInfo;
    }
    public DataTable GetEmployeeDOJ(string CompanyId, string EmpId)
    {
        DataTable dtInfo = new DataTable();
        //PassDataToSql[] paramList = new PassDataToSql[5];
        //paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);      
        //paramList[3] = new PassDataToSql("@EmpId", EmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);        
        dtInfo = daClass.return_DataTable("Select DOJ From Set_EmployeeMaster Where Emp_Id=" + EmpId + " AND Company_Id=" + CompanyId + " AND IsActive='True'");
        return dtInfo;
    }

    public DataTable GetEmployeeDOJ(string CompanyId, string EmpId, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        //PassDataToSql[] paramList = new PassDataToSql[5];
        //paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);      
        //paramList[3] = new PassDataToSql("@EmpId", EmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);        
        dtInfo = daClass.return_DataTable("Select DOJ From Set_EmployeeMaster Where Emp_Id=" + EmpId + " AND Company_Id=" + CompanyId + " AND IsActive='True'", ref trns);
        return dtInfo;
    }
    public DataTable GetEmployeeParameter(string CompanyId, string EmpId)
    {
        DataTable dtInfo = new DataTable();
        dtInfo = daClass.return_DataTable("select Basic_Salary,Assign_Min,Emp_Id from Set_Employee_Parameter Where ISNULL(IsActive,0)=1 AND Company_Id=" + CompanyId + " AND Emp_Id=" + EmpId + "");
        return dtInfo;
    }
    // Function For Salary Calculation Method Monthly/Fixed Days

    public int GetTotalDaysInMonth(int CompId, int Emp_Month, int Emp_Year)
    {
        DataTable dtInfo = new DataTable();
        dtInfo = daClass.return_DataTable("select * from Set_ApplicationParameter where (Param_Name = 'Salary Calculate According To' OR Param_Name='Days In Month') AND Company_Id=" + CompId + "");
        int TotalMonthDays = 0;
        string SalaryCalculationMethod = "";

        SalaryCalculationMethod = dtInfo.Rows[0]["Param_Value"].ToString();
        if (SalaryCalculationMethod == "Monthly")
        {
            TotalMonthDays = DateTime.DaysInMonth(Convert.ToInt32(Emp_Year), Convert.ToInt32(Emp_Month));
        }
        else
        {
            TotalMonthDays = Convert.ToInt32(dtInfo.Rows[1]["Param_Value"].ToString());
        }
        return TotalMonthDays;
    }

    public double GetEmpSalary(int CompId, int EmpId, int Month, int Year, int AssignMin, int SalaryType, int NoOfDays)
    {
        double EmpSal = 0;
        int TotalDaysInMonth = GetTotalDaysInMonth(CompId, Month, Year);

        if (SalaryType == 1)
        {
            // Return per Day Salary


        }
        else
        {
            // Retrun Per Min Salary
        }

        return EmpSal;
    }

    public DataTable GetSalCalculationMethod(string CompanyId, string BrandId, string LocationId)
    {
        DataTable dtInfo = new DataTable();
        dtInfo = daClass.return_DataTable("select * from Set_ApplicationParameter where (Param_Name = 'Salary Calculate According To' OR Param_Name='Days In Month') AND Company_Id=" + CompanyId + " AND Brand_Id=" + BrandId + " AND Location_Id=" + LocationId + "");
        return dtInfo;
    }
    // Get Employee Name
    public DataTable GetEmployeeName(string EmpCode)
    {
        DataTable dtInfo = new DataTable();
        dtInfo = daClass.return_DataTable("select Emp_Name From Set_EmployeeMaster where Emp_Code='" + EmpCode + "'");
        return dtInfo;
    }

    // Get Employee Name
    public string GetEmployeeNameByEmpId(string EmpId)
    {
        string EmpName;
        DataTable dtInfo = new DataTable();
        dtInfo = daClass.return_DataTable("select Emp_Name From Set_EmployeeMaster where Emp_Id='" + EmpId + "'");
        return dtInfo.Rows[0]["Emp_Name"].ToString();
    }
    // Absent Penalty 
    public double AbsentDaysSalary(string CompId, double PerMinSal, string EmpId, int PerDayAssignMin, double AbsentDays, string strBrandId, string strLocId)
    {
        double absentsal = 0;

        string AbsentType = string.Empty;
        double Value = 0;
        AbsentType = objAppParam.GetApplicationParameterValueByParamName("Absent_Type", CompId, strBrandId, strLocId);
        Value = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Absent_Value", CompId, strBrandId, strLocId));

        //if (AbsentType == "2")
        //{
        //    Value = Value - 100;
        //}
        bool IsEmpAbsent = false;
        try
        {
            IsEmpAbsent = Convert.ToBoolean(objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Field3"));

        }
        catch
        {

        }
        if (IsEmpAbsent)
        {

            if (AbsentType == "2")
            {
                absentsal = (PerMinSal * Value) / 100;


                //absentsal = (PerMinSal * Value);


                absentsal = absentsal * PerDayAssignMin * AbsentDays;

            }
            else if (AbsentType == "1")
            {
                //absentsal = Value / 60;


                absentsal = Value;
            }

        }
        try
        {
            //absentsal = absentsal * PerDayAssignMin * AbsentDays;
        }
        catch (Exception Ex)
        {
        }
        return absentsal;
    }

    public double AbsentDaysSalary(string CompId, double PerMinSal, string EmpId, int PerDayAssignMin, double AbsentDays, ref SqlTransaction trns, string strBrandId, string strLocId)
    {
        double absentsal = 0;

        string AbsentType = string.Empty;
        double Value = 0;
        AbsentType = objAppParam.GetApplicationParameterValueByParamName("Absent_Type", CompId, strBrandId, strLocId, ref trns);
        Value = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Absent_Value", CompId, strBrandId, strLocId, ref trns));

        //if (AbsentType == "2")
        //{
        //    Value = Value - 100;
        //}
        bool IsEmpAbsent = false;
        try
        {
            IsEmpAbsent = Convert.ToBoolean(objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Field3", ref trns));

        }
        catch
        {

        }
        if (IsEmpAbsent)
        {

            if (AbsentType == "2")
            {
                absentsal = (PerMinSal * Value) / 100;


                //absentsal = (PerMinSal * Value);


                absentsal = absentsal * PerDayAssignMin * AbsentDays;

            }
            else if (AbsentType == "1")
            {
                //absentsal = Value / 60;


                absentsal = Value;
            }

        }
        try
        {
            //absentsal = absentsal * PerDayAssignMin * AbsentDays;
        }
        catch (Exception Ex)
        {
        }
        return absentsal;
    }

    // Late Penalty
    public double LatePenalty(string CompId, double PerMinSal, string EmpId, int PerDayAssignMin, int LateMin, string PageType, string strBrandId, string strlocId)
    {
        double sal = 0;
        string Method = string.Empty;
        string Type = string.Empty;
        double Value = 0;
        bool IsEmpLate = false;
        int Late_Penalty_Min_Deduct = 0;
        try
        {
            IsEmpLate = Convert.ToBoolean(objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Field1"));
        }
        catch
        {
        }
        Method = objAppParam.GetApplicationParameterValueByParamName("Is_Late_Penalty_Method", CompId, strBrandId, strlocId);

        if (IsEmpLate)
        {

            if (Method == "Salary")
            {
                Type = objAppParam.GetApplicationParameterValueByParamName("Is_Late_Penalty_Salary_Type", CompId, strBrandId, strlocId);
                Value = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Is_Late_Penalty_Salary_Value", CompId, strBrandId, strlocId));

                if (Type == "2")
                {
                    sal = (PerMinSal * Value) / 100;

                }
                else
                {
                    sal = Value / 60;
                }
            }
            else
            {
                Late_Penalty_Min_Deduct = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Late_Penalty_Min_Deduct", CompId, strBrandId, strlocId));
                if (PageType == "EmpDetail" || PageType == "DailySalary" || PageType == "LogProcess")
                {
                    sal = PerMinSal * Late_Penalty_Min_Deduct;
                }
                else
                {
                    sal = PerMinSal;
                }

            }


            //......
        }
        else
        {
            sal = 0;
        }
        try
        {
            sal = sal * LateMin;
        }
        catch (Exception Ex)
        {

        }
        return sal;

    }
    public double LatePenalty(string CompId, double PerMinSal, string EmpId, int PerDayAssignMin, int LateMin, string PageType, ref SqlTransaction trns, string strBrandId, string strlocId)
    {
        double sal = 0;
        string Method = string.Empty;
        string Type = string.Empty;
        double Value = 0;
        bool IsEmpLate = false;
        int Late_Penalty_Min_Deduct = 0;
        try
        {
            IsEmpLate = Convert.ToBoolean(objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Field1", ref trns));
        }
        catch
        {
        }
        Method = objAppParam.GetApplicationParameterValueByParamName("Is_Late_Penalty_Method", CompId, strBrandId, strlocId, ref trns);

        if (IsEmpLate)
        {

            if (Method == "Salary")
            {
                Type = objAppParam.GetApplicationParameterValueByParamName("Is_Late_Penalty_Salary_Type", CompId, strBrandId, strlocId, ref trns);
                Value = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Is_Late_Penalty_Salary_Value", CompId, strBrandId, strlocId, ref trns));

                if (Type == "2")
                {
                    sal = (PerMinSal * Value) / 100;

                }
                else
                {
                    sal = Value / 60;
                }
            }
            else
            {
                Late_Penalty_Min_Deduct = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Late_Penalty_Min_Deduct", CompId, strBrandId, strlocId, ref trns));
                if (PageType == "EmpDetail" || PageType == "DailySalary" || PageType == "LogProcess")
                {
                    sal = PerMinSal * Late_Penalty_Min_Deduct;
                }
                else
                {
                    sal = PerMinSal;
                }

            }


            //......
        }
        else
        {
            sal = 0;
        }
        try
        {
            sal = sal * LateMin;
        }
        catch (Exception Ex)
        {

        }
        return sal;

    }
    // Early Penalty 
    public double EarlyPenalty(string CompId, double PerMinSal, string EmpId, int PerDayAssignMin, int EarlyMin, string PageType, string strBrandId, string strlocId)
    {
        double sal = 0;

        string Type = string.Empty;
        string Method = string.Empty;
        double Value = 0;
        bool IsEmpEarly = false;
        int Early_Penalty_Min_Deduct = 0;
        try
        {
            IsEmpEarly = Convert.ToBoolean(objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Field2"));
        }
        catch
        {

        }


        Method = objAppParam.GetApplicationParameterValueByParamName("Is_Early_Penalty_Method", CompId, strBrandId, strlocId);
        if (IsEmpEarly)
        {
            if (Method == "Salary")
            {
                Type = objAppParam.GetApplicationParameterValueByParamName("Is_Early_Penalty_Salary_Type", CompId, strBrandId, strlocId);
                Value = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Is_Early_Penalty_Salary_Value", CompId, strBrandId, strlocId));

                if (Type == "2")
                {
                    sal = (PerMinSal * Value) / 100;
                }
                else
                {
                    sal = Value / 60;
                }
            }
            else
            {
                Early_Penalty_Min_Deduct = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Early_Penalty_Min_Deduct", CompId, strBrandId, strlocId));
                if (PageType == "EmpDetail" || PageType == "DailySalary" || PageType == "LogProcess")
                {
                    sal = PerMinSal * Early_Penalty_Min_Deduct;
                }
                else
                {
                    sal = PerMinSal;
                }
            }
        }
        try
        {
            sal = sal * EarlyMin;
        }
        catch (Exception Ex)
        {

        }
        return sal;

    }

    public double EarlyPenalty(string CompId, double PerMinSal, string EmpId, int PerDayAssignMin, int EarlyMin, string PageType, ref SqlTransaction trns, string strBrandId, string strlocId
)
    {
        double sal = 0;

        string Type = string.Empty;
        string Method = string.Empty;
        double Value = 0;
        bool IsEmpEarly = false;
        int Early_Penalty_Min_Deduct = 0;
        try
        {
            IsEmpEarly = Convert.ToBoolean(objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Field2", ref trns));
        }
        catch
        {

        }


        Method = objAppParam.GetApplicationParameterValueByParamName("Is_Early_Penalty_Method", CompId, strBrandId, strlocId, ref trns);
        if (IsEmpEarly)
        {
            if (Method == "Salary")
            {
                Type = objAppParam.GetApplicationParameterValueByParamName("Is_Early_Penalty_Salary_Type", CompId, strBrandId, strlocId, ref trns);
                Value = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Is_Early_Penalty_Salary_Value", CompId, strBrandId, strlocId, ref trns));

                if (Type == "2")
                {
                    sal = (PerMinSal * Value) / 100;
                }
                else
                {
                    sal = Value / 60;
                }
            }
            else
            {
                Early_Penalty_Min_Deduct = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Early_Penalty_Min_Deduct", CompId, strBrandId, strlocId, ref trns));
                if (PageType == "EmpDetail" || PageType == "DailySalary" || PageType == "LogProcess")
                {
                    sal = PerMinSal * Early_Penalty_Min_Deduct;
                }
                else
                {
                    sal = PerMinSal;
                }
            }
        }
        try
        {
            sal = sal * EarlyMin;
        }
        catch (Exception Ex)
        {

        }
        return sal;

    }
    // OT For Normal/Week Off/Holiday

    public double GetOTSalary(string CompId, string EmpId, double PerMinSal, int OTMin, string OTType)
    {
        double Sal = 0;
        string Normal_OT_Type = "";
        double Normal_OT_Value = 0;
        string Normal_HOT_Type = "";
        double Normal_HOT_Value = 0;
        string Normal_WOT_Type = "";
        double Normal_WOT_Value = 0;
        double BasicSalary = 0;
        double AssignMin = 0;
        //2 means per
        try
        {
            BasicSalary = double.Parse(objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Basic_Salary"));
            AssignMin = double.Parse(objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Assign_Min"));
        }
        catch
        {
        }
        if (OTType == "Normal")
        {
            try
            {
                Normal_OT_Type = objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Normal_OT_Type");
                Normal_OT_Value = double.Parse(objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Normal_OT_Value"));
            }
            catch
            {

            }
            if (Normal_OT_Type == "2")
            {
                Sal = (PerMinSal * Normal_OT_Value) / 100;
            }
            // Modify By Priya Jain(04.04.2014)
            else
            {
                Sal = (Normal_OT_Value) / 60;
            }
        }
        else if (OTType == "WeekOff")
        {
            try
            {
                Normal_WOT_Type = objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Normal_WOT_Type");
                Normal_WOT_Value = double.Parse(objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Normal_WOT_Value"));
            }
            catch
            {
            }
            if (Normal_WOT_Type == "2")
            {
                Sal = (PerMinSal * Normal_WOT_Value) / 100;

            }
            // Modify By Priya Jain(04.04.2014)
            else
            {
                Sal = (Normal_WOT_Value) / 60;
            }
        }
        else if (OTType == "Holiday")
        {
            try
            {
                Normal_HOT_Type = objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Normal_HOT_Type");
                Normal_HOT_Value = double.Parse(objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Normal_HOT_Value"));
            }
            catch
            {
            }
            if (Normal_HOT_Type == "2")
            {
                Sal = (PerMinSal * Normal_HOT_Value) / 100;

            }
            // Modify By Priya Jain(04.04.2014)
            else
            {
                Sal = (Normal_HOT_Value) / 60;
            }
        }
        try
        {
            Sal = Sal * OTMin;
        }
        catch (Exception Ex)
        {
        }

        return Sal;


    }

    public double GetOTSalary(string CompId, string EmpId, double PerMinSal, int OTMin, string OTType, ref SqlTransaction trns)
    {
        double Sal = 0;
        string Normal_OT_Type = "";
        double Normal_OT_Value = 0;
        string Normal_HOT_Type = "";
        double Normal_HOT_Value = 0;
        string Normal_WOT_Type = "";
        double Normal_WOT_Value = 0;
        double BasicSalary = 0;
        double AssignMin = 0;
        //2 means per
        try
        {
            BasicSalary = double.Parse(objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Basic_Salary", ref trns));
            AssignMin = double.Parse(objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Assign_Min", ref trns));
        }
        catch
        {
        }
        if (OTType == "Normal")
        {
            try
            {
                Normal_OT_Type = objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Normal_OT_Type", ref trns);
                Normal_OT_Value = double.Parse(objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Normal_OT_Value", ref trns));
            }
            catch
            {

            }
            if (Normal_OT_Type == "2")
            {
                Sal = (PerMinSal * Normal_OT_Value) / 100;
            }
            // Modify By Priya Jain(04.04.2014)
            else
            {
                Sal = (Normal_OT_Value) / 60;
            }
        }
        else if (OTType == "WeekOff")
        {
            try
            {
                Normal_WOT_Type = objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Normal_WOT_Type", ref trns);
                Normal_WOT_Value = double.Parse(objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Normal_WOT_Value", ref trns));
            }
            catch
            {
            }
            if (Normal_WOT_Type == "2")
            {
                Sal = (PerMinSal * Normal_WOT_Value) / 100;

            }
            // Modify By Priya Jain(04.04.2014)
            else
            {
                Sal = (Normal_WOT_Value) / 60;
            }
        }
        else if (OTType == "Holiday")
        {
            try
            {
                Normal_HOT_Type = objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Normal_HOT_Type", ref trns);
                Normal_HOT_Value = double.Parse(objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Normal_HOT_Value", ref trns));
            }
            catch
            {
            }
            if (Normal_HOT_Type == "2")
            {
                Sal = (PerMinSal * Normal_HOT_Value) / 100;

            }
            // Modify By Priya Jain(04.04.2014)
            else
            {
                Sal = (Normal_HOT_Value) / 60;
            }
        }
        try
        {
            Sal = Sal * OTMin;
        }
        catch (Exception Ex)
        {
        }

        return Sal;


    }
    // Partial Penalty 

    public static double GetPerMinuteSalary_For_OT_Penalty(double Total_Assign_Min, double Basic_Salary, double Total_Days, string TimeTableId, bool IsrelaxationMinute_In_Penalty, bool IsbreakMinute_In_Penalty, bool IsworkMinute_In_Penalty, string strConString, string strCompId)
    {
        Att_TimeTable objTimeTable = new Att_TimeTable(strConString);
        double PerMinuteSalary = 0;
        double workMinute = 0;
        double RelaxationMinute = 0;
        double BreakMinute = 0;
        double TotalMinute = 0;
        DataTable dt = objTimeTable.GetTimeTableMasterById(strCompId, TimeTableId);

        if (dt.Rows.Count > 0)
        {
            if (IsrelaxationMinute_In_Penalty)
            {
                RelaxationMinute += Convert.ToDouble(dt.Rows[0]["Field1"].ToString());
            }
            if (IsbreakMinute_In_Penalty)
            {
                BreakMinute += Convert.ToDouble(dt.Rows[0]["Break_Min"].ToString());
            }
            if (IsworkMinute_In_Penalty)
            {
                workMinute += Convert.ToDouble(dt.Rows[0]["Work_Minute"].ToString());
            }

            PerMinuteSalary = Basic_Salary / (Total_Days * (workMinute + BreakMinute + RelaxationMinute));

        }
        else
        {
            PerMinuteSalary = Basic_Salary / (Total_Days * Total_Assign_Min);
        }

        return PerMinuteSalary;
    }


    public double GetPerMinuteSalary_For_OT_Penalty1(double Total_Assign_Min, double Basic_Salary, double Total_Days, string TimeTableId, bool IsrelaxationMinute_In_Penalty, bool IsbreakMinute_In_Penalty, bool IsworkMinute_In_Penalty, ref SqlTransaction trns, string strCompId, DataTable dt)
    {
        Att_TimeTable objTimeTable = new Att_TimeTable(trns.Connection.ConnectionString);
        double PerMinuteSalary = 0;
        double workMinute = 0;
        double RelaxationMinute = 0;
        double BreakMinute = 0;
        double TotalMinute = 0;
        //DataTable dt = objTimeTable.GetTimeTableMasterById(strCompId, TimeTableId);

        if (dt.Rows.Count > 0)
        {
            if (IsrelaxationMinute_In_Penalty)
            {
                RelaxationMinute += Convert.ToDouble(dt.Rows[0]["Field1"].ToString());
            }
            if (IsbreakMinute_In_Penalty)
            {
                BreakMinute += Convert.ToDouble(dt.Rows[0]["Break_Min"].ToString());
            }
            if (IsworkMinute_In_Penalty)
            {
                workMinute += Convert.ToDouble(dt.Rows[0]["Work_Minute"].ToString());
            }

            PerMinuteSalary = Basic_Salary / (Total_Days * (workMinute + BreakMinute + RelaxationMinute));

        }
        else
        {
            PerMinuteSalary = Basic_Salary / (Total_Days * Total_Assign_Min);
        }

        return PerMinuteSalary;
    }


    public static double GetPerMinuteSalary_For_OT_Penalty(double Total_Assign_Min, double Basic_Salary, double Total_Days, string TimeTableId, bool IsrelaxationMinute_In_Penalty, bool IsbreakMinute_In_Penalty, bool IsworkMinute_In_Penalty, ref SqlTransaction trns, string strCompId)
    {
        Att_TimeTable objTimeTable = new Att_TimeTable(trns.Connection.ConnectionString);
        double PerMinuteSalary = 0;
        double workMinute = 0;
        double RelaxationMinute = 0;
        double BreakMinute = 0;
        double TotalMinute = 0;
        // DataTable dt = objTimeTable.GetTimeTableMasterById(strCompId, TimeTableId);
        DataTable dt = objTimeTable.GetTimeTableMasterById(strCompId, TimeTableId, ref trns);
        if (dt.Rows.Count > 0)
        {
            if (IsrelaxationMinute_In_Penalty)
            {
                RelaxationMinute += Convert.ToDouble(dt.Rows[0]["Field1"].ToString());
            }
            if (IsbreakMinute_In_Penalty)
            {
                BreakMinute += Convert.ToDouble(dt.Rows[0]["Break_Min"].ToString());
            }
            if (IsworkMinute_In_Penalty)
            {
                workMinute += Convert.ToDouble(dt.Rows[0]["Work_Minute"].ToString());
            }

            PerMinuteSalary = Basic_Salary / (Total_Days * (workMinute + BreakMinute + RelaxationMinute));

        }
        else
        {
            PerMinuteSalary = Basic_Salary / (Total_Days * Total_Assign_Min);
        }

        return PerMinuteSalary;
    }

    public double ParialPenalty(string CompId, double PerMinSal, string EmpId, int PerDayAssignMin, int PartialMin, string PageType, string strBrandId, string strlocId)
    {
        double sal = 0;
        string Method = string.Empty;
        string Type = string.Empty;
        double Value = 0;
        bool IsEmpPartial = false;
        int Partial_Penalty_Min_Deduct = 0;
        try
        {
            IsEmpPartial = Convert.ToBoolean(objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Is_Partial_Enable"));
        }
        catch
        {
        }

        Method = objAppParam.GetApplicationParameterValueByParamName("Partial_Penalty_Method", CompId, strBrandId, strlocId);
        if (IsEmpPartial)
        {
            if (Method == "Salary")
            {
                Type = objAppParam.GetApplicationParameterValueByParamName("Partial_Penalty_Salary_Type", CompId, strBrandId, strlocId);
                Value = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Partial_Penalty_Salary_Value", CompId, strBrandId, strlocId));

                if (Type == "2")
                {
                    sal = (PerMinSal * Value) / 100;
                }
                else
                {
                    sal = Value / 60;
                }
            }
            else
            {
                Partial_Penalty_Min_Deduct = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Partial_Penalty_Min_Deduct", CompId, strBrandId, strlocId));
                if (PageType == "EmpDetail" || PageType == "DailySalary" || PageType == "LogProcess")
                {
                    sal = PerMinSal * Partial_Penalty_Min_Deduct;
                }
                else
                {
                    sal = PerMinSal;
                }
            }
        }
        try
        {
            sal = sal * PartialMin;
        }
        catch (Exception Ex)
        {

        }
        return sal;

    }
    public double ParialPenalty(string CompId, double PerMinSal, string EmpId, int PerDayAssignMin, int PartialMin, string PageType, ref SqlTransaction trns, string strBrandId, string strlocId)
    {
        double sal = 0;
        string Method = string.Empty;
        string Type = string.Empty;
        double Value = 0;
        bool IsEmpPartial = false;
        int Partial_Penalty_Min_Deduct = 0;
        try
        {
            IsEmpPartial = Convert.ToBoolean(objEmpParam.GetEmployeeParameterByParameterName(EmpId, "Is_Partial_Enable", ref trns));
        }
        catch
        {
        }

        Method = objAppParam.GetApplicationParameterValueByParamName("Partial_Penalty_Method", CompId, strBrandId, strlocId, ref trns);
        if (IsEmpPartial)
        {
            if (Method == "Salary")
            {
                Type = objAppParam.GetApplicationParameterValueByParamName("Partial_Penalty_Salary_Type", CompId, strBrandId, strlocId, ref trns);
                Value = double.Parse(objAppParam.GetApplicationParameterValueByParamName("Partial_Penalty_Salary_Value", CompId, strBrandId, strlocId, ref trns));

                if (Type == "2")
                {
                    sal = (PerMinSal * Value) / 100;
                }
                else
                {
                    sal = Value / 60;
                }
            }
            else
            {
                Partial_Penalty_Min_Deduct = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Partial_Penalty_Min_Deduct", CompId, strBrandId, strlocId, ref trns));
                if (PageType == "EmpDetail" || PageType == "DailySalary" || PageType == "LogProcess")
                {
                    sal = PerMinSal * Partial_Penalty_Min_Deduct;
                }
                else
                {
                    sal = PerMinSal;
                }
            }
        }
        try
        {
            sal = sal * PartialMin;
        }
        catch (Exception Ex)
        {

        }
        return sal;

    }
    // Indemnity Salary 
    public double IndemnitySalary(string CompId, string EmpId, string MonthSal, string strBrandId, string strlocId)
    {
        bool IsIndemnity = false;
        int NextIndemnityDuration = 0;
        int IndemnityGivenType = 0;
        int IndemnitySalaryType = 0;
        int value = 0;
        int IndemnityLeave = 0;
        double Sal = 0;

        IsIndemnity = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsIndemnity", CompId, strBrandId, strlocId));

        if (IsIndemnity == true)
        {
            NextIndemnityDuration = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("NextIndemnityDuration", CompId, strBrandId, strlocId));
            IndemnityGivenType = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Indemnity_GivenType", CompId, strBrandId, strlocId));
            if (IndemnityGivenType == 1)
            {
                IndemnitySalaryType = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("Indemenity_SalaryCalculationType", CompId, strBrandId, strlocId));
                value = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("IndemnitySalaryValue", CompId, strBrandId, strlocId));
                if (IndemnitySalaryType == 1)
                {
                    Sal = (Convert.ToDouble(MonthSal) * value) / 100;

                }
                else
                {
                    Sal = value;
                }
            }
            else
            {
                IndemnityLeave = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("IndemnityLeave", CompId, strBrandId, strlocId));
            }
        }
        return Sal;
    }

    // Get Leave Status of Full Day, Half Day, Partial Leave
    public string GetLeaveApprovalStatus(string CompId, string EmpId, DateTime FromDate, DateTime ToDate, string LeaveType, int TotalLeaveDays, string strBrandId, string strlocId, string strTimeZoneId)
    {
        DataTable dtLeaveReq2 = new DataTable();

        string ApprovalDate = string.Empty;
        DataTable DtLeave = new DataTable();
        if (LeaveType == "FullDay")
        {

            //DtLeave = objleaveReq.GetLeaveRequestById(CompId, EmpId);
            while (FromDate <= ToDate)
            {
                dtLeaveReq2 = daClass.return_DataTable("select Is_Approved,Is_Pending from dbo.Att_Leave_Request where Emp_Id=" + EmpId + " and ('" + FromDate + "' between From_Date and To_Date ) order by trans_id desc");

                // DataTable dtLeaveReq2 = new DataView(DtLeave, "From_Date >='" +FromDate.ToString()+ "' and To_Date<='" + FromDate.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtLeaveReq2.Rows.Count > 0)
                {
                    if (dtLeaveReq2.Rows[0]["Is_Approved"].ToString() == "True")
                    {
                        ApprovalDate = "You have Already Apply Leave between From Date and to Date";
                        break;
                    }
                    if (dtLeaveReq2.Rows[0]["Is_Pending"].ToString() == "True")
                    {
                        ApprovalDate = "You have Already Apply Leave between From Date and to Date";
                        break;
                    }
                    //ApprovalDate = "You have Already Apply Leave between From Date and to Date";
                }
                FromDate = FromDate.AddDays(1);
            }

        }
        else if (LeaveType == "HalfDay")
        {
            DtLeave = objHalfDay.GetHalfDayRequestById(CompId, EmpId);
            DtLeave = new DataView(DtLeave, "Emp_Id='" + EmpId + "' and HalfDay_Date>='" + FromDate + "' and HalfDay_Date<='" + ToDate + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (DtLeave.Rows.Count > 0)
            {
                if (DtLeave.Rows[0]["Is_Confirmed"].ToString().Trim() != "Canceled")
                {
                    ApprovalDate = "Your Half Day Leave Already Approved On Date : " + DateFormat(FromDate.ToString()) + " So Cannot Apply";
                }
            }
        }
        else if (LeaveType == "PartialLeave")
        {
            DtLeave = objPartial.GetPartialLeaveRequest(CompId);
            DtLeave = new DataView(DtLeave, "Is_Confirmed='Approved' and Emp_Id='" + EmpId + "' and Partial_Leave_Date>='" + FromDate + "' and Partial_Leave_Date<='" + ToDate + "'", "", DataViewRowState.CurrentRows).ToTable();
            if (DtLeave.Rows.Count > 0)
            {
                if (DtLeave.Rows[0]["Is_Confirmed"].ToString().Trim() != "Canceled")
                {
                    ApprovalDate = "You have Already Apply Partial Leave On Date : " + DateFormat(FromDate.ToString()) + " So You May Not Apply Partial Leave";
                }
            }
        }
        else if (LeaveType == "Holiday")
        {
            if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Count_On_Holiday", CompId, strBrandId, strlocId)) == false)
            {
                // Check Holiday or Not For Leave Apply For the Day...............
                DataTable dtHoliday2 = objEmpHoliday.GetEmployeeHolidayMaster(CompId, strTimeZoneId);
                while (FromDate <= ToDate)
                {
                    DataTable dtHoliday1 = new DataView(dtHoliday2, "Holiday_Date='" + FromDate.ToString() + "' and Emp_Id='" + EmpId + "'", "", DataViewRowState.CurrentRows).ToTable();

                    if (dtHoliday1.Rows.Count > 0 && TotalLeaveDays == 1)
                    {
                        ApprovalDate = "Employee has Holiday on Date " + DateFormat(FromDate.ToString()) + " So Cannot Apply";
                    }
                    FromDate = FromDate.AddDays(1);
                }
            }
        }
        else if (LeaveType == "WeekOff")
        {
            if (Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Leave_Count_On_WeekOff", CompId, strBrandId, strlocId)) == false)
            {

                DataTable dtSch = objEmpSch.GetSheduleDescription(EmpId);

                while (FromDate <= ToDate)
                {
                    // Modified By Nitin jain, Date 09-07-2014 Check If Week Off Or Not For Leave Apply Date....
                    DataTable dtSch1 = new DataView(dtSch, "Att_Date='" + DateFormat(FromDate.ToString()) + "' and Emp_Id='" + EmpId + "' and Is_Off='True'", "", DataViewRowState.CurrentRows).ToTable();
                    if (dtSch1.Rows.Count > 0)
                    {
                        //ApprovalDate = "Employee has Week off on Date " + DateFormat(FromDate.ToString()) + " So Cannot Apply";
                        ApprovalDate = "You Can not apply half day on weekoff.";
                    }
                    FromDate = FromDate.AddDays(1);
                }
            }
        }
        else
        {
        }
        return ApprovalDate;
    }



    public static double GetMaxLeaveBalance(string strleaveTypeId, string strConString)
    {
        DataAccessClass Objda = new DataAccessClass(strConString);

        DataTable dt = Objda.return_DataTable("select isnull(Field4,0)  from Att_LeaveMaster where leave_id=" + strleaveTypeId + "");
        return Convert.ToDouble(dt.Rows[0][0].ToString());
    }
    // Get Company Name
    public string GetCompanyName(string CompId, string LangId, string Op_Type)
    {
        string CompanyName = "";
        DataTable DtCompany = objComp.GetCompanyMasterById(CompId);
        if (Op_Type == "1")
        {
            if (LangId == "2")
            {
                CompanyName = DtCompany.Rows[0]["Company_Name_L"].ToString();
            }
            else
            {
                CompanyName = DtCompany.Rows[0]["Company_Name"].ToString();
            }
        }
        else
        {
            CompanyName = "~/CompanyResource/" + CompId + "/" + DtCompany.Rows[0]["Logo_Path"].ToString();
        }
        return CompanyName;
    }

    // Get Brand Name
    public string GetBrandName(string CompId, string BrandId, string LangId)
    {
        string BrandName = "";
        try
        {
            DataTable DtBrand = objBrand.GetBrandMasterById(CompId, BrandId);
            if (LangId == "1")
            {
                BrandName = DtBrand.Rows[0]["Brand_Name"].ToString();
            }
            else
            {
                BrandName = DtBrand.Rows[0]["Brand_Name_L"].ToString();
            }
        }
        catch
        {
        }
        return BrandName;
    }
    // Get Location Name
    public string GetLocationName(string CompId, string BrandId, string LocId, string LangId)
    {
        string LocationName = "";
        DataTable DtLocation = objLoc.GetLocationMasterById(CompId, LocId);

        try
        {
            if (LangId == "1")
            {
                LocationName = DtLocation.Rows[0]["Location_Name"].ToString();
            }
            else
            {
                LocationName = DtLocation.Rows[0]["Location_Name_L"].ToString();
            }
        }
        catch
        {

        }
        return LocationName;
    }
    // Get Work Calculation Methode
    public string GetWorkCalculationMethod(string EmpId, string CompId, string BrandId, string LocId)
    {
        string WorkCalMethod = string.Empty;
        DataTable dt = objEmpParam.GetEmployeeParameterByEmpId(EmpId, CompId);
        if (dt.Rows.Count > 0)
        {
            WorkCalMethod = dt.Rows[0]["Effective_Work_Cal_Method"].ToString();
        }
        else
        {
            WorkCalMethod = objAppParam.GetApplicationParameterValueByParamName("Effective Work Calculation Method", CompId, BrandId, LocId);
        }
        return WorkCalMethod;
    }

    // Get Pending Leave Request For Employees 
    public DataTable GetLeavePending(string EmpList, string CompId, string BrandId, string LocId, string LeaveType, string OpType)
    {
        DataTable DtLeave = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_Id", CompId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Loc_Id", LocId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Emp_Ids", EmpList, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Op_Type", @OpType, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        DtLeave = daClass.Reuturn_Datatable_Search("sp_GetLeaveStatus", paramList);
        if (LeaveType == "FullDayPending")
        {
            DtLeave = new DataView(DtLeave, "Is_Pending='True'", "", DataViewRowState.CurrentRows).ToTable();
        }
        else if (LeaveType == "FullDayApproved")
        {
            DtLeave = new DataView(DtLeave, "Is_Pending='False'", "", DataViewRowState.CurrentRows).ToTable();
        }
        return DtLeave;
    }
    private string DateFormat(string Date)
    {
        string SystemDate = string.Empty;
        try
        {
            SystemDate = Convert.ToDateTime(Date).ToString(objSys.SetDateFormat());
        }
        catch
        {
            SystemDate = Date;
        }
        return SystemDate;
    }
    public int GetOverTimeMin(string EmpId, DateTime InTime, DateTime OutTime, DateTime OnDutyTime, DateTime OffDutyTime, int WorkMin, string CompId, string strBrandId, string strlocId)
    {
        int OtMin = 0;

        bool IsCompOT = false;
        bool IsEmpOT = false;
        int MaxOt = 0;
        int MinOt = 0;
        string OverTimeMethod = string.Empty;
        string IsOverTimeApproval = string.Empty;

        IsCompOT = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", CompId, strBrandId, strlocId));
        MaxOt = int.Parse(objAppParam.GetApplicationParameterValueByParamName("Max Over Time Min", CompId, strBrandId, strlocId));
        MinOt = int.Parse(objAppParam.GetApplicationParameterValueByParamName("Min OVer Time Min", CompId, strBrandId, strlocId));
        IsOverTimeApproval = objAppParam.GetApplicationParameterValueByParamName("OverTime Approval", CompId, strBrandId, strlocId);

        if (IsCompOT)
        {
            DataTable dt = objEmpParam.GetEmployeeParameterByEmpId(EmpId, CompId);
            int EmpAssignMin = 0;
            try
            {
                EmpAssignMin = Convert.ToInt32(dt.Rows[0]["Assign_Min"].ToString());
            }
            catch
            {
                EmpAssignMin = 0;
            }
            if (dt.Rows.Count > 0)
            {
                IsEmpOT = Convert.ToBoolean(dt.Rows[0]["Is_OverTime"].ToString());

                if (IsEmpOT)
                {
                    OverTimeMethod = dt.Rows[0]["Normal_OT_Method"].ToString();
                    if (OverTimeMethod == "In")
                    {
                        if (OnDutyTime != OffDutyTime)
                        {
                            if (InTime < OnDutyTime)
                            {
                                OtMin = GetTimeDifference(InTime, OnDutyTime);
                            }
                        }
                        else
                        {
                            EmpAssignMin = Convert.ToInt32(dt.Rows[0]["Assign_Min"].ToString());
                            if (WorkMin > EmpAssignMin)
                            {
                                OtMin = WorkMin - EmpAssignMin;
                            }
                            else
                            {
                                OtMin = 0;
                            }
                        }

                    }
                    else if (OverTimeMethod == "Out")
                    {
                        if (OnDutyTime != OffDutyTime)
                        {
                            if (OutTime > OffDutyTime)
                            {
                                OtMin = GetTimeDifference(OffDutyTime, OutTime);
                            }
                        }
                        else
                        {
                            EmpAssignMin = Convert.ToInt32(dt.Rows[0]["Assign_Min"].ToString());
                            if (WorkMin > EmpAssignMin)
                            {
                                OtMin = WorkMin - EmpAssignMin;
                            }
                            else
                            {
                                OtMin = 0;
                            }
                        }
                    }
                    else if (OverTimeMethod == "Both")
                    {
                        int AssignedMin = 0;
                        if (OnDutyTime != OffDutyTime)
                        {
                            if (InTime < OnDutyTime)
                            {
                                if (OutTime < OnDutyTime)
                                {
                                    OtMin = GetTimeDifference(InTime, OutTime);
                                }
                                else
                                {
                                    OtMin = GetTimeDifference(InTime, OnDutyTime);
                                }

                            }
                            if (OutTime > OffDutyTime)
                            {
                                OtMin += GetTimeDifference(OffDutyTime, OutTime);
                            }
                        }
                        else
                        {

                            AssignedMin = int.Parse(GetAssignWorkMin(EmpId, CompId, strBrandId, strlocId));
                            if (WorkMin > AssignedMin)
                            {
                                OtMin = WorkMin - AssignedMin;
                            }
                            else
                            {
                                OtMin = 0;
                            }
                        }
                    }
                    else if (OverTimeMethod == "Work Hour")
                    {
                        int assignMin = 0;
                        // Modified By Nitin Jain TO Get OT Minutes On Bases Of With Shift and Without Shift For Key Bases On 06/09/2014..
                        //Update On 26-03-2015
                        if (OnDutyTime.ToString() != "" && OffDutyTime.ToString() != "" && OnDutyTime.ToString() != Convert.ToDateTime("1/1/1900").ToString() && OffDutyTime.ToString() != Convert.ToDateTime("1/1/1900").ToString())
                        {
                            assignMin = GetTimeDifference(OnDutyTime, OffDutyTime);
                        }
                        else
                        {
                            assignMin = int.Parse(GetAssignWorkMin(EmpId, CompId, strBrandId, strlocId));
                        }

                        if (WorkMin > assignMin)
                        {
                            OtMin = WorkMin - assignMin;

                        }
                    }
                }
            }
        }
        if (OtMin < MinOt)
        {
            OtMin = 0;
        }
        if (OtMin > MaxOt)
        {
            OtMin = MaxOt;
        }

        //Add Code On 18-08-2015 for Approval Parameter
        if (OtMin != 0)
        {
            if (IsOverTimeApproval == "1")
            {
                OtMin = OverTimeApproval(CompId, EmpId, InTime, OtMin);
            }
        }
        return OtMin;
    }

    public int GetOverTimeMinNic(DateTime att_date, string EmpId, DateTime InTime, DateTime OutTime, DateTime OnDutyTime, DateTime OffDutyTime, int WorkMin, string CompId, string BrandId, string LocId, string strTimeTableOtMIn)
    {
        if (strTimeTableOtMIn == "")
        {
            strTimeTableOtMIn = "0";
        }
        int OtMin = 0;

        bool IsCompOT = false;
        bool IsEmpOT = false;
        int MaxOt = 0;
        int MinOt = 0;
        string OverTimeMethod = string.Empty;
        string IsOverTimeApproval = string.Empty;

        IsCompOT = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", CompId, BrandId, LocId));
        MaxOt = int.Parse(objAppParam.GetApplicationParameterValueByParamName("Max Over Time Min", CompId, BrandId, LocId));
        MinOt = int.Parse(objAppParam.GetApplicationParameterValueByParamName("Min OVer Time Min", CompId, BrandId, LocId));
        IsOverTimeApproval = objAppParam.GetApplicationParameterValueByParamName("OverTime Approval", CompId, BrandId, LocId);


        if (att_date.DayOfWeek == DayOfWeek.Friday)
        {
            if(strTimeTableOtMIn == "0")
            {
                int EarlyCheckIn = 0;
                if(OnDutyTime> InTime)  
                {
                    EarlyCheckIn =((OnDutyTime - InTime).Hours * 60) + (OnDutyTime - InTime).Minutes;
                }
                return (WorkMin - EarlyCheckIn );
            }
            else
            {

                int EarlyCheckIn = 0;
                if (OnDutyTime > InTime)
                {
                    EarlyCheckIn = ((OnDutyTime - InTime).Hours * 60) + (OnDutyTime - InTime).Minutes;
                }

                WorkMin = WorkMin - EarlyCheckIn;
                if (Convert.ToInt32(strTimeTableOtMIn)>= WorkMin)
                {
                   
                    return WorkMin;
                }
                else
                {
                    return Convert.ToInt32(strTimeTableOtMIn);
                }
            }
        }
        else
        {
            if (IsCompOT)
            {
                DataTable dt = objEmpParam.GetEmployeeParameterByEmpId(EmpId, CompId);
                int EmpAssignMin = 0;
                try
                {
                    EmpAssignMin = Convert.ToInt32(dt.Rows[0]["Assign_Min"].ToString());
                }
                catch
                {
                    EmpAssignMin = 0;
                }
                if (dt.Rows.Count > 0)
                {
                    IsEmpOT = Convert.ToBoolean(dt.Rows[0]["Is_OverTime"].ToString());

                    if (IsEmpOT)
                    {
                        OverTimeMethod = dt.Rows[0]["Normal_OT_Method"].ToString();
                        if (OverTimeMethod == "In")
                        {
                            if (OnDutyTime != OffDutyTime)
                            {
                                if (InTime < OnDutyTime)
                                {
                                    OtMin = GetTimeDifference(InTime, OnDutyTime);
                                }

                                if (OnDutyTime.ToString() != "" && OffDutyTime.ToString() != "" && OnDutyTime.ToString() != Convert.ToDateTime("1/1/1900").ToString() && OffDutyTime.ToString() != Convert.ToDateTime("1/1/1900").ToString())
                                {
                                    EmpAssignMin = GetTimeDifference(OnDutyTime, OffDutyTime);
                                }

                                if (WorkMin > EmpAssignMin)
                                {
                                    if (OtMin > (WorkMin - EmpAssignMin))
                                    {
                                        OtMin = WorkMin - EmpAssignMin;
                                    }
                                }
                                else
                                {
                                    OtMin = 0;
                                }


                            }
                            else
                            {
                                EmpAssignMin = Convert.ToInt32(dt.Rows[0]["Assign_Min"].ToString());
                                if (WorkMin > EmpAssignMin)
                                {
                                    OtMin = WorkMin - EmpAssignMin;
                                }
                                else
                                {
                                    OtMin = 0;
                                }
                            }

                        }
                        else if (OverTimeMethod == "Out")
                        {
                            if (OnDutyTime != OffDutyTime)
                            {
                                if (OutTime > OffDutyTime)
                                {
                                    OtMin = GetTimeDifference(OffDutyTime, OutTime);
                                }


                                //if (OnDutyTime.ToString() != "" && OffDutyTime.ToString() != "" && OnDutyTime.ToString() != Convert.ToDateTime("1/1/1900").ToString() && OffDutyTime.ToString() != Convert.ToDateTime("1/1/1900").ToString())
                                //{
                                //    EmpAssignMin = GetTimeDifference(OnDutyTime, OffDutyTime);
                                //}

                                //if (WorkMin > EmpAssignMin)
                                //{
                                //    if (OtMin > (WorkMin - EmpAssignMin))
                                //    {
                                //        OtMin = WorkMin - EmpAssignMin;
                                //    }
                                //}
                                //else
                                //{
                                //    OtMin = 0;
                                //}


                            }
                            else
                            {
                                EmpAssignMin = Convert.ToInt32(dt.Rows[0]["Assign_Min"].ToString());
                                if (WorkMin > EmpAssignMin)
                                {
                                    OtMin = WorkMin - EmpAssignMin;
                                }
                                else
                                {
                                    OtMin = 0;
                                }
                            }
                        }
                        else if (OverTimeMethod == "Both")
                        {
                            int AssignedMin = 0;
                            if (OnDutyTime != OffDutyTime)
                            {
                                if (InTime < OnDutyTime)
                                {
                                    if (OutTime < OnDutyTime)
                                    {
                                        OtMin = GetTimeDifference(InTime, OutTime);
                                    }
                                    else
                                    {
                                        OtMin = GetTimeDifference(InTime, OnDutyTime);
                                    }

                                }
                                if (OutTime > OffDutyTime)
                                {
                                    OtMin += GetTimeDifference(OffDutyTime, OutTime);
                                }


                                if (OnDutyTime.ToString() != "" && OffDutyTime.ToString() != "" && OnDutyTime.ToString() != Convert.ToDateTime("1/1/1900").ToString() && OffDutyTime.ToString() != Convert.ToDateTime("1/1/1900").ToString())
                                {
                                    EmpAssignMin = GetTimeDifference(OnDutyTime, OffDutyTime);
                                }

                                if (WorkMin > EmpAssignMin)
                                {
                                    if (OtMin > (WorkMin - EmpAssignMin))
                                    {
                                        OtMin = WorkMin - EmpAssignMin;
                                    }
                                }
                                else
                                {
                                    OtMin = 0;
                                }
                            }
                            else
                            {

                                AssignedMin = int.Parse(GetAssignWorkMin(EmpId, CompId, BrandId, LocId));
                                if (WorkMin > AssignedMin)
                                {
                                    OtMin = WorkMin - AssignedMin;
                                }
                                else
                                {
                                    OtMin = 0;
                                }
                            }
                        }
                        else if (OverTimeMethod == "Work Hour")
                        {
                            int assignMin = 0;
                            // Modified By Nitin Jain TO Get OT Minutes On Bases Of With Shift and Without Shift For Key Bases On 06/09/2014..
                            //Update On 26-03-2015
                            if (OnDutyTime.ToString() != "" && OffDutyTime.ToString() != "" && OnDutyTime.ToString() != Convert.ToDateTime("1/1/1900").ToString() && OffDutyTime.ToString() != Convert.ToDateTime("1/1/1900").ToString())
                            {
                                assignMin = GetTimeDifference(OnDutyTime, OffDutyTime);
                            }
                            else
                            {
                                assignMin = int.Parse(GetAssignWorkMin(EmpId, CompId, BrandId, LocId));
                            }

                            if (WorkMin > assignMin)
                            {
                                OtMin = WorkMin - assignMin;

                            }
                        }
                    }
                }
            }
            if (OtMin < MinOt)
            {
                OtMin = 0;
            }
            if (OtMin > MaxOt)
            {
                OtMin = MaxOt;
            }

            //Add Code On 18-08-2015 for Approval Parameter
            if (OtMin != 0)
            {
                //if (IsOverTimeApproval == "1")
                //{
                //    OtMin = OverTimeApproval(CompId, EmpId, InTime, OtMin);
                //}
            }

            int TimeTableOT = Convert.ToInt32(strTimeTableOtMIn);


            if (TimeTableOT > 0)
            {
                if (OtMin > TimeTableOT)
                {
                    OtMin = TimeTableOT;
                }
            }
        }

        if(OtMin < MinOt)
        {
            OtMin = 0;
        }


        return OtMin;
    }


    public int GetOverTimeMin(string EmpId, DateTime InTime, DateTime OutTime, DateTime OnDutyTime, DateTime OffDutyTime, int WorkMin, string CompId, string BrandId, string LocId, string strTimeTableOtMIn)
    {
        if (strTimeTableOtMIn == "")
        {
            strTimeTableOtMIn = "0";
        }
        int OtMin = 0;

        bool IsCompOT = false;
        bool IsEmpOT = false;
        int MaxOt = 0;
        int MinOt = 0;
        string OverTimeMethod = string.Empty;
        string IsOverTimeApproval = string.Empty;

        IsCompOT = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", CompId, BrandId, LocId));
        MaxOt = int.Parse(objAppParam.GetApplicationParameterValueByParamName("Max Over Time Min", CompId, BrandId, LocId));
        MinOt = int.Parse(objAppParam.GetApplicationParameterValueByParamName("Min OVer Time Min", CompId, BrandId, LocId));
        IsOverTimeApproval = objAppParam.GetApplicationParameterValueByParamName("OverTime Approval", CompId, BrandId, LocId);

        if (IsCompOT)
        {
            DataTable dt = objEmpParam.GetEmployeeParameterByEmpId(EmpId, CompId);
            int EmpAssignMin = 0;
            try
            {
                EmpAssignMin = Convert.ToInt32(dt.Rows[0]["Assign_Min"].ToString());
            }
            catch
            {
                EmpAssignMin = 0;
            }
            if (dt.Rows.Count > 0)
            {
                IsEmpOT = Convert.ToBoolean(dt.Rows[0]["Is_OverTime"].ToString());

                if (IsEmpOT)
                {
                    OverTimeMethod = dt.Rows[0]["Normal_OT_Method"].ToString();
                    if (OverTimeMethod == "In")
                    {
                        if (OnDutyTime != OffDutyTime)
                        {
                            if (InTime < OnDutyTime)
                            {
                                OtMin = GetTimeDifference(InTime, OnDutyTime);
                            }

                            if (OnDutyTime.ToString() != "" && OffDutyTime.ToString() != "" && OnDutyTime.ToString() != Convert.ToDateTime("1/1/1900").ToString() && OffDutyTime.ToString() != Convert.ToDateTime("1/1/1900").ToString())
                            {
                                EmpAssignMin = GetTimeDifference(OnDutyTime, OffDutyTime);
                            }

                            if (WorkMin > EmpAssignMin)
                            {
                                if (OtMin > (WorkMin - EmpAssignMin))
                                {
                                    OtMin = WorkMin - EmpAssignMin;
                                }
                            }
                            else
                            {
                                OtMin = 0;
                            }


                        }
                        else
                        {
                            EmpAssignMin = Convert.ToInt32(dt.Rows[0]["Assign_Min"].ToString());
                            if (WorkMin > EmpAssignMin)
                            {
                                OtMin = WorkMin - EmpAssignMin;
                            }
                            else
                            {
                                OtMin = 0;
                            }
                        }

                    }
                    else if (OverTimeMethod == "Out")
                    {
                        if (OnDutyTime != OffDutyTime)
                        {
                            if (OutTime > OffDutyTime)
                            {
                                OtMin = GetTimeDifference(OffDutyTime, OutTime);
                            }


                            //if (OnDutyTime.ToString() != "" && OffDutyTime.ToString() != "" && OnDutyTime.ToString() != Convert.ToDateTime("1/1/1900").ToString() && OffDutyTime.ToString() != Convert.ToDateTime("1/1/1900").ToString())
                            //{
                            //    EmpAssignMin = GetTimeDifference(OnDutyTime, OffDutyTime);
                            //}

                            //if (WorkMin > EmpAssignMin)
                            //{
                            //    if (OtMin > (WorkMin - EmpAssignMin))
                            //    {
                            //        OtMin = WorkMin - EmpAssignMin;
                            //    }
                            //}
                            //else
                            //{
                            //    OtMin = 0;
                            //}


                        }
                        else
                        {
                            EmpAssignMin = Convert.ToInt32(dt.Rows[0]["Assign_Min"].ToString());
                            if (WorkMin > EmpAssignMin)
                            {
                                OtMin = WorkMin - EmpAssignMin;
                            }
                            else
                            {
                                OtMin = 0;
                            }
                        }
                    }
                    else if (OverTimeMethod == "Both")
                    {
                        int AssignedMin = 0;
                        if (OnDutyTime != OffDutyTime)
                        {
                            if (InTime < OnDutyTime)
                            {
                                if (OutTime < OnDutyTime)
                                {
                                    OtMin = GetTimeDifference(InTime, OutTime);
                                }
                                else
                                {
                                    OtMin = GetTimeDifference(InTime, OnDutyTime);
                                }

                            }
                            if (OutTime > OffDutyTime)
                            {
                                OtMin += GetTimeDifference(OffDutyTime, OutTime);
                            }


                            if (OnDutyTime.ToString() != "" && OffDutyTime.ToString() != "" && OnDutyTime.ToString() != Convert.ToDateTime("1/1/1900").ToString() && OffDutyTime.ToString() != Convert.ToDateTime("1/1/1900").ToString())
                            {
                                EmpAssignMin = GetTimeDifference(OnDutyTime, OffDutyTime);
                            }

                            if (WorkMin > EmpAssignMin)
                            {
                                if (OtMin > (WorkMin - EmpAssignMin))
                                {
                                    OtMin = WorkMin - EmpAssignMin;
                                }
                            }
                            else
                            {
                                OtMin = 0;
                            }
                        }
                        else
                        {

                            AssignedMin = int.Parse(GetAssignWorkMin(EmpId, CompId, BrandId, LocId));
                            if (WorkMin > AssignedMin)
                            {
                                OtMin = WorkMin - AssignedMin;
                            }
                            else
                            {
                                OtMin = 0;
                            }
                        }
                    }
                    else if (OverTimeMethod == "Work Hour")
                    {
                        int assignMin = 0;
                        // Modified By Nitin Jain TO Get OT Minutes On Bases Of With Shift and Without Shift For Key Bases On 06/09/2014..
                        //Update On 26-03-2015
                        if (OnDutyTime.ToString() != "" && OffDutyTime.ToString() != "" && OnDutyTime.ToString() != Convert.ToDateTime("1/1/1900").ToString() && OffDutyTime.ToString() != Convert.ToDateTime("1/1/1900").ToString())
                        {
                            assignMin = GetTimeDifference(OnDutyTime, OffDutyTime);
                        }
                        else
                        {
                            assignMin = int.Parse(GetAssignWorkMin(EmpId, CompId, BrandId, LocId));
                        }

                        if (WorkMin > assignMin)
                        {
                            OtMin = WorkMin - assignMin;

                        }
                    }
                }
            }
        }
        if (OtMin < MinOt)
        {
            OtMin = 0;
        }
        if (OtMin > MaxOt)
        {
            OtMin = MaxOt;
        }

        //Add Code On 18-08-2015 for Approval Parameter
        if (OtMin != 0)
        {
            //if (IsOverTimeApproval == "1")
            //{
            //    OtMin = OverTimeApproval(CompId, EmpId, InTime, OtMin);
            //}
        }

        int TimeTableOT = Convert.ToInt32(strTimeTableOtMIn);


        if (TimeTableOT > 0)
        {
            if (OtMin > TimeTableOT)
            {
                OtMin = TimeTableOT;
            }
        }


        return OtMin;
    }

    public int OverTimeApproval(string CompId, string EmpId, DateTime dtOTDate, int OverTimeMin)
    {
        int OTMin = 0;

        DateTime OTDate = new DateTime(dtOTDate.Year, dtOTDate.Month, dtOTDate.Day, 00, 00, 00);
        DataTable dtOTRequest = objOTRequest.GetOvertimeRequestByEmpAndOverTimedate(CompId, EmpId, OTDate.ToString());
        dtOTRequest = new DataView(dtOTRequest, "Is_Approved='True'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtOTRequest.Rows.Count > 0)
        {
            string strDuration = dtOTRequest.Rows[0]["Time_Duration"].ToString();
            strDuration = (float.Parse(strDuration) * float.Parse("60")).ToString();

            if (OverTimeMin >= int.Parse(strDuration))
            {
                OTMin = int.Parse(strDuration);
            }
            else if (OverTimeMin <= int.Parse(strDuration))
            {
                OTMin = OverTimeMin;
            }
        }
        else
        {
            OTMin = 0;
        }
        return OTMin;
    }

    public int GetTimeDifference(DateTime inTime, DateTime outTime)
    {
        //inTime = DateTime.Parse(Convert.ToDateTime(inTime).ToString("MM/dd/yyyy HH:mm"));
        //outTime = DateTime.Parse(Convert.ToDateTime(outTime).ToString("MM/dd/yyyy HH:mm"));
        // On Duty time  = in Time
        // OutTime ==  Actual In Time
        int timeDifference = 0;
        //int Second = 0;
        if (outTime.Day != inTime.Day)
        {
            //Update On 26-03-2015
            TimeSpan duration = outTime - inTime;
            timeDifference = int.Parse(duration.Days.ToString()) * 24 * 60 + int.Parse(duration.Hours.ToString()) * 60 + int.Parse(duration.Minutes.ToString());
            //timeDifference = timeDifference + 1;
        }
        else if (outTime >= inTime)
        {
            //DateTime dTest = new DateTime(inTime.Year, inTime.Month, inTime.Day , inTime.Hour, inTime.Minute, 0);
            timeDifference = outTime.Subtract(inTime).Hours * 60 + outTime.Subtract(inTime).Minutes;
        }
        else
        {
            DateTime TempDateIn = new DateTime(inTime.Year, inTime.Month, inTime.Day, 23, 59, 0);
            DateTime TempDateOut = new DateTime(outTime.Year, outTime.Month, outTime.Day, 0, 0, 0);
            timeDifference = TempDateIn.Subtract(inTime).Hours * 60 + TempDateIn.Subtract(inTime).Minutes;
            timeDifference += outTime.Subtract(TempDateOut).Hours * 60 + outTime.Subtract(TempDateOut).Minutes + 1;
        }
        return timeDifference;
    }
    public string GetAssignWorkMin(string EmpId, string CompId, string strBrandId, string strlocId)
    {
        string AssignMin = string.Empty;


        DataTable dt = objEmpParam.GetEmployeeParameterByEmpId(EmpId, CompId);

        if (dt.Rows.Count > 0)
        {
            AssignMin = dt.Rows[0]["Assign_Min"].ToString();

        }
        else
        {
            AssignMin = objAppParam.GetApplicationParameterValueByParamName("Work Day Min", CompId, strBrandId, strlocId);
        }

        return AssignMin;
    }

    // Get Max Emp Code From DataBase
    public string GetNewEmpCode()
    {
        DataTable dtInfo = new DataTable();
        dtInfo = daClass.return_DataTable("select  MAX(CAST(Emp_Code AS int))+1 AS Emp_Code from Set_EmployeeMaster");
        return dtInfo.Rows[0]["Emp_Code"].ToString();
    }

    public string GetEmployeeCode(string EmpIds)
    {
        string EmpCodeList = string.Empty;
        DataTable dtInfo = new DataTable();
        dtInfo = daClass.return_DataTable("select distinct Emp_Code from set_EmployeeMaster where (Emp_Id  IN (SELECT CAST(Value AS INT) FROM F_Split('" + EmpIds + "', ',')))");
        if (dtInfo.Rows.Count > 0)
        {
            for (int i = 0; i < dtInfo.Rows.Count; i++)
            {
                EmpCodeList = dtInfo.Rows[i]["Emp_Code"].ToString() + ",";
            }
        }
        return EmpCodeList;
    }

    public string UpdateRejoinDate(string RejoinDate, string TransId)
    {
        string a = string.Empty;
        DataTable dtInfo = new DataTable();
        dtInfo = daClass.return_DataTable("Update Att_Leave_Request SET Field7 = '" + RejoinDate + "' where Trans_Id='" + TransId + "'");
        return a;
    }


    // Update  Pay_Employee_Claim  SET IsActive=1 WHERE (Emp_Id='14') AND (Claim_Name='Previous Leave' OR Claim_Name='Balanced Leave')

    public string DeleteClaim(string EmpId)
    {
        string a = string.Empty;
        DataTable dtInfo = new DataTable();
        dtInfo = daClass.return_DataTable("Update  Pay_Employee_Claim  SET IsActive=0 WHERE (Emp_Id='" + EmpId + "') AND (Claim_Name='Previous Leave' OR Claim_Name='Balanced Leave')");
        return a;
    }

    public string DeleteLeaveSalaryClaim(string EmpId, string Month, string Year)
    {
        string a = string.Empty;
        DataTable dtInfo = new DataTable();
        dtInfo = daClass.return_DataTable("Update  Pay_Employee_Claim  SET IsActive=0 WHERE Emp_Id='" + EmpId + "' AND Claim_Name='Leave Salary'");
        return a;
    }

    public string DeletePenalty(string EmpId)
    {
        string a = string.Empty;
        DataTable dtInfo = new DataTable();
        dtInfo = daClass.return_DataTable("Update  Pay_Employee_Penalty  SET IsActive=0 WHERE (Emp_Id='" + EmpId + "') AND (Penalty_Name='Balanced Leave Penalty')");
        return a;
    }

    public DataTable GetLeaveRequestByEmpId(string CompanyId, string EmpId)
    {
        DataTable dtInfo = new DataTable();
        dtInfo = daClass.return_DataTable("select Trans_Id,From_Date,To_Date, Field7 AS RejoiningDate,(SELECT top 1 REPLACE(CONVERT(CHAR(11), Att_AttendanceLog.Event_Date, 106),' ','-') FROM Att_AttendanceLog where Att_AttendanceLog.Event_Date > Att_Leave_Request.To_Date and Att_AttendanceLog.Emp_Id=Att_Leave_Request.Emp_Id) as Actual_Rejoin from Att_Leave_Request Where ISNULL(IsActive,0)=1 AND Is_Approved='True' AND Company_Id=" + CompanyId + " AND Emp_Id=" + EmpId + " and leave_type_id in (select Leave_type_id from Att_LeaveSalary where emp_id=" + EmpId + " and F5='Approved')");
        return dtInfo;
    }
    public string GePayrollPostedByEmpId(string CompanyId, string EmpIds, string Month, string Year)
    {
        DataTable dtInfo = new DataTable();
        string EmpCode = string.Empty;
        dtInfo = daClass.return_DataTable("Select Emp_Id From Pay_Employe_Month WHERE (Emp_Id  IN (SELECT CAST(Value AS INT) FROM F_Split('" + EmpIds.ToString() + "', ','))) AND Company_Id='" + CompanyId.ToString() + "' AND Month='" + Month.ToString() + "' AND Year='" + Year.ToString() + "' AND ISNULL(IsActive,0)=1");
        if (dtInfo.Rows.Count > 0)
        {
            for (int i = 0; i < dtInfo.Rows.Count; i++)
            {
                EmpCode = dtInfo.Rows[i]["Emp_Id"].ToString() + ",";
            }
            EmpCode = GetEmployeeCode(EmpCode);

        }
        return EmpCode;
    }

    public DataTable GetFullDayLeaveDashboardEmpId(string EmpIds)
    {
        DataTable dtInfo = new DataTable();
        dtInfo = daClass.return_DataTable("select Att_LeaveMaster.Leave_Name,LeaveChild.Leave_Date,Att_Leave_Request.* from Att_Leave_Request INNER JOIN  Att_Leave_Request_Child AS LeaveChild ON Att_Leave_Request.Trans_Id = LeaveChild.Ref_Id AND Month(LeaveChild.Leave_Date)=MONTH(GETDATE()) AND YEAR(LeaveChild.Leave_Date)=YEAR(GETDATE()) INNER JOIN  Att_LeaveMaster ON Att_LeaveMaster.Leave_Id = Att_Leave_Request.Leave_Type_Id  WHERE (Att_Leave_Request.Emp_Id  IN (SELECT CAST(Value AS INT) FROM F_Split('" + EmpIds + "', ','))) ");
        return dtInfo;
    }


    public bool IsLeaveMaturity(string strEmpid, string strleaveType, string strCompId, string strTimeZoneId)
    {
        bool Result = true;

        int MaturityDays = 0;
        double serviceDays = 0;

        DataTable Dt = GetEmployeeDOJ(strCompId, strEmpid);


        try
        {
            MaturityDays = Convert.ToInt32(objleave.GetLeaveMasterById(strCompId, strleaveType).Rows[0]["Field2"].ToString());
        }
        catch
        {

        }


        if (MaturityDays > 0)
        {

            serviceDays = Math.Round((Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId) - Convert.ToDateTime(Dt.Rows[0]["DOJ"].ToString())).TotalDays);


            if (serviceDays < MaturityDays)
            {
                Result = false;
            }
        }

        return Result;
    }

    public bool IsLeaveMaturity(string strEmpid, string strleaveType, ref SqlTransaction trns, string strCompId, string strTimeZoneId)
    {
        bool Result = true;

        int MaturityDays = 0;
        double serviceDays = 0;

        DataTable Dt = GetEmployeeDOJ(strCompId, strEmpid, ref trns);


        try
        {
            MaturityDays = Convert.ToInt32(objleave.GetLeaveMasterById(strCompId, strleaveType, ref trns).Rows[0]["Field2"].ToString());
        }
        catch
        {

        }


        if (MaturityDays > 0)
        {

            serviceDays = Math.Round((Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId) - Convert.ToDateTime(Dt.Rows[0]["DOJ"].ToString())).TotalDays);


            if (serviceDays < MaturityDays)
            {
                Result = false;
            }
        }

        return Result;
    }


    public string getEmployeePerMinSalary(string empId, string strCurrencyId)
    {
        string salary = "0";
        salary = daClass.get_SingleValue("select Basic_Salary/datediff(day, dateadd(day, 1-day(GETDATE()), GETDATE()),dateadd(month, 1, dateadd(day, 1-day(GETDATE()), GETDATE())))/Assign_Min as perMinSalary from dbo.Set_Employee_Parameter where Emp_Id='" + empId + "'");
        salary = salary == "@NOTFOUND@" ? "0" : salary;
        salary = Common.GetAmountDecimal(salary, _strConString, strCurrencyId);
        return salary;
    }
}
