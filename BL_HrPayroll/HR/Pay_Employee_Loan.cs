using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using PegasusDataAccess;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Pay_Employee_Loan
/// </summary>
public class Pay_Employee_Loan
{
    DataAccessClass da = null;
    public Pay_Employee_Loan(string strConString)
    {
        da = new DataAccessClass(strConString);
    }
    public int Insert_In_Pay_Employee_Loan(string Company_Id, string Emp_Id, string Loan_Name, string Loan_Request_Date, string Loan_Approval_Date, string Loan_Amount, string Loan_Duration, string Loan_Interest, string Gross_Amount, string Monthly_Installment, string Is_Status, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] param = new PassDataToSql[25];
        param[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Loan_Name", Loan_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Loan_Request_Date", Loan_Request_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[4] = new PassDataToSql("@Loan_Approval_Date", Loan_Approval_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Loan_Amount", Loan_Amount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);        
        param[6] = new PassDataToSql("@Loan_Duration", Loan_Duration, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[7] = new PassDataToSql("@Loan_Interest", Loan_Interest, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[8] = new PassDataToSql("@Gross_Amount", Gross_Amount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[9] = new PassDataToSql("@Monthly_Installment", Monthly_Installment, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[10] = new PassDataToSql("@Is_Status", Is_Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[11] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[12] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[13] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[14] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[15] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[16] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        param[17] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[18] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        param[19] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[20] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[21] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[22] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[23] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        param[24] = new PassDataToSql("@Op_Type", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        da.execute_Sp("sp_Pay_Employee_Loan_Insert", param);
        return Convert.ToInt32(param[23].ParaValue);
    }

    public int Insert_In_Pay_Employee_Loan(string Company_Id, string Emp_Id, string Loan_Name, string Loan_Request_Date, string Loan_Approval_Date, string Loan_Amount, string Loan_Duration, string Loan_Interest, string Gross_Amount, string Monthly_Installment, string Is_Status, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate,ref SqlTransaction trns)
    {
        PassDataToSql[] param = new PassDataToSql[25];
        param[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Loan_Name", Loan_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Loan_Request_Date", Loan_Request_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[4] = new PassDataToSql("@Loan_Approval_Date", Loan_Approval_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Loan_Amount", Loan_Amount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Loan_Duration", Loan_Duration, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[7] = new PassDataToSql("@Loan_Interest", Loan_Interest, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[8] = new PassDataToSql("@Gross_Amount", Gross_Amount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[9] = new PassDataToSql("@Monthly_Installment", Monthly_Installment, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[10] = new PassDataToSql("@Is_Status", Is_Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[11] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[12] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[13] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[14] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[15] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[16] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        param[17] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[18] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        param[19] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[20] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[21] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[22] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[23] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        param[24] = new PassDataToSql("@Op_Type", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        da.execute_Sp("sp_Pay_Employee_Loan_Insert", param, ref trns);
        return Convert.ToInt32(param[23].ParaValue);
    }

    public int Insert_In_Pay_Employee_Loan_Request(string Company_Id, string Emp_Id, string Loan_Name, string Loan_Request_Date, string Loan_Amount, string Is_Status, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] param = new PassDataToSql[25];
        param[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Loan_Name", Loan_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Loan_Request_Date", Loan_Request_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[4] = new PassDataToSql("@Loan_Approval_Date", Loan_Request_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Loan_Amount", Loan_Amount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Loan_Duration", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[7] = new PassDataToSql("@Loan_Interest", "0", PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[8] = new PassDataToSql("@Gross_Amount", "0", PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[9] = new PassDataToSql("@Monthly_Installment", "0", PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[10] = new PassDataToSql("@Is_Status", Is_Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[11] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[12] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[13] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[14] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[15] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[16] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        param[17] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[18] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        param[19] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[20] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[21] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[22] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[23] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        param[24] = new PassDataToSql("@Op_Type", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        da.execute_Sp("sp_Pay_Employee_Loan_Insert", param);
        return Convert.ToInt32(param[23].ParaValue);
    }

    public DataTable GetRecord_From_PayEmployeeLoanByStatus(string CompanyId, string IsStatus)
    {
        DataTable Dt = new DataTable();
        PassDataToSql[] param = new PassDataToSql[4];
        param[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[1] = new PassDataToSql("@Loan_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[2] = new PassDataToSql("@Is_Status", IsStatus, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@OpType", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        Dt = da.Reuturn_Datatable_Search("sp_Pay_Employee_Loan_SelectRow", param);
        return Dt;
    }

    public DataTable GetRecord_From_PayEmployeeLoanByStatus(string CompanyId, string IsStatus,ref SqlTransaction trns)
    {
        DataTable Dt = new DataTable();
        PassDataToSql[] param = new PassDataToSql[4];
        param[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[1] = new PassDataToSql("@Loan_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[2] = new PassDataToSql("@Is_Status", IsStatus, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@OpType", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        Dt = da.Reuturn_Datatable_Search("sp_Pay_Employee_Loan_SelectRow", param,ref trns);
        return Dt;
    }

    public static DataTable GetLoanName(string prefixText, string comp,string strConString)
    {
        DataAccessClass da = new DataAccessClass(strConString);
        DataTable Dt = new DataTable();
        PassDataToSql[] param = new PassDataToSql[4];
        param[0] = new PassDataToSql("@Company_Id", comp, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[1] = new PassDataToSql("@Loan_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[2] = new PassDataToSql("@Is_Status", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@OpType", "3", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        Dt = da.Reuturn_Datatable_Search("sp_Pay_Employee_Loan_SelectRow", param);
        return Dt;

    }

    public DataTable Get_Employee_Loan(string Loan_Id, string Emp_Id, string Voucher_ID, string IsStatus, string Payment_Status, string Op_Type)
    {
        DataTable Dt = new DataTable();
        PassDataToSql[] param = new PassDataToSql[6];
        param[0] = new PassDataToSql("@Loan_Id", Loan_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Voucher_ID", Voucher_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@IsStatus", IsStatus, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[4] = new PassDataToSql("@Payment_Status", Payment_Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Op_Type", Op_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        Dt = da.Reuturn_Datatable_Search("Get_Employee_Loan", param);
        return Dt;
    }

    public DataTable Get_Employee_Loan(string Loan_Id, string Emp_Id, string Voucher_ID, string IsStatus, string Payment_Status, string Op_Type,ref SqlTransaction trns)
    {
        DataTable Dt = new DataTable();
        PassDataToSql[] param = new PassDataToSql[6];
        param[0] = new PassDataToSql("@Loan_Id", Loan_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Voucher_ID", Voucher_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@IsStatus", IsStatus, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[4] = new PassDataToSql("@Payment_Status", Payment_Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Op_Type", Op_Type, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        Dt = da.Reuturn_Datatable_Search("Get_Employee_Loan", param, ref trns);
        return Dt;
    }

    public DataTable GetRecord_From_PayEmployeeLoan_usingLoanId(string CompanyId, string LoanId, string IsStatus)
    {
        DataTable Dt = new DataTable();
        PassDataToSql[] param = new PassDataToSql[4];
        param[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[1] = new PassDataToSql("@Loan_Id", LoanId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[2] = new PassDataToSql("@Is_Status", IsStatus, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@OpType", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        Dt = da.Reuturn_Datatable_Search("Sp_Pay_Employee_Loan_SelectRow", param);
        return Dt;

    }

    public DataTable GetRecord_From_PayEmployeeLoan_usingLoanId(string CompanyId, string LoanId, string IsStatus,ref SqlTransaction trns)
    {
        DataTable Dt = new DataTable();
        PassDataToSql[] param = new PassDataToSql[4];
        param[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[1] = new PassDataToSql("@Loan_Id", LoanId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[2] = new PassDataToSql("@Is_Status", IsStatus, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@OpType", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        Dt = da.Reuturn_Datatable_Search("Sp_Pay_Employee_Loan_SelectRow", param,ref trns);
        return Dt;

    }
    public int DeleteRecord_in_Pay_Employee_Loan(string companyId, string LoanId, string IsStatus, string ModifiedBy, string Modifieddate)
    {

        PassDataToSql[] param = new PassDataToSql[6];
        param[0] = new PassDataToSql("@Company_Id", companyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Loan_Id", LoanId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Is_Status", IsStatus, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[4] = new PassDataToSql("@ModifiedDate", Modifieddate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        //param[0] = new PassDataToSql("@Company_id",CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        //param[0] = new PassDataToSql("@Company_id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        da.execute_Sp("sp_Pay_Employee_Loan_RowStatus", param);
        return Convert.ToInt32(param[5].ParaValue);

    }
    public int UpdateRecord_In_Pay_Employee_Loan(string CompanyId, string LoanId, string LoanAmount, string Loan_Approval_Date, string LoanDuration, string LoanIntereset, string GrossAmount, string MonthlyInstallment, string IsStatus, string ModifiedBy, string Modifieddate)
    {

        PassDataToSql[] param = new PassDataToSql[12];
        param[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Loan_Id", LoanId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Loan_Approval_Date", Loan_Approval_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Loan_Duration", LoanDuration, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[4] = new PassDataToSql("@Loan_Interest", LoanIntereset, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Gross_Amount", GrossAmount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Monthly_Installment", MonthlyInstallment, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[7] = new PassDataToSql("@Is_Status", IsStatus, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[8] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[9] = new PassDataToSql("@ModifiedDate", Modifieddate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[10] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        param[11] = new PassDataToSql("@Loan_Amount", LoanAmount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        //param[0] = new PassDataToSql("@Company_id",CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        //param[0] = new PassDataToSql("@Company_id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        da.execute_Sp("sp_Pay_Employee_Loan_Update", param);
        return Convert.ToInt32(param[10].ParaValue);
    }

    public int UpdateRecord_In_Pay_Employee_Loan(string CompanyId, string LoanId, string LoanAmount,string Loan_Request_Date, string Loan_Approval_Date, string LoanDuration, string LoanIntereset, string GrossAmount, string MonthlyInstallment, string IsStatus, string ModifiedBy, string Modifieddate,ref SqlTransaction trns)
    {

        PassDataToSql[] param = new PassDataToSql[13];
        param[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Loan_Id", LoanId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Loan_Approval_Date", Loan_Approval_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Loan_Duration", LoanDuration, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[4] = new PassDataToSql("@Loan_Interest", LoanIntereset, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Gross_Amount", GrossAmount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Monthly_Installment", MonthlyInstallment, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[7] = new PassDataToSql("@Is_Status", IsStatus, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[8] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[9] = new PassDataToSql("@ModifiedDate", Modifieddate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[10] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        param[11] = new PassDataToSql("@Loan_Amount", LoanAmount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[12] = new PassDataToSql("@Loan_Request_date", Loan_Request_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        

        da.execute_Sp("sp_Pay_Employee_Loan_Update", param,ref trns);
        return Convert.ToInt32(param[10].ParaValue);
    }
    public int UpdateStatus_In_Pay_Employee_Loan(string CompanyId, string LoanId, string Loan_Approval_Date, string IsStatus, string ModifiedBy, string Modifieddate)
    {

        PassDataToSql[] param = new PassDataToSql[7];
        param[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Loan_Id", LoanId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Loan_Approval_Date", Loan_Approval_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        param[3] = new PassDataToSql("@Is_Status", IsStatus, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[4] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@ModifiedDate", Modifieddate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        //param[0] = new PassDataToSql("@Company_id",CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        //param[0] = new PassDataToSql("@Company_id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        da.execute_Sp("sp_Pay_Employee_Loan_UpdateStatus", param);
        return Convert.ToInt32(param[6].ParaValue);
    }

    public int UpdateStatus_In_Pay_Employee_Loan(string CompanyId, string LoanId, string Loan_Approval_Date, string IsStatus, string ModifiedBy, string Modifieddate,ref SqlTransaction trns)
    {

        PassDataToSql[] param = new PassDataToSql[7];
        param[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Loan_Id", LoanId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Loan_Approval_Date", Loan_Approval_Date, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);

        param[3] = new PassDataToSql("@Is_Status", IsStatus, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[4] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@ModifiedDate", Modifieddate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        //param[0] = new PassDataToSql("@Company_id",CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        //param[0] = new PassDataToSql("@Company_id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        da.execute_Sp("sp_Pay_Employee_Loan_UpdateStatus", param,ref trns);
        return Convert.ToInt32(param[6].ParaValue);
    }

    public int UpdateRecord_loandetials_Amt(string CompanyId, string LoanId, string trnsid, string prvbalnce, string totalamt, string Emppaidamt)
    {

        PassDataToSql[] param = new PassDataToSql[9];
        param[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Loan_Id", LoanId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Trans_Id", trnsid, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Prv_Balance", prvbalnce, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[4] = new PassDataToSql("@total_amount", totalamt, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Employee_PaidAm", Emppaidamt, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Sttus", "Pending", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[7] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        param[8] = new PassDataToSql("@optyp", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        da.execute_Sp("sp_Pay_Employee_LoanDetail_Update", param);
        return Convert.ToInt32(param[8].ParaValue);
    }

    public int UpdateRecord_loandetials_Amt(string CompanyId, string LoanId, string trnsid, string prvbalnce, string totalamt, string Emppaidamt,ref SqlTransaction trns)
    {

        PassDataToSql[] param = new PassDataToSql[9];
        param[0] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Loan_Id", LoanId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Trans_Id", trnsid, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Prv_Balance", prvbalnce, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[4] = new PassDataToSql("@total_amount", totalamt, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Employee_PaidAm", Emppaidamt, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Sttus", "Pending", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[7] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        param[8] = new PassDataToSql("@optyp", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        da.execute_Sp("sp_Pay_Employee_LoanDetail_Update", param, ref trns);
        return Convert.ToInt32(param[8].ParaValue);
    }

    public int Update_Pay_Employee_Loan_Detail(string Company_Id, string Loan_Id, string Trans_Id, string Month, string Year, string Prv_Balance, string Monthly_Installment, string Total_Amount, string Employee_Paid, string Is_Status, string OpType)
    {
        PassDataToSql[] param = new PassDataToSql[12];
        param[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Loan_Id", Loan_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Month", Month, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[4] = new PassDataToSql("@Year", Year, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Prv_Balance", Prv_Balance, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Monthly_Installment", Monthly_Installment, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[7] = new PassDataToSql("@Total_Amount", Total_Amount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[8] = new PassDataToSql("@Employee_Paid", Employee_Paid, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[9] = new PassDataToSql("@Is_Status", Is_Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[10] = new PassDataToSql("@OpType", OpType, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[11] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);        
        da.execute_Sp("Update_Pay_Employee_Loan_Detail", param);
        return Convert.ToInt32(param[11].ParaValue);
    }

    public int Update_Pay_Employee_Loan_Detail(string Company_Id, string Loan_Id, string Trans_Id, string Month, string Year, string Prv_Balance, string Monthly_Installment, string Total_Amount, string Employee_Paid, string Is_Status, string OpType,ref SqlTransaction trns)
    {
        PassDataToSql[] param = new PassDataToSql[12];
        param[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Loan_Id", Loan_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Trans_Id", Trans_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Month", Month, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[4] = new PassDataToSql("@Year", Year, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Prv_Balance", Prv_Balance, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Monthly_Installment", Monthly_Installment, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[7] = new PassDataToSql("@Total_Amount", Total_Amount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[8] = new PassDataToSql("@Employee_Paid", Employee_Paid, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[9] = new PassDataToSql("@Is_Status", Is_Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[10] = new PassDataToSql("@OpType", OpType, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[11] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        da.execute_Sp("Update_Pay_Employee_Loan_Detail", param, ref trns);
        return Convert.ToInt32(param[11].ParaValue);
    }

    public int UpdateRecord_loandetials_WithPaidStatusandAmount(string LoanId, string trnsid, string Emppaidamt, string status)
    {
        PassDataToSql[] param = new PassDataToSql[9];
        param[0] = new PassDataToSql("@Company_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Loan_Id", LoanId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Trans_Id", trnsid, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Prv_Balance", "0", PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[4] = new PassDataToSql("@total_amount", "0", PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Employee_PaidAm", Emppaidamt, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Sttus", status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[7] = new PassDataToSql("@optyp", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[8] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        da.execute_Sp("sp_Pay_Employee_LoanDetail_Update", param);
        return Convert.ToInt32(param[8].ParaValue);
    }

    public int UpdateRecord_loandetials_WithPaidStatusandAmount(string LoanId, string trnsid, string Emppaidamt, string status,ref SqlTransaction trns)
    {
        PassDataToSql[] param = new PassDataToSql[9];
        param[0] = new PassDataToSql("@Company_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@Loan_Id", LoanId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Trans_Id", trnsid, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Prv_Balance", "0", PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[4] = new PassDataToSql("@total_amount", "0", PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Employee_PaidAm", Emppaidamt, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Sttus", status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[7] = new PassDataToSql("@optyp", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[8] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        da.execute_Sp("sp_Pay_Employee_LoanDetail_Update", param, ref trns);
        return Convert.ToInt32(param[8].ParaValue);
    }
    public int Insert_In_Pay_Employee_LoanDetail(string LOanId, string Month, string Year, string PreviousBalance, string MonthlyInstallment, string TotalAmount, string EmployeePaid, string Is_Status, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] param = new PassDataToSql[21];
        param[0] = new PassDataToSql("@Loan_Id", LOanId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[1] = new PassDataToSql("@Month", Month, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Year", Year, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Previous_Balance", PreviousBalance, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[4] = new PassDataToSql("@Montly_Installment", MonthlyInstallment, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Total_Amount", TotalAmount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Employee_Paid", EmployeePaid, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[7] = new PassDataToSql("@Is_Status", Is_Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[8] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[9] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[10] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[11] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[12] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[13] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        param[14] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[15] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        param[16] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[17] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[18] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[19] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[20] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        //param[0] = new PassDataToSql("@Company_id",CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        //param[0] = new PassDataToSql("@Company_id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        da.execute_Sp("sp_Pay_Employee_LoanDetail_Insert", param);
        return Convert.ToInt32(param[20].ParaValue);

        //return Convert.ToInt32(param[3].ParaValue);

    }

    public int Insert_In_Pay_Employee_LoanDetail(string LOanId, string Month, string Year, string PreviousBalance, string MonthlyInstallment, string TotalAmount, string EmployeePaid, string Is_Status, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string IsActive, string CreatedBy, string CreatedDate, string ModifiedBy, string ModifiedDate,ref SqlTransaction trns)
    {
        PassDataToSql[] param = new PassDataToSql[21];
        param[0] = new PassDataToSql("@Loan_Id", LOanId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        param[1] = new PassDataToSql("@Month", Month, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[2] = new PassDataToSql("@Year", Year, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[3] = new PassDataToSql("@Previous_Balance", PreviousBalance, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[4] = new PassDataToSql("@Montly_Installment", MonthlyInstallment, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[5] = new PassDataToSql("@Total_Amount", TotalAmount, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);
        param[6] = new PassDataToSql("@Employee_Paid", EmployeePaid, PassDataToSql.ParaTypeList.Float, PassDataToSql.ParaDirectonList.Input);

        param[7] = new PassDataToSql("@Is_Status", Is_Status, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[8] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[9] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[10] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[11] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[12] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[13] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        param[14] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[15] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        param[16] = new PassDataToSql("@CreatedBy", CreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[17] = new PassDataToSql("@CreatedDate", CreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[18] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        param[19] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        param[20] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        //param[0] = new PassDataToSql("@Company_id",CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        //param[0] = new PassDataToSql("@Company_id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        da.execute_Sp("sp_Pay_Employee_LoanDetail_Insert", param, ref trns);
        return Convert.ToInt32(param[20].ParaValue);

        //return Convert.ToInt32(param[3].ParaValue);

    }

    public DataTable GetRecord_From_PayEmployeeLoanDetailByLoanId(string LoanId)
    {
        DataTable Dt = new DataTable();
        PassDataToSql[] param = new PassDataToSql[2];
        param[0] = new PassDataToSql("@Loan_Id", LoanId.ToString(), PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        Dt = da.Reuturn_Datatable_Search("sp_Pay_Employee_LoanDetail_SelectRow", param);
        return Dt;
    }

    public DataTable GetRecord_From_PayEmployeeLoanDetailByLoanId(string LoanId,ref SqlTransaction trns)
    {
        DataTable Dt = new DataTable();
        PassDataToSql[] param = new PassDataToSql[2];
        param[0] = new PassDataToSql("@Loan_Id", LoanId.ToString(), PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        Dt = da.Reuturn_Datatable_Search("sp_Pay_Employee_LoanDetail_SelectRow", param, ref trns);
        return Dt;
    }

    public DataTable GetRecord_From_PayEmployeeLoanDetailAll()
    {
        DataTable Dt = new DataTable();
        PassDataToSql[] param = new PassDataToSql[2];
        param[0] = new PassDataToSql("@Loan_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        Dt = da.Reuturn_Datatable_Search("sp_Pay_Employee_LoanDetail_SelectRow", param);
        return Dt;
    }

    public DataTable GetRecord_From_PayEmployeeLoanDetailAll(ref SqlTransaction trns)
    {
        DataTable Dt = new DataTable();
        PassDataToSql[] param = new PassDataToSql[2];
        param[0] = new PassDataToSql("@Loan_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        Dt = da.Reuturn_Datatable_Search("sp_Pay_Employee_LoanDetail_SelectRow", param, ref trns);
        return Dt;
    }
    public int DeleteRecord_From_PayEmployeeLoanDetail(string LoanId)
    {

        PassDataToSql[] param = new PassDataToSql[2];
        param[0] = new PassDataToSql("@Loan_Id", LoanId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        da.execute_Sp("sp_Pay_Employee_LoanDetail_DeleteRow", param);
        return Convert.ToInt32(param[1].ParaValue);
    }

    public int DeleteRecord_From_PayEmployeeLoanDetail(string LoanId,ref SqlTransaction trns)
    {

        PassDataToSql[] param = new PassDataToSql[2];
        param[0] = new PassDataToSql("@Loan_Id", LoanId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        param[1] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        da.execute_Sp("sp_Pay_Employee_LoanDetail_DeleteRow", param, ref trns);
        return Convert.ToInt32(param[1].ParaValue);
    }

   
}
