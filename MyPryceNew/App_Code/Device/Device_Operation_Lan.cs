using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using zkemkeeper;
using System.Web;

public class Device_Operation_Lan
{
    public zkemkeeper.CZKEMClass axCZKEM1 = new zkemkeeper.CZKEMClass();
    Att_DeviceMaster ObjDevice = null;

    //Device Connection

    public Device_Operation_Lan(string strConString)
    {
        ObjDevice = new Att_DeviceMaster(strConString);
    }

    public bool Device_Connection(string IPAddres, int port, int i)
    {
        int idwErrorCode = 0;


        if (i == 1)
        {
            axCZKEM1.Disconnect();

            return true;
        }
        else
        {
            bool bIsConnected = axCZKEM1.Connect_Net(IPAddres, Convert.ToInt32(port));
            if (bIsConnected == true)
            {
                return true;
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);
                return false;
                //MessageBox.Show("Unable to connect the device,ErrorCode=" + idwErrorCode.ToString(), "Error");
            }
        }
        //  Cursor = Cursors.Default;
    }

    //Get Log
    private DataTable GetLogTable()
    {
        DataTable dt = new DataTable("Dt");
        dt.Columns.Add("sdwEnrollNumber");
        dt.Columns.Add("idwVerifyMode");
        dt.Columns.Add("idwInOutMode");
        dt.Columns.Add("sTimeString");
        dt.Columns.Add("idwWorkcode");

        return dt;
    }
    public DataTable GetUserLog(string IPAddress, int port)
    {



        //if (!Device_Connection(IPAddress, port, 0))
        //{
        //    DataTable dt = new DataTable();
        //    return dt;
        //}
        int idwErrorCode = 0;

        string sdwEnrollNumber = "";
        int idwVerifyMode = 0;
        int idwInOutMode = 0;
        int idwYear = 0;
        int idwMonth = 0;
        int idwDay = 0;
        int idwHour = 0;
        int idwMinute = 0;
        int idwSecond = 0;
        int idwWorkcode = 0;

        int iGLCount = 0;
        int iIndex = 0;
        int iMachineNumber = 1;
        DataTable dtLog = new DataTable("DtLog");
        dtLog = GetLogTable();


        DataColumn dtColumn = new DataColumn("LDateTime");
        dtColumn.DataType = System.Type.GetType("System.DateTime");
        dtLog.Columns.Add(dtColumn);

        dtLog.AcceptChanges();

        axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device
        if (axCZKEM1.ReadGeneralLogData(iMachineNumber))//read all the attendance records to the memory
        {
            while (axCZKEM1.SSR_GetGeneralLogData(iMachineNumber, out sdwEnrollNumber, out idwVerifyMode,
            out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))//get records from the memory
            {
                iGLCount++;

                DataRow row = dtLog.NewRow();
                //lvLogs.Items.Add(iGLCount.ToString());
                row[0] = (sdwEnrollNumber);//modify by Darcy on Nov.26 2009
                row[1] = (idwVerifyMode.ToString());
                row[2] = (idwInOutMode.ToString());
                row[3] = (idwYear.ToString() + "-" + idwMonth.ToString() + "-" + idwDay.ToString() + " " + idwHour.ToString() + ":" + idwMinute.ToString() + ":" + idwSecond.ToString());
                row[4] = (idwWorkcode.ToString());
                row[5] = Convert.ToDateTime(row[3]);
                dtLog.Rows.Add(row);
                iIndex++;
            }
        }
        else
        {
            axCZKEM1.GetLastError(ref idwErrorCode);

            if (idwErrorCode != 0)
            {
                //  MessageBox.Show("Reading data from terminal failed,ErrorCode: " + idwErrorCode.ToString(), "Error");
            }
            else
            {
                //  MessageBox.Show("No data from terminal returns!", "Error");
            }
        }
        axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device
                                                    //Cursor = Cursors.Default;
        Device_Connection(IPAddress, port, 1);
        return dtLog;
    }

    //Get User With Finger Only
    private DataTable GetUserFingerTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("sdwEnrollNumber");
        dt.Columns.Add("sName");
        dt.Columns.Add("idwFingerIndex");
        dt.Columns.Add("sTmpData");
        dt.Columns.Add("iPrivilege");
        dt.Columns.Add("sPassword");
        dt.Columns.Add("sEnabled");
        dt.Columns.Add("iFlag");



        return dt;
    }
    public DataTable GetUserFinger(string IPAddress, int port)
    {
        if (!Device_Connection(IPAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return dt;
        }

        string sdwEnrollNumber = "";
        string sName = "";
        string sPassword = "";
        int iPrivilege = 0;
        bool bEnabled = false;

        int idwFingerIndex = 0; ;
        string sTmpData = "";
        int iTmpLength = 0;
        int iFlag = 0;
        int iMachineNumber = 1;

        axCZKEM1.EnableDevice(iMachineNumber, false);
        DataTable dtUserFinger = GetUserFingerTable();
        axCZKEM1.ReadAllUserID(iMachineNumber);//read all the user information to the memory
        axCZKEM1.ReadAllTemplate(iMachineNumber);//read all the users' fingerprint templates to the memory
        while (axCZKEM1.SSR_GetAllUserInfo(iMachineNumber, out sdwEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))//get all the users' information from the memory
        {
            for (idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
            {
                if (axCZKEM1.GetUserTmpExStr(iMachineNumber, sdwEnrollNumber, idwFingerIndex, out iFlag, out sTmpData, out iTmpLength))//get the corresponding templates string and length from the memory
                {
                    DataRow row = dtUserFinger.NewRow();
                    row[0] = sdwEnrollNumber;
                    row[1] = (sName);
                    row[2] = (idwFingerIndex.ToString());
                    row[3] = (sTmpData);
                    row[4] = (iPrivilege.ToString());
                    row[5] = (sPassword);
                    if (bEnabled == true)
                    {
                        row[6] = ("true");
                    }
                    else
                    {
                        row[6] = ("false");
                    }
                    row[7] = (iFlag.ToString());
                    dtUserFinger.Rows.Add(row);
                }
            }
        }

        axCZKEM1.EnableDevice(iMachineNumber, true);
        Device_Connection(IPAddress, port, 1);
        return dtUserFinger;
    }

    // Upload User
    public bool UploadUser(DataTable dtUserFinger, string IPAddress, int port)
    {
        if (!Device_Connection(IPAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return false;
        }

        if (dtUserFinger.Rows.Count == 0)
        {
            return false;
        }
        int idwErrorCode = 0;

        string sdwEnrollNumber = "";
        string sName = "";
        int idwFingerIndex = 0;
        string sTmpData = "";
        int iPrivilege = 0;
        string sPassword = "";
        int iFlag = 1;
        string sEnabled = "";
        bool bEnabled = false;
        int iUpdateFlag = 1;
        int iMachineNumber = 1;
        axCZKEM1.EnableDevice(iMachineNumber, false);
        if (axCZKEM1.BeginBatchUpdate(iMachineNumber, iUpdateFlag))//create memory space for batching data
        {
            string sLastEnrollNumber = "";//the former enrollnumber you have upload(define original value as 0)
            for (int i = 0; i < dtUserFinger.Rows.Count; i++)
            {
                sdwEnrollNumber = dtUserFinger.Rows[i][0].ToString();
                sName = dtUserFinger.Rows[i][1].ToString();
                idwFingerIndex = Convert.ToInt32(dtUserFinger.Rows[i][2].ToString());
                sTmpData = dtUserFinger.Rows[i][3].ToString();
                iPrivilege = Convert.ToInt32(dtUserFinger.Rows[i][4].ToString());
                sPassword = dtUserFinger.Rows[i][5].ToString();

                sEnabled = dtUserFinger.Rows[i][6].ToString();
                iFlag = Convert.ToInt32(dtUserFinger.Rows[i][7].ToString());



                if (sEnabled == "true")
                {
                    bEnabled = true;
                }
                else
                {
                    bEnabled = false;
                }
                if (sdwEnrollNumber != sLastEnrollNumber)//identify whether the user information(except fingerprint templates) has been uploaded
                {
                    if (axCZKEM1.SSR_SetUserInfo(iMachineNumber, sdwEnrollNumber, sName, sPassword, iPrivilege, bEnabled))//upload user information to the memory
                    {
                        axCZKEM1.SetUserTmpExStr(iMachineNumber, sdwEnrollNumber, idwFingerIndex, iFlag, sTmpData);//upload templates information to the memory
                    }
                    else
                    {
                        axCZKEM1.GetLastError(ref idwErrorCode);
                        //MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
                        //Cursor = Cursors.Default;
                        axCZKEM1.EnableDevice(iMachineNumber, true);
                        return false;
                    }
                }
                else//the current fingerprint and the former one belongs the same user,that is ,one user has more than one template
                {
                    axCZKEM1.SetUserTmpExStr(iMachineNumber, sdwEnrollNumber, idwFingerIndex, iFlag, sTmpData);
                }
                sLastEnrollNumber = sdwEnrollNumber;//change the value of iLastEnrollNumber dynamicly
            }
        }
        axCZKEM1.BatchUpdate(iMachineNumber);//upload all the information in the memory
        axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed

        axCZKEM1.EnableDevice(iMachineNumber, true);

        return true;
        //  MessageBox.Show("Successfully upload fingerprint templates in batches , " + "total:" + lvDownload.Items.Count.ToString(), "Success");
        //for (int i = 0; i < dtUserFinger.Rows.Count; i++)
        //{
        //    sdwEnrollNumber = dtUserFinger.Rows[i][0].ToString();
        //    sName = dtUserFinger.Rows[i][1].ToString();
        //    idwFingerIndex = Convert.ToInt32(dtUserFinger.Rows[i][2].ToString());
        //    sTmpData = dtUserFinger.Rows[i][3].ToString();
        //    iPrivilege = Convert.ToInt32(dtUserFinger.Rows[i][4].ToString());
        //    sPassword = dtUserFinger.Rows[i][5].ToString();

        //    sEnabled = dtUserFinger.Rows[i][6].ToString();
        //    iFlag = Convert.ToInt32(dtUserFinger.Rows[i][7].ToString());
        //    if (sEnabled == "true")
        //    {
        //        bEnabled = true;
        //    }
        //    else
        //    {
        //        bEnabled = false;
        //    }

        //    if (axCZKEM1.SSR_SetUserInfo(iMachineNumber, sdwEnrollNumber, sName, sPassword, iPrivilege, bEnabled))//upload user information to the device
        //    {
        //        axCZKEM1.SetUserTmpExStr(iMachineNumber, sdwEnrollNumber, idwFingerIndex, iFlag, sTmpData);//upload templates information to the device
        //    }
        //    else
        //    {
        //        axCZKEM1.GetLastError(ref idwErrorCode);
        //        // MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
        //        // Cursor = Cursors.Default;
        //        axCZKEM1.EnableDevice(iMachineNumber, true);
        //        return false;
        //    }
        //}
        //axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
        ////  Cursor = Cursors.Default;
        //axCZKEM1.EnableDevice(iMachineNumber, true);
        //Device_Connection(IPAddress, port, 1);
        //return true;
    }

    public bool UploadUserFinger(DataTable dtUserFinger, string IPAddress, int port)
    {
        //if (!Device_Connection(IPAddress, port, 0))
        //{
        //    DataTable dt = new DataTable();
        //    return false;
        //}

        //if (dtUserFinger.Rows.Count == 0)
        //{
        //    return false;
        //}
        int idwErrorCode = 0;

        string sdwEnrollNumber = "";
        string sName = "";
        int idwFingerIndex = 0;
        string sTmpData = "";
        int iPrivilege = 0;
        string sPassword = "";
        int iFlag = 1;
        string sEnabled = "";
        bool bEnabled = false;
        int iUpdateFlag = 1;
        int iMachineNumber = 1;
        //axCZKEM1.EnableDevice(iMachineNumber, false);
        if (axCZKEM1.BeginBatchUpdate(iMachineNumber, iUpdateFlag))//create memory space for batching data
        {
            string sLastEnrollNumber = "";//the former enrollnumber you have upload(define original value as 0)
            for (int i = 0; i < dtUserFinger.Rows.Count; i++)
            {
                sdwEnrollNumber = dtUserFinger.Rows[i][0].ToString();
                sName = dtUserFinger.Rows[i][1].ToString();
                idwFingerIndex = Convert.ToInt32(dtUserFinger.Rows[i][2].ToString());
                sTmpData = dtUserFinger.Rows[i][3].ToString();
                iPrivilege = Convert.ToInt32(dtUserFinger.Rows[i][4].ToString());
                sPassword = dtUserFinger.Rows[i][5].ToString();

                sEnabled = dtUserFinger.Rows[i][6].ToString();
                iFlag = Convert.ToInt32(dtUserFinger.Rows[i][7].ToString());

                axCZKEM1.SetUserTmpExStr(iMachineNumber, sdwEnrollNumber, idwFingerIndex, iFlag, sTmpData);

                //if (sEnabled == "true")
                //{
                //    bEnabled = true;
                //}
                //else
                //{
                //    bEnabled = false;
                //}
                //if (sdwEnrollNumber != sLastEnrollNumber)//identify whether the user information(except fingerprint templates) has been uploaded
                //{
                //    if (axCZKEM1.SSR_SetUserInfo(iMachineNumber, sdwEnrollNumber, sName, sPassword, iPrivilege, bEnabled))//upload user information to the memory
                //    {
                //        axCZKEM1.SetUserTmpExStr(iMachineNumber, sdwEnrollNumber, idwFingerIndex, iFlag, sTmpData);//upload templates information to the memory
                //    }
                //    else
                //    {
                //        axCZKEM1.GetLastError(ref idwErrorCode);
                //        //MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
                //        //Cursor = Cursors.Default;
                //        axCZKEM1.EnableDevice(iMachineNumber, true);
                //        return false;
                //    }
                //}
                //else//the current fingerprint and the former one belongs the same user,that is ,one user has more than one template
                //{
                //    axCZKEM1.SetUserTmpExStr(iMachineNumber, sdwEnrollNumber, idwFingerIndex, iFlag, sTmpData);
                //}
                //sLastEnrollNumber = sdwEnrollNumber;//change the value of iLastEnrollNumber dynamicly
            }
        }
        axCZKEM1.BatchUpdate(iMachineNumber);//upload all the information in the memory
        //axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed

        //axCZKEM1.EnableDevice(iMachineNumber, true);

        return true;

    }
    // Device operation
    public bool ClearUserInfo(string IPAddress, int port)
    {
        if (!Device_Connection(IPAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return false;
        }
        int idwErrorCode = 0;

        int iDataFlag = 5;
        bool b = false;
        int iMachineNumber = 1;
        if (axCZKEM1.ClearData(iMachineNumber, iDataFlag))
        {
            axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed

            b = true;
        }
        else
        {
            axCZKEM1.GetLastError(ref idwErrorCode);
        }
        Device_Connection(IPAddress, port, 1);
        return b;
        ;
    }

    public bool ClearFingerTemplates(string IPAddress, int port)
    {
        if (!Device_Connection(IPAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return false;
        }
        int idwErrorCode = 0;

        int iDataFlag = 2;
        int iMachineNumber = 1;
        bool b = false;
        if (axCZKEM1.ClearData(iMachineNumber, iDataFlag))
        {
            axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
            b = true;
        }
        else
        {
            axCZKEM1.GetLastError(ref idwErrorCode);
        }
        Device_Connection(IPAddress, port, 1);
        return b;
    }

    public bool ClearLog(string IPAddress, int port)
    {
        if (!Device_Connection(IPAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return false;
        }
        int idwErrorCode = 0;

        int iMachineNumber = 1;
        bool b = false;
        if (axCZKEM1.ClearGLog(iMachineNumber))
        {
            axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
            b = true;
        }
        else
        {
            axCZKEM1.GetLastError(ref idwErrorCode);
        }
        Device_Connection(IPAddress, port, 1);
        return b;
    }

    public bool ClearAdminPrivilege(string IPAddress, int port)
    {
        if (!Device_Connection(IPAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return false;
        }


        int idwErrorCode = 0;
        bool b = false;
        int iMachineNumber = 1;

        if (axCZKEM1.ClearAdministrators(iMachineNumber))
        {
            axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
                                                 // MessageBox.Show("Successfully clear administrator privilege from teiminal!", "Success");
            b = true;
        }
        else
        {
            axCZKEM1.GetLastError(ref idwErrorCode);
            //MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
        }
        Device_Connection(IPAddress, port, 1);
        return b;
    }

    public DataTable GetUser(string IPAddress, int port)
    {
        if (!Device_Connection(IPAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return dt;
        }

        string sdwEnrollNumber = "";
        string sName = "";
        string sPassword = "";
        int iPrivilege = 0;
        bool bEnabled = false;
        string sCardnumber = "";
        int iMachineNumber = 1;
        DataTable dtUser = getUser();
        axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device
        axCZKEM1.ReadAllUserID(iMachineNumber);//read all the user information to the memory
        while (axCZKEM1.SSR_GetAllUserInfo(iMachineNumber, out sdwEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))//get user information from memory
        {
            if (axCZKEM1.GetStrCardNumber(out sCardnumber))//get the card number from the memory
            {
                DataRow row = dtUser.NewRow();
                row[0] = sdwEnrollNumber;
                row[1] = (sName);
                row[2] = (sCardnumber);
                row[3] = (iPrivilege.ToString());
                row[4] = (sPassword);
                if (bEnabled == true)
                {
                    row[5] = ("true");
                }
                else
                {
                    row[5] = ("false");
                }
                dtUser.Rows.Add(row);
            }
        }
        axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device
        Device_Connection(IPAddress, port, 1);
        return dtUser;
    }
    public DataTable GetUser(string IPAddress, int port, DataTable dtDbEmp, string strDeviceId, string DeviceName)
    {
        if (!Device_Connection(IPAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return dt;
        }

        int iLength = 0;
        int iFaceIndex = 50;
        string sdwEnrollNumber = "";
        string sName = "";
        string sPassword = "";
        int iPrivilege = 0;
        bool bEnabled = false;
        string sCardnumber = "";
        int iMachineNumber = 1;
        DataTable dtUser = getUser();
        dtUser.Columns.Add("Device_Id");
        dtUser.Columns.Add("IP");
        dtUser.Columns.Add("Port");
        dtUser.Columns.Add("Emp_Id");
        dtUser.Columns.Add("Designation");
        dtUser.Columns.Add("Department");
        dtUser.Columns.Add("DeviceName");
        dtUser.Columns.Add("Finger");
        dtUser.Columns.Add("Face");
        string strEmpId = "0";
        int idwFingerIndex = 0;

        string sTmpData = "";
        int iTmpLength = 0;
        int iFlag = 0;

        axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device

        axCZKEM1.ReadAllUserID(iMachineNumber);//read all the user information to the memory
        while (axCZKEM1.SSR_GetAllUserInfo(iMachineNumber, out sdwEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))//get user information from memory
        {
            if (axCZKEM1.GetStrCardNumber(out sCardnumber))//get the card number from the memory
            {

                DataRow row = dtUser.NewRow();
                row[0] = sdwEnrollNumber;
                row[1] = (sName);
                row[2] = (sCardnumber);
                row[3] = (iPrivilege.ToString());
                row[4] = (sPassword);

                if (bEnabled == true)
                {
                    row[5] = ("true");
                }
                else
                {
                    row[5] = ("false");
                }

                strEmpId = "0";
                DataRow[] drEmp = dtDbEmp.Select("emp_code='" + sdwEnrollNumber + "'");
                if (drEmp.Length > 0)
                {
                    strEmpId = drEmp[0]["emp_id"].ToString();
                    row["Designation"] = drEmp[0]["Designation"].ToString();
                    row["Department"] = drEmp[0]["Department"].ToString();
                }

                row["Face"] = "False";
                row["Finger"] = "False";


                row["emp_id"] = strEmpId;
                row["Device_Id"] = strDeviceId;
                row["IP"] = IPAddress;
                row["Port"] = port;
                row["DeviceName"] = DeviceName;


                //for (idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
                //{
                //if (axCZKEM1.GetUserTmpExStr(iMachineNumber, sdwEnrollNumber, idwFingerIndex, out iFlag, out sTmpData, out iTmpLength))//get the corresponding templates string and length from the memory
                //{
                //    row["Finger"] = "True";

                //}
                ////}

                //if (axCZKEM1.GetUserFaceStr(iMachineNumber, sdwEnrollNumber, iFaceIndex, ref sTmpData, ref iLength))//get the face templates from the memory
                //{
                //    row["Face"] = "True";

                //}


                dtUser.Rows.Add(row);
            }

        }
        axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device
        Device_Connection(IPAddress, port, 1);
        return dtUser;
    }


    public DataTable GetUser(string IPAddress, int port, DataTable dtDbEmp, string strDeviceId, string DeviceName, DataTable dtFace, DataTable dtFinger)
    {
        if (!Device_Connection(IPAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return dt;
        }

        int iLength = 0;
        int iFaceIndex = 50;
        string sdwEnrollNumber = "";
        string sName = "";
        string sPassword = "";
        int iPrivilege = 0;
        bool bEnabled = false;
        string sCardnumber = "";
        int iMachineNumber = 1;
        DataTable dtUser = getUser();
        dtUser.Columns.Add("Device_Id");
        dtUser.Columns.Add("IP");
        dtUser.Columns.Add("Port");
        dtUser.Columns.Add("Emp_Id");
        dtUser.Columns.Add("Designation");
        dtUser.Columns.Add("Department");
        dtUser.Columns.Add("DeviceName");
        dtUser.Columns.Add("Finger", typeof(Boolean));
        dtUser.Columns.Add("Face", typeof(Boolean));
        string strEmpId = "0";
        int idwFingerIndex = 0;

        string sTmpData = "";
        int iTmpLength = 0;
        int iFlag = 0;

        axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device

        axCZKEM1.ReadAllUserID(iMachineNumber);//read all the user information to the memory
        while (axCZKEM1.SSR_GetAllUserInfo(iMachineNumber, out sdwEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))//get user information from memory
        {
            if (axCZKEM1.GetStrCardNumber(out sCardnumber))//get the card number from the memory
            {

                DataRow row = dtUser.NewRow();
                row[0] = sdwEnrollNumber;
                row[1] = (sName);
                row[2] = (sCardnumber);
                row[3] = (iPrivilege.ToString());
                row[4] = (sPassword);

                if (bEnabled == true)
                {
                    row[5] = ("true");
                }
                else
                {
                    row[5] = ("false");
                }

                strEmpId = "0";
                DataRow[] drEmp = dtDbEmp.Select("emp_code='" + sdwEnrollNumber + "'");
                if (drEmp.Length > 0)
                {
                    strEmpId = drEmp[0]["emp_id"].ToString();
                    row["Designation"] = drEmp[0]["Designation"].ToString();
                    row["Department"] = drEmp[0]["Department"].ToString();
                }

                row["Face"] = "False";
                row["Finger"] = "False";

                try
                {
                    if (new DataView(dtFinger, "sdwEnrollNumber='" + sdwEnrollNumber + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                    {
                        row["Finger"] = "True";
                    }
                }
                catch
                {

                }

                try
                {

                    if (new DataView(dtFace, "sUSERID='" + sdwEnrollNumber + "'", "", DataViewRowState.CurrentRows).ToTable().Rows.Count > 0)
                    {
                        row["Face"] = "True";
                    }
                }
                catch
                {

                }


                row["emp_id"] = strEmpId;
                row["Device_Id"] = strDeviceId;
                row["IP"] = IPAddress;
                row["Port"] = port;
                row["DeviceName"] = DeviceName;




                dtUser.Rows.Add(row);
            }

        }
        axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device
        Device_Connection(IPAddress, port, 1);
        return dtUser;
    }


    private DataTable getUser()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("sdwEnrollNumber");
        dt.Columns.Add("sName");
        dt.Columns.Add("sCardnumber");
        dt.Columns.Add("iPrivilege");
        dt.Columns.Add("sPassword");
        dt.Columns.Add("bEnabled");
        return dt;
    }

    public bool RestartDevice(string IPAddress, int port)
    {
        if (!Device_Connection(IPAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return false;
        }
        int iMachineNumber = 1;

        bool b = false;


        int idwErrorCode = 0;


        if (axCZKEM1.RestartDevice(iMachineNumber) == true)
        {
            b = true;
        }
        else
        {
            axCZKEM1.GetLastError(ref idwErrorCode);
        }


        return b;
    }

    public bool InitializeDevice(string IPAddress, int port)
    {
        if (!Device_Connection(IPAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return false;
        }
        int iMachineNumber = 1;

        bool b = false;


        int idwErrorCode = 0;


        if (axCZKEM1.ClearKeeperData(iMachineNumber) == true)
        {
            b = true;
        }
        else
        {
            axCZKEM1.GetLastError(ref idwErrorCode);
        }


        return b;
    }

    public bool SynchTime(string IPAddress, int port)
    {
        if (!Device_Connection(IPAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return false;
        }
        bool b = false;
        int idwErrorCode = 0;
        int iMachineNumber = 1;


        int idwYear = Convert.ToInt32(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Year);
        int idwMonth = Convert.ToInt32(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month);
        int idwDay = Convert.ToInt32(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Day);
        int idwHour = Convert.ToInt32(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Hour);
        int idwMinute = Convert.ToInt32(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Month);
        int idwSecond = Convert.ToInt32(Common.getCountryTimeFormatStatic(DateTime.Now.ToUniversalTime().ToString(), HttpContext.Current.Session["TimeZoneId"].ToString()).Second);


        if (axCZKEM1.SetDeviceTime2(iMachineNumber, idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond))
        {
            axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
                                                 //MessageBox.Show("Successfully set the time of the machine as you have set!", "Error");
            b = true;
        }
        else
        {
            axCZKEM1.GetLastError(ref idwErrorCode);
            //MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
        }
        Device_Connection(IPAddress, port, 1);
        return b;
    }

    public bool PowerOffDevice(string IPAddress, int port)
    {
        if (!Device_Connection(IPAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return false;
        }
        int iMachineNumber = 1;

        bool b = false;

        int idwErrorCode = 0;

        if (axCZKEM1.PowerOffDevice(iMachineNumber))
        {
            b = true;
        }
        else
        {
            axCZKEM1.GetLastError(ref idwErrorCode);
        }



        return b;
    }

    public bool UploadCardUser(DataTable dtUser, string IPAddress, int port)
    {
        string txtUserID = "";
        int cbPrivilege;
        string txtCardnumber;
        bool chbEnabled;
        string txtName;
        string txtPassword;
        //if (!Device_Connection(IPAddress, port, 0))
        //{
        //    DataTable dt = new DataTable();
        //    return false;
        //}
        int iMachineNumber = 1;
        bool b = false;
        for (int loop = 0; loop < dtUser.Rows.Count; loop++)
        {



            txtUserID = dtUser.Rows[loop]["UserID"].ToString();
            cbPrivilege = Convert.ToInt16(dtUser.Rows[loop]["Privilege"].ToString());
            txtCardnumber = dtUser.Rows[loop]["Cardnumber"].ToString();
            chbEnabled = Convert.ToBoolean(dtUser.Rows[loop]["Enabled"].ToString());
            txtName = dtUser.Rows[loop]["Name"].ToString();
            txtPassword = dtUser.Rows[loop]["Password"].ToString();

            if (txtUserID.ToString().Trim() == "" || cbPrivilege.ToString().Trim() == "" || txtCardnumber.ToString().Trim() == "")
            {
                return false;
            }
            int idwErrorCode = 0;

            bool bEnabled = true;
            if (chbEnabled)
            {
                bEnabled = true;
            }
            else
            {
                bEnabled = false;
            }
            string sName = txtName.ToString().Trim();
            string sPassword = txtPassword.ToString().Trim();
            int iPrivilege = Convert.ToInt32(cbPrivilege.ToString().Trim());
            string sCardnumber = txtCardnumber.ToString().Trim();


            axCZKEM1.EnableDevice(iMachineNumber, false);
            string sdwEnrollNumber = txtUserID.ToString().Trim();
            axCZKEM1.SetStrCardNumber(sCardnumber);//Before you using function SetUserInfo,set the card number to make sure you can upload it to the device
            if (axCZKEM1.SSR_SetUserInfo(iMachineNumber, sdwEnrollNumber, sName, sPassword, iPrivilege, bEnabled))//upload the user's information(card number included)
            {

                //MessageBox.Show("(SSR_)SetUserInfo,UserID:" + sdwEnrollNumber + " Privilege:" + iPrivilege.ToString() + " Enabled:" + bEnabled.ToString(), "Success");
                b = true;
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);
                //MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");

            }
            axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
        }
        axCZKEM1.EnableDevice(iMachineNumber, true);
        Device_Connection(IPAddress, port, 1);
        return b;
    }


    public bool UploadCardUserFpFace(DataTable dtUser, string IPAddress, int port, DataTable dtFinger, DataTable dtFace)
    {
        string txtUserID = "";
        int cbPrivilege;
        string txtCardnumber;
        bool chbEnabled;
        string txtName;
        string txtPassword;
        int iFaceIndex = 0;
        string sTmpData = "";
        int iLength = 0;

        int idwFingerIndex = 0;
        // string sTmpData = "";
        int iFlag = 1;
        //if (!Device_Connection(IPAddress, port, 0))
        //{
        //    DataTable dt = new DataTable();
        //    return false;
        //}
        int iMachineNumber = 1;
        bool b = false;
        DataTable dtEmpFinger = new DataTable();
        DataTable dtEmpFace = new DataTable();
        axCZKEM1.EnableDevice(iMachineNumber, false);
        for (int loop = 0; loop < dtUser.Rows.Count; loop++)
        {
            try
            {


                txtUserID = dtUser.Rows[loop]["UserID"].ToString();
                cbPrivilege = Convert.ToInt16(dtUser.Rows[loop]["Privilege"].ToString());
                txtCardnumber = dtUser.Rows[loop]["Cardnumber"].ToString();
                chbEnabled = Convert.ToBoolean(dtUser.Rows[loop]["Enabled"].ToString());
                txtName = dtUser.Rows[loop]["Name"].ToString();
                txtPassword = dtUser.Rows[loop]["Password"].ToString();

                if (txtUserID.ToString().Trim() == "" || cbPrivilege.ToString().Trim() == "" || txtCardnumber.ToString().Trim() == "")
                {
                    continue;
                }
                int idwErrorCode = 0;

                bool bEnabled = true;
                if (chbEnabled)
                {
                    bEnabled = true;
                }
                else
                {
                    bEnabled = false;
                }
                string sName = txtName.ToString().Trim();
                string sPassword = txtPassword.ToString().Trim();
                int iPrivilege = Convert.ToInt32(cbPrivilege.ToString().Trim());
                string sCardnumber = txtCardnumber.ToString().Trim();



                string sdwEnrollNumber = txtUserID.ToString().Trim();
                axCZKEM1.SetStrCardNumber(sCardnumber);//Before you using function SetUserInfo,set the card number to make sure you can upload it to the device
                axCZKEM1.SSR_SetUserInfo(iMachineNumber, sdwEnrollNumber, sName, sPassword, iPrivilege, bEnabled);
                //if (axCZKEM1.SSR_SetUserInfo(iMachineNumber, sdwEnrollNumber, sName, sPassword, iPrivilege, bEnabled))//upload the user's information(card number included)
                //{

                //MessageBox.Show("(SSR_)SetUserInfo,UserID:" + sdwEnrollNumber + " Privilege:" + iPrivilege.ToString() + " Enabled:" + bEnabled.ToString(), "Success");
                b = true;
                if (dtFinger.Rows.Count > 0)
                {
                    dtEmpFinger = new DataView(dtFinger, "sdwEnrollNumber='" + sdwEnrollNumber + "'", "", DataViewRowState.CurrentRows).ToTable();
                    for (int i = 0; i < dtEmpFinger.Rows.Count; i++)
                    {
                        Int32.TryParse(dtEmpFinger.Rows[i][2].ToString(), out idwFingerIndex);
                        //idwFingerIndex = Convert.ToInt32(dtEmpFinger.Rows[i][2].ToString());
                        sTmpData = dtEmpFinger.Rows[i][3].ToString();
                        Int32.TryParse(dtEmpFinger.Rows[i][7].ToString(), out iFlag);
                        //iFlag = Convert.ToInt32(dtEmpFinger.Rows[i][7].ToString());
                        axCZKEM1.SetUserTmpExStr(iMachineNumber, sdwEnrollNumber, idwFingerIndex, iFlag, sTmpData);
                    }
                }
                //UploadUser(dtFinger, IPAddress, port);

                if (dtFace.Rows.Count > 0)
                {
                    dtEmpFace = new DataView(dtFace, "sUserID='" + sdwEnrollNumber + "'", "", DataViewRowState.CurrentRows).ToTable();
                    for (int i = 0; i < dtEmpFace.Rows.Count; i++)
                    {
                        Int32.TryParse(dtEmpFace.Rows[i][4].ToString(), out iFaceIndex);
                        //iFaceIndex = Convert.ToInt32(dtEmpFace.Rows[i][4].ToString());
                        sTmpData = dtEmpFace.Rows[i][5].ToString();
                        Int32.TryParse(dtEmpFace.Rows[i][6].ToString(), out iLength);
                        //iLength = Convert.ToInt32(dtEmpFace.Rows[i][6].ToString());
                        axCZKEM1.SetUserFaceStr(iMachineNumber, sdwEnrollNumber, iFaceIndex, sTmpData, iLength);
                    }
                }
                //}
                //else
                //{
                //    axCZKEM1.GetLastError(ref idwErrorCode);
                //    //MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");

                //}
            }
            catch
            {
                continue;
            }

        }
        axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
        axCZKEM1.EnableDevice(iMachineNumber, true);
        Device_Connection(IPAddress, port, 1);
        return true;


    }
    public bool UploadCardUserFpFace(DataTable dtUser, string IPAddress, int port, DataTable dtFinger, DataTable dtFace, bool isBatchUpdate)
    {
        string txtUserID = "";
        int cbPrivilege;
        string txtCardnumber;
        bool chbEnabled;
        string txtName;
        string txtPassword;
        int iFaceIndex = 0;
        string sTmpData = "";
        int iLength = 0;
        int iUpdateFlag = 1;
        int idwFingerIndex = 0;
        // string sTmpData = "";
        int iFlag = 1;
        //if (!Device_Connection(IPAddress, port, 0))
        //{
        //    DataTable dt = new DataTable();
        //    return false;
        //}
        int iMachineNumber = 1;
        bool b = false;
        DataTable dtEmpFinger = new DataTable();
        DataTable dtEmpFace = new DataTable();
        axCZKEM1.EnableDevice(iMachineNumber, false);
        if (axCZKEM1.BeginBatchUpdate(iMachineNumber, iUpdateFlag))
        {
            for (int loop = 0; loop < dtUser.Rows.Count; loop++)
            {
                try
                {


                    txtUserID = dtUser.Rows[loop]["UserID"].ToString();
                    try
                    {
                        cbPrivilege = Convert.ToInt16(dtUser.Rows[loop]["Privilege"].ToString());
                    }
                    catch
                    {
                        cbPrivilege = 0;
                    }
                    txtCardnumber = dtUser.Rows[loop]["Cardnumber"].ToString();

                    if (txtCardnumber == "")
                    {
                        txtCardnumber = "0";
                    }

                    try
                    {
                        chbEnabled = Convert.ToBoolean(dtUser.Rows[loop]["Enabled"].ToString());
                    }
                    catch
                    {
                        chbEnabled = true;
                    }
                    txtName = dtUser.Rows[loop]["Name"].ToString();
                    txtPassword = dtUser.Rows[loop]["Password"].ToString();

                    if (txtUserID.ToString().Trim() == "" || cbPrivilege.ToString().Trim() == "" || txtCardnumber.ToString().Trim() == "")
                    {
                        continue;
                    }
                    int idwErrorCode = 0;

                    bool bEnabled = true;
                    if (chbEnabled)
                    {
                        bEnabled = true;
                    }
                    else
                    {
                        bEnabled = false;
                    }
                    string sName = txtName.ToString().Trim().Length > 22 ? txtName.ToString().Trim().Substring(0, 22) : txtName.ToString().Trim();
                    string sPassword = txtPassword.ToString().Trim();
                    int iPrivilege = Convert.ToInt32(cbPrivilege.ToString().Trim());
                    string sCardnumber = txtCardnumber.ToString().Trim();



                    string sdwEnrollNumber = txtUserID.ToString().Trim();
                    axCZKEM1.SetStrCardNumber(sCardnumber);//Before you using function SetUserInfo,set the card number to make sure you can upload it to the device
                    axCZKEM1.SSR_SetUserInfo(iMachineNumber, sdwEnrollNumber, sName, sPassword, iPrivilege, bEnabled);
                    //if (axCZKEM1.SSR_SetUserInfo(iMachineNumber, sdwEnrollNumber, sName, sPassword, iPrivilege, bEnabled))//upload the user's information(card number included)
                    //{

                    //MessageBox.Show("(SSR_)SetUserInfo,UserID:" + sdwEnrollNumber + " Privilege:" + iPrivilege.ToString() + " Enabled:" + bEnabled.ToString(), "Success");
                    b = true;
                    if (dtFinger.Rows.Count > 0)
                    {
                        dtEmpFinger = new DataView(dtFinger, "sdwEnrollNumber='" + sdwEnrollNumber + "'", "", DataViewRowState.CurrentRows).ToTable();
                        for (int i = 0; i < dtEmpFinger.Rows.Count; i++)
                        {

                            Int32.TryParse(dtEmpFinger.Rows[i][2].ToString(), out idwFingerIndex);
                            //idwFingerIndex = Convert.ToInt32(dtEmpFinger.Rows[i][2].ToString());
                            sTmpData = dtEmpFinger.Rows[i][3].ToString();
                            Int32.TryParse(dtEmpFinger.Rows[i][7].ToString(), out iFlag);
                            //iFlag = Convert.ToInt32(dtEmpFinger.Rows[i][7].ToString());
                            axCZKEM1.SetUserTmpExStr(iMachineNumber, sdwEnrollNumber, idwFingerIndex, iFlag, sTmpData);
                        }
                    }
                    //UploadUser(dtFinger, IPAddress, port);

                    //if (dtFace.Rows.Count > 0)
                    //{
                    //    dtEmpFace = new DataView(dtFace, "sUserID='" + sdwEnrollNumber + "'", "", DataViewRowState.CurrentRows).ToTable();
                    //    for (int i = 0; i < dtEmpFace.Rows.Count; i++)
                    //    {
                    //        Int32.TryParse(dtEmpFace.Rows[i][4].ToString(), out iFaceIndex);
                    //        //iFaceIndex = Convert.ToInt32(dtEmpFace.Rows[i][4].ToString());
                    //        sTmpData = dtEmpFace.Rows[i][5].ToString();
                    //        Int32.TryParse(dtEmpFace.Rows[i][6].ToString(), out iLength);
                    //        //iLength = Convert.ToInt32(dtEmpFace.Rows[i][6].ToString());
                    //        try
                    //        {
                    //            axCZKEM1.SetUserFaceStr(iMachineNumber, sdwEnrollNumber, iFaceIndex, sTmpData, iLength);

                    //        }
                    //        catch (Exception exxx)
                    //        {

                    //        }

                    //    }
                    //}

                    //else
                    //{
                    //    axCZKEM1.GetLastError(ref idwErrorCode);
                    //    //MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");

                    //}
                }
                catch
                {
                    continue;
                }

            }
        }





        axCZKEM1.BatchUpdate(iMachineNumber);
        axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
        axCZKEM1.EnableDevice(iMachineNumber, true);
        axCZKEM1.EnableDevice(iMachineNumber, false);
        if (axCZKEM1.BeginBatchUpdate(iMachineNumber, iUpdateFlag))
        {
            for (int loop = 0; loop < dtUser.Rows.Count; loop++)
            {
                if (dtFace != null)
                {
                    if (dtFace.Rows.Count > 0)
                    {
                        string sdwEnrollNumber = dtUser.Rows[loop]["UserID"].ToString();
                        dtEmpFace = new DataView(dtFace, "sUserID='" + sdwEnrollNumber + "'", "", DataViewRowState.CurrentRows).ToTable();
                        for (int i = 0; i < dtEmpFace.Rows.Count; i++)
                        {
                            Int32.TryParse(dtEmpFace.Rows[i][4].ToString(), out iFaceIndex);
                            //iFaceIndex = Convert.ToInt32(dtEmpFace.Rows[i][4].ToString());
                            sTmpData = dtEmpFace.Rows[i][5].ToString();
                            Int32.TryParse(dtEmpFace.Rows[i][6].ToString(), out iLength);
                            //iLength = Convert.ToInt32(dtEmpFace.Rows[i][6].ToString());

                            //axCZKEM1.SetUserFace(iMachineNumber, sdwEnrollNumber, iFaceIndex,Convert.ToByte(dtEmpFace.Rows[i][5]), iLength);
                            axCZKEM1.SetUserFaceStr(iMachineNumber, sdwEnrollNumber, iFaceIndex, sTmpData, iLength);
                        }
                    }
                }
            }

        }

        axCZKEM1.BatchUpdate(iMachineNumber);
        axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed
        axCZKEM1.EnableDevice(iMachineNumber, true);
        Device_Connection(IPAddress, port, 1);
        return true;


    }
    public DataTable GetUserFace(string IPAddress, int port)
    {
        DataTable dtUserFace = new DataTable();
        if (!Device_Connection(IPAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return dt;
        }
        int iMachineNumber = 1;
        bool b = false;

        string sUserID = "";
        string sName = "";
        string sPassword = "";
        int iPrivilege = 0;
        bool bEnabled = false;

        int iFaceIndex = 50;//the only possible parameter value
        string sTmpData = "";
        int iLength = 0;



        axCZKEM1.EnableDevice(iMachineNumber, false);
        axCZKEM1.ReadAllUserID(iMachineNumber);//read all the user information to the memory
        dtUserFace = UserFace();
        while (axCZKEM1.SSR_GetAllUserInfo(iMachineNumber, out sUserID, out sName, out sPassword, out iPrivilege, out bEnabled))//get all the users' information from the memory
        {
            if (axCZKEM1.GetUserFaceStr(iMachineNumber, sUserID, iFaceIndex, ref sTmpData, ref iLength))//get the face templates from the memory
            {
                DataRow row = dtUserFace.NewRow();

                row[0] = sUserID;
                row[1] = (sName);
                row[2] = (sPassword);
                row[3] = (iPrivilege.ToString());
                row[4] = (iFaceIndex.ToString());
                row[5] = (sTmpData);
                row[6] = (iLength.ToString());
                if (bEnabled == true)
                {
                    row[7] = ("true");
                }
                else
                {
                    row[7] = ("false");
                }

                dtUserFace.Rows.Add(row);
            }
        }
        axCZKEM1.EnableDevice(iMachineNumber, true);

        Device_Connection(IPAddress, port, 1);
        return dtUserFace;
    }

    private DataTable UserFace()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("sUserID");
        dt.Columns.Add("sName");
        dt.Columns.Add("sPassword");
        dt.Columns.Add("iPrivilege");
        dt.Columns.Add("iFaceIndex");
        dt.Columns.Add("sTmpData");
        dt.Columns.Add("iLength");
        dt.Columns.Add("bEnabled");

        return dt;
    }

    public bool UploadUserFace(string IPAddress, int port, DataTable dtUserFace)
    {
        bool b = false;
        //if (!Device_Connection(IPAddress, port, 0))
        //{
        //    DataTable dt = new DataTable();
        //    return b;
        //}
        int iMachineNumber = 1;

        int idwErrorCode = 0;

        string sUserID = "";
        string sName = "";
        int iFaceIndex = 0;
        string sTmpData = "";
        int iLength = 0;
        int iPrivilege = 0;
        string sPassword = "";
        string sEnabled = "";
        bool bEnabled = false;


        axCZKEM1.EnableDevice(iMachineNumber, false);
        for (int i = 0; i < dtUserFace.Rows.Count; i++)
        {
            sUserID = dtUserFace.Rows[i][0].ToString();
            sName = dtUserFace.Rows[i][1].ToString();
            sPassword = dtUserFace.Rows[i][2].ToString();
            ;
            iPrivilege = Convert.ToInt32(dtUserFace.Rows[i][3].ToString());
            iFaceIndex = Convert.ToInt32(dtUserFace.Rows[i][4].ToString());
            sTmpData = dtUserFace.Rows[i][5].ToString();
            iLength = Convert.ToInt32(dtUserFace.Rows[i][6].ToString());
            sEnabled = dtUserFace.Rows[i][7].ToString();
            if (sEnabled == "true")
            {
                bEnabled = true;
            }
            else
            {
                bEnabled = false;
            }

            if (axCZKEM1.SSR_SetUserInfo(iMachineNumber, sUserID, sName, sPassword, iPrivilege, bEnabled))//face templates are part of users' information
            {
                axCZKEM1.SetUserFaceStr(iMachineNumber, sUserID, iFaceIndex, sTmpData, iLength);//upload face templates information to the device
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);

                axCZKEM1.EnableDevice(iMachineNumber, true);
                Device_Connection(IPAddress, port, 1);
                return false;
            }
        }

        axCZKEM1.RefreshData(iMachineNumber);//the data in the device should be refreshed

        axCZKEM1.EnableDevice(iMachineNumber, true);
        Device_Connection(IPAddress, port, 1);
        return true;
        //MessageBox.Show("Successfully Upload the face templates, " + "total:" + lvFace.Items.Count.ToString(), "Success");
    }


    public DataTable GetSingleUserFace(string IPAddress, int port, string UserId)
    {
        DataTable dtUser = new DataTable();
        dtUser.Columns.Add("TempData");
        dtUser.Columns.Add("TempLength");

        //if (!Device_Connection(IPAddress, port, 0))
        //{
        //    DataTable dt = new DataTable();
        //    return dt;
        //}
        int iMachineNumber = 1;
        bool b = false;

        if (UserId.ToString().Trim() == "")
        {
            //MessageBox.Show("Please input the UserID first!", "Error");
            return dtUser;
        }
        int idwErrorCode = 0;

        string sUserID = UserId.ToString().Trim();
        int iFaceIndex = 50;//the only possible parameter value
        int iLength = 128 * 1024;//initialize the length(cannot be zero)
                                 //byte[] byTmpData = new byte[iLength];

        string byTmpData = "";
        // axCZKEM1.EnableDevice(iMachineNumber, false);
        //ef byTmpData[0], ref iLength
        if (axCZKEM1.GetUserFaceStr(iMachineNumber, sUserID.ToString(), iFaceIndex, ref byTmpData, ref iLength))
        {
            //Here you can manage the information of the face templates according to your request.(for example,you can sava them to the database)
            //MessageBox.Show("GetUserFace,the  length of the bytes array byTmpData is " + iLength.ToString(), "Success");
            DataRow row = dtUser.NewRow();
            //                string s = System.Text.ASCIIEncoding.ASCII.GetString(byTmpData);
            row[0] = byTmpData;
            row[1] = iLength.ToString();

            dtUser.Rows.Add(row);
        }
        else
        {

            axCZKEM1.GetLastError(ref idwErrorCode);
            axCZKEM1.EnableDevice(iMachineNumber, true);
            //MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
            return dtUser;
        }

        axCZKEM1.EnableDevice(iMachineNumber, true);
        //Device_Connection(IPAddress, port, 1);
        return dtUser;
    }
    public DataTable GetSingleUser(string IPAddress, int port, int UserId)
    {
        DataTable dtUser = new DataTable();
        dtUser.Columns.Add("dwEnrollNumber");
        dtUser.Columns.Add("name");
        dtUser.Columns.Add("password");
        dtUser.Columns.Add("privileg");
        dtUser.Columns.Add("enable");

        bool b = false;

        if (!Device_Connection(IPAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return dt;
        }
        int iMachineNumber = 1;
        string name, password;
        name = "";
        password = "";
        int privileg = 0;
        bool enable = false;
        axCZKEM1.EnableDevice(iMachineNumber, false);

        b = axCZKEM1.SSR_GetUserInfo(iMachineNumber, UserId.ToString(), out name, out password, out privileg, out enable);
        if (b)
        {
            DataRow row = dtUser.NewRow();
            row[0] = UserId;
            row[1] = name;
            row[2] = password;
            row[3] = privileg;
            row[4] = enable;

            dtUser.Rows.Add(row);
        }


        axCZKEM1.EnableDevice(iMachineNumber, true);
        Device_Connection(IPAddress, port, 1);

        return dtUser;
    }

    public DataTable GetSingleUserFinger(string IPAddress, int port, int UserId)
    {
        DataTable dtUser = new DataTable();
        dtUser.Columns.Add("dwEnrollNumber");
        dtUser.Columns.Add("FingerIndex");

        dtUser.Columns.Add("Flag");
        dtUser.Columns.Add("tempData");
        dtUser.Columns.Add("templength");

        bool b = false;

        if (!Device_Connection(IPAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return dt;
        }
        int iMachineNumber = 1;
        int idwFingerIndex = 0;
        axCZKEM1.EnableDevice(iMachineNumber, false);
        string tempData;
        int templength;
        int Flag;
        for (idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
        {
            try
            {
                b = axCZKEM1.GetUserTmpExStr(iMachineNumber, UserId.ToString(), idwFingerIndex, out Flag, out tempData, out templength);
                if (b)
                {
                    DataRow row = dtUser.NewRow();
                    row[0] = UserId;
                    row[1] = idwFingerIndex;
                    row[2] = Flag;
                    row[3] = tempData;
                    row[4] = templength;


                    dtUser.Rows.Add(row);
                }
            }
            catch
            {
            }
        }

        axCZKEM1.EnableDevice(iMachineNumber, true);
        Device_Connection(IPAddress, port, 1);

        return dtUser;
    }



    public bool DelMultipleUser(string IPAddress, int port, DataTable dtUser, string strdwBackupNumber)
    {
        bool b = false;

        if (!Device_Connection(IPAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return b;
        }

        int iMachineNumber = 1;
        int dwBackupNumber = 0;
        int dwEnrollNumber = 0;

        foreach (string str in strdwBackupNumber.Split(','))
        {
            if (str == "")
            {
                continue;

            }

            for (int i = 0; i < dtUser.Rows.Count; i++)
            {

                dwEnrollNumber = Convert.ToInt32(dtUser.Rows[i]["sdwEnrollNumber"].ToString());

                if (axCZKEM1.SSR_DeleteEnrollData(iMachineNumber, dwEnrollNumber.ToString(), Convert.ToInt32(str)))
                {
                    b = true;
                }
            }
        }

        axCZKEM1.RefreshData(iMachineNumber);
        axCZKEM1.EnableDevice(iMachineNumber, true);
        Device_Connection(IPAddress, port, 1);
        return b;
    }



    public bool DelSingleUser(string IPAddress, int port, int UserId)
    {
        bool b = false;

        if (!Device_Connection(IPAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return b;
        }
        int iMachineNumber = 1;
        int dwBackupNumber = 0;
        int dwEmachineNumber = 1;
        int dwEnrollNumber = UserId;

        if (axCZKEM1.SSR_DeleteEnrollData(iMachineNumber, dwEnrollNumber.ToString(), dwBackupNumber))
        {
            b = true;
        }

        axCZKEM1.RefreshData(iMachineNumber);
        axCZKEM1.EnableDevice(iMachineNumber, true);
        Device_Connection(IPAddress, port, 1);
        return b;
    }




    public bool DelMultipleUserFinger(string IPAddress, int port, DataTable dtUser, DataTable dtFingertemp)
    {
        bool b = false;


        if (!Device_Connection(IPAddress, port, 0))
        {
            return b;
        }

        DataTable dttemp = new DataTable();

        //DataTable dtFingertemp = GetUserFinger(IPAddress, Convert.ToInt32(port));

        int iMachineNumber = 1;

        for (int j = 0; j < dtUser.Rows.Count; j++)
        {

            dttemp = new DataView(dtFingertemp, "sdwEnrollNumber=" + dtUser.Rows[j]["sdwEnrollNumber"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

            for (int i = 0; i < dttemp.Rows.Count; i++)
            {
                if (axCZKEM1.SSR_DelUserTmp(iMachineNumber, dtUser.Rows[j]["sdwEnrollNumber"].ToString(), Convert.ToInt32(dttemp.Rows[i]["idwFingerIndex"].ToString())))
                {
                    b = true;
                }
            }
        }
        //}

        axCZKEM1.RefreshData(iMachineNumber);
        axCZKEM1.EnableDevice(iMachineNumber, true);
        Device_Connection(IPAddress, port, 1);

        return b;
    }



    public bool DelSingleUserFinger(string IPAddress, int port, int UserId, int FingerIndex)
    {
        bool b = false;


        if (!Device_Connection(IPAddress, port, 0))
        {
            return b;
        }

        int iMachineNumber = 1;
        int dwEnrollNumber = UserId;

        //DataTable dtFingertemp = GetSingleUserFinger(IPAddress, Convert.ToInt32(port), UserId);

        //if (dtFingertemp.Rows.Count > 0)
        //{
        //    FingerIndex = Convert.ToInt32(dtFingertemp.Rows[0]["FingerIndex"].ToString());
        //axCZKEM1.DelUserTmp(iMachineNumber, dwEnrollNumber.ToString(), 0);


        //if (axCZKEM1.SSR_DelUserTmpExt(iMachineNumber, dwEnrollNumber.ToString(), 0))
        //{

        //    b = true;
        //}
        for (int i = 0; i < 10; i++)
        {
            if (axCZKEM1.SSR_DelUserTmp(iMachineNumber, dwEnrollNumber.ToString(), i))
            {
                b = true;
            }
        }
        //}

        axCZKEM1.RefreshData(iMachineNumber);
        axCZKEM1.EnableDevice(iMachineNumber, true);
        Device_Connection(IPAddress, port, 1);

        return b;
    }




    public bool DelMultipleUserFace(string IPAddress, int port, DataTable dtUser)
    {

        if (!Device_Connection(IPAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return false;
        }
        int iMachineNumber = 1;
        bool b = false;


        int idwErrorCode = 0;


        int iFaceIndex = 50;

        for (int i = 0; i < dtUser.Rows.Count; i++)
        {

            if (axCZKEM1.DelUserFace(iMachineNumber, dtUser.Rows[i]["sdwEnrollNumber"].ToString(), iFaceIndex))
            {
                axCZKEM1.RefreshData(iMachineNumber);
                b = true;
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);
            }
        }


        Device_Connection(IPAddress, port, 1);
        return b;
    }



    public bool DelSingleUserFace(string IPAddress, int port, string UserId)
    {




        if (!Device_Connection(IPAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return false;
        }
        int iMachineNumber = 1;
        bool b = false;

        if (UserId.ToString().Trim() == "")
        {
            //MessageBox.Show("Please input the UserID first!", "Error");
            return false;
        }
        int idwErrorCode = 0;

        string sUserID = UserId.ToString().Trim();
        int iFaceIndex = 50;



        if (axCZKEM1.DelUserFace(iMachineNumber, sUserID, iFaceIndex))
        {
            axCZKEM1.RefreshData(iMachineNumber);
            b = true;
        }
        else
        {
            axCZKEM1.GetLastError(ref idwErrorCode);
        }
        Device_Connection(IPAddress, port, 1);
        return b;
    }




    public bool SetLastCount(string IPAddress, int port, int Count)
    {
        if (!Device_Connection(IPAddress, port, 0))
        {
            return false;
        }

        if (axCZKEM1.SetLastCount(Count))
        {
            return true;
        }
        else
        {
            return false;
        }
    }




    public int sta_GetCapacityInfo(out int adminCnt, out int userCount, out int fpCnt, out int recordCnt, out int pwdCnt, out int oplogCnt, out int faceCnt)
    {
        int ret = 0;

        adminCnt = 0;
        userCount = 0;
        fpCnt = 0;
        recordCnt = 0;
        pwdCnt = 0;
        oplogCnt = 0;
        faceCnt = 0;



        axCZKEM1.EnableDevice(0, false);//disable the device
        axCZKEM1.GetDeviceStatus(0, 2, ref userCount);
        axCZKEM1.GetDeviceStatus(0, 1, ref adminCnt);
        axCZKEM1.GetDeviceStatus(0, 3, ref fpCnt);
        axCZKEM1.GetDeviceStatus(0, 4, ref pwdCnt);
        axCZKEM1.GetDeviceStatus(0, 5, ref oplogCnt);
        axCZKEM1.GetDeviceStatus(0, 6, ref recordCnt);
        axCZKEM1.GetDeviceStatus(0, 21, ref faceCnt);
        axCZKEM1.EnableDevice(0, true);//enable the device

        ret = 1;
        return ret;
    }

    public int sta_GetCapacityInfo(out int userCount, out int fpCnt, out int faceCnt)
    {
        int ret = 0;
        userCount = 0;
        fpCnt = 0;
        faceCnt = 0;


        axCZKEM1.EnableDevice(0, false);//disable the device
        axCZKEM1.GetDeviceStatus(0, 2, ref userCount);

        axCZKEM1.GetDeviceStatus(0, 3, ref fpCnt);

        axCZKEM1.GetDeviceStatus(0, 21, ref faceCnt);
        axCZKEM1.EnableDevice(0, true);//enable the device

        ret = 1;
        return ret;
    }
}

