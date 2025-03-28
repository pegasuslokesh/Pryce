using System;
using System.Data;
using System.Web;
using PegasusDataAccess;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Reflection;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;


/// <summary>
/// Summary description for Common
/// </summary>
public class Common
{
    DataAccessClass daClass = null;
    CompanyMaster objComp = null;
    BrandMaster objbrand = null;
    LocationMaster objLocation = null;
    Set_AddressChild Objaddress = null;
    Set_Location_Department objLocDept = null;
    RoleDataPermission objRoleData = null;
    UserPermission objUserPermission = null;
    DataAccessClass objDa = null;
    private string _strConString = string.Empty;


    public Common(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        daClass = new DataAccessClass(strConString);
        objDa = new DataAccessClass(strConString);
        objComp = new CompanyMaster(strConString);
        objbrand = new BrandMaster(strConString);
        objLocation = new LocationMaster(strConString);
        Objaddress = new Set_AddressChild(strConString);
        objLocDept = new Set_Location_Department(strConString);
        objRoleData = new RoleDataPermission(strConString);
        objUserPermission = new UserPermission(strConString);
        _strConString = strConString;
    }



    public static bool LeaveApprovalFunctionality(string strLeaveTypeId, string strConString)
    {
        DataAccessClass objDa = new DataAccessClass(strConString);
        bool Flag = false;

        try
        {
            Flag = Convert.ToBoolean(objDa.return_DataTable("select Field5 from Att_LeaveMaster where Leave_Id=" + strLeaveTypeId + "").Rows[0][0].ToString());
        }
        catch
        {
        }

        return Flag;
    }


    public static bool LeaveNegativeBalance(string strLeaveTypeId, string strConString)
    {
        DataAccessClass objDa = new DataAccessClass(strConString);
        bool Flag = false;

        try
        {
            Flag = Convert.ToBoolean(objDa.return_DataTable("select is_partial from Att_LeaveMaster where Leave_Id=" + strLeaveTypeId + "").Rows[0][0].ToString());
        }
        catch
        {
        }

        return Flag;
    }


    public static bool LeaveApprovalFunctionality(string strLeaveTypeId, ref SqlTransaction trns)
    {
        DataAccessClass objDa = new DataAccessClass(trns.Connection.ConnectionString);
        bool Flag = false;

        try
        {
            Flag = Convert.ToBoolean(objDa.return_DataTable("select Field5 from Att_LeaveMaster where Leave_Id=" + strLeaveTypeId + "", ref trns).Rows[0][0].ToString());
        }
        catch
        {
        }

        return Flag;
    }


    public static bool LeaveTransactionFunctionality(string strLeaveTypeId, string strConString)
    {
        DataAccessClass objDa = new DataAccessClass(strConString);
        bool Flag = false;

        try
        {
            Flag = Convert.ToBoolean(objDa.return_DataTable("select Field6 from Att_LeaveMaster where Leave_Id=" + strLeaveTypeId + "").Rows[0][0].ToString());
        }
        catch
        {
        }

        return Flag;
    }


    public enum TransactionType
    {
        Other = 0,
        IntraState = 1,
        InterState = 2,
        InterUnionTerritory = 3,
        International = 4
    }

    public TransactionType TransactionTypeProperty
    {
        get; set;
    }

    public static DataTable DtPhysical
    {
        get;
        set;
    }
    public static string GetMonthName(int MonthNumber)
    {
        System.Globalization.DateTimeFormatInfo mfi = new
    System.Globalization.DateTimeFormatInfo();
        string strMonthName = mfi.GetMonthName(MonthNumber).ToString();
        return strMonthName;
    }

    public static string GetObjectIdbyPageURL(string strPageURL, string strConString)
    {
        DataAccessClass Objda = new DataAccessClass(strConString);
        string strVal = "0";

        DataTable dt = Objda.return_DataTable("select OBJECT_ID from it_objectentry where page_url = '" + strPageURL + "' and isactive='True'");
        if (dt.Rows.Count > 0)
        {
            strVal = dt.Rows[0]["OBJECT_ID"].ToString();
        }

        return strVal;
    }

    //public static string ChangeTDForDefaultLeft()
    //{
    //    string retval = string.Empty;
    //    try
    //    {
    //        string lang = HttpContext.Current.Session["lang"].ToString();
    //    }
    //    catch (Exception)
    //    {
    //        return "left";
    //    }
    //    if (HttpContext.Current.Session["lang"] != null && HttpContext.Current.Session["lang"].ToString() == "2")
    //    {
    //        retval = "right";
    //    }
    //    else
    //    {
    //        retval = "left";
    //    }
    //    return retval;
    //}
    //public static string ChangeTDForDefaultRight()
    //{
    //    string retval = string.Empty;
    //    if (HttpContext.Current.Session["lang"] != null && HttpContext.Current.Session["lang"] == "2")
    //    {
    //        retval = "left";
    //    }
    //    else
    //    {
    //        retval = "right";
    //    }
    //    return retval;
    //}
    public DataTable GetModuleName()
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[1];
        paramList[0] = new PassDataToSql("@optype", "2", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Application_Menu", paramList);
        return dtInfo;
    }
    public DataTable GetObjectName()
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[1];
        paramList[0] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("sp_Application_Menu", paramList);

        return dtInfo;
    }

    public DataTable GetAccodion(string CompanyId, string UserId, string Application_Id)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@UserId", UserId.ToString(), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Application_Id", Application_Id.ToString(), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Companyid", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("Set_Accodion", paramList);

        return dtInfo;
    }
    public DataTable GetAccodion(string CompanyId, string UserId, string Application_Id, string isFromRole)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@UserId", UserId.ToString(), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Application_Id", Application_Id.ToString(), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Companyid", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@isFromRole", isFromRole, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = daClass.Reuturn_Datatable_Search("Set_Accodion", paramList);
        return dtInfo;
    }

    public DataTable GetAllPagePermission(string UserId, string ModuleId, string ObjectId, string strCompId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@UserId", UserId.ToString(), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ModuleId", ModuleId.ToString(), PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ObjectId", ObjectId.ToString(), PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Company_Id", strCompId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("Set_AllPageCode", paramList);

        return dtInfo;
    }

    public DataTable Get_UserPermission(string UserId, string ModuleId, string ObjectId, string strCompId)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[4];
        paramList[0] = new PassDataToSql("@UserId", UserId.ToString(), PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@ModuleId", ModuleId.ToString(), PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ObjectId", ObjectId.ToString(), PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Company_Id", strCompId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("Get_User_permission", paramList);

        return dtInfo;
    }

    public static DataTable GetActiveEmployeeWithLocation(string prefixText, string comp, string strBrandId, string strLocId, string strConString)
    {
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        con.ConnectionString = strConString;
        con.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "Set_EmployeeMaster_SelectEmployeeName";
        cmd.Parameters.AddWithValue("@CompanyId", comp);
        cmd.Parameters.AddWithValue("@EmpName", prefixText);
        cmd.CommandType = CommandType.StoredProcedure;
        da.SelectCommand = cmd;
        da.Fill(dt);

        if (strLocId.Length > 0)
        {
            dt = new DataView(dt, "Brand_Id='" + strBrandId + "' and Location_Id='" + strLocId + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        return dt;
    }

    public static DataTable GetActiveEmployeeWithoutLocation(string prefixText, string comp, string strBrandId, string strLocId, string strConString)
    {
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        con.ConnectionString = strConString;
        con.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "Set_EmployeeMaster_SelectEmployeeName";
        cmd.Parameters.AddWithValue("@CompanyId", comp);
        cmd.Parameters.AddWithValue("@EmpName", prefixText);
        cmd.CommandType = CommandType.StoredProcedure;
        da.SelectCommand = cmd;
        da.Fill(dt);

        if (strLocId.Length > 0)
        {
            dt = new DataView(dt, "Brand_Id='" + strBrandId + "' and   Field2 ='False' and Year(Termination_date) =1900 and IsActive ='1'", "", DataViewRowState.CurrentRows).ToTable();
        }

        return dt;
    }

    public static DataTable GetEmployee(string prefixText, string comp, string strBrandId, string strLocId, string strConString)
    {
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        con.ConnectionString = strConString;
        con.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "Set_EmployeeMaster_SelectEmployeeName";
        cmd.Parameters.AddWithValue("@CompanyId", comp);
        cmd.Parameters.AddWithValue("@EmpName", prefixText);
        cmd.CommandType = CommandType.StoredProcedure;
        da.SelectCommand = cmd;
        da.Fill(dt);

        if (strLocId.Length > 0)
        {
            dt = new DataView(dt, "Brand_Id='" + strBrandId + "' and Location_Id='" + strLocId + "'", "", DataViewRowState.CurrentRows).ToTable();
        }

        return dt;
    }

    // Modified By Nitin jain on 13/11/2014....
    public static DataTable GetShift(string prefixText, string comp, string strBrandId, string strConString)
    {
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        con.ConnectionString = strConString;
        con.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "sp_Att_ShiftManagement_SelectRow";
        cmd.Parameters.AddWithValue("@Company_Id", comp);
        cmd.Parameters.AddWithValue("@Shift_Id", "0");
        cmd.Parameters.AddWithValue("@Shift_Name", prefixText);
        cmd.Parameters.AddWithValue("@Optype", "6");
        cmd.Parameters.AddWithValue("@location_id", "0");
        cmd.CommandType = CommandType.StoredProcedure;
        da.SelectCommand = cmd;
        da.Fill(dt);
        dt = new DataView(dt, "Brand_Id='" + strBrandId + "'", "", DataViewRowState.CurrentRows).ToTable();


        return dt;
    }
    public static int DisableEmployee(string Companyid, string EmpId, string strConString)
    {
        DataAccessClass da = new DataAccessClass(strConString);
        PassDataToSql[] paramList = new PassDataToSql[3];
        paramList[0] = new PassDataToSql("@CompanyId", Companyid, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@EmpId", EmpId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@ReferenceID", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Output);
        da.execute_Sp("sp_Set_Employee_Deactive", paramList);
        return Convert.ToInt32(paramList[2].ParaValue);

    }

    public static DataTable GetTimeTable(string prefixText, string comp, string strBrandId, string strConString)
    {
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection();
        SqlDataAdapter da = new SqlDataAdapter();
        con.ConnectionString = strConString;
        con.Open();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "sp_Att_TimeTable_SelectRow";
        cmd.Parameters.AddWithValue("@Company_Id", comp);
        cmd.Parameters.AddWithValue("@TimeTable_Id", "0");
        cmd.Parameters.AddWithValue("@TimeTable_Name", prefixText);
        cmd.Parameters.AddWithValue("@location_id", "");
        cmd.Parameters.AddWithValue("@Optype", "6");
        cmd.CommandType = CommandType.StoredProcedure;
        da.SelectCommand = cmd;
        da.Fill(dt);
        dt = new DataView(dt, "Brand_Id='" + strBrandId + "'", "", DataViewRowState.CurrentRows).ToTable();


        return dt;
    }
    //.........................................
    public string GetEmpName(string Emp_Id, string strCompId)
    {
        string retval = "";
        DataAccessClass daClass = new DataAccessClass(_strConString);
        PassDataToSql[] ps = new PassDataToSql[2];
        ps[0] = new PassDataToSql("@CompanyId", strCompId, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        ps[1] = new PassDataToSql("@Emp_Id", Emp_Id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        DataTable dt = daClass.Reuturn_Datatable_Search("Set_EmployeeMaster_SelectEmployeeNameByEmpCode", ps);
        if (dt != null && dt.Rows.Count > 0)
            retval = "" + dt.Rows[0]["Emp_Name"].ToString() + "/(" + dt.Rows[0]["Designation"].ToString() + ")/" + dt.Rows[0][0].ToString() + "";
        return retval;
    }


    public static string GetEmployeeImage(string EmployeeId, string strConString, string strCompId)
    {
        EmployeeMaster objEmp = new EmployeeMaster(strConString);
        string strEmpImage = string.Empty;
        DataTable Dt = objEmp.GetEmployeeMasterById(strCompId, EmployeeId);
        if (Dt.Rows.Count > 0)
        {

            strEmpImage = "../CompanyResource/2/" + Dt.Rows[0]["Emp_Image"].ToString();
        }
        else
        {
            strEmpImage = "";
        }
        Dt.Dispose();
        return strEmpImage;
    }


    public static string GetEmployeeImage(string EmployeeId, ref SqlTransaction trns, string strCompId)
    {
        EmployeeMaster objEmp = new EmployeeMaster(trns.Connection.ConnectionString);
        string strEmpImage = string.Empty;
        DataTable Dt = objEmp.GetEmployeeMasterById(strCompId, EmployeeId, ref trns);
        if (Dt.Rows.Count > 0)
        {
            strEmpImage = "../CompanyResource/2/" + Dt.Rows[0]["Emp_Image"].ToString();
        }
        else
        {
            strEmpImage = "";
        }
        Dt.Dispose();
        return strEmpImage;
    }

    public static string GetEmployeeImage(string strCompanyId, string EmployeeId, ref SqlTransaction trns)
    {
        EmployeeMaster objEmp = new EmployeeMaster(trns.Connection.ConnectionString);
        string strEmpImage = string.Empty;
        DataTable Dt = objEmp.GetEmployeeMasterById(strCompanyId, EmployeeId, ref trns);
        if (Dt.Rows.Count > 0)
        {

            strEmpImage = "../CompanyResource/2/" + Dt.Rows[0]["Emp_Image"].ToString();
        }
        else
        {
            strEmpImage = "";
        }
        Dt.Dispose();
        return strEmpImage;
    }

    public DataTable GetCheckEsistenceId(string id, string Optype)
    {
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@optype", Optype, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@id", id, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("Sp_CheckEsistenceId", paramList);

        return dtInfo;
    }
    public void UpdateCodeForDocumentNo(string TableName, string FieldName, string TransField, string Trans, string Code)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];

        paramList[0] = new PassDataToSql("@TableName", TableName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@FieldName", FieldName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@FieldTrans_Id", TransField, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans", Trans, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Code", Code, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Inv_Common_UpdateCode", paramList);
    }
    //created for rollback Trsnaction
    //15-02-2016
    //by jitendra upadhyay

    public void UpdateCodeForDocumentNo(string TableName, string FieldName, string TransField, string Trans, string Code, ref SqlTransaction trns)
    {
        PassDataToSql[] paramList = new PassDataToSql[5];

        paramList[0] = new PassDataToSql("@TableName", TableName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@FieldName", FieldName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@FieldTrans_Id", TransField, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Trans", Trans, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@Code", Code, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_Inv_Common_UpdateCode", paramList, ref trns);
    }



    public DataTable handledBy_Code(string empid, string strCompId)
    {

        UserMaster objUserMaster = new UserMaster(_strConString);
        DataTable dt1 = new DataTable();

        dt1 = objUserMaster.GetUserHandledBy(empid, strCompId);
        string ids = empid;
        DataTable ds = new DataTable();
        ds = objUserMaster.GetEmpDtlFromEmpID(empid, strCompId);
        for (int i = 0; i < dt1.Rows.Count; i++)
        {

            if (dt1.Rows.Count != 0)
            {
                ds.Merge(handledBy_Code(dt1.Rows[i]["Employee_Id"].ToString(), strCompId));
            }
            else
            {

                //ids = empid;
            }
        }

        return ds;
    }

    public static string[] TaxDiscountCaluculation(string Price, string Qty, string Taxper, string taxVal, string Disper, string DisVal, bool IsUnitLevel, string CurrencyId, bool IsTaxOnly, string strConString)
    {
        SystemParameter obj = new SystemParameter(strConString);


        string[] StrReturn = new string[7];
        if (IsUnitLevel)
        {
            StrReturn[0] = obj.GetCurencyConversionForInv(CurrencyId, (Convert.ToDouble(Price) * Convert.ToDouble(Qty)).ToString());
        }
        else
        {
            StrReturn[0] = obj.GetCurencyConversionForInv(CurrencyId, Price.ToString());

        }
        if (!IsTaxOnly)
        {
            if (Disper != "")
            {
                DisVal = (Convert.ToDouble(Convert.ToDouble(Price) * Convert.ToDouble(Disper)) / 100).ToString();
                if (DisVal.Contains("NaN"))
                {
                    DisVal = "0";

                }
            }
            else
            {
                if (DisVal != "")
                {
                    Disper = ((Convert.ToDouble(DisVal) * 100) / Convert.ToDouble(Price)).ToString();
                    if (Disper.Contains("NaN"))
                    {
                        Disper = "0";

                    }
                }

            }
        }
        if (Disper == "")
        {
            Disper = "0";
        }
        if (DisVal == "")
        {
            DisVal = "0";
        }
        StrReturn[1] = obj.GetCurencyConversionForInv(CurrencyId, Disper.ToString());
        StrReturn[2] = obj.GetCurencyConversionForInv(CurrencyId, DisVal.ToString());

        if (Taxper != "")
        {
            taxVal = (((Convert.ToDouble(Price) - Convert.ToDouble(DisVal)) * Convert.ToDouble(Taxper)) / 100).ToString();
            if (taxVal.Contains("NaN"))
            {
                taxVal = "0";

            }
        }
        else
        {
            if (taxVal != "")
            {
                Taxper = ((Convert.ToDouble(taxVal) * 100) / (Convert.ToDouble(Price) - Convert.ToDouble(DisVal))).ToString();
                if (Taxper.Contains("NaN"))
                {
                    Taxper = "0";

                }
            }
            else
            {
                taxVal = "0";
            }

        }
        StrReturn[3] = obj.GetCurencyConversionForInv(CurrencyId, Taxper.ToString());
        //StrReturn[4] = obj.GetCurencyConversionForInv(CurrencyId, taxVal.ToString());
        StrReturn[4] = taxVal.ToString();
        if (IsUnitLevel)
        {
            StrReturn[5] = obj.GetCurencyConversionForInv(CurrencyId, (((Convert.ToDouble(Price) - Convert.ToDouble(DisVal)) + Convert.ToDouble(taxVal)) * Convert.ToDouble(Qty)).ToString());
        }
        else
        {
            StrReturn[5] = obj.GetCurencyConversionForInv(CurrencyId, (((Convert.ToDouble(Price) - Convert.ToDouble(DisVal)) + Convert.ToDouble(taxVal))).ToString());

        }

        StrReturn[6] = Disper.ToString();

        return StrReturn;


    }


    //public void DisableControls(Control parent, bool State)
    //{
    //    foreach (Control c in parent.Controls)
    //    {
    //        if (c is DropDownList)
    //        {
    //            ((DropDownList)(c)).Enabled = State;
    //        }

    //        if (c is TextBox)
    //            ((TextBox)c).Enabled = State;
    //        if (c is Button)
    //            ((Button)c).Enabled = State;
    //        else if (c is DropDownList)
    //            ((DropDownList)c).Enabled = State;
    //        else if (c is CheckBox)
    //            ((CheckBox)c).Enabled = State;
    //        else if (c is RadioButton)
    //            ((RadioButton)c).Enabled = State;
    //        else if (c is ImageButton)
    //            ((ImageButton)c).Enabled = State;
    //        else if (c is GridView)
    //            ((GridView)c).Enabled = State;

    //        DisableControls(c, State);
    //    }
    //}


    public static DataTable GetReportHeader(string RefId, string Optype, string strConString)
    {
        DataAccessClass daClass = new DataAccessClass(strConString);
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[2];
        paramList[0] = new PassDataToSql("@RefId", RefId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@OpType", Optype, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        dtInfo = daClass.Reuturn_Datatable_Search("sp_Inv_ReportHeader", paramList);

        return dtInfo;
    }

    public static string[] ReportHeaderSetup(string ParameterType, string PrameterValue, string strConString)
    {
        string Address = "";
        string telno = string.Empty;
        string Faxno = string.Empty;
        string Website = string.Empty;
        string ImageUrl = string.Empty;
        string GSTIN = string.Empty;
        string[] str = new string[8];
        DataTable dt = new DataTable();

        if (ParameterType == "Company")
        {

            dt = GetReportHeader(PrameterValue, "0", strConString);
        }

        if (ParameterType == "Brand")
        {

            dt = GetReportHeader(PrameterValue, "1", strConString);
        }

        if (ParameterType == "Location")
        {

            dt = GetReportHeader(PrameterValue, "2", strConString);
        }

        if (dt.Rows.Count > 0)
        {
            str[0] = dt.Rows[0]["HeaderName"].ToString();
            str[1] = dt.Rows[0]["HeaderName_L"].ToString();

            Address = dt.Rows[0]["Address"].ToString();
            if (dt.Rows[0]["Address"].ToString() != "")
            {
                Address = dt.Rows[0]["Address"].ToString();
            }
            if (dt.Rows[0]["Street"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["Street"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["Street"].ToString();
                }
            }
            if (dt.Rows[0]["Block"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["Block"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["Block"].ToString();
                }

            }
            if (dt.Rows[0]["Avenue"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["Avenue"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["Avenue"].ToString();
                }
            }

            if (dt.Rows[0]["CityId"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["CityId"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["CityId"].ToString();
                }

            }
            if (dt.Rows[0]["StateId"].ToString() != "")
            {


                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["StateId"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["StateId"].ToString();
                }

            }
            if (dt.Rows[0]["CountryId"].ToString() != "")
            {
                CountryMaster objCountry = new CountryMaster(strConString);


                if (Address != "")
                {
                    Address = Address + "," + objCountry.GetCountryMasterById(dt.Rows[0]["CountryId"].ToString()).Rows[0]["Country_Name"].ToString();
                }
                else
                {
                    Address = objCountry.GetCountryMasterById(dt.Rows[0]["CountryId"].ToString()).Rows[0]["Country_Name"].ToString();
                }

            }
            if (dt.Rows[0]["PinCode"].ToString() != "")
            {
                if (Address != "")
                {
                    Address = Address + "," + dt.Rows[0]["PinCode"].ToString();
                }
                else
                {
                    Address = dt.Rows[0]["PinCode"].ToString();
                }

            }
            telno = dt.Rows[0]["PhoneNo1"].ToString();

            if (dt.Rows[0]["PhoneNo2"].ToString() != "")
            {

                if (telno != "")
                {
                    telno = telno + "," + dt.Rows[0]["PhoneNo2"].ToString();
                }
                else
                {
                    telno = dt.Rows[0]["PhoneNo2"].ToString();
                }
            }
            if (dt.Rows[0]["MobileNo1"].ToString() != "")
            {
                if (telno != "")
                {
                    telno = telno + "," + dt.Rows[0]["MobileNo1"].ToString();
                }
                else
                {
                    telno = dt.Rows[0]["MobileNo1"].ToString();
                }
            }
            if (dt.Rows[0]["MobileNo2"].ToString() != "")
            {
                if (telno != "")
                {
                    telno = telno + "," + dt.Rows[0]["MobileNo2"].ToString();
                }
                else
                {
                    telno = dt.Rows[0]["MobileNo2"].ToString();
                }
            }
            if (dt.Rows[0]["FaxNo"].ToString() != "")
            {
                Faxno = dt.Rows[0]["FaxNo"].ToString();
            }
            if (dt.Rows[0]["WebSite"].ToString() != "")
            {
                Website = dt.Rows[0]["WebSite"].ToString();
            }
            //if (dt.Rows[0]["GSTIN"].ToString() != "")
            //{
            //    GSTIN = dt.Rows[0]["GSTIN"].ToString();
            //}
            ImageUrl = dt.Rows[0]["Imageurl"].ToString();
            str[2] = Address;
            str[3] = telno;
            str[4] = Faxno;
            str[5] = Website;
            str[6] = ImageUrl;
            str[7] = GSTIN;
        }



        return str;


    }




    public static bool RollbackTransaction(string TableName, string FieldName, string FieldValue, string strConString)
    {
        bool Result = false;
        DataAccessClass daClass = new DataAccessClass(strConString);

        PassDataToSql[] paramList = new PassDataToSql[5];

        paramList[0] = new PassDataToSql("@TableName", TableName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@FieldName", FieldName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@FieldValue", FieldValue, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@TransType", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@TransTypeValue", "0", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_RollbackTransaction", paramList);

        return Result;

    }
    public static int GetLineNumber(Exception ex)
    {
        var lineNumber = 0;
        const string lineSearch = ":line ";
        var index = ex.StackTrace.LastIndexOf(lineSearch);
        if (index != -1)
        {
            var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
            if (int.TryParse(lineNumberText, out lineNumber))
            {
            }
        }
        return lineNumber;
    }

    public static string ConvertErrorMessage(string ErrorMsg, Exception ex)
    {
        string Message = ErrorMsg.ToString().Replace("'", " ");

        Message = Message.ToString().Replace("\r", "");
        Message = Message.ToString().Replace("\n", "");

        Message = Message + " Line Number  " + Common.GetLineNumber(ex);


        return Message;
    }

    public static bool RollbackTransaction(string TableName, string FieldName, string FieldValue, string TransType, string TransTypeValue, string strConString)
    {
        bool Result = false;
        DataAccessClass daClass = new DataAccessClass(strConString);

        PassDataToSql[] paramList = new PassDataToSql[5];

        paramList[0] = new PassDataToSql("@TableName", TableName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@FieldName", FieldName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@FieldValue", FieldValue, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@TransType", TransType, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[4] = new PassDataToSql("@TransTypeValue", TransTypeValue, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);

        daClass.execute_Sp("sp_RollbackTransaction", paramList);


        return Result;


    }

    public void databasebackup(string Path, string strConString, string strTimeZoneId)
    {
        string strconn = strConString;
        SqlConnection conn = new SqlConnection(strconn);
        string Databasename = conn.Database;

        string sql = "Backup Database " + Databasename + " to Disk='" + Path + "\\" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).ToString("ddMMyyyy_HHmmss") + ".Bak'";
        try
        {
            daClass.execute_Command(sql);
        }
        catch
        {
        }
    }

    public string GetRoleDataPermission(string RoleId, string RecordType, string strUserId, string strCompId, string strEmpId)
    {
        RoleId = string.Empty;

        UserMaster ObjUser = new UserMaster(_strConString);


        DataTable dtUser = ObjUser.GetUserMasterByUserId(strUserId, strCompId);


        if (dtUser.Rows.Count > 0)
        {
            RoleId = dtUser.Rows[0]["DistinctRoleId"].ToString();


        }

        string IDs = string.Empty;

        if (strEmpId != "0")
        {

            string strSql = " Select *       From Set_UserDataPermission   where    Set_UserDataPermission.Company_Id=" + strCompId + " and  Set_UserDataPermission.User_Id ='" + strUserId + "' and Set_UserDataPermission.IsActive='True'  ";
            DataTable dtRoleData = objDa.return_DataTable(strSql);
            //DataTable dtRoleData = objRoleData.GetRoleDataPermissionById(RoleId);
            if (dtRoleData.Rows.Count > 0)
            {
                dtRoleData = new DataView(dtRoleData, "Record_Type='" + RecordType + "'", "", DataViewRowState.CurrentRows).ToTable();

                if (dtRoleData.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRoleData.Rows.Count; i++)
                    {
                        if (RecordType == "D")
                        {
                            //if (GetDeptIdbyTransId(dtRoleData.Rows[i]["Record_Id"].ToString()) != "")
                            //{
                            IDs += dtRoleData.Rows[i]["Record_Id"].ToString() + ",";
                            //}
                        }
                        else
                        {
                            IDs += dtRoleData.Rows[i]["Record_Id"].ToString() + ",";
                        }
                    }
                }
            }
        }
        return IDs;

    }

    public string GetRoleDataPermission(string RoleId, string RecordType, string strLocationId, string strUserId, string strCompId, string strEmpId)
    {
        DataTable dtDepartment = new DataTable();
        RoleId = string.Empty;

        int intLocationId = 0;
        int.TryParse(strLocationId, out intLocationId);

        if (intLocationId == 0)
        {
            strLocationId = strLocationId.Substring(0, strLocationId.Length - 1);
        }
        else
        {

        }

        UserMaster ObjUser = new UserMaster(_strConString);


        DataTable dtUser = ObjUser.GetUserMasterByUserId(strUserId, strCompId);


        if (dtUser.Rows.Count > 0)
        {
            RoleId = dtUser.Rows[0]["DistinctRoleId"].ToString();


        }



        string IDs = string.Empty;

        if (strEmpId != "0")
        {

            string strSql = "Select *       From Set_UserDataPermission   where    Set_UserDataPermission.Company_Id=" + strCompId + " and  Set_UserDataPermission.User_Id ='" + strUserId + "' and Set_UserDataPermission.IsActive='True' ";
            DataTable dtRoleData = objDa.return_DataTable(strSql);
            //DataTable dtRoleData = objRoleData.GetRoleDataPermissionById(RoleId);
            if (dtRoleData.Rows.Count > 0)
            {
                dtRoleData = new DataView(dtRoleData, "Record_Type='" + RecordType + "'", "", DataViewRowState.CurrentRows).ToTable();
                if (RecordType == "D")
                {
                    dtRoleData = new DataView(dtRoleData, "Field1 in (" + strLocationId + ")", "", DataViewRowState.CurrentRows).ToTable();
                }
                if (dtRoleData.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRoleData.Rows.Count; i++)
                    {
                        if (RecordType == "D")
                        {
                            //if (GetDeptIdbyTransId(dtRoleData.Rows[i]["Record_Id"].ToString()) != "")
                            //{
                            IDs += dtRoleData.Rows[i]["Record_Id"].ToString() + ",";
                            //}
                        }
                        else
                        {
                            IDs += dtRoleData.Rows[i]["Record_Id"].ToString() + ",";
                        }
                    }
                }
            }
        }
        else
        {
            if (RecordType == "D")
            {
                dtDepartment = objDa.return_DataTable("select distinct Dep_id from set_location_department  where location_id  in(" + strLocationId + ")");
                for (int i = 0; i < dtDepartment.Rows.Count; i++)
                {
                    IDs += dtDepartment.Rows[i]["Dep_id"].ToString() + ",";
                }
            }
            if (RecordType == "L")
            {
                dtDepartment = objDa.return_DataTable("select location_id from set_locationMaster  where isActive='true'");
                for (int i = 0; i < dtDepartment.Rows.Count; i++)
                {
                    IDs += dtDepartment.Rows[i]["location_id"].ToString() + ",";
                }
            }
        }
        return IDs;

    }
    public string GetDeptIdbyTransId(string TransId)
    {

        string DepId = string.Empty;

        DataTable dt = objLocDept.GetDepartmentLocation();
        dt = new DataView(dt, "Trans_Id='" + TransId + "'", "", DataViewRowState.CurrentRows).ToTable();

        if (dt.Rows.Count > 0)
        {
            DepId = dt.Rows[0]["Dep_Id"].ToString();
        }
        return DepId;
    }

    public static bool GetStatus(string strEmpId)
    {
        bool status = false;

        if (strEmpId == "0")
        {
            status = true;
        }
        return status;
    }
    public static bool IsFinancialyearAllow(DateTime dtVoucherDate, string strModuleType, string strConString, string strFinanceToDate, string strCompId, string strFinanceYearId, string strLocId)
    {

        SystemParameter objsys = new SystemParameter(strConString);
        Ac_Finance_Year_Info objFianceYear = new Ac_Finance_Year_Info(strConString);
        Ac_FinancialYear_Detail objFinanceDetail = new Ac_FinancialYear_Detail(strConString);
        string strStatus = string.Empty;
        string strFinanceFromDate = string.Empty;
        bool Result = false;

        DateTime FinanceTodate = new DateTime(Convert.ToDateTime(strFinanceToDate).Year, Convert.ToDateTime(strFinanceToDate).Month, Convert.ToDateTime(strFinanceToDate).Day, 23, 59, 1);
        DateTime VoucherDate = new DateTime(dtVoucherDate.Year, dtVoucherDate.Month, dtVoucherDate.Day);

        try
        {
            using (DataTable _dt = objFianceYear.GetInfoByTransId(strCompId, strFinanceYearId))
            {
                if (_dt.Rows.Count > 0)
                {
                    strStatus = _dt.Rows[0]["Status"].ToString();
                    strFinanceFromDate = _dt.Rows[0]["From_Date"].ToString();
                }
            }
            //strStatus = objFianceYear.GetInfoByTransId(strCompId, strFinanceYearId).Rows[0]["Status"].ToString();
        }
        catch
        {

        }

        if (strStatus.Trim().ToUpper() == "OPEN" || strStatus.Trim().ToUpper() == "REOPEN")
        {
            if (Convert.ToDateTime(strFinanceFromDate) <= dtVoucherDate && FinanceTodate >= VoucherDate)
            {
                DataTable dtFinanceDetail = objFinanceDetail.GetAllDataByHeader_Id(strFinanceYearId);
                if (dtFinanceDetail.Rows.Count > 0)
                {
                    if (strModuleType == "I")
                    {
                        dtFinanceDetail = new DataView(dtFinanceDetail, "Location_Id='" + strLocId + "' and (Inv_Status='Open' or Inv_Status='ReOpen')", "", System.Data.DataViewRowState.CurrentRows).ToTable();
                    }
                    else if (strModuleType == "F")
                    {
                        dtFinanceDetail = new DataView(dtFinanceDetail, "Location_Id='" + strLocId + "' and (Status='Open' or Status='ReOpen')", "", System.Data.DataViewRowState.CurrentRows).ToTable();
                    }
                }

                if (dtFinanceDetail.Rows.Count > 0)
                {
                    Result = true;
                }
            }
        }
        return Result;
    }


    public static bool IsFinancialyearAllow(DateTime dtVoucherDate, string strModuleType, ref SqlTransaction trns, string strFinanceToDate, string strCompId, string strFinanceYearId, string strLocId)
    {

        SystemParameter objsys = new SystemParameter(trns.Connection.ConnectionString);
        Ac_Finance_Year_Info objFianceYear = new Ac_Finance_Year_Info(trns.Connection.ConnectionString);
        Ac_FinancialYear_Detail objFinanceDetail = new Ac_FinancialYear_Detail(trns.Connection.ConnectionString);
        string strStatus = string.Empty;
        string strFinanceFromDate = string.Empty;
        bool Result = false;

        DateTime FinanceTodate = new DateTime(Convert.ToDateTime(strFinanceToDate).Year, Convert.ToDateTime(strFinanceToDate).Month, Convert.ToDateTime(strFinanceToDate).Day, 23, 59, 1);
        DateTime VoucherDate = new DateTime(dtVoucherDate.Year, dtVoucherDate.Month, dtVoucherDate.Day);

        try
        {
            //strStatus = objFianceYear.GetInfoByTransId(strCompId, strFinanceYearId, ref trns).Rows[0]["Status"].ToString();
            using (DataTable _dt = objFianceYear.GetInfoByTransId(strCompId, strFinanceYearId))
            {
                if (_dt.Rows.Count > 0)
                {
                    strStatus = _dt.Rows[0]["Status"].ToString();
                    strFinanceFromDate = _dt.Rows[0]["From_Date"].ToString();
                }
            }
        }
        catch (Exception ex)
        {

        }

        if (strStatus.Trim().ToUpper() == "OPEN" || strStatus.Trim().ToUpper() == "REOPEN")
        {
            if (Convert.ToDateTime(strFinanceFromDate) <= dtVoucherDate && FinanceTodate >= VoucherDate)
            {
                DataTable dtFinanceDetail = objFinanceDetail.GetAllDataByHeader_Id(strFinanceYearId, ref trns);
                if (dtFinanceDetail.Rows.Count > 0)
                {
                    if (strModuleType == "I")
                    {
                        dtFinanceDetail = new DataView(dtFinanceDetail, "Location_Id='" + strLocId + "' and (Inv_Status='Open' or Inv_Status='ReOpen')", "", System.Data.DataViewRowState.CurrentRows).ToTable();
                    }
                    else if (strModuleType == "F")
                    {
                        dtFinanceDetail = new DataView(dtFinanceDetail, "Location_Id='" + strLocId + "' and (Status='Open' or Status='ReOpen')", "", System.Data.DataViewRowState.CurrentRows).ToTable();
                    }
                }

                if (dtFinanceDetail.Rows.Count > 0)
                {
                    Result = true;
                }
            }
        }
        return Result;
    }

    public static bool IsFinancialyearDateCheckOnly(DateTime dtVoucherDate, string strFinanceToDate, string strFinanceFromDate)
    {
        bool Result = false;

        DateTime FinanceTodate = new DateTime(Convert.ToDateTime(strFinanceToDate).Year, Convert.ToDateTime(strFinanceToDate).Month, Convert.ToDateTime(strFinanceToDate).Day, 23, 59, 1);
        DateTime VoucherDate = new DateTime(dtVoucherDate.Year, dtVoucherDate.Month, dtVoucherDate.Day);

        if (Convert.ToDateTime(strFinanceFromDate) <= dtVoucherDate && FinanceTodate >= VoucherDate)
        {
            Result = true;
        }
        return Result;
    }
    public static int getFinancialYearId(DateTime trans_date, string strConString, string strCompId)
    {
        DataAccessClass objDa = new DataAccessClass(strConString);
        SystemParameter Objsys = new SystemParameter(strConString);
        int financialYearId = 0;
        try
        {
            financialYearId = int.Parse(objDa.get_SingleValue("select isnull(Trans_Id,0) from dbo.Ac_Finance_Year_Info where ('" + trans_date + "' between From_Date and To_Date) and Company_id='" + strCompId + "' and isActive='true'"));
        }
        catch
        {

        }
        return financialYearId;
    }



    public static string Encrypt(string clearText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }

    public static string Decrypt(string cipherText)
    {
        byte[] cipherBytes;
        string EncryptionKey = "MAKV2SPBNI99212";
        cipherText = cipherText.Replace(" ", "+");
        //try
        //{
        cipherBytes = Convert.FromBase64String(cipherText);
        //}
        //catch
        //{
        //    cipherText = Encrypt(cipherText);
        //    cipherText = cipherText.Replace(" ", "+");
        //    cipherBytes = Convert.FromBase64String(cipherText);
        //}



        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }

    public static string GetMasterEmailId(string strConString, string strCompId, string strBrandId, string strLocId)
    {
        string strAppMailId = string.Empty;
        Set_ApplicationParameter objAppParam = new Set_ApplicationParameter(strConString);

        DataTable dtFrom = objAppParam.GetApplicationParameterByParamName("Master_Email", strCompId, strBrandId, strLocId);
        if (dtFrom.Rows.Count > 0)
        {
            strAppMailId = dtFrom.Rows[0]["Param_Value"].ToString();
        }
        return strAppMailId;

    }

    public static string GetMasterEmailId(ref SqlTransaction trns, string strCompId, string strBrandId, string strLocId)
    {
        string strAppMailId = string.Empty;
        Set_ApplicationParameter objAppParam = new Set_ApplicationParameter(trns.Connection.ConnectionString);

        DataTable dtFrom = objAppParam.GetApplicationParameterByParamName("Master_Email", strCompId, ref trns, strBrandId, strLocId);
        if (dtFrom.Rows.Count > 0)
        {
            strAppMailId = dtFrom.Rows[0]["Param_Value"].ToString();
        }
        return strAppMailId;

    }


    public static string GetMasterEmailPassword(string strConString, string strCompId, string strBrandId, string strLocId)
    {
        string strAppPassword = string.Empty;
        Set_ApplicationParameter objAppParam = new Set_ApplicationParameter(strConString);


        DataTable dtPass = objAppParam.GetApplicationParameterByParamName("Master_Email_Password", strCompId, strBrandId, strLocId);
        if (dtPass.Rows.Count > 0)
        {
            strAppPassword = Common.Decrypt(dtPass.Rows[0]["Param_Value"].ToString());
        }

        return strAppPassword;
    }

    public static string GetMasterEmailPassword(ref SqlTransaction trns, string strCompId, string strBrandId, string strLocId)
    {
        string strAppPassword = string.Empty;
        Set_ApplicationParameter objAppParam = new Set_ApplicationParameter(trns.Connection.ConnectionString);


        DataTable dtPass = objAppParam.GetApplicationParameterByParamName("Master_Email_Password", strCompId, ref trns, strBrandId, strLocId);
        if (dtPass.Rows.Count > 0)
        {
            strAppPassword = Common.Decrypt(dtPass.Rows[0]["Param_Value"].ToString());
        }

        return strAppPassword;
    }

    public static string GetEmployeeName(string EmployeeId, string strConString, string strCompId)
    {
        string EmployeeName = string.Empty;
        EmployeeMaster objEmp = new EmployeeMaster(strConString);

        DataTable Dt = objEmp.GetEmployeeMasterById(strCompId, EmployeeId);
        if (Dt.Rows.Count > 0)
        {
            EmployeeName = Dt.Rows[0]["Emp_Name"].ToString();
        }

        return EmployeeName;
    }

    public static string GetEmployeeName(string EmployeeId, ref SqlTransaction trns, string strCompId)
    {
        string EmployeeName = string.Empty;
        EmployeeMaster objEmp = new EmployeeMaster(trns.Connection.ConnectionString);

        DataTable Dt = objEmp.GetEmployeeMasterById(strCompId, EmployeeId, ref trns);
        if (Dt.Rows.Count > 0)
        {
            EmployeeName = Dt.Rows[0]["Emp_Name"].ToString();
        }

        return EmployeeName;
    }

    public static string GetEmployeeName(string strCompanyId, string EmployeeId, ref SqlTransaction trns)
    {
        string EmployeeName = string.Empty;
        EmployeeMaster objEmp = new EmployeeMaster(trns.Connection.ConnectionString);

        DataTable Dt = objEmp.GetEmployeeMasterById(strCompanyId, EmployeeId, ref trns);
        if (Dt.Rows.Count > 0)
        {
            EmployeeName = Dt.Rows[0]["Emp_Name"].ToString();
        }

        return EmployeeName;
    }

    public static string GetEmployeeCode(string EmployeeId, string strConString, string strCompId)
    {
        string EmployeeCode = string.Empty;
        EmployeeMaster objEmp = new EmployeeMaster(strConString);

        DataTable Dt = objEmp.GetEmployeeMasterById(strCompId, EmployeeId);
        if (Dt.Rows.Count > 0)
        {
            EmployeeCode = Dt.Rows[0]["Emp_Code"].ToString();
        }

        return EmployeeCode;
    }

    public static string GetEmployeeCode(string EmployeeId, ref SqlTransaction trns, string strCompId)
    {
        string EmployeeCode = string.Empty;
        EmployeeMaster objEmp = new EmployeeMaster(trns.Connection.ConnectionString);

        DataTable Dt = objEmp.GetEmployeeMasterById(strCompId, EmployeeId, ref trns);
        if (Dt.Rows.Count > 0)
        {
            EmployeeCode = Dt.Rows[0]["Emp_Code"].ToString();
        }

        return EmployeeCode;
    }


    public static string GetEmployeeCode(string strCompanyId, string EmployeeId, ref SqlTransaction trns)
    {
        string EmployeeCode = string.Empty;
        EmployeeMaster objEmp = new EmployeeMaster(trns.Connection.ConnectionString);

        DataTable Dt = objEmp.GetEmployeeMasterById(strCompanyId, EmployeeId, ref trns);
        if (Dt.Rows.Count > 0)
        {
            EmployeeCode = Dt.Rows[0]["Emp_Code"].ToString();
        }

        return EmployeeCode;
    }

    public static string GetDepartmentName(string EmployeeId, string strConString, string strCompId)
    {
        string strDepartmentName = string.Empty;
        EmployeeMaster objEmp = new EmployeeMaster(strConString);

        DataTable Dt = objEmp.GetEmployeeMasterById(strCompId, EmployeeId);
        if (Dt.Rows.Count > 0)
        {
            strDepartmentName = Dt.Rows[0]["Department_Name"].ToString();
        }

        return strDepartmentName;
    }

    public static string GetDepartmentName(string EmployeeId, ref SqlTransaction trns, string strCompId)
    {

        string strDepartmentName = string.Empty;
        EmployeeMaster objEmp = new EmployeeMaster(trns.Connection.ConnectionString);

        DataTable Dt = objEmp.GetEmployeeMasterById(strCompId, EmployeeId, ref trns);
        if (Dt.Rows.Count > 0)
        {
            strDepartmentName = Dt.Rows[0]["Department_Name"].ToString();
        }

        return strDepartmentName;
    }

    public static string GetDepartmentName(string strCompanyId, string EmployeeId, ref SqlTransaction trns)
    {

        string strDepartmentName = string.Empty;
        EmployeeMaster objEmp = new EmployeeMaster(trns.Connection.ConnectionString);

        DataTable Dt = objEmp.GetEmployeeMasterById(strCompanyId, EmployeeId, ref trns);
        if (Dt.Rows.Count > 0)
        {
            strDepartmentName = Dt.Rows[0]["Department_Name"].ToString();
        }

        return strDepartmentName;
    }

    public static string GetEmployeeDateOfJoining(string EmployeeId, string strConString, string strCompId)
    {
        string strDoj = string.Empty;
        EmployeeMaster objEmp = new EmployeeMaster(strConString);
        SystemParameter objsys = new SystemParameter(strConString);

        DataTable Dt = objEmp.GetEmployeeMasterById(strCompId, EmployeeId);
        if (Dt.Rows.Count > 0)
        {
            strDoj = Convert.ToDateTime(Dt.Rows[0]["DOJ"].ToString()).ToString(objsys.SetDateFormat());
        }

        return strDoj;
    }


    public static string GetEmployeeDateOfJoining(string EmployeeId, ref SqlTransaction trns, string strCompId)
    {

        string strDoj = string.Empty;
        EmployeeMaster objEmp = new EmployeeMaster(trns.Connection.ConnectionString);
        SystemParameter objsys = new SystemParameter(trns.Connection.ConnectionString);

        DataTable Dt = objEmp.GetEmployeeMasterById(strCompId, EmployeeId, ref trns);
        if (Dt.Rows.Count > 0)
        {
            strDoj = Convert.ToDateTime(Dt.Rows[0]["DOJ"].ToString()).ToString(objsys.SetDateFormat());
        }

        return strDoj;
    }


    public static string GetEmployeeDateOfJoining(string strCompanyId, string EmployeeId, ref SqlTransaction trns)
    {

        string strDoj = string.Empty;
        EmployeeMaster objEmp = new EmployeeMaster(trns.Connection.ConnectionString);
        SystemParameter objsys = new SystemParameter(trns.Connection.ConnectionString);

        DataTable Dt = objEmp.GetEmployeeMasterById(strCompanyId, EmployeeId, ref trns);
        if (Dt.Rows.Count > 0)
        {
            strDoj = Convert.ToDateTime(Dt.Rows[0]["DOJ"].ToString()).ToString(objsys.SetDateFormat());
        }

        return strDoj;
    }
    public static string GetEmployeeLeaveRemainingBalance_By_LeaveTypeId(string strEmployeeid, string strLeaveTypeId, string strConString, string strTimeZoneId)
    {

        string strRemainingLeave = "0";
        DataAccessClass objda = new DataAccessClass(strConString);

        DataTable Dt = objda.return_DataTable("select Remaining_Days from Att_Employee_Leave_Trans where Emp_Id=" + strEmployeeid + " and year='" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Year + "' and Leave_Type_Id='" + strLeaveTypeId + "'");
        if (Dt.Rows.Count > 0)
        {
            strRemainingLeave = Dt.Rows[0]["Remaining_Days"].ToString();
        }

        return strRemainingLeave;
    }

    public static string GetEmployeeLeaveRemainingBalance_By_LeaveTypeId(string strEmployeeid, string strLeaveTypeId, ref SqlTransaction trns, string strTimeZoneId)
    {

        string strRemainingLeave = "0";
        DataAccessClass objda = new DataAccessClass(trns.Connection.ConnectionString);

        DataTable Dt = objda.return_DataTable("select Remaining_Days from Att_Employee_Leave_Trans where Emp_Id=" + strEmployeeid + " and year='" + Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), strTimeZoneId).Year + "' and Leave_Type_Id='" + strLeaveTypeId + "'", ref trns);
        if (Dt.Rows.Count > 0)
        {
            strRemainingLeave = Dt.Rows[0]["Remaining_Days"].ToString();
        }

        return strRemainingLeave;
    }

    public static string GetleaveNameById(string strLeaveTypeId, string strConString, string strCompId)
    {
        LeaveMaster objLeave = new LeaveMaster(strConString);

        string strleaveName = string.Empty;

        DataTable dt = objLeave.GetLeaveMasterById(strCompId, strLeaveTypeId);
        if (dt.Rows.Count > 0)
        {
            strleaveName = dt.Rows[0]["Leave_Name"].ToString();

        }

        return strleaveName;


    }


    public static string GetleaveNameById(string strLeaveTypeId, ref SqlTransaction trns, string strCompId)
    {
        LeaveMaster objLeave = new LeaveMaster(trns.Connection.ConnectionString);

        string strleaveName = string.Empty;

        DataTable dt = objLeave.GetLeaveMasterById(strCompId, strLeaveTypeId, ref trns);
        if (dt.Rows.Count > 0)
        {
            strleaveName = dt.Rows[0]["Leave_Name"].ToString();

        }

        return strleaveName;


    }



    public static string GetleaveNameById(string strCompanyId, string strLeaveTypeId, ref SqlTransaction trns)
    {
        LeaveMaster objLeave = new LeaveMaster(trns.Connection.ConnectionString);

        string strleaveName = string.Empty;

        DataTable dt = objLeave.GetLeaveMasterById(strCompanyId, strLeaveTypeId, ref trns);
        if (dt.Rows.Count > 0)
        {
            strleaveName = dt.Rows[0]["Leave_Name"].ToString();

        }

        return strleaveName;


    }


    public static string GetmailContentByLeaveTypeId(string strLeaveHeaderId, string strEmpId, string strConString, string strCompId)
    {
        SystemParameter ObjSysParam = new SystemParameter(strConString);
        DataAccessClass ObjDa = new PegasusDataAccess.DataAccessClass(strConString);

        string strMailContent = string.Empty;


        strMailContent = "</br></br>  <table  border='1px' cellpadding='5' cellspacing='0' style='border: solid 1px Silver; font-size: x-small; width: 100%;'> <tr> <td align='Center'  bgcolor='#F79646'   style='font-weight:bold;color:White; font-size: 14px;' > Leave Type</td> <td align='Center'  bgcolor='#F79646'   style='font-weight:bold;color:White; font-size: 14px;' > From Date</td>  <td align='Center'  bgcolor='#F79646'   style='font-weight:bold; color:White; font-size: 14px;' > To Date </td>  <td align='Center'  bgcolor='#F79646'   style='font-weight:bold; color:White; font-size: 14px;' > Applied Leave day </td><td align='Center'  bgcolor='#F79646'   style='font-weight:bold; color:White; font-size: 14px;' > Description</td></tr>";


        DataTable dtleaveRquestDetail = ObjDa.return_DataTable("select Trans_id,Leave_Type_Id,Emp_Description,From_date,To_Date,(select  COUNT(*) from Att_Leave_Request_Child where Ref_Id = Att_Leave_Request.Trans_Id) as LeaveCount from Att_Leave_Request where Field2='" + strLeaveHeaderId + "'");


        foreach (DataRow drdetail in dtleaveRquestDetail.Rows)
        {

            strMailContent += "<tr><td align='Center'   style='font-weight:bold; font-size: 13px;'>" + Common.GetleaveNameById(drdetail["Leave_Type_Id"].ToString(), strConString, strCompId) + "</td><td align='Center'  style='font-weight:bold; font-size: 13px;'>" + Convert.ToDateTime(drdetail["From_Date"].ToString()).ToString("dd-MMM-yyyy") + "</td><td align='Center'  style='font-weight:bold; font-size: 13px;'>" + Convert.ToDateTime(drdetail["To_Date"].ToString()).ToString("dd-MMM-yyyy") + "</td><td align='Center'  style='font-weight:bold; font-size: 13px;'>" + drdetail["LeaveCount"].ToString() + "</td><td align='Center'  style='font-weight:bold; font-size: 13px;'>" + drdetail["Emp_Description"].ToString() + "</td></tr>";



        }


        strMailContent += "</table>";
        return strMailContent;
    }

    public static string GetmailContentByLeaveTypeId(string strLeaveHeaderId, string strEmpId, ref SqlTransaction trns, string strCompId)
    {
        SystemParameter ObjSysParam = new SystemParameter(trns.Connection.ConnectionString);
        DataAccessClass ObjDa = new PegasusDataAccess.DataAccessClass(trns.Connection.ConnectionString);

        string strMailContent = string.Empty;

        strMailContent = "</br></br>  <table  border='1px' cellpadding='5' cellspacing='0' style='border: solid 1px Silver; font-size: x-small; width: 100%;'> <tr> <td align='left'  bgcolor='#F79646'   style='font-weight:bold;color:White; font-size: 14px;' > Leave Type</td> <td align='left'  bgcolor='#F79646'   style='font-weight:bold;color:White; font-size: 14px;' > From Date</td>  <td align='left'  bgcolor='#F79646'   style='font-weight:bold; color:White; font-size: 14px;' > To Date </td>  <td align='left'  bgcolor='#F79646'   style='font-weight:bold; color:White; font-size: 14px;' > Applied Leave day </td><td align='left'  bgcolor='#F79646'   style='font-weight:bold; color:White; font-size: 14px;' > Description</td></tr>";


        DataTable dtleaveRquestDetail = ObjDa.return_DataTable("select Trans_id,Leave_Type_Id,Emp_Description,From_date,To_Date,(select  COUNT(*) from Att_Leave_Request_Child where Ref_Id = Att_Leave_Request.Trans_Id) as LeaveCount from Att_Leave_Request where Field2='" + strLeaveHeaderId + "'", ref trns);


        foreach (DataRow drdetail in dtleaveRquestDetail.Rows)
        {

            strMailContent += "<tr><td align='left'   style='font-weight:bold; font-size: 13px;'>" + Common.GetleaveNameById(drdetail["Leave_Type_Id"].ToString(), ref trns, strCompId) + "</td><td align='left'  style='font-weight:bold; font-size: 13px;'>" + Convert.ToDateTime(drdetail["From_Date"].ToString()).ToString("dd-MMM-yyyy") + "</td><td align='left'  style='font-weight:bold; font-size: 13px;'>" + Convert.ToDateTime(drdetail["To_Date"].ToString()).ToString("dd-MMM-yyyy") + "</td><td align='left'  style='font-weight:bold; font-size: 13px;'>" + drdetail["LeaveCount"].ToString() + "</td><td align='left'  style='font-weight:bold; font-size: 13px;'>" + drdetail["Emp_Description"].ToString() + "</td></tr>";



        }


        strMailContent += "</table>";



        return strMailContent;

    }



    public static string GetAmountDecimal(string strAmount, string strConString, string strCurrnecyId)
    {

        SystemParameter Objsys = new SystemParameter(strConString);

        if (strAmount == "")
        {

            strAmount = "0";
        }

        return Objsys.GetCurencyConversionForInv(strCurrnecyId, strAmount);


    }

    public static string GetAmountDecimal(string strAmount, ref SqlTransaction trns, string strCurrencyId)
    {

        SystemParameter Objsys = new SystemParameter(trns.Connection.ConnectionString);

        if (strAmount == "")
        {

            strAmount = "0";
        }

        return Objsys.GetCurencyConversionForInv(strCurrencyId, strAmount, ref trns);


    }


    public static string GetEmployeeCurreny(string strEmpId, string strConString, string CurrencyId, string strCompId, string strLocId)
    {
        LocationMaster ObjLocation = new LocationMaster(strConString);

        if (strEmpId == "")
        {
            strEmpId = "0";
        }
        DataAccessClass Objda = new DataAccessClass(strConString);

        string strCurrencyId = string.Empty;
        DataTable dt = Objda.return_DataTable("select isnull(set_locationmaster.Field1,0) from set_employeemaster inner join set_locationmaster on set_employeemaster.Location_Id = set_locationmaster.Location_id where set_employeemaster.Emp_Id=" + strEmpId + "");

        if (dt.Rows.Count > 0)
        {

            strCurrencyId = dt.Rows[0][0].ToString();

            if (strCurrencyId == "" || strCurrencyId == "0")
            {
                strCurrencyId = CurrencyId;
            }
        }
        else
        {
            strCurrencyId = ObjLocation.GetLocationMasterById(strCompId, strLocId).Rows[0]["Field1"].ToString();

        }

        return strCurrencyId;

    }






    public static bool IsAlive(string aIP)
    {
        bool result = false;
        Ping pingSender = new Ping();
        PingOptions options = new PingOptions();
        options.DontFragment = true;
        string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        byte[] buffer = Encoding.ASCII.GetBytes(data);
        int timeout = 120;
        PingReply reply = pingSender.Send(aIP);
        if (reply.Status == IPStatus.Success)
        {
            result = true;
        }
        else
        {

        }
        return result;
    }
    public static string Get_Location_Currency_ID(string Company_Id, string strConString, string strLocId)
    {
        string Currency_ID = string.Empty;
        try
        {
            LocationMaster objLocation = new LocationMaster(strConString);
            DataTable Dt_Currency = objLocation.Get_Currency_By_Location_ID(Company_Id, strLocId);
            if (Dt_Currency != null && Dt_Currency.Rows.Count > 0)
            {
                Currency_ID = Dt_Currency.Rows[0]["Currency_ID"].ToString();
            }
        }
        catch
        {
            Currency_ID = "";
        }
        return Currency_ID;
    }

    public static string Get_Location_Currency_LControl(string Company_Id, string strConString, string strLocId)
    {
        string Currency = string.Empty;
        try
        {
            LocationMaster objLocation = new LocationMaster(strConString);
            DataTable Dt_Currency = objLocation.Get_Currency_By_Location_ID(Company_Id, strLocId);
            if (Dt_Currency != null && Dt_Currency.Rows.Count > 0)
            {
                Currency = Dt_Currency.Rows[0]["Currency_Code"].ToString();
            }
        }
        catch
        {
            Currency = "";
        }
        return Currency;
    }

    public static string Get_Currency_By_Location(string Location_Id, string strConString, string strCompId)
    {
        string Currency = string.Empty;
        try
        {
            LocationMaster objLocation = new LocationMaster(strConString);
            DataTable Dt_Currency = objLocation.Get_Currency_By_Location_ID(strCompId, Location_Id);
            if (Dt_Currency != null && Dt_Currency.Rows.Count > 0)
            {
                Currency = Dt_Currency.Rows[0]["Currency_Code"].ToString();
            }
        }
        catch
        {
            Currency = "";
        }
        return Currency;
    }


    public static string Get_Roundoff_Amount_By_Location(string Amount, string strConString, string strCompId, string strLocId)
    {

        decimal decValue = 0;
        try
        {
            decValue = Convert.ToDecimal(Amount.ToString().Split('.')[1]);
        }
        catch
        {

        }
        if (decValue <= 0)
        {
            return Amount;
        }

        string Currency_Id = string.Empty;
        string Round_Of_Value = "";
        double Net_Value = 0;
        string Denomination = string.Empty;
        try
        {
            LocationMaster objLocation = new LocationMaster(strConString);
            DataTable Dt_Currency = objLocation.Get_Currency_By_Location_ID(strCompId, strLocId);
            if (Dt_Currency != null && Dt_Currency.Rows.Count > 0)
            {
                if (Dt_Currency.Rows[0]["Smallest_Denomination"].ToString() != "")
                {
                    Denomination = Dt_Currency.Rows[0]["Smallest_Denomination"].ToString();
                    Currency_Id = Dt_Currency.Rows[0]["Currency_ID"].ToString();
                }
                else
                {
                    Denomination = "0";
                    Currency_Id = Dt_Currency.Rows[0]["Currency_ID"].ToString();
                }
            }
            else
            {
                Denomination = "0";
                Currency_Id = "0";
            }

            double Get_Amount = Convert.ToDouble(Amount);
            double Denomination_Value = Convert.ToDouble(Denomination);
            if (Denomination_Value != 0)
            {

                double Moduler_Value = Get_Amount % Denomination_Value;
                double Act_Value = Get_Amount - Moduler_Value;
                double Devide_Value = Denomination_Value / 2;
                if (Moduler_Value >= Devide_Value)
                    Net_Value = Act_Value + Denomination_Value;
                else
                    Net_Value = Act_Value + 0;

                Round_Of_Value = Net_Value.ToString();

            }
            else
            {
                Round_Of_Value = Amount;
            }
            if (Currency_Id != "0")
            {
                SystemParameter Objsys = new SystemParameter(strConString);
                Round_Of_Value = Objsys.GetCurencyConversionForInv(Currency_Id, Round_Of_Value);
            }
        }
        catch
        {
            Round_Of_Value = Amount.ToString();
        }
        return Round_Of_Value;
    }

    public static string GetAmountDecimal_By_Location(string strAmount, string strConString, string strLocCurrencyId)
    {
        SystemParameter Objsys = new SystemParameter(strConString);
        if (strAmount == "")
        {
            strAmount = "0";
        }
        return Objsys.GetCurencyConversionForInv(strLocCurrencyId, strAmount);
    }



    public string Get_Decimal_Count_By_Location(ref SqlTransaction trns, string strCompId, string strBrandId, string strLocId)
    {
        string Decimal_Count = "0";
        try
        {
            string TaxQuery = "Select SCC.Field1 As Decimal_Count From Sys_Country_Currency SCC Inner Join Set_LocationMaster SLM on SCC.Currency_ID=SLM.Field1 Where SLM.Company_Id='" + strCompId + "' and SLM.Brand_Id='" + strBrandId + "' and SLM.Location_Id='" + strLocId + "' and SLM.IsActive='True' and SCC.IsActive='True'";
            Decimal_Count = Convert.ToString(objDa.return_DataTable(TaxQuery, ref trns).Rows[0][0]);
            return Decimal_Count;
        }
        catch
        {
            return Decimal_Count;
        }
    }

    public string Get_Decimal_Count_By_Location(string strCompId, string strBrandId, string strLocId)
    {
        string Decimal_Count = "0";
        try
        {
            string TaxQuery = "Select SCC.Field1 As Decimal_Count From Sys_Country_Currency SCC Inner Join Set_LocationMaster SLM on SCC.Currency_ID=SLM.Field1 Where SLM.Company_Id='" + strCompId + "' and SLM.Brand_Id='" + strBrandId + "' and SLM.Location_Id='" + strLocId + "' and SLM.IsActive='True' and SCC.IsActive='True'";
            Decimal_Count = Convert.ToString(objDa.return_DataTable(TaxQuery).Rows[0][0]);
            return Decimal_Count;
        }
        catch
        {
            return Decimal_Count;
        }
    }


    public static double[] getAllowanceanddeductionCalculation(string strCalculationType, string strCalculationValue, DataTable dtPay, double PerDaySalary, string strEmpId, string strMonth, string strYear, string strConString)
    {
        DataAccessClass Objda = new DataAccessClass(strConString);
        double[] val = new double[2];

        double WorkDays = 0;
        double ActualSalary = 0;
        bool IsLeaveAdded = false;

        if (strCalculationType == "0")
        {
            WorkDays = int.Parse(dtPay.Rows[0]["Worked_Days"].ToString()) + int.Parse(dtPay.Rows[0]["Week_Off_Days"].ToString()) + int.Parse(dtPay.Rows[0]["Holiday_Days"].ToString()) + int.Parse(dtPay.Rows[0]["Leave_Days"].ToString());

            ActualSalary = Convert.ToDouble(dtPay.Rows[0]["Basic_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Normal_OT_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["WeekOff_OT_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Holiday_OT_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Leave_Days_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Week_Off_Days_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Holiday_Days_Salary"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Absent_Day_Penalty"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Late_Min_Penalty"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Early_Min_Penalty"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Parital_Violation_Penalty"].ToString());
        }
        else
        {
            DataTable dtattendancesummary = Objda.return_DataTable("select sum(case when (is_absent=0 and is_holiday =0 and is_leave=0 and Is_Week_Off=0) then 1-cast( Halfday_count as numeric(18,3))/2 else 0 end) as Worked_Days, sum(case when is_absent=1 then 1 else 0 end) as Absent_Days, sum(case when is_week_off=1 then 1 else 0 end) as Week_Off_Days, sum(case when Is_Holiday=1 then 1 else 0 end) as Holiday_Days, sum(case when Is_Leave=1 and Att_Leave_Request_Child.Is_Paid=1 then 1 else 0 end) as PaidLeave_Days, sum(case when Is_Leave=1 and Att_Leave_Request_Child.Is_Paid=0 then 1 else 0 end) as UnPaidLeave_Days, sum(case when Halfday_Count>0 then cast( Halfday_count as numeric(18,3))/2 else 0 end) as HalfDays from att_attendanceregister left join att_leave_request on att_attendanceregister.leave_type_id=att_leave_request.Leave_type_id and att_leave_request.emp_id=att_attendanceregister.Emp_Id and att_attendanceregister.Is_leave='True' and att_leave_request.Is_Approved='True' left join Att_Leave_Request_Child on Att_Leave_Request_Child.Ref_Id = att_leave_request.trans_id and Att_Leave_Request_Child.Leave_date=att_attendanceregister.Att_Date where att_attendanceregister.emp_id=" + strEmpId + " and month(att_attendanceregister.att_date)=" + strMonth + " and year(att_attendanceregister.att_date)=" + strYear + "");

            foreach (string str in strCalculationValue.Split(','))
            {
                if (str.Trim() == "")
                {
                    continue;
                }

                if (str == "0")
                {//present day

                    if (dtattendancesummary.Rows[0][0] != null)
                    {
                        WorkDays = Convert.ToDouble(dtattendancesummary.Rows[0]["Worked_Days"].ToString());
                    }
                    else
                    {
                        WorkDays = Convert.ToDouble(dtPay.Rows[0]["Worked_Days"].ToString());
                    }

                    //ActualSalary = Convert.ToDouble(dtPay.Rows[0]["Basic_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Normal_OT_Work_Salary"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Late_Min_Penalty"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Early_Min_Penalty"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Parital_Violation_Penalty"].ToString());
                }
                if (str == "1")
                {
                    //week off day

                    if (dtattendancesummary.Rows[0][0] != null)
                    {
                        WorkDays += Convert.ToDouble(dtattendancesummary.Rows[0]["Week_Off_Days"].ToString());
                    }
                    else
                    {
                        WorkDays += Convert.ToDouble(dtPay.Rows[0]["Week_Off_Days"].ToString());
                    }
                    //ActualSalary += Convert.ToDouble(dtPay.Rows[0]["WeekOff_OT_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Week_Off_Days_Salary"].ToString());
                }
                if (str == "2")
                {
                    //holiday
                    if (dtattendancesummary.Rows[0][0] != null)
                    {
                        WorkDays += Convert.ToDouble(dtattendancesummary.Rows[0]["Holiday_Days"].ToString());
                    }
                    else
                    {
                        WorkDays += Convert.ToDouble(dtPay.Rows[0]["Holiday_Days"].ToString());
                    }
                    //ActualSalary += Convert.ToDouble(dtPay.Rows[0]["Holiday_OT_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Holiday_Days_Salary"].ToString());
                }
                if (str == "3")
                {
                    //absent day
                    if (dtattendancesummary.Rows[0][0] != null)
                    {
                        WorkDays += Convert.ToDouble(dtattendancesummary.Rows[0]["Absent_Days"].ToString());
                    }
                    else
                    {
                        WorkDays += Convert.ToDouble(dtPay.Rows[0]["Absent_Days"].ToString());
                    }
                    //ActualSalary += -Convert.ToDouble(dtPay.Rows[0]["Absent_Day_Penalty"].ToString());
                }
                if (str == "4")
                {
                    //paid leave
                    if (dtattendancesummary.Rows[0][0] != null)
                    {
                        WorkDays += Convert.ToDouble(dtattendancesummary.Rows[0]["PaidLeave_Days"].ToString());
                    }
                    else
                    {
                        WorkDays += Convert.ToDouble(dtPay.Rows[0]["Leave_Days"].ToString());
                    }

                    IsLeaveAdded = true;


                }
                if (str == "5")
                {
                    //unpaid leave
                    if (dtattendancesummary.Rows[0][0] != null)
                    {
                        WorkDays += Convert.ToDouble(dtattendancesummary.Rows[0]["UnPaidLeave_Days"].ToString());
                    }
                    else if (!IsLeaveAdded)
                    {
                        WorkDays += Convert.ToDouble(dtPay.Rows[0]["Leave_Days"].ToString());
                    }
                    IsLeaveAdded = true;

                }
                if (str == "6")
                {
                    //Half day
                    if (dtattendancesummary.Rows[0][0] != null)
                    {
                        WorkDays += Convert.ToDouble(dtattendancesummary.Rows[0]["HalfDays"].ToString());
                    }
                    else if (!IsLeaveAdded)
                    {
                        WorkDays += Convert.ToDouble(dtPay.Rows[0]["Leave_Days"].ToString());
                    }

                    IsLeaveAdded = true;
                    // ActualSalary += Convert.ToDouble(dtPay.Rows[0]["Leave_Days_Salary"].ToString());
                }
            }

            ActualSalary = WorkDays * PerDaySalary;

        }


        val[0] = WorkDays;
        val[1] = ActualSalary;

        return val;
    }

    public static double[] getAllowanceanddeductionCalculation(string strCalculationType, string strCalculationValue, DataTable dtPay, double PerDaySalary, string strEmpId, string strMonth, string strYear, ref SqlTransaction trns)
    {
        DataAccessClass Objda = new DataAccessClass(trns.Connection.ConnectionString);
        double[] val = new double[2];

        double WorkDays = 0;
        double ActualSalary = 0;
        bool IsLeaveAdded = false;

        if (strCalculationType == "0")
        {
            WorkDays = int.Parse(dtPay.Rows[0]["Worked_Days"].ToString()) + int.Parse(dtPay.Rows[0]["Week_Off_Days"].ToString()) + int.Parse(dtPay.Rows[0]["Holiday_Days"].ToString()) + int.Parse(dtPay.Rows[0]["Leave_Days"].ToString());

            ActualSalary = Convert.ToDouble(dtPay.Rows[0]["Basic_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Normal_OT_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["WeekOff_OT_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Holiday_OT_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Leave_Days_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Week_Off_Days_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Holiday_Days_Salary"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Absent_Day_Penalty"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Late_Min_Penalty"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Early_Min_Penalty"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Parital_Violation_Penalty"].ToString());
        }
        else
        {
            DataTable dtattendancesummary = Objda.return_DataTable("select sum(case when (is_absent=0 and is_holiday =0 and is_leave=0 and Is_Week_Off=0) then 1-cast( Halfday_count as numeric(18,3))/2 else 0 end) as Worked_Days, sum(case when is_absent=1 then 1 else 0 end) as Absent_Days, sum(case when is_week_off=1 then 1 else 0 end) as Week_Off_Days, sum(case when Is_Holiday=1 then 1 else 0 end) as Holiday_Days, sum(case when Is_Leave=1 and Att_Leave_Request_Child.Is_Paid=1 then 1 else 0 end) as PaidLeave_Days, sum(case when Is_Leave=1 and Att_Leave_Request_Child.Is_Paid=0 then 1 else 0 end) as UnPaidLeave_Days, sum(case when Halfday_Count>0 then cast( Halfday_count as numeric(18,3))/2 else 0 end) as HalfDays from att_attendanceregister left join att_leave_request on att_attendanceregister.leave_type_id=att_leave_request.Leave_type_id and att_leave_request.emp_id=att_attendanceregister.Emp_Id and att_attendanceregister.Is_leave='True' and att_leave_request.Is_Approved='True' left join Att_Leave_Request_Child on Att_Leave_Request_Child.Ref_Id = att_leave_request.trans_id and Att_Leave_Request_Child.Leave_date=att_attendanceregister.Att_Date where att_attendanceregister.emp_id=" + strEmpId + " and month(att_attendanceregister.att_date)=" + strMonth + " and year(att_attendanceregister.att_date)=" + strYear + "");
            //DataTable dtattendancesummary = Objda.return_DataTable("select sum(case when (is_absent=0 and is_holiday =0 and is_leave=0 and Is_Week_Off=0) then 1-cast( Halfday_count as numeric(18,3))/2 else 0 end) as Worked_Days, sum(case when is_absent=1 then 1 else 0 end) as Absent_Days, sum(case when is_week_off=1 then 1 else 0 end) as Week_Off_Days, sum(case when Is_Holiday=1 then 1 else 0 end) as Holiday_Days, sum(case when Is_Leave=1 and Att_Leave_Request_Child.Is_Paid=1 then 1 else 0 end) as PaidLeave_Days, sum(case when Is_Leave=1 and Att_Leave_Request_Child.Is_Paid=0 then 1 else 0 end) as UnPaidLeave_Days, sum(case when Halfday_Count>0 then cast( Halfday_count as numeric(18,3))/2 else 0 end) as HalfDays from att_attendanceregister left join att_leave_request on att_attendanceregister.leave_type_id=att_leave_request.Leave_type_id and att_leave_request.emp_id=att_attendanceregister.Emp_Id and att_attendanceregister.Is_leave='True' and att_leave_request.Is_Approved='True' left join Att_Leave_Request_Child on Att_Leave_Request_Child.Ref_Id = att_leave_request.trans_id and Att_Leave_Request_Child.Leave_date=att_attendanceregister.Att_Date where att_attendanceregister.emp_id=" + strEmpId + " and month(att_attendanceregister.att_date)=" + strMonth + " and year(att_attendanceregister.att_date)=" + strYear + "", ref trns);

            foreach (string str in strCalculationValue.Split(','))
            {
                if (str.Trim() == "")
                {
                    continue;
                }

                if (str == "0")
                {//present day

                    if (dtattendancesummary.Rows[0][0] != null)
                    {
                        WorkDays = Convert.ToDouble(dtattendancesummary.Rows[0]["Worked_Days"].ToString());
                    }
                    else
                    {
                        WorkDays = Convert.ToDouble(dtPay.Rows[0]["Worked_Days"].ToString());
                    }

                    //ActualSalary = Convert.ToDouble(dtPay.Rows[0]["Basic_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Normal_OT_Work_Salary"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Late_Min_Penalty"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Early_Min_Penalty"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Parital_Violation_Penalty"].ToString());
                }
                if (str == "1")
                {
                    //week off day

                    if (dtattendancesummary.Rows[0][0] != null)
                    {
                        WorkDays += Convert.ToDouble(dtattendancesummary.Rows[0]["Week_Off_Days"].ToString());
                    }
                    else
                    {
                        WorkDays += Convert.ToDouble(dtPay.Rows[0]["Week_Off_Days"].ToString());
                    }
                    //ActualSalary += Convert.ToDouble(dtPay.Rows[0]["WeekOff_OT_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Week_Off_Days_Salary"].ToString());
                }
                if (str == "2")
                {
                    //holiday
                    if (dtattendancesummary.Rows[0][0] != null)
                    {
                        WorkDays += Convert.ToDouble(dtattendancesummary.Rows[0]["Holiday_Days"].ToString());
                    }
                    else
                    {
                        WorkDays += Convert.ToDouble(dtPay.Rows[0]["Holiday_Days"].ToString());
                    }
                    //ActualSalary += Convert.ToDouble(dtPay.Rows[0]["Holiday_OT_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Holiday_Days_Salary"].ToString());
                }
                if (str == "3")
                {
                    //absent day
                    if (dtattendancesummary.Rows[0][0] != null)
                    {
                        WorkDays += Convert.ToDouble(dtattendancesummary.Rows[0]["Absent_Days"].ToString());
                    }
                    else
                    {
                        WorkDays += Convert.ToDouble(dtPay.Rows[0]["Absent_Days"].ToString());
                    }
                    //ActualSalary += -Convert.ToDouble(dtPay.Rows[0]["Absent_Day_Penalty"].ToString());
                }
                if (str == "4")
                {
                    //paid leave
                    if (dtattendancesummary.Rows[0][0] != null)
                    {
                        WorkDays += Convert.ToDouble(dtattendancesummary.Rows[0]["PaidLeave_Days"].ToString());
                    }
                    else
                    {
                        WorkDays += Convert.ToDouble(dtPay.Rows[0]["Leave_Days"].ToString());
                    }

                    IsLeaveAdded = true;


                }
                if (str == "5")
                {
                    //unpaid leave
                    if (dtattendancesummary.Rows[0][0] != null)
                    {
                        WorkDays += Convert.ToDouble(dtattendancesummary.Rows[0]["UnPaidLeave_Days"].ToString());
                    }
                    else if (!IsLeaveAdded)
                    {
                        WorkDays += Convert.ToDouble(dtPay.Rows[0]["Leave_Days"].ToString());
                    }
                    IsLeaveAdded = true;

                }
                if (str == "6")
                {
                    //Half day
                    if (dtattendancesummary.Rows[0][0] != null)
                    {
                        WorkDays += Convert.ToDouble(dtattendancesummary.Rows[0]["HalfDays"].ToString());
                    }
                    else if (!IsLeaveAdded)
                    {
                        WorkDays += Convert.ToDouble(dtPay.Rows[0]["Leave_Days"].ToString());
                    }

                    IsLeaveAdded = true;
                    // ActualSalary += Convert.ToDouble(dtPay.Rows[0]["Leave_Days_Salary"].ToString());
                }
            }

            ActualSalary = WorkDays * PerDaySalary;

        }


        val[0] = WorkDays;
        val[1] = ActualSalary;

        return val;
    }


    public static double[] getAllowanceanddeductionCalculation1(string strCalculationType, string strCalculationValue, DataTable dtPay, double PerDaySalary, string strEmpId, string strMonth, string strYear, ref SqlTransaction trns, DataTable dtattendancesummary)
    {
        DataAccessClass Objda = new DataAccessClass(trns.Connection.ConnectionString);
        double[] val = new double[2];

        double WorkDays = 0;
        double ActualSalary = 0;
        bool IsLeaveAdded = false;

        if (strCalculationType == "0")
        {
            WorkDays = int.Parse(dtPay.Rows[0]["Worked_Days"].ToString()) + int.Parse(dtPay.Rows[0]["Week_Off_Days"].ToString()) + int.Parse(dtPay.Rows[0]["Holiday_Days"].ToString()) + int.Parse(dtPay.Rows[0]["Leave_Days"].ToString());

            ActualSalary = Convert.ToDouble(dtPay.Rows[0]["Basic_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Normal_OT_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["WeekOff_OT_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Holiday_OT_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Leave_Days_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Week_Off_Days_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Holiday_Days_Salary"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Absent_Day_Penalty"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Late_Min_Penalty"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Early_Min_Penalty"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Parital_Violation_Penalty"].ToString());
        }
        else
        {
            //  DataTable dtattendancesummary = Objda.return_DataTable("select sum(case when (is_absent=0 and is_holiday =0 and is_leave=0 and Is_Week_Off=0) then 1-cast( Halfday_count as numeric(18,3))/2 else 0 end) as Worked_Days, sum(case when is_absent=1 then 1 else 0 end) as Absent_Days, sum(case when is_week_off=1 then 1 else 0 end) as Week_Off_Days, sum(case when Is_Holiday=1 then 1 else 0 end) as Holiday_Days, sum(case when Is_Leave=1 and Att_Leave_Request_Child.Is_Paid=1 then 1 else 0 end) as PaidLeave_Days, sum(case when Is_Leave=1 and Att_Leave_Request_Child.Is_Paid=0 then 1 else 0 end) as UnPaidLeave_Days, sum(case when Halfday_Count>0 then cast( Halfday_count as numeric(18,3))/2 else 0 end) as HalfDays from att_attendanceregister left join att_leave_request on att_attendanceregister.leave_type_id=att_leave_request.Leave_type_id and att_leave_request.emp_id=att_attendanceregister.Emp_Id and att_attendanceregister.Is_leave='True' and att_leave_request.Is_Approved='True' left join Att_Leave_Request_Child on Att_Leave_Request_Child.Ref_Id = att_leave_request.trans_id and Att_Leave_Request_Child.Leave_date=att_attendanceregister.Att_Date where att_attendanceregister.emp_id=" + strEmpId + " and month(att_attendanceregister.att_date)=" + strMonth + " and year(att_attendanceregister.att_date)=" + strYear + "");
            //DataTable dtattendancesummary = Objda.return_DataTable("select sum(case when (is_absent=0 and is_holiday =0 and is_leave=0 and Is_Week_Off=0) then 1-cast( Halfday_count as numeric(18,3))/2 else 0 end) as Worked_Days, sum(case when is_absent=1 then 1 else 0 end) as Absent_Days, sum(case when is_week_off=1 then 1 else 0 end) as Week_Off_Days, sum(case when Is_Holiday=1 then 1 else 0 end) as Holiday_Days, sum(case when Is_Leave=1 and Att_Leave_Request_Child.Is_Paid=1 then 1 else 0 end) as PaidLeave_Days, sum(case when Is_Leave=1 and Att_Leave_Request_Child.Is_Paid=0 then 1 else 0 end) as UnPaidLeave_Days, sum(case when Halfday_Count>0 then cast( Halfday_count as numeric(18,3))/2 else 0 end) as HalfDays from att_attendanceregister left join att_leave_request on att_attendanceregister.leave_type_id=att_leave_request.Leave_type_id and att_leave_request.emp_id=att_attendanceregister.Emp_Id and att_attendanceregister.Is_leave='True' and att_leave_request.Is_Approved='True' left join Att_Leave_Request_Child on Att_Leave_Request_Child.Ref_Id = att_leave_request.trans_id and Att_Leave_Request_Child.Leave_date=att_attendanceregister.Att_Date where att_attendanceregister.emp_id=" + strEmpId + " and month(att_attendanceregister.att_date)=" + strMonth + " and year(att_attendanceregister.att_date)=" + strYear + "", ref trns);

            foreach (string str in strCalculationValue.Split(','))
            {
                if (str.Trim() == "")
                {
                    continue;
                }

                if (str == "0")
                {//present day

                    if (dtattendancesummary.Rows[0][0] != null)
                    {
                        WorkDays = Convert.ToDouble(dtattendancesummary.Rows[0]["Worked_Days"].ToString());
                    }
                    else
                    {
                        WorkDays = Convert.ToDouble(dtPay.Rows[0]["Worked_Days"].ToString());
                    }

                    //ActualSalary = Convert.ToDouble(dtPay.Rows[0]["Basic_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Normal_OT_Work_Salary"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Late_Min_Penalty"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Early_Min_Penalty"].ToString()) - Convert.ToDouble(dtPay.Rows[0]["Parital_Violation_Penalty"].ToString());
                }
                if (str == "1")
                {
                    //week off day

                    if (dtattendancesummary.Rows[0][0] != null)
                    {
                        WorkDays += Convert.ToDouble(dtattendancesummary.Rows[0]["Week_Off_Days"].ToString());
                    }
                    else
                    {
                        WorkDays += Convert.ToDouble(dtPay.Rows[0]["Week_Off_Days"].ToString());
                    }
                    //ActualSalary += Convert.ToDouble(dtPay.Rows[0]["WeekOff_OT_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Week_Off_Days_Salary"].ToString());
                }
                if (str == "2")
                {
                    //holiday
                    if (dtattendancesummary.Rows[0][0] != null)
                    {
                        WorkDays += Convert.ToDouble(dtattendancesummary.Rows[0]["Holiday_Days"].ToString());
                    }
                    else
                    {
                        WorkDays += Convert.ToDouble(dtPay.Rows[0]["Holiday_Days"].ToString());
                    }
                    //ActualSalary += Convert.ToDouble(dtPay.Rows[0]["Holiday_OT_Work_Salary"].ToString()) + Convert.ToDouble(dtPay.Rows[0]["Holiday_Days_Salary"].ToString());
                }
                if (str == "3")
                {
                    //absent day
                    if (dtattendancesummary.Rows[0][0] != null)
                    {
                        WorkDays += Convert.ToDouble(dtattendancesummary.Rows[0]["Absent_Days"].ToString());
                    }
                    else
                    {
                        WorkDays += Convert.ToDouble(dtPay.Rows[0]["Absent_Days"].ToString());
                    }
                    //ActualSalary += -Convert.ToDouble(dtPay.Rows[0]["Absent_Day_Penalty"].ToString());
                }
                if (str == "4")
                {
                    //paid leave
                    if (dtattendancesummary.Rows[0][0] != null)
                    {
                        WorkDays += Convert.ToDouble(dtattendancesummary.Rows[0]["PaidLeave_Days"].ToString());
                    }
                    else
                    {
                        WorkDays += Convert.ToDouble(dtPay.Rows[0]["Leave_Days"].ToString());
                    }

                    IsLeaveAdded = true;


                }
                if (str == "5")
                {
                    //unpaid leave
                    if (dtattendancesummary.Rows[0][0] != null)
                    {
                        WorkDays += Convert.ToDouble(dtattendancesummary.Rows[0]["UnPaidLeave_Days"].ToString());
                    }
                    else if (!IsLeaveAdded)
                    {
                        WorkDays += Convert.ToDouble(dtPay.Rows[0]["Leave_Days"].ToString());
                    }
                    IsLeaveAdded = true;

                }
                if (str == "6")
                {
                    //Half day
                    if (dtattendancesummary.Rows[0][0] != null)
                    {
                        WorkDays += Convert.ToDouble(dtattendancesummary.Rows[0]["HalfDays"].ToString());
                    }
                    else if (!IsLeaveAdded)
                    {
                        WorkDays += Convert.ToDouble(dtPay.Rows[0]["Leave_Days"].ToString());
                    }

                    IsLeaveAdded = true;
                    // ActualSalary += Convert.ToDouble(dtPay.Rows[0]["Leave_Days_Salary"].ToString());
                }
            }

            ActualSalary = WorkDays * PerDaySalary;

        }


        val[0] = WorkDays;
        val[1] = ActualSalary;

        return val;
    }


    public DataTable ListToDataTable<T>(List<T> items)
    {
        DataTable dataTable = new DataTable(typeof(T).Name);
        //Get all the properties by using reflection   
        PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo prop in Props)
        {
            //Setting column names as Property names  
            if (prop.PropertyType.Name == "DateTime")
            {
                dataTable.Columns.Add(prop.Name, typeof(DateTime));
            }
            else
            {
                dataTable.Columns.Add(prop.Name);
            }


        }
        foreach (T item in items)
        {
            var values = new object[Props.Length];
            for (int i = 0; i < Props.Length; i++)
            {

                values[i] = Props[i].GetValue(item, null);
            }
            dataTable.Rows.Add(values);
        }

        return dataTable;
    }
    public byte[] FileToByteArray(string fileName)
    {
        byte[] buff = null;
        FileStream fs = new FileStream(fileName,
                                       FileMode.Open,
                                       FileAccess.Read);
        BinaryReader br = new BinaryReader(fs);
        long numBytes = new FileInfo(fileName).Length;
        buff = br.ReadBytes((int)numBytes);
        return buff;
    }

    public string GetRoundoffAsCurrencyDenomination(string strCurrencyId, string Amount)
    {
        decimal decValue = 0;
        decValue = Convert.ToDecimal(Amount.ToString().Split('.')[1]);
        double Denomination = 0;
        int decCount = 0;

        string sql = "select field1,field2 from sys_currencyMaster where Currency_Id='" + strCurrencyId + "' and isActive='true'";
        DataTable _dtTemp = objDa.return_DataTable(sql);
        if (_dtTemp.Rows.Count > 0)
        {
            int.TryParse(_dtTemp.Rows[0]["field1"].ToString(), out decCount);
            double.TryParse(_dtTemp.Rows[0]["field2"].ToString(), out Denomination);
        }
        _dtTemp.Dispose();


        string Round_Of_Value = "";
        double Net_Value = 0;
        try
        {
            double Get_Amount = Convert.ToDouble(Amount);
            //double Denomination_Value = Convert.ToDouble(Denomination);
            if (Denomination != 0)
            {
                double Moduler_Value = Get_Amount % Denomination;
                double Act_Value = Get_Amount - Moduler_Value;
                double Devide_Value = Denomination / 2;
                if (Moduler_Value >= Devide_Value)
                    Net_Value = Act_Value + Denomination;
                else
                    Net_Value = Act_Value + 0;

                Round_Of_Value = Net_Value.ToString();
            }
            else
            {
                Round_Of_Value = Amount;
            }
            if (strCurrencyId != "0")
            {
                SystemParameter Objsys = new SystemParameter(_strConString);
                Round_Of_Value = Objsys.GetCurencyConversionForInv(strCurrencyId, Round_Of_Value);
            }
        }
        catch
        {
            Round_Of_Value = Amount.ToString();
        }
        return Round_Of_Value;
    }
    [Serializable]
    public class clsPagePermission
    {
        public bool bHavePermission { get; set; }
        public bool bAdd { get; set; }
        public bool bEdit { get; set; }
        public bool bDelete { get; set; }
        public bool bRestore { get; set; }
        public bool bView { get; set; }
        public bool bPrint { get; set; }
        public bool bDownload { get; set; }
        public bool bUpload { get; set; }
        public bool bIncValue { get; set; }
        public bool bSendMail { get; set; }
        public bool bVerify { get; set; }
        public bool bFeedback { get; set; }
        public bool bEditProductId { get; set; }
        public bool bViewAllUserRecord { get; set; }
        public bool bAddManually { get; set; }
        public bool bAddAgentCommission { get; set; }
        public bool bModifyDate { get; set; }
        public bool bViewCustomerStatement { get; set; }
        public bool bShowCostPrice { get; set; }
        public bool bShowSupplier { get; set; }
        public bool bPayCommission { get; set; }
        public bool bReorderAllow { get; set; }
        public bool bCanChangeControsAttribute { get; set; }
    }
    public clsPagePermission getPagePermission(string strPageUrl, string strCompId, string strUserId, string strApplicationId)
    {
        clsPagePermission cls = new clsPagePermission();
        if (string.IsNullOrEmpty(strUserId))
        {
            return cls;
        }
        string userId = strUserId;
        if (userId.ToUpper() == "SUPERADMIN")
        {
            cls.bHavePermission = true;
            cls.bAdd = true;
            cls.bEdit = true;
            cls.bDelete = true;
            cls.bRestore = true;
            cls.bView = true;
            cls.bPrint = true;
            cls.bDownload = true;
            cls.bUpload = true;
            cls.bIncValue = true;
            cls.bSendMail = true;
            cls.bVerify = true;
            cls.bFeedback = true;
            cls.bEditProductId = true;
            cls.bViewAllUserRecord = true;
            cls.bAddManually = true;
            cls.bAddAgentCommission = true;
            cls.bModifyDate = true;
            cls.bViewCustomerStatement = true;
            cls.bShowCostPrice = true;
            cls.bShowSupplier = true;
            cls.bPayCommission = true;
            cls.bReorderAllow = true;
            cls.bCanChangeControsAttribute = true;
        }
        else
        {
            try
            {
                string sql = "select top 1 IT_App_Mod_Object.module_id,IT_App_Mod_Object.object_id from IT_App_Mod_Object inner join IT_ObjectEntry on IT_ObjectEntry.Object_Id=IT_App_Mod_Object.Object_Id where application_id=" + strApplicationId + " and IT_ObjectEntry.Page_Url='" + strPageUrl + "'";
                using (DataTable dtModObject = objDa.return_DataTable(sql))
                {
                    string moduleId = dtModObject.Rows[0]["module_id"].ToString();
                    string objectId = dtModObject.Rows[0]["object_id"].ToString();
                    using (DataTable dtAllPageCode = GetAllPagePermission(userId, moduleId, objectId, strCompId))
                    {
                        cls.bHavePermission = dtAllPageCode.Rows.Count > 0 ? true : false;
                        foreach (DataRow DtRow in dtAllPageCode.Rows)
                        {
                            switch (DtRow["Op_Id"].ToString())
                            {
                                case "1":
                                    cls.bAdd = true;
                                    break;
                                case "2":
                                    cls.bEdit = true;
                                    break;
                                case "3":
                                    cls.bDelete = true;
                                    break;
                                case "4":
                                    cls.bRestore = true;
                                    break;
                                case "5":
                                    cls.bView = true;
                                    break;
                                case "6":
                                    cls.bPrint = true;
                                    break;
                                case "7":
                                    cls.bDownload = true;
                                    break;
                                case "8":
                                    cls.bUpload = true;
                                    break;
                                case "9":
                                    cls.bIncValue = true;
                                    break;
                                case "10":
                                    cls.bSendMail = true;
                                    break;
                                case "11":
                                    cls.bVerify = true;
                                    break;
                                case "12":
                                    cls.bFeedback = true;
                                    break;
                                case "13":
                                    cls.bEditProductId = true;
                                    break;
                                case "14":
                                    cls.bViewAllUserRecord = true;
                                    break;
                                case "15":
                                    cls.bAddManually = true;
                                    break;
                                case "16":
                                    cls.bAddAgentCommission = true;
                                    break;
                                case "17":
                                    cls.bModifyDate = true;
                                    break;
                                case "18":
                                    cls.bViewCustomerStatement = true;
                                    break;
                                case "19":
                                    cls.bShowCostPrice = true;
                                    break;
                                case "20":
                                    cls.bShowSupplier = true;
                                    break;
                                case "21":
                                    cls.bPayCommission = true;
                                    break;
                                case "22":
                                    cls.bReorderAllow = true;
                                    break;
                                case "23":
                                    cls.bCanChangeControsAttribute = true;
                                    break;
                            }
                        }
                    }
                }
            }
            catch
            {
                return cls;
            }

        }
        return cls;
    }



    public static DataTable GetEmployeeListbyLocationIdandDepartmentValue(string strLocationId, string strDepartmentId, bool IsPayroll, string strConString, string strCompId, string strBrandId, string strRoleId, string strEmpId, string strUserId)
    {
        //if you want  to employee list in attendance section then is payroll should be false 
        // for payroll section is should be true

        string strDepId = string.Empty;
        EmployeeMaster objEmp = new EmployeeMaster(strConString);
        DataTable dtEmp = new DataTable();
        Common objcmn = new Common(strConString);
        if (IsPayroll)
        {
            dtEmp = objEmp.GetEmployee_InPayroll(strCompId);
        }
        else
        {
            dtEmp = objEmp.GetEmployeeMasterOnRole(strCompId);
        }

        dtEmp = new DataView(dtEmp, "Brand_Id='" + strBrandId + "'  and Location_Id in (" + strLocationId + ")", "", DataViewRowState.CurrentRows).ToTable();


        strDepId = objcmn.GetRoleDataPermission(strLocationId, "D", strLocationId, strUserId, strCompId, strEmpId);

        if (strDepId == "")
        {
            strDepId = "0";
        }

        dtEmp = new DataView(dtEmp, "Department_Id in(" + strDepId + ")", "", DataViewRowState.CurrentRows).ToTable();

        if (strDepartmentId != "0")
        {
            dtEmp = new DataView(dtEmp, "Department_Id = " + strDepartmentId + "", "", DataViewRowState.CurrentRows).ToTable();
        }

        return dtEmp;

    }

    public static DataTable GetEmployeeListbyLocationIdandDepartmentValue(string strDepartmentId, bool IsPayroll, string strConString, string strCompId, string strBrandId, string strRoleId, string strUserId, string strEmpId)
    {
        //if you want  to employee list in attendance section then is payroll should be false 
        // for payroll section is should be true

        string strDepId = string.Empty;
        EmployeeMaster objEmp = new EmployeeMaster(strConString);
        DataTable dtEmp = new DataTable(strConString);
        Common objcmn = new Common(strConString);
        if (IsPayroll)
        {
            dtEmp = objEmp.GetEmployee_InPayroll(strCompId);
        }
        else
        {
            dtEmp = objEmp.GetEmployeeMasterOnRole(strCompId);
        }

        dtEmp = new DataView(dtEmp, "Brand_Id='" + strBrandId + "'", "", DataViewRowState.CurrentRows).ToTable();


        strDepId = objcmn.GetRoleDataPermission(strRoleId, "D", strUserId, strCompId, strEmpId);

        if (strDepId == "")
        {
            strDepId = "0";
        }

        dtEmp = new DataView(dtEmp, "Department_Id in(" + strDepId + ")", "", DataViewRowState.CurrentRows).ToTable();

        if (strDepartmentId != "0")
        {
            dtEmp = new DataView(dtEmp, "Department_Id = " + strDepartmentId + "", "", DataViewRowState.CurrentRows).ToTable();
        }

        return dtEmp;

    }







    public static DateTime getCountryTimeFormatStatic(string time, string strTimeZoneId)
    {
        var timeZones = TimeZoneInfo.GetSystemTimeZones();
        DateTime dbDateTime = Convert.ToDateTime(time);
        DateTimeOffset dbDateTimeOffset = new DateTimeOffset(dbDateTime, TimeSpan.Zero);
        try
        {
            //HttpContext.Current.Session["TimeZoneId"].ToString()
            TimeZoneInfo userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(strTimeZoneId);
            DateTimeOffset userDateTimeOffset = TimeZoneInfo.ConvertTime(dbDateTimeOffset, userTimeZone);
            //return Convert.ToDateTime(userDateTimeOffset);
            string datetime = userDateTimeOffset.ToString("dd-MMM-yyyy HH:mm");
            return Convert.ToDateTime(datetime);
        }
        catch
        {
            return DateTime.Now;
        }
    }


    public class DashboardFilter
    {
        public string from_date { get; set; }
        public string to_date { get; set; }
        public string store_no { get; set; }
        public string filter_type { get; set; }
    }

    public enum MyDashboardFilter
    {
        Today,
        Yesterday,
        ThisWeek,
        ThisMonth,
        ThisQuarter,
        ThisYear,
        Custom
    }

    public static DashboardFilter getDashboardFilterParamValues(MyDashboardFilter filterType, string fromDate, string toDate, string storeNo)
    {
        DashboardFilter cls = new DashboardFilter();
        try
        {
            DateTime todayDate = GetTimeZoneDate(DateTime.UtcNow, TimeZone.CurrentTimeZone.ToString());
            switch (filterType)
            {
                case MyDashboardFilter.Today:
                    //cls.from_date = cls.to_date = DateTime.Now.ToString("dd-MMM-yyyy");
                    cls.from_date = cls.to_date = todayDate.ToString("dd-MMM-yyyy");
                    break;
                case MyDashboardFilter.Yesterday:
                    //cls.from_date = cls.to_date = DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy");
                    cls.from_date = cls.to_date = todayDate.AddDays(-1).ToString("dd-MMM-yyyy");
                    break;
                case MyDashboardFilter.ThisWeek:
                    //cls.from_date = DateTime.Now.AddDays(-1 * (int)DateTime.Today.DayOfWeek).ToString("dd-MMM-yyyy");
                    //cls.to_date = DateTime.Now.ToString("dd-MMM-yyyy");
                    cls.from_date = todayDate.AddDays(-1 * (int)todayDate.DayOfWeek).ToString("dd-MMM-yyyy");
                    cls.to_date = todayDate.ToString("dd-MMM-yyyy");
                    break;
                case MyDashboardFilter.ThisMonth:
                    //cls.from_date = (new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)).ToString("dd-MMM-yyyy");
                    //cls.to_date = DateTime.Now.ToString("dd-MMM-yyyy");
                    cls.from_date = (new DateTime(todayDate.Year, todayDate.Month, 1)).ToString("dd-MMM-yyyy");
                    cls.to_date = todayDate.ToString("dd-MMM-yyyy");
                    break;
                case MyDashboardFilter.ThisQuarter:
                    //var current_month = DateTime.Today.Month;
                    var current_month = todayDate.Month;
                    if (current_month >= 1 && current_month <= 3)
                    {
                        //cls.from_date = (new DateTime(DateTime.Today.Year, 1, 1)).ToString("dd-MMM-yyyy");
                        cls.from_date = (new DateTime(todayDate.Year, 1, 1)).ToString("dd-MMM-yyyy");
                    }
                    else if (current_month >= 4 && current_month <= 6)
                    {
                        //cls.from_date = (new DateTime(DateTime.Today.Year, 4, 1)).ToString("dd-MMM-yyyy");
                        cls.from_date = (new DateTime(todayDate.Year, 4, 1)).ToString("dd-MMM-yyyy");
                    }
                    else if (current_month >= 7 && current_month <= 9)
                    {
                        //cls.from_date = (new DateTime(DateTime.Today.Year, 7, 1)).ToString("dd-MMM-yyyy");
                        cls.from_date = (new DateTime(todayDate.Year, 7, 1)).ToString("dd-MMM-yyyy");
                    }
                    else if (current_month >= 9 && current_month <= 12)
                    {
                        //cls.from_date = (new DateTime(DateTime.Today.Year, 9, 1)).ToString("dd-MMM-yyyy");
                        cls.from_date = (new DateTime(todayDate.Year, 9, 1)).ToString("dd-MMM-yyyy");
                    }
                    //cls.to_date = DateTime.Now.ToString("dd-MMM-yyyy");
                    cls.to_date = todayDate.ToString("dd-MMM-yyyy");
                    break;
                case MyDashboardFilter.ThisYear:
                    //cls.from_date = (new DateTime(DateTime.Today.Year, 1, 1)).ToString("dd-MMM-yyyy");
                    //cls.to_date = DateTime.Now.ToString("dd-MMM-yyyy");
                    cls.from_date = (new DateTime(todayDate.Year, 1, 1)).ToString("dd-MMM-yyyy");
                    cls.to_date = todayDate.ToString("dd-MMM-yyyy");
                    break;
                case MyDashboardFilter.Custom:
                    cls.from_date = DateTime.Parse(fromDate).ToString("dd-MMM-yyyy");
                    cls.to_date = DateTime.Parse(toDate).ToString("dd-MMM-yyyy");
                    break;
            }
            //DateTime NewDate = Common.GetTimeZoneDate(DateTime.Parse(cls.from_date), HttpContext.Current.Session["timeZone"].ToString());
            cls.filter_type = ((int)filterType).ToString();

            return cls;
        }
        catch (Exception ex)
        {
            return cls;

        }

    }

    public static DateTime GetTimeZoneDate(DateTime dtValue, string strTzName)
    {
        try
        {
            var timeZones = TimeZoneInfo.GetSystemTimeZones();
            DateTime dbDateTime = Convert.ToDateTime(dtValue.ToString());
            //Central America Standard Time
            DateTimeOffset dbDateTimeOffset = new DateTimeOffset(dbDateTime, TimeSpan.Zero);
            try
            {
                TimeZoneInfo userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(strTzName);
                DateTimeOffset userDateTimeOffset = TimeZoneInfo.ConvertTime(dbDateTimeOffset, userTimeZone);
                //return Convert.ToDateTime(userDateTimeOffset);
                string datetime = userDateTimeOffset.ToString("dd-MMM-yyyy HH:mm");
                return Convert.ToDateTime(datetime);
            }
            catch
            {
                return DateTime.Now;
            }

        }
        catch (Exception ex)
        {
            return dtValue;
        }
    }

    public bool IsValidateGstNo(string strGstNo)
    {
        //as per discussion with kiran accountnat india location i added following validation
        bool result = false;
        try
        {
            //lengh should be 15
            if (strGstNo.Length != 15)
            {
                result = false;
            }
            int ab = 0;

            //first two digit should be number
            int.TryParse(strGstNo.Substring(0, 2), out ab);
            if (ab == 0 || ab < 1 || ab > 37)
            {
                return false;
            }

            //next five digit should be alphabet
            if (Regex.Matches(strGstNo.Substring(2, 5), @"[a-zA-Z]").Count != 5)
            {
                return false;
            }

            //next four digit should be number
            if (Regex.Matches(strGstNo.Substring(7, 4), @"[0-9]").Count != 4)
            {
                return false;
            }

            //next one digit should be number
            if (Regex.Matches(strGstNo.Substring(10, 1), @"[0-9]").Count != 1)
            {
                return false;
            }

            //Third last should be 1 or 2
            if (strGstNo.Substring(12, 1).ToLower() == "1" || strGstNo.Substring(12, 1).ToLower() == "2" || strGstNo.Substring(12, 1).ToLower() == "3")
            {

            }
            else
            {
                return false;
            }

            //second last should be Z
            if (strGstNo.Substring(13, 1).ToLower() == "z" || strGstNo.Substring(13, 1).ToLower() == "c")
            {

            }
            else
            {
                return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
        //return result;
    }

    public bool IsValidEmailId(string InputEmail)
    {
        //Regex To validate Email Address
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(InputEmail);
        if (match.Success)
            return true;
        else
            return false;
    }

    public class clsApplicationModules
    {
        public bool isInventoryModule { get; set; } // module_id =142
        public bool isFinanceModule { get; set; } // module_id =150
        public bool isProjectManagementModule { get; set; } //module_id =156
        public bool isServiceManagementModule { get; set; } // module_id =158
        public bool isDutyManagementModule { get; set; } // module_id =172
        public bool isEmailMarketingModule { get; set; } // module_id =152
        public bool isReportBuilderModule { get; set; } // 
        public bool isEmailSystemModule { get; set; } //module_id =157
        public bool isCrmModule { get; set; } // module_id =171
        public bool isAttendanceModule { get; set; } //module_id =108
        public bool isHrAndPayrollModule { get; set; } //module_id = 153
        public bool isArchvingModule { get; set; } // module_id = 151 
    }

    public clsApplicationModules getApplicationModules(string application_id, string company_id)
    {
        clsApplicationModules _cls = null;
        try
        {
            PassDataToSql[] paramList = new PassDataToSql[1];
            paramList[0] = new PassDataToSql("@application_id", application_id, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
            string sql = "select distinct mm.module_id,mm.module_name FROM IT_App_Mod_Object amb INNER JOIN IT_ModuleMaster mm ON amb.Module_Id = mm.Module_Id where amb.application_id=@application_id";
            using (DataTable _dt = objDa.GetDtFromQryWithParam(sql, paramList))
            {
                if (_dt.Rows.Count == 0)
                {
                    return null;
                }
                _cls = new clsApplicationModules();
                foreach (DataRow dr in _dt.Rows)
                {
                    switch (dr["Module_id"].ToString())
                    {
                        case "142": //inventory
                            _cls.isInventoryModule = true;
                            break;
                        case "150":
                            _cls.isFinanceModule = true;
                            break;
                        case "156":
                            _cls.isProjectManagementModule = true;
                            break;
                        case "158":
                            _cls.isServiceManagementModule = true;
                            break;
                        case "152":
                            _cls.isEmailMarketingModule = true;
                            break;
                        case "157":
                            _cls.isEmailSystemModule = true;
                            break;
                        case "171":
                            _cls.isCrmModule = true;
                            break;
                        case "108":
                            _cls.isAttendanceModule = true;
                            break;
                        case "153":
                            _cls.isHrAndPayrollModule = true;
                            break;
                        case "151":
                            _cls.isArchvingModule = true;
                            break;
                    }
                }
            }
            return _cls;

        }
        catch (Exception ex)
        {
            return null;
        }
    }

}