using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using PryceDevicesLib;
using System.Data.SqlClient;
using Newtonsoft.Json;
using FKWeb;
using Newtonsoft.Json.Linq;
using System.Web;
using System.IO;

class Att_a7PlusPush : IAttDeviceOperation
{
    SqlConnection msqlConn;
    private String mDeviceId;
    const int GET_USER_LIST = 0;
    const int GET_USER_INFO = 1;
    const int SET_USER_INFO = 2;
    const int DEL_USER_INFO = 3;
    const int ALL_DEL = 4;

    public Att_a7PlusPush()
    {
        msqlConn = FKWebTools.GetDBPool();
        //constructor
    }

    public bool ClearAdminPrivilege(string ipAddress, string port)
    {
        throw new NotImplementedException();
    }

    public bool ClearLog(string strDeviceId, string port)
    {
        bool result = false;
        try
        {
            string strTransId = FKWebTools.MakeCmd(msqlConn, "CLEAR_LOG_DATA", strDeviceId, null);
            while (true)
            {
                string strResult = FKWebTools.getCmdResult(msqlConn, strTransId);
                if (strResult == "RESULT")
                {
                    string strSql = "delete from tbl_realtime_glog where device_id='" + strDeviceId + "'";
                    SqlCommand sqlCmd = new SqlCommand(strSql, msqlConn);
                    sqlCmd.ExecuteNonQuery();
                    result = true;
                    break;
                }
                else if (strResult == "CANCELLED")
                {
                    result = false;
                    break;
                }
                else
                {

                }
            }
            return result;
            //StatusTxt.Text = "Success : All of log data has been deleted!";
            //Enables(false);
        }
        catch (Exception ex)
        {
            return result;
            //StatusTxt.Text = "Error: All of log data delete operation failed!";
        }
        throw new NotImplementedException();
    }


    public bool ClearEnrollData(string strDeviceId, string port)
    {
        bool result = false;
        try
        {
            string strTransId = FKWebTools.MakeCmd(msqlConn, "CLEAR_ENROLL_DATA", strDeviceId, null);
            while (true)
            {
                string strResult = FKWebTools.getCmdResult(msqlConn, strTransId);
                if (strResult == "RESULT")
                {
                    string strSql = "delete from tbl_realtime_enroll_data where device_id='" + strDeviceId + "'";
                    SqlCommand sqlCmd = new SqlCommand(strSql, msqlConn);
                    sqlCmd.ExecuteNonQuery();
                    result = true;
                    break;
                }
                else if (strResult == "CANCELLED")
                {
                    result = false;
                    break;
                }
                else
                {

                }
            }
            return result;
            //StatusTxt.Text = "Success : All of log data has been deleted!";
            //Enables(false);
        }
        catch (Exception ex)
        {
            return result;
            //StatusTxt.Text = "Error: All of log data delete operation failed!";
        }
        throw new NotImplementedException();
    }


    public bool DelMultipleUser(string strDeviceId, string port, DataTable dtUserList, string strdwBackupNumber)
    {
        try
        {
            //Session["operation"] = DEL_USER_INFO;
            //string sUserId = UserList.SelectedItem.Value;
            FKWebCmdTrans cmdTrans = new FKWebCmdTrans();
            JObject vResultJson = new JObject();
            string sUserId = string.Empty;
            foreach (DataRow dr in dtUserList.Rows)
            {
                sUserId = dr["sdwEnrollNumber"].ToString();
                vResultJson.Add("user_id", sUserId);
                string sFinal = vResultJson.ToString(Formatting.None);
                byte[] strParam = new byte[0];
                cmdTrans.CreateBSCommBufferFromString(sFinal, out strParam);
                string strTransId = FKWebTools.MakeCmd(msqlConn, "DELETE_USER", strDeviceId, strParam);
                while (true)
                {
                    string strResult = FKWebTools.getCmdResult(msqlConn, strTransId);
                    if (strResult == "RESULT")
                    {
                        //delete real time log
                        string strSql = "delete from tbl_realtime_glog where device_id='" + strDeviceId + "' and user_id='" + sUserId + "'";
                        SqlCommand sqlCmd = new SqlCommand(strSql, msqlConn);
                        sqlCmd.ExecuteNonQuery();

                        //delete real time enroll data
                        strSql = "delete from tbl_realtime_enroll_data where device_id='" + strDeviceId + "' and user_id='" + sUserId + "'";
                        sqlCmd = new SqlCommand(strSql, msqlConn);
                        sqlCmd.ExecuteNonQuery();

                        break;
                    }
                    else if (strResult == "CANCELLED")
                    {
                        break;
                    }
                    else
                    {

                    }
                }
            }
            //we have to check status of command
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }

    }

    public bool DelMultipleUserFace(string ipAddress, string port, DataTable dtUserList)
    {
        throw new NotImplementedException();
    }

    public bool DelMultipleUserFinger(string ipAddress, string port, DataTable dtUserList, DataTable dtFingerList)
    {
        throw new NotImplementedException();
    }

    public bool Device_Connection(string strDeviceId, string port, int index)
    {


        string sSql;
        string sJson;
        mDeviceId = strDeviceId;//Device_List.SelectedItem.Value;
        //LDev_Name.Text = Device_List.SelectedItem.Text + ":" + Device_List.SelectedItem.Value;// sqlReader.GetString(1);
        int mFlag = 0;
        sSql = "select * from tbl_fkdevice_status where device_id='" + mDeviceId + "'";
        SqlCommand sqlCmd = new SqlCommand(sSql, msqlConn);
        SqlDataReader sqlReader = sqlCmd.ExecuteReader();
        DateTime dtNow, dtDev;
        dtNow = DateTime.Now;
        long nTimeDiff = 0;
        try
        {
            if (sqlReader.HasRows)
            {
                if (sqlReader.Read())
                {
                    mFlag = 1;
                    //LDev_ID.Text = mDeviceId;
                    //LDev_Name.Text = sqlReader.GetString(1);
                    sJson = sqlReader.GetString(5);
                    JObject jobjTest = JObject.Parse(sJson);
                    //LDev_Firmware.Text = jobjTest["firmware"].ToString();
                    //LDev_Support.Text = jobjTest["supported_enroll_data"].ToString();//
                    dtDev = sqlReader.GetDateTime(3);
                    nTimeDiff = dtNow.Ticks - dtDev.Ticks;
                    //UpdateTimeTxt.Text = Convert.ToString(dtDev);// +":" + Convert.ToString(dtNow) + "---->" + Convert.ToString(nTimeDiff);//
                    if (nTimeDiff > 60000000)
                    {
                        sqlReader.Close();
                        sSql = "UPDATE tbl_fkdevice_status SET connected='0' where device_id='" + mDeviceId + "'";
                        FKWebTools.ExecuteSimpleCmd(msqlConn, sSql);
                        FKWebTools.CheckDeviceLives(msqlConn, mDeviceId);
                        return false;
                    }
                }
            }
            if (mFlag == 0)
            {
                //LDev_ID.Text = mDeviceId;
                //LDev_Name.Text = Device_List.SelectedItem.Text;
                //LDev_Firmware.Text = "";
                //LDev_Support.Text = "";//
                return false;
            }
            sqlReader.Close();
            FKWebTools.CheckDeviceLives(msqlConn, mDeviceId);
            return true;
            //throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            //StatusTxt.Text = ex.ToString();
            sqlReader.Close();
            return false;
        }
    }

    public clsDeviceCapacity GetCapacityInfo(string strDeviceId, string port)
    {
        clsDeviceCapacity cls = new clsDeviceCapacity();
        try
        {
            string strTransId = FKWebTools.MakeCmd(msqlConn, "GET_DEVICE_STATUS", strDeviceId, null);
            while (true)
            {
                string strResult = FKWebTools.getCmdResult(msqlConn, strTransId);
                if (strResult == "RESULT")
                {
                    FKWebCmdTrans cmdTrans = new FKWebCmdTrans();
                    JObject vResultJson;// = new JObject();
                    string sSql = "select @cmd_result=cmd_result from tbl_fkcmd_trans_cmd_result where trans_id='" + strTransId + "'";
                    SqlCommand sqlCmd = new SqlCommand(sSql, msqlConn);
                    SqlParameter sqlParamCmdParamBin = new SqlParameter("@cmd_result", SqlDbType.VarBinary);
                    sqlParamCmdParamBin.Direction = ParameterDirection.Output;
                    sqlParamCmdParamBin.Size = -1;
                    sqlCmd.Parameters.Add(sqlParamCmdParamBin);
                    sqlCmd.ExecuteNonQuery();
                    byte[] bytCmdResult = (byte[])sqlParamCmdParamBin.Value;
                    byte[] bytResultBin = new byte[0];
                    string sResultText;
                    cmdTrans.GetStringAndBinaryFromBSCommBuffer(bytCmdResult, out sResultText, out bytResultBin);
                    vResultJson = JObject.Parse(sResultText);
                    try
                    {
                        cls.fingerCount = Int32.Parse(vResultJson["fp_count"].ToString());
                    }
                    catch
                    { }
                    try
                    {
                        cls.faceCount = Int32.Parse(vResultJson["face_count"].ToString());
                    }
                    catch
                    { }
                    cls.adminCount = Int32.Parse(vResultJson["manager_count"].ToString());
                    cls.userCount = Int32.Parse(vResultJson["user_count"].ToString());
                    cls.passwordCount = Int32.Parse(vResultJson["password_count"].ToString());
                    cls.logCount = Int32.Parse(vResultJson["total_log_count"].ToString());
                    break;
                }
                else if (strResult == "CANCELLED")
                {
                    //result = false;
                    break;
                }
                else
                {

                }
            }
            return cls;
        }
        catch (Exception ex)
        {
            //StatusTxt.Text = "Error: Reboot device fail!";
            return cls;
        }
        //throw new NotImplementedException();
    }

    public List<clsUserFace> GetSingleUserFace(string ipAddress, string port, int userId)
    {

        List<clsUserFace> lstCls = new List<clsUserFace> { };
        //List<clsUserFinger> _lstCls = new List<clsUserFinger> { };
        try
        {
            FKWebCmdTrans cmdTrans = new FKWebCmdTrans();
            JObject vResultJson;// = new JObject();
            string sSql = "select * from dbo.tbl_realtime_enroll_data tred inner join (select distinct user_id,max(update_time)as update_time from tbl_realtime_enroll_data where device_id='" + ipAddress + "' group by user_id)tbl_updated on tbl_updated.user_id=tred.user_id and tbl_updated.update_time=tred.update_time where tred.device_id='" + ipAddress + "' and and tred.user_id = '" + userId.ToString() + "'";
            SqlCommand sqlCmd = new SqlCommand(sSql, msqlConn);
            SqlDataReader sr = sqlCmd.ExecuteReader();
            if (sr.HasRows)
            {
                while (sr.Read())
                {
                    byte[] bytCmdResult = (byte[])sr.GetValue(3);
                    byte[] bytResultBin = new byte[0];
                    string sResultText;
                    cmdTrans.GetStringAndBinaryFromBSCommBuffer(bytCmdResult, out sResultText, out bytResultBin);
                    vResultJson = JObject.Parse(sResultText);
                    if (bytCmdResult == null)
                    {

                        //clsUserFinger cls = new clsUserFinger();
                        //cls.enrollNumber = strUserId;
                        //lstCls.Add(cls);
                        continue;
                        //return lstCls;
                    }
                    //byte[] bytCmdResult = (byte[])sqlParamCmdParamBin.Value;
                    //byte[] bytResultBin = new byte[0];
                    //string sResultText;
                    //cmdTrans.GetStringAndBinaryFromBSCommBuffer(bytCmdResult, out sResultText, out bytResultBin);
                    //vResultJson = JObject.Parse(sResultText);

                    string strUserId = vResultJson["user_id"].ToString();
                    string strUserNmae = vResultJson["user_name"].ToString();

                    string sUserpriv = vResultJson["user_privilege"].ToString();

                    int vnBinIndex = 0;
                    int vnBinCount = FKWebTools.BACKUP_MAX + 1;
                    int[] vnBackupNumbers = new int[vnBinCount];


                    for (int i = 0; i < vnBinCount; i++)
                    {
                        vnBackupNumbers[i] = -1;

                    }


                    try
                    {
                        //DeleteFile(AbsImgUri);
                        string vStrUserPhotoBinIndex = vResultJson["user_photo"].ToString(); //aCmdParamJson.get("user_photo", "").asString();
                        if (vStrUserPhotoBinIndex.Length != 0)
                        {
                            vnBinIndex = FKWebTools.GetBinIndex(vStrUserPhotoBinIndex) - 1;
                            vnBackupNumbers[vnBinIndex] = FKWebTools.BACKUP_USER_PHOTO;
                        }
                    }
                    catch
                    {
                        //StatusTxt.Text = "No Photo";
                    }




                    string tmp = "";
                    string enroll_data = vResultJson["enroll_data_array"].ToString();
                    if (enroll_data.Equals("null") || enroll_data == "null" || enroll_data.Length == 0)
                    {
                        //StatusTxt.Text = "Enroll data is empty !!!";
                        continue;
                        //return lstCls;
                    }

                    JArray vEnrollDataArrayJson = JArray.Parse(vResultJson["enroll_data_array"].ToString());
                    foreach (JObject content in vEnrollDataArrayJson.Children<JObject>())
                    {
                        int vnBackupNumber = Convert.ToInt32(content["backup_number"].ToString());
                        string vStrBinIndex = content["enroll_data"].ToString();
                        vnBinIndex = FKWebTools.GetBinIndex(vStrBinIndex) - 1;
                        vnBackupNumbers[vnBinIndex] = vnBackupNumber;
                        tmp = tmp + ":" + Convert.ToInt32(vnBinIndex) + "-" + Convert.ToInt32(vnBackupNumber);
                    }

                    string strmphoto = string.Empty;

                    for (int i = 0; i < vnBinCount; i++)
                    {

                        if (vnBackupNumbers[i] == -1) continue;

                        byte[] bytResultBinParam = new byte[0];
                        int vnBinLen = FKWebTools.GetBinarySize(bytResultBin, out bytResultBin);
                        FKWebTools.GetBinaryData(bytResultBin, vnBinLen, out bytResultBinParam, out bytResultBin);
                        //Fp.Checked = true;

                        if (vnBackupNumbers[i] == FKWebTools.BACKUP_USER_PHOTO)
                        {
                            FKWebTools.mPhoto = new byte[vnBinLen];
                            Array.Copy(bytResultBinParam, FKWebTools.mPhoto, vnBinLen);
                            strmphoto = Convert.ToBase64String(FKWebTools.mPhoto, 0, FKWebTools.mPhoto.Length);
                        }
                        if (vnBackupNumbers[i] == FKWebTools.BACKUP_FACE)
                        {
                            clsUserFace cls = new clsUserFace();
                            FKWebTools.mFace = new byte[vnBinLen];
                            Array.Copy(bytResultBinParam, FKWebTools.mFace, vnBinLen);

                            cls.tmpData = Convert.ToBase64String(FKWebTools.mFace, 0, FKWebTools.mFace.Length) + ";" + strmphoto;

                            string fn_debug = "c:\\data\\to_"
                                    + strUserId + "_" + vnBackupNumbers[i];//filename
                            SaveToFile(fn_debug, bytResultBinParam);
                            cls.enrollNumber = strUserId;
                            cls.name = (strUserNmae);
                            cls.password = "";
                            cls.privilege = (sUserpriv.ToString());
                            cls.faceIndex = vnBackupNumbers[i].ToString();
                            cls.length = "0";
                            cls.enabled = ("true");
                            lstCls.Add(cls);
                        }
                    }


                }
            }
            sr.Close();
            return lstCls;
        }
        catch (Exception ex)
        {
            return lstCls;
        }
    }

    private List<clsUser> getUserDetail_New(string strTransId, string strUserId)
    {
        List<clsUser> lstCls = new List<clsUser> { };

        FKWebCmdTrans cmdTrans = new FKWebCmdTrans();
        JObject vResultJson;// = new JObject();
        string sSql = "select @cmd_result=cmd_result from tbl_fkcmd_trans_cmd_result where trans_id='" + strTransId + "'";
        //string sSql = "select @cmd_result=user_data from tbl_realtime_enroll_data where user_id='" + 20 + "'";
        SqlCommand sqlCmd = new SqlCommand(sSql, msqlConn);
        SqlParameter sqlParamCmdParamBin = new SqlParameter("@cmd_result", SqlDbType.VarBinary);
        sqlParamCmdParamBin.Direction = ParameterDirection.Output;
        sqlParamCmdParamBin.Size = -1;
        sqlCmd.Parameters.Add(sqlParamCmdParamBin);
        sqlCmd.ExecuteNonQuery();


        clsUser cls = new clsUser();
        if (sqlParamCmdParamBin.Value == null || sqlParamCmdParamBin.Value == System.DBNull.Value)
        {

            cls.enrollNumber = strUserId;
            lstCls.Add(cls);
            return lstCls;
        }


        byte[] bytCmdResult = (byte[])sqlParamCmdParamBin.Value;
        byte[] bytResultBin = new byte[0];
        string sResultText;
        cmdTrans.GetStringAndBinaryFromBSCommBuffer(bytCmdResult, out sResultText, out bytResultBin);
        vResultJson = JObject.Parse(sResultText);
        if (bytCmdResult == null)
        {
            cls.enrollNumber = strUserId;
            lstCls.Add(cls);
            return lstCls;
        }


        strUserId = vResultJson["user_id"].ToString();
        string strUserNmae = vResultJson["user_name"].ToString();
        string strUserpriv = vResultJson["user_privilege"].ToString();


        cls.enrollNumber = strUserId;
        cls.name = strUserNmae;
        cls.privilege = strUserpriv;

        int vnBinIndex = 0;
        int vnBinCount = FKWebTools.BACKUP_MAX + 1;
        int[] vnBackupNumbers = new int[vnBinCount];

        for (int i = 0; i < vnBinCount; i++)
        {
            vnBackupNumbers[i] = -1;

        }


        try
        {
            //DeleteFile(AbsImgUri);
            string vStrUserPhotoBinIndex = vResultJson["user_photo"].ToString(); //aCmdParamJson.get("user_photo", "").asString();
            if (vStrUserPhotoBinIndex.Length != 0)
            {
                vnBinIndex = FKWebTools.GetBinIndex(vStrUserPhotoBinIndex) - 1;
                vnBackupNumbers[vnBinIndex] = FKWebTools.BACKUP_USER_PHOTO;
            }
        }
        catch
        {
            //StatusTxt.Text = "No Photo";
        }



        string tmp = "";
        string enroll_data = vResultJson["enroll_data_array"].ToString();
        if (enroll_data.Equals("null") || enroll_data == "null" || enroll_data.Length == 0)
        {
            //StatusTxt.Text = "Enroll data is empty !!!";
            cls.enrollNumber = strUserId;
            lstCls.Add(cls);
            return lstCls;
            //return lstCls;
        }

        JArray vEnrollDataArrayJson = JArray.Parse(vResultJson["enroll_data_array"].ToString());
        foreach (JObject content in vEnrollDataArrayJson.Children<JObject>())
        {
            int vnBackupNumber = Convert.ToInt32(content["backup_number"].ToString());
            string vStrBinIndex = content["enroll_data"].ToString();
            vnBinIndex = FKWebTools.GetBinIndex(vStrBinIndex) - 1;
            vnBackupNumbers[vnBinIndex] = vnBackupNumber;
            tmp = tmp + ":" + Convert.ToInt32(vnBinIndex) + "-" + Convert.ToInt32(vnBackupNumber);
        }

        for (int i = 0; i < vnBinCount; i++)
        {
            if (vnBackupNumbers[i] == -1) continue;

            byte[] bytResultBinParam = new byte[0];
            int vnBinLen = FKWebTools.GetBinarySize(bytResultBin, out bytResultBin);
            FKWebTools.GetBinaryData(bytResultBin, vnBinLen, out bytResultBinParam, out bytResultBin);
            //Fp.Checked = true;

            if (vnBackupNumbers[i] == FKWebTools.BACKUP_PSW)
            {
                cls.password = System.Text.Encoding.Default.GetString(bytResultBinParam);

            }

            if (vnBackupNumbers[i] == FKWebTools.BACKUP_CARD)
            {

                cls.cardNumber = System.Text.Encoding.Default.GetString(bytResultBinParam);

            }
        }


        lstCls.Add(cls);
        return lstCls;
    }

    public List<clsUser> GetUser(string strDeviceId, string port)
    {
        List<clsUser> lstCls = new List<clsUser> { };
        List<clsUser> _lstCls = new List<clsUser> { };
        try
        {
            string strTransId = FKWebTools.MakeCmd(msqlConn, "GET_USER_ID_LIST", mDeviceId, null);
            while (true)
            {
                string strResult = FKWebTools.getCmdResult(msqlConn, strTransId);
                if (strResult == "RESULT")
                {
                    string strSelectCmd = "SELECT * FROM tbl_fkcmd_trans_cmd_result_user_id_list where trans_id = '" + strTransId + "'";
                    SqlCommand sqlCmd = new SqlCommand(strSelectCmd, msqlConn);
                    SqlDataReader reader = sqlCmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        DataTable _dtUserList = new DataTable();
                        DataTable dtTemp = new DataTable();
                        _dtUserList.Load(reader);
                        dtTemp = _dtUserList;
                        _dtUserList = null;
                        _dtUserList = dtTemp.DefaultView.ToTable(true, "user_id");
                        reader.Close();

                        FKWebCmdTrans cmdTrans = new FKWebCmdTrans();
                        byte[] mByteParam = new byte[0];
                        foreach (DataRow dr in _dtUserList.Rows)
                        {
                            JObject vResultJson = new JObject();
                            vResultJson.Add("user_id", dr["user_id"].ToString());
                            string mStrParam = vResultJson.ToString(Formatting.None);
                            cmdTrans.CreateBSCommBufferFromString(mStrParam, out mByteParam);
                            string mTransId = FKWebTools.MakeCmd(msqlConn, "GET_USER_INFO", strDeviceId, mByteParam);
                            while (true)
                            {
                                strResult = FKWebTools.getCmdResult(msqlConn, mTransId);
                                if (strResult == "RESULT")
                                {
                                    _lstCls = getUserDetail_New(mTransId, dr["user_id"].ToString());
                                    lstCls.AddRange(_lstCls);
                                    break;
                                }
                                else if (strResult == "CANCELLED")
                                {
                                    break;
                                }
                                else
                                {

                                }
                            }
                        }

                    }
                    break;
                }
                else if (strResult == "CANCELLED")
                {
                    //result = false;
                    break;
                }
                else
                {

                }
            }
            return lstCls;
        }
        catch (Exception ex)
        {
            return lstCls;
        }

    }
    //Last Correct Code
    public List<clsUser> GetUser_Old(string strDeviceId, string port)
    {

        List<clsUser> lstCls = new List<clsUser> { };
        try
        {
            FKWebCmdTrans cmdTrans = new FKWebCmdTrans();
            JObject vResultJson;// = new JObject();
            string sSql = "select * from dbo.tbl_realtime_enroll_data tred inner join (select distinct user_id,max(update_time)as update_time from tbl_realtime_enroll_data where device_id='" + strDeviceId + "' group by user_id)tbl_updated on tbl_updated.user_id=tred.user_id and tbl_updated.update_time=tred.update_time where tred.device_id='" + strDeviceId + "'";
            SqlCommand sqlCmd = new SqlCommand(sSql, msqlConn);
            SqlDataReader sr = sqlCmd.ExecuteReader();
            if (sr.HasRows)
            {
                while (sr.Read())
                {
                    byte[] bytCmdResult = (byte[])sr.GetValue(3);
                    byte[] bytResultBin = new byte[0];
                    string sResultText;
                    cmdTrans.GetStringAndBinaryFromBSCommBuffer(bytCmdResult, out sResultText, out bytResultBin);
                    vResultJson = JObject.Parse(sResultText);
                    if (bytCmdResult == null)
                    {
                        continue;
                    }


                    string strUserId = vResultJson["user_id"].ToString();
                    string strUserNmae = vResultJson["user_name"].ToString();
                    string strUserpriv = vResultJson["user_privilege"].ToString();

                    clsUser cls = new clsUser();
                    cls.enrollNumber = strUserId;
                    cls.name = strUserNmae;
                    cls.privilege = strUserpriv;

                    int vnBinIndex = 0;
                    int vnBinCount = FKWebTools.BACKUP_MAX + 1;
                    int[] vnBackupNumbers = new int[vnBinCount];

                    for (int i = 0; i < vnBinCount; i++)
                    {
                        vnBackupNumbers[i] = -1;

                    }


                    try
                    {
                        //DeleteFile(AbsImgUri);
                        string vStrUserPhotoBinIndex = vResultJson["user_photo"].ToString(); //aCmdParamJson.get("user_photo", "").asString();
                        if (vStrUserPhotoBinIndex.Length != 0)
                        {
                            vnBinIndex = FKWebTools.GetBinIndex(vStrUserPhotoBinIndex) - 1;
                            vnBackupNumbers[vnBinIndex] = FKWebTools.BACKUP_USER_PHOTO;
                        }
                    }
                    catch
                    {
                        //StatusTxt.Text = "No Photo";
                    }



                    string tmp = "";
                    string enroll_data = vResultJson["enroll_data_array"].ToString();
                    if (enroll_data.Equals("null") || enroll_data == "null" || enroll_data.Length == 0)
                    {
                        //StatusTxt.Text = "Enroll data is empty !!!";
                        continue;
                        //return lstCls;
                    }

                    JArray vEnrollDataArrayJson = JArray.Parse(vResultJson["enroll_data_array"].ToString());
                    foreach (JObject content in vEnrollDataArrayJson.Children<JObject>())
                    {
                        int vnBackupNumber = Convert.ToInt32(content["backup_number"].ToString());
                        string vStrBinIndex = content["enroll_data"].ToString();
                        vnBinIndex = FKWebTools.GetBinIndex(vStrBinIndex) - 1;
                        vnBackupNumbers[vnBinIndex] = vnBackupNumber;
                        tmp = tmp + ":" + Convert.ToInt32(vnBinIndex) + "-" + Convert.ToInt32(vnBackupNumber);
                    }

                    for (int i = 0; i < vnBinCount; i++)
                    {
                        if (vnBackupNumbers[i] == -1) continue;

                        byte[] bytResultBinParam = new byte[0];
                        int vnBinLen = FKWebTools.GetBinarySize(bytResultBin, out bytResultBin);
                        FKWebTools.GetBinaryData(bytResultBin, vnBinLen, out bytResultBinParam, out bytResultBin);
                        //Fp.Checked = true;

                        if (vnBackupNumbers[i] == FKWebTools.BACKUP_PSW)
                        {
                            cls.password = System.Text.Encoding.Default.GetString(bytResultBinParam);

                        }
                    }

                    lstCls.Add(cls);
                }
            }
            sr.Close();
            return lstCls;
        }
        catch (Exception ex)
        {
            return lstCls;
        }
    }

    public List<clsUser> GetUserSynchronous(string strDeviceId, string port)
    {
        List<clsUser> lstCls = new List<clsUser> { };
        try
        {
            string strTransId = FKWebTools.MakeCmd(msqlConn, "GET_USER_ID_LIST", strDeviceId, null);
            bool result = false;
            while (true)
            {
                string strResult = FKWebTools.getCmdResult(msqlConn, strTransId);
                if (strResult == "RESULT")
                {
                    result = true;
                    break;
                }
                else if (strResult == "CANCELLED")
                {
                    result = false;
                    break;
                }
                else
                {

                }
            }
            if (result == true)
            {
                string sSql;
                sSql = "select DISTINCT user_id from tbl_fkcmd_trans_cmd_result_user_id_list where trans_id='" + strTransId + "'";
                SqlCommand sqlCmd = new SqlCommand(sSql, msqlConn);
                SqlDataReader sqlReader = sqlCmd.ExecuteReader();
                if (sqlReader.HasRows)
                {
                    while (sqlReader.Read())
                    {
                        clsUser cls = new clsUser();
                        cls.enrollNumber = sqlReader.GetString(0);
                        lstCls.Add(cls);
                    }
                }
                sqlReader.Close();
            }
            return lstCls;
        }
        catch (Exception ex)
        {
            return lstCls;
        }
    }



    //Last Checked code 
    public List<clsUserFinger> GetUserFinger_Old(string strDeviceId, string port)
    {
        List<clsUserFinger> lstCls = new List<clsUserFinger> { };
        //List<clsUserFinger> _lstCls = new List<clsUserFinger> { };
        try
        {
            FKWebCmdTrans cmdTrans = new FKWebCmdTrans();
            JObject vResultJson;// = new JObject();
            string sSql = "select * from dbo.tbl_realtime_enroll_data tred inner join (select distinct user_id,max(update_time)as update_time from tbl_realtime_enroll_data where device_id='" + strDeviceId + "' group by user_id)tbl_updated on tbl_updated.user_id=tred.user_id and tbl_updated.update_time=tred.update_time where tred.device_id='" + strDeviceId + "'";
            SqlCommand sqlCmd = new SqlCommand(sSql, msqlConn);
            //SqlParameter sqlParamCmdParamBin = new SqlParameter("@cmd_result", SqlDbType.VarBinary);
            //sqlParamCmdParamBin.Direction = ParameterDirection.Output;
            //sqlParamCmdParamBin.Size = -1;
            //sqlCmd.Parameters.Add(sqlParamCmdParamBin);
            SqlDataReader sr = sqlCmd.ExecuteReader();
            if (sr.HasRows)
            {
                while (sr.Read())
                {
                    byte[] bytCmdResult = (byte[])sr.GetValue(3);
                    byte[] bytResultBin = new byte[0];
                    string sResultText;
                    cmdTrans.GetStringAndBinaryFromBSCommBuffer(bytCmdResult, out sResultText, out bytResultBin);
                    vResultJson = JObject.Parse(sResultText);
                    if (bytCmdResult == null)
                    {

                        //clsUserFinger cls = new clsUserFinger();
                        //cls.enrollNumber = strUserId;
                        //lstCls.Add(cls);
                        continue;
                        //return lstCls;
                    }
                    //byte[] bytCmdResult = (byte[])sqlParamCmdParamBin.Value;
                    //byte[] bytResultBin = new byte[0];
                    //string sResultText;
                    //cmdTrans.GetStringAndBinaryFromBSCommBuffer(bytCmdResult, out sResultText, out bytResultBin);
                    //vResultJson = JObject.Parse(sResultText);

                    string strUserId = vResultJson["user_id"].ToString();
                    string strUserNmae = vResultJson["user_name"].ToString();

                    string sUserpriv = vResultJson["user_privilege"].ToString();
                    ////UserPriv.SelectedIndex = UserPriv.Items.IndexOf(UserPriv.Items.FindByText(sUserpriv));

                    int vnBinIndex = 0;
                    int vnBinCount = FKWebTools.BACKUP_MAX + 1;
                    int[] vnBackupNumbers = new int[vnBinCount];

                    for (int i = 0; i < vnBinCount; i++)
                    {
                        vnBackupNumbers[i] = -1;
                        if (i <= FKWebTools.BACKUP_FP_9)
                            FKWebTools.mFinger[i] = new byte[0];
                    }


                    try
                    {
                        //DeleteFile(AbsImgUri);
                        string vStrUserPhotoBinIndex = vResultJson["user_photo"].ToString(); //aCmdParamJson.get("user_photo", "").asString();
                        if (vStrUserPhotoBinIndex.Length != 0)
                        {
                            vnBinIndex = FKWebTools.GetBinIndex(vStrUserPhotoBinIndex) - 1;
                            vnBackupNumbers[vnBinIndex] = FKWebTools.BACKUP_USER_PHOTO;
                        }
                    }
                    catch
                    {
                        //StatusTxt.Text = "No Photo";
                    }



                    string tmp = "";
                    string enroll_data = vResultJson["enroll_data_array"].ToString();
                    if (enroll_data.Equals("null") || enroll_data == "null" || enroll_data.Length == 0)
                    {
                        //StatusTxt.Text = "Enroll data is empty !!!";
                        continue;
                        //return lstCls;
                    }

                    JArray vEnrollDataArrayJson = JArray.Parse(vResultJson["enroll_data_array"].ToString());
                    foreach (JObject content in vEnrollDataArrayJson.Children<JObject>())
                    {
                        int vnBackupNumber = Convert.ToInt32(content["backup_number"].ToString());
                        string vStrBinIndex = content["enroll_data"].ToString();
                        vnBinIndex = FKWebTools.GetBinIndex(vStrBinIndex) - 1;
                        vnBackupNumbers[vnBinIndex] = vnBackupNumber;
                        tmp = tmp + ":" + Convert.ToInt32(vnBinIndex) + "-" + Convert.ToInt32(vnBackupNumber);
                    }

                    for (int i = 0; i < vnBinCount; i++)
                    {
                        if (vnBackupNumbers[i] == -1) continue;

                        byte[] bytResultBinParam = new byte[0];
                        int vnBinLen = FKWebTools.GetBinarySize(bytResultBin, out bytResultBin);
                        FKWebTools.GetBinaryData(bytResultBin, vnBinLen, out bytResultBinParam, out bytResultBin);
                        //Fp.Checked = true;

                        if (vnBackupNumbers[i] >= FKWebTools.BACKUP_FP_0 && vnBackupNumbers[i] <= FKWebTools.BACKUP_FP_9)
                        {
                            FKWebTools.mFinger[vnBackupNumbers[i]] = new byte[vnBinLen];
                            Array.Copy(bytResultBinParam, FKWebTools.mFinger[vnBackupNumbers[i]], vnBinLen);
                            clsUserFinger cls = new clsUserFinger();
                            cls.tmpData = Convert.ToBase64String(FKWebTools.mFinger[vnBackupNumbers[i]], 0, FKWebTools.mFinger[vnBackupNumbers[i]].Length);

                            string fn_debug = "c:\\data\\to_"
                                    + strUserId + "_" + vnBackupNumbers[i];//filename
                            SaveToFile(fn_debug, bytResultBinParam);
                            cls.enrollNumber = strUserId;
                            cls.name = strUserNmae;
                            cls.privilege = sUserpriv;
                            cls.fingerIndex = vnBackupNumbers[i];
                            cls.fngBytes = FKWebTools.mFinger[vnBackupNumbers[i]];
                            lstCls.Add(cls);
                        }
                    }

                    //if (lstCls.Count == 0)
                    //{
                    //    clsUserFinger _cls = new clsUserFinger();
                    //    _cls.enrollNumber = strUserId;
                    //    _cls.name = strUserNmae;
                    //    lstCls.Add(_cls);
                    //}
                }
            }
            sr.Close();
            return lstCls;
        }
        catch (Exception ex)
        {
            return lstCls;
        }
        //throw new NotImplementedException();
    }


    public List<clsUserFinger> GetUserFinger(string strDeviceId, string port)
    {
        List<clsUserFinger> lstCls = new List<clsUserFinger> { };
        List<clsUserFinger> _lstCls = new List<clsUserFinger> { };
        try
        {
            string strTransId = FKWebTools.MakeCmd(msqlConn, "GET_USER_ID_LIST", mDeviceId, null);
            while (true)
            {
                string strResult = FKWebTools.getCmdResult(msqlConn, strTransId);
                if (strResult == "RESULT")
                {
                    string strSelectCmd = "SELECT * FROM tbl_fkcmd_trans_cmd_result_user_id_list where trans_id = '" + strTransId + "'";
                    SqlCommand sqlCmd = new SqlCommand(strSelectCmd, msqlConn);
                    SqlDataReader reader = sqlCmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        DataTable _dtUserList = new DataTable();
                        DataTable dtTemp = new DataTable();
                        _dtUserList.Load(reader);
                        dtTemp = _dtUserList;
                        _dtUserList = null;
                        _dtUserList = dtTemp.DefaultView.ToTable(true, "user_id");
                        reader.Close();

                        FKWebCmdTrans cmdTrans = new FKWebCmdTrans();
                        byte[] mByteParam = new byte[0];
                        foreach (DataRow dr in _dtUserList.Rows)
                        {
                            JObject vResultJson = new JObject();
                            vResultJson.Add("user_id", dr["user_id"].ToString());
                            string mStrParam = vResultJson.ToString(Formatting.None);
                            cmdTrans.CreateBSCommBufferFromString(mStrParam, out mByteParam);
                            string mTransId = FKWebTools.MakeCmd(msqlConn, "GET_USER_INFO", strDeviceId, mByteParam);
                            while (true)
                            {
                                strResult = FKWebTools.getCmdResult(msqlConn, mTransId);
                                if (strResult == "RESULT")
                                {
                                    _lstCls = getUserDetail_FingerNew(mTransId, dr["user_id"].ToString());
                                    try
                                    {
                                        if (_lstCls[0].name != "Empty")
                                        {

                                            lstCls.AddRange(_lstCls);


                                        }
                                    }
                                    catch
                                    {

                                    }

                                    break;
                                }
                                else if (strResult == "CANCELLED")
                                {
                                    break;
                                }
                                else
                                {

                                }
                            }
                        }

                    }
                    break;
                }
                else if (strResult == "CANCELLED")
                {
                    //result = false;
                    break;
                }
                else
                {

                }
            }
            return lstCls;
        }
        catch (Exception ex)
        {
            return lstCls;
        }

    }
    public List<clsUserFinger> getUserDetail_FingerNew(string strTransId, string strUserId)
    {
        List<clsUserFinger> lstCls = new List<clsUserFinger> { };
        //List<clsUserFinger> _lstCls = new List<clsUserFinger> { };
        try
        {
            FKWebCmdTrans cmdTrans = new FKWebCmdTrans();
            JObject vResultJson;// = new JObject();
            string sSql = "select @cmd_result=cmd_result from tbl_fkcmd_trans_cmd_result where trans_id='" + strTransId + "'";
            //string sSql = "select @cmd_result=user_data from tbl_realtime_enroll_data where user_id='" + 20 + "'";
            SqlCommand sqlCmd = new SqlCommand(sSql, msqlConn);
            SqlParameter sqlParamCmdParamBin = new SqlParameter("@cmd_result", SqlDbType.VarBinary);
            sqlParamCmdParamBin.Direction = ParameterDirection.Output;
            sqlParamCmdParamBin.Size = -1;
            sqlCmd.Parameters.Add(sqlParamCmdParamBin);
            sqlCmd.ExecuteNonQuery();

            if (sqlParamCmdParamBin.Value == null || sqlParamCmdParamBin.Value == System.DBNull.Value)
            {
                clsUserFinger cls = new clsUserFinger();
                cls.enrollNumber = strUserId;
                lstCls.Add(cls);
                return lstCls;
            }
            byte[] bytCmdResult = (byte[])sqlParamCmdParamBin.Value;


            byte[] bytResultBin = new byte[0];
            string sResultText;
            cmdTrans.GetStringAndBinaryFromBSCommBuffer(bytCmdResult, out sResultText, out bytResultBin);
            vResultJson = JObject.Parse(sResultText);
            if (bytCmdResult == null)
            {

                clsUserFinger cls = new clsUserFinger();
                cls.enrollNumber = strUserId;
                cls.name = "Empty";
                lstCls.Add(cls);
                return lstCls;
                //return lstCls;
            }


            strUserId = vResultJson["user_id"].ToString();
            string strUserNmae = vResultJson["user_name"].ToString();

            string sUserpriv = vResultJson["user_privilege"].ToString();
            ////UserPriv.SelectedIndex = UserPriv.Items.IndexOf(UserPriv.Items.FindByText(sUserpriv));

            int vnBinIndex = 0;
            int vnBinCount = FKWebTools.BACKUP_MAX + 1;
            int[] vnBackupNumbers = new int[vnBinCount];

            for (int i = 0; i < vnBinCount; i++)
            {
                vnBackupNumbers[i] = -1;
                if (i <= FKWebTools.BACKUP_FP_9)
                    FKWebTools.mFinger[i] = new byte[0];
            }


            try
            {
                //DeleteFile(AbsImgUri);
                string vStrUserPhotoBinIndex = vResultJson["user_photo"].ToString(); //aCmdParamJson.get("user_photo", "").asString();
                if (vStrUserPhotoBinIndex.Length != 0)
                {
                    vnBinIndex = FKWebTools.GetBinIndex(vStrUserPhotoBinIndex) - 1;
                    vnBackupNumbers[vnBinIndex] = FKWebTools.BACKUP_USER_PHOTO;
                }
            }
            catch
            {
                //StatusTxt.Text = "No Photo";
            }



            string tmp = "";
            string enroll_data = vResultJson["enroll_data_array"].ToString();
            if (enroll_data.Equals("null") || enroll_data == "null" || enroll_data.Length == 0)
            {
                //StatusTxt.Text = "Enroll data is empty !!!";
                clsUserFinger cls = new clsUserFinger();
                cls.enrollNumber = strUserId;
                cls.name = "Empty";
                lstCls.Add(cls);
                return lstCls;
                //return lstCls;
            }

            JArray vEnrollDataArrayJson = JArray.Parse(vResultJson["enroll_data_array"].ToString());
            foreach (JObject content in vEnrollDataArrayJson.Children<JObject>())
            {
                int vnBackupNumber = Convert.ToInt32(content["backup_number"].ToString());
                string vStrBinIndex = content["enroll_data"].ToString();
                vnBinIndex = FKWebTools.GetBinIndex(vStrBinIndex) - 1;
                vnBackupNumbers[vnBinIndex] = vnBackupNumber;
                tmp = tmp + ":" + Convert.ToInt32(vnBinIndex) + "-" + Convert.ToInt32(vnBackupNumber);
            }

            for (int i = 0; i < vnBinCount; i++)
            {
                if (vnBackupNumbers[i] == -1) continue;

                byte[] bytResultBinParam = new byte[0];
                int vnBinLen = FKWebTools.GetBinarySize(bytResultBin, out bytResultBin);
                FKWebTools.GetBinaryData(bytResultBin, vnBinLen, out bytResultBinParam, out bytResultBin);
                //Fp.Checked = true;

                if (vnBackupNumbers[i] >= FKWebTools.BACKUP_FP_0 && vnBackupNumbers[i] <= FKWebTools.BACKUP_FP_9)
                {
                    FKWebTools.mFinger[vnBackupNumbers[i]] = new byte[vnBinLen];
                    Array.Copy(bytResultBinParam, FKWebTools.mFinger[vnBackupNumbers[i]], vnBinLen);
                    clsUserFinger cls = new clsUserFinger();
                    cls.tmpData = Convert.ToBase64String(FKWebTools.mFinger[vnBackupNumbers[i]], 0, FKWebTools.mFinger[vnBackupNumbers[i]].Length);

                    string fn_debug = "c:\\data\\to_"
                            + strUserId + "_" + vnBackupNumbers[i];//filename
                    SaveToFile(fn_debug, bytResultBinParam);
                    cls.enrollNumber = strUserId;
                    cls.name = strUserNmae;
                    cls.privilege = sUserpriv;
                    cls.fingerIndex = vnBackupNumbers[i];
                    cls.fngBytes = FKWebTools.mFinger[vnBackupNumbers[i]];
                    lstCls.Add(cls);
                }
            }

            //if (lstCls.Count == 0)
            //{
            //    clsUserFinger _cls = new clsUserFinger();
            //    _cls.enrollNumber = strUserId;
            //    _cls.name = strUserNmae;
            //    lstCls.Add(_cls);
            //}

            return lstCls;
        }
        catch (Exception ex)
        {
            clsUserFinger cls = new clsUserFinger();
            cls.enrollNumber = strUserId;
            cls.name = "Empty";
            lstCls.Add(cls);
            return lstCls;
        }
        //throw new NotImplementedException();
    }

    public List<clsUserFinger> GetUserFingerSynchronous(string strDeviceId, string port)
    {
        List<clsUserFinger> lstCls = new List<clsUserFinger> { };
        List<clsUserFinger> _lstCls = new List<clsUserFinger> { };
        try
        {
            string strTransId = FKWebTools.MakeCmd(msqlConn, "GET_USER_ID_LIST", mDeviceId, null);
            while (true)
            {
                string strResult = FKWebTools.getCmdResult(msqlConn, strTransId);
                if (strResult == "RESULT")
                {
                    string strSelectCmd = "SELECT * FROM tbl_fkcmd_trans_cmd_result_user_id_list where trans_id = '" + strTransId + "'";
                    SqlCommand sqlCmd = new SqlCommand(strSelectCmd, msqlConn);
                    SqlDataReader reader = sqlCmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        using (DataTable _dtUserList = new DataTable())
                        {
                            _dtUserList.Load(reader);
                            reader.Close();

                            FKWebCmdTrans cmdTrans = new FKWebCmdTrans();
                            byte[] mByteParam = new byte[0];
                            foreach (DataRow dr in _dtUserList.Rows)
                            {
                                JObject vResultJson = new JObject();
                                vResultJson.Add("user_id", dr["user_id"].ToString());
                                string mStrParam = vResultJson.ToString(Formatting.None);
                                cmdTrans.CreateBSCommBufferFromString(mStrParam, out mByteParam);
                                string mTransId = FKWebTools.MakeCmd(msqlConn, "GET_USER_INFO", strDeviceId, mByteParam);
                                while (true)
                                {
                                    strResult = FKWebTools.getCmdResult(msqlConn, mTransId);
                                    if (strResult == "RESULT")
                                    {
                                        _lstCls = getUserFingerDetail(mTransId, dr["user_id"].ToString());
                                        lstCls.AddRange(_lstCls);
                                        break;
                                    }
                                    else if (strResult == "CANCELLED")
                                    {
                                        break;
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                        }
                    }
                    break;
                }
                else if (strResult == "CANCELLED")
                {
                    //result = false;
                    break;
                }
                else
                {

                }
            }
            return lstCls;
        }
        catch (Exception ex)
        {
            return lstCls;
        }
        //throw new NotImplementedException();
    }

    public List<clsUserLog> GetUserLog(string strDeviceId, string port)
    {
        List<clsUserLog> lstCls = new List<clsUserLog> { };
        //-----implementation using realtime log table----------------
        try
        {
            string strSelectCmd = "SELECT * FROM tbl_realtime_glog where device_id = '" + strDeviceId + "'";
            SqlCommand sqlCmd = new SqlCommand(strSelectCmd, msqlConn);
            SqlDataReader reader = sqlCmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    clsUserLog cls = new clsUserLog();
                    cls.enrollNumber = reader.GetString(2);
                    cls.verifyMode = reader.GetString(3);
                    cls.inOutMode = reader.GetString(4);
                    cls.logTime = reader.GetDateTime(5).ToString();
                    cls.workCode = "";
                    cls.lDateTime = reader.GetDateTime(5);
                    lstCls.Add(cls);
                }
            }
            reader.Close();
            return lstCls;

        }
        catch (Exception ex)
        {
            return lstCls;
        }

        //---implementaton with operation code-------------------
        //JObject vResultJson = new JObject();
        //FKWebCmdTrans cmdTrans = new FKWebCmdTrans();
        //try
        //{
        //    string sFinal = vResultJson.ToString(Formatting.None);
        //    byte[] strParam = new byte[0];
        //    cmdTrans.CreateBSCommBufferFromString(sFinal, out strParam);
        //    string strTransId = FKWebTools.MakeCmd(msqlConn, "GET_LOG_DATA", strDeviceId, strParam);
        //    while (true)
        //    {
        //        string strResult = FKWebTools.getCmdResult(msqlConn, strTransId);
        //        if (strResult == "RESULT")
        //        {
        //            string strSelectCmd = "SELECT * FROM tbl_fkcmd_trans_cmd_result_log_data where trans_id = '" + strTransId + "'";
        //            SqlCommand sqlCmd = new SqlCommand(strSelectCmd, msqlConn);
        //            SqlDataReader reader = sqlCmd.ExecuteReader();
        //            if (reader.HasRows)
        //            {
        //                while (reader.Read())
        //                {
        //                    clsUserLog cls = new clsUserLog();
        //                    cls.enrollNumber = reader.GetString(2);
        //                    cls.verifyMode = reader.GetString(3);
        //                    cls.inOutMode = reader.GetString(4);
        //                    cls.logTime = reader.GetDateTime(5).ToString();
        //                    cls.workCode = "";
        //                    cls.lDateTime = reader.GetDateTime(5);
        //                    lstCls.Add(cls);
        //                }
        //            }
        //            //result = true;
        //            reader.Close();
        //            break;
        //        }
        //        else if (strResult == "CANCELLED")
        //        {
        //            //result = false;
        //            break;
        //        }
        //        else
        //        {

        //        }
        //    }
        //    return lstCls;

        //}
        //catch (Exception ex)
        //{
        //    return lstCls;
        //}
    }

    public bool InitializeDevice(string ipAddress, string port)
    {
        if (ClearLog(ipAddress, port) && ClearEnrollData(ipAddress, port))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public bool PowerOffDevice(string ipAddress, string port)
    {
        throw new NotImplementedException();
    }

    public bool RestartDevice(string strDeviceId, string port)
    {
        bool result = false; ;
        try
        {
            string strTransId = FKWebTools.MakeCmd(msqlConn, "RESET_FK", strDeviceId, null);
            while (true)
            {
                string strResult = FKWebTools.getCmdResult(msqlConn, strTransId);
                if (strResult == "RESULT")
                {
                    result = true;
                    break;
                }
                else if (strResult == "CANCELLED")
                {
                    result = false;
                    break;
                }
                else
                {

                }
            }
            return result;
        }
        catch
        {
            //StatusTxt.Text = "Error: Reboot device fail!";
            return result;
        }
        //throw new NotImplementedException();
    }

    public bool SetDeviceTime(string strDeviceId, string port)
    {
        bool result = false;
        try
        {
            DateTime now = DateTime.Now;
            string sNowTxt = FKWebTools.GetFKTimeString14(now);
            JObject vResultJson = new JObject();
            FKWebCmdTrans cmdTrans = new FKWebCmdTrans();
            vResultJson.Add("time", sNowTxt);
            string sFinal = vResultJson.ToString(Formatting.None);
            byte[] strParam = new byte[0];
            cmdTrans.CreateBSCommBufferFromString(sFinal, out strParam);
            string strTransId = FKWebTools.MakeCmd(msqlConn, "SET_TIME", strDeviceId, strParam);
            while (true)
            {
                string strResult = FKWebTools.getCmdResult(msqlConn, strTransId);
                if (strResult == "RESULT")
                {
                    result = true;
                    break;
                }
                else if (strResult == "CANCELLED")
                {
                    result = false;
                    break;
                }
                else
                {

                }
            }
            return result;
            //Enables(false);
        }
        catch
        {
            return result;
            //StatusTxt.Text = "Error: Set time fail!";
        }
        //throw new NotImplementedException();
    }

    public bool UploadCardUserFpFace(string strDeviceId, string port, DataTable dtUserList, DataTable dtFingerList, DataTable dtFaceList, bool isBatchUpdate)
    {
        try
        {
            FKWebCmdTrans cmdTrans = new FKWebCmdTrans();
            for (int loop = 0; loop < dtUserList.Rows.Count; loop++)
            {

                try
                {
                    JObject vResultJson = new JObject();
                    JArray vEnrollDataArrayJson = new JArray();
                    string sUserPwd = dtUserList.Rows[loop]["Password"].ToString();
                    string sUserCard = dtUserList.Rows[loop]["Cardnumber"].ToString();
                    int index = 1;
                    string sUserId = dtUserList.Rows[loop]["UserID"].ToString();
                    string sUserName = dtUserList.Rows[loop]["Name"].ToString();
                    string sUserPriv = dtUserList.Rows[loop]["Privilege"].ToString();
                    //UserPhoto.ImageUrl = ".\\photo\\" + mDevId + "_" + sUserId + ".jpg";

                    vResultJson.Add("user_id", sUserId);
                    vResultJson.Add("user_name", sUserName);
                    vResultJson.Add("user_privilege", sUserPriv);
                    //if (FKWebTools.mPhoto.Length > 0)
                    //    vResultJson.Add("user_photo", FKWebTools.GetBinIndexString(index++));
                    int count = 0;
                    byte[] binData = new byte[0];


                    if (dtFaceList.Rows.Count > 0)
                    {
                        string sdwEnrollNumber = dtUserList.Rows[loop]["UserID"].ToString();
                        using (DataTable dtEmpFace = new DataView(dtFaceList, "sUserID='" + sdwEnrollNumber + "'", "", DataViewRowState.CurrentRows).ToTable())
                        {
                            for (int i = 0; i < dtEmpFace.Rows.Count; i++)
                            {
                                count++;
                                //for photo

                                FKWebTools.mPhoto = Convert.FromBase64String(dtEmpFace.Rows[i][5].ToString().Split(';')[1]);

                                vResultJson.Add("user_photo", FKWebTools.GetBinIndexString(count));
                                FKWebTools.AppendBinaryData(ref binData, FKWebTools.mPhoto);
                                break;
                            }
                        }

                    }



                    if (dtFingerList.Rows.Count > 0)
                    {
                        using (DataTable dtEmpFinger = new DataView(dtFingerList, "sdwEnrollNumber='" + sUserId + "'", "", DataViewRowState.CurrentRows).ToTable())
                        {
                            int idwFingerIndex = 0;
                            string sTmpData = string.Empty;

                            for (int i = 0; i < dtEmpFinger.Rows.Count; i++)
                            {
                                count++;
                                Int32.TryParse(dtEmpFinger.Rows[i][2].ToString(), out idwFingerIndex);
                                sTmpData = dtEmpFinger.Rows[i][3].ToString();
                                byte[] temp = Convert.FromBase64String(sTmpData);
                                //byte[] temp = lstFng[0].fngBytes;
                                JObject vEnrollDataJson = new JObject();
                                vEnrollDataJson.Add("backup_number", idwFingerIndex);
                                vEnrollDataJson.Add("enroll_data", FKWebTools.GetBinIndexString(count));
                                FKWebTools.AppendBinaryData(ref binData, temp);
                                vEnrollDataArrayJson.Add(vEnrollDataJson);
                                string fn_debug = "c:\\data\\from_"
                                    + sUserId + "_" + idwFingerIndex;//filename
                                SaveToFile(fn_debug, temp);


                            }
                        }
                    }

                    if (sUserPwd.Length > 0)
                    {
                        count++;
                        JObject vEnrollDataJson = new JObject();
                        vEnrollDataJson.Add("backup_number", FKWebTools.BACKUP_PSW);
                        vEnrollDataJson.Add("enroll_data", FKWebTools.GetBinIndexString(count));
                        vEnrollDataArrayJson.Add(vEnrollDataJson);

                        byte[] mPwdBin = System.Text.Encoding.UTF8.GetBytes(sUserPwd);
                        FKWebTools.AppendBinaryData(ref binData, mPwdBin);
                    }

                    if (sUserCard.Length > 0)
                    {
                        count++;
                        JObject vEnrollDataJson = new JObject();
                        vEnrollDataJson.Add("backup_number", FKWebTools.BACKUP_CARD);
                        vEnrollDataJson.Add("enroll_data", FKWebTools.GetBinIndexString(count));
                        vEnrollDataArrayJson.Add(vEnrollDataJson);

                        //byte[] mCardBin = new byte[0];
                        //cmdTrans.CreateBSCommBufferFromString(sUserCard, out mCardBin);
                        //FKWebTools.ConcateByteArray(ref binData, mCardBin);

                        byte[] mCardBin = System.Text.Encoding.UTF8.GetBytes(sUserCard);
                        FKWebTools.AppendBinaryData(ref binData, mCardBin);
                    }
                    if (dtFaceList.Rows.Count > 0)
                    {
                        string sdwEnrollNumber = dtUserList.Rows[loop]["UserID"].ToString();
                        using (DataTable dtEmpFace = new DataView(dtFaceList, "sUserID='" + sdwEnrollNumber + "'", "", DataViewRowState.CurrentRows).ToTable())
                        {
                            for (int i = 0; i < dtEmpFace.Rows.Count; i++)
                            {
                                count++;
                                //Int32.TryParse(dtEmpFace.Rows[i][4].ToString(), out iFaceIndex);
                                //iFaceIndex = Convert.ToInt32(dtEmpFace.Rows[i][4].ToString());
                                string sTmpData = dtEmpFace.Rows[i][5].ToString().Split(';')[0];
                                FKWebTools.mFace = Convert.FromBase64String(sTmpData);
                                JObject vEnrollDataJson = new JObject();
                                vEnrollDataJson.Add("backup_number", FKWebTools.BACKUP_FACE);
                                vEnrollDataJson.Add("enroll_data", FKWebTools.GetBinIndexString(count));
                                vEnrollDataArrayJson.Add(vEnrollDataJson);
                                FKWebTools.AppendBinaryData(ref binData, FKWebTools.mFace);
                                break;
                            }
                        }

                    }


                    vResultJson.Add("enroll_data_array", vEnrollDataArrayJson);
                    string sFinal = vResultJson.ToString(Formatting.None);
                    while (sFinal.Contains("\r\n "))
                    {
                        sFinal.Replace("\r\n ", "\r\n");
                    }
                    sFinal.Replace("\r\n", "");

                    byte[] strParam = new byte[0];

                    //start
                    //09-aug-2019

                    //if (sUserPwd.Length > 0)
                    //{
                    //    byte[] mPwdBin = new byte[0];
                    //    cmdTrans.CreateBSCommBufferFromString(sUserPwd, out mPwdBin);
                    //    FKWebTools.ConcateByteArray(ref binData, mPwdBin);
                    //}
                    //if (sUserCard.Length > 0)
                    //{
                    //    byte[] mCardBin = new byte[0];
                    //    cmdTrans.CreateBSCommBufferFromString(sUserCard, out mCardBin);
                    //    FKWebTools.ConcateByteArray(ref binData, mCardBin);
                    //}

                    //end

                    cmdTrans.CreateBSCommBufferFromString(sFinal, out strParam);
                    FKWebTools.ConcateByteArray(ref strParam, binData);
                    string strTransId = FKWebTools.MakeCmd(msqlConn, "SET_USER_INFO", strDeviceId, strParam);
                    while (true)
                    {
                        string strResult = FKWebTools.getCmdResult(msqlConn, strTransId);
                        if (strResult == "RESULT")
                        {
                            string strSql = "INSERT INTO [tbl_realtime_enroll_data]           ([update_time]           ,[device_id]           ,[user_id]           ,[user_data])     VALUES           ('" + DateTime.Now.ToString() + "','" + strDeviceId + "','" + sUserId + "',@bindata)";
                            SqlParameter sqlparam = new SqlParameter();
                            SqlCommand sqlCmd = new SqlCommand(strSql, msqlConn);
                            sqlCmd.Parameters.Add("@bindata", SqlDbType.Binary).Value = strParam;
                            sqlCmd.ExecuteNonQuery();
                            break;
                        }
                        else if (strResult == "CANCELLED")
                        {
                            break;
                        }
                        else
                        {

                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
            //StatusTxt.Text = ex.ToString();
        }
    }
    protected void SaveToFile(string fn, byte[] buff)
    {
        try
        {
            FileStream fs = new FileStream(fn, FileMode.OpenOrCreate, FileAccess.Write);
            fs.Write(buff, 0, buff.Length);
            fs.Close();
        }
        catch
        { }
    }
    //Correct Code

    private List<clsUserFace> getUserFace_Old2(string strTransId, string strUserId)
    {
        List<clsUserFace> lstCls = new List<clsUserFace> { };
        FKWebCmdTrans cmdTrans = new FKWebCmdTrans();
        JObject vResultJson;// = new JObject();
        string sSql = "select @cmd_result=cmd_result from tbl_fkcmd_trans_cmd_result where trans_id='" + strTransId + "'";
        SqlCommand sqlCmd = new SqlCommand(sSql, msqlConn);
        SqlParameter sqlParamCmdParamBin = new SqlParameter("@cmd_result", SqlDbType.VarBinary);
        sqlParamCmdParamBin.Direction = ParameterDirection.Output;
        sqlParamCmdParamBin.Size = -1;
        sqlCmd.Parameters.Add(sqlParamCmdParamBin);
        sqlCmd.ExecuteNonQuery();
        byte[] bytCmdResult = (byte[])sqlParamCmdParamBin.Value;
        byte[] bytResultBin = new byte[0];
        string sResultText;
        cmdTrans.GetStringAndBinaryFromBSCommBuffer(bytCmdResult, out sResultText, out bytResultBin);
        vResultJson = JObject.Parse(sResultText);

        int vnBinIndex = 0;
        int vnBinCount = FKWebTools.BACKUP_MAX + 1;
        int[] vnBackupNumbers = new int[vnBinCount];

        for (int i = 0; i < vnBinCount; i++)
        {
            vnBackupNumbers[i] = -1;
            if (i <= FKWebTools.BACKUP_FP_9)
                FKWebTools.mFinger[i] = new byte[0];
        }
        FKWebTools.mFace = new byte[0];
        //FKWebTools.mPhoto = new byte[0];


        string tmp = "";
        string enroll_data = vResultJson["enroll_data_array"].ToString();
        if (enroll_data.Equals("null") || enroll_data == "null" || enroll_data.Length == 0)
        {
            //StatusTxt.Text = "Enroll data is empty !!!";
            return lstCls;
        }

        JArray vEnrollDataArrayJson = JArray.Parse(vResultJson["enroll_data_array"].ToString());
        foreach (JObject content in vEnrollDataArrayJson.Children<JObject>())
        {
            int vnBackupNumber = Convert.ToInt32(content["backup_number"].ToString());
            string vStrBinIndex = content["enroll_data"].ToString();
            vnBinIndex = FKWebTools.GetBinIndex(vStrBinIndex) - 1;
            vnBackupNumbers[vnBinIndex] = vnBackupNumber;
            tmp = tmp + ":" + Convert.ToInt32(vnBinIndex) + "-" + Convert.ToInt32(vnBackupNumber);
        }
        //Fp.Checked = false;
        //Face.Checked = false;
        //StatusTxt.Text = tmp;
        for (int i = 0; i < vnBinCount; i++)
        {
            if (vnBackupNumbers[i] == -1) continue;
            if (vnBackupNumbers[i] == FKWebTools.BACKUP_FACE)
            {
                clsUserFace cls = new clsUserFace();
                byte[] bytResultBinParam = new byte[0];
                int vnBinLen = FKWebTools.GetBinarySize(bytResultBin, out bytResultBin);
                FKWebTools.GetBinaryData(bytResultBin, vnBinLen, out bytResultBinParam, out bytResultBin);
                //Face.Checked = true;
                FKWebTools.mFace = new byte[vnBinLen];
                Array.Copy(bytResultBinParam, FKWebTools.mFace, vnBinLen);
                cls.tmpData = FKWebTools.mFace.ToString();
                cls.enrollNumber = strUserId;
                lstCls.Add(cls);
            }
        }
        return lstCls;
    }
    private List<clsUserFace> getUserFace_Old(string strTransId, string strUserId)
    {
        List<clsUserFace> lstCls = new List<clsUserFace> { };
        FKWebCmdTrans cmdTrans = new FKWebCmdTrans();
        JObject vResultJson;// = new JObject();
        string sSql = "select @cmd_result=cmd_result from tbl_fkcmd_trans_cmd_result where trans_id='" + strTransId + "'";
        SqlCommand sqlCmd = new SqlCommand(sSql, msqlConn);
        SqlParameter sqlParamCmdParamBin = new SqlParameter("@cmd_result", SqlDbType.VarBinary);
        sqlParamCmdParamBin.Direction = ParameterDirection.Output;
        sqlParamCmdParamBin.Size = -1;
        sqlCmd.Parameters.Add(sqlParamCmdParamBin);
        sqlCmd.ExecuteNonQuery();
        byte[] bytCmdResult = (byte[])sqlParamCmdParamBin.Value;
        byte[] bytResultBin = new byte[0];
        string sResultText;
        cmdTrans.GetStringAndBinaryFromBSCommBuffer(bytCmdResult, out sResultText, out bytResultBin);
        vResultJson = JObject.Parse(sResultText);

        int vnBinIndex = 0;
        int vnBinCount = FKWebTools.BACKUP_MAX + 1;
        int[] vnBackupNumbers = new int[vnBinCount];

        for (int i = 0; i < vnBinCount; i++)
        {
            vnBackupNumbers[i] = -1;
            if (i <= FKWebTools.BACKUP_FP_9)
                FKWebTools.mFinger[i] = new byte[0];
        }
        FKWebTools.mFace = new byte[0];
        //FKWebTools.mPhoto = new byte[0];


        string tmp = "";
        string enroll_data = vResultJson["enroll_data_array"].ToString();
        if (enroll_data.Equals("null") || enroll_data == "null" || enroll_data.Length == 0)
        {
            //StatusTxt.Text = "Enroll data is empty !!!";
            return lstCls;
        }

        JArray vEnrollDataArrayJson = JArray.Parse(vResultJson["enroll_data_array"].ToString());
        foreach (JObject content in vEnrollDataArrayJson.Children<JObject>())
        {
            int vnBackupNumber = Convert.ToInt32(content["backup_number"].ToString());
            string vStrBinIndex = content["enroll_data"].ToString();
            vnBinIndex = FKWebTools.GetBinIndex(vStrBinIndex) - 1;
            vnBackupNumbers[vnBinIndex] = vnBackupNumber;
            tmp = tmp + ":" + Convert.ToInt32(vnBinIndex) + "-" + Convert.ToInt32(vnBackupNumber);
        }
        //Fp.Checked = false;
        //Face.Checked = false;
        //StatusTxt.Text = tmp;
        for (int i = 0; i < vnBinCount; i++)
        {
            if (vnBackupNumbers[i] == -1) continue;
            if (vnBackupNumbers[i] == FKWebTools.BACKUP_FACE)
            {
                clsUserFace cls = new clsUserFace();
                byte[] bytResultBinParam = new byte[0];
                int vnBinLen = FKWebTools.GetBinarySize(bytResultBin, out bytResultBin);
                FKWebTools.GetBinaryData(bytResultBin, vnBinLen, out bytResultBinParam, out bytResultBin);
                //Face.Checked = true;
                FKWebTools.mFace = new byte[vnBinLen];
                Array.Copy(bytResultBinParam, FKWebTools.mFace, vnBinLen);
                cls.tmpData = FKWebTools.mFace.ToString();
                cls.enrollNumber = strUserId;
                lstCls.Add(cls);
            }
        }
        return lstCls;
    }
    public List<clsUserFace> GetUserFace_old1(string ipAddress, string port)
    {
        List<clsUserFace> lstCls = new List<clsUserFace> { };
        //List<clsUserFinger> _lstCls = new List<clsUserFinger> { };
        try
        {
            FKWebCmdTrans cmdTrans = new FKWebCmdTrans();
            JObject vResultJson;// = new JObject();
            string sSql = "select * from dbo.tbl_realtime_enroll_data tred inner join (select distinct user_id,max(update_time)as update_time from tbl_realtime_enroll_data where device_id='" + ipAddress + "' group by user_id)tbl_updated on tbl_updated.user_id=tred.user_id and tbl_updated.update_time=tred.update_time where tred.device_id='" + ipAddress + "'";
            SqlCommand sqlCmd = new SqlCommand(sSql, msqlConn);
            SqlDataReader sr = sqlCmd.ExecuteReader();
            if (sr.HasRows)
            {
                while (sr.Read())
                {
                    byte[] bytCmdResult = (byte[])sr.GetValue(3);
                    byte[] bytResultBin = new byte[0];
                    string sResultText;
                    cmdTrans.GetStringAndBinaryFromBSCommBuffer(bytCmdResult, out sResultText, out bytResultBin);
                    vResultJson = JObject.Parse(sResultText);
                    if (bytCmdResult == null)
                    {

                        //clsUserFinger cls = new clsUserFinger();
                        //cls.enrollNumber = strUserId;
                        //lstCls.Add(cls);
                        continue;
                        //return lstCls;
                    }
                    //byte[] bytCmdResult = (byte[])sqlParamCmdParamBin.Value;
                    //byte[] bytResultBin = new byte[0];
                    //string sResultText;
                    //cmdTrans.GetStringAndBinaryFromBSCommBuffer(bytCmdResult, out sResultText, out bytResultBin);
                    //vResultJson = JObject.Parse(sResultText);

                    string strUserId = vResultJson["user_id"].ToString();
                    string strUserNmae = vResultJson["user_name"].ToString();

                    string sUserpriv = vResultJson["user_privilege"].ToString();

                    int vnBinIndex = 0;
                    int vnBinCount = FKWebTools.BACKUP_MAX + 1;
                    int[] vnBackupNumbers = new int[vnBinCount];


                    for (int i = 0; i < vnBinCount; i++)
                    {
                        vnBackupNumbers[i] = -1;

                    }


                    try
                    {
                        //DeleteFile(AbsImgUri);
                        string vStrUserPhotoBinIndex = vResultJson["user_photo"].ToString(); //aCmdParamJson.get("user_photo", "").asString();
                        if (vStrUserPhotoBinIndex.Length != 0)
                        {
                            vnBinIndex = FKWebTools.GetBinIndex(vStrUserPhotoBinIndex) - 1;
                            vnBackupNumbers[vnBinIndex] = FKWebTools.BACKUP_USER_PHOTO;
                        }
                    }
                    catch
                    {
                        //StatusTxt.Text = "No Photo";
                    }




                    string tmp = "";
                    string enroll_data = vResultJson["enroll_data_array"].ToString();
                    if (enroll_data.Equals("null") || enroll_data == "null" || enroll_data.Length == 0)
                    {
                        //StatusTxt.Text = "Enroll data is empty !!!";
                        continue;
                        //return lstCls;
                    }

                    JArray vEnrollDataArrayJson = JArray.Parse(vResultJson["enroll_data_array"].ToString());
                    foreach (JObject content in vEnrollDataArrayJson.Children<JObject>())
                    {
                        int vnBackupNumber = Convert.ToInt32(content["backup_number"].ToString());
                        string vStrBinIndex = content["enroll_data"].ToString();
                        vnBinIndex = FKWebTools.GetBinIndex(vStrBinIndex) - 1;
                        vnBackupNumbers[vnBinIndex] = vnBackupNumber;
                        tmp = tmp + ":" + Convert.ToInt32(vnBinIndex) + "-" + Convert.ToInt32(vnBackupNumber);
                    }

                    string strmphoto = string.Empty;

                    for (int i = 0; i < vnBinCount; i++)
                    {

                        if (vnBackupNumbers[i] == -1) continue;

                        byte[] bytResultBinParam = new byte[0];
                        int vnBinLen = FKWebTools.GetBinarySize(bytResultBin, out bytResultBin);
                        FKWebTools.GetBinaryData(bytResultBin, vnBinLen, out bytResultBinParam, out bytResultBin);
                        //Fp.Checked = true;

                        if (vnBackupNumbers[i] == FKWebTools.BACKUP_USER_PHOTO)
                        {
                            FKWebTools.mPhoto = new byte[vnBinLen];
                            Array.Copy(bytResultBinParam, FKWebTools.mPhoto, vnBinLen);
                            strmphoto = Convert.ToBase64String(FKWebTools.mPhoto, 0, FKWebTools.mPhoto.Length);
                        }
                        if (vnBackupNumbers[i] == FKWebTools.BACKUP_FACE)
                        {
                            clsUserFace cls = new clsUserFace();
                            FKWebTools.mFace = new byte[vnBinLen];
                            Array.Copy(bytResultBinParam, FKWebTools.mFace, vnBinLen);

                            cls.tmpData = Convert.ToBase64String(FKWebTools.mFace, 0, FKWebTools.mFace.Length) + ";" + strmphoto;

                            string fn_debug = "c:\\data\\to_"
                                    + strUserId + "_" + vnBackupNumbers[i];//filename
                            SaveToFile(fn_debug, bytResultBinParam);
                            cls.enrollNumber = strUserId;
                            cls.name = (strUserNmae);
                            cls.password = "";
                            cls.privilege = (sUserpriv.ToString());
                            cls.faceIndex = vnBackupNumbers[i].ToString();
                            cls.length = "0";
                            cls.enabled = ("true");
                            lstCls.Add(cls);
                        }
                    }


                }
            }
            sr.Close();
            return lstCls;
        }
        catch (Exception ex)
        {
            return lstCls;
        }
    }
    public List<clsUserFace> GetUserFace_old3(string ipAddress, string port)
    {
        List<clsUserFace> lstCls = new List<clsUserFace> { };
        //List<clsUserFinger> _lstCls = new List<clsUserFinger> { };
        try
        {
            FKWebCmdTrans cmdTrans = new FKWebCmdTrans();
            JObject vResultJson;// = new JObject();
            string sSql = "select * from dbo.tbl_realtime_enroll_data tred inner join (select distinct user_id,max(update_time)as update_time from tbl_realtime_enroll_data where device_id='" + ipAddress + "' group by user_id)tbl_updated on tbl_updated.user_id=tred.user_id and tbl_updated.update_time=tred.update_time where tred.device_id='" + ipAddress + "'";
            SqlCommand sqlCmd = new SqlCommand(sSql, msqlConn);
            SqlDataReader sr = sqlCmd.ExecuteReader();
            if (sr.HasRows)
            {
                while (sr.Read())
                {
                    byte[] bytCmdResult = (byte[])sr.GetValue(3);
                    byte[] bytResultBin = new byte[0];
                    string sResultText;
                    cmdTrans.GetStringAndBinaryFromBSCommBuffer(bytCmdResult, out sResultText, out bytResultBin);
                    vResultJson = JObject.Parse(sResultText);
                    if (bytCmdResult == null)
                    {

                        //clsUserFinger cls = new clsUserFinger();
                        //cls.enrollNumber = strUserId;
                        //lstCls.Add(cls);
                        continue;
                        //return lstCls;
                    }
                    //byte[] bytCmdResult = (byte[])sqlParamCmdParamBin.Value;
                    //byte[] bytResultBin = new byte[0];
                    //string sResultText;
                    //cmdTrans.GetStringAndBinaryFromBSCommBuffer(bytCmdResult, out sResultText, out bytResultBin);
                    //vResultJson = JObject.Parse(sResultText);

                    string strUserId = vResultJson["user_id"].ToString();
                    string strUserNmae = vResultJson["user_name"].ToString();

                    string sUserpriv = vResultJson["user_privilege"].ToString();

                    int vnBinIndex = 0;
                    int vnBinCount = FKWebTools.BACKUP_MAX + 1;
                    int[] vnBackupNumbers = new int[vnBinCount];


                    for (int i = 0; i < vnBinCount; i++)
                    {
                        vnBackupNumbers[i] = -1;

                    }


                    try
                    {
                        //DeleteFile(AbsImgUri);
                        string vStrUserPhotoBinIndex = vResultJson["user_photo"].ToString(); //aCmdParamJson.get("user_photo", "").asString();
                        if (vStrUserPhotoBinIndex.Length != 0)
                        {
                            vnBinIndex = FKWebTools.GetBinIndex(vStrUserPhotoBinIndex) - 1;
                            vnBackupNumbers[vnBinIndex] = FKWebTools.BACKUP_USER_PHOTO;
                        }
                    }
                    catch
                    {
                        //StatusTxt.Text = "No Photo";
                    }




                    string tmp = "";
                    string enroll_data = vResultJson["enroll_data_array"].ToString();
                    if (enroll_data.Equals("null") || enroll_data == "null" || enroll_data.Length == 0)
                    {
                        //StatusTxt.Text = "Enroll data is empty !!!";
                        continue;
                        //return lstCls;
                    }

                    JArray vEnrollDataArrayJson = JArray.Parse(vResultJson["enroll_data_array"].ToString());
                    foreach (JObject content in vEnrollDataArrayJson.Children<JObject>())
                    {
                        int vnBackupNumber = Convert.ToInt32(content["backup_number"].ToString());
                        string vStrBinIndex = content["enroll_data"].ToString();
                        vnBinIndex = FKWebTools.GetBinIndex(vStrBinIndex) - 1;
                        vnBackupNumbers[vnBinIndex] = vnBackupNumber;
                        tmp = tmp + ":" + Convert.ToInt32(vnBinIndex) + "-" + Convert.ToInt32(vnBackupNumber);
                    }

                    string strmphoto = string.Empty;

                    for (int i = 0; i < vnBinCount; i++)
                    {

                        if (vnBackupNumbers[i] == -1) continue;

                        byte[] bytResultBinParam = new byte[0];
                        int vnBinLen = FKWebTools.GetBinarySize(bytResultBin, out bytResultBin);
                        FKWebTools.GetBinaryData(bytResultBin, vnBinLen, out bytResultBinParam, out bytResultBin);
                        //Fp.Checked = true;

                        if (vnBackupNumbers[i] == FKWebTools.BACKUP_USER_PHOTO)
                        {
                            FKWebTools.mPhoto = new byte[vnBinLen];
                            Array.Copy(bytResultBinParam, FKWebTools.mPhoto, vnBinLen);
                            strmphoto = Convert.ToBase64String(FKWebTools.mPhoto, 0, FKWebTools.mPhoto.Length);
                        }
                        if (vnBackupNumbers[i] == FKWebTools.BACKUP_FACE)
                        {
                            clsUserFace cls = new clsUserFace();
                            FKWebTools.mFace = new byte[vnBinLen];
                            Array.Copy(bytResultBinParam, FKWebTools.mFace, vnBinLen);

                            cls.tmpData = Convert.ToBase64String(FKWebTools.mFace, 0, FKWebTools.mFace.Length) + ";" + strmphoto;

                            string fn_debug = "c:\\data\\to_"
                                    + strUserId + "_" + vnBackupNumbers[i];//filename
                            SaveToFile(fn_debug, bytResultBinParam);
                            cls.enrollNumber = strUserId;
                            cls.name = (strUserNmae);
                            cls.password = "";
                            cls.privilege = (sUserpriv.ToString());
                            cls.faceIndex = vnBackupNumbers[i].ToString();
                            cls.length = "0";
                            cls.enabled = ("true");
                            lstCls.Add(cls);
                        }
                    }


                }
            }
            sr.Close();
            return lstCls;
        }
        catch (Exception ex)
        {
            return lstCls;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strDeviceId"></param>
    /// <param name="port"></param>
    /// <returns></returns>
    public List<clsUserFace> GetUserFace(string strDeviceId, string port)
    {
        List<clsUserFace> lstCls = new List<clsUserFace> { };
        List<clsUserFace> _lstCls = new List<clsUserFace> { };
        try
        {
            string strTransId = FKWebTools.MakeCmd(msqlConn, "GET_USER_ID_LIST", mDeviceId, null);
            while (true)
            {
                string strResult = FKWebTools.getCmdResult(msqlConn, strTransId);
                if (strResult == "RESULT")
                {
                    string strSelectCmd = "SELECT * FROM tbl_fkcmd_trans_cmd_result_user_id_list where trans_id = '" + strTransId + "'";
                    SqlCommand sqlCmd = new SqlCommand(strSelectCmd, msqlConn);
                    SqlDataReader reader = sqlCmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        DataTable _dtUserList = new DataTable();
                        DataTable dtTemp = new DataTable();
                        _dtUserList.Load(reader);
                        dtTemp = _dtUserList;
                        _dtUserList = null;
                        _dtUserList = dtTemp.DefaultView.ToTable(true, "user_id");
                        reader.Close();

                        FKWebCmdTrans cmdTrans = new FKWebCmdTrans();
                        byte[] mByteParam = new byte[0];
                        foreach (DataRow dr in _dtUserList.Rows)
                        {
                            JObject vResultJson = new JObject();
                            vResultJson.Add("user_id", dr["user_id"].ToString());
                            string mStrParam = vResultJson.ToString(Formatting.None);
                            cmdTrans.CreateBSCommBufferFromString(mStrParam, out mByteParam);
                            string mTransId = FKWebTools.MakeCmd(msqlConn, "GET_USER_INFO", strDeviceId, mByteParam);
                            while (true)
                            {
                                strResult = FKWebTools.getCmdResult(msqlConn, mTransId);
                                if (strResult == "RESULT")
                                {
                                    _lstCls = getUserDetail_FaceNew(mTransId, dr["user_id"].ToString());                                       
                                    try
                                    {
                                        if (_lstCls[0].name != "Empty")
                                        {
                                            lstCls.AddRange(_lstCls);
                                        }
                                    }
                                    catch
                                    {
                                    }

                                    break;
                                }
                                else if (strResult == "CANCELLED")
                                {
                                    break;
                                }
                                else
                                {

                                }
                            }
                        }

                    }
                    break;
                }
                else if (strResult == "CANCELLED")
                {
                    //result = false;
                    break;
                }
                else
                {

                }
            }
            return lstCls;
        }
        catch (Exception ex)
        {
            return lstCls;
        }

    }
    public List<clsUserFace> getUserDetail_FaceNew(string strTransId, string strUserId)
    {
        List<clsUserFace> lstCls = new List<clsUserFace> { };

        try
        {
            FKWebCmdTrans cmdTrans = new FKWebCmdTrans();
            JObject vResultJson;// = new JObject();
            string sSql = "select @cmd_result=cmd_result from tbl_fkcmd_trans_cmd_result where trans_id='" + strTransId + "'";
            SqlCommand sqlCmd = new SqlCommand(sSql, msqlConn);
            SqlParameter sqlParamCmdParamBin = new SqlParameter("@cmd_result", SqlDbType.VarBinary);
            sqlParamCmdParamBin.Direction = ParameterDirection.Output;
            sqlParamCmdParamBin.Size = -1;
            sqlCmd.Parameters.Add(sqlParamCmdParamBin);
            sqlCmd.ExecuteNonQuery();
            byte[] bytCmdResult = (byte[])sqlParamCmdParamBin.Value;
            byte[] bytResultBin = new byte[0];
            string sResultText;
            cmdTrans.GetStringAndBinaryFromBSCommBuffer(bytCmdResult, out sResultText, out bytResultBin);
            vResultJson = JObject.Parse(sResultText);





            if (bytCmdResult == null)
            {

                clsUserFace cls = new clsUserFace();
                cls.enrollNumber = strUserId;
                cls.name = "Empty";
                lstCls.Add(cls);
                return lstCls;
            }
            //byte[] bytCmdResult = (byte[])sqlParamCmdParamBin.Value;
            //byte[] bytResultBin = new byte[0];
            //string sResultText;
            //cmdTrans.GetStringAndBinaryFromBSCommBuffer(bytCmdResult, out sResultText, out bytResultBin);
            //vResultJson = JObject.Parse(sResultText);

            strUserId = vResultJson["user_id"].ToString();
            string strUserNmae = vResultJson["user_name"].ToString();

            string sUserpriv = vResultJson["user_privilege"].ToString();

            int vnBinIndex = 0;
            int vnBinCount = FKWebTools.BACKUP_MAX + 1;
            int[] vnBackupNumbers = new int[vnBinCount];


            for (int i = 0; i < vnBinCount; i++)
            {
                vnBackupNumbers[i] = -1;

            }


            try
            {
                //DeleteFile(AbsImgUri);
                string vStrUserPhotoBinIndex = vResultJson["user_photo"].ToString(); //aCmdParamJson.get("user_photo", "").asString();
                if (vStrUserPhotoBinIndex.Length != 0)
                {
                    vnBinIndex = FKWebTools.GetBinIndex(vStrUserPhotoBinIndex) - 1;
                    vnBackupNumbers[vnBinIndex] = FKWebTools.BACKUP_USER_PHOTO;
                }
            }
            catch
            {
                //StatusTxt.Text = "No Photo";
            }




            string tmp = "";
            string enroll_data = vResultJson["enroll_data_array"].ToString();
            if (enroll_data.Equals("null") || enroll_data == "null" || enroll_data.Length == 0)
            {
                clsUserFace cls = new clsUserFace();
                cls.enrollNumber = strUserId;
                cls.name = "Empty";
                lstCls.Add(cls);
                return lstCls;
            }

            JArray vEnrollDataArrayJson = JArray.Parse(vResultJson["enroll_data_array"].ToString());
            foreach (JObject content in vEnrollDataArrayJson.Children<JObject>())
            {
                int vnBackupNumber = Convert.ToInt32(content["backup_number"].ToString());
                string vStrBinIndex = content["enroll_data"].ToString();
                vnBinIndex = FKWebTools.GetBinIndex(vStrBinIndex) - 1;
                vnBackupNumbers[vnBinIndex] = vnBackupNumber;
                tmp = tmp + ":" + Convert.ToInt32(vnBinIndex) + "-" + Convert.ToInt32(vnBackupNumber);
            }

            string strmphoto = string.Empty;

            for (int i = 0; i < vnBinCount; i++)
            {

                if (vnBackupNumbers[i] == -1) continue;

                byte[] bytResultBinParam = new byte[0];
                int vnBinLen = FKWebTools.GetBinarySize(bytResultBin, out bytResultBin);
                FKWebTools.GetBinaryData(bytResultBin, vnBinLen, out bytResultBinParam, out bytResultBin);
                //Fp.Checked = true;

                if (vnBackupNumbers[i] == FKWebTools.BACKUP_USER_PHOTO)
                {
                    FKWebTools.mPhoto = new byte[vnBinLen];
                    Array.Copy(bytResultBinParam, FKWebTools.mPhoto, vnBinLen);
                    strmphoto = Convert.ToBase64String(FKWebTools.mPhoto, 0, FKWebTools.mPhoto.Length);
                }
                if (vnBackupNumbers[i] == FKWebTools.BACKUP_FACE)
                {
                    clsUserFace cls = new clsUserFace();
                    FKWebTools.mFace = new byte[vnBinLen];
                    Array.Copy(bytResultBinParam, FKWebTools.mFace, vnBinLen);

                    cls.tmpData = Convert.ToBase64String(FKWebTools.mFace, 0, FKWebTools.mFace.Length) + ";" + strmphoto;

                    string fn_debug = "c:\\data\\to_"
                            + strUserId + "_" + vnBackupNumbers[i];//filename
                    SaveToFile(fn_debug, bytResultBinParam);
                    cls.enrollNumber = strUserId;
                    cls.name = (strUserNmae);
                    cls.password = "";
                    cls.privilege = (sUserpriv.ToString());
                    cls.faceIndex = vnBackupNumbers[i].ToString();
                    cls.length = "0";
                    cls.enabled = ("true");
                    lstCls.Add(cls);
                }
            }

        }
        catch (Exception ex)
        {
            clsUserFace cls = new clsUserFace();
            cls.enrollNumber = strUserId;
            cls.name = "Empty";
            lstCls.Add(cls);
            return lstCls;
        }
        return lstCls;
    }


    private List<clsUserFinger> getUserFingerDetail(string strTransId, string strUserId)
    {
        List<clsUserFinger> lstCls = new List<clsUserFinger> { };
        FKWebCmdTrans cmdTrans = new FKWebCmdTrans();
        JObject vResultJson;// = new JObject();
        string sSql = "select @cmd_result=cmd_result from tbl_fkcmd_trans_cmd_result where trans_id='" + strTransId + "'";
        //string sSql = "select @cmd_result=user_data from tbl_realtime_enroll_data where user_id='" + 20 + "'";
        SqlCommand sqlCmd = new SqlCommand(sSql, msqlConn);
        SqlParameter sqlParamCmdParamBin = new SqlParameter("@cmd_result", SqlDbType.VarBinary);
        sqlParamCmdParamBin.Direction = ParameterDirection.Output;
        sqlParamCmdParamBin.Size = -1;
        sqlCmd.Parameters.Add(sqlParamCmdParamBin);
        sqlCmd.ExecuteNonQuery();
        if (sqlParamCmdParamBin.Value == null || sqlParamCmdParamBin.Value == System.DBNull.Value)
        {
            clsUserFinger cls = new clsUserFinger();
            cls.enrollNumber = strUserId;
            lstCls.Add(cls);
            return lstCls;
        }
        byte[] bytCmdResult = (byte[])sqlParamCmdParamBin.Value;
        byte[] bytResultBin = new byte[0];
        string sResultText;
        cmdTrans.GetStringAndBinaryFromBSCommBuffer(bytCmdResult, out sResultText, out bytResultBin);
        vResultJson = JObject.Parse(sResultText);

        //string strUserId = vResultJson["user_id"].ToString();
        string strUserNmae = vResultJson["user_name"].ToString();

        string sUserpriv = vResultJson["user_privilege"].ToString();
        //UserPriv.SelectedIndex = UserPriv.Items.IndexOf(UserPriv.Items.FindByText(sUserpriv));

        int vnBinIndex = 0;
        int vnBinCount = FKWebTools.BACKUP_MAX + 1;
        int[] vnBackupNumbers = new int[vnBinCount];

        for (int i = 0; i < vnBinCount; i++)
        {
            vnBackupNumbers[i] = -1;
            if (i <= FKWebTools.BACKUP_FP_9)
                FKWebTools.mFinger[i] = new byte[0];
        }

        string tmp = "";
        string enroll_data = vResultJson["enroll_data_array"].ToString();
        if (enroll_data.Equals("null") || enroll_data == "null" || enroll_data.Length == 0)
        {
            //StatusTxt.Text = "Enroll data is empty !!!";
            return lstCls;
        }

        JArray vEnrollDataArrayJson = JArray.Parse(vResultJson["enroll_data_array"].ToString());
        foreach (JObject content in vEnrollDataArrayJson.Children<JObject>())
        {
            int vnBackupNumber = Convert.ToInt32(content["backup_number"].ToString());
            string vStrBinIndex = content["enroll_data"].ToString();
            vnBinIndex = FKWebTools.GetBinIndex(vStrBinIndex) - 1;
            vnBackupNumbers[vnBinIndex] = vnBackupNumber;
            tmp = tmp + ":" + Convert.ToInt32(vnBinIndex) + "-" + Convert.ToInt32(vnBackupNumber);
        }
        //Fp.Checked = false;
        //Face.Checked = false;
        //StatusTxt.Text = tmp;
        for (int i = 0; i < vnBinCount; i++)
        {
            if (vnBackupNumbers[i] == -1) continue;
            if (vnBackupNumbers[i] >= FKWebTools.BACKUP_FP_0 && vnBackupNumbers[i] <= FKWebTools.BACKUP_FP_9)
            {
                clsUserFinger cls = new clsUserFinger();
                byte[] bytResultBinParam = new byte[0];
                int vnBinLen = FKWebTools.GetBinarySize(bytResultBin, out bytResultBin);
                FKWebTools.GetBinaryData(bytResultBin, vnBinLen, out bytResultBinParam, out bytResultBin);
                //Fp.Checked = true;
                FKWebTools.mFinger[vnBackupNumbers[i]] = new byte[vnBinLen];
                Array.Copy(bytResultBinParam, FKWebTools.mFinger[vnBackupNumbers[i]], vnBinLen);
                cls.tmpData = FKWebTools.mFinger[vnBackupNumbers[i]].ToString();
                cls.enrollNumber = strUserId;
                cls.name = strUserNmae;
                cls.privilege = sUserpriv;
                lstCls.Add(cls);
            }
        }

        if (lstCls.Count == 0)
        {
            clsUserFinger _cls = new clsUserFinger();
            _cls.enrollNumber = strUserId;
            _cls.name = strUserNmae;
            lstCls.Add(_cls);
        }
        return lstCls;
    }
}

