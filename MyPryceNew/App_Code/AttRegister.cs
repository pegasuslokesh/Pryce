using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AttRegister
/// </summary>
public class AttRegister
{
    public AttRegister()
    {

        //
        // TODO: Add constructor logic here
        //
    }


    public class timeRecordEventAttribute
    {
        public string attributeName { get; set; }
        public string attributeValue { get; set; }
    }

    public class timeRecordEvent
    {
        public string timeRecordId { get; set; }
        public string timeRecordVersion { get; set; }
        public string startTime { get; set; }
        public string stopTime { get; set; }
        public string reporterIdType { get; set; }
        public string reporterId { get; set; }
        public string assignmentNumber { get; set; }
        public string comment { get; set; }
        public string operationType { get; set; }
        public string measure { get; set; }
        public List<timeRecordEventAttribute> timeRecordEventAttribute { get; set; }
    }

    public  class Attpushcls
    {
        public string processInline { get; set; }
        public string processMode { get; set; }
       public List<timeRecordEvent> timeRecordEvent { get; set; }
    }

}