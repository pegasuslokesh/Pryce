using System;
using System.Data;
using PegasusDataAccess;
using System.Data.SqlClient;
/// <summary>
/// Summary description for Prj_ProjectTask
/// </summary>
public class Prj_ProjectTask
{
    DataAccessClass daClass = null;

    public Prj_ProjectTask(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
    }
    // Insert record into project Task
    public int InsertProjectTask(string ProjectId, string EmployeeId, string AssignDate, string AssignTime, string EmpCloseDate, string EmpCloseTime, string TlCloseDate, string TlCloseTime, string Subject, string Description, string FileId, string Status, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate, string strTask_Type, string strTask_Site_Address, string strContact_Person, string strExpectedCost,string requiredHours,string assignby)
    {
        if (strExpectedCost == "")
        {
            strExpectedCost = "0";
        }
        PassDataToSql[] paramList = new PassDataToSql[31];
        paramList[0] = new PassDataToSql("@Project_Id", ProjectId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Employee_Id", EmployeeId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Assign_Date", AssignDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Assign_Time", Convert.ToDateTime(AssignTime).ToString(), PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Emp_Close_Date", EmpCloseDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Emp_Close_Time", Convert.ToDateTime(EmpCloseTime).ToString(), PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@TL_Close_Date", TlCloseDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@TL_Close_Time", Convert.ToDateTime(TlCloseTime).ToString(), PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@subject", Subject, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Description", Description, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@File_Id", FileId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Status", Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[25] = new PassDataToSql("@Task_Type", strTask_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@Task_Site_Address", strTask_Site_Address, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@Contact_Person", strContact_Person, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@Expected_Cost", strExpectedCost, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[29] = new PassDataToSql("@RequiredHours", requiredHours, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[30] = new PassDataToSql("@AssignBy", assignby, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Prj_Project_Task_Insert", paramList);
        return Convert.ToInt32(paramList[24].ParaValue);
    }
    public int InsertProjectTask(string ProjectId, string EmployeeId, string AssignDate, string AssignTime, string EmpCloseDate, string EmpCloseTime, string TlCloseDate, string TlCloseTime, string Subject, string Description, string FileId, string Status, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate, string strTask_Type, string strTask_Site_Address, string strContact_Person, string strExpectedCost, string requiredHours, string assignby,ref SqlTransaction trans)
    {
        if (strExpectedCost == "")
        {
            strExpectedCost = "0";
        }
        PassDataToSql[] paramList = new PassDataToSql[31];
        paramList[0] = new PassDataToSql("@Project_Id", ProjectId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Employee_Id", EmployeeId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Assign_Date", AssignDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Assign_Time", Convert.ToDateTime(AssignTime).ToString(), PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Emp_Close_Date", EmpCloseDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Emp_Close_Time", Convert.ToDateTime(EmpCloseTime).ToString(), PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@TL_Close_Date", TlCloseDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@TL_Close_Time", Convert.ToDateTime(TlCloseTime).ToString(), PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@subject", Subject, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Description", Description, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@File_Id", FileId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Status", Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[25] = new PassDataToSql("@Task_Type", strTask_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@Task_Site_Address", strTask_Site_Address, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@Contact_Person", strContact_Person, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@Expected_Cost", strExpectedCost, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[29] = new PassDataToSql("@RequiredHours", requiredHours, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[30] = new PassDataToSql("@AssignBy", assignby, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Prj_Project_Task_Insert", paramList,ref trans);
        return Convert.ToInt32(paramList[24].ParaValue);
    }

    //here update record
    public int UpdateProjcetTask(string TaskId, string ProjectId, string EmployeeId, string AssignDate, string AssignTime, string EmpCloseDate, string EmpCloseTime, string TlCloseDate, string TlCloseTime, string Subject, string Description, string FileId, string Status, string duration, string strParentTask, string strPriority, string strtaskCompletion, string strModifiedBy, string strModifiedDate, string strTask_Type, string strTask_Site_Address, string strContact_Person, string IsBugs, string strExpectedCost, string ExpectedCost,string requiredHours,string assignby)
    {
        if (ExpectedCost == "")
        {
            ExpectedCost = "0";
        }

        PassDataToSql[] paramList = new PassDataToSql[28];
        paramList[0] = new PassDataToSql("@Task_Id", TaskId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Project_Id", ProjectId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Employee_Id", EmployeeId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Assign_Date", AssignDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Assign_Time", AssignTime, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Emp_Close_Date", EmpCloseDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Emp_Close_Time", EmpCloseTime, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@TL_Close_Date", TlCloseDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@TL_Close_Time", TlCloseTime, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@subject", Subject, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Description", Description, PassDataToSql.ParaTypeList.Text, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@File_Id", FileId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Status", Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[13] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[16] = new PassDataToSql("@Field1", duration, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Field3", strParentTask, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Field4", strPriority, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@Field5", strtaskCompletion, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@Task_Type", strTask_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@Task_Site_Address", strTask_Site_Address, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@Contact_Person", strContact_Person, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@Field6", IsBugs, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@Field7", strExpectedCost, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@Expected_Cost", ExpectedCost, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@requiredHours", requiredHours, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@AssignBy", assignby, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Prj_Project_Task_Update", paramList);
        return Convert.ToInt32(paramList[15].ParaValue);
    }



    public int UpdateProjcetTaskMailStatus(string TaskId, string MailStatus)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Task_Id", TaskId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Field2", MailStatus, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Prj_Project_Task_UpdateMailStatus", paramList);
        return Convert.ToInt32(paramList[2].ParaValue);
    }


    public int UpdateProjcetTaskStatus(string TaskId, string Status, string strActualHours, string strActualCost, string strModifiedBy, string strModifiedDate, string strTlClosedate, string TLCloseTime, string strCompletion, string ClosedRemarks, string Loss)
    {
        if (Loss == "")
        {
            Loss = "0";
        }

        if (strActualCost == "")
        {
            strActualCost = "0";
        }

        PassDataToSql[] paramList = new PassDataToSql[12];
        paramList[0] = new PassDataToSql("@Task_Id", TaskId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Status", Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[5] = new PassDataToSql("@Actual_Hours", strActualHours, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Actual_Cost", strActualCost, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@TL_Close_Date", strTlClosedate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@TL_Close_Time", TLCloseTime, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field5", strCompletion, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@closed_remarks", ClosedRemarks, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@loss", Loss, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Prj_Project_Task_UpdateStatus", paramList);
        return Convert.ToInt32(paramList[4].ParaValue);
    }
    // Here Delete record
    public int DeleteProjectTask(string TaskId, string strIsActive, string strModifiedBy, string strModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Task_Id", TaskId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Prj_Project_Task_RowStatus", paramList);
        return Convert.ToInt32(paramList[4].ParaValue);
    }

    // Get Record By Task Id 
    public DataTable GetRecordByTaskId(string TaskId)
    {

        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Task_Id", TaskId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Project_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Employee_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", "Assigned", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Prj_Project_Task_Select_Row", paramList))
        {
            return dtInfo;
        }

    }

    // Get all Record of project task  
    public DataTable GetAllRecord()
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Task_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Project_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Employee_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", "Assigned", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Prj_Project_Task_Select_Row", paramList))
        {
            return dtInfo;
        }
    }





    public DataTable GetRecordTaskVisibilityTrue(string Empid)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Task_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Project_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Employee_id", Empid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", "Assigned", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@optype", "6", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Prj_Project_Task_Select_Row", paramList))
        {
            return dtInfo;
        }
    }



    public DataTable GetRecordTaskFeedbackbyEmpId(string Empid,string taskId="0")
    {
        taskId = taskId == "" ? "0" : taskId;
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Task_Id", taskId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Project_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Employee_id", Empid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", "Assigned", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@optype", "11", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Prj_Project_Task_Select_Row", paramList))
        {
            return dtInfo;
        }
    }
    public DataTable GetRecordTaskFeedbackbyEmpId(string Empid,string projectId,string status,string taskId = "0")
    {
        taskId = taskId == "" ? "0" : taskId;
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Task_Id", taskId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Project_Id", projectId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Employee_id", Empid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@optype", "11", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Prj_Project_Task_Select_Row", paramList))
        {
            return dtInfo;
        }
    }

    public DataTable GetRecordTaskVisibilityTrueWithBugs(string Empid,string projectId="0")
    {
        projectId = projectId == "" ? "0" : projectId;
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Task_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Project_Id", projectId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Employee_id", Empid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", "Assigned", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@optype", "9", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Prj_Project_Task_Select_Row", paramList))
        {
            return dtInfo;
        }
    }

    public DataTable GetRecordTaskVisibilityTrueWithoutBugs(string Empid)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Task_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Project_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Employee_id", Empid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", "Assigned", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@optype", "10", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Prj_Project_Task_Select_Row", paramList))
        {
            return dtInfo;
        }
    }

    public DataTable GetRecordTaskVisibilityandProjectId(string Empid, string strProjectId)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Task_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Project_Id", strProjectId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Employee_id", Empid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", "Assigned", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@optype", "8", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Prj_Project_Task_Select_Row", paramList))
        {
            return dtInfo;
        }
    }

    public DataTable GetRecordTaskVisibilityandProjectId(string Empid, string strProjectId,ref SqlTransaction trans)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Task_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Project_Id", strProjectId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Employee_id", Empid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", "Assigned", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@optype", "8", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Prj_Project_Task_Select_Row", paramList,ref trans))
        {
            return dtInfo;
        }
    }

    // Get all Record by 
    public DataTable GetRecordByIdAssigned()
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Task_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Project_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Employee_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", "Assigned", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Prj_Project_Task_Select_Row", paramList))
        {
            return dtInfo;
        }
    }

    // Get all Record by status 
    public DataTable GetRecordBystatus(string status)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Task_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Project_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Employee_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@optype", "4", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Prj_Project_Task_Select_Row", paramList))
        {
            return dtInfo;
        }
    }

    public DataTable GetStatusByProjectId(string projectid)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Task_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Project_Id", projectid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Employee_id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", "Assigned", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@optype", "5", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Prj_Project_Task_Select_Row", paramList))
        {
            return dtInfo;
        }
    }
    public DataTable GetDataProjectId(string projectid, string EmployeeId = "0")
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Task_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Project_Id", projectid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Employee_id", EmployeeId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", "Assigned", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@optype", "7", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Prj_Project_Task_Select_Row", paramList))
        {
            return dtInfo;
        }
    }

    public int UpdateExtendedIdNStatus(string ParentTaskId, string task_id)
    {
        daClass.execute_Command("update Prj_Project_Task set Extended_Id = '" + ParentTaskId + "' where Task_Id ='" + task_id + "'");
        daClass.execute_Command("update Prj_Project_Task set Status = 'Extended' where Task_Id ='" + ParentTaskId + "'");
        return 1;
    }
    public int UpdateFromDt_ToDt_TimeByTaskId(string fromDt, string toDt, string requiredHours, string task_id)
    {
        daClass.execute_Command("update Prj_Project_Task set Assign_Date = '" + fromDt + "', Emp_Close_Date = '" + toDt + "' , requiredHours = '" + requiredHours + "' where Task_Id ='" + task_id + "'");
        return 1;
    }
    public DataTable GetRecordTaskVisibilityTrueWithoutBugs(string Empid, string projectId)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Task_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Project_Id", projectId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Employee_id", Empid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", "Assigned", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@optype", "12", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Prj_Project_Task_Select_Row", paramList))
        {
            return dtInfo;
        }
    }
    public DataTable GetBugDataForProjectId(string projectid)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Task_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Project_Id", projectid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Employee_id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@optype", "13", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Prj_Project_Task_Select_Row", paramList))
        {
            return dtInfo;
        }
    }
    public DataTable GetParentTaskVisibilityTrueWithoutBugs(string Empid, string projectId, string status = "", string TaskNotIn = "")
    {
        DataTable dtInfo = new DataTable();
        TaskNotIn = TaskNotIn != "" ? " and Prj_Project_Task.Task_Id<>'" + TaskNotIn + "'" : "";
        //AND Prj_Project_Task.IsActive ='True'
        if (status == "")
        {
            dtInfo = daClass.return_DataTable("SELECT distinct Prj_Project_Task.Task_Id, case when Prj.Subject<>'' then  Prj_Project_Task.Subject + '('+Prj.Subject+')' else  Prj_Project_Task.Subject end as Subject FROM Prj_Project_Task LEFT JOIN Prj_Project_Master ON Prj_Project_Master.Project_Id = Prj_Project_Task.Project_Id LEFT JOIN Prj_Project_Team ON Prj_Project_Team.Project_Id = Prj_Project_Master.Project_Id LEFT JOIN Ems_ContactMaster ON Prj_Project_Master.Customer_Id = Ems_ContactMaster.Trans_Id LEFT JOIN Set_EmployeeMaster ON Set_EmployeeMaster.Emp_Id = Prj_Project_Master.Project_Manager LEFT JOIN Prj_Project_Task AS Prj ON Prj.Task_Id = Prj_Project_Task.Field3 WHERE Prj_Project_Team.Task_Visibility = 'True' AND Prj_Project_Task.status <> 'Cancelled' AND Prj_Project_Task.Field6 IN (SELECT Prj_Project_TaskTypeMaster.Trans_Id FROM Prj_Project_TaskTypeMaster WHERE Prj_Project_TaskTypeMaster.Is_Bug = 'False') AND Prj_Project_Team.Emp_Id IN ('" + Empid + "') AND Prj_Project_Task.Project_Id = '" + projectId + "' " + TaskNotIn + " AND Prj_Project_Task.IsActive ='True' ");
        }
        else
        {
            dtInfo = daClass.return_DataTable("SELECT distinct Prj_Project_Task.Task_Id, case when Prj.Subject<>'' then  Prj_Project_Task.Subject + '('+Prj.Subject+')' else  Prj_Project_Task.Subject end as Subject FROM Prj_Project_Task LEFT JOIN Prj_Project_Master ON Prj_Project_Master.Project_Id = Prj_Project_Task.Project_Id LEFT JOIN Prj_Project_Team ON Prj_Project_Team.Project_Id = Prj_Project_Master.Project_Id LEFT JOIN Ems_ContactMaster ON Prj_Project_Master.Customer_Id = Ems_ContactMaster.Trans_Id LEFT JOIN Set_EmployeeMaster ON Set_EmployeeMaster.Emp_Id = Prj_Project_Master.Project_Manager LEFT JOIN Prj_Project_Task AS Prj ON Prj.Task_Id = Prj_Project_Task.Field3 WHERE Prj_Project_Team.Task_Visibility = 'True' AND Prj_Project_Task.status <> 'Cancelled' AND Prj_Project_Task.Field6 IN (SELECT Prj_Project_TaskTypeMaster.Trans_Id FROM Prj_Project_TaskTypeMaster WHERE Prj_Project_TaskTypeMaster.Is_Bug = 'False') AND Prj_Project_Team.Emp_Id IN ('" + Empid + "') AND Prj_Project_Task.IsActive ='True' AND Prj_Project_Task.Project_Id = '" + projectId + "' and Prj_Project_Task.Status in ('" + status + "') " + TaskNotIn + " ");
        }

        return dtInfo;
    }

    public DataTable GetTaskTreeData(string Empid, string projectId = "")
    {
        DataTable dtInfo = new DataTable();
        // AND Prj_Project_Task.IsActive ='True'
        if (projectId != "")
        {
            dtInfo = daClass.return_DataTable("SELECT Prj_Project_Master.field7, Prj_Project_Master.Project_Id, Prj_Project_Task.Task_Id, CASE WHEN Prj_Project_Task.Assign_Date <> '1900-01-01 00:00:00.000' THEN CONVERT(varchar(11), Prj_Project_Task.Assign_Date, 106) + ' ' + CONVERT(varchar(8), Prj_Project_Task.Assign_Time, 8) ELSE ' ' END AS AssignDate1, Prj_Project_Task.Subject, Prj_Project_Task.Status, Prj_Project_Task.Task_Type, Prj_Project_Task.Field5 as Field51, (SELECT STUFF((SELECT DISTINCT ',' + RTRIM(Set_EmployeeMaster.Emp_Name) FROM Set_EmployeeMaster WHERE Set_EmployeeMaster.Emp_Id IN ((SELECT Prj_Project_Task_Employeee.Employee_Id FROM Prj_Project_Task_Employeee WHERE Prj_Project_Task_Employeee.Ref_Id = Prj_Project_Task.Task_Id AND Prj_Project_Task_Employeee.Ref_Type = 'Task')) FOR xml PATH ('')), 1, 1, '')) AS AssignTo, CAST(Prj_Project_Task.Field3 AS int) AS ParentTaskId,assign.Emp_Name as assignBy, Prj.Subject AS parenttaskSubject FROM Prj_Project_Task LEFT JOIN set_employeemaster assign ON assign.Emp_Id = Prj_Project_Task.AssignBy LEFT JOIN Prj_Project_Master ON Prj_Project_Master.Project_Id = Prj_Project_Task.Project_Id LEFT JOIN Prj_Project_Team ON Prj_Project_Team.Project_Id = Prj_Project_Master.Project_Id LEFT JOIN Ems_ContactMaster ON Prj_Project_Master.Customer_Id = Ems_ContactMaster.Trans_Id LEFT JOIN Set_EmployeeMaster ON Set_EmployeeMaster.Emp_Id = Prj_Project_Master.Project_Manager LEFT JOIN Prj_Project_Task AS Prj ON Prj.Task_Id = Prj_Project_Task.Field3 WHERE Prj_Project_Team.Task_Visibility = 'True' AND Prj_Project_Team.Emp_Id IN ('" + Empid + "') AND Prj_Project_Task.status <> 'Cancelled' AND Prj_Project_Task.Field6 IN (SELECT Prj_Project_TaskTypeMaster.Trans_Id FROM Prj_Project_TaskTypeMaster WHERE Prj_Project_TaskTypeMaster.Is_Bug = 'False') AND Prj_Project_Task.Project_Id = '" + projectId + "'  AND Prj_Project_Task.IsActive ='True' ORDER BY Prj_Project_Task.Assign_Date, Prj_Project_Task.Assign_Time,Prj_Project_Task.Task_Id  ");
        }
        else
        {
            dtInfo = daClass.return_DataTable("SELECT Prj_Project_Master.field7,Prj_Project_Master.Project_Id, Prj_Project_Task.Task_Id, CASE WHEN Prj_Project_Task.Assign_Date <> '1900-01-01 00:00:00.000' THEN CONVERT(varchar(11), Prj_Project_Task.Assign_Date, 106) + ' ' + CONVERT(varchar(8), Prj_Project_Task.Assign_Time, 8) ELSE ' ' END AS AssignDate1, Prj_Project_Task.Subject, Prj_Project_Task.Status, Prj_Project_Task.Task_Type, Prj_Project_Task.Field5 as Field51, (SELECT STUFF((SELECT DISTINCT ',' + RTRIM(Set_EmployeeMaster.Emp_Name) FROM Set_EmployeeMaster WHERE Set_EmployeeMaster.Emp_Id IN ((SELECT Prj_Project_Task_Employeee.Employee_Id FROM Prj_Project_Task_Employeee WHERE Prj_Project_Task_Employeee.Ref_Id = Prj_Project_Task.Task_Id AND Prj_Project_Task_Employeee.Ref_Type = 'Task')) FOR xml PATH ('')), 1, 1, '')) AS AssignTo, CAST(Prj_Project_Task.Field3 AS int) AS ParentTaskId,assign.Emp_Name as assignBy, Prj.Subject AS parenttaskSubject FROM Prj_Project_Task LEFT JOIN set_employeemaster assign ON assign.Emp_Id = Prj_Project_Task.AssignBy LEFT JOIN Prj_Project_Master ON Prj_Project_Master.Project_Id = Prj_Project_Task.Project_Id LEFT JOIN Prj_Project_Team ON Prj_Project_Team.Project_Id = Prj_Project_Master.Project_Id LEFT JOIN Ems_ContactMaster ON Prj_Project_Master.Customer_Id = Ems_ContactMaster.Trans_Id LEFT JOIN Set_EmployeeMaster ON Set_EmployeeMaster.Emp_Id = Prj_Project_Master.Project_Manager LEFT JOIN Prj_Project_Task AS Prj ON Prj.Task_Id = Prj_Project_Task.Field3 WHERE Prj_Project_Team.Task_Visibility = 'True' AND Prj_Project_Team.Emp_Id IN ('" + Empid + "') AND Prj_Project_Task.status <> 'Cancelled' AND Prj_Project_Task.Field6 IN (SELECT Prj_Project_TaskTypeMaster.Trans_Id  AND Prj_Project_Task.IsActive ='True' FROM Prj_Project_TaskTypeMaster WHERE Prj_Project_TaskTypeMaster.Is_Bug = 'False')  ORDER BY Prj_Project_Task.Assign_Date, Prj_Project_Task.Assign_Time,Prj_Project_Task.Task_Id");
        }
        return dtInfo;
    }
    public DataTable GetAllWorkingTask(string userId)
    {
        using (DataTable dtInfo = daClass.return_DataTable("select * from (select Prj_Project_Task.Project_Id, sum(case when Prj_Project_Feedback.Field2 = '' and Prj_Project_Feedback.Field3 = '' or Prj_Project_Feedback.Field2 <> '' and Prj_Project_Feedback.Field3 <> '' then 0 when(Prj_Project_Feedback.Field2 <> '' or Prj_Project_Feedback.Field3 Is not NULL) and(Prj_Project_Feedback.Field3 = '' or Prj_Project_Feedback.Field3 Is NULL) then 1 else 0 end) as workStatus  from Prj_Project_Feedback inner join Prj_Project_Task on Prj_Project_Task.Task_Id = Prj_Project_Feedback.task_id where Prj_Project_Feedback.createdby = '"+ userId + "' group by Prj_Project_Task.Project_Id) data where data.workStatus <> 0"))
        {
            return dtInfo;
        }
    }
    public DataTable GetAllWorkingEmployeeTasks(string ProjectId="")
    {
        if(ProjectId=="")
        {
            using (DataTable dtInfo = daClass.return_DataTable("SELECT * FROM (SELECT prj_project_master.project_name, CAST((CASE WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, STUFF(CAST( REPLACE(CONVERT(varchar(11), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 106) + ' ' + CONVERT(varchar(8), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 8),' ','-') as varchar(17)), 12, 1, ' ')) AS int) ELSE CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, Prj_Project_Feedback.Field3) AS int) END) / 60 AS varchar(10)) + ':' + CASE WHEN LEN(CAST((CASE WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, STUFF(CAST( REPLACE(CONVERT(varchar(11), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 106) + ' ' + CONVERT(varchar(8), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 8),' ','-') as varchar(17)), 12, 1, ' ')) AS int) ELSE CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, Prj_Project_Feedback.Field3) AS int) END) % 60 AS varchar(10))) = 1 THEN '0' + CAST((CASE WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, STUFF(CAST( REPLACE(CONVERT(varchar(11), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 106) + ' ' + CONVERT(varchar(8), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 8),' ','-') as varchar(17)), 12, 1, ' ')) AS int) ELSE CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, Prj_Project_Feedback.Field3) AS int) END) % 60 AS varchar(10)) ELSE CAST((CASE WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, STUFF(CAST( REPLACE(CONVERT(varchar(11), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 106) + ' ' + CONVERT(varchar(8), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 8),' ','-') as varchar(17)), 12, 1, ' ')) AS int) ELSE CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, Prj_Project_Feedback.Field3) AS int) END) % 60 AS varchar(10)) END AS timediff, Prj_Project_Task.Project_Id, Prj_Project_Task.Subject, Set_EmployeeMaster.Emp_Name,Prj_Project_Feedback.field4 as timeZoneID, Prj_Project_Feedback.field2, Prj_Project_Feedback.field3, dbo.StripHTML(Prj_Project_Feedback.description) AS description, CASE WHEN (Prj_Project_Feedback.Field2 = '' AND Prj_Project_Feedback.Field3 = '') OR (Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 <> '') THEN 'Start' WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN 'Stop' ELSE 'Start' END AS workStatus, dbo.getProjectCost(Prj_Project_Task.Project_Id, Prj_Project_Task.task_id) AS ActualCost, Prj_Project_Task.Expected_Cost, Prj_Project_Task.task_id, CASE WHEN Prj_Project_Task.requiredhours IS NULL THEN '0:00' ELSE Prj_Project_Task.requiredhours END AS requiredhours FROM Prj_Project_Feedback LEFT JOIN Prj_Project_Task ON Prj_Project_Task.Task_Id = Prj_Project_Feedback.Task_Id LEFT JOIN Set_UserMaster ON Set_UserMaster.User_Id = Prj_Project_Feedback.CreatedBy LEFT JOIN Set_EmployeeMaster ON Set_EmployeeMaster.Emp_Id = Set_UserMaster.Emp_Id INNER JOIN prj_project_master ON prj_project_master.Project_Id = Prj_Project_Task.Project_Id) data WHERE data.workstatus = 'stop'"))
            {
                return dtInfo;
            }
        }
        else
        {
            using (DataTable dtInfo = daClass.return_DataTable("SELECT * FROM (SELECT prj_project_master.project_name, CAST((CASE WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, STUFF(CAST( REPLACE(CONVERT(varchar(11), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 106) + ' ' + CONVERT(varchar(8), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 8),' ','-') as varchar(17)), 12, 1, ' ')) AS int) ELSE CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, Prj_Project_Feedback.Field3) AS int) END) / 60 AS varchar(10)) + ':' + CASE WHEN LEN(CAST((CASE WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, STUFF(CAST( REPLACE(CONVERT(varchar(11), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 106) + ' ' + CONVERT(varchar(8), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 8),' ','-') as varchar(17)), 12, 1, ' ')) AS int) ELSE CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, Prj_Project_Feedback.Field3) AS int) END) % 60 AS varchar(10))) = 1 THEN '0' + CAST((CASE WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, STUFF(CAST( REPLACE(CONVERT(varchar(11), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 106) + ' ' + CONVERT(varchar(8), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 8),' ','-') as varchar(17)), 12, 1, ' ')) AS int) ELSE CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, Prj_Project_Feedback.Field3) AS int) END) % 60 AS varchar(10)) ELSE CAST((CASE WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, STUFF(CAST( REPLACE(CONVERT(varchar(11), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 106) + ' ' + CONVERT(varchar(8), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 8),' ','-') as varchar(17)), 12, 1, ' ')) AS int) ELSE CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, Prj_Project_Feedback.Field3) AS int) END) % 60 AS varchar(10)) END AS timediff, Prj_Project_Task.Project_Id, Prj_Project_Task.Subject, Set_EmployeeMaster.Emp_Name,Prj_Project_Feedback.field4 as timeZoneID, Prj_Project_Feedback.field2, Prj_Project_Feedback.field3, dbo.StripHTML(Prj_Project_Feedback.description) AS description, CASE WHEN (Prj_Project_Feedback.Field2 = '' AND Prj_Project_Feedback.Field3 = '') OR (Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 <> '') THEN 'Start' WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN 'Stop' ELSE 'Start' END AS workStatus, dbo.getProjectCost(Prj_Project_Task.Project_Id, Prj_Project_Task.task_id) AS ActualCost, Prj_Project_Task.Expected_Cost, Prj_Project_Task.task_id, CASE WHEN Prj_Project_Task.requiredhours IS NULL THEN '0:00' ELSE Prj_Project_Task.requiredhours END AS requiredhours FROM Prj_Project_Feedback LEFT JOIN Prj_Project_Task ON Prj_Project_Task.Task_Id = Prj_Project_Feedback.Task_Id LEFT JOIN Set_UserMaster ON Set_UserMaster.User_Id = Prj_Project_Feedback.CreatedBy LEFT JOIN Set_EmployeeMaster ON Set_EmployeeMaster.Emp_Id = Set_UserMaster.Emp_Id INNER JOIN prj_project_master ON prj_project_master.Project_Id = Prj_Project_Task.Project_Id) data WHERE data.project_id IN (SELECT CAST(Value AS int) AS location FROM F_Split('" + ProjectId+"', ',')) AND data.workstatus = 'stop'"))
            {
                return dtInfo;
            }
        }        
    }
    public string CheckChildTaskClosedOrNot(string parentProjectId, string projectId,string EmpId)
    {
        string count = "";
        count = daClass.get_SingleValue("SELECT COUNT(*) FROM Prj_Project_Task LEFT OUTER JOIN Arc_File_Transaction ON Prj_Project_Task.File_Id = Arc_File_Transaction.Trans_Id LEFT OUTER JOIN Prj_Project_Team RIGHT OUTER JOIN Prj_Project_Master ON Prj_Project_Team.Project_Id = Prj_Project_Master.Project_Id ON Prj_Project_Task.Project_Id = Prj_Project_Master.Project_Id LEFT OUTER JOIN Ems_ContactMaster ON Prj_Project_Master.Customer_Id = Ems_ContactMaster.Trans_Id LEFT JOIN Set_EmployeeMaster ON Set_EmployeeMaster.Emp_Id = Prj_Project_Master.Project_Manager LEFT JOIN Prj_Project_Task AS Prj ON Prj.Task_Id = Prj_Project_Task.Field3 WHERE (Prj_Project_Team.Task_Visibility = 'True') and Prj_Project_Task.Field3 ='"+parentProjectId+"' and Prj_Project_Task.Status in ('Assigned','ReAssigned') and Prj_Project_Task.Project_id='"+projectId+"' AND Prj_Project_Team.Emp_Id IN ('"+ EmpId + "') AND Prj_Project_Task.Field6 IN (SELECT Prj_Project_TaskTypeMaster.Trans_Id FROM Prj_Project_TaskTypeMaster WHERE Prj_Project_TaskTypeMaster.Is_Bug = 'False')");
        count = count == "@NOTFOUND@" ? "0" : count;
        return count;
    }
    public string getProjectCostByProjectId(string projectId, string task_id)
    {
        string taskCost = "0.00";
        taskCost = daClass.get_SingleValue("SELECT SUM(projectCost.projectCost) FROM (SELECT cost.project_id, cost.task_id, (Basic_Salary / DATEDIFF(DAY, DATEADD(DAY, 1 - DAY(GETDATE()), GETDATE()), DATEADD(MONTH, 1, DATEADD(DAY, 1 - DAY(GETDATE()), GETDATE()))) / Assign_Min) * cost.totalMins AS projectCost FROM Set_Employee_Parameter INNER JOIN ( SELECT Prj_Project_Task.task_id, SUM( CASE WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, GETDATE()) ELSE DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, Prj_Project_Feedback.Field3) END ) AS totalMins, Prj_Project_Task.Project_Id, Set_EmployeeMaster.Emp_id FROM Prj_Project_Feedback LEFT JOIN Prj_Project_Task ON Prj_Project_Task.Task_Id = Prj_Project_Feedback.Task_Id LEFT JOIN Set_UserMaster ON Set_UserMaster.User_Id = Prj_Project_Feedback.CreatedBy LEFT JOIN Set_EmployeeMaster ON Set_EmployeeMaster.Emp_Id = Set_UserMaster.Emp_Id LEFT JOIN prj_project_master ON prj_project_master.Project_Id = Prj_Project_Task.Project_Id where Prj_Project_Feedback.Field2 <> '' GROUP BY Set_EmployeeMaster.Emp_id,Prj_Project_Task.Project_Id,Prj_Project_Task.Task_Id ) cost ON cost.emp_id = Set_Employee_Parameter.emp_id WHERE cost.Project_Id = '" + projectId + "' AND cost.Task_Id = '" + task_id + "') projectCost GROUP BY projectCost.Task_Id,projectCost.Project_Id");
        taskCost = taskCost == "@NOTFOUND@" ? "0.00" : taskCost;
        return taskCost;
    }
    public string getTaskStatusById(string task_id)
    {
        string status = "";
        status = daClass.get_SingleValue("select top 1 status from prj_project_task where task_id ='"+task_id+"' and isactive='true'");
        status = status == "@NOTFOUND@" ? "" : status;
        return status;
    }
    public DataTable GetCurrentWorkingTask(string Empid, string projectId, string status)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Task_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Project_Id", projectId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Employee_id", Empid, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@optype", "14", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Prj_Project_Task_Select_Row", paramList))
        {
            return dtInfo;
        }
    }

    public string getActualHours(string task_id)
    {
        string hr = "";
        hr = daClass.get_SingleValue("SELECT CAST(CAST(SUM(data.timediff) / 60 AS int) AS varchar(10)) + ':' + case when len(CAST(CAST(SUM(data.timediff) % 60 AS int) AS varchar(10)))=1 then '0'+CAST(CAST(SUM(data.timediff) % 60 AS int) AS varchar(10)) else CAST(CAST(SUM(data.timediff) % 60 AS int) AS varchar(10)) end AS totalWorkingHr FROM (SELECT Prj_Project_Feedback.Task_Id, SUM(CASE WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, GETDATE()) AS decimal(18, 2)) ELSE CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, Prj_Project_Feedback.Field3) AS decimal(18, 2)) END) AS timediff FROM Prj_Project_Feedback GROUP BY Prj_Project_Feedback.Task_Id, Prj_Project_Feedback.Field2, Prj_Project_Feedback.field3) data where data.task_id='"+task_id+"' GROUP BY data.task_id");
        hr = hr == "@NOTFOUND@" ? "" : hr;
        return hr;
    }
    public string getActualHours(string task_id,ref SqlTransaction trans)
    {
        string hr = "";
        hr = daClass.get_SingleValue("SELECT CAST(CAST(SUM(data.timediff) / 60 AS int) AS varchar(10)) + ':' + case when len(CAST(CAST(SUM(data.timediff) % 60 AS int) AS varchar(10)))=1 then '0'+CAST(CAST(SUM(data.timediff) % 60 AS int) AS varchar(10)) else CAST(CAST(SUM(data.timediff) % 60 AS int) AS varchar(10)) end AS totalWorkingHr FROM (SELECT Prj_Project_Feedback.Task_Id, SUM(CASE WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, GETDATE()) AS decimal(18, 2)) ELSE CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, Prj_Project_Feedback.Field3) AS decimal(18, 2)) END) AS timediff FROM Prj_Project_Feedback GROUP BY Prj_Project_Feedback.Task_Id, Prj_Project_Feedback.Field2, Prj_Project_Feedback.field3) data where data.task_id='" + task_id + "' GROUP BY data.task_id", ref trans);
        hr = hr == "@NOTFOUND@" ? "" : hr;
        return hr;
    }
    public class ProjectBug
    {
        public string projectId { get; set; }
        public string bugName { get; set; }
        public string description { get; set; }
        public string priority { get; set; }
        public string assignedBy { get; set; }
    }
}
