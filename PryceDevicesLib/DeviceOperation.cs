using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PryceDevicesLib
{
    public enum TechType
    {
        PULL = 1,
        PUSH = 2
    }
    public class DeviceOperation
    {
        public DeviceOperation() //constructor
        {

        }
        public static IAttDeviceOperation getDeviceLibObj(TechType strTechType, string strDeviceMake)
        {
            if (strTechType == TechType.PULL)
            {
                switch (strDeviceMake)
                {
                    case "cpPlus":
                        return new Att_cpPlus();
                    case "eSSL":
                        return new Att_eSSL();
                    case "zkTecho":
                        return new Att_zkTecho();
                    case "ZiComm":
                        return new Att_ZiComm();
                    case "RealBio":
                        return new Att_RealBio();
                    case "a7Plus":
                        return new Att_a7PlusPull();
                    default:
                        return new Att_zkTecho();
                }
            }
            else
            {
                switch (strDeviceMake)
                {
                    case "cpPlus":
                        return new Att_cpPlus();
                    case "eSSL":
                        return new Att_eSSL();
                    case "zkTecho":
                        return new Att_zkTecho();
                    case "ZiComm":
                        return new Att_ZiComm();
                    case "RealBio":
                        return new Att_RealBio();
                    case "a7Plus":
                        return new Att_a7PlusPush();
                    default:
                        return new Att_zkTecho();
                }
            }
        }

    }
    public class clsUserLog
    {
        public string machineNo { get; set; }
        public string enrollNumber { get; set; }
        public string verifyMode { get; set; }
        public string inOutMode { get; set; }
        public string logTime { get; set; }
        public DateTime lDateTime { get; set; }
        public string workCode { get; set; }
    }
    public class clsDeviceCapacity
    {
        public int adminCount { get; set; }
        public int passwordCount { get; set; }
        public int logCount { get; set; }
        public int fingerCount { get; set; }
        public int faceCount { get; set; }
        public int userCount { get; set; }
        public int opLogCount { get; set; }
    }
    public class clsUserFinger:clsUser
    {
        public int fingerIndex { get; set; }
        public string tmpData { get; set; }
        public string flag { get; set; }
        public byte[] fngBytes { get; set; }
    }

    public class clsUserFace:clsUser
    {
        public string faceIndex { get; set; }
        public string tmpData { get; set; }
        public string length { get; set; }
    }

    public class clsUser
    {
        public string enrollNumber { get; set; }
        public string name { get; set; }
        public string cardNumber { get; set; }
        public string password { get; set; }
        public string privilege { get; set; }
        public string enabled { get; set; }
    }

}
