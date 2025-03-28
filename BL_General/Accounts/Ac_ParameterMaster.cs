using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PegasusDataAccess;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Ac_ParameterMaster
/// </summary>
public class Ac_ParameterMaster
{
    DataAccessClass daClass = null;
    LocationMaster objLocationMaster = null;
    SystemParameter objSys = null;
    private string _strConString = string.Empty;
    public string supplierAcNo = "0";
    public string customerAcNo = "0";
    public Ac_ParameterMaster(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
        objLocationMaster = new LocationMaster(strConString);
        objSys = new SystemParameter(strConString);
        _strConString = strConString;
    }



    public static string GetAccountNameByTransId(string strAccountNo,string strConString,string strCompId)
    {
        DataAccessClass daClass = new DataAccessClass(strConString);
        string strAccountName = string.Empty;
        if (strAccountNo != "0" && strAccountNo != "")
        {
            string sql = "select AccountName,Trans_Id from Ac_ChartOfAccount where Company_Id=" + strCompId + " and Trans_Id=" + strAccountNo + " and IsActive='True'";

            DataTable dtAccName = daClass.return_DataTable(sql);
            if (dtAccName.Rows.Count > 0)
            {
                strAccountName = dtAccName.Rows[0]["AccountName"].ToString() + "/" + strAccountNo;
            }
        }
        else
        {
            strAccountName = "";
        }
        return strAccountName;
    }


    public static string GetSupplierAccountNo(string strCompany_Id,string strConString)
    {

        Ac_ParameterMaster objAc = new Ac_ParameterMaster(strConString);
        string strSuppAc = string.Empty;


        DataTable dt = objAc.GetParameterValue_By_ParameterName(strCompany_Id, "Payment Vouchers");
        if (dt.Rows.Count > 0)
        {
            strSuppAc = dt.Rows[0]["Param_Value"].ToString();
        }

        return strSuppAc;


    }

    public  string GetSupplierAccountNo1(string strCompany_Id, ref SqlTransaction trns)
    {

      //  Ac_ParameterMaster objAc = new Ac_ParameterMaster(trns.Connection.ConnectionString);
        string strSuppAc = string.Empty;


        DataTable dt = GetParameterValue_By_ParameterName(strCompany_Id, "Payment Vouchers", ref trns);
        if (dt.Rows.Count > 0)
        {
            strSuppAc = dt.Rows[0]["Param_Value"].ToString();
        }

        return strSuppAc;

    }

    public static string GetSupplierAccountNo(string strCompany_Id,ref SqlTransaction trns)
    {

        Ac_ParameterMaster objAc = new Ac_ParameterMaster(trns.Connection.ConnectionString);
        string strSuppAc = string.Empty;


        DataTable dt = objAc.GetParameterValue_By_ParameterName(strCompany_Id, "Payment Vouchers",ref trns);
        if (dt.Rows.Count > 0)
        {
            strSuppAc = dt.Rows[0]["Param_Value"].ToString();
        }

        return strSuppAc;

    }
    //Created by Ghanshyam Suthar on 08/12/2017
    public static string Get_Other_Account_Name(string strContactId, string Account_no, string Company_Id,string strConString)
    {
        DataAccessClass daClass = new DataAccessClass(strConString);
        string Other_Account = string.Empty;
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@strContactId", strContactId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Account_no", Account_no, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("Get_Other_Account_Name", paramList);
        if (dtInfo == null)
            Other_Account = "";
        else if (dtInfo.Rows.Count > 0)
            Other_Account = dtInfo.Rows[0][0].ToString();
        return Other_Account;
    }
    // End

    public static string GetOtherAccountNameForDetail(string strOtherAccountId, string Account_no,string Company_Id,string strConString)
    {
        DataAccessClass ObjDa = new DataAccessClass(strConString);
        string strAccountName = string.Empty;
        if (strOtherAccountId == "0" || strOtherAccountId == string.Empty)
        {
            return strAccountName;
        }
        string strCustomerAcNo = Ac_ParameterMaster.GetCustomerAccountNo(Company_Id, strConString);
        string strSupplierAcNo = Ac_ParameterMaster.GetSupplierAccountNo(Company_Id, strConString);
        string strEmployeeAcNo = Ac_ParameterMaster.GetEmployeeAccountNo(Company_Id, strConString);
        string strVehicleAcNo = Ac_ParameterMaster.GetVehicleAccountNo(Company_Id, strConString);
        string strEmpLoanAcNo = Ac_ParameterMaster.GetEmployeeLoanAccountNo(Company_Id,strConString);
        string sql = string.Empty;
        if (Account_no == strCustomerAcNo || Account_no == strSupplierAcNo)
        {
            sql = "select Ems_ContactMaster.Name + '(' + Sys_CurrencyMaster.Currency_Name + ')' as Name from ac_accountMaster inner join Sys_CurrencyMaster on Sys_CurrencyMaster.Currency_ID=Ac_AccountMaster.Currency_Id inner join ems_contactMaster on ems_contactMaster.Trans_Id=ac_accountMaster.Ref_Id where Ac_AccountMaster.Trans_Id='" + strOtherAccountId + "'";
        }
        else if (Account_no == strEmployeeAcNo || Account_no == strEmpLoanAcNo)
        {
            sql = "Select Emp_Name as Name from Set_EmployeeMaster where Emp_Id = '" + strOtherAccountId + "'";
        }
        else if (Account_no == strVehicleAcNo)
        {
            sql = "Select Name from Prj_VehicleMaster where Vehicle_Id ='" + strOtherAccountId + "'";
        }
        if (sql != string.Empty)
        {            
            strAccountName = ObjDa.get_SingleValue(sql);
        }

        return strAccountName;
    }

    public  string GetCustomerAccountNo1(string strCompany_Id, ref SqlTransaction trns)
    {
        string customerAcNo = "0";
        
        DataTable dt = GetParameterValue_By_ParameterName(strCompany_Id, "Receive Vouchers",ref trns);
        if (dt.Rows.Count > 0)
        {
            customerAcNo = dt.Rows[0]["Param_Value"].ToString();
        }
        return customerAcNo;
    }
    public static string GetCustomerAccountNo(string strCompany_Id, string strConString)
    {
        string customerAcNo = "0";
        Ac_ParameterMaster objAc = new Ac_ParameterMaster(strConString);
        DataTable dt = objAc.GetParameterValue_By_ParameterName(strCompany_Id, "Receive Vouchers");
        if (dt.Rows.Count > 0)
        {
            customerAcNo = dt.Rows[0]["Param_Value"].ToString();
        }
        return customerAcNo;
    }

    public static string GetCustomerAccountNo(string strCompany_Id,ref SqlTransaction trns)
    {

        Ac_ParameterMaster objAc = new Ac_ParameterMaster(trns.Connection.ConnectionString);
        string customerAcNo = string.Empty;


        DataTable dt = objAc.GetParameterValue_By_ParameterName(strCompany_Id, "Receive Vouchers",ref trns);
        if (dt.Rows.Count > 0)
        {
            customerAcNo = dt.Rows[0]["Param_Value"].ToString();
        }


        return customerAcNo;
    }

    public static string GetEmployeeAccountNo(string strCompany_Id,string strConString)
    {

        Ac_ParameterMaster objAc = new Ac_ParameterMaster(strConString);
        string employeeAcNo = string.Empty;


        DataTable dt = objAc.GetParameterValue_By_ParameterName(strCompany_Id, "Employee Account");
        if (dt.Rows.Count > 0)
        {
            employeeAcNo = dt.Rows[0]["Param_Value"].ToString();
        }


        return employeeAcNo;
    }



    public static string GetEmployeeLoanAccountNo(string strCompany_Id, string strConString)
    {

        Ac_ParameterMaster objAc = new Ac_ParameterMaster(strConString);
        string employeeAcNo = string.Empty;


        DataTable dt = objAc.GetParameterValue_By_ParameterName(strCompany_Id, "Employee Loan Account");
        if (dt.Rows.Count > 0)
        {
            employeeAcNo = dt.Rows[0]["Param_Value"].ToString();
        }


        return employeeAcNo;
    }


    public static string GetEmployeeAccountNo(string strCompany_Id, ref SqlTransaction trns)
    {

        Ac_ParameterMaster objAc = new Ac_ParameterMaster(trns.Connection.ConnectionString);
        string employeeAcNo = string.Empty;


        DataTable dt = objAc.GetParameterValue_By_ParameterName(strCompany_Id, "Employee Account", ref trns);
        if (dt.Rows.Count > 0)
        {
            employeeAcNo = dt.Rows[0]["Param_Value"].ToString();
        }


        return employeeAcNo;
    }

    public static string GetVehicleAccountNo(string strCompany_Id, string strConString)
    {

        Ac_ParameterMaster objAc = new Ac_ParameterMaster(strConString);
        string vehicleAcNo = string.Empty;


        DataTable dt = objAc.GetParameterValue_By_ParameterName(strCompany_Id, "Vehicle Account");
        if (dt.Rows.Count > 0)
        {
            vehicleAcNo = dt.Rows[0]["Param_Value"].ToString();
        }


        return vehicleAcNo;
    }

    public static string GetVehicleAccountNo(string strCompany_Id, ref SqlTransaction trns)
    {

        Ac_ParameterMaster objAc = new Ac_ParameterMaster(trns.Connection.ConnectionString);
        string vehicleAcNo = string.Empty;


        DataTable dt = objAc.GetParameterValue_By_ParameterName(strCompany_Id, "Vehicle Account", ref trns);
        if (dt.Rows.Count > 0)
        {
            vehicleAcNo = dt.Rows[0]["Param_Value"].ToString();
        }


        return vehicleAcNo;
    }


    public Ac_ParameterMaster(string strCompany_Id,ref SqlTransaction trns)
    {
        DataTable dt = GetParameterValue_By_ParameterName(strCompany_Id, "Receive Vouchers");
        if (dt.Rows.Count > 0)
        {
            customerAcNo = dt.Rows[0]["Param_Value"].ToString();
        }

        dt = GetParameterValue_By_ParameterName(strCompany_Id, "Payment Vouchers");
        if (dt.Rows.Count > 0)
        {
            supplierAcNo = dt.Rows[0]["Param_Value"].ToString();
        }
        //
        // TODO: Add constructor logic here
        //
    }

    public int InsertRecord(string strCompany_Id, string strParamName, string strParamvalue, string strDescription, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[17];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Param_Name", strParamName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Param_Value", strParamvalue, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Description", strDescription, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field3", strField3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Field4", strField4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field5", strField5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field6", strField6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field7", strField7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Ac_Parameter_Insert", paramList);
        return Convert.ToInt32(paramList[16].ParaValue);
    }
    //Created for Rollback Transaction
    //26-02-2016
    //By Lokesh
    public int InsertRecord(string strCompany_Id, string strParamName, string strParamvalue, string strDescription, string strField1, string strField2, string strField3, string strField4, string strField5, string strField6, string strField7, string strIsActive, string strCreatedBy, string strCreatedDate, string strModifiedBy, string strModifiedDate, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[17];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Param_Name", strParamName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Param_Value", strParamvalue, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Description", strDescription, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Field1", strField1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Field2", strField2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Field3", strField3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Field4", strField4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field5", strField5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field6", strField6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field7", strField7, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@IsActive", strIsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@ModifiedBy", strModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@ModifiedDate", strModifiedDate, PassDataToSql.ParaTypeList.Date, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Ac_Parameter_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[16].ParaValue);
    }
    public int DeleteRecord(string strCompany_Id)
    {
        PassDataToSql[] paramList = new PassDataToSql[2];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Ac_Parameter_DeleteRow", paramList);
        return Convert.ToInt32(paramList[1].ParaValue);
    }
    //Created for Rollback Transaction
    //26-02-2016
    //By Lokesh
    public int DeleteRecord(string strCompany_Id, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[2];

        paramList[0] = new PassDataToSql("@Company_Id", strCompany_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Ac_Parameter_DeleteRow", paramList, ref trns);
        return Convert.ToInt32(paramList[1].ParaValue);
    }
    public DataTable GetParameterMasterById(string Company_Id, string Parameter_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Parameter_Id", Parameter_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("Optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Param_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Parameter_SelectRow", paramList);

        return dtInfo;
    }
    public DataTable GetParameterMasterAllTrue(string Company_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Parameter_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("Optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Param_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Parameter_SelectRow", paramList);

        return dtInfo;
    }
    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay
    public DataTable GetParameterMasterAllTrue(string Company_Id, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Parameter_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("Optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Param_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Parameter_SelectRow", paramList, ref trns);

        return dtInfo;
    }
    public DataTable GetParameterValue_By_ParameterName(string Company_Id, string ParameterName)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Parameter_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("Optype", "3", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Param_Name", ParameterName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);


        dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Parameter_SelectRow", paramList);

        if (dtInfo.Rows.Count == 0)
        {
            DataRow dr = dtInfo.NewRow();
            dr["Company_Id"] = "0";
            dr["Parameter_Id"] = "0";
            dr["Param_Name"] = "0";
            dr["Param_Value"] = "0";
            dr["Field1"] = "0";
            dr["Field2"] = "0";
            dr["Field3"] = "0";
            dr["Field4"] = "0";
            dr["Field5"] = "0";
            dr["Field6"] = true.ToString();
            dr["Field7"] = DateTime.Now.ToString();
            dr["IsActive"] = true.ToString();
            dr["CreatedBy"] = "";
            dr["CreatedDate"] = DateTime.Now.ToString();
            dr["ModifiedBy"] = "";
            dr["ModifiedDate"] = DateTime.Now.ToString();
            dtInfo.Rows.Add(dr);
        }
        return dtInfo;
    }
    //Created for Rollback Transaction
    //26-02-2016
    //By Lokesh
    public DataTable GetParameterValue_By_ParameterName(string Company_Id, string ParameterName, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Parameter_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("Optype", "3", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Param_Name", ParameterName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Ac_Parameter_SelectRow", paramList, ref trns);

        return dtInfo;
    }

    public  int GetCounterforVoucherNumber1(string Company_Id, string Brand_Id, string Location_Id, string VoucherType, string Finance_Code, ref SqlTransaction trns)
    {
        
        int counter = 0;
        string strsql = "SELECT       count(*)    FROM Ac_Voucher_Header    WHERE Company_Id =" + Company_Id + "   AND Brand_Id = " + Brand_Id + "   AND Location_Id = " + Location_Id + " and voucher_type='" + VoucherType + "'  AND Finance_Code = '" + Finance_Code + "'   ";
        counter = Convert.ToInt32(daClass.return_DataTable(strsql, ref trns).Rows[0][0].ToString());
        return counter;
    }


   // public static int GetCounterforVoucherNumber(string Company_Id,string Brand_Id,string Location_Id,string VoucherType,string Finance_Code,ref SqlTransaction trns)
   // {
   //     DataAccessClass ObjDa = new DataAccessClass(trns.Connection.ConnectionString);
   //     int counter = 0;
   //     string strsql = "SELECT       count(*)    FROM Ac_Voucher_Header    WHERE Company_Id ="+Company_Id+"   AND Brand_Id = "+Brand_Id+"   AND Location_Id = "+Location_Id+" and voucher_type='"+ VoucherType + "'  AND Finance_Code = '"+Finance_Code+"'   ";
   //     counter = Convert.ToInt32(ObjDa.return_DataTable(strsql, ref trns).Rows[0][0].ToString());
   //     return counter;
   //}

    public string GetCompanyCurrency(string strToCurrency, string strLocalAmount,string strCompId, string strLocId)
    {
        string strExchangeRate = string.Empty;
        string strForeignAmount = string.Empty;
        string strCurrency = objLocationMaster.GetLocationMasterById(strCompId, strLocId).Rows[0]["Field1"].ToString();

        strExchangeRate = SystemParameter.GetExchageRate(strCurrency, strToCurrency,_strConString);
        try
        {
            strForeignAmount = objSys.GetCurencyConversionForInv(strToCurrency, (float.Parse(strExchangeRate) * float.Parse(strLocalAmount)).ToString());
            strForeignAmount = strForeignAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForeignAmount = "0";
        }
        return strForeignAmount;
    }

    public DataTable GetCustomerAsPerSearchText(string Company_Id,string strFilterText,bool bInActive=false)
    {
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@StrFilterText", strFilterText, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("Optype", bInActive==false?"1":"3", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        using (DataTable _dt = daClass.Reuturn_Datatable_Search("sp_Ac_OtherAccount_SelectPreText", paramList))
        {
            return _dt;
        }
    }
    
    public DataTable GetSupplierAsPerSearchText(string Company_Id, string strFilterText)
    {
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@StrFilterText", strFilterText, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("Optype", "2", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        using (DataTable _dt = daClass.Reuturn_Datatable_Search("sp_Ac_OtherAccount_SelectPreText", paramList))
        {
            return _dt;
        }
    }
}