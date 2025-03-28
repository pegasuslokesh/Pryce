using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using PegasusDataAccess;
using System.Resources;
using System.IO;
using System.Collections;
using System.Data.SqlClient;
using System.Resources;
using System.Windows;
using System.Text;
/// <summary>
/// Summary description for SystemParameter
/// </summary>
public class SystemParameter
{
    DataAccessClass daClass = null;
    EmployeeParameter objempparam = null;
    CurrencyMaster ObjCurrency = null;
    Country_Currency objCountryCurrency = null;

    
    Set_ApplicationParameter objAppParam = null;
    public SystemParameter(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
        EmployeeParameter objempparam = new EmployeeParameter(strConString);
        ObjCurrency = new CurrencyMaster(strConString);
        objCountryCurrency = new Country_Currency(strConString);
        objAppParam = new Set_ApplicationParameter(strConString);
        
    }
    public string CommaSeprator(string str)
    {
        string strInput = str;
        try
        {
            string[] multiArray = strInput.Split(new Char[] { '.' });
            if (Convert.ToInt32(multiArray[0]) > 0)
            {
                return string.Format("{0:n}", Convert.ToDouble(multiArray[0])).Split(new[] { '.' })[0] + "." + multiArray[1];

            }
        }
        catch
        {

        }

        return str;
    }

    public int UpdateSysParameterMaster(string TransId, string Param_Name, string Param_Value, string IsActive, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Param_Name", Param_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Param_Value", Param_Value, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@TransId", TransId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[4] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Sys_Parameter_Update", paramList);
        return Convert.ToInt32(paramList[6].ParaValue);
    }

    public int UpdateSysParameterMaster(string TransId, string Param_Name, string Param_Value, string IsActive, string ModifiedBy, string ModifiedDate, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[7];

        paramList[0] = new PassDataToSql("@Param_Name", Param_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Param_Value", Param_Value, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[2] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@TransId", TransId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        paramList[4] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Sys_Parameter_Update", paramList, ref trns);
        return Convert.ToInt32(paramList[6].ParaValue);
    }



    public DataTable GetSysParameterByParamName(string ParamName)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@TransId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Param_Name", ParamName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_Parameter_SelectRow", paramList);

        return dtInfo;
    }
    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay


    public DataTable GetSysParameterByParamName(string ParamName, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@TransId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Param_Name", ParamName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_Parameter_SelectRow", paramList, ref trns);

        return dtInfo;
    }
    public DataTable GetSysParameterByParamName(string ParamName, string strConString="")
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@TransId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Param_Name", ParamName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_Parameter_SelectRow", paramList, strConString);

        return dtInfo;
    }
    public DataTable GetSysParameterAll()
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@TransId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Param_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_Parameter_SelectRow", paramList);

        return dtInfo;
    }
    public DataTable GetSysParameter_For_UserDisplay()
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@TransId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Param_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "4", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_Parameter_SelectRow", paramList);

        return dtInfo;
    }
    public DataTable GetSysParameterByTransId(string TransId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@TransId", TransId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Param_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "3", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_Parameter_SelectRow", paramList);

        return dtInfo;
    }

    // Add by Ghanshyam Suthar on 27-01-2018
    public int Update_Approval_Parameter_Master(string Approval_Id, string Approval_Name, string Approval_Level, string ModifiedBy)
    {
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Approval_Id", Approval_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Approval_Name", Approval_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Approval_Level", Approval_Level, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Optype", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("SP_Update_Approval", paramList);
        return Convert.ToInt32(paramList[5].ParaValue);
    }

    public int Set_Approval_Master_Employee_Update(string Company_Id, string Brand_Id, string Location_Id, string Department_Id, string Approval_Id, string Emp_Id, string ModifiedBy)
    {
        PassDataToSql[] paramList = new PassDataToSql[8];
        paramList[0] = new PassDataToSql("@Company_Id", Company_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_Id", Brand_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_Id", Location_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Department_Id", Department_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Approval_Id", Approval_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[7] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("Set_Approval_Master_Employee_Update", paramList);
        return Convert.ToInt32(paramList[7].ParaValue);
    }

    public DataTable Get_Approval_Parameter()
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@TransId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Param_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "5", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_Parameter_SelectRow", paramList);

        return dtInfo;
    }

    public DataTable Get_Approval_Parameter_By_ID(string TransId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@TransId", TransId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Param_Name", "", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "6", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_Parameter_SelectRow", paramList);

        return dtInfo;
    }

    public DataTable Get_Approval_Parameter_By_Name(string Approval_Name)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@TransId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Param_Name", Approval_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "7", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_Parameter_SelectRow", paramList);

        return dtInfo;
    }

    public DataTable Get_Approval_Parameter_By_Name(string Approval_Name, ref SqlTransaction trns)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@TransId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Param_Name", Approval_Name, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "7", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_Parameter_SelectRow", paramList, ref trns);

        return dtInfo;
    }
    //---------------------------------------------

    public DataTable GET_Inv_ProductTaxMaster(string Company_ID, string Brand_ID, string Location_ID, string IsActive, string OpType)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[5];
        paramList[0] = new PassDataToSql("@Company_ID", Company_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_ID", Brand_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_ID", Location_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@OpType", OpType, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("GET_Inv_ProductTaxMaster", paramList);
        return dtInfo;
    }

    public int SET_Inv_ProductTaxMaster(string Company_ID, string Brand_ID, string Location_ID, string IsActive, string ModifiedBy, string OpType, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[7];
        paramList[0] = new PassDataToSql("@Company_ID", Company_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Brand_ID", Brand_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Location_ID", Location_ID, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@OpType", OpType, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[6] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        daClass.execute_Sp("SET_Inv_ProductTaxMaster", paramList, ref trns);
        return Convert.ToInt32(paramList[6].ParaValue);
    }

    public int UpdateSystemParameterMaster(string TransId, string Param_Value, string IsActive, string ModifiedBy, string ModifiedDate)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];

        paramList[0] = new PassDataToSql("@Param_Value", Param_Value, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@IsActive", IsActive, PassDataToSql.ParaTypeList.Bit, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ModifiedBy", ModifiedBy, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@ModifiedDate", ModifiedDate, PassDataToSql.ParaTypeList.DateTime, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);

        daClass.execute_Sp("sp_Set_SystemParameter_Update", paramList);
        return Convert.ToInt32(paramList[8].ParaValue);
    }
    public string GetSysTitle()
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@TransId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Param_Name", "Application_Title", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Sys_Parameter_SelectRow", paramList);

        return dtInfo.Rows[0]["Param_Value"].ToString();
    }
    public int SetPageSize()
    {
        int size = 10;

        DataTable dtParam = new DataTable();

        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@TransId", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Param_Name", "Grid_Size", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtParam = daClass.Reuturn_Datatable_Search("sp_Sys_Parameter_SelectRow", paramList);

        try
        {
            if (dtParam.Rows.Count > 0)
            {
                size = Convert.ToInt32(dtParam.Rows[0]["Param_Value"]);
            }
        }
        catch (Exception)
        {

        }
        return size;
    }
    public DateTime getDateForInput(string givendate)
    {
        DateTime dtdate = new DateTime();
        if (givendate.ToString().Trim() != "")
        {
            string d = "";
            string m = "";
            string y = "";

            string DateFormat = string.Empty;
            DateFormat = SetDateFormat();

            if (DateFormat.Trim() == "dd-MM-yyyy")
            {
                d = givendate.Split('-')[0];
                m = givendate.Split('-')[1];
                y = givendate.Split('-')[2].Split(' ')[0];
                dtdate = new DateTime(Convert.ToInt32(y), Convert.ToInt32(m), Convert.ToInt32(d));
            }
            else if (DateFormat.Trim() == "dd-MMM-yyyy")
            {
                d = givendate.Split('-')[0];
                m = givendate.Split('-')[1];
                y = givendate.Split('-')[2];
                dtdate = Convert.ToDateTime(givendate);
            }
            else if (DateFormat.Trim() == "dd/MMM/yyyy")
            {
                d = givendate.Split('/')[0];
                m = givendate.Split('/')[1];
                y = givendate.Split('/')[2];
                dtdate = Convert.ToDateTime(givendate);
            }
            else if (DateFormat.Trim() == "dd/MM/yyyy")
            {
                d = givendate.Split('-')[0];
                m = givendate.Split('-')[1];
                y = givendate.Split('-')[2];
                dtdate = new DateTime(Convert.ToInt32(y), Convert.ToInt32(m), Convert.ToInt32(d));
            }
            else if (DateFormat.Trim() == "MM/dd/yyyy")
            {
                d = givendate.Split('/')[1];
                m = givendate.Split('/')[0];
                y = givendate.Split('/')[2];
                dtdate = new DateTime(Convert.ToInt32(y), Convert.ToInt32(m), Convert.ToInt32(d));
            }
            else if (DateFormat.Trim() == "MM/dd/yyyy")
            {
                d = givendate.Split('/')[1];
                m = givendate.Split('/')[0];
                y = givendate.Split('/')[2];
                dtdate = new DateTime(Convert.ToInt32(y), Convert.ToInt32(m), Convert.ToInt32(d));
            }
            else if (DateFormat.Trim() == "MM-dd-yyyy")
            {
                d = givendate.Split('-')[1];
                m = givendate.Split('-')[0];
                y = givendate.Split('-')[2];
                dtdate = new DateTime(Convert.ToInt32(y), Convert.ToInt32(m), Convert.ToInt32(d));
            }
            else if (DateFormat.Trim() == "MM-dd-yy")
            {
                d = givendate.Split('-')[1];
                m = givendate.Split('-')[0];
                y = givendate.Split('-')[2];
                dtdate = new DateTime(Convert.ToInt32(y), Convert.ToInt32(m), Convert.ToInt32(d));
            }
            else if (DateFormat.Trim() == "MM/dd/yy")
            {
                d = givendate.Split('-')[1];
                m = givendate.Split('-')[0];
                y = givendate.Split('-')[2];
                dtdate = new DateTime(Convert.ToInt32(y), Convert.ToInt32(m), Convert.ToInt32(d));
            }
        }
        return dtdate;
    }
    public string SetDateFormat()
    {
        string DateFormat = "dd-MMM-yyyy";
        //if (HttpContext.Current.Session["DateFormat"] != null)
        //{
        //    DateFormat = HttpContext.Current.Session["DateFormat"].ToString();
        //}
        //else
        //{
        //    DataTable dtDate = GetSysParameterByParamName("Date_Format");
        //    if (dtDate.Rows.Count > 0)
        //    {
        //        DateFormat = dtDate.Rows[0]["Param_Value"].ToString();
        //        HttpContext.Current.Session["DateFormat"] = DateFormat;
        //    }
        //}
        return DateFormat;
    }
    public string SetDateFormat(ref SqlTransaction trns)
    {
        string DateFormat = "dd-MMM-yyyy";
        DataTable dtDate = GetSysParameterByParamName("Date_Format", ref trns);
        if (dtDate.Rows.Count > 0)
        {
            DateFormat = dtDate.Rows[0]["Param_Value"].ToString();
            //HttpContext.Current.Session["DateFormat"] = DateFormat;
        }
        return DateFormat;
    }
    public DataTable GetTable()
    {
        DataTable dtTemp = new DataTable();
        dtTemp.Columns.Add(new DataColumn("Key"));
        dtTemp.Columns.Add(new DataColumn("Value"));
        return dtTemp;
    }
    
   
    //this code is created on 02-04-2014 by jitendra upadhyay
    //for convert the amount in selected currency by employee and company
    //for this work i create new function GetCurencyConversion where we pass the employeeid and amount for conversion 
    //this function return the amount after currency conversion

    //code start

    public string GetCurencyConversion(string Employee_Id, string Amount,string strCurrencyId)
    {

        string ConvertAmount = "0";

        ConvertAmount = GetCurencyConversionForInv(strCurrencyId, Amount.ToString());

        return ConvertAmount;
      
    }
    public string GetCurencyConversionForInvForm(string DecimalCount, string Amount)
    {


        if (DecimalCount.Length == 0)
        {
            DecimalCount = "3";
        }
        double Amt = 0;
        double convertedAmt = 0;
        string DotValue = string.Empty;

        if (Amount != "")
        {



            try
            {
                Amt = Convert.ToDouble(Amount);
            }
            catch
            {
                Amt = 0;
            }



            convertedAmt = Math.Round(Amt, Convert.ToInt32(DecimalCount));

            try
            {


                for (int i = 0; i < Convert.ToInt32(DecimalCount); i++)
                {
                    DotValue = DotValue + "0";

                }

            }
            catch
            {
                DotValue = "0";
            }
        }
        return convertedAmt.ToString("0." + DotValue);
    }

    public string GetCurencyConversion_For_EmployeeCurrency(string Employee_Id, string Amount,string strCompId, string strCurrencyId)
    {


        string ConvertAmount = "0";

        string EmployeeCurrency = string.Empty;
        string ExchangeRate = string.Empty;

        if (Amount != "" && Amount.Trim() != "NaN")
        {
            try
            {
                EmployeeCurrency = objempparam.GetEmployeeParameterByEmpId(Employee_Id, strCompId).Rows[0]["Currency_Id"].ToString();
            }
            catch
            {
                EmployeeCurrency = "0";
            }


            if (EmployeeCurrency != "0")
            {

                if (EmployeeCurrency != strCurrencyId)
                {
                    ExchangeRate = ObjCurrency.GetCurrencyMasterById(EmployeeCurrency).Rows[0]["Currency_Value"].ToString();

                    ConvertAmount = (float.Parse(Amount) * float.Parse(ExchangeRate)).ToString();

                    ConvertAmount = GetCurencyConversionForInv(EmployeeCurrency, ConvertAmount.ToString());


                }
                else
                {

                    ConvertAmount = GetCurencyConversionForInv(EmployeeCurrency, Amount.ToString());
                }
            }
            else
            {
                ConvertAmount = GetCurencyConversionForInv(strCurrencyId, Amount.ToString());
            }
        }


        return ConvertAmount;
     
    }

    public string GetCurencyConversionForInv(string CurrenyId, string Amount)
    {
        double Amt = 0;
        double convertedAmt = 0;
        string DotValue = string.Empty;
        DataTable dt = new DataTable();

        if (Amount != "")
        {

            dt = objCountryCurrency.GetCurrencyByCountryId(CurrenyId, "2");

            try
            {
                Amt = Convert.ToDouble(Amount);
            }
            catch
            {
                Amt = 0;
            }
           
            if (dt.Rows.Count > 0 && dt.Rows[0]["Field1"].ToString() == "")
            {

                convertedAmt = Math.Round(Amt, 0);
            }
            if (dt.Rows.Count > 0 && dt.Rows[0]["Field1"].ToString() == "0")
            {

                convertedAmt = Math.Round(Amt, 0);
            }
            if (dt.Rows.Count > 0 && dt.Rows[0]["Field1"].ToString() != "")
            {

                convertedAmt = Math.Round(Amt, Convert.ToInt32(dt.Rows[0]["Field1"].ToString()));
            }
            try
            {
                if (dt.Rows[0]["Field1"].ToString() != "")
                {

                    for (int i = 0; i < Convert.ToInt32(dt.Rows[0]["Field1"].ToString()); i++)
                    {
                        DotValue = DotValue + "0";

                    }
                }
            }
            catch
            {
                DotValue = "0";
            }
        }
        return convertedAmt.ToString("0." + DotValue);
    }
    
    public static string GetAmountInLoginLocationCurrency(string Amount,string strConString, int LoginLocDecimalCount,string locCurrencyId)
    {
        SystemParameter objSysParam = new SystemParameter(strConString);
        int decimalCount = 0;
        if (LoginLocDecimalCount == 0)
        {
            DataTable dt = new DataTable();
            dt = objSysParam.objCountryCurrency.GetCurrencyByCountryId(locCurrencyId, "2");
            int.TryParse(dt.Rows[0]["Field1"].ToString(), out decimalCount);
            LoginLocDecimalCount = decimalCount;
        }
        else
        {
            decimalCount = LoginLocDecimalCount;
        }
        double Amt = 0;
        double convertedAmt = 0;
        string DotValue = "";


        if (Amount != "")
        {
            Amt = Convert.ToDouble(Amount);
            convertedAmt = Math.Round(Amt, decimalCount);
            for (int i = 0; i < decimalCount; i++)
            {
                DotValue = DotValue + "0";

            }
        }
        if (DotValue == string.Empty)
        {
            DotValue = "0";
        }
        return convertedAmt.ToString("0." + DotValue);
    }

    public string GetCurencyConversionForFinance(string CurrenyId, string Amount)
    {
        double convertedAmt = 0;
        string DotValue = string.Empty;
        DataTable dt = new DataTable();
        if (Amount != "" && Amount != "0")
        {

            dt = objCountryCurrency.GetCurrencyByCountryId(CurrenyId, "2");
            //double Amt = Convert.ToDouble(Amount);
            convertedAmt = Convert.ToDouble(Amount);
            //if (dt.Rows.Count > 0 && dt.Rows[0]["Field1"].ToString() == "")
            //{

            //    convertedAmt = Math.Round(Amt, 0);
            //}
            //if (dt.Rows.Count > 0 && dt.Rows[0]["Field1"].ToString() == "0")
            //{

            //    convertedAmt = Math.Round(Amt, 0);
            //}
            //if (dt.Rows.Count > 0 && dt.Rows[0]["Field1"].ToString() != "")
            //{

            //    convertedAmt = Math.Round(Amt, Convert.ToInt32(dt.Rows[0]["Field1"].ToString()));
            //}
            try
            {
                if (dt.Rows[0]["Field1"].ToString() != "")
                {

                    for (int i = 0; i < Convert.ToInt32(dt.Rows[0]["Field1"].ToString()); i++)
                    {
                        DotValue = DotValue + "0";
                    }
                }
            }
            catch
            {
                DotValue = "0";
            }
        }
        return convertedAmt.ToString("0." + DotValue);
    }
    //created for rollback Trsnaction
    //16-02-2016
    //by jitendra upadhyay
    public string GetCurencyConversionForInv(string CurrenyId, string Amount, ref SqlTransaction trns)
    {

        double convertedAmt = 0;
        string DotValue = string.Empty;
        DataTable dt = new DataTable();
        if (Amount != "")
        {

            dt = objCountryCurrency.GetCurrencyByCountryId(CurrenyId, "2");
            double Amt = Convert.ToDouble(Amount);
            if (dt.Rows.Count > 0 && dt.Rows[0]["Field1"].ToString() == "")
            {

                convertedAmt = Math.Round(Amt, 0);
            }
            if (dt.Rows.Count > 0 && dt.Rows[0]["Field1"].ToString() == "0")
            {

                convertedAmt = Math.Round(Amt, 0);
            }
            if (dt.Rows.Count > 0 && dt.Rows[0]["Field1"].ToString() != "")
            {

                convertedAmt = Math.Round(Amt, Convert.ToInt32(dt.Rows[0]["Field1"].ToString()));
            }
            try
            {
                if (dt.Rows[0]["Field1"].ToString() != "")
                {

                    for (int i = 0; i < Convert.ToInt32(dt.Rows[0]["Field1"].ToString()); i++)
                    {
                        DotValue = DotValue + "0";

                    }
                }
            }
            catch
            {
                DotValue = "0";
            }
        }
        return convertedAmt.ToString("0." + DotValue);
    }

    //code end
    //this code is created on 03-04-2014 by jitendra upadhyay
    //this function is used for get the currency symbol

    public string GetCurencySymbol(string Employee_Id,string strCurrencyId, string strCompId)
    {

        string CurrencySymbol = string.Empty;
        string CompanyCurrency = string.Empty;
        string EmployeeCurrency = string.Empty;



        CompanyCurrency = strCurrencyId;
        try
        {

            EmployeeCurrency = ObjCurrency.GetCurrencyMasterById(objempparam.GetEmployeeParameterByEmpId(Employee_Id, strCompId).Rows[0]["Currency_Id"].ToString()).Rows[0]["Currency_Name"].ToString();
        }
        catch
        {

        }
        return CurrencySymbol;
    }
    public string GetCurencySymbol_For_EmployeeCurrency(string Employee_Id, string strCurrencyId, string strCompId)
    {

        string CurrencySymbol = string.Empty;
        string CompanyCurrency = string.Empty;
        string EmployeeCurrency = string.Empty;



        CompanyCurrency = strCurrencyId;
        try
        {

            EmployeeCurrency = ObjCurrency.GetCurrencyMasterById(objempparam.GetEmployeeParameterByEmpId(Employee_Id, strCompId).Rows[0]["Currency_Id"].ToString()).Rows[0]["Currency_Name"].ToString();
        }
        catch
        {

        }


        if (EmployeeCurrency != "")
        {

            DataTable DtempCurrencyValue = ObjCurrency.GetCurrencyMasterByCurrencyNames(EmployeeCurrency);
            if (DtempCurrencyValue.Rows.Count > 0)
            {
                CurrencySymbol = DtempCurrencyValue.Rows[0]["Currency_Symbol"].ToString();

            }
            else
            {

                DataTable DtCompCurrencyValue = ObjCurrency.GetCurrencyMasterByCurrencyNames(CompanyCurrency);
                if (DtCompCurrencyValue.Rows.Count > 0)
                {
                    CurrencySymbol = DtCompCurrencyValue.Rows[0]["Currency_Symbol"].ToString();
                }
            }
        }
        else
        {
            DataTable DtCompCurrencyValue = ObjCurrency.GetCurrencyMasterByCurrencyNames(CompanyCurrency);
            if (DtCompCurrencyValue.Rows.Count > 0)
            {
                CurrencySymbol = DtCompCurrencyValue.Rows[0]["Currency_Symbol"].ToString();
            }
        }
        return CurrencySymbol;
    }
    public static string GetCurrencySmbol(string StrCurrencyId, string StrAmount,string strConString)
    {
        CurrencyMaster ObjCurrency = new CurrencyMaster(strConString);
        try
        {
            //return StrAmount.ToString() + "(" + ObjCurrency.GetCurrencyMasterById(StrCurrencyId).Rows[0]["Currency_Symbol"].ToString() + ")";

            return ObjCurrency.GetCurrencyMasterById(StrCurrencyId).Rows[0]["Currency_Symbol"].ToString() + " " + StrAmount.ToString();

        }
        catch
        {
            return StrAmount;
        }
    }
    public int Restoredatabase(string databasename, string path,string strConString)
    {
        string mdfpath = string.Empty;
        string ldfpath = string.Empty;
        string query = string.Empty;
        query = "SELECT SUBSTRING(physical_name, 1, CHARINDEX(N'master.mdf', LOWER(physical_name)) - 1) as Path FROM master.sys.master_files WHERE database_id ='1' AND file_id = '1'";

        DataTable dtpath = daClass.return_DataTable(query);

        if (dtpath.Rows.Count > 0)
        {
            mdfpath = dtpath.Rows[0]["Path"].ToString();
            ldfpath = dtpath.Rows[0]["Path"].ToString();

            mdfpath += databasename + ".mdf";
            ldfpath += databasename + ".ldf";

        }

        System.Data.SqlClient.SqlConnection sqlconn = new System.Data.SqlClient.SqlConnection();
        System.Data.SqlClient.SqlCommand sqlcmd = new System.Data.SqlClient.SqlCommand();
        sqlconn.ConnectionString = strConString;
        if (sqlconn.State == ConnectionState.Closed)
        {
            sqlconn.Open();
        }
        sqlcmd.Connection = sqlconn;
        sqlcmd.CommandType = CommandType.StoredProcedure;
        sqlcmd.CommandText = "sp_Restore_database";
        sqlcmd.CommandTimeout = 600;
        sqlcmd.Parameters.Add("@database_name", SqlDbType.NVarChar).Value = databasename;
        sqlcmd.Parameters.Add("@path1", SqlDbType.NVarChar).Value = path;
        sqlcmd.Parameters.Add("@Mdfpath", SqlDbType.NVarChar).Value = mdfpath;
        sqlcmd.Parameters.Add("@ldfpath", SqlDbType.NVarChar).Value = ldfpath;
        sqlcmd.Parameters.Add("@Refference_id", SqlDbType.Int).Value = 0;
        sqlcmd.ExecuteNonQuery();

        return 1;
    }
    public static string GetScaleAmount(string Amount, string Scale)
    {
        string ScaleAmount = string.Empty;

        if (Amount == "" || Amount == "0")
        {
            ScaleAmount = "0";
        }
        else
        {
            try
            {
                double result = 0.0;
                string DotValue = string.Empty;
                string s = Amount.Substring(Amount.Length - 1);
                int lastindexValue = Convert.ToInt32(s);
                bool IsScaleFound = false;
                int[] MArray = new int[10];

                if (Scale == "0")
                {
                    MArray[0] = 0;
                    MArray[1] = -1;
                    MArray[2] = -2;
                    MArray[3] = -3;
                    MArray[4] = -4;
                    MArray[5] = 5;
                    MArray[6] = 4;
                    MArray[7] = 3;
                    MArray[8] = 2;
                    MArray[9] = 1;
                    IsScaleFound = true;
                }
                else if (Scale == "5")
                {
                    MArray[0] = 0;
                    MArray[1] = -1;
                    MArray[2] = -2;
                    MArray[3] = +2;
                    MArray[4] = +1;
                    MArray[5] = 0;
                    MArray[6] = -1;
                    MArray[7] = -2;
                    MArray[8] = +2;
                    MArray[9] = +1;
                    IsScaleFound = true;

                }

                if (IsScaleFound)
                {

                    int decimalcount = 0;
                    try
                    {

                        decimalcount = Amount.Split('.')[1].Length;
                    }
                    catch
                    {

                    }

                    double Powervalue = Math.Pow(10, decimalcount);

                    double mval = (1 / Powervalue) * Convert.ToDouble(MArray[lastindexValue]);

                    result = Convert.ToDouble(Amount) + mval;

                    for (int i = 0; i < decimalcount; i++)
                    {
                        DotValue = DotValue + "0";

                    }

                    ScaleAmount = result.ToString("0." + DotValue);
                }
                else
                {
                    ScaleAmount = Amount;
                }
            }
            catch
            {
                ScaleAmount = Amount;
            }
        }
        return ScaleAmount;
    }

    public static string GetExchageRate(string strFromCurrency, string strToCurrency,string strConString)
    {
        string strExchangeRate = string.Empty;

        SystemParameter objSysParam = new SystemParameter(strConString);
        CurrencyMaster ObjCurrency = new CurrencyMaster(strConString);

        if (objSysParam.GetSysParameterByParamName("Base Currency").Rows[0]["Param_Value"].ToString() == strFromCurrency)
        {
            strExchangeRate = ObjCurrency.GetCurrencyMasterById(strToCurrency).Rows[0]["Currency_Value"].ToString();
        }
        else
        {
            strExchangeRate = ((1 / float.Parse(ObjCurrency.GetCurrencyMasterById(strFromCurrency).Rows[0]["Currency_Value"].ToString())) * float.Parse(ObjCurrency.GetCurrencyMasterById(strToCurrency).Rows[0]["Currency_Value"].ToString())).ToString();

        }

        return strExchangeRate;
    }
    public  string GetExchageRate2(string strFromCurrency, string strToCurrency, ref SqlTransaction trns)
    {
        string strExchangeRate = string.Empty;

        SystemParameter objSysParam = new SystemParameter(trns.Connection.ConnectionString);
        CurrencyMaster ObjCurrency = new CurrencyMaster(trns.Connection.ConnectionString);
        if (objSysParam.GetSysParameterByParamName("Base Currency", ref trns).Rows[0]["Param_Value"].ToString() == strFromCurrency)
        {
            strExchangeRate = ObjCurrency.GetCurrencyMasterById(strToCurrency, ref trns).Rows[0]["Currency_Value"].ToString();
            //strExchangeRate = ObjCurrency.GetCurrencyMasterById(strToCurrency, trns.Connection.ConnectionString).Rows[0]["Currency_Value"].ToString();
        }
        else
        {
            strExchangeRate = ((1 / float.Parse(ObjCurrency.GetCurrencyMasterById(strFromCurrency, ref trns).Rows[0]["Currency_Value"].ToString())) * float.Parse(ObjCurrency.GetCurrencyMasterById(strToCurrency, ref trns).Rows[0]["Currency_Value"].ToString())).ToString();

        }
        return strExchangeRate;
    }
    public static string GetExchageRate(string strFromCurrency, string strToCurrency, ref SqlTransaction trns)
    {
        string strExchangeRate = string.Empty;
       
        SystemParameter objSysParam = new SystemParameter(trns.Connection.ConnectionString);
        CurrencyMaster ObjCurrency = new CurrencyMaster(trns.Connection.ConnectionString);
        if (objSysParam.GetSysParameterByParamName("Base Currency", ref trns).Rows[0]["Param_Value"].ToString() == strFromCurrency)
        {
            strExchangeRate = ObjCurrency.GetCurrencyMasterById(strToCurrency, ref trns).Rows[0]["Currency_Value"].ToString();
            //strExchangeRate = ObjCurrency.GetCurrencyMasterById(strToCurrency, trns.Connection.ConnectionString).Rows[0]["Currency_Value"].ToString();
        }
        else
        {
            strExchangeRate = ((1 / float.Parse(ObjCurrency.GetCurrencyMasterById(strFromCurrency, ref trns).Rows[0]["Currency_Value"].ToString())) * float.Parse(ObjCurrency.GetCurrencyMasterById(strToCurrency, ref trns).Rows[0]["Currency_Value"].ToString())).ToString();

        }
        return strExchangeRate;
    }
    public string GetExchageRate1(string strFromCurrency, string strToCurrency, ref SqlTransaction trns)
    {
        string strExchangeRate = string.Empty;

        SystemParameter objSysParam = new SystemParameter(trns.Connection.ConnectionString);
        CurrencyMaster ObjCurrency = new CurrencyMaster(trns.Connection.ConnectionString);
        
        if (objSysParam.GetSysParameterByParamName("Base Currency", ref trns).Rows[0]["Param_Value"].ToString() == strFromCurrency)
        {
            strExchangeRate = ObjCurrency.GetCurrencyMasterById(strToCurrency, ref trns).Rows[0]["Currency_Value"].ToString();
            //strExchangeRate = ObjCurrency.GetCurrencyMasterById(strToCurrency, trns.Connection.ConnectionString).Rows[0]["Currency_Value"].ToString();
        }
        else
        {
            strExchangeRate = ((1 / float.Parse(ObjCurrency.GetCurrencyMasterById(strFromCurrency, ref trns).Rows[0]["Currency_Value"].ToString())) * float.Parse(ObjCurrency.GetCurrencyMasterById(strToCurrency, ref trns).Rows[0]["Currency_Value"].ToString())).ToString();

        }
        return strExchangeRate;
    }

    public  string GetCurrency1(string strToCurrency, string strLocalAmount, ref SqlTransaction trns, string strCompId, string strLocId,double dExchangeRate)
    {
        SystemParameter objsys = new SystemParameter(trns.Connection.ConnectionString);
        LocationMaster objLocation = new LocationMaster(trns.Connection.ConnectionString);
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        // string strCurrency = objLocation.GetLocationMasterById(strCompId, strLocId).Rows[0]["Field1"].ToString();

        //strExchangeRate = objsys.GetExchageRate2(strCurrency, strToCurrency, ref trns);
        // strExchangeRate = SystemParameter.GetExchageRate(strCurrency, strToCurrency, ref trns);
        strExchangeRate = dExchangeRate.ToString();
        try
        {
            strForienAmount = objsys.GetCurencyConversionForInv(strToCurrency, (float.Parse(strExchangeRate) * float.Parse(strLocalAmount)).ToString(), ref trns);
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
    }
    public  string GetCurrency1(string strToCurrency, string strLocalAmount, string strConString, string strCompId, string strLocId)
    {
        SystemParameter objsys = new SystemParameter(strConString);
        LocationMaster objLocation = new LocationMaster(strConString);
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = objLocation.GetLocationMasterById(strCompId, strLocId).Rows[0]["Field1"].ToString();

        strExchangeRate = SystemParameter.GetExchageRate(strCurrency, strToCurrency, strConString);
        try
        {
            strForienAmount = objsys.GetCurencyConversionForInv(strToCurrency, (float.Parse(strExchangeRate) * float.Parse(strLocalAmount)).ToString());
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
    }


    public static string GetCurrency(string strToCurrency, string strLocalAmount,string strConString,string strCompId, string strLocId)
    {
        SystemParameter objsys = new SystemParameter(strConString);
        LocationMaster objLocation = new LocationMaster(strConString);
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = objLocation.GetLocationMasterById(strCompId, strLocId).Rows[0]["Field1"].ToString();

        strExchangeRate = SystemParameter.GetExchageRate(strCurrency, strToCurrency, strConString);
        try
        {
            strForienAmount = objsys.GetCurencyConversionForInv(strToCurrency, (float.Parse(strExchangeRate) * float.Parse(strLocalAmount)).ToString());
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
    }

    public static string GetCurrency(string strToCurrency, string strLocalAmount, ref SqlTransaction trns,string strCompId, string strLocId)
    {
        SystemParameter objsys = new SystemParameter(trns.Connection.ConnectionString);
        LocationMaster objLocation = new LocationMaster(trns.Connection.ConnectionString);
        string strExchangeRate = string.Empty;
        string strForienAmount = string.Empty;
        string strCurrency = objLocation.GetLocationMasterById(strCompId, strLocId, ref trns).Rows[0]["Field1"].ToString();

        strExchangeRate = SystemParameter.GetExchageRate(strCurrency, strToCurrency, ref trns);
        try
        {
            strForienAmount = objsys.GetCurencyConversionForInv(strToCurrency, (float.Parse(strExchangeRate) * float.Parse(strLocalAmount)).ToString(), ref trns);
            strForienAmount = strForienAmount + "/" + strExchangeRate;
        }
        catch
        {
            strForienAmount = "0";
        }
        return strForienAmount;
    }

    public static string GetLocationCurrencyId(string strConString,string strCompId, string strLocId)
    {
        LocationMaster ObjLocation = new LocationMaster(strConString);

        return ObjLocation.GetLocationMasterById(strCompId, strLocId).Rows[0]["Field1"].ToString();
    }

    public static string GetLocationCurrencyId(ref SqlTransaction trns,string strCompId, string strLocId)
    {
        LocationMaster ObjLocation = new LocationMaster(trns.Connection.ConnectionString);

        return ObjLocation.GetLocationMasterById(strCompId,strLocId, ref trns).Rows[0]["Field1"].ToString();
    }
    public  string GetLocationCurrencyId1(ref SqlTransaction trns, string strCompId, string strLocId)
    {
        LocationMaster ObjLocation = new LocationMaster(trns.Connection.ConnectionString);

        return ObjLocation.GetLocationMasterById(strCompId, strLocId, ref trns).Rows[0]["Field1"].ToString();
    }

    public static string SetDecimal(string amount,string strConString,string strCompId, string strLocId)
    {
        SystemParameter objsys = new SystemParameter(strConString);
        return objsys.GetCurencyConversionForInv(GetLocationCurrencyId(strConString,strCompId,strLocId), amount);
    }

    public static string GetAmountWithDecimal(string Amount, string strDecimalCount)
    {
        double Amt = 0;
        int decimalCount = 0;
        double convertedAmt = 0;
        string DotValue = "";
        double.TryParse(Amount, out Amt);
        int.TryParse(strDecimalCount, out decimalCount);
        convertedAmt = Math.Round(Amt, decimalCount);
        for (int i = 0; i < decimalCount; i++)
        {
            DotValue = DotValue + "0";
        }
        if (DotValue == string.Empty)
        {
            DotValue = "0";
        }
        return convertedAmt.ToString("0." + DotValue);
    }

    public static string Get_Tax_Parameter(string strConString)
    {
        SystemParameter ObjSysParam = new SystemParameter(strConString);
        string strApprovalLevel = string.Empty;
        DataTable Dt_Parameter = ObjSysParam.GetSysParameterByParamName("Tax System");
        if (Dt_Parameter != null && Dt_Parameter.Rows.Count > 0)
        {
            if (Dt_Parameter.Rows[0]["Param_Value"].ToString() == "Company")
            {
                strApprovalLevel = "Company";
            }
            else if (Dt_Parameter.Rows[0]["Param_Value"].ToString() == "Location")
            {
                strApprovalLevel = "Location";
            }
            else if (Dt_Parameter.Rows[0]["Param_Value"].ToString() == "System")
            {
                strApprovalLevel = "System";
            }
            else
            {
                strApprovalLevel = "Select";
            }
        }
        return strApprovalLevel;
    }
}
