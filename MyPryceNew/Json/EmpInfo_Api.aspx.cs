using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Newtonsoft.Json.Converters;
using System.Configuration;
using Newtonsoft.Json.Serialization;

using PegasusDataAccess;
using System.IO;
using System.Xml;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Threading;

public partial class EmpInfo_Api : System.Web.UI.Page
{
    DataAccessClass objDA = null;
    Att_Leave_Request ObjLeave = null;
    LocationMaster ObjLOcation = null;
    EmployeeMaster objEmp = null;
    SystemParameter ObjSys = null;
    HR_Indemnity_Master objIndemnity = null;
    Set_Emp_SalaryIncrement objEmpSalInc = null;
    EmployeeParameter objEmpParam = null;
    Device_Operation_Lan objDeviceOp = null;
    Att_Employee_Notification objEmpNotice = null;
    CompanyMaster ObjCompany = null;
    hr_laborLaw_leave ObjLabourLeavedetail = null;
    Att_Employee_Leave objEmpleave = null;
    LeaveMaster_deduction ObjLeavededuction = null;
    Set_ApplicationParameter objAppParam = null;
    protected void Page_Load(object sender, EventArgs e)
    {


        objDA = new DataAccessClass(Session["DBConnection"].ToString());
        ObjLeave = new Att_Leave_Request(Session["DBConnection"].ToString());
        ObjLOcation = new LocationMaster(Session["DBConnection"].ToString());
        objEmp = new EmployeeMaster(Session["DBConnection"].ToString());
        ObjSys = new SystemParameter(Session["DBConnection"].ToString());
        objIndemnity = new HR_Indemnity_Master(Session["DBConnection"].ToString());
        objEmpSalInc = new Set_Emp_SalaryIncrement(Session["DBConnection"].ToString());
        objEmpParam = new EmployeeParameter(Session["DBConnection"].ToString());
        objDeviceOp = new Device_Operation_Lan(Session["DBConnection"].ToString());
        objEmpNotice = new Att_Employee_Notification(Session["DBConnection"].ToString());
        ObjCompany = new CompanyMaster(Session["DBConnection"].ToString());
        ObjLabourLeavedetail = new hr_laborLaw_leave(Session["DBConnection"].ToString());
        objEmpleave = new Att_Employee_Leave(Session["DBConnection"].ToString());
        ObjLeavededuction = new LeaveMaster_deduction(Session["DBConnection"].ToString());
        objAppParam = new Set_ApplicationParameter(Session["DBConnection"].ToString());


        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        string registration_code;
        DataTable dtResult = new DataTable();
        dtResult.Columns.Add(new DataColumn("result"));
        string s = string.Empty;
        DataRow row = dtResult.NewRow();
        string strSQL = string.Empty;
        string Emp_Code = string.Empty;
        string strEmpId = string.Empty;
        string strMaxId = string.Empty;
        string strLeaveId = string.Empty;
        string strLocationId = string.Empty;
        string strDeptId = string.Empty;
        string strDesignationId = string.Empty;
        string strReligionId = string.Empty;
        string strNationalityId = string.Empty;
        string strqualificationId = string.Empty;
        DataTable dtEmployee = new DataTable();
        DataTable dtNf = new DataTable();
        string strCompanyId = "1";
        string strBrandId = "1";
        string Normal_OT_Method = string.Empty;
        string Assign_Min = string.Empty;
        string Is_OverTime = string.Empty;
        string Effective_Work_Cal_Method = string.Empty;
        string Is_Partial_Enable = string.Empty;
        string Partial_Leave_Mins = string.Empty;
        string Partial_Leave_Day = string.Empty;
        string Field1 = string.Empty;
        string Field2 = string.Empty;
        string Field12 = string.Empty;
        string Field8 = string.Empty;
        string Field9 = string.Empty;
        string Field10 = string.Empty;
        bool IsIndemnity = false;
        int IndemnityYear = 0;
        int indenmitydays = 10;
        string strCurrLOcationId = string.Empty;
        string strLabourLaw = string.Empty;
        int FinancialYearMonth = 0;
        DateTime FinancialYearStartDate = new DateTime();
        DateTime FinancialYearEndDate = new DateTime();

        //....................
        int Counter = 0;

        try
        {
            registration_code = Request.Form["Query"].ToString();


            JObject results = JObject.Parse(registration_code);

            int b = 0;

            if (results.Last.Path.ToString() == "ADDEMP")
            {
                //for add and update employee


                foreach (var result in results["ADDEMP"])
                {

                    strLocationId = GetLocationId((string)result["Location_Code"]);

                    if (strLocationId == "0")
                    {
                        continue;
                    }
                    else
                    {
                        strCompanyId = strLocationId.Split(',')[0].ToString();
                        strBrandId = strLocationId.Split(',')[1].ToString();
                        strLocationId = strLocationId.Split(',')[2].ToString();
                    }



                    Emp_Code = (string)result["Emp_Code"];

                    strEmpId = GetEmpIdbyEmployeeCode(strCompanyId, Emp_Code).Split(',')[0];



                    strLabourLaw = ObjLOcation.GetLocationMasterById(strCompanyId, strLocationId).Rows[0]["Field3"].ToString();



                    FinancialYearMonth = Convert.ToInt32(objAppParam.GetApplicationParameterValueByParamName("FinancialYearStartMonth", strCompanyId, strBrandId, strLocationId));


                    if (DateTime.Now.Month < FinancialYearMonth)
                    {
                        FinancialYearStartDate = new DateTime(DateTime.Now.Year - 1, FinancialYearMonth, 1);
                        FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
                    }
                    else
                    {
                        FinancialYearStartDate = new DateTime(DateTime.Now.Year, FinancialYearMonth, 1);
                        FinancialYearEndDate = FinancialYearStartDate.AddYears(1).AddDays(-1);
                    }







                    strDeptId = GetDepartmentId((string)result["Department"]);

                    //if (strDeptId == "0")
                    //{
                    //    continue;
                    //}


                    strDesignationId = GetDesignationId((string)result["Designation"]);

                    //if (strDesignationId == "0")
                    //{
                    //    continue;
                    //}


                    strReligionId = GetReligionId((string)result["Religion"]);

                    //if (strReligionId == "0")
                    //{
                    //    continue;
                    //}



                    strqualificationId = GetQualificationId((string)result["Qualification"]);

                    //if (strqualificationId == "0")
                    //{
                    //    continue;
                    //}


                    strNationalityId = GetNationalityId((string)result["Nationality"]);



                    if (Counter == 0)
                    {


                        try
                        {
                            Assign_Min = GetApplicationParameterValueByParamName("Work Day Min", strCompanyId, strBrandId, strLocationId);
                        }
                        catch
                        {
                            Assign_Min = "540";
                        }

                        if (Assign_Min == "")
                        {
                            Assign_Min = "540";
                        }
                        try
                        {
                            Effective_Work_Cal_Method = GetApplicationParameterValueByParamName("Effective Work Calculation Method", strCompanyId, strBrandId, strLocationId);
                        }
                        catch
                        {
                            Effective_Work_Cal_Method = "InOut";
                        }
                        try
                        {
                            Is_OverTime = Convert.ToBoolean(GetApplicationParameterValueByParamName("IsOverTime", strCompanyId, strBrandId, strLocationId)).ToString();
                        }
                        catch
                        {
                            Is_OverTime = false.ToString();
                        }

                        try
                        {

                            Normal_OT_Method = GetApplicationParameterValueByParamName("Over Time Calculation Method", strCompanyId, strBrandId, strLocationId);
                        }
                        catch
                        {
                            Normal_OT_Method = "Work Hour";
                        }


                        try
                        {
                            Is_Partial_Enable = Convert.ToBoolean(GetApplicationParameterValueByParamName("Partial_Leave_Enable", strCompanyId, strBrandId, strLocationId)).ToString();
                        }
                        catch
                        {
                            Is_Partial_Enable = false.ToString();
                        }



                        try
                        {
                            Partial_Leave_Mins = GetApplicationParameterValueByParamName("Total Partial Leave Minutes", strCompanyId, strBrandId, strLocationId);

                        }
                        catch
                        {
                            Partial_Leave_Mins = "240";
                        }



                        try
                        {
                            Partial_Leave_Day = GetApplicationParameterValueByParamName("Partial Leave Minute Use In A Day", strCompanyId, strBrandId, strLocationId);
                        }
                        catch
                        {
                            Partial_Leave_Day = "60";
                        }


                        try
                        {
                            Field1 = Convert.ToBoolean(GetApplicationParameterValueByParamName("Is_Late_Penalty", strCompanyId, strBrandId, strLocationId)).ToString();
                        }
                        catch
                        {
                            Field1 = false.ToString();
                        }



                        try
                        {
                            Field2 = Convert.ToBoolean(GetApplicationParameterValueByParamName("Is_Early_Penalty", strCompanyId, strBrandId, strLocationId)).ToString();
                        }
                        catch
                        {
                            Field2 = false.ToString();
                        }



                        try
                        {
                            Field12 = GetApplicationParameterValueByParamName("Half_Day_Count", strCompanyId, strBrandId, strLocationId);
                        }
                        catch
                        {

                        }


                        Field8 = GetApplicationParameterValueByParamName("Sal_Increment_Duration", strCompanyId, strBrandId, strLocationId);
                        Field9 = GetApplicationParameterValueByParamName("Sal_Increment_Per_Fresher_From", strCompanyId, strBrandId, strLocationId);
                        Field10 = GetApplicationParameterValueByParamName("Sal_Increment_Per_Fresher_To", strCompanyId, strBrandId, strLocationId);



                        try
                        {
                            IsIndemnity = Convert.ToBoolean(GetApplicationParameterValueByParamName("IsIndemnity", strCompanyId, strBrandId, strLocationId));
                        }
                        catch
                        {

                        }
                        try
                        {
                            IndemnityYear = Convert.ToInt32(GetApplicationParameterValueByParamName("IndemnityYear", strCompanyId, strBrandId, strLocationId));
                        }
                        catch
                        {

                        }
                        try
                        {
                            indenmitydays = Convert.ToInt32(GetApplicationParameterValueByParamName("IndemnityDayas", strCompanyId, strBrandId, strLocationId));
                        }
                        catch
                        {

                        }

                        dtNf = objEmpNotice.GetAllNotification_By_NOtificationType("Report");
                    }


                    if (strEmpId == "0")
                    {

                        b = objEmp.InsertEmployeeMaster(strCompanyId, (string)result["Emp_Name"], (string)result["Emp_Name_L"], (string)result["Emp_Code"], "", strBrandId, strLocationId, strDeptId, (string)result["Civil_Id"], strDesignationId, strReligionId, strNationalityId, strqualificationId, ObjSys.getDateForInput((string)result["DOB"]).ToString(), ObjSys.getDateForInput((string)result["DOJ"]).ToString(), "On Role", "1900-01-01 00:00:00.000", (string)result["Gender"], "Employee", false.ToString(), "", "", "0", true.ToString(), DateTime.Now.ToString(), true.ToString(), "SuperAdmin", DateTime.Now.ToString(), "SuperAdmin", DateTime.Now.ToString(), (string)result["Email_Id"], (string)result["Phone_No"], "", "", "", "True", "","","","","");





                        dtEmployee = objDA.return_DataTable("select Emp_Code,Field4,DOJ,Gender from set_employeemaster where emp_id=" + b.ToString() + "");

                        if (dtEmployee.Rows.Count > 0)
                        {


                            objEmp.InsertEmployeeLocationTransfer(b.ToString(), strLocationId, strLocationId, DateTime.Now.ToString(), "Employee Created", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), "SuperAdmin", DateTime.Now.ToString(), "SuperAdmin", DateTime.Now.ToString());

                            int Indemnity = objIndemnity.InsertIndemnityRecord("0", strCompanyId, b.ToString(), Convert.ToDateTime(dtEmployee.Rows[0]["DOJ"].ToString()).AddYears(IndemnityYear).ToString(), "Pending", "", "", "", "", "", "", "", "", "", "", "", "", true.ToString(), "SuperAdmin", DateTime.Now.ToString(), "SuperAdmin", DateTime.Now.ToString());
                            SystemLog.SaveSystemLog("Employee Master", DateTime.Now.ToString(), strCompanyId, "SuperAdmin", "Employee Saved", "", "", true.ToString(), "SuperAdmin", DateTime.Now.ToString(), "SuperAdmin", DateTime.Now.ToString(), Session["DBConnection"].ToString());

                            InsertEmployeeParameterOnEmployeeInsert(strCompanyId, b.ToString(), Convert.ToDateTime(dtEmployee.Rows[0]["DOJ"].ToString()), Normal_OT_Method, Assign_Min, Is_OverTime, Effective_Work_Cal_Method, Is_Partial_Enable, Partial_Leave_Mins, Partial_Leave_Day, Field1, Field2, Field12, Field8, Field9, Field10);

                            objEmpParam.InsertEmpParameterNew(b.ToString(), strCompanyId, "IsIndemnity", IsIndemnity.ToString(), "", "", true.ToString(), "SuperAdmin", DateTime.Now.ToString());
                            // Indemnity Duration
                            objEmpParam.InsertEmpParameterNew(b.ToString(), strCompanyId, "IndemnityYear", IndemnityYear.ToString(), "", "", true.ToString(), "SuperAdmin", DateTime.Now.ToString());
                            objEmpParam.InsertEmpParameterNew(b.ToString(), strCompanyId, "IndemnityDayas", indenmitydays.ToString(), "", "", true.ToString(), "SuperAdmin", DateTime.Now.ToString());

                            foreach (DataRow dr in dtNf.Rows)
                            {
                                try
                                {
                                    objEmpNotice.InsertEmployeeNotification(b.ToString(), dr["Notification_Id"].ToString(), "", "", "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), "SuperAdmin", DateTime.Now.ToString(), "SuperAdmin", DateTime.Now.ToString());
                                }
                                catch
                                {

                                }
                            }

                            string strGender = dtEmployee.Rows[0]["Gender"].ToString() == "M" ? "Male" : "Female";

                            //code added by jitendra upadhyay on 08/02/2018
                            //for insert default configuration according labour law

                            //code start
                            if (strLabourLaw.Trim() != "0" && strLabourLaw.Trim() != "")
                            {

                                DataTable dtleavedetail = ObjLabourLeavedetail.GetRecord_By_LaborLawId(strLabourLaw);

                                dtleavedetail = new DataView(dtleavedetail, "Gender='" + strGender + "' or Gender='Both'", "", DataViewRowState.CurrentRows).ToTable();


                                foreach (DataRow dr in dtleavedetail.Rows)
                                {
                                    objEmpleave.InsertEmployeeLeave(strCompanyId, b.ToString(), dr["Leave_Type_Id"].ToString(), dr["Total_Leave_days"].ToString(), dr["Paid_Leave_days"].ToString(), "100", dr["schedule_type"].ToString(), true.ToString(), dr["is_yearcarry"].ToString(), "", "", "", dr["is_rule"].ToString(), dr["is_auto"].ToString(), true.ToString(), DateTime.Now.ToString(), true.ToString(), "SuperAdmin", DateTime.Now.ToString(), "SuperAdmin", DateTime.Now.ToString());

                                    SaveLeave(strCompanyId, "No", Convert.ToDateTime((string)result["DOJ"]), dr["Leave_Type_Id"].ToString(), b.ToString(), dr["schedule_type"].ToString(), dr["Total_Leave_days"].ToString(), dr["Paid_Leave_days"].ToString(), dr["is_yearcarry"].ToString(), "", "", "", dr["is_rule"].ToString(), FinancialYearStartDate, FinancialYearEndDate);

                                    //here we are checking that any deduction slab exists or not for selected leave type

                                    DataTable dtdeduction = ObjLeavededuction.GetRecordbyLeaveTypeId(dr["Leave_Type_Id"].ToString()).DefaultView.ToTable(true, "Trans_Id", "DaysFrom", "Daysto", "Deduction_Percentage");

                                    foreach (DataRow childrow in dtdeduction.Rows)
                                    {
                                        ObjLeavededuction.InsertRecord(strCompanyId, strBrandId, strLocationId, dr["Leave_Type_Id"].ToString(), b.ToString(), childrow["DaysFrom"].ToString(), childrow["Daysto"].ToString(), childrow["Deduction_Percentage"].ToString(), "", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), "superadmin", DateTime.Now.ToString());
                                    }
                                }
                            }


                        }


                    }
                    else
                    {
                        objDA.execute_Command("update Set_EmployeeMaster set  Location_Id=" + strLocationId + " ,Emp_Code ='" + (string)result["Emp_Code"] + "', Civil_Id ='" + (string)result["Civil_Id"] + "', Emp_Name ='" + (string)result["Emp_Name"] + "', Emp_Name_L ='" + (string)result["Emp_Name_L"] + "', Department_Id=" + strDeptId + ", Designation_Id =" + strDesignationId + ", Religion_Id =" + strReligionId + ", Nationality_Id =" + strNationalityId + ", Qualification_Id=" + strqualificationId + ", DOB ='" + ObjSys.getDateForInput((string)result["DOB"]).ToString() + "', DOJ ='" + ObjSys.getDateForInput((string)result["DOJ"]).ToString() + "', Email_Id='" + (string)result["Email_Id"] + "', Phone_No='" + (string)result["Phone_No"] + "',Gender='"+(string)result["Gender"]+ "' where Emp_id=" + strEmpId + "");

                    }


                    Counter++;


                }

                row[0] = Counter.ToString() + " row affected";

            }
            else if (results.Last.Path.ToString() == "CNGEMP")
            {
                //for update employee location

                foreach (var result in results["CNGEMP"])
                {
                    strEmpId = GetEmpIdbyEmployeeCode((string)result["Emp_Code"]).Split(',')[0];
                    strCurrLOcationId = GetEmpIdbyEmployeeCode((string)result["Emp_Code"]).Split(',')[1];

                    if (strEmpId == "0")
                    {
                        continue;
                    }

                    strLocationId = GetLocationId((string)result["New_Location_Code"]);

                    strLocationId = strLocationId.Split(',')[2].ToString();

                    if (strCurrLOcationId != strLocationId)
                    {
                        objDA.execute_Command("update set_employeemaster set Location_Id=" + strLocationId + " where emp_id=" + strEmpId + "");


                        objEmp.InsertEmployeeLocationTransfer(strEmpId, strCurrLOcationId, strLocationId, DateTime.Now.ToString(), "Location Updated", "", "", "", "", false.ToString(), DateTime.Now.ToString(), true.ToString(), "SuperAdmin", DateTime.Now.ToString(), "SuperAdmin", DateTime.Now.ToString());

                        Counter++;
                    }



                }


                row[0] = Counter.ToString() + " row affected";
            }
            else if (results.Last.Path.ToString() == "DELEMP")
            {
                //for deactivate employee

                foreach (var result in results["DELEMP"])
                {
                    strEmpId = GetEmpIdbyEmployeeCode((string)result["Emp_Code"]).Split(',')[0];

                    if (strEmpId == "0")
                    {
                        continue;
                    }

                    objDA.execute_Command("update set_employeemaster  set IsActive='False' where Emp_id=" + strEmpId + "");



                    ThreadStart ts = delegate ()
                    {
                        DataTable dtDevice = objDA.return_DataTable("select IP_Address,Port from Att_DeviceMaster where IsActive='True'");

                        foreach (DataRow dr in dtDevice.Rows)
                        {
                            if (Common.IsAlive(dr["IP_Address"].ToString()))
                            {
                                objDeviceOp.DelSingleUserFinger(dr["IP_Address"].ToString(), Convert.ToInt32(dr["Port"].ToString()), Convert.ToInt32((string)result["Emp_Code"]),0);
                                objDeviceOp.DelSingleUserFace(dr["IP_Address"].ToString(), Convert.ToInt32(dr["Port"].ToString()), (string)result["Emp_Code"]);


                            }

                        }
                    };


                    Thread t = new Thread(ts);

                    // Run the thread.
                    t.Start();

                    Counter++;

                }

                row[0] = Counter.ToString() + " row affected";

            }


        }
        catch (Exception Ex)
        {
            row[0] = Ex.ToString();
        }
        dtResult.Rows.Add(row);
        Common objcmn = new Common(Session["DBConnection"].ToString());


        Response.Write(DataTableToJSONWithJSONNet(dtResult));

    }

    public static bool IsAlive(string aIP)
    {
        //bool result = false;


        try
        {
            TcpClient client = new TcpClient(aIP, 4370);
            return true;
        }
        catch (Exception ex)
        {
            //MessageBox.Show("Error pinging host:'" + aIP + ":4370'");
            return false;
        }




    }



    public void SaveLeave(string strCompanyId, string Edit, DateTime dtJoining, string LeaveTypeId, string EmpId, string SchType, string AssignLeave, string PaidLeave, string IsYearCarry, string PrevSchduleType, string PrevAssignLeave, string TransNo, string IsRule, DateTime FinancialYearStartDate, DateTime FinancialYearEndDate)
    {


        //code commneted by jitendra on 24-05-2017

        //new code wrote for proper leave assigning according date of joining

        /// means employee joined in january but hr forgot to assign leave in that case system assigning leave according current month instead of joining month and year 


        //code start




        if (dtJoining > DateTime.Now)
        {
            return;
        }






        double TotalAssignLeave = Convert.ToDouble(AssignLeave);
        double TotalpaidLeave = Convert.ToDouble(PaidLeave);

        if (SchType == "Yearly")
        {

            if (dtJoining >= FinancialYearStartDate)
            {
                int month = 1 + (FinancialYearEndDate.Month - dtJoining.Month) + 12 * (FinancialYearEndDate.Year - dtJoining.Year);

                if (Convert.ToBoolean(IsRule) == false)
                {
                    TotalAssignLeave = (TotalAssignLeave / 12) * month;

                    TotalAssignLeave = Math.Round(TotalAssignLeave);

                    TotalpaidLeave = (TotalpaidLeave / 12) * month;

                    TotalpaidLeave = Math.Round(TotalpaidLeave);
                }
            }

        }

        //here we are deleting previous row for same leave type




        if (SchType == "Monthly")
        {
            if (TransNo.Trim() != "")
            {

                objEmpleave.DeleteEmployeeLeaveTransByEmpIdandleaveTypeId(EmpId, LeaveTypeId, DateTime.Now.Month.ToString(), FinancialYearStartDate.Year.ToString());

            }


            objEmpleave.InsertEmployeeLeaveTrans(strCompanyId, EmpId, LeaveTypeId, FinancialYearStartDate.Year.ToString(), DateTime.Now.Month.ToString(), "0", TotalAssignLeave.ToString(), TotalAssignLeave.ToString(), "0", TotalAssignLeave.ToString(), "0", TotalpaidLeave.ToString(), TotalpaidLeave.ToString(), "Open", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), "superadmin", DateTime.Now.ToString(), "superadmin", DateTime.Now.ToString());
        }
        else
        {

            int totalLogPostedCount = Convert.ToInt32(objDA.return_DataTable("SELECT COUNT(*) from pay_employee_attendance where Emp_Id='" + EmpId + "' and DATEADD(year, pay_employee_attendance.year-1900, DATEADD(month, pay_employee_attendance.Month-1, DATEADD(day, 1-1, 0)))>='" + FinancialYearStartDate + "' and DATEADD(year, pay_employee_attendance.year-1900, DATEADD(month, pay_employee_attendance.Month-1, DATEADD(day, 1-1, 0)))<='" + FinancialYearEndDate + "'").Rows[0][0].ToString());

            double Remainingdays = 0;



            Remainingdays = ((Convert.ToDouble(AssignLeave) / 12) * totalLogPostedCount);


            if (Remainingdays.ToString().Contains("."))
            {
                Remainingdays = Convert.ToDouble(Remainingdays.ToString().Split('.')[0].ToString());
            }


            if (TransNo.Trim() != "")
            {

                objEmpleave.DeleteEmployeeLeaveTransByEmpIdandleaveTypeId(EmpId, LeaveTypeId, "0", FinancialYearStartDate.Year.ToString());

            }

            if (Convert.ToBoolean(IsRule) == true)
            {
                objEmpleave.InsertEmployeeLeaveTrans(strCompanyId, EmpId, LeaveTypeId, FinancialYearStartDate.Year.ToString(), "0", "0", TotalAssignLeave.ToString(), TotalAssignLeave.ToString(), "0", Remainingdays.ToString(), "0", TotalpaidLeave.ToString(), TotalpaidLeave.ToString(), "Open", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), "superadmin", DateTime.Now.ToString(), "superadmin", DateTime.Now.ToString());
            }
            else
            {
                objEmpleave.InsertEmployeeLeaveTrans(strCompanyId, EmpId, LeaveTypeId, FinancialYearStartDate.Year.ToString(), "0", "0", TotalAssignLeave.ToString(), TotalAssignLeave.ToString(), "0", TotalAssignLeave.ToString(), "0", TotalpaidLeave.ToString(), TotalpaidLeave.ToString(), "Open", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), "superadmin", DateTime.Now.ToString(), "superadmin", DateTime.Now.ToString());
            }
        }

        SystemLog.SaveSystemLog("Employee Master : Leave", DateTime.Now.ToString(), strCompanyId, "superadmin", "Full Day Leave Saved", "", "", true.ToString(), "superadmin", DateTime.Now.ToString(), "superadmin", DateTime.Now.ToString(), Session["DBConnection"].ToString());
    }

    public string GetApplicationParameterValueByParamName(string ParamName, string CompanyId, string strBrandId, string strLocId)
    {
        string str = string.Empty;
        DataTable dtInfo = new DataTable();
        PassDataToSql[] paramList = new PassDataToSql[6];
        paramList[0] = new PassDataToSql("@Trans_Id", "0", PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[1] = new PassDataToSql("@Company_Id", CompanyId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[2] = new PassDataToSql("@Brand_Id", strBrandId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);
        paramList[3] = new PassDataToSql("@Location_Id", strLocId, PassDataToSql.ParaTypeList.Int, PassDataToSql.ParaDirectonList.Input);

        paramList[4] = new PassDataToSql("@Param_Name", ParamName, PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        paramList[5] = new PassDataToSql("@optype", "1", PassDataToSql.ParaTypeList.Nvarchar, PassDataToSql.ParaDirectonList.Input);
        dtInfo = objDA.Reuturn_Datatable_Search("sp_Set_ApplicationParameter_SelectRow", paramList);

        if (dtInfo.Rows.Count > 0)
        {
            str = dtInfo.Rows[0]["Param_Value"].ToString();
        }
        return str;
    }

    public void InsertEmployeeParameterOnEmployeeInsert(string CompanyId, string Emp_Id, DateTime JoinDate, string Normal_OT_Method, string Assign_Min, string Is_OverTime, string Effective_Work_Cal_Method, string Is_Partial_Enable, string Partial_Leave_Mins, string Partial_Leave_Day, string Field1, string Field2, string Field12, string Field8, string Field9, string Field10)
    {
        string Basic_Salary = "0";
        string Salary_Type = "Monthly";
        string Currency_Id = "0";
        try
        {
            Currency_Id = ObjCompany.GetCompanyMasterById(CompanyId).Rows[0]["Currency_Id"].ToString();
        }
        catch
        {

        }


        string Normal_OT_Type = "2";
        string Normal_OT_Value = "100";
        string Normal_HOT_Type = "2";
        string Normal_HOT_Value = "100";
        string Normal_WOT_Type = "2";
        string Normal_WOT_Value = "100";
        string Is_Partial_Carry = false.ToString();
        string Field3 = string.Empty;
        Field3 = true.ToString();
        string Field4 = false.ToString();
        string Field5 = false.ToString();
        string Field6 = false.ToString();
        string Field7 = DateTime.Now.ToString();
        string Field11 = string.Empty;
        Field11 = "Fresher";
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
        //objEmpSalInc.InsertEmpSalaryIncrement(Session["CompId"].ToString(), hdnEmpIdSal.Value, BasicSal.ToString(), ddlCategory1.SelectedValue, IncrementDate.Month.ToString(), IncrementDate.Year.ToString(), txtIncrementPerTo1.Text, IncrementValue.ToString(), IncrementSalary.ToString(), Duration.ToString(), txtIncrementPerFrom1.Text, txtIncrementPerTo1.Text, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString(), Session["UserId"].ToString(), DateTime.Now.ToString());

        objEmpSalInc.InsertEmpSalaryIncrement(CompanyId, Emp_Id, BasicSal.ToString(), "Fresher", IncrementDate.Month.ToString(), IncrementDate.Year.ToString(), Field10, IncrementValue.ToString(), IncrementSalary.ToString(), Duration.ToString(), Field9, Field10, "", "", "", true.ToString(), DateTime.Now.ToString(), true.ToString(), "SuperAdmin", DateTime.Now.ToString(), "SuperAdmin", DateTime.Now.ToString());

        objEmpParam.InsertEmployeeParameter(CompanyId, Emp_Id, "0", Salary_Type, Currency_Id, Assign_Min, Effective_Work_Cal_Method, Is_OverTime, Normal_OT_Method, Normal_OT_Type, Normal_OT_Value, Normal_HOT_Type, Normal_HOT_Value, Normal_WOT_Type, Normal_WOT_Value, Is_Partial_Enable, Partial_Leave_Mins, Partial_Leave_Day, Is_Partial_Carry, Field1, Field2, Field3, Field4, Field5, Field6, Field7, Field8, Field9, Field10, Field11, Field12, true.ToString(), "", true.ToString(), "SuperAdmin", DateTime.Now.ToString(), "SuperAdmin", DateTime.Now.ToString(), "0", true.ToString(), "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0");
    }

    public string GetLocationId(string strLocationCode)
    {

        DataTable dtLoc = objDA.return_DataTable("select company_id,brand_id,Location_Id from set_locationmaster where Location_Code='" + strLocationCode + "' and isactive='True'");

        if (dtLoc.Rows.Count > 0)
        {
            return dtLoc.Rows[0][0].ToString() + "," + dtLoc.Rows[0][1].ToString() + "," + dtLoc.Rows[0][2].ToString();
        }
        else
        {
            return "0";
        }

    }

    public string GetEmpIdbyEmployeeCode(string strCompanyId, string strEmpCode)
    {
        string strEmpId = string.Empty;

        DataTable dtEmp = objDA.return_DataTable("select emp_id,Location_Id from set_employeemaster where Company_Id=" + strCompanyId + " and Emp_Code='" + strEmpCode + "'");

        if (dtEmp.Rows.Count > 0)
        {
            strEmpId = dtEmp.Rows[0]["emp_id"].ToString() + "," + dtEmp.Rows[0]["Location_id"].ToString();
        }
        else
        {
            strEmpId = "0,0";
        }

        return strEmpId;

    }



    public string GetEmpIdbyEmployeeCode(string strEmpCode)
    {
        string strEmpId = string.Empty;

        DataTable dtEmp = objDA.return_DataTable("select emp_id,Location_Id from set_employeemaster where Emp_Code='" + strEmpCode + "'");

        if (dtEmp.Rows.Count > 0)
        {
            strEmpId = dtEmp.Rows[0]["emp_id"].ToString() + "," + dtEmp.Rows[0]["Location_id"].ToString();
        }
        else
        {
            strEmpId = "0,0";
        }

        return strEmpId;

    }

    public string GetDepartmentId(string strDepName)
    {
        DataTable dt = objDA.return_DataTable("select Dep_Id from set_departmentmaster where dep_name='" + strDepName.Trim() + "'");

        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }

    }

    public string GetDesignationId(string strDesgName)
    {
        DataTable dt = objDA.return_DataTable("select Designation_id from set_designationmaster where designation='" + strDesgName.Trim() + "'");

        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }

    }

    public string GetReligionId(string strReligionName)
    {
        DataTable dt = objDA.return_DataTable("select Religion_id from set_religionmaster where religion='" + strReligionName.Trim() + "'");

        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }


    }

    public string GetQualificationId(string strQualificationName)
    {
        DataTable dt = objDA.return_DataTable("select Qualification_Id from Set_QualificationMaster where Qualification='" + strQualificationName.Trim() + "'");

        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }


    }

    public string GetNationalityId(string strNationalityName)
    {
        DataTable dt = objDA.return_DataTable("select Nationality_Id from Set_NationalityMaster where Nationality='" + strNationalityName.Trim() + "'");

        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0][0].ToString();
        }
        else
        {
            return "0";
        }


    }

    public string DataTableToJSONWithJSONNet(DataTable table)
    {

        string json = JsonConvert.SerializeObject(table, Newtonsoft.Json.Formatting.Indented);
        return json;
    }

}