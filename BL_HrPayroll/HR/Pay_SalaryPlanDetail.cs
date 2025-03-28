using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PegasusDataAccess;
using System.Data.SqlClient;
using System.Data;
/// <summary>
/// Summary description for Pay_SalaryPlanDetail
/// </summary>
public class Pay_SalaryPlanDetail
{
    Set_Deduction ObjDeduc = null;
    Set_Pay_Employee_Allow_Deduc ObjAllDeduc = null;
    DataAccessClass daClass = null;

    public Pay_SalaryPlanDetail(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
        ObjDeduc = new Set_Deduction(strConString);
        ObjAllDeduc = new Set_Pay_Employee_Allow_Deduc(strConString);
    }


    public int InsertRecord(string Header_Id, string Detail_Type, string Ref_Id, string Calculation_Method, string Value, string Amount, string Deduction_Applicable, string Field1, string Field2, string Field3, string Field4, string Field5, string Field6, string Field7, string strCreatedBy, string strCreatedDate, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[17];
        paramList[0] = new PassDataToSql("@Header_Id", Header_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Detail_Type", Detail_Type, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Ref_Id", Ref_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Calculation_Method", Calculation_Method, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Value", Value, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Amount", Amount, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@Deduction_Applicable", Deduction_Applicable, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@Field1", Field1, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[8] = new PassDataToSql("@Field2", Field2, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[9] = new PassDataToSql("@Field3", Field3, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[10] = new PassDataToSql("@Field4", Field4, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[11] = new PassDataToSql("@Field5", Field5, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[12] = new PassDataToSql("@Field6", Field6, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[13] = new PassDataToSql("@Field7", Field7, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[14] = new PassDataToSql("@CreatedBy", strCreatedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[15] = new PassDataToSql("@CreatedDate", strCreatedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[16] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Pay_SalaryPlanDetail_Insert", paramList, ref trns);
        return Convert.ToInt32(paramList[16].ParaValue);
    }


    public int DeleteDeduction_By_headerId(string Header_Id, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@Header_Id", Header_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Pay_SalaryPlanDetail_RowStatus", paramList, ref trns);
        return Convert.ToInt32(paramList[1].ParaValue);
    }




    public DataTable GetDeduction_By_headerId(string Header_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Header_Id", Header_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@optype", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Deduction_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Pay_SalaryPlanDetail_SelectRow", paramList);
        return dtInfo;
    }


    public DataTable GetDeduction_By_headerId(string Header_Id,ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Header_Id", Header_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@optype", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Deduction_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Pay_SalaryPlanDetail_SelectRow", paramList,ref trns);
        return dtInfo;
    }



    public DataTable GetApplicableAllowance_By_headerId_and_deductionId(string Header_Id, string strDeductionId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Header_Id", Header_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Deduction_Id", strDeductionId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Pay_SalaryPlanDetail_SelectRow", paramList);
        return dtInfo;
    }

    public DataTable GetApplicableAllowance_By_headerId_and_deductionId(string Header_Id, string strDeductionId,ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@Header_Id", Header_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Deduction_Id", strDeductionId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Pay_SalaryPlanDetail_SelectRow", paramList,ref trns);
        return dtInfo;
    }



    public int Insert_Allowance_Using_SalaryPlan(string Company_Id, string UserId, string strSalaryPlanId, string strEmployeeId, string strGrossAmount, double BasicSalary)
    {
        try
        {
            daClass.execute_Command("delete from Pay_Employee_Allow_Deduction where Emp_Id=" + strEmployeeId + " and type=1");
            int Return_Value = 0;
            double GrossAmt = 0;
            double RowAmount = 0;
            try
            {
                GrossAmt = Convert.ToDouble(strGrossAmount);
            }
            catch
            {

            }
            DataTable dtsalaryPlanAll = GetDeduction_By_headerId(strSalaryPlanId);
            DataTable dtsalaryPlan = new DataView(dtsalaryPlanAll, "Field1='False'", "", DataViewRowState.CurrentRows).ToTable();
            foreach (DataRow dr in dtsalaryPlan.Rows)
            {
                string strCalcmethod = string.Empty;
                if (dr["Calculation_Method"].ToString() == "Fixed")
                {
                    strCalcmethod = "1";
                    RowAmount += Convert.ToDouble(dr["Value"].ToString());
                }
                else
                {
                    strCalcmethod = "2";
                    RowAmount += (BasicSalary * Convert.ToDouble(dr["Value"].ToString())) / 100;
                }
                ObjAllDeduc.InsertPayEmpAllowDeduc(Company_Id, strEmployeeId, "1", dr["ref_id"].ToString(), "Monthly", strCalcmethod, dr["Value"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId, DateTime.Now.ToString(), UserId, DateTime.Now.ToString());
            }
            if ((GrossAmt - (BasicSalary + RowAmount)) > 0)
            {
                dtsalaryPlan = new DataView(dtsalaryPlanAll, "Field1='True'", "", DataViewRowState.CurrentRows).ToTable();
                if (dtsalaryPlan.Rows.Count > 0)
                {
                    Return_Value = ObjAllDeduc.InsertPayEmpAllowDeduc(Company_Id, strEmployeeId, "1", dtsalaryPlan.Rows[0]["Ref_Id"].ToString(), "Monthly", "1", (GrossAmt - (BasicSalary + RowAmount)).ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), UserId, DateTime.Now.ToString(), UserId, DateTime.Now.ToString());
                }
            }
            return Return_Value;
        }
        catch
        {
            return 0;
        }
    }

    //public string GetDeductionIdbyDeductionType(string Company_Id, string strType)
    //{
    //    string strDeductionId = "0";
    //    DataTable dt = ObjDeduc.GetDeductionTrueAll(Company_Id);
    //    dt = new DataView(dt, "Field1='" + strType + "'", "", DataViewRowState.CurrentRows).ToTable();
    //    if (dt.Rows.Count > 0)
    //    {
    //        strDeductionId = dt.Rows[0]["Deduction_Id"].ToString();
    //    }
    //    return strDeductionId;
    //}

    //public int Insert_Deductions_Using_SalaryPlan(string Company_Id, string UserId, string strSalaryPlanId, string strEmployeeId, string strGrossAmount, double BasicSalary)
    //{
    //    int Return_Value = 0;
    //    string PF_ID = GetDeductionIdbyDeductionType(Company_Id, "PF");
    //    string ESIC_ID = GetDeductionIdbyDeductionType(Company_Id, "ESIC");
    //    string TDS_ID = GetDeductionIdbyDeductionType(Company_Id, "TDS");
    //    string PT_ID = GetDeductionIdbyDeductionType(Company_Id, "PT");
    //    //PF
    //    daClass.execute_Command("delete from Pay_Employee_Allow_Deduction where Emp_Id=" + strEmployeeId + " and type=2 and ref_id=" + PF_ID + "");
    //    //ESIC
    //    daClass.execute_Command("delete from Pay_Employee_Allow_Deduction where Emp_Id=" + strEmployeeId + " and type=2 and ref_id=" + ESIC_ID + "");
    //    //TDS
    //    daClass.execute_Command("delete from Pay_Employee_Allow_Deduction where Emp_Id=" + strEmployeeId + " and type=2 and ref_id=" + TDS_ID + "");
    //    //PT
    //    daClass.execute_Command("delete from Pay_Employee_Allow_Deduction where Emp_Id=" + strEmployeeId + " and type=2 and ref_id=" + PT_ID + "");

    //    return Return_Value;
    //}
}