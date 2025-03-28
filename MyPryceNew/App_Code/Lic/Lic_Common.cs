using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Lic_Common
/// </summary>
public class Lic_Common
{
    SystemParameter objSysParam = null;
    public Lic_Common(string strConString)
    {
        //
        // TODO: Add constructor logic here
        //
        objSysParam = new SystemParameter(strConString);

    }
    public int getAttDeviceLicCount()
    {
        int _result = 1;
        try
        {
            clsLicenseInfo clsLicInfo = getLicInfo();
            _result = clsLicInfo.attDeviceCount;
        }
        catch
        {
            _result = 1;
        }
        return _result;
    }

    public int getUserLicCount()
    {
        int _result = 0;

        return _result;
    }

    public string getEmployeeLicCount()
    {
        return "";
    }

    private clsLicenseInfo getLicInfo()
    {
        clsLicenseInfo clsLicInfo = new clsLicenseInfo();
        try
        {
            using (DataTable dt = objSysParam.GetSysParameterByParamName("Lic_Key"))
            {
                if (dt.Rows.Count > 0)
                {
                    string strLicInfo = dt.Rows[0]["Param_Value"].ToString();
                    strLicInfo = Common.Decrypt(strLicInfo);
                    string[] strLicContent = strLicInfo.Split('#');
                    foreach (string str in strLicContent)
                    {
                        switch (str.Split('|')[0].ToString())
                        {
                            case "Reg_Code":
                                clsLicInfo.regCode = str.Split('|')[1].ToString();
                                break;
                            case "Expiry_Date":
                                //clsLicInfo.expiryDate = DateTime.Parse(str.Split(':')[1].ToString());
                                DateTime expiryDate;
                                DateTime.TryParse(str.Split('|')[1].ToString(), out expiryDate);
                                clsLicInfo.expiryDate= expiryDate;
                                break;
                            case "Att_Device_Count":
                                clsLicInfo.attDeviceCount = Convert.ToInt32(str.Split('|')[1].ToString());
                                break;
                            case "Version_Type":
                                clsLicInfo.versionType = str.Split('|')[1].ToString();
                                break;
                            case "License_Key":
                                clsLicInfo.licenseKey = str.Split('|')[1].ToString();
                                break;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
        return clsLicInfo;
    }
    public class clsLicenseInfo
    {
        public string regCode { get; set; }
        public DateTime expiryDate { get; set; }
        public int attDeviceCount { get; set; }
        public string versionType { get; set; }
        public string licenseKey { get; set; }
        public string machineId { get; set; }
        public string regEmail { get; set; }
        public string regPhone { get; set; }
    }
}