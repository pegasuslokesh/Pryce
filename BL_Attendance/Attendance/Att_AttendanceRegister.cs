using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;

using System.Xml.Linq;
using PegasusDataAccess;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Att_AttendanceRegister
/// </summary>
public class Att_AttendanceRegister
{
    DataAccessClass daClass = null;
    Attendance objAttendance = null;
    EmployeeParameter objEmpParam = null;
    EmployeeMaster objEmp = null;
   
    private string _strConString = string.Empty;
    public Att_AttendanceRegister(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
        objAttendance = new Attendance(strConString);
        objEmpParam = new EmployeeParameter(strConString);
        objEmp = new EmployeeMaster(strConString);
        _strConString = strConString;
    }

    string LateIn_MinuteDeductionType = string.Empty;
    string EarlyOut_MinuteDeductionType = string.Empty;
    int Late_Min = 0;
    int Early_Min = 0;
    string strValue = string.Empty;
    public int InsertAttendanceRegister(string CompanyId, string Emp_Id, string Att_Date, string Shift_Id, string Is_TempShift, string TimeTable_Id, string OnDuty_Time, string OffDuty_Time, string In_Time, string Out_Time, string IsLate, string LateMin, string Late_Relaxation_Min, string Late_Penalty_Min, string IsEarlyOut, string EarlyMin, string Early_Relaxation_Min, string Early_Penalty_Min, string Is_Week_Off, string Is_Holiday, string Is_Leave, string Is_Absent, string Week_Off_Min, string Holiday_Min, string OverTime_Min, string Partial_Min, string Partial_Violation_Min, string EffectiveWork_Min, string TotalAssign_Min, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate, string EmployeeLevel, string strCheckIn_by_deviceId, string strCheckOut_by_deviceId, string strBreak_in, string strBreak_out, string strLeave_Type_Id, string strHoliday_Id, DataTable dtCompanyParameter, string strbrandId, string strLocationId, DateTime OnTime, DateTime OffTime, string HalfDayCount, string strLogDetail)
    {
        LogProcess  ObjLogProcess = new LogProcess(_strConString);

        //here we are added code for count late in and early out when employee present but check in and check out not exists due to clock in and clock out parameter 


        if ((!Convert.ToBoolean(Is_Week_Off) && !Convert.ToBoolean(Is_Holiday) && !Convert.ToBoolean(Is_Absent) && !Convert.ToBoolean(Is_Leave) && TimeTable_Id.Trim() != "0") && (In_Time != "1/1/1900" || Out_Time != "1/1/1900"))
        {
            DataTable dtTimetable = daClass.return_DataTable("select Late_min,early_min from att_timetable where timetable_id=" + TimeTable_Id.Trim() + "");
            //if in log exists then we will calculate late in penalty
            if (In_Time != "1/1/1900" && In_Time != "1/1/1900 12:00:00 AM")
            {
                if (Convert.ToDateTime(In_Time) > Convert.ToDateTime(OnTime) && !Convert.ToBoolean(IsLate))
                {
                    //condition add on date 18 Jan 2014 kunal
                    if (Convert.ToDateTime(In_Time).Hour == Convert.ToDateTime(OnTime).Hour && Convert.ToDateTime(In_Time).Minute == Convert.ToDateTime(OnTime).Minute)
                    {
                        //Do Nothing 
                    }
                    else
                    {
                        Late_Min = objAttendance.GetTimeDifference(Convert.ToDateTime(OnTime), Convert.ToDateTime(In_Time));
                        if (Late_Min > 0)
                        {
                            IsLate = "True";
                            strValue = ObjLogProcess.GetLateRelaxMinPenaltyMin(Emp_Id, Convert.ToDateTime(Att_Date), Late_Min, CompanyId, strbrandId, strLocationId, dtCompanyParameter, Convert.ToInt32(dtTimetable.Rows[0]["Late_min"].ToString()));
                            Late_Relaxation_Min = strValue.Split('-')[0];
                            Late_Penalty_Min = strValue.Split('-')[1];
                            LateMin = Late_Penalty_Min;
                        }
                        else
                        {
                            IsLate = "False";
                            Late_Relaxation_Min = "0";
                            Late_Penalty_Min = "0";
                            LateMin = "0";
                        }
                    }
                }

            }
            //if out log exists then we will calculate early out penalty
            if (Out_Time != "1/1/1900" && Out_Time != "1/1/1900 12:00:00 AM")
            {
                if (Convert.ToDateTime(Out_Time) < Convert.ToDateTime(OffTime) && !Convert.ToBoolean(IsEarlyOut))
                {
                    //condition add on date 18 Jan 2014 kunal
                    if (Convert.ToDateTime(Out_Time).Hour == Convert.ToDateTime(OffTime).Hour && Convert.ToDateTime(Out_Time).Minute == Convert.ToDateTime(OffTime).Minute)
                    {
                        //Do Nothing 
                    }
                    else
                    {
                        //Modified On 20-08-2015

                        Early_Min = objAttendance.GetTimeDifference(Convert.ToDateTime(Out_Time), Convert.ToDateTime(OffTime));

                        //EarlyMin = objAttendance.GetTimeDifference(Convert.ToDateTime(OutTimeF), OffTime);
                        if (Early_Min > 0)
                        {
                            IsEarlyOut = "True";
                            strValue = ObjLogProcess.GetEarlyRelaxMinPenaltyMin(Emp_Id, Convert.ToDateTime(Att_Date), Early_Min, CompanyId, strbrandId, strLocationId, dtCompanyParameter, Convert.ToInt32(dtTimetable.Rows[0]["early_min"].ToString()));
                            Early_Relaxation_Min = strValue.Split('-')[0];
                            Early_Penalty_Min = strValue.Split('-')[1];
                            EarlyMin = Early_Penalty_Min;
                        }
                        else
                        {
                            IsEarlyOut = "False";
                            Early_Relaxation_Min = "0";
                            Early_Penalty_Min = "0";
                            EarlyMin = "0";

                        }
                    }

                }

            }
        }



        if (In_Time == "1/1/1900" || In_Time == "1/1/1900 12:00:00 AM")
        {
            strCheckIn_by_deviceId = "0";
        }

        if (Out_Time == "1/1/1900" || Out_Time == "1/1/1900 12:00:00 AM")
        {
            strCheckOut_by_deviceId = "0";
        }

        if (EmployeeLevel == "CEO")
        {
            EffectiveWork_Min = TotalAssign_Min;
            LateMin = "0";
            EarlyMin = "0";
            Partial_Min = "0";

            if (Convert.ToBoolean(Is_Week_Off) || Convert.ToBoolean(Is_Holiday) || Convert.ToBoolean(Is_Leave))
            {

            }
            else
            {
                Is_Absent = false.ToString();
            }

        }
        else if (EmployeeLevel == "Manager")
        {
            if (Convert.ToBoolean(Is_Week_Off) || Convert.ToBoolean(Is_Holiday) || Convert.ToBoolean(Is_Leave))
            {

            }
            else if (Convert.ToDateTime(In_Time).ToString("dd/MM/yyyy") != "01/01/1900" || Convert.ToDateTime(Out_Time).ToString("dd/MM/yyyy") != "01/01/1900")
            {
                EffectiveWork_Min = TotalAssign_Min;
                LateMin = "0";
                EarlyMin = "0";
                Partial_Min = "0";
                Is_Absent = false.ToString();
            }
        }

        PassDataToSql[] paramList = new PassDataToSql[50];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Att_Date", Att_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Shift_Id", Shift_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Is_TempShift", Is_TempShift, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@TimeTable_Id", TimeTable_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@OnDuty_Time", OnDuty_Time, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@OffDuty_Time", OffDuty_Time, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@In_Time", In_Time, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Out_Time", Out_Time, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@IsLate", IsLate, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@LateMin", LateMin, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Late_Relaxation_Min", Late_Relaxation_Min, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Late_Penalty_Min", Late_Penalty_Min, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@IsEarlyOut", IsEarlyOut, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@EarlyMin", EarlyMin, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Early_Relaxation_Min", Early_Relaxation_Min, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Early_Penalty_Min", Early_Penalty_Min, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Is_Week_Off", Is_Week_Off, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@Is_Holiday", Is_Holiday, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@Is_Leave", Is_Leave, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@Is_Absent", Is_Absent, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@Week_Off_Min", Week_Off_Min, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@Holiday_Min", Holiday_Min, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@OverTime_Min", OverTime_Min, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@Partial_Min", Partial_Min, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@Partial_Violation_Min", Partial_Violation_Min, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@EffectiveWork_Min", EffectiveWork_Min, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@TotalAssign_Min", TotalAssign_Min, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        //try
        //{
        if (Is_Holiday == true.ToString() || Is_Week_Off == true.ToString())
        {
            paramList[29] = new PassDataToSql("@Field1", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        }
        else
        {
            paramList[29] = new PassDataToSql("@Field1", Late_Relaxation_Min, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        }
        //HttpContext.Current.Session["CurLate"] = null;
        //}
        //catch
        //{
        //    paramList[29] = new PassDataToSql("@Field1", Late_Relaxation_Min, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        //}

        //try
        //{
        //    paramList[30] = new PassDataToSql("@Field2", HttpContext.Current.Session["EarlyMin"].ToString(), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        //    HttpContext.Current.Session["EarlyMin"] = null;

        //}
        //catch (Exception Ex)
        //{
            paramList[30] = new PassDataToSql("@Field2", Early_Relaxation_Min, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        //}

        paramList[31] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[32] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[33] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[34] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[35] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


        paramList[36] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[37] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[38] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[39] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[40] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[41] = new PassDataToSql("@CheckIn_by_deviceId", strCheckIn_by_deviceId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[42] = new PassDataToSql("@CheckOut_by_deviceId", strCheckOut_by_deviceId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[43] = new PassDataToSql("@Break_in", strBreak_in, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[44] = new PassDataToSql("@Break_out", strBreak_out, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[45] = new PassDataToSql("@Leave_Type_Id", strLeave_Type_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[46] = new PassDataToSql("@Holiday_Id", strHoliday_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[47] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[48] = new PassDataToSql("@HalfDay_Count", HalfDayCount, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[49] = new PassDataToSql("@Log_Detail", strLogDetail, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Att_AttendanceRegister_Insert", paramList);
        return Convert.ToInt32(paramList[47].ParaValue);
    }
    public string GetLateRelaxMinPenaltyMin(string empid, DateTime date, int LateMin, string strCompanyId, string strBrandId, string strLocationId, DataTable dtCompanyParameter)
    {

        LateIn_MinuteDeductionType = GetApplicationParameterValueByParamName("LateIn_MinuteDeduction_Type", dtCompanyParameter);


        string LateRelaxMinPenaltyMin = "0";

        bool IsLateFun = false;

        int RelaxMin = 0;

        int PenaltyMin = 0;
        int RelaxMinPrev = 0;
        int LateCount = 0;
        string PenaltyMethod = string.Empty;
        DataTable dtAttReg = new DataTable();
        bool IsEmpLate = false;
        try
        {
            IsEmpLate = Convert.ToBoolean(objEmpParam.GetEmployeeParameterByParameterName(empid, "Field1"));
        }
        catch
        {

        }

        dtAttReg = GetAttendanceRegDataByMonth_Year_EmpId(empid, date.Month.ToString(), date.Year.ToString());
        dtAttReg = new DataView(dtAttReg, "Late_Relaxation_Min<>'0'", "Att_Date", DataViewRowState.CurrentRows).ToTable();

        try
        {
            IsLateFun = Convert.ToBoolean(GetApplicationParameterValueByParamName("Is_Late_Penalty", dtCompanyParameter));

            RelaxMin = int.Parse(GetApplicationParameterValueByParamName("Late_Relaxation_Min", dtCompanyParameter));
        }
        catch
        {
        }

        if (IsLateFun)
        {

            PenaltyMethod = GetApplicationParameterValueByParamName("Is_Late_Penalty_Method", dtCompanyParameter);

            if (PenaltyMethod == "Salary")
            {
                if (LateIn_MinuteDeductionType == "2")
                {
                    LateCount = int.Parse(GetApplicationParameterValueByParamName("Late_Occurence", dtCompanyParameter));


                    if (LateMin > 0)
                    {

                        if (dtAttReg.Rows.Count > 0)
                        {
                            if (dtAttReg.Rows.Count >= LateCount)
                            {
                                RelaxMin = 0;
                                PenaltyMin = LateMin;
                            }
                            else
                            {


                                RelaxMinPrev = 0;
                                for (int i = 0; i < dtAttReg.Rows.Count; i++)
                                {
                                    RelaxMinPrev += int.Parse(dtAttReg.Rows[i]["Late_Relaxation_Min"].ToString());
                                }

                                //RelaxMinPrev = int.Parse(dtAttReg.Rows[dtAttReg.Rows.Count - 1]["Late_Relaxation_Min"].ToString());

                                if (RelaxMinPrev < RelaxMin)
                                {

                                    if (LateMin > (RelaxMin - RelaxMinPrev))
                                    {
                                        PenaltyMin = (LateMin - (RelaxMin - RelaxMinPrev));
                                        RelaxMin = LateMin - PenaltyMin;
                                    }
                                    else
                                    {
                                        PenaltyMin = 0;
                                        RelaxMin = LateMin;
                                    }
                                }
                                else
                                {

                                    int LastLate = 0;
                                    for (int i = 0; i < dtAttReg.Rows.Count; i++)
                                    {
                                        LastLate += int.Parse(dtAttReg.Rows[i]["Late_Relaxation_Min"].ToString());
                                    }

                                    if (LastLate < RelaxMin && LastLate != 0)
                                    {
                                        PenaltyMin = RelaxMin - LastLate;
                                        RelaxMin = PenaltyMin;
                                        PenaltyMin = LateMin - PenaltyMin;
                                    }
                                    else
                                    {
                                        RelaxMin = 0;
                                        PenaltyMin = LateMin;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (LateMin > RelaxMin)
                            {
                                PenaltyMin = LateMin - RelaxMin;
                                //RelaxMin = 0;       here Code modified on date 18 Jan 2014 Kunal
                            }
                            else
                            {

                                RelaxMin = LateMin;
                                //10-04-2014
                                //Start
                                // Session["CurLate"] = LateMin.ToString();
                                //End

                            }
                        }




                    }
                    else
                    {

                        int TotLateMin = 0;
                        for (int i = 0; i < dtAttReg.Rows.Count; i++)
                        {
                            TotLateMin += int.Parse(dtAttReg.Rows[i]["Late_Relaxation_Min"].ToString());

                        }

                        if (TotLateMin >= RelaxMin)
                        {
                            //  PenaltyMin = PenaltyDedMin * LateMin;
                            PenaltyMin = LateMin;

                            RelaxMin = 0;
                        }

                    }
                }
                else
                {
                    LateCount = int.Parse(GetApplicationParameterValueByParamName("Late_Occurence", dtCompanyParameter));
                    if (RelaxMin != 0)
                    {
                        RelaxMin = RelaxMin / LateCount;
                    }

                    if (LateMin > 0)
                    {
                        if (LateCount > 0)
                        {
                            if (dtAttReg.Rows.Count >= LateCount)
                            {
                                RelaxMin = 0;
                                PenaltyMin = LateMin;
                            }
                            else
                            {

                                if (LateMin > RelaxMin)
                                {
                                    PenaltyMin = LateMin - RelaxMin;
                                }
                                else
                                {
                                    PenaltyMin = 0;
                                    RelaxMin = LateMin;
                                }
                            }
                        }
                        else if (dtAttReg.Rows.Count > 0)
                        {
                            if (dtAttReg.Rows.Count >= LateCount)
                            {
                                RelaxMin = 0;
                                PenaltyMin = LateMin;
                            }
                            else
                            {
                                RelaxMinPrev = 0;
                                for (int i = 0; i < dtAttReg.Rows.Count; i++)
                                {
                                    RelaxMinPrev += int.Parse(dtAttReg.Rows[i]["Late_Relaxation_Min"].ToString());

                                }


                                //RelaxMinPrev = int.Parse(dtAttReg.Rows[dtAttReg.Rows.Count - 1]["Late_Relaxation_Min"].ToString());

                                if (RelaxMinPrev == RelaxMin && RelaxMinPrev != 0)
                                {
                                    if (LateMin > RelaxMin)
                                    {
                                        PenaltyMin = LateMin - RelaxMin;
                                        if (RelaxMin.ToString() == "0")
                                        {
                                            RelaxMin = 0;
                                        }
                                        else
                                        {
                                            RelaxMin = RelaxMin;
                                        }
                                    }
                                    else
                                    {
                                        // RelaxMin = EarlyMin + RelaxMinPrev;
                                        RelaxMin = LateMin;
                                        //Session["EarlyMin"] = LateMin;
                                    }
                                }

                                else
                                {

                                    //int LastLate = int.Parse(dtAttReg.Rows[dtAttReg.Rows.Count - 1]["Late_Relaxation_Min"].ToString());


                                    int LastLate = 0;
                                    for (int i = 0; i < dtAttReg.Rows.Count; i++)
                                    {
                                        LastLate += int.Parse(dtAttReg.Rows[i]["Late_Relaxation_Min"].ToString());

                                    }

                                    if (LastLate < RelaxMin && LastLate != 0)
                                    {
                                        if (LateMin < RelaxMin)
                                        {
                                            PenaltyMin = 0;
                                            RelaxMin = LateMin;
                                        }
                                        else
                                        {
                                        }

                                        //10-04-2014
                                        //Start
                                        //Session["CurLate"] = PenaltyMin.ToString();
                                        //End
                                        PenaltyMin = LateMin - PenaltyMin;

                                    }
                                    else
                                    {
                                        RelaxMin = 0;
                                        PenaltyMin = LateMin;


                                    }


                                }
                            }
                        }
                        else
                        {
                            if (LateMin > RelaxMin)
                            {
                                PenaltyMin = LateMin - RelaxMin;
                                RelaxMin = RelaxMin;
                            }
                            else
                            {

                                RelaxMin = LateMin;
                                //10-04-2014
                                //Start
                                //HttpContext.Current.Session["CurLate"] = LateMin.ToString();
                                //End

                            }
                        }




                    }
                    else
                    {

                        int TotLateMin = 0;
                        for (int i = 0; i < dtAttReg.Rows.Count; i++)
                        {
                            TotLateMin += int.Parse(dtAttReg.Rows[i]["Late_Relaxation_Min"].ToString());

                        }

                        if (TotLateMin >= RelaxMin)
                        {
                            //  PenaltyMin = PenaltyDedMin * LateMin;
                            PenaltyMin = LateMin;

                            RelaxMin = 0;
                        }

                    }
                }

            }


            /////----------------------------------
            else
            {
                int PenaltyDedMin = 0;

                if (LateMin > 0)
                {
                    PenaltyDedMin = int.Parse(GetApplicationParameterValueByParamName("Late_Penalty_Min_Deduct", dtCompanyParameter));


                    int TotLateMin = 0;
                    for (int i = 0; i < dtAttReg.Rows.Count; i++)
                    {
                        TotLateMin += int.Parse(dtAttReg.Rows[i]["Late_Relaxation_Min"].ToString());

                    }

                    if (TotLateMin >= RelaxMin)
                    {
                        //  PenaltyMin = PenaltyDedMin * LateMin;
                        PenaltyMin = LateMin;

                        RelaxMin = 0;
                    }
                    else
                    {
                        if ((TotLateMin + LateMin) > RelaxMin)
                        {
                            RelaxMin = RelaxMin - TotLateMin;
                            PenaltyMin = LateMin - RelaxMin;

                        }
                        else
                        {
                            RelaxMin = LateMin;
                        }
                    }
                }
                else
                {

                    int TotLateMin = 0;
                    for (int i = 0; i < dtAttReg.Rows.Count; i++)
                    {
                        TotLateMin += int.Parse(dtAttReg.Rows[i]["Late_Relaxation_Min"].ToString());

                    }

                    if (TotLateMin >= RelaxMin)
                    {
                        //  PenaltyMin = PenaltyDedMin * LateMin;
                        PenaltyMin = LateMin;

                        RelaxMin = 0;
                    }

                }
            }
        }
        LateRelaxMinPenaltyMin = RelaxMin.ToString() + "-" + PenaltyMin.ToString();
        return LateRelaxMinPenaltyMin;


    }
    public string GetEarlyRelaxMinPenaltyMin(string empid, DateTime date, int EarlyMin, string strCompanyId, string strBrandId, string strLocationId, DataTable dtCompanyParameter)
    {
        EarlyOut_MinuteDeductionType = GetApplicationParameterValueByParamName("EarlyOut_MinuteDeduction_Type", dtCompanyParameter);
        string EarlyRelaxMinPenaltyMin = "0";

        bool IsEarlyFun = false;

        int RelaxMin = 0;

        int PenaltyMin = 0;
        int RelaxMinPrev = 0;
        int EarlyCount = 0;
        string PenaltyMethod = string.Empty;
        DataTable dtAttReg = new DataTable();
        dtAttReg = GetAttendanceRegDataByMonth_Year_EmpId(empid, date.Month.ToString(), date.Year.ToString());
        dtAttReg = new DataView(dtAttReg, "Early_Relaxation_Min<>'0'", "Att_Date", DataViewRowState.CurrentRows).ToTable();

        bool IsEmpEarly = false;
        try
        {
            IsEmpEarly = Convert.ToBoolean(objEmpParam.GetEmployeeParameterByParameterName(empid, "Field2"));
        }
        catch
        {

        }
        try
        {
            IsEarlyFun = Convert.ToBoolean(GetApplicationParameterValueByParamName("Is_Early_Penalty", dtCompanyParameter));
            RelaxMin = int.Parse(GetApplicationParameterValueByParamName("Early_Relaxation_Min", dtCompanyParameter));
        }
        catch
        {

        }
        if (IsEarlyFun)
        {

            PenaltyMethod = GetApplicationParameterValueByParamName("Is_Early_Penalty_Method", dtCompanyParameter);

            if (PenaltyMethod == "Salary")
            {

                if (EarlyOut_MinuteDeductionType == "2")
                {
                    EarlyCount = int.Parse(GetApplicationParameterValueByParamName("Early_Occurence", dtCompanyParameter));


                    if (EarlyMin > 0)
                    {

                        if (dtAttReg.Rows.Count > 0)
                        {
                            if (dtAttReg.Rows.Count >= EarlyCount)
                            {
                                RelaxMin = 0;
                                PenaltyMin = EarlyMin;
                            }
                            else
                            {
                                RelaxMinPrev = int.Parse(dtAttReg.Rows[dtAttReg.Rows.Count - 1]["Early_Relaxation_Min"].ToString());

                                if (RelaxMinPrev < RelaxMin && RelaxMinPrev != 0)
                                {
                                    if (EarlyMin > (RelaxMin - RelaxMinPrev))
                                    {
                                        PenaltyMin = EarlyMin - (RelaxMin - RelaxMinPrev);
                                        RelaxMin = RelaxMinPrev + (RelaxMin - RelaxMinPrev);
                                    }
                                    else
                                    {
                                        RelaxMin = EarlyMin + RelaxMinPrev;
                                        //Session["EarlyMin"] = EarlyMin;
                                    }
                                }
                                else
                                {

                                    int LastEarly = int.Parse(dtAttReg.Rows[dtAttReg.Rows.Count - 1]["Early_Relaxation_Min"].ToString());

                                    if (LastEarly < RelaxMin && LastEarly != 0)
                                    {

                                        PenaltyMin = RelaxMin - LastEarly;
                                        RelaxMin = LastEarly + PenaltyMin;
                                        PenaltyMin = EarlyMin - PenaltyMin;

                                    }
                                    else
                                    {



                                        RelaxMin = 0;
                                        PenaltyMin = EarlyMin;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (EarlyMin > RelaxMin)
                            {
                                PenaltyMin = EarlyMin - RelaxMin;

                            }
                            else
                            {
                                RelaxMin = EarlyMin;
                            }
                        }




                    }

                    else
                    {
                        int TotEarlyMin = 0;
                        for (int i = 0; i < dtAttReg.Rows.Count; i++)
                        {
                            TotEarlyMin += int.Parse(dtAttReg.Rows[i]["Early_Relaxation_Min"].ToString());

                        }

                        if (TotEarlyMin >= RelaxMin)
                        {
                            // PenaltyMin = PenaltyDedMin * EarlyMin;
                            PenaltyMin = EarlyMin;

                            RelaxMin = 0;
                        }
                    }
                }
                else
                {
                    EarlyCount = int.Parse(GetApplicationParameterValueByParamName("Early_Occurence", dtCompanyParameter));

                    if (RelaxMin != 0)
                    {
                        RelaxMin = RelaxMin / EarlyCount;
                    }

                    if (EarlyMin > 0)
                    {

                        if (dtAttReg.Rows.Count > 0)
                        {
                            if (dtAttReg.Rows.Count >= EarlyCount)
                            {
                                RelaxMin = 0;
                                PenaltyMin = EarlyMin;
                            }
                            else
                            {

                                RelaxMinPrev = int.Parse(dtAttReg.Rows[dtAttReg.Rows.Count - 1]["Early_Relaxation_Min"].ToString());




                                if (RelaxMinPrev == RelaxMin && RelaxMinPrev != 0)
                                {
                                    if (EarlyMin > RelaxMin)
                                    {
                                        PenaltyMin = EarlyMin - RelaxMin;
                                        if (RelaxMin.ToString() == "0")
                                        {
                                            RelaxMin = 0;
                                        }
                                        else
                                        {
                                            RelaxMin = RelaxMin;
                                        }
                                    }
                                    else
                                    {
                                        // RelaxMin = EarlyMin + RelaxMinPrev;
                                        RelaxMin = EarlyMin;
                                        //Session["EarlyMin"] = EarlyMin;
                                    }
                                }
                                else
                                {

                                    int LastEarly = int.Parse(dtAttReg.Rows[dtAttReg.Rows.Count - 1]["Early_Relaxation_Min"].ToString());

                                    if (LastEarly < RelaxMin && LastEarly != 0)
                                    {

                                        //PenaltyMin = RelaxMin - LastEarly;
                                        RelaxMin = RelaxMin;

                                        //Update Condition On 02-06-2015
                                        if (EarlyMin > RelaxMin)
                                        {
                                            PenaltyMin = EarlyMin - RelaxMin;
                                        }
                                        else
                                        {
                                            PenaltyMin = RelaxMin - EarlyMin;
                                        }
                                    }
                                    else
                                    {



                                        RelaxMin = 0;
                                        PenaltyMin = EarlyMin;
                                    }


                                }
                            }
                        }
                        else
                        {
                            if (EarlyMin > RelaxMin)
                            {
                                PenaltyMin = EarlyMin - RelaxMin;

                            }
                            else
                            {
                                RelaxMin = EarlyMin;
                            }
                        }




                    }

                    else
                    {
                        int TotEarlyMin = 0;
                        for (int i = 0; i < dtAttReg.Rows.Count; i++)
                        {
                            TotEarlyMin += int.Parse(dtAttReg.Rows[i]["Early_Relaxation_Min"].ToString());

                        }

                        if (TotEarlyMin >= RelaxMin)
                        {
                            // PenaltyMin = PenaltyDedMin * EarlyMin;
                            PenaltyMin = EarlyMin;

                            RelaxMin = 0;
                        }
                    }
                }
            }


            //.................... Salary End
            else
            {
                int PenaltyDedMin = 0;

                if (EarlyMin > 0)
                {
                    PenaltyDedMin = int.Parse(GetApplicationParameterValueByParamName("Early_Penalty_Min_Deduct", dtCompanyParameter));

                    int TotEarlyMin = 0;
                    for (int i = 0; i < dtAttReg.Rows.Count; i++)
                    {
                        TotEarlyMin += int.Parse(dtAttReg.Rows[i]["Early_Relaxation_Min"].ToString());

                    }

                    if (TotEarlyMin >= RelaxMin)
                    {
                        // PenaltyMin = PenaltyDedMin * EarlyMin;
                        PenaltyMin = EarlyMin;

                        RelaxMin = 0;
                    }
                    else
                    {




                        if ((TotEarlyMin + EarlyMin) > RelaxMin)
                        {
                            RelaxMin = RelaxMin - TotEarlyMin;
                            PenaltyMin = EarlyMin - RelaxMin;

                        }
                        else
                        {
                            RelaxMin = EarlyMin;
                        }
                    }


                }
                else
                {
                    int TotEarlyMin = 0;
                    for (int i = 0; i < dtAttReg.Rows.Count; i++)
                    {
                        TotEarlyMin += int.Parse(dtAttReg.Rows[i]["Early_Relaxation_Min"].ToString());

                    }

                    if (TotEarlyMin >= RelaxMin)
                    {
                        // PenaltyMin = PenaltyDedMin * EarlyMin;
                        PenaltyMin = EarlyMin;

                        RelaxMin = 0;
                    }
                }
            }
        }
        EarlyRelaxMinPenaltyMin = RelaxMin.ToString() + "-" + PenaltyMin.ToString();



        return EarlyRelaxMinPenaltyMin;


    }
    public string GetApplicationParameterValueByParamName(string strParamName, DataTable dtCompanyParameter)
    {
        return new DataView(dtCompanyParameter, "Param_Name='" + strParamName + "'", "", DataViewRowState.CurrentRows).ToTable().Rows[0]["Param_Value"].ToString();
    }
    public int DeleteAttendanceRegister(string empid, string FromDate, string ToDate)
    {

        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@From_Date", FromDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@To_Date", ToDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Att_AttendanceRegister_Delete", paramList);
        return Convert.ToInt32(paramList[3].ParaValue);
    }

    public DataTable GetAttendanceRegDataByMonth_Year_EmpId(string empid, string month, string year)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        if (empid != string.Empty)
        {
            paramList[0] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        }
        else
        {
            paramList[0] = new PassDataToSql("@Emp_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        }
        paramList[1] = new PassDataToSql("@Month", month, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Year", year, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_AttendanceRegister_SelectRowBy_Month_Year_Emp_Id", paramList);

        return dtInfo;
    }



    public DataTable GetAttendanceRegDataByMonth_Year_EmpId(string empid, string month, string year, ref SqlTransaction trns)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        if (empid != string.Empty)
        {
            paramList[0] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        }
        else
        {
            paramList[0] = new PassDataToSql("@Emp_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        }
        paramList[1] = new PassDataToSql("@Month", month, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Year", year, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_AttendanceRegister_SelectRowBy_Month_Year_Emp_Id", paramList, ref trns);

        return dtInfo;
    }

    public DataTable GetAttendanceRegDataByDate_EmpId(string empid, string StartDate, string EndDate)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        if (empid != string.Empty)
        {
            paramList[0] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        }
        else
        {
            paramList[0] = new PassDataToSql("@Emp_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        }
        paramList[1] = new PassDataToSql("@StartDate", StartDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@EndDate", EndDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_AttendanceRegister_SelectRowBy_Date_Emp_Id", paramList);

        return dtInfo;
    }

    public DataTable GetAttendanceRegDataByDate_EmpId(string empid, string StartDate, string EndDate, ref SqlTransaction trns)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        if (empid != string.Empty)
        {
            paramList[0] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        }
        else
        {
            paramList[0] = new PassDataToSql("@Emp_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        }
        paramList[1] = new PassDataToSql("@StartDate", StartDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@EndDate", EndDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_AttendanceRegister_SelectRowBy_Date_Emp_Id", paramList, ref trns);

        return dtInfo;
    }

    public DataTable GetAttendanceRegDataByMonth_Year_EmpIdPenalty(string empid, string month, string year)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        if (empid != string.Empty)
        {
            paramList[0] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        }
        else
        {
            paramList[0] = new PassDataToSql("@Emp_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        }
        paramList[1] = new PassDataToSql("@Month", month, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Year", year, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_AttendanceRegister_SelectRowBy_Month_Year_Emp_Id", paramList);

        return dtInfo;
    }

    public DataTable GetAttendanceRegDataByMonth_Year_EmpId_TotalDays(string empid, string month, string year, string DaysInMonth)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        if (empid != string.Empty)
        {
            paramList[0] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        }
        else
        {
            paramList[0] = new PassDataToSql("@Emp_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        }
        paramList[1] = new PassDataToSql("@Month", month, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Year", year, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@DaysInMonth", DaysInMonth, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_AttendanceRegister_SelectRowBy_Month_Year_Emp_Id_TotalDays", paramList);

        return dtInfo;
    }



    public DataTable GetAttendanceRegDataByEmpId(string empid, string fromdate, string todate)
    {
        if (empid == "")
        {
            empid = "0";

        }
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@FromDate", fromdate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ToDate", todate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_AttendanceRegister_SelectByDate", paramList);

        return dtInfo;
    }
    public DataTable GetTimeSheetReportByEmpId(string empid, string fromdate, string todate, string OpType)
    {
        if (empid == "")
        {
            empid = "0";

        }
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@EmpId", empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@FromDate", fromdate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ToDate", todate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@OpType", OpType, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("GetTimesheetSummery_Report", paramList);
        return dtInfo;
    }

    //public DataTable GetSalarySummeryReportByEmpId(string empid, string fromdate, string todate)
    //{
    //    if (empid == "")
    //    {
    //        empid = "0";

    //    }
    //    DataTable dtInfo = new DataTable();
    //    PassDataToSql[] paramList = new PassDataToSql[3];
    //    paramList[0] = new PassDataToSql("@EmpId", empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
    //    paramList[1] = new PassDataToSql("@FromDate", fromdate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
    //    paramList[2] = new PassDataToSql("@ToDate", todate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

    //    dtInfo = daClass.Reuturn_Datatable_Search("Get_SalarySummeryReportBy_EmpId", paramList);
    //    return dtInfo;
    //}

    public DataTable GetAttendanceReport(string strEmpId, string strFromDate, string strToDate, string strOptype)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Emp_Id", strEmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@FromDate", strFromDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ToDate", strToDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", strOptype, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_AttendanceRegister_Report", paramList);

        return dtInfo;
    }
}