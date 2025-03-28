using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using PegasusDataAccess;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Att_ScheduleMaster
/// </summary>
public class Att_ScheduleMaster
{

    DataAccessClass daClass = null;


    public Att_ScheduleMaster(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
    }
    public int DeleteScheduleDescByEmpIdandDate(string empid, string fromdate, string todate)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@From_Date", fromdate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@To_Date", todate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Att_ScheduleDescription_Delete", paramList);
        return Convert.ToInt32(paramList[3].ParaValue);
    }

    public int DeleteScheduleDescByEmpIdandDate(string empid, string fromdate, string todate, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@From_Date", fromdate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@To_Date", todate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[3] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Att_ScheduleDescription_Delete", paramList, ref trns);
        return Convert.ToInt32(paramList[3].ParaValue);
    }


    public int InsertScheduleDescriptionForTempShift(string Schedule_Id, string TimeTable_Id, string Att_Date, string Is_Off, string Is_Temp, string Is_OverTime, string Emp_Id, string Shift_Id, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate)
    {

        if (Field1.Trim() == "")
        {
            Field1 = "0";
        }

        PassDataToSql[] paramList = new PassDataToSql[21];
        paramList[0] = new PassDataToSql("@Schedule_Id", Schedule_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@TimeTable_Id", TimeTable_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Shift_Id", Shift_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);


        paramList[4] = new PassDataToSql("@Att_Date", Att_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);



        paramList[5] = new PassDataToSql("@Is_Off", Is_Off, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Is_Temp", Is_Temp, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);

        paramList[7] = new PassDataToSql("@Is_OverTime", Is_OverTime, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);

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

        daClass.execute_Sp("sp_Att_ScheduleDescription_Insert", paramList);
        return Convert.ToInt32(paramList[20].ParaValue);
    }


    public int InsertScheduleDescription(string Schedule_Id, string TimeTable_Id, string Att_Date, string Is_Off, string Is_Temp, string Is_OverTime, string Emp_Id, string Shift_Id, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate)
    {
        if (Shift_Id.Trim() == "0")
        {
            Is_Off = true.ToString();
            TimeTable_Id = "0";
            Is_Temp = false.ToString();
            Is_OverTime = false.ToString();
        }

        if (Field1.Trim() == "")
        {
            Field1 = "0";
        }






        PassDataToSql[] paramList = new PassDataToSql[21];
        paramList[0] = new PassDataToSql("@Schedule_Id", Schedule_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@TimeTable_Id", TimeTable_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Shift_Id", Shift_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);


        paramList[4] = new PassDataToSql("@Att_Date", Att_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);



        paramList[5] = new PassDataToSql("@Is_Off", Is_Off, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Is_Temp", Is_Temp, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);

        paramList[7] = new PassDataToSql("@Is_OverTime", Is_OverTime, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);

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

        daClass.execute_Sp("sp_Att_ScheduleDescription_Insert", paramList);
        return Convert.ToInt32(paramList[20].ParaValue);
    }

    public int InsertScheduleDescription(string Schedule_Id, string TimeTable_Id, string Att_Date, string Is_Off, string Is_Temp, string Is_OverTime, string Emp_Id, string Shift_Id, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
    {

        if (Field1.Trim() == "")
        {
            Field1 = "0";
        }


        PassDataToSql[] paramList = new PassDataToSql[21];
        paramList[0] = new PassDataToSql("@Schedule_Id", Schedule_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@TimeTable_Id", TimeTable_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Shift_Id", Shift_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);


        paramList[4] = new PassDataToSql("@Att_Date", Att_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);



        paramList[5] = new PassDataToSql("@Is_Off", Is_Off, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Is_Temp", Is_Temp, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);

        paramList[7] = new PassDataToSql("@Is_OverTime", Is_OverTime, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);

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

        daClass.execute_Sp("sp_Att_ScheduleDescription_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[20].ParaValue);
    }


    public int InsertScheduleDescriptionByTimeTable(string Schedule_Id, string TimeTable_Id, string Att_Date, string Is_Off, string Is_Temp, string Is_OverTime, string Emp_Id, string Shift_Id, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
    {

        if (Field1.Trim() == "")
        {
            Field1 = "0";
        }


        PassDataToSql[] paramList = new PassDataToSql[21];
        paramList[0] = new PassDataToSql("@Schedule_Id", Schedule_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@TimeTable_Id", Shift_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Shift_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);


        paramList[4] = new PassDataToSql("@Att_Date", Att_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);



        paramList[5] = new PassDataToSql("@Is_Off", Is_Off, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Is_Temp", Is_Temp, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);

        paramList[7] = new PassDataToSql("@Is_OverTime", Is_OverTime, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);

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

        daClass.execute_Sp("sp_Att_ScheduleDescription_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[20].ParaValue);
    }


    public int InsertScheduleMaster(string Company_Id, string Brand_Id, string Location_Id, string Emp_Id, string Shift_Id, string Shift_Type, string From_Date, string To_Date, string Remark, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[22];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", Brand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", Location_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Shift_Id", Shift_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);


        paramList[5] = new PassDataToSql("@Shift_Type", Shift_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@From_Date", From_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[7] = new PassDataToSql("@To_Date", To_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


        paramList[8] = new PassDataToSql("@Remark", Remark, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);


        paramList[9] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[10] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


        paramList[16] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[21] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Att_ScheduleMaster_Insert", paramList);
        return Convert.ToInt32(paramList[21].ParaValue);
    }


    public int InsertScheduleMaster(string Company_Id, string Brand_Id, string Location_Id, string Emp_Id, string Shift_Id, string Shift_Type, string From_Date, string To_Date, string Remark, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[22];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", Brand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", Location_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Shift_Id", Shift_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);


        paramList[5] = new PassDataToSql("@Shift_Type", Shift_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@From_Date", From_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[7] = new PassDataToSql("@To_Date", To_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


        paramList[8] = new PassDataToSql("@Remark", Remark, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);


        paramList[9] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[10] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


        paramList[16] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[21] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Att_ScheduleMaster_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[21].ParaValue);
    }
    public int UpdateScheduleMaster(string Schedule_Id, string Company_Id, string Brand_Id, string Location_Id, string Emp_Id, string Shift_Id, string Shift_Type, string From_Date, string To_Date, string Remark, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[21];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", Brand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", Location_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Shift_Id", Shift_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);


        paramList[5] = new PassDataToSql("@Shift_Type", Shift_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@From_Date", From_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[7] = new PassDataToSql("@To_Date", To_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


        paramList[8] = new PassDataToSql("@Remark", Remark, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);


        paramList[9] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[10] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


        paramList[16] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[19] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        paramList[20] = new PassDataToSql("@Schedule_Id", Schedule_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);



        daClass.execute_Sp("sp_Att_ScheduleMaster_Update", paramList);
        return Convert.ToInt32(paramList[19].ParaValue);
    }


    public int UpdateScheduleMaster(string Schedule_Id, string Company_Id, string Brand_Id, string Location_Id, string Emp_Id, string Shift_Id, string Shift_Type, string From_Date, string To_Date, string Remark, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[21];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", Brand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", Location_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Shift_Id", Shift_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);


        paramList[5] = new PassDataToSql("@Shift_Type", Shift_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@From_Date", From_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[7] = new PassDataToSql("@To_Date", To_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


        paramList[8] = new PassDataToSql("@Remark", Remark, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);


        paramList[9] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[10] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);


        paramList[16] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[19] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        paramList[20] = new PassDataToSql("@Schedule_Id", Schedule_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);



        daClass.execute_Sp("sp_Att_ScheduleMaster_Update", paramList, ref trns);
        return Convert.ToInt32(paramList[19].ParaValue);
    }

    public DataTable GetSheduleDescriptionByEmpId(string Emp_Id, string date)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];

        paramList[0] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Att_Date", date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_ScheduleDescription_SelectRow", paramList);

        return dtInfo;
    }

    public DataTable GetSheduleDescriptionByEmpId(string Emp_Id, string date, ref SqlTransaction trns)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];

        paramList[0] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Att_Date", date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_ScheduleDescription_SelectRow", paramList, ref trns);

        return dtInfo;
    }
    public DataTable GetSheduleDescription(string Emp_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Att_Date", "1/1/2000", PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_ScheduleDescription_SelectRow", paramList);

        return dtInfo;
    }

    public DataTable GetSheduleDescription(string Emp_Id,ref SqlTransaction trans)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Att_Date", "1/1/2000", PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_ScheduleDescription_SelectRow", paramList,ref trans);
        return dtInfo;
    }

    //
    public DataTable Sp_GetEMployeeSchedule(string Emp_Id, String StartDate, string EndDate)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@EmpIds", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@FromDate", StartDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@TODate", EndDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("Sp_GetEMployeeSchedule", paramList);

        return dtInfo;
    }
    //

    public DataTable GetSheduleDescriptionAll(string EmpList)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Emp_Id", EmpList, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Att_Date", "1/1/2000", PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@optype", "5", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_ScheduleDescription_SelectRow", paramList);

        return dtInfo;
    }

    public DataTable GetSheduleDescriptionByDate(string Emp_Id, string AttDate)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];

        paramList[0] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Att_Date", AttDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@optype", "4", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_ScheduleDescription_SelectRow", paramList);

        return dtInfo;
    }


    public DataTable GetSheduleDescription()
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];

        paramList[0] = new PassDataToSql("@Emp_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Att_Date", "1/1/2000", PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_ScheduleDescription_SelectRow", paramList);

        return dtInfo;
    }
    public DataTable GetSheduleMasterByShiftId(string Schedule_Id)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];

        paramList[0] = new PassDataToSql("@Schedule_Id", @Schedule_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_ScheduleMaster_SelectRow", paramList);

        return dtInfo;
    }
    public DataTable GetSheduleMaster()
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];

        paramList[0] = new PassDataToSql("@Schedule_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_ScheduleMaster_SelectRow", paramList);

        return dtInfo;
    }


    public DataTable GetSheduleMaster(ref SqlTransaction trns)
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];

        paramList[0] = new PassDataToSql("@Schedule_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_ScheduleMaster_SelectRow", paramList, ref trns);

        return dtInfo;
    }


    public  bool AssignShift1(DataTable dtTempShiftDetail, string strEmpId, DateTime DtFromDate, DateTime dtToDate, ref SqlTransaction trns, string strCompanyid, string strBrandid, string strLocationId, string strTimeZoneId, string struserid)
    {

        DataAccessClass ObjDa = new DataAccessClass(trns.Connection.ConnectionString);
        Set_Employee_Holiday objEmpHoliday = new Set_Employee_Holiday(trns.Connection.ConnectionString);
        Att_ScheduleMaster objEmpSch = new Att_ScheduleMaster(trns.Connection.ConnectionString);
        Att_ShiftManagement objShift = new Att_ShiftManagement(trns.Connection.ConnectionString);
        Att_ShiftDescription objShiftdesc = new Att_ShiftDescription(trns.Connection.ConnectionString);

        Set_ApplicationParameter objAppParam = new Set_ApplicationParameter(trns.Connection.ConnectionString);
        DataTable dtTempsch = new DataTable();
        DataTable dtSch = new DataTable();
        DataTable dtTime = new DataTable();
        DateTime dtStartCheck = new DateTime();
        DataTable dtTempShift = new DataTable();
        DataTable dtShift = new DataTable();
        DataTable dtShiftD = new DataTable();
        string OverlapDate = string.Empty;
        string ExcludeDayAs = string.Empty;
        string CompWeekOffDay = string.Empty;
        string strShiftId = string.Empty;
        ExcludeDayAs = objAppParam.GetApplicationParameterValueByParamName("Exclude Day As Absent or IsOff", strCompanyid, strBrandid, strLocationId);

        //Update On 25-06-2015 For Week Off Parameter
        bool strWeekOffParam = true;
        DataTable dtWeekOffParam = objAppParam.GetApplicationParameterByCompanyId("AddWeekOffInShift", strCompanyid, ref trns, strBrandid, strLocationId);
        dtWeekOffParam = new DataView(dtWeekOffParam, "Param_Name='AddWeekOffInShift'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtWeekOffParam.Rows.Count > 0)
        {
            strWeekOffParam = Convert.ToBoolean(dtWeekOffParam.Rows[0]["Param_Value"].ToString());
        }
        else
        {
            strWeekOffParam = true;
        }


        string WeekOff = string.Empty;
        if (strWeekOffParam == true)
        {
            CompWeekOffDay = objAppParam.GetApplicationParameterValueByParamName("Week Off Days", strCompanyid, strBrandid, strLocationId, ref trns);
        }

        int b = 1;
        int rem = 0;



        //while (DtFromDate <= dtToDate)
        //{

        foreach (DataRow dr in dtTempShiftDetail.Rows)
        {

            dtSch = ObjDa.return_DataTable("SELECT Schedule_Id,shift_id from Att_ScheduleMaster  where IsActive='True' and emp_id=" + strEmpId + " and Shift_Type='Normal Shift'", ref trns);

            DtFromDate = Convert.ToDateTime(dr["schedule_date"].ToString());

            //deleting exists holiday
            objEmpHoliday.DeleteEmployeeHolidayMasterByEmpIdandDateOnly(strCompanyid, strEmpId, DtFromDate.ToString(), ref trns);



            if (dr["ref_Type"].ToString().ToUpper() == "OFF")
            {
                objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, "0", dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                DtFromDate = DtFromDate.AddDays(1);
                continue;
            }
            if (dr["ref_Type"].ToString().ToUpper() == "HOLIDAY")
            {
                //deleting scheduled shift
                objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                //inserting holiday
                objEmpHoliday.InsertEmployeeHolidayMaster(strCompanyid, dr["ref_Id"].ToString(), DtFromDate.ToString(), strEmpId, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                DtFromDate = DtFromDate.AddDays(1);
                continue;

            }
            if (dr["ref_Type"].ToString().Trim() == "")
            {
                objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                DtFromDate = DtFromDate.AddDays(1);
                continue;
            }

            //here we are deleting shift for specific day

            objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

            strShiftId = dr["ref_id"].ToString();

            dtShift = objShift.GetShiftMasterById(strCompanyid, strShiftId, ref trns);
            dtShiftD = objShiftdesc.GetShiftDescriptionByShiftId(strShiftId, ref trns);

            if (dtSch != null)
            {
                dtTempsch = new DataView(dtSch, "Shift_Id='" + strShiftId + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtTempsch.Rows.Count == 0)
                {

                    b = objEmpSch.InsertScheduleMaster(strCompanyid, strBrandid, strLocationId, strEmpId, strShiftId, "Normal Shift", DtFromDate.ToString(), DtFromDate.ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                }
                else
                {
                    b = objEmpSch.UpdateScheduleMaster(dtTempsch.Rows[0]["Schedule_Id"].ToString(), strCompanyid, strBrandid, strLocationId, strEmpId, strShiftId, "Normal Shift", DtFromDate.ToString(), DtFromDate.ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                    //DisplayMessage("Shift Already Assignd to this Employee");
                    //return;
                }
            }
            else
            {
                b = objEmpSch.InsertScheduleMaster(strCompanyid, strBrandid, strLocationId, strEmpId, strShiftId, "Normal Shift", DtFromDate.ToString(), DtFromDate.ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
            }


            // From Date to To Date

            dtStartCheck = Convert.ToDateTime(dtShift.Rows[0]["Apply_From"].ToString());


            int index = Convert.ToInt16(dtShift.Rows[0]["Cycle_Unit"].ToString().Trim());
            int cycle = Convert.ToInt16(dtShift.Rows[0]["Cycle_No"].ToString().Trim());

            string cycletype = string.Empty;
            string cycleday = string.Empty;

            if (index == 7)
            {
                int daysShift = cycle * index;
                string weekday = DtFromDate.DayOfWeek.ToString();

                int k = GetCycleDay(weekday);
                int j = 1;
                int a = k;
                int f = 0;

                if (k % 7 == 0)
                {
                    if (f != 0)
                    {
                        if (k % 7 == 0 && j > cycle)
                        {
                            j++;
                            rem = 1;
                        }
                        //else
                        //{
                        //    j++;
                        //}

                        if (j > cycle)
                        {
                            j = 1;
                        }
                    }

                }
                f++;
                if (k <= daysShift || j == cycle)
                {


                    a = GetCycleDay(DtFromDate.DayOfWeek.ToString());
                    if (rem == 1 && k % 7 == 0)
                    {
                        j++;
                    }

                    DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Week-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (k % 7 == 0 && j == cycle)
                    {
                        j = 1;
                    }
                    if (k % 7 == 0 && j < cycle)
                    {
                        j++;
                    }

                    objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);


                    if (dtGetTemp1.Rows.Count > 0)
                    {
                        dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                        if (dtTime.Rows.Count > 0)
                        {
                            if (dtTime.Rows[0]["Is_Off"].ToString() == "True")
                            {

                                //here we are deleting shift for selected date 

                                if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() == "")
                                {
                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                }
                                else
                                {

                                    for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                            }
                            else
                            {

                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                {
                                    DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString, strCompanyid));
                                    DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString, strCompanyid));
                                    int flag1 = 0;
                                    for (int s = 0; s < dtTime.Rows.Count; s++)
                                    {


                                        if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime, strTimeZoneId))
                                        {
                                            flag1 = 1;
                                            OverlapDate += dtTime.Rows[s]["Att_Date"].ToString() + ",";
                                        }

                                    }
                                    if (flag1 == 0)
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);


                                    }
                                }


                            }
                        }
                        else
                        {
                            if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() != "")
                            {
                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                {
                                    objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                }

                            }
                            else
                            {

                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);


                            }

                        }
                    }
                    else
                    {
                        //Modified accoding to excludedays parameter 19 sept 2013 kunal
                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                        if (dts.Rows.Count == 0)
                        {
                            if (ExcludeDayAs == "IsOff")
                            {
                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

                            }
                            else
                            {
                                // Modified By Nitin Jain On 27/08/2014....
                                foreach (string str in CompWeekOffDay.Split(','))
                                {
                                    if (str == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                                //..............
                            }

                        }
                        else
                        {


                        }
                    }

                    k++;
                }
                else
                {
                    k = 1;
                    j = 1;
                    f = 0;
                    a = GetCycleDay(DtFromDate.DayOfWeek.ToString());

                    DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Week-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                    if (dtGetTemp1.Rows.Count > 0)
                    {
                        dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                        if (dtTime.Rows.Count > 0)
                        {
                            if (dtTime.Rows[0]["Is_Off"].ToString() == "True")
                            {
                                objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() == "")
                                {
                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                }
                                else
                                {

                                    for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                            }
                            else
                            {
                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                {
                                    DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString, strCompanyid));
                                    DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString, strCompanyid));
                                    int flag1 = 0;
                                    for (int s = 0; s < dtTime.Rows.Count; s++)
                                    {
                                        if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime, strTimeZoneId))
                                        {
                                            flag1 = 1;
                                        }

                                    }
                                    if (flag1 == 0)
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

                                    }
                                }

                            }
                        }
                        else
                        {
                            if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() != "")
                            {
                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                {
                                    objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                }

                            }
                            else
                            {

                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);


                            }
                        }
                    }
                    else
                    {
                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                        if (dts.Rows.Count == 0)
                        {
                            if (ExcludeDayAs == "IsOff")
                            {
                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                            }
                            else
                            {
                                // Modifed By Nitin On 27/8/2014/////
                                foreach (string str in CompWeekOffDay.Split(','))
                                {
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                            }
                        }
                    }
                    k++;

                }

            }
            else if (index == 31)
            {
                int daysShift = cycle * index;

                int k = DtFromDate.Day;
                int a = 0;
                int j = 1;
                int mon = DtFromDate.Month;
                if (k <= daysShift)
                {
                    a = DtFromDate.Day;

                    DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Month-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    //
                    if (dtGetTemp1.Rows.Count > 0)
                    {


                        for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                        {
                            dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                            if (dtGetTemp1.Rows[t]["TimeTable_Id"].ToString() == "")
                            {

                                DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                                if (dts.Rows.Count == 0)
                                {
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                    else
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                            }
                            else
                            {
                                int flag1 = 0;

                                if (dtTime.Rows.Count > 0)
                                {
                                    DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString, strCompanyid));
                                    DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString, strCompanyid));
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                    else
                                    {
                                        for (int s = 0; s < dtTime.Rows.Count; s++)
                                        {


                                            if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime, strTimeZoneId))
                                            {
                                                flag1 = 1;
                                            }

                                        }
                                        if (flag1 == 0)
                                        {

                                            objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);


                                        }
                                    }

                                }
                                else
                                {
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                    else
                                    {
                                        for (int t1 = 0; t1 < dtGetTemp1.Rows.Count; t1++)
                                        {
                                            objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t1]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

                                        }

                                    }

                                }
                            }

                        }



                    }
                    else
                    {
                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                        if (dts.Rows.Count == 0)
                        {
                            if (ExcludeDayAs == "IsOff")
                            {
                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                            }
                            else
                            {
                                // Modified By Nitin Jain to Check Multiple Week Off On 27/08/2014....
                                foreach (string str in CompWeekOffDay.Split(','))
                                {
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                            }
                        }
                    }
                }

                k++;
                if (k > daysShift)
                {

                    k = 1;
                    j = 1;
                }
                if (DtFromDate.Day == 1)
                {

                    j++;

                }
            }
            else if (index == 1)
            {
                int k = 1;
                int a = k;
                int daysShift = cycle * index;
                if (k <= daysShift)
                {
                    a = k;


                    DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Day' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    //
                    if (dtGetTemp1.Rows.Count > 0)
                    {


                        for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                        {
                            dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                            if (dtGetTemp1.Rows[t]["TimeTable_Id"].ToString() == "")
                            {

                                DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                                if (dts.Rows.Count == 0)
                                {
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                    else
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                            }
                            else
                            {
                                int flag1 = 0;

                                if (dtTime.Rows.Count > 0)
                                {
                                    DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString, strCompanyid));
                                    DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString, strCompanyid));
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                    else
                                    {
                                        for (int s = 0; s < dtTime.Rows.Count; s++)
                                        {

                                            if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime, strTimeZoneId))
                                            {
                                                flag1 = 1;
                                            }

                                        }
                                        if (flag1 == 0)
                                        {
                                            objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);


                                        }

                                    }
                                }
                                else
                                {
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                    else
                                    {

                                        for (int t1 = 0; t1 < dtGetTemp1.Rows.Count; t1++)
                                        {

                                            objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t1]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

                                        }
                                    }

                                }
                            }

                        }

                    }
                    else
                    {
                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                        if (dts.Rows.Count == 0)
                        {
                            if (ExcludeDayAs == "IsOff")
                            {

                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                            }
                            else
                            {
                                // Modified By Nitin Jain to Check Multiple Week Off On 27/08/2014....
                                foreach (string str in CompWeekOffDay.Split(','))
                                {
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                            }
                        }
                    }


                }

                k++;
                if (k > daysShift)
                {

                    k = 1;

                }

            }

            DtFromDate = DtFromDate.AddDays(1);
        }


        dtTempsch.Dispose();
        dtSch.Dispose();
        dtShift.Dispose();
        dtShiftD.Dispose();
        dtTime.Dispose();
        dtTempShift.Dispose();
        return true;
    }


    public static bool AssignShift(DataTable dtTempShiftDetail, string strEmpId, DateTime DtFromDate, DateTime dtToDate, ref SqlTransaction trns,string strCompanyid,string strBrandid,string strLocationId,string strTimeZoneId,string struserid)
    {
        
        DataAccessClass ObjDa = new DataAccessClass(trns.Connection.ConnectionString);
        Set_Employee_Holiday objEmpHoliday = new Set_Employee_Holiday(trns.Connection.ConnectionString);
        Att_ScheduleMaster objEmpSch = new Att_ScheduleMaster(trns.Connection.ConnectionString);
        Att_ShiftManagement objShift = new Att_ShiftManagement(trns.Connection.ConnectionString);
        Att_ShiftDescription objShiftdesc = new Att_ShiftDescription(trns.Connection.ConnectionString);

        Set_ApplicationParameter objAppParam = new Set_ApplicationParameter(trns.Connection.ConnectionString);
        DataTable dtTempsch = new DataTable();
        DataTable dtSch = new DataTable();
        DataTable dtTime = new DataTable();
        DateTime dtStartCheck = new DateTime();
        DataTable dtTempShift = new DataTable();
        DataTable dtShift = new DataTable();
        DataTable dtShiftD = new DataTable();
        string OverlapDate = string.Empty;
        string ExcludeDayAs = string.Empty;
        string CompWeekOffDay = string.Empty;
        string strShiftId = string.Empty;
        ExcludeDayAs = objAppParam.GetApplicationParameterValueByParamName("Exclude Day As Absent or IsOff",strCompanyid,strBrandid,strLocationId, ref trns);

        //Update On 25-06-2015 For Week Off Parameter
        bool strWeekOffParam = true;
        DataTable dtWeekOffParam = objAppParam.GetApplicationParameterByCompanyId("AddWeekOffInShift", strCompanyid,ref trns, strBrandid, strLocationId);
        dtWeekOffParam = new DataView(dtWeekOffParam, "Param_Name='AddWeekOffInShift'", "", DataViewRowState.CurrentRows).ToTable();
        if (dtWeekOffParam.Rows.Count > 0)
        {
            strWeekOffParam = Convert.ToBoolean(dtWeekOffParam.Rows[0]["Param_Value"].ToString());
        }
        else
        {
            strWeekOffParam = true;
        }


        string WeekOff = string.Empty;
        if (strWeekOffParam == true)
        {
            CompWeekOffDay = objAppParam.GetApplicationParameterValueByParamName("Week Off Days", strCompanyid, strBrandid, strLocationId, ref trns);
        }

        int b = 1;
        int rem = 0;



        //while (DtFromDate <= dtToDate)
        //{

        foreach (DataRow dr in dtTempShiftDetail.Rows)
        {

            dtSch = ObjDa.return_DataTable("SELECT Schedule_Id,shift_id from Att_ScheduleMaster  where IsActive='True' and emp_id=" + strEmpId + " and Shift_Type='Normal Shift'", ref trns);

            DtFromDate = Convert.ToDateTime(dr["schedule_date"].ToString());

            //deleting exists holiday
            objEmpHoliday.DeleteEmployeeHolidayMasterByEmpIdandDateOnly(strCompanyid, strEmpId, DtFromDate.ToString(), ref trns);



            if (dr["ref_Type"].ToString().ToUpper() == "OFF")
            {
                objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, "0", dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(),struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                DtFromDate = DtFromDate.AddDays(1);
                continue;
            }
            if (dr["ref_Type"].ToString().ToUpper() == "HOLIDAY")
            {
                //deleting scheduled shift
                objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                //inserting holiday
                objEmpHoliday.InsertEmployeeHolidayMaster(strCompanyid, dr["ref_Id"].ToString(), DtFromDate.ToString(), strEmpId, "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                DtFromDate = DtFromDate.AddDays(1);
                continue;

            }
            if (dr["ref_Type"].ToString().Trim() == "")
            {
                objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                DtFromDate = DtFromDate.AddDays(1);
                continue;
            }

            //here we are deleting shift for specific day

            objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

            strShiftId = dr["ref_id"].ToString();

            dtShift = objShift.GetShiftMasterById(strCompanyid, strShiftId, ref trns);
            dtShiftD = objShiftdesc.GetShiftDescriptionByShiftId(strShiftId, ref trns);

            if (dtSch != null)
            {
                dtTempsch = new DataView(dtSch, "Shift_Id='" + strShiftId + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtTempsch.Rows.Count == 0)
                {

                    b = objEmpSch.InsertScheduleMaster(strCompanyid, strBrandid,strLocationId, strEmpId, strShiftId, "Normal Shift", DtFromDate.ToString(), DtFromDate.ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), ref trns);
                }
                else
                {
                    b = objEmpSch.UpdateScheduleMaster(dtTempsch.Rows[0]["Schedule_Id"].ToString(), strCompanyid, strBrandid, strLocationId, strEmpId, strShiftId, "Normal Shift", DtFromDate.ToString(), DtFromDate.ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), true.ToString(), struserid , Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), ref trns);
                    //DisplayMessage("Shift Already Assignd to this Employee");
                    //return;
                }
            }
            else
            {
                b = objEmpSch.InsertScheduleMaster(strCompanyid, strBrandid, strLocationId, strEmpId, strShiftId, "Normal Shift", DtFromDate.ToString(), DtFromDate.ToString(), "", "", "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), ref trns);
            }


            // From Date to To Date

            dtStartCheck = Convert.ToDateTime(dtShift.Rows[0]["Apply_From"].ToString());


            int index = Convert.ToInt16(dtShift.Rows[0]["Cycle_Unit"].ToString().Trim());
            int cycle = Convert.ToInt16(dtShift.Rows[0]["Cycle_No"].ToString().Trim());

            string cycletype = string.Empty;
            string cycleday = string.Empty;

            if (index == 7)
            {
                int daysShift = cycle * index;
                string weekday = DtFromDate.DayOfWeek.ToString();

                int k = GetCycleDay(weekday);
                int j = 1;
                int a = k;
                int f = 0;

                if (k % 7 == 0)
                {
                    if (f != 0)
                    {
                        if (k % 7 == 0 && j > cycle)
                        {
                            j++;
                            rem = 1;
                        }
                        //else
                        //{
                        //    j++;
                        //}

                        if (j > cycle)
                        {
                            j = 1;
                        }
                    }

                }
                f++;
                if (k <= daysShift || j == cycle)
                {


                    a = GetCycleDay(DtFromDate.DayOfWeek.ToString());
                    if (rem == 1 && k % 7 == 0)
                    {
                        j++;
                    }

                    DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Week-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    if (k % 7 == 0 && j == cycle)
                    {
                        j = 1;
                    }
                    if (k % 7 == 0 && j < cycle)
                    {
                        j++;
                    }

                    objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);


                    if (dtGetTemp1.Rows.Count > 0)
                    {
                        dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                        if (dtTime.Rows.Count > 0)
                        {
                            if (dtTime.Rows[0]["Is_Off"].ToString() == "True")
                            {

                                //here we are deleting shift for selected date 

                                if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() == "")
                                {
                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), true.ToString(), struserid , Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), ref trns);
                                }
                                else
                                {

                                    for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                            }
                            else
                            {

                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                {
                                    DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString,strCompanyid));
                                    DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString, strCompanyid));
                                    int flag1 = 0;
                                    for (int s = 0; s < dtTime.Rows.Count; s++)
                                    {


                                        if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime,strTimeZoneId))
                                        {
                                            flag1 = 1;
                                            OverlapDate += dtTime.Rows[s]["Att_Date"].ToString() + ",";
                                        }

                                    }
                                    if (flag1 == 0)
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), ref trns);


                                    }
                                }


                            }
                        }
                        else
                        {
                            if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() != "")
                            {
                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                {
                                    objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), ref trns);
                                }

                            }
                            else
                            {

                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(),struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), ref trns);


                            }

                        }
                    }
                    else
                    {
                        //Modified accoding to excludedays parameter 19 sept 2013 kunal
                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                        if (dts.Rows.Count == 0)
                        {
                            if (ExcludeDayAs == "IsOff")
                            {
                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

                            }
                            else
                            {
                                // Modified By Nitin Jain On 27/08/2014....
                                foreach (string str in CompWeekOffDay.Split(','))
                                {
                                    if (str == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                                //..............
                            }

                        }
                        else
                        {


                        }
                    }

                    k++;
                }
                else
                {
                    k = 1;
                    j = 1;
                    f = 0;
                    a = GetCycleDay(DtFromDate.DayOfWeek.ToString());

                    DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Week-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();
                    dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                    if (dtGetTemp1.Rows.Count > 0)
                    {
                        dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                        if (dtTime.Rows.Count > 0)
                        {
                            if (dtTime.Rows[0]["Is_Off"].ToString() == "True")
                            {
                                objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() == "")
                                {
                                    objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                }
                                else
                                {

                                    for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                            }
                            else
                            {
                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                {
                                    DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString,strCompanyid));
                                    DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString,strCompanyid));
                                    int flag1 = 0;
                                    for (int s = 0; s < dtTime.Rows.Count; s++)
                                    {
                                        if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime,strTimeZoneId))
                                        {
                                            flag1 = 1;
                                        }

                                    }
                                    if (flag1 == 0)
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

                                    }
                                }

                            }
                        }
                        else
                        {
                            if (dtGetTemp1.Rows[0]["TimeTable_Id"].ToString() != "")
                            {
                                for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                                {
                                    objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                }

                            }
                            else
                            {

                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(),struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);


                            }
                        }
                    }
                    else
                    {
                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                        if (dts.Rows.Count == 0)
                        {
                            if (ExcludeDayAs == "IsOff")
                            {
                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(),struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                            }
                            else
                            {
                                // Modifed By Nitin On 27/8/2014/////
                                foreach (string str in CompWeekOffDay.Split(','))
                                {
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                            }
                        }
                    }
                    k++;

                }

            }
            else if (index == 31)
            {
                int daysShift = cycle * index;

                int k = DtFromDate.Day;
                int a = 0;
                int j = 1;
                int mon = DtFromDate.Month;
                if (k <= daysShift)
                {
                    a = DtFromDate.Day;

                    DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Month-" + j.ToString() + "' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    //
                    if (dtGetTemp1.Rows.Count > 0)
                    {


                        for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                        {
                            dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                            if (dtGetTemp1.Rows[t]["TimeTable_Id"].ToString() == "")
                            {

                                DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                                if (dts.Rows.Count == 0)
                                {
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(),struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                    else
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                            }
                            else
                            {
                                int flag1 = 0;

                                if (dtTime.Rows.Count > 0)
                                {
                                    DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString,strCompanyid));
                                    DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString,strCompanyid));
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(),struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), ref trns);
                                    }
                                    else
                                    {
                                        for (int s = 0; s < dtTime.Rows.Count; s++)
                                        {


                                            if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime,strTimeZoneId))
                                            {
                                                flag1 = 1;
                                            }

                                        }
                                        if (flag1 == 0)
                                        {

                                            objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);


                                        }
                                    }

                                }
                                else
                                {
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                    else
                                    {
                                        for (int t1 = 0; t1 < dtGetTemp1.Rows.Count; t1++)
                                        {
                                            objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t1]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

                                        }

                                    }

                                }
                            }

                        }



                    }
                    else
                    {
                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                        if (dts.Rows.Count == 0)
                        {
                            if (ExcludeDayAs == "IsOff")
                            {
                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                            }
                            else
                            {
                                // Modified By Nitin Jain to Check Multiple Week Off On 27/08/2014....
                                foreach (string str in CompWeekOffDay.Split(','))
                                {
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), true.ToString(),struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                            }
                        }
                    }
                }

                k++;
                if (k > daysShift)
                {

                    k = 1;
                    j = 1;
                }
                if (DtFromDate.Day == 1)
                {

                    j++;

                }
            }
            else if (index == 1)
            {
                int k = 1;
                int a = k;
                int daysShift = cycle * index;
                if (k <= daysShift)
                {
                    a = k;


                    DataTable dtGetTemp1 = new DataView(dtShiftD, "Cycle_Type='Day' and Cycle_Day='" + a.ToString() + "'", "", DataViewRowState.CurrentRows).ToTable();

                    //
                    if (dtGetTemp1.Rows.Count > 0)
                    {


                        for (int t = 0; t < dtGetTemp1.Rows.Count; t++)
                        {
                            dtTime = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                            if (dtGetTemp1.Rows[t]["TimeTable_Id"].ToString() == "")
                            {

                                DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                                if (dts.Rows.Count == 0)
                                {
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                    else
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid,Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(),  ref trns);
                                    }
                                }
                            }
                            else
                            {
                                int flag1 = 0;

                                if (dtTime.Rows.Count > 0)
                                {
                                    DateTime OnDutyTime = Convert.ToDateTime(GetDutyTime("On", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString,strCompanyid));
                                    DateTime OffDutyTime = Convert.ToDateTime(GetDutyTime("Off", dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), trns.Connection.ConnectionString,strCompanyid));
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                    else
                                    {
                                        for (int s = 0; s < dtTime.Rows.Count; s++)
                                        {

                                            if (ISOverLapTimeTable(Convert.ToDateTime(dtTime.Rows[s]["OnDuty_Time"].ToString()), Convert.ToDateTime(dtTime.Rows[s]["OffDuty_Time"].ToString()), OnDutyTime, OffDutyTime,strTimeZoneId))
                                            {
                                                flag1 = 1;
                                            }

                                        }
                                        if (flag1 == 0)
                                        {
                                            objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);


                                        }

                                    }
                                }
                                else
                                {
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.DeleteScheduleDescByEmpIdandDate(strEmpId, DtFromDate.ToString(), DtFromDate.ToString(), ref trns);

                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                    else
                                    {

                                        for (int t1 = 0; t1 < dtGetTemp1.Rows.Count; t1++)
                                        {

                                            objEmpSch.InsertScheduleDescription(b.ToString(), dtGetTemp1.Rows[t1]["TimeTable_Id"].ToString(), DtFromDate.ToString(), false.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);

                                        }
                                    }

                                }
                            }

                        }

                    }
                    else
                    {
                        DataTable dts = objEmpSch.GetSheduleDescriptionByEmpId(strEmpId, DtFromDate.ToString(), ref trns);

                        if (dts.Rows.Count == 0)
                        {
                            if (ExcludeDayAs == "IsOff")
                            {

                                objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                            }
                            else
                            {
                                // Modified By Nitin Jain to Check Multiple Week Off On 27/08/2014....
                                foreach (string str in CompWeekOffDay.Split(','))
                                {
                                    if (CompWeekOffDay == DtFromDate.DayOfWeek.ToString())
                                    {
                                        objEmpSch.InsertScheduleDescription(b.ToString(), "0", DtFromDate.ToString(), true.ToString(), false.ToString(), false.ToString(), strEmpId, strShiftId, dr["Field1"].ToString(), "", "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), struserid, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ref trns);
                                    }
                                }
                            }
                        }
                    }


                }

                k++;
                if (k > daysShift)
                {

                    k = 1;

                }

            }

            DtFromDate = DtFromDate.AddDays(1);
        }


        dtTempsch.Dispose();
        dtSch.Dispose();
        dtShift.Dispose();
        dtShiftD.Dispose();
        dtTime.Dispose();
        dtTempShift.Dispose();
        return true;
    }


    public static bool ISOverLapTimeTable(DateTime dtintime1, DateTime dtouttime1, DateTime dtintime, DateTime dtouttime,string strTimeZoneId)
    {
        dtintime1 = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Year, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Month, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Day, dtintime1.Hour, dtintime1.Minute, dtintime1.Second);
        dtouttime1 = new DateTime(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Year, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Month, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Day, dtouttime1.Hour, dtouttime1.Minute, dtouttime1.Second);

        bool isoverlap = false;
        if (dtintime >= dtintime1 && dtintime <= dtouttime1)
        {
            isoverlap = true;

        }
        else if (dtouttime >= dtintime1 && dtouttime <= dtouttime1)
        {
            isoverlap = true;

        }

        else if (dtintime1 >= dtintime && dtintime1 <= dtouttime)
        {
            isoverlap = true;

        }

        else if (dtouttime1 >= dtintime && dtouttime1 <= dtouttime)
        {
            isoverlap = true;

        }
        else if (dtintime1 == dtintime && dtouttime1 == dtouttime)
        {
            isoverlap = true;

        }
        return isoverlap;
    }

    public static int GetCycleDay(string day)
    {
        string cycleday = string.Empty;
        string[] weekdays = new string[8];

        weekdays[1] = "Monday";
        weekdays[2] = "Tuesday";
        weekdays[3] = "Wednesday";
        weekdays[4] = "Thursday";
        weekdays[5] = "Friday";
        weekdays[6] = "Saturday";
        weekdays[7] = "Sunday";

        for (int i = 1; i <= 7; i++)
        {
            if (weekdays[i] == day)
            {
                cycleday = i.ToString();

            }
        }

        return int.Parse(cycleday);

    }
    public static string GetDutyTime(string OnOff, string timetableid,string strConString,string strCompanyid)
    {
        Att_TimeTable objTimeTable = new Att_TimeTable(strConString);
        string retval = "";
        DataTable dtTimeTableId = objTimeTable.GetTimeTableMasterById(strCompanyid, timetableid);
        if (OnOff == "On")
        {
            retval = dtTimeTableId.Rows[0]["OnDuty_Time"].ToString();
        }
        else
        {
            retval = dtTimeTableId.Rows[0]["OffDuty_Time"].ToString();
        }
        return retval;

    }



}
