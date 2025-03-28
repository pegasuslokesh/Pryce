using System;
using System.Collections.Generic;
using System.Linq;
using PryceDevicesLib;
using System.Data;

public interface IAttDeviceOperation
{
    bool ClearAdminPrivilege(string ipAddress, string port);
    bool InitializeDevice(string ipAddress, string port);
    bool RestartDevice(string ipAddress, string port);
    bool PowerOffDevice(string ipAddress, string port);
    bool SetDeviceTime(string ipAddress, string port);
    bool ClearLog(string ipAddress, string port);
    bool Device_Connection(string ipAddress, string port, int index);
    List<clsUserLog> GetUserLog(string ipAddress, string port);
    clsDeviceCapacity GetCapacityInfo(string ipAddress, string port);
    List<clsUserFinger> GetUserFinger(string ipAddress, string port);
    List<clsUserFace> GetUserFace(string ipAddress, string port);
    List<clsUser> GetUser(string ipAddress, string port);
    List<clsUserFace> GetSingleUserFace(string ipAddress, string port, int userId);
    bool DelMultipleUser(string ipAddress, string port, DataTable dtUserList, string strdwBackupNumber);
    bool DelMultipleUserFinger(string ipAddress, string port, DataTable dtUserList, DataTable dtFingerList);
    bool DelMultipleUserFace(string ipAddress, string port, DataTable dtUserList);
    bool UploadCardUserFpFace(string ipAddress, string port, DataTable dtUserList, DataTable dtFingerList, DataTable dtFaceList, bool isBatchUpdate);
}
