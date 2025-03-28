using System;
using System.Data;
using System.Data.SqlClient;
using PegasusDataAccess;

public class Prj_ScrumMaster
{
    DataAccessClass daClass = null;
    Common cmn = null;
    public Prj_ScrumMaster(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
        cmn = new Common(strConString);
    }
    public int InsertScrumMaster(string CompanyId, string BrandId, string Location_Id, string ScrumNumber, string ScrumName, string ScrumDate, string StartDate, string EndDate, string Status, string AssignTo, string AssignDate, string NoOfHours, string Remark, string Priority, string ParentScrumId, string ReleaseDate, string Percentage, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[29];
        paramList[0] = new PassDataToSql("@CompanyId", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@BrandId", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", Location_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ScrumNumber", ScrumNumber, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ScrumName", ScrumName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ScrumDate", ScrumDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@StartDate", StartDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@EndDate", EndDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Status", Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@AssignTo", AssignTo, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@AssignDate", AssignDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@NumberOfHours", NoOfHours, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Remark", Remark, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Priority", Priority, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@ParentScrumId", ParentScrumId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@ReleaseDate", ReleaseDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Percentage", Percentage, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Prj_Scrum_Master_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[28].ParaValue);

    }
    public int UpdateScrumMaster(string CompanyId, string BrandId, string Location_Id, string ScrumNumber, string ScrumName, string ScrumDate, string StartDate, string EndDate, string Status, string AssignTo, string AssignDate, string NoOfHours, string Remark, string Priority, string ParentScrumId, string ReleaseDate, string Percentage, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate, string ScrumId, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[30];
        paramList[0] = new PassDataToSql("@CompanyId", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@BrandId", BrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", Location_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ScrumNumber", ScrumNumber, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ScrumName", ScrumName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ScrumDate", ScrumDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@StartDate", StartDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@EndDate", EndDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Status", Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@AssignTo", AssignTo, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@AssignDate", AssignDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@NumberOfHours", NoOfHours, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Remark", Remark, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Priority", Priority, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@ParentScrumId", ParentScrumId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@ReleaseDate", ReleaseDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Percentage", Percentage, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[25] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@ScrumId", ScrumId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[29] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Prj_Scrum_Master_Update", paramList, ref trns);
        return Convert.ToInt32(paramList[29].ParaValue);

    }
    public DataTable GetScrumRecord(string CompanyId, string Brand_Id, string LocationId)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@companyId", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@brandId", Brand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@locationId", LocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        using (DataTable dtInfo = daClass.Reuturn_Datatable_Search("sp_Prj_Scrum_Master_Select_Row", paramList))
        {
            return dtInfo;
        }
    }
    public DataTable GetScrumReport(string CompanyId, string Brand_Id, string LocationId,string EmpId ,string FromDate,string ToDate,string Status)
    {
        DataTable dt = new DataTable();
        string STQRL = "Select ScrumId,ScrumNumber,(Prj_ScrumMaster.CreatedBy) as CreatedBy,ScrumName,REPLACE(CONVERT(NVARCHAR, ScrumDate, 106), ' ', '-') as ScrumDate,REPLACE(CONVERT(NVARCHAR, StartDate, 106), ' ', '-')  as StartDate,REPLACE(CONVERT(NVARCHAR, EndDate, 106), ' ', '-') as EndDate,  (Case When Status = '1' then 'Running' When Status = '2' then 'Failed' When Status = '3' then 'Pass' When Status = '4' then 'Pending' When Status = '5' then 'Delayed' Else '' End) as Status,(Set_EmployeeMaster.Emp_Name) as AssignTo,REPLACE(CONVERT(NVARCHAR, AssignDate, 106), ' ', '-') as AssignDate,NumberOfHours,Remark,REPLACE(CONVERT(NVARCHAR, ReleaseDate, 106), ' ', '-') as ReleaseDate from Prj_ScrumMaster inner join Set_EmployeeMaster on Set_EmployeeMaster.Emp_Code = Prj_ScrumMaster.AssignTo where Prj_ScrumMaster.IsActive = '1' and Prj_ScrumMaster.Company_Id = " + CompanyId+" and Prj_ScrumMaster.Brand_Id = "+Brand_Id+" and Prj_ScrumMaster.Location_Id = "+LocationId+"";
        if (EmpId != "")
        {
            STQRL += "And Prj_ScrumMaster.AssignTo='"+EmpId+"'";
        }
        if (FromDate != "" && ToDate != "")
        {
            STQRL += "And Prj_ScrumMaster.ScrumDate between '"+ FromDate + "' And '"+ToDate+"'";
        }
        if (Status != "")
        {
            STQRL += "And Prj_ScrumMaster.Status='"+Status+"'";
        }
        dt = daClass.return_DataTable(STQRL);
        return dt;
    }
  
    public DataTable GetEmployeeTaskList(string projectId)
    {
        DataTable dt = new DataTable();
        dt = daClass.return_DataTable("Select * from Prj_Project_Task where Project_Id='" + projectId + "'");
        return dt;
    }
    public DataTable GetScrumRecordByScrumId(string ScrumID)
    {
        DataTable dt = new DataTable();
        dt = daClass.return_DataTable("Select Prj_ScrumMaster.Company_Id,Prj_ScrumMaster.Brand_Id,Prj_ScrumMaster.Location_Id,ScrumNumber,ScrumId,ScrumName,REPLACE(CONVERT(NVARCHAR,ScrumDate, 106), ' ', '-') as ScrumDate,REPLACE(CONVERT(NVARCHAR,StartDate, 106), ' ', '-') as StartDate, REPLACE(CONVERT(NVARCHAR,EndDate, 106), ' ', '-') as EndDate,Status,(Set_EmployeeMaster.Emp_Name+'/'+Set_EmployeeMaster.Emp_Code)as AssignTo,REPLACE(CONVERT(NVARCHAR,AssignDate, 106), ' ', '-')as AssignDate,Remark,NumberOfHours,Priority,ParentScrumId,REPLACE(CONVERT(NVARCHAR,ReleaseDate, 106), ' ', '-')as ReleaseDate,Percentage  From Prj_ScrumMaster inner join Set_EmployeeMaster on Set_EmployeeMaster.Emp_Code = Prj_ScrumMaster.AssignTo where Prj_ScrumMaster.IsActive = '1' And Prj_ScrumMaster.ScrumId = '" + ScrumID + "'");
        return dt;
    }
    public DataTable GetScrumDetailByScrumId(string ScrumID)
    {
        DataTable dt = new DataTable();
        dt = daClass.return_DataTable("Select Prj_ScrumDetail.Trans_Id,(Prj_ScrumDetail.Field1) as TaskHour,Prj_Project_Task.Subject as Task,(Prj_Project_Master.Project_Name) as Project, ROW_NUMBER() OVER(ORDER BY ScrumID ASC) as ScrumId, TaskStatus,(Case When TaskStatus = '1' then 'Running' When TaskStatus = '2' then 'Failed' When TaskStatus = '3' then 'Pass' When TaskStatus = '4' then 'Pending' When TaskStatus = '5' then 'Delayed' Else '' End) as StatusId,TaskPercentage,(Prj_Project_Task.Expected_Cost) as Cost,(Concat('AssignDate :', ' ', REPLACE(CONVERT(NVARCHAR, Prj_Project_Task.Assign_Date, 106), ' ', '-'), ' ', 'CloseDate:', ' ', REPLACE(CONVERT(NVARCHAR, Prj_Project_Task.Emp_Close_Date, 106), ' ', '-'))) as Date,Remark From Prj_ScrumDetail inner join Prj_Project_Master on Prj_Project_Master.Project_Id = Prj_ScrumDetail.Project_Id inner join Prj_Project_Task on Prj_Project_Task.Task_Id = Prj_ScrumDetail.TaskId where Prj_ScrumDetail.IsActive = '1'  And ScrumID='" + ScrumID + "'");
        return dt;
    }
    public DataTable GetTaskIdBySubject(string strTaskName, string strProjectId)
    {
        DataTable dt = new DataTable();
        dt = daClass.return_DataTable("Select*From Prj_Project_Task Where Subject='" + strTaskName + "' and Project_Id=" + strProjectId + "");
        return dt;
    }
    public DataTable GetProjectId(string ProjectName)
    {

        DataTable dt = new DataTable();
        dt = daClass.return_DataTable("Select*From Prj_Project_Master Where Project_Name='" + ProjectName + "' And IsActive='1' And Project_Title='Open'");
        return dt;
    }

    public DataTable GetTaskId(string TaskId)
    {
        DataTable dt = new DataTable();
        dt = daClass.return_DataTable("Select * from Prj_Project_Task where Task_Id='" + TaskId + "'");
        return dt;
    }
    public int InsertScrumDetail(string ScrumId, string ProjectId, string TaskId, string TaskStatus, string TaskPercentage, string Remark, string Field1, string Field2, String Field3, string Field4, string Field5, string Field6, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
    {

        PassDataToSql[] paramList = new PassDataToSql[18];
        paramList[0] = new PassDataToSql("@ScrumId", ScrumId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Project_Id", ProjectId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@TaskId", TaskId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@TaskStatus", TaskStatus, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@TaskPercentage", TaskPercentage, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Remark", Remark, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("sp_Prj_Scrum_Detail_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[17].ParaValue);
    }

    public int DeleteScrumDetail(string ScrumId, ref SqlTransaction trns)
    {

        daClass.execute_Command("Delete From Prj_ScrumDetail where ScrumId='" + ScrumId + "'", ref trns);
        return 1;
    }

    public DataTable GetScrumReport(string Id)
    {
        DataTable dt = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[1];
        paramList[0] = new PassDataToSql("@ScrumId", Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
     
        dt= daClass.Reuturn_Datatable_Search("sp_Prj_ScrumMaster_SelectRow_Report", paramList);
        return dt;
    }

    public DataTable GetScrumStaticsReport(string LocationId)
    {
        DataTable dt = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@LocationId", LocationId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dt = daClass.Reuturn_Datatable_Search("sp_EmployeeScrum_Statics_Report", paramList);
        return dt;
    }

}