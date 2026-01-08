using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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

        private IEnumerable<NuGetPackageItem> ReadNuGetPackageItems(string centralPackagePath)
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

        private async Task<LicenseComponent> ReadComponentFileAsync(string baseFilePath, CancellationToken cancellationToken)
        {
            var baseFileContent = await System.IO.File.ReadAllTextAsync(baseFilePath, cancellationToken);

            var licenseComponent = JsonSerializer.Deserialize<LicenseComponent>(baseFileContent);
            if(licenseComponent is null) {
                throw new InvalidOperationException();
            }

            return licenseComponent;
        }

        private LicenseComponentItem ToLicenseComponentItem(IPackageSearchMetadata metadata)
        {
            return new LicenseComponentItem() {
                Name = metadata.Identity.Id,
                Uri = metadata.ProjectUrl?.ToString() ?? string.Empty,
                License = new LicenseComponentLicense() {
                    Name = metadata.LicenseMetadata?.License ?? "Unknown",
                    Uri = metadata.LicenseUrl?.ToString() ?? string.Empty,
                },
            };
        }

        private List<LicenseComponentItem> ApplyMetadata(List<LicenseComponentItem> target, IEnumerable<IPackageSearchMetadata> metadatas)
        {
            foreach(var metadata in metadatas) {
                var item = ToLicenseComponentItem(metadata);
                target.Add(item);
            }

            return target;
        }

        private async Task WriteComponentFileAsync(LicenseComponent component, string outputPath, CancellationToken cancellationToken)
        {
            var options = new JsonSerializerOptions() {
                WriteIndented = true,
            };
            var content = JsonSerializer.Serialize(component, options);
            await System.IO.File.WriteAllTextAsync(outputPath, content, cancellationToken);
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var nugetPackageItems = ReadNuGetPackageItems(Options.CentralPackage);

            // nuget.org の V3 エンドポイント
            var providers = Repository.Provider.GetCoreV3();
            var packageSource = new PackageSource("https://api.nuget.org/v3/index.json");
            var sourceRepository = new SourceRepository(packageSource, providers);

            // メタデータ取得用リソースを取得
            var metadataResource = await sourceRepository.GetResourceAsync<PackageMetadataResource>();

            // キャッシュコンテキスト
            using var cache = new SourceCacheContext();

            var metadataTasks = nugetPackageItems.Select(a => GetMetadataAsync(a, metadataResource, cache, cancellationToken));
            var metadatas = (await Task.WhenAll(metadataTasks)).OrderBy(a => a.Identity.Id).ToArray();

            var component = await ReadComponentFileAsync(Options.BaseJson, cancellationToken);

            var library = ApplyMetadata(component.Library, metadatas);
            var resource = component.Resource;

            var newComponent = new LicenseComponent() {
                Library = library,
                Resource = resource,
            };

            await WriteComponentFileAsync(newComponent, Options.OutputJson, cancellationToken);


        }

        #endregion
    }
}
