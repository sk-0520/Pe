using System;

namespace License
{
    public class NuGetPackageItem
    {
        #region property

        public string PackageId { get; init; } = string.Empty;
        public Version Version { get; init; } = new Version(0, 0, 0);

        #endregion
    }
}
