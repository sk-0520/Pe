using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace License
{
    public class LicenseComponentLicense
    {
        #region property

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("uri")]
        public string Uri { get; set; } = string.Empty;

        #endregion
    }
    public class LicenseComponentItem
    {
        #region property

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("uri")]
        public string Uri { get; set; } = string.Empty;

        [JsonPropertyName("license")]
        public LicenseComponentLicense License { get; set; } = new LicenseComponentLicense();

        [JsonPropertyName("comment")]
        public string Comment { get; set; } = string.Empty;

        #endregion
    }

    public class LicenseComponent
    {
        #region property

        [JsonPropertyName("library")]
        public List<LicenseComponentItem> Library { get; set; } = new List<LicenseComponentItem>();

        [JsonPropertyName("resource")]
        public List<LicenseComponentItem> Resource { get; set; } = new List<LicenseComponentItem>();

        #endregion
    }
}
