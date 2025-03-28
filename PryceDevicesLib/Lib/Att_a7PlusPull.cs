using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using PryceDevicesLib;

    class Att_a7PlusPull : IAttDeviceOperation
    {
        public Att_a7PlusPull()
        {
            //constructor
        }

        public bool ClearAdminPrivilege(string ipAddress, string port)
        {
            throw new NotImplementedException();
        }

        public bool ClearLog(string ipAddress, string port)
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

        public bool Device_Connection(string ipAddress, string port, int index)
        {
            throw new NotImplementedException();
        }

        public clsDeviceCapacity GetCapacityInfo(string ipAddress, string port)
        {
            throw new NotImplementedException();
        }

        public List<clsUserFace> GetSingleUserFace(string ipAddress, string port, int userId)
        {
            throw new NotImplementedException();
        }

        public List<clsUser> GetUser(string ipAddress, string port)
        {
            throw new NotImplementedException();
        }

        public List<clsUserFace> GetUserFace(string ipAddress, string port)
        {
            throw new NotImplementedException();
        }

        public List<clsUserFinger> GetUserFinger(string ipAddress, string port)
        {
            throw new NotImplementedException();
        }

        public List<clsUserLog> GetUserLog(string ipAddress, string port)
        {
            throw new NotImplementedException();
        }

        public bool InitializeDevice(string ipAddress, string port)
        {
            throw new NotImplementedException();
        }

        public bool PowerOffDevice(string ipAddress, string port)
        {
            throw new NotImplementedException();
        }

        public bool RestartDevice(string ipAddress, string port)
        {
            throw new NotImplementedException();
        }

        public bool SetDeviceTime(string ipAddress, string port)
        {
            throw new NotImplementedException();
        }

        public bool UploadCardUserFpFace(string ipAddress, string port, DataTable dtUserList, DataTable dtFingerList, DataTable dtFaceList, bool isBatchUpdate)
        {
            throw new NotImplementedException();
        }
    }

