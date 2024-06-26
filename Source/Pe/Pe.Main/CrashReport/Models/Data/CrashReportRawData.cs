using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Models.Data
{
    [Serializable, DataContract]
    public class CrashReportRawData
    {
        #region property

        [DataMember]
        public string UserId { get; set; } = string.Empty;
        [DataMember]
        public Version Version { get; set; } = new Version();
        [DataMember]
        public string Build { get; set; } = string.Empty;
        [DataMember]
        public string Revision { get; set; } = string.Empty;

        [DataMember]
        public string Exception { get; set; } = string.Empty;
        [DataMember]
        public Dictionary<string, Dictionary<string, string?>> InformationMap { get; set; } = new Dictionary<string, Dictionary<string, string?>>();
        [DataMember]
        public List<LogItem> LogItems { get; set; } = new List<LogItem>();

        #endregion
    }
}
