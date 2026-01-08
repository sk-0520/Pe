using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;
using NuGet.Configuration;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;

namespace License
{
    public class LicenseApplication
    {
        public LicenseApplication(LicenseOptions options, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType());
            Options = options;
        }

        #region property

        private ILoggerFactory LoggerFactory { get; }
        private ILogger Logger { get; }
        private LicenseOptions Options { get; }

        #endregion

        #region function


        private NuGetPackageItem ToNuGetPackageItem(XmlElement xmlElement)
        {
            return new NuGetPackageItem() {
                PackageId = xmlElement.GetAttribute("Include"),
                Version = Version.Parse(xmlElement.GetAttribute("Version")),
            };
        }

        private IEnumerable<NuGetPackageItem> LoadNuGetPackageItems(string centralPackagePath)
        {
            var propsXml = new XmlDocument();
            propsXml.Load(centralPackagePath);

            var packageVersions = propsXml.SelectNodes("//PackageVersion");
            if(packageVersions is null) {
                throw new InvalidOperationException("No PackageVersion elements found.");
            }

            return packageVersions
                .OfType<XmlElement>()
                .Select(ToNuGetPackageItem)
            ;
        }

        private async Task<IPackageSearchMetadata> GetMetadataAsync(NuGetPackageItem item, PackageMetadataResource metadataResource, SourceCacheContext cache, CancellationToken cancellationToken)
        {
            var version = NuGetVersion.Parse(item.Version.ToString());
            var identity = new PackageIdentity(item.PackageId, version);

            var metadata = await metadataResource.GetMetadataAsync(identity, cache, NuGet.Common.NullLogger.Instance, cancellationToken);
            if(metadata is null) {
                throw new InvalidOperationException($"PackageId: {item.PackageId}, Version: {version}");
            }

            return metadata;
        }


        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var nugetPackageItems = LoadNuGetPackageItems(Options.CentralPackage);


            // nuget.org の V3 エンドポイント
            var providers = Repository.Provider.GetCoreV3();
            var packageSource = new PackageSource("https://api.nuget.org/v3/index.json");
            var sourceRepository = new SourceRepository(packageSource, providers);

            // メタデータ取得用リソースを取得
            var metadataResource = await sourceRepository.GetResourceAsync<PackageMetadataResource>();

            // キャッシュコンテキストとロガー（NuGet の API 用）
            using var cache = new SourceCacheContext();

            List<IPackageSearchMetadata> metadatas = new();
            foreach(var nugetPackageItem in nugetPackageItems) {
                var metadata = await GetMetadataAsync(nugetPackageItem, metadataResource, cache, cancellationToken);
                metadatas.Add(metadata);
            }

        }

        #endregion
    }
}
