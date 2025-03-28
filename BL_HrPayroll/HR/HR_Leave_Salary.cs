using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using PegasusDataAccess;
using System.Data.SqlClient;

/// <summary>
/// Summary description for HR_Leave_Salary
/// </summary>
public class HR_Leave_Salary
{
    DataAccessClass daClass = null;
	public HR_Leave_Salary(string strConString)
	{
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
    }
    public DataTable GetAllLeaveSalaryByEmpID(string EmpId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_Leave_Salary_SelectRow", paramList);
        return dtInfo;
    }
    public DataTable GetAllLeaveSalaryByEmpID_LeaveTaken(string EmpId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_Leave_Salary_SelectRow", paramList);
        return dtInfo;
    }
    public DataTable GetAllLeaveSalaryByEmpID_LeaveTakenNew(string EmpId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@optype", "5", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_Leave_Salary_SelectRow", paramList);
        return dtInfo;
    }   
    public DataTable GetAllLeaveSalaryAllRecord_ByEmpID(string EmpId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_Leave_Salary_SelectRow", paramList);
        return dtInfo;
    }
    public DataTable GetAllLeaveSalary(string EmpId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@optype", "4", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Att_Leave_Salary_SelectRow", paramList);
        return dtInfo;
    }
    public string GetLeaveSalaryByEmpId(string EmpId)
    {
        DataTable dtInfo = new DataTable();
        dtInfo = daClass.return_DataTable("select SUM(Total) AS LeaveSalary from Att_LeaveSalary where Emp_Id='" + EmpId + "'");
        return dtInfo.Rows[0]["LeaveSalary"].ToString();
    }
    public string GetClaimSalByEmpId(string EmpId)
    {      
        DataTable dtInfo = new DataTable();
        dtInfo = daClass.return_DataTable("select SUM(Value) AS ClaimSalary from Pay_Employee_Claim where Emp_Id='" + EmpId + "' AND Claim_Name='Leave Salary'");
        return dtInfo.Rows[0]["LeaveSalary"].ToString();
    }
    public int UpdateLeaveSalaryStatus(string TransId,string strModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Trans_Id", TransId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Att_Leave_Salary_Update", paramList);
        return Convert.ToInt32(paramList[2].ParaValue);
    }

    public int UpdateLeaveSalaryStatus(string TransId, string strModifiedDate,ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Trans_Id", TransId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Att_Leave_Salary_Update", paramList,ref trns);
        return Convert.ToInt32(paramList[2].ParaValue);
    }
}