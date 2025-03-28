using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PegasusDataAccess;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for HR_Indemnity_Master
/// </summary>
public class HR_Indemnity_Master
{
    DataAccessClass daClass = null;
	public HR_Indemnity_Master(string strConString)
	{
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
	}

    public int InsertIndemnityRecord(string Indemnity_Id,string Company_Id, string Employee_Id, string Indemnity_Date, string Status, string P1, string P2, string P3, string P4, string P5,string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[23];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Employee_Id", Employee_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Indemnity_Date", Indemnity_Date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@P1", P1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@P2", P2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@P3", P3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@P4", P4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@P5", P5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[16] = new PassDataToSql("@Is_Active", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Created_By", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Created_Date", strCreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@Modified_By", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@Modified_Date", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@Reference_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[22] = new PassDataToSql("@Indemnity_Id", Indemnity_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_HR_HR_Indemnity_Master_Insert", paramList);
        return Convert.ToInt32(paramList[21].ParaValue);
    }

    public int InsertIndemnityRecord(string Indemnity_Id, string Company_Id, string Employee_Id, string Indemnity_Date, string Status, string P1, string P2, string P3, string P4, string P5, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate,ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[23];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Employee_Id", Employee_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Indemnity_Date", Indemnity_Date, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Status", Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@P1", P1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@P2", P2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@P3", P3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@P4", P4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@P5", P5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[16] = new PassDataToSql("@Is_Active", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Created_By", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Created_Date", strCreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[19] = new PassDataToSql("@Modified_By", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@Modified_Date", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@Reference_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[22] = new PassDataToSql("@Indemnity_Id", Indemnity_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_HR_HR_Indemnity_Master_Insert", paramList,ref trns);
        return Convert.ToInt32(paramList[21].ParaValue);
    }
    public DataTable GetIndemnityEmployee()
    {
        DataTable DtList = new DataTable();
        DtList = daClass.return_DataTable("select HR_Indemnity_Master.Indemnity_Id,replace(CONVERT(VARCHAR(11),HR_Indemnity_Master.Indemnity_Date,106),'','-') AS Indemnity_Date,Set_EmployeeMaster.Emp_Name from HR_Indemnity_Master Inner Join Set_EmployeeMaster ON Set_EmployeeMaster.Emp_Id = HR_Indemnity_Master.Employee_Id WHERE ISNULL(HR_Indemnity_Master.IsActive,0)=1");
        return DtList;
    }
    public DataTable GetIndemnityEmployeeById(string Indemnity_Id)
    {
        DataTable DtList = new DataTable();
        DtList = daClass.return_DataTable("select replace(CONVERT(VARCHAR(11),HR_Indemnity_Master.Indemnity_Date,106),'','-') AS Indemnity_Date from HR_Indemnity_Master WHERE ISNULL(HR_Indemnity_Master.IsActive,0)=1 AND Indemnity_Id=" + Indemnity_Id + "");
        return DtList;
    }
    // Nitin Jain On 10/12/2014, Indemnity Leave For New Page Indemnity Leave Request
    public DataTable GetIndemnityLeaveByEmpId(string EmpId)
    {
        DataTable DtList = new DataTable();
        DtList = daClass.return_DataTable("select *,Att_LeaveMaster.Leave_Name from HR_Indemnity_Master Inner Join Att_LeaveMaster ON Att_LeaveMaster.Leave_Id = 1  where ISNULL(HR_Indemnity_Master.IsActive,0)=1 AND HR_Indemnity_Master.Status='Approved' AND HR_Indemnity_Master.P2='Not Used' and HR_Indemnity_Master.Employee_Id='"+EmpId+"'");
        return DtList;
    }

    public DataTable GetIndemnityLeaveByEmpId(string EmpId,ref SqlTransaction trns)
    {
        DataTable DtList = new DataTable();
        DtList = daClass.return_DataTable("select *,Att_LeaveMaster.Leave_Name from HR_Indemnity_Master Inner Join Att_LeaveMaster ON Att_LeaveMaster.Leave_Id = 1  where ISNULL(HR_Indemnity_Master.IsActive,0)=1 AND HR_Indemnity_Master.Status='Approved' AND HR_Indemnity_Master.P2='Not Used' and HR_Indemnity_Master.Employee_Id='" + EmpId + "'",ref trns);
        return DtList;
    }
    public DataTable GetIndemnityEmployeeForPayroll(string EmpIds,string Month,string Year)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Emp_Ids", EmpIds, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Month", Month, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Year", Year, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Get_HR_Indemnity_Master_Select", paramList);
        return dtInfo;
    }

}
