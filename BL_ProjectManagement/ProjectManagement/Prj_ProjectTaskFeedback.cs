using System;
using System.Data;
using PegasusDataAccess;
using System.Data.SqlClient;
/// <summary>
/// Summary description for Prj_ProjectTaskFeedback
/// </summary>
public class Prj_ProjectTaskFeedback
{
    public DataAccessClass daClass = null;
    public Prj_ProjectTaskFeedback(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
    }


    // Insert record into project Task Feedback
    public int InsertProjectFeedback(string CompanyId, string Taskid, string Descrption, string Datetime, string Commentby, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[18];
        paramList[0] = new PassDataToSql("@Compnay_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Task_Id", Taskid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Description", Descrption, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@DateTime", Datetime, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@CommentBy", Commentby, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[5] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);


        paramList[12] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Prj_Project_Feedback_Insert", paramList);
        return Convert.ToInt32(paramList[17].ParaValue);
    }

    //here update record
    public int UpdateProjcetFeedback(string transid, string CompanyId, string Taskid, string Descrption, string Datetime, string Commentby, string strModifiedBy, string strModifiedDate, string closeTime)
    {
        PassDataToSql[] paramList = new PassDataToSql[10];
        paramList[0] = new PassDataToSql("@Trans_Id", transid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Compnay_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Task_Id", Taskid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Description", Descrption, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@DateTime", Datetime, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@CommentBy", Commentby, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[6] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[9] = new PassDataToSql("@Field3", closeTime, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Prj_Project_Feedback_update", paramList);
        return Convert.ToInt32(paramList[8].ParaValue);
    }
    public int UpdateProjcetFeedback(string transid, string CompanyId, string Taskid, string Descrption, string Datetime, string Commentby, string strModifiedBy, string strModifiedDate, string closeTime,ref SqlTransaction trans)
    {
        PassDataToSql[] paramList = new PassDataToSql[10];
        paramList[0] = new PassDataToSql("@Trans_Id", transid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Compnay_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Task_Id", Taskid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Description", Descrption, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@DateTime", Datetime, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@CommentBy", Commentby, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[6] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[9] = new PassDataToSql("@Field3", closeTime, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Prj_Project_Feedback_update", paramList,ref trans);
        return Convert.ToInt32(paramList[8].ParaValue);
    }

    public int UpdateProjcetFeedback(string transid, string CompanyId, string Taskid, string Descrption, string Datetime, string Commentby, string strModifiedBy, string strModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[9];
        paramList[0] = new PassDataToSql("@Trans_Id", transid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Compnay_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Task_Id", Taskid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Description", Descrption, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@DateTime", Datetime, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@CommentBy", Commentby, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[6] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);        
        daClass.execute_Sp("sp_Prj_Project_Feedback_update", paramList);
        return Convert.ToInt32(paramList[8].ParaValue);
    }


    // Get Record By Trans Id
    public DataTable GetRecordByTransId(string TransId)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Trans_Id", TransId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Task_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Prj_Project_Feedback_Select_Row", paramList))
        {
            return dtInfo;
        }
    }
    // Get Record By Company Id
    public DataTable GetRecordByCompanyId(string CompanyId)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Compnay_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Task_Id", "", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Prj_Project_Feedback_Select_Row", paramList))
        {
            return dtInfo;
        }
    }

    // Get Record By Task Id
    public DataTable GetRecordByTaskId(string TaskId)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Task_Id", TaskId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Prj_Project_Feedback_Select_Row", paramList))
        {
            return dtInfo;
        }
    }

    // Get All Record 
    public DataTable GetAllRecord()
    {
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Trans_Id", "", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Compnay_Id", "", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Task_Id", "", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "4", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Prj_Project_Feedback_Select_Row", paramList))
        {
            return dtInfo;
        }
    }

    // Here Delete Record
    public int DeleteProjectFeedBack(string TransId, string strIsActive, string strModifiedBy, string strModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Trans_Id", TransId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Prj_Project_Feedback_RowStatus", paramList);
        return Convert.ToInt32(paramList[4].ParaValue);
    }

    public DataTable getTaskHistoryData(string date,string projectId, string empCode = "")
    {
        if (empCode == "")
        {
            using (DataTable dt = daClass.return_DataTable("SELECT DISTINCT prj_project_feedback.field4 AS timeZoneID,prj_project_feedback.trans_id AS feedbackId, prj_project_feedback.Field2, prj_project_feedback.Field3, prj_project_feedback.datetime, prj_project_task.Task_Id, prj_project_task.Subject, Prj_Project_Master.Project_Id, Prj_Project_Master.Project_Name, CASE WHEN prj_project_task.expected_cost IS NULL THEN '0.00' ELSE prj_project_task.expected_cost END AS expected_cost, dbo.getProjectCost(Prj_Project_Master.Project_Id, prj_project_task.Task_Id) AS actualCost, CASE WHEN prj_project_task.RequiredHours IS NULL OR prj_project_task.RequiredHours = '' THEN '0:00' ELSE prj_project_task.RequiredHours END AS RequiredHours, timeData.totalWorkingHr, set_employeemaster.Emp_Name FROM prj_project_feedback INNER JOIN prj_project_task ON prj_project_task.task_id = prj_project_feedback.task_id INNER JOIN Prj_Project_Master ON Prj_Project_Master.Project_Id = prj_project_task.Project_Id INNER JOIN Set_UserMaster ON Set_UserMaster.user_id = prj_project_feedback.createdby INNER JOIN Set_EmployeeMaster ON Set_EmployeeMaster.Emp_Id = Set_UserMaster.Emp_Id INNER JOIN (SELECT data.Task_Id, data.createdby, CAST(CAST(SUM(data.timediff) / 60 AS int) AS varchar(10)) + ':' + CASE WHEN LEN(CAST(CAST(SUM(data.timediff) % 60 AS int) AS varchar(10))) = 1 THEN '0' + CAST(CAST(SUM(data.timediff) % 60 AS int) AS varchar(10)) ELSE CAST(CAST(SUM(data.timediff) % 60 AS int) AS varchar(10)) END AS totalWorkingHr FROM (SELECT Prj_Project_Feedback.Task_Id, Prj_Project_Feedback.createdby, SUM(CASE WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, STUFF(CAST( REPLACE(CONVERT(varchar(11), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 106) + ' ' + CONVERT(varchar(8), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 8),' ','-') as varchar(17)), 12, 1, ' ')) AS decimal(18, 2)) ELSE CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, Prj_Project_Feedback.Field3) AS decimal(18, 2)) END) AS timediff FROM Prj_Project_Feedback WHERE Prj_Project_Feedback.isactive = 'true' GROUP BY Prj_Project_Feedback.Task_Id, Prj_Project_Feedback.createdby, Prj_Project_Feedback.Field2, Prj_Project_Feedback.field3) data GROUP BY data.task_id, data.createdby) timeData ON timeData.Task_Id = prj_project_task.task_id AND timeData.CreatedBy = prj_project_feedback.createdby WHERE prj_project_feedback.IsActive = 'true' AND prj_project_feedback.DateTime IS NOT NULL and Prj_Project_Master.project_id in (" + projectId + ") and prj_project_feedback.DateTime='" + date + "'"))
                return dt;
        }
        else
        {
            using (DataTable dt = daClass.return_DataTable("SELECT DISTINCT prj_project_feedback.field4 AS timeZoneID,prj_project_feedback.trans_id AS feedbackId, prj_project_feedback.Field2, prj_project_feedback.Field3, prj_project_feedback.datetime, prj_project_task.Task_Id, prj_project_task.Subject, Prj_Project_Master.Project_Id, Prj_Project_Master.Project_Name, CASE WHEN prj_project_task.expected_cost IS NULL THEN '0.00' ELSE prj_project_task.expected_cost END AS expected_cost, dbo.getProjectCost(Prj_Project_Master.Project_Id, prj_project_task.Task_Id) AS actualCost, CASE WHEN prj_project_task.RequiredHours IS NULL OR prj_project_task.RequiredHours = '' THEN '0:00' ELSE prj_project_task.RequiredHours END AS RequiredHours, timeData.totalWorkingHr, set_employeemaster.Emp_Name FROM prj_project_feedback INNER JOIN prj_project_task ON prj_project_task.task_id = prj_project_feedback.task_id INNER JOIN Prj_Project_Master ON Prj_Project_Master.Project_Id = prj_project_task.Project_Id INNER JOIN Set_UserMaster ON Set_UserMaster.user_id = prj_project_feedback.createdby INNER JOIN Set_EmployeeMaster ON Set_EmployeeMaster.Emp_Id = Set_UserMaster.Emp_Id INNER JOIN (SELECT data.Task_Id, data.createdby, CAST(CAST(SUM(data.timediff) / 60 AS int) AS varchar(10)) + ':' + CASE WHEN LEN(CAST(CAST(SUM(data.timediff) % 60 AS int) AS varchar(10))) = 1 THEN '0' + CAST(CAST(SUM(data.timediff) % 60 AS int) AS varchar(10)) ELSE CAST(CAST(SUM(data.timediff) % 60 AS int) AS varchar(10)) END AS totalWorkingHr FROM (SELECT Prj_Project_Feedback.Task_Id, Prj_Project_Feedback.createdby, SUM(CASE WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, STUFF(CAST( REPLACE(CONVERT(varchar(11), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 106) + ' ' + CONVERT(varchar(8), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 8),' ','-') as varchar(17)), 12, 1, ' ')) AS decimal(18, 2)) ELSE CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, Prj_Project_Feedback.Field3) AS decimal(18, 2)) END) AS timediff FROM Prj_Project_Feedback WHERE Prj_Project_Feedback.isactive = 'true' GROUP BY Prj_Project_Feedback.Task_Id, Prj_Project_Feedback.createdby, Prj_Project_Feedback.Field2, Prj_Project_Feedback.field3) data GROUP BY data.task_id, data.createdby) timeData ON timeData.Task_Id = prj_project_task.task_id AND timeData.CreatedBy = prj_project_feedback.createdby WHERE prj_project_feedback.IsActive = 'true' AND prj_project_feedback.DateTime IS NOT NULL and  Prj_Project_Master.project_id in (" + projectId + ") and prj_project_feedback.DateTime='" + date + "' and emp_code='" + empCode + "'"))
                return dt;
        }
    }

    public DataTable getTaskFeedbackHistory(string date, string taskId, string empCode="")
    {
        if(empCode=="")
        {
            using (DataTable dt = daClass.return_DataTable("SELECT prj_project_feedback.field4 as timeZoneID, prj_project_feedback.Description, prj_project_feedback.DateTime, prj_project_feedback.Field2, prj_project_feedback.field3, Set_EmployeeMaster.emp_name, Set_EmployeeMaster.emp_code, prj_project_task.Subject, Prj_Project_Master.Project_Name, prj_project_task.task_id, CAST(CAST((CASE WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, STUFF(CAST( REPLACE(CONVERT(varchar(11), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 106) + ' ' + CONVERT(varchar(8), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 8),' ','-') as varchar(17)), 12, 1, ' ')) AS decimal(18, 2)) ELSE CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, Prj_Project_Feedback.Field3) AS decimal(18, 2)) END) / 60 AS int) AS varchar(10)) + ':' + CASE WHEN LEN(CAST(CAST((CASE WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, STUFF(CAST( REPLACE(CONVERT(varchar(11), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 106) + ' ' + CONVERT(varchar(8), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 8),' ','-') as varchar(17)), 12, 1, ' ')) AS decimal(18, 2)) ELSE CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, Prj_Project_Feedback.Field3) AS decimal(18, 2)) END) % 60 AS int) AS varchar(10))) = 1 THEN '0' + CAST(CAST((CASE WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, STUFF(CAST( REPLACE(CONVERT(varchar(11), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 106) + ' ' + CONVERT(varchar(8), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 8),' ','-') as varchar(17)), 12, 1, ' ')) AS decimal(18, 2)) ELSE CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, Prj_Project_Feedback.Field3) AS decimal(18, 2)) END) % 60 AS int) AS varchar(10)) ELSE CAST(CAST((CASE WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, STUFF(CAST( REPLACE(CONVERT(varchar(11), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 106) + ' ' + CONVERT(varchar(8), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 8),' ','-') as varchar(17)), 12, 1, ' ')) AS decimal(18, 2)) ELSE CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, Prj_Project_Feedback.Field3) AS decimal(18, 2)) END) % 60 AS int) AS varchar(10)) END AS timediff FROM prj_project_feedback INNER JOIN prj_project_task ON prj_project_task.Task_Id = prj_project_feedback.Task_Id INNER JOIN Prj_Project_Master ON Prj_Project_Master.Project_Id = prj_project_task.project_id INNER JOIN Set_EmployeeMaster ON Set_EmployeeMaster.Emp_Id = prj_project_feedback.CommentBy where prj_project_feedback.DateTime='" + date + "' and prj_project_task.task_id='" + taskId + "' order by trans_id desc"))
                return dt;
        }
        else
        {
            using (DataTable dt = daClass.return_DataTable("SELECT prj_project_feedback.field4 as timeZoneID,prj_project_feedback.Description, prj_project_feedback.DateTime, prj_project_feedback.Field2, prj_project_feedback.field3, Set_EmployeeMaster.emp_name, Set_EmployeeMaster.emp_code, prj_project_task.Subject, Prj_Project_Master.Project_Name, prj_project_task.task_id, CAST(CAST((CASE WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, STUFF(CAST( REPLACE(CONVERT(varchar(11), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 106) + ' ' + CONVERT(varchar(8), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 8),' ','-') as varchar(17)), 12, 1, ' ')) AS decimal(18, 2)) ELSE CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, Prj_Project_Feedback.Field3) AS decimal(18, 2)) END) / 60 AS int) AS varchar(10)) + ':' + CASE WHEN LEN(CAST(CAST((CASE WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, STUFF(CAST( REPLACE(CONVERT(varchar(11), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 106) + ' ' + CONVERT(varchar(8), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 8),' ','-') as varchar(17)), 12, 1, ' ')) AS decimal(18, 2)) ELSE CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, Prj_Project_Feedback.Field3) AS decimal(18, 2)) END) % 60 AS int) AS varchar(10))) = 1 THEN '0' + CAST(CAST((CASE WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, STUFF(CAST( REPLACE(CONVERT(varchar(11), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 106) + ' ' + CONVERT(varchar(8), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 8),' ','-') as varchar(17)), 12, 1, ' ')) AS decimal(18, 2)) ELSE CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, Prj_Project_Feedback.Field3) AS decimal(18, 2)) END) % 60 AS int) AS varchar(10)) ELSE CAST(CAST((CASE WHEN Prj_Project_Feedback.Field2 <> '' AND Prj_Project_Feedback.Field3 = '' THEN CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, STUFF(CAST( REPLACE(CONVERT(varchar(11), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 106) + ' ' + CONVERT(varchar(8), DATEADD(second, DATEDIFF(second, GETDATE(), GETUTCDATE()), getdate()), 8),' ','-') as varchar(17)), 12, 1, ' ')) AS decimal(18, 2)) ELSE CAST(DATEDIFF(MINUTE, Prj_Project_Feedback.Field2, Prj_Project_Feedback.Field3) AS decimal(18, 2)) END) % 60 AS int) AS varchar(10)) END AS timediff FROM prj_project_feedback INNER JOIN prj_project_task ON prj_project_task.Task_Id = prj_project_feedback.Task_Id INNER JOIN Prj_Project_Master ON Prj_Project_Master.Project_Id = prj_project_task.project_id INNER JOIN Set_EmployeeMaster ON Set_EmployeeMaster.Emp_Id = prj_project_feedback.CommentBy where prj_project_feedback.DateTime='" + date + "' and prj_project_task.task_id='" + taskId + "' and Set_EmployeeMaster.Emp_Code='" + empCode + "' order by trans_id desc")) 
                return dt;
        }
    }
    public DataTable getTaskFeedbackList(string strPageIndex, string strPageSize, string strWhereClause,string employeeId)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@PageIndex", strPageIndex, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@PageSize", strPageSize, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@employee_id", employeeId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@WhereClause", strWhereClause, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("Sp_prj_project_feedback_By_PageIndex", paramList))
        {
            return dtInfo;
        }
    }
}
