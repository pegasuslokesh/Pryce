using System;
using System.Collections.Generic;
using System.Linq;
using PryceDevicesLib;
using System.Collections;
using zkemkeeper;
using System.Data;
using System.Data.SqlClient;
using System.Web;



/// <summary>
/// Summary description for Att_zkTecho
/// </summary>
public class Att_zkTecho:IAttDeviceOperation
{
    private SqlConnection db;
    public zkemkeeper.CZKEMClass axCZKEM1 = new zkemkeeper.CZKEMClass();
    public Att_zkTecho()
    {
        //
        // TODO: Add constructor logic here
        //
    }



  

    public bool ClearAdminPrivilege(string ipAddress, string port)
    {
        if (!Device_Connection(ipAddress, port, 0))
        {
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
        Device_Connection(ipAddress, port, 1);
        return b;
    }

    public bool ClearLog(string ipAddress, string port)
    {
        if (!Device_Connection(ipAddress, port, 0))
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
        Device_Connection(ipAddress, port, 1);
        return b;
    }

    public bool DelMultipleUser(string ipAddress, string port, DataTable dtUserList, string strdwBackupNumber)
    {
        bool b = false;

        if (!Device_Connection(ipAddress, port, 0))
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

            for (int i = 0; i < dtUserList.Rows.Count; i++)
            {

                dwEnrollNumber = Convert.ToInt32(dtUserList.Rows[i]["sdwEnrollNumber"].ToString());

                if (axCZKEM1.SSR_DeleteEnrollData(iMachineNumber, dwEnrollNumber.ToString(), Convert.ToInt32(str)))
                {
                    b = true;
                }
            }
        }

        axCZKEM1.RefreshData(iMachineNumber);
        axCZKEM1.EnableDevice(iMachineNumber, true);
        Device_Connection(ipAddress, port, 1);
        return b;
    }

    public bool DelMultipleUserFace(string ipAddress, string port, DataTable dtUserList)
    {
        if (!Device_Connection(ipAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return false;
        }
        int iMachineNumber = 1;
        bool b = false;


        int idwErrorCode = 0;


        int iFaceIndex = 50;

        for (int i = 0; i < dtUserList.Rows.Count; i++)
        {

            if (axCZKEM1.DelUserFace(iMachineNumber, dtUserList.Rows[i]["sdwEnrollNumber"].ToString(), iFaceIndex))
            {
                axCZKEM1.RefreshData(iMachineNumber);
                b = true;
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);
            }
        }


        Device_Connection(ipAddress, port, 1);
        return b;
    }

    public bool DelMultipleUserFinger(string ipAddress, string port, DataTable dtUserList, DataTable dtFingerList)
    {
        bool b = false;


        if (!Device_Connection(ipAddress, port, 0))
        {
            return b;
        }

        DataTable dttemp = new DataTable();

        //DataTable dtFingertemp = GetUserFinger(IPAddress, Convert.ToInt32(port));

        int iMachineNumber = 1;

        for (int j = 0; j < dtUserList.Rows.Count; j++)
        {

            dttemp = new DataView(dtFingerList, "enrollNumber=" + dtUserList.Rows[j]["sdwEnrollNumber"].ToString() + "", "", DataViewRowState.CurrentRows).ToTable();

            for (int i = 0; i < dttemp.Rows.Count; i++)
            {
                if (axCZKEM1.SSR_DelUserTmp(iMachineNumber, dtUserList.Rows[j]["sdwEnrollNumber"].ToString(), Convert.ToInt32(dttemp.Rows[i]["fingerIndex"].ToString())))
                {
                    b = true;
                }
            }
        }
        //}

        axCZKEM1.RefreshData(iMachineNumber);
        axCZKEM1.EnableDevice(iMachineNumber, true);
        Device_Connection(ipAddress, port, 1);

        return b;
    }

    public bool Device_Connection(string ipAddress, string port, int i)
    {
        int idwErrorCode = 0;
        if (i == 1)
        {
            axCZKEM1.Disconnect();
            return true;
        }
        else
        {
            bool bIsConnected = axCZKEM1.Connect_Net(ipAddress, Convert.ToInt32(port));
            if (bIsConnected == true)
            {
                return true;
            }
            else
            {
                axCZKEM1.GetLastError(ref idwErrorCode);
                return false;
            }
        }
    }

    public clsDeviceCapacity GetCapacityInfo(string ipAddress, string port)
    {
        clsDeviceCapacity cls = new clsDeviceCapacity();
        int adminCnt = 0;
        int userCount = 0;
        int fpCnt = 0;
        int recordCnt = 0;
        int pwdCnt = 0;
        int oplogCnt = 0;
        int faceCnt = 0;
        axCZKEM1.EnableDevice(0, false);//disable the device
        axCZKEM1.GetDeviceStatus(0, 2, ref userCount);
        axCZKEM1.GetDeviceStatus(0, 1, ref adminCnt);
        axCZKEM1.GetDeviceStatus(0, 3, ref fpCnt);
        axCZKEM1.GetDeviceStatus(0, 4, ref pwdCnt);
        axCZKEM1.GetDeviceStatus(0, 5, ref oplogCnt);
        axCZKEM1.GetDeviceStatus(0, 6, ref recordCnt);
        axCZKEM1.GetDeviceStatus(0, 21, ref faceCnt);
        axCZKEM1.EnableDevice(0, true);//enable the device
        cls.userCount = userCount;
        cls.adminCount = adminCnt;
        cls.fingerCount = fpCnt;
        cls.passwordCount = pwdCnt;
        cls.logCount = recordCnt;
        cls.opLogCount = oplogCnt;
        cls.faceCount = faceCnt;
        return cls;
    }

    public List<clsUserFace> GetSingleUserFace(string ipAddress, string port, int userId)
    {
        List<clsUserFace> lstCls = new List<clsUserFace> { };
        
        int iMachineNumber = 1;
        bool b = false;

        if (userId.ToString().Trim() == "")
        {
            return lstCls;
        }
        int idwErrorCode = 0;

        string sUserID = userId.ToString().Trim();
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
            clsUserFace cls = new clsUserFace();
            //                string s = System.Text.ASCIIEncoding.ASCII.GetString(byTmpData);
            cls.tmpData = byTmpData;
            cls.length = iLength.ToString();
            cls.faceIndex = iFaceIndex.ToString();
            cls.enabled = "true";
            cls.privilege = "0";
            lstCls.Add(cls);
        }
        else
        {

            axCZKEM1.GetLastError(ref idwErrorCode);
            axCZKEM1.EnableDevice(iMachineNumber, true);
            //MessageBox.Show("Operation failed,ErrorCode=" + idwErrorCode.ToString(), "Error");
            return lstCls;
        }

        axCZKEM1.EnableDevice(iMachineNumber, true);
        //Device_Connection(IPAddress, port, 1);
        return lstCls;
    }

    public List<clsUser> GetUser(string ipAddress, string port)
    {
        List<clsUser> lstCls = new List<clsUser> { };
        if (!Device_Connection(ipAddress, port, 0))
        {
            return lstCls;
        }

        string sdwEnrollNumber = "";
        string sName = "";
        string sPassword = "";
        int iPrivilege = 0;
        bool bEnabled = false;
        string sCardnumber = "";
        int iMachineNumber = 1;
        axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device
        axCZKEM1.ReadAllUserID(iMachineNumber);//read all the user information to the memory
        while (axCZKEM1.SSR_GetAllUserInfo(iMachineNumber, out sdwEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))//get user information from memory
        {
            if (axCZKEM1.GetStrCardNumber(out sCardnumber))//get the card number from the memory
            {
                clsUser cls = new clsUser();
                cls.enrollNumber = sdwEnrollNumber;
                cls.name = (sName);
                cls.cardNumber = (sCardnumber);
                cls.privilege = (iPrivilege.ToString());
                cls.password = (sPassword);
                if (bEnabled == true)
                {
                    cls.enabled = ("true");
                }
                else
                {
                    cls.enabled = ("false");
                }
                lstCls.Add(cls);
            }
        }
        axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device
        Device_Connection(ipAddress, port, 1);
        return lstCls;
    }

    public List<clsUserFace> GetUserFace(string ipAddress, string port)
    {
        List<clsUserFace> lstCls = new List<clsUserFace> { };
        if (!Device_Connection(ipAddress, port, 0))
        {
            return lstCls;
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
        while (axCZKEM1.SSR_GetAllUserInfo(iMachineNumber, out sUserID, out sName, out sPassword, out iPrivilege, out bEnabled))//get all the users' information from the memory
        {
            if (axCZKEM1.GetUserFaceStr(iMachineNumber, sUserID, iFaceIndex, ref sTmpData, ref iLength))//get the face templates from the memory
            {
                clsUserFace cls = new clsUserFace();
                cls.enrollNumber = sUserID;
                cls.name = (sName);
                cls.password = (sPassword);
                cls.privilege = (iPrivilege.ToString());
                cls.faceIndex = (iFaceIndex.ToString());
                cls.tmpData = (sTmpData);
                cls.length = (iLength.ToString());
                if (bEnabled == true)
                {
                    cls.enabled = ("true");
                }
                else
                {
                    cls.enabled = ("false");
                }

                lstCls.Add(cls);
            }
        }
        axCZKEM1.EnableDevice(iMachineNumber, true);
        Device_Connection(ipAddress, port, 1);
        return lstCls;
    }

    public List<clsUserFinger> GetUserFinger(string ipAddress, string port)
    {
        List<clsUserFinger> lstCls = new List<clsUserFinger> { };
        if (!Device_Connection(ipAddress, port, 0))
        {
            return lstCls;    
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
        axCZKEM1.ReadAllUserID(iMachineNumber);//read all the user information to the memory
        axCZKEM1.ReadAllTemplate(iMachineNumber);//read all the users' fingerprint templates to the memory
        while (axCZKEM1.SSR_GetAllUserInfo(iMachineNumber, out sdwEnrollNumber, out sName, out sPassword, out iPrivilege, out bEnabled))//get all the users' information from the memory
        {
            for (idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
            {
                if (axCZKEM1.GetUserTmpExStr(iMachineNumber, sdwEnrollNumber, idwFingerIndex, out iFlag, out sTmpData, out iTmpLength))//get the corresponding templates string and length from the memory
                {
                    clsUserFinger cls = new clsUserFinger();
                    cls.enrollNumber = sdwEnrollNumber;
                    cls.name = (sName);
                    cls.fingerIndex = idwFingerIndex;
                    cls.tmpData= (sTmpData);
                    cls.privilege = (iPrivilege.ToString());
                    cls.password = (sPassword);
                    if (bEnabled == true)
                    {
                        cls.enabled = ("true");
                    }
                    else
                    {
                        cls.enabled = ("false");
                    }
                    cls.flag = (iFlag.ToString());
                    lstCls.Add(cls);
                }
            }
        }
        axCZKEM1.EnableDevice(iMachineNumber, true);
        Device_Connection(ipAddress, port, 1);
        return lstCls;
    }

    public List<clsUserLog> GetUserLog(string ipAddress, string port)
    {
        List<clsUserLog> lstCls = new List<clsUserLog> { };
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
        axCZKEM1.EnableDevice(iMachineNumber, false);//disable the device
        if (axCZKEM1.ReadGeneralLogData(iMachineNumber))//read all the attendance records to the memory
        {
            while (axCZKEM1.SSR_GetGeneralLogData(iMachineNumber, out sdwEnrollNumber, out idwVerifyMode,
            out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode))//get records from the memory
            {
                iGLCount++;
                clsUserLog cls = new clsUserLog();
                //lvLogs.Items.Add(iGLCount.ToString());
                cls.enrollNumber = (sdwEnrollNumber);//modify by Darcy on Nov.26 2009
                cls.verifyMode = (idwVerifyMode.ToString());
                cls.inOutMode = (idwInOutMode.ToString());
                cls.logTime = (idwYear.ToString() + "-" + idwMonth.ToString() + "-" + idwDay.ToString() + " " + idwHour.ToString() + ":" + idwMinute.ToString() + ":" + idwSecond.ToString());
                cls.workCode = (idwWorkcode.ToString());
                cls.lDateTime = Convert.ToDateTime(cls.logTime);
                lstCls.Add(cls);
                iIndex++;
            }
        }
        else
        {
            axCZKEM1.GetLastError(ref idwErrorCode);
        }
        axCZKEM1.EnableDevice(iMachineNumber, true);//enable the device
                                                    //Cursor = Cursors.Default;
        Device_Connection(ipAddress, port, 1);
        return lstCls;
    }

    public bool InitializeDevice(string ipAddress, string port)
    {
        if (!Device_Connection(ipAddress, port, 0))
        {
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

    public bool PowerOffDevice(string ipAddress, string port)
    {
        if (!Device_Connection(ipAddress, port, 0))
        {
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

    public bool RestartDevice(string ipAddress, string port)
    {
        if (!Device_Connection(ipAddress, port, 0))
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

    public bool SetDeviceTime(string ipAddress, string port)
    {
        if (!Device_Connection(ipAddress, port, 0))
        {
            DataTable dt = new DataTable();
            return false;
        }
        bool b = false;
        int idwErrorCode = 0;
        int iMachineNumber = 1;
        int idwYear = Convert.ToInt32(DateTime.Now.Year);
        int idwMonth = Convert.ToInt32(DateTime.Now.Month);
        int idwDay = Convert.ToInt32(DateTime.Now.Day);
        int idwHour = Convert.ToInt32(DateTime.Now.Hour);
        int idwMinute = Convert.ToInt32(DateTime.Now.Minute);
        int idwSecond = Convert.ToInt32(DateTime.Now.Second);

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
        Device_Connection(ipAddress, port, 1);
        return b;
    }

    public bool UploadCardUserFpFace(string ipAddress, string port, DataTable dtUserList, DataTable dtFingerList, DataTable dtFaceList, bool isBatchUpdate)
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
     
        int iMachineNumber = 1;
        bool b = false;
        DataTable dtEmpFinger = new DataTable();
        DataTable dtEmpFace = new DataTable();
        axCZKEM1.EnableDevice(iMachineNumber, false);
        if (axCZKEM1.BeginBatchUpdate(iMachineNumber, iUpdateFlag))
        {
            for (int loop = 0; loop < dtUserList.Rows.Count; loop++)
            {
                try
                {


                    txtUserID = dtUserList.Rows[loop]["UserID"].ToString();
                    try
                    {
                        cbPrivilege = Convert.ToInt16(dtUserList.Rows[loop]["Privilege"].ToString());
                    }
                    catch
                    {
                        cbPrivilege = 0;
                    }
                    txtCardnumber = dtUserList.Rows[loop]["Cardnumber"].ToString();

                    if (txtCardnumber == "")
                    {
                        txtCardnumber = "0";
                    }

                    try
                    {
                        chbEnabled = Convert.ToBoolean(dtUserList.Rows[loop]["Enabled"].ToString());
                    }
                    catch
                    {
                        chbEnabled = true;
                    }
                    txtName = dtUserList.Rows[loop]["Name"].ToString();
                    txtPassword = dtUserList.Rows[loop]["Password"].ToString();

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
                    if (dtFingerList.Rows.Count > 0)
                    {
                        dtEmpFinger = new DataView(dtFingerList, "sdwEnrollNumber='" + sdwEnrollNumber + "'", "", DataViewRowState.CurrentRows).ToTable();
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
            for (int loop = 0; loop < dtUserList.Rows.Count; loop++)
            {
                if (dtFaceList != null)
                {
                    if (dtFaceList.Rows.Count > 0)
                    {
                        string sdwEnrollNumber = dtUserList.Rows[loop]["UserID"].ToString();
                        dtEmpFace = new DataView(dtFaceList, "sUserID='" + sdwEnrollNumber + "'", "", DataViewRowState.CurrentRows).ToTable();
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
        Device_Connection(ipAddress, port, 1);
        return true;
    }
}