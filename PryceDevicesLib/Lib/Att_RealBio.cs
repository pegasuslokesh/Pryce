using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using PryceDevicesLib;
using zkemkeeper;

/// <summary>
/// Summary description for Att_RealBio
/// </summary>
public class Att_RealBio:IAttDeviceOperation
{

    public Att_RealBio()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public bool ClearAdminPrivilege(string ipAddress, string portNo)
    {
        throw new NotImplementedException();
    }

    public bool ClearLog(string ipAddress, string portNo)
    {
        throw new NotImplementedException();
    }

    public void connectDevice(string ipAddress)
    {
        throw new NotImplementedException();
    }

    public bool DelMultipleUser(string ipAddress, string port, DataTable dtUserList, string strdwBackupNumber)
    {
        throw new NotImplementedException();
    }

    public bool DelMultipleUserFace(string ipAddress, string port, DataTable dtUserList)
    {
        throw new NotImplementedException();
    }

    public bool DelMultipleUserFinger(string ipAddress, string port, DataTable dtUserList, DataTable dtFingerList)
    {
        throw new NotImplementedException();
    }

    public bool Device_Connection(string ipAddress, string portNo, int index)
    {
        throw new NotImplementedException();
    }

    public void disconnectDevice(string ipAddress)
    {
        throw new NotImplementedException();
    }

    public List<clsDeviceCapacity> GetCapacityInfo(string ipAddress, string portNo)
    {
        throw new NotImplementedException();
    }

    public List<clsUserLog> getDeviceLog(string ipAddress, DateTime fromDate, DateTime toDate)
    {
        return null;
    }

    public List<clsUserFace> GetSingleUserFace(string ipAddress, string portNo)
    {
        throw new NotImplementedException();
    }

    public List<clsUserFace> GetSingleUserFace(string ipAddress, string port, int userId)
    {
        throw new NotImplementedException();
    }

    public List<clsUser> GetUser(string ipAddress, string portNo)
    {
        throw new NotImplementedException();
    }

    public List<clsUserFace> GetUserFace(string ipAddress, string portNo)
    {
        throw new NotImplementedException();
    }

    public List<clsUserFinger> GetUserFinger(string ipAddress, string portNo)
    {
        throw new NotImplementedException();
    }

    public List<clsUserLog> GetUserLog(string ipAddress, string portNo)
    {
        throw new NotImplementedException();
    }

    public bool InitializeDevice(string ipAddress, string portNo)
    {
        throw new NotImplementedException();
    }

    public bool PowerOffDevice(string ipAddress, string portNo)
    {
        throw new NotImplementedException();
    }

    public bool RestartDevice(string ipAddress, string portNo)
    {
        throw new NotImplementedException();
    }

    public bool SetDeviceTime(string ipAddress, string portNo)
    {
        throw new NotImplementedException();
    }

    public bool UploadCardUserFpFace(string ipAddress, string port, DataTable dtUserList, DataTable dtFingerList, DataTable dtFaceList, bool isBatchUpdate)
    {
        throw new NotImplementedException();
    }

    clsDeviceCapacity IAttDeviceOperation.GetCapacityInfo(string ipAddress, string port)
    {
        throw new NotImplementedException();
    }
  
}