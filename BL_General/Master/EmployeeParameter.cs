using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using PegasusDataAccess;
using System.Data.SqlClient;
/// <summary>
/// Summary description for EmployeeParameter
/// </summary>
public class EmployeeParameter
{
    DataAccessClass daClass = null;
    EmployeeMaster objEmp = null;
    Set_Emp_SalaryIncrement objEmpSalInc = null;
    private string _strConString = string.Empty;
    public EmployeeParameter(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
        objEmp = new EmployeeMaster(strConString);
        objEmpSalInc = new Set_Emp_SalaryIncrement(strConString);
        _strConString = strConString;

    }

    public void DeleteEmployeeParameterByEmpId(string empid)
    {
        PassDataToSql[] paramList = new PassDataToSql[1];
        paramList[0] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Set_Employee_Parameter_Delete", paramList);
    }
    public void DeleteEmployeeParameterByEmpIdNew(string empid)
    {
        PassDataToSql[] paramList = new PassDataToSql[1];
        paramList[0] = new PassDataToSql("@Emp_Id", empid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Set_Employee_Parameter_Delete_New", paramList);
    }

    public string GetEmployeeParameterByParameterName(string EmpId, string ParamName)
    {
        string ParamValue = string.Empty;
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Company_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Employee_Parameter_SelectRow", paramList);

        if (dtInfo.Rows.Count > 0)
        {

            ParamValue = dtInfo.Rows[0][ParamName].ToString();

        }


        return ParamValue;
    }

    public string GetEmployeeParameterByParameterName(string EmpId, string ParamName,ref SqlTransaction trns)
    {
        string ParamValue = string.Empty;
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Company_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Employee_Parameter_SelectRow", paramList,ref trns);

        if (dtInfo.Rows.Count > 0)
        {

            ParamValue = dtInfo.Rows[0][ParamName].ToString();

        }


        return ParamValue;
    }

    public DataTable GetEmployeeParameterByEmpId(string EmpId, string CompanyId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Employee_Parameter_SelectRow", paramList);

        return dtInfo;
    }

    public DataTable Get_Account_Employee_Payment(string Account_ID, string Optype)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Account_ID", Account_ID, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);        
        paramList[1] = new PassDataToSql("@Optype", Optype, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("Get_Account_Employee_Payment", paramList);
        return dtInfo;
    }


    public DataTable GetEmployeeParameterByEmpId(string EmpId, string CompanyId,ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Employee_Parameter_SelectRow", paramList,ref trns);

        return dtInfo;
    }


    public DataTable GetEmployeeParameter(string CompanyId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[1] = new PassDataToSql("@Emp_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Set_Employee_Parameter_SelectRow", paramList);

        return dtInfo;
    }
    public int InsertEmployeeParameter(string CompanyId, string Emp_Id, string Basic_Salary, string Salary_Type, string Currency_Id, string Assign_Min, string Effective_Work_Cal_Method, string Is_OverTime, string Normal_OT_Method, string Normal_OT_Type, string Normal_OT_Value, string Normal_HOT_Type, string Normal_HOT_Value, string Normal_WOT_Type, string Normal_WOT_Value, string Is_Partial_Enable, string Partial_Leave_Mins, string Partial_Leave_Day, string Is_Partial_Carry, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string Field8, string Field9, string Field10, string Field11, string Field12, string Field13, string Field14, string IsActive, string createdby, string createddate, string ModifiedBy, string ModifiedDate, string Gross_Salary, string IsCtc_Employee, string MobileBill_Limit, string Salary_Plan_Id,string Opening_Credit,string Opening_Debit,string Previous_Employer_Earning,string Previous_Employer_TDS,string Field15, string Field16, string Field17, string Field18, string Field19, string Field20,string Payment_Opt_Account_Id)
    {

        PassDataToSql[] paramList = new PassDataToSql[54];
        paramList[0] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Basic_Salary", Basic_Salary, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Salary_Type", Salary_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Currency_Id", Currency_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Assign_Min", Assign_Min, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Effective_Work_Cal_Method", Effective_Work_Cal_Method, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Is_OverTime", Is_OverTime, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Normal_OT_Method", Normal_OT_Method, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Normal_OT_Type", Normal_OT_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Normal_OT_Value", Normal_OT_Value, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Normal_HOT_Type", Normal_HOT_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Normal_HOT_Value", Normal_HOT_Value, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Normal_WOT_Type", Normal_WOT_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Normal_WOT_Value", Normal_WOT_Value, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Is_Partial_Enable", Is_Partial_Enable, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Partial_Leave_Mins", Partial_Leave_Mins, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Partial_Leave_Day", Partial_Leave_Day, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Is_Partial_Carry", Is_Partial_Carry, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[19] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[25] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@CreatedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@CreatedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[29] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[30] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[31] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);


        paramList[32] = new PassDataToSql("@Field8", Field8, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[33] = new PassDataToSql("@Field9", Field9, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[34] = new PassDataToSql("@Field10", Field10, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[35] = new PassDataToSql("@Field11", Field11, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[36] = new PassDataToSql("@Field12", Field12, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[37] = new PassDataToSql("@Field13", Field13, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[38] = new PassDataToSql("@Field14", Field14, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[39] = new PassDataToSql("@Gross_Salary", Gross_Salary, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[40] = new PassDataToSql("@IsCtc_Employee", IsCtc_Employee, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[41] = new PassDataToSql("@MobileBill_Limit", MobileBill_Limit, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[42] = new PassDataToSql("@Salary_Plan_Id", Salary_Plan_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[43] = new PassDataToSql("@Opening_Credit", Opening_Credit, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[44] = new PassDataToSql("@Opening_Debit", Opening_Debit, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[45] = new PassDataToSql("@Previous_Employer_Earning", Previous_Employer_Earning, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[46] = new PassDataToSql("@Previous_Employer_TDS", Previous_Employer_TDS, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[47] = new PassDataToSql("@Field15", Field15, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[48] = new PassDataToSql("@Field16", Field16, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[49] = new PassDataToSql("@Field17", Field17, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[50] = new PassDataToSql("@Field18", Field18, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[51] = new PassDataToSql("@Field19", Field19, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[52] = new PassDataToSql("@Field20", Field20, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[53] = new PassDataToSql("@Payment_Opt_Account_Id", Payment_Opt_Account_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        
        daClass.execute_Sp("sp_Set_Employee_Parameter_Insert", paramList);
        return Convert.ToInt32(paramList[31].ParaValue);
    }



    public int InsertEmployeeParameter(string CompanyId, string Emp_Id, string Basic_Salary, string Salary_Type, string Currency_Id, string Assign_Min, string Effective_Work_Cal_Method, string Is_OverTime, string Normal_OT_Method, string Normal_OT_Type, string Normal_OT_Value, string Normal_HOT_Type, string Normal_HOT_Value, string Normal_WOT_Type, string Normal_WOT_Value, string Is_Partial_Enable, string Partial_Leave_Mins, string Partial_Leave_Day, string Is_Partial_Carry, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string Field8, string Field9, string Field10, string Field11, string Field12, string Field13, string Field14, string IsActive, string createdby, string createddate, string ModifiedBy, string ModifiedDate, string Gross_Salary, string IsCtc_Employee, string MobileBill_Limit, string Salary_Plan_Id, string Opening_Credit, string Opening_Debit, string Previous_Employer_Earning, string Previous_Employer_TDS, string Field15, string Field16, string Field17, string Field18, string Field19, string Field20, string Payment_Opt_Account_Id,ref SqlTransaction trns)
    {

        PassDataToSql[] paramList = new PassDataToSql[54];
        paramList[0] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Basic_Salary", Basic_Salary, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Salary_Type", Salary_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Currency_Id", Currency_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Assign_Min", Assign_Min, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Effective_Work_Cal_Method", Effective_Work_Cal_Method, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Is_OverTime", Is_OverTime, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Normal_OT_Method", Normal_OT_Method, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Normal_OT_Type", Normal_OT_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Normal_OT_Value", Normal_OT_Value, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Normal_HOT_Type", Normal_HOT_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Normal_HOT_Value", Normal_HOT_Value, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Normal_WOT_Type", Normal_WOT_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Normal_WOT_Value", Normal_WOT_Value, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Is_Partial_Enable", Is_Partial_Enable, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Partial_Leave_Mins", Partial_Leave_Mins, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Partial_Leave_Day", Partial_Leave_Day, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Is_Partial_Carry", Is_Partial_Carry, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[19] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[25] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@CreatedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@CreatedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[29] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[30] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[31] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);


        paramList[32] = new PassDataToSql("@Field8", Field8, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[33] = new PassDataToSql("@Field9", Field9, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[34] = new PassDataToSql("@Field10", Field10, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[35] = new PassDataToSql("@Field11", Field11, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[36] = new PassDataToSql("@Field12", Field12, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[37] = new PassDataToSql("@Field13", Field13, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[38] = new PassDataToSql("@Field14", Field14, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[39] = new PassDataToSql("@Gross_Salary", Gross_Salary, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[40] = new PassDataToSql("@IsCtc_Employee", IsCtc_Employee, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[41] = new PassDataToSql("@MobileBill_Limit", MobileBill_Limit, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[42] = new PassDataToSql("@Salary_Plan_Id", Salary_Plan_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[43] = new PassDataToSql("@Opening_Credit", Opening_Credit, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[44] = new PassDataToSql("@Opening_Debit", Opening_Debit, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[45] = new PassDataToSql("@Previous_Employer_Earning", Previous_Employer_Earning, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[46] = new PassDataToSql("@Previous_Employer_TDS", Previous_Employer_TDS, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[47] = new PassDataToSql("@Field15", Field15, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[48] = new PassDataToSql("@Field16", Field16, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[49] = new PassDataToSql("@Field17", Field17, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[50] = new PassDataToSql("@Field18", Field18, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[51] = new PassDataToSql("@Field19", Field19, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[52] = new PassDataToSql("@Field20", Field20, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[53] = new PassDataToSql("@Payment_Opt_Account_Id", Payment_Opt_Account_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Set_Employee_Parameter_Insert", paramList,ref trns);
        return Convert.ToInt32(paramList[31].ParaValue);
    }


    public int Update_Employee_payment_Account(string CompanyId, string Emp_Id, string Payment_Opt_Account_ID)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Payment_Opt_Account_ID", Payment_Opt_Account_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);        
        paramList[3] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("Update_Employee_Parameter_Insert", paramList);
        return Convert.ToInt32(paramList[3].ParaValue);
    }

    public int Update_Employee_payment_Account(string CompanyId, string Emp_Id, string Payment_Opt_Account_ID,ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Payment_Opt_Account_ID", Payment_Opt_Account_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("Update_Employee_Parameter_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[3].ParaValue);
    }

    public int InsertEmployeeParameter(string CompanyId, string Emp_Id, string Basic_Salary, string Salary_Type, string Currency_Id, string Assign_Min, string Effective_Work_Cal_Method, string Is_OverTime, string Normal_OT_Method, string Normal_OT_Type, string Normal_OT_Value, string Normal_HOT_Type, string Normal_HOT_Value, string Normal_WOT_Type, string Normal_WOT_Value, string Is_Partial_Enable, string Partial_Leave_Mins, string Partial_Leave_Day, string Is_Partial_Carry, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string Field8, string Field9, string Field10, string Field11, string Field12, string Field13, string Field14, string IsActive, string createdby, string createddate, string ModifiedBy, string ModifiedDate, string Gross_Salary, string IsCtc_Employee, string MobileBill_Limit, string Salary_Plan_Id, string Opening_Credit, string Opening_Debit, string Previous_Employer_Earning, string Previous_Employer_TDS, string Field15, string Field16, string Field17, string Field18, string Field19, string Field20,ref SqlTransaction trns,string Payment_Opt_Account_Id)
    {

        PassDataToSql[] paramList = new PassDataToSql[54];
        paramList[0] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Basic_Salary", Basic_Salary, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Salary_Type", Salary_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Currency_Id", Currency_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Assign_Min", Assign_Min, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Effective_Work_Cal_Method", Effective_Work_Cal_Method, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Is_OverTime", Is_OverTime, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Normal_OT_Method", Normal_OT_Method, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Normal_OT_Type", Normal_OT_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Normal_OT_Value", Normal_OT_Value, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Normal_HOT_Type", Normal_HOT_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Normal_HOT_Value", Normal_HOT_Value, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Normal_WOT_Type", Normal_WOT_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Normal_WOT_Value", Normal_WOT_Value, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Is_Partial_Enable", Is_Partial_Enable, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Partial_Leave_Mins", Partial_Leave_Mins, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Partial_Leave_Day", Partial_Leave_Day, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Is_Partial_Carry", Is_Partial_Carry, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[19] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[25] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@CreatedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@CreatedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[29] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[30] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[31] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);


        paramList[32] = new PassDataToSql("@Field8", Field8, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[33] = new PassDataToSql("@Field9", Field9, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[34] = new PassDataToSql("@Field10", Field10, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[35] = new PassDataToSql("@Field11", Field11, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[36] = new PassDataToSql("@Field12", Field12, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[37] = new PassDataToSql("@Field13", Field13, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[38] = new PassDataToSql("@Field14", Field14, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[39] = new PassDataToSql("@Gross_Salary", Gross_Salary, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[40] = new PassDataToSql("@IsCtc_Employee", IsCtc_Employee, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[41] = new PassDataToSql("@MobileBill_Limit", MobileBill_Limit, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[42] = new PassDataToSql("@Salary_Plan_Id", Salary_Plan_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[43] = new PassDataToSql("@Opening_Credit", Opening_Credit, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[44] = new PassDataToSql("@Opening_Debit", Opening_Debit, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[45] = new PassDataToSql("@Previous_Employer_Earning", Previous_Employer_Earning, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[46] = new PassDataToSql("@Previous_Employer_TDS", Previous_Employer_TDS, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[47] = new PassDataToSql("@Field15", Field15, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[48] = new PassDataToSql("@Field16", Field16, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[49] = new PassDataToSql("@Field17", Field17, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[50] = new PassDataToSql("@Field18", Field18, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[51] = new PassDataToSql("@Field19", Field19, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[52] = new PassDataToSql("@Field20", Field20, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[53] = new PassDataToSql("@Payment_Opt_Account_Id", Payment_Opt_Account_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Set_Employee_Parameter_Insert", paramList,ref trns);
        return Convert.ToInt32(paramList[31].ParaValue);
    }


    public int InsertEmpParameterNew(string EmpId, string CompanyId, string Param_Name, string Param_Value, string Param_Cat_Id, string Description, string IsActive, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[10];
        paramList[0] = new PassDataToSql("@EmpId", EmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Param_Name", Param_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Param_Value", Param_Value, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Description", Description, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Param_Cat_Id", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[5] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[7] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Set_EmployeeParameter_Insert", paramList);
        return Convert.ToInt32(paramList[9].ParaValue);
    }

    public int InsertEmpParameterNew(string EmpId, string CompanyId, string Param_Name, string Param_Value, string Param_Cat_Id, string Description, string IsActive, string ModifiedBy, string ModifiedDate,ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[10];
        paramList[0] = new PassDataToSql("@EmpId", EmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Param_Name", Param_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Param_Value", Param_Value, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Description", Description, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Param_Cat_Id", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[5] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[7] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Set_EmployeeParameter_Insert", paramList,ref trns);
        return Convert.ToInt32(paramList[9].ParaValue);
    }
    public string GetEmployeeParameterValueByParamNameNew(string ParamName, string EmpId)
    {
        string str = string.Empty;
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Param_Name", ParamName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Get_EmployeeParameter_SelectRow", paramList);
        if (dtInfo.Rows.Count > 0)
        {
            str = dtInfo.Rows[0]["Param_Value"].ToString();
        }
        return str;
    }

    public DataTable GetEmployeeParameterValueByAll()
    {

        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Emp_Id", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Param_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Get_EmployeeParameter_SelectRow", paramList);
        return dtInfo;
    }

    public DataTable GetEmployeeParameterValueByEmpIdNew(string EmpId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Emp_Id", EmpId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Param_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Get_EmployeeParameter_SelectRow", paramList);
        return dtInfo;
    }

    public int InsertEmployeeParameterOnEmployeeInsert(string CompanyId, string Emp_Id, string ModifiedBy, string ModifiedDate,string Payment_Opt_Account_Id,string strBrandId,string strLocId,string strTimeZoneId)
    {
        Set_ApplicationParameter objAppParam = new Set_ApplicationParameter(_strConString);
        PassDataToSql[] paramList = new PassDataToSql[54];
        
        string Basic_Salary = "0";
        string Salary_Type = "Monthly";
        string Currency_Id = "1";
        string Assign_Min = string.Empty;
        try
        {
            Assign_Min = objAppParam.GetApplicationParameterValueByParamName("Work Day Min", CompanyId,strBrandId,strLocId);
        }
        catch
        {
            Assign_Min = "540";
        }
        string Effective_Work_Cal_Method = string.Empty;
        try
        {
            Effective_Work_Cal_Method = objAppParam.GetApplicationParameterValueByParamName("Effective Work Calculation Method", CompanyId,strBrandId,strLocId);
        }
        catch
        {
            Effective_Work_Cal_Method = "InOut";
        }

        string Is_OverTime = string.Empty;
        try
        {
            Is_OverTime = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("IsOverTime", CompanyId, strBrandId, strLocId)).ToString();
        }
        catch
        {
            Is_OverTime = false.ToString();
        }
        string Normal_OT_Method = string.Empty;
        try
        {

            Normal_OT_Method = objAppParam.GetApplicationParameterValueByParamName("Over Time Calculation Method", CompanyId, strBrandId, strLocId);
        }
        catch
        {
            Normal_OT_Method = "Work Hour";
        }
        string Normal_OT_Type = "2";
        string Normal_OT_Value = "100";
        string Normal_HOT_Type = "2";
        string Normal_HOT_Value = "100";
        string Normal_WOT_Type = "2";
        string Normal_WOT_Value = "100";
        string Is_Partial_Enable = string.Empty;

        try
        {
            Is_Partial_Enable = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Partial_Leave_Enable", CompanyId, strBrandId, strLocId)).ToString();
        }
        catch
        {
            Is_Partial_Enable = false.ToString();
        }
        string Partial_Leave_Mins = string.Empty;

        try
        {
            Partial_Leave_Mins = objAppParam.GetApplicationParameterValueByParamName("Total Partial Leave Minutes", CompanyId, strBrandId, strLocId);

        }
        catch
        {
            Partial_Leave_Mins = "240";
        }


        string Partial_Leave_Day = string.Empty;


        try
        {
            Partial_Leave_Day = objAppParam.GetApplicationParameterValueByParamName("Partial Leave Minute Use In A Day", CompanyId, strBrandId, strLocId);
        }
        catch
        {
            Partial_Leave_Day = "60";
        }

        string Is_Partial_Carry = false.ToString();
        string Field1 = string.Empty;
        try
        {
            Field1 = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Is_Late_Penalty", CompanyId, strBrandId, strLocId)).ToString();
        }
        catch
        {
            Field1 = false.ToString();
        }



        string Field2 = string.Empty;
        try
        {
            Field2 = Convert.ToBoolean(objAppParam.GetApplicationParameterValueByParamName("Is_Early_Penalty", CompanyId, strBrandId, strLocId)).ToString();
        }
        catch
        {
            Field2 = false.ToString();
        }
        string Field3 = string.Empty;

        Field3 = true.ToString();

        string Field4 = false.ToString();
        string Field5 = false.ToString();
        string Field6 = false.ToString();
        string Field7 = Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString();
        string Field8 = string.Empty;
        string Field9 = string.Empty;
        string Field10 = string.Empty;
        string Field11 = string.Empty;
        string Field12 = string.Empty;
        try
        {
            Field12 = objAppParam.GetApplicationParameterValueByParamName("Half_Day_Count", CompanyId, strBrandId, strLocId);
        }
        catch
        {

        }


        Field8 = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Duration", CompanyId, strBrandId, strLocId);
        Field9 = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Fresher_From", CompanyId, strBrandId, strLocId);
        Field10 = objAppParam.GetApplicationParameterValueByParamName("Sal_Increment_Per_Fresher_To", CompanyId, strBrandId, strLocId);
        Field11 = "Fresher";


        //this code is created by jitendra upadhyay on 17-09-2014 to insert the record in salary increment table
        DataTable dtEmp = objEmp.GetEmployeeMasterById(CompanyId, Emp_Id);
        DateTime JoinDate = new DateTime();
        if (dtEmp.Rows.Count > 0)
        {
            try
            {
                JoinDate = Convert.ToDateTime(dtEmp.Rows[0]["Doj"].ToString());
            }
            catch
            {

            }
        }



        double IncrementPer = 0;
        try
        {
            IncrementPer = double.Parse(Field10);
        }
        catch
        {

        }


        double BasicSal = 0;


        double IncrementValue = 0;
        try
        {
            IncrementValue = (BasicSal * IncrementPer) / 100;
        }
        catch
        {

        }

        double IncrementSalary = 0;
        int Duration = 0;
        try
        {
            Duration = int.Parse(Field8);
        }
        catch
        {

        }

        IncrementSalary = BasicSal + IncrementValue;

        DateTime IncrementDate = JoinDate.AddMonths(Duration);

        objEmpSalInc.DeleteEmpSalaryIncrementByEmpId(Emp_Id);
        //objEmpSalInc.InsertEmpSalaryIncrement(Session["CompId"].ToString(), hdnEmpIdSal.Value, BasicSal.ToString(), ddlCategory1.SelectedValue, IncrementDate.Month.ToString(), IncrementDate.Year.ToString(), txtIncrementPerTo1.Text, IncrementValue.ToString(), IncrementSalary.ToString(), Duration.ToString(), txtIncrementPerFrom1.Text, txtIncrementPerTo1.Text, "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString()).ToString(), true.ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString()).ToString(), Session["UserId"].ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString()).ToString());

        objEmpSalInc.InsertEmpSalaryIncrement(CompanyId, Emp_Id, BasicSal.ToString(), "Fresher", IncrementDate.Month.ToString(), IncrementDate.Year.ToString(), Field10, IncrementValue.ToString(), IncrementSalary.ToString(), Duration.ToString(), Field9, Field10, "", "", "", true.ToString(), Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), true.ToString(), ModifiedBy, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString(), ModifiedBy, Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(),strTimeZoneId).ToString());




        paramList[0] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Basic_Salary", Basic_Salary, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Salary_Type", Salary_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Currency_Id", Currency_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        if (Assign_Min != string.Empty)
        {
            paramList[4] = new PassDataToSql("@Assign_Min", Assign_Min, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        }
        else
        {
            paramList[4] = new PassDataToSql("@Assign_Min", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        }
        paramList[5] = new PassDataToSql("@Effective_Work_Cal_Method", Effective_Work_Cal_Method, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Is_OverTime", Is_OverTime, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Normal_OT_Method", Normal_OT_Method, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Normal_OT_Type", Normal_OT_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Normal_OT_Value", Normal_OT_Value, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Normal_HOT_Type", Normal_HOT_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Normal_HOT_Value", Normal_HOT_Value, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Normal_WOT_Type", Normal_WOT_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Normal_WOT_Value", Normal_WOT_Value, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Is_Partial_Enable", Is_Partial_Enable, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        if (Partial_Leave_Mins != string.Empty)
        {

            paramList[15] = new PassDataToSql("@Partial_Leave_Mins", Partial_Leave_Mins, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        }
        else
        {
            paramList[15] = new PassDataToSql("@Partial_Leave_Mins", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        }
        if (Partial_Leave_Day != string.Empty)
        {
            paramList[16] = new PassDataToSql("@Partial_Leave_Day", Partial_Leave_Day, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        }
        else
        {
            paramList[16] = new PassDataToSql("@Partial_Leave_Day", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        }
        paramList[17] = new PassDataToSql("@Is_Partial_Carry", Is_Partial_Carry, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[19] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[25] = new PassDataToSql("@IsActive", true.ToString(), PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[27] = new PassDataToSql("@CreatedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@CreatedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[29] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[30] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[31] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        paramList[32] = new PassDataToSql("@Field8", Field8, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[33] = new PassDataToSql("@Field9", Field9, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[34] = new PassDataToSql("@Field10", Field10, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[35] = new PassDataToSql("@Field11", Field11, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[36] = new PassDataToSql("@Field12", Field12, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[37] = new PassDataToSql("@Field13", "True", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[38] = new PassDataToSql("@Field14", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[39] = new PassDataToSql("@Gross_Salary", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[40] = new PassDataToSql("@IsCtc_Employee", "True", PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[41] = new PassDataToSql("@MobileBill_Limit", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[42] = new PassDataToSql("@Salary_Plan_Id", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[43] = new PassDataToSql("@Opening_Credit", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[44] = new PassDataToSql("@Opening_Debit", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[45] = new PassDataToSql("@Previous_Employer_Earning", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[46] = new PassDataToSql("@Previous_Employer_TDS", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[47] = new PassDataToSql("@Field15", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[48] = new PassDataToSql("@Field16", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[49] = new PassDataToSql("@Field17", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[50] = new PassDataToSql("@Field18", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[51] = new PassDataToSql("@Field19", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[52] = new PassDataToSql("@Field20", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[53] = new PassDataToSql("@Payment_Opt_Account_Id", Payment_Opt_Account_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);



        daClass.execute_Sp("sp_Set_Employee_Parameter_Insert", paramList);
        return Convert.ToInt32(paramList[31].ParaValue);
    }
    public int UpdateEmployeeParameter(string CompanyId, string Emp_Id, string Basic_Salary, string Salary_Type, string Currency_Id, string Assign_Min, string Effective_Work_Cal_Method, string Is_OverTime, string Normal_OT_Method, string Normal_OT_Type, string Normal_OT_Value, string Normal_HOT_Type, string Normal_HOT_Value, string Normal_WOT_Type, string Normal_WOT_Value, string Is_Partial_Enable, string Partial_Leave_Mins, string Partial_Leave_Day, string Is_Partial_Carry, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string Field8, string Field9, string Field10, string Field11, string Field12, string Field13, string Field14, string IsActive, string createdby, string createddate, string ModifiedBy, string ModifiedDate, string Gross_Salary, string IsCtc_Employee, string MobileBill_Limit, string Salary_Plan_Id, string Opening_Credit, string Opening_Debit, string Previous_Employer_Earning, string Previous_Employer_TDS, string Field15, string Field16, string Field17, string Field18, string Field19, string Field20, string Payment_Opt_Account_Id)
    {

        PassDataToSql[] paramList = new PassDataToSql[52];


        paramList[0] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Basic_Salary", Basic_Salary, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Salary_Type", Salary_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Currency_Id", Currency_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Assign_Min", Assign_Min, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Effective_Work_Cal_Method", Effective_Work_Cal_Method, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Is_OverTime", Is_OverTime, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Normal_OT_Method", Normal_OT_Method, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Normal_OT_Type", Normal_OT_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Normal_OT_Value", Normal_OT_Value, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Normal_HOT_Type", Normal_HOT_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Normal_HOT_Value", Normal_HOT_Value, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Normal_WOT_Type", Normal_WOT_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Normal_WOT_Value", Normal_WOT_Value, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@Is_Partial_Enable", Is_Partial_Enable, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@Partial_Leave_Mins", Partial_Leave_Mins, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@Partial_Leave_Day", Partial_Leave_Day, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[17] = new PassDataToSql("@Is_Partial_Carry", Is_Partial_Carry, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[18] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[19] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[20] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[21] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[22] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[23] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[24] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        paramList[25] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[26] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[27] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[28] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[29] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[30] = new PassDataToSql("@Field8", Field8, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[31] = new PassDataToSql("@Field9", Field9, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[32] = new PassDataToSql("@Field10", Field10, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[33] = new PassDataToSql("@Field11", Field11, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[34] = new PassDataToSql("@Field12", Field12, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[35] = new PassDataToSql("@Field13", Field13, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[36] = new PassDataToSql("@Field14", Field14, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[37] = new PassDataToSql("@Gross_Salary", Gross_Salary, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[38] = new PassDataToSql("@IsCtc_Employee", IsCtc_Employee, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[39] = new PassDataToSql("@MobileBill_Limit", MobileBill_Limit, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[40] = new PassDataToSql("@Salary_Plan_Id", Salary_Plan_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[41] = new PassDataToSql("@Opening_Credit", Opening_Credit, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[42] = new PassDataToSql("@Opening_Debit", Opening_Debit, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[43] = new PassDataToSql("@Previous_Employer_Earning", Previous_Employer_Earning, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[44] = new PassDataToSql("@Previous_Employer_TDS", Previous_Employer_TDS, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[45] = new PassDataToSql("@Field15", Field15, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[46] = new PassDataToSql("@Field16", Field16, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[47] = new PassDataToSql("@Field17", Field17, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[48] = new PassDataToSql("@Field18", Field18, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[49] = new PassDataToSql("@Field19", Field19, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[50] = new PassDataToSql("@Field20", Field20, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[51] = new PassDataToSql("@Payment_Opt_Account_Id", Payment_Opt_Account_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        daClass.execute_Sp("sp_Set_Employee_Parameter_Update", paramList);
        return Convert.ToInt32(paramList[29].ParaValue);
    }




    public int UpdateEmployeeSalaryParameter(string CompanyId, string Emp_Id, string Basic_Salary, string Field8, string Field9, string Field10, string Field11, string IsActive, string ModifiedBy, string ModifiedDate)
    {

        PassDataToSql[] paramList = new PassDataToSql[11];


        paramList[0] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Basic_Salary", Basic_Salary, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[4] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        paramList[7] = new PassDataToSql("@Field8", Field8, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[8] = new PassDataToSql("@Field9", Field9, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field10", Field10, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field11", Field11, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Set_Employee_Parameter_Update_Salary", paramList);
        return Convert.ToInt32(paramList[6].ParaValue);
    }




    public int UpdateEmployeeHalfDayCount(string CompanyId, string Emp_Id, string Halfday, string IsActive, string ModifiedBy, string ModifiedDate)
    {
        if (Halfday == "")
        {
            Halfday = "0";

        }

        PassDataToSql[] paramList = new PassDataToSql[7];


        paramList[0] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Field12", Halfday, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[4] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Set_Employee_Parameter_Update_HalfDay", paramList);
        return Convert.ToInt32(paramList[6].ParaValue);
    }

  
}
