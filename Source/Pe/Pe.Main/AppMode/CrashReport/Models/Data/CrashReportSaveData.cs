using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ContentTypeTextNet.Pe.Main.AppMode.CrashReport.Models.Data
{
    [Serializable, DataContract]
    internal class CrashReportSaveData
    {
        #region property

        [JsonPropertyName("mail_address")]
        public string MailAddress { get; set; } = string.Empty;
        [JsonPropertyName("comment")]

        public string Comment { get; set; } = string.Empty;
        [JsonPropertyName("user_id")]
        public string UserId { get; set; } = string.Empty;
        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;
        [JsonPropertyName("revision")]
        public string Revision { get; set; } = string.Empty;
        [JsonPropertyName("build")]
        public string Build { get; set; } = string.Empty;
        [JsonPropertyName("exception")]
        public string Exception { get; set; } = string.Empty;
        [JsonPropertyName("information")]
        public Dictionary<string, Dictionary<string, string?>> Information { get; set; } = new Dictionary<string, Dictionary<string, string?>>();
        [JsonPropertyName("log_items")]
        public List<LogItem> LogItems { get; set; } = new List<LogItem>();

        #endregion
    }
}
